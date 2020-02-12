using CsvHelper.Configuration;
using Models;
using System.Linq;

namespace Models.Mappers
{
    public sealed class EmployeeMap : ClassMap<EmployeeModel>
    {
        public EmployeeMap()
        {
            //typeof(EmployeeModel).GetProperties().ToList().ForEach(o =>
            //{
            //    Map(m => m.GetType().GetProperties().Where(oo => oo.Name == o.Name).FirstOrDefault().Name).Name(o.Name);
            //});

            Map(m => m.CityName).Name(Constants.CsvHeaders.CityName);
            Map(m => m.CityCode).Name(Constants.CsvHeaders.CityCode);
            Map(m => m.DistrictName).Name(Constants.CsvHeaders.DistrictName);
            Map(m => m.ZipCode).Name(Constants.CsvHeaders.ZipCode);
        }
    }
}
