using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using System.Data;
using MySql.Data.MySqlClient;
using System.Globalization;
using Newtonsoft.Json;
using CwcExim.Models;
using CwcExim.Areas.Export.Models;
using CwcExim.Areas.CashManagement.Models;

namespace CwcExim.Repositories
{
    public class DSR_ExportRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
         
        public void MenuAccessRight(int RoleId, int BranchId, int ModuleId, int MenuId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_MenuId", MySqlDbType = MySqlDbType.Int32, Value = MenuId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ModuleId", MySqlDbType = MySqlDbType.Int32, Value = ModuleId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Branch_Id", MySqlDbType = MySqlDbType.Int32, Value = BranchId });

            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetMenuWiseAccessRight", CommandType.StoredProcedure, dpram);
            IList<DSR_Menu> lstMenu = new List<DSR_Menu>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (result.Read())
                {

                    lstMenu.Add(new DSR_Menu
                    {
                        CanAdd = Convert.ToInt32(result["CanAdd"]),
                        CanEdit = Convert.ToInt32(result["CanEdit"]),
                        CanDelete = Convert.ToInt32(result["CanDelete"]),
                        CanView = Convert.ToInt32(result["CanView"])

                    });
                }
                _DBResponse.Data = new { lstMenu };
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";

            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }




        #region CCIN 
        public void ListOfPackUQCForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_UQCCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPackUQCForPage", CommandType.StoredProcedure, Dparam);
            IList<PackUQCForPage> lstPackUQC = new List<PackUQCForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstPackUQC.Add(new PackUQCForPage
                    {
                        PackUQCId = Convert.ToInt32(Result["PackId"]),
                        PackUQCDescription = Result["PackName"].ToString(),
                        PackUQCCode = Result["PackCode"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstPackUQC, State };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }
        public void GetPortOfLoadingCcin()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortOfLoadingccin", CommandType.StoredProcedure);
            List<Port> LstPort = new List<Port>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new Port
                    {
                        PortName = Convert.ToString(Result["PortName"]),
                        PortId = Convert.ToInt32(Result["PortId"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPort;
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
        public void GetPortOfDestccin()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortOfDestccin", CommandType.StoredProcedure);
            List<Port> LstPort = new List<Port>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new Port
                    {
                        PortName = Convert.ToString(Result["PortName"]),
                        PortId = Convert.ToInt32(Result["PortId"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPort;
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
        public void GetPortOfLoading()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortOfLoading", CommandType.StoredProcedure);
            List<Port> LstPort = new List<Port>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new Port
                    {
                        PortName = Convert.ToString(Result["PortName"]),
                        PortId = Convert.ToInt32(Result["PortId"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPort;
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

        public void GetSBList()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSBList", CommandType.StoredProcedure);
            List<DSR_CCINEntry> LstSB = new List<DSR_CCINEntry>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new DSR_CCINEntry
                    {
                        SBNo = Convert.ToString(Result["SBNo"]),
                        SBId = Convert.ToInt32(Result["SBId"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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

        public void GetSBDetailsBySBId(int SBId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_SBId", MySqlDbType = MySqlDbType.Int32, Value = SBId });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetSBDetailsBySBId", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                DSR_CCINEntry objCCINEntry = new DSR_CCINEntry();

                if (Result != null && Result.Tables.Count > 0)
                {
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        Status = 1;
                        objCCINEntry.SBNo = Convert.ToString(Result.Tables[0].Rows[0]["SB_NO"]);
                        objCCINEntry.SBDate = Convert.ToString(Result.Tables[0].Rows[0]["SB_DATE"]);
                        objCCINEntry.ExporterName = Convert.ToString(Result.Tables[0].Rows[0]["EXP_NAME"]);
                        objCCINEntry.ExporterId = Convert.ToInt32(Result.Tables[0].Rows[0]["EXP_ID"]);
                        objCCINEntry.FOB = Convert.ToDecimal(Result.Tables[0].Rows[0]["FOB"]);
                        objCCINEntry.Package = Convert.ToInt32(Result.Tables[0].Rows[0]["Quantity"]);
                        objCCINEntry.PackUQCDescription = Convert.ToString(Result.Tables[0].Rows[0]["PackUQCDescription"]);
                        objCCINEntry.PackUQCCode = Convert.ToString(Result.Tables[0].Rows[0]["PackUQCCode"]);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objCCINEntry;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }

        public void AddEditCCINEntry(DSR_CCINEntry objCCINEntry,string PostPaymentXML)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CCINDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCCINEntry.CCINDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.SBNo }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SBDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCCINEntry.SBDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SBType", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.SBType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ExporterName", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.ExporterName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ExporterId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.ExporterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.ShippingLineId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.ShippingLineName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.GodownId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GodownName", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.GodownName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.CHAId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.CHAName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ConsigneeName", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.ConsigneeName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ConsigneeAdd", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.ConsigneeAdd });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.CountryId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_StateId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.StateId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CityId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.CityId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortOfLoadingId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.PortOfLoadingId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortOfDischarge", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PortOfDischarge });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Package", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.Package });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.Weight });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FOB", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.FOB });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.CommodityId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ChargeXML", MySqlDbType = MySqlDbType.Text, Value = PostPaymentXML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.CargoType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortofDestId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.PortOfDestId });

            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.InvoiceId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.String, Value = "Tax" });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objCCINEntry.CCINDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PartyName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.AllTotal });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.TotalCGST });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.TotalSGST });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.TotalIGST });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.TotalAmt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.RoundUp });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = objCCINEntry.InvoiceValue });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OTHr", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.OTEHr });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NoOfVehicle", MySqlDbType = MySqlDbType.Int32, Value = objCCINEntry.NoOfVehicles });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PaymentMode", MySqlDbType = MySqlDbType.String, Value = objCCINEntry.PaymentMode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackageType", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.PackageType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackUQCDesc", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.PackUQCDescription });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PackUQCCode", MySqlDbType = MySqlDbType.VarChar, Value = objCCINEntry.PackUQCCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(objCCINEntry.IsSEZ) });

            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditCCINEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Message = (Result == 1) ? "CCIN Entry Saved Successfully" : "CCIN Entry Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "This Party has insufficient SD Balance to save the Invoice.";
                    _DBResponse.Status = 0;
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Unable to Edit!Next Step is Done.";
                    _DBResponse.Status = 0;
                }
                else if (Result == 6)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "SB No Already Exist !";
                    _DBResponse.Status = 0;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
        }

        public void AddEditCCINEntryApproval(int Id, bool IsApproved)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CCIN_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IsApproved", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(IsApproved) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditCcinEntryApproval", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "CCIN Entry Approval Saved Successfully";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
        }

        public void GetAllCCINEntry(string ListOrEntry = "")
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntry", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<DSR_CCINEntry> CCINEntryList = new List<DSR_CCINEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        DSR_CCINEntry objCCINEntry = new DSR_CCINEntry();
                        objCCINEntry.Id = Convert.ToInt32(dr["Id"]);
                        objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
                        objCCINEntry.CCINNo = Convert.ToString(dr["CCINNo"]);
                        objCCINEntry.CCINDate = Convert.ToString(dr["CCINDate"]);
                        objCCINEntry.SBNo = Convert.ToString(dr["SBNo"].ToString());
                        objCCINEntry.SBDate = Convert.ToString(dr["SBDate"]);

                        CCINEntryList.Add(objCCINEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CCINEntryList;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }

        public void GetAllCCINEntryForPage(int Page, string ListOrEntry = "")
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
               lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntryForPage", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<DSR_CCINEntry> CCINEntryList = new List<DSR_CCINEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        DSR_CCINEntry objCCINEntry = new DSR_CCINEntry();
                        objCCINEntry.Id = Convert.ToInt32(dr["Id"]);
                        objCCINEntry.PartyId = Convert.ToInt32(dr["PartyId"]);
                        objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
                        objCCINEntry.CCINNo = Convert.ToString(dr["CCINNo"]);
                        objCCINEntry.CCINDate = Convert.ToString(dr["CCINDate"]);
                        objCCINEntry.SBNo = Convert.ToString(dr["SBNo"].ToString());
                        objCCINEntry.SBDate = Convert.ToString(dr["SBDate"]);

                        CCINEntryList.Add(objCCINEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CCINEntryList;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }

        public void GetAllCCINEntrySearch(string SearchValue)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                lstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
                
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntrySearch", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<DSR_CCINEntry> CCINEntryList = new List<DSR_CCINEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        DSR_CCINEntry objCCINEntry = new DSR_CCINEntry();
                        objCCINEntry.Id = Convert.ToInt32(dr["Id"]);
                        objCCINEntry.PartyId = Convert.ToInt32(dr["PartyId"]);
                        objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
                        objCCINEntry.CCINNo = Convert.ToString(dr["CCINNo"]);
                        objCCINEntry.CCINDate = Convert.ToString(dr["CCINDate"]);
                        objCCINEntry.SBNo = Convert.ToString(dr["SBNo"].ToString());
                        objCCINEntry.SBDate = Convert.ToString(dr["SBDate"]);

                        CCINEntryList.Add(objCCINEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CCINEntryList;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }
        public void GetAllCCINEntryApprovalForPage(int Page,string SBNo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
               // lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
                lstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = SBNo });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllCCINEntryApprovalForPage", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<DSR_CCINEntry> CCINEntryList = new List<DSR_CCINEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        DSR_CCINEntry objCCINEntry = new DSR_CCINEntry();
                        objCCINEntry.Id = Convert.ToInt32(dr["Id"]);
                        // objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);
                        objCCINEntry.CCINNo = Convert.ToString(dr["CCINNo"]);
                        objCCINEntry.CCINDate = Convert.ToString(dr["CCINDate"]);
                        objCCINEntry.SBNo = Convert.ToString(dr["SBNo"].ToString());
                        objCCINEntry.SBDate = Convert.ToString(dr["SBDate"]);
                        objCCINEntry.IsApproved = Convert.ToBoolean(dr["IsApproved"]);

                        CCINEntryList.Add(objCCINEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CCINEntryList;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }
        

        public void GetCCINPartyList()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCCINPartyList", CommandType.StoredProcedure);
            List<Party> LstParty = new List<Party>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstParty.Add(new Party
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstParty;
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


        public void GetCCINCharges(int CCINEntryId, int PartyId, decimal Weight, decimal FOB, string CargoType,int NoOfVehicles)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                int OperationType = 2; // For Export
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_CcinEntry_Id", MySqlDbType = MySqlDbType.Int32, Value = CCINEntryId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Weight });
                LstParam.Add(new MySqlParameter { ParameterName = "In_FOB", MySqlDbType = MySqlDbType.Decimal, Value = FOB });
                LstParam.Add(new MySqlParameter { ParameterName = "In_OperationType", MySqlDbType = MySqlDbType.Int32, Value = OperationType });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CargoType", MySqlDbType = MySqlDbType.VarChar, Value = CargoType });
                LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfVehicles", MySqlDbType = MySqlDbType.Int32, Value = NoOfVehicles });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetCcinCharges", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                DSR_CCINEntry objCCINEntry = new DSR_CCINEntry();
                List<PostPaymentCharge> lstPostPaymentChrg = new List<PostPaymentCharge>();

                if (Result != null && Result.Tables.Count > 0)
                {

                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        Status = 1;
                        foreach (DataRow dr in Result.Tables[0].Rows)
                        {
                            lstPostPaymentChrg.Add(new PostPaymentCharge()
                            {
                                ChargeId = lstPostPaymentChrg.Count + 1,
                                OperationId = Convert.ToInt32(dr["OperationId"]),
                                Clause = Convert.ToString(dr["Clause"]),
                                ChargeName = Convert.ToString(dr["ChargeName"]),
                                ChargeType = Convert.ToString(dr["ChargeType"]),
                                SACCode = Convert.ToString(dr["SACCode"]),
                                Quantity = Convert.ToInt32(dr["Quantity"]),
                                Rate = Convert.ToDecimal(dr["Rate"]),
                                Amount = Convert.ToDecimal(dr["Amount"]),
                                Discount = 0,
                                Taxable = 0,
                                CGSTPer = Convert.ToDecimal(dr["CGSTPer"]),
                                SGSTPer = Convert.ToDecimal(dr["SGSTPer"]),
                                IGSTPer = Convert.ToDecimal(dr["IGSTPer"]),
                                CGSTAmt = Convert.ToDecimal(dr["CGSTAmt"]),
                                SGSTAmt = Convert.ToDecimal(dr["SGSTAmt"]),
                                IGSTAmt = Convert.ToDecimal(dr["IGSTAmt"]),
                                Total = Convert.ToDecimal(dr["Total"]),
                                ActualFullCharge = Convert.ToDecimal(dr["ActualFullCharge"]),
                            });
                        }

                        objCCINEntry.IsPartyStateInCompState = Convert.ToBoolean(Result.Tables[0].Rows[0]["IsLocalGST"]);
                        if (lstPostPaymentChrg.Count > 0)
                        {
                            objCCINEntry.lstPostPaymentChrg = lstPostPaymentChrg;
                            objCCINEntry.PaymentSheetModelJson = "";
                           // objCCINEntry.PaymentSheetModelJson = Newtonsoft.Json.JsonConvert.SerializeObject(lstPostPaymentChrg).ToString();
                        }

                    }

                    if (Result.Tables[1].Rows.Count > 0)
                    {
                        objCCINEntry.PartySDBalance = Convert.ToDecimal(Result.Tables[1].Rows[0]["SDBalance"]);
                    }
                    if (Result.Tables[2].Rows.Count > 0)
                    {
                        //objCCINEntry.PartySDBalance = Convert.ToDecimal(Result.Tables[1].Rows[0]["SDBalance"]);
                        objCCINEntry.PaymentMode = Result.Tables[2].Rows[0]["IN_MODE"].ToString();
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objCCINEntry;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }

        public void GetCCINEntryById(int Id)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCCINEntryById", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            DSR_CCINEntry objCCINEntry = new DSR_CCINEntry();
            try
            {

                while (Result.Read())
                {
                    objCCINEntry.nonApproval = Convert.ToString(Result["nonApproval"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objCCINEntry.Id = Convert.ToInt32(Result["Id"]);
                        objCCINEntry.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                        objCCINEntry.InvoiceNo = Convert.ToString(Result["InvoiceNo"]);
                        objCCINEntry.CCINNo = Convert.ToString(Result["CCINNo"]);
                        objCCINEntry.CCINDate = Convert.ToString(Result["CCINDate"]);
                        objCCINEntry.SBNo = Convert.ToString(Result["SBNo"]);
                        objCCINEntry.SBDate = Convert.ToString(Result["SBDate"]);
                        objCCINEntry.SBType = Convert.ToInt32(Result["SBType"]);
                        objCCINEntry.ExporterId = Convert.ToInt32(Result["ExporterId"]);
                        objCCINEntry.ExporterName = Convert.ToString(Result["ExporterName"]);
                        objCCINEntry.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                        objCCINEntry.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                        objCCINEntry.CHAId = Convert.ToInt32(Result["CHAId"]);
                        objCCINEntry.CHAName = Convert.ToString(Result["CHAName"]);
                        objCCINEntry.ConsigneeName = Convert.ToString(Result["ConsigneeName"]);
                        objCCINEntry.ConsigneeAdd = Convert.ToString(Result["ConsigneeAdd"]);
                        objCCINEntry.CountryId = Convert.ToInt32(Result["CountryId"]);
                        objCCINEntry.StateId = Convert.ToInt32(Result["StateId"]);
                        objCCINEntry.CityId = Convert.ToInt32(Result["CityId"]);
                        objCCINEntry.PortOfLoadingId = Convert.ToInt32(Result["PortOfLoadingId"]);
                        objCCINEntry.PortOfLoadingName = Convert.ToString(Result["PortOfLoadingName"]);
                        objCCINEntry.PortOfDischarge = Convert.ToString(Result["PortOfDischarge"]);
                        objCCINEntry.Package = Convert.ToInt32(Result["Package"]);
                        objCCINEntry.Weight = Convert.ToDecimal(Result["Weight"]);
                        objCCINEntry.FOB = Convert.ToDecimal(Result["FOB"]);
                        objCCINEntry.CommodityId = Convert.ToInt32(Result["CommodityId"]);
                        objCCINEntry.CommodityName = Convert.ToString(Result["CommodityName"]);
                        objCCINEntry.PartyId = Convert.ToInt32(Result["PartyId"]);
                        objCCINEntry.PartyName = Convert.ToString(Result["PartyName"]);
                        objCCINEntry.Remarks = Convert.ToString(Result["Remarks"]);
                        objCCINEntry.IsInGateEntry = Convert.ToInt32(Result["IsInGateEntry"]);
                        objCCINEntry.IsApproved = Convert.ToBoolean(Result["IsApproved"]);
                        objCCINEntry.CargoType = Convert.ToInt32(Result["CargoType"]);
                        objCCINEntry.GodownId = Convert.ToInt32(Result["GodownId"]);
                        objCCINEntry.GodownName = Convert.ToString(Result["GodownName"]);
                        objCCINEntry.PortOfDestId = Convert.ToInt32(Result["PortOfDestId"]);
                        objCCINEntry.PortOfDestName = Convert.ToString(Result["PortOfDestName"]);
                        objCCINEntry.CartingType = Convert.ToString(Result["CartingType"]);
                        objCCINEntry.OTEHr = Convert.ToInt32(Result["OTHr"]);
                        objCCINEntry.PackageType = Result["PackageType"].ToString();
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objCCINEntry.lstPostPaymentChrg.Add(new PostPaymentCharge()
                        {
                            ChargeId = objCCINEntry.lstPostPaymentChrg.Count + 1,
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            Clause = "",
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            SACCode = Convert.ToString(Result["SACCode"]),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = 0,
                            Taxable = 0,
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            ActualFullCharge = Convert.ToDecimal(Result["ActualFullCharge"]),
                        });
                    }
                    objCCINEntry.IsPartyStateInCompState = Convert.ToBoolean(Result["IsLocalGST"]);
                    if (objCCINEntry.lstPostPaymentChrg.Count > 0)
                    {
                        objCCINEntry.PaymentSheetModelJson = Newtonsoft.Json.JsonConvert.SerializeObject(objCCINEntry.lstPostPaymentChrg).ToString();
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objCCINEntry;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        public void GetCCINEntryForEdit(int Id,int PartyId)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            lstParam.Add(new MySqlParameter { ParameterName = "In_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCCINDetForEdit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            DSR_CCINEntry objCCINEntry = new DSR_CCINEntry();
 
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    objCCINEntry.Id = Convert.ToInt32(Result["Id"]);
                    objCCINEntry.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    objCCINEntry.InvoiceNo = Convert.ToString(Result["InvoiceNo"]);
                    objCCINEntry.CCINNo = Convert.ToString(Result["CCINNo"]);
                    objCCINEntry.CCINDate = Convert.ToString(Result["CCINDate"]);
                    objCCINEntry.SBNo = Convert.ToString(Result["SBNo"]);
                    objCCINEntry.SBDate = Convert.ToString(Result["SBDate"]);
                    objCCINEntry.SBType = Convert.ToInt32(Result["SBType"]);
                    objCCINEntry.ExporterId = Convert.ToInt32(Result["ExporterId"]);
                    objCCINEntry.ExporterName = Convert.ToString(Result["ExporterName"]);
                    objCCINEntry.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    objCCINEntry.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                    objCCINEntry.CHAId = Convert.ToInt32(Result["CHAId"]);
                    objCCINEntry.CHAName = Convert.ToString(Result["CHAName"]);
                    objCCINEntry.ConsigneeName = Convert.ToString(Result["ConsigneeName"]);
                    objCCINEntry.ConsigneeAdd = Convert.ToString(Result["ConsigneeAdd"]);
                    objCCINEntry.CountryId = Convert.ToInt32(Result["CountryId"]);
                    objCCINEntry.StateId = Convert.ToInt32(Result["StateId"]);
                    objCCINEntry.CityId = Convert.ToInt32(Result["CityId"]);
                    objCCINEntry.PortOfLoadingId = Convert.ToInt32(Result["PortOfLoadingId"]);
                    objCCINEntry.PortOfLoadingName = Convert.ToString(Result["PortOfLoadingName"]);
                    objCCINEntry.PortOfDischarge = Convert.ToString(Result["PortOfDischarge"]);
                    objCCINEntry.Package = Convert.ToInt32(Result["Package"]);
                    objCCINEntry.Weight = Convert.ToDecimal(Result["Weight"]);
                    objCCINEntry.FOB = Convert.ToDecimal(Result["FOB"]);
                    objCCINEntry.CommodityId = Convert.ToInt32(Result["CommodityId"]);
                    objCCINEntry.CommodityName = Convert.ToString(Result["CommodityName"]);
                    objCCINEntry.PartyId = Convert.ToInt32(Result["PartyId"]);
                    objCCINEntry.PartyName = Convert.ToString(Result["PartyName"]);
                    objCCINEntry.Remarks = Convert.ToString(Result["Remarks"]);
                    objCCINEntry.IsInGateEntry = Convert.ToInt32(Result["IsInGateEntry"]);
                    objCCINEntry.IsApproved = Convert.ToBoolean(Result["IsApproved"]);
                    objCCINEntry.CargoType = Convert.ToInt32(Result["CargoType"]);
                    objCCINEntry.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objCCINEntry.GodownName = Convert.ToString(Result["GodownName"]);
                    objCCINEntry.PortOfDestId = Convert.ToInt32(Result["PortOfDestId"]);
                    objCCINEntry.PortOfDestName = Convert.ToString(Result["PortOfDestName"]);
                    objCCINEntry.CartingType = Convert.ToString(Result["CartingType"]);
                    objCCINEntry.OTEHr = Convert.ToInt32(Result["OTHr"]);
                    objCCINEntry.PaymentMode= Convert.ToString(Result["PaymentMode"]);
                    objCCINEntry.PackageType = Convert.ToString(Result["PackageType"]);
                    objCCINEntry.PackUQCCode = Convert.ToString(Result["PackUQCCode"]);
                    objCCINEntry.PackUQCDescription = Convert.ToString(Result["PackUQCDesc"]);
                    objCCINEntry.IsSEZ = Convert.ToBoolean(Result["SEZ"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objCCINEntry.lstPostPaymentChrg.Add(new PostPaymentCharge()
                            {
                                ChargeId = objCCINEntry.lstPostPaymentChrg.Count + 1,
                                OperationId = Convert.ToInt32(Result["OperationId"]),
                                Clause = "",
                                ChargeName = Convert.ToString(Result["ChargeName"]),
                                ChargeType = Convert.ToString(Result["ChargeType"]),
                                SACCode = Convert.ToString(Result["SACCode"]),
                                Quantity = Convert.ToInt32(Result["Quantity"]),
                                Rate = Convert.ToDecimal(Result["Rate"]),
                                Amount = Convert.ToDecimal(Result["Amount"]),
                                Discount = 0,
                                Taxable = 0,
                                CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                                SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                                IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                                CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                                SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                                IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                                Total = Convert.ToDecimal(Result["Total"]),
                                ActualFullCharge = Convert.ToDecimal(Result["ActualFullCharge"]),
                            });
                        }
                    objCCINEntry.IsPartyStateInCompState = Convert.ToBoolean(Result["IsLocalGST"]);
                    if (objCCINEntry.lstPostPaymentChrg.Count > 0)
                    {
                        objCCINEntry.PaymentSheetModelJson = Newtonsoft.Json.JsonConvert.SerializeObject(objCCINEntry.lstPostPaymentChrg).ToString();
                    }
                }
                if(Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objCCINEntry.PartySDBalance = Convert.ToDecimal(Result["SDBalance"]);
                    }

                }

                    if (Status == 1)
                    {
                        _DBResponse.Data = objCCINEntry;
                        _DBResponse.Message = "Success";
                        _DBResponse.Status = 1;
                    }
                    else
                    {
                        _DBResponse.Data = null;
                        _DBResponse.Message = "Error";
                        _DBResponse.Status = 0;
                    }
                }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetCargoDetBTTById(int Id)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CarId", MySqlDbType = MySqlDbType.Int32, Value = Id });
        //    lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCargoDetBTTById", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            DSRBTTCargoDet objBTTCargo = new DSRBTTCargoDet();
            try
            {

               
                    while (Result.Read())
                    {
                        Status = 1;
                    objBTTCargo.ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]);
                    objBTTCargo.ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]);
                    objBTTCargo.CommodityName = Convert.ToString(Result["CommodityName"]);
                    objBTTCargo.NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]);
                    objBTTCargo.GrossWeight = Convert.ToDecimal(Result["GrossWeight"]);
                    objBTTCargo.Fob = Convert.ToDecimal(Result["Fob"]);

                }


                if (Status == 1)
                {
                    _DBResponse.Data = objBTTCargo;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetCargoDetShiftById(string Id)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CarId", MySqlDbType = MySqlDbType.VarChar, Value = Id });
            // /   lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetCargoDetShiftById", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            List<DSRBTTCargoDet> SbNoList = new List<DSRBTTCargoDet>();
            try
            {


                while (Result.Read())
                {
                    Status = 1;
                    DSRBTTCargoDet objBTTCargo = new DSRBTTCargoDet();

                    objBTTCargo.ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]);
                    objBTTCargo.ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]);
                    objBTTCargo.CommodityName = Convert.ToString(Result["CommodityName"]);
                    objBTTCargo.NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]);
                    objBTTCargo.GrossWeight = Convert.ToDecimal(Result["Weight"]);
                    objBTTCargo.Fob = Convert.ToDecimal(Result["FobValue"]);
                    objBTTCargo.exporter = Convert.ToString(Result["Exporter"]);
                    SbNoList.Add(objBTTCargo);
                }


                if (Status == 1)
                {
                    _DBResponse.Data = SbNoList;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void DeleteCCINEntry(int CCINEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CCINEntryId", MySqlDbType = MySqlDbType.Int32, Value = CCINEntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("DeleteCCINEntry", CommandType.StoredProcedure, Dparam);
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "CCIN Entry Deleted Successfully.";
                    _DBResponse.Status = 1;
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "CCIN Entry Can't be Deleted as It Is Used In Gate Entry.";
                    _DBResponse.Status = 2;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
        }
        public void GetCCINShippingLine()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCCINShippingLine", CommandType.StoredProcedure, DParam);
            List<ShippingLine> LstShippingLine = new List<ShippingLine>();
           
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstShippingLine.Add(new ShippingLine
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Result["ShippingLine"].ToString(),
                        //GodownId = Convert.ToInt32(Result["GodownId"]),
                        //GodownName = Convert.ToString(Result["GodownName"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstShippingLine;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        #endregion

        public void GetMCIN(string SBNo, string SBDATE)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = SBNo });


            LstParam.Add(new MySqlParameter { ParameterName = "in_SBDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(SBDATE).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMCIN", CommandType.StoredProcedure, DParam);
            LEOPage leo = new LEOPage();
            // ShippingLine LstShippingLine = new ShippingLine();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    leo.Id = Convert.ToInt32(Result["id"]);
                    leo.MCIN = Result["MCIN"].ToString();

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { leo };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void AddEditLEOEntry(LEOPage objLEOPage)
        {

            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = objLEOPage.Id });

            lstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.String, Value = objLEOPage.ShipBillNo }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SBDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objLEOPage.ShipBillDate).ToString("yyyy-MM-dd") });

            lstParam.Add(new MySqlParameter { ParameterName = "in_MCIN", MySqlDbType = MySqlDbType.String, Value = objLEOPage.MCIN });


            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });


            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditLEOEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {

                    _DBResponse.Message = (Result == 1) ? "LEO Entry Saved Successfully" : "LEO Entry Updated Successfully";
                    _DBResponse.Status = Result;
                }

                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
        }


        public void GetAllLEOEntryBYID(int id)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                // lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "in_ID", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = id });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllLEOPage", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                // List<LEOPage> LEOPageList = new List<LEOPage>();
                LEOPage LEOPageEntry = new LEOPage();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        // LEOPage LEOPageEntry = new LEOPage();
                        LEOPageEntry.Id = Convert.ToInt32(dr["Id"]);
                        // objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);

                        LEOPageEntry.ShipBillNo = Convert.ToString(dr["SB_NO"].ToString());
                        LEOPageEntry.ShipBillDate = Convert.ToString(dr["SB_DATE"]);
                        LEOPageEntry.MCIN = Convert.ToString(dr["MCIN"]);

                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LEOPageEntry;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }

        public void GetAllLEOEntryBYSBMCIN(string SERCHVALUE)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                // lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "in_SERCHVALUE", MySqlDbType = MySqlDbType.String, Size = 11, Value = SERCHVALUE });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllLEOSerch", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<LEOPage> LEOPageList = new List<LEOPage>();
                //LEOPage LEOPageEntry = new LEOPage();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        LEOPage LEOPageEntry = new LEOPage();
                        LEOPageEntry.Id = Convert.ToInt32(dr["Id"]);
                        // objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);

                        LEOPageEntry.ShipBillNo = Convert.ToString(dr["SB_NO"].ToString());
                        LEOPageEntry.ShipBillDate = Convert.ToString(dr["SB_DATE"]);
                        LEOPageEntry.MCIN = Convert.ToString(dr["MCIN"]);
                        LEOPageList.Add(LEOPageEntry);

                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LEOPageList;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }

        public void GetAllLEOEntryForPage()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                // lstParam.Add(new MySqlParameter { ParameterName = "in_ListOrApp", MySqlDbType = MySqlDbType.VarChar, Value = ListOrEntry });
                lstParam.Add(new MySqlParameter { ParameterName = "in_ID", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllLEOPage", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<LEOPage> LEOPageList = new List<LEOPage>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        LEOPage LEOPageEntry = new LEOPage();
                        LEOPageEntry.Id = Convert.ToInt32(dr["Id"]);
                        // objCCINEntry.InvoiceNo = Convert.ToString(dr["InvoiceNo"]);

                        LEOPageEntry.ShipBillNo = Convert.ToString(dr["SB_NO"].ToString());
                        LEOPageEntry.ShipBillDate = Convert.ToString(dr["SB_DATE"]);
                        LEOPageEntry.MCIN = Convert.ToString(dr["MCIN"]);

                        LEOPageList.Add(LEOPageEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LEOPageList;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }

        #region Stuffing Request
        public void AddEditStuffingRequest(DSR_StuffingRequest ObjStuffing, string StuffingXML, string StuffingContrXML)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.CartingRegisterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingHdrLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ShippingHdrLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ForwarderId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ForwarderId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Vessel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ForeignLiner", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ForeignLiner });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Voyage", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Voyage });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MainLine", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.MainLine });

            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RequestDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjStuffing.RequestDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.StuffingType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingXML", MySqlDbType = MySqlDbType.Text, Value = StuffingXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingContrXML", MySqlDbType = MySqlDbType.Text, Value = StuffingContrXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditStuffingRequest", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = (ObjStuffing.StuffingReqId == 0 ? "Stuffing Request Details Saved Successfully" : "Stuffing Request Details Updated Successfully");
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Stuffing Request Details Already Exist";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Update Stuffing Request Details As It Already Exist In Another Page";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        public void DeleteStuffingRequest(int StuffingReqId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteStuffingRequest", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Stuffing Request Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2 || Result == 3)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Stuffing Request Details As It Exist In Another Page";
                    _DBResponse.Data = null;
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
        public void GetAllStuffingRequest(int RoleId, int Uid, int Page, string ShippbillNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = ShippbillNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequest", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_StuffingRequest> LstStuffing = new List<DSR_StuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new DSR_StuffingRequest
                    {
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        RequestDate = Result["RequestDate"].ToString(),
                        CHA = Result["CHA"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
                        ShippingHdrLine = (Result["ShippingHdrLine"] == null ? "" : Result["ShippingHdrLine"]).ToString(),
                        Forwarder = (Result["Forwarder"] == null ? "" : Result["Forwarder"]).ToString(),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"] == null ? "" : Result["NoOfUnits"]),
                        Fob = Convert.ToDecimal(Result["Fob"] == null ? "" : Result["Fob"]),
                        StuffWeight = Convert.ToDecimal(Result["StuffWeight"] == null ? "" : Result["StuffWeight"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetStuffingRequest(int StuffingReqId, int RoleId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequest", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSR_StuffingRequest ObjStuffing = new DSR_StuffingRequest();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStuffing.StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]);
                    ObjStuffing.StuffingReqNo = Result["StuffingReqNo"].ToString();
                    ObjStuffing.StuffingType = Result["StuffingType"].ToString();
                    ObjStuffing.RequestDate = Result["RequestDate"].ToString();
                    ObjStuffing.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjStuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    ObjStuffing.CHA = Result["CHA"].ToString();
                    ObjStuffing.CartingRegisterNo = Result["CartingRegisterNo"].ToString();
                    ObjStuffing.ShippingHdrLine = (Result["ShippingHdrLine"] == null ? "" : Result["ShippingHdrLine"]).ToString();
                    ObjStuffing.Forwarder = (Result["Forwarder"] == null ? "" : Result["Forwarder"]).ToString();
                    ObjStuffing.NoOfUnits = Convert.ToInt32(Result["NoOfUnits"] == null ? "" : Result["NoOfUnits"]);
                    ObjStuffing.Fob = Convert.ToDecimal(Result["Fob"] == null ? "" : Result["Fob"]);
                    ObjStuffing.StuffWeight = Convert.ToDecimal(Result["StuffWeight"] == null ? "" : Result["StuffWeight"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffing.Add(new DSR_StuffingRequestDtl
                        {
                            StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
                            StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
                            CartingRegisterDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                            Fob = Convert.ToDecimal(Result["Fob"]),
                            ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"].ToString()),
                            ShippingDate = Result["ShippingDate"].ToString(),
                            ExporterId = Convert.ToInt32(Result["ExporterId"]),
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            //  ContainerNo = Result["ContainerNo"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            //  Size = Convert.ToString(Result["Size"]),
                            //  ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            RQty = Convert.ToInt32(Result["RQty"]),
                            RWt = Convert.ToDecimal(Result["RWt"]),
                            //  StuffQuantity = Convert.ToInt32(Result["StuffQuantity"]),
                            //  StuffWeight = Convert.ToDecimal(Result["StuffWeight"]),
                            Exporter = Result["Exporter"].ToString(),
                            CHA = Result["CHA"].ToString(),
                            //  ShippingLine = Result["ShippingLine"].ToString(),
                            // CFSCode = Result["CFSCode"].ToString()
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingContainer.Add(new DSR_StuffingReqContainerDtl
                        {
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            StuffQuantity = Convert.ToInt32(Result["StuffQuantity"]),
                            StuffWeight = Convert.ToDecimal(Result["StuffWeight"]),
                            Size = Convert.ToString(Result["Size"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            StuffingReqContrId = Convert.ToInt32(Result["StuffingReqContrId"]),
                            CommodityName = Result["CommodityName"].ToString()
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetCartRegNoForStuffingReq(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
          //  LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartRegNoForStuffingReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_StuffingRequest> LstStuffing = new List<DSR_StuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new DSR_StuffingRequest
                    {
                        CartingRegisterNo = Result["CartingRegisterNo"].ToString(),
                        CartingRegisterId = Convert.ToInt32(Result["CartingRegisterId"]),
                        ShortCargoDtlId = Convert.ToInt32(Result["ShortCargoDtlId"]),
                        GodownId= Convert.ToInt32(Result["GodownId"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetCartRegDetForStuffingReq(int CartingRegisterId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartRegDetForStuffingReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_StuffingRequestDtl> LstStuffing = new List<DSR_StuffingRequestDtl>();
            List<DSR_StuffingReqContainerDtl> LstStuffingContr = new List<DSR_StuffingReqContainerDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new DSR_StuffingRequestDtl
                    {
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        ShippingDate = Result["ShippingDate"].ToString(),
                        CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"] == DBNull.Value ? 0 : Result["NoOfUnits"]),
                        GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),

                        Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]),
                        ExporterId = Convert.ToInt32(Result["ExporterId"]),
                        Exporter = Result["Exporter"].ToString(),
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHA = Result["CHA"].ToString(),
                        CartingRegisterDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                        CartingRegisterNo = Result["CartingRegisterNo"].ToString(),
                        CartingRegisterId = Convert.ToInt32(Result["CartingRegisterId"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstStuffingContr.Add(new DSR_StuffingReqContainerDtl
                        {
                            CartRegDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                            CommodityName = (Result["CommodityName"] == null ? "" : Result["CommodityName"]).ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { LstStuffing = LstStuffing, ContainerDetails = LstStuffingContr };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetAllContainerNo()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = '0' });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            // List<StuffingRequestDtl> LstStuffing = new List<StuffingRequestDtl>();
            List<DSR_StuffingReqContainerDtl> LstStuffing = new List<DSR_StuffingReqContainerDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new DSR_StuffingReqContainerDtl
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetPackUQCForStuffingReq()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPackUQCForStuffingReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PackUQCForPage> Lstpackuqc = new List<PackUQCForPage>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstpackuqc.Add(new PackUQCForPage
                    {
                        PackUQCCode = Result["PackUQCCode"].ToString(),
                        PackUQCDescription = Result["PackUQCDescription"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lstpackuqc;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetContainerNoDet(string CFSCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNo", CommandType.StoredProcedure, DParam);
            // StuffingRequestDtl ObjStuffing = new StuffingRequestDtl();
            DSR_StuffingReqContainerDtl ObjStuffing = new DSR_StuffingReqContainerDtl();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // ObjStuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    // ObjStuffing.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjStuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjStuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                    ObjStuffing.Size = (Result["Size"] == null ? "" : Result["Size"]).ToString();
                    ObjStuffing.CFSCode = Result["CFSCode"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetShippingBillNoOfCartApp(int CartingRegisterId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            List<DSR_StuffingRequestDtl> LstBillingNo = new List<DSR_StuffingRequestDtl>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingBillNoOfCartApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstBillingNo.Add(new DSR_StuffingRequestDtl
                    {
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBillingNo;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        

        public void ShippinglineDtlAfterEmptyCont(string CFSCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar,Value= CFSCode });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ShippinglineDtlAfterEmptyCont", CommandType.StoredProcedure, DParam);
            DSR_StuffingReqContainerDtl ObjSR = new DSR_StuffingReqContainerDtl();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSR.ShippingLineId = Convert.ToInt32(Result["ToShippingId"]);
                    ObjSR.ShippingLine = Result["ShippingLine"].ToString();
                    ObjSR.Size= Result["Size"].ToString();

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSR;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetShippingLine()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLine", CommandType.StoredProcedure, DParam);
            List<ShippingLine> LstShippingLine = new List<ShippingLine>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstShippingLine.Add(new ShippingLine
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Result["ShippingLine"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstShippingLine;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetForwarder()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetForwarder", CommandType.StoredProcedure, DParam);
            List<ForwarderList> LstForwarder = new List<ForwarderList>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstForwarder.Add(new ForwarderList
                    {
                        ForwarderId = Convert.ToInt32(Result["ShippingLineId"]),
                        Forwarder = Result["ShippingLine"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstForwarder;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        //public void ListOfExporter()
        //{
        //    DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader result = DA.ExecuteDataReader("ListOfExporter", CommandType.StoredProcedure);
        //    IList<Exporter> lstExporter = new List<Exporter>();
        //    int Status = 0;
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        while (result.Read())
        //        {
        //            Status = 1;
        //            lstExporter.Add(new Exporter
        //            {
        //                EXPEximTraderId = Convert.ToInt32(result["EximTraderId"]),
        //                ExporterName = result["EximTraderName"].ToString()
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Data = lstExporter;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Status = 1;
        //        }
        //        else
        //        {
        //            _DBResponse.Data = null;
        //            _DBResponse.Message = "Error";
        //            _DBResponse.Status = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Data = null;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Status = 0;
        //    }
        //    finally
        //    {
        //        result.Dispose();
        //        result.Close();
        //    }
        //}
        //public void ListOfCHA()
        //{
        //    List<MySqlParameter> lstParam = new List<MySqlParameter>();
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
        //    IDataParameter[] dparam = lstParam.ToArray();
        //    DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader result = DA.ExecuteDataReader("ListOfCHA", CommandType.StoredProcedure, dparam);
        //    IList<CHA> lstCHA = new List<CHA>();
        //    int Status = 0;
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        while (result.Read())
        //        {
        //            Status = 1;
        //            lstCHA.Add(new CHA
        //            {
        //                CHAEximTraderId = Convert.ToInt32(result["EximTraderId"]),
        //                CHAName = result["EximTraderName"].ToString()
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Data = lstCHA;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Status = 1;
        //        }
        //        else
        //        {
        //            _DBResponse.Data = null;
        //            _DBResponse.Message = "Error";
        //            _DBResponse.Status = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Data = null;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Status = 0;
        //    }
        //    finally
        //    {
        //        result.Dispose();
        //        result.Close();
        //    }
        //}
        //public void DSR_GetStuffingRequest(int StuffingReqId, int RoleId, int Uid,int Page=0,string ShippbillNo="")
        //{
        //    int Status = 0;
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = ShippbillNo });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequest", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    DSR_StuffingRequest ObjStuffing = new DSR_StuffingRequest();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            ObjStuffing.StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]);
        //            ObjStuffing.StuffingReqNo = Result["StuffingReqNo"].ToString();
        //            ObjStuffing.StuffingType = Result["StuffingType"].ToString();
        //            ObjStuffing.RequestDate = Result["RequestDate"].ToString();
        //            ObjStuffing.CHAId = Convert.ToInt32(Result["CHAId"]);
        //            ObjStuffing.ShippingHdrLineId = Convert.ToInt32(Result["ShippingHdrLineId"]);
        //            ObjStuffing.ForwarderId = Convert.ToInt32(Result["ForwarderId"]);
        //            ObjStuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
        //            ObjStuffing.CHA = Result["CHA"].ToString();
        //            ObjStuffing.Forwarder = Result["Forwarder"].ToString();
        //            ObjStuffing.ShippingHdrLine = Result["ShippingHdrLine"].ToString();
        //            ObjStuffing.Vessel = Result["Vessel"].ToString();
        //            ObjStuffing.ForeignLiner = Result["ForeignLiner"].ToString();
        //            ObjStuffing.Voyage = Result["Voyage"].ToString();
        //            ObjStuffing.MainLine = Result["MainLine"].ToString();
        //            //ObjStuffing.CartingRegisterNo = Result["CartingRegisterNo"].ToString();
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                ObjStuffing.LstStuffing.Add(new DSR_StuffingRequestDtl
        //                {
        //                    StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
        //                    StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
        //                    CartingRegisterDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
        //                    Fob = Convert.ToDecimal(Result["Fob"]),
        //                    CommInvNo = (Result["ComInv"] == null ? "" : Result["ComInv"].ToString()),
        //                    ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"].ToString()),
        //                    ShippingDate = Result["ShippingDate"].ToString(),
        //                    ExporterId = Convert.ToInt32(Result["ExporterId"]),
        //                    CHAId = Convert.ToInt32(Result["CHAId"]),
        //                    CargoDescription = (Result["CargoDescription"] == null ? " " : Result["CargoDescription"]).ToString(),
        //                    //  ContainerNo = Result["ContainerNo"].ToString(),
        //                    CargoType = Convert.ToInt32(Result["CargoType"]),
        //                    //  Size = Convert.ToString(Result["Size"]),
        //                    //  ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
        //                    NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
        //                    GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
        //                    //  StuffQuantity = Convert.ToInt32(Result["StuffQuantity"]),
        //                    //  StuffWeight = Convert.ToDecimal(Result["StuffWeight"]),
        //                    Exporter = Result["Exporter"].ToString(),
        //                    CHA = Result["CHA"].ToString(),
        //                    CartingRegisterNo = Result["CartingRegisterNo"].ToString(),
        //                    CartingRegisterId = Convert.ToInt32(Result["CartingRegisterId"]),
        //                    //  ShippingLine = Result["ShippingLine"].ToString(),
        //                    // CFSCode = Result["CFSCode"].ToString()
        //                    RQty = Convert.ToInt32(Result["RQty"]),
        //                    RWt = Convert.ToDecimal(Result["RWt"]),
        //                    CommodityName = Result["CommodityName"].ToString(),
        //                    ShortCargoId= Convert.ToInt32(Result["ShortCargoId"]),
        //                    GodownId= Convert.ToInt32(Result["GodownId"])
        //                });
        //            }
        //        }

        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                ObjStuffing.LstStuffingContainer.Add(new DSR_StuffingReqContainerDtl
        //                {
        //                    ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
        //                    CFSCode = Result["CFSCode"].ToString(),
        //                    StuffQuantity = Convert.ToInt32(Result["StuffQuantity"]),
        //                    StuffWeight = Convert.ToDecimal(Result["StuffWeight"]),
        //                    Size = Convert.ToString(Result["Size"]),
        //                    ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]),
        //                    ContainerNo = Result["ContainerNo"].ToString(),
        //                    StuffingReqContrId = Convert.ToInt32(Result["StuffingReqContrId"]),
        //                    CommodityName = Result["CommodityName"].ToString(),
        //                    CartRegDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
        //                     GodownId = Convert.ToInt32(Result["GodownId"]),
        //                      ShortCargoId= Convert.ToInt32(Result["ShortCargoDtlId"]),
        //                      Fob = Convert.ToDecimal(Result["Fob"])

        //                });
        //            }
        //        }

        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = ObjStuffing;
        //        }
        //        else
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "No Data";
        //            _DBResponse.Data = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Status = 0;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Data = null;
        //    }
        //    finally
        //    {
        //        Result.Dispose();
        //        Result.Close();
        //    }
        //}

        public void DSR_GetStuffingRequest(int StuffingReqId, int RoleId, int Uid, int Page = 0, string ShippbillNo = "")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = ShippbillNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequest", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSR_StuffingRequest ObjStuffing = new DSR_StuffingRequest();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStuffing.StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]);
                    ObjStuffing.StuffingReqNo = Result["StuffingReqNo"].ToString();
                    ObjStuffing.StuffingType = Result["StuffingType"].ToString();
                    ObjStuffing.RequestDate = Result["RequestDate"].ToString();
                    ObjStuffing.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjStuffing.ShippingHdrLineId = Convert.ToInt32(Result["ShippingHdrLineId"]);
                    ObjStuffing.ForwarderId = Convert.ToInt32(Result["ForwarderId"]);
                    ObjStuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    ObjStuffing.CHA = Result["CHA"].ToString();
                    ObjStuffing.Forwarder = Result["Forwarder"].ToString();
                    ObjStuffing.ShippingHdrLine = Result["ShippingHdrLine"].ToString();
                    ObjStuffing.Vessel = Result["Vessel"].ToString();
                    ObjStuffing.ForeignLiner = Result["ForeignLiner"].ToString();
                    ObjStuffing.Voyage = Result["Voyage"].ToString();
                    ObjStuffing.MainLine = Result["MainLine"].ToString();
                    //ObjStuffing.CartingRegisterNo = Result["CartingRegisterNo"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffing.Add(new DSR_StuffingRequestDtl
                        {
                            StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
                            StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
                            CartingRegisterDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                            Fob = Convert.ToDecimal(Result["Fob"]),
                            CommInvNo = (Result["ComInv"] == null ? "" : Result["ComInv"].ToString()),
                            ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"].ToString()),
                            ShippingDate = Result["ShippingDate"].ToString(),
                            ExporterId = Convert.ToInt32(Result["ExporterId"]),
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CargoDescription = (Result["CargoDescription"] == null ? " " : Result["CargoDescription"]).ToString(),
                            //  ContainerNo = Result["ContainerNo"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            //  Size = Convert.ToString(Result["Size"]),
                            //  ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            //  StuffQuantity = Convert.ToInt32(Result["StuffQuantity"]),
                            //  StuffWeight = Convert.ToDecimal(Result["StuffWeight"]),
                            Exporter = Result["Exporter"].ToString(),
                            CHA = Result["CHA"].ToString(),
                            CartingRegisterNo = Result["CartingRegisterNo"].ToString(),
                            CartingRegisterId = Convert.ToInt32(Result["CartingRegisterId"]),
                            //  ShippingLine = Result["ShippingLine"].ToString(),
                            // CFSCode = Result["CFSCode"].ToString()
                            RQty = Convert.ToInt32(Result["RQty"]),
                            RWt = Convert.ToDecimal(Result["RWt"]),
                            CommodityName = Result["CommodityName"].ToString(),
                            PackUQCDescription = Convert.ToString(Result["PackUQCDesc"]),
                            PackUQCCode = Convert.ToString(Result["PackUQCCode"]),
                            ShortCargoId = Convert.ToInt32(Result["ShortCargoId"]),
                            GodownId = Convert.ToInt32(Result["GodownId"])
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstStuffingContainer.Add(new DSR_StuffingReqContainerDtl
                        {
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            StuffQuantity = Convert.ToInt32(Result["StuffQuantity"]),
                            StuffWeight = Convert.ToDecimal(Result["StuffWeight"]),
                            Size = Convert.ToString(Result["Size"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            StuffingReqContrId = Convert.ToInt32(Result["StuffingReqContrId"]),
                            CommodityName = Result["CommodityName"].ToString(),
                            CartRegDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            ShortCargoId = Convert.ToInt32(Result["ShortCargoDtlId"]),
                            Fob = Convert.ToDecimal(Result["Fob"])

                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void Wfld_GetCartRegDetForStuffingReq(int CartingRegisterId,int ShortCargoId,int GodownId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShortCargoId", MySqlDbType = MySqlDbType.Int32, Value = ShortCargoId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartRegDetForStuffingReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_StuffingRequestDtl> LstStuffing = new List<DSR_StuffingRequestDtl>();
            List<DSR_StuffingReqContainerDtl> LstStuffingContr = new List<DSR_StuffingReqContainerDtl> ();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new DSR_StuffingRequestDtl
                    {
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        CommInvNo = Result["CommInvNo"].ToString(),
                        ShippingDate = Result["ShippingDate"].ToString(),
                        CargoDescription = (Result["CargoDescription"] == null ? " " : Result["CargoDescription"]).ToString(),
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"] == DBNull.Value ? 0 : Result["NoOfUnits"]),
                        GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                        Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]),
                        ExporterId = Convert.ToInt32(Result["ExporterId"]),
                        Exporter = Result["Exporter"].ToString(),
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHA = Result["CHA"].ToString(),
                        CartingRegisterDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                        CartingRegisterNo = Result["CartingRegisterNo"].ToString(),
                        CartingRegisterId = Convert.ToInt32(Result["CartingRegisterId"]),
                        RQty = Convert.ToInt32(Result["RemainingUnits"]),
                        RWt = Convert.ToDecimal(Result["RemainingWeight"]),
                        GodownId= Convert.ToInt32(Result["GodownId"]),
                        ShortCargoId = Convert.ToInt32(Result["ShortCargoId"]),
                        PackUQCDescription = Convert.ToString(Result["PackUQCDesc"]),
                        PackUQCCode = Convert.ToString(Result["PackUQCCode"])

                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        LstStuffingContr.Add(new DSR_StuffingReqContainerDtl
                        {
                            CartRegDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                            CommodityName = (Result["CommodityName"] == null ? "" : Result["CommodityName"]).ToString(),
                            ShippingLine = (Result["ShippingLineName"] == null ? "" : Result["ShippingLineName"]).ToString(),
                             ShortCargoId=Convert.ToInt32(Result["ShortCargoDtlId"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { LstStuffing = LstStuffing, ContainerDetails = LstStuffingContr };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        #endregion

        #region Container Stuffing
        public void ListOfFinalDestination(string CustodianName)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianName", MySqlDbType = MySqlDbType.VarChar, Value = CustodianName });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetFinalDestination", CommandType.StoredProcedure, DParam);


            List<DSR_FinalDestination> LstCustodian = new List<DSR_FinalDestination>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCustodian.Add(new DSR_FinalDestination
                    {
                        CustodianCode = Convert.ToString(Result["CustodianCode"]),
                        CustodianName = Convert.ToString(Result["CustodianName"]),
                        CustodianId = Convert.ToInt32(Result["CustodianId"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCustodian;
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
        public void AddEditContainerStuffing(DSR_ContainerStuffing ObjStuffing, string ContainerStuffingXML)//, string GREOperationCFSCodeWiseAmtXML, string GREContainerWiseAmtXML,
                                                                                                            // string INSOperationCFSCodeWiseAmtLstXML, string INSContainerWiseAmtXML, string STOContainerWiseAmtXML, string STOOperationCFSCodeWiseAmtXML, string HNDOperationCFSCodeWiseAmtXML, string GENSPOperationCFSCodeWiseAmtXML, string ShippingBillAmtXML, string ShippingBillAmtGenXML)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "0";


            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.Datetime, Value = Convert.ToDateTime(ObjStuffing.StuffingDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Remarks });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Origin", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContOrigin }); commented by vineet for 
            LstParam.Add(new MySqlParameter { ParameterName = "in_Via", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContVia });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingSeal", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ShippingSeal });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSeal", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.CustomSeal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POL", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToUInt32(ObjStuffing.PolId)});
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContPOL", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContPOL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.POD });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PODId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.PODId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DirectStuffing", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(ObjStuffing.DirectStuffing) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = ContainerStuffingXML });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.CustodianCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.CustodianId });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            LstParam.Add(new MySqlParameter { ParameterName = "In_TransportMode", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.TransportMode });

            IDataParameter[] DParam = LstParam.ToArray();

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditContainerStuffing", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            DSR_InvoiceCSList inv = new DSR_InvoiceCSList();
            try
            {

                if (Result == 1)
                {
                    //String[] invobj;
                    //invobj = ReturnObj.Split(',');
                    //if (invobj.Length > 0)
                    //    inv.invoicenoGRE = invobj[0]; //InvoiceGRE;

                    //if (invobj.Length > 1)
                    //    inv.invoicenoINS = invobj[1];// InvoiceINS;
                    //if (invobj.Length > 2)
                    //    inv.invoicenoSTO = invobj[2];// InvoiceSTO;
                    //if (invobj.Length > 3)
                    //    inv.invoicenoHND = invobj[3];// InvoiceHND;
                    //if (invobj.Length > 4)
                    //    inv.invoicenoGENSP = invobj[4];
                    //_DBResponse.Data = inv;
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = (ObjStuffing.ContainerStuffingId == 0 ? "Container Stuffing Details Saved Successfully" : "Container Stuffing Details Updated Successfully");
                }

                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Container Stuffing Details Already Exist";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }

        //public void AddEditContainerStuffing(DSR_ContainerStuffing ObjStuffing, string ContainerStuffingXML, string GREOperationCFSCodeWiseAmtXML, string GREContainerWiseAmtXML,
        //string INSOperationCFSCodeWiseAmtLstXML, string INSContainerWiseAmtXML, string STOContainerWiseAmtXML, string STOOperationCFSCodeWiseAmtXML, string HNDOperationCFSCodeWiseAmtXML, string GENSPOperationCFSCodeWiseAmtXML,string ShippingBillAmtXML, string ShippingBillAmtGenXML)
        //{
        //    string GeneratedClientId = "0";
        //    string ReturnObj = "0";


        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ContainerStuffingId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingReqId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjStuffing.StuffingDate).ToString("yyyy-MM-dd") });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Remarks });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Origin", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContOrigin});
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Via", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContVia });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_POL", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContPOL });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.POD });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_DirectStuffing", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(ObjStuffing.DirectStuffing) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = ContainerStuffingXML });

        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.GREPartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREPartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREPartyCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREOperationId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.GREOperationId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREChargeType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREChargeType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREChargeName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRECharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRECharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRECGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRECGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRESGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRESGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GREIGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GREIGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRECGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRECGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRESGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRESGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRETotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GRETotalAmount });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRESACCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GRESACCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREStartdate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GREEndDate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GRECFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GRECFSCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.INSPartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSPartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSPartyCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSOperationId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.INSOperationId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSChargeType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSChargeType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSChargeName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSCGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSCGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSSGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSSGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSIGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSIGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSCGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSCGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSSGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSSGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSTotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.INSTotalAmount });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSSACCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSSACCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSStartdate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSEndDate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSCFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.INSCFSCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.STOPartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOPartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOPartyCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOOperationId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.STOOperationId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOChargeType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOChargeType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOChargeName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOCGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOCGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOSGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOSGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOIGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOIGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOCGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOCGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOSGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOSGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOTotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.STOTotalAmount });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOSACCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOSACCode });

        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.HandalingPartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDPartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HandalingPartyCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDOperationId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.HandalingOperationId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDChargeType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HandalingChargeType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HandalingChargeName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDCGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingCGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDSGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingSGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingIGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingIGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDCGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingCGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDSGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingSGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDTotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.HandalingTotalAmount });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDSACCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HandalingSACCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HNDStartdate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HNDEndDate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDCFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.HNDCFSCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPPartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.GENSPPartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPPartyCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GENSPPartyCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPOperationId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.GENSPOperationId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPChargeType", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GENSPChargeType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPChargeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GENSPChargeName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPCGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPCGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPSGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPSGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPIGSTCharge", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPIGSTCharge });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPIGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPIGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPCGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPCGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPSGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPSGSTPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPTotalAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.GENSPTotalAmount });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPSACCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.GENSPSACCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOStartdate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOStartdate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOEndDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOEndDate});
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOECFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.STOCFSCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GREOperationCFSCodeWiseAmtXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GREContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GREContainerWiseAmtXML });

        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSOperationCFSCodeWiseAmtLstXML", MySqlDbType = MySqlDbType.Text, Value = INSOperationCFSCodeWiseAmtLstXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_INSContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = INSContainerWiseAmtXML });

        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOContainerWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = STOContainerWiseAmtXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_STOOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = STOOperationCFSCodeWiseAmtXML });

        //    LstParam.Add(new MySqlParameter { ParameterName = "In_HNDOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = HNDOperationCFSCodeWiseAmtXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_GENSPOperationCFSCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GENSPOperationCFSCodeWiseAmtXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_ShippingBillAmtXML", MySqlDbType = MySqlDbType.Text, Value = ShippingBillAmtXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_ShippingBillAmtGenXML", MySqlDbType = MySqlDbType.Text, Value = ShippingBillAmtGenXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.CargoTypeId });

        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Cbm", MySqlDbType = MySqlDbType.Decimal, Value = ObjStuffing.CBM });

        //    LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
        //    LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
        //    LstParam.Add(new MySqlParameter { ParameterName = "In_TransportMode", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.TransportMode });

        //    IDataParameter[] DParam = LstParam.ToArray();

        //    DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
        //    int Result = DA.ExecuteNonQuery("AddEditContainerStuffing", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
        //    _DBResponse = new DatabaseResponse();
        //    DSR_InvoiceCSList inv = new DSR_InvoiceCSList();
        //    try
        //    {

        //        if (Result == 1)
        //        {
        //            String[] invobj;
        //            invobj = ReturnObj.Split(',');
        //            if (invobj.Length > 0)
        //                inv.invoicenoGRE = invobj[0]; //InvoiceGRE;

        //            if (invobj.Length > 1)
        //                inv.invoicenoINS = invobj[1];// InvoiceINS;
        //            if (invobj.Length > 2)
        //                inv.invoicenoSTO = invobj[2];// InvoiceSTO;
        //            if (invobj.Length > 3)
        //                inv.invoicenoHND = invobj[3];// InvoiceHND;
        //            if (invobj.Length > 4)
        //                inv.invoicenoGENSP = invobj[4];
        //            _DBResponse.Data = inv;
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = (ObjStuffing.ContainerStuffingId == 0 ? "Container Stuffing Details Saved Successfully" : "Container Stuffing Details Updated Successfully");
        //        }

        //        else if (Result == 2)
        //        {
        //            _DBResponse.Data = null;
        //            _DBResponse.Status = 2;
        //            _DBResponse.Message = "Container Stuffing Details Already Exist";
        //        }
        //        else if (Result == 3)
        //        {
        //            _DBResponse.Data = null;
        //            _DBResponse.Status = 3;
        //            _DBResponse.Message = ReturnObj;
        //        }
        //        else
        //        {
        //            _DBResponse.Data = null;
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "Error";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Data = null;
        //        _DBResponse.Status = -1;
        //        _DBResponse.Message = "Error";
        //    }
        //}



        public void GetContainerDetForStuffing(int StuffingReqId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = StuffingReqId });
           // LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 35, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerDetForStuffing", CommandType.StoredProcedure, DParam);
            DSR_ContainerStuffingDtlBase ObjStuffing = new DSR_ContainerStuffingDtlBase();
             List<DSR_ContainerStuffingDtlBase> LstStuffing = new List<DSR_ContainerStuffingDtlBase>();
          ///  int PortId = 0; string PortName = "";
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                   
                    LstStuffing.Add(new DSR_ContainerStuffingDtlBase
                    {
                        StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                        Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        StuffingType = Convert.ToString(Result["StuffingType"] == null ? "" : Result["StuffingType"]),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                        CustomSeal = (Result["CustomSeal"] == null ? "" : Result["CustomSeal"]).ToString(),
                        ShippingSeal = (Result["ShippingSeal"] == null ? "" : Result["ShippingSeal"]).ToString(),
                        ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"]).ToString(),
                        ShippingDate = (Result["ShippingDate"] == null ? "" : Result["ShippingDate"]).ToString(),
                        CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]),
                        ExporterId = Convert.ToInt32(Result["ExporterId"]),
                        CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                        Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]),
                        StuffQuantity = Convert.ToInt32(Result["StuffQuantity"] == DBNull.Value ? 0 : Result["StuffQuantity"]),
                        StuffWeight = Convert.ToDecimal(Result["StuffWeight"] == DBNull.Value ? 0 : Result["StuffWeight"]),
                        Exporter = Result["Exporter"].ToString(),
                        CHA = Result["CHA"].ToString(),
                        RequestDate = Result["RequestDate"].ToString(),
                        MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString(),
                        Consignee = (Result["ConsigneeName"] == null ? "" : Result["ConsigneeName"]).ToString(),
                        SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]),
                        spacetype = Result["spacetype"].ToString(),
                        CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                        CBM = 0,
                        Palletaisation = 0,
                        WashingOfCargo = 0,
                        Weighment = 0,
                        Reworking = 0,
                        InternalShifting = 0,
                        LiftOnEmpty = 0,
                        LiftOffEmpty = 0,
                        POD = Result["PortOfDischarge"].ToString(),
                        PODId=Convert.ToInt32(Result["PortofDestId"]),
                        POL = Convert.ToInt32(Result["PortOfLoadingId"].ToString()),
                        ContPOL= Result["PortOfLoading"].ToString(),
                        RMSValue = 1,
                        Refer=0,
                        ReeferHrs=0,
                        GodownId= Convert.ToInt32(Result["GodownId"]),
                        EquipmentQUC = Convert.ToString(Result["EquipmentQUC"]),
                        EquipmentSealType = Convert.ToString(Result["EquipmentSealType"]),
                        EquipmentStatus = Convert.ToString(Result["EquipmentStatus"]),
                        MCINPCIN= Convert.ToString(Result["MCINPCIN"]),
                        ShortCargoDtlId = Convert.ToInt32(Result["ShortCargoDtlId"]),
                        SEZ = Convert.ToInt32(Result["SEZ"])


                    });
                }
                if (Status == 1)
                {
                    ObjStuffing.LstStuffing = LstStuffing;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                     _DBResponse.Data = ObjStuffing;                  
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetAllContainerStuffing(int Uid,int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffing", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_ContainerStuffing> LstStuffing = new List<DSR_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new DSR_ContainerStuffing
                    {
                        StuffingNo = Result["StuffingNo"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetAllContainerSearchStuffing(int Uid,string SearchValue,int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchText", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffingByContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_ContainerStuffing> LstStuffing = new List<DSR_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new DSR_ContainerStuffing
                    {
                        StuffingNo = Result["StuffingNo"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetSearchContainerStuffing(string SearchText,int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.VarChar, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchText", MySqlDbType = MySqlDbType.VarChar, Value = SearchText });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSearchContainerStuffing", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_ContainerStuffing> LstStuffing = new List<DSR_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new DSR_ContainerStuffing
                    {
                        StuffingNo = Result["StuffingNo"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetContainerStuffing(int ContainerStuffingId, int Uid,int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffing", CommandType.StoredProcedure, DParam);
            DSR_ContainerStuffing ObjStuffing = new DSR_ContainerStuffing();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStuffing.StuffingNo = Result["StuffingNo"].ToString();
                    ObjStuffing.StuffingDate = (Result["StuffingDate"] == null ? "" : Result["StuffingDate"]).ToString();
                    ObjStuffing.ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]);
                    ObjStuffing.StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]);
                    ObjStuffing.Remarks = Convert.ToString(Result["Remarks"] == null ? "" : Result["Remarks"]);
                    ObjStuffing.StuffingReqNo = Convert.ToString(Result["StuffingReqNo"] == null ? "" : Result["StuffingReqNo"]);
                    ObjStuffing.RequestDate = (Result["RequestDate"] == null ? "" : Result["RequestDate"]).ToString();
                    ObjStuffing.DirectStuffing = Convert.ToBoolean(Result["DirectStuffing"]);
                    ObjStuffing.TransportMode = Convert.ToInt32(Result["TransportMode"]);
                    ObjStuffing.ContOrigin = Convert.ToString(Result["Origin"] == null ? "" : Result["Origin"]);
                    ObjStuffing.ContVia = Convert.ToString(Result["Via"] == null ? "" : Result["Via"]);
                    ObjStuffing.ContPOL = Convert.ToString(Result["POL"] == null ? "" : Result["POL"]);
                    ObjStuffing.PolId = Convert.ToInt32(Result["PolId"] == DBNull.Value ? 0 : Result["PolId"]);
                    ObjStuffing.POD = Convert.ToString(Result["POD"] == null ? "" : Result["POD"]);
                    ObjStuffing.PODId = Convert.ToInt32(Result["PODId"] == DBNull.Value ? null : Result["PODId"]);
                    ObjStuffing.CustodianCode = Convert.ToString(Result["CustodianCode"] == null ? "" : Result["CustodianCode"]);
                    ObjStuffing.CustodianId = Convert.ToInt32(Result["CustodianId"] == DBNull.Value ? 0 : Result["CustodianId"]);

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstppgStuffingDtl.Add(new DSR_ContainerStuffingDtl
                        {
                            ContainerStuffingDtlId = Convert.ToInt32(Result["ContainerStuffingDtlId"]),
                            ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                            StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            StuffingType = (Result["StuffingType"] == null ? "" : Result["StuffingType"]).ToString(),
                            Consignee = (Result["Consignee"] == null ? "" : Result["Consignee"]).ToString(),
                            MarksNo = !string.IsNullOrEmpty(Result["MarksNo"].ToString()) ? Result["MarksNo"].ToString() : "",
                            Insured = Convert.ToInt32(Result["Insured"] == DBNull.Value ? 0 : Result["Insured"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"]).ToString(),
                            ShippingDate = (Result["ShippingDate"] == null ? "" : Result["ShippingDate"]).ToString(),
                            ExporterId = Convert.ToInt32(Result["ExporterId"]),
                            Exporter = (Result["Exporter"] == null ? "" : Result["Exporter"]).ToString(),
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            CustomSeal = Convert.ToString(Result["CustomSeal"] == null ? "" : Result["CustomSeal"]),
                            ShippingSeal = Convert.ToString(Result["ShippingSeal"] == null ? "" : Result["ShippingSeal"]),
                            Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            Size = Convert.ToString(Result["Size"] == null ? "" : Result["Size"]),
                            StuffQuantity = Convert.ToInt32(Result["StuffQuantity"] == DBNull.Value ? 0 : Result["StuffQuantity"]),
                            StuffWeight = Convert.ToDecimal(Result["StuffWeight"] == DBNull.Value ? 0 : Result["StuffWeight"]),
                            CBM= Convert.ToDecimal(Result["CBM"] == DBNull.Value ? 0 : Result["CBM"]),
                            Refer = Convert.ToInt32(Result["Refer"] == DBNull.Value ? 0 : Result["Refer"]),
                            ReeferHrs = Convert.ToInt32(Result["ReeferHrs"] == DBNull.Value ? 0 : Result["ReeferHrs"]),
                            Palletaisation = Convert.ToInt32(Result["Palletaisation"] == DBNull.Value ? 0 : Result["Palletaisation"]),
                            WashingOfCargo = Convert.ToInt32(Result["WashingOfCargo"] == DBNull.Value ? 0 : Result["WashingOfCargo"]),
                            Weighment = Convert.ToInt32(Result["Weighment"] == DBNull.Value ? 0 : Result["Weighment"]),
                            Reworking = Convert.ToInt32(Result["Reworking"] == DBNull.Value ? 0 : Result["Reworking"]),
                            InternalShifting = Convert.ToInt32(Result["InternalShifting"] == DBNull.Value ? 0 : Result["InternalShifting"]),
                            LiftOnEmpty = Convert.ToInt32(Result["LiftOnEmpty"] == DBNull.Value ? 0 : Result["LiftOnEmpty"]),
                            LiftOffEmpty = Convert.ToInt32(Result["LiftOffEmpty"] == DBNull.Value ? 0 : Result["LiftOffEmpty"]),
                            RMSValue = Convert.ToInt32(Result["RMSValue"] == DBNull.Value ? 0 : Result["RMSValue"]),
                            GodownId=Convert.ToInt32(Result["Godownid"]),
                            ContStuffingDate= (Result["ContStuffingDate"] == null ? "" : Result["ContStuffingDate"]).ToString(),
                            EquipmentSealType = Convert.ToString(Result["EquipmentSealType"] == null ? "" : Result["EquipmentSealType"]),
                            EquipmentStatus = Convert.ToString(Result["EquipmentStatus"] == null ? "" : Result["EquipmentStatus"]),
                            EquipmentQUC = Convert.ToString(Result["EquipmentQUC"] == null ? "" : Result["EquipmentQUC"]),
                            MCINPCIN = Convert.ToString(Result["MCINPCIN"] == null ? "" : Result["MCINPCIN"]),
                            ShortCargoDtlId = Convert.ToInt32(Result["ShortCargoDtlId"])

                        });
                    }
                }
                

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetReqNoForContainerStuffing(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReqNoForContainerStuffing", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_ContainerStuffing> LstStuffing = new List<DSR_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new DSR_ContainerStuffing
                    {
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
                        RequestDate= Result["RequestDate"].ToString(),
                        PortofDischargeId= Convert.ToInt32(Result["PortofDischargeId"]),
                        POD=Result["PortOfDischarge"].ToString(),
                        ContPOL=Result["PortOfLoad"].ToString(),
                        PolId= Convert.ToInt32(Result["PortOfLoadingId"].ToString())
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetContainerNoByStuffingReq(int StuffingReqId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = StuffingReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNoByStuffingReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ContainerStuffingDtl> LstStuffing = new List<ContainerStuffingDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new ContainerStuffingDtl
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void DeleteContainerStuffing(int ContainerStuffingId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteContainerStuffing", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Container Stuffing Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Container Stuffing Details As It Exist In Another Page";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete Container Stuffing Details As Next Invoice Generated";
                    _DBResponse.Data = null;
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

        public void GetContainerStuffForPrint(int ContainerStuffingId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffForPrint", CommandType.StoredProcedure, DParam);
            DSR_ContainerStuffing ObjStuffing = new DSR_ContainerStuffing();
            DSR_ContainerStuffingDtl lstcont = new DSR_ContainerStuffingDtl();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjStuffing.StuffingNo = Result["StuffingNo"].ToString();
                    ObjStuffing.StuffingDate = Result["StuffingDate"].ToString();
                    ObjStuffing.ContVia = Result["Via"].ToString();
                    ObjStuffing.GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString();
                    ObjStuffing.CargoType = (Result["CargoType"] == null ? "" : Result["CargoType"]).ToString();
                    ObjStuffing.ForwarderName = (Result["ForwarderName"] == null ? "" : Result["ForwarderName"]).ToString();
                    ObjStuffing.POD = (Result["POD"] == null ? "" : Result["POD"]).ToString();
                    ObjStuffing.ContPOL= Result["POL"].ToString();
                    ObjStuffing.CustodianCode = (Result["CustodianCode"] == null ? "" : Result["CustodianCode"]).ToString();
                }
                if (Result.NextResult())
                {
                    ObjStuffing.Size = "";
                    while (Result.Read())
                    {
                        ObjStuffing.Size += Result["Size"].ToString() + ",";

                    }
                    ObjStuffing.Size = ObjStuffing.Size.Remove(ObjStuffing.Size.Length - 1);
                    ObjStuffing.ShippingLineNo= (Result["ShippingSeal"] == null ? "" : Result["ShippingSeal"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstppgStuffingDtl.Add(new DSR_ContainerStuffingDtl
                        {
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            Consignee = (Result["Consignee"] == null ? "" : Result["Consignee"]).ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"]).ToString(),
                            ShippingDate = (Result["ShippingDate"] == null ? "" : Result["ShippingDate"]).ToString(),
                            EntryNo = (Result["EntryNo"] == null ? "" : Result["EntryNo"]).ToString(),
                            InDate = (Result["InDate"] == null ? "" : Result["InDate"]).ToString(),
                            Exporter = (Result["Exporter"] == null ? "" : Result["Exporter"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            CustomSeal = Convert.ToString(Result["CustomSeal"] == null ? "" : Result["CustomSeal"]),
                            Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]),
                            Area = Convert.ToDecimal(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            Remarks = Convert.ToString(Result["Remarks"] == null ? "" : Result["Remarks"]),
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            StuffQuantity = Convert.ToInt32(Result["StuffQuantity"] == DBNull.Value ? 0 : Result["StuffQuantity"]),
                            StuffWeight = Convert.ToInt32(Result["StuffWeight"] == DBNull.Value ? 0 : Result["StuffWeight"]),
                            PortName = (Result["PortName"] == null ? "" : Result["PortName"]).ToString(),
                            PortDestination = (Result["PortDestination"] == null ? "" : Result["PortDestination"]).ToString(),
                            CommodityName = (Result["CommodityName"] == null ? "" : Result["CommodityName"]).ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            CartingDate = Result["CartingDate"].ToString(),

                            CargoTypeName=Convert.ToString(Result["CargoTypeName"]),
                            CartingType = Convert.ToString(Result["CartingType"]),
                            ShippingSeal = Convert.ToString(Result["ShippingSeal"] == null ? "" : Result["ShippingSeal"]),
                            ContStuffingDate = Convert.ToString(Result["ContStuffingDate"] == null ? "" : Result["ContStuffingDate"]),
                            EquipmentSealType = (Result["EquipmentSealType"] == null ? "" : Result["EquipmentSealType"]).ToString(),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstppgCharge.Add(new DSR_ContainerStuffingCharge
                        {
                          Invoiceno= Convert.ToString(Result["invoiceno"] == DBNull.Value ? "" : Result["invoiceno"]),
                           InvoiceDate= Convert.ToString(Result["invoicedate"] == DBNull.Value ?"" : Result["invoicedate"]),
                            chargetype=  Convert.ToString(Result["chargetype"] == DBNull.Value ? "" : Result["chargetype"]),
                            total=Convert.ToString(Result["total"] == DBNull.Value ? 0 : Result["total"]),
                            eximtraderalias= Convert.ToString(Result["eximtraderalias"] == DBNull.Value ? "" : Result["eximtraderalias"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstppgShipDtl.Add(new DSR_ShippingBillNo
                        {
                            ContainerNo = Convert.ToString(Result["ContainerNo"] == DBNull.Value ? "" : Result["ContainerNo"]),
                            CFSCode = Convert.ToString(Result["CFSCode"] == DBNull.Value ? "" : Result["CFSCode"]),
                             CargoType= Convert.ToString(Result["CargoType"] == DBNull.Value ? "" : Result["CargoType"]),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void ListOfGREParty()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetListOfGREParty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_ContainerStuffing> LstStuffing = new List<DSR_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new DSR_ContainerStuffing
                    {
                        GREPartyId = Convert.ToInt32(Result["EximTraderId"].ToString()),
                        GREPartyCode = Result["EximTraderName"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void CalculateGroundRentEmpty(String StuffingDate, String ContainerStuffingXML, int GREPartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(GREPartyId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = Convert.ToString(ContainerStuffingXML) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(StuffingDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("CalculateGroundRentEmpty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                DSR_ContainerStuffing ObjCS = new DSR_ContainerStuffing();
                while (result.Read())
                {
                    Status = 1;
                    ObjCS.GREOperationId = Convert.ToInt32(result["OperationId"]);
                    ObjCS.GREChargeType = Convert.ToString(result["ChargeType"]);
                    ObjCS.GREChargeName = Convert.ToString(result["ChargeName"]);
                    ObjCS.GRECharge = Convert.ToDecimal(result["Amount"]);
                    ObjCS.GRECGSTCharge = Convert.ToDecimal(result["CGSTAmt"]);
                    ObjCS.GRESGSTCharge = Convert.ToDecimal(result["SGSTAmt"]);
                    ObjCS.GREIGSTCharge = Convert.ToDecimal(result["IGSTAmt"]);
                    ObjCS.GREIGSTPer = Convert.ToDecimal(result["IGSTPer"].ToString());
                    ObjCS.GRECGSTPer = Convert.ToDecimal(result["CGSTPer"].ToString());
                    ObjCS.GRESGSTPer = Convert.ToDecimal(result["SGSTPer"]);
                    ObjCS.GREAmount = Convert.ToDecimal(result["Amount"]);
                    ObjCS.GRETaxable = Convert.ToDecimal(result["Taxable"]);
                    ObjCS.GRETotalAmount = Convert.ToDecimal(result["Total"]);
                    ObjCS.GRESACCode = Convert.ToString(result["SACCode"]);
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.GREOperationCFSCodeWiseAmtLst.Add(new GREOperationCFSCodeWiseAmt
                        {
                            ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                            Size = (result["Size"] == null ? "" : result["Size"]).ToString(),
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            OperationID = Convert.ToInt32(result["OperationID"]),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            ChargeType = (result["ChargeType"] == null ? "" : result["ChargeType"]).ToString(),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                        });
                    }
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.GREContainerWiseAmtLst.Add(new GREContainerWiseAmt
                        {
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            GrEmpty = Convert.ToDecimal(result["GrEmpty"]),
                        });
                    }
                }


                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.GREStartdate = result["StartDate"].ToString();
                        ObjCS.GREEndDate = result["EndDate"].ToString();
                        ObjCS.GRECFSCode = result["CFSCode"].ToString();
                        ObjCS.lstPostPaymentChrgBreakup.Add(new DSRGRLPostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(result["OperationId"]),
                            Clause = result["ChargeType"].ToString(),
                            ChargeType = result["ChargeType"].ToString(),
                            ChargeName = result["ChargeName"].ToString(),
                            SACCode = result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                            OperationId = Convert.ToInt32(result["OperationId"]),
                            CFSCode = result["CFSCode"].ToString(),
                            Startdate = result["StartDate"].ToString(),
                            EndDate = result["EndDate"].ToString(),
                            
                        });
                    }


                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCS;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }


        internal void ListOfINSParty()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetListOfINSParty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_ContainerStuffing> LstStuffing = new List<DSR_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new DSR_ContainerStuffing
                    {
                        INSPartyId = Convert.ToInt32(Result["EximTraderId"].ToString()),
                        INSPartyCode = Result["EximTraderName"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        internal void CalculateINS(string stuffingDate, String ContainerStuffingXML, int iNSPartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(iNSPartyId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = Convert.ToString(ContainerStuffingXML) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(stuffingDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("CalculateInsurence", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                DSR_ContainerStuffing ObjCS = new DSR_ContainerStuffing();
                while (result.Read())
                {
                    Status = 1;
                    ObjCS.INSOperationId = Convert.ToInt32(result["OperationId"]);
                    ObjCS.INSChargeType = Convert.ToString(result["ChargeType"]);
                    ObjCS.INSChargeName = Convert.ToString(result["ChargeName"]);
                    ObjCS.INSCharge = Convert.ToDecimal(result["Amount"]);
                    ObjCS.INSCGSTCharge = Convert.ToDecimal(result["CGSTAmt"]);
                    ObjCS.INSSGSTCharge = Convert.ToDecimal(result["SGSTAmt"]);
                    ObjCS.INSIGSTCharge = Convert.ToDecimal(result["IGSTAmt"]);
                    ObjCS.INSIGSTPer = Convert.ToDecimal(result["IGSTPer"].ToString());
                    ObjCS.INSCGSTPer = Convert.ToDecimal(result["CGSTPer"].ToString());
                    ObjCS.INSSGSTPer = Convert.ToDecimal(result["SGSTPer"]);
                    ObjCS.INSAmount = Convert.ToDecimal(result["Amount"]);
                    ObjCS.INSTaxable = Convert.ToDecimal(result["Taxable"]);
                    ObjCS.INSTotalAmount = Convert.ToDecimal(result["Total"]);
                    ObjCS.INSSACCode = Convert.ToString(result["SACCode"]);
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.INSOperationCFSCodeWiseAmtLst.Add(new INSOperationCFSCodeWiseAmt
                        {
                            ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                            Size = (result["Size"] == null ? "" : result["Size"]).ToString(),
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            OperationID = Convert.ToInt32(result["OperationID"]),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            ChargeType = (result["ChargeType"] == null ? "" : result["ChargeType"]).ToString(),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                        });
                    }
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.INSContainerWiseAmtLst.Add(new INSContainerWiseAmt
                        {
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            InsuranceCharge = Convert.ToDecimal(result["InsuranceCharge"])
                        });
                    }
                }


                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.LstppgShipDtl.Add(new DSR_ShippingBillNo
                        {
                            ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            ShippingBillNo = (result["ShippingBillNo"] == null ? "" : result["ShippingBillNo"]).ToString(),
                            ShippingDate = result["ShippingDate"].ToString(),
                            FOB = Convert.ToDecimal(result["FOB"]),
                            Amount = Convert.ToDecimal(result["Amount"])
                        });
                    }
                }


                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.INSStartdate = result["StartDate"].ToString();
                        ObjCS.INSEndDate = result["EndDate"].ToString();
                        ObjCS.INSCFSCode = result["CFSCode"].ToString();
                        ObjCS.lstPostPaymentChrgBreakup.Add(new DSRGRLPostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(result["OperationId"]),
                            Clause = result["ChargeType"].ToString(),
                            ChargeType = result["ChargeType"].ToString(),
                            ChargeName = result["ChargeName"].ToString(),
                            SACCode = result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                            OperationId = Convert.ToInt32(result["OperationId"]),
                            CFSCode = result["CFSCode"].ToString(),
                            Startdate = result["StartDate"].ToString(),
                            EndDate = result["EndDate"].ToString(),

                        });
                    }


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCS;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        internal void ListOfSTOParty()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetListOfSTOParty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_ContainerStuffing> LstStuffing = new List<DSR_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new DSR_ContainerStuffing
                    {
                        STOPartyId = Convert.ToInt32(Result["EximTraderId"].ToString()),
                        STOPartyCode = Result["EximTraderName"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        internal void CalculateSTO(string stuffingDate, String ContainerStuffingXML, int StoPartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(StoPartyId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = Convert.ToString(ContainerStuffingXML) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(stuffingDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = "EXPCSSTO" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("CalculateStorageCharge", CommandType.StoredProcedure, DParam);

            _DBResponse = new DatabaseResponse();

            try
            {
                DSR_ContainerStuffing ObjCS = new DSR_ContainerStuffing();
                while (result.Read())
                {
                    Status = 1;
                    ObjCS.STOOperationId = Convert.ToInt32(result["OperationId"]);
                    ObjCS.STOChargeType = Convert.ToString(result["ChargeType"]);
                    ObjCS.STOChargeName = Convert.ToString(result["ChargeName"]);
                    ObjCS.STOCharge = Convert.ToDecimal(result["Amount"]);
                    ObjCS.STOCGSTCharge = Convert.ToDecimal(result["CGSTAmt"]);
                    ObjCS.STOSGSTCharge = Convert.ToDecimal(result["SGSTAmt"]);
                    ObjCS.STOIGSTCharge = Convert.ToDecimal(result["IGSTAmt"]);
                    ObjCS.STOIGSTPer = Convert.ToDecimal(result["IGSTPer"].ToString());
                    ObjCS.STOCGSTPer = Convert.ToDecimal(result["CGSTPer"].ToString());
                    ObjCS.STOSGSTPer = Convert.ToDecimal(result["SGSTPer"]);
                    ObjCS.STOAmount = Convert.ToDecimal(result["Amount"]);
                    ObjCS.STOTaxable = Convert.ToDecimal(result["Taxable"]);
                    ObjCS.STOTotalAmount = Convert.ToDecimal(result["Total"]);
                    ObjCS.STOSACCode = Convert.ToString(result["SACCode"]);
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.STOinvoicecargodtlLst.Add(new STOinvoicecargodtl
                        {
                            BOENo = (result["BOENo"] == null ? "" : result["BOENo"]).ToString(),
                            BOEDate = Convert.ToDateTime(result["BOEDate"]),
                            BOLNo = Convert.ToString((result["BOENo"])),
                            BOLDate = Convert.ToDateTime(result["BOLDate"]),
                            CargoDescription = Convert.ToString(result["CargoDescription"]),
                            GodownId = Convert.ToInt32(result["GodownId"]),
                            GodownName = Convert.ToString(result["GodownName"] == null ? "" : result["GodownName"]),
                            GdnWiseLctnIds = Convert.ToString(result["GdnWiseLctnIds"]),
                            GdnWiseLctnNames = Convert.ToString(result["GdnWiseLctnNames"]),
                            CargoType = Convert.ToInt32(result["CargoType"] == null ? "" : result["CargoType"]),
                            DestuffingDate = Convert.ToDateTime(result["DestuffingDate"] == null ? "" : result["DestuffingDate"]),
                            CartingDate = Convert.ToDateTime(result["CartingDate"] == null ? "" : result["CartingDate"]),
                            StuffingDate = Convert.ToDateTime(result["StuffingDate"]),
                            NoOfPackages = Convert.ToInt32(result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(result["GrossWt"] == null ? "" : result["GrossWt"]),
                            WtPerUnit = Convert.ToDecimal(result["WtPerUnit"]),
                            SpaceOccupied = Convert.ToDecimal(result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Convert.ToDecimal(result["SpaceOccupiedUnit"] == null ? "" : result["SpaceOccupiedUnit"]),
                            CIFValue = Convert.ToDecimal(result["CIFValue"]),
                            Duty = Convert.ToDecimal(result["Duty"])
                        });
                    }
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.STOOperationCFSCodeWiseAmtLst.Add(new STOOperationCFSCodeWiseAmt
                        {
                            ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                            Size = (result["Size"] == null ? "" : result["Size"]).ToString(),
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            OperationID = Convert.ToInt32(result["OperationID"]),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            ChargeType = (result["ChargeType"] == null ? "" : result["ChargeType"]).ToString(),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"])
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCS;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }


        internal void ListOfHandalingParty()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetListOfHandalingParty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_ContainerStuffing> LstStuffing = new List<DSR_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new DSR_ContainerStuffing
                    {
                        HandalingPartyId = Convert.ToInt32(Result["EximTraderId"].ToString()),
                        HandalingPartyCode = Result["EximTraderName"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        internal void ListOfGENSPParty()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetListOfHandalingParty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_ContainerStuffing> LstStuffing = new List<DSR_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new DSR_ContainerStuffing
                    {
                        GENSPPartyId = Convert.ToInt32(Result["EximTraderId"].ToString()),
                        GENSPPartyCode = Result["EximTraderName"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        internal void CalculateHandaling(string stuffingDate,String Origin,String Via, string ContainerStuffingXML, int HandalingPartyId,String CargoType)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HandalingPartyId) });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TMode", MySqlDbType = MySqlDbType.VarChar, Value = TMode });
          LstParam.Add(new MySqlParameter { ParameterName = "in_Origin", MySqlDbType = MySqlDbType.VarChar, Value = Origin });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Via", MySqlDbType = MySqlDbType.VarChar, Value = Via });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = Convert.ToString(ContainerStuffingXML) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(stuffingDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.VarChar, Value = CargoType });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DataAccess.ExecuteDataReader("CalculateHandallingCharge", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                DSR_ContainerStuffing ObjCS = new DSR_ContainerStuffing();
                while (result.Read())
                {
                    Status = 1;
                    ObjCS.HandalingOperationId = Convert.ToInt32(result["OperationId"]);
                    ObjCS.HandalingChargeType = Convert.ToString(result["ChargeType"]);
                    ObjCS.HandalingChargeName = Convert.ToString(result["ChargeName"]);
                    ObjCS.HandalingCharge = Convert.ToDecimal(result["Amount"]);
                    ObjCS.HandalingCGSTCharge = Convert.ToDecimal(result["CGSTAmt"]);
                    ObjCS.HandalingSGSTCharge = Convert.ToDecimal(result["SGSTAmt"]);
                    ObjCS.HandalingIGSTCharge = Convert.ToDecimal(result["IGSTAmt"]);
                    ObjCS.HandalingIGSTPer = Convert.ToDecimal(result["IGSTPer"].ToString());
                    ObjCS.HandalingCGSTPer = Convert.ToDecimal(result["CGSTPer"].ToString());
                    ObjCS.HandalingSGSTPer = Convert.ToDecimal(result["SGSTPer"]);
                    ObjCS.HandalingAmount = Convert.ToDecimal(result["Amount"]);
                    ObjCS.HandalingTaxable = Convert.ToDecimal(result["Taxable"]);
                    ObjCS.HandalingTotalAmount = Convert.ToDecimal(result["Total"]);
                    ObjCS.HandalingSACCode = Convert.ToString(result["SACCode"]);
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.HNDOperationCFSCodeWiseAmtLst.Add(new HNDOperationCFSCodeWiseAmt
                        {
                            ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                            Size = (result["Size"] == null ? "" : result["Size"]).ToString(),
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            OperationID = Convert.ToInt32(result["OperationID"]),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            ChargeType = (result["ChargeType"] == null ? "" : result["ChargeType"]).ToString(),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                        });
                    }
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.HNDStartdate = result["StartDate"].ToString();
                        ObjCS.HNDEndDate = result["EndDate"].ToString();
                        ObjCS.HNDCFSCode = result["CFSCode"].ToString();
                        ObjCS.lstPostPaymentChrgBreakup.Add(new DSRGRLPostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(result["OperationId"]),
                            Clause = result["ChargeType"].ToString(),
                            ChargeType = result["ChargeType"].ToString(),
                            ChargeName = result["ChargeName"].ToString(),
                            SACCode = result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                            OperationId = Convert.ToInt32(result["OperationId"]),
                            CFSCode = result["CFSCode"].ToString(),
                            Startdate = result["StartDate"].ToString(),
                            EndDate = result["EndDate"].ToString(),

                        });
                    }


                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCS;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        internal void CalculateGENSP(string stuffingDate, string ContainerStuffingXML, int GENSPPartyId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(GENSPPartyId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.LongText, Value = Convert.ToString(ContainerStuffingXML) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(stuffingDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = "EXPCSGEN" });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
              IDataReader result = DataAccess.ExecuteDataReader("CalculateGENSPCharge", CommandType.StoredProcedure, DParam);
           // DataSet Result = new DataSet();
            //Result = DataAccess.ExecuteDataSet("CalculateGENSPCharge", CommandType.StoredProcedure, DParam);

            _DBResponse = new DatabaseResponse();

            try
            {

                DSR_ContainerStuffing ObjCS = new DSR_ContainerStuffing();
                while (result.Read())
                {
                    Status = 1;
                    ObjCS.GENSPOperationId = Convert.ToInt32(result["OperationId"]);
                    ObjCS.GENSPChargeType = Convert.ToString(result["ChargeType"]);
                    ObjCS.GENSPChargeName = Convert.ToString(result["ChargeName"]);
                    ObjCS.GENSPCharge = Convert.ToDecimal(result["Amount"]);
                    ObjCS.GENSPCGSTCharge = Convert.ToDecimal(result["CGSTAmt"]);
                    ObjCS.GENSPSGSTCharge = Convert.ToDecimal(result["SGSTAmt"]);
                    ObjCS.GENSPIGSTCharge = Convert.ToDecimal(result["IGSTAmt"]);
                     ObjCS.GENSPIGSTPer = Convert.ToDecimal(result["IGSTPer"].ToString());
                    ObjCS.GENSPCGSTPer = Convert.ToDecimal(result["CGSTPer"].ToString());
                    ObjCS.GENSPSGSTPer = Convert.ToDecimal(result["SGSTPer"]);
                    ObjCS.GENSPAmount = Convert.ToDecimal(result["Amount"]);
                    ObjCS.GENSPTaxable = Convert.ToDecimal(result["Taxable"]);
                    ObjCS.GENSPTotalAmount = Convert.ToDecimal(result["Total"]);
                    ObjCS.GENSPSACCode = Convert.ToString(result["SACCode"]);
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.STOinvoicecargodtlLst.Add(new STOinvoicecargodtl
                        {
                            BOENo = (result["BOENo"] == null ? "" : result["BOENo"]).ToString(),
                            BOEDate = Convert.ToDateTime(result["BOEDate"]),
                            BOLNo = Convert.ToString((result["BOENo"])),
                            BOLDate = Convert.ToDateTime(result["BOLDate"]),
                            CargoDescription = Convert.ToString(result["CargoDescription"]),
                            GodownId = Convert.ToInt32(result["GodownId"]),
                            GodownName = Convert.ToString(result["GodownName"] == null ? "" : result["GodownName"]),
                            GdnWiseLctnIds = Convert.ToString(result["GdnWiseLctnIds"]),
                            GdnWiseLctnNames = Convert.ToString(result["GdnWiseLctnNames"]),
                            CargoType = Convert.ToInt32(result["CargoType"] == null ? "" : result["CargoType"]),
                            DestuffingDate = Convert.ToDateTime(result["DestuffingDate"] == null ? "" : result["DestuffingDate"]),
                            CartingDate = Convert.ToDateTime(result["CartingDate"] == null ? "" : result["CartingDate"]),
                            StuffingDate = Convert.ToDateTime(result["StuffingDate"]),
                            NoOfPackages = Convert.ToInt32(result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(result["GrossWt"] == null ? "" : result["GrossWt"]),
                            WtPerUnit = Convert.ToDecimal(result["WtPerUnit"]),
                            SpaceOccupied = Convert.ToDecimal(result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Convert.ToDecimal(result["SpaceOccupiedUnit"] == null ? "" : result["SpaceOccupiedUnit"]),
                            CIFValue = Convert.ToDecimal(result["CIFValue"]),
                            Duty = Convert.ToDecimal(result["Duty"])
                        });
                    }
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.GENSPOperationCFSCodeWiseAmtLst.Add(new DSRGENSPOperationCFSCodeWiseAmt
                        {
                            ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                            Size = (result["Size"] == null ? "" : result["Size"]).ToString(),
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            OperationID = Convert.ToInt32(result["OperationID"]),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            ChargeType = (result["ChargeType"] == null ? "" : result["ChargeType"]).ToString(),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                        });
                    }
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.LstppgShipDtl.Add(new DSR_ShippingBillNo
                        {
                            ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                            CFSCode = (result["CFSCode"] == null ? "" : result["CFSCode"]).ToString(),
                            ShippingBillNo = (result["ShippingBillNo"] == null ? "" : result["ShippingBillNo"]).ToString(),
                            ShippingDate = result["ShippingDate"].ToString(),
                            FOB = Convert.ToDecimal(result["AREA"]),
                            Amount = Convert.ToDecimal(result["Amount"])
                        });
                    }
                }

                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        ObjCS.STOStartdate = result["StartDate"].ToString();
                        ObjCS.STOEndDate = result["EndDate"].ToString();
                        ObjCS.STOCFSCode = result["CFSCode"].ToString();
                        ObjCS.lstPostPaymentChrgBreakup.Add(new DSRGRLPostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(result["OperationId"]),
                            Clause = result["ChargeType"].ToString(),
                            ChargeType = result["ChargeType"].ToString(),
                            ChargeName = result["ChargeName"].ToString(),
                            SACCode = result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(result["Quantity"]),
                            Rate = Convert.ToDecimal(result["Rate"]),
                            Amount = Convert.ToDecimal(result["Amount"]),
                            OperationId = Convert.ToInt32(result["OperationId"]),
                            CFSCode = result["CFSCode"].ToString(),
                            Startdate = result["StartDate"].ToString(),
                            EndDate = result["EndDate"].ToString(),

                        });
                    }

                     
                }
                
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCS;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void ListOfAllPort()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PODContStuff", CommandType.StoredProcedure);
            List<Port> LstPort = new List<Port>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new Port
                    {
                        PortName = Convert.ToString(Result["PortName"]),
                        PortId = Convert.ToInt32(Result["PortId"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPort;
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

        public void ListOfTrader(string PartyCode, string TFlag, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TFlag", MySqlDbType = MySqlDbType.VarChar, Value = TFlag });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetExpTraderList", CommandType.StoredProcedure, Dparam);
            IList<CwcExim.Areas.Import.Models.Trader> lstTrader = new List<CwcExim.Areas.Import.Models.Trader>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstTrader.Add(new CwcExim.Areas.Import.Models.Trader
                    {
                        TraderId = Convert.ToInt32(Result["TraderId"]),
                        TraderName = Convert.ToString(Result["TraderName"]),
                        TraderCode = Result["TraderCode"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstTrader, State };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }

        #endregion

        #region Container Stuffing Amendment

        public void ListOfStuffingNoForAmendment()
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingNo", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfStuffingNoForAmendment", CommandType.StoredProcedure, DParam);
            List<DSR_ContainerStuffing> Lstsr = new List<DSR_ContainerStuffing>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstsr.Add(new DSR_ContainerStuffing
                    {
                        StuffingNo = Convert.ToString(Result["StuffingNo"]),
                        ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                        StuffingDate = Convert.ToString(Result["StuffingDate"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lstsr;
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


        public void GetContainerStuffingDetails(int ContainerStuffingId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerStuffing", CommandType.StoredProcedure, DParam);
            DSR_ContainerStuffing ObjStuffing = new DSR_ContainerStuffing();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
            
                    ObjStuffing.StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]);
                    ObjStuffing.Remarks = Convert.ToString(Result["Remarks"] == null ? "" : Result["Remarks"]);
                    ObjStuffing.StuffingReqNo = Convert.ToString(Result["StuffingReqNo"] == null ? "" : Result["StuffingReqNo"]);
                    ObjStuffing.RequestDate = (Result["RequestDate"] == null ? "" : Result["RequestDate"]).ToString();
                    ObjStuffing.DirectStuffing = Convert.ToBoolean(Result["DirectStuffing"]);
                    ObjStuffing.TransportMode = Convert.ToInt32(Result["TransportMode"]);
                    ObjStuffing.ContOrigin = Convert.ToString(Result["Origin"] == null ? "" : Result["Origin"]);
                    ObjStuffing.ContVia = Convert.ToString(Result["Via"] == null ? "" : Result["Via"]);
                    ObjStuffing.ContPOL = Convert.ToString(Result["POL"] == null ? "" : Result["POL"]);
                    ObjStuffing.PolId = Convert.ToInt32(Result["PolId"] == DBNull.Value ? 0 : Result["PolId"]);
                    ObjStuffing.POD = Convert.ToString(Result["POD"] == null ? "" : Result["POD"]);
                    ObjStuffing.PODId = Convert.ToInt32(Result["PODId"] == DBNull.Value ? null : Result["PODId"]);
                    ObjStuffing.CustodianCode = Convert.ToString(Result["CustodianCode"] == null ? "" : Result["CustodianCode"]);
                    ObjStuffing.CustodianId = Convert.ToInt32(Result["CustodianId"] == DBNull.Value ? 0 : Result["CustodianId"]);

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjStuffing.LstppgStuffingDtl.Add(new DSR_ContainerStuffingDtl
                        {
                            ContainerStuffingDtlId = Convert.ToInt32(Result["ContainerStuffingDtlId"]),
                            ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                            StuffingReqDtlId = Convert.ToInt32(Result["StuffingReqDtlId"]),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            StuffingType = (Result["StuffingType"] == null ? "" : Result["StuffingType"]).ToString(),
                            Consignee = (Result["Consignee"] == null ? "" : Result["Consignee"]).ToString(),
                            MarksNo = !string.IsNullOrEmpty(Result["MarksNo"].ToString()) ? Result["MarksNo"].ToString() : "",
                            Insured = Convert.ToInt32(Result["Insured"] == DBNull.Value ? 0 : Result["Insured"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"]).ToString(),
                            ShippingDate = (Result["ShippingDate"] == null ? "" : Result["ShippingDate"]).ToString(),
                            ExporterId = Convert.ToInt32(Result["ExporterId"]),
                            Exporter = (Result["Exporter"] == null ? "" : Result["Exporter"]).ToString(),
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            CustomSeal = Convert.ToString(Result["CustomSeal"] == null ? "" : Result["CustomSeal"]),
                            ShippingSeal = Convert.ToString(Result["ShippingSeal"] == null ? "" : Result["ShippingSeal"]),
                            Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            Size = Convert.ToString(Result["Size"] == null ? "" : Result["Size"]),
                            StuffQuantity = Convert.ToInt32(Result["StuffQuantity"] == DBNull.Value ? 0 : Result["StuffQuantity"]),
                            StuffWeight = Convert.ToDecimal(Result["StuffWeight"] == DBNull.Value ? 0 : Result["StuffWeight"]),
                            CBM = Convert.ToDecimal(Result["CBM"] == DBNull.Value ? 0 : Result["CBM"]),
                            Refer = Convert.ToInt32(Result["Refer"] == DBNull.Value ? 0 : Result["Refer"]),
                            ReeferHrs = Convert.ToInt32(Result["ReeferHrs"] == DBNull.Value ? 0 : Result["ReeferHrs"]),
                            Palletaisation = Convert.ToInt32(Result["Palletaisation"] == DBNull.Value ? 0 : Result["Palletaisation"]),
                            WashingOfCargo = Convert.ToInt32(Result["WashingOfCargo"] == DBNull.Value ? 0 : Result["WashingOfCargo"]),
                            Weighment = Convert.ToInt32(Result["Weighment"] == DBNull.Value ? 0 : Result["Weighment"]),
                            Reworking = Convert.ToInt32(Result["Reworking"] == DBNull.Value ? 0 : Result["Reworking"]),
                            InternalShifting = Convert.ToInt32(Result["InternalShifting"] == DBNull.Value ? 0 : Result["InternalShifting"]),
                            LiftOnEmpty = Convert.ToInt32(Result["LiftOnEmpty"] == DBNull.Value ? 0 : Result["LiftOnEmpty"]),
                            LiftOffEmpty = Convert.ToInt32(Result["LiftOffEmpty"] == DBNull.Value ? 0 : Result["LiftOffEmpty"]),
                            RMSValue = Convert.ToInt32(Result["RMSValue"] == DBNull.Value ? 0 : Result["RMSValue"]),
                            GodownId = Convert.ToInt32(Result["Godownid"]),
                            ContStuffingDate = (Result["ContStuffingDate"] == null ? "" : Result["ContStuffingDate"]).ToString(),
                            EquipmentSealType = Convert.ToString(Result["EquipmentSealType"] == null ? "" : Result["EquipmentSealType"]),
                            EquipmentStatus = Convert.ToString(Result["EquipmentStatus"] == null ? "" : Result["EquipmentStatus"]),
                            EquipmentQUC = Convert.ToString(Result["EquipmentQUC"] == null ? "" : Result["EquipmentQUC"])

                        });
                    }
                }                

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void AddEditAmendmentContainerStuffing(DSR_ContainerStuffing ObjStuffing, string ContainerStuffingXML)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "0";


            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.Datetime, Value = Convert.ToDateTime(ObjStuffing.StuffingDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.Remarks });          
            LstParam.Add(new MySqlParameter { ParameterName = "in_Via", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContVia });          
            LstParam.Add(new MySqlParameter { ParameterName = "in_POL", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToUInt32(ObjStuffing.PolId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContPOL", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.ContPOL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.POD });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PODId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.PODId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DirectStuffing", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = Convert.ToInt32(ObjStuffing.DirectStuffing) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingXML", MySqlDbType = MySqlDbType.Text, Value = ContainerStuffingXML });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjStuffing.CustodianCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustodianId", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.CustodianId });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            LstParam.Add(new MySqlParameter { ParameterName = "In_TransportMode", MySqlDbType = MySqlDbType.Int32, Value = ObjStuffing.TransportMode });

            IDataParameter[] DParam = LstParam.ToArray();

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditAmendmentContainerStuffing", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {

                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = (ObjStuffing.ContainerStuffingId == 0 ? "Amendment Container Stuffing Details Saved Successfully" : " Amendment Container Stuffing Details Updated Successfully");
                }

                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Container Stuffing Approval Already Done";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        public void GetAllAmendmentContainerStuffing(int Page, int Uid, string SearchValue = "")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.VarChar, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.VarChar, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.VarChar, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllAmendmentContainerStuffing", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_ContainerStuffing> LstStuffing = new List<DSR_ContainerStuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffing.Add(new DSR_ContainerStuffing
                    {
                        StuffingNo = Result["StuffingNo"].ToString(),
                        StuffingDate = Result["StuffingDate"].ToString(),
                        ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                        ShipBillNo = Convert.ToString(Result["ShippingBillNo"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        AmendmentDate = Convert.ToString(Result["AmendmentDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        #endregion

        #region Get CIM-SF Details

        public void GetCIMSFDetails(int ContainerStuffingId, string MsgType)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("ICES_GetSCMTRStuffingDetailsV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();




            try
            {

                int count = Result.Tables.Count;
                if (count == 1)
                {
                    if (Convert.ToInt32(Result.Tables[0].Rows[0]["Result"]) == 1)
                    {
                        _DBResponse.Status = 2;
                        _DBResponse.Message = "CIM SF Message Already Send.";
                        _DBResponse.Data = Result;
                    }
                    else
                    {
                        _DBResponse.Status = 3;
                        _DBResponse.Message = "CIM SF Acknowledgement Received Successfully Please Do Amendment";
                        _DBResponse.Data = Result;
                    }

                }
                else
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Result;
                }



            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
                //Result.Close();
            }
        }

        public void GetCIMSFDetailsUpdateStatus(int ContainerStuffingId)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("UpdateCIMSFStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Result;


            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
                //Result.Close();
            }
        }
        #endregion


        #region Get CIM-ASR Details

        public void GetCIMASRDetails(int ContainerStuffingId, string MsgType)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("ICES_GetSCMTRCIMASRDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Result;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
                //Result.Close();
            }
        }

        public void GetCIMASRDetailsUpdateStatus(int ContainerStuffingId)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("UpdateCIMASRStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Result;


            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
                //Result.Close();
            }
        }
        #endregion

        #region Container Stuffing Approval
        public void GetContStuffingForApproval(int StuffingReqId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequestForApproval", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<DSR_ContainerStuffingApproval> objPaySheetStuffing = new List<DSR_ContainerStuffingApproval>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new DSR_ContainerStuffingApproval()
                    {

                        StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
                        StuffingReqNo = Convert.ToString(Result["StuffingReqNo"]),
                        StuffingReqDate = Convert.ToString(Result["RequestDate"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        Size = Convert.ToString(Result["Size"]),
                        ExporterId = Convert.ToInt32(Result["ExporterId"]),
                        ExporterName = Convert.ToString(Result["ExporterName"]),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetPortOfCall()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortOfCall", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<PortOfCall> objPortOfCallList = new List<PortOfCall>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPortOfCallList.Add(new PortOfCall()
                    {

                        PortOfCallId = Convert.ToInt32(Result["PortOfCallId"]),
                        PortOfCallName = Convert.ToString(Result["PortOfCallName"]),
                        PortOfCallCode = Convert.ToString(Result["PortOfCallCode"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPortOfCallList;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetPortOfCallForPage(string PortCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PortCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortOfCallForPage", CommandType.StoredProcedure, Dparam);
            IList<PortOfCall> lstPortOfCall = new List<PortOfCall>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool StatePortOfCall = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstPortOfCall.Add(new PortOfCall
                    {
                        PortOfCallId = Convert.ToInt32(Result["PortOfCallId"]),
                        PortOfCallName = Convert.ToString(Result["PortOfCallName"]),
                        PortOfCallCode = Convert.ToString(Result["PortOfCallCode"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        StatePortOfCall = Convert.ToBoolean(Result["StateParty"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstPortOfCall, StatePortOfCall };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }

        public void GetNextPortOfCallForPage(string PortCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PortCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetNextPortOfCallForPage", CommandType.StoredProcedure, Dparam);
            IList<PortOfCall> lstNextPortOfCall = new List<PortOfCall>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool StateNextPortOFCall = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstNextPortOfCall.Add(new PortOfCall
                    {
                        PortOfCallId = Convert.ToInt32(Result["PortOfCallId"]),
                        PortOfCallName = Convert.ToString(Result["PortOfCallName"]),
                        PortOfCallCode = Convert.ToString(Result["PortOfCallCode"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        StateNextPortOFCall = Convert.ToBoolean(Result["StatePayer"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstNextPortOfCall, StateNextPortOFCall };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }


        public void AddEditContainerStuffingApproval(PortOfCall objPortOfCall, int Uid)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_ApprovalId", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.ApprovalId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ApprovalDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objPortOfCall.ApprovalDate).ToString("yyyy-MM-dd") });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.StuffingReqId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.StuffingReqNo });
            lstparam.Add(new MySqlParameter { ParameterName = "in_PortOfCallName", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.PortOfCallName });
            lstparam.Add(new MySqlParameter { ParameterName = "in_PortOfCallCode", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.PortOfCallCode });
            lstparam.Add(new MySqlParameter { ParameterName = "in_NextPortOfCallName", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.NextPortOfCallName });
            lstparam.Add(new MySqlParameter { ParameterName = "in_NextPortOfCallCode", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.NextPortOfCallCode });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ModeOfTransport", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.ModeOfTransport });
            lstparam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("AddEditContainerStuffingApproval", CommandType.StoredProcedure, dpram);
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Stuffing Approved Successfully" : "Stuffing Approval Updated Successfully";

                }
               
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Can't Update CIM SF File Already Send.";
                }
                else if (result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Can't Update CIM SF Acknowledgement Received.";
                }
                else if (result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Can't Update Stuffing Amendment Done.";
                }
                else if (result == 6)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Can't Update Stuffing Approval Done.";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
            }
        }

        public void ListofContainerStuffingApproval(int Page)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofContainerStuffingApproval", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PortOfCall> LstStuffingApproval = new List<PortOfCall>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffingApproval.Add(new PortOfCall
                    {
                        ApprovalId = Convert.ToInt32(Result["ApprovalId"]),
                        ApprovalDate = Result["ApprovalDate"].ToString(),
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        StuffingReqDate = Result["RequestDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffingApproval;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetContainerStuffingApprovalById(int ApprovalId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ApprovalId", MySqlDbType = MySqlDbType.Int32, Value = ApprovalId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("ViewContainerStuffingApproval", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            PortOfCall objDestuffing = new PortOfCall();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDestuffing.ApprovalId = Convert.ToInt32(Result["ApprovalId"]);
                    objDestuffing.ApprovalDate = Convert.ToString(Result["ApprovalDate"]);
                    objDestuffing.StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]);
                    objDestuffing.StuffingReqNo = Convert.ToString(Result["StuffingReqNo"]);
                    objDestuffing.StuffingReqDate = Convert.ToString(Result["RequestDate"]);
                    objDestuffing.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objDestuffing.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    objDestuffing.Size = Convert.ToString(Result["Size"].ToString());
                    objDestuffing.PortOfCallName = Convert.ToString(Result["PortOfCallName"]);
                    objDestuffing.PortOfCallCode = Convert.ToString(Result["PortOfCallCode"]);
                    objDestuffing.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                    objDestuffing.ExporterName = Convert.ToString(Result["ExporterName"]);
                    objDestuffing.NextPortOfCallName = Convert.ToString(Result["NextPortOfCallName"]);
                    objDestuffing.NextPortOfCallCode = Convert.ToString(Result["NextPortOfCallCode"]);
                    objDestuffing.ModeOfTransportName = Convert.ToString(Result["ModeOfTransport"]);
                    objDestuffing.ModeOfTransport = Convert.ToInt32(Result["ModeOfTransprt"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Data = objDestuffing;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetAllContainerStuffingApprovalSearch(string SearchValue = "")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofContainerStuffingApprovalSearch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PortOfCall> LstStuffingApproval = new List<PortOfCall>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffingApproval.Add(new PortOfCall
                    {
                        ApprovalId = Convert.ToInt32(Result["ApprovalId"]),
                        ApprovalDate = Result["ApprovalDate"].ToString(),
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        StuffingReqDate = Result["RequestDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffingApproval;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        public void GetAmendmentContainerStuffing()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmendmentContainerStuffing", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        #endregion       

        #region Carting Application
        public void ListOfCHA()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfCHA", CommandType.StoredProcedure, dparam);
            IList<CHA> lstCHA = new List<CHA>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CHA
                    {
                        CHAEximTraderId = Convert.ToInt32(result["EximTraderId"]),
                        CHAName = result["EximTraderName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstCHA;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void ListOfExporter()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfExporter", CommandType.StoredProcedure);
            IList<Exporter> lstExporter = new List<Exporter>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstExporter.Add(new Exporter
                    {
                        EXPEximTraderId = Convert.ToInt32(result["EximTraderId"]),
                        ExporterName = result["EximTraderName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstExporter;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void GetAllCommodity()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCommodity", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Export.Models.Commodity> LstCommodity = new List<Areas.Export.Models.Commodity>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCommodity.Add(new Areas.Export.Models.Commodity
                    {
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        CommodityName = Result["CommodityName"].ToString()
                        // CommodityAlias = (Result["CommodityAlias"] == null ? "" : Result["CommodityAlias"]).ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCommodity;
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
        public void ListOfPOD()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfPOD", CommandType.StoredProcedure);
            List<Port> LstPort = new List<Port>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new Port
                    {
                        PortName = Convert.ToString(Result["PortName"]),
                        PortId = Convert.ToInt32(Result["PortId"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPort;
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
        public void GetAllCommodityForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCommodityForPage", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.CommodityForPage> LstCommodity = new List<Areas.Import.Models.CommodityForPage>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstCommodity.Add(new Areas.Import.Models.CommodityForPage
                    {
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        CommodityName = Result["CommodityName"].ToString(),
                        PartyCode = Result["CommodityAlias"].ToString(),
                        CommodityType=Result["CommodityType"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { LstCommodity, State };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }
        public void GetAllCartingApp(int RoleId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = RoleId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CartingList> LstCartingApp = new List<CartingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCartingApp.Add(new CartingList
                    {
                        CartingAppId = Convert.ToInt32(Result["CartingAppId"]),
                        CartingNo = Result["CartingNo"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ApplicationDate = Result["ApplicationDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCartingApp;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetCartingApp(int CartingAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CartingApplication ObjCartingApp = new CartingApplication();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjCartingApp.CartingAppId = Convert.ToInt32(Result["CartingAppId"]);
                    ObjCartingApp.CartingNo = Result["CartingNo"].ToString();
                    ObjCartingApp.ApplicationNo = Result["ApplicationNo"].ToString();
                    ObjCartingApp.ApplicationDate = Result["ApplicationDate"].ToString().Split()[0];
                    ObjCartingApp.CHAEximTraderId = Convert.ToInt32(Result["CHAEximTraderId"]);
                    ObjCartingApp.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    ObjCartingApp.CHAName = Result["CHAName"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjCartingApp.lstShipping.Add(new ShippingDetails
                        {
                            CartingAppDtlId = Convert.ToInt32(Result["CartingAppDtlId"]),
                            CartingAppId = Convert.ToInt32(Result["CartingAppId"]),
                            ShippingBillNo = (Result["ShippingBillNo"] == null ? "" : Result["ShippingBillNo"]).ToString(),
                            ShippingDate = Result["ShippingBillDate"].ToString(),
                            CommInvcNo = (Result["CommInvNo"] == null ? "" : Result["CommInvNo"]).ToString(),
                            PackingListNo = (Result["PackingListNo"] == null ? "" : Result["PackingListNo"]).ToString(),
                            Exporter = (Result["EximTraderName"]).ToString(),
                            ExporterId = Convert.ToInt32(Result["ExporterEximTraderId"]),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            MarksAndNo = Result["MarksAndNo"].ToString(),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            Weight = Convert.ToDecimal(Result["Weight"]),
                            FoBValue = Convert.ToDecimal(Result["FobValue"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            CommodityName = Result["CommodityName"].ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCartingApp;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void AddEditCartingApp(CartingApplication objCA, int Uid)
        {
            string Param = "0", ReturnObj = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = objCA.CartingAppId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objCA.CartingNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objCA.ApplicationNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCA.ApplicationDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAEximTraderId", MySqlDbType = MySqlDbType.Int32, Value = objCA.CHAEximTraderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = objCA.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = objCA.StringifyData });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output }); ;
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 60, Value = objCA.CartingAppId, Direction = ParameterDirection.Output });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditCartingApp", CommandType.StoredProcedure, dparam, out Param, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Carting Application Saved Successfully" : "Carting Application Updated Successfully";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Update Carting Application As It Exist In Another Page";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
            }
        }
        public void DeleteCartingApp(int CartingAppId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = CartingAppId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("DeleteCartingApp", CommandType.StoredProcedure, Dparam);
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Carting Application Deleted Successfully.";
                    _DBResponse.Status = 1;
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Carting Application Can't be Deleted as It Is Used In Carting Work Order.";
                    _DBResponse.Status = 2;
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Carting Application Can't be Deleted as It Is Used In Another Page";
                    _DBResponse.Status = 3;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
        }
        public void PrintCartingApp(int CartingAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintCrtngApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PrintCA> lstCA = new List<PrintCA>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCA.Add(new PrintCA
                    {
                        ShipBillNo = Result["ShippingBillNo"].ToString(),
                        ShipBillDate = Result["ShippingBillDate"].ToString(),
                        Exporter = Result["Exporter"].ToString(),
                        Commodity = Result["Commodity"].ToString(),
                        MarksAndNo = Result["MarksAndNo"].ToString(),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        Weight = Convert.ToDecimal(Result["Weight"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstCA[0].CartingNo = Result["CartingNo"].ToString();
                        lstCA[0].CartingDt = Result["ApplicationDate"].ToString();
                        lstCA[0].CHAName = Result["CHAName"].ToString();
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCA;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        #endregion

        #region  Carting Register
        public void AddEditCartingRegister(DSR_CartingRegister objCR, string XML /*, string LocationXML,string ClearLocation=null*/)
        {
            string OutParam = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = objCR.CartingRegisterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = objCR.CartingAppId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_RegisterDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objCR.RegisterDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objCR.CartingType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objCR.CommodityType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = objCR.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = objCR.Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objCR.GodownId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = objCR.CHAId });

            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objCR.ShippingLineId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.VarChar, Value = objCR.ShippingLineName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SpaceType", MySqlDbType = MySqlDbType.VarChar, Value = objCR.SpaceType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingXML", MySqlDbType = MySqlDbType.Text, Value = XML });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_LocationXML", MySqlDbType = MySqlDbType.Text, Value = LocationXML });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_ClearLocation", MySqlDbType = MySqlDbType.Text, Value = ClearLocation });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = OutParam });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditCartingRegister", CommandType.StoredProcedure, Dparam, out OutParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1 ? "Carting Register Details Saved Successfully" : "Carting Register Details Updated Successfully");
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Data Already Exists";
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Location Already Occupied";
                }
                else if (Result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Cannot Update Carting Register Details As It Already Used In Next Page";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = -1;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        public void GetAllRegisterDetails(int Page,int Uid,string SBNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Value = null });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.VarChar, Value = SBNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAllCartingRegister", CommandType.StoredProcedure, Dparam);
            List<DSR_CartingRegister> lstCR = new List<DSR_CartingRegister>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCR.Add(new DSR_CartingRegister
                    {
                        CartingRegisterId = Convert.ToInt32(result["CartingRegisterId"]),
                        CartingRegisterNo = result["CartingRegisterNo"].ToString(),
                        CartingAppId = Convert.ToInt32(result["CartingAppId"]),
                        ShippingBillNo = result["ShippingBillNo"].ToString(),
                        RegisterDate = result["RegisterDate"].ToString(),
                        Remarks = (result["Remarks"] == null ? "" : result["Remarks"]).ToString(),
                        ApplicationNo = result["ApplicationNo"].ToString(),
                        CHAName = result["CHANAME"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstCR;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void GetRegisterDetails(int CartingRegisterId, int Uid, string Purpose = null,string SBNo="",int Page=0)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Purpose", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = Purpose });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.VarChar, Value = SBNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.VarChar, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAllCartingRegister", CommandType.StoredProcedure, Dparam);
            DSR_CartingRegister objCR = new DSR_CartingRegister();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objCR.CartingRegisterId = Convert.ToInt32(result["CartingRegisterId"]);
                    objCR.CartingAppId = Convert.ToInt32(result["CartingAppId"]);
                    objCR.CartingRegisterNo = result["CartingRegisterNo"].ToString();
                    //objCR.CartingAppId = Convert.ToInt32(result["CartingAppId"]);
                    objCR.RegisterDate = result["RegisterDate"].ToString();
                    objCR.ApplicationDate = result["ApplicationDate"].ToString();
                    objCR.Remarks = (result["Remarks"] == null ? "" : result["Remarks"]).ToString();
                   // objCR.GodownName = result["GodownName"].ToString();
                    objCR.ApplicationNo = result["ApplicationNo"].ToString();
                    objCR.CHAName = result["CHANAME"].ToString();
                    objCR.CartingType = Convert.ToInt32(result["CartingType"]);
                    objCR.CommodityType = Convert.ToInt32(result["CommodityType"]);
                    //objCR.GodownId = Convert.ToInt32(result["GodownId"]);
                    objCR.CHAId = Convert.ToInt32(result["CHAId"]);
                    objCR.ShippingLineId = Convert.ToInt32(result["ShippingLineId"]);
                    objCR.ShippingLineName = result["ShippingLineName"].ToString();
                    objCR.SpaceType = result["SpaceType"].ToString();
                    objCR.IsShortCargo = Convert.ToInt32(result["IsShortCargo"]);
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objCR.lstRegisterDtl.Add(new DSR_CartingRegisterDtl
                        {
                            ShippingBillNo = result["ShippingBillNo"].ToString(),
                            ShippingDate = result["ShippingBillDate"].ToString(),
                            CargoDescription = result["CargoDescription"].ToString(),
                            GodownId = Convert.ToInt32(result["GodownId"]),
                            GodownName = Convert.ToString(result["GodownName"]),
                            CommodityName = result["CommodityName"].ToString(),
                            CargoType = Convert.ToInt32(result["CargoType"]),
                            MarksAndNo = (result["MarksAndNo"] == null ? "" : result["MarksAndNo"]).ToString(),
                            NoOfUnits = Convert.ToInt32(result["NoOfUnits"]),
                            Weight = Convert.ToDecimal(result["Weight"]),
                            FoBValue = Convert.ToDecimal(result["FobValue"]),
                            CUM = Convert.ToDecimal(result["CUM"]),
                            SQM = Convert.ToDecimal(result["SQM"]),
                            ReservedSQM = Convert.ToDecimal(result["ReservedSQM"]),
                            TotalSQM = Convert.ToDecimal(result["TotalSQM"]),

                            UnReservedWT = Convert.ToDecimal(result["UnReservedWT"]),
                            ReservedCBM = Convert.ToDecimal(result["ReservedCBM"]),
                            ManualWT = Convert.ToDecimal(result["ManualWT"]),
                            MechanicalWT = Convert.ToDecimal(result["MechanicalWT"]),

                            ActualQty = Convert.ToInt32(result["ActualQty"]),
                            ActualWeight = Convert.ToDecimal(result["ActualWeight"]),
                            Exporter = Convert.ToString(result["Exporter"]),
                            CartingAppDtlId = Convert.ToInt32(result["CartingAppDtlId"]),
                            LocationDetails = (result["LocationDetails"] == null ? "" : result["LocationDetails"]).ToString(),
                            Location = (result["Location"] == null ? "" : result["Location"]).ToString(),
                            ExporterId = Convert.ToInt32(result["ExporterId"]),
                            CommodityId = Convert.ToInt32(result["CommodityId"]),
                            CartingRegisterDtlId = Convert.ToInt32(result["CartingRegisterDtlId"]),
                        });
                    }
                }
                if(result.NextResult())
                {
                    while(result.Read())
                    {
                        objCR.lstShortCargoDetails.Add(new DSR_ShortCargoDetails
                        {
                            ShortCargoDtlId=Convert.ToInt32(result["ShortCargoDtlId"]),
                            CartingDate=result["CartingDate"].ToString(),
                            Qty=Convert.ToInt32(result["Qty"]),
                            Weight = Convert.ToDecimal(result["Weight"]),
                            SQM = Convert.ToDecimal(result["SQM"]),
                            FOB = Convert.ToDecimal(result["FOB"]),
                            ReservedSQM= Convert.ToDecimal(result["ReservedSQM"]),
                            TotalSQM = Convert.ToDecimal(result["TotalSQM"]),
                            ManualWT= Convert.ToDecimal(result["ManualWT"]),
                            MechanicalWT= Convert.ToDecimal(result["MechanicalWT"]),
                             modalGodownId = Convert.ToInt32(result["GodownId"]),
                             modalGodownName = Convert.ToString(result["GodownName"]),
                        });
                    }
                }
                if (Purpose == "edit")
                {
                    if (result.NextResult())
                    {
                        while (result.Read())
                        {
                            objCR.lstGdnWiseLctn.Add(new Areas.Export.Models.DSRGodownWiseLocation
                            {
                               // Row = result["Row"].ToString(),
                                LocationName = result["LocationName"].ToString(),
                                LocationId = Convert.ToInt32(result["LocationId"]),
                                //IsOccupied=Convert.ToBoolean(result["IsOccupied"])
                            });
                        }
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objCR;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void GetAllApplicationNo()
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] Dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAppNoForCartingRegister", CommandType.StoredProcedure, Dpram);
            _DBResponse = new DatabaseResponse();
            List<DSRApplicationNoDet> lstApplication = new List<DSRApplicationNoDet>();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstApplication.Add(new DSRApplicationNoDet
                    {
                        ApplicationNo = result["ApplicationNo"].ToString(),
                        CartingAppId = Convert.ToInt32(result["CartingAppId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstApplication;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        public void GetAppDetForCartingRegister(int CartingAppId,int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = CartingAppId });           
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppDetForCartingRegister", CommandType.StoredProcedure, DParam);
            DSR_CartingRegister ObjCarting = new DSR_CartingRegister();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjCarting.CartingAppId = Convert.ToInt32(Result["CartingAppId"]);
                    ObjCarting.ApplicationDate = Convert.ToString(Result["ApplicationDate"]);
                    ObjCarting.GodownName = Result["GodownName"].ToString();
                    ObjCarting.CHAName = Result["EximTraderName"].ToString();
                    ObjCarting.CHAId = Convert.ToInt32(Result["CHAEximTraderId"]);
                    ObjCarting.ShippingLineName = Result["ShippingLineName"].ToString();
                    ObjCarting.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjCarting.lstRegisterDtl.Add(new DSR_CartingRegisterDtl
                        {
                            CartingAppDtlId = Convert.ToInt32(Result["CartingAppDtlId"]),
                            ShippingBillNo = Result["ShippingBillNo"].ToString(),
                            ShippingDate = Result["ShippingBillDate"].ToString(),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            MarksAndNo = Result["MarksAndNo"].ToString(),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            FoBValue = Convert.ToDecimal(Result["FobValue"]),
                            Weight = Convert.ToDecimal(Result["Weight"]),
                            CommodityName = Result["CommodityName"].ToString(),
                            Exporter = Result["EximTraderName"].ToString(),
                            ExporterId = Convert.ToInt32(Result["EximTraderId"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            FoBValue1 = Convert.ToDecimal(Result["FobValue"]),
                            //ReservedSpace= Convert.ToDecimal(Result["ReservedSpace"]),
                        });
                    }
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        ObjCarting.lstGdnWiseLctn.Add(new Areas.Export.Models.DSRGodownWiseLocation
                //        {
                //            LocationId = Convert.ToInt32(Result["LocationId"]),
                //            LocationName = Result["LocationName"].ToString(),
                          
                //        });
                //    }
                //}

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjCarting;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void DeleteCartingRegister(int CartingRegisterId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("DelCartingRegister", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Carting Register Details Deleted Successfully";
                }
                else if (Result == 2 || Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete As It Is Used In Another Page";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
            }
        }

        public void GetLocationDetailsByGodownId(int GodownId)
        {

            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_godownid", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            IDataParameter[] Dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetLocationDetailsByGodownId", CommandType.StoredProcedure, Dpram);
            _DBResponse = new DatabaseResponse();
            List<Areas.Export.Models.DSRGodownWiseLocation> lstApplication = new List<Areas.Export.Models.DSRGodownWiseLocation>();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstApplication.Add(new Areas.Export.Models.DSRGodownWiseLocation
                    {
                        LocationId = Convert.ToInt32(result["LocationId"]),
                        //Row = result["Row"].ToString(),
                        //Column = result["Column"].ToString(),
                        LocationName= result["LocationName"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstApplication;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        public void GetAvailableReservedSpace(int GodownId,int ExporterId,string RegisterDate)
        {

            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ExporterId", MySqlDbType = MySqlDbType.Int32, Value = ExporterId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_RegisterDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(RegisterDate).ToString("yyyy-MM-dd") });
            IDataParameter[] Dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetAvailableReservedSpace", CommandType.StoredProcedure, Dpram);
            _DBResponse = new DatabaseResponse();
            DSR_CartingRegisterDtl ResvSpace = new DSR_CartingRegisterDtl();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    ResvSpace.ReservedSpace = Convert.ToDecimal(result["ResvSpace"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Data = ResvSpace;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        public void GetAllGodown(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllGodownExp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Godown> LstGodown = new List<Godown>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstGodown.Add(new Godown
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Result["GodownName"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstGodown;
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
        public void AddShortCargoDetail(string XML,int CartingRegisterId,int CartingRegisterDtlId,string ShippingBillNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegisterDtlId", MySqlDbType = MySqlDbType.Int32, Value = CartingRegisterDtlId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingBillNo", MySqlDbType = MySqlDbType.VarChar, Value = ShippingBillNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CartingXML", MySqlDbType = MySqlDbType.Text, Value = XML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("Addshortcargodtl", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 )
                {
                    _DBResponse.Data = 1;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Short Cargo Details Saved Successfully";
                }
                else if(Result==2)
                {
                    _DBResponse.Data = 1;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Cannot Save data as next step already done";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = -1;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        #endregion

        #region Container Movement
        public void GetAllInternalMovement()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_ContainerMovement> LstInternalMovement = new List<DSR_ContainerMovement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new DSR_ContainerMovement
                    {
                        MovementNo = Result["MovementNo"].ToString(),
                        //BOENo = Result["BOENo"].ToString(),
                        MovementId = Convert.ToInt32(Result["MovementId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInternalMovement;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        public void GetInternalPaymentSheetInvoice(int ContainerStuffingDtlId, int ContainerStuffingId, string ContainerNo, String MovementDate,
            string InvoiceType, int DestLocationIdiceId, int Partyid, string ctype, int pvalue,decimal trv,string cargotype, int InvoiceId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ConstuffingId", MySqlDbType = MySqlDbType.Int32, Value = ContainerStuffingDtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(MovementDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "In_DestLocationId", MySqlDbType = MySqlDbType.Int32, Value = DestLocationIdiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_partyId", MySqlDbType = MySqlDbType.Int32, Value = Partyid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_charge_type", MySqlDbType = MySqlDbType.VarChar, Value = ctype });
            LstParam.Add(new MySqlParameter { ParameterName = "in_port_value", MySqlDbType = MySqlDbType.Int32, Value = pvalue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_trweight", MySqlDbType = MySqlDbType.Decimal, Value = trv });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Cargotype", MySqlDbType = MySqlDbType.VarChar, Value = cargotype });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Constuffing", MySqlDbType = MySqlDbType.Int32, Value = ContainerStuffingId });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            //try
            //{
            //    var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetDeliveryPaymentSheet", DParam);

            //    var objPostPaymentSheet = new PostPaymentSheet();
            //    objPostPaymentSheet.InvoiceType = InvoiceType;
            //    objPostPaymentSheet.InvoiceDate = InvoiceDate;
            //    objPostPaymentSheet.RequestId = DestuffingAppId;
            //    objPostPaymentSheet.RequestNo = DestuffingAppNo;
            //    objPostPaymentSheet.RequestDate = DestuffingAppDate;
            //    objPostPaymentSheet.PartyId = PartyId;
            //    objPostPaymentSheet.PartyName = PartyName;
            //    objPostPaymentSheet.PartyAddress = PartyAddress;
            //    objPostPaymentSheet.PartyState = PartyState;
            //    objPostPaymentSheet.PartyStateCode = PartyStateCode;
            //    objPostPaymentSheet.PartyGST = PartyGST;
            //    objPostPaymentSheet.PayeeId = PayeeId;
            //    objPostPaymentSheet.PayeeName = PayeeName;
            //    objPostPaymentSheet.DeliveryType = DeliveryType;

            //    objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));
            //    objPostPaymentSheet.lstPreInvoiceCargo = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreInvoiceCargo>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreInvoiceCargo));


            //    objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
            //    {
            //        if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
            //            objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
            //        if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
            //            objPostPaymentSheet.CHAName += item.CHAName + ", ";
            //        if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
            //            objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
            //        if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
            //            objPostPaymentSheet.BOENo += item.BOENo + ", ";
            //        if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
            //            objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
            //        if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
            //            objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
            //        if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
            //            objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + " , ";
            //        if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
            //            objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
            //        if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
            //            objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
            //        if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
            //            objPostPaymentSheet.CartingDate += item.CartingDate + ", ";

            //        if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
            //        {
            //            objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
            //            {
            //                CargoType = item.CargoType,
            //                CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
            //                CFSCode = item.CFSCode,
            //                CIFValue = item.CIFValue,
            //                ContainerNo = item.ContainerNo,
            //                ArrivalDate = item.ArrivalDate,
            //                ArrivalTime = item.ArrivalTime,
            //                LineNo = item.LineNo,
            //                BOENo = item.BOENo,
            //                DeliveryType = item.DeliveryType,
            //                DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
            //                Duty = item.Duty,
            //                GrossWt = item.GrossWeight,
            //                Insured = item.Insured,
            //                NoOfPackages = item.NoOfPackages,
            //                Reefer = item.Reefer,
            //                RMS = item.RMS,
            //                Size = item.Size,
            //                SpaceOccupied = item.SpaceOccupied,
            //                SpaceOccupiedUnit = item.SpaceOccupiedUnit,
            //                StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
            //                WtPerUnit = item.WtPerPack,
            //                AppraisementPerct = item.AppraisementPerct,
            //                HeavyScrap = item.HeavyScrap,
            //                StuffCUM = item.StuffCUM
            //            });
            //        }
            //        objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
            //        {
            //            CargoType = item.CargoType,
            //            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
            //            CFSCode = item.CFSCode,
            //            CIFValue = item.CIFValue,
            //            ContainerNo = item.ContainerNo,
            //            ArrivalDate = item.ArrivalDate,
            //            ArrivalTime = item.ArrivalTime,
            //            LineNo = item.LineNo,
            //            BOENo = item.BOENo,
            //            DeliveryType = item.DeliveryType,
            //            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
            //            Duty = item.Duty,
            //            GrossWt = item.GrossWeight,
            //            Insured = item.Insured,
            //            NoOfPackages = item.NoOfPackages,
            //            Reefer = item.Reefer,
            //            RMS = item.RMS,
            //            Size = item.Size,
            //            SpaceOccupied = item.SpaceOccupied,
            //            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
            //            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
            //            WtPerUnit = item.WtPerPack,
            //            AppraisementPerct = item.AppraisementPerct,
            //            HeavyScrap = item.HeavyScrap,
            //            StuffCUM = item.StuffCUM
            //        });

            //        objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
            //        objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
            //        objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
            //        objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
            //        objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
            //        objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
            //            + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
            //    });

            //    var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");

            //    //******************************************************************************************************
            //    //Get Godown Type From Godown Master By GodownId
            //    if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 1)
            //    {
            //        List<MySqlParameter> LstParam2 = new List<MySqlParameter>();
            //        LstParam2.Add(new MySqlParameter
            //        {
            //            ParameterName = "in_godownid",
            //            MySqlDbType = MySqlDbType.Int32,
            //            Value = objPostPaymentSheet.lstPreInvoiceCargo.Count > 0 ? objPostPaymentSheet.lstPreInvoiceCargo[0].GodownId : 0
            //        });

            //        IDataParameter[] DParam2 = { };
            //        DParam2 = LstParam2.ToArray();

            //        var GodowntypeId = Convert.ToInt32(DataAccess.ExecuteScalar("getgodowntypeid", CommandType.StoredProcedure, DParam2));
            //        objPostPaymentSheet.CalculateCharges(5, ChargeName, GodowntypeId);
            //    }
            //    else
            //    {
            //        objPostPaymentSheet.CalculateCharges(5, ChargeName);
            //    }
            //    //*******************************************************************************************************
            //    _DBResponse.Status = 1;
            //    _DBResponse.Message = "Success";
            //    _DBResponse.Data = objPostPaymentSheet;
            //}
            //catch (Exception ex)
            //{
            //    _DBResponse.Status = 0;
            //    _DBResponse.Message = "No Data";
            //    _DBResponse.Data = null;
            //}
            //finally
            //{

            //}



            DSR_MovementInvoice objInvoice = new DSR_MovementInvoice();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerMovementPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {
               
                _DBResponse = new DatabaseResponse();

                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["ShippingLineId"]);
                        objInvoice.CHAName = Result["ShippingLineName"].ToString();
                        objInvoice.PartyName = Result["ShippingLineName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["DestuffingId"]);
                        objInvoice.RequestNo = Result["DestuffingNo"].ToString();
                        objInvoice.RequestDate = Result["DestuffingDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();

                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPrePaymentCont.Add(new DSRPreInvoiceContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            // BOENo = Result["BOENo"].ToString(),
                            GrossWeight = Result["GrossWt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWt"]),
                            WtPerPack = Result["WtPerPack"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Result["CIFValue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Result["Duty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Duty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Result["RMS"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Result["DeliveryType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Result["IsInsured"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["IsInsured"]),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            SpaceOccupied = Result["SpaceOccupied"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Result["HeavyScrap"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Result["AppraisementPerct"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = " ",
                            // CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ApproveOn = "0",
                            // BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            //  LineNo = Result["LineNo"].ToString(),
                            OperationType = 0,
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                        });
                        objInvoice.lstPostPaymentCont.Add(new DSRPostPaymentContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            //BOENo = Result["BOENo"].ToString(),
                            GrossWt = Result["GrossWt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWt"]),
                            WtPerUnit = Result["WtPerPack"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Result["CIFValue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Result["Duty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Duty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Result["IsInsured"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["IsInsured"]),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            SpaceOccupied = Result["SpaceOccupied"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Result["HeavyScrap"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Result["AppraisementPerct"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = " ",
                            //  CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),

                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new DSRPostPaymentChrg
                        {
                            ChargeId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"] == System.DBNull.Value ? "" : Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                            Discount = Result["Discount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Discount"]),
                            Taxable = Result["Taxable"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Result["IGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Result["IGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Result["SGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Result["SGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Result["CGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Result["CGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Result["Total"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Total"]),
                            OperationId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                        });
                    }


                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContWiseAmount.Add(new DSRContainerWiseAmount
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                            ContainerId = 0,
                            InvoiceId = 0,
                            LineNo = ""
                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new DSROperationCFSCodeWiseAmount
                        {
                            InvoiceId = InvoiceId,
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"] == System.DBNull.Value ? "" : Result["ContainerNo"].ToString(),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            OperationId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    if (Result.Read())
                    {
                        objInvoice.hasSDAvailableBalance = Convert.ToInt32(Result["hasSDAvailableBalance"]);
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrgBreakup.Add(new DSRCMPostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            CFSCode = Result["CFSCode"].ToString(),
                            Startdate = Result["StartDate"].ToString(),
                            EndDate = Result["EndDate"].ToString()
                        });
                    }


                }

                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objInvoice;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        internal void getOnlyRightsGodown()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid )}); 
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getOnlyRightsGodown", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Godown> LstGodown = new List<Godown>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstGodown.Add(new Godown
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Result["GodownName"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstGodown;
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

        public void GodownWiseLocation(int GodownId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGdwnWiseLctn", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.GodownWiseLctn> lstGodownlctn = new List<Areas.Import.Models.GodownWiseLctn>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGodownlctn.Add(new Areas.Import.Models.GodownWiseLctn
                    {
                        LocationId = Convert.ToInt32(Result["LocationId"]),
                        LocationName = Convert.ToString(Result["LocationName"]),
                        // IsOccupied = Convert.ToInt32(Result["IsOccupied"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstGodownlctn;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void AddEditInvoiceMovement(DSR_Movement_Invoice ObjPostPaymentSheet, DSR_Movement_Invoice ObjPostPaymentSheett, DSR_Movement_Invoice ObjPostPaymentSheettt, string ContainerXML, string ContainerXMLL, string ContainerXMLLL, string ChargesXML, string ChargesXMLL, string ChargesXMLLL, string ContWiseChargeXML, string ContWiseChargeXMLL, string ContWiseChargeXMLLL, string OperationCfsCodeWiseAmtXML, string OperationCfsCodeWiseAmtXMLL, string OperationCfsCodeWiseAmtXMLLL,string ChargesBreakupXML,
           string ChargesBreakupXMLL, string ChargesBreakupXMLLL,
      int BranchId, int Uid,
     string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";

            string ReturnObj = "";

            if (ObjPostPaymentSheet.BOEDate == "")
            {
                ObjPostPaymentSheet.BOEDate = "1900-01-01";
            }

            if (ObjPostPaymentSheet.OldLctnNames == "")
            {
                ObjPostPaymentSheet.OldLctnNames = "1900-01-01";
            }
            else
            {
                ObjPostPaymentSheet.OldLctnNames = "1900-01-01";
            }



            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPostPaymentSheet.MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_port", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.OldLocationIds });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.OldGodownName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjPostPaymentSheet.BOEDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.NewGodownName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjPostPaymentSheet.OldLctnNames).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Area });
            LstParam.Add(new MySqlParameter { ParameterName = "in_sid", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.FromGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_sname", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });

            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLocationIds", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.LocationName });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_ModuleNameMovement", MySqlDbType = MySqlDbType.VarChar, Value = "Import" });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Cargo", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.Cargo });


            //  LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });
            //  LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });


            //LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyIdd", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheett.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyNamee", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheett.PartyName });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyIddd", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheettt.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyNameee", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheettt.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNoo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheett.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddresss", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheett.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStatee", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheett.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCodee", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheett.PartyStateCode });

            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNooo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheettt.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddressss", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheettt.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateee", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheettt.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCodeee", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheettt.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });

            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotall", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.AllTotal });

            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotalll", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.AllTotal });


            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmtt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.TotalAmt });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscountt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.TotalDiscount });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxablee", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGSTT", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGSTT", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGSTT", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmtt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheett.InvoiceAmt });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmttt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.TotalAmt });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscounttt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.TotalDiscount });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxableee", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGSTTT", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGSTTT", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGSTTT", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmttt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.InvoiceAmt });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            //   LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });


            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXMLL", MySqlDbType = MySqlDbType.Text, Value = ContainerXMLL });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXMLL", MySqlDbType = MySqlDbType.Text, Value = ChargesXMLL });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXMLL", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXMLL });
            //   LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXMLL", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXMLL });


            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXMLLL", MySqlDbType = MySqlDbType.Text, Value = ContainerXMLLL });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXMLLL", MySqlDbType = MySqlDbType.Text, Value = ChargesXMLLL });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXMLLL", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXMLLL });
            //   LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXMLLL", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXMLLL });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXMLL", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXMLL });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXMLLL", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXMLLL });

            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });


            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            //  LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientIdd", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientIdd });

            // LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientIddd", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientIddd });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TareWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.TareWeight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheettt.CargoWeight });


            //   LstParam.Add(new MySqlParameter { ParameterName = "ReturnObjj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObjj });

            //  LstParam.Add(new MySqlParameter { ParameterName = "ReturnObjjj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObjjj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditImpContainerMovement", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            DSR_InvoiceList inv = new DSR_InvoiceList();
            try
            {
                if (Result == 1)
                {
                    String[] invobj;
                    invobj = GeneratedClientId.Split(',');
                    String[] movobj;
                    movobj = ReturnObj.Split(',');
                    if (invobj.Length >= 1)
                        inv.invoiceno = invobj[0];
                    if (invobj.Length >= 2)
                        inv.invoicenoo = invobj[1];
                    if (invobj.Length >= 3)
                        inv.invoicenooo = invobj[2];
                    inv.MovementNo = movobj[0];
                    _DBResponse.Data = inv;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Movement Payment Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Payment Invoice have not generated due to low SD Balance";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }

        public void GetInvoiceDetailsForMovementPrintByNo(string InvoiceNo, string InvoiceType = "EXPMovement")
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceType });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceDetailsForMovementExportPrintByNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSR_Movement_Invoice objPaymentSheet = new DSR_Movement_Invoice();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheet.CompanyName = Result["CompanyName"].ToString();
                    objPaymentSheet.CompanyShortName = Result["CompanyShortName"].ToString();
                    objPaymentSheet.CompanyAddress = Result["CompanyAddress"].ToString();
                    objPaymentSheet.CompGST = Result["GstIn"].ToString();
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPaymentSheet.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                        objPaymentSheet.InvoiceNo = Result["InvoiceNo"].ToString();
                        objPaymentSheet.InvoiceDate = Result["InvoiceDate"].ToString();
                        objPaymentSheet.PartyName = Result["PartyName"].ToString();
                        objPaymentSheet.PartyState = Result["PartyState"].ToString();
                        objPaymentSheet.PartyAddress = Result["PartyAddress"].ToString();
                        objPaymentSheet.PartyStateCode = Result["PartyStateCode"].ToString();
                        objPaymentSheet.PartyGST = Result["PartyGSTNo"].ToString();
                        objPaymentSheet.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                        objPaymentSheet.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                        objPaymentSheet.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                        objPaymentSheet.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                        objPaymentSheet.TotalAmt = Convert.ToDecimal(Result["InvoiceAmt"]);

                        objPaymentSheet.ShippingLineName = Result["ShippingLinaName"] == System.DBNull.Value ? "" : Result["ShippingLinaName"].ToString();
                        //   objPaymentSheet.BOENo = Result["BOENo"].ToString();
                        //   objPaymentSheet.BOEDate = Result["BOEDate"].ToString();
                        objPaymentSheet.TotalSpaceOccupied = Result["TotalSpaceOccupied"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                        objPaymentSheet.RequestNo = Result["StuffingReqNo"].ToString();
                        objPaymentSheet.TotalNoOfPackages = Result["TotalNoOfPackages"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["TotalNoOfPackages"]);
                        objPaymentSheet.TotalGrossWt = Result["TotalGrossWt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["TotalGrossWt"]);
                        objPaymentSheet.CHAName = Result["CHAName"].ToString();
                        objPaymentSheet.ImporterExporter = Result["ExporterImporterName"].ToString();
                        objPaymentSheet.ArrivalDate = Result["ArrivalDate"].ToString();
                        objPaymentSheet.DeliveryDate = Result["DeliveryDate"].ToString();
                        objPaymentSheet.DestuffingDate = Result["DestuffingDate"].ToString();
                        objPaymentSheet.PartyId = Convert.ToInt32(Result["PartyId"]);
                        objPaymentSheet.PartyCode = Result["PartyAlias"].ToString();
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPaymentSheet.lstPostPaymentCont.Add(new DSRPostPaymentContainer()
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            ArrivalDate = Convert.ToString(Result["FromDate"]),
                            DeliveryDate = Convert.ToString(Result["ToDate"]),
                            // GrossWt = Convert.ToDecimal(Result["TotalGrossWt"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        if (Convert.ToDecimal(Result["Rate"]) > 0)
                        {
                            objPaymentSheet.lstPostPaymentChrg.Add(new DSRPostPaymentChrg()
                            {
                                Clause = Convert.ToString(Result["OperationSDesc"]),
                                ChargeName = Convert.ToString(Result["OperationDesc"]),
                                SACCode = Convert.ToString(Result["SACCode"]),
                                Rate = Convert.ToDecimal(Result["Rate"]),
                                Taxable = Convert.ToDecimal(Result["Taxable"]),

                                CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                                CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                                SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                                SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                                IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                                IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                                Total = Convert.ToDecimal(Result["Total"]),

                            });
                        }
                    }
                }

                //-------------------------------------------------------------------------
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheet;
                }

                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
                //-----------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetInternalMovement(int MovementId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSR_ContainerMovement ObjInternalMovement = new DSR_ContainerMovement();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjInternalMovement.MovementId = Convert.ToInt32(Result["MovementId"]);
                    ObjInternalMovement.MovementNo = Result["MovementNo"].ToString();
                    ObjInternalMovement.MovementDate = Result["MovementDate"].ToString();
                    //    ObjInternalMovement.CargoDescription = Result["CargoDescription"].ToString();
                    //    ObjInternalMovement.BOENo = Result["BOENo"].ToString();
                    //    ObjInternalMovement.BOEDate = Result["BOEDate"].ToString();
                    //    ObjInternalMovement.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    //    ObjInternalMovement.GrossWeight = Convert.ToInt32(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    //    ObjInternalMovement.OldLocationIds = Result["OldLocationIds"].ToString();
                    //    ObjInternalMovement.OldLctnNames = Result["OldLctnNames"].ToString();
                    //    ObjInternalMovement.NewLocationIds = Result["NewLocationIds"].ToString();
                    //    ObjInternalMovement.NewLctnNames = Result["NewLctnNames"].ToString();
                    //    ObjInternalMovement.FromGodownId = Convert.ToInt32(Result["FromGodownId"]);
                    //    ObjInternalMovement.ToGodownId = Convert.ToInt32(Result["ToGodownId"]);
                    //    ObjInternalMovement.NewGodownName = Result["NewGodownName"].ToString();
                    //    ObjInternalMovement.OldGodownName = Result["OldGodownName"].ToString();
                    //    ObjInternalMovement.DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]);
                    //
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjInternalMovement;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void DelInternalMovement(int MovementId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Internal Movement Details Deleted Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Internal Movement Details As It Exists In Another Page";
                    _DBResponse.Data = null;
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
        public void AddEditImpInternalMovement(DSR_ContainerMovement ObjInternalMovement)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = ObjInternalMovement.Container });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.MovementDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.SealDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.SealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.CustomSealDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.port });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.POD });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLocationIds", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.sid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.Sline });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLocationIds", MySqlDbType = MySqlDbType.Int32, Value = ObjInternalMovement.LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.LocationName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjInternalMovement.MovementId == 0 ? "Internal Movement Details Saved Successfully" : "Internal Movement  Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Internal Movement  Details Already Exists";
                    _DBResponse.Data = null;
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


        public void GetContainerForMovement()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerForMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_ContainerMovement> LstStuffing = new List<DSR_ContainerMovement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    if (LstStuffing.Count <= 0)
                    {
                        LstStuffing.Add(new DSR_ContainerMovement
                        {
                            Container = Result["ContainerNo"].ToString(),
                            ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"])
                        });
                    }

                    else
                    {
                        int flag = 0;

                        for (int i1 = 0; i1 < LstStuffing.Count; i1++)
                        {
                            if (LstStuffing[i1].Container == Result["ContainerNo"].ToString())
                            {
                                flag = 1;
                                break;
                            }
                        }

                        if (flag == 0)
                        {
                            LstStuffing.Add(new DSR_ContainerMovement
                            {
                                Container = Result["ContainerNo"].ToString(),
                                ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"])
                            });
                        }
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetPaymentParty()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyExport", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<PaymentPartyName> objPaymentPartyName = new List<PaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new PaymentPartyName()
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentPartyName;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetLocationForInternalMovement()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortForContainerMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_ContainerMovement> LstInternalMovement = new List<DSR_ContainerMovement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new DSR_ContainerMovement
                    {
                        LocationName = Result["PortName"].ToString(),
                        LocationId = Convert.ToInt32(Result["PortId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInternalMovement;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetConDetForMovement(int ContainerStuffingDtlId, String ContNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ConstuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingDtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContNo });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetConDetForInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSR_ContainerMovement ObjInternalMovement = new DSR_ContainerMovement();
            List<DSR_ShippingBill> LstShippingBillNo = new List<DSR_ShippingBill>();

            try
            {
                if (Result.Read())
                {
                    Status = 1;
                    ObjInternalMovement.Container = Result["ContainerNo"].ToString();
                    ObjInternalMovement.CFSCode = Result["CFSCode"].ToString();
                    ObjInternalMovement.SealNo = Result["ShippingSeal"].ToString();
                    ObjInternalMovement.CustomSealNo = Result["CustomSeal"].ToString();
                    ObjInternalMovement.ContainerStuffingDtlId = Convert.ToInt32(Result["ContainerStuffingDtlId"].ToString());
                    ObjInternalMovement.ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"].ToString());
                    ObjInternalMovement.TransportMode = Result["TransportMode"] == System.DBNull.Value ? 1 : Convert.ToInt32(Result["TransportMode"].ToString());
                    ObjInternalMovement.CargoWeight = Result["CargoWeight"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CargoWeight"].ToString());
                    ObjInternalMovement.LocationName = Result["POL"].ToString();
                    ObjInternalMovement.LocationId = Convert.ToInt32(Result["PolId"].ToString());
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        if (LstShippingBillNo.Count <= 0)
                        {
                            LstShippingBillNo.Add(new DSR_ShippingBill
                            {
                                shippingBillNo = Result["ShippingBillNo"].ToString(),
                                CargoWeight=Convert.ToDecimal(Result["CargoWeight"].ToString()),
                                CargoType = Convert.ToInt32(Result["CargoType"].ToString()),
                            });
                        }
                        else
                        {
                            int flag = 0;

                            for (int i1 = 0; i1 < LstShippingBillNo.Count; i1++)
                            {
                                if (LstShippingBillNo[i1].shippingBillNo == Result["ShippingBillNo"].ToString())
                                {
                                    flag = 1;
                                    break;
                                }
                            }

                            if (flag == 0)
                            {
                                LstShippingBillNo.Add(new DSR_ShippingBill
                                {
                                    shippingBillNo = Result["ShippingBillNo"].ToString(),
                                    CargoWeight = Convert.ToDecimal(Result["CargoWeight"].ToString()),
                                    CargoType = Convert.ToInt32(Result["CargoType"].ToString()),
                                });
                            }
                        }
                    }
                }
                ObjInternalMovement.ShipBill = LstShippingBillNo;
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjInternalMovement;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        #endregion

        #region BTT Cargo Entry
        public void AddEditBTTCargoEntry(DSRBTTCargoEntry ObjBTT, string StuffingXML, int BranchId, int Uid)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BTTId", MySqlDbType = MySqlDbType.Int32, Value = ObjBTT.BTTId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BTTDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjBTT.BTTDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = ObjBTT.CartingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjBTT.CartingNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjBTT.CartingDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjBTT.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjBTT.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UId", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "BTTDetailXML", MySqlDbType = MySqlDbType.Text, Value = StuffingXML });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditBTTCargoEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "BTT Cargo Entry Saved Successfully";
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "BTT Cargo Entry Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "BTT Cargo Entry Details Already Exist";
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Cannot Edit BTT Cargo Entry Details As It Already Exist In Another Page";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        public void GetBTTCargoEntry(string SearchValue="", int Page=0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BTTId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32,  Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar,  Value = SearchValue });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBTTCargoEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<DSRBTTCargoEntry> ObjBTT = new List<DSRBTTCargoEntry>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjBTT.Add(new DSRBTTCargoEntry()
                    {
                        BTTId = Convert.ToInt32(Result["BTTCargoEntryId"]),
                        CartingNo = Convert.ToString(Result["CartingAppNo"]),
                        CartingDate = Convert.ToString(Result["CartingDate"]),
                        CHAName = Convert.ToString(Result["EximTraderName"]),
                        BTTNo = Convert.ToString(Result["BTTCargoEntryNo"]),
                        BTTDate = Convert.ToString(Result["BTTCargoEntryDate"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjBTT;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetBTTCargoEntryById(int BTTId,int Page=0, string SearchValue="")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BTTId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = BTTId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar,  Value = SearchValue });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBTTCargoEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRBTTCargoEntry ObjBTT = new DSRBTTCargoEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjBTT.BTTId = Convert.ToInt32(Result["BTTCargoEntryId"]);
                    ObjBTT.BTTNo = Result["BTTCargoEntryNo"].ToString();
                    ObjBTT.BTTDate = Result["BTTCargoEntryDate"].ToString();
                    ObjBTT.CartingId = Convert.ToInt32(Result["CartingAppId"]);
                    ObjBTT.CartingNo = Result["CartingAppNo"].ToString();
                    ObjBTT.CartingDate = Result["CartingDate"].ToString();
                    ObjBTT.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjBTT.CHAName = Result["EximTraderName"].ToString();
                    ObjBTT.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjBTT.lstBTTCargoEntryDtl.Add(new DSRBTTCargoEntryDtl
                        {
                            BTTDetailId = Convert.ToInt32(Result["BTTCargoEntryDetailID"]),
                            CartingDetailId = Convert.ToInt32(Result["CartingDetailId"]),
                            ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]),
                            ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            CommodityName = Convert.ToString(Result["CommodityName"]),
                            CargoDescription = Convert.ToString(Result["CargoDescription"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            BTTQuantity = Convert.ToInt32(Result["BTTQuantity"]),
                            BTTWeight = Convert.ToInt32(Result["BTTWeight"]),
                            ManualWT = Convert.ToInt32(Result["ManualWT"]),
                            MechanicalWT = Convert.ToInt32(Result["MechanicalWT"]),
                            FOBValue = Convert.ToInt32(Result["Fob"]),
                            SQM = Convert.ToInt32(Result["BTTSqm"]),
                            TFOBValue= Convert.ToDecimal(Result["TFOBValue"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjBTT;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void DeleteBTTCargoEntry(int BTTId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BTTId", MySqlDbType = MySqlDbType.Int32, Value = BTTId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteBTTCargoEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "BTT Cargo Entry Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "BTT Cargo Entry Details Not Found";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete BTT Cargo Entry Details As It Exists In Another Page";
                    _DBResponse.Data = null;
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
        public void GetCartingAppList(int BTTCargoEntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BTTCargoEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = BTTCargoEntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //IDataReader Result = DataAccess.ExecuteDataReader("GetCartingAppList", CommandType.StoredProcedure, DParam);
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingRegListForBTT", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<DSRBTTCartingList> lstBTTCartingList = new List<DSRBTTCartingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBTTCartingList.Add(new DSRBTTCartingList()
                    {
                        CartingId = Convert.ToInt32(Result["CartingAppId"]),
                        CartingNo = Convert.ToString(Result["CartingNo"]),
                        CartingDate = Convert.ToString(Result["ApplicationDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBTTCartingList;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetCartingDetailList(int CartingAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //IDataReader Result = DataAccess.ExecuteDataReader("GetCartingDetailList", CommandType.StoredProcedure, DParam);
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingRegDetailBTT", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<DSRBTTCartingDetailList> lstBTTCartingDetailList = new List<DSRBTTCartingDetailList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstBTTCartingDetailList.Add(new DSRBTTCartingDetailList()
                    {
                        CartingDetailId = Convert.ToInt32(Result["CartingAppDtlId"]),
                        ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]),
                        ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]),
                        CargoDescription = Convert.ToString(Result["CargoDescription"]),
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        CommodityName = Convert.ToString(Result["CommodityName"]),
                        NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                        GrossWeight = Convert.ToDecimal(Result["Weight"]),
                        FOBValue = Convert.ToDecimal(Result["FobValue"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstBTTCartingDetailList;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetCHAListForBTT()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfCHA", CommandType.StoredProcedure);
            IList<DSRCHAList> lstCHA = new List<DSRCHAList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new DSRCHAList
                    {
                        CHAId = Convert.ToInt32(result["EximTraderId"]),
                        CHAName = result["EximTraderName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstCHA;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        #endregion


        #region Job Order
        public void GetAllTrainNo()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetExportTrainNo", CommandType.StoredProcedure, dpram);
            IList<DSR_TrainList> lstTrainNo = new List<DSR_TrainList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {

                    Status = 1;
                    lstTrainNo.Add(new DSR_TrainList { TrainNo = result["TrainNo"].ToString(), TrainSummaryID = Convert.ToInt32(result["TrainSummaryID"]) });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstTrainNo;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        public void GetImportJODetailsFrPrint(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetDetForPrntexpjo", CommandType.StoredProcedure, dpram);
            DSRPrintJOModel objMdl = new DSRPrintJOModel();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objMdl.ContainerType = result["ContainerType"].ToString();
                    objMdl.JobOrderNo = result["JobOrderNo"].ToString();
                    objMdl.JobOrderDate = result["JobOrderDate"].ToString();
                    objMdl.ShippingLineName = result["ShippingLineName"].ToString();
                    objMdl.FromLocation = result["FromLocation"].ToString();
                    objMdl.ToLocation = result["ToLocation"].ToString();
                    objMdl.TrainNo = result["TrainNo"].ToString();
                    objMdl.TrainDate = result["TrainDate"].ToString();

                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objMdl.lstDet.Add(new DSRPrintJOModelDet
                        {
                            ContainerNo = result["ContainerNo"].ToString(),
                            ContainerSize = result["ContainerSize"].ToString(),
                            ShippingLineName = result["ShippingLineName"].ToString(),
                            OnBehalf = result["OnBehalf"].ToString(),
                            CustomSealNo = result["CustomSealNo"].ToString(),
                           
                            Sline = result["Line_Seal_No"].ToString(),
                            POL = result["POL"].ToString(),
                            POD = result["POD"].ToString(),
                            Ct_Tare = Convert.ToDecimal(result["Ct_Tare"].ToString()),
                            Cargo_Wt = Convert.ToDecimal(result["Cargo_Wt"].ToString()),
                            CFSCode = result["CFSCode"].ToString(),

                            
                            CargoType = result["CargoType"].ToString(),
                            ContainerLoadType = result["ContainerLoadType"].ToString(),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objMdl;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void GetTrainDtl(int TrainSumId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_TrainSumId", MySqlDbType = MySqlDbType.Int32, Value = TrainSumId });
            //   lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetExportTrainDet", CommandType.StoredProcedure, dpram);
            FormOneForImpJO objFormone = new FormOneForImpJO();
            IList<DSR_TrainDtl> lstDtl = new List<DSR_TrainDtl>();
            // IList<Areas.Import.Models.Yard> lstYard = new List<Areas.Import.Models.Yard>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    // objFormone.FormOneNo = result["FormOneNo"].ToString();
                    lstDtl.Add(new DSR_TrainDtl
                    {
                        TrainSummarySerial = Convert.ToInt32(result["TrainSummarySerial"]),
                        TrainSummaryID = Convert.ToInt32(result["TrainSummaryID"]),
                        TrainNo = result["TrainNo"].ToString(),
                        TrainDate = Convert.ToDateTime(result["TrainDate"]).ToString("dd/MM/yyyy"),
                        PortId = (result["PortId"]) == null ? 0 : Convert.ToInt32(result["PortId"]),
                        Wagon = (result["Wagon"] == null ? "" : result["Wagon"]).ToString(),
                        ContainerNo = (result["ContainerNo"] == null ? "" : result["ContainerNo"]).ToString(),
                        SZ = (result["SZ"] == null ? "" : result["SZ"]).ToString(),
                        LineSeal = (result["LineSeal"] == null ? "" : result["LineSeal"]).ToString(),
                        Commodity = (result["Commodity"] == null ? "" : result["Commodity"]).ToString(),
                        SLine = (result["SLine"] == null ? "" : result["SLine"]).ToString(),
                        TW = (result["TW"] == null ? 0 : Convert.ToDecimal(result["TW"])),
                        CW = (result["CW"] == null ? 0 : Convert.ToDecimal(result["CW"])),
                        GW = (result["GW"] == null ? 0 : Convert.ToDecimal(result["GW"])),
                        Ct_Status = (result["Status"] == null ? "" : result["Status"]).ToString(),
                        PKGS = (result["PKGS"] == null ? 0 : Convert.ToDecimal(result["PKGS"])),
                        CustomSeal = (result["CustomSeal"] == null ? "" : result["CustomSeal"]).ToString(),
                        Shipper = (result["Shipper"] == null ? "" : result["Shipper"]).ToString(),
                        CHA = (result["CHA"] == null ? "" : result["CHA"]).ToString(),
                        CRRSBillingParty = (result["CRRSBillingParty"] == null ? "" : result["CRRSBillingParty"]).ToString(),
                        BillingParty = (result["BillingParty"] == null ? "" : result["BillingParty"]).ToString(),
                        StuffingMode = (result["StuffingMode"] == null ? "" : result["StuffingMode"]).ToString(),
                        SBillNo = (result["SBillNo"] == null ? "" : result["SBillNo"]).ToString(),
                        Date = (result["Date"] == null ? "" : result["Date"]).ToString(),
                        Origin = (result["Origin"] == null ? "" : result["Origin"]).ToString(),
                        POL = (result["POL"] == null ? "" : result["POL"]).ToString(),
                        POD = (result["POD"] == null ? "" : result["POD"]).ToString(),
                        DepDate = (result["DepDate"] == null ? "" : result["DepDate"]).ToString(),
                        

                        TransportName = result["PortName"].ToString(),

                        ShippingLineName = result["S_Linee"] == DBNull.Value ? "" : result["S_Linee"].ToString(),
                        ShippingLineId = result["S_LineId"] == DBNull.Value ? 0 : Convert.ToInt32(result["S_LineId"].ToString())

                    });
                }
                if (Status == 1)
                {

                    _DBResponse.Data = lstDtl;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void GetTrainDetailsOnEditMode(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfexpJobOrd", CommandType.StoredProcedure, dpram);
            DSRExportJobOrder objImpJO = new DSRExportJobOrder();
            IList<DSR_TrainDtl> lstDtl = new List<DSR_TrainDtl>();
            IList<DSR_ImportJobDel > lstdel = new List<DSR_ImportJobDel>();
            // IList<Areas.Import.Models.Yard> lstYard = new List<Areas.Import.Models.Yard>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;

            lstdel.Add(new DSR_ImportJobDel
            {
                JobOrderDtlId = 0
            });

            objImpJO.deleteXML = Newtonsoft.Json.JsonConvert.SerializeObject(lstdel);
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    //objImpJO.PickUpLocation = result["PickUpLocation"].ToString();
                    //objImpJO.ImpJobOrderId = Convert.ToInt32(result["ImpJobOrderId"]);
                    //objImpJO.JobOrderNo = result["JobOrderNo"].ToString();
                    //objImpJO.JobOrderDate = Convert.ToDateTime(result["JobOrderDate"]);
                    //objImpJO.TrainNo = result["TrainNo"].ToString();
                    //objImpJO.TrainDate = Convert.ToDateTime(result["TrainDate"]);

                    //  objImpJO.Remarks = result["Remarks"].ToString();

                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        lstDtl.Add(new DSR_TrainDtl
                        {
                            JobOrderDtlId = Convert.ToInt32(result["JobOrderDtlId"]),
                            JobOrderId = Convert.ToInt32(result["JobOrderId"]),
                            ContainerNo = result["ContainerNo"].ToString(),
                           SZ = result["ContainerSize"].ToString(),
                           LineSeal = result["Line_Seal_No"].ToString(),
                            Commodity = result["Cont_Commodity"].ToString(),
                            PortId = Convert.ToInt32(result["PortId"]),
                            CustomSealNo = result["CustomSealNo"].ToString(),
                            ShippingLineNo = result["ShippingLineNo"].ToString(),
                            ShippingLineId = Convert.ToInt32(result["ShippingLine"].ToString()),
                            ShippingLineName = result["ShippingLineName"].ToString(),
                            CargoType = result["CargoType"].ToString(),
                            ContainerLoadType = result["ContainerLoadType"].ToString(),
                            PKGS = Convert.ToDecimal(result["NoOfPackages"].ToString()),
                            Wagon = result["Wagon_No"].ToString(),
                            SLine = result["S_Line"].ToString(),
                            TransportName = result["Transportfrom"].ToString(),
                           TW = Convert.ToDecimal(result["Ct_Tare"].ToString()),
                            CW = Convert.ToDecimal(result["Cargo_Wt"].ToString()),
                            GW = Convert.ToDecimal(result["Gross_Wt"].ToString()),
                            Ct_Status = result["Ct_Status"].ToString(),
                           // Destination = result["Destination"].ToString(),
                          //  Smtp_No = result["Smtp_No"].ToString(),
                            TrainSummaryID = Convert.ToInt32(result["TrainSummaryID"]),
                            TrainSummarySerial = Convert.ToInt32(result["TrainSummarySerial"]),
                            Remarks = result["Remarks"].ToString(),
                            CargoDescription = result["CargoDescription"].ToString(),
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Data = lstDtl;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        public void AddEditImpJO(DSRExportJobOrder objJO, string XML = null)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = objJO.ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PickUpLocation", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objJO.PickUpLocation });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TrainSummaryID", MySqlDbType = MySqlDbType.Int32, Value = objJO.TrainSummaryID });
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderFor", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objJO.JobOrderFor });
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objJO.JobOrderDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_transportfor", MySqlDbType = MySqlDbType.Int32, Value = objJO.TransportBy });

            lstParam.Add(new MySqlParameter { ParameterName = "in_Train_no", MySqlDbType = MySqlDbType.VarChar, Value = objJO.TrainNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Train_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objJO.TrainDate) });
            //  lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = objJO.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.LongText, Value = XML });
            //  lstParam.Add(new MySqlParameter { ParameterName = "in_LctnXML", MySqlDbType = MySqlDbType.Text, Value = LocationXML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditExportJobOrder", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Export Job Order Saved Successfully" : "Export Job Order Saved Successfully";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }


        public void GetAllImpJO(int Page)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfExpJobOrdForPage", CommandType.StoredProcedure, dpram);
            IList<DSR_ImportJobOrderList> lstImpJO = new List<DSR_ImportJobOrderList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstImpJO.Add(new DSR_ImportJobOrderList
                    {
                        ImpJobOrderId = Convert.ToInt32(result["ImpJobOrderId"]),
                        JobOrderNo = result["JobOrderNo"].ToString(),
                        JobOrderDate = result["JobOrderDate"].ToString(),
                        TrainNo = result["TrainNo"].ToString(),
                        TrainDate = result["TrainDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstImpJO;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }


        public void GetAllImpJO(string ContainerNo)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            //lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo.TrimStart() });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("SearchListOfExportJobOrder", CommandType.StoredProcedure, dpram);
            IList<DSR_ImportJobOrderList> lstImpJO = new List<DSR_ImportJobOrderList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstImpJO.Add(new DSR_ImportJobOrderList
                    {
                        ImpJobOrderId = Convert.ToInt32(result["ImpJobOrderId"]),
                        JobOrderNo = result["JobOrderNo"].ToString(),
                        JobOrderDate = result["JobOrderDate"].ToString(),
                        TrainNo = result["TrainNo"].ToString(),
                        TrainDate = result["TrainDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstImpJO;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        public void GetImpJODetails(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfexpJobOrd", CommandType.StoredProcedure, dpram);
            DSRExportJobOrder objImpJO = new DSRExportJobOrder();
            IList<DSR_TrainDtl> lstDtl = new List<DSR_TrainDtl>();
            IList<DSR_ImportJobDel> lstdel = new List<DSR_ImportJobDel>();
            // IList<Areas.Import.Models.Yard> lstYard = new List<Areas.Import.Models.Yard>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;

            lstdel.Add(new DSR_ImportJobDel
            {
                JobOrderDtlId = 0
            });

            objImpJO.deleteXML = Newtonsoft.Json.JsonConvert.SerializeObject(lstdel);
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objImpJO.PickUpLocation = result["PickUpLocation"].ToString();
                    objImpJO.ImpJobOrderId = Convert.ToInt32(result["ImpJobOrderId"]);
                    objImpJO.JobOrderNo = result["JobOrderNo"].ToString();
                    objImpJO.JobOrderDate = Convert.ToDateTime(result["JobOrderDate"]);
                    objImpJO.TrainNo = result["TrainNo"].ToString();
                    objImpJO.TrainDate = Convert.ToDateTime(result["TrainDate"]);
                    objImpJO.TrainSummaryID = result["TrainSummaryId"].ToString();
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        lstDtl.Add(new DSR_TrainDtl
                        {
                            JobOrderDtlId = Convert.ToInt32(result["JobOrderDtlId"]),
                            JobOrderId = Convert.ToInt32(result["JobOrderId"]),
                            ContainerNo = result["ContainerNo"].ToString(),
                           SZ = result["ContainerSize"].ToString(),
                           LineSeal = result["Line_Seal_No"].ToString(),
                            Commodity = result["Cont_Commodity"].ToString(),
                            PortId = Convert.ToInt32(result["PortId"]),
                            CustomSealNo = result["CustomSealNo"].ToString(),
                            ShippingLineNo = result["ShippingLineNo"].ToString(),
                            ShippingLineId = Convert.ToInt32(result["ShippingLine"].ToString()),
                            ShippingLineName = result["ShippingLineName"].ToString(),
                            CargoType = result["CargoType"].ToString(),
                            ContainerLoadType = result["ContainerLoadType"].ToString(),
                            PKGS = Convert.ToDecimal(result["NoOfPackages"].ToString()),
                            Wagon = result["Wagon_No"].ToString(),
                            SLine = result["S_Line"].ToString(),
                            TransportForm = result["Transportfrom"].ToString(),
                            TW= Convert.ToDecimal(result["Ct_Tare"].ToString()),
                            CW = Convert.ToDecimal(result["Cargo_Wt"].ToString()),
                            GW = Convert.ToDecimal(result["Gross_Wt"].ToString()),
                            Ct_Status = result["Ct_Status"].ToString(),
                           // Destination = result["Destination"].ToString(),
                           // Smtp_No = result["Smtp_No"].ToString(),
                            TrainSummaryID = Convert.ToInt32(result["TrainSummaryID"]),
                            TrainSummarySerial = Convert.ToInt32(result["TrainSummarySerial"]),
                            Remarks = result["Remarks"].ToString(),
                            CargoDescription = result["CargoDescription"].ToString(),
                        });
                    }
                }
                if (lstDtl.Count > 0)
                {
                    objImpJO.StringifyXML = Newtonsoft.Json.JsonConvert.SerializeObject(lstDtl);
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objImpJO;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }


        public void DeleteImpJO(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("DeleteExpJO", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Export Job Order Deleted Successfully";
                }
                else if (result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete As It Is Used In Another Form";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        public void ListOfShippingLinePartyCode(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLineForJO", CommandType.StoredProcedure, Dparam);
            IList<ShippingLineForPage> lstShippingLine = new List<ShippingLineForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstShippingLine.Add(new ShippingLineForPage
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
                        PartyCode = Result["PartyCode"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstShippingLine, State };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }

        public void ListOfShippingLine()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetShippingLine", CommandType.StoredProcedure, DParam);
            IList<ShippingLine> lstShippingLine = new List<ShippingLine>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstShippingLine.Add(new ShippingLine
                    {
                        ShippingLineId = Convert.ToInt32(result["ShippingLineId"]),
                        ShippingLineName = result["ShippingLine"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstShippingLine;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        public void GetAllPickupLocation()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //    LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPickupLocation", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRPickupModel> LstPickUp = new List<DSRPickupModel>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPickUp.Add(new DSRPickupModel
                    {
                        pickup_location = Result["PickUpLctn"].ToString(),
                        alias = (Result["LctnAlias"] == null ? "" : Result["LctnAlias"]).ToString(),
                        id = Convert.ToInt32(Result["Id"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPickUp;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetAllPortForJobOrderTrasport()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //    LstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            //  DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<TransformList> LstPort = new List<TransformList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new TransformList
                    {
                        PortName = Result["PortName"].ToString(),
                        PortId = Convert.ToInt32(Result["PortId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPort;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        #endregion

        #region Loaded Container Payment Sheet
        public void GetLoadedContainerRequestForPaymentSheet(int LoadContReqId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = LoadContReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLoadedContainerRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<DSR_PaySheetStuffingRequest> objPaySheetStuffing = new List<DSR_PaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new DSR_PaySheetStuffingRequest()
                    {
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Convert.ToString(Result["CHAName"]),
                        CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                        Address = Result["Address"].ToString(),
                        State = Result["State"].ToString(),
                        StateCode = Result["StateCode"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["LoadContReqId"]),
                        StuffingReqNo = Convert.ToString(Result["LoadContReqNo"]),
                        StuffingReqDate = Convert.ToString(Result["RequestDate"]),
                        GateEntryDateTime = Convert.ToString(Result["EntryDateTime"]),
                        MoveTo = Convert.ToInt32(Result["MoveTo"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetLoadedPaymentSheetInvoice(int ContainerStuffingId, string InvoiceDate, string DeliveryDate, string InvoiceType, string contxml,
           int PayeeId, int PartyId, int IntercartingApplicable, int ICDDestuffing, int OnWheel, int NoVehicle, decimal Distance,string PortMovement, string ExportUnder, int InvoiceId = 0,int PortId=0, int POD=0, int ExamType=0, int TransportFrom=0, int TransportTo=0, int IsWashing=0, int IsWeighment=0, int IsReworking=0,int  IsInternalShifting=0, int IsLiftOnEmpty=0, int IsLiftOffEmpty=0,int ISGREPaid=0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(DeliveryDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = contxml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.VarChar, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.VarChar, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IntercartingApplicable", MySqlDbType = MySqlDbType.Int32, Value = IntercartingApplicable });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ICDDestuffing", MySqlDbType = MySqlDbType.Int32, Value = ICDDestuffing });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OnWheel", MySqlDbType = MySqlDbType.Int32, Value = OnWheel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoVehicle", MySqlDbType = MySqlDbType.Int32, Value = NoVehicle });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal, Value = Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MoveToPort", MySqlDbType = MySqlDbType.Int32, Value = PortId });
          
            LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.Int32, Value = POD });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ExamType", MySqlDbType = MySqlDbType.Int32, Value = ExamType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportFrom", MySqlDbType = MySqlDbType.Int32, Value = TransportFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportTo", MySqlDbType = MySqlDbType.Int32, Value = TransportTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsWashing", MySqlDbType = MySqlDbType.Int32, Value = IsWashing });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsWeighment", MySqlDbType = MySqlDbType.Int32, Value = IsWeighment });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsReworking", MySqlDbType = MySqlDbType.Int32, Value = IsReworking });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsInternalShifting", MySqlDbType = MySqlDbType.Int32, Value = IsInternalShifting });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsLiftOnEmpty", MySqlDbType = MySqlDbType.Int32, Value = IsLiftOnEmpty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsLiftOffEmpty", MySqlDbType = MySqlDbType.Int32, Value = IsLiftOffEmpty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GREPaidBy", MySqlDbType = MySqlDbType.Int32, Value = ISGREPaid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortMovement", MySqlDbType = MySqlDbType.VarString, Value = PortMovement });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            DSR_ExpPaymentSheet objInvoice = new DSR_ExpPaymentSheet();
            IDataReader Result = DataAccess.ExecuteDataReader("getLoadedContainerPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {

                _DBResponse = new DatabaseResponse();

                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();
                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();
                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();
                        objInvoice.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                        objInvoice.PayeeName = Result["PayeeName"].ToString();
                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentCont.Add(new DSRExpInvoiceContainerBase
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDateTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            GrossWt = Result["GrossWeight"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Result["WtPerPack"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Result["CIFValue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Result["Duty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Duty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            Insured = Result["IsInsured"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["IsInsured"]),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            SpaceOccupied = Result["SpaceOccupied"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SpaceOccupied"]),
                           /* DestuffingDate = Result["DestuffingDate"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            CartingDate = Result["CartingDate"].ToString(),
                            GateExitDate = Result["GateExitDate"].ToString(),
                            OldArrivalDate = Result["OldArrivalDate"].ToString(),*/
                            DestuffingDate = "",
                            StuffingDate = "",
                            CartingDate = "",
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new DSRExpInvoiceChargeBase
                        {
                            ChargeId = Result["ChargeId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["ChargeId"]),
                            Clause = Result["Clause"] == System.DBNull.Value ? "" : Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"] == System.DBNull.Value ? "" : Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                            Discount = Result["Discount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Discount"]),
                            Taxable = Result["Taxable"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Result["IGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Result["IGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Result["SGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Result["SGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Result["CGSTPer"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Result["CGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Result["Total"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Total"]),
                            OperationId = Result["OperationId"].ToString(),
                            HazAmt = Convert.ToDecimal(Result["HazAmt"]),
                            OdcAmt = Convert.ToDecimal(Result["OdcAmt"]),
                            HazTotal = Convert.ToDecimal(Result["HazTotal"]),
                            OdcTotal = Convert.ToDecimal(Result["OdcTotal"]),

                            HAZIGSTAmt = Result["HAZIGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["HAZIGSTAmt"]),
                            HAZCGSTAmt = Result["HAZCGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["HAZCGSTAmt"]),
                            HAZSGSTAmt = Result["HAZSGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["HAZSGSTAmt"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstGREPostPaymentChrg.Add(new DSRExpInvoiceChargeBaseGRE
                        {
                            ChargeId = Convert.ToInt32(Result["ChargeId"]),
                            InoviceId = Convert.ToInt32(Result["InoviceId"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            OperationId = Result["OperationId"].ToString(),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            HazAmt = Convert.ToDecimal(Result["HazAmt"]),
                            OdcAmt = Convert.ToDecimal(Result["OdcAmt"]),
                            HazTotal = Convert.ToDecimal(Result["HazTotal"]),
                            OdcTotal = Convert.ToDecimal(Result["OdcTotal"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContwiseAmt.Add(new DSR_ExpContWiseAmount
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstGREContwiseAmt.Add(new DSR_ExpContWiseAmountGRE
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingLineName = Result["ShippingLineName"].ToString()
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationContwiseAmt.Add(new DSR_ExpOperationContWiseCharge
                        {
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"] == System.DBNull.Value ? "" : Result["ContainerNo"].ToString(),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            OperationId = Result["OperationId"].ToString(),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                            Clause = Result["Clause"].ToString(),
                            DocumentNo = Result["SBNo"].ToString(),
                            DocumentDate = Result["SBDate"].ToString(),
                            DocumentType = (Result["SBNo"].ToString() == "" ? "" : "SB"),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstGREOperationContwiseAmt.Add(new DSR_ExpOperationContWiseChargeGRE
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Result["OperationID"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Clause = Result["Clause"].ToString(),
                            SBNo = Result["SBNo"].ToString(),
                            SBDate = Result["SBDate"].ToString(),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingLineName = Result["ShippingLineName"].ToString()
                        });
                    }
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        objInvoice.ActualApplicable.Add(new DSRHTClauseList
                //        {
                //            ClauseName = Result["ClauseName"].ToString(),
                //            ClauseId = Result["Clause"].ToString(),


                //        });

                //        //objInvoice.ActualApplicable.Add(Result["Clause"].ToString());
                //    }
                //}

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.ActualApplicable.Add(Result["Clause"].ToString());
                    }
                }

                        if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.IsPartyStateInCompState = Convert.ToBoolean(Result["IsLocalGST"]);
                    }
                }
                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objInvoice;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        public void GetTransportTo()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPortOfTransportTo", CommandType.StoredProcedure);
            List<Port> LstPort = new List<Port>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new Port
                    {
                        PortName = Convert.ToString(Result["PortName"]),
                        PortId = Convert.ToInt32(Result["PortId"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstPort;
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
       

        public void GetContainerForLoadedContainerPaymentSheet(int LoadContReqId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = LoadContReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLoadedContainerRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<DSR_PaymentSheetContainer> objPaymentSheetContainer = new List<DSR_PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new DSR_PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Selected = false,
                        Size = Result["Size"].ToString(),
                        ArrivalDt = Result["ArrivalDt"].ToString(),
                        IsHaz = Result["IsHaz"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheetContainer;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void AddEditInvoiceContLoaded(DSR_ExpPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
        int BranchId, int Uid, string Module, string GREChargesXML, string GREContWiseChargXML, string GREOperationCodeWiseAmtXML, int PODId, int TransferFromId, int TransportToId, int GREPaidById, string CargoXML="")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
          
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.DeliveryDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "GREChargesXML", MySqlDbType = MySqlDbType.Text, Value = GREChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "GREContWiseChargXML", MySqlDbType = MySqlDbType.Text, Value = GREContWiseChargXML });
            LstParam.Add(new MySqlParameter { ParameterName = "GREOperationCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GREOperationCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Intercarting", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.IntercartingApplicable });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ICDDestuffing", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.ICDDestuffing });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PODId", MySqlDbType = MySqlDbType.Int32, Value = PODId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransferFromId", MySqlDbType = MySqlDbType.Int32, Value = TransferFromId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportToId", MySqlDbType = MySqlDbType.Int32, Value = TransportToId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GREPaidById", MySqlDbType = MySqlDbType.Int32, Value = GREPaidById });
            
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditContLoadedInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }

    

        public void GetInvoiceDetailsForContLoadedPrintByNo(string InvoiceNo, string InvoiceType = "EXPLod")
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceType });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceDetailsForGodownPrintByNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSR_MovementInvoice objPaymentSheet = new DSR_MovementInvoice();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheet.CompanyName = Result["CompanyName"].ToString();
                    objPaymentSheet.CompanyShortName = Result["CompanyShortName"].ToString();
                    objPaymentSheet.CompanyAddress = Result["CompanyAddress"].ToString();
                    objPaymentSheet.CompGST = Result["GstIn"].ToString();
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPaymentSheet.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                        objPaymentSheet.InvoiceNo = Result["InvoiceNo"].ToString();
                        objPaymentSheet.InvoiceDate = Result["InvoiceDate"].ToString();
                        objPaymentSheet.PartyName = Result["PartyName"].ToString();
                        objPaymentSheet.PartyState = Result["PartyState"].ToString();
                        objPaymentSheet.PartyAddress = Result["PartyAddress"].ToString();
                        objPaymentSheet.PartyStateCode = Result["PartyStateCode"].ToString();
                        objPaymentSheet.PartyGST = Result["PartyGSTNo"].ToString();
                        objPaymentSheet.TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"]);
                        objPaymentSheet.TotalCGST = Convert.ToDecimal(Result["TotalCGST"]);
                        objPaymentSheet.TotalSGST = Convert.ToDecimal(Result["TotalSGST"]);
                        objPaymentSheet.TotalIGST = Convert.ToDecimal(Result["TotalIGST"]);
                        objPaymentSheet.TotalAmt = Convert.ToDecimal(Result["InvoiceAmt"]);

                        objPaymentSheet.ShippingLineName = Result["ShippingLinaName"].ToString();
                        objPaymentSheet.BOENo = Result["BOENo"].ToString();
                        objPaymentSheet.BOEDate = Result["BOEDate"].ToString();
                        objPaymentSheet.TotalSpaceOccupied = Convert.ToDecimal(Result["TotalSpaceOccupied"]);
                        objPaymentSheet.RequestNo = Result["StuffingReqNo"].ToString();
                        objPaymentSheet.TotalNoOfPackages = Convert.ToInt32(Result["TotalNoOfPackages"]);
                        objPaymentSheet.TotalGrossWt = Convert.ToDecimal(Result["TotalGrossWt"]);
                        objPaymentSheet.CHAName = Result["CHAName"].ToString();
                        objPaymentSheet.ImporterExporter = Result["ExporterImporterName"].ToString();
                        objPaymentSheet.ArrivalDate = Result["ArrivalDate"].ToString();
                        objPaymentSheet.DeliveryDate = Result["DeliveryDate"].ToString();
                        objPaymentSheet.DestuffingDate = Result["DestuffingDate"].ToString();
                        objPaymentSheet.PartyId = Convert.ToInt32(Result["PartyId"]);
                        objPaymentSheet.PartyCode = Result["PartyAlias"].ToString();
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        objPaymentSheet.lstPostPaymentCont.Add(new DSRPostPaymentContainer()
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            ArrivalDate = Convert.ToString(Result["FromDate"]),
                            DeliveryDate = Convert.ToString(Result["ToDate"]),
                            // GrossWt = Convert.ToDecimal(Result["TotalGrossWt"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        if (Convert.ToDecimal(Result["Rate"]) > 0)
                        {
                            objPaymentSheet.lstPostPaymentChrg.Add(new DSRPostPaymentChrg()
                            {
                                Clause = Convert.ToString(Result["OperationSDesc"]),
                                ChargeName = Convert.ToString(Result["OperationDesc"]),
                                SACCode = Convert.ToString(Result["SACCode"]),
                                Rate = Convert.ToDecimal(Result["Rate"]),
                                Taxable = Convert.ToDecimal(Result["Taxable"]),

                                CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                                CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                                SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                                SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                                IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                                IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                                Total = Convert.ToDecimal(Result["Total"]),

                            });
                        }
                    }
                }

                //-------------------------------------------------------------------------
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheet;
                }

                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
                //-----------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        #endregion

        #region Load Container Request
        public void ListOfLoadCont(int Page, string ShippbillNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();            
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippbillNo", MySqlDbType = MySqlDbType.VarChar, Value = ShippbillNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListLoadContainerRequest", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRListLoadContReq> LstCont = new List<DSRListLoadContReq>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCont.Add(new DSRListLoadContReq
                    {
                        LoadContReqId = Convert.ToInt32(Result["LoadContReqId"]),
                        LoadContReqNo = Result["LoadContReqNo"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        LoadContReqDate = Result["LoadContReqDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCont;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void SearchListLoadContainerRequest(int Page, string ShippbillNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippbillNo", MySqlDbType = MySqlDbType.VarChar, Value = ShippbillNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("SearchListLoadContainerRequest", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRListLoadContReq> LstCont = new List<DSRListLoadContReq>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCont.Add(new DSRListLoadContReq
                    {
                        LoadContReqId = Convert.ToInt32(Result["LoadContReqId"]),
                        LoadContReqNo = Result["LoadContReqNo"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        LoadContReqDate = Result["LoadContReqDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstCont;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetLoadContDetails(int LoadContReqId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = LoadContReqId });            
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippbillNo", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListLoadCntReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSR_LoadContReq objDet = new DSR_LoadContReq();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDet.LoadContReqId = Convert.ToInt32(Result["LoadContReqId"]);
                    objDet.LoadContReqNo = Result["LoadContReqNo"].ToString();
                    objDet.CHAName = Result["CHAName"].ToString();
                    objDet.LoadContReqDate = Result["LoadContReqDate"].ToString();
                    objDet.PickupValidity = Result["ValidityDate"].ToString();
                    objDet.Remarks = Result["Remarks"].ToString();
                    objDet.CHAId = Convert.ToInt32(Result["CHAId"]);
                    objDet.IsMoveToRCT = Convert.ToBoolean(Result["IsMoveToRCT"]);
                    objDet.MoveTo = Convert.ToString(Result["MoveTo"]);
                    objDet.PODID = Convert.ToInt32(Result["POD"]);
                    objDet.POD = Convert.ToString(Result["PortName"]);
                    objDet.ForeignLiner = Convert.ToString(Result["ForeignLiner"]);
                    objDet.FactoryAddress = Convert.ToString(Result["FactoryAddress"]);
                    objDet.Voyage = Convert.ToString(Result["Voyage"]);
                    objDet.Vessel = Convert.ToString(Result["Vessel"]);
                    objDet.PikUpFrom = Convert.ToString(Result["PickUpFrom"]);
                    objDet.Movement = Convert.ToString(Result["Movement"]);
                    objDet.Distance = Convert.ToDecimal(Result["Distance"]);
                    objDet.Loadmovement = Convert.ToBoolean(Result["Loadmovement"]);
                    objDet.FinalDestinationLocationID = Convert.ToInt32(Result["FinalDestinationLocationID"]);
                    objDet.FinalDestinationLocation = Convert.ToString(Result["FinalDestinationLocation"]);
                    objDet.CustomExaminationType= Convert.ToString(Result["ExamType"]);

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objDet.lstContDtl.Add(new DSRLoadContReqDtl
                        {
                            LoadContReqDetlId = Convert.ToInt32(Result["LoadContReqDetlId"]),
                            ExporterId = Convert.ToInt32(Result["ExporterId"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            IsReefer = Convert.ToBoolean(Result["Reefer"]),
                            IsInsured = Convert.ToBoolean(Result["IsInsured"]),
                            ShippingBillNo = Result["ShippingBillNo"].ToString(),
                            ShippingBillDate = Result["ShippingBillDate"].ToString(),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            FobValue = Convert.ToDecimal(Result["FobValue"]),
                            ExporterName = Result["ExporterName"].ToString(),
                            ShippingLineName = (Result["ShippingLineName"] == null ? "" : Result["ShippingLineName"]).ToString(),
                            CommodityName = Result["CommodityName"].ToString(),
                            EntryNo= Result["EntryNo"].ToString(),
                            IsWashing=Convert.ToBoolean(Result["IsWashing"]),
                            IsWeighment = Convert.ToBoolean(Result["IsWeighment"]),
                            IsReworking = Convert.ToBoolean(Result["IsReworking"]),
                            IsInternalShifting = Convert.ToBoolean(Result["IsInternalShifting"]),
                            IsLiftOnEmpty = Convert.ToBoolean(Result["IsLiftOnEmpty"]),
                            IsLiftOffEmpty = Convert.ToBoolean(Result["IsLiftOffEmpty"]),
                            RMSValue= Convert.ToInt32(Result["RMSValue"]),
                            ReeferHrs= Convert.ToInt32(Result["ReeferHrs"]),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ShippingSeal= Result["ShippingSeal"].ToString(),
                            CustomSeal = Result["CustomSeal"].ToString(),
                            EquipmentSealType = Result["EquipmentSealType"].ToString(),
                            EquipmentStatus = Result["EquipmentStatus"].ToString(),
                            EquipmentQUC = Result["EquipmentQUC"].ToString(),
                            PackageType = Result["PackageType"].ToString(),
                            PackUQCCode = Result["PackUQCCode"].ToString(),
                            PackUQCDescription = Result["PackUQCDescription"].ToString(),
                            IsSEZ = Convert.ToBoolean(Result["SEZ"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objDet;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void AddEditLoadContDetails(DSR_LoadContReq objLoadContReq, string XML)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.LoadContReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objLoadContReq.LoadContReqDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PickupValidity", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objLoadContReq.PickupValidity).ToString("yyyy-MM-dd") });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objLoadContReq.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Movement", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objLoadContReq.Movement });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExamType", MySqlDbType = MySqlDbType.VarChar, Value = objLoadContReq.CustomExaminationType });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PickUpFrom", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = objLoadContReq.PikUpFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MoveTo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = objLoadContReq.MoveTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal,  Value = objLoadContReq.Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FactoryAddress", MySqlDbType = MySqlDbType.VarChar, Size = 150, Value = objLoadContReq.FactoryAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = objLoadContReq.Vessel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ForeignLiner", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = objLoadContReq.ForeignLiner });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Voyage", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = objLoadContReq.Voyage });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.PODID });

            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocationID", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.FinalDestinationLocationID });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocation", MySqlDbType = MySqlDbType.VarChar, Value = objLoadContReq.FinalDestinationLocation });

            LstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = XML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsMoveToRCT", MySqlDbType = MySqlDbType.Bit, Value = objLoadContReq.IsMoveToRCT });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadedExam", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.Loadmovement });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditLoadCntReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = ((Result == 1) ? "Loaded Container Request Saved Successfully" : "Loaded Container Request Updated Successfully");
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Edit Loaded Container Request Details As It Exist In Another Page";
                    _DBResponse.Data = null;
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
        public void DelLoadContReqhdr(int LoadContReqId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = LoadContReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelLoadContReqhdr", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Loaded Container Request DetailsDeleted Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Loaded Container Request Details As It Exist In Another Page";
                    _DBResponse.Data = null;
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
        public void ListOfCommodity()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetMstCommodity", CommandType.StoredProcedure, DParam);
            IList<Areas.Export.Models.Commodity> lstCommodity = new List<Areas.Export.Models.Commodity>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCommodity.Add(new Areas.Export.Models.Commodity
                    {
                        CommodityId = Convert.ToInt32(result["CommodityId"]),
                        CommodityName = result["CommodityName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstCommodity;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }

        public void GetContainerListLoadContReq(string CONTCBT = "")
        {           
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();            
            lstParam.Add(new MySqlParameter { ParameterName = "in_CONTCBT", MySqlDbType = MySqlDbType.String, Value = CONTCBT });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerListLoadContReq", CommandType.StoredProcedure, DParam);
            List<DSRLoadCont> lstContainerInfo = new List<DSRLoadCont>();            
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstContainerInfo.Add(new DSRLoadCont
                    {
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Size = Result["Size"].ToString(),
                        EntryNo = Result["EntryNo"].ToString(),
                        CargoType = Result["CargoType"].ToString(),
                    });
                   
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";                    
                    _DBResponse.Data = lstContainerInfo;
                   
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetLoadedRequestPrint(int LoadContReqId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = LoadContReqId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetPrintLoadContainerRequest", CommandType.StoredProcedure, dpram);
            DSR_LoadContReq objMdl = new DSR_LoadContReq();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    objMdl.lstContDtl.Add(new DSRLoadContReqDtl
                    {
                        ContainerNo = result["ContainerNo"].ToString(),
                        Size = result["Size"].ToString(),
                        ShippingLineName = result["ShippingLineName"].ToString(),
                        EntryNo = result["CFSCode"].ToString(),
                        EmptyDate = result["EmptyDate"].ToString(),
                        StuffingDate = result["StuffingDate"].ToString(),
                    });                    

                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        Status = 1;
                        objMdl.LoadContReqNo = result["LoadContReqNo"].ToString();
                        objMdl.LoadContReqDate = result["LoadContReqDate"].ToString();
                        objMdl.PickupValidity = result["ValidityDate"].ToString();
                        objMdl.CHAName = result["CHAName"].ToString();
                        objMdl.POD = result["PortName"].ToString();
                        objMdl.Distance = Convert.ToDecimal(result["Distance"]);
                        objMdl.MoveTo = result["MoveTo"].ToString();
                        objMdl.PikUpFrom = result["PickUpFrom"].ToString();
                        objMdl.Movement = result["Movement"].ToString();
                        objMdl.Remarks = result["Remarks"].ToString();
                        objMdl.FactoryAddress = result["FactoryAddress"].ToString();
                        objMdl.ExporterName = result["ExporterName"].ToString();
                        objMdl.StuffingDate = result["StuffingDate"].ToString();
                        objMdl.ValidDate = result["ValidDate"].ToString();
                        objMdl.CargoWeight = result["GrossWt"].ToString();
                        objMdl.ShippingLine = result["ShippingLineName"].ToString();
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objMdl;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        #endregion
        #region BTT Payment Sheet
        public void GetCartingApplicationForPaymentSheet(int CartingAppId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //IDataReader Result = DataAccess.ExecuteDataReader("GetCartingApplicationForPaymentSheet", CommandType.StoredProcedure, DParam);

            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingRegisterForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaySheetStuffingRequest> objPaySheetStuffing = new List<PaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new PaySheetStuffingRequest()
                    {
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Convert.ToString(Result["CHAName"]),
                        CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                        Address = Result["Address"].ToString(),
                        State = Result["State"].ToString(),
                        StateCode = Result["StateCode"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["CartingAppId"]),
                        StuffingReqNo = Convert.ToString(Result["CartingAppNo"]),
                        StuffingReqDate = Convert.ToString(Result["RequestDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetShipBillForPaymentSheet(int CartingAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingRegId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CartingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //IDataReader Result = DataAccess.ExecuteDataReader("GetCartingApplicationForPaymentSheet", CommandType.StoredProcedure, DParam);
            IDataReader Result = DataAccess.ExecuteDataReader("GetCartingRegisterForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaymentSheetContainer> objPaymentSheetContainer = new List<PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Selected = false,
                        Size = Result["Size"].ToString(),
                        ArrivalDt = Result["ArrivalDt"].ToString(),
                        IsHaz = Result["IsHaz"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheetContainer;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetBTTPaymentSheet(string InvoiceDate, int AppraisementId, int PartyId,string InvoiceType, string ContainerXML, string ExportUnder, int InvoiceId,int NoOfVehicles)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfVehicles", MySqlDbType = MySqlDbType.Int32, Value = NoOfVehicles });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            DSRInvoiceBTT objInvoice = new DSRInvoiceBTT();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBTTPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {
                
                _DBResponse = new DatabaseResponse();

                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();
                    objInvoice.PartyStateCode = Result["PartyStateCode"].ToString();
                    

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.CHAName = Result["CHAName"].ToString();
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();

                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPrePaymentCont.Add(new DSRPreInvoiceContainerBTT
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerPack = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ApproveOn = Result["ApproveOn"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            LineNo = Result["LineNo"].ToString(),
                            OperationType = Convert.ToInt32(Result["OperationType"]),
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                        });
                        objInvoice.lstPostPaymentCont.Add(new DSRPostPaymentContainerBTT
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new DSRPostPaymentChrgBTT
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            HazAmt = Convert.ToDecimal(Result["HazAmt"]),
                            OdcAmt = Convert.ToDecimal(Result["OdcAmt"]),
                            HazTotal = Convert.ToDecimal(Result["HazTotal"]),
                            OdcTotal = Convert.ToDecimal(Result["OdcTotal"]),
                            HAZIGSTAmt = Result["HAZIGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["HAZIGSTAmt"]),
                            HAZCGSTAmt = Result["HAZCGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["HAZCGSTAmt"]),
                            HAZSGSTAmt = Result["HAZSGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["HAZSGSTAmt"])


                        });
                    }


                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContWiseAmount.Add(new DSRContainerWiseAmountBTT
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                            ContainerId = 0,
                            InvoiceId = 0,
                            LineNo = ""
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new DSROperationCFSCodeWiseAmountBTT
                        {
                            InvoiceId = InvoiceId,
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Clause = Result["Clause"].ToString(),
                            DocumentType ="BTT",
                            DocumentDate = Result["SBDate"].ToString(),
                            DocumentNo = Result["SBNo"].ToString(),
                              
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPreInvoiceCargo.Add(new DSRPreInvoiceCargoBTT
                        {
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            BOLDate = Result["BOLDate"].ToString(),
                            BOLNo = Result["BOLNo"].ToString(),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            CartingDate = Result["CartingDate"].ToString(),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            GodownName = Result["GodownName"].ToString(),
                            GodownWiseLctnNames = Result["GodownWiseLctnNames"].ToString(),
                            GodownWiseLocationIds = Result["GodownWiseLocationIds"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrgBreakup.Add(new DSRBTTPostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            CFSCode = Result["CFSCode"].ToString(),
                            Startdate = Result["StartDate"].ToString(),
                            EndDate = Result["EndDate"].ToString()
                        });
                    }


                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.ActualApplicable.Add(Result["Clause"].ToString());
                    }
                }
                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objInvoice;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        public void AddEditBTTInvoice(DSRInvoiceBTT ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,string ChargesBreakupXML
           ,int BranchId, int Uid,
          string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceBTT", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        #endregion

        #region Export Destuffing
        public void GetContainersForExpDestuffing()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ContainerListForExportDestuffing", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<ContainersExpDestuffing> objPaySheetStuffing = new List<ContainersExpDestuffing>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new ContainersExpDestuffing()
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Result["CHAName"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ContainerStuffingDtlId = Convert.ToInt32(Result["ContainerStuffingDtlId"]),
                        ContainerStuffingId = Convert.ToInt32(Result["ContainerStuffingId"]),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Result["PartyName"].ToString(),
                        ShippingBillNo = Result["ShippingBillNo"].ToString(),
                        ShippingDate = Result["ShippingDate"].ToString(),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Result["ShippingLineName"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetChargesForExpDestuffing(int ContainerStuffingId)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Value = ContainerStuffingId });
            IDataParameter[] DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ChargesForExportDestuffing", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<ExportDestuffingCharges> objPaySheetStuffing = new List<ExportDestuffingCharges>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new ExportDestuffingCharges()
                    {
                        OperationId = Convert.ToInt32(Result["OperationId"]),
                        ChargeType = Result["ChargeType"].ToString(),
                        ChargeName = Result["ChargeName"].ToString(),
                        Charge = Convert.ToDecimal(Result["Charge"]),
                        CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                        CGSTCharge = Convert.ToDecimal(Result["CGSTCharge"]),
                        SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                        SGSTCharge = Convert.ToDecimal(Result["SGSTCharge"]),
                        IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                        IGSTCharge = Convert.ToDecimal(Result["IGSTCharge"]),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Result["PartyName"].ToString(),
                        SACCode = Result["SACCode"].ToString(),
                        Amount = Convert.ToDecimal(Result["Charge"]),
                        Taxable = Convert.ToDecimal(Result["Charge"]),
                        TotalAmount = Convert.ToDecimal(Result["TotalAmount"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        public void GetExportDestuffingPaymentSheet(string InvoiceDate, int AppraisementId, string InvoiceType, string ContainerXML, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingDtlId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            DSRInvoiceExpDestuf objInvoice = new DSRInvoiceExpDestuf();
            IDataReader Result = DataAccess.ExecuteDataReader("GetExportDestuffingPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {
                
                _DBResponse = new DatabaseResponse();

                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.CHAName = Result["CHAName"].ToString();
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();

                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPrePaymentCont.Add(new DSRPreInvoiceContainerExpDestuf
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerPack = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ApproveOn = Result["ApproveOn"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            LineNo = Result["LineNo"].ToString(),
                            OperationType = Convert.ToInt32(Result["OperationType"]),
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                        });
                        objInvoice.lstPostPaymentCont.Add(new DSRPostPaymentContainerExpDestuf
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new DSRPostPaymentChrgExpDestuf
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                        });
                    }


                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContWiseAmount.Add(new DSRContainerWiseAmountExpDestuf
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                            ContainerId = 0,
                            InvoiceId = 0,
                            LineNo = ""
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new DSROperationCFSCodeWiseAmountExpDestuf
                        {
                            InvoiceId = InvoiceId,
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPreInvoiceCargo.Add(new DSRPreInvoiceCargoExpDestuf
                        {
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            BOLDate = Result["BOLDate"].ToString(),
                            BOLNo = Result["BOLNo"].ToString(),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            CartingDate = Result["CartingDate"].ToString(),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            GodownName = Result["GodownName"].ToString(),
                            GodownWiseLctnNames = Result["GodownWiseLctnNames"].ToString(),
                            GodownWiseLocationIds = Result["GodownWiseLocationIds"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerUnit"]),
                        });
                    }
                }

                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objInvoice;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        public void AddEditExpDestufInvoice(DSRInvoiceExpDestuf ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
           int BranchId, int Uid,
          string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceExpDestuf", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Destuffing Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Destuffing Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        #endregion

        #region cargo shifting
        public void GetShippingLineForInvoice()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLineForInvoice", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<PaymentPartyName> objPaymentPartyName = new List<PaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new PaymentPartyName()
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentPartyName;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetShipBillDetails(int ShippingLineId, int ShiftingType, int GodownId)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShiftingType", MySqlDbType = MySqlDbType.Int32, Value = ShiftingType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            IDataParameter[] DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShipBillsForCargoShifting", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<CargoShiftingShipBillDetails> objShipBills = new List<CargoShiftingShipBillDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objShipBills.Add(new CargoShiftingShipBillDetails()
                    {
                        CartingRegisterId = Convert.ToInt32(Result["CartingRegisterId"]),
                        CartingRegisterNo = Convert.ToString(Result["CartingRegisterNo"]),
                        RegisterDate = Convert.ToString(Result["RegisterDate"]),
                        CartingRegisterDtlId = Convert.ToInt32(Result["CartingRegisterDtlId"]),
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        ShippingBillNo = Convert.ToString(Result["ShippingBillNo"]),
                        ShippingBillDate = Convert.ToString(Result["ShippingBillDate"]),
                        ActualQty = Convert.ToDecimal(Result["ActualQty"]),
                        ActualWeight = Convert.ToDecimal(Result["ActualWeight"]),
                        IsChecked = Convert.ToInt32(Result["IsChecked"]) == 0 ? false : true,
                        SQM = Convert.ToDecimal(Result["SQM"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objShipBills;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetCargoShiftingPaymentSheet(string InvoiceDate, int PartyId, string ShipBillsXML, int InvoiceId, string TaxType, int PayeeId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "ShipBillsXML", MySqlDbType = MySqlDbType.Text, Value = ShipBillsXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = TaxType });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            DSRInvoiceCargoShifting objInvoice = new DSRInvoiceCargoShifting();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCargoShiftingPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {
                
                _DBResponse = new DatabaseResponse();

                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.CHAName = Result["CHAName"].ToString();
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();
                        objInvoice.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                        objInvoice.PayeeName = Result["PayeeName"].ToString();

                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new DSRPostPaymentChrgShifting
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                        });
                    }


                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new DSROperationCFSCodeWiseAmountCS
                        {
                            InvoiceId = InvoiceId,
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        objInvoice.lstPostPaymentChrgBreakup.Add(new DSRCargoPostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            CFSCode = Result["CFSCode"].ToString(),
                            Startdate = Result["StartDate"].ToString(),
                            EndDate = Result["EndDate"].ToString(),

                        });
                    }


                }

                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Taxable);
                objInvoice.TotalDiscount = objInvoice.lstPostPaymentChrg.Sum(o => o.Discount);
                objInvoice.TotalTaxable = objInvoice.lstPostPaymentChrg.Sum(o => o.Taxable);
                objInvoice.TotalCGST = objInvoice.lstPostPaymentChrg.Sum(o => o.CGSTAmt);
                objInvoice.TotalSGST = objInvoice.lstPostPaymentChrg.Sum(o => o.SGSTAmt);
                objInvoice.TotalIGST = objInvoice.lstPostPaymentChrg.Sum(o => o.IGSTAmt);
                objInvoice.CWCAmtTotal = objInvoice.TotalTaxable + objInvoice.TotalCGST + objInvoice.TotalSGST + objInvoice.TotalIGST;
                objInvoice.HTAmtTotal = 0;
                objInvoice.CWCTDSPer = 0;
                objInvoice.HTTDSPer = 0;
                objInvoice.TDS = 0;
                objInvoice.TDSCol = 0;
                objInvoice.AllTotal = objInvoice.CWCAmtTotal;
                objInvoice.RoundUp = 0;
                objInvoice.InvoiceAmt = objInvoice.AllTotal;
                objInvoice.ShippingLineName = objInvoice.PartyName;
                objInvoice.CHAName = "";
                objInvoice.ImporterExporter = "";
                objInvoice.Module = "EXPCRGSHFT";
                objInvoice.InvoiceId = InvoiceId;

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objInvoice;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void AddEditCargoShiftInvoice(DSRInvoiceCargoShifting ObjPostPaymentSheet, string ChargesXML,string ChargesBreakXML, string OperationCfsCodeWiseAmtXML,
           int BranchId, int Uid, string CartingRgisterDtlXML, int FromGodownId, int ToGodownId, int ToShippingId, int ShiftingType,int FromShippingLineId)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
             LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Module });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakXML });

            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = FromShippingLineId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_FromShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.FromShippingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToShippingId", MySqlDbType = MySqlDbType.Int32, Value = ToShippingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromGodownId", MySqlDbType = MySqlDbType.Int32, Value = FromGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToGodownId", MySqlDbType = MySqlDbType.Int32, Value = ToGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "ShipBillsXML", MySqlDbType = MySqlDbType.Text, Value = CartingRgisterDtlXML });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShiftingType", MySqlDbType = MySqlDbType.Int32, Value = ShiftingType });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceCargoShift", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        #endregion

        #region Export Container Credit Debit RR
        public void GetInvoiceNoForCreditDebitRR()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getInvoiceForRRCreditDebit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<dynamic> LstInv = new List<dynamic>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInv.Add(new
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(Result["InvoiceDate"]).ToString("dd/MM/yyyy"),
                        CFSCode = Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInv;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetContainerDetForExpRR(int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getInvoiceForRRCreditDebit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRRRCreditDebitInvoiceDetails obj = new DSRRRCreditDebitInvoiceDetails();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    obj.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    obj.PartyId = Convert.ToInt32(Result["PartyId"]);
                    obj.PartyName = Convert.ToString(Result["PartyName"]);
                    obj.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                    obj.PayeeName = Convert.ToString(Result["PayeeName"]);
                    obj.Address = Convert.ToString(Result["Address"]);
                    obj.StateCode = Convert.ToString(Result["StateCode"]);
                }
                if (Result.NextResult())
                {
                    while(Result.Read())
                    {
                        obj.lstContDetailsRRCD.Add(new DSRPostPaymentContainerRRCD
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            Reefer =Convert.ToInt32(Result["IsReefer"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            DestuffingDate =Convert.ToDateTime(Result["DestuffingDate"]),
                            CartingDate = Convert.ToDateTime(Result["CartingDate"]),
                            StuffingDate = Convert.ToDateTime(Result["StuffingDate"]),
                            NoOfPackages= Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                            WtPerUnit = Convert.ToInt32(Result["WtPerUnit"]),
                            SpaceOccupied=Convert.ToDecimal(Result["SpaceOccupied"]),
                            SpaceOccupiedUnit=Convert.ToString(Result["SpaceOccupiedUnit"]),
                            CIFValue=Convert.ToDecimal(Result["CIFValue"]),
                            Duty=Convert.ToDecimal(Result["Duty"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        obj.lstChrgDetailsRRCD.Add(new DSRPostPaymentChrgRRCD
                        {
                            OperationId= Convert.ToInt32(Result["OperationId"]),
                            Clause=Convert.ToString(Result["Clause"]),
                            ChargeType = Convert.ToString(Result["ChargeType"]),
                            ChargeName = Convert.ToString(Result["ChargeName"]),
                            SACCode = Convert.ToString(Result["SACCode"]),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate= Convert.ToDecimal(Result["Rate"]),
                            Amount=Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = obj;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetEXPRRCDSheetInvoice(int InvoiceId,int ShippingLineId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ShippingLineId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetRRCreditDebitPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRInvoiceRRCreditDebit objInvoice = new DSRInvoiceRRCreditDebit();
            try
            {
                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();

                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.InvoiceDate = Convert.ToString(Result["InvoiceDate"]);
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.CHAName = Result["CHAName"].ToString();
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();

                    }
                }

                if(Result.NextResult())
                { 
                    while(Result.Read())
                    { 
                        objInvoice.lstPostPaymentContRRCD.Add(new DSRPostPaymentContainerRRCD
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = string.IsNullOrEmpty(Result["CartingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["CartingDate"].ToString()),
                            DestuffingDate = string.IsNullOrEmpty(Result["DestuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["DestuffingDate"].ToString()),
                            StuffingDate = string.IsNullOrEmpty(Result["StuffingDate"].ToString()) ? (DateTime?)null : DateTime.Parse(Result["StuffingDate"].ToString()),

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrgRRCD.Add(new DSRPostPaymentChrgRRCD
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["ChargeType"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                        });
                    }
                }
                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrgRRCD.Sum(o => o.Total);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objInvoice;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void AddEditExportRRCreditDebitModule(DSRInvoiceRRCreditDebit ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
           int BranchId, int Uid,
          string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = (ObjPostPaymentSheet.InvoiceType==null?"Tax": ObjPostPaymentSheet.InvoiceType) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceIdCRNote", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceIdCRNote});

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
              DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceRRCreditDebit", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = ReturnObj;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = ReturnObj;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        #endregion

        #region Train Summary

       


        public void AddUpdateTrainSummaryUpload(DSRExportTrainSummaryUpload Obj, string TrainSummaryUploadXML = "")
        {
            try
            {
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainSummaryId", MySqlDbType = MySqlDbType.Int32, Value = Obj.TrainSummaryUploadId });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.TrainNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(Obj.TrainDate) });
                LstParam.Add(new MySqlParameter { ParameterName = "In_PortId", MySqlDbType = MySqlDbType.Int32, Value = Obj.PortId });
                LstParam.Add(new MySqlParameter { ParameterName = "TrainSummaryUploadXML", MySqlDbType = MySqlDbType.String, Value = TrainSummaryUploadXML });
                LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("AddUpdateExportTrainSummaryUpload", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                _DBResponse.Status = 1;
                _DBResponse.Message = "";
                _DBResponse.Data = Result;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        public void CheckTrainSummaryUpload(string TrainNo, string TrainSummaryUploadXML)
        {
             
            DataSet Result = new DataSet();
            try
            {

                int RetValue = 0;
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainNo", MySqlDbType = MySqlDbType.VarChar, Value = TrainNo }); ;
                LstParam.Add(new MySqlParameter { ParameterName = "TrainSummaryUploadXML", MySqlDbType = MySqlDbType.String, Value = TrainSummaryUploadXML });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = RetValue });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("checkexporttrainsummaryupload", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                RetValue = Convert.ToInt32(DParam.Where(x => x.ParameterName == "RetValue").Select(x => x.Value).FirstOrDefault());

                List<DSRExportTrainSummaryUpload> TrainSummaryUploadList = new List<DSRExportTrainSummaryUpload>();
                foreach (DataRow dr in Result.Tables[0].Rows)
                {

                    DSRExportTrainSummaryUpload objTrainSummaryUpload = new DSRExportTrainSummaryUpload();
                    objTrainSummaryUpload.SrNo = TrainSummaryUploadList.Count + 1;
                    objTrainSummaryUpload.Wagon = Convert.ToString(dr["Wagon"]);
                    objTrainSummaryUpload.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                    objTrainSummaryUpload.SZ = Convert.ToString(dr["SZ"]);
                    objTrainSummaryUpload.Status = Convert.ToString(dr["Status"]);
                    objTrainSummaryUpload.SLine = Convert.ToString(dr["SLine"]);
                    objTrainSummaryUpload.TW = Convert.ToDecimal(dr["TW"]);
                    objTrainSummaryUpload.CW = Convert.ToDecimal(dr["CW"]);
                    objTrainSummaryUpload.GW = Convert.ToDecimal(dr["GW"]);
                    objTrainSummaryUpload.PKGS = Convert.ToInt32(dr["PKGS"]);
                    objTrainSummaryUpload.Commodity = Convert.ToString(dr["Commodity"]);
                    objTrainSummaryUpload.LineSeal = Convert.ToString(dr["LineSeal"]);
                    objTrainSummaryUpload.CustomSeal = Convert.ToString(dr["CustomSeal"]);
                    objTrainSummaryUpload.Shipper = Convert.ToString(dr["Shipper"]);
                    objTrainSummaryUpload.ForeignLiner = Convert.ToString(dr["ForeignLiner"]);
                    objTrainSummaryUpload.VesselName = Convert.ToString(dr["VesselName"]);
                    objTrainSummaryUpload.VesselNo = Convert.ToString(dr["VesselNo"]);
                    objTrainSummaryUpload.CHA = Convert.ToString(dr["CHA"]);
                    objTrainSummaryUpload.CRRSBillingParty = Convert.ToString(dr["CRRSBillingParty"]);
                    objTrainSummaryUpload.BillingParty = Convert.ToString(dr["BillingParty"]);
                    objTrainSummaryUpload.StuffingMode = Convert.ToString(dr["StuffingMode"]);
                    objTrainSummaryUpload.SBillNo = Convert.ToString(dr["SBillNo"]);
                    objTrainSummaryUpload.Date = Convert.ToDateTime(dr["Date"]);
                    objTrainSummaryUpload.Origin = Convert.ToString(dr["Origin"]);
                    objTrainSummaryUpload.POL = Convert.ToString(dr["POL"]);
                    objTrainSummaryUpload.POD = Convert.ToString(dr["POD"]);
                    objTrainSummaryUpload.DepDate = Convert.ToDateTime(dr["DepDate"]);

                    objTrainSummaryUpload.StatusValue = Convert.ToInt32(dr["StatusValue"]);

                    if (objTrainSummaryUpload.StatusValue == 0)
                    {
                        objTrainSummaryUpload.StatusDesc = "OK";
                    }
                    else if (objTrainSummaryUpload.StatusValue == 1)
                    {
                        objTrainSummaryUpload.StatusDesc = "Already Exist.";
                    }
                    else if (objTrainSummaryUpload.StatusValue == 2)
                    {
                        objTrainSummaryUpload.StatusDesc = "Cannot Save";
                    }


                    TrainSummaryUploadList.Add(objTrainSummaryUpload);
                }

                _DBResponse.Status = RetValue;
                _DBResponse.Message = "";
                _DBResponse.Data = TrainSummaryUploadList;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }

        

        public void GetAllTrainSummary()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetExportTrainSummaryList", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<DSRExportTrainSummaryUpload> TrainSummaryUploadList = new List<DSRExportTrainSummaryUpload>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        DSRExportTrainSummaryUpload objTrainSummaryUpload = new DSRExportTrainSummaryUpload();
                        objTrainSummaryUpload.TrainNo = Convert.ToString(dr["TrainNo"]);
                        objTrainSummaryUpload.TrainDate = Convert.ToString(dr["TrainDate"]);
                        objTrainSummaryUpload.PortId = Convert.ToInt32(dr["PortId"]);
                        objTrainSummaryUpload.PortName = Convert.ToString(dr["PortName"]);
                        //objTrainSummaryUpload.UploadDate = Convert.ToString(dr["UploadDate"].ToString());

                        TrainSummaryUploadList.Add(objTrainSummaryUpload);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = TrainSummaryUploadList;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }

     
        public void GetTrainSummaryDetails(int TrainSummaryUploadId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_TrainSummaryId", MySqlDbType = MySqlDbType.Int32, Value = TrainSummaryUploadId });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetExportTrainSummaryDetails", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                List<DSRExportTrainSummaryUpload> TrainSummaryUploadList = new List<DSRExportTrainSummaryUpload>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        DSRExportTrainSummaryUpload objTrainSummaryUpload = new DSRExportTrainSummaryUpload();
                        objTrainSummaryUpload.SrNo = TrainSummaryUploadList.Count + 1;
                        // objTrainSummaryUpload.Wagon_No = Convert.ToString(dr["Wagon_No"]);
                        objTrainSummaryUpload.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objTrainSummaryUpload.SZ = Convert.ToString(dr["SZ"]);
                        objTrainSummaryUpload.LineSeal = Convert.ToString(dr["LineSeal"]);
                        objTrainSummaryUpload.Commodity= Convert.ToString(dr["Commodity"]);
                        objTrainSummaryUpload.SLine = Convert.ToString(dr["SLine"]);
                        //objTrainSummaryUpload.Ct_Tare = Convert.ToDecimal(dr["Ct_Tare"]);
                        //objTrainSummaryUpload.Cargo_Wt = Convert.ToDecimal(dr["Cargo_Wt"]);
                        //objTrainSummaryUpload.Gross_Wt = Convert.ToDecimal(dr["Gross_Wt"]);
                        //objTrainSummaryUpload.Ct_Status = Convert.ToString(dr["Ct_Status"]);
                        //objTrainSummaryUpload.Destination = Convert.ToString(dr["Destination"]);
                        //objTrainSummaryUpload.Smtp_No = Convert.ToString(dr["Smtp_No"]);
                        //objTrainSummaryUpload.Received_Date = Convert.ToString(dr["Received_Date"]);
                        //objTrainSummaryUpload.Genhaz = Convert.ToString(dr["Genhaz"]);
                        objTrainSummaryUpload.StatusDesc = "";
                        TrainSummaryUploadList.Add(objTrainSummaryUpload);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = TrainSummaryUploadList;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }

        public void ListOfTrainSummary()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetExportTrainSummaryList", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<DSRExportTrainSummaryUpload> TrainSummaryUploadList = new List<DSRExportTrainSummaryUpload>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        DSRExportTrainSummaryUpload objTrainSummaryUpload = new DSRExportTrainSummaryUpload();
                        objTrainSummaryUpload.TrainSummaryUploadId = Convert.ToInt32(dr["TrainSummaryId"]);
                        objTrainSummaryUpload.TrainNo = Convert.ToString(dr["TrainNo"]);
                        objTrainSummaryUpload.TrainDate = Convert.ToString(dr["TrainDate"]);
                        objTrainSummaryUpload.PortId = Convert.ToInt32(dr["PortId"]);
                        objTrainSummaryUpload.PortName = Convert.ToString(dr["PortName"]);
                        objTrainSummaryUpload.UploadDate = Convert.ToString(dr["UploadDate"].ToString());

                        TrainSummaryUploadList.Add(objTrainSummaryUpload);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = TrainSummaryUploadList;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }

        #endregion

        public void GetContStuffingForPaymentSheet(int StuffingReqId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<DSR_ContainerStuffingPSReq> objPaySheetStuffing = new List<DSR_ContainerStuffingPSReq>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new DSR_ContainerStuffingPSReq()
                    {
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Convert.ToString(Result["CHAName"]),
                        CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                        Address = Result["Address"].ToString(),
                        State = Result["State"].ToString(),
                        StateCode = Result["StateCode"].ToString(),
                        StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
                        StuffingReqNo = Convert.ToString(Result["StuffingReqNo"]),
                        StuffingReqDate = Convert.ToString(Result["RequestDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

       /* public void GetAmenSBList()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmendSBList", CommandType.StoredProcedure);
            List<Amendment> LstSB = new List<Amendment>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new Amendment
                    {
                        ShipBillNo = Convert.ToString(Result["SBNo"]),
                        ShipBillDate = Result["SBDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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

        public void GetAllCommodityForPageAmendment(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCommodityForPage", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.CommodityForPage> LstCommodity = new List<Areas.Import.Models.CommodityForPage>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstCommodity.Add(new Areas.Import.Models.CommodityForPage
                    {
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        CommodityName = Result["CommodityName"].ToString(),
                        PartyCode = Result["CommodityAlias"].ToString(),
                        CommodityType = Result["CommodityType"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { LstCommodity, State };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }
        */

        public void AddEditExpInvoice(DSR_ExpPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
           string GREChargesXML, string GREContWiseChargXML, string GREOperationCodeWiseAmtXML, int BranchId, int Uid, string Module,int GREPaidById, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.DeliveryDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });

            LstParam.Add(new MySqlParameter { ParameterName = "GREChargesXML", MySqlDbType = MySqlDbType.Text, Value = GREChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "GREContWiseChargXML", MySqlDbType = MySqlDbType.Text, Value = GREContWiseChargXML });
            LstParam.Add(new MySqlParameter { ParameterName = "GREOperationCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GREOperationCodeWiseAmtXML });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ICDDestuffing", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.ICDDestuffing });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MoveTo", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.MoveTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.MovementType });

            LstParam.Add(new MySqlParameter { ParameterName = "in_GREPaidById", MySqlDbType = MySqlDbType.Int32, Value = GREPaidById });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PolId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PolId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditExpInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }


        public void GetContDetForPaymentSheet(int StuffingReqId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetStuffingRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<DSR_ContainerDetails> objPaySheetStuffing = new List<DSR_ContainerDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new DSR_ContainerDetails()
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        Size = Result["Size"].ToString(),
                        ArrivalDate = Convert.ToString(Result["ArrivalDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetExportPaymentSheet(string InvoiceDate,string DeliveryDate, int AppraisementId, string InvoiceType, string ExportUnder, string ContainerXML, 
           int InvoiceId, int PartyId, int PayeeId, int IntercartingApplicable, int ICDDestuffing,int distance,
           int cwcMovement,int insured, int movetoport,int ISGREPaid,int ownMovement,int PortOfLoading)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(DeliveryDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_distance", MySqlDbType = MySqlDbType.Int32, Value = distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MoveToPort", MySqlDbType = MySqlDbType.Int32, Value = movetoport });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNos", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POL", MySqlDbType = MySqlDbType.Int32, Value = PortOfLoading });
            LstParam.Add(new MySqlParameter { ParameterName = "in_insured", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IntercartingApplicable", MySqlDbType = MySqlDbType.Int32, Value = IntercartingApplicable });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ICDDestuffing", MySqlDbType = MySqlDbType.Int32, Value = ICDDestuffing });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PrivateMovement", MySqlDbType = MySqlDbType.Int32, Value = ownMovement });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ISGREPaid", MySqlDbType = MySqlDbType.Int32, Value = ISGREPaid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCMovement", MySqlDbType = MySqlDbType.Int32, Value = ownMovement });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            DSR_ExpPaymentSheet objInvoice = new DSR_ExpPaymentSheet();
            IDataReader Result = DataAccess.ExecuteDataReader("GetExportPS", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();
                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();
                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.CHAName = Result["CHAName"].ToString();
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();
                        objInvoice.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                        objInvoice.PayeeName = Convert.ToString(Result["PayeeName"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentCont.Add(new DSRExpInvoiceContainerBase
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDateTime"].ToString(),
                            //ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            //LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = "",
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                        });
                        objInvoice.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new DSRExpInvoiceChargeBase
                        {
                            ChargeId = Convert.ToInt32(Result["ChargeId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            OperationId = Result["OperationId"].ToString(),
                            HazAmt = Convert.ToDecimal(Result["HazAmt"]),
                            OdcAmt = Convert.ToDecimal(Result["OdcAmt"]),
                            HazTotal = Convert.ToDecimal(Result["HazTotal"]),
                            OdcTotal = Convert.ToDecimal(Result["OdcTotal"]),
                            HAZIGSTAmt = Result["HAZIGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["HAZIGSTAmt"]),
                            HAZCGSTAmt = Result["HAZCGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["HAZCGSTAmt"]),
                            HAZSGSTAmt = Result["HAZSGSTAmt"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["HAZSGSTAmt"])

                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstGREPostPaymentChrg.Add(new DSRExpInvoiceChargeBaseGRE
                        {
                            ChargeId = Convert.ToInt32(Result["ChargeId"]),
                            InoviceId= Convert.ToInt32(Result["InoviceId"]),
                            ShippingLineId= Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingLineName= Result["ShippingLineName"].ToString(),
                            OperationId = Result["OperationId"].ToString(),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"])
                            
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContwiseAmt.Add(new DSR_ExpContWiseAmount
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstGREContwiseAmt.Add(new DSR_ExpContWiseAmountGRE
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingLineName = Result["ShippingLineName"].ToString()
                        });
                    }
                }
                
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationContwiseAmt.Add(new DSR_ExpOperationContWiseCharge
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Result["OperationId"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            DocumentNo = Convert.ToString(Result["SBNo"]),
                            DocumentType="Export CMINV",
                            DocumentDate = Convert.ToString(Result["SBDate"]),
                            Clause = Result["Clause"].ToString(),
                            GodownId=Convert.ToInt32(Result["GodownId"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstGREOperationContwiseAmt.Add(new DSR_ExpOperationContWiseChargeGRE
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Result["OperationID"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Clause = Result["Clause"].ToString(),
                            SBNo = Result["SBNo"].ToString(),
                            SBDate = Result["SBDate"].ToString(),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            ShippingLineName = Result["ShippingLineName"].ToString()
                        });
                    }
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        objInvoice.ActualApplicable.Add(new DSRHTClauseList
                //        {
                //            ClauseName = Result["ClauseName"].ToString(),
                //            ClauseId = Result["Clause"].ToString(),


                //        });

                //        //objInvoice.ActualApplicable.Add(Result["Clause"].ToString());
                //    }
                //}
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.ActualApplicable.Add(Result["Clause"].ToString());
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.IsPartyStateInCompState = Convert.ToBoolean(Result["IsLocalGST"]);
                    }
                }
                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objInvoice;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        public void ListOfExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null,int Page=0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofexpInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<DSRListOfExpInvoice> lstExpInvoice = new List<DSRListOfExpInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExpInvoice.Add(new DSRListOfExpInvoice()
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        Module = Convert.ToString(Result["Module"]),
                        ModuleName = Convert.ToString(Result["ModuleName"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstExpInvoice;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void ListLoadMoreExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if (InvoiceDate == "") InvoiceDate = null;
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListLoadMoreExpInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<DSRListOfExpInvoice> lstExpInvoice = new List<DSRListOfExpInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExpInvoice.Add(new DSRListOfExpInvoice()
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        Module = Convert.ToString(Result["Module"]),
                        ModuleName = Convert.ToString(Result["ModuleName"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstExpInvoice;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetPaymentPartyForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyForPage", CommandType.StoredProcedure, Dparam);
            IList<Areas.Import.Models.DSR_PartyForPage> lstParty = new List<Areas.Import.Models.DSR_PartyForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new Areas.Import.Models.DSR_PartyForPage
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]).Replace("\r\n", ""),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        PartyCode = Convert.ToString(Result["PartyCode"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstParty, State };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }

        public void GetPaymentPayerForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPayerForPage", CommandType.StoredProcedure, Dparam);
            IList<Areas.Import.Models.DSR_PartyForPage> lstPayer = new List<Areas.Import.Models.DSR_PartyForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool StatePayer = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstPayer.Add(new Areas.Import.Models.DSR_PartyForPage
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]).Replace("\r\n", ""),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        PartyCode = Convert.ToString(Result["PartyCode"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        StatePayer = Convert.ToBoolean(Result["StatePayer"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstPayer, StatePayer };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }



        #region Shippingbill Amendment
        public void GetAmenSBList()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmendSBList", CommandType.StoredProcedure);
            List<DSR_Amendment> LstSB = new List<DSR_Amendment>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new DSR_Amendment
                    {
                        ShipBillNo = Convert.ToString(Result["SBNo"]),
                        ShipBillDate = Result["SBDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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

        public void GetAmenSBDetailsBySbNoDate(string SbNo, string SbDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = SbNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SBDate", MySqlDbType = MySqlDbType.Date, Size = 11, Value = Convert.ToDateTime(SbDate) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmendSBDetailsBySbNoDate", CommandType.StoredProcedure, DParam);
            List<DSR_Amendment> LstSB = new List<DSR_Amendment>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new DSR_Amendment
                    {
                        ShipBillNo = Convert.ToString(Result["SBNo"]),
                        ShipBillDate = Convert.ToString(Result["SBDate"]),
                        Exporter = Convert.ToString(Result["Exporter"]),
                        FOBValue = Convert.ToString(Result["FOB"]),
                        Pkg = Convert.ToString(Result["Package"]),
                        Weight = Convert.ToString(Result["Weight"]),
                        CCINId = Convert.ToString(Result["CCINID"]),
                        Cargo = Convert.ToString(Result["Cargo"]),
                        CommodityId = Convert.ToInt32((Result["CommodityId"]) ?? 0),
                        ExporterId = Convert.ToInt32((Result["ExporterId"]) ?? 0),
                        Area = Convert.ToString((Result["SQM"]) ?? 0),

                        IsApprove = Convert.ToInt32((Result["IsApproved"]) ?? 0),
                        Cutting = Convert.ToInt32((Result["isCutting"]) ?? 0),
                        ShortCargo = Result["ShortCargo"].ToString(),
                        CCINNO = Result["CCINNo"].ToString(),
                        SBType = Result["SBType"].ToString(),
                        ShippingLineName = Result["ShippingLineName"].ToString(),
                        PortOfLoading = Result["PortOfLoading"].ToString(),
                        PostOfDest = Result["PostOfDest"].ToString(),
                        ShippingLineId = Convert.ToInt32((Result["ShippingLineId"]) ?? 0),
                        PortOfLoadingId = Convert.ToInt32((Result["PortOfLoadingId"]) ?? 0),
                        PortofDestId = Convert.ToInt32((Result["PortofDestId"]) ?? 0),
                        ManualWT = Convert.ToString(Result["ManualWT"]),
                        MechanicalWT = Convert.ToString(Result["MechanicalWT"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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

        public void AddEditAmendment(string OldSBNoValue, string NewSBNoValue, string Date, int InvoiceId, string InvoiceNo, string InvoiceDate, string FlagMerger)
        {
            //InvoiceDate = DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            //lstParam.Add(new MySqlParameter { ParameterName = "in_AmendmentId", MySqlDbType = MySqlDbType.String, Value = AmendmentNO });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AmendmentDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(Date) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OldSBNoXML", MySqlDbType = MySqlDbType.Text, Value = OldSBNoValue });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NewSBNoXML", MySqlDbType = MySqlDbType.Text, Value = NewSBNoValue });

            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceDate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Flag", MySqlDbType = MySqlDbType.VarChar, Value = FlagMerger });


            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditAmendment", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Message = (Result == 1) ? "Amendment Entry Saved Successfully" : "Amendment Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Message = "Data with this invoice no already exists in Amendment";
                    _DBResponse.Status = Result;
                }
                else if (Result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Shipbill should be same stage  ";
                    _DBResponse.Status = 0;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
        }


        public void AddEditShipAmendment(DSR_AmendmentViewModel vm)
        {

            string OblSBDate= DateTime.ParseExact(vm.OldShipBillDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();

            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.String, Value = vm.ID });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShipBillNo", MySqlDbType = MySqlDbType.String, Value = vm.NewShipBillNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OldShipBillNo", MySqlDbType = MySqlDbType.String, Value = vm.ShipBillNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OldShipBillDate", MySqlDbType = MySqlDbType.Date, Value = OblSBDate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShipBillDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(vm.ShipbillDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(vm.Date) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_exporterId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(vm.ExporterID) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Commodityid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(vm.CargoID) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.Weight) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ManualWT", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.ManualWT) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MechanicalWT", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.MechanicalWT) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_pkg", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.Pkg) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FOB", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(vm.FOB) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SbType", MySqlDbType = MySqlDbType.Int32, Value = vm.SBType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = vm.ShippingLineId }); 
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.String, Value = vm.ShippingLineName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortLoadingId", MySqlDbType = MySqlDbType.Int32, Value = vm.PortOfLoadingId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortDestId", MySqlDbType = MySqlDbType.Int32, Value = vm.PortOfDestId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditShipBillAmendment", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Message = (Result == 1) ? "Amendment Saved Successfully" : "Amendment Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Message = "Data with this invoice no already exists in Amendment";
                    _DBResponse.Status = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "";
                    _DBResponse.Status = 0;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
        }



        public void GetAllCommodityForPageAmendment(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstCommodityForPage", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.CommodityForPage> LstCommodity = new List<Areas.Import.Models.CommodityForPage>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    LstCommodity.Add(new Areas.Import.Models.CommodityForPage
                    {
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        CommodityName = Result["CommodityName"].ToString(),
                        PartyCode = Result["CommodityAlias"].ToString(),
                        CommodityType = Result["CommodityType"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { LstCommodity, State };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }

        public void GetAmenSBDetailsByAmendNO(string AmendNO)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AmendNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = AmendNO });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmendmentDetailsByAmendNo", CommandType.StoredProcedure, DParam);
            List<DSR_AmendmentViewModel> LstSB = new List<DSR_AmendmentViewModel>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new DSR_AmendmentViewModel
                    {
                        AmendNo = Convert.ToString(Result["AmendNo"]),
                        Date = Convert.ToDateTime(Result["AmendDate"]),
                        NewShipBillNo = Convert.ToString(Result["ShipbillNo"]),
                        ShipbillDate = Convert.ToString(Result["ShipbillDate"]),
                        ShipBillNo = Convert.ToString(Result["OldShipBillNo"]),
                        OldShipBillDate = Convert.ToString(Result["OldShipBillDate"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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



        public void GetAmenDetailsByAmendNO(string AmendNO)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AmendNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = AmendNO });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmendmentDetailsByAmendNo", CommandType.StoredProcedure, DParam);
            List<DSR_Amendment> LstSB = new List<DSR_Amendment>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new DSR_Amendment
                    {
                        AmendmentNo = Convert.ToString(Result["AmendNo"]),
                        AmendmentDate = Convert.ToString(Result["AmendDate"]),
                        Exporter = Convert.ToString(Result["Exporter"]),
                        FOBValue = Convert.ToString(Result["FOB"]),
                        Pkg = Convert.ToString(Result["Pkg"]),
                        ShipBillDate = Convert.ToString(Result["SBDate"]),
                        ShipBillNo = Convert.ToString(Result["SBNo"]),
                        Weight = Convert.ToString(Result["Weight"]),
                        ManualWT = Convert.ToString(Result["ManualWT"]),
                        MechanicalWT = Convert.ToString(Result["MechanicalWT"]),
                        Cargo = Convert.ToString(Result["Cargo"]),
                        Type = Convert.ToString(Result["Type"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        Area = Result["Area"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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



        public void lstAmendDate()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_AmendNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = AmendNO });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAmenDataAll", CommandType.StoredProcedure, DParam);
            List<DSR_Amendment> LstSB = new List<DSR_Amendment>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new DSR_Amendment
                    {
                        AmendmentNo = Convert.ToString(Result["ShipbillNo"]),
                        AmendmentDate = Convert.ToString(Result["AmendDate"])


                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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
        public void GetInvoiceListForShipbillAmend()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("InvoiceListForShipbillAmend", CommandType.StoredProcedure);
            List<dynamic> LstSB = new List<dynamic>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new
                    {
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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
        public void ListForShipbillAmend()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_id", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfShipBillAmned", CommandType.StoredProcedure, Dparam);
            List<DSR_AmendmentViewModel> LstSB = new List<DSR_AmendmentViewModel>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB.Add(new DSR_AmendmentViewModel
                    {
                        ID = Convert.ToInt32(Result["id"]),
                        AmendNo = Convert.ToString(Result["AmendNo"]),
                        Date = Convert.ToDateTime(Result["AmendDate"]),
                        NewShipBillNo = Convert.ToString(Result["ShipbillNo"]),
                        ShipbillDate = Convert.ToString(Result["ShipbillDate"]),
                        ShipBillNo = Convert.ToString(Result["OldShipBillNo"]),
                        OldShipBillDate = Convert.ToString(Result["AmendDate"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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
        public void GetShipbillAmendDet(int id)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_id", MySqlDbType = MySqlDbType.Int32, Value = id });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfShipBillAmned", CommandType.StoredProcedure, Dparam);
           DSR_AmendmentViewModel LstSB = new DSR_AmendmentViewModel();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSB = (new DSR_AmendmentViewModel
                    {
                        ID = Convert.ToInt32(Result["id"]),
                        AmendNo = Convert.ToString(Result["AmendNo"]),
                        Date = Convert.ToDateTime(Result["AmendDate"]),
                        NewShipBillNo = Convert.ToString(Result["ShipbillNo"]),
                        ShipbillDate = Convert.ToString(Result["ShipbillDate"]),
                        ExporterID = Convert.ToString(Result["ExporterID"]),
                        ExporterName = Result["ExporterName"].ToString(),
                        CargoID = Convert.ToInt32(Result["CommodityID"]),
                        Cargo = Result["CommodityName"].ToString(),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        Pkg = Convert.ToInt32(Result["Pkg"]),
                        FOB = Convert.ToString(Result["FOB"]),
                        ShipBillNo = Convert.ToString(Result["OldShipBillNo"]),
                        OldShipBillDate = Convert.ToString(Result["AmendDate"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        ShippingLineName = Result["ShippingLineName"].ToString(),
                        PortOfLoading = Result["PortOfLoading"].ToString(),
                        PortOfDest = Result["PortOfDest"].ToString(),
                        SBTypeName = Result["SBTypeName"].ToString(),
                        CCINNo = Result["CCINNo"].ToString(),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSB;
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

        public void ListOfShippingLineName()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfShippingLine", CommandType.StoredProcedure);
            IList<DSR_ShippingLine> lstShipping = new List<DSR_ShippingLine>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstShipping.Add(new DSR_ShippingLine
                    {
                        Id = Convert.ToInt32(result["EximTraderId"]),
                        ShippingLineName = result["EximTraderName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstShipping;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                result.Dispose();
                result.Close();
            }
        }
        #endregion

        

        #region RCT Sealing Payment Sheet

        public void GetCHAExporterForRCTSealing(string CHAExporter)
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAExporter", MySqlDbType = MySqlDbType.VarChar, Value = CHAExporter });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCHAExporterForRCTSealing", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            IList<PaymentPartyName> objCHAExporterName = new List<PaymentPartyName>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objCHAExporterName.Add(new PaymentPartyName()
                    {
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),                        
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objCHAExporterName;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        
        public void GetRCTSealingInvoice(string InvoiceDate, string InvoiceType, string ContainerXML, string ShipbillXML,int PartyId,int PayeeId,string ExportUnder, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd") });            
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ShipbillXML", MySqlDbType = MySqlDbType.Text, Value = ShipbillXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });            
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32,  Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            DSRInvoiceRCTSealing objInvoice = new DSRInvoiceRCTSealing();
            IDataReader Result = DataAccess.ExecuteDataReader("GetRCTSealingPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {

                _DBResponse = new DatabaseResponse();

                while (Result.Read())
                {
                    objInvoice.CompanyId= Convert.ToInt32(Result["CompanyId"]);
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();
                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();
                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();
                    objInvoice.PartyStateCode = Result["PartyStateCode"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.CHAName = Result["CHAName"].ToString();
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();                       
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();

                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPrePaymentCont.Add(new DSRPreInvoiceContainerRCTSealing
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            PortId = Convert.ToInt32(Result["PortId"]),
                            PortName = Result["PortName"].ToString(),                           
                        });
                        
                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPrePaymentShipbill.Add(new DSRPreInvoiceShipbillRCTSealing
                        {
                            ShipbillNo = Result["ShipbillNo"].ToString(),
                            ShipbillDate = Result["ShipbillDate"].ToString(),
                            NoOfPkg = Convert.ToInt32(Result["NoOfPkg"]),
                            GrossWT = Convert.ToDecimal(Result["GrossWT"]),
                            FOBValue = Convert.ToDecimal(Result["FOBValue"])
                        });                       
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new DSRPostPaymentChrgRCTSealing
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                        });
                    }

                }
               
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrgBreakup.Add(new DSRRCTSealingPostPaymentChargebreakupdate
                        {
                            ChargeId = Convert.ToInt32(Result["OperationId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            CFSCode = Result["CFSCode"].ToString(),
                            Startdate = Result["StartDate"].ToString(),
                            EndDate = Result["EndDate"].ToString()
                        });
                    }

                }
               
                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objInvoice;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        public void AddRCTSealingInvoice(DSRInvoiceRCTSealing ObjPostPaymentSheet, string ContainerXML, string ShipbillXML, string ChargesXML, string ChargesBreakupXML, int BranchId, int Uid, string Module)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });           
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });            
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });            
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });            
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.ExporterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExporterName });            
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });            
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ShipbillXML", MySqlDbType = MySqlDbType.Text, Value = ShipbillXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });           
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddInvoiceRCTSealing", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        public void GetRCTShipbillDetById(int Id)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = Id });           
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetRCTShipbillDetById", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            DSRPostInvoiceShipbillRCTSealing objRCTSealing = new DSRPostInvoiceShipbillRCTSealing();
            try
            {

                while (Result.Read())
                {
                    Status = 1;
                    objRCTSealing.ShipbillNo = Convert.ToString(Result["ShipbillNo"]);
                    objRCTSealing.ShipbillDate = Convert.ToString(Result["ShipbillDate"]);                    
                    objRCTSealing.NoOfPkg = Convert.ToInt32(Result["NoOfPkg"]);
                    objRCTSealing.GrossWT = Convert.ToDecimal(Result["GrossWt"]);
                    objRCTSealing.FOBValue = Convert.ToDecimal(Result["FobValue"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Data = objRCTSealing;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        #endregion

        #region Empty Container Ground Rent
        public void GetPaymentPartyExpEmpty(string PartyCode,int Page)
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyExportEmp", CommandType.StoredProcedure,Dparam);
            _DBResponse = new DatabaseResponse();
            IList<DSR_PaymentParty> objPaymentPartyName = new List<DSR_PaymentParty>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new DSR_PaymentParty()
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        PartyAlias = Convert.ToString(Result["PartyAlias"])
                    });
                }
                bool Sts = false;
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Sts = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data =new { lstParty=objPaymentPartyName, PartyState= Sts };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetShippingLinePartyExpEmpty(string PartyCode, int Page)
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSLAForExpEmp", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            IList<DSR_PaymentParty> objPaymentPartyName = new List<DSR_PaymentParty>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new DSR_PaymentParty()
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        PartyAlias=Convert.ToString(Result["PartyAlias"])
                    });
                }
                bool Sts = false;
                if(Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Sts = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstShippingLine = objPaymentPartyName, State = Sts };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetContainerListForExpEmpty(int Sla)
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_SLA", MySqlDbType = MySqlDbType.Int32,  Value = Sla });
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContListForExpEmpInv", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            IList<DSR_PaymentSheetContainerExpEmp> objPaymentPartyName = new List<DSR_PaymentSheetContainerExpEmp>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentPartyName.Add(new DSR_PaymentSheetContainerExpEmp()
                    {
                        CFSCode = Convert.ToString(Result["EntryNo"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        InDateEcy = Convert.ToString(Result["InDate"]),
                        Size = Convert.ToString(Result["Size"]),
                        Selected = false,
                        Days = Convert.ToInt32(Result["Days"]),
                        OutDateEcy = Convert.ToString(Result["OutDate"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentPartyName;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetGREPaymentSheet(string InvoiceDate, string DeliveryDate, int AppraisementId, string InvoiceType, string ContainerXML,
           int InvoiceId, int PartyId, int PayeeId,string ExportUnder)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(DeliveryDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            DSR_ExpPaymentSheet objInvoice = new DSR_ExpPaymentSheet();
            IDataReader Result = DataAccess.ExecuteDataReader("GetExportGREPS", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    objInvoice.ROAddress = Result["ROAddress"].ToString();
                    objInvoice.CompanyName = Result["CompanyName"].ToString();
                    objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = Result["PhoneNo"].ToString();
                    objInvoice.FaxNumber = Result["FaxNumber"].ToString();
                    objInvoice.EmailAddress = Result["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
                    objInvoice.StateCode = Result["StateCode"].ToString();
                    objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
                    objInvoice.GstIn = Result["GstIn"].ToString();
                    objInvoice.Pan = Result["Pan"].ToString();
                    objInvoice.CompGST = Result["GstIn"].ToString();
                    objInvoice.CompPAN = Result["Pan"].ToString();
                    objInvoice.CompStateCode = Result["StateCode"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
                        objInvoice.CHAName = Result["CHAName"].ToString();
                        objInvoice.PartyName = Result["CHAName"].ToString();
                        objInvoice.PartyGST = Result["GSTNo"].ToString();
                        objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        objInvoice.RequestNo = Result["AppraisementNo"].ToString();
                        objInvoice.RequestDate = Result["AppraisementDate"].ToString();
                        objInvoice.PartyAddress = Result["Address"].ToString();
                        objInvoice.PartyStateCode = Result["StateCode"].ToString();
                        objInvoice.PayeeId = Convert.ToInt32(Result["PayeeId"]);
                        objInvoice.PayeeName = Convert.ToString(Result["PayeeName"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentCont.Add(new DSRExpInvoiceContainerBase
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDateTime"].ToString(),
                            //ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            Reefer = Convert.ToInt32(Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"]),
                            Insured = Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            //LCLFCL = Result["LCLFCL"].ToString(),
                            CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                        });
                        objInvoice.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new DSRExpInvoiceChargeBase
                        {
                            ChargeId = Convert.ToInt32(Result["ChargeId"]),
                            Clause = Result["Clause"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            ChargeName = Result["ChargeName"].ToString(),
                            SACCode = Result["SACCode"].ToString(),
                            Quantity = Convert.ToInt32(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            Discount = Convert.ToDecimal(Result["Discount"]),
                            Taxable = Convert.ToDecimal(Result["Taxable"]),
                            IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
                            SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
                            CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
                            Total = Convert.ToDecimal(Result["Total"]),
                            OperationId = Result["OperationId"].ToString()
                        });
                    }
                }
                
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContwiseAmt.Add(new DSR_ExpContWiseAmount
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            EntryFee = Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"])
                        });
                    }
                }
                
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationContwiseAmt.Add(new DSR_ExpOperationContWiseCharge
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Result["OperationId"].ToString(),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                            DocumentNo = Convert.ToString(Result["SBNo"]),
                            DocumentType = "Export CMINV",
                            DocumentDate = Convert.ToString(Result["SBDate"]),
                            Clause = Result["Clause"].ToString(),
                        });
                    }
                }
                
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        //objInvoice.ActualApplicable.Add(new DSRHTClauseList
                        //{
                        //    ClauseName = "",
                        //    ClauseId = Result["Clause"].ToString(),
                           

                        //});

                        objInvoice.ActualApplicable.Add(Result["Clause"].ToString());
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.IsPartyStateInCompState = Convert.ToBoolean(Result["IsLocalGST"]);
                    }
                }
                objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objInvoice;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void AddEditExpGREInvoice(DSR_ExpPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
           string GREChargesXML, string GREContWiseChargXML, string GREOperationCodeWiseAmtXML, int BranchId, int Uid, string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.DeliveryDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = null });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "GREChargesXML", MySqlDbType = MySqlDbType.Text, Value = GREChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "GREContWiseChargXML", MySqlDbType = MySqlDbType.Text, Value = GREContWiseChargXML });
            LstParam.Add(new MySqlParameter { ParameterName = "GREOperationCodeWiseAmtXML", MySqlDbType = MySqlDbType.Text, Value = GREOperationCodeWiseAmtXML });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ICDDestuffing", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.ICDDestuffing });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditEXPGREInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully.";

                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
            }
        }
        #endregion


        #region Loaded Container Request For Update

        public void GetLoadedContainerRequestNoForUpdate()
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();           
            IDataParameter[] Dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLoadedRequstNoForUpdate", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            IList<DSR_LoadContReq> lstRequestNo = new List<DSR_LoadContReq>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstRequestNo.Add(new DSR_LoadContReq()
                    {
                        LoadContReqNo= Convert.ToString(Result["LoadContReqNo"]),
                        LoadContReqId= Convert.ToInt32(Result["LoadContReqId"])
                        
                    });
                }
              
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstRequestNo;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetLoadContDetailsForUpdate(int LoadContReqId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = LoadContReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippbillNo", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLoadCntReqForUpdate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSR_LoadContReq objDet = new DSR_LoadContReq();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDet.LoadContReqId = Convert.ToInt32(Result["LoadContReqId"]);
                    objDet.LoadContReqNo = Result["LoadContReqNo"].ToString();
                    objDet.CHAName = Result["CHAName"].ToString();
                    objDet.LoadContReqDate = Result["LoadContReqDate"].ToString();
                    objDet.PickupValidity = Result["ValidityDate"].ToString();
                    objDet.Remarks = Result["Remarks"].ToString();
                    objDet.CHAId = Convert.ToInt32(Result["CHAId"]);
                    objDet.IsMoveToRCT = Convert.ToBoolean(Result["IsMoveToRCT"]);
                    objDet.MoveTo = Convert.ToString(Result["MoveTo"]);
                    objDet.PODID = Convert.ToInt32(Result["POD"]);
                    objDet.POD = Convert.ToString(Result["PortName"]);
                    objDet.ForeignLiner = Convert.ToString(Result["ForeignLiner"]);
                    objDet.FactoryAddress = Convert.ToString(Result["FactoryAddress"]);
                    objDet.Voyage = Convert.ToString(Result["Voyage"]);
                    objDet.Vessel = Convert.ToString(Result["Vessel"]);
                    objDet.PikUpFrom = Convert.ToString(Result["PickUpFrom"]);
                    objDet.Movement = Convert.ToString(Result["Movement"]);
                    objDet.Distance = Convert.ToDecimal(Result["Distance"]);
                    objDet.Loadmovement = Convert.ToBoolean(Result["Loadmovement"]);
                    objDet.FinalDestinationLocationID = Convert.ToInt32(Result["FinalDestinationLocationID"]);
                    objDet.FinalDestinationLocation = Convert.ToString(Result["FinalDestinationLocation"]);
                    objDet.CustomExaminationType = Convert.ToString(Result["ExamType"]);

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objDet.lstContDtl.Add(new DSRLoadContReqDtl
                        {
                            LoadContReqDetlId = Convert.ToInt32(Result["LoadContReqDetlId"]),
                            ExporterId = Convert.ToInt32(Result["ExporterId"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            IsReefer = Convert.ToBoolean(Result["Reefer"]),
                            IsInsured = Convert.ToBoolean(Result["IsInsured"]),
                            ShippingBillNo = Result["ShippingBillNo"].ToString(),
                            ShippingBillDate = Result["ShippingBillDate"].ToString(),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]),
                            FobValue = Convert.ToDecimal(Result["FobValue"]),
                            ExporterName = Result["ExporterName"].ToString(),
                            ShippingLineName = (Result["ShippingLineName"] == null ? "" : Result["ShippingLineName"]).ToString(),
                            CommodityName = Result["CommodityName"].ToString(),
                            EntryNo = Result["EntryNo"].ToString(),
                            IsWashing = Convert.ToBoolean(Result["IsWashing"]),
                            IsWeighment = Convert.ToBoolean(Result["IsWeighment"]),
                            IsReworking = Convert.ToBoolean(Result["IsReworking"]),
                            IsInternalShifting = Convert.ToBoolean(Result["IsInternalShifting"]),
                            IsLiftOnEmpty = Convert.ToBoolean(Result["IsLiftOnEmpty"]),
                            IsLiftOffEmpty = Convert.ToBoolean(Result["IsLiftOffEmpty"]),
                            RMSValue = Convert.ToInt32(Result["RMSValue"]),
                            ReeferHrs = Convert.ToInt32(Result["ReeferHrs"]),
                            BothWayEmpty = Convert.ToBoolean(Result["Bothwayempty"]),
                            ShippingSeal = Result["ShippingSeal"].ToString(),
                            CustomSeal = Result["CustomSeal"].ToString(),
                            EquipmentSealType = Result["EquipmentSealType"].ToString(),
                            EquipmentStatus = Result["EquipmentStatus"].ToString(),
                            EquipmentQUC = Result["EquipmentQUC"].ToString(),
                            PackageType = Result["PackageType"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            PackUQCCode = Result["PackUQCCode"].ToString(),
                            PackUQCDescription = Result["PackUQCDescription"].ToString(),
                            IsSEZ = Convert.ToBoolean(Result["SEZ"])




                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objDet;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void UpdateLoadContDetails(DSR_LoadContReq objLoadContReq, string XML)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.LoadContReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objLoadContReq.LoadContReqDate).ToString("yyyy-MM-dd HH:mm:ss") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PickupValidity", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objLoadContReq.PickupValidity).ToString("yyyy-MM-dd") });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objLoadContReq.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Movement", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objLoadContReq.Movement });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PickUpFrom", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = objLoadContReq.PikUpFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MoveTo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = objLoadContReq.MoveTo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal, Value = objLoadContReq.Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FactoryAddress", MySqlDbType = MySqlDbType.VarChar, Size = 150, Value = objLoadContReq.FactoryAddress });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = objLoadContReq.Vessel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ForeignLiner", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = objLoadContReq.ForeignLiner });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Voyage", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = objLoadContReq.Voyage });
            LstParam.Add(new MySqlParameter { ParameterName = "in_POD", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.PODID });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocationID", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.FinalDestinationLocationID });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FinalDestinationLocation", MySqlDbType = MySqlDbType.VarChar, Value = objLoadContReq.FinalDestinationLocation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = XML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsMoveToRCT", MySqlDbType = MySqlDbType.Bit, Value = objLoadContReq.IsMoveToRCT });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadedExam", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.Loadmovement });
           // LstParam.Add(new MySqlParameter { ParameterName = "in_Bothwayempty", MySqlDbType = MySqlDbType.Int32, Value = objLoadContReq.BothWayEmpty }); 
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("UpdateLoadCntReq", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = ((Result == 1) ? "Loaded Container Request Saved Successfully" : "Loaded Container Request Updated Successfully");
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Edit Loaded Container Request Details As It Exist In Another Page";
                    _DBResponse.Data = null;
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

        public void GetContainerListLoadContReqUpdate(string CONTCBT = "")
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CONTCBT", MySqlDbType = MySqlDbType.String, Value = CONTCBT });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerListLoadContReqUpdate", CommandType.StoredProcedure, DParam);
            List<DSRLoadCont> lstContainerInfo = new List<DSRLoadCont>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    lstContainerInfo.Add(new DSRLoadCont
                    {
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Size = Result["Size"].ToString(),
                        EntryNo = Result["EntryNo"].ToString(),
                        CargoType = Result["CargoType"].ToString(),
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstContainerInfo;

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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        #endregion

        #region Loaded Container Stuffing Approval
        public void GetLoadedContainerStuffingForApproval(int LoadContReqId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = LoadContReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLoadContainerRequestForApproval", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<DSR_ContainerStuffingApproval> objPaySheetStuffing = new List<DSR_ContainerStuffingApproval>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new DSR_ContainerStuffingApproval()
                    {

                        StuffingReqId = Convert.ToInt32(Result["LoadContReqId"]),
                        StuffingReqNo = Convert.ToString(Result["LoadContReqNo"]),
                        StuffingReqDate = Convert.ToString(Result["LoadContReqDate"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        Size = Convert.ToString(Result["Size"]),
                        ExporterId = Convert.ToInt32(Result["ExporterId"]),
                        ExporterName = Convert.ToString(Result["ExporterName"]),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        public void AddEditLoadContainerStuffingApproval(PortOfCall objPortOfCall, int Uid)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_ApprovalId", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.ApprovalId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ApprovalDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objPortOfCall.ApprovalDate).ToString("yyyy-MM-dd HH:mm:ss") });
            lstparam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.StuffingReqId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_LoadContReqNo", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.StuffingReqNo });
            lstparam.Add(new MySqlParameter { ParameterName = "in_PortOfCallName", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.PortOfCallName });
            lstparam.Add(new MySqlParameter { ParameterName = "in_PortOfCallCode", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.PortOfCallCode });
            lstparam.Add(new MySqlParameter { ParameterName = "in_NextPortOfCallName", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.NextPortOfCallName });
            lstparam.Add(new MySqlParameter { ParameterName = "in_NextPortOfCallCode", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.NextPortOfCallCode });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ModeOfTransport", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.ModeOfTransport });
            lstparam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("AddEditLoadContainerStuffingApproval", CommandType.StoredProcedure, dpram);
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Loaded Container Stuffing Approved Successfully" : "Loaded Container Stuffing Approval Updated Successfully";

                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Can't Update CIM ASR File Already Send.";
                }
                else if (result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Can't Update CIM ASR Acknowledgement Received.";
                }
                else if (result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Can't Update Stuffing Amendment Done.";
                }
                else if (result == 6)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Can't Update Loaded Container Stuffing Approval Done.";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
            }
        }

        public void ListofLoadContainerStuffingApproval(int Page, string SearchValue = "")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofLoadContainerStuffingApproval", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PortOfCall> LstStuffingApproval = new List<PortOfCall>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffingApproval.Add(new PortOfCall
                    {
                        ApprovalId = Convert.ToInt32(Result["ApprovalId"]),
                        ApprovalDate = Result["ApprovalDate"].ToString(),
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        StuffingReqDate = Result["RequestDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffingApproval;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetLoadContainerStuffingApprovalById(int ApprovalId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ApprovalId", MySqlDbType = MySqlDbType.Int32, Value = ApprovalId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("ViewLoadContainerStuffingApproval", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            PortOfCall objDestuffing = new PortOfCall();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDestuffing.ApprovalId = Convert.ToInt32(Result["ApprovalId"]);
                    objDestuffing.ApprovalDate = Convert.ToString(Result["ApprovalDate"]);
                    objDestuffing.StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]);
                    objDestuffing.StuffingReqNo = Convert.ToString(Result["StuffingReqNo"]);
                    objDestuffing.StuffingReqDate = Convert.ToString(Result["RequestDate"]);
                    objDestuffing.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objDestuffing.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    objDestuffing.Size = Convert.ToString(Result["Size"].ToString());
                    objDestuffing.PortOfCallName = Convert.ToString(Result["PortOfCallName"]);
                    objDestuffing.PortOfCallCode = Convert.ToString(Result["PortOfCallCode"]);
                    objDestuffing.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                    objDestuffing.ExporterName = Convert.ToString(Result["ExporterName"]);
                    objDestuffing.NextPortOfCallName = Convert.ToString(Result["NextPortOfCallName"]);
                    objDestuffing.NextPortOfCallCode = Convert.ToString(Result["NextPortOfCallCode"]);
                    objDestuffing.ModeOfTransportName = Convert.ToString(Result["ModeOfTransport"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Data = objDestuffing;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }




        #endregion

        #region Get Load Container CIM-ASR Details

        public void GetLoadedContainerCIMASRDetails(int ContainerStuffingId, string MsgType)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("ICES_GetLoadContSCMTRCIMASRDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Result;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
                //Result.Close();
            }
        }

        public void GetLoadContCIMASRDetailsUpdateStatus(int ContainerStuffingId)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("UpdateLoadContCIMASRStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Result;


            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
                //Result.Close();
            }
        }
        #endregion
        #region ACTUAL ARRIVAL DATE AND TIME 

        public void GetContainerNoForActualArrival(string ContainerBoxSearch, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchText", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ContainerBoxSearch });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerNoForActualArrival", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<ContainerNoForActualArrival> objContainerNoForActualArrival = new List<ContainerNoForActualArrival>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    objContainerNoForActualArrival.Add(new ContainerNoForActualArrival()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        GatePassNo = Convert.ToString(Result["GatePassNo"]),
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { ContainerList = objContainerNoForActualArrival, State }; ;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }
        public void AddEditActualArrivalDatetime(DSR_ActualArrivalDatetime objActualArrivalDatetime)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = objActualArrivalDatetime.Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = objActualArrivalDatetime.ContainerNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = objActualArrivalDatetime.CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassNo", MySqlDbType = MySqlDbType.VarChar, Value = objActualArrivalDatetime.GatePassNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDateTime", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objActualArrivalDatetime.ArrivalDateTime).ToString("yyyy-MM-dd HH:mm:ss") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditActualArrivalDatetime", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Arrival Datetime Saved Successfully" : "Arrival Datetime Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "CFSCode already exist";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
        }
        public void GetListOfArrivalDatetime(int Uid, int Id)
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Value = Id });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfArrivalDatetime", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<DSR_ActualArrivalDatetime> objArrivalDatetimeList = new List<DSR_ActualArrivalDatetime>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objArrivalDatetimeList.Add(new DSR_ActualArrivalDatetime()
                    {
                        Id = Convert.ToInt32(Result["Id"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        GatePassNo = Convert.ToString(Result["GatePassNo"]),
                        ArrivalDateTime = Convert.ToString(Result["ArrivalDateTime"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objArrivalDatetimeList;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }

        }

        #endregion

        #region Get Loaded CIM-SF Details

        public void GetLoadedCIMSFDetails(int ContainerStuffingId, string MsgType)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("ICES_GetLoadedSCMTRStuffingDetailsV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();




            try
            {

                int count = Result.Tables.Count;
                if (count == 1)
                {
                    if (Convert.ToInt32(Result.Tables[0].Rows[0]["Result"]) == 1)
                    {
                        _DBResponse.Status = 2;
                        _DBResponse.Message = "CIM SF Message Already Send.";
                        _DBResponse.Data = Result;
                    }
                    else
                    {
                        _DBResponse.Status = 3;
                        _DBResponse.Message = "CIM SF Acknowledgement Received Successfully Please Do Amendment";
                        _DBResponse.Data = Result;
                    }

                }
                else
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Result;
                }



            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
                //Result.Close();
            }
        }

        public void GetLoadedCIMSFDetailsUpdateStatus(int ContainerStuffingId)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerStuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ContainerStuffingId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("UpdateLoadedCIMSFStatus", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Result;


            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
                //Result.Close();
            }
        }
        #endregion

        #region Loaded Container Stuffing SF
        public void GetLoadedContainerStuffingForSF(int LoadContReqId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = LoadContReqId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLoadContainerRequestForSF", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<DSR_ContainerStuffingApproval> objPaySheetStuffing = new List<DSR_ContainerStuffingApproval>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new DSR_ContainerStuffingApproval()
                    {

                        StuffingReqId = Convert.ToInt32(Result["LoadContReqId"]),
                        StuffingReqNo = Convert.ToString(Result["LoadContReqNo"]),
                        StuffingReqDate = Convert.ToString(Result["LoadContReqDate"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        Size = Convert.ToString(Result["Size"]),
                        ExporterId = Convert.ToInt32(Result["ExporterId"]),
                        ExporterName = Convert.ToString(Result["ExporterName"]),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaySheetStuffing;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        public void AddEditLoadContainerStuffingSF(DSR_LoadContSF objPortOfCall, int Uid)
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_ApprovalId", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.ApprovalId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = objPortOfCall.StuffingReqId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_LoadContReqNo", MySqlDbType = MySqlDbType.VarChar, Value = objPortOfCall.StuffingReqNo });
            lstparam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstparam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int result = DA.ExecuteNonQuery("AddEditLoadContainerStuffingSF", CommandType.StoredProcedure, dpram);
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = "Loaded Container For SF Saved Successfully";

                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Can't Update CIM ASR File Already Send.";
                }
                else if (result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Can't Update CIM ASR Acknowledgement Received.";
                }
                else if (result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Can't Update Stuffing Amendment Done.";
                }
                else if (result == 6)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Can't Update Loaded Container Stuffing Approval Done.";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
            }
        }

        public void ListofLoadContainerStuffingSF(int Page, string SearchValue = "")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchValue", MySqlDbType = MySqlDbType.VarChar, Value = SearchValue });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofLoadContainerStuffingSF", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_LoadContSF> LstStuffingApproval = new List<DSR_LoadContSF>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstStuffingApproval.Add(new DSR_LoadContSF
                    {
                        ApprovalId = Convert.ToInt32(Result["ApprovalId"]),
                        StuffingReqNo = Result["LoadReqNo"].ToString(),
                        StuffingReqDate = Result["RequestDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstStuffingApproval;
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetLoadContainerStuffingSFById(int ApprovalId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ApprovalId", MySqlDbType = MySqlDbType.Int32, Value = ApprovalId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("ViewLoadContainerStuffingSF", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            DSR_LoadContSF objDestuffing = new DSR_LoadContSF();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDestuffing.ApprovalId = Convert.ToInt32(Result["ApprovalId"]);
                    objDestuffing.StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]);
                    objDestuffing.StuffingReqNo = Convert.ToString(Result["StuffingReqNo"]);
                    objDestuffing.StuffingReqDate = Convert.ToString(Result["RequestDate"]);
                    objDestuffing.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objDestuffing.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    objDestuffing.Size = Convert.ToString(Result["Size"].ToString());
                    objDestuffing.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                    objDestuffing.ExporterName = Convert.ToString(Result["ExporterName"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Data = objDestuffing;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }




        #endregion

        #region Get CIM-AT Details

        public void GetATDetails(string CFSCode, string MsgType)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = Convert.ToString(CFSCode) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("GetScmtrATDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Result;

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                //Result.Dispose();
                //Result.Close();
            }
        }

        //public void GetCIMARDetailsUpdateStatus(int HeaderId)
        //{
        //    int Status = 0;
        //    DataSet Result = new DataSet();
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_GateExitID", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = HeaderId });
        //    //LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
        //    // LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    Result = DataAccess.ExecuteDataSet("GetCIMARDetailsUpdateStatus", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {

        //        _DBResponse.Status = 1;
        //        _DBResponse.Message = "Success";
        //        _DBResponse.Data = Result;


        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Status = 0;
        //        _DBResponse.Message = "Error";
        //        _DBResponse.Data = null;
        //    }
        //    finally
        //    {
        //        //Result.Dispose();
        //        //Result.Close();
        //    }
        //}
        #endregion

    }
}
