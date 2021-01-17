using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace parseData
{
    class Program
    {
        

        static void Main(string[] args)
        {
            List<DataParser> dataParsers = new List<DataParser>();
            String sDataLayoutFileName = "";
            String sDataFileName = "";

            Console.WriteLine("Hi there! This program is a parser for cobol structured files. \nFirst, you must enter a name of LAYOUT DATA FILE, it must be placed in program home directory. \nSecond, you must enter a name of DATA FILE, it must be placed in program home directory. \nThis program will process files and save result in CSV-files in same directory.\n\nPlease enter DATA LAYOUT file name (with extension at the end) : ");
            sDataLayoutFileName = Directory.GetCurrentDirectory() + "\\" + Console.ReadLine();
            Console.WriteLine(" \nPlease enter DATA FILE name (with extension at the end) : ");
            sDataFileName = Directory.GetCurrentDirectory() + "\\" + Console.ReadLine();

            while (System.IO.File.Exists(sDataLayoutFileName)&& System.IO.File.Exists(sDataFileName)) 
            {
                DataParser dataParser = new DataParser(sDataLayoutFileName, sDataFileName);
                System.Console.WriteLine(" \nFiles processed succesesfully!");
                dataParser.ExportDataToCSV();
                System.Console.WriteLine(" \nData expordet to CSV files in home directory!");

                dataParsers.Add(dataParser);

               


                Console.WriteLine(" \nIf you want to parse another DATA, please follow the instruction. \nIf u want to close program please enter two empty string.\n\nPlease enter DATA LAYOUT file name (with extension at the end) : ");
                sDataLayoutFileName = Directory.GetCurrentDirectory() + "\\" + Console.ReadLine();
                Console.WriteLine(" \nPlease enter DATA FILE name (with extension at the end): ");
                sDataFileName = Directory.GetCurrentDirectory() + "\\" + Console.ReadLine();
            }

        


           

        }

       

    }
}
