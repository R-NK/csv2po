using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;
using System.Text.RegularExpressions;

namespace csv2po
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var csv = new CsvHelper.CsvReader(new StreamReader(args[0])))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("msgid \"\"\r\nmsgstr \"\"\r\n").Append(string.Format(@"""Project-Id-Version: PACKAGE VERSION\n""
""Report-Msgid-Bugs-To: \n""
""POT-Creation-Date: {0}\n""
""PO-Revision-Date: YEAR-MO-DA HO:MI+ZONE\n""
""Last-Translator: FULL NAME <EMAIL@ADDRESS>\n""
""Language-Team: LANGUAGE <LL@li.org>\n""
""MIME-Version: 1.0\n""
""Content-Type: text/plain; charset=UTF-8\n""
""Content-Transfer-Encoding: 8bit\n""
""X-Generator: Translate Toolkit 1.9.0\n""", DateTime.Now)).Append("\r\n\r\n");
                
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap<CsvMap>();
                var records = csv.GetRecords<Columns>();
                foreach (var record in records)
                {
                    if (record.location != "")
                    {
                        sb.Append("#: ").Append(record.location).Append("\r\n");

                    }
                    if (record.source != "")
                    {
                        sb.Append("msgid \"").Append(record.source.Replace("\"", "\\\"").Replace("\n", "\\n\"\r\n\"")).Append("\"\r\n");
                        if (record.target != "")
                        {
                            sb.Append("msgstr \"").Append(record.target.Replace("\"", "\\\"").Replace("\n", "\\n\"\r\n\"")).Append("\"\r\n\r\n");
                        }
                        else
                        {
                            sb.Append("msgstr \"").Append(record.source.Replace("\"", "\\\"").Replace("\n", "\\n\"\r\n\"")).Append("\"\r\n\r\n");
                        }
                    }                   
                }
                StreamWriter sw = new StreamWriter(args[0] + ".po");
                sw.Write(sb.ToString());
                sw.Close();
            }
        }
    }

    public class Columns
    {
        public string location { get; set; }
        public string source { get; set; }
        public string target { get; set; }
        public string Googletranslation { get; set; }
    }
    public sealed class CsvMap : CsvHelper.Configuration.CsvClassMap<Columns>
    {
        public CsvMap()
        {
            Map(x => x.location).Index(0);
            Map(x => x.source).Index(1);
            Map(x => x.target).Index(2);
            Map(x => x.Googletranslation).Index(3);
        }
    }
}
