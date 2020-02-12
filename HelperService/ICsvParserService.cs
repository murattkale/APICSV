using CsvHelper;
using Models;
using System.Collections.Generic;

namespace HelperService.Services
{
    public interface ICsvParserService
    {
        List<T> ReadCsvFileToEmployeeModel<T>(string path);
        CsvWriter WriteNewCsvFile<T>(string path, List<T> employeeModels);
    }
}
