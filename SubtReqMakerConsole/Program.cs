using SimpleLogger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SubtReqMakerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleLogger.LogHelper.GetLogger().Info(string.Join(",", ConvertFiles()));
        }

        public static List<string> ConvertFiles()
        {
            var list = new List<string>();
            try
            {
                list = ConvertMain();
                Console.WriteLine(string.Join(Environment.NewLine, list));
            }
            catch (Exception ex)
            {
                list = ConvertMain();
                Console.WriteLine(string.Join(Environment.NewLine, list));
            }

            return list;
        }

        private static List<string> ConvertMain()
        {
            var pathList = System.Configuration.ConfigurationSettings.AppSettings["srtPath"].Split(',').ToList();
            List<string> srtFiles = new List<string>();
            foreach (var path in pathList)
            {
                srtFiles.AddRange(Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                    .Where(s => s.Contains(".srt") && !Equals(GetEncoding(s), Encoding.UTF8)));
            }

            AnsiConvert(srtFiles);
            return srtFiles.ToList();
        }

        public static void AnsiConvert(IEnumerable<string> fileList)
        {
            Parallel.ForEach(fileList, f => { EncodeStringNew(f, Encoding.UTF8); });
            LogHelper.GetLogger().Info($"Conversion done for {fileList.Count()} items.");
        }

        public static void EncodeStringNew(string path, Encoding destEncoding)
        {
            try
            {
                Encoding encoding;
                string original = string.Empty;

                using (StreamReader sr = new StreamReader(path))
                {
                    original = sr.ReadToEnd();
                    encoding = sr.CurrentEncoding;
                    sr.Close();
                }

                original = ReplaceAll(original, CharReplacements.GetList());

                File.WriteAllText(path, original, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                return;
                //throw;
            }
        }

        public static string ReplaceAll(string seed, List<KeyValuePair<string, string>> replacementList)
        {
            foreach (var keyValuePair in replacementList)
            {
                seed = Regex.Replace(seed, keyValuePair.Key, keyValuePair.Value);
            }

            return seed;
        }

        public static bool ContainsUnicodeCharacter(string input)
        {
            const int MaxAnsiCode = 255;

            return input.Any(c => c > MaxAnsiCode);
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private static string ReplaceNonReadable(string input, Dictionary<string, string> replacementList)
        {
            return Regex.Replace(input, @"%+", "%");
        }

        public static Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }

        /// <summary>
        /// Converts a file from one encoding to another.
        /// </summary>
        /// <param name="path">the file to convert</param>
        /// <param name="destPath">the destination for the converted file</param>
        /// <param name="sourceEncoding">the original file encoding</param>
        /// <param name="destEncoding">the encoding to which the contents should be converted</param>
        public static string ConvertFileEncoding(string text)
        {
            try
            {
                Encoding fileEncode = Encoding.GetEncoding("ISO-8859-9");

                byte[] bytes = fileEncode.GetBytes(text);

                byte[] utfBytes = Encoding.Convert(fileEncode, Encoding.UTF8, bytes);

                return Encoding.UTF8.GetString(utfBytes);
            }
            catch (Exception ex)
            {
                LogHelper.GetLogger().Error(ex.Message);
                return text;
            }

        }
    }
}
