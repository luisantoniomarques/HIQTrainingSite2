using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;
using System.Resources;

namespace ExcellToDB
{
    class Program
    {
        private static void TestExcel()
        {
            Application app = new Application();
            Workbook book = null;
            Range range = null;

            try
            {
                app.Visible = false;
                app.ScreenUpdating = false;
                app.DisplayAlerts = false;

                string execPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

                book = app.Workbooks.Open(@"C:\Adentis\HIQTraining\Files\BD_RU_2016.xlsx", Missing.Value, Missing.Value, Missing.Value
                                                  , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                 , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                , Missing.Value, Missing.Value, Missing.Value);
                foreach (Worksheet sheet in book.Worksheets)
                {

                    Console.WriteLine(@"Values for Sheet " + sheet.Index);

                    // get a range to work with
                    range = sheet.get_Range("A1", Missing.Value);
                    // get the end of values to the right (will stop at the first empty cell)
                    range = range.get_End(XlDirection.xlToRight);
                    // get the end of values toward the bottom, looking in the last column (will stop at first empty cell)
                    range = range.get_End(XlDirection.xlDown);

                    // get the address of the bottom, right cell
                    string downAddress = range.get_Address(
                        false, false, XlReferenceStyle.xlA1,
                        Type.Missing, Type.Missing);

                    // Get the range, then values from a1
                    range = sheet.get_Range("A1", downAddress);
                    object[,] values = (object[,])range.Value2;

                    // View the values
                    Console.Write("\t");
                    Console.WriteLine();
                    for (int i = 1; i <= values.GetLength(0); i++)
                    {
                        for (int j = 1; j <= values.GetLength(1); j++)
                        {
                            Console.Write("{0}\t", values[i, j]);
                        }
                        Console.WriteLine();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                range = null;
                if (book != null)
                    book.Close(false, Missing.Value, Missing.Value);
                book = null;
                if (app != null)
                    app.Quit();
                app = null;
            }
        }


        static void Main(string[] args)
        {
            TestExcel();
            Console.ReadLine();
            //Console.WriteLine( Resource1.String1 + " " + Resource1.String2 );
            //Console.ReadLine();
        }
    }
}






/*
FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

//1. Reading from a binary Excel file ('97-2003 format; *.xls)
IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
//...
//2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
//...
//3. DataSet - The result of each spreadsheet will be created in the result.Tables
DataSet result = excelReader.AsDataSet();
//...
//4. DataSet - Create column names from first row
excelReader.IsFirstRowAsColumnNames = true;
DataSet result = excelReader.AsDataSet();

//5. Data Reader methods
while (excelReader.Read())
{
//excelReader.GetInt32(0);
}

//6. Free resources (IExcelDataReader is IDisposable)
excelReader.Close();






private static void TestExcel()
{
    ApplicationClass app = new ApplicationClass();
    Workbook book = null;
    Range range = null;

    try
    {
        app.Visible = false;
        app.ScreenUpdating = false;
        app.DisplayAlerts = false;

        string execPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

        book = app.Workbooks.Open(@"C:\data.xls", Missing.Value, Missing.Value, Missing.Value
                                          , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                         , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                        , Missing.Value, Missing.Value, Missing.Value);
        foreach (Worksheet sheet in book.Worksheets)
        {

            Console.WriteLine(@"Values for Sheet " + sheet.Index);

            // get a range to work with
            range = sheet.get_Range("A1", Missing.Value);
            // get the end of values to the right (will stop at the first empty cell)
            range = range.get_End(XlDirection.xlToRight);
            // get the end of values toward the bottom, looking in the last column (will stop at first empty cell)
            range = range.get_End(XlDirection.xlDown);

            // get the address of the bottom, right cell
            string downAddress = range.get_Address(
                false, false, XlReferenceStyle.xlA1,
                Type.Missing, Type.Missing);

            // Get the range, then values from a1
            range = sheet.get_Range("A1", downAddress);
            object[,] values = (object[,])range.Value2;

            // View the values
            Console.Write("\t");
            Console.WriteLine();
            for (int i = 1; i <= values.GetLength(0); i++)
            {
                for (int j = 1; j <= values.GetLength(1); j++)
                {
                    Console.Write("{0}\t", values[i, j]);
                }
                Console.WriteLine();
            }
        }

    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
    finally
    {
        range = null;
        if (book != null)
            book.Close(false, Missing.Value, Missing.Value);
        book = null;
        if (app != null)
            app.Quit();
        app = null;
    }
}
*/

