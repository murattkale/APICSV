using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using HelperService.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;

namespace APICSV.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        ICsvParserService _ICsvParserService;
        IHostingEnvironment _IHostingEnvironment;
        public UploadController(IHostingEnvironment _IHostingEnvironment, ICsvParserService _ICsvParserService)
        {
            this._IHostingEnvironment = _IHostingEnvironment;
            this._ICsvParserService = _ICsvParserService;
        }

        [HttpPost]
        public async Task<List<EmployeeModel>> ReadFile(IFormFile objfile)
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
            //var jsonString = JsonConvert.SerializeObject(result);
            return result;
        }
    }
}