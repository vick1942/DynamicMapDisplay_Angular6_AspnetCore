using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Constants
{
    public static class ExceptionMessages
    {
        public const string ORGANIZATION_CODE_DOES_NOT_EXISTS = "Invalid organization code.";
        public const string NETWORK_CODE_DOES_NOT_EXISTS = "Invalid network code.";
        public const string INVALID_FILE_EXTENSION = "Invalid file extension. File Extension should be .xls or .xlsx";
        public const string INVALID_FILE_NAME = "Invalid file name format. File name format should be 'OrganizationCode_NetworkCode_Date.xls/.xlsx'";
        public const string LESS_NUMBER_OF_COLUMNS = "Excel file should contain 30 columns.";
        public const string INVALID_COLUMNS_LAYOUT = "File has invalid column layout. Please check the each column name and its position.";
        public const string INVALID_EFFECTIVE_DATE = "Invalid date format in Effective date column";
        public const string INVALID_TERMINATION_DATE = "Invalid date format in Termination date column";
        public const string INVALID_SPECIALITY_CODE_NUMBER = "Speciality doesn't exist in database with given speciality code";
        public const string INVALID_PROVIDER_CATEGORY_CODE = "Provider category doesn't exist in database with given provider code";
        public const string ADDRESS_IS_EMPTY = "Practice address is empty";
        public const string ZIPCODE_IS_EMPTY = "ZipCode is empty";
        public const string CITY_IS_EMPTY = "City is empty";
        public const string STATE_IS_EMPTY = "State is empty";
        public const string PHONE_NUMBER_IS_EMPTY = "Phone number is empty";
        public const string FULL_NAME_IS_EMPTY = "Provider Full Name is empty";
        public const string FIRST_NAME_IS_EMPTY = "Provider First Name is empty";
        public const string LAST_NAME_IS_EMPTY = "Provider Last Name is empty";
    }
}
