using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using CwcExim.Repositories;

namespace CwcExim.UtilityClasses
{
    public static class Utility
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string CreateXML(Object Object)
        {
            XmlDocument XmlDoc = new XmlDocument();   //Represents an XML document, 
            // Initializes a new instance of the XmlDocument class.          
            XmlSerializer XmlSerializer = new XmlSerializer(Object.GetType());
            // Creates a stream whose backing store is memory. 
            try
            {
                using (MemoryStream XmlStream = new MemoryStream())
                {
                    XmlSerializer.Serialize(XmlStream, Object);
                    XmlStream.Position = 0;
                    //Loads the XML document from the specified string.
                    XmlDoc.Load(XmlStream);
                    return XmlDoc.InnerXml;
                }
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static T DeserializeXml<T>(string XmlInput) where T : class
        {
            System.Xml.Serialization.XmlSerializer XmlSr = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader StringRdr = new StringReader(XmlInput))
            {
                return (T)XmlSr.Deserialize(StringRdr);
            }
        }
        public static string CreateRandomPassword(int PasswordLength)
        {
            string AllowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] Chars = new char[PasswordLength];
            Random rd = new Random();

            for (int i = 0; i < PasswordLength; i++)
            {
                Chars[i] = AllowedChars[rd.Next(0, AllowedChars.Length)];
            }

            return new string(Chars);
        }


        public static string Encrypt(string ClearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] ClearBytes = Encoding.Unicode.GetBytes(ClearText);
            using (Aes Encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                Encryptor.Key = pdb.GetBytes(32);
                Encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, Encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(ClearBytes, 0, ClearBytes.Length);
                        cs.Close();
                    }
                    ClearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return ClearText;
        }

        public static string Decrypt(string CipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] CipherBytes = Convert.FromBase64String(CipherText);
            using (Aes Encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes Pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                Encryptor.Key = Pdb.GetBytes(32);
                Encryptor.IV = Pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, Encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(CipherBytes, 0, CipherBytes.Length);
                        cs.Close();
                    }
                    CipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return CipherText;
        }

        public static DataTable GetExcelData(HttpPostedFileBase PostedFile)
        {
            DataTable dt = new DataTable();

            try
            {
                if (PostedFile != null)
                {
                    string basePath = HttpContext.Current.Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }
                    string filePath = basePath + Path.GetFileName(PostedFile.FileName);
                    string extension = Path.GetExtension(PostedFile.FileName);
                    PostedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source ={0}; Extended Properties = 'Excel 8.0;HDR=YES'";
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                    }

                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();

                                //Read Data from First Sheet.
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                connExcel.Close();
                            }
                        }
                    }

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                }
            }
            catch (Exception ex)
            {
                 log.Error(ex.Message +" :\r\n"+ex.StackTrace);
                return null;
            }


            return dt;
        }



        public static DataTable GetExcelData(string FilePath)
        {
            DataTable dt = new DataTable();

            try
            {
                if (FilePath != null)
                {
                    
                    string filePath = FilePath;
                    string extension = Path.GetExtension(filePath);
                    //PostedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source ={0}; Extended Properties = 'Excel 8.0;HDR=YES'";
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                    }

                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable NEWdtExcelSchema;
                                NEWdtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName2 = NEWdtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();

                                //Read Data from First Sheet.
                               
                                //        connExcel.Open();
                                //string Query = "SELECT CAST(ISO AS String)  From [" + sheetName2 + "]";
                                ////var execteQuery=new OleDbDataAdapter(Query, connExcel);
                                //var command = new OleDbDataAdapter(Query, connExcel);
                               


                                cmdExcel.CommandText = "SELECT *  From [" + sheetName2 + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);
                                // check newly
                                //DataTable dt1 = new DataTable();
                                //dt1.Columns.Add("Sr No", typeof(string));
                                //dt1.Columns.Add("Container No", typeof(string));
                                //dt1.Columns.Add("ISO", typeof(string));
                                //dt1.Columns.Add("Base Sts", typeof(string));
                                //dt1.Columns.Add("F/E", typeof(string));
                                //dt1.Columns.Add("Name", typeof(string));
                                //dt1.Columns.Add("Grr Wgt", typeof(double));
                                //dt1.Columns.Add("Category", typeof(string));
                                //dt1.Columns.Add("Imo", typeof(string));
                                //dt1.Columns.Add("Un no", typeof(string));
                                //dt1.Columns.Add("Temp", typeof(string));
                                //dt1.Columns.Add("ODC", typeof(string));
                                //dt1.Columns.Add("POL", typeof(string));
                                //dt1.Columns.Add("FPD", typeof(string));
                                //dt1.Columns.Add("CFS CODE", typeof(string));
                                //dt1.Columns.Add("EXIT MODE", typeof(string));
                                //dt1.Columns.Add("Connecting Vessel name & VIA", typeof(string));
                                //dt1.Columns.Add("Seal 1", typeof(string));
                                //dt1.Columns.Add("Seal 2", typeof(string));
                                //dt1.Columns.Add("IGM number", typeof(string));
                                //dt1.Columns.Add("STOWATE", typeof(string));
                                


                                //var excelDataAdapter = new System.Data.OleDb.OleDbDataAdapter(string.Format(@"select * from [{0}]", sheetName2), connExcel);
                                //    connExcel.Close();
                                //    DataTable dtSourceData = new DataTable();
                                //excelDataAdapter.FillSchema(dtSourceData, SchemaType.Mapped);

                                //foreach (DataColumn cl in dtSourceData.Columns)
                                //{

                                //    if (cl.ColumnName.Contains("Seal 1"))
                                //        cl.DataType = typeof(string);
                                //    if (cl.ColumnName.Contains("Seal 2"))
                                //        cl.DataType = typeof(string);
                                //}
                                //excelDataAdapter.FillSchema(dtSourceData, SchemaType.Source);

                                //excelDataAdapter.Fill(dt1);

                                // check newly

                                connExcel.Close();
                            }
                        }
                    }

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                }
            }
            catch (Exception ex)
            {
                return null;
            }


            return dt;
        }
        public static DateTime StringToDateConversion(string date, string format)
        {
            /* Convert Date to Currrnt Culture */
            DateTimeFormatInfo dateTimeFormatterProvider = DateTimeFormatInfo.CurrentInfo.Clone() as DateTimeFormatInfo;
            dateTimeFormatterProvider.ShortDatePattern = format; //source date format
            DateTime NewDate = DateTime.Parse(date, dateTimeFormatterProvider);
            return NewDate;
        }
        public static void WriteLog(string text)
        {
            string logPath = HttpContext.Current.Server.MapPath("~/Docs/"); ;
            //if (!Directory.Exists(logPath))
            //    Directory.CreateDirectory(logPath);
            string path = logPath + @"\ErrorLog.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
            }
        }



        public static string ResizeImage(String Source, String Target)
        {
            // string imgPath = @"D:\TESTPROJECT\Einvoice\CwcExim\CwcExim\Content\Images1\49dc6397-31bb-43d7-b4d0-2db8f04f57c8.png";
            // string imgPathTarget = @"D:\TESTPROJECT\Einvoice\CwcExim\CwcExim\Content\InvQrcode2.png";
            Bitmap source = new Bitmap(Source);
            Bitmap t = CropWhiteSpace(source);
            t.Save(Target);

            return (Target);
        }
        public static Bitmap CropWhiteSpace(Bitmap bmp)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            int white = 0xffffff;

            Func<int, bool> allWhiteRow = r =>
            {
                for (int i = 0; i < w; ++i)
                    if ((bmp.GetPixel(i, r).ToArgb() & white) != white)
                        return false;
                return true;
            };

            Func<int, bool> allWhiteColumn = c =>
            {
                for (int i = 0; i < h; ++i)
                    if ((bmp.GetPixel(c, i).ToArgb() & white) != white)
                        return false;
                return true;
            };

            int topmost = 0;
            for (int row = 0; row < h; ++row)
            {
                if (!allWhiteRow(row))
                    break;
                topmost = row;
            }

            int bottommost = 0;
            for (int row = h - 1; row >= 0; --row)
            {
                if (!allWhiteRow(row))
                    break;
                bottommost = row;
            }

            int leftmost = 0, rightmost = 0;
            for (int col = 0; col < w; ++col)
            {
                if (!allWhiteColumn(col))
                    break;
                leftmost = col;
            }

            for (int col = w - 1; col >= 0; --col)
            {
                if (!allWhiteColumn(col))
                    break;
                rightmost = col;
            }

            if (rightmost == 0) rightmost = w; // As reached left
            if (bottommost == 0) bottommost = h; // As reached top.

            int croppedWidth = rightmost - leftmost;
            int croppedHeight = bottommost - topmost;

            if (croppedWidth == 0) // No border on left or right
            {
                leftmost = 0;
                croppedWidth = w;
            }

            if (croppedHeight == 0) // No border on top or bottom
            {
                topmost = 0;
                croppedHeight = h;
            }

            try
            {
                var target = new Bitmap(croppedWidth, croppedHeight);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(bmp,
                      new RectangleF(0, 0, croppedWidth, croppedHeight),
                      new RectangleF(leftmost, topmost, croppedWidth, croppedHeight),
                      GraphicsUnit.Pixel);
                }
                return target;
            }
            catch (Exception ex)
            {
                throw new Exception(
                  string.Format("Values are topmost={0} btm={1} left={2} right={3} croppedWidth={4} croppedHeight={5}", topmost, bottommost, leftmost, rightmost, croppedWidth, croppedHeight),
                  ex);
            }
        }


        public static string GetServerDate()
        {

            UtilityRepository obj = new UtilityRepository();
            obj.GetServerDate();
            string ServerDate = "";
            if(obj.DBResponse.Status==1)
            {
                ServerDate=Convert.ToString(obj.DBResponse.Data);
            }
            return ServerDate;
          
        }    

    }
}