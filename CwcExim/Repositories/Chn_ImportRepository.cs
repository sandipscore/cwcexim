using CwcExim.Areas.Export.Models;
using CwcExim.Areas.Import.Controllers;
using CwcExim.Areas.Import.Models;
using CwcExim.Areas.Report.Models;
using CwcExim.DAL;
using CwcExim.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Globalization;
using EinvoiceLibrary;
namespace CwcExim.Repositories
{
    public class Chn_ImportRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }



        #region Custom Appraisement Application
        public void ListOfShippingLine()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetShippingLine", CommandType.StoredProcedure, DParam);
            IList<Areas.Import.Models.ShippingLine> lstShippingLine = new List<Areas.Import.Models.ShippingLine>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstShippingLine.Add(new Areas.Import.Models.ShippingLine
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
        public void ListOfCHA()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfCHA", CommandType.StoredProcedure, dparam);
            IList<Areas.Bond.Models.CHA> lstCHA = new List<Areas.Bond.Models.CHA>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new Areas.Bond.Models.CHA
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
        public void GetContnrDetForCustomAppraise(string CFSCode, string LineNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_LineNo", MySqlDbType = MySqlDbType.VarChar, Value = LineNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCntrDetForImpCstmAppraise", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            ChnCustomAppraisement ObjAppraisement = new ChnCustomAppraisement();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppraisement.ContainerNo = (Result["ContainerNo"] == null ? "" : Result["ContainerNo"]).ToString();
                    ObjAppraisement.CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString();
                    ObjAppraisement.Size = (Result["Size"] == null ? "" : Result["Size"]).ToString();
                    ObjAppraisement.ContainerLoadType = (Result["ContainerLoadType"] == null ? "" : Result["ContainerLoadType"]).ToString();
                    ObjAppraisement.LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString();
                    ObjAppraisement.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjAppraisement.Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString();
                    ObjAppraisement.CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]);
                    ObjAppraisement.CHAName = (Result["CHA"] == null ? "" : Result["CHA"]).ToString();
                    ObjAppraisement.Vessel = (Result["VesselName"] == null ? "" : Result["VesselName"]).ToString();
                    ObjAppraisement.Voyage = (Result["VoyageNo"] == null ? "" : Result["VoyageNo"]).ToString();
                    ObjAppraisement.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                    ObjAppraisement.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjAppraisement.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjAppraisement.Rotation = (Result["RotationNo"] == null ? "" : Result["RotationNo"]).ToString();
                    ObjAppraisement.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjAppraisement.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                    // ObjAppraisement.ContainerType = Convert.ToInt32(Result["ContainerType"] == DBNull.Value ? 0 : Result["ContainerType"]);
                    ObjAppraisement.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    //  ObjAppraisement.Reefer = Convert.ToInt32(Result["Reefer"] == DBNull.Value ? 0 : Result["Reefer"]);
                    // ObjAppraisement.RMS = Convert.ToInt32(Result["RMS"] == DBNull.Value ? 0 : Result["RMS"]);
                    ObjAppraisement.GateEntryDate = Convert.ToDateTime(Result["EntryDateTime"]).ToString("MM/dd/yyyy");


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjAppraisement;
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
        public void GetAllCustomAppraisementApp()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCustomAppraisementApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ChnCustomAppraisement> LstAppraisement = new List<ChnCustomAppraisement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new ChnCustomAppraisement
                    {
                        AppraisementNo = Result["AppraisementNo"].ToString(),
                        AppraisementDate = Result["AppraisementDate"].ToString(),
                        CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstAppraisement;
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
        public void GetCustomAppraisement(int CustomAppraisementId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCustomAppraisementApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            ChnCustomAppraisement ObjAppraisement = new ChnCustomAppraisement();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppraisement.CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]);
                    ObjAppraisement.AppraisementNo = Result["AppraisementNo"].ToString();
                    ObjAppraisement.AppraisementDate = Result["AppraisementDate"].ToString();
                    ObjAppraisement.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    ObjAppraisement.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjAppraisement.CHAName = Result["CHA"].ToString();
                    ObjAppraisement.ShippingLine = Result["ShippingLine"].ToString();
                    ObjAppraisement.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjAppraisement.Voyage = Result["Voyage"].ToString();
                    ObjAppraisement.Rotation = Result["Rotation"].ToString();
                    ObjAppraisement.Fob = Convert.ToDecimal(Result["Fob"]);
                    ObjAppraisement.GrossDuty = Convert.ToDecimal(Result["GrossDuty"]);
                    ObjAppraisement.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    ObjAppraisement.IsDO = Convert.ToInt32(Result["IsDO"]);
                    ObjAppraisement.ApplicationForApp = Convert.ToInt32(Result["Application_For"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.LstAppraisement.Add(new ChnCustomAppraisementDtl
                        {
                            CustomAppraisementDtlId = Convert.ToInt32(Result["CustomAppraisementDtlId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                            LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),

                            OBLNo = (Result["OBLNo"] == null ? "" : Result["OBLNo"]).ToString(),
                            OBLDate = (Result["OBLDate"] == null ? "" : Result["OBLDate"]).ToString(),
                            // Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            //  Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString(),
                            CHANameId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]),
                            ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]),
                            Importer = Result["Importer"].ToString(),
                            CHA = Result["CHA"].ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            WithoutDOSealNo = (Result["WithoutDOSealNo"] == null ? "" : Result["WithoutDOSealNo"]).ToString(),
                            ContainerType = Convert.ToInt32(Result["ContainerType"] == DBNull.Value ? 0 : Result["ContainerType"]),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            RMSValue = Convert.ToInt32(Result["RMS"]),

                            //  Reefer = Convert.ToInt32(Result["Reefer"] == DBNull.Value ? 0 : Result["Reefer"]),
                            //  RMS = Convert.ToInt32(Result["RMS"] == DBNull.Value ? 0 : Result["RMS"]),
                            //  HeavyScrap = Convert.ToInt32(Result["HeavyScrap"] == DBNull.Value ? 0 : Result["HeavyScrap"]),
                            //  AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"] == DBNull.Value ? 0 : Result["AppraisementPerct"]),
                            //  IsInsured = Convert.ToInt32(Result["IsInsured"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.LstCustomAppraisementOrdDtl.Add(new ChnCustomAppraisementOrdDtl
                        {
                            OrderId = Convert.ToInt32(Result["OrderId"]),
                            CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]),
                            IssuedBy = (Result["DoIssueBy"] == null ? "" : Result["DoIssueBy"]).ToString(),
                            DeliveredTo = (Result["CargsDelivered"] == null ? "" : Result["CargsDelivered"]).ToString(),
                            ValidType = (Result["ValidType"] == null ? "" : Result["ValidType"]).ToString(),
                            ValidDate = (Result["ValidDate"] == null ? "" : Result["ValidDate"]).ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjAppraisement;
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
        public void AddEditCustomAppraisement(ChnCustomAppraisement ObjAppraisement, string AppraisementXML, string CAOrdXML)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjAppraisement.CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_application_For", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.ApplicationForApp });

            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjAppraisement.AppraisementDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Vessel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Voyage", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Voyage });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Rotation", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Rotation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fob", MySqlDbType = MySqlDbType.Decimal, Value = ObjAppraisement.Fob });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossDuty", MySqlDbType = MySqlDbType.Decimal, Value = ObjAppraisement.GrossDuty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsDO", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.IsDO });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementXML", MySqlDbType = MySqlDbType.Text, Value = AppraisementXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CAOrdXML", MySqlDbType = MySqlDbType.Text, Value = CAOrdXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditCustomAppraisementApp", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjAppraisement.CustomAppraisementId == 0 ? "Custom Appraisement Application Details Saved Successfully" : "Custom Appraisement Application Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Update Custom Appraisement Application Details As It Already Exists In Another Page";
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
        public void DelCustomAppraisement(int CustomAppraisementId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelCustomAppraisementApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Custom Appraisement Application Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Custom Appraisement Application Details As It Exist In Another Page";
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

        public void ListOfShippingLinePartyCode(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLineForJO", CommandType.StoredProcedure, Dparam);
            IList<Areas.Import.Models.ShippingLineForPage> lstShippingLine = new List<Areas.Import.Models.ShippingLineForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstShippingLine.Add(new Areas.Import.Models.ShippingLineForPage
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
        public void GetContnrNoForCustomAppraise()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCntrNoForImpCstmAppraise", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ChnCustomAppraisementDtl> LstAppraisement = new List<ChnCustomAppraisementDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new ChnCustomAppraisementDtl
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstAppraisement;
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



        public void ListOfChaForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChaForPage", CommandType.StoredProcedure, Dparam);
            IList<CHAForPage> lstCHA = new List<CHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
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
                    _DBResponse.Data = new { lstCHA, State };
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

        public void ListOfChaForPageforSingleClick(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChaForSinglePage", CommandType.StoredProcedure, Dparam);
            IList<CHAForPage> lstCHA = new List<CHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
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
                    _DBResponse.Data = new { lstCHA, State };
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
        //Access Right information
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
            IList<Chn_Menu> lstMenu = new List<Chn_Menu>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (result.Read())
                {

                    lstMenu.Add(new Chn_Menu
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
        public void ListOfImporterForMerge()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            // lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfImporterMergePage", CommandType.StoredProcedure);
            IList<Importer> lstImporterName = new List<Importer>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstImporterName.Add(new Importer
                    {
                        ImporterId = Convert.ToInt32(result["ImporterId"]),
                        ImporterName = result["ImporterName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstImporterName;
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



        public void ListOfChaForMergeApp(string PartyCode)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            //   lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChaForMergePage", CommandType.StoredProcedure, Dparam);
            IList<CHNCHAForPage> lstCHA = new List<CHNCHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CHNCHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString(),
                        BillToParty = Convert.ToBoolean(Result["BillToParty"]),
                        InsuredFrmdate = Result["InsuredDateFrm"].ToString(),
                        InsuredTodate = Result["InsuredDateTo"].ToString(),
                        IsInsured = Convert.ToBoolean(Result["IsInsured"])
                    });
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        State = Convert.ToBoolean(Result["State"]);
                //    }
                //}
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCHA;
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

        public void ListOfChaForSingleMergeApp(string PartyCode)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            //   lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChaForSingleMergePage", CommandType.StoredProcedure, Dparam);
            IList<CHAForPage> lstCHA = new List<CHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new CHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
                        PartyCode = Result["PartyCode"].ToString()
                    });
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        State = Convert.ToBoolean(Result["State"]);
                //    }
                //}
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCHA;
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

        #region Custom Appraisement Approval
        public void NewCustomeAppraisement(int Skip)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Skip", MySqlDbType = MySqlDbType.Int32, Value = Skip });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfNewAppfrCustApp", CommandType.StoredProcedure, dpram);
            IList<Chn_ListOfCustAppraismentAppr> lstApproval = new List<Chn_ListOfCustAppraismentAppr>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            bool State = false;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstApproval.Add(new Chn_ListOfCustAppraismentAppr
                    {
                        CstmAppraiseAppId = Convert.ToInt32(result["CustomAppraisementId"]),
                        AppraisementNo = result["AppraisementNo"].ToString(),
                        BOENo = (result["BOENo"] == null ? "" : result["BOENo"]).ToString(),
                        CHA = (result["CHA"] == null ? "" : result["CHA"]).ToString(),
                        Importer = (result["Importer"] == null ? "" : result["Importer"]).ToString(),
                    });
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        State = Convert.ToBoolean(result["STATE"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = new { lstApproval = lstApproval, State = State };
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
        public void ApprovalHoldCustomAppraisement(int Skip)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Skip", MySqlDbType = MySqlDbType.Int32, Value = Skip });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfAppHoldCustApp", CommandType.StoredProcedure, dpram);
            IList<Chn_ListOfCustAppraismentAppr> lstApproval = new List<Chn_ListOfCustAppraismentAppr>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            bool State = false;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstApproval.Add(new Chn_ListOfCustAppraismentAppr
                    {
                        CstmAppraiseAppId = Convert.ToInt32(result["CustomAppraisementId"]),
                        AppraisementNo = result["AppraisementNo"].ToString(),
                        BOENo = (result["BOENo"] == null ? "" : result["BOENo"]).ToString(),
                        CHA = (result["CHA"] == null ? "" : result["CHA"]).ToString(),
                        Importer = (result["Importer"] == null ? "" : result["Importer"]).ToString(),
                    });
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        State = Convert.ToBoolean(result["STATE"]);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = new { lstApproval, State };
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

        public void GetCstmAppraiseApplication(int CstmAppraiseAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CstmAppraiseAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCstmAppraiseApplication", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Chn_Custom_AppraiseApproval ObjAppraisement = new Chn_Custom_AppraiseApproval();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppraisement.CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]);
                    // ObjAppraisement.CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]);
                    ObjAppraisement.AppraisementNo = Result["AppraisementNo"].ToString();
                    ObjAppraisement.AppraisementDate = Result["ApplicationDate"].ToString();
                    ObjAppraisement.ShippingLine = Result["ShippingLine"].ToString();
                    ObjAppraisement.Vessel = Convert.ToString(Result["Vessel"] == null ? "" : Result["Vessel"]);
                    ObjAppraisement.Voyage = Convert.ToString(Result["Voyage"] == null ? "" : Result["Voyage"]);
                    ObjAppraisement.Rotation = Result["Rotation"].ToString();
                    ObjAppraisement.Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]);
                    ObjAppraisement.GrossDuty = Convert.ToDecimal(Result["GrossDuty"] == DBNull.Value ? 0 : Result["GrossDuty"]);

                    ObjAppraisement.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);

                    ObjAppraisement.IsApproved = Convert.ToInt32(Result["IsApproved"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.LstAppraisementDtl.Add(new ChnCustomAppraisementDtl
                        {
                            CustomAppraisementDtlId = Convert.ToInt32(Result["CustomAppraisementDtlId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                            LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            //  Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            // Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString(),
                            // CHANameId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]),
                            //ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]),
                            Importer = Result["Importer"].ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            // WithoutDOSealNo = (Result["WithoutDOSealNo"] == null ? "" : Result["WithoutDOSealNo"]).ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjAppraisement;
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
        public void UpdateCustomApproval(int CstmAppraiseAppId, int IsApproved, int Uid)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseAppId", MySqlDbType = MySqlDbType.Int32, Value = CstmAppraiseAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Approved", MySqlDbType = MySqlDbType.Int32, Value = IsApproved });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            // LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] dpram = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("UpdateCustomApp", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Custom Appraisement Approval Details Saved Successfully";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Cannot Update Custom Appraisement Approval Details As It Already Exist In Another Page";
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
        #endregion





        #region Job Order By Road
        public void AddEditJobOrderByRoad(Chn_JobOrderByRoad objJO, int BranchId, int CreatedBy, string FormOneDetailXML = null)
        {
            string GeneratedClientId = "0";
            string Status = "0";
            string ReturnObj = "";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Value = objJO.FormOneId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneDate", MySqlDbType = MySqlDbType.Date, Value = objJO.FormOneDate.ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objJO.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneDetailXML", MySqlDbType = MySqlDbType.Text, Value = FormOneDetailXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CONTCBT", MySqlDbType = MySqlDbType.Text, Value = objJO.CONTCBT });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.Text, Value = objJO.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportBy", MySqlDbType = MySqlDbType.Text, Value = objJO.TransportBy });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = CreatedBy });
            LstParam.Add(new MySqlParameter { ParameterName = "@RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "@GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "" });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("AddEditJobOrderByRoad", CommandType.StoredProcedure, DParam, out Status, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "JobOrder By Road Saved Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "JobOrder By Road Updated Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Edit Form-1 Details As It Already Exists In Another Page";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = ReturnObj;
                    _DBResponse.Data = null;
                }
                else if (Result == -1)
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
        public void GetJobOrderByRoadList()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetJobOrderByRoad", CommandType.StoredProcedure, DParam);
            IList<Chn_ImportJobOrderByRoadList> lstFormOne = new List<Chn_ImportJobOrderByRoadList>();
            int Status = 0;

            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new Chn_ImportJobOrderByRoadList
                    {
                        FormOneId = Convert.ToInt32(result["FormOneId"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        FormOneDate = Convert.ToString(result["FormOneDate"]),
                        ShippingLineName = Convert.ToString(result["EximTraderName"]),
                        CONTCBT = Convert.ToString(result["CONTCBT"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstFormOne;
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
        public void GetJobOrderByRoadList(string ContainerNo)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo.TrimStart() });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("SearchListJobOrderByRoad", CommandType.StoredProcedure, DParam);
            IList<Chn_ImportJobOrderByRoadList> lstFormOne = new List<Chn_ImportJobOrderByRoadList>();
            int Status = 0;

            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new Chn_ImportJobOrderByRoadList
                    {
                        FormOneId = Convert.ToInt32(result["FormOneId"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        FormOneDate = Convert.ToString(result["FormOneDate"]),
                        ShippingLineName = Convert.ToString(result["EximTraderName"]),
                        CONTCBT = Convert.ToString(result["CONTCBT"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstFormOne;
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
        public void GetJobOrderByRoadId(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetJobOrderByRoad", CommandType.StoredProcedure, DParam);
            Chn_JobOrderByRoad lstFormOne = new Chn_JobOrderByRoad();
            int Status = 0;

            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.FormOneId = Convert.ToInt32(result["FormOneId"]);
                    lstFormOne.FormOneNo = Convert.ToString(result["FormOneNo"]);
                    lstFormOne.FormOneDate = Convert.ToDateTime(result["FormOneDate"]);
                    lstFormOne.ShippingLineId = Convert.ToInt32(result["ShippingLineId"]);
                    lstFormOne.ShippingLineName = Convert.ToString(result["EximTraderName"]);
                    lstFormOne.TransportBy = Convert.ToString(result["TransportBy"]);
                    lstFormOne.CONTCBT = Convert.ToString(result["CONTCBT"]);
                    //  lstFormOne.ForeignLiner= Convert.ToString(result["ForeignLiner"]);
                    //   lstFormOne.VesselName = Convert.ToString(result["VesselName"]);
                    //  lstFormOne.VesselNo = Convert.ToString(result["VesselNo"]);
                    lstFormOne.Remarks = Convert.ToString(result["Remarks"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstFormOne;
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
        public void GetJobOrderByRoadByOnEditMode(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetJobOrderByRoad", CommandType.StoredProcedure, DParam);
            Chn_JobOrderByRoad objFormOne = new Chn_JobOrderByRoad();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objFormOne.FormOneId = Convert.ToInt32(result["FormOneId"]);
                    objFormOne.FormOneNo = Convert.ToString(result["FormOneNo"]);
                    objFormOne.FormOneDate = Convert.ToDateTime(result["FormOneDate"]);
                    objFormOne.ShippingLineId = Convert.ToInt32(result["ShippingLineId"]);
                    objFormOne.ShippingLineName = Convert.ToString(result["EximTraderName"]);
                    objFormOne.CONTCBT = Convert.ToString(result["CONTCBT"]);
                    objFormOne.Remarks = Convert.ToString(result["Remarks"]);
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objFormOne.lstFormOneDetail.Add(new Chn_ImportJobOrderByRoadDtl()
                        {
                            FormOneDetailID = Convert.ToInt32(result["FormOneDetailId"]),
                            ContainerNo = Convert.ToString(result["ContainerNo"]),
                            ContainerSize = Convert.ToString(result["ContainerSize"]),
                            LCLFCL = Convert.ToString(result["LCLFCL"]),
                            ShippingLineId = Convert.ToInt32(result["ShippingLineId"]),
                            ShippingLineName = Convert.ToString(result["EximTraderName"]),
                            ForeignLiner = Convert.ToString(result["ForeignLiner"]),
                            VesselName = Convert.ToString(result["VesselName"]),
                            VesselNo = Convert.ToString(result["VesselNo"]),
                            IsODC=result["IsODC"].ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objFormOne.lstFormOneDetail;
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
        public void DeleteJobOrderByRoad(int FormOneId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", Direction = ParameterDirection.Output, MySqlDbType = MySqlDbType.Int32, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            int Result = DataAccess.ExecuteNonQuery("DeleteFormOne", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Job Order By Road Data Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Job Order By Road Data Does Not Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete Job Order By Road Details As It Already Exists In Another Page";
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
        public void JobOrderPrintDetails(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("JobOrderPrintDetails", CommandType.StoredProcedure, DParam);
            List<dynamic> lstFormOne = new List<dynamic>();
            int Status = 0;

            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new
                    {
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        FormOneDate = Convert.ToString(result["FormOneDate"]),
                        TransportBy = Convert.ToString(result["TransportBy"]),
                        ShippingLineName = Convert.ToString(result["ShippingLine"]),
                        CONTCBT = Convert.ToString(result["CONTCBT"]),
                        ContainerNo = result["ContainerNo"].ToString(),
                        ContainerSize = result["ContainerSize"].ToString(),
                        LCLFCL = result["LCLFCL"].ToString(),
                        ShippingLine = result["ShippingLine"].ToString(),
                        ForeignLiner = result["ForeignLiner"].ToString(),
                        VesselName = result["VesselName"].ToString(),
                        VesselNo = result["VesselNo"].ToString(),
                        Remarks = result["Remarks"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstFormOne;
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

        public void ListOfImporterForm()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "Importer" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTrader", CommandType.StoredProcedure, DParam);
            IList<Importer> lstImporter = new List<Importer>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstImporter.Add(new Importer
                    {
                        ImporterId = Convert.ToInt32(result["EximTraderId"]),
                        ImporterName = result["NameAddress"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstImporter;
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

        #region Train Summary

        public void AddUpdateTrainSummaryUpload(TrainSummaryUpload Obj, string TrainSummaryUploadXML = "")
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
                int Result = DataAccess.ExecuteNonQuery("AddUpdateTrainSummaryUpload", CommandType.StoredProcedure, DParam);
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
                Result = DataAccess.ExecuteDataSet("CheckTrainSummaryUpload", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                RetValue = Convert.ToInt32(DParam.Where(x => x.ParameterName == "RetValue").Select(x => x.Value).FirstOrDefault());

                List<TrainSummaryUpload> TrainSummaryUploadList = new List<TrainSummaryUpload>();
                foreach (DataRow dr in Result.Tables[0].Rows)
                {

                    TrainSummaryUpload objTrainSummaryUpload = new TrainSummaryUpload();
                    objTrainSummaryUpload.SrNo = TrainSummaryUploadList.Count + 1;
                    objTrainSummaryUpload.Wagon_No = Convert.ToString(dr["Wagon_No"]);
                    objTrainSummaryUpload.Container_No = Convert.ToString(dr["Container_No"]);
                    objTrainSummaryUpload.CT_Size = Convert.ToString(dr["CT_Size"]);
                    objTrainSummaryUpload.Line_Seal_No = Convert.ToString(dr["Line_Seal_No"]);
                    objTrainSummaryUpload.Cont_Commodity = Convert.ToString(dr["Cont_Commodity"]);
                    objTrainSummaryUpload.S_Line = Convert.ToString(dr["S_Line"]);
                    objTrainSummaryUpload.Foreign_Liner = Convert.ToString(dr["Foreign_Liner"]);
                    objTrainSummaryUpload.Vessel_Name = Convert.ToString(dr["Vessel_Name"]);
                    objTrainSummaryUpload.Vessel_No = Convert.ToString(dr["Vessel_No"]);
                    objTrainSummaryUpload.Ct_Tare = Convert.ToDecimal(dr["Ct_Tare"]);
                    objTrainSummaryUpload.Cargo_Wt = Convert.ToDecimal(dr["Cargo_Wt"]);
                    objTrainSummaryUpload.Gross_Wt = Convert.ToDecimal(dr["Gross_Wt"]);
                    objTrainSummaryUpload.Ct_Status = Convert.ToString(dr["Ct_Status"]);
                    objTrainSummaryUpload.Destination = Convert.ToString(dr["Destination"]);
                    objTrainSummaryUpload.Smtp_No = Convert.ToString(dr["Smtp_No"]);
                    objTrainSummaryUpload.Received_Date = Convert.ToString(dr["Received_Date"]);
                    objTrainSummaryUpload.Genhaz = Convert.ToString(dr["Genhaz"]);
                    objTrainSummaryUpload.Status = Convert.ToInt32(dr["Status"]);

                    if (objTrainSummaryUpload.Status == 0)
                    {
                        objTrainSummaryUpload.StatusDesc = "OK";
                    }
                    else if (objTrainSummaryUpload.Status == 1)
                    {
                        objTrainSummaryUpload.StatusDesc = "Already Exist.";
                    }
                    else if (objTrainSummaryUpload.Status == 2)
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





        public void GetAllOBLPort()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllOBLPort", CommandType.StoredProcedure);
            List<Port> LstPort = new List<Port>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPort.Add(new Port
                    {
                        PortName = Result["PortName"].ToString(),
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



        public void GetAllTrainSummary()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetTrainSummaryList", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<TrainSummaryUpload> TrainSummaryUploadList = new List<TrainSummaryUpload>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        TrainSummaryUpload objTrainSummaryUpload = new TrainSummaryUpload();
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

        public void GetOBLContainer(String OBLNo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLNo", MySqlDbType = MySqlDbType.VarChar, Value = OBLNo });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLContainer", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                ChnCustomAppraisement objOBLEntry = new ChnCustomAppraisement();
                //List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        ChnCustomAppraisementOBLCont objOBLCont = new ChnCustomAppraisementOBLCont();
                        objOBLCont.CFSCode = Convert.ToString(dr["CFSCode"]);
                        objOBLCont.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objOBLEntry.lstCustomAppraisementCont.Add(objOBLCont);
                    }
                }
                if (Status == 1)
                {
                    //if (objOBLEntry.OblEntryDetailsList.Count > 0)
                    //{
                    //    objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(objOBLEntry.OblEntryDetailsList);
                    //}
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objOBLEntry.lstCustomAppraisementCont;
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

        public void GetBOEDetail(String BOENo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "in_BOE_NO", MySqlDbType = MySqlDbType.VarChar, Value = BOENo });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetBOEDetail", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                ChnCustomAppraisementBOECont objOBLCont = new ChnCustomAppraisementBOECont();
                if (Result != null && Result.Tables.Count > 0)
                {

                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        objOBLCont.BOENo = Convert.ToString(dr["BOE_NO"]);
                        objOBLCont.BOEDate = Convert.ToString(dr["BOE_Date"]);
                        objOBLCont.CIFValue = Convert.ToDecimal(dr["CIF_VALUE"]);
                        objOBLCont.Duty = Convert.ToDecimal(dr["DUTY"]);
                    }
                }
                if (Status == 1)
                {

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objOBLCont;
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

        public void GetContainerOBL(String CFSCode)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetContainerOBL", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                ChnCustomAppraisement objOBLEntry = new ChnCustomAppraisement();
                //List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        ChnCustomAppraisementOBLCont objOBLCont = new ChnCustomAppraisementOBLCont();
                        objOBLCont.OBLNo = Convert.ToString(dr["OBLNo"]);
                        objOBLCont.OBLDate = Convert.ToString(dr["OBLDate"]);
                        objOBLCont.NoOfPackages = Convert.ToInt32(dr["NO_PKG"]);
                        objOBLCont.GrossWeight = Convert.ToDecimal(dr["GR_WT"]);
                        objOBLCont.CIFValue = Convert.ToDecimal(dr["CIFValue"]);
                        objOBLCont.ImporterId = Convert.ToInt32(dr["ImporterId"]);
                        objOBLCont.Importer = Convert.ToString(dr["Importer"]);
                        objOBLCont.ShippingLineId = Convert.ToInt32(dr["ShippingLineId"]);
                        objOBLCont.ShippingLine = Convert.ToString(dr["ShippingLine"]);
                        objOBLCont.CargoDescription = Convert.ToString(dr["CargoDescription"]).Replace(",", "_c_").Replace("<br/>", "_br_");
                        objOBLEntry.lstCustomAppraisementCont.Add(objOBLCont);
                    }
                }
                if (Status == 1)
                {
                    //if (objOBLEntry.OblEntryDetailsList.Count > 0)
                    //{
                    //    objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(objOBLEntry.OblEntryDetailsList);
                    //}
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objOBLEntry.lstCustomAppraisementCont;
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
        //public void GetBOEDetail(String BOENo)
        //{
        //    DataSet Result = new DataSet();
        //    int Status = 0;
        //    try
        //    {
        //        //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        //        IDataParameter[] DParam = { };
        //        List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //        DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //        LstParam.Add(new MySqlParameter { ParameterName = "in_BOE_NO", MySqlDbType = MySqlDbType.VarChar, Value = BOENo });
        //        DParam = LstParam.ToArray();
        //        Result = DataAccess.ExecuteDataSet("GetBOEDetail", CommandType.StoredProcedure, DParam);
        //        _DBResponse = new DatabaseResponse();

        //        PPGCustomAppraisementBOECont objOBLCont = new PPGCustomAppraisementBOECont();
        //        if (Result != null && Result.Tables.Count > 0)
        //        {

        //            foreach (DataRow dr in Result.Tables[0].Rows)
        //            {
        //                Status = 1;
        //                objOBLCont.BOENo = Convert.ToString(dr["BOE_NO"]);
        //                objOBLCont.BOEDate = Convert.ToString(dr["BOE_Date"]);
        //                objOBLCont.CIFValue = Convert.ToDecimal(dr["CIF_VALUE"]);
        //                objOBLCont.Duty = Convert.ToDecimal(dr["DUTY"]);
        //            }
        //        }
        //        if (Status == 1)
        //        {

        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = objOBLCont;
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
        //    }
        //}

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
                Result = DataAccess.ExecuteDataSet("GetTrainSummaryDetails", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                List<TrainSummaryUpload> TrainSummaryUploadList = new List<TrainSummaryUpload>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        TrainSummaryUpload objTrainSummaryUpload = new TrainSummaryUpload();
                        objTrainSummaryUpload.SrNo = TrainSummaryUploadList.Count + 1;
                        // objTrainSummaryUpload.Wagon_No = Convert.ToString(dr["Wagon_No"]);
                        objTrainSummaryUpload.Container_No = Convert.ToString(dr["Container_No"]);
                        objTrainSummaryUpload.CT_Size = Convert.ToString(dr["CT_Size"]);
                        objTrainSummaryUpload.Line_Seal_No = Convert.ToString(dr["Line_Seal_No"]);
                        objTrainSummaryUpload.Cont_Commodity = Convert.ToString(dr["Cont_Commodity"]);
                        objTrainSummaryUpload.S_Line = Convert.ToString(dr["S_Line"]);
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
                Result = DataAccess.ExecuteDataSet("GetTrainSummaryList", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<TrainSummaryUpload> TrainSummaryUploadList = new List<TrainSummaryUpload>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        TrainSummaryUpload objTrainSummaryUpload = new TrainSummaryUpload();
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


        //#region Yard Payment Sheet
        //public void GetAppraismentRequestForPaymentSheet(int StuffingReqId = 0)
        //{
        //    int Status = 0;
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetAppraismentRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    IList<CHN_ContainerStuffingPSReq> objPaySheetStuffing = new List<CHN_ContainerStuffingPSReq>();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            objPaySheetStuffing.Add(new CHN_ContainerStuffingPSReq()
        //            {
        //                CHAId = Convert.ToInt32(Result["CHAId"]),
        //                CHAName = Convert.ToString(Result["CHAName"]),
        //                CHAGSTNo = Convert.ToString(Result["GSTNo"]),
        //                Address = Result["Address"].ToString(),
        //                State = Result["State"].ToString(),
        //                StateCode = Result["StateCode"].ToString(),
        //                StuffingReqId = Convert.ToInt32(Result["StuffingReqId"]),
        //                StuffingReqNo = Convert.ToString(Result["StuffingReqNo"]),
        //                StuffingReqDate = Convert.ToString(Result["RequestDate"])
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = objPaySheetStuffing;
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
        //public void GetContDetForPaymentSheet(int StuffingReqId)
        //{
        //    int Status = 0;
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = StuffingReqId });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetAppraismentRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    IList<CHN_ContainerDetails> objPaySheetStuffing = new List<CHN_ContainerDetails>();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            objPaySheetStuffing.Add(new CHN_ContainerDetails()
        //            {
        //                ContainerNo = Result["ContainerNo"].ToString(),
        //                CFSCode = Result["CFSCode"].ToString(),
        //                Size = Result["Size"].ToString(),
        //                ArrivalDate = Convert.ToString(Result["ArrivalDate"])
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = objPaySheetStuffing;
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
        //public void GetYardPaymentSheet(string InvoiceDate, int AppraisementId, string InvoiceType, string ContainerXML,
        //    int InvoiceId, int PartyId, int PayeeId)
        //{
        //    int Status = 0;
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Value = 0 });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_isdirect", MySqlDbType = MySqlDbType.Int32, Value = 0 });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    _DBResponse = new DatabaseResponse();
        //    CHN_ExpPaymentSheet objInvoice = new CHN_ExpPaymentSheet();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetYardPaymentSheet", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            objInvoice.ROAddress = Result["ROAddress"].ToString();
        //            objInvoice.CompanyName = Result["CompanyName"].ToString();
        //            objInvoice.CompanyShortName = Result["CompanyShortName"].ToString();
        //            objInvoice.CompanyAddress = Result["CompanyAddress"].ToString();
        //            objInvoice.PhoneNo = Result["PhoneNo"].ToString();
        //            objInvoice.FaxNumber = Result["FaxNumber"].ToString();
        //            objInvoice.EmailAddress = Result["EmailAddress"].ToString();
        //            objInvoice.StateId = Convert.ToInt32(Result["StateId"]);
        //            objInvoice.StateCode = Result["StateCode"].ToString();
        //            objInvoice.CityId = Convert.ToInt32(Result["CityId"]);
        //            objInvoice.GstIn = Result["GstIn"].ToString();
        //            objInvoice.Pan = Result["Pan"].ToString();
        //            objInvoice.CompGST = Result["GstIn"].ToString();
        //            objInvoice.CompPAN = Result["Pan"].ToString();
        //            objInvoice.CompStateCode = Result["StateCode"].ToString();
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objInvoice.PartyId = Convert.ToInt32(Result["CHAId"]);
        //                objInvoice.CHAName = Result["CHAName"].ToString();
        //                objInvoice.PartyName = Result["CHAName"].ToString();
        //                objInvoice.PartyGST = Result["GSTNo"].ToString();
        //                objInvoice.RequestId = Convert.ToInt32(Result["CustomAppraisementId"]);
        //                objInvoice.RequestNo = Result["AppraisementNo"].ToString();
        //                objInvoice.RequestDate = Result["AppraisementDate"].ToString();
        //                objInvoice.PartyAddress = Result["Address"].ToString();
        //                objInvoice.PartyStateCode = Result["StateCode"].ToString();
        //                objInvoice.PayeeId = Convert.ToInt32(Result["PayeeId"]);
        //                objInvoice.PayeeName = Convert.ToString(Result["PayeeName"]);
        //            }
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objInvoice.lstPostPaymentCont.Add(new ChnExpInvoiceContainerBase
        //                {
        //                    ContainerNo = Result["ContainerNo"].ToString(),
        //                    CFSCode = Result["CFSCode"].ToString(),
        //                    ArrivalDate = Result["ArrivalDateTime"].ToString(),
        //                    //ArrivalTime = Result["ArrivalTime"].ToString(),
        //                    CargoType = Convert.ToInt32(Result["CargoType"]),
        //                    BOENo = Result["BOENo"].ToString(),
        //                    BOEDate = Result["BOEDate"].ToString(),
        //                    GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
        //                    WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
        //                    NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
        //                    CIFValue = Convert.ToDecimal(Result["CIFValue"]),
        //                    Duty = Convert.ToDecimal(Result["Duty"]),
        //                    Reefer = Convert.ToInt32(Result["Reefer"]),
        //                    RMS = Convert.ToInt32(Result["RMS"]),
        //                    Insured = Convert.ToInt32(Result["Insured"]),
        //                    Size = Result["Size"].ToString(),
        //                    SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
        //                    //LCLFCL = Result["LCLFCL"].ToString(),
        //                    CartingDate = Result["CartingDate"].ToString(),
        //                    DestuffingDate = "",
        //                    StuffingDate = Result["StuffingDate"].ToString(),
        //                    ShippingLineName = Result["ShippingLineName"].ToString(),
        //                    CHAName = Result["CHAName"].ToString(),
        //                    ImporterExporter = Result["ImporterExporter"].ToString(),
        //                });
        //                objInvoice.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
        //            }
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objInvoice.lstPostPaymentChrg.Add(new ChnExpInvoiceChargeBase
        //                {
        //                    ChargeId = Convert.ToInt32(Result["ChargeId"]),
        //                    Clause = Result["Clause"].ToString(),
        //                    ChargeType = Result["ChargeType"].ToString(),
        //                    ChargeName = Result["ChargeName"].ToString(),
        //                    SACCode = Result["SACCode"].ToString(),
        //                    Quantity = Convert.ToInt32(Result["Quantity"]),
        //                    Rate = Convert.ToDecimal(Result["Rate"]),
        //                    Amount = Convert.ToDecimal(Result["Amount"]),
        //                    Discount = Convert.ToDecimal(Result["Discount"]),
        //                    Taxable = Convert.ToDecimal(Result["Taxable"]),
        //                    IGSTPer = Convert.ToDecimal(Result["IGSTPer"]),
        //                    IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"]),
        //                    SGSTPer = Convert.ToDecimal(Result["SGSTPer"]),
        //                    SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"]),
        //                    CGSTPer = Convert.ToDecimal(Result["CGSTPer"]),
        //                    CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"]),
        //                    Total = Convert.ToDecimal(Result["Total"]),
        //                    OperationId = Result["OperationId"].ToString()
        //                });
        //            }
        //        }

        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objInvoice.lstContwiseAmt.Add(new CHN_ExpContWiseAmount
        //                {
        //                    CFSCode = Result["CFSCode"].ToString(),
        //                    EntryFee = Convert.ToDecimal(Result["EntryFee"]),
        //                    CstmRevenue = Convert.ToDecimal(Result["CstmRevenue"]),
        //                    GrEmpty = Convert.ToDecimal(Result["GrEmpty"]),
        //                    GrLoaded = Convert.ToDecimal(Result["GrLoaded"]),
        //                    ReeferCharge = Convert.ToDecimal(Result["ReeferCharge"]),
        //                    StorageCharge = Convert.ToDecimal(Result["StorageCharge"]),
        //                    InsuranceCharge = Convert.ToDecimal(Result["InsuranceCharge"]),
        //                    PortCharge = Convert.ToDecimal(Result["PortCharge"]),
        //                    WeighmentCharge = Convert.ToDecimal(Result["WeighmentCharge"])
        //                });
        //            }
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objInvoice.lstOperationContwiseAmt.Add(new CHN_ExpOperationContWiseCharge
        //                {
        //                    CFSCode = Result["CFSCode"].ToString(),
        //                    ContainerNo = Result["ContainerNo"].ToString(),
        //                    Size = Result["Size"].ToString(),
        //                    OperationId = Result["OperationId"].ToString(),
        //                    ChargeType = Result["ChargeType"].ToString(),
        //                    Quantity = Convert.ToDecimal(Result["Quantity"]),
        //                    Rate = Convert.ToDecimal(Result["Rate"]),
        //                    Amount = Convert.ToDecimal(Result["Amount"]),
        //                    DocumentNo = Convert.ToString(Result["SBNo"]),
        //                    DocumentDate = Convert.ToString(Result["SBDate"]),
        //                    Clause = Result["Clause"].ToString(),
        //                });
        //            }
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objInvoice.ActualApplicable.Add(Result["Clause"].ToString());
        //            }
        //        }

        //        objInvoice.TotalAmt = objInvoice.lstPostPaymentChrg.Sum(o => o.Total);

        //        _DBResponse.Status = 1;
        //        _DBResponse.Message = "Success";
        //        _DBResponse.Data = objInvoice;
        //    }
        //    catch (Exception ex)
        //    {
        //        _DBResponse.Status = 0;
        //        _DBResponse.Message = "No Data";
        //        _DBResponse.Data = null;
        //    }
        //    finally
        //    {
        //        Result.Close();
        //        Result.Dispose();
        //    }
        //}
        //public void ListOfExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        //{
        //    int Status = 0;
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
        //    LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
        //    LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
        //    IDataParameter[] DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("ListofexpInvoice", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    IList<CHNListOfExpInvoice> lstExpInvoice = new List<CHNListOfExpInvoice>();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            lstExpInvoice.Add(new CHNListOfExpInvoice()
        //            {
        //                InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
        //                InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
        //                InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
        //                PartyName = Convert.ToString(Result["PartyName"]),
        //                Module = Convert.ToString(Result["Module"]),
        //                ModuleName = Convert.ToString(Result["ModuleName"])
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = lstExpInvoice;
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
        //public void AddEditInvoiceYard(CHN_ExpPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
        //    int BranchId, int Uid, string Module, string CargoXML = "")
        //{
        //    string GeneratedClientId = "0";
        //    string ReturnObj = "";
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.InvoiceDate) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.RequestId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.RequestNo });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.RequestDate) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGST });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddress", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddress });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PartyState", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyState });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_TotalAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_TotalDiscount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalDiscount });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalAmt });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_TotalCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalCGST });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSGST });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_TotalIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalIGST });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTotal });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTotal });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDS });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDS });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CWCTDSPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_HTTDSPerc", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HTTDSPer });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_TDS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDS });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_TDSCol", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TDSCol });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.AllTotal });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.RoundUp });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.InvoiceAmt });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ShippingLineName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalSpaceOccupied });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalValueOfCargo });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.DestuffingDate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.StuffingDate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CartingDate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
        //    LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });

        //    LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
        //    LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
        //    IDataParameter[] DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
        //    int Result = DA.ExecuteNonQuery("AddEditInvoiceYard", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        if (Result == 1)
        //        {
        //            _DBResponse.Data = GeneratedClientId;
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Payment Invoice Saved Successfully.";

        //        }
        //        else if (Result == 2)
        //        {
        //            _DBResponse.Data = GeneratedClientId;
        //            _DBResponse.Status = 2;
        //            _DBResponse.Message = "Payment Invoice Updated Successfully";
        //        }
        //        else if (Result == 3)
        //        {
        //            _DBResponse.Data = null;
        //            _DBResponse.Status = 0;
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
        //public void GetPaymentParty()
        //{
        //    int Status = 0;
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyExport", CommandType.StoredProcedure);
        //    _DBResponse = new DatabaseResponse();
        //    IList<Areas.Import.Models.PaymentPartyName> objPaymentPartyName = new List<Areas.Import.Models.PaymentPartyName>();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            objPaymentPartyName.Add(new Areas.Import.Models.PaymentPartyName()
        //            {
        //                PartyId = Convert.ToInt32(Result["CHAId"]),
        //                PartyName = Convert.ToString(Result["CHAName"]),
        //                Address = Convert.ToString(Result["Address"]),
        //                State = Convert.ToString(Result["StateName"]),
        //                StateCode = Convert.ToString(Result["StateCode"]),
        //                GSTNo = Convert.ToString(Result["GSTNo"])
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = objPaymentPartyName;
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
        //#endregion

        #region OBL Wise Container
        public void GetOBLWiseContainerDetails(string OBLNo)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLNo", MySqlDbType = MySqlDbType.String, Value = OBLNo });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLWiseContainerDetails", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                Chn_OBLWiseContainerEntry objOBLEntry = new Chn_OBLWiseContainerEntry();

                //List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        objOBLEntry.OBL_No = Convert.ToString(Result.Tables[0].Rows[0]["OBL_NO"]);
                        objOBLEntry.OBL_Date = Convert.ToString(Result.Tables[0].Rows[0]["OBL_DATE"]);
                        objOBLEntry.IGM_No = Convert.ToString(Result.Tables[0].Rows[0]["IGM_NO"]);
                        objOBLEntry.IGM_Date = Convert.ToString(Result.Tables[0].Rows[0]["IGM_DATE"]);
                        objOBLEntry.TPNo = Convert.ToString(Result.Tables[0].Rows[0]["TP_NO"]);
                        objOBLEntry.TPDate = Convert.ToString(Result.Tables[0].Rows[0]["TP_DATE"]);
                        objOBLEntry.MovementType = Convert.ToString(Result.Tables[0].Rows[0]["MovementType"]);
                        objOBLEntry.PortId = Convert.ToInt32(Result.Tables[0].Rows[0]["PortId"]);
                        objOBLEntry.LineNo = Convert.ToString(Result.Tables[0].Rows[0]["LINE_NO"].ToString());
                        objOBLEntry.CargoDescription = Convert.ToString(Result.Tables[0].Rows[0]["CargoDescription"]);
                        objOBLEntry.NoOfPkg = Convert.ToString(Result.Tables[0].Rows[0]["NO_PKG"]);
                        objOBLEntry.PkgType = Convert.ToString(Result.Tables[0].Rows[0]["PKG_TYPE"]);
                        objOBLEntry.GR_WT = Convert.ToDecimal(Result.Tables[0].Rows[0]["GR_WT"]);
                        objOBLEntry.ImporterId = Convert.ToInt32(Result.Tables[0].Rows[0]["ImporterId"]);
                        objOBLEntry.ImporterName = Convert.ToString(Result.Tables[0].Rows[0]["ImporterName"]);
                        objOBLEntry.ImporterAddress = Convert.ToString(Result.Tables[0].Rows[0]["ImporterAddress"]);
                        objOBLEntry.ImporterAddress1 = Convert.ToString(Result.Tables[0].Rows[0]["ImporterAddress1"]);
                        objOBLEntry.CargoType = Convert.ToInt32(Result.Tables[0].Rows[0]["CargoType"]);
                        objOBLEntry.SMTPNo = Convert.ToString(Result.Tables[0].Rows[0]["SMTPNo"]);
                    }

                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        CHN_OBLWiseContainerEntryDetails objOBLEntryDetails = new CHN_OBLWiseContainerEntryDetails();
                        objOBLEntryDetails.ShippingLineName = Convert.ToString(dr["ShippingLineName"]);
                        objOBLEntryDetails.ShippingLineId = Convert.ToInt32(dr["ShippingLineId"]);
                        objOBLEntryDetails.ContainerSize = Convert.ToString(dr["ContainerSize"]);
                        objOBLEntryDetails.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objOBLEntryDetails.NoOfPkg = Convert.ToString(dr["NO_PKG"]);
                        objOBLEntryDetails.GR_WT = Convert.ToDecimal(dr["GR_WT"]);
                        objOBLEntry.OblEntryDetailsList.Add(objOBLEntryDetails);
                    }
                }

                if (Status == 1)
                {
                    //if (OblEntryDetailsList.Count > 0)
                    //{
                    //    objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(OblEntryDetailsList);
                    //}

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objOBLEntry;
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
        public void AddEditOBLWiseContainerEntry(Chn_OBLWiseContainerEntry objOBL, string XmlText)
        {
            string GeneratedClientId = "";
            objOBL.OBL_Date = (string.IsNullOrEmpty(objOBL.OBL_Date) ? null : DateTime.ParseExact(objOBL.OBL_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
            if (!string.IsNullOrEmpty(objOBL.IGM_Date))
            {
                objOBL.IGM_Date = DateTime.ParseExact(objOBL.IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            }
            else
            {
                objOBL.IGM_Date = null;
            }

            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.impobldtlId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLNO", MySqlDbType = MySqlDbType.String, Value = objOBL.OBL_No });
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLDate", MySqlDbType = MySqlDbType.DateTime, Value = objOBL.OBL_Date });
            lstParam.Add(new MySqlParameter { ParameterName = "in_LINE_NO", MySqlDbType = MySqlDbType.String, Value = objOBL.LineNo }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SMTPNo", MySqlDbType = MySqlDbType.String, Value = objOBL.SMTPNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SMTP_Date", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objOBL.SMTP_Date) }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPkg", MySqlDbType = MySqlDbType.String, Value = objOBL.NoOfPkg });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = objOBL.CargoType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.String, Value = objOBL.CargoDescription });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PkgType", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.PkgType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GRWT", MySqlDbType = MySqlDbType.Decimal, Value = objOBL.GR_WT });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_No", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.IGM_No });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_Date", MySqlDbType = MySqlDbType.DateTime, Value = objOBL.IGM_Date });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TPNo", MySqlDbType = MySqlDbType.String, Value = objOBL.TPNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TPDate", MySqlDbType = MySqlDbType.DateTime, Value = objOBL.TPDate != null ? Convert.ToDateTime(objOBL.TPDate).ToString("yyyy-MM-dd") : null });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MovementType", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.MovementType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.PortId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.CountryId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.ImporterId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DtlXml", MySqlDbType = MySqlDbType.Text, Value = XmlText });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.CommodityId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TSANo", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.TSANo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TSA_Date", MySqlDbType = MySqlDbType.DateTime, Value = objOBL.TSA_Date != null ? Convert.ToDateTime(objOBL.TSA_Date).ToString("yyyy-MM-dd") : null });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CIFValue", MySqlDbType = MySqlDbType.Decimal, Value = objOBL.CIFValue });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AreaCBM", MySqlDbType = MySqlDbType.Decimal, Value = objOBL.AreaCBM });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.ShippingLineId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditOBLWiseContDet", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "OBL Entry FCL Saved Successfully" : "OBL Entry FCL Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "This OBL information is already exists!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    //_DBResponse.Message = "Can not update as seal cutting done!";
                    _DBResponse.Message = "Can not update next step already done!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update as job order by train done!";
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
        public void ListOfOBLWiseContainer()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("ListOfOBLWiseContainer", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                List<Chn_OBLWiseContainerEntry> OblEntryList = new List<Chn_OBLWiseContainerEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Chn_OBLWiseContainerEntry objOBLEntry = new Chn_OBLWiseContainerEntry();
                        objOBLEntry.impobldtlId = Convert.ToInt32(dr["impobldtlId"]);
                        objOBLEntry.OBL_No = Convert.ToString(dr["OBL_No"]);
                        objOBLEntry.OBL_Date = Convert.ToString(dr["OBL_Date"]);
                        objOBLEntry.IGM_No = Convert.ToString(dr["IGM_No"].ToString());
                        objOBLEntry.IGM_Date = Convert.ToString(dr["IGM_Date"]);
                        //objOBLEntry.IsAlreadyUsed = Convert.ToInt32(dr["IsAlreadyUsed"]);
                        objOBLEntry.OBLCreateDate = Convert.ToString(dr["OBLCreate"]);
                        OblEntryList.Add(objOBLEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OblEntryList;
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
        public void ListOfImporterForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfImporterForPage", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.ImporterForPage> lstImporter = new List<Areas.Import.Models.ImporterForPage>();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstImporter.Add(new Areas.Import.Models.ImporterForPage
                    {
                        ImporterId = Convert.ToInt32(Result["ImporterId"]),
                        ImporterName = Result["ImporterName"].ToString(),
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
                    _DBResponse.Data = new { lstImporter, State };
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
        public void GetOBLWiseContainerDetailsByID(int impobldtlId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_impobldtlId", MySqlDbType = MySqlDbType.Int32, Value = impobldtlId });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLWiseContainerDetailsByID", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                Chn_OBLWiseContainerEntry objOBLEntry = new Chn_OBLWiseContainerEntry();

                //List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        objOBLEntry.OBL_No = Convert.ToString(Result.Tables[0].Rows[0]["OBL_NO"]);
                        objOBLEntry.OBL_Date = Convert.ToString(Result.Tables[0].Rows[0]["OBL_DATE"]);
                        objOBLEntry.IGM_No = Convert.ToString(Result.Tables[0].Rows[0]["IGM_NO"]);
                        objOBLEntry.IGM_Date = Convert.ToString(Result.Tables[0].Rows[0]["IGM_DATE"]);
                        objOBLEntry.TPNo = Convert.ToString(Result.Tables[0].Rows[0]["TP_NO"]);
                        objOBLEntry.TPDate = Convert.ToString(Result.Tables[0].Rows[0]["TP_DATE"]);
                        objOBLEntry.MovementType = Convert.ToString(Result.Tables[0].Rows[0]["MovementType"]);
                        objOBLEntry.PortId = Convert.ToInt32(Result.Tables[0].Rows[0]["PortId"]);
                        objOBLEntry.CountryId = Convert.ToInt32(Result.Tables[0].Rows[0]["CountryId"]);
                        objOBLEntry.LineNo = Convert.ToString(Result.Tables[0].Rows[0]["LINE_NO"].ToString());
                        objOBLEntry.CargoDescription = Convert.ToString(Result.Tables[0].Rows[0]["CargoDescription"]);
                        objOBLEntry.NoOfPkg = Convert.ToString(Result.Tables[0].Rows[0]["NO_PKG"]);
                        objOBLEntry.PkgType = Convert.ToString(Result.Tables[0].Rows[0]["PKG_TYPE"]);
                        objOBLEntry.GR_WT = Convert.ToDecimal(Result.Tables[0].Rows[0]["GR_WT"]);
                        objOBLEntry.ImporterId = Convert.ToInt32(Result.Tables[0].Rows[0]["ImporterId"]);
                        objOBLEntry.ImporterName = Convert.ToString(Result.Tables[0].Rows[0]["ImporterName"]);
                        objOBLEntry.CargoType = Convert.ToInt32(Result.Tables[0].Rows[0]["CargoType"]);
                        objOBLEntry.SMTPNo = Convert.ToString(Result.Tables[0].Rows[0]["SMTPNo"]);
                        objOBLEntry.SMTP_Date = Convert.ToString(Result.Tables[0].Rows[0]["SMTP_Date"]);
                        objOBLEntry.CommodityId = Convert.ToInt32(Result.Tables[0].Rows[0]["CommodityId"]);
                        objOBLEntry.Commodity = Convert.ToString(Result.Tables[0].Rows[0]["Commodity"]);
                        objOBLEntry.TSANo = Result.Tables[0].Rows[0]["TSANo"].ToString();
                        objOBLEntry.TSA_Date = Result.Tables[0].Rows[0]["TSA_Date"].ToString();
                        objOBLEntry.ShippingLineId = Convert.ToInt32(Result.Tables[0].Rows[0]["ShippingLineId"]);
                        objOBLEntry.ShippingLineName = Result.Tables[0].Rows[0]["ShippingLineName"].ToString();
                        objOBLEntry.AreaCBM = Convert.ToDecimal(Result.Tables[0].Rows[0]["AreaCBM"]);
                        objOBLEntry.CIFValue = Convert.ToDecimal(Result.Tables[0].Rows[0]["CIFValue"]);
                    }

                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        CHN_OBLWiseContainerEntryDetails objOBLEntryDetails = new CHN_OBLWiseContainerEntryDetails();
                        objOBLEntryDetails.ShippingLineName = Convert.ToString(dr["ShippingLineName"]);
                        objOBLEntryDetails.ShippingLineId = Convert.ToInt32(dr["ShippingLineId"]);
                        objOBLEntryDetails.ContainerSize = Convert.ToString(dr["ContainerSize"]);
                        objOBLEntryDetails.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objOBLEntryDetails.NoOfPkg = Convert.ToString(dr["NO_PKG"]);
                        objOBLEntryDetails.PkgType = Convert.ToString(dr["PKG_TYPE"]);
                        objOBLEntryDetails.GR_WT = Convert.ToDecimal(dr["GR_WT"]);
                        objOBLEntryDetails.impobldtlId = Convert.ToInt32(dr["impobldtlId"]);
                        objOBLEntryDetails.DetailsID = Convert.ToInt32(dr["DetailsID"]);
                        objOBLEntryDetails.CONTCBT = Convert.ToString(dr["CONTCBT"]);
                        objOBLEntry.OblEntryDetailsList.Add(objOBLEntryDetails);
                    }
                }

                if (Status == 1)
                {
                    if (objOBLEntry.OblEntryDetailsList.Count > 0)
                    {
                        objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(objOBLEntry.OblEntryDetailsList);
                    }

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objOBLEntry;
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
        public void DeleteOBLWiseContainer(int OblEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("DeleteOBLWiseContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    //_DBResponse.Message = "Cannot Delete As It Exists In Seal Cutting";
                    _DBResponse.Message = "Cannot Delete As It Exists In Another Page";
                    _DBResponse.Status = 2;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Cannot Delete  As It Exists In Job Order By Train";
                    _DBResponse.Status = 3;
                }
                //else if (Result == -1)
                //{
                //    _DBResponse.Data = null;
                //    _DBResponse.Message = "Cannot Delete As It Exists In Another Page";
                //    _DBResponse.Status = -1;
                //}
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

        public void GetOBLContCBT(string ContCBT)
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContCBT", MySqlDbType = MySqlDbType.String, Value = ContCBT });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOblContainerCbt", CommandType.StoredProcedure, DParam);
            List<Chn_ContainerInfo> LstContainerInfo = new List<Chn_ContainerInfo>();
            string size = "";
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainerInfo.Add(new Chn_ContainerInfo
                    {
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLine = Convert.ToString(Result["ShippingLine"]),
                        NoOfPackages = Convert.ToDecimal(Result["NoOfPackages"]),
                        GrossWeight = Convert.ToDecimal(Result["GrossWeight"]),
                        Size = Convert.ToString(Result["Size"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";

                    _DBResponse.Data = LstContainerInfo;


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
        public void GetOBLContCBTSearch(string ContCBT, String ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContCBT", MySqlDbType = MySqlDbType.String, Value = ContCBT });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Container", MySqlDbType = MySqlDbType.String, Value = ContainerNo });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOblContainerCbtBySearch", CommandType.StoredProcedure, DParam);
            List<CwcExim.Areas.Report.Models.ContainerInfo> LstContainerInfo = new List<CwcExim.Areas.Report.Models.ContainerInfo>();
            string size = "";
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstContainerInfo.Add(new CwcExim.Areas.Report.Models.ContainerInfo
                    {

                        ContainerNo = Convert.ToString(Result["ContainerNo"]),

                        // MovementType = Convert.ToString(Result["MovementType"])
                    });


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";

                    _DBResponse.Data = LstContainerInfo;


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
        public void GetCFSCodeFromContainer(string ContainerNo, string ContainerSize, string CFSCode)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_CONTAINER_NO", MySqlDbType = MySqlDbType.String, Value = ContainerNo });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CONT_SIZE", MySqlDbType = MySqlDbType.String, Value = ContainerSize });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CFSCode", MySqlDbType = MySqlDbType.String, Value = CFSCode });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetCFSCodeFromContainer", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                int RetVal = 0;
                //string CFSCode = "";

                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        RetVal = Convert.ToInt32(Result.Tables[0].Rows[0]["RetVal"]);
                        CFSCode = Convert.ToString(Result.Tables[0].Rows[0]["CFSCode"]);
                    }

                }

                if (Status == 1)
                {
                    _DBResponse.Status = RetVal;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CFSCode;
                }
                else
                {
                    _DBResponse.Status = -1;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = -1;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
            }
        }
        public void GetOBLDetails(string CFSCode, string ContainerNo, string ContainerSize, string IGM_No, string IGM_Date, int OBLEntryId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "In_CFSCode", MySqlDbType = MySqlDbType.String, Value = CFSCode });
                LstParam.Add(new MySqlParameter { ParameterName = "In_CONTAINER_NO", MySqlDbType = MySqlDbType.String, Value = ContainerNo });
                //LstParam.Add(new MySqlParameter { ParameterName = "In_CONT_SIZE", MySqlDbType = MySqlDbType.String, Value = ContainerSize });
                //LstParam.Add(new MySqlParameter { ParameterName = "In_IGM_NO", MySqlDbType = MySqlDbType.String, Value = IGM_No });
                //LstParam.Add(new MySqlParameter { ParameterName = "In_IGM_DATE", MySqlDbType = MySqlDbType.DateTime, Value = dt.ToString("yyyy/MM/dd") });
                LstParam.Add(new MySqlParameter { ParameterName = "In_OBLEntryId", MySqlDbType = MySqlDbType.Int32, Value = OBLEntryId });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOblEntryDetails", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                OBLEntry objOBLEntry = new OBLEntry();

                //List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        objOBLEntry.MovementType = Convert.ToString(Result.Tables[0].Rows[0]["MovementType"]);
                        objOBLEntry.PortId = Convert.ToInt32(Result.Tables[0].Rows[0]["PortId"]);
                        objOBLEntry.ShippingLineName = Convert.ToString(Result.Tables[0].Rows[0]["ShippingLineName"]);
                        objOBLEntry.ShippingLineId = Convert.ToInt32(Result.Tables[0].Rows[0]["ShippingLineId"]);
                        objOBLEntry.ContainerSize = Convert.ToString(Result.Tables[0].Rows[0]["ContainerSize"]);
                        objOBLEntry.IGM_No = Convert.ToString(Result.Tables[0].Rows[0]["IGM_NO"]);
                        objOBLEntry.IGM_Date = Convert.ToString(Result.Tables[0].Rows[0]["IGM_DATE"]);
                        objOBLEntry.TPNo = Convert.ToString(Result.Tables[0].Rows[0]["TP_NO"]);
                        objOBLEntry.TPDate = Convert.ToString(Result.Tables[0].Rows[0]["TP_DATE"]);
                    }

                    foreach (DataRow dr in Result.Tables[1].Rows)
                    {
                        OblEntryDetails objOBLEntryDetails = new OblEntryDetails();
                        objOBLEntryDetails.icesContId = Convert.ToInt32(dr["icesContId"]);
                        objOBLEntryDetails.OBL_No = Convert.ToString(dr["OBL_NO"]);
                        objOBLEntryDetails.OBL_Date = Convert.ToString(dr["OBL_DATE"]);
                        objOBLEntryDetails.LineNo = Convert.ToString(dr["LINE_NO"].ToString());
                        objOBLEntryDetails.CargoDescription = Convert.ToString(dr["CargoDescription"]);
                        objOBLEntryDetails.NoOfPkg = Convert.ToString(dr["NO_PKG"]);
                        objOBLEntryDetails.PkgType = Convert.ToString(dr["PKG_TYPE"]);
                        objOBLEntryDetails.GR_WT = Convert.ToDecimal(dr["GR_WT"]);
                        objOBLEntryDetails.ImporterId = Convert.ToInt32(dr["ImporterId"]);
                        objOBLEntryDetails.ImporterName = Convert.ToString(dr["ImporterName"]);
                        objOBLEntryDetails.CargoType = Convert.ToInt32(dr["CargoType"]);
                        objOBLEntryDetails.SMTPNo = Convert.ToString(dr["SMTPNo"]);
                        objOBLEntryDetails.IsProcessed = Convert.ToInt32(dr["IsProcessed"]);
                        objOBLEntryDetails.IGM_IMPORTER = Convert.ToString(dr["IGM_IMPORTER"]);
                        objOBLEntry.OblEntryDetailsList.Add(objOBLEntryDetails);
                    }
                }

                if (Status == 1)
                {
                    //if (OblEntryDetailsList.Count > 0)
                    //{
                    //    objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(OblEntryDetailsList);
                    //}

                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objOBLEntry;
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

        #region OBL Entry
        public void GetOBLContainerListOrSize(string CFSCode = "", string Type = "")
        {
            int Status = 0;
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.String, Value = CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CONTCBT", MySqlDbType = MySqlDbType.String, Value = Type });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLContainerList", CommandType.StoredProcedure, DParam);
            List<CwcExim.Areas.Import.Models.WFLD_ContainerInfo> LstContainerInfo = new List<CwcExim.Areas.Import.Models.WFLD_ContainerInfo>();
            string size = "";
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    if (CFSCode == "" || CFSCode == null)
                    {
                        LstContainerInfo.Add(new CwcExim.Areas.Import.Models.WFLD_ContainerInfo
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            Size = Convert.ToString(Result["Size"]),
                            MovementType = Convert.ToString(Result["ContainerLoadType"]),
                            TPNo = Convert.ToString(Result["TPNo"]),
                            PortName = Convert.ToString(Result["PortName"]),
                            ShippingLine = Convert.ToString(Result["ShippingLine"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                            PortId = Convert.ToInt32(Result["PortId"]),

                            // MovementType = Convert.ToString(Result["MovementType"])
                        });
                    }
                    else
                    {
                        size = Convert.ToString(Result["Size"]);
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    if (CFSCode == "" || CFSCode == null)
                    {
                        _DBResponse.Data = LstContainerInfo;
                    }
                    else
                    {
                        _DBResponse.Data = size;
                    }

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
        public void AddEditOBLEntry(Chn_OblEntry objOBL, string XmlText)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OBLId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.Id });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.String, Value = objOBL.ContainerNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CONTCBT", MySqlDbType = MySqlDbType.String, Value = objOBL.CONTCBT });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerSize", MySqlDbType = MySqlDbType.String, Value = objOBL.ContainerSize }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.String, Value = objOBL.CFSCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_No", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.IGM_No });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IGM_Date", MySqlDbType = MySqlDbType.DateTime, Value = objOBL.IGM_Date != null ? Convert.ToDateTime(objOBL.IGM_Date).ToString("yyyy-MM-dd") : null });// Value = Convert.ToDateTime(objOBL.IGM_Date).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TPNo", MySqlDbType = MySqlDbType.String, Value = objOBL.TPNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_TPDate", MySqlDbType = MySqlDbType.DateTime, Value = objOBL.TPDate != null ? Convert.ToDateTime(objOBL.TPDate).ToString("yyyy-MM-dd") : null });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MovementType", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.MovementType });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PortId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.PortId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CountryId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.CountryId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objOBL.ShippingLineId });

            lstParam.Add(new MySqlParameter { ParameterName = "in_DtlXml", MySqlDbType = MySqlDbType.Text, Value = XmlText });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = GeneratedClientId, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddEditOBLEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "OBL Entry Saved Successfully" : "OBL Entry Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "This Container information is already exists!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    //_DBResponse.Message = "Can not update as seal cutting done!";
                    _DBResponse.Message = "Can not update next step already exists!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update as job order by road done!";
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
        public void GetAllOblEntry(int Page)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("GetAllOblEntryForPage", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<Chn_OblEntry> OblEntryList = new List<Chn_OblEntry>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Chn_OblEntry objOBLEntry = new Chn_OblEntry();
                        objOBLEntry.Id = Convert.ToInt32(dr["Id"]);
                        objOBLEntry.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objOBLEntry.ContainerSize = Convert.ToString(dr["ContainerSize"]);
                        objOBLEntry.IGM_No = Convert.ToString(dr["IGM_No"].ToString());
                        objOBLEntry.IGM_Date = Convert.ToString(dr["IGM_Date"]);
                        objOBLEntry.IsAlreadyUsed = Convert.ToInt32(dr["IsAlreadyUsed"]);
                        objOBLEntry.OBLCreateDate = Convert.ToString(dr["OBLCreateDate"]);
                        OblEntryList.Add(objOBLEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = OblEntryList;
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
        public void DeleteOBLEntry(int OblEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("DeleteOBLEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = null;
                    //_DBResponse.Message = "Cannot Delete As It Exists In Seal Cutting";
                    _DBResponse.Message = "Cannot Delete As It Exists In Another Page";
                    _DBResponse.Status = 2;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Cannot Delete  As It Exists In Job Order By Road";
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
        public void GetOblEntryDetailsByOblEntryId(int OblEntryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_OblEntryId", MySqlDbType = MySqlDbType.Int32, Value = OblEntryId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetOblEntryDetailsByOblEntryId", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            Chn_OblEntry objOBLEntry = new Chn_OblEntry();
            IList<Chn_OblEntryDetails> lstOBLEntryDetails = new List<Chn_OblEntryDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objOBLEntry.Id = Convert.ToInt32(Result["Id"]);
                    objOBLEntry.ContainerNo = Convert.ToString(Result["ContainerNo"]);
                    objOBLEntry.ContainerSize = Convert.ToString(Result["ContainerSize"]);
                    objOBLEntry.CFSCode = Convert.ToString(Result["CFSCode"]);
                    objOBLEntry.IGM_No = Convert.ToString(Result["IGM_No"]);
                    objOBLEntry.IGM_Date = Convert.ToString(Result["IGM_Date"]);
                    objOBLEntry.TPNo = Convert.ToString(Result["TPNo"]);
                    objOBLEntry.TPDate = Convert.ToString(Result["TPDate"]);
                    objOBLEntry.MovementType = Convert.ToString(Result["MovementType"]);
                    objOBLEntry.PortId = Convert.ToInt32(Result["PortId"].ToString());
                    objOBLEntry.PortName = Convert.ToString(Result["PortName"]);
                    objOBLEntry.CountryId = Convert.ToInt32(Result["CountryId"].ToString());
                    objOBLEntry.CountryName = Convert.ToString(Result["CountryName"]);
                    objOBLEntry.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    objOBLEntry.ShippingLineName = Convert.ToString(Result["ShippingLineName"]);
                    objOBLEntry.IsAlreadyUsed = Convert.ToInt32(Result["IsAlreadyUsed"]);
                    objOBLEntry.CONTCBT = Convert.ToString(Result["CONTCBT"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstOBLEntryDetails.Add(new Chn_OblEntryDetails
                        {
                            OBLEntryId = Convert.ToInt32(Result["OblEntry_DtlId"]),
                            OBL_No = Convert.ToString(Result["OBL_NO"]),
                            OBL_Date = Convert.ToString(Result["OBL_DATE"]),
                            SMTP_Date = Convert.ToString(Result["SMTP_Date"]),
                            LineNo = Convert.ToString(Result["LINE_NO"].ToString()),
                            CargoDescription = Convert.ToString(Result["CargoDescription"]),
                            NoOfPkg = Convert.ToString(Result["NO_PKG"]),
                            PkgType = Convert.ToString(Result["PKG_TYPE"]),
                            GR_WT = Convert.ToDecimal(Result["GR_WT"]),
                            ImporterId = Convert.ToInt32(Result["ImporterId"]),
                            ImporterName = Convert.ToString(Result["ImporterName"]),
                            SMTPNo = Convert.ToString(Result["SMTPNo"]),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            IsProcessed = Convert.ToInt32(Result["IsProcessed"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"]),
                            Commodity = Convert.ToString(Result["Commodity"]),
                            IGM_IMPORTER = Convert.ToString(Result["IGM_Importer"]),
                            TSANo = Result["TSANo"].ToString(),
                            TSA_Date = Result["TSA_Date"].ToString(),
                            AreaCBM = Convert.ToDecimal(Result["AreaCBM"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"])
                        });
                    }
                }
                if (lstOBLEntryDetails.Count > 0)
                {
                    objOBLEntry.StringifiedText = Newtonsoft.Json.JsonConvert.SerializeObject(lstOBLEntryDetails);
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objOBLEntry;
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

        #region Destuffing Entry
        public void AddEditDestuffingEntry(CHNDestuffingEntry ObjDestuffing, string DestuffingEntryXML /*, string GodownXML, string ClearLcoationXML */, int BranchId, int Uid)
        {

            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDestuffing.DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StartDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.StartDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.DestuffingEntryDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TallySheetId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(ObjDestuffing.TallySheetId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(ObjDestuffing.ContainerId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.CHAId == 0 ? null : ObjDestuffing.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Rotation", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ObjDestuffing.Rotation });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoTally", MySqlDbType = MySqlDbType.Int32, Size = 45, Value = Convert.ToInt32(ObjDestuffing.TallyCargo == true ? 1 : 0) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Scanning", MySqlDbType = MySqlDbType.Int32, Size = 45, Value = Convert.ToInt32(ObjDestuffing.Scanning == true ? 1 : 0) });
            /*LstParam.Add(new MySqlParameter { ParameterName = "in_FOB", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjDestuffing.FOB) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossDuty", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjDestuffing.GrossDuty) });*/

            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDestuffing.DeliveryType) });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_DOType", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDestuffing.DOType) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDestuffing.GodownId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingXML", MySqlDbType = MySqlDbType.Text, Value = DestuffingEntryXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });

            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            int Result = DataAccess.ExecuteNonQuery("AddEditImpDestuffingEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjDestuffing.DestuffingEntryId == 0 ? "Destuffing Entry Details Saved Successfully" : "Destuffing Entry Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Destuffing Entry  Details Already Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Edit Destuffing Entry Details As It Already Exists In Another Page";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = 4;
                    _DBResponse.Message = "Data already exists";
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
        public void DelDestuffingEntry(int DestuffingEntryId, int BranchId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelImpDestuffingEntry", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Destuffing Entry Application Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Destuffing Entry Application Details As It Exist In Another Page";
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
        public void GetDestuffingEntry(int DestuffingEntryId, int BranchId, string Action)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Action", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = Action });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHNDestuffingEntry ObjDestuffing = new CHNDestuffingEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDestuffing.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    ObjDestuffing.StartDate = (Result["StartDate"] == null ? "" : Result["StartDate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.TallySheetId = Result["TallySheetId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["TallySheetId"]);
                    ObjDestuffing.ContainerId = Convert.ToInt32(Result["ContainerId"]);
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]);
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    //ObjDestuffing.DOType = Convert.ToInt32(Result["DOType"]);
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                    ObjDestuffing.CHA = (Result["CHA"]).ToString();
                    ObjDestuffing.GodownId = Convert.ToInt32(Result["GodownId"]);
                    ObjDestuffing.TallySheetDate = Result["TallySheetDate"].ToString();
                    ObjDestuffing.TallyCargo = Convert.ToBoolean(Result["CargoTally"]);
                    ObjDestuffing.Scanning = Convert.ToBoolean(Result["Scanning"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.lstDtl.Add(new CHN_DestuffingEntryDtl
                        {
                            DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                            TallySheetDtlId = Result["TallySheetDtlId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["TallySheetDtlId"]),
                            BOLNo = (Result["BOLNo"] == null ? "" : Result["BOLNo"]).ToString(),
                            BOLDate = (Result["BOLDate"] == null ? "" : Result["BOLDate"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            ReceivedPackages = Convert.ToInt32(Result["ReceivedPackages"] == DBNull.Value ? 0 : Result["ReceivedPackages"]),
                            UOM = Result["UOM"].ToString(),
                            Area = Convert.ToDecimal(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            AreaCbm = Convert.ToDecimal(Result["AreaCbm"] == DBNull.Value ? 0 : Result["AreaCbm"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            DestuffingWeight = Convert.ToDecimal(Result["DestuffWeight"] == DBNull.Value ? 0 : Result["DestuffWeight"]),
                            GodownWiseLocationIds = (Result["GodownWiseLocationIds"] == null ? "" : Result["GodownWiseLocationIds"]).ToString(),
                            GodownWiseLocationNames = (Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]).ToString(),
                            Remarks = (Result["Remarks"]).ToString(),
                            TallySheetArea = Result["AreaTallySheet"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["AreaTallySheet"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            GrossDuty = Convert.ToDecimal(Result["GrossDuty"] == DBNull.Value ? 0 : Result["GrossDuty"]),
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            OBLWiseDestuffingDate = Result["OBLWiseDestuffingDate"].ToString(),
                            IsEditable = Convert.ToInt32(Result["IsEditable"] == DBNull.Value ? 0 : Result["IsEditable"]),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            IsHoldRelease = Convert.ToInt32(Result["IsHoldRelease"]),
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            GodownName = Convert.ToString(Result["GodownName"]),
                            GdTransId = Convert.ToInt32(Result["GdTransId"]),
                            StockDetailsId = Convert.ToInt32(Result["StockDetailsId"])

                        });
                    }
                }
                //if (Action == "Edit")
                //{
                //    if (Result.NextResult())
                //    {
                //        while (Result.Read())
                //        {
                //            ObjDestuffing.lstLocation.Add(new GodownLocation
                //            {
                //                LocationId = Convert.ToInt32(Result["LocationId"]),
                //                LocationName = Result["LocationName"].ToString()
                //            });
                //        }
                //    }
                //}
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDestuffing;
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
        public void GetAllDestuffingEntry(int Page, int UId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UId", MySqlDbType = MySqlDbType.Int32, Value = UId });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Action", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntryForPage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_DestuffingList> LstDestuffing = new List<CHN_DestuffingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new CHN_DestuffingList
                    {
                        DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]),
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        //Rotation = Result["Rotation"].ToString(),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                        CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDestuffing;
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
        public void GetAllDestuffingEntry(int UId, string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_UId", MySqlDbType = MySqlDbType.Int32, Value = UId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo.TrimStart() });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntrySearchByContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_DestuffingList> LstDestuffing = new List<CHN_DestuffingList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new CHN_DestuffingList
                    {
                        DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]),
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        //Rotation = Result["Rotation"].ToString(),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                        CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDestuffing;
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
        public void GetContrNoForDestuffingEntry(int BranchId, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContrNoForDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<dynamic> LstDestuffing = new List<dynamic>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        TallySheetId = Convert.ToInt32(Result["TallySheetId"]),
                        Id = Convert.ToInt32(Result["Id"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDestuffing;
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
        public void GetOBLforDestuffingEntry(int TallySheetId, int BranchId, String CFSCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TallySheetId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = TallySheetId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CfsCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLforDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            //List<dynamic> lstOBL = new List<dynamic>();
            List<CHN_DestuffingEntryDtl> lstOBL = new List<CHN_DestuffingEntryDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //lstOBL.Add(new
                    //{
                    //    TallySheetDtlId = Convert.ToInt32(Result["TallySheetDtlId"]),
                    //    BOLNo = Result["OblNo"].ToString()
                    //});
                    lstOBL.Add(new CHN_DestuffingEntryDtl
                    {
                        TallySheetDtlId = Convert.ToInt32(Result["TallySheetDtlId"]),
                        BOLNo = Result["OblNo"].ToString(),
                        BOLDate = (Result["OBLDATE"] == null ? "" : Result["OblDate"]).ToString(),
                        LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                        CargoDescription = (Result["Cargo"] == null ? "" : Result["Cargo"]).ToString(),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPkg"]),
                        GrossWeight = Convert.ToDecimal(Result["Weight"]),
                        UOM = Result["UOM"].ToString(),
                        CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        Commodity = Result["Commodity"].ToString(),
                        TallySheetArea = Convert.ToDecimal(Result["AreaTallysheet"]),
                        //GodownId = Convert.ToInt32(Result["GodownId"]),
                        ContainerId = Convert.ToInt32(Result["ContainerId"]),
                        CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        RemWeight = Convert.ToDecimal(Result["RemWeight"]),
                        RemPackages = Convert.ToInt32(Result["RemPackages"]),
                        IsHoldRelease = Convert.ToInt32(Result["IsHoldRelease"]),
                        BOENo = Result["BOENo"].ToString(),
                        BOEDate = Result["BOEDate"].ToString(),
                        Remarks = Result["Remarks"].ToString(),
                        ShippingLine = Result["ShippingLineName"].ToString(),

                        OBLWiseDestuffingDate = Result["OblWiseDestuffingDate"].ToString(),

                    });

                }


                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstOBL;
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
        public void GetContrDetForDestuffingEntry(int TallySheetId, int BranchId, String CFSCode)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_TallySheetId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = TallySheetId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CfsCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContrDetForDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CHN_DestuffingEntryDtl> lstdynamic = new List<CHN_DestuffingEntryDtl>();
            List<dynamic> lstdynamicGodown = new List<dynamic>();
            List<dynamic> lstdynamicloc = new List<dynamic>();
            List<dynamic> lstdynamichdr = new List<dynamic>();
            List<CHN_DestufGodownDetails> lstgodowndynamic = new List<CHN_DestufGodownDetails>();
            string ShippingLine = "";
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstdynamic.Add(new CHN_DestuffingEntryDtl
                    {
                        //TallySheetDtlId = Convert.ToInt32(Result["TallySheetDtlId"]),
                        //BOLNo = Result["OblNo"].ToString(),
                        //BOLDate = (Result["OBLDATE"] == null ? "" : Result["OblDate"]).ToString(),
                        //LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                        //CargoDescription = (Result["Cargo"] == null ? "" : Result["Cargo"]).ToString(),
                        //NoOfPackages = Convert.ToInt32(Result["NoOfPkg"]),
                        //GrossWeight = Convert.ToDecimal(Result["Weight"]),
                        //UOM = Result["UOM"].ToString(),
                        //CommodityId = Convert.ToInt32(Result["CommodityId"]),
                        //Commodity = Result["Commodity"].ToString(),
                        //TallySheetArea = Convert.ToDecimal(Result["AreaTallysheet"]),
                        //GodownId = Convert.ToInt32(Result["GodownId"]),
                        ContainerId = Convert.ToInt32(Result["ContainerId"]),
                        //CargoType = Convert.ToInt32(Result["CargoType"]),
                        // ReceivedPackages = Convert.ToInt32(Result["ReceivedPackages"]),
                        //DestuffingWeight = Convert.ToInt32(Result["DestuffingWeight"]),
                        // OBLWiseDestuffingDate = "",
                    });

                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstdynamicloc.Add(new
                        {
                            LocationId = Convert.ToInt32(Result["LocationId"]),
                            LocationName = Result["LocationName"].ToString()
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ShippingLine = Convert.ToInt32(Result["ShippingLineId"]) + "," + Result["ShippingLineName"].ToString() + "," + Result["TallySheetDate"].ToString();
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstdynamichdr.Add(new
                        {
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CHAName = Result["CHAName"].ToString(),
                            RotationNo = Result["RotationNo"].ToString()
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstdynamicGodown.Add(new
                        {
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            GodownName = Result["GodownName"].ToString()
                        });
                    }
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        lstgodowndynamic.Add(new WFLD_DestufGodownDetails
                //        {

                //            TallySheetId = Convert.ToInt32(Result["TallySheetId"]),
                //            GodownId = Convert.ToInt32(Result["GodownId"]),
                //            GodownName = Convert.ToString(Result["GodownName"]),
                //            GodownWiseLocationIds = Convert.ToString(Result["LocationIds"]),
                //            GodownWiseLocationNames = Convert.ToString(Result["LocationNames"]),
                //            ReceivedPackages = Convert.ToInt32(Result["ReceivedPackages"]),
                //            DestuffingWeight = Convert.ToInt32(Result["DestuffingWeight"]),
                //            Area = Convert.ToDecimal(Result["SQM"]),
                //            AreaCbm = Convert.ToDecimal(Result["SQM"]),
                //            EntryDate = Convert.ToString(Result["EntryDate"])

                //        });
                //    }
                //}

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstdynamic, lstdynamicloc, ShippingLine, lstdynamichdr };
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
        public void GetDestuffEntryForPrint(int DestuffingEntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffEntryForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CHN_DestuffingSheet ObjDestuffing = new CHN_DestuffingSheet();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    ObjDestuffing.StartDate = (Result["StartDate"] == null ? "" : Result["StartDate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.DestuffingEntryDateTime = Result["DestuffingEntryDateTime"].ToString();
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.GateInDate = Result["GateInDate"].ToString();
                    ObjDestuffing.CustomSealNo = Result["CustomSealNo"].ToString();
                    ObjDestuffing.SlaSealNo = Result["SlaSealNo"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.IGMNo = Result["IGMNo"].ToString();
                    ObjDestuffing.MovementType = Result["MovementType"].ToString();
                    ObjDestuffing.ShippingLine = Result["ShippingLine"].ToString();
                    ObjDestuffing.POL = (Result["POL"] == null ? "" : Result["POL"]).ToString();
                    ObjDestuffing.POD = (Result["POD"] == null ? "" : Result["POD"]).ToString();
                    ObjDestuffing.GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        decimal tArea = 0;
                        if (Convert.ToDecimal(Result["Area"]) > 0)
                        {
                            tArea = Convert.ToDecimal(Result["Area"] == DBNull.Value ? 0 : Result["Area"]);
                        }
                        else
                        {
                            tArea = Convert.ToDecimal(Result["AreaCbm"] == DBNull.Value ? 0 : Result["AreaCbm"]);
                        }
                        ObjDestuffing.lstDtl.Add(new CHN_DestuffingSheetDtl
                        {
                            SMTPNo = (Result["SMTPNo"] == null ? "" : Result["SMTPNo"]).ToString(),
                            OblNo = (Result["OblNo"] == null ? "" : Result["OblNo"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            Cargo = (Result["Cargo"] == null ? "" : Result["Cargo"]).ToString(),
                            Type = (Result["Type"] == null ? "" : Result["Type"]).ToString(),
                            NoOfPkg = Convert.ToInt32(Result["NoOfPkg"] == DBNull.Value ? 0 : Result["NoOfPkg"]),
                            PkgRec = Convert.ToInt32(Result["PkgRec"] == DBNull.Value ? 0 : Result["PkgRec"]),
                            Weight = Convert.ToDecimal(Result["Weight"] == DBNull.Value ? 0 : Result["Weight"]),
                            Area = tArea,
                            //Area = Convert.ToDecimal(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            //AreaCbm = Convert.ToDecimal(Result["AreaCbm"] == DBNull.Value ? 0 : Result["AreaCbm"]),
                            GodownWiseLctnNames = (Result["GodownWiseLctnNames"]).ToString(),
                            Remarks = (Result["Remarks"]).ToString(),
                            CBM = Convert.ToDecimal(Result["Cbm"]),
                            GeneralHazardous = Convert.ToString(Result["CargoType"]),
                            LineNo = Convert.ToString(Result["LineNo"]),
                            ShippingLine = Convert.ToString(Result["ShippingLine"]),
                            TSANO = Convert.ToString(Result["TSANo"]),

                        });
                    }
                }
                /*if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.objCom.Name = Result["Name"].ToString();
                        ObjDestuffing.objCom.RoleName = Result["RoleName"].ToString();
                        ObjDestuffing.objCom.CompanyShortName = Result["CompanyShortName"].ToString();
                        ObjDestuffing.objCom.CompanyAddress = Result["CompanyAddress"].ToString();
                    }
                }*/
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDestuffing;
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
        public void GetCIFandDutyForBOE(string BOENo, string BOEDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(BOEDate).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCIFDutyForBOE", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            dynamic obj = new { CIFValue = "", GrossDuty = "" };
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    obj = new
                    {
                        CIFValue = Result["CIF_VALUE"].ToString(),
                        GrossDuty = Result["DUTY"].ToString()
                    };
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
        public void ListOfGodownData(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "In_UserId", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstGodownRights", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.GodownList> lstGodownList = new List<Areas.Import.Models.GodownList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGodownList.Add(new Areas.Import.Models.GodownList
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        GodownNo = Convert.ToString(Result["GodownName"]),

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstGodownList;
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
        public void GetGodownLocationById(int GodownId)
        {
            int Status = 0;
            int uid = ((CwcExim.Models.Login)HttpContext.Current.Session["LoginUser"]).Uid;

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = GodownId });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGodownLocationById", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<dynamic> lstdynamicloc = new List<dynamic>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstdynamicloc.Add(new
                    {
                        LocationId = Convert.ToInt32(Result["LocationId"]),
                        LocationName = Result["LocationName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstdynamicloc };
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
        public void ListOfGodownByOBL(string OBL, int DestuffingEntryDtlId)
        {
            int Status = 0;
            int uid = ((CwcExim.Models.Login)HttpContext.Current.Session["LoginUser"]).Uid;

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryDtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OBL", MySqlDbType = MySqlDbType.VarChar, Value = OBL });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstGodownAccessRightsByOBLGodown", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.CHNGodownListWithDestiffDetails> lstGodownList = new List<Areas.Import.Models.CHNGodownListWithDestiffDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGodownList.Add(new Areas.Import.Models.CHNGodownListWithDestiffDetails
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"]),
                        GodownNo = Convert.ToString(Result["GodownName"]),
                        DestuffWeight = Convert.ToDecimal(Result["DestuffWeight"]),
                        DstuffReceivedPackages = Convert.ToInt32(Result["ReceivedPackages"]),
                        DstuffCBM = Convert.ToDecimal(Result["CBM"]),
                        DstuffSQM = Convert.ToDecimal(Result["SQM"]),
                        DstuffCIFValue = Convert.ToDecimal(Result["CIFValue"]),
                        DstuffGrossDuty = Convert.ToDecimal(Result["GrossDuty"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstGodownList;
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
        public void GetCIFFromOOCDelivery(String BOE, String BOEDT)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = BOE });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(BOEDT).ToString("yyyy-MM-dd") });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCIFDutyForBOE", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CIFFromOOC CIFFromOOCOBJ = new CIFFromOOC();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    CIFFromOOCOBJ.CIF = Result["CIF_VALUE"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIF_VALUE"]);
                    CIFFromOOCOBJ.Duty = Result["DUTY"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["DUTY"]);
                    CIFFromOOCOBJ.BOE_DATE = Convert.ToString(Result["BOE_DATE"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = CIFFromOOCOBJ;
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
            IList<Models.Commodity> LstCommodity = new List<Models.Commodity>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstCommodity.Add(new Models.Commodity
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
        public void getCompanyDetails()
        {

            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();


            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("getCompanyDetails", CommandType.StoredProcedure, DParam);
            //CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            dynamic objcomp = new System.Dynamic.ExpandoObject();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    objcomp.RoAddress = Convert.ToString(Result["ROAddress"]).Replace("<br/>", " ");
                    objcomp.CompanyName = Convert.ToString(Result["CompanyName"]);
                    objcomp.CompanyAddress = Convert.ToString(Result["CompanyAddress"]).Replace("<br/>", " ");
                    objcomp.EmailAddress = Convert.ToString(Result["EmailAddress"]).Replace("<br/>", " ");
                    objcomp.Location = Result["Location"].ToString();


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objcomp;
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
                Result.Close();
                Result.Dispose();

            }
        }
        #endregion

        #region Empty Container Payment Sheet
        public void GetImpPaymentPartyForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPartyForPage", CommandType.StoredProcedure, Dparam);
            IList<Chn_ImpPartyForpage> lstParty = new List<Chn_ImpPartyForpage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new Chn_ImpPartyForpage
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
        public void AddEditEmptyContPaymentSheet(Chn_ImportPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML, string ChargesBreakupXML,
            int BranchId, int Uid,
           string Module, string CargoXML = "")
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SEZ });

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
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });


            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditEmptyContPaymentSheet", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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
        public void GetEmptyContainerListForInvoice()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = DateTime.Now.ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "GateInId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetEmptyContainerListForInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<EmptyContainerListForInvoice> objListForInvoice = new List<EmptyContainerListForInvoice>();
            try
            {
                //ShippingLineId, ShippingLineName, GSTNo, CFSCode, ContainerNo, EmptyDate, Address, StateCode, StateName
                while (Result.Read())
                {
                    Status = 1;

                    objListForInvoice.Add(new EmptyContainerListForInvoice()
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        EmptyDate = Convert.ToString(Result["EmptyDate"]),
                        Address = Convert.ToString(Result["Address"]),
                        StateName = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objListForInvoice;
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
        public void GetEmptyContByEntryId(int EntryId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = DateTime.Now.ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "GateInId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetEmptyContainerListForInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.PaymentSheetContainer> objPaymentSheetContainer = new List<Areas.Import.Models.PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new Areas.Import.Models.PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Selected = true,
                        ArrivalDt = Convert.ToString(Result["ArrivalDt"]),
                        IsHaz = Convert.ToString(Result["IsHaz"]),
                        Size = Convert.ToString(Result["Size"])
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
        public void GetEmptyContPaymentSheet(string InvoiceDate, int DestuffingAppId, string InvoiceType, String SEZ, string ContainerXML, int InvoiceId, string InvoiceFor, int PartyId)
        {


            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 20, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceFor", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceFor });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Size = 20, Value = PartyId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            Chn_ImportPaymentSheet objInvoice = new Chn_ImportPaymentSheet();
            DataSet ds = DataAccess.ExecuteDataSet("GetEmptyContPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {
                //IDataReader Result = DataAccess.ExecuteDataReader("GetYardPaymentSheetCWCCharges", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                // while (Result.Read())
                foreach (DataRow Result in ds.Tables[0].Rows)
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
                //if (Result.NextResult())
                //{
                //while (Result.Read())
                foreach (DataRow Result in ds.Tables[1].Rows)
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
                //}

                // if (Result.NextResult())
                //{

                // while (Result.Read())
                foreach (DataRow Result in ds.Tables[2].Rows)
                {
                    objInvoice.lstPrePaymentCont.Add(new Chn_ImpPreInvoiceContainer
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
                    objInvoice.lstPostPaymentCont.Add(new Chn_ImpPostPaymentContainer
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
                // }
                //if (Result.NextResult())
                //{
                //while (Result.Read())
                foreach (DataRow Result in ds.Tables[3].Rows)
                {
                    objInvoice.lstPostPaymentChrg.Add(new Chn_ImpPostPaymentChrg
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
                        OperationId = Convert.ToInt32(Result["OperationId"]),

                    });
                }


                //}

                //if (Result.NextResult())
                //{
                //while (Result.Read())
                foreach (DataRow Result in ds.Tables[4].Rows)
                {
                    objInvoice.lstContWiseAmount.Add(new Chn_ImpContainerWiseAmount
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
                //}

                //if (Result.NextResult())
                //{
                //while (Result.Read())
                foreach (DataRow Result in ds.Tables[5].Rows)
                {
                    objInvoice.lstOperationCFSCodeWiseAmount.Add(new Chn_ImpOperationCFSCodeWiseAmount
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
                        DocumentNo = Result["SBNo"].ToString(),
                        DocumentType = "BOE",
                        DocumentDate = Result["SBDate"].ToString(),
                        Clause = Convert.ToString(Result["Clause"]),
                    });
                }

                foreach (DataRow Result in ds.Tables[6].Rows)
                {
                    objInvoice.lstPostPaymentChrgBreakup.Add(new Chn_ImpPostPaymentChargebreakupdate
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
                        EndDate = Result["EndDate"].ToString(),
                        //Clause = Convert.ToString(Result["Clause"]),
                    });
                }


                //}
                //if (Result.NextResult())
                //{
                // while (Result.Read())
                //foreach (DataRow Result in ds.Tables[6].Rows)
                //{
                //    objInvoice.ActualApplicable.Add(Convert.ToString(Result["Clause"]));
                //}
                //}

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
                ds.Dispose();
            }
        }
        #endregion


        #region Yard Invoice
        public void GetYardPaymentSheet(string InvoiceDate, string DeliveryDate, String SEZ, int AppraisementId, string InvoiceType,
            string ContainerXML, int InvoiceId, int OTHours, int PartyId, int PayeeId, int isdirect, int NoOfVehicles, int Distance, int OwnMovement, int InsuredParty, int YardToBond, int DirectDelivery, int ExaminationType, int Weighment, int DiscountPer,int Scanning,int FactoryDestuffing,int DirectDestuffing)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(DeliveryDate) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Value = OTHours });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.Int32, Value = NoOfVehicles });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value = Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEAL", MySqlDbType = MySqlDbType.Int32, Value = OwnMovement });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_Insured", MySqlDbType = MySqlDbType.Int32, Value = InsuredParty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsInsured", MySqlDbType = MySqlDbType.Int32, Value = InsuredParty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_YardToBond", MySqlDbType = MySqlDbType.Int32, Value = YardToBond });

            LstParam.Add(new MySqlParameter { ParameterName = "in_isdirect", MySqlDbType = MySqlDbType.Int32, Value = isdirect });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DirectDelivery", MySqlDbType = MySqlDbType.Int32, Value = DirectDelivery });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExaminationType", MySqlDbType = MySqlDbType.Int32, Value = ExaminationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weighment", MySqlDbType = MySqlDbType.Int32, Value = Weighment });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DiscountPer", MySqlDbType = MySqlDbType.Int32, Value = DiscountPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Scanning", MySqlDbType = MySqlDbType.Int32, Value = Scanning });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FactoryDestuffing", MySqlDbType = MySqlDbType.Int32, Value = FactoryDestuffing });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DirectDestuffing", MySqlDbType = MySqlDbType.Int32, Value = DirectDestuffing });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            Chn_ImportPaymentSheet objInvoice = new Chn_ImportPaymentSheet();
            IDataReader Result = DataAccess.ExecuteDataReader("GetYardPaymentSheet", CommandType.StoredProcedure, DParam);
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

                    }
                }

                if (Result.NextResult())
                {

                    while (Result.Read())
                    {
                        objInvoice.lstPrePaymentCont.Add(new Chn_ImpPreInvoiceContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            OBLNo = Result["OBLNo"].ToString(),
                            SealCutDate = Result["SealCutDate"].ToString(),
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
                        objInvoice.lstPostPaymentCont.Add(new Chn_ImpPostPaymentContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            OBLNo = Result["OBLNo"].ToString(),
                            SealCutDate = Result["SealCutDate"].ToString(),
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
                            ParkDays = Convert.ToInt32(Result["ParkDays"]),
                            LockDays = Convert.ToInt32(Result["LockDays"]),
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new Chn_ImpPostPaymentChrg
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
                            OperationId = Convert.ToInt32(Result["OperationId"]),

                        });
                    }


                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContWiseAmount.Add(new Chn_ImpContainerWiseAmount
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
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new Chn_ImpOperationCFSCodeWiseAmount
                        {
                            InvoiceId = InvoiceId,
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            OperationId = Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"].ToString(),
                            Quantity = Convert.ToDecimal(Result["Quantity"]),
                            Rate = Convert.ToDecimal(Result["Rate"]),
                            Clause = Result["Clause"].ToString(),
                            DocumentNo = Result["SBNo"] == System.DBNull.Value ? "" : Result["SBNo"].ToString(),
                            DocumentType = "Yard",
                            DocumentDate = Result["SBDate"] == System.DBNull.Value ? System.DateTime.Now.ToString("yyyy/MM/dd") : Result["SBDate"].ToString(),
                            Amount = Convert.ToDecimal(Result["Amount"]),
                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.PaymentMode = Result["IN_MODE"].ToString();
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.NDays = Result["NDays"].ToString();
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        objInvoice.lstPostPaymentChrgBreakup.Add(new Chn_ImpPostPaymentChargebreakupdate
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
                            EndDate = Result["EndDate"].ToString(),

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
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.IsPartyStateInCompState = Convert.ToBoolean(Result["IsLocalGST"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        //  objInvoice.IsPartyStateInCompState = Convert.ToBoolean(Result["IsLocalGST"]);
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstADDPostPaymentChrg.Add(new CwcExim.Areas.Import.Models.Chn_ADDCWCImpPostPaymentCharge
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
                            OperationId = Convert.ToInt32(Result["OperationId"].ToString()),
                            ADDCWC = Convert.ToInt32(Result["AddCWC"])
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





        public void GetIRNForInvoice(String InvoiceNo, String TypeOfInv)
        {
            EinvoicePayload Obj = new EinvoicePayload();

            TranDtls tr = new TranDtls();
            DocDtls doc = new DocDtls();
            SellerDtls seller = new SellerDtls();
            BuyerDtls buyer = new BuyerDtls();
            DispDtls disp = new DispDtls();

            ShipDtls ship = new ShipDtls();
            BchDtls btc = new BchDtls();
            AttribDtls attr = new AttribDtls();
            ValDtls vald = new ValDtls();
            PayDtls payd = new PayDtls();
            RefDtls refd = new RefDtls();
            PrecDocDtls pred = new PrecDocDtls();
            ContrDtls Cont = new ContrDtls();
            AddlDocDtls addl = new AddlDocDtls();
            ExpDtls expd = new ExpDtls();
            EwbDtls ewb = new EwbDtls();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_type", MySqlDbType = MySqlDbType.VarChar, Value = TypeOfInv });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();


            log.Info("Before Calling Stored Procedure GetIRNDetails");
            IDataReader Result = DataAccess.ExecuteDataReader("GetIRNDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    Obj.Version = Result["Version"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        tr.TaxSch = Result["TaxSch"].ToString();
                        tr.SupTyp = Result["SupTyp"].ToString();
                        tr.RegRev = Result["RegRev"].ToString();
                        tr.EcmGstin = null;

                        tr.IgstOnIntra = Result["IgstOnIntra"].ToString();

                    }
                }




                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        doc.Typ = Result["Typ"].ToString();
                        doc.No = Result["No"].ToString();
                        doc.Dt = Result["Dt"].ToString();


                    }
                }



                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        seller.Gstin = Result["Gstin"].ToString();
                        seller.LglNm = Result["LglNm"].ToString();
                        seller.TrdNm = Result["TrdNm"].ToString() == "" ? null : Result["TrdNm"].ToString();
                        seller.Addr1 = Result["Addr1"].ToString();
                        seller.Addr2 = null;
                        seller.Loc = Result["Loc"].ToString();
                        seller.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        seller.Stcd = Result["Stcd"].ToString();
                        seller.Ph = null;// Result["Ph"].ToString().Length > 12 ? Result["Ph"].ToString().Substring(3, Result["Ph"].ToString().Length) : Result["Ph"].ToString();
                        seller.Em = null;//Result["Em"].ToString();
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        buyer.Gstin = Result["Gstin"].ToString();
                        buyer.LglNm = Result["LglNm"].ToString();
                        buyer.TrdNm = Result["TrdNm"].ToString() == "" ? null : Result["TrdNm"].ToString();
                        buyer.Addr1 = Result["Addr1"].ToString();
                        buyer.Addr2 = null;
                        buyer.Pos = Convert.ToString(Result["Stcd"]);
                        buyer.Loc = Result["Loc"].ToString();
                        buyer.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        buyer.Stcd = Result["Stcd"].ToString();
                        buyer.Ph = null;//Result["Ph"].ToString().Length > 12 ? Result["Ph"].ToString().Substring(3, Result["Ph"].ToString().Length) : Result["Ph"].ToString();
                        buyer.Em = null; //Result["Em"].ToString();
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        disp.Nm = Result["Nm"].ToString();
                        disp.Addr1 = Result["Addr1"].ToString();
                        disp.Addr2 = null;
                        disp.Loc = Result["Loc"].ToString();
                        disp.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        disp.Stcd = Result["Stcd"].ToString();

                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        ship.Gstin = Result["Gstin"].ToString();
                        ship.LglNm = Result["LglNm"].ToString();
                        ship.TrdNm = Result["TrdNm"].ToString() == "" ? null : Result["TrdNm"].ToString();
                        ship.Addr1 = Result["Addr1"].ToString();
                        ship.Addr2 = null;
                        ship.Loc = Result["Loc"].ToString();
                        ship.Pin = Convert.ToInt32(Result["Pin"].ToString());
                        ship.Stcd = Result["Stcd"].ToString();

                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        btc.Nm = Result["Nm"].ToString();
                        btc.ExpDt = Result["ExpDt"].ToString();
                        btc.WrDt = Result["WrDt"].ToString();
                        // attr.Nm = Result["Nmm"].ToString();
                        // attr.Val= Result["Val"].ToString();
                        attr = null;
                        Obj.ItemList.Add(new ItemList
                        {
                            SlNo = Result["SlNo"].ToString(),
                            PrdDesc = null,// Result["PrdDesc"].ToString(),
                            IsServc = Result["IsServc"].ToString(),
                            HsnCd = Result["HsnCd"].ToString(),
                            Barcde = Result["Barcde"].ToString(),
                            Qty = Convert.ToDecimal(Result["Qty"].ToString()),
                            FreeQty = Convert.ToInt32(Result["FreeQty"].ToString()),
                            Unit = null,// Result["Unit"].ToString(),
                            UnitPrice = Convert.ToDecimal(Result["UnitPrice"].ToString()),
                            TotAmt = Convert.ToDecimal(Result["TotAmt"].ToString()),
                            Discount = Convert.ToInt32(Result["Discount"].ToString()),
                            PreTaxVal = Convert.ToInt32(Result["PreTaxVal"].ToString()),
                            AssAmt = Convert.ToDecimal(Result["AssAmt"].ToString()),
                            GstRt = Convert.ToDecimal(Result["GstRt"].ToString()),
                            IgstAmt = Convert.ToDecimal(Result["IgstAmt"].ToString()),
                            CgstAmt = Convert.ToDecimal(Result["CgstAmt"].ToString()),
                            SgstAmt = Convert.ToDecimal(Result["SgstAmt"].ToString()),
                            CesRt = Convert.ToInt32(Result["CesRt"].ToString()),
                            CesAmt = Convert.ToDecimal(Result["CesAmt"].ToString()),
                            CesNonAdvlAmt = Convert.ToInt32(Result["CesNonAdvlAmt"].ToString()),
                            StateCesRt = Convert.ToInt32(Result["StateCesRt"].ToString()),
                            StateCesAmt = Convert.ToDecimal(Result["StateCesAmt"].ToString()),
                            StateCesNonAdvlAmt = Convert.ToInt32(Result["StateCesNonAdvlAmt"].ToString()),
                            OthChrg = Convert.ToInt32(Result["OthChrg"].ToString()),
                            TotItemVal = Convert.ToDecimal(Result["TotItemVal"].ToString()),
                            OrdLineRef = Convert.ToString(Result["OrdLineRef"].ToString()),
                            OrgCntry = Result["OrgCntry"].ToString(),
                            PrdSlNo = Convert.ToString(Result["PrdSlNo"].ToString()),
                            BchDtls = null,
                            AttribDtls = null,

                        });



                    }


                }



                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        vald.AssVal = Convert.ToDecimal(Result["AssVal"].ToString());
                        vald.CgstVal = Convert.ToDecimal(Result["CgstVal"].ToString());
                        vald.SgstVal = Convert.ToDecimal(Result["SgstVal"].ToString());
                        vald.IgstVal = Convert.ToDecimal(Result["IgstVal"].ToString());
                        vald.CesVal = Convert.ToDecimal(Result["CesVal"].ToString());
                        vald.StCesVal = Convert.ToDecimal(Result["StCesVal"].ToString());
                        vald.Discount = Convert.ToDecimal(Result["Discount"].ToString());
                        vald.OthChrg = Convert.ToDecimal(Result["OthChrg"].ToString());
                        vald.RndOffAmt = Convert.ToDecimal(Result["RndOffAmt"].ToString());
                        vald.TotInvVal = Convert.ToDecimal(Result["TotInvVal"].ToString());
                        vald.TotInvValFc = Convert.ToDecimal(Result["TotInvValFc"].ToString());


                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        payd.Nm = Result["Nm"].ToString();
                        payd.AccDet = Result["AccDet"].ToString();
                        payd.Mode = Result["Mode"].ToString();
                        payd.FinInsBr = Result["FinInsBr"].ToString();
                        payd.PayTerm = Result["PayTerm"].ToString();
                        payd.PayInstr = Result["PayInstr"].ToString();
                        payd.CrTrn = Result["CrTrn"].ToString();
                        payd.DirDr = Result["DirDr"].ToString();
                        payd.CrDay = Result["CrDay"].ToString();
                        payd.PaidAmt = Result["PaidAmt"].ToString();
                        payd.PaymtDue = Result["PaymtDue"].ToString();


                    }
                }


                if (Result.NextResult())
                {
                    DocPerdDtls docp = new DocPerdDtls();
                    while (Result.Read())
                    {
                        refd.InvRm = Result["InvRm"].ToString();
                        docp.InvStDt = Result["InvStDt"].ToString();
                        docp.InvEndDt = Result["InvEndDt"].ToString();
                        refd.DocPerdDtls = docp;


                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        pred.InvNo = Result["InvNo"].ToString();
                        pred.InvDt = Result["InvDt"].ToString();
                        pred.OthRefNo = Result["OthRefNo"].ToString();


                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Cont.RecAdvRefr = Result["RecAdvRefr"].ToString();
                        Cont.RecAdvDt = Result["RecAdvDt"].ToString();
                        Cont.TendRefr = Result["TendRefr"].ToString();
                        Cont.ContrRefr = Result["ContrRefr"].ToString();
                        Cont.ExtRefr = Result["ExtRefr"].ToString();
                        Cont.ProjRefr = Result["ProjRefr"].ToString();
                        Cont.PORefr = Result["PORefr"].ToString();
                        Cont.PORefDt = Result["PORefDt"].ToString();



                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        addl.Url = Result["Url"].ToString();
                        addl.Docs = Result["Docs"].ToString();
                        addl.Info = Result["Info"].ToString();




                    }
                }

                if (Result.NextResult())
                {
                    // while (Result.Read())
                    // {



                    // Obj.ExpDtls.Add(new ExpDtls
                    // {
                    // ShipBNo = Result["ShipBNo"].ToString(),
                    // ShipBDt = Result["ShipBDt"].ToString(),
                    // Port = Result["Port"].ToString(),
                    // RefClm = Result["RefClm"].ToString(),
                    // ForCur = Result["ForCur"].ToString(),
                    // CntCode = Result["CntCode"].ToString(),
                    // ExpDuty = Result["ExpDuty"].ToString(),


                    // });












                    // }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ewb.TransId = Result["TransId"].ToString();
                        ewb.TransName = Result["TransName"].ToString();
                        ewb.Distance = Result["Distance"].ToString();
                        ewb.TransDocNo = Result["TransDocNo"].ToString();
                        ewb.TransDocDt = Result["TransDocDt"].ToString();
                        ewb.VehNo = Result["VehNo"].ToString();
                        ewb.VehType = Result["VehType"].ToString();
                        ewb.TransMode = Result["TransMode"].ToString();




                    }
                }

                //if (Result.NextResult())
                //{
                // while (Result.Read())
                // {
                // hp.ClientID = Result["ClientID"].ToString();
                // hp.ClientSecret = Result["ClientSecret"].ToString();
                // hp.GSTIN = Result["GSTIN"].ToString();
                // hp.UserName = Result["UserName"].ToString();
                // hp.Password = Result["Password"].ToString();
                // hp.AppKey = "";



                // }
                //}
                Obj.TranDtls = tr;
                Obj.DocDtls = doc;
                Obj.SellerDtls = seller;
                Obj.BuyerDtls = buyer;
                Obj.DispDtls = disp;
                Obj.ShipDtls = ship;
                Obj.AttribDtls = attr;
                Obj.ValDtls = vald;
                Obj.PayDtls = null;
                Obj.RefDtls = null;
                Obj.PrecDocDtls = pred;
                Obj.ContrDtls = Cont;
                Obj.AddlDocDtls = null;
                Obj.ExpDtls = null;
                // Obj.ExpDtls = expd;
                Obj.EwbDtls = null;

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Obj;

                log.Info("After Calling Stored Procedure GetIRNDetails");

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
        public void GetIRNForB2CInvoice(String InvoiceNo, String TypeOfInv)
        {
            CHN_IrnB2CDetails Obj = new CHN_IrnB2CDetails();

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_type", MySqlDbType = MySqlDbType.VarChar, Value = TypeOfInv });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetIrnB2CDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    Obj.SellerGstin = Result["SellerGST"].ToString();
                    Obj.BuyerGstin = Result["BuyerGstin"].ToString();
                    Obj.DocNo = Result["DocNo"].ToString();
                    Obj.DocTyp = Result["DocTyp"].ToString();
                    Obj.DocDt = Result["DocDt"].ToString();
                    Obj.TotInvVal = (Int32)Convert.ToDecimal(Result["TotInvVal"].ToString());
                    Obj.ItemCnt = Convert.ToInt32(Result["ItemCnt"].ToString());
                    Obj.MainHsnCode = Result["MainHsnCode"].ToString();
                    //  Obj.Irn = Result["Irn"].ToString();
                    //   Obj.IrnDt = Result["IrnDt"].ToString();
                    //  Obj.iss = Result["iss"].ToString();
                    Obj.ver = Convert.ToString(Result["ver"]);

                    Obj.tier = Convert.ToString(Result["tier"]);
                    Obj.tid = Convert.ToString(Result["tid"]);
                    //Obj.sign = Convert.ToString(Result["sign"]);
                    Obj.SGST = Convert.ToDecimal(Result["SGSTAmt"]);
                    Obj.CGST = Convert.ToDecimal(Result["CGSTAmt"]);
                    Obj.IGST = Convert.ToDecimal(Result["IGSTAmt"]);
                    Obj.CESS = 0;

                    Obj.qrMedium = Convert.ToInt32(Result["qrMedium"]);
                    Obj.QRexpire = Convert.ToString(Result["QRexpireDays"]);

                    Obj.pinCode = Convert.ToInt32(Result["pinCode"]);
                    Obj.pa = Convert.ToString(Result["pa"]);
                    Obj.orgId = Convert.ToInt32(Result["orgId"]);
                    Obj.mtid = Convert.ToString(Result["mtid"]);
                    Obj.msid = Convert.ToString(Result["msid"]);
                    Obj.mode = Convert.ToInt32(Result["mode"]);
                    Obj.mc = Convert.ToString(Result["mc"]);
                    Obj.mam = Convert.ToString(Result["InvoiceAmt"]);
                    Obj.GSTPCT = Convert.ToInt32(Result["IGSTPer"]);
                    Obj.GSTIncentive = 0;
                    Obj.mid = Convert.ToString(Result["mid"]);
                    Obj.InvoiceName = Convert.ToString(Result["PartyName"]);
                    Obj.tr = Convert.ToString(Result["tr"]);
                    Obj.pn = Convert.ToString(Result["pn"]);
                    Obj.gstIn = Convert.ToString(Result["SellerGST"]);

                }
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = Obj;

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
        public void AddEditIRNResponsec(IrnResponse objOBL, string InvoiceNo)
        {
            string GeneratedClientId = "";
            log.Info("Before Calling Stored Procedure Addeditirnresponce");
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AckNo", MySqlDbType = MySqlDbType.String, Value = objOBL.AckNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AckDt", MySqlDbType = MySqlDbType.String, Value = objOBL.AckDt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_irn", MySqlDbType = MySqlDbType.String, Value = objOBL.irn }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SignedInvoice", MySqlDbType = MySqlDbType.String, Value = objOBL.SignedInvoice });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SignedQRCode", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.SignedQRCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_QRCodeImageBase64", MySqlDbType = MySqlDbType.LongText, Value = objOBL.QRCodeImageBase64 });// Value = Convert.ToDateTime(objOBL.IGM_Date).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IrnStatus", MySqlDbType = MySqlDbType.String, Value = objOBL.IrnStatus });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EwbNo", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.EwbNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EwbDt", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.EwbDt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EwbValidTill", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.EwbValidTill });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IrnResponsecol", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });

            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("Addeditirnresponce", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    log.Info("After Calling Stored Procedure Addeditirnresponce Record Saved Successfully");
                    _DBResponse.Message = (Result == 1) ? "IRN Saved Successfully" : "IRN Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else
                {
                    log.Info("After Calling Stored Procedure Error" + " Invoice No " + InvoiceNo + " signed Invoice: " + objOBL.SignedInvoice + " SignedQRCode " + objOBL.SignedQRCode);
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Error";
                    _DBResponse.Status = 0;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " :\r\n" + ex.StackTrace);
                _DBResponse.Data = null;
                _DBResponse.Message = "Error";
                _DBResponse.Status = 0;
            }
        }

        public void AddEditIRNB2C(String IRN, B2cQRCodeResponse QrCode, string InvoiceNo)
        {
            string GeneratedClientId = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_irn", MySqlDbType = MySqlDbType.String, Value = IRN });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SignedQRCod", MySqlDbType = MySqlDbType.LongText, Value = QrCode.QrCodeJson });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });

            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("AddeditirnB2Cresponce", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = (Result == 1) ? "IRN Saved Successfully" : "IRN Entry Updated Successfully";
                    _DBResponse.Status = Result;
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "This Container information is already exists!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 4)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update as seal cutting done!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 5)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Can not update as job order by road done!";
                    _DBResponse.Status = Result;
                }
                else if (Result == 6)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Message = "Duplicate OBL No.!";
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

        public void GetHeaderIRNForYard()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            log.Info("Before Calling Stored Procedure GetIRNheaderDetails");
            IDataReader Result = DataAccess.ExecuteDataReader("GetIRNheaderDetails", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            try
            {


                while (Result.Read())
                {
                    hp.ClientID = Result["ClientID"].ToString();
                    hp.ClientSecret = Result["ClientSecret"].ToString();
                    hp.GSTIN = Result["GSTIN"].ToString();
                    hp.UserName = Result["UserName"].ToString();
                    hp.Password = Result["Password"].ToString();
                    hp.AppKey = "";


                }

                log.Info("after Calling Stored Procedure GetIRNheaderDetails");

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = hp;

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


        public void AddEditInvoice(Chn_ImportPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML, string ChargesBreakupXML,
            int BranchId, int Vehicle, int Uid, int Direct, string SEZ, int ParkDays, int Lockdays,
           string Module, string CargoXML = "")
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExaminationType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExaminationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weighment", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Weighment });

            LstParam.Add(new MySqlParameter { ParameterName = "in_SealCharge", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.OwnMovement });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Scanning", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Scanning });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FactoryDestuffing", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.FactoryDestuffing });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsDirect", MySqlDbType = MySqlDbType.Int32, Value = Direct });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ParkDays", MySqlDbType = MySqlDbType.Int32, Value = ParkDays });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LockDays", MySqlDbType = MySqlDbType.Int32, Value = Lockdays });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Vehicle", MySqlDbType = MySqlDbType.Int32, Value = Vehicle });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceYard", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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
        public void GetImpPaymentPartyForFCLPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPartyForFCLPage", CommandType.StoredProcedure, Dparam);
            IList<Chn_ImpPartyForpage> lstParty = new List<Chn_ImpPartyForpage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {

                    Status = 1;
                    lstParty.Add(new Chn_ImpPartyForpage
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]).Replace("\r\n", ""),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        //InsuredToDate = Convert.ToString(Result["InsuredDateTo"]),
                        //InsuredFrmDate = Convert.ToString(Result["InsuredDateFrm"]),
                        // IsTransporter = Convert.ToBoolean(Result["transporter"]),
                        // IsInsured = Convert.ToBoolean(Result["IsInsured"]),
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

        public void GetAppraismentRequestForPaymentSheet(int AppraisementAppId = 0, string Type = "I")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = AppraisementAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppraismentRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Chn_PaySheetStuffingRequest> objPaySheetStuffing = new List<Chn_PaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new Chn_PaySheetStuffingRequest()
                    {
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Convert.ToString(Result["CHAName"]),

                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        PartyName = Convert.ToString(Result["PartyName"]),

                        Billtoparty = Result["BilltoParty"].ToString(),
                        IsInsured = Result["IsInsured"].ToString(),
                        Transporter = Result["transporter"].ToString(),
                        InsuredFrom = Result["InsuredDateFrm"].ToString(),
                        InsuredTo = Result["InsuredDateTo"].ToString(),

                        CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                        Address = Convert.ToString(Result["Address"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        StuffingReqId = Convert.ToInt32(Result["CustomAppraisementId"]),
                        DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                        StuffingReqNo = Convert.ToString(Result["AppraisementNo"]),
                        StuffingReqDate = Convert.ToString(Result["AppraisementDate"])

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

        public void YardListOfChaForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfChaForPage", CommandType.StoredProcedure, Dparam);
            IList<Chn_CHAForPage> lstCHA = new List<Chn_CHAForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstCHA.Add(new Chn_CHAForPage
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Result["EximTraderName"].ToString(),
                        //BillToParty = Convert.ToBoolean(Convert.ToInt32(Result["BilltoParty"].ToString())),
                        //IsInsured = Convert.ToBoolean(Convert.ToInt32(Result["IsInsured"].ToString())),
                        //IsTransporter = Convert.ToBoolean(Convert.ToInt32(Result["transporter"].ToString())),
                        //InsuredFrmdate = Result["InsuredDateFrm"].ToString(),
                        //InsuredTodate = Result["InsuredDateTo"].ToString(),
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
                    _DBResponse.Data = new { lstCHA, State };
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

        public void GetContainerForPaymentSheet(int AppraisementAppId, string Type = "I")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = AppraisementAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppraismentRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.PaymentSheetContainer> objPaymentSheetContainer = new List<Areas.Import.Models.PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new Areas.Import.Models.PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Selected = false,
                        ArrivalDt = Convert.ToString(Result["ArrivalDt"]),
                        IsHaz = Convert.ToString(Result["IsHaz"]),
                        Size = Convert.ToString(Result["Size"]),
                        OBLNo = Convert.ToString(Result["OBLNo"]),
                        SealCutDate = Convert.ToString(Result["SealCutDate"]),
                        NoOfPkg = Convert.ToInt32(Result["NoOfPackages"]),
                        GrWait = Convert.ToDecimal(Result["GrossWeight"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheetContainer.GroupBy(o => o.CFSCode).Select(o => o.First()).ToList();
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


        //public void GetContainerForPaymentSheet(int AppraisementAppId, string Type = "I")
        //{
        //    int Status = 0;
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = AppraisementAppId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = Type });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetAppraismentRequestForPaymentSheet", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    IList<PaymentSheetContainer> objPaymentSheetContainer = new List<PaymentSheetContainer>();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            objPaymentSheetContainer.Add(new PaymentSheetContainer()
        //            {
        //                CFSCode = Convert.ToString(Result["CFSCode"]),
        //                ContainerNo = Convert.ToString(Result["ContainerNo"]),
        //                Selected = false,
        //                ArrivalDt = Convert.ToString(Result["ArrivalDt"]),
        //                IsHaz = Convert.ToString(Result["IsHaz"]),
        //                Size = Convert.ToString(Result["Size"]),
        //                OBLNo = Convert.ToString(Result["OBLNo"]),
        //                SealCutDate = Convert.ToString(Result["SealCutDate"]),
        //                NoOfPkg = Convert.ToInt32(Result["NoOfPackages"]),
        //                GrWait = Convert.ToDecimal(Result["GrossWeight"])
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = objPaymentSheetContainer.GroupBy(o => o.CFSCode).Select(o => o.First()).ToList();
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
        #endregion

        #region SignleClick DeliveryPaymentSheet
        public void GetDeliveryPaymentSheetSingle(string InvoiceType, int DestuffingAppId, String SEZ, string DeliveryDate, string InvoiceDate,
            int InvoiceId, string XMLText, int PartyId, int PayeeId, int vhecleNo, int vhecleNoUn, bool IsInsured, bool IsBilltoparty, bool Transporter, bool ImportToBond, bool SealCharge, int Cargo, int OTHours = 0, int OblFlag = 0, int ParkingHours = 0, int LockingHours = 0, int ExaminationType = 0, int Weighment = 0,int Scanned=0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryXML", MySqlDbType = MySqlDbType.Text, Value = XMLText });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = DeliveryDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = InvoiceDate });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Value = OTHours });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.Int32, Value = vhecleNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNoUn", MySqlDbType = MySqlDbType.Int32, Value = vhecleNoUn });

            LstParam.Add(new MySqlParameter { ParameterName = "in_BillToParty", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(IsBilltoparty == false ? 0 : 1) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Cargo", MySqlDbType = MySqlDbType.Int32, Value = Cargo });

            LstParam.Add(new MySqlParameter { ParameterName = "in_IsInsured", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(IsInsured == false ? 0 : 1) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsTransporter", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Transporter == false ? 0 : 1) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImportToBond", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ImportToBond == false ? 0 : 1) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealCharge", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(SealCharge == false ? 0 : 1) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_OblFlag", MySqlDbType = MySqlDbType.Int32, Value = OblFlag });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ParkingHours", MySqlDbType = MySqlDbType.Int32, Value = ParkingHours });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LockingHours", MySqlDbType = MySqlDbType.Int32, Value = LockingHours });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExaminationType", MySqlDbType = MySqlDbType.Int32, Value = ExaminationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weighment", MySqlDbType = MySqlDbType.Int32, Value = Weighment });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Scanned", MySqlDbType = MySqlDbType.Int32, Value = Scanned });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();



            //getsingleclickdeliverypaymentsheet_new1

            //getsingleclickdeliverypaymentsheet_v2
            CHNInvoiceGodown objInvoice = new CHNInvoiceGodown();
            IDataReader Result = DataAccess.ExecuteDataReader("getsingleclickdeliverypaymentsheet", CommandType.StoredProcedure, DParam);
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
                        objInvoice.lstPrePaymentCont.Add(new Chn_ImpPreInvoiceContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWeight = Result["GrossWeight"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerPack = Result["WtPerPack"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Result["CIFValue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Result["DelDuty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["DelDuty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Result["RMS"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Result["DeliveryType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Result["Insured"] == DBNull.Value ? 0 : Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Result["HeavyScrap"] == DBNull.Value ? 0 : Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Result["AppraisementPerct"] == DBNull.Value ? 0 : Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = " ",
                            CartingDate = Result["CartingDate"].ToString(),
                            DestuffingDate = Result["DestuffingDate"].ToString(),
                            StuffingDate = Result["StuffingDate"].ToString(),
                            ApproveOn = "0",
                            BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            LineNo = Result["LineNo"].ToString(),
                            OperationType = 0,
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                        });
                        objInvoice.lstPostPaymentCont.Add(new Chn_ImpPostPaymentContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"].ToString(),
                            GrossWt = Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["DelDuty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Result["RMS"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Result["DeliveryType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Result["Insured"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Result["HeavyScrap"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"]),
                            LCLFCL = " ",
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
                        objInvoice.lstContWiseAmount.Add(new Chn_ImpContainerWiseAmount
                        {
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            EntryFee = Result["EntryFee"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["EntryFee"]),
                            CstmRevenue = Result["CstmRevenue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CstmRevenue"]),
                            GrEmpty = Result["GrEmpty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrEmpty"]),
                            GrLoaded = Result["GrLoaded"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrLoaded"]),
                            ReeferCharge = Result["ReeferCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["ReeferCharge"]),
                            StorageCharge = Result["StorageCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["StorageCharge"]),
                            InsuranceCharge = Result["InsuranceCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["InsuranceCharge"]),
                            PortCharge = Result["PortCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["PortCharge"]),
                            WeighmentCharge = Result["WeighmentCharge"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WeighmentCharge"]),
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
                        objInvoice.lstInvoiceCargo.Add(new CHNInvoiceCargo
                        {
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            BOLDate = Result["BOLDate"].ToString(),
                            BOLNo = Result["BOLNo"].ToString(),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            CargoType = Result["CargoType"].ToString() == "" ? 0 : Convert.ToInt32(Result["CargoType"]),
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
                            LineNo = Result["LineNo"].ToString() == null ? "" : Result["LineNo"].ToString()
                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new Chn_ImpPostPaymentChrg
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
                            OperationId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                            ADDCWC = Convert.ToInt32(Result["AddCWC"])

                        });
                    }
                }

                if (Result.NextResult())
                {
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new Chn_ImpOperationCFSCodeWiseAmount
                        {
                            InvoiceId = InvoiceId,
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            Clause = Result["Clause"] == System.DBNull.Value ? "" : Result["Clause"].ToString(),
                            ContainerNo = Result["ContainerNo"] == System.DBNull.Value ? "" : Result["ContainerNo"].ToString(),
                            Size = Result["Size"] == System.DBNull.Value ? "" : Result["Size"].ToString(),
                            OperationId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                            ChargeType = Result["ChargeType"] == System.DBNull.Value ? "" : Result["ChargeType"].ToString(),
                            Quantity = Result["Quantity"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Quantity"]),
                            Rate = Result["Rate"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Rate"]),
                            Amount = Result["Amount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Amount"]),
                            DocumentNo = Result["SBNo"] == System.DBNull.Value ? "" : Result["SBNo"].ToString(),
                            DocumentType = "Godown",
                            DocumentDate = Result["SBDate"].ToString()// == System.DBNull.Value ? System.DateTime.Now.ToString("yyyy/MM/dd") : Result["SBDate"].ToString(),

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
                    if (Result.Read())
                    {
                        objInvoice.PaymentMode = Result["IN_MODE"].ToString();
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {

                        objInvoice.lstPostPaymentChrgBreakup.Add(new Chn_ImpPostPaymentChargebreakupdate
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
                            EndDate = Result["EndDate"].ToString(),

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
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.IsPartyStateInCompState = Convert.ToBoolean(Result["IsLocalGST"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstADDPostPaymentChrg.Add(new Chn_ADDCWCImpPostPaymentCharge
                        {
                            ChargeId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
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
                            OperationId = Result["OperationId"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["OperationId"]),
                            ADDCWC = Convert.ToInt32(Result["AddCWC"])

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
        public void GetGSTValue(int PartyId)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });

            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGSTforSClick", CommandType.StoredProcedure, Dparam);
            String GSTNo = "";
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;


                    GSTNo = Result["GSTNO"].ToString();

                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = GSTNo;
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
        public void GetDestuffEntryNo(int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Uid) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffEntryNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DestuffingEntryNoList> LstDestuffEntryNo = new List<DestuffingEntryNoList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffEntryNo.Add(new DestuffingEntryNoList
                    {
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingId = Convert.ToInt32(Result["DestuffingId"]),
                        ImporterName = Result["ImporterName"].ToString(),
                        ImporterId = Convert.ToInt32(Result["ImporterId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDestuffEntryNo;
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
        public void AddEditMergeDeliveryApplication(WFLDMergeDeliveryIssueViewModel ObjDeliveryApp, string DeliveryXml, String ObjDeliveryOrd)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";


            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.DeliveryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryXML", MySqlDbType = MySqlDbType.Text, Value = DeliveryXml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrdXML", MySqlDbType = MySqlDbType.Text, Value = ObjDeliveryOrd });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = ObjDeliveryApp.DeliApp.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjDeliveryApp.DeliApp.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.OTHr });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.ImporterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output, Value = ReturnObj });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            WFLDmergedet pdet = new WFLDmergedet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMergeDeliveryApplication", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    pdet.Id = Convert.ToInt32(GeneratedClientId);
                    pdet.AppNo = ReturnObj;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Delivery Application Details  Saved Successfully";
                    _DBResponse.Data = pdet;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Delivery Application Details Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Invoice cannot be generated due to sd balance is low";
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
        public void AddEditSingleMergeDeliveryApplication(CHNMergeDeliveryIssueViewModel ObjDeliveryApp,
            string DeliveryAppDtlXml, string DeliveryOrdDtlXml,
            string lstPrePaymentContXML, string lstPostPaymentContXML,
                  string lstPostPaymentChrgXML, string lstContWiseAmountXML,
                  string lstOperationCFSCodeWiseAmountXML, string lstPostPaymentChrgBreakupXML, string lstInvoiceCargoXML, string DeliveryGodownDtlXml)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";


            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.DeliveryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = ObjDeliveryApp.DeliApp.SEZ });// ObjDeliveryApp.DeliApp.DeliveryDate });


            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryXML", MySqlDbType = MySqlDbType.Text, Value = DeliveryAppDtlXml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrdXML", MySqlDbType = MySqlDbType.Text, Value = DeliveryOrdDtlXml });


            LstParam.Add(new MySqlParameter { ParameterName = "in_lstPostPaymentChrgXML", MySqlDbType = MySqlDbType.Text, Value = lstPostPaymentChrgXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_lstContWiseAmountXML", MySqlDbType = MySqlDbType.Text, Value = lstContWiseAmountXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryGodownDtlXml", MySqlDbType = MySqlDbType.Text, Value = DeliveryGodownDtlXml });


            LstParam.Add(new MySqlParameter { ParameterName = "in_lstOperationCFSCodeWiseAmountXML", MySqlDbType = MySqlDbType.Text, Value = lstOperationCFSCodeWiseAmountXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_lstPostPaymentChrgBreakupXML", MySqlDbType = MySqlDbType.Text, Value = lstPostPaymentChrgBreakupXML });

            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjDeliveryApp.DeliApp.DeliveryDate).ToString("yyyy-MM-dd HH:mm:ss") });// ObjDeliveryApp.DeliApp.DeliveryDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjDeliveryApp.DeliApp.InvoiceDate).ToString("yyyy-MM-dd HH:mm:ss") });// ObjDeliveryApp.DeliApp.DeliveryDate });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = ObjDeliveryApp.DeliApp.PartyName });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = ObjDeliveryApp.DeliApp.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = ObjDeliveryApp.DeliApp.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OTHours", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.OTHr });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNos", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.VehicleNos });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNoUn", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.VehicleNoUn });

            // LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.ImporterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });

            LstParam.Add(new MySqlParameter { ParameterName = "in_SealCharge", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDeliveryApp.DeliApp.IsSealCharge == false ? 0 : 1) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ParkingHours", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.ParkingHours });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LockingHours", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliApp.LockingHours });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsSupplyICD", MySqlDbType = MySqlDbType.Bit, Value = ObjDeliveryApp.IsSupply });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjDeliveryApp.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExaminationType", MySqlDbType = MySqlDbType.VarChar, Value = ObjDeliveryApp.ExaminationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weighment", MySqlDbType = MySqlDbType.Int32, Value = ObjDeliveryApp.Weighment });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Scanned", MySqlDbType = MySqlDbType.Int32, Value = ObjDeliveryApp.Scanned });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output, Value = ReturnObj });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            WFLDmergedet pdet = new WFLDmergedet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMergeDelPaymentIssue", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    pdet.Id = Convert.ToInt32(GeneratedClientId);
                    pdet.AppNo = ReturnObj;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Delivery Application Details with Invoice and Issue Slip Saved Successfully";
                    _DBResponse.Data = pdet;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Delivery Application Details Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Invoice cannot be generated due to sd balance is low";
                    _DBResponse.Data = null;
                }

                else if (Result == 4)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "0 Amount Invoice can not be generated";
                    _DBResponse.Data = null;
                }

                else if (Result == 5)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Invoice already generated";
                    _DBResponse.Data = null;
                }

                else if (Result == 6)
                {
                    _DBResponse.Status = 6;
                    _DBResponse.Message = "Invoice Date should be Greater than or equal to already generated Invoice Date";
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
        public void GetDeliveryAppforMerge(int DeliId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DeliId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetDeliveryApplicationForMerge", CommandType.StoredProcedure, DParam);
            WFLDdeliverydet objFormOne = new WFLDdeliverydet();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objFormOne.CHAId = Convert.ToInt32(result["CHAId"]);
                    objFormOne.CHAName = Convert.ToString(result["CHAName"]);
                    objFormOne.GSTNo = Convert.ToString(result["GSTNo"]);
                    objFormOne.CFSCode = Convert.ToString(result["CFSCode"]);
                    objFormOne.LineNo = Convert.ToString(result["LineNo"]);
                    objFormOne.BOENo = Convert.ToString(result["BOENo"]);
                    objFormOne.StateCode = Convert.ToString(result["StateCode"]);
                    objFormOne.Address = Convert.ToString(result["Address"]);

                    objFormOne.StateName = Convert.ToString(result["StateName"]);
                    objFormOne.CargoType = Convert.ToInt32(result["CargoType"]);
                    objFormOne.DeliveryAppDate = Convert.ToString(result["DeliveryAppDate"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Data = objFormOne;
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
        public void AddEditMergeIssueSlip(WFLDMergeDeliveryIssueViewModel ObjIssueSlip)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjIssueSlip.IssueSlip.IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjIssueSlip.IssueSlip.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjIssueSlip.IssueSlip.IssueSlipDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = ObjIssueSlip.IssueSlip.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjIssueSlip.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 100, Direction = ParameterDirection.Output, Value = ReturnObj });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            WFLDmergedet pdet = new WFLDmergedet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditMergeImpIssueSlip", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    pdet.Id = Convert.ToInt32(GeneratedClientId);
                    pdet.AppNo = ReturnObj;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjIssueSlip.IssueSlip.IssueSlipId == 0 ? "Issue Slip Details Saved Successfully" : "Issue Slip Details Updated Successfully";
                    _DBResponse.Data = pdet;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Issue Slip  Details Already Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Edit Issue Slip  Details As It Already Exists In Another Page";
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
        public void GetInvoiceDetForMergeIssueSlip(String InvoiceNo)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = InvoiceNo });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceDetForMergeIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            WFLD_Issueslip ObjIssueSlip = new WFLD_Issueslip();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjIssueSlip.InvoiceId = Convert.ToInt32(Result["InvoiceId"].ToString());
                    ObjIssueSlip.InvoiceDate = Result["InvoiceDate"].ToString();
                    ObjIssueSlip.CargoDescription = Result["CargoDescription"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstContainer.Add(new WFLD_IssueSlipContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            Size = Result["Size"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),

                            Total = Convert.ToDecimal(Result["Total"] == DBNull.Value ? 0 : Result["Total"])
                        });
                    }

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstCargo.Add(new WFLD_IssueSlipCargo
                        {
                            OBLNo = (Result["OBLNo"] == null ? "" : Result["OBLNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            GodownNo = (Result["GodownNo"] == null ? "" : Result["GodownNo"]).ToString(),
                            Location = (Result["Location"] == null ? "" : Result["Location"]).ToString(),
                            StackNo = (Result["StackNo"] == null ? "" : Result["StackNo"]).ToString(),
                            Area = Convert.ToString(Result["Area"] == DBNull.Value ? 0 : Result["Area"]),
                            NetWeight = Convert.ToDecimal(Result["NetWeight"] == DBNull.Value ? 0 : Result["NetWeight"])
                        });
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjIssueSlip;
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
        public void AddEditInvoiceGodownMerge(WFLDInvoiceGodown ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
string ChargesBreakupXML, int BranchId, int Uid,
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
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesBreakupXML", MySqlDbType = MySqlDbType.Text, Value = ChargesBreakupXML });

            // LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            WFLDmergedet pdet = new WFLDmergedet();
            int Result = DA.ExecuteNonQuery("AddEditExpInvoiceMerge", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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
                else if (Result == 5)
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
        public void GetImpPaymentPartyForMergePage(string PartyCode)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            //   lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPartyForMergePage", CommandType.StoredProcedure, Dparam);
            IList<ImpPartyForpage> lstParty = new List<ImpPartyForpage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new ImpPartyForpage
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

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstParty;
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
        public void GetBOELineNoForDelivery(int DestuffingId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpBOELineNoForDelivery", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<WFLDBOELineNoList> LstBOELineNo = new List<WFLDBOELineNoList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstBOELineNo.Add(new WFLDBOELineNoList
                    {

                        BOELineNo = Result["BOELineNo"].ToString(),
                        CHA = Result["CHA"].ToString(),
                        CHAId = Result["CHAId"].ToString(),
                        Billtoparty = Result["BilltoParty"].ToString(),
                        IsInsured = Result["IsInsured"].ToString(),
                        Transporter = Result["transporter"].ToString(),
                        InsuredFrom = Result["InsuredDateFrm"].ToString(),
                        InsuredTo = Result["InsuredDateTo"].ToString(),
                        Oblcount = Result["Oblcount"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        PartyId = Result["PartyId"].ToString(),
                        PartyInsured = Result["PartyInsured"].ToString(),
                        DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBOELineNo;
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
        public void GetBOELineNoDetForDelivery(int DestuffingEntryDtlId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryDtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpBOELineNoDetForDelivery", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            WFLDDeliveryApplicationDtl ObjDeliveryApp = new WFLDDeliveryApplicationDtl();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDeliveryApp.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                    ObjDeliveryApp.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjDeliveryApp.CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]);
                    ObjDeliveryApp.SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]);
                    ObjDeliveryApp.GrossWt = Convert.ToDecimal(Result["GrossWt"] == DBNull.Value ? 0 : Result["GrossWt"]);
                    ObjDeliveryApp.Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]);
                    ObjDeliveryApp.CIF = Convert.ToDecimal(Result["CIF"] == DBNull.Value ? 0 : Result["CIF"]);
                    ObjDeliveryApp.OOC_BOENo = Result["BOENo"].ToString();
                    ObjDeliveryApp.OOC_BOEDATE = Result["BOEDate"] == DBNull.Value ? "" : Convert.ToDateTime(Result["BOEDate"]).ToString("dd/MM/yyyy");
                    ObjDeliveryApp.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjDeliveryApp.Importer = Convert.ToString(Result["Importer"] == DBNull.Value ? "" : Result["Importer"]);
                    ObjDeliveryApp.ShippingLine = Convert.ToString(Result["ShippingLineName"] == DBNull.Value ? "" : Result["ShippingLineName"]);
                    ObjDeliveryApp.TSA = Convert.ToString(Result["TSANo"] == DBNull.Value ? "" : Result["TSANo"]);
                    ObjDeliveryApp.CommodityId = Convert.ToInt32(Result["Cargo"] == DBNull.Value ? 0 : Result["Cargo"]);




                    // ObjDeliveryApp.Commodity = Result["Commodity"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryApp.OblFreeFlag = Convert.ToInt32(Result["Flag"].ToString());
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDeliveryApp;
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
        public void GetAllDeliveryMergeApplication(int Page, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMergeDelivaryAppForPage", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<WFLDMergeDeliveryApplicationList> LstDeliveryApp = new List<WFLDMergeDeliveryApplicationList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDeliveryApp.Add(new WFLDMergeDeliveryApplicationList
                    {
                        DeliveryId = Convert.ToInt32(Result["DeliveryId"].ToString()),
                        DeliveryNo = Result["DeliveryNo"].ToString(),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        DeliveryDate = Result["InvoiceDate"].ToString(),
                        PartyName = Result["PartyName"].ToString(),
                        PayeeName = Result["PayeeName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDeliveryApp;
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
        public void GetIssueSlipForPreview(int IssueSlipId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Value = IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("GetIssueSlipForPreview", CommandType.StoredProcedure, DParam);
            CHN_Issueslip ObjIssueSlip = new CHN_Issueslip();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjIssueSlip.TotalCWCDues = Convert.ToDecimal(Result["TotalCWCDues"] == DBNull.Value ? 0 : Result["TotalCWCDues"]);
                    ObjIssueSlip.CRNoDate = Convert.ToString(Result["CRNoDate"] == null ? "" : Result["CRNoDate"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstIssueSlipRpt.Add(new CHN_IssueSlipReport
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = Result["Size"].ToString(),
                            Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            MarksNo = Convert.ToString(Result["MarksNo"] == null ? "" : Result["MarksNo"]),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString(),
                            Weight = Convert.ToString(Result["Weight"] == null ? "" : Result["Weight"]),
                            ArrivalDate = (Result["ArrivalDate"] == null ? "" : Result["ArrivalDate"]).ToString(),
                            DestuffingDate = (Result["DestuffingDate"] == null ? "" : Result["DestuffingDate"]).ToString(),
                            Location = Result["Location"].ToString(),
                            DeliveryDate = Result["DeliveryDate"].ToString(),
                        });
                    }

                }
                if (Status == 1)
                {
                    _DBResponse.Data = ObjIssueSlip;
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
                Result.Dispose();
                Result.Close();
            }
        }
        #endregion


        #region Destuffing Payment Sheet


        public void GetDeStuffingRequestForImpPaymentSheet(int DestuffingAppId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDestuffingRequestForImpPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<CHN_DestuffingPaymentSheet> objPaySheetStuffing = new List<CHN_DestuffingPaymentSheet>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaySheetStuffing.Add(new CHN_DestuffingPaymentSheet()
                    {
                        CHAId = Convert.ToInt32(Result["CHAId"]),
                        CHAName = Convert.ToString(Result["CHAName"]),
                        CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                        StuffingReqId = Convert.ToInt32(Result["DestuffingId"]),
                        StuffingReqNo = Convert.ToString(Result["DestuffingNo"]),
                        StuffingReqDate = Convert.ToString(Result["DestuffingDate"]),
                        Address = Convert.ToString(Result["Address"]),
                        DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"])
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
        public void GetDestuffingContForPaymentSheet(int DestuffingAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDestuffingRequestForImpPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<CHN_PaymentSheetContainer> objPaymentSheetContainer = new List<CHN_PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new CHN_PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Selected = false,
                        ArrivalDt = Convert.ToString(Result["ArrivalDt"]),
                        IsHaz = Convert.ToString(Result["IsHaz"]),
                        Size = Convert.ToString(Result["Size"])
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

        #region Godown Destuffing Payment Sheet 
        public void GetDestuffingPaymentSheet(string InvoiceDate, int DestuffingAppId, string SEZ, int PartyId, int PayeeId,
            string InvoiceType, string ContainerXML, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyID", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                CHN_ExpPaymentSheet objInvoice = new CHN_ExpPaymentSheet();
                IDataReader Result = DataAccess.ExecuteDataReader("GetAfterDestuffingPaymentSheet", CommandType.StoredProcedure, DParam);

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
                        objInvoice.lstPostPaymentCont.Add(new ChnExpInvoiceContainerBase
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
                            StuffingDate = "",
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            PltBox = Convert.ToInt32(Result["PLtBox"]),
                            ParkDays = Convert.ToInt32(Result["ParkDays"]),
                            LockDays = Convert.ToInt32(Result["LockDays"]),

                        });
                        objInvoice.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstPostPaymentChrg.Add(new ChnExpInvoiceChargeBase
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
                            ADDCWC = Convert.ToInt32(Result["AddCWC"])
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstContwiseAmt.Add(new CHN_ExpContWiseAmount
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
                        objInvoice.lstOperationContwiseAmt.Add(new CHN_ExpOperationContWiseCharge
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
                            DocumentDate = Convert.ToString(Result["SBDate"]),
                            Clause = Result["Clause"].ToString(),
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

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objInvoice.lstADDPostPaymentChrg.Add(new Areas.Export.Models.ChnExpADDCWCInvoiceChargeBase
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
                            ADDCWC = Convert.ToInt32(Result["AddCWC"])
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

            }
        }


        public void AddEditDestuffingInvoice(CHN_ExpPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML,
          int BranchId, int Uid, string Module, string CargoXML = "")
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SEZ });

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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ICDDestuffing", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.ICDDestuffing });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealCharge", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.SealCharge });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditDestuffingInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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


        public void GetAllDestuffingPaymentSheet()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllDestuffingPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<CHNListOfExpInvoice> objPaymentSheetContainer = new List<CHNListOfExpInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new CHNListOfExpInvoice()
                    {
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        Module = Convert.ToString(Result["Module"]),
                        ModuleName = Convert.ToString(Result["ModuleName"]),
                        PartyName = Convert.ToString(Result["PartyName"]),

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



        #endregion

        // Not required
        //public void GetCargoPaymentSheet(string InvoiceDate, int StuffingAppId, string ContainerXML)
        //{
        //    int Status = 0;
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = StuffingAppId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentSheet", CommandType.StoredProcedure, DParam);
        //    _DBResponse = new DatabaseResponse();
        //    PaySheetChargeDetails objPaymentSheet = new PaySheetChargeDetails();
        //    try
        //    {
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            objPaymentSheet.lstPSContainer.Add(new PSContainer()
        //            {
        //                CFSCode = Convert.ToString(Result["CFSCode"]),
        //                ContainerNo = Convert.ToString(Result["ContainerNo"]),
        //                Size = Convert.ToString(Result["Size"]),
        //                IsReefer = Convert.ToBoolean(Result["Reefer"]),
        //                Insured = Convert.ToString(Result["Insured"])
        //            });
        //        }
        //        //if (Result.NextResult())
        //        //{
        //        //    while (Result.Read())
        //        //    {
        //        //        objPaymentSheet.lstEmptyGR.Add(new PSEmptyGroudRent()
        //        //        {
        //        //            ContainerType = Convert.ToString(Result["ContainerType"]),
        //        //            CommodityType = Convert.ToString(Result["CommodityType"]),
        //        //            IsReefer = Convert.ToBoolean(Result["Reefer"]),
        //        //            Size = Convert.ToString(Result["Size"]),
        //        //            DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"]),
        //        //            DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"]),
        //        //            RentAmount = Convert.ToDecimal(Result["RentAmount"]),
        //        //            ElectricityCharge = Convert.ToDecimal(Result["ElectricityCharge"]),
        //        //            GroundRentPeriod = Convert.ToInt32(Result["GRPeriod"]),
        //        //            CFSCode = Convert.ToString(Result["CFSCode"]),
        //        //            FOBValue = Convert.ToDecimal(Result["Fob"]),
        //        //            IsInsured = Convert.ToInt32(Result["Insured"])
        //        //        });
        //        //    }
        //        //}
        //        //if (Result.NextResult())
        //        //{
        //        //    while (Result.Read())
        //        //    {
        //        //        objPaymentSheet.lstLoadedGR.Add(new PSLoadedGroudRent()
        //        //        {
        //        //            ContainerType = Convert.ToString(Result["ContainerType"]),
        //        //            CommodityType = Convert.ToString(Result["CommodityType"]),
        //        //            IsReefer = Convert.ToBoolean(Result["Reefer"]),
        //        //            Size = Convert.ToString(Result["Size"]),
        //        //            DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"]),
        //        //            DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"]),
        //        //            RentAmount = Convert.ToDecimal(Result["RentAmount"]),
        //        //            ElectricityCharge = Convert.ToDecimal(Result["ElectricityCharge"]),
        //        //            GroundRentPeriod = Convert.ToInt32(Result["GRPeriod"]),
        //        //            CFSCode = Convert.ToString(Result["CFSCode"]),
        //        //            FOBValue = Convert.ToDecimal(Result["Fob"]),
        //        //            IsInsured = Convert.ToInt32(Result["Insured"])
        //        //        });
        //        //    }
        //        //}
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objPaymentSheet.InsuranceRate = Convert.ToDecimal(Result["InsuranceCharge"]) / 100 / 1000;
        //            }
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objPaymentSheet.lstStorageRent.Add(new Areas.Import.Models.StorageRent()
        //                {
        //                    CFSCode = Convert.ToString(Result["CFSCode"]),
        //                    ActualCUM = Convert.ToDecimal(Result["CUM"]),
        //                    ActualSQM = Convert.ToDecimal(Result["SQM"]),
        //                    ActualWeight = Convert.ToDecimal(Result["GrossWeight"]),
        //                    StuffCUM = Convert.ToDecimal(Result["DeStuffCUM"]),
        //                    StuffSQM = Convert.ToDecimal(Result["DeStuffSQM"]),
        //                    StuffWeight = Convert.ToDecimal(Result["DestuffingWeight"]),
        //                    StorageDays = Convert.ToInt32(Result["StorageDays"]),
        //                    StorageWeeks = Convert.ToInt32(Result["StorageWeeks"]),
        //                    StorageMonths = Convert.ToInt32(Result["StorageMonths"]),
        //                    StorageMonthWeeks = Convert.ToInt32(Result["StorageMonthWeeks"])
        //                });
        //            }
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objPaymentSheet.RateSQMPerWeek = Convert.ToDecimal(Result["RateSqMPerWeek"]);
        //                objPaymentSheet.RateSQMPerMonth = Convert.ToDecimal(Result["RateSqMeterPerMonth"]);
        //                objPaymentSheet.RateCUMPerWeek = Convert.ToDecimal(Result["RateCubMeterPerDay"]);
        //                objPaymentSheet.RateMTPerDay = Convert.ToDecimal(Result["RateMetricTonPerDay"]);
        //            }
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objPaymentSheet.lstInsuranceCharges.Add(new InsuranceCharge()
        //                {
        //                    CFSCode = Convert.ToString(Result["CFSCode"]),
        //                    StorageWeeks = Convert.ToInt32(Result["StorageWeeks"]),
        //                    IsInsured = Convert.ToInt16(Result["Insured"]),
        //                    FOB = Convert.ToDecimal(Result["Fob"])
        //                });
        //            }
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                objPaymentSheet.lstPSHTCharge.Add(new PSHTCharges()
        //                {
        //                    ChargeId = Convert.ToInt32(Result["HTChargesID"]),
        //                    ChargeName = Convert.ToString(Result["Description"]),
        //                    Charge = Convert.ToDecimal(Result["RateCWC"])
        //                });
        //            }
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = objPaymentSheet;
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
        //public void AddEditCargoInvoice(PaymentSheetFinalModel ObjPSFinalModel, string ContainerXML, string ChargesXML, int BranchId, int Uid)
        //{
        //    string GeneratedClientId = "0";
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.InvoiceId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.InvoiceType });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.InvoiceNo });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPSFinalModel.InvoiceDate) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.StuffingReqId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.StuffingReqNo });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPSFinalModel.StuffingDate) });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.PartyId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.PartyName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.PayeeId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.PayeeName });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.GSTNo });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.CWCTotal });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.HTTotal });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.AllTotal });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.RoundUp });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.Invoice });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.Remarks });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = "IMPCA" });

        //    LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
        //    LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });

        //    LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
        //    LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
        //    IDataParameter[] DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
        //    int Result = DA.ExecuteNonQuery("AddEditExpInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        if (Result == 1)
        //        {
        //            _DBResponse.Data = GeneratedClientId;
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Payment Invoice Saved Successfully";
        //        }
        //        else if (Result == 2)
        //        {
        //            _DBResponse.Data = GeneratedClientId;
        //            _DBResponse.Status = 2;
        //            _DBResponse.Message = "Payment Invoice Updated Successfully";
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
        //
        #endregion

        public void ListOfImpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofImpInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<CHNListOfImpInvoice> lstExpInvoice = new List<CHNListOfImpInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExpInvoice.Add(new CHNListOfImpInvoice()
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Convert.ToString(Result["InvoiceNo"]),
                        InvoiceDate = Convert.ToString(Result["InvoiceDate"]),
                        PartyName = Convert.ToString(Result["PartyName"]),
                        Module = Convert.ToString(Result["Module"]),
                        ModuleName = Convert.ToString(Result["ModuleName"]),
                        IssueSlipId = Convert.ToInt32(Result["IssueSlipId"])

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
    }
}