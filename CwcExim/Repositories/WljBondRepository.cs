using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
using CwcExim.DAL;
using CwcExim.UtilityClasses;
using System.Data;
using MySql.Data.MySqlClient;
using CwcExim.Areas.Bond.Models;
using CwcExim.Areas.Import.Models;
using CwcExim.Areas.Export.Models;

namespace CwcExim.Repositories
{
    public class WljBondRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

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
        public void ListOfImporter()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfImporter", CommandType.StoredProcedure, dparam);
            IList<Areas.Bond.Models.Importer> lstImporterName = new List<Areas.Bond.Models.Importer>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstImporterName.Add(new Areas.Bond.Models.Importer
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
        public void ListOfGodown()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMstGodown", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Areas.Import.Models.Godown> lstGodownList = new List<Areas.Import.Models.Godown>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGodownList.Add(new Areas.Import.Models.Godown
                    {
                        GodownId = Convert.ToInt32(Result["GodownId"]),
                        GodownName = Convert.ToString(Result["GodownName"])
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


        #region Application for availability of space
        public void ListOfSpaceAvailability()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Role.RoleId) });
            LstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfSpcAviailApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<WljListOfBondApp> lstApp = new List<WljListOfBondApp>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstApp.Add(new WljListOfBondApp
                    {
                        SpaceappId = Convert.ToInt32(Result["SpaceappId"]),
                        ApplicationNo = Convert.ToString(Result["ApplicationNo"]),
                        ImporterLicenseNo = Result["ImporterLicenseNo"].ToString(),
                        CHAName = Result["CHAName"].ToString(),
                        ImporterName = Result["ImporterName"].ToString(),
                        BOENo = Result["BOENo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstApp;
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

        public void GetSpaceAvailAppCertForPrint(int SpaceappId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Value = SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSpaceAvailAppCertForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            WljSpaceAvailAppCertPdf ObjSpaceAvailCert = new WljSpaceAvailAppCertPdf();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjSpaceAvailCert.ApplicationNo = Result["ApplicationNo"].ToString();
                    ObjSpaceAvailCert.Importer = Result["Importer"].ToString();
                    ObjSpaceAvailCert.BOLAWBNoDate = (Result["BOLNoDate"] == null ? "" : Result["BOLNoDate"]).ToString();
                    ObjSpaceAvailCert.AWBNoDate = (Result["AWBNoDate"] == null ? "" : Result["AWBNoDate"]).ToString();
                    ObjSpaceAvailCert.BOENoDate = (Result["BOENoDate"] == null ? "" : Result["BOENoDate"]).ToString();
                    ObjSpaceAvailCert.SacNo = (Result["SacNo"] == null ? "" : Result["SacNo"]).ToString();
                    ObjSpaceAvailCert.SacDate = (Result["SacDate"] == null ? "" : Result["SacDate"]).ToString();
                    ObjSpaceAvailCert.AreaReserved = Convert.ToDecimal(Result["AreaReserved"] == DBNull.Value ? 0 : Result["AreaReserved"]);
                    ObjSpaceAvailCert.ValidUpto = Convert.ToString(Result["ValidUpto"] == null ? "" : Result["ValidUpto"]);
                    ObjSpaceAvailCert.CHAName = Result["CHAName"].ToString();

                    ObjSpaceAvailCert.SysDt = Result["SysDt"].ToString();
                    ObjSpaceAvailCert.EmailAddress = Result["EmailAddress"].ToString();
                    ObjSpaceAvailCert.CompanyAddress = Result["CompanyAddress"].ToString();
                    ObjSpaceAvailCert.ImportLicenseNo = Result["ImportLicenseNo"].ToString();
                    ObjSpaceAvailCert.CargeDescrip = Result["CargeDescrip"].ToString();
                    ObjSpaceAvailCert.Unit = Convert.ToInt32(Result["Unit"].ToString());
                    ObjSpaceAvailCert.Packages = Result["Packages"].ToString();
                    ObjSpaceAvailCert.DUnit = Convert.ToDecimal(Result["DUnit"].ToString());
                    ObjSpaceAvailCert.Weight = Convert.ToDecimal(Result["Weight"].ToString());
                    ObjSpaceAvailCert.CIF = Convert.ToDecimal(Result["CIF"].ToString());
                    ObjSpaceAvailCert.Duty = Convert.ToDecimal(Result["Duty"].ToString());
                    ObjSpaceAvailCert.NatureMaterial = Result["NatureMaterial"].ToString();
                    ObjSpaceAvailCert.Encls = Convert.ToString(Result["Encls"]);
                    ObjSpaceAvailCert.ExpDateofWarehouse = Convert.ToString(Result["ExpDateofWarehouse"]);
                    ObjSpaceAvailCert.OthersValue = Convert.ToString(Result["Others"]);
                    ObjSpaceAvailCert.WeightUOM = Convert.ToString(Result["WeightUOM"]);
                    ObjSpaceAvailCert.DimensionUOM = Convert.ToString(Result["DimensionUOM"]);
                    //ObjSpaceAvailCert.BOLAWB = Convert.ToString(Result["BOLAWB"]);
                    //ObjSpaceAvailCert.BOE = Convert.ToString(Result["BOE"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSpaceAvailCert;
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


        public void SpaceAvailabilityDetails(int SpaceappId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Role.RoleId) });
            LstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfSpcAviailApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            WljBondApp objBA = new WljBondApp();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objBA.SpaceappId = Convert.ToInt32(Result["SpaceappId"]);
                    objBA.ApplicationNo = Result["ApplicationNo"].ToString();
                    objBA.ApplicationDate = Result["ApplicationDate"].ToString();
                    objBA.CHAId = Convert.ToInt32(Result["CHAId"]);
                    objBA.ImporterId = Convert.ToInt32(Result["ImporterId"]);
                    objBA.ImporterLicenseNo = Result["ImporterLicenseNo"].ToString();
                    objBA.BOLAWBNo = Result["BOLAWBNo"].ToString();
                    objBA.BolAwbDate = Result["BOLAWBDate"].ToString();
                    objBA.BOENo = Result["BOENo"].ToString();
                    objBA.BOEDate = Result["BOEDate"].ToString();
                    objBA.CargoDescription = Result["CargoDescription"].ToString();
                    objBA.NatureOfPackages = Result["NatureOfPackages"].ToString();
                    objBA.Others = Result["Others"].ToString();
                    objBA.NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]);
                    objBA.NatureOfMaterial = Convert.ToInt32(Result["NatureOfMaterial"]);
                    objBA.DimensionPerUnit = Convert.ToDecimal(Result["DimensionPerUnit"]);
                    objBA.Weight = Convert.ToDecimal(Result["Weight"]);
                    objBA.SpaceReq = Convert.ToDecimal(Result["SpaceReq"]);
                    objBA.AssessCIFvalue = Convert.ToDecimal(Result["AssessCIFvalue"]);
                    objBA.DutyAmt = Convert.ToDecimal(Result["DutyAmt"]);
                    objBA.StorageType = Result["StorageType"].ToString();
                    objBA.ExpDateofWarehouse = Result["ExpDateofWarehouse"].ToString();
                    objBA.CHAName = Result["CHAName"].ToString();
                    objBA.ImporterName = Result["ImporterName"].ToString();
                    objBA.IsInvoiceCopy = Convert.ToBoolean(Result["IsInvoiceCopy"]);
                    objBA.IsPackingList = Convert.ToBoolean(Result["IsPackingList"]);
                    objBA.IsBOLAWB = Convert.ToBoolean(Result["IsBOLAWB"]);
                    objBA.IsBOE = Convert.ToBoolean(Result["IsBOE"]);
                    objBA.Length = Convert.ToDecimal(Result["Length"]);
                    objBA.Width = Convert.ToDecimal(Result["Width"]);
                    objBA.Height = Convert.ToDecimal(Result["Height"]);

                    objBA.AWBNo = Convert.ToString(Result["AWBNo"]);
                    objBA.AWBDate = Convert.ToString(Result["AWBDate"]);
                    objBA.DimensionUOM = Convert.ToString(Result["DimensionUOM"]);
                    objBA.WeightUOM = Convert.ToString(Result["WeightUOM"]);
                    objBA.CustomSealNo = Convert.ToString(Result["CustomSealNo"]);
                    objBA.GodownName = Convert.ToString(Result["GodownName"]);
                    objBA.GodownId = Convert.ToInt32(Result["GodownId"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objBA;
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
        public void AddEditBondApp(WljBondApp objBA)
        {
            string ApplicationNo = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objBA.SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationNo", MySqlDbType = MySqlDbType.VarChar, Value = objBA.ApplicationNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objBA.ApplicationDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objBA.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objBA.ImporterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterLicenseNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objBA.ImporterLicenseNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOLAWBNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objBA.BOLAWBNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOLAWBDate", MySqlDbType = MySqlDbType.Date, Value = (objBA.BolAwbDate == null ? null : Convert.ToDateTime(objBA.BolAwbDate).ToString("yyyy-MM-dd")) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objBA.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.Date, Value = (objBA.BOEDate == null ? null : Convert.ToDateTime(objBA.BOEDate).ToString("yyyy-MM-dd")) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objBA.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NatureOfPackages", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = objBA.NatureOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Others", MySqlDbType = MySqlDbType.VarChar, Value = objBA.Others });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfUnits", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objBA.NoOfUnits });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NatureOfMaterial", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objBA.NatureOfMaterial });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DimensionPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = objBA.DimensionPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = objBA.Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceReq", MySqlDbType = MySqlDbType.Decimal, Value = objBA.SpaceReq });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AssessCIFvalue", MySqlDbType = MySqlDbType.Decimal, Value = objBA.AssessCIFvalue });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DutyAmt", MySqlDbType = MySqlDbType.Decimal, Value = objBA.DutyAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StorageType", MySqlDbType = MySqlDbType.VarChar, Value = objBA.StorageType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExpDateofWarehouse", MySqlDbType = MySqlDbType.Date, Value = (objBA.ExpDateofWarehouse == null ? null : Convert.ToDateTime(objBA.ExpDateofWarehouse).ToString("yyyy-MM-dd")) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_IsInvoiceCopy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objBA.IsInvoiceCopy == true ? 1 : 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsPackingList", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objBA.IsPackingList == true ? 1 : 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsBOLAWB", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objBA.IsBOLAWB == true ? 1 : 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsBOE", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objBA.IsBOE == true ? 1 : 0 });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Length", MySqlDbType = MySqlDbType.Decimal, Value = (objBA.Length == null ? 0 : objBA.Length) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Width", MySqlDbType = MySqlDbType.Decimal, Value = (objBA.Width == null ? 0 : objBA.Width) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Height", MySqlDbType = MySqlDbType.Decimal, Value = (objBA.Height == null ? 0 : objBA.Height) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_AWBNo", MySqlDbType = MySqlDbType.VarChar, Value = objBA.AWBNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AWBDate", MySqlDbType = MySqlDbType.Date, Value = (objBA.AWBDate == null ? null : Convert.ToDateTime(objBA.AWBDate).ToString("yyyy-MM-dd")) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = objBA.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = objBA.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DimensionUOM", MySqlDbType = MySqlDbType.VarChar, Value = objBA.DimensionUOM });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WeightUOM", MySqlDbType = MySqlDbType.VarChar, Value = objBA.WeightUOM });

            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Value = 0, Size = 30, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditSpcAvlApp", CommandType.StoredProcedure, DParam, out ApplicationNo);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Bond Application Saved Successfully" : "Bond Application Updated Successfully";
                    _DBResponse.Data = ApplicationNo;
                }
                else if (Result == 3)
                {
                    _DBResponse.Message = ApplicationNo;
                }
                else if (Result == 4)
                {
                    _DBResponse.Message = "Only Updated BOENo. and BOEDate  As It Already Used In Next Page";
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
        public void DeleteBondApp(int SpaceappId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteSpcAvailApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Bond Application Deleted Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Cannot Delete Application";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Cannot Delete Bond Application  As It Already Used In Next Page";
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
        #endregion

        #region Space Availability Certificate
        public void AddEditBondSpaceAvailCert(WljSpaceAvailableCert ObjSpaceAvail)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Value = ObjSpaceAvail.SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjSpaceAvail.SacNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacDate", MySqlDbType = MySqlDbType.Date, Value = (ObjSpaceAvail.SacDate == null ? null : Convert.ToDateTime(ObjSpaceAvail.SacDate).ToString("yyy/MM/dd")) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AreaReserved", MySqlDbType = MySqlDbType.Decimal, Value = ObjSpaceAvail.AreaReserved });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ValidUpto", MySqlDbType = MySqlDbType.Date, Value = (ObjSpaceAvail.ValidUpto == null ? null : Convert.ToDateTime(ObjSpaceAvail.ValidUpto).ToString("yyyy/MM/dd")) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsApproved", MySqlDbType = MySqlDbType.Int32, Value = ObjSpaceAvail.IsApproved });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsSubmitted", MySqlDbType = MySqlDbType.Int32, Value = ObjSpaceAvail.IsSubmitted });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApprovedBy", MySqlDbType = MySqlDbType.Int32, Value = ObjSpaceAvail.ApprovedBy });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NextApprovalDate", MySqlDbType = MySqlDbType.Date, Value = (ObjSpaceAvail.NextApprovalDate == null ? null : Convert.ToDateTime(ObjSpaceAvail.NextApprovalDate).ToString("yyyy-MM-dd")) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditBondSpaceAvailCert", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Space Availability Certificate Details Saved Successfully";
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
        public void GetSpaceAvailCert(int Status, int Skip)
        {
            int status = 0;
            bool State = false;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Status", MySqlDbType = MySqlDbType.Int32, Value = Status });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Skip", MySqlDbType = MySqlDbType.Int32, Value = Skip });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSpaceAvailCert", CommandType.StoredProcedure, DParam);
            List<WljListOfBondApp> LstSpaceAvail = new List<WljListOfBondApp>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    status = 1;
                    LstSpaceAvail.Add(new WljListOfBondApp
                    {
                        SpaceappId = Convert.ToInt32(Result["SpaceappId"]),
                        ApplicationNo = Convert.ToString(Result["ApplicationNo"]),
                        CHAName = (Result["CHAName"] == null ? "" : Result["CHAName"]).ToString(),
                        ImporterName = (Result["ImporterName"] == null ? "" : Result["ImporterName"]).ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { List = LstSpaceAvail, State = State };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetSpaceAvailCertById(int SpaceappId, int Stats)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Role.RoleId) });
            LstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfSpcAviailApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            WljSpaceAvailableCert ObjSpaceAvail = new WljSpaceAvailableCert();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSpaceAvail.SpaceappId = Convert.ToInt32(Result["SpaceappId"]);
                    ObjSpaceAvail.ApplicationNo = Result["ApplicationNo"].ToString();
                    ObjSpaceAvail.ApplicationDate = Result["ApplicationDate"].ToString();
                    ObjSpaceAvail.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjSpaceAvail.ImporterId = Convert.ToInt32(Result["ImporterId"]);
                    ObjSpaceAvail.ImporterLicenseNo = Result["ImporterLicenseNo"].ToString();
                    ObjSpaceAvail.BOLAWBNo = Result["BOLAWBNo"].ToString();
                    ObjSpaceAvail.BolAwbDate = Result["BOLAWBDate"].ToString();
                    ObjSpaceAvail.BOENo = Result["BOENo"].ToString();
                    ObjSpaceAvail.BOEDate = Result["BOEDate"].ToString();
                    ObjSpaceAvail.CargoDescription = Result["CargoDescription"].ToString();
                    ObjSpaceAvail.NatureOfPackages = Result["NatureOfPackages"].ToString();
                    ObjSpaceAvail.NoOfUnits = Convert.ToInt32(Result["NoOfUnits"]);
                    ObjSpaceAvail.NatureOfMaterial = Convert.ToInt32(Result["NatureOfMaterial"]);
                    ObjSpaceAvail.DimensionPerUnit = Convert.ToDecimal(Result["DimensionPerUnit"]);
                    ObjSpaceAvail.Weight = Convert.ToDecimal(Result["Weight"]);
                    ObjSpaceAvail.SpaceReq = Convert.ToDecimal(Result["SpaceReq"]);
                    ObjSpaceAvail.AssessCIFvalue = Convert.ToDecimal(Result["AssessCIFvalue"]);
                    ObjSpaceAvail.DutyAmt = Convert.ToDecimal(Result["DutyAmt"]);
                    ObjSpaceAvail.ExpDateofWarehouse = Result["ExpDateofWarehouse"].ToString();
                    ObjSpaceAvail.CHAName = Result["CHAName"].ToString();
                    ObjSpaceAvail.ImporterName = Result["ImporterName"].ToString();
                    ObjSpaceAvail.IsInvoiceCopy = Convert.ToBoolean(Result["IsInvoiceCopy"]);
                    ObjSpaceAvail.IsPackingList = Convert.ToBoolean(Result["IsPackingList"]);
                    ObjSpaceAvail.IsBOLAWB = Convert.ToBoolean(Result["IsBOLAWB"]);
                    ObjSpaceAvail.IsBOE = Convert.ToBoolean(Result["IsBOE"]);
                    ObjSpaceAvail.Length = Convert.ToDecimal(Result["Length"]);
                    ObjSpaceAvail.Width = Convert.ToDecimal(Result["Width"]);
                    ObjSpaceAvail.Height = Convert.ToDecimal(Result["Height"]);
                    ObjSpaceAvail.StorageType = Result["StorageType"].ToString();
                    if (Stats == 1)
                    {
                        ObjSpaceAvail.SacNo = "";
                        ObjSpaceAvail.SacDate = DateTime.Now.ToString("dd/MM/yyyy");
                        ObjSpaceAvail.AreaReserved = 0;
                        ObjSpaceAvail.IsApproved = 0;
                        ObjSpaceAvail.NextApprovalDate = "";
                        ObjSpaceAvail.ValidUpto = Convert.ToDateTime(DateTime.Now).AddDays(13).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        ObjSpaceAvail.SacNo = (Result["SacNo"] == null ? "" : Result["SacNo"]).ToString();
                        ObjSpaceAvail.SacDate = (Result["SacDate"] == null ? "" : Result["SacDate"]).ToString();
                        ObjSpaceAvail.AreaReserved = Convert.ToDecimal(Result["AreaReserved"] == DBNull.Value ? 0 : Result["AreaReserved"]);
                        ObjSpaceAvail.IsApproved = Convert.ToInt32(Result["IsApproved"] == DBNull.Value ? 0 : Result["IsApproved"]);
                        ObjSpaceAvail.NextApprovalDate = Convert.ToString(Result["NextApprovalDate"]);
                        ObjSpaceAvail.ValidUpto = Convert.ToString(Result["ValidUpto"]);
                    }
                    //  ObjSpaceAvail.ValidUpto = Convert.ToString(Result["ValidUpto"] == null ? "" : Result["ValidUpto"]);
                    ObjSpaceAvail.IsSubmitted = Convert.ToInt32(Result["IsSubmitted"]);
                    ObjSpaceAvail.AWBNo = Convert.ToString(Result["AWBNo"]);
                    ObjSpaceAvail.AWBDate = Convert.ToString(Result["AWBDate"]);
                    ObjSpaceAvail.DimensionUOM = Convert.ToString(Result["DimensionUOM"]);
                    ObjSpaceAvail.WeightUOM = Convert.ToString(Result["WeightUOM"]);
                    ObjSpaceAvail.CustomSealNo = Convert.ToString(Result["CustomSealNo"]);
                    ObjSpaceAvail.GodownName = Convert.ToString(Result["GodownName"]);
                    ObjSpaceAvail.GodownId = Convert.ToInt32(Result["GodownId"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSpaceAvail;
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
        public void SearchUsingAWBNo(string AWBNo, int Status)
        {
            int status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "IN_SEARCH", MySqlDbType = MySqlDbType.VarChar, Value = AWBNo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_STATE", MySqlDbType = MySqlDbType.Int32, Value = Status });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("SearchUsingBondAppAWBNo", CommandType.StoredProcedure, DParam);
            List<WljListOfBondApp> LstSpaceAvail = new List<WljListOfBondApp>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    status = 1;
                    LstSpaceAvail.Add(new WljListOfBondApp
                    {
                        SpaceappId = Convert.ToInt32(Result["SpaceappId"]),
                        ApplicationNo = Convert.ToString(Result["ApplicationNo"]),
                        CHAName = (Result["CHAName"] == null ? "" : Result["CHAName"]).ToString(),
                        ImporterName = (Result["ImporterName"] == null ? "" : Result["ImporterName"]).ToString()
                    });
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { List = LstSpaceAvail, State = false };
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
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetSpaceAvailCertForPrint(int SpaceappId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Value = SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSpaceAvailCertForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            WljSpaceAvailCertPdf ObjSpaceAvailCert = new WljSpaceAvailCertPdf();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSpaceAvailCert.Importer = Result["Importer"].ToString();
                    ObjSpaceAvailCert.BOLAWBNo = (Result["BOLAWBNo"] == null ? "" : Result["BOLAWBNo"]).ToString();
                    ObjSpaceAvailCert.BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString();
                    ObjSpaceAvailCert.AWBNo = (Result["AWBNo"] == null ? "" : Result["AWBNo"]).ToString();
                    ObjSpaceAvailCert.AWBDate = (Result["AWBDate"] == null ? "" : Result["AWBDate"]).ToString();
                    ObjSpaceAvailCert.NextApprovalDate = (Result["NextApprovalDate"] == null ? "" : Result["NextApprovalDate"]).ToString();
                    ObjSpaceAvailCert.BOLAWBDate = (Result["BOLAWBDate"] == null ? "" : Result["BOLAWBDate"]).ToString();
                    ObjSpaceAvailCert.BOENoDate = (Result["BOENoDate"] == null ? "" : Result["BOENoDate"]).ToString();
                    ObjSpaceAvailCert.SacNo = (Result["SacNo"] == null ? "" : Result["SacNo"]).ToString();
                    ObjSpaceAvailCert.SacDate = (Result["SacDate"] == null ? "" : Result["SacDate"]).ToString();
                    ObjSpaceAvailCert.AreaReserved = Convert.ToDecimal(Result["AreaReserved"] == DBNull.Value ? 0 : Result["AreaReserved"]);
                    ObjSpaceAvailCert.ValidUpto = Convert.ToString(Result["ValidUpto"] == null ? "" : Result["ValidUpto"]);
                    ObjSpaceAvailCert.CHAName = Result["CHAName"].ToString();
                    ObjSpaceAvailCert.EmailAddress = Result["EmailAddress"].ToString();
                    ObjSpaceAvailCert.CompanyAddress = Result["CompanyAddress"].ToString();
                    ObjSpaceAvailCert.Location = Result["Location"].ToString();

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSpaceAvailCert;
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

        #endregion

        #region Bond Advance Payment Sheet
        public void GetBondAdvancePaymentSheet(string InvoiceDate, int DestuffingAppId, int DeliveryType,string SEZ, string DestuffingAppNo, string DestuffingAppDate, int PartyId, string PartyName,
    string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, int CasualLabour,
    string InvoiceType, decimal area, int InvoiceId = 0, int No = 0, string ExportUnder = "")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UptoDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(DestuffingAppDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CasualLabour", MySqlDbType = MySqlDbType.Int32, Value = CasualLabour });
            LstParam.Add(new MySqlParameter { ParameterName = "Area", MySqlDbType = MySqlDbType.Decimal, Value = area });
            LstParam.Add(new MySqlParameter { ParameterName = "In_SacNo", MySqlDbType = MySqlDbType.VarChar, Value = DestuffingAppNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_No", MySqlDbType = MySqlDbType.Int32, Value = No });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            WljPostPaymentSheet objInvoice = new WljPostPaymentSheet();
            List<String> Clause = new List<String>();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondAdvancePaymentSheet", CommandType.StoredProcedure, DParam);

            try
            {
                //DataSet Result = DataAccess.ExecuteDataSet("GetYardPaymentSheetCWCCharges", CommandType.StoredProcedure, DParam);
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
                        objInvoice.lstPrePaymentCont.Add(new WljPreInvoiceContainer
                        {
                            ContainerNo = Result["ContainerNo"] == System.DBNull.Value ? "" : Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"] == System.DBNull.Value ? "" : Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"] == System.DBNull.Value ? "" : Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"] == System.DBNull.Value ? "" : Result["BOENo"].ToString(),
                            GrossWeight = Result["GrossWeight"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerPack = Result["WtPerPack"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WtPerPack"]),
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
                            // ApproveOn = Result["ApproveOn"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            LineNo = Result["LineNo"].ToString(),
                            // OperationType = Convert.ToInt32(Result["OperationType"]),
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                        });
                        objInvoice.lstPostPaymentCont.Add(new WljPostPaymentContainer
                        {
                            ContainerNo = Result["ContainerNo"] == System.DBNull.Value ? "" : Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"] == System.DBNull.Value ? "" : Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"] == System.DBNull.Value ? "" : Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"] == System.DBNull.Value ? "" : Result["BOENo"].ToString(),
                            GrossWt = Result["GrossWeight"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Result["WtPerPack"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Result["CIFValue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Result["Duty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Duty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Result["RMS"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Result["DeliveryType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Result["Insured"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Result["SpaceOccupied"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Result["HeavyScrap"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Result["AppraisementPerct"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["AppraisementPerct"]),
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
                        objInvoice.lstPostPaymentChrg.Add(new WljPostPaymentCharge
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
                        objInvoice.lstContWiseAmount.Add(new WljContainerWiseAmount
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
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new WljOperationCFSCodeWiseAmount
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
                            Clause = Result["Clause"].ToString()
                        });
                    }
                }



                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Clause.Add(Result["Clause"].ToString());

                    }
                }
                objInvoice.ActualApplicable = Clause;
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
                Result.Dispose();
                Result.Close();
            }
        }


        public void GetSACNoForAdvBondPaymentSheet(int CartingAppId = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objBondSacDetails = (IList<WljBondSacDetails>)DataAccess.ExecuteDynamicSet<WljBondSacDetails>("GetSACNoForBondAdvPaymentSheet");

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objBondSacDetails;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        public void AddEditInvoice(WljPostPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string CfsChargeXML, int BranchId, int Uid,
          string Module, string CargoXML = "")
        {
            //  var cfsCodeWiseHtRate = HttpContext.Current.Session["lstCfsCodewiseRateHT"];
            //  string cfsCodeWiseHtRateXML = UtilityClasses.Utility.CreateXML(cfsCodeWiseHtRate);
            string GeneratedClientId = "0";
            //  string ReturnObj = "";
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
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = CfsChargeXML });

            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });


            // LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditAdvanceBondInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            if (GeneratedClientId == "")
            {
                Result = 3;
            }
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully";

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

        #endregion

        #region Deposit Application

        public void GetSacNoForDepositApp()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSacNoForDepositApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<WljDepositApp> LstDepositApp = new List<WljDepositApp>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDepositApp.Add(new WljDepositApp
                    {
                        // EntryId = Convert.ToInt32(Result["EntryId"]),
                        //SpaceappId = Convert.ToInt32(Result["SpaceappId"]),
                        SacNo = Result["SacNo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDepositApp;
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

        public void GetAllVehicle(string SacNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = SacNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetVehicleForBondDeposit", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<VehicleForWljBondDeposit> LstVehicle = new List<VehicleForWljBondDeposit>();
            try
            {
                while (Result.Read())

                {
                    Status = 1;
                    LstVehicle.Add(new VehicleForWljBondDeposit
                    {
                        VehicleNo = Result["VehicleNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        SpaceappId = Convert.ToInt32(Result["SpaceappId"])

                    });
                    if (Status == 1)
                    {
                        _DBResponse.Status = 1;
                        _DBResponse.Message = "Success";
                        _DBResponse.Data = LstVehicle;
                    }
                    else
                    {
                        _DBResponse.Status = 0;
                        _DBResponse.Message = "No Data";
                        _DBResponse.Data = null;
                    }
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

        public void AddEditBondDepositApp(WljDepositApp ObjDepositApp)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = ObjDepositApp.DepositAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Value = ObjDepositApp.SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = ObjDepositApp.EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDepositApp.DepositDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BondBOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDepositApp.BondBOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BondBOEDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDepositApp.BondBOEDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BondNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDepositApp.BondNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BondDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDepositApp.BondDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomBondNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDepositApp.CustomBondNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomBondDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjDepositApp.CustomBondDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDepositApp.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDepositApp.VehicleNo });

            LstParam.Add(new MySqlParameter { ParameterName = "in_WRNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDepositApp.WRNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WRDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDepositApp.WRDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = ObjDepositApp.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Value = ObjDepositApp.ImporterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = ObjDepositApp.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Units", MySqlDbType = MySqlDbType.Decimal, Value = ObjDepositApp.Units });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = ObjDepositApp.Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Value", MySqlDbType = MySqlDbType.Decimal, Value = ObjDepositApp.Value });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Duty", MySqlDbType = MySqlDbType.Decimal, Value = ObjDepositApp.Duty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjDepositApp.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsInsured", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjDepositApp.IsInsured) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Status = DataAccess.ExecuteNonQuery("AddEditBondDepositApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjDepositApp.DepositAppId == 0 ? "Deposit Application Details Saved Successfully" : "Deposit Application Details Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Status == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Update Deposit Application Details As It Already Exist In Another Page";
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
        public void GetDepositApp(int DepositAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = DepositAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondDepositApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            WljDepositApp ObjDepositApp = new WljDepositApp();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDepositApp.DepositAppId = Convert.ToInt32(Result["DepositAppId"]);
                    ObjDepositApp.SpaceappId = Convert.ToInt32(Result["SpaceappId"]);
                    ObjDepositApp.DepositNo = Convert.ToString(Result["DepositNo"]);
                    ObjDepositApp.DepositDate = Convert.ToString(Result["DepositDate"]);
                    ObjDepositApp.BondBOENo = Convert.ToString(Result["BondBOENo"] == null ? "" : Result["BondBOENo"]);
                    ObjDepositApp.BondBOEDate = Convert.ToString(Result["BondBOEDate"] == null ? "" : Result["BondBOEDate"]);
                    ObjDepositApp.BondNo = Convert.ToString(Result["BondNo"] == null ? "" : Result["BondNo"]);
                    ObjDepositApp.BondDate = Convert.ToString(Result["BondDate"] == null ? "" : Result["BondDate"]);
                    ObjDepositApp.CustomBondNo = Convert.ToString(Result["CustomBondNo"] == null ? "" : Result["CustomBondNo"]);
                    ObjDepositApp.CustomBondDate = Convert.ToString(Result["CustomBondDate"] == null ? "" : Result["CustomBondDate"]);
                    ObjDepositApp.CustomSealNo = Convert.ToString(Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]);
                    ObjDepositApp.VehicleNo = Convert.ToString(Result["VehicleNo"] == null ? "" : Result["VehicleNo"]);
                    ObjDepositApp.WRNo = Convert.ToString(Result["WRNo"] == null ? "" : Result["WRNo"]);
                    ObjDepositApp.WRDate = Convert.ToString(Result["WRDate"] == null ? "" : Result["WRDate"]);
                    ObjDepositApp.GodownId = Convert.ToInt32(Result["GodownId"] == DBNull.Value ? 0 : Result["GodownId"]);
                    ObjDepositApp.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjDepositApp.CargoDescription = Convert.ToString(Result["CargoDescription"] == null ? "" : Result["CargoDescription"]);
                    ObjDepositApp.Units = Convert.ToInt32(Result["Units"]);
                    ObjDepositApp.Weight = Convert.ToDecimal(Result["Weight"]);
                    ObjDepositApp.Value = Convert.ToDecimal(Result["Value"]);
                    ObjDepositApp.Duty = Convert.ToDecimal(Result["Duty"]);
                    ObjDepositApp.Remarks = Convert.ToString(Result["Remarks"] == null ? "" : Result["Remarks"]);
                    ObjDepositApp.IsInsured = Convert.ToBoolean(Result["IsInsured"]);
                    ObjDepositApp.SacNo = Convert.ToString(Result["SacNo"]);
                    ObjDepositApp.SacDate = Convert.ToString(Result["SacDate"]);
                    ObjDepositApp.EntryDate = Convert.ToString(Result["EntryDate"]);
                    ObjDepositApp.CHA = Convert.ToString(Result["CHA"]);
                    ObjDepositApp.Importer = Convert.ToString(Result["Importer"]);
                    ObjDepositApp.GodownName = Convert.ToString(Result["GodownName"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDepositApp;
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

        public void GetSacNodepositDet(int SpaceappId, int EntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Value = SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSacNoDepositDet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            WljDepositApp ObjSpaceAvail = new WljDepositApp();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    //  ObjSpaceAvail.AreaReserved = Convert.ToDecimal(Result["AreaReserved"]);
                    ObjSpaceAvail.SacNo = Convert.ToString(Result["SacNo"]);
                    //   ObjSpaceAvail.ApplicationNo = Convert.ToString(Result["ApplicationNo"]);
                    ObjSpaceAvail.SacDate = Convert.ToString(Result["SacDate"]);
                    //  ObjSpaceAvail.ApplicationDate = Convert.ToString(Result["ApplicationDate"]);
                    //  ObjSpaceAvail.ValidUpto = Convert.ToString(Result["ValidUpto"]);
                    ObjSpaceAvail.CustomSealNo = Convert.ToString(Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]);
                    ObjSpaceAvail.VehicleNo = Convert.ToString(Result["VehicleNo"] == null ? "" : Result["VehicleNo"]);
                    ObjSpaceAvail.GodownId = Convert.ToInt32(Result["GodownId"] == DBNull.Value ? 0 : Result["GodownId"]);
                    ObjSpaceAvail.GodownName = Convert.ToString(Result["GodownName"] == null ? "" : Result["GodownName"]);
                    ObjSpaceAvail.CHA = Convert.ToString(Result["CHAName"] == null ? "" : Result["CHAName"]);
                    ObjSpaceAvail.Importer = Convert.ToString(Result["ImporterName"] == null ? "" : Result["ImporterName"]);
                    ObjSpaceAvail.BondBOENo = Convert.ToString(Result["BOENo"] == null ? "" : Result["BOENo"]);
                    ObjSpaceAvail.BondBOEDate = Convert.ToString(Result["BOEDate"] == null ? "" : Result["BOEDate"]);
                    ObjSpaceAvail.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjSpaceAvail.WRNo = Convert.ToString(Result["WBNO"] == null ? "" : Result["WBNO"]);
                    ObjSpaceAvail.Units = Convert.ToInt32(Result["Unit"] == null ? 0 : Result["Unit"]);
                    ObjSpaceAvail.Value = Convert.ToDecimal(Result["CIF"] == null ? 0 : Result["CIF"]);
                    ObjSpaceAvail.Duty = Convert.ToDecimal(Result["Duty"] == null ? "" : Result["Duty"]);
                    ObjSpaceAvail.Weight = Convert.ToDecimal(Result["Weight"] == null ? "" : Result["Weight"]);
                    ObjSpaceAvail.CargoDescription = Convert.ToString(Result["Cargo"] == null ? "" : Result["Cargo"]);
                    ObjSpaceAvail.EntryDate = Convert.ToString(Result["EntryDate"] == null ? "" : Result["EntryDate"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSpaceAvail;
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

        public void GetAllDepositApp()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondDepositApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<WljDepositApp> LstDepositApp = new List<WljDepositApp>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDepositApp.Add(new WljDepositApp
                    {
                        DepositNo = Convert.ToString(Result["DepositNo"]),
                        DepositDate = Convert.ToString(Result["DepositDate"]),
                        SacNo = Result["SacNo"].ToString(),
                        SacDate = (Result["SacDate"] == null ? "" : Result["SacDate"]).ToString(),
                        DepositAppId = Convert.ToInt32(Result["DepositAppId"])
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDepositApp;
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

        public void PrintDA(int DepositeAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = DepositeAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintDepositeApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            WljPrintDA ObjDepositApp = new WljPrintDA();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDepositApp.DepositeNo = Convert.ToString(Result["DepositNo"]);
                    ObjDepositApp.DepositeDate = Convert.ToString(Result["DepositDate"]);
                    ObjDepositApp.BondNo = Result["BondNo"].ToString();
                    ObjDepositApp.BondDt = Convert.ToString(Result["BondDt"] == null ? "" : Result["BondDt"]);
                    ObjDepositApp.WRNo = Convert.ToString(Result["WRNo"] == null ? "" : Result["WRNo"]);
                    ObjDepositApp.WRDt = Convert.ToString(Result["WRDate"] == null ? "" : Result["WRDate"]);
                    ObjDepositApp.BondBOENo = Result["BondBOENo"].ToString();
                    ObjDepositApp.BondBOEDate = Convert.ToString(Result["BondBOEDate"] == null ? "" : Result["BondBOEDate"]);
                    ObjDepositApp.GodownName = Convert.ToString(Result["GodownName"]);
                    ObjDepositApp.CompanyAddress = Result["CompanyAddress"].ToString();
                    ObjDepositApp.EmailAddress = Result["EmailAddress"].ToString();
                    ObjDepositApp.Location = Result["Location"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDepositApp.lstSAC.Add(new WljSACDet
                        {
                            SacNo = Result["SacNo"].ToString(),
                            SacDate = Result["SacDate"].ToString(),
                            ImporterName = Result["ImporterName"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"])
                        });
                    }
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDepositApp.lstGodown.Add(new WljGodDet
                        {
                            GodownName = Result["GodownName"].ToString(),
                            AreaReserved = Convert.ToDecimal(Result["AreaReserved"].ToString()),
                            CargoDesc = Result["CargoDesc"].ToString(),
                            Units = Convert.ToDecimal(Result["Units"].ToString()),
                            Weight = (Convert.ToDecimal(Result["Weight"].ToString())) / 1000,
                            Remarks = Result["Remarks"].ToString(),
                            NatureOfPackages = Result["NatureOfPackages"].ToString()
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDepositApp;
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

        #region Unloading at Bonded Warehouse
        public void GetBondNoForWOUnloading()
        {
            int status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondNoUnloading", CommandType.StoredProcedure, DParam);
            List<ListOfWlj_BOENo> LstBond = new List<ListOfWlj_BOENo>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    status = 1;
                    LstBond.Add(new ListOfWlj_BOENo
                    {
                        //WorkOrderNo = Convert.ToString(Result["WorkOrderNo"]),
                        //BondWOId = Convert.ToInt32(Result["BondWOId"])
                        BondNo = Convert.ToString(Result["BondNo"]),
                        DepositAppId = Convert.ToInt32(Result["DepositAppId"])
                    });
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBond;
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
        public void GetBondNoForWOUnloadingDet(int DepositAppId)
        {
            int status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = DepositAppId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondNoUnloading", CommandType.StoredProcedure, DParam);
            WljBOENoDetails objBOE = new WljBOENoDetails();
            _DBResponse = new DatabaseResponse();
            List<GodownDetails> lstGodown = new List<GodownDetails>();
            try
            {
                while (Result.Read())
                {
                    status = 1;
                    objBOE.SpaceAppId = Convert.ToInt32(Result["SpaceAppId"]);
                    objBOE.DepositDate = Result["DepositDate"].ToString();
                    //  objBOE.BondWOId = Convert.ToInt32(Result["BondWOId"]);
                    objBOE.BondBOENo = Convert.ToString(Result["BondBOENo"]);
                    objBOE.BondBOEDate = Convert.ToString(Result["BondBOEDate"]);
                    objBOE.DepositNo = Convert.ToString(Result["DepositNo"]);
                    objBOE.BondDate = Convert.ToString(Result["BondDate"]);
                    objBOE.WRNo = Result["WRNo"].ToString();
                    objBOE.WRDate = Result["WRDate"].ToString();
                    objBOE.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objBOE.GodownName = Result["GodownName"].ToString();
                    objBOE.CargoDescription = Result["CargoDescription"].ToString();
                    objBOE.SacNo = Result["SacNo"].ToString();
                    objBOE.SacDate = Result["SacDate"].ToString();
                    objBOE.Units = Convert.ToInt32(Result["Units"]);
                    objBOE.Weight = Convert.ToDecimal(Result["Weight"]);
                    objBOE.AreaReserved = Convert.ToDecimal(Result["AreaReserved"]);
                    objBOE.Remarks = Result["Remarks"].ToString();


                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstGodown.Add(new GodownDetails { LocationId = Convert.ToInt32(Result["LocationId"]), LocationName = Result["LocationName"].ToString() });
                    }
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { BOE = objBOE, lstGodown };
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
        public void ListForWOUnloading()
        {
            int status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UnloadingId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Action", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = "" });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfWOUnloading", CommandType.StoredProcedure, DParam);
            List<ListOfWOunloading> LstBond = new List<ListOfWOunloading>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    status = 1;
                    LstBond.Add(new ListOfWOunloading
                    {
                        UnloadingId = Convert.ToInt32(Result["UnloadingId"]),
                        BondBOENo = Convert.ToString(Result["BondNo"]),
                        BondBOEDate = Convert.ToString(Result["BondDate"]),
                        DepositNo = Result["DepositNo"].ToString(),
                        DepositDate = Result["DepositDate"].ToString(),
                    });
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBond;
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
        public void WOUnloadingDetails(int UnloadingId, string Action = "")
        {
            int status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UnloadingId", MySqlDbType = MySqlDbType.Int32, Value = UnloadingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Action", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = Action });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfWOUnloading", CommandType.StoredProcedure, DParam);
            WljBondWOUnloading objBond = new WljBondWOUnloading();
            List<GodownDetails> lstGodown = new List<GodownDetails>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    status = 1;
                    objBond.SpaceAppId = Convert.ToInt32(Result["SpaceAppId"]);
                    objBond.DepositAppId = Convert.ToInt32(Result["DepositAppId"]);
                    objBond.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objBond.UnloadingId = Convert.ToInt32(Result["UnloadingId"]);
                    objBond.DepositNo = Result["DepositNo"].ToString();
                    objBond.DepositDate = Result["DepositDate"].ToString();
                    objBond.BondBOENo = Convert.ToString(Result["BondBOENo"]);
                    objBond.BondBOEDate = Convert.ToString(Result["BondBOEDate"]);
                    objBond.DepositNo = Convert.ToString(Result["DepositNo"]);
                    objBond.BondNo = Convert.ToString(Result["BondNo"]);
                    objBond.BondDate = Convert.ToString(Result["BondDate"]);
                    objBond.WRNo = Result["WRNo"].ToString();
                    objBond.WRDate = Result["WRDate"].ToString();
                    objBond.GodownName = Result["GodownName"].ToString();
                    objBond.SacNo = Result["SacNo"].ToString();
                    objBond.SacDate = Result["SacDate"].ToString();
                    objBond.UnloadedDate = Result["UnloadedDate"].ToString();
                    objBond.LocationName = Result["LocationName"].ToString();
                    objBond.LocationId = Result["LocationId"].ToString();
                    objBond.AreaOccupied = Convert.ToDecimal(Result["AreaOccupied"]);
                    objBond.CargoDescription = Result["CargoDescription"].ToString();
                    objBond.UnloadedUnits = Convert.ToInt32(Result["UnloadedUnits"]);
                    objBond.UnloadedWeights = Convert.ToDecimal(Result["UnloadedWeights"]);
                    objBond.BalancedUnits = Convert.ToInt32(Result["BalancedUnits"]);
                    objBond.BalancedWeights = Convert.ToDecimal(Result["BalancedWeights"]);
                    objBond.PackageCondition = Result["PackageCondition"].ToString();
                    objBond.Remarks = Result["Remarks"].ToString();
                    objBond.Units = Convert.ToInt32(Result["Units"]);
                    objBond.Weight = Convert.ToDecimal(Result["Weight"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstGodown.Add(new GodownDetails { LocationId = Convert.ToInt32(Result["LocationId"]), LocationName = Result["LocationName"].ToString() });
                    }
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    if (Action == "") _DBResponse.Data = objBond;
                    else _DBResponse.Data = new { objBond, lstGodown };
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
        public void AddEditWOUnloading(WljBondWOUnloading obj)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_UnloadingId", MySqlDbType = MySqlDbType.Int32, Value = obj.UnloadingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = obj.DepositAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAppId", MySqlDbType = MySqlDbType.Int32, Value = obj.SpaceAppId });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_WRDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(obj.WRDate).ToString("yyyy-MM-dd") });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_WRNo", MySqlDbType = MySqlDbType.VarChar, Value = obj.WRNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = obj.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UnloadedDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(obj.UnloadedDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LocationName", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = obj.LocationName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LocationId", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = obj.LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AreaOccupied", MySqlDbType = MySqlDbType.Decimal, Value = obj.AreaOccupied });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UnloadedUnits", MySqlDbType = MySqlDbType.Int32, Value = obj.UnloadedUnits });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UnloadedWeights", MySqlDbType = MySqlDbType.Decimal, Value = obj.UnloadedWeights });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BalancedUnits", MySqlDbType = MySqlDbType.Int32, Value = obj.BalancedUnits });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BalancedWeights", MySqlDbType = MySqlDbType.Decimal, Value = obj.BalancedWeights });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PackageCondition", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = obj.PackageCondition });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = obj.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditWOUnloading", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = ((Result == 1) ? "Unloading Work Order Saved Successfully" : "Unloading Work Order Updated Successfully");
                    _DBResponse.Data = null;
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
        }

        public void PrintWOUnloading(int UnloadingId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_UnloadingId", MySqlDbType = MySqlDbType.Int32, Value = UnloadingId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("BondUnloadingPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            WljBondWOUnloadingPDF ObjUnloading = new WljBondWOUnloadingPDF();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjUnloading.DepositNo = Convert.ToString(Result["DepositNo"]);
                    ObjUnloading.UnloadingDate = Convert.ToString(Result["UnloadedDate"]);
                    ObjUnloading.GodownNo = Result["GodownName"].ToString();
                    ObjUnloading.CargoDescription = Convert.ToString(Result["CargoDesc"] == null ? "" : Result["CargoDesc"]);
                    ObjUnloading.NoOfUnits = Convert.ToInt32(Result["NoOfUnits"] == null ? "" : Result["NoOfUnits"]);
                    ObjUnloading.Weight = Convert.ToDecimal(Result["Weight"] == null ? "" : Result["Weight"]);
                    ObjUnloading.Area = Convert.ToDecimal(Result["Area"] == null ? "" : Result["Area"]);
                    ObjUnloading.PkgCondition = Convert.ToString(Result["PkgCondition"] == null ? "" : Result["PkgCondition"]);
                    ObjUnloading.Remarks = Convert.ToString(Result["Remarks"] == null ? "" : Result["Remarks"]);
                    ObjUnloading.CompanyAddress = Result["CompanyAddress"].ToString();
                    ObjUnloading.EmailAddress = Result["EmailAddress"].ToString();
                    ObjUnloading.Location = Result["Location"].ToString();
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjUnloading;
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

        #region Delivery Order

        public void GetWOForDeliveryOrder()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondWOForDeliveryOrder", CommandType.StoredProcedure, DParam);
            List<ListOfWljWorkOrderNo> LstWONo = new List<ListOfWljWorkOrderNo>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    if (Result["BondNo"].Equals(System.DBNull.Value))
                    {
                    }
                    else
                    {
                        LstWONo.Add(new ListOfWljWorkOrderNo
                        {
                            SacNo = Convert.ToString(Result["SacNo"]),
                            SpaceappId = Convert.ToInt32(Result["SpaceAppId"]),
                            BondNo = Convert.ToString(Result["BondNo"]),
                            // DepositappId = Convert.ToInt32(Result["DepositAppId"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstWONo;
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
        public void GetWODetForDeliveryOrder(int SpaceAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAppId", MySqlDbType = MySqlDbType.Int32, Value = SpaceAppId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondWOForDeliveryOrder", CommandType.StoredProcedure, DParam);
            WljDeliveryOrder ObjDeliOrder = new WljDeliveryOrder();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // ObjWorkOrder.SpaceAppId = Convert.ToInt32(Result["SpaceAppId"]);
                    //  ObjWorkOrder.BondWOId = Convert.ToInt32(Result["BondWOId"]);
                    //  ObjWorkOrder.WorkOrderDate = Result["WorkOrderDate"].ToString();
                    ObjDeliOrder.LastDeliveryDate = Convert.ToString(Result["LastDeliveryDt"]);
                    ObjDeliOrder.SacNo = Convert.ToString(Result["SacNo"]);
                    ObjDeliOrder.BondNo = Convert.ToString(Result["BondNo"]);
                    ObjDeliOrder.BondDate = Convert.ToString(Result["BondDate"]);
                    //  ObjWorkOrder.WRNo = Result["WRNo"].ToString();
                    //  ObjWorkOrder.WRDate = Result["WRDate"].ToString();
                    //   ObjWorkOrder.GodownId = Convert.ToInt32(Result["GodownId"]);
                    //  ObjWorkOrder.SacNo = Result["SacNo"].ToString();
                    ObjDeliOrder.SacDate = Result["SacDate"].ToString();
                    ObjDeliOrder.Importer = Result["Importer"].ToString();
                    ObjDeliOrder.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjDeliOrder.Remarks = Result["Remarks"].ToString();
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        ObjWorkOrder.ClosingUnits = Convert.ToInt32(Result["ClosingUnits"]);
                //        ObjWorkOrder.ClosingWeight = Convert.ToDecimal(Result["ClosingWeight"]==DBNull.Value?0:Result["ClosingWeight"]);
                //    }
                // }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDeliOrder;
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
        public void GetAllDeliveryOrder()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrderId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondDeliveryOrder", CommandType.StoredProcedure, DParam);
            List<ListOfDeliveryOrder> LstBond = new List<ListOfDeliveryOrder>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstBond.Add(new ListOfDeliveryOrder
                    {
                        DeliveryOrderId = Convert.ToInt32(Result["DeliveryOrderId"]),
                        DeliveryOrderNo = Convert.ToString(Result["DeliveryOrderNo"]),
                        DeliveryOrderDate = Convert.ToString(Result["DeliveryOrderDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstBond;
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
        public void GetDeliveryOrder(int DeliveryOrderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrderId", MySqlDbType = MySqlDbType.Int32, Value = DeliveryOrderId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondDeliveryOrder", CommandType.StoredProcedure, DParam);
            WljDeliveryOrder ObjDeliveryOrder = new WljDeliveryOrder();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // ObjDeliveryOrder.BondWOId = Convert.ToInt32(Result["BondWOId"]);
                    ObjDeliveryOrder.SpaceAppId = Convert.ToInt32(Result["SpaceAppId"]);
                    ObjDeliveryOrder.DeliveryOrderId = Convert.ToInt32(Result["DeliveryOrderId"]);
                    ObjDeliveryOrder.DeliveryOrderNo = Result["DeliveryOrderNo"].ToString();
                    ObjDeliveryOrder.DeliveryOrderDate = Result["DeliveryOrderDate"].ToString();
                    //  ObjDeliveryOrder.WorkOrderNo = Result["WorkOrderNo"].ToString();
                    //  ObjDeliveryOrder.WorkOrderDate = Result["WorkOrderDate"].ToString();
                    ObjDeliveryOrder.BondNo = Convert.ToString(Result["BondNo"]);
                    ObjDeliveryOrder.BondDate = Convert.ToString(Result["BondDate"]);
                    //ObjDeliveryOrder.WRNo = Result["WRNo"].ToString();
                    //ObjDeliveryOrder.WRDate = Result["WRDate"].ToString();
                    //ObjDeliveryOrder.GodownId = Convert.ToInt32(Result["GodownId"]);
                    ObjDeliveryOrder.SacNo = Result["SacNo"].ToString();
                    ObjDeliveryOrder.SacDate = Result["SacDate"].ToString();
                    //ObjDeliveryOrder.Units = Convert.ToInt32(Result["Units"]);
                    //ObjDeliveryOrder.Weight = Convert.ToDecimal(Result["Weight"]);
                    //ObjDeliveryOrder.ClosingUnits = Convert.ToInt32(Result["ClosingUnits"]);
                    //ObjDeliveryOrder.ClosingWeight = Convert.ToDecimal(Result["ClosingWeight"]);
                    //ObjDeliveryOrder.Value = Convert.ToDecimal(Result["Value"]);
                    //ObjDeliveryOrder.Duty = Convert.ToDecimal(Result["Duty"]);
                    ObjDeliveryOrder.Remarks = Result["Remarks"].ToString();
                    ObjDeliveryOrder.LastDeliveryDate = Result["LastDeliveryDate"].ToString();
                    ObjDeliveryOrder.Importer = Result["Importer"].ToString();
                    ObjDeliveryOrder.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjDeliveryOrder.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                    ObjDeliveryOrder.TotalValue = Convert.ToDecimal(Result["TotalValue"]);
                    ObjDeliveryOrder.TotalDuty = Convert.ToDecimal(Result["TotalDuty"]);
                    ObjDeliveryOrder.TotalUnits = Convert.ToInt32(Result["TotalUnits"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstDeliveryOrderhdb.Add(new WljDeliveryOrderDtl
                        {
                            WRNo = Result["WRNo"].ToString(),
                            WRDate = Result["WRDate"].ToString(),
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            Units = Convert.ToInt32(Result["Units"]),
                            Weight = Convert.ToDecimal(Result["Weight"]),
                            ClosingUnits = Convert.ToInt32(Result["ClosingUnits"]),
                            Areas = Convert.ToDecimal(Result["AreaApplied"]),
                            ClosingAreas = Convert.ToInt32(Result["ClosingAreas"]),
                            ClosingWeight = Convert.ToDecimal(Result["ClosingWeight"]),
                            Value = Convert.ToDecimal(Result["Value"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            ClosingValue = Convert.ToDecimal(Result["ClosingValue"]),
                            ClosingDuty = Convert.ToDecimal(Result["ClosingDuty"]),
                            DeliveryOrderDtlId = Convert.ToInt32(Result["DeliveryOrderDtlId"]),
                            DepositAppId = Convert.ToInt32(Result["DepositAppId"]),
                            DepositNo = Result["DepositNo"].ToString(),
                            DepositDate = Result["DepositDate"].ToString(),
                            ExBoeNo = Result["ExBoeNo"].ToString(),
                            ExBoeDate = Result["ExBoeDate"].ToString(),
                        });
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDeliveryOrder;
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


        public void PrintDelivery(int DeliveryAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrderId", MySqlDbType = MySqlDbType.Int32, Value = DeliveryAppId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintDeliveryApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            WljDeliveryOrder ObjDeliveryOrder = new WljDeliveryOrder();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDeliveryOrder.SpaceAppId = Convert.ToInt32(Result["SpaceAppId"]);
                    ObjDeliveryOrder.DeliveryOrderId = Convert.ToInt32(Result["DeliveryOrderId"]);
                    ObjDeliveryOrder.DeliveryOrderNo = Result["DeliveryOrderNo"].ToString();
                    ObjDeliveryOrder.DeliveryOrderDate = Result["DeliveryOrderDate"].ToString();
                    //  ObjDeliveryOrder.WorkOrderNo = Result["WorkOrderNo"].ToString();
                    //  ObjDeliveryOrder.WorkOrderDate = Result["WorkOrderDate"].ToString();
                    ObjDeliveryOrder.BondNo = Convert.ToString(Result["BondNo"]);
                    ObjDeliveryOrder.BondDate = Convert.ToString(Result["BondDate"]);
                    ObjDeliveryOrder.BondBOENo = Convert.ToString(Result["BondBOENo"]);
                    ObjDeliveryOrder.BondBOEDate = Convert.ToString(Result["BondBOEDate"]);
                    //ObjDeliveryOrder.WRNo = Result["WRNo"].ToString();
                    //ObjDeliveryOrder.WRDate = Result["WRDate"].ToString();
                    //ObjDeliveryOrder.GodownId = Convert.ToInt32(Result["GodownId"]);
                    ObjDeliveryOrder.SacNo = Result["SacNo"].ToString();
                    ObjDeliveryOrder.SacDate = Result["SacDate"].ToString();
                    //ObjDeliveryOrder.Units = Convert.ToInt32(Result["Units"]);
                    //ObjDeliveryOrder.Weight = Convert.ToDecimal(Result["Weight"]);
                    //ObjDeliveryOrder.ClosingUnits = Convert.ToInt32(Result["ClosingUnits"]);
                    //ObjDeliveryOrder.ClosingWeight = Convert.ToDecimal(Result["ClosingWeight"]);
                    //ObjDeliveryOrder.Value = Convert.ToDecimal(Result["Value"]);
                    //ObjDeliveryOrder.Duty = Convert.ToDecimal(Result["Duty"]);
                    ObjDeliveryOrder.Remarks = Result["Remarks"].ToString();
                    ObjDeliveryOrder.Importer = Result["Importer"].ToString();
                    ObjDeliveryOrder.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjDeliveryOrder.Units_print = Convert.ToDecimal(Result["Units"].ToString());

                    ObjDeliveryOrder.Weight_print = Convert.ToDecimal(Result["Weight"].ToString());
                    ObjDeliveryOrder.GodownId_print = Convert.ToInt32(Result["GodownId"]);
                    ObjDeliveryOrder.CompanyEmail = Result["CompanyEmail"].ToString();
                    ObjDeliveryOrder.CompanyAddress = Result["CompanyAddress"].ToString();
                    ObjDeliveryOrder.CompanyLoaction = Result["Loaction"].ToString();


                    ObjDeliveryOrder.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstDeliveryOrderhdb.Add(new WljDeliveryOrderDtl
                        {
                            WRNo = Result["WRNo"].ToString(),
                            WRDate = Result["WRDate"].ToString(),
                            GodownId = Convert.ToInt32(Result["GodownId"]),
                            Units = Convert.ToInt32(Result["Units"]),
                            Weight = Convert.ToDecimal(Result["Weight"]),
                            ClosingUnits = Convert.ToInt32(Result["ClosingUnits"]),
                            ClosingWeight = Convert.ToDecimal(Result["ClosingWeight"]),
                            Value = Convert.ToDecimal(Result["Value"]),
                            Duty = Convert.ToDecimal(Result["Duty"]),
                            DeliveryOrderDtlId = Convert.ToInt32(Result["DeliveryOrderDtlId"]),
                            DepositAppId = Convert.ToInt32(Result["DepositAppId"]),
                            DepositNo = Result["DepositNo"].ToString(),
                            DepositDate = Result["DepositDate"].ToString(),
                            Remarks = Result["Remarks"].ToString(),
                            CargoDescription = Result["CargoDesc"].ToString(),
                            ExBoeNo = Result["ExBOENo"].ToString(),
                            ExBoeDate = Result["ExBOEDate"].ToString()

                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstDeliveryPrintOrder.Add(new WljDeliveryOrderforPrint
                        {
                            Importer = Result["Importer"].ToString(),
                            CHA = Result["CHA"].ToString(),
                            CargoDesc = Result["CargoDesc"].ToString(),
                            Units = Convert.ToDecimal(Result["Units"]),
                            Weight = Convert.ToDecimal(Result["Weight"]),
                            SacNo = Result["SacNo"].ToString(),
                            SacDate = Result["SacDate"].ToString(),
                            StorageUptoDate = Result["StorageUptoDate"].ToString(),
                            StorageFromDate = Result["StorageFromDate"].ToString(),
                            StorageToDate = Result["StorageToDate"].ToString(),
                            StorageDays = Result["Storagedays"].ToString(),
                            StorageWeek = Result["StorageWeek"].ToString(),
                            PaidStorage = Convert.ToDecimal(Result["PaidStorage"]),
                            InsuranceUptoDate = Result["InsuranceUptoDate"].ToString(),
                            InsuranceFromDate = Result["InsuranceFromDate"].ToString(),
                            InsuranceToDate = Result["InsuranceToDate"].ToString(),
                            InsuranceDays = Result["InsuranceDays"].ToString(),
                            InsuranceWeek = Result["InsuranceWeek"].ToString(),
                            Paidinsurance = Convert.ToDecimal(Result["PaidInsurance"]),
                            // PayableStorage = Convert.ToDecimal(Result["PayableStorage"]),
                            //PayableInsurance = Convert.ToDecimal(Result["PayableInsurance"]),
                            Storagerate = Result["Storagerate"].ToString(),
                            Remarks = Result["Remarks"].ToString(),
                        });
                    }
                }


                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstDeliveryOrderPayment.Add(new WljDeliveryOrderPaymentPrint
                        {
                            SacDate = Result["SacDate"].ToString(),
                            DepositDate = Result["DepositDate"].ToString(),
                            DepositNo = Result["DepositNo"].ToString(),
                            SpaceReq = Convert.ToDecimal(Result["SpaceReq"]),
                            AreaReserved = Convert.ToDecimal(Result["AreaReserved"]),
                            ReceiptNo = Result["ReceiptNo"].ToString(),
                            InsAmt = Convert.ToDecimal(Result["InsAmt"].ToString()),
                            StoAmt = Convert.ToDecimal(Result["StoAmt"].ToString()),
                            FromDate = Result["FromDate"].ToString(),
                            ToDate = Result["ToDate"].ToString(),
                            days = Convert.ToInt32(Result["days"].ToString()),
                            Weeks = Convert.ToInt32(Result["Weeks"].ToString()),
                            InvInsAmt = Convert.ToDecimal(Result["InvInsAmt"].ToString()),
                            InvStoAmt = Convert.ToDecimal(Result["InvStoAmt"].ToString()),
                            TotalTaxable = Convert.ToDecimal(Result["TotalTaxable"].ToString()),
                            //SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"].ToString()),
                            //CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"].ToString()),
                            //IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"].ToString()),
                            InvoiceAmt = Convert.ToDecimal(Result["InvoiceAmt"].ToString()),
                            ReceiptDate = Result["ReceiptDate"].ToString()
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryOrder.LstGst.Add(new WljDeliveryGSTCharge
                        {
                            SGSTAmt = Convert.ToDecimal(Result["SGSTAmt"].ToString()),
                            CGSTAmt = Convert.ToDecimal(Result["CGSTAmt"].ToString()),
                            IGSTAmt = Convert.ToDecimal(Result["IGSTAmt"].ToString()),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDeliveryOrder;
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


        public void AddEditDeliveryOrder(WljDeliveryOrder ObjDeliveryOrder, string DeliveryOrderXml)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrderId", MySqlDbType = MySqlDbType.Int32, Value = ObjDeliveryOrder.DeliveryOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrderXML", MySqlDbType = MySqlDbType.Text, Value = DeliveryOrderXml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAppId", MySqlDbType = MySqlDbType.Int32, Value = ObjDeliveryOrder.SpaceAppId });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = ObjDeliveryOrder.GodownId });
            //    LstParam.Add(new MySqlParameter { ParameterName = "in_WRDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDeliveryOrder.WRDate).ToString("yyyy-MM-dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_WRNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDeliveryOrder.WRNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryOrderDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDeliveryOrder.DeliveryOrderDate).ToString("yyyy/MM/dd") });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Units", MySqlDbType = MySqlDbType.Int32, Value = ObjDeliveryOrder.Units });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = ObjDeliveryOrder.Weight });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_ClosingUnits", MySqlDbType = MySqlDbType.Int32, Value = ObjDeliveryOrder.ClosingUnits });
            //   LstParam.Add(new MySqlParameter { ParameterName = "in_ClosingWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjDeliveryOrder.ClosingWeight });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Value", MySqlDbType = MySqlDbType.Decimal, Value = ObjDeliveryOrder.Value });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Duty", MySqlDbType = MySqlDbType.Decimal, Value = ObjDeliveryOrder.Duty });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = ObjDeliveryOrder.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjDeliveryOrder.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LastDeliveryDate", MySqlDbType = MySqlDbType.Date, Value = (ObjDeliveryOrder.LastDeliveryDate == null ? null : Convert.ToDateTime(ObjDeliveryOrder.LastDeliveryDate).ToString("yyyy-MM-dd")) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjDeliveryOrder.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditBondDeliveryOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = ((Result == 1) ? "Delivery Order Details Saved Successfully" : "Delivery Order Details Updated Successfully");
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Cannot Update Delivery Order Details As It Already Exist In Another Page";
                    _DBResponse.Data = null;
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
        }
        public void GetDepositNoForDelvOrder(int SpaceAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAppId", MySqlDbType = MySqlDbType.Int32, Value = SpaceAppId });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDepositNoForBondDelOrd", CommandType.StoredProcedure, DParam);
            List<DeliveryOrderDtl> LstDepositNo = new List<DeliveryOrderDtl>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDepositNo.Add(new DeliveryOrderDtl
                    {
                        DepositAppId = Convert.ToInt32(Result["DepositAppId"]),
                        DepositNo = Convert.ToString(Result["DepositNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDepositNo;
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
        public void GetDepositDetForDelvOrder(int DepositAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAppId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositAppId", MySqlDbType = MySqlDbType.Int32, Value = DepositAppId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDepositDetForBondDelvOrder", CommandType.StoredProcedure, DParam);
            WljDeliveryOrderDtl ObjDepositNo = new WljDeliveryOrderDtl();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDepositNo.ClosingAreas = Convert.ToDecimal(Result["ClosingAreas"]);
                    ObjDepositNo.ClosingUnits = Convert.ToInt32(Result["ClosingUnits"]);
                    ObjDepositNo.GodownId = Convert.ToInt32(Result["GodownId"]);
                    ObjDepositNo.WRNo = Convert.ToString(Result["WRNo"]);
                    ObjDepositNo.WRDate = Convert.ToString(Result["WRDate"]);
                    ObjDepositNo.ClosingWeight = Convert.ToDecimal(Result["ClosingWeight"]);
                    ObjDepositNo.Duty = Convert.ToDecimal(Result["Duty"]);
                    ObjDepositNo.Value = Convert.ToDecimal(Result["Value"]);
                    ObjDepositNo.DepositDate = Convert.ToString(Result["DepositDate"]);
                    ObjDepositNo.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                    ObjDepositNo.UnloadedUnits = Convert.ToInt32(Result["UnloadedUnits"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDepositNo;
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

        #region Space Availability Certificate (Extend)
        public void AddEditBondSpaceAvailCertExt(WljSpaceAvailCertExtend ObjSpaceAvail)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAvailCertExtId", MySqlDbType = MySqlDbType.Int32, Value = ObjSpaceAvail.SpaceAvailCertExtId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Value = ObjSpaceAvail.SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AreaReserved", MySqlDbType = MySqlDbType.Decimal, Value = ObjSpaceAvail.AreaReserved });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExtendOn", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjSpaceAvail.ExtendOn).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExtendUpto", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjSpaceAvail.ExtendUpto).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Status = DataAccess.ExecuteNonQuery("AddEditBondSpaceAvailCertExt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjSpaceAvail.SpaceAvailCertExtId == 0 ? "Space Availability Certificate (Extend) Details Saved Successfully" : "Space Availability Certificate (Extend) Details Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Status == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Can not Update Space Availability Certificate (Extend) As It Already Used In Next Page";
                    _DBResponse.Data = null;
                }
                else if (Status == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Can not Save as extended SAC application is pending for Approval";
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
        public void DelBondSpaceAvailCertExt(int SpaceAvailCertExtId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAvailCertExtId", MySqlDbType = MySqlDbType.Int32, Value = SpaceAvailCertExtId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Status = DataAccess.ExecuteNonQuery("DelBondSpaceAvailCertExt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Space Availability Certificate (Extend) Details Deleted Successfully";
                    _DBResponse.Data = null;
                }
                else if (Status == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Space Availability Certificate (Extend) Details As It Already Exist In Another Page";
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
        public void GetAllSpaceAvailCertExt()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAvailCertExtId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSpaceAvailCertExt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<WljSpaceAvailCertExtend> LstSpaceAvail = new List<WljSpaceAvailCertExtend>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSpaceAvail.Add(new WljSpaceAvailCertExtend
                    {
                        SpaceAvailCertExtId = Convert.ToInt32(Result["SpaceAvailCertExtId"]),
                        SacNo = Convert.ToString(Result["SacNo"]),
                        ApplicationNo = Result["ApplicationNo"].ToString(),
                        CHAName = (Result["CHAName"] == null ? "" : Result["CHAName"]).ToString(),
                        ImporterName = (Result["ImporterName"] == null ? "" : Result["ImporterName"]).ToString()
                    });

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSpaceAvail;
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
        public void GetSpaceAvailCertExt(int SpaceAvailCertExtId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAvailCertExtId", MySqlDbType = MySqlDbType.Int32, Value = SpaceAvailCertExtId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSpaceAvailCertExt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            WljSpaceAvailCertExtend ObjSpaceAvail = new WljSpaceAvailCertExtend();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSpaceAvail.SpaceAvailCertExtId = Convert.ToInt32(Result["SpaceAvailCertExtId"]);
                    ObjSpaceAvail.AreaReserved = Convert.ToDecimal(Result["AreaReserved"]);
                    ObjSpaceAvail.SpaceappId = Convert.ToInt32(Result["SpaceappId"]);
                    ObjSpaceAvail.ExtendOn = Convert.ToString(Result["ExtendOn"]);
                    ObjSpaceAvail.ExtendUpto = Convert.ToString(Result["ExtendUpto"]);
                    ObjSpaceAvail.SacNo = Convert.ToString(Result["SacNo"]);
                    ObjSpaceAvail.ApplicationNo = Convert.ToString(Result["ApplicationNo"]);
                    ObjSpaceAvail.SacDate = Convert.ToString(Result["SacDate"]);
                    ObjSpaceAvail.ApplicationDate = Convert.ToString(Result["ApplicationDate"]);
                    ObjSpaceAvail.ValidUpto = Convert.ToString(Result["ValidUpto"]);
                    ObjSpaceAvail.CHAName = Convert.ToString(Result["CHAName"]);
                    ObjSpaceAvail.ImporterName = Convert.ToString(Result["ImporterName"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSpaceAvail;
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
        public void GetSacNo()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSacNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<WljSpaceAvailableCert> LstSpaceAvail = new List<WljSpaceAvailableCert>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstSpaceAvail.Add(new WljSpaceAvailableCert
                    {
                        SpaceappId = Convert.ToInt32(Result["SpaceappId"]),
                        SacNo = Result["SacNo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSpaceAvail;
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
        public void GetSacNoDet(int SpaceappId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceappId", MySqlDbType = MySqlDbType.Int32, Value = SpaceappId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSacNoDet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            WljSpaceAvailableCert ObjSpaceAvail = new WljSpaceAvailableCert();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSpaceAvail.AreaReserved = Convert.ToDecimal(Result["AreaReserved"]);
                    ObjSpaceAvail.SacNo = Convert.ToString(Result["SacNo"]);
                    ObjSpaceAvail.ApplicationNo = Convert.ToString(Result["ApplicationNo"]);
                    ObjSpaceAvail.SacDate = Convert.ToString(Result["SacDate"]);
                    ObjSpaceAvail.ApplicationDate = Convert.ToString(Result["ApplicationDate"]);
                    ObjSpaceAvail.ValidUpto = Convert.ToString(Result["ValidUpto"]);
                    ObjSpaceAvail.CHAName = Convert.ToString(Result["CHAName"] == null ? "" : Result["CHAName"]);
                    ObjSpaceAvail.ImporterName = Convert.ToString(Result["ImporterName"] == null ? "" : Result["ImporterName"]);
                    ObjSpaceAvail.BOENo = Convert.ToString(Result["BOENo"] == null ? "" : Result["BOENo"]);
                    ObjSpaceAvail.BOEDate = Convert.ToString(Result["BOEDate"] == null ? "" : Result["BOEDate"]);
                    ObjSpaceAvail.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjSpaceAvail.CargoDescription = Convert.ToString(Result["CargoDescription"] == null ? "" : Result["CargoDescription"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSpaceAvail;
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

        #region Approval for Space Availability Criteria (Extend)
        public void ListOfSpaceAvailabilityExt(string Status, int Skip)
        {
            int status = 0;
            bool State = false;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Status", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = Status });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Skip", MySqlDbType = MySqlDbType.Int32, Value = Skip });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("bondAppExtendList", CommandType.StoredProcedure, DParam);
            List<BondAppExtendList> LstDtl = new List<BondAppExtendList>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    status = 1;
                    LstDtl.Add(new BondAppExtendList
                    {
                        SpaceAvailCertExtId = Convert.ToInt32(Result["SpaceAvailCertExtId"]),
                        SacNo = Convert.ToString(Result["SacNo"]),
                        CHAName = Result["CHAName"].ToString(),
                        ImporterName = Result["ImporterName"].ToString()
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        State = Convert.ToBoolean(Result["State"]);
                    }
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { List = LstDtl, State = State };
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
        public void SearchListSpaceAvailabilityExt(string Status, string Search)
        {
            int status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Status", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = Status });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Search", MySqlDbType = MySqlDbType.VarChar, Value = Search });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("SearchAppExtendListUsingSAC", CommandType.StoredProcedure, DParam);
            List<BondAppExtendList> LstDtl = new List<BondAppExtendList>();
            _DBResponse = new DatabaseResponse();

            try
            {
                while (Result.Read())
                {
                    status = 1;
                    LstDtl.Add(new BondAppExtendList
                    {
                        SpaceAvailCertExtId = Convert.ToInt32(Result["SpaceAvailCertExtId"]),
                        SacNo = Convert.ToString(Result["SacNo"]),
                        CHAName = Result["CHAName"].ToString(),
                        ImporterName = Result["ImporterName"].ToString()
                    });
                }
                if (status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { List = LstDtl };
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
        public void GetSpaceAvailabilityExt(int SpaceAvailCertExtId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAvailCertExtId", MySqlDbType = MySqlDbType.Int32, Value = SpaceAvailCertExtId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondSpaceAvailCertExt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            WljBondAppExtendDetails ObjSpaceAvail = new WljBondAppExtendDetails();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSpaceAvail.SpaceAvailCertExtId = Convert.ToInt32(Result["SpaceAvailCertExtId"]);
                    ObjSpaceAvail.AreaReserved = Convert.ToDecimal(Result["AreaReserved"]);
                    ObjSpaceAvail.SpaceappId = Convert.ToInt32(Result["SpaceappId"]);
                    ObjSpaceAvail.ExtendOn = Convert.ToString(Result["ExtendOn"]);
                    ObjSpaceAvail.ExtendUpto = Convert.ToString(Result["ExtendUpto"]);
                    ObjSpaceAvail.SacNo = Convert.ToString(Result["SacNo"]);
                    ObjSpaceAvail.ApplicationNo = Convert.ToString(Result["ApplicationNo"]);
                    ObjSpaceAvail.SacDate = Convert.ToString(Result["SacDate"]);
                    ObjSpaceAvail.ApplicationDate = Convert.ToString(Result["ApplicationDate"]);
                    ObjSpaceAvail.ValidUpto = Convert.ToString(Result["ValidUpto"]);
                    ObjSpaceAvail.CHAName = Convert.ToString(Result["CHAName"]);
                    ObjSpaceAvail.ImporterName = Convert.ToString(Result["ImporterName"]);
                    ObjSpaceAvail.IsApproved = Convert.ToInt32(Result["IsApproved"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSpaceAvail;
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
        public void UpdateAppExtend(int SpaceAvailCertExtId, int IsApproved)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAvailCertExtId", MySqlDbType = MySqlDbType.Int32, Value = SpaceAvailCertExtId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsApproved", MySqlDbType = MySqlDbType.Int32, Value = IsApproved });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)(HttpContext.Current.Session["LoginUser"])).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Status = DataAccess.ExecuteNonQuery("bondUpdateAppExtend", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Space Availability Certificate (Extend) Details Saved Successfully";
                    _DBResponse.Data = null;
                }
                else if (Status == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Can't Approve / Reject data as Unloading has been done against this SAC";
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
        public void PrintSACExt(int SpaceAvailCertExtId, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceAvailCertExtId", MySqlDbType = MySqlDbType.Int32, Value = SpaceAvailCertExtId });
            LstParam.Add(new MySqlParameter { ParameterName = "BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("PrintSACExt", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            WljSpaceAvailCertPdf ObjSpaceAvail = new WljSpaceAvailCertPdf();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjSpaceAvail.SacNo = Convert.ToString(Result["SacNo"]);
                    ObjSpaceAvail.SacDate = Convert.ToString(Result["SacDate"]);
                    ObjSpaceAvail.BOLAWBNo = Convert.ToString(Result["BOLAWBNo"]);
                    ObjSpaceAvail.Importer = Convert.ToString(Result["ImporterName"]);
                    ObjSpaceAvail.CHAName = Convert.ToString(Result["CHAName"]);
                    ObjSpaceAvail.BOENo = Convert.ToString(Result["BOENo"]);
                    ObjSpaceAvail.BOENoDate = Convert.ToString(Result["BOE"]);
                    ObjSpaceAvail.AreaReserved = Convert.ToDecimal(Result["AreaReserved"]);
                    ObjSpaceAvail.ValidUpto = Convert.ToString(Result["ExtendUpto"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjSpaceAvail;
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

        #region Bond Advance Payment Sheet (Interim)

        public void GetBondAdvancePaymentSheetInterim(string InvoiceDate, int DestuffingAppId, int DeliveryType,string SEZ, string DestuffingAppNo, string DestuffingAppDate, int PartyId, string PartyName,
string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, int CasualLabour,
string InvoiceType, decimal area, int InvoiceId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });

            
            LstParam.Add(new MySqlParameter { ParameterName = "in_SpaceId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UptoDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(DestuffingAppDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CasualLabour", MySqlDbType = MySqlDbType.Int32, Value = CasualLabour });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Area", MySqlDbType = MySqlDbType.Decimal, Value = area });
            LstParam.Add(new MySqlParameter { ParameterName = "In_SacNo", MySqlDbType = MySqlDbType.VarChar, Value = DestuffingAppNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });


            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            WljPostPaymentSheet objInvoice = new WljPostPaymentSheet();
            List<String> Clause = new List<String>();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBondAdvancePaymentSheetInterim", CommandType.StoredProcedure, DParam);

            try
            {
                //DataSet Result = DataAccess.ExecuteDataSet("GetYardPaymentSheetCWCCharges", CommandType.StoredProcedure, DParam);
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
                        objInvoice.lstPrePaymentCont.Add(new WljPreInvoiceContainer
                        {
                            ContainerNo = Result["ContainerNo"] == System.DBNull.Value ? "" : Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"] == System.DBNull.Value ? "" : Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"] == System.DBNull.Value ? "" : Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"] == System.DBNull.Value ? "" : Result["BOENo"].ToString(),
                            GrossWeight = Result["GrossWeight"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerPack = Result["WtPerPack"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WtPerPack"]),
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
                            // ApproveOn = Result["ApproveOn"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterExporter = Result["ImporterExporter"].ToString(),
                            LineNo = Result["LineNo"].ToString(),
                            // OperationType = Convert.ToInt32(Result["OperationType"]),
                            ShippingLineName = Result["ShippingLineName"].ToString(),
                            SpaceOccupiedUnit = Result["SpaceOccupiedUnit"].ToString(),

                        });
                        objInvoice.lstPostPaymentCont.Add(new WljPostPaymentContainer
                        {
                            ContainerNo = Result["ContainerNo"] == System.DBNull.Value ? "" : Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"] == System.DBNull.Value ? "" : Result["CFSCode"].ToString(),
                            ArrivalDate = Result["ArrivalDate"] == System.DBNull.Value ? "" : Result["ArrivalDate"].ToString(),
                            ArrivalTime = Result["ArrivalTime"] == System.DBNull.Value ? "" : Result["ArrivalTime"].ToString(),
                            CargoType = Result["CargoType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["CargoType"]),
                            BOENo = Result["BOENo"] == System.DBNull.Value ? "" : Result["BOENo"].ToString(),
                            GrossWt = Result["GrossWeight"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["GrossWeight"]),
                            WtPerUnit = Result["WtPerPack"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["WtPerPack"]),
                            NoOfPackages = Result["NoOfPackages"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["NoOfPackages"]),
                            CIFValue = Result["CIFValue"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["CIFValue"]),
                            Duty = Result["Duty"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["Duty"]),
                            Reefer = Result["Reefer"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Reefer"]),
                            RMS = Result["RMS"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["RMS"]),
                            DeliveryType = Result["DeliveryType"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["DeliveryType"]),
                            Insured = Result["Insured"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["Insured"]),
                            Size = Result["Size"].ToString(),
                            SpaceOccupied = Result["SpaceOccupied"] == System.DBNull.Value ? 0 : Convert.ToDecimal(Result["SpaceOccupied"]),
                            HeavyScrap = Result["HeavyScrap"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["HeavyScrap"]),
                            AppraisementPerct = Result["AppraisementPerct"] == System.DBNull.Value ? 0 : Convert.ToInt32(Result["AppraisementPerct"]),
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
                        objInvoice.lstPostPaymentChrg.Add(new WljPostPaymentCharge
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
                        objInvoice.lstContWiseAmount.Add(new WljContainerWiseAmount
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
                        objInvoice.lstOperationCFSCodeWiseAmount.Add(new WljOperationCFSCodeWiseAmount
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
                            Clause = Result["Clause"].ToString()
                        });
                    }
                }



                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Clause.Add(Result["Clause"].ToString());

                    }
                }
                objInvoice.ActualApplicable = Clause;
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
                Result.Dispose();
                Result.Close();
            }
        }

        public void GetSACNoForAdvBondPaymentSheetInterim(int CartingAppId = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objBondSacDetails = (IList<HdbBondSacDetails>)DataAccess.ExecuteDynamicSet<HdbBondSacDetails>("GetSACNoForBondAdvPaymentSheeForInterim");

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objBondSacDetails;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }

        public void GetPaymentPartyForAdvanceBond(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPartyForPage", CommandType.StoredProcedure, Dparam);
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



        public void AddEditInvoiceForBond(WljPostPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string CfsChargeXML, int BranchId, int Uid,
       string Module, string CargoXML = "")
        {
            //  var cfsCodeWiseHtRate = HttpContext.Current.Session["lstCfsCodewiseRateHT"];
            //  string cfsCodeWiseHtRateXML = UtilityClasses.Utility.CreateXML(cfsCodeWiseHtRate);
            string GeneratedClientId = "0";
            //  string ReturnObj = "";
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
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ArrivalDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = CfsChargeXML });

            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SEZ });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.DeliveryType });


            // LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditAdvanceBondInvoiceInterim", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            if (GeneratedClientId == "")
            {
                Result = 3;
            }
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully";

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
        #endregion

        #region Bond Delivery Payment

        public void GetSACNoForDelBondPaymentSheet(int CartingAppId = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objBondSacDetails = (List<WljBondSacDetails>)DataAccess.ExecuteDynamicSet<WljBondSacDetails>("GetSACForBondDelivery");

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objBondSacDetails;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }

        public void GetBondDelPaymentSheet(string InvoiceDate, string InvoiceType, String SEZ,int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate, string UptoDate, decimal Area,
           int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, int CasualLabour, int NoOfPrinting, decimal weight, decimal CIFValue, decimal Duty,
           int Units, int IsInsured, string DepositDate, string BOENo, string BOEDate, string ImporterExporter, string CHAName, int InvoiceId = 0)
        {
            if (UptoDate == null)
            {
                UptoDate = "2019-01-01";
            }
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_UptoDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(UptoDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Area", MySqlDbType = MySqlDbType.Decimal, Value = Area });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CasualLabour", MySqlDbType = MySqlDbType.Int32, Value = CasualLabour });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPrinting", MySqlDbType = MySqlDbType.Int32, Value = NoOfPrinting });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            WljBondPostPaymentSheet objInvoice = new WljBondPostPaymentSheet();
            List<String> Clause = new List<String>();
            DataSet ds = DataAccess.ExecuteDataSet("GetBondDelPaymentSheet", CommandType.StoredProcedure, DParam);
            //IDataReader Result = DataAccess.ExecuteDataReader("GetBondDelPaymentSheet", CommandType.StoredProcedure, DParam);
            try
            {

                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    objInvoice.ROAddress = row["ROAddress"].ToString();
                    objInvoice.CompanyName = row["CompanyName"].ToString();
                    objInvoice.CompanyShortName = row["CompanyShortName"].ToString();
                    objInvoice.CompanyAddress = row["CompanyAddress"].ToString();
                    objInvoice.PhoneNo = row["PhoneNo"].ToString();
                    objInvoice.FaxNumber = row["FaxNumber"].ToString();
                    objInvoice.EmailAddress = row["EmailAddress"].ToString();
                    objInvoice.StateId = Convert.ToInt32(row["StateId"]);
                    objInvoice.StateCode = row["StateCode"].ToString();

                    objInvoice.CityId = Convert.ToInt32(row["CityId"]);
                    objInvoice.GstIn = row["GstIn"].ToString();
                    objInvoice.Pan = row["Pan"].ToString();

                    objInvoice.CompGST = row["GstIn"].ToString();
                    objInvoice.CompPAN = row["Pan"].ToString();
                    objInvoice.CompStateCode = row["StateCode"].ToString();
                }

                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    objInvoice.lstPostPaymentChrg.Add(new WljPostPaymentCharge
                    {

                        Clause = row["Clause"].ToString(),
                        ChargeType = row["ChargeType"].ToString(),
                        ChargeName = row["ChargeName"].ToString(),
                        SACCode = row["SACCode"].ToString(),
                        Quantity = Convert.ToInt32(row["Quantity"]),
                        Rate = Convert.ToDecimal(row["Rate"]),
                        Amount = Convert.ToDecimal(row["Amount"]),
                        Discount = Convert.ToDecimal(row["Discount"]),
                        Taxable = Convert.ToDecimal(row["Taxable"]),
                        IGSTPer = Convert.ToDecimal(row["IGSTPer"]),
                        IGSTAmt = Convert.ToDecimal(row["IGSTAmt"]),
                        SGSTPer = Convert.ToDecimal(row["SGSTPer"]),
                        SGSTAmt = Convert.ToDecimal(row["SGSTAmt"]),
                        CGSTPer = Convert.ToDecimal(row["CGSTPer"]),
                        CGSTAmt = Convert.ToDecimal(row["CGSTAmt"]),
                        Total = Convert.ToDecimal(row["Total"])
                    });
                }

                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    Clause.Add(row["Clause"].ToString());
                }

                /* while (Result.Read())
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
                         objInvoice.lstPostPaymentChrg.Add(new PostPaymentCharge
                         {
                             //ChargeId = Convert.ToInt32(Result["OperationId"]),
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
                             //OperationId = Convert.ToInt32(Result["OperationId"]),
                         });
                     }


                 }


                 if (Result.NextResult())
                 {
                     while (Result.Read())
                     {
                         Clause.Add(Result["Clause"].ToString());

                     }
                 }*/
                objInvoice.ActualApplicable = Clause;
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
                /*Result.Dispose();
                Result.Close();*/
            }
        }



        public void AddEditBondDelInvoice(WljBondPostPaymentSheet ObjPostPaymentSheet, string ChargesXML, int BranchId, int Uid, string Module, decimal TotalValueOfCargo)
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLinaName", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterImporterName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ImporterExporter });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.BOEDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalNoOfPackages", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalNoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalGrossWt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalGrossWt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalWtPerUnit", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TotalWtPerUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupied", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Area });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalSpaceOccupiedUnit", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.TotalSpaceOccupiedUnit });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalValueOfCargo", MySqlDbType = MySqlDbType.Decimal, Value = TotalValueOfCargo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompGST", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompPAN", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompPAN });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CompStateCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CompStateCode });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmExaminationDate", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ApproveOn });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingDate", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingDate", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SEZ });
            //LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });
            //LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.Text, Value = ContWiseChargeXML });
            //LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.Text, Value = CargoXML });
            //LstParam.Add(new MySqlParameter { ParameterName = "OperationCfsCodAmtXML", MySqlDbType = MySqlDbType.Text, Value = OperationCfsCodeWiseAmtXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = "" });            
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditInvoiceBondDel", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
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

        public void ListOfBondInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfBondInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<WljListOfBondInvoice> lstInvoice = new List<WljListOfBondInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstInvoice.Add(new WljListOfBondInvoice()
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
                    _DBResponse.Data = lstInvoice;
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

    }
}