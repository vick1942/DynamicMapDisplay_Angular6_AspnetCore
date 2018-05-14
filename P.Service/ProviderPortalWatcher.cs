using Business.Entities.Entities;
using Common.Constants;
using Common.Utilities;
using IBusiness;
using P.Service.Utility;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace P.Service
{
    public class PWatcher
    {
        Logger loggers = LogManager.GetLogger(Constants.PServiceLogger);
        string filePath = System.Configuration.ConfigurationManager.AppSettings["fileInPath"];
        string fileArchivePath = System.Configuration.ConfigurationManager.AppSettings["fileArchivePath"];
        string fileReprocess = System.Configuration.ConfigurationManager.AppSettings["IsFailedFileReprocess"];

        public readonly IImportService _importService;
        private List<ProviderCategoryEntity> providerCategories;
        private List<SpecialityEntity> specialities;
        public PWatcher(IImportService importService)
        {
            _importService = importService;
        }
        int counter = 1;
        public async Task RunWatcherAsync()
        {
            loggers.Debug("RunWatcherAsync has started.");

            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            if (directoryInfo.Exists)
            {
                FileInfo[] filesToBeProcessed = directoryInfo.GetFiles();

                foreach (FileInfo fileToBeProcessed in filesToBeProcessed)
                {
                    counter = 1;
                    var fileName = fileToBeProcessed.Name;
                    await ReprocessingFile(fileToBeProcessed,false);
                }
            }
            else
            {
                loggers.Error("The inbound file location cannot be contacted.  We will attempt again in 20 seconds.");
            }
        }

        private async Task ReprocessingFile( FileInfo fileToBeProcessed, bool Isreprocessing)
        {
            string emailMessage=string.Empty;
            var fileName = fileToBeProcessed.Name;
            try
            {
                if (!Isreprocessing)
                {
                    emailMessage = EmailService.PrepareEmailTemplate(false, fileName, "File has been successfully picked up and will now be processed.", "");
                    EmailService.SendEmail("Providers Import Service - File Picked Up Successfully", emailMessage);
                }
                loggers.Debug($"The file {fileName} has been picked up and acknowledgment email has been sent.");

                ValidateFileNameAndExtension(fileToBeProcessed);

                string[] fileInformation = fileName.Split(new string[] { "_" }, StringSplitOptions.None);
                var organizationCode = fileInformation[0].Trim();
                var networkCode = fileInformation[1].Trim();

                if (!_importService.IsOrganizationCodeExists(organizationCode).Result)
                {
                    throw new Exception(ExceptionMessages.ORGANIZATION_CODE_DOES_NOT_EXISTS);
                }
                loggers.Debug($"The file {fileName} has valid Organization Code.");


                var networkName = _importService.GetNetworkNameByCode(networkCode, organizationCode).Result;

                if (String.IsNullOrEmpty(networkName))
                {
                    throw new Exception(ExceptionMessages.NETWORK_CODE_DOES_NOT_EXISTS);
                }
                loggers.Debug($"The file {fileName} has valid Network Code.");

                List<PEntity> providers = ExcelToProviderParser.GetProvidersFromExcel(fileToBeProcessed);
                loggers.Debug($"Successfully created the List of Provider objects from the file {fileName}");
                if (!Isreprocessing)
                {
                    emailMessage = EmailService.PrepareEmailTemplate(false, fileToBeProcessed.Name, "File has been accepted structurally and will now be processed.", "");
                    EmailService.SendEmail("Providers Import Service - File Accepted Structurally", emailMessage);
                }
                loggers.Debug($"The file {fileName} has been accepted structurally and acknowledgment email has been sent.");

                await StartProcessingProvidersData(providers, networkName, fileName);

                emailMessage = EmailService.PrepareEmailTemplate(false, fileName, "File has been successfully save into Database.", "");
                EmailService.SendEmail("Providers Import Service - File Import Success", emailMessage);
                loggers.Debug($"The file {fileName} has been successfully save into Database and acknowledgment email has been sent.");

                var fileArchiveDirectory = new DirectoryInfo(fileArchivePath);
                if (!fileArchiveDirectory.Exists)
                    fileArchiveDirectory.Create();

                var sourceFilePath = fileToBeProcessed.FullName;
                var targetFilePath = fileArchiveDirectory.FullName + fileName;
                File.Delete(targetFilePath);
                File.Move(sourceFilePath, targetFilePath);
                loggers.Debug($"The file {fileName} has been successfully moved into the folder {fileArchiveDirectory.FullName}");
            }
            catch (Exception ex)
            {
                loggers.Error(ex, $"Failed to process the file {fileName}");
                HandleFileFailureAsync(fileToBeProcessed, ex.Message);
            }
        }

        private void ValidateFileNameAndExtension(FileInfo file)
        {
            if (!file.Extension.ToLower().Equals(".xls") && !file.Extension.ToLower().Equals(".xlsx"))
            {
                throw new Exception(ExceptionMessages.INVALID_FILE_EXTENSION);
            }
            loggers.Debug($"The file {file.Name} has valid extension");

            string[] fileInformation = file.Name.Split(new string[] { "_" }, StringSplitOptions.None);
            if (fileInformation == null || fileInformation.Length != 3)
            {
                throw new Exception(ExceptionMessages.INVALID_FILE_NAME);
            }
            loggers.Debug($"Format of the file name is valid");
        }

        private async void HandleFileFailureAsync(FileInfo file, string failureMessage)
        {
            if (counter <= Convert.ToInt32(fileReprocess))
            {
                counter++;
                await ReprocessingFile(file, true);

            }
            else
            {
                var emailMessage = EmailService.PrepareEmailTemplate(true, file.Name, "", failureMessage);
                EmailService.SendEmail("Providers Import Service - File Import Failed", emailMessage);
                loggers.Debug($"Failed to process the file {file.Name} and acknowledgment email has been sent.");

                var directoryInfo = file.Directory;
                var fileFailedPath = System.Configuration.ConfigurationManager.AppSettings["fileFailedPath"];
                var fileFailedDirectory = new DirectoryInfo(fileFailedPath);

                if (!fileFailedDirectory.Exists)
                    fileFailedDirectory.Create();

                var sourceFilePath = file.FullName;
                var targetFilePath = fileFailedDirectory.FullName + file.Name;
                File.Delete(targetFilePath);
                File.Move(sourceFilePath, targetFilePath);
                loggers.Debug($"The file {file.Name} has been successfully moved into the folder {fileFailedDirectory.FullName}");
            }
        }

        private async Task StartProcessingProvidersData(List<PEntity> providers, string networkName,string fileName)
        {
            try
            {
                providerCategories = _importService.GetAllProviderCategories().Result;
                loggers.Debug($"Successfully got the List of Provider categories from DB.");

                specialities = _importService.GetAllSpecialities().Result;
                loggers.Debug($"Successfully got the List of Specialities from DB.");

                for (int i = 0; i < providers.Count; i++)
                {
                    try
                    {
                        DoesProviderHasAllMandatoryFields(providers[i]);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                loggers.Debug($"The file has all mandatory fields data.");
                int counter = 1;
                foreach (var provider in providers)
                {
                    counter = counter + 1;
                    var address = provider.ServiceAddress1 + " " + (string.IsNullOrEmpty(provider.ServiceAddress2) ? " " : provider.ServiceAddress2) +
                                                                                          " " + provider.ServiceCity + " " + provider.ServiceState + " " + provider.ServiceZip.Substring(0, 5);
                    var locationEntity = GeoCoordinateTool.GetLocation(address, System.Configuration.ConfigurationManager.AppSettings["Google_Map_Api_Url"], System.Configuration.ConfigurationManager.AppSettings["Google_Map_Api_Key"]).Result;
                    if (locationEntity != null && string.IsNullOrEmpty(locationEntity.ErrorMessage_GoogleMapAPI) && string.IsNullOrEmpty(locationEntity.Status_GoogleMapAPI))
                    {
                        provider.Latitude = locationEntity.Latitude.ToString();
                        provider.Longitude = locationEntity.Longitude.ToString();
                    }
                    else
                    {
                        provider.Latitude = "0";
                        provider.Longitude = "0";
                        loggers.Debug($"Latitude: 0 Longitude: 0 Address:{address} for the ProviderFullName: {provider.ProviderFullName} ProviderFirstName: {provider.ProviderFirstName} ProviderLastName:{provider.ProviderLastName} API_ErrorMessage:{locationEntity.ErrorMessage_GoogleMapAPI} API_Status:{locationEntity.Status_GoogleMapAPI} ");
                    }
                }
                loggers.Debug($"Succesfully updated the latitude and longitude for each provider.");

                int bulkInsertRecordsCount = 0;
                if (System.Configuration.ConfigurationManager.AppSettings["BulkInsertRecordsCount"] != null)
                    bulkInsertRecordsCount = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["BulkInsertRecordsCount"]);

                await _importService.SaveProviderCollection(providers, networkName, fileName, bulkInsertRecordsCount);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool DoesProviderHasAllMandatoryFields(PEntity provider)
        {
            if (specialities?.Where(sp => sp.CodeNumber.ToLower() == provider.ProviderSpecialityCode.Trim().ToLower()).Count() == 0)
                throw new Exception($"{ExceptionMessages.INVALID_SPECIALITY_CODE_NUMBER} at row number { provider.IdGeneratedNumber }");

            var providerCategoryEntity = providerCategories?.Where(pc => pc.Code.ToLower() == provider.ProviderCategory.Trim().ToLower()).FirstOrDefault();

            if (providerCategoryEntity == null)
                throw new Exception($"{ExceptionMessages.INVALID_PROVIDER_CATEGORY_CODE} at row number { provider.IdGeneratedNumber }");

            if (String.IsNullOrEmpty(provider.ServiceAddress1))
                throw new Exception($"{ExceptionMessages.ADDRESS_IS_EMPTY} at row number { provider.IdGeneratedNumber }");

            if (String.IsNullOrEmpty(provider.ServiceZip))
                throw new Exception($"{ExceptionMessages.ZIPCODE_IS_EMPTY} at row number { provider.IdGeneratedNumber }");

            if (String.IsNullOrEmpty(provider.ServiceCity))
                throw new Exception($"{ExceptionMessages.CITY_IS_EMPTY} at row number { provider.IdGeneratedNumber }");

            if (String.IsNullOrEmpty(provider.ServiceState))
                throw new Exception($"{ExceptionMessages.STATE_IS_EMPTY} at row number { provider.IdGeneratedNumber }");

            if (String.IsNullOrEmpty(provider.PhoneNumber))
                throw new Exception($"{ExceptionMessages.PHONE_NUMBER_IS_EMPTY} at row number { provider.IdGeneratedNumber }");

            if (providerCategoryEntity.Type.ToLower() == "facility")
            {
                if (String.IsNullOrEmpty(provider.ProviderFullName))
                    throw new Exception($"{ExceptionMessages.FULL_NAME_IS_EMPTY} at row number { provider.IdGeneratedNumber }");
            }
            else
            {
                if (String.IsNullOrEmpty(provider.ProviderFirstName))
                    throw new Exception($"{ExceptionMessages.FIRST_NAME_IS_EMPTY} at row number { provider.IdGeneratedNumber }");

                if (String.IsNullOrEmpty(provider.ProviderLastName))
                    throw new Exception($"{ExceptionMessages.LAST_NAME_IS_EMPTY} at row number { provider.IdGeneratedNumber }");
            }

            return true;
        }

    }
}