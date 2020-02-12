using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Models
{

    public class CsvMediaTypeFormatter : TextOutputFormatter
    {

        /// <summary>
        /// CSV Formatter
        /// </summary>
        public CsvMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }



        /// <summary>
        /// Write the response
        /// </summary>
        /// <param name="context"></param>
        /// <param name="selectedEncoding"></param>
        /// <returns></returns>
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            StringBuilder csv = new StringBuilder();
            Type type = GetTypeOf(context.Object);

            csv.AppendLine(
                string.Join<string>(
                    ",", type.GetProperties().Select(x => x.Name)
                )
            );

            foreach (var obj in (IEnumerable<object>)context.Object)
            {
                var vals = obj.GetType().GetProperties().Select(
                    pi => new
                    {
                        Value = pi.GetValue(obj, null),
                        Name = pi.Name
                    }
                ).OrderBy(o => o.Name == "District");

                List<string> values = new List<string>();
                foreach (var valParent in vals)
                {
                    if (valParent.Name != "District")
                    {
                        var tmpvalParent = valParent.Value.ToString();
                        if (tmpvalParent.Contains(","))
                            tmpvalParent = string.Concat("\"", tmpvalParent, "\"");

                        tmpvalParent = tmpvalParent.Replace("\r", " ", StringComparison.InvariantCultureIgnoreCase);
                        tmpvalParent = tmpvalParent.Replace("\n", " ", StringComparison.InvariantCultureIgnoreCase);

                        values.Add(tmpvalParent);
                        continue;
                    }

                    if (valParent.Name == "District")
                        foreach (var val in ((List<District>)valParent.Value))
                        {


                            var tmpval = val.Name;

                            if (tmpval.Contains(","))
                                tmpval = string.Concat("\"", tmpval, "\"");

                            tmpval = tmpval.Replace("\r", " ", StringComparison.InvariantCultureIgnoreCase);
                            tmpval = tmpval.Replace("\n", " ", StringComparison.InvariantCultureIgnoreCase);

                            values.Add(tmpval);

                            foreach (var val2 in (List<Zip>)val.Zip)
                            {
                                var tmpval2 = val2.Code;

                                if (tmpval2.Contains(","))
                                    tmpval2 = string.Concat("\"", tmpval2, "\"");

                                tmpval2 = tmpval2.Replace("\r", " ", StringComparison.InvariantCultureIgnoreCase);
                                tmpval2 = tmpval2.Replace("\n", " ", StringComparison.InvariantCultureIgnoreCase);

                                values.Add(tmpval2);
                            }

                        }


                }



                csv.AppendLine(string.Join(",", values));
            }
            return context.HttpContext.Response.WriteAsync(csv.ToString(), selectedEncoding);
        }

        private static Type GetTypeOf(object obj)
        {
            Type type = obj.GetType();
            Type itemType;
            if (type.GetGenericArguments().Length > 0)
            {
                itemType = type.GetGenericArguments()[0];
            }
            else
            {
                itemType = type.GetElementType();
            }
            return itemType;
        }

        protected override bool CanWriteType(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }
    }
}
