using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using HIQTrainingSite.ViewModel;
using HIQTraining.ModelDetail;
using HIQTraining.DAL.Inscription;

namespace HIQTrainingSite.Excel
{
    public class ExcelToBD
    {
        public int Extract(string ee)
        {
            Application app = new Application();
            Workbook book = null;
            Range range = null;


            string fii = @"C:\Adentis++++++++++++++++\HIQTraining\HIQ-Training\HIQTrainingSite\HIQTrainingSite\Images\" + ee;

            //InscriptionDao ss = new InscriptionDao();
            //InscriptionDetail tt = new InscriptionDetail();
            //tt = ss.GetInscriptionByCourseIDAndStudentId(5, 1);
            ////string wsw = tt.CourseName;
            //tt.CourseName = "Luis";
            ////int dd = 1;
            //return 1;

            //InscriptionViewModel wsw = new InscriptionViewModel();
            //wsw.CourseName = "Luis";
            //return 1;

            try
            {
                app.Visible = false;
                app.ScreenUpdating = false;
                app.DisplayAlerts = false;

                book = app.Workbooks.Open(fii, Missing.Value, Missing.Value, Missing.Value
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
                    }
                }
                Console.ReadLine();
            }
            catch (Exception e)
            {
                //Console.WriteLine("Excepção");
                File.Delete(@"C:\Adentis++++++++++++++++\HIQTraining\HIQ-Training\HIQTrainingSite\HIQTrainingSite\Images\.xls");
                File.Delete(@"C:\Adentis++++++++++++++++\HIQTraining\HIQ-Training\HIQTrainingSite\HIQTrainingSite\Images\.xlsx");
                File.Delete(@"C:\Adentis++++++++++++++++\HIQTraining\HIQ-Training\HIQTrainingSite\HIQTrainingSite\Images\e.xls");
                File.Delete(@"C:\Adentis++++++++++++++++\HIQTraining\HIQ-Training\HIQTrainingSite\HIQTrainingSite\Images\e.xls");
                return 1;
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
            return 0;
        }


        public int Extract__(string ee)
        {
            Application app = new Application();
            Workbook book = null;
            Range range = null;

            InscriptionDao ss = new InscriptionDao();
            InscriptionDetail tt = new InscriptionDetail();
            tt = ss.GetInscriptionByCourseIDAndStudentId(5, 1);
            Console.WriteLine(tt.CourseName);

            try
            {
                app.Visible = false;
                app.ScreenUpdating = false;
                app.DisplayAlerts = false;

                book = app.Workbooks.Open(ee, Missing.Value, Missing.Value, Missing.Value
                                                     , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                   , Missing.Value, Missing.Value, Missing.Value);

                foreach (Worksheet sheet in book.Worksheets)
                {
                    int sh = 0;
                    //Console.WriteLine(@"Values for Sheet " + sheet.Index);

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

                    //if (sh == 0 && (values[1, 1] is int) ||
                    //                (values[1, 2] is string) ||
                    //                (values[1, 3] is string) ||
                    //                (values[1, 4] is int) ||
                    //                (values[1, 5] is int) ||
                    //                (values[1, 6] is int) ||
                    //                (values[1, 7] is int) ||
                    //                (values[1, 8] is int) ||
                    //                (values[1, 9] is int) ||
                    //                (values[1, 10] is int) ||
                    //                (values[1, 11] is int) ||
                    //                (values[1, 12] is int) ||
                    //                (values[1, 13] is int)
                    //                ) Console.WriteLine();

                    sh++;
                    // View the values
                    Console.Write("\t");
                    Console.WriteLine();
                    for (int i = 1; i <= values.GetLength(0); i++)
                    {
                        for (int j = 1; j <= values.GetLength(1); j++)
                        {
                            Console.Write("{0}\t", values[i, j]);
                        }
                    }
                    Console.WriteLine(values.GetLength(0) + "  " + values.GetLength(1));

                    Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Workbook xlWorkBook;
                    Worksheet xlWorkSheet;
                    object misValue = System.Reflection.Missing.Value;

                    xlWorkBook = xlApp.Workbooks.Add(misValue);
                    xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    xlWorkSheet.Cells[1, 1] = values[1, 1];
                    xlWorkSheet.Cells[1, 2] = values[1, 2];
                    xlWorkSheet.Cells[2, 1] = values[1, 3];
                    xlWorkSheet.Cells[2, 2] = values[1, 4];
                    xlWorkSheet.Cells[3, 1] = values[1, 5];
                    xlWorkSheet.Cells[3, 2] = values[1, 6];



                    xlWorkBook.SaveAs(@"C:\Adentis++++++++++++++++\HIQTraining\HIQ-Training\HIQTrainingSite\HIQTrainingSite\Excel\Expo.xlsx", XlFileFormat.xlWorkbookDefault, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();

                    Marshal.ReleaseComObject(xlWorkSheet);
                    Marshal.ReleaseComObject(xlWorkBook);
                    Marshal.ReleaseComObject(xlApp);
                }
                Console.ReadLine();
            }
            catch (Exception e)
            {
                //Console.WriteLine("Excepção");
                return 1;
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
            return 0;
        }
    }
}
