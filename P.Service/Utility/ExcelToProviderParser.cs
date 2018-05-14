using Business.Entities.Entities;
using Common.Constants;
using NLog;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace P.Service.Utility
{
    public static class ExcelToProviderParser
    {
        static Logger loggers = LogManager.GetLogger(Constants.PServiceLogger);
        private const int ColumnsCount = 30;
        public static List<PEntity> GetProvidersFromExcel(FileInfo fileToBeProcessed)
        {
            ISheet sheet = null;
            List<PEntity> providers = new List<PEntity>();

            using (FileStream file = new FileStream(fileToBeProcessed.FullName, FileMode.Open, FileAccess.Read))
            {
                if (fileToBeProcessed.Extension.ToLower().Equals(".xlsx"))
                    sheet = new XSSFWorkbook(file).GetSheetAt(0);
                else
                    sheet = new HSSFWorkbook(file).GetSheetAt(0);
            }

            IRow headerRow = sheet.GetRow(0);
            if (headerRow.Cells.Count < ColumnsCount)
            {
                //log
                throw new Exception(ExceptionMessages.LESS_NUMBER_OF_COLUMNS);
            }
            loggers.Debug($"The file {fileToBeProcessed.Name} has valid number of columns.");

            if (!IsExcelLayoutValid(headerRow))
            {
                throw new Exception(ExceptionMessages.INVALID_COLUMNS_LAYOUT);
            }
            loggers.Debug($"The file {fileToBeProcessed.Name} has valid layout.");

            for (int index = 1; index <= sheet.LastRowNum; index++)
            {
                IRow row = sheet.GetRow(index);
                if (row != null) //null is when the row only contains empty cells 
                {
                    Object[] cells = new Object[ColumnsCount];
                    for (int i = 0; i < cells.Length; i++)
                    {
                        cells[i] = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    }
                    var values = cells.Select(o => o.ToString()).ToList();

                    // ignore empty lines.
                    if (values.Where(y => y.ToString().Trim().Length > 0).Count() == 0)
                    {
                        // move to next line.
                        continue;
                    }
                    providers.Add(GetProvider(cells, index));
                }
            }
            return providers;
        }

        private static string GetItemFromArray(Object[] obj, int index)
        {
            if (obj[index] == null)
            {
                return string.Empty;
            }
            return obj[index].ToString();
        }
        private static PEntity GetProvider(Object[] row, int rowNumber)
        {
            PEntity provider = new PEntity();

            provider.IdGeneratedNumber = rowNumber;
            provider.ProviderTIN = GetItemFromArray(row,0);
            provider.ProviderFullName = GetItemFromArray(row, 1);
            provider.BillingAddress1 = GetItemFromArray(row, 2);
            provider.BillingAddress2 = GetItemFromArray(row, 3);
            provider.BillingCity = GetItemFromArray(row, 4);
            provider.BillingState = GetItemFromArray(row, 5);
            provider.BillingZip = GetItemFromArray(row, 6);
            provider.PhoneAreaCode = GetItemFromArray(row, 7);
            provider.PhoneNumber = GetItemFromArray(row, 8);
            provider.PhoneExtension = GetItemFromArray(row, 9);
            provider.ProviderSpecialityCode = FormatProviderSpecialityCode(GetItemFromArray(row, 10));//Spacility Tab code
            provider.ServiceAddress1 = GetItemFromArray(row, 11);
            provider.ServiceAddress2 = GetItemFromArray(row, 12);
            provider.ServiceCity = GetItemFromArray(row, 13);
            provider.ServiceState = GetItemFromArray(row, 14);
            provider.ServiceZip = GetItemFromArray(row, 15);
            provider.ProviderCategory = GetItemFromArray(row, 16); //ProviderCategory code
            provider.ProviderFirstName = GetItemFromArray(row, 17);
            provider.ProviderLastName = GetItemFromArray(row, 18);
            provider.ProviderNPI = GetItemFromArray(row, 19);
            provider.ProviderFeeScheduleIdentifier = GetItemFromArray(row, 20);
            provider.ProviderGroupAssignment = GetItemFromArray(row, 21);

            try
            {
                provider.EffectiveDate = Convert.ToDateTime(GetItemFromArray(row, 22));
            }
            catch (Exception ex)
            {
                throw new Exception($"{ExceptionMessages.INVALID_EFFECTIVE_DATE} at row number { rowNumber }");
            }

            try
            {
                provider.TerminationDate = GetItemFromArray(row, 23).Trim().Length == 0 ? (DateTime?)null : Convert.ToDateTime(GetItemFromArray(row, 23));
            }
            catch (Exception)
            {
                throw new Exception($"{ExceptionMessages.INVALID_TERMINATION_DATE} at row number { rowNumber }");
            }

            provider.DiscountPercentage = GetItemFromArray(row, 24).Trim().Length == 0 ? (Double?)null : Convert.ToDouble(GetItemFromArray(row, 24));
            provider.Endorsedornonendorsed = GetItemFromArray(row, 25);
            provider.SecondProviderSpecialtyifapplicable = GetItemFromArray(row, 26);
            provider.ContractName = GetItemFromArray(row, 27);
            provider.EPOidentifier = GetItemFromArray(row, 28);
            provider.MiddleInitialIfApplicable = GetItemFromArray(row, 29);
            provider.Latitude = "";
            provider.Longitude = "";

            return provider;
        }

        /// <summary>
        /// If specialityCode is single digit number
        /// then this method prepends the zero to given specialityCode
        /// </summary>
        /// <param name="specialityCode"></param>
        /// <returns></returns>
        private static string FormatProviderSpecialityCode(string specialityCode)
        {
            if(specialityCode.Trim().Length == 1 && Char.IsNumber(specialityCode[0]))
            {
                specialityCode = "0" + specialityCode;
            }
            return specialityCode;
        }

        private static bool IsExcelLayoutValid(IRow headerRow)
        {
            string[] excelColumnsLayout = new string[] { "ProviderTIN",
                                                        "ProviderFullName",
                                                        "BillingAddress1",
                                                        "BillingAddress2",
                                                        "BillingCity",
                                                        "BillingState",
                                                        "BillingZip",
                                                        "PhoneAreaCode",
                                                        "PhoneNumber",
                                                        "PhoneExtension",
                                                        "ProviderSpecialityCode",
                                                        "ServiceAddress1",
                                                        "ServiceAddress2",
                                                        "ServiceCity",
                                                        "ServiceState",
                                                        "ServiceZip",
                                                        "ProviderCategory",
                                                        "ProviderFirstName",
                                                        "ProviderLastName",
                                                        "ProviderNPI",
                                                        "ProviderFeeScheduleIdentifier",
                                                        "ProviderGroupAssignment",
                                                        "EffectiveDate",
                                                        "TerminationDate",
                                                        "DiscountPercentage",
                                                        "Endorsedornon-endorsed",
                                                        "SecondProviderSpecialtyifapplicable",
                                                        "ContractName",
                                                        "EPOidentifier",
                                                        "MiddleInitialIfApplicable" };
            for (int i = 0; i < ColumnsCount; i++)
            {
                if (excelColumnsLayout[i].ToLower().Equals(headerRow.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToLower()))
                    continue;
                return false;
            }
            return true;
        }
    }
}
