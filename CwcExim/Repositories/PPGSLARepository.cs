using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using CwcExim.Areas.SLA.Models;
using CwcExim.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;
using System.Text;

namespace CwcExim.Repositories
{
    public class PPGSLARepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

            }
        }

        #region SLA Registration 
        public void AddEditSLARegistration(PPGSLA ObjSLA)
        {
            /* Commodity Type:1.HAZ 2.Non HAZ */
            string GeneratedClientId = "0";
            string ReturnObj = "";
            var RaisedDate = (dynamic)null;
            RaisedDate = Convert.ToDateTime(ObjSLA.RaisedOn).ToString("yyyy-MM-dd HH:mm:ss");

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjSLA.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Description", MySqlDbType = MySqlDbType.VarChar, Value = ObjSLA.IssueDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FileName", MySqlDbType = MySqlDbType.VarChar, Value = ObjSLA.FileName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ResolutionLevel", MySqlDbType = MySqlDbType.VarChar, Value = ObjSLA.ResolutionLevel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueType", MySqlDbType = MySqlDbType.VarChar, Value = ObjSLA.IssueType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjSLA.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RaisedOn", MySqlDbType = MySqlDbType.DateTime, Size = 25, Value = RaisedDate });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            //LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditSLARegistration", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse(); 
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Ticket Generated Successfully.";
                    _DBResponse.Data = GeneratedClientId;
                }                
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;

            }
        }

        public void GetAllSLARegistration(int TicketId,int UserId,int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TicketId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = TicketId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = UserId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllSLARegistration", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPGSLARegistrationList> LstSLARegistration = new List<PPGSLARegistrationList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSLARegistration.Add(new PPGSLARegistrationList
                    {
                        TicketId = Convert.ToInt32(Result["TicketId"]),
                        TicketNo = Result["TicketNo"].ToString(),
                        RaisedOn = Result["RaisedOn"].ToString(),
                        IssueDescription = Result["IssueDescription"].ToString(),
                        FileName = Result["FileName"].ToString(),
                        RaisedBy = Result["RaisedBy"].ToString(),
                        ResolutionLevel = Result["ResolutionLevel"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSLARegistration;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetViewSLARegistration(int TicketId, int UserId, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TicketId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = TicketId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = UserId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllSLARegistration", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPGSLARegistrationList objSLARegistration = new PPGSLARegistrationList();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objSLARegistration.TicketId = Convert.ToInt32(Result["TicketId"]);
                    objSLARegistration.TicketNo = Result["TicketNo"].ToString();
                    objSLARegistration.RaisedOn = Result["RaisedOn"].ToString();
                    objSLARegistration.IssueDescription = Result["IssueDescription"].ToString();
                    objSLARegistration.FileName = Result["FileName"].ToString();
                    objSLARegistration.RaisedBy = Result["RaisedBy"].ToString();
                    objSLARegistration.ResolutionLevel = Result["ResolutionLevel"].ToString();
                    objSLARegistration.IssueType = Result["IssueType"].ToString();

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objSLARegistration;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch(Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetAllSLARegistrationSitl(int TicketId, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TicketId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = TicketId });           
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllSLARegistrationSitl", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPGSLARegistrationList> LstSLARegistration = new List<PPGSLARegistrationList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSLARegistration.Add(new PPGSLARegistrationList
                    {
                        TicketId = Convert.ToInt32(Result["TicketId"]),
                        TicketNo = Result["TicketNo"].ToString(),
                        RaisedOn = Result["RaisedOn"].ToString(),
                        IssueDescription = Result["IssueDescription"].ToString(),
                        ResolveDate = Result["ResolveDate"].ToString(),
                        RaisedBy = Result["RaisedBy"].ToString(),
                        ResolutionLevel = Result["ResolutionLevel"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSLARegistration;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetSLARegistrationDetail(int TicketId, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TicketId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = TicketId });           
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllSLARegistrationSitl", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPGSLARegistrationList objSLARegistration = new PPGSLARegistrationList();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objSLARegistration.TicketId = Convert.ToInt32(Result["TicketId"]);
                    objSLARegistration.TicketNo = Result["TicketNo"].ToString();
                    objSLARegistration.RaisedOn = Result["RaisedOn"].ToString();
                    objSLARegistration.IssueDescription = Result["IssueDescription"].ToString();
                    objSLARegistration.FileName = Result["FileName"].ToString();
                    objSLARegistration.RaisedBy = Result["RaisedBy"].ToString();
                    objSLARegistration.ResolutionLevel = Result["ResolutionLevel"].ToString();
                    objSLARegistration.ResolveDate = Result["ResolveDate"].ToString();
                    objSLARegistration.ResolveTime = Result["ResolveTime"].ToString();
                    objSLARegistration.Remarks = Result["Remarks"].ToString();
                    objSLARegistration.ResolvedBy = Result["ResolvedBy"].ToString();
                    objSLARegistration.IssueType = Result["IssueType"].ToString();
                    objSLARegistration.IssueStatus = Result["IssueStatus"].ToString();

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objSLARegistration;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void AddEditSLAResolution(PPGSLARegistrationList ObjSLA)
        {
            /* Commodity Type:1.HAZ 2.Non HAZ */
            string GeneratedClientId = "0";            
            var ResolvedDate = (dynamic)null;
            ResolvedDate = Convert.ToDateTime(ObjSLA.ResolveDate).ToString("yyyy-MM-dd HH:mm:ss");

            List<MySqlParameter> LstParam = new List<MySqlParameter>();            
            LstParam.Add(new MySqlParameter { ParameterName = "in_ResolvedBy", MySqlDbType = MySqlDbType.VarChar, Value = ObjSLA.ResolvedBy });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjSLA.Remarks });           
            LstParam.Add(new MySqlParameter { ParameterName = "in_ResolutionDateTime", MySqlDbType = MySqlDbType.DateTime, Size = 25, Value = ResolvedDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueStatus", MySqlDbType = MySqlDbType.VarChar, Value = ObjSLA.IssueStatus });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TicketId", MySqlDbType = MySqlDbType.Int32, Value = ObjSLA.TicketId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditSLAResolution", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Resolution added Successfully.";
                    _DBResponse.Data = GeneratedClientId;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;

            }
        }

        public void GetSLARegistrationListUser(int TicketId,int UserId, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TicketId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = TicketId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = UserId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllSLARegistrationUser", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPGSLARegistrationList> objSLARegistration = new List<PPGSLARegistrationList>();
            try
            {
               
                while (Result.Read())
                {
                    Status = 1;
                    objSLARegistration.Add(new PPGSLARegistrationList
                    {
                        TicketId = TicketId = Convert.ToInt32(Result["TicketId"]),
                        TicketNo = Result["TicketNo"].ToString(),
                        RaisedOn = Result["RaisedOn"].ToString(),
                        IssueDescription = Result["IssueDescription"].ToString(),
                        FileName = Result["FileName"].ToString(),
                        RaisedBy = Result["RaisedBy"].ToString(),
                        ResolutionLevel = Result["ResolutionLevel"].ToString(),
                        ResolveDate = Result["ResolveDate"].ToString(),
                        ResolveTime = Result["ResolveTime"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                        ResolvedBy = Result["ResolvedBy"].ToString(),
                        ResolutionHours = Result["ResolutionHours"].ToString()
                    });
                }
                
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objSLARegistration;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetAllRegistrationSearch(int UserId, string SearchValue, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = UserId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = SearchValue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllSLARegistrationSearchUser", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPGSLARegistrationList> objSLARegistration = new List<PPGSLARegistrationList>();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    objSLARegistration.Add(new PPGSLARegistrationList
                    {
                        TicketId = Convert.ToInt32(Result["TicketId"]),
                        TicketNo = Result["TicketNo"].ToString(),
                        RaisedOn = Result["RaisedOn"].ToString(),
                        IssueDescription = Result["IssueDescription"].ToString(),
                        FileName = Result["FileName"].ToString(),
                        RaisedBy = Result["RaisedBy"].ToString(),
                        ResolutionLevel = Result["ResolutionLevel"].ToString(),
                        ResolveDate = Result["ResolveDate"].ToString(),
                        ResolveTime = Result["ResolveTime"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                        ResolvedBy = Result["ResolvedBy"].ToString(),
                        ResolutionHours = Result["ResolutionHours"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objSLARegistration;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }



        #endregion

        #region SLA Report

        public void SLAReportEXCEL(PPGSLAReport Obj)
        {             
            int Status = 0;

            //PPGSLAReport model = new PPGSLAReport();
            List<PPGSLAReportIncidents> lstSLAIncidents = new List<PPGSLAReportIncidents>();
            //List<PPGSLAReportDefects> lstSLADefects = new List<PPGSLAReportDefects>();
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Year });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Quarter", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Quarter });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getSLAReport", CommandType.StoredProcedure, DParam);
            

            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstSLAIncidents.Add(new PPGSLAReportIncidents
                    {
                        M4 = "",
                        M5 = "",
                        M1 = Result["M1"].ToString(),
                        M2 = Result["M2"].ToString(),
                        M3 = Result["M3"].ToString()                        
                    });
                }

                DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = ConSLADetail(lstSLAIncidents, Obj.Year, Obj.Quarter);

                //}                
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();

            }
        }

        private string ConSLADetail(List<PPGSLAReportIncidents> lstSLAIncidents, string Year, string Quarter)
        {
            var BrnchName = "";
            var BrnchId = Convert.ToInt32(HttpContext.Current.Session["BranchId"]);
            if(BrnchId == 1)
            {
                BrnchName = "CFS Kandla";
            }           
            if (BrnchId == 2)
            {
                BrnchName = "CFS Kolkata";
            }
            if (BrnchId == 3)
            {
                BrnchName = "ICD Patparganj";
            }
            if (BrnchId == 4)
            {
                BrnchName = "CFS Hyderabad";
            }
            if (BrnchId == 6)
            {
                BrnchName = "ICD Dashrath";
            }
            if (BrnchId == 7)
            {
                BrnchName = "CFS Whitefield";
            }
            if (BrnchId == 10)
            {
                BrnchName = "CFS Ambad";
            }
            if (BrnchId == 14)
            {
                BrnchName = "CFS Tuticorin";
            }

            string QName = "";
            string QM1 = "";
            string QM2 = "";
            string QM3 = "";

            string CDate = DateTime.Now.ToShortDateString();

            if (Quarter == "Q1")
            {
                int QYear = int.Parse(Year) - 1;
                QName = "Period : " + "30/Dec/" + QYear + "-29/Mar/" + Year;
                QM1 = "Jan" + " _ " + Year;
                QM2 = "Feb" + " _ " + Year;
                QM3 = "Mar" + " _ " + Year;
            }
            if (Quarter == "Q2")
            {
                QName = "Period : " + "30/Mar/" + Year + "-29/Jun/" + Year;
                QM1 = "Apr" + " _ " + Year;
                QM2 = "May" + " _ " + Year;
                QM3 = "Jun" + " _ " + Year;
            }
            if (Quarter == "Q3")
            {
                QName = "Period : " + "30/Jun/" + Year + "-29/Sep/" + Year;
                QM1 = "Jul" + " _ " + Year;
                QM2 = "Aug" + " _ " + Year;
                QM3 = "Sep" + " _ " + Year;
            }
            if (Quarter == "Q4")
            {
                QName = "Period : " + "30/Oct/" + Year + "-29/Dec/" + Year;
                QM1 = "Oct" + " _ " + Year;
                QM2 = "Nov" + " _ " + Year;
                QM3 = "Dec" + " _ " + Year;
            }

            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");

            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Govt. of India Undertaking)"
                        + Environment.NewLine + "ICD Patparganj-Delhi"
                        + Environment.NewLine + Environment.NewLine
                        + "Format for Certificate of Help Desk Support";
                string typeOfValue = "";

                typeOfValue = "During Period Of " + Year + " Quarter " + Quarter;

                exl.AddTable("A", 4, lstSLAIncidents, new[] { 30, 30, 30, 30, 30,30,30});
                exl.MargeCell("A1:E1", "Format for Certificate of Help Desk Support", DynamicExcel.CellAlignment.Middle,DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A4:B4", QName, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C4:E4", "Months", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A5:B5", "Client : " + BrnchName, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C5:C5", QM1, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D5:D5", QM2, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E5:E5", QM3, DynamicExcel.CellAlignment.Middle);                

                exl.MargeCell("A6:A10", "Total Incidents", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A11:A11", "", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("B6:B6", "Raised", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B7:B7", "Closed / Resolved", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B8:B8", "Active", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B9:B9", "New", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B10:B10", "On Hold", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B11:B11", "", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("C6:C6", lstSLAIncidents[0].M1, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D6:D6", lstSLAIncidents[0].M2, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E6:E6", lstSLAIncidents[0].M3, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("C7:C7", lstSLAIncidents[1].M1, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D7:D7", lstSLAIncidents[1].M2, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E7:E7", lstSLAIncidents[1].M3, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("C8:C8", lstSLAIncidents[2].M1, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D8:D8", lstSLAIncidents[2].M2, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E8:E8", lstSLAIncidents[2].M3, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("C9:C9", lstSLAIncidents[3].M1, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D9:D9", lstSLAIncidents[3].M2, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E9:E9", lstSLAIncidents[3].M3, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("C10:C10", lstSLAIncidents[4].M1, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D10:D10", lstSLAIncidents[4].M2, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E10:E10", lstSLAIncidents[4].M3, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("C11:C11", "", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D12:D12", "", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E13:E13", "", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A12:A16", "Total Defects", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("B12:B12", "Total", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B13:B13", "A. Category R1", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B14:B14", "B. Category R2", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B15:B15", "C. Category R3", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B16:B16", "Average Resolution Time - Defects", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("C12:C12", lstSLAIncidents[5].M1, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D12:D12", lstSLAIncidents[5].M2, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E12:E12", lstSLAIncidents[5].M3, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("C13:C13", lstSLAIncidents[6].M1, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D13:D13", lstSLAIncidents[6].M2, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E13:E13", lstSLAIncidents[6].M3, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("C14:C14", lstSLAIncidents[7].M1, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D14:D14", lstSLAIncidents[7].M2, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E14:E14", lstSLAIncidents[7].M3, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("C15:C15", lstSLAIncidents[8].M1, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D15:D15", lstSLAIncidents[8].M2, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E15:E15", lstSLAIncidents[8].M3, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("C16:C16", lstSLAIncidents[9].M1, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D16:D16", lstSLAIncidents[9].M2, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E16:E16", lstSLAIncidents[9].M3, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A21:B21", "(Signature of Authorized representative of Vendor)", DynamicExcel.CellAlignment.Middle,DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D21:E21", "(Signature of Warehouse Manager)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A23:A23", "Dated :" + CDate, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                


                exl.Save();
            }
            return excelFile;
        }

        #endregion

        #region DownTime SITL

        public void AddEditDTSitl(PPGDownTime ObjDTSLA)
        {
            /* Commodity Type:1.HAZ 2.Non HAZ */
            string GeneratedClientId = "0";
            string ReturnObj = "";
            
            var DTEndDate = (dynamic)null;
            DTEndDate = Convert.ToDateTime(ObjDTSLA.CompletionDateTime).ToString("yyyy-MM-dd HH:mm:ss");

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TicketId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDTSLA.TicketId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDTSLA.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Reason", MySqlDbType = MySqlDbType.VarChar, Value = ObjDTSLA.RemarksSitl });            
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDateTime", MySqlDbType = MySqlDbType.DateTime, Value = DTEndDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompletionStatus", MySqlDbType = MySqlDbType.VarChar, Value = ObjDTSLA.CompletionStatus });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });            
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditDTSitlRegistration", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Ticket Generated Successfully.";
                    _DBResponse.Data = GeneratedClientId;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;

            }
        }


        public void GetAllDTSitlRegistration(int TicketId, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TicketId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = TicketId });            
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllDTSitlRegistration", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPGDownTime> LstSLARegistration = new List<PPGDownTime>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSLARegistration.Add(new PPGDownTime
                    {
                        TicketId = Convert.ToInt32(Result["TicketId"]),
                        TicketNo = Result["TicketNo"].ToString(),
                        RaisedOn = Result["RaisedOn"].ToString(),
                        RemarksCwc = Result["Reason"].ToString(),
                        RemarksSitl = Result["Remarks"].ToString(),
                        StartDateTime = Result["StartDateTime"].ToString(),
                        CompletionDateTime = Result["CompletionDateTime"].ToString(),
                        CompletionStatus = Result["CompletionStatus"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSLARegistration;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetViewDTSitlRegistration(int TicketId, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TicketId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = TicketId });            
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllDTSitlRegistration", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPGDownTime objSLARegistration = new PPGDownTime();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objSLARegistration.TicketId = Convert.ToInt32(Result["TicketId"]);
                    objSLARegistration.TicketNo = Result["TicketNo"].ToString();
                    objSLARegistration.RaisedOn = Result["RaisedOn"].ToString();
                    objSLARegistration.RaisedBy = Result["RaisedBy"].ToString();
                    objSLARegistration.RemarksCwc = Result["Reason"].ToString();
                    objSLARegistration.RemarksSitl = Result["Remarks"].ToString();
                    objSLARegistration.StartDateTime = Result["StartDateTime"].ToString();
                    objSLARegistration.CompletionDateTime = Result["CompletionDateTime"].ToString();
                    objSLARegistration.CompletionStatus = Result["CompletionStatus"].ToString();

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objSLARegistration;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetAllDTSitlRegistrationSearch(string SearchValue, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();            
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = SearchValue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllDTSitlRegistrationSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPGDownTime> objSLARegistration = new List<PPGDownTime>();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    objSLARegistration.Add(new PPGDownTime
                    {
                        TicketId = Convert.ToInt32(Result["TicketId"]),
                        TicketNo = Result["TicketNo"].ToString(),
                        RaisedOn = Result["RaisedOn"].ToString(),
                        RemarksCwc = Result["Reason"].ToString(),
                        RemarksSitl = Result["Remarks"].ToString(),
                        StartDateTime = Result["StartDateTime"].ToString(),
                        CompletionDateTime = Result["CompletionDateTime"].ToString(),
                        CompletionStatus = Result["CompletionStatus"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objSLARegistration;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        #endregion 

        #region DownTime CWC

        public void AddEditDTCwc(PPGDownTime ObjDTSLA)
        {
            /* Commodity Type:1.HAZ 2.Non HAZ */
            string GeneratedClientId = "0";
            string ReturnObj = "";
            var RaisedDate = (dynamic)null;
            RaisedDate = Convert.ToDateTime(ObjDTSLA.RaisedOn).ToString("yyyy-MM-dd HH:mm:ss");

            var DTStartDate = (dynamic)null;
            DTStartDate = Convert.ToDateTime(ObjDTSLA.StartDateTime).ToString("yyyy-MM-dd HH:mm:ss");

            var DTEndDate = (dynamic)null;
            DTEndDate = Convert.ToDateTime(ObjDTSLA.CompletionDateTime).ToString("yyyy-MM-dd HH:mm:ss");

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDTSLA.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Reason", MySqlDbType = MySqlDbType.VarChar, Value = ObjDTSLA.RemarksCwc });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StartDateTime", MySqlDbType = MySqlDbType.DateTime, Value = DTStartDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDateTime", MySqlDbType = MySqlDbType.DateTime, Value = DTEndDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RaisedOn", MySqlDbType = MySqlDbType.DateTime, Size = 25, Value = RaisedDate });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditDTCwcRegistration", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Downtime Entered Successfully.";
                    _DBResponse.Data = GeneratedClientId;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;

            }
        }


        public void GetAllDTCwcRegistration(int TicketId, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TicketId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = TicketId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllDTCwcRegistration", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPGDownTime> LstSLARegistration = new List<PPGDownTime>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSLARegistration.Add(new PPGDownTime
                    {
                        TicketId = Convert.ToInt32(Result["TicketId"]),
                        TicketNo = Result["TicketNo"].ToString(),
                        RaisedOn = Result["RaisedOn"].ToString(),
                        RemarksCwc = Result["Reason"].ToString(),
                        StartDateTime = Result["StartDateTime"].ToString(),
                        CompletionDateTime = Result["CompletionDateTime"].ToString(),
                        CompletionStatus = Result["CompletionStatus"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSLARegistration;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetViewDTCwcRegistration(int TicketId, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TicketId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = TicketId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllDTCwcRegistration", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PPGDownTime objSLARegistration = new PPGDownTime();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objSLARegistration.TicketId = Convert.ToInt32(Result["TicketId"]);
                    objSLARegistration.TicketNo = Result["TicketNo"].ToString();
                    objSLARegistration.RaisedOn = Result["RaisedOn"].ToString();
                    objSLARegistration.RaisedBy = Result["RaisedBy"].ToString();
                    objSLARegistration.RemarksCwc = Result["Reason"].ToString();
                    objSLARegistration.RemarksSitl = Result["Remarks"].ToString();
                    objSLARegistration.StartDateTime = Result["StartDateTime"].ToString();
                    objSLARegistration.CompletionDateTime = Result["CompletionDateTime"].ToString();
                    objSLARegistration.CompletionStatus = Result["CompletionStatus"].ToString();

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objSLARegistration;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetAllDTCwcRegistrationSearch(string SearchValue, int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = SearchValue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllDTCwcRegistrationSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PPGDownTime> objSLARegistration = new List<PPGDownTime>();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    objSLARegistration.Add(new PPGDownTime
                    {
                        TicketId = Convert.ToInt32(Result["TicketId"]),
                        TicketNo = Result["TicketNo"].ToString(),
                        RaisedOn = Result["RaisedOn"].ToString(),
                        RemarksCwc = Result["Reason"].ToString(),
                        RemarksSitl = Result["Remarks"].ToString(),
                        StartDateTime = Result["StartDateTime"].ToString(),
                        CompletionDateTime = Result["CompletionDateTime"].ToString(),
                        CompletionStatus = Result["CompletionStatus"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objSLARegistration;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        #endregion 

        #region SLA Report

        public void SLAReportDTEXCEL(PPGSLAReportDT Obj)
        {
            int Status = 0;

            PPGSLAReportDT model = new PPGSLAReportDT();
            List<PPGSLAReportIncidentsUPDT> lstSLAIncidentsUPDT = new List<PPGSLAReportIncidentsUPDT>();
            List<PPGSLAReportIncidentsSHDT> lstSLAIncidentsSHDT = new List<PPGSLAReportIncidentsSHDT>();
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Year", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Year });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Quarter", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Quarter });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getSLAReportDT", CommandType.StoredProcedure, DParam);


            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstSLAIncidentsUPDT.Add(new PPGSLAReportIncidentsUPDT
                    {
                        StartDateTime = Result["StartDateTime"].ToString(),
                        EndDateTime = Result["EndDateTime"].ToString(),
                        Reason = Result["Reason"].ToString(),
                        Duration = Result["Duration"].ToString()
                    });
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstSLAIncidentsSHDT.Add(new PPGSLAReportIncidentsSHDT
                        {
                            StartDateTime = Result["StartDateTime"].ToString(),
                            EndDateTime = Result["EndDateTime"].ToString(),
                            Reason = Result["Reason"].ToString(),
                            Duration = Result["Duration"].ToString()
                        });
                    }
                }

                model.lstIncidentsUP = lstSLAIncidentsUPDT;
                model.lstIncidentsSH = lstSLAIncidentsSHDT;

                DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = ConSLADTDetail(model, Obj.Year, Obj.Quarter);

                //}                
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();

            }
        }

        private string ConSLADTDetail(PPGSLAReportDT model, string Year, string Quarter)
        {
            var BrnchName = "";
            var BrnchId = Convert.ToInt32(HttpContext.Current.Session["BranchId"]);
            if (BrnchId == 1)
            {
                BrnchName = "CFS Kandla";
            }
            if (BrnchId == 2)
            {
                BrnchName = "CFS Kolkata";
            }
            if (BrnchId == 3)
            {
                BrnchName = "ICD Patparganj";
            }
            if (BrnchId == 4)
            {
                BrnchName = "CFS Hyderabad";
            }
            if (BrnchId == 6)
            {
                BrnchName = "ICD Dashrath";
            }
            if (BrnchId == 7)
            {
                BrnchName = "CFS Whitefield";
            }
            if (BrnchId == 10)
            {
                BrnchName = "CFS Ambad";
            }
            if (BrnchId == 14)
            {
                BrnchName = "CFS Tuticorin";
            }

            string QName = "";
            string QM1 = "";
            string QM2 = "";
            string QM3 = "";

            string CDate = DateTime.Now.ToShortDateString();

            if (Quarter == "Q1")
            {
                int QYear = int.Parse(Year) - 1;
                QName = "Period : " + "30/Dec/" + QYear + "-29/Mar/" + Year;
                QM1 = "Jan" + " _ " + Year;
                QM2 = "Feb" + " _ " + Year;
                QM3 = "Mar" + " _ " + Year;
            }
            if (Quarter == "Q2")
            {
                QName = "Period : " + "30/Mar/" + Year + "-29/Jun/" + Year;
                QM1 = "Apr" + " _ " + Year;
                QM2 = "May" + " _ " + Year;
                QM3 = "Jun" + " _ " + Year;
            }
            if (Quarter == "Q3")
            {
                QName = "Period : " + "30/Jun/" + Year + "-29/Sep/" + Year;
                QM1 = "Jul" + " _ " + Year;
                QM2 = "Aug" + " _ " + Year;
                QM3 = "Sep" + " _ " + Year;
            }
            if (Quarter == "Q4")
            {
                QName = "Period : " + "30/Oct/" + Year + "-29/Dec/" + Year;
                QM1 = "Oct" + " _ " + Year;
                QM2 = "Nov" + " _ " + Year;
                QM3 = "Dec" + " _ " + Year;
            }

            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/Docs/Excel")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Docs/Excel"));
            }
            var excelFile = HttpContext.Current.Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");

            using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
            {
                var title = @"CENTRAL WAREHOUSING CORPORATION"
                        + Environment.NewLine + "(A Govt. of India Undertaking)"
                        + Environment.NewLine + "ICD Patparganj-Delhi"
                        + Environment.NewLine + Environment.NewLine
                        + "Format for Certificate of System Availability";
                string typeOfValue = "";

                typeOfValue = "During Period Of " + Year + " Quarter " + Quarter;

                //exl.AddTable("A", 4, model, new[] { 30, 30, 30, 30, 30, 30, 30 });
                exl.MargeCell("A3:K3", "Annexure - X", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("A6:K6", "Format for Certificate of System Availability", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A8:C8", "Client", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A9:C9", "Application :", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A10:C10", "Report Period :", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A11:C11", "Report Generated on :", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A14:K14", "Schedule Down Time: : " + BrnchName, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("C5:C5", QM1, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D5:D5", QM2, DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E5:E5", QM3, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A6:A10", "Total Incidents", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("A11:A11", "", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("B6:B6", "Raised", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B7:B7", "Closed / Resolved", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B8:B8", "Active", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B9:B9", "New", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B10:B10", "On Hold", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B11:B11", "", DynamicExcel.CellAlignment.Middle);

                //exl.MargeCell("C6:C6", lstSLAIncidents[0].M1, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("D6:D6", lstSLAIncidents[0].M2, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("E6:E6", lstSLAIncidents[0].M3, DynamicExcel.CellAlignment.Middle);

                //exl.MargeCell("C7:C7", lstSLAIncidents[1].M1, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("D7:D7", lstSLAIncidents[1].M2, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("E7:E7", lstSLAIncidents[1].M3, DynamicExcel.CellAlignment.Middle);

                //exl.MargeCell("C8:C8", lstSLAIncidents[2].M1, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("D8:D8", lstSLAIncidents[2].M2, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("E8:E8", lstSLAIncidents[2].M3, DynamicExcel.CellAlignment.Middle);

                //exl.MargeCell("C9:C9", lstSLAIncidents[3].M1, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("D9:D9", lstSLAIncidents[3].M2, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("E9:E9", lstSLAIncidents[3].M3, DynamicExcel.CellAlignment.Middle);

                //exl.MargeCell("C10:C10", lstSLAIncidents[4].M1, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("D10:D10", lstSLAIncidents[4].M2, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("E10:E10", lstSLAIncidents[4].M3, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("C11:C11", "", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("D12:D12", "", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("E13:E13", "", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A12:A16", "Total Defects", DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("B12:B12", "Total", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B13:B13", "A. Category R1", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B14:B14", "B. Category R2", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B15:B15", "C. Category R3", DynamicExcel.CellAlignment.Middle);
                exl.MargeCell("B16:B16", "Average Resolution Time - Defects", DynamicExcel.CellAlignment.Middle);

                //exl.MargeCell("C12:C12", lstSLAIncidents[5].M1, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("D12:D12", lstSLAIncidents[5].M2, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("E12:E12", lstSLAIncidents[5].M3, DynamicExcel.CellAlignment.Middle);

                //exl.MargeCell("C13:C13", lstSLAIncidents[6].M1, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("D13:D13", lstSLAIncidents[6].M2, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("E13:E13", lstSLAIncidents[6].M3, DynamicExcel.CellAlignment.Middle);

                //exl.MargeCell("C14:C14", lstSLAIncidents[7].M1, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("D14:D14", lstSLAIncidents[7].M2, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("E14:E14", lstSLAIncidents[7].M3, DynamicExcel.CellAlignment.Middle);

                //exl.MargeCell("C15:C15", lstSLAIncidents[8].M1, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("D15:D15", lstSLAIncidents[8].M2, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("E15:E15", lstSLAIncidents[8].M3, DynamicExcel.CellAlignment.Middle);

                //exl.MargeCell("C16:C16", lstSLAIncidents[9].M1, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("D16:D16", lstSLAIncidents[9].M2, DynamicExcel.CellAlignment.Middle);
                //exl.MargeCell("E16:E16", lstSLAIncidents[9].M3, DynamicExcel.CellAlignment.Middle);

                exl.MargeCell("A21:B21", "(Signature of Authorized representative of Vendor)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);
                exl.MargeCell("D21:E21", "(Signature of Warehouse Manager)", DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);

                exl.MargeCell("A23:A23", "Dated :" + CDate, DynamicExcel.CellAlignment.Middle, DynamicExcel.CellFontStyle.Bold);




                exl.Save();
            }
            return excelFile;
        }

        #endregion

    }





}