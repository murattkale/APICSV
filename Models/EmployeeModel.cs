using CsvHelper.Configuration.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class EmployeeModel
    {
        [Name(Constants.CsvHeaders.CityName)]
        public string CityName { get; set; }

        [Name(Constants.CsvHeaders.CityCode)]
        public string CityCode { get; set; }

        [Name(Constants.CsvHeaders.DistrictName)]
        public string DistrictName { get; set; }

        [Name(Constants.CsvHeaders.ZipCode)]
        public string ZipCode { get; set; }
    }
}
