using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SubReformatter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var path = ConfigurationManager.AppSettings["srtPath"];
            var myFiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(s => s.Contains(".srt"));
            AnsiConvert(myFiles);
            return View(myFiles.ToList());
        }

        public void AnsiConvert(IEnumerable<string> fileList)
        {
            Parallel.ForEach(fileList, f => { ConvertFileEncoding(f, Encoding.UTF8); });
        }

        /// <summary>
        /// Converts a file from one encoding to another.
        /// </summary>
        /// <param name="path">the file to convert</param>
        /// <param name="destPath">the destination for the converted file</param>
        /// <param name="sourceEncoding">the original file encoding</param>
        /// <param name="destEncoding">the encoding to which the contents should be converted</param>
        public static void ConvertFileEncoding(string path, Encoding destEncoding)
        {
            try
            {
                Encoding fileEncode = Encoding.GetEncoding("ISO-8859-9");

                string stt = System.IO.File.ReadAllText(path, fileEncode);

                byte[] bytes = fileEncode.GetBytes(stt);

                byte[] utfBytes = Encoding.Convert(fileEncode, Encoding.UTF8, bytes);

                var utf8_String = Encoding.UTF8.GetString(utfBytes);

                System.IO.File.Delete(path);

                System.IO.File.WriteAllText(path, utf8_String, Encoding.UTF8);
            }
            catch (Exception)
            {

            }

        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}