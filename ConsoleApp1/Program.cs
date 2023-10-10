using HIQTrainingSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HIQTrainingSite.Helper;
using HIQTraining.Business.Company;
using HIQTraining.DAL.Company;
using HIQTraining.Exceptions;
using HIQTraining.ModelDetail;
using HIQTraining.ActiveDirectory;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;
using HIQTrainingSite.Excel;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ExcelToBD ww = new ExcelToBD();
            ww.Extract__(@"C:\HIQTraining\ExcellImport\BD_RU_2016.xlsx");
            Console.ReadLine();
        }
    }
}
