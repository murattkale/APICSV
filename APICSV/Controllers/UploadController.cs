using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using CsvHelper;
using HelperService.Services;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using Models;
using Models.Mappers;
using Newtonsoft.Json; 
 
namespace APICSV.Controllers
{
    //[Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        ICsvParserService _ICsvParserService;
        IHostingEnvironment _IHostingEnvironment;
        public UploadController(IHostingEnvironment _IHostingEnvironment, ICsvParserService _ICsvParserService)
        {
            this._IHostingEnvironment = _IHostingEnvironment;
            this._ICsvParserService = _ICsvParserService;
        }

        [Route("api/[controller]/CsvToJsonOrXml")]
        [HttpPost]
        public async Task<List<EmployeeModel>> CsvToJsonOrXml(IFormFile objfile, string CityName)
        {
            if (!Directory.Exists(this._IHostingEnvironment.ContentRootPath + "\\uploads\\"))
                Directory.CreateDirectory(this._IHostingEnvironment.ContentRootPath + "\\uploads\\");
            using (FileStream filestream = System.IO.File.Create(this._IHostingEnvironment.ContentRootPath + "\\uploads\\" + objfile.FileName))
            {
                await objfile.CopyToAsync(filestream);
                filestream.Flush();
                filestream.Close();
            }
            var fullPath = this._IHostingEnvironment.ContentRootPath + "\\uploads\\" + objfile.FileName;
            var result = _ICsvParserService.ReadCsvFileToEmployeeModel<EmployeeModel>(fullPath);
            result = result.Where(o => (!string.IsNullOrEmpty(CityName) ? o.CityName == CityName : true)).ToList();
            result = result.OrderBy(o => o.CityName).ThenBy(o => o.DistrictName).ToList();
            return result;
        }

        [Route("api/[controller]/XmlToCsv")]
        [HttpPost]
        public async Task<List<City>> XmlToCsv(IFormFile objfile, string CityName)
        {
            if (!Directory.Exists(this._IHostingEnvironment.ContentRootPath + "\\uploads\\"))
                Directory.CreateDirectory(this._IHostingEnvironment.ContentRootPath + "\\uploads\\");
            using (FileStream filestream = System.IO.File.Create(this._IHostingEnvironment.ContentRootPath + "\\uploads\\" + objfile.FileName))
            {
                objfile.CopyTo(filestream);
                filestream.Flush();
                filestream.Close();
            }
            var fname = objfile.FileName.Split('.').FirstOrDefault();
            var fullPath = this._IHostingEnvironment.ContentRootPath + "\\uploads\\" + objfile.FileName;

            var result = new List<City>();
            using (StreamReader reader = new StreamReader(fullPath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AddressInfo));
                result = ((AddressInfo)serializer.Deserialize(reader)).City;
            }
            result = result.Where(o => (!string.IsNullOrEmpty(CityName) ? o.Name == CityName : true)).ToList();

            //using (var writer = new StringWriter())
            //{
            //    ExtensionsEx.Helper.WriteCSV(result, writer, true);

            //    string csv = writer.ToString();

            //    return File(csv, "text/csv", fname + ".csv");

            //}


            //HttpResponseMessage results = new HttpResponseMessage(HttpStatusCode.OK);

            //results.Content = new StringContent(WriteTsv(result));
            //results.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
            //results.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment"); 
            //results.Content.Headers.ContentDisposition.FileName = "RecordExport.csv";


            return result;


        }

        public string WriteTsv<T>(IEnumerable<T> data)
        {
            StringBuilder output = new StringBuilder();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output.Append(prop.DisplayName); // header
                output.Append("\t");
            }
            output.AppendLine();
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output.Append(prop.Converter.ConvertToString(
                         prop.GetValue(item)));
                    output.Append("\t");
                }
                output.AppendLine();
            }
            return output.ToString();
        }




    }
}