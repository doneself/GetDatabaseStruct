using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetDatabaseStruct.Interfaces;
using GetDatabaseStruct.Concretes;
using System.IO;

namespace GetDatabaseStruct
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStringSettingName = "dzhdygz";
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings[connStringSettingName].ToString();
            string outputDir = @"C:\Users\lok\Desktop\test";
            outputDir = Path.Combine(outputDir, DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("yyyyMMdd"));
            if(!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            string fullName = Path.Combine(outputDir, $"{DateTime.Now.ToString("HHmm")}_字段说明.txt");
            Console.WriteLine(connString);

            IGetStruct service = new SqlServerGetStruct(connString);
            //IGetStruct service = new OracleGetStruct(connString);
            var tableList = service.GetTableDetail();
            //Console.WriteLine(ObjectDumper.Dump(tableList));

            StringBuilder sb = new StringBuilder();
            string nullvalue = "-";
            foreach (var table in tableList)
            {
                sb.AppendLine($"## {table.TableName}");
                var columnList = service.GetColumnDetail(table.TableName);
                foreach (var col in columnList)
                {
                    sb.AppendLine($"{col.ColumnName}\t\t\t\t{col.DataType}\tlength({col.DataLength??nullvalue})\tNullable({col.Nullable})\t{col.Comments}");
                }
                sb.AppendLine();
            }

            using (StreamWriter sw = new StreamWriter(fullName, false, Encoding.UTF8))
            {
                sw.Write(sb.ToString());
            }
            System.Diagnostics.Process.Start(fullName);
            Console.WriteLine("导出成功。");

            Console.ReadKey();
        }
    }
}
