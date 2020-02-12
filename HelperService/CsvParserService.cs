using CsvHelper;
using Models;
using Models.Mappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HelperService.Services
{
    public class CsvParserService : ICsvParserService
    {
        public List<T> ReadCsvFileToEmployeeModel<T>(string path)
        {
            try
            {
                using (var reader = new StreamReader(path, Encoding.Default))
                using (var csv = new CsvReader(reader))
                {
                    //csv.Configuration.HeaderValidated = null;
                    //csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.Delimiter = ",";
                    csv.Configuration.RegisterClassMap<EmployeeMap>();
                    var records = csv.GetRecords<T>().ToList();
                    return records;
                }
            }
            catch (UnauthorizedAccessException e)
            {
                throw new Exception(e.Message);
            }
            catch (FieldValidationException e)
            {
                throw new Exception(e.Message);
            }
            catch (CsvHelperException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void WriteNewCsvFile<T>(string path, List<T> employeeModels)
        {
            using (StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(true)))
            using (CsvWriter cw = new CsvWriter(sw))
            {
                cw.WriteHeader<T>();
                cw.NextRecord();
                foreach (T emp in employeeModels)
                {
                    cw.WriteRecord<T>(emp);
                    cw.NextRecord();
                }
            }
        }
    }
}

