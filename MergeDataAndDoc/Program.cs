using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 4/11 4.00 PM -
namespace MergeDataAndDoc
{
    class Program
    {

        static void Main(string[] args)
        {
            string TemplateFileName = "Template.txt";
            string DataFileName = "Data.txt";
            string outputFileName = "Output.txt";
            

        //    Console.WriteLine( args.Length.ToString() );

            if (args.Length == 6)
            {
                int count = 0;
                while( count < 3 )
                {
                    switch(args[count*2])
                    {
                        case "-i":
                            DataFileName = args[count * 2 + 1];
                        //    Console.WriteLine(DataFileName);
                            break;
                        case "-t":
                            TemplateFileName = args[count * 2 + 1];
                        //    Console.WriteLine(TemplateFileName);
                            break;
                        case "-r":
                            outputFileName = args[count * 2 + 1];
                        //    Console.WriteLine(outputFileName);
                            break;
                        default:
                            Console.WriteLine("參數不正確");
                            break;
                    }
                    count++;
                }
            }

            Program p = new Program();
           
            using (StreamWriter outFile = new StreamWriter(outputFileName)) // 寫檔案
            using (StreamReader templateFile = new StreamReader(TemplateFileName)) // 讀檔案
            using (StreamReader dataFile = new StreamReader(DataFileName)) // 讀檔案
                p.Merge(outFile, templateFile, dataFile);

              
        }
        public void Merge(TextWriter outFile, TextReader templateFile, TextReader dataFile)
        {
            string line = "";
            string tem = templateFile.ReadToEnd();
            List<string> Var = new List<string>(); // 用來存變數名稱
            List<List<string>> data = new List<List<string>>(); // 存Data
            Get_Data(dataFile, Var, data);

            for( int k = 0; k < data.Count; k++ ) // 對每一筆資料(中文姓名/身分證/年數)進行替換
            {
                string write = tem;
                for( int i = 0; i < Var.Count; i++ )
                    write = write.Replace("${" + Var[i] + "}", data[k][i]);
                outFile.Write(write);
            }
        }

        public void Get_Data(TextReader dataFile, List<string> Var, List<List<string>> data)
        {
            string line;
            int count = 0;
            while ((line = dataFile.ReadLine()) != null)
            {
                string tmpVar = "";
                int index = 0, nextIndex = 0;
                List<string> listData = new List<string>(); // 存單筆資料(一行)

                while (nextIndex != -1) // 讀完那一行
                {
                    nextIndex = line.IndexOf('\t', index + 1);
                    if (nextIndex >= 0) // 避免第一行只有一個變數欄位，或是讀到最後一個變數
                        tmpVar = line.Substring(index, nextIndex - index);
                    else
                        tmpVar = line.Substring(index, line.Length - index);
                    index = nextIndex + 1;
                    while (line[index].Equals('\t'))
                        index++;
                    if (count == 0) // 表示第一行，要存變數名稱
                        Var.Add(tmpVar);
                    else
                        listData.Add(tmpVar);
                }
                if (count != 0)
                {
                    data.Add(listData);
                    listData = new List<string>();
                }
                count++;
            }
        }
    }
}
