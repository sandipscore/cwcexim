using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Renci.SshNet;

namespace CwcExim.UtilityClasses
{
    public static class FtpFileManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string _FtpServerIP;
        private static string _FtpUid;
        private static string _FtpPwd;
        private static int _PortNo;
        static FtpFileManager()
        {
            /*_FtpServerIP = System.Configuration.ConfigurationManager.AppSettings["ResourceUrl"];
            _FtpUid = System.Configuration.ConfigurationManager.AppSettings["FtpUid"];
            _FtpPwd = System.Configuration.ConfigurationManager.AppSettings["FtpPwd"];*/
        }

        public static string UploadFileToFtp(string FtpFilePath, string FileName, byte[] FileBytes)
        {
            _FtpServerIP = HttpContext.Current.Session["FtpServerIP"].ToString();
            _FtpUid = HttpContext.Current.Session["FtpUid"].ToString();
            _FtpPwd = HttpContext.Current.Session["FtpPwd"].ToString();
            _PortNo = Convert.ToInt32(HttpContext.Current.Session["PortNo"]);

            string Status = "Success";
            MakeFTPDir(FtpFilePath);
            try
            {
                FtpWebRequest FtpRequest;
                FtpRequest = (FtpWebRequest)WebRequest.Create(
                  String.Format("{0}/{1}", _FtpServerIP, FtpFilePath + "/" + FileName));
                FtpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                FtpRequest.Credentials = new NetworkCredential(_FtpUid, _FtpPwd);
                // get file bytes
                //byte[] fileBytes = File.ReadAllBytes(filePath);
                FtpRequest.ContentLength = FileBytes.Length;
                Stream requestStream = FtpRequest.GetRequestStream();
                requestStream.Write(FileBytes, 0, FileBytes.Length);
                requestStream.Close();
                return Status;
            }
            catch (Exception ex)
            {
                Status = "Error";
                return Status;
            }

        }
        public static string UploadFileToFtp(string FtpFilePath, string FileName, byte[] FileBytes, string PortNo, string FilePath)
        {

            log.Error("FtpFilePath:" + FtpFilePath);
            log.Error("File Name:" + FileName);
            log.Error("File Path:" + FilePath);

            _FtpServerIP = HttpContext.Current.Session["FtpServerIP"].ToString();
            _FtpUid = HttpContext.Current.Session["FtpUid"].ToString();
            _FtpPwd = HttpContext.Current.Session["FtpPwd"].ToString();
            _PortNo = Convert.ToInt32(HttpContext.Current.Session["PortNo"]);
            log.Info("FtpServerIP: "+ _FtpServerIP);
            log.Info("FtpUid: " + _FtpUid);
            log.Info("FtpPwd: " + _FtpPwd);
            log.Info("PortNo: " + _PortNo);


            bool type = true; //FileValidation.IsvalidFile(FileBytes);
            if (!type)
            {
                string Status = "Error";
                return Status;
            }
            else
            {
                string Status = "Success";
                //MakeFTPDir(FtpFilePath);
                try
                {
                    //:: START OF FTP CONFIGURATION :://

                    /* FtpWebRequest FtpRequest;
                     string _FtpServerIPwithport;
                     _FtpServerIPwithport = _FtpServerIP + ":" + PortNo;
                     FtpRequest = (FtpWebRequest)WebRequest.Create(
                       String.Format("{0}/{1}", _FtpServerIPwithport, FtpFilePath + "/" + FileName));
                     FtpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                     FtpRequest.Credentials = new NetworkCredential(_FtpUid, _FtpPwd);
                     // get file bytes
                     //byte[] fileBytes = File.ReadAllBytes(filePath);
                     FtpRequest.ContentLength = FileBytes.Length;
                     Stream requestStream = FtpRequest.GetRequestStream();
                     requestStream.Write(FileBytes, 0, FileBytes.Length);
                     requestStream.Close();
                     return Status;*/

                    // :: END OF FTP CONFIGURATION :://

                    //:: START  SFTP CONFIGURATION :://

                    SftpClient objSftpClient = null;
                    objSftpClient = new SftpClient(_FtpServerIP, _PortNo, _FtpUid, _FtpPwd);
                    log.Info("IP:" + _FtpServerIP + " Port No:" + _PortNo + " Ftp Uid:" + _FtpUid + " Ftp Pwd:" + _FtpPwd);
                    log.Info("Upload SFTP connection start");
                    objSftpClient.Connect();
                    log.Info("Upload SFTP connection end");

                    string[] SubDirs = FtpFilePath.Split('/');
                    foreach (string SubDir in SubDirs)
                    {
                        if (SubDir != "")
                        {
                            if (!objSftpClient.Exists(SubDir))
                                objSftpClient.CreateDirectory(SubDir);
                            objSftpClient.ChangeDirectory(SubDir);
                        }
                    }
                    var TempPath = FilePath;//HttpContext.Current.Server.MapPath("~/Docs/" + DateTime.Now.ToOADate().ToString() + FileName);//_MessageExchangeTempFile + FileName;
                    //System.IO.File.WriteAllBytes(TempPath, FileBytes);
                    Stream fin = File.OpenRead(TempPath);

                    log.Error("SFTP working directory   :" + objSftpClient.WorkingDirectory);
                    log.Error("SFTP UPLOAD start filename  :"+ FileName);

                    objSftpClient.UploadFile(fin, objSftpClient.WorkingDirectory + "/" + FileName);

                    log.InfoFormat("SFTP UPLOAD end filename  :", FileName);
                    fin.Close();
                    //System.IO.File.Delete(TempPath);
                    objSftpClient.Disconnect();


                    return Status;

                    //:: END OF SFTP CONFIGURATION :://
                }
                catch (Exception ex)
                {
                    log.Error("Err : SFTP connection error\r\n" + ex.Message + "\r\n" + ex.StackTrace);
                    Status = "Error";
                    return Status;
                }
            }

        }
        public static string DeleteFileFromFtp(string FtpFilePath, string FileName)
        {
            _FtpServerIP = HttpContext.Current.Session["FtpServerIP"].ToString();
            _FtpUid = HttpContext.Current.Session["FtpUid"].ToString();
            _FtpPwd = HttpContext.Current.Session["FtpPwd"].ToString();
            _PortNo = Convert.ToInt32(HttpContext.Current.Session["PortNo"]);

            string Status = "Success";
            try
            {
                Stream ftpStream = null;
                FtpWebRequest FtpRequest;
                FtpRequest = (FtpWebRequest)WebRequest.Create(
                  String.Format("{0}/{1}", _FtpServerIP, FtpFilePath + "/" + FileName));
                FtpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpRequest.Credentials = new NetworkCredential(_FtpUid, _FtpPwd);
                FtpWebResponse response = (FtpWebResponse)FtpRequest.GetResponse();
                ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
                return Status;
            }
            catch (Exception ex)
            {
                Status = "Error";
                return Status;
            }
        }
        private static void MakeFTPDir(string PathToCreate)
        {
            _FtpServerIP = HttpContext.Current.Session["FtpServerIP"].ToString();
            _FtpUid = HttpContext.Current.Session["FtpUid"].ToString();
            _FtpPwd = HttpContext.Current.Session["FtpPwd"].ToString();
            _PortNo = Convert.ToInt32(HttpContext.Current.Session["PortNo"]);

            //, byte[] fileContents, string ftpProxy = null
            FtpWebRequest ReqFTP = null;
            Stream FtpStream = null;
            string[] SubDirs = PathToCreate.Split('/');
            string CurrentDir = string.Format("{0}", _FtpServerIP);
            foreach (string SubDir in SubDirs)
            {
                try
                {
                    if (SubDir != "")
                    {
                        CurrentDir = CurrentDir + "/" + SubDir;

                        ReqFTP = (FtpWebRequest)FtpWebRequest.Create(CurrentDir);
                        ReqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                        ReqFTP.UseBinary = true;
                        ReqFTP.Credentials = new NetworkCredential(_FtpUid, _FtpPwd);
                        FtpWebResponse response = (FtpWebResponse)ReqFTP.GetResponse();
                        FtpStream = response.GetResponseStream();
                        FtpStream.Close();
                        response.Close();
                    }
                }
                catch (Exception ex)
                {
                    //directory already exist I know that is weak but there is no way to check if a folder exist on ftp...
                }
            }
        }

        public static string DownloadFile(string FtpDirectory, string FileName, string LocalDirectory)
        {
            _FtpServerIP = HttpContext.Current.Session["FtpServerIP"].ToString();
            _FtpUid = HttpContext.Current.Session["FtpUid"].ToString();
            _FtpPwd = HttpContext.Current.Session["FtpPwd"].ToString();
            _PortNo = Convert.ToInt32(HttpContext.Current.Session["PortNo"]);

            string Status = "Success";
            try
            {
                Directory.CreateDirectory(LocalDirectory);
            }
            catch (Exception ex)
            {

            }

            if (File.Exists(LocalDirectory + "/" + FileName))
            {
                File.Delete(LocalDirectory + "/" + FileName);
            }
            try
            {
                FtpWebRequest RequestFileDownload = (FtpWebRequest)WebRequest.Create(_FtpServerIP + "/" + FtpDirectory + "/" + FileName);
                RequestFileDownload.Credentials = new NetworkCredential(_FtpUid, _FtpPwd);
                RequestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
                FtpWebResponse ResponseFileDownload = (FtpWebResponse)RequestFileDownload.GetResponse();
                Stream ResponseStream = ResponseFileDownload.GetResponseStream();
                FileStream WriteStream = new FileStream(LocalDirectory + "/" + FileName, FileMode.Create);
                int Length = 2048;
                Byte[] Buffer = new Byte[Length];
                int BytesRead = ResponseStream.Read(Buffer, 0, Length);
                while (BytesRead > 0)
                {
                    WriteStream.Write(Buffer, 0, BytesRead);
                    BytesRead = ResponseStream.Read(Buffer, 0, Length);
                }
                ResponseStream.Close();
                WriteStream.Close();
                RequestFileDownload = null;

                return Status;
            }
            catch (Exception ex)
            {
                //throw ex;
                Status = "Error";
                return Status;
            }

        }

        public static string ClearDownloadFolder(string DirectoryPath)
        {
            string Status = "";

            try
            {
                Directory.Delete(DirectoryPath, true);
                Status = "Success";
            }
            catch (DirectoryNotFoundException DnfEx)
            {
                Status = DnfEx.Message;
            }

            return Status;
        }

    }
}