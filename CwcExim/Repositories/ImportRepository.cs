using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
using CwcExim.Areas.Import.Models;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using CwcExim.UtilityClasses;
using EinvoiceLibrary;

namespace CwcExim.Repositories
{
    public class ImportRepository
    {
        private DatabaseResponse _DBResponse;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
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

        #region Import Job Order
        public void GetAllBlNofromFormOne()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetBlnoFromform1", CommandType.StoredProcedure, dpram);
            IList<BlNoList> lstBlNo = new List<BlNoList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstBlNo.Add(new BlNoList { BlNo = result["BLNo"].ToString(), FormOneId = Convert.ToInt32(result["FormOneId"]) });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstBlNo;
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
        public void GetBlNoDtl(int FormOneId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Value = FormOneId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetBlnoFromform1", CommandType.StoredProcedure, dpram);
            FormOneForImpJO objFormone = new FormOneForImpJO();
            IList<FormOneDtlForImpJO> lstDtl = new List<FormOneDtlForImpJO>();
            IList<Areas.Import.Models.Yard> lstYard = new List<Areas.Import.Models.Yard>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    // objFormone.FormOneNo = result["FormOneNo"].ToString();
                    objFormone.FormOneDate = result["FormOneDate"].ToString();
                    objFormone.ShippingLineId = Convert.ToInt32(result["ShippingLineId"] == DBNull.Value ? 0 : result["ShippingLineId"]);
                    objFormone.ShippingLineName = (result["ShippingLineName"] == null ? "" : result["ShippingLineName"]).ToString();
                    objFormone.CHAId = Convert.ToInt32(result["CHAId"]);
                    objFormone.CHAName = (result["CHAName"]).ToString();
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        lstDtl.Add(new FormOneDtlForImpJO
                        {
                            FormOneDetailId = Convert.ToInt32(result["FormOneDetailId"]),
                            ContainerNo = result["ContainerNo"].ToString(),
                            ContainerSize = result["ContainerSize"].ToString(),
                            Reefer = (Convert.ToInt32(result["Reefer"]) == 1 ? "Reefer" : "Non Reefer").ToString(),
                            SealNo = (result["SealNo"] == null ? "" : result["SealNo"]).ToString()
                        });
                    }
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        lstYard.Add(new Areas.Import.Models.Yard { YardId = Convert.ToInt32(result["YardId"]), YardName = result["YardName"].ToString() });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = new { FormOne = objFormone, FormOneDtl = lstDtl, YardList = lstYard };
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
        public void GetAllImpJO()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfImpJobOrd", CommandType.StoredProcedure, dpram);
            IList<ImportJobOrderList> lstImpJO = new List<ImportJobOrderList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstImpJO.Add(new ImportJobOrderList
                    {
                        ImpJobOrderId = Convert.ToInt32(result["ImpJobOrderId"]),
                        JobOrderNo = result["JobOrderNo"].ToString(),
                        JobOrderDate = result["JobOrderDate"].ToString(),
                        FormOneNo = result["FormOneNo"].ToString()
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
            IDataReader result = DA.ExecuteDataReader("ListOfImpJobOrd", CommandType.StoredProcedure, dpram);
            ImportJobOrder objImpJO = new ImportJobOrder();
            IList<ImportJobOrderDtl> lstDtl = new List<ImportJobOrderDtl>();
            IList<Areas.Import.Models.Yard> lstYard = new List<Areas.Import.Models.Yard>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objImpJO.PickUpLocation = result["PickUpLocation"].ToString();
                    objImpJO.ImpJobOrderId = Convert.ToInt32(result["ImpJobOrderId"]);
                    objImpJO.JobOrderNo = result["JobOrderNo"].ToString();
                    objImpJO.JobOrderDate = result["JobOrderDate"].ToString();
                    objImpJO.FormOneNo = result["FormOneNo"].ToString();
                    objImpJO.ShippingLineId = Convert.ToInt32(result["ShippingLineId"]);
                    objImpJO.CHAId = Convert.ToInt32(result["CHAId"] == DBNull.Value ? 0 : result["CHAId"]);
                    objImpJO.FromLocation = result["FromLocation"].ToString();
                    objImpJO.ToYardId = Convert.ToInt32(result["ToYardId"]);
                    objImpJO.YardWiseLocationIds = result["YardWiseLocationIds"].ToString();
                    objImpJO.YardWiseLctnNames = result["YardWiseLctnNames"].ToString();
                    objImpJO.Remarks = result["Remarks"] == null ? "" : result["Remarks"].ToString();
                    objImpJO.ShippingLineName = result["ShippingLineName"].ToString();
                    objImpJO.CHAName = (result["CHAName"] == null ? "" : result["CHAName"]).ToString();
                    objImpJO.FormOneNo = result["FormOneNo"].ToString();
                    objImpJO.FormOneDate = result["FormOneDate"].ToString();
                    objImpJO.YardName = result["YardName"].ToString();
                    //objImpJO.BlNo = result["BLNo"].ToString();
                    objImpJO.FormOneId = Convert.ToInt32(result["FormOneId"]);
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        lstDtl.Add(new ImportJobOrderDtl
                        {
                            FormOneDetailId = Convert.ToInt32(result["FormOneDetailId"]),
                            ContainerNo = result["ContainerNo"].ToString(),
                            ContainerSize = result["ContainerSize"].ToString(),
                            Reefer = (Convert.ToInt32(result["Reefer"]) == 1 ? "Reefer" : "Non Reefer").ToString(),
                            SealNo = (result["SealNo"] == null ? "" : result["SealNo"]).ToString()
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
            int result = DA.ExecuteNonQuery("DeleteImpJO", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Import Job Order Deleted Successfully";
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
        public void AddEditImpJO(ImportJobOrder objJO, string XML = null, string LocationXML = null)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = objJO.ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PickUpLocation", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objJO.PickUpLocation });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Value = objJO.FormOneId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderFor", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objJO.JobOrderFor });
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objJO.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objJO.ShippingLineId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = objJO.CHAId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_FromLocation", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = objJO.FromLocation });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ToYardId", MySqlDbType = MySqlDbType.Int32, Value = objJO.ToYardId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_YardWiseLocationIds", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = objJO.YardWiseLocationIds });
            lstParam.Add(new MySqlParameter { ParameterName = "in_YardWiseLctnNames", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = objJO.YardWiseLctnNames });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = objJO.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = XML });
            //  lstParam.Add(new MySqlParameter { ParameterName = "in_LctnXML", MySqlDbType = MySqlDbType.Text, Value = LocationXML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditImpJO", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Import Job Order Saved Successfully" : "Import Job Order Saved Successfully";
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
        public void GetYardWiseLocation(int YardId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_YardId", MySqlDbType = MySqlDbType.Int32, Value = YardId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetYardWiseLocation", CommandType.StoredProcedure, dpram);
            IList<Areas.Import.Models.YardWiseLocation> lstDtl = new List<Areas.Import.Models.YardWiseLocation>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstDtl.Add(new Areas.Import.Models.YardWiseLocation
                    {
                        LocationId = Convert.ToInt32(result["LocationId"]),
                        Location = result["Location"].ToString(),
                        //IsOccupied = Convert.ToBoolean(result["IsOccupied"])
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

        #region Import Job Order Print
        public void GetImportJODetailsFrPrint(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetDetForPrntimpjo", CommandType.StoredProcedure, dpram);
            PrintJOModel objMdl = new PrintJOModel();
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
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objMdl.lstDet.Add(new PrintJOModelDet
                        {
                            ContainerNo = result["ContainerNo"].ToString(),
                            ContainerSize = result["ContainerSize"].ToString()
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
        #endregion

        #endregion

        #region Custom Appraisement Application
        public void AddEditCustomAppraisement(CustomAppraisement ObjAppraisement, string AppraisementXML)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjAppraisement.CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjAppraisement.AppraisementDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.CHAId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Vessel });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Voyage", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Voyage });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Rotation", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Rotation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fob", MySqlDbType = MySqlDbType.Decimal, Value = ObjAppraisement.Fob });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossDuty", MySqlDbType = MySqlDbType.Decimal, Value = ObjAppraisement.GrossDuty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsDO", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.IsDO });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjAppraisement.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementXML", MySqlDbType = MySqlDbType.Text, Value = AppraisementXML });
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
            CustomAppraisement ObjAppraisement = new CustomAppraisement();
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
                    //ObjAppraisement.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    //ObjAppraisement.Voyage = Result["Voyage"].ToString();
                    ObjAppraisement.Rotation = Result["Rotation"].ToString();
                    ObjAppraisement.Fob = Convert.ToDecimal(Result["Fob"]);
                    ObjAppraisement.GrossDuty = Convert.ToDecimal(Result["GrossDuty"]);
                    ObjAppraisement.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    ObjAppraisement.IsDO = Convert.ToInt32(Result["IsDO"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.LstAppraisement.Add(new CustomAppraisementDtl
                        {
                            CustomAppraisementDtlId = Convert.ToInt32(Result["CustomAppraisementDtlId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                            LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString(),
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
                            Reefer = Convert.ToInt32(Result["Reefer"] == DBNull.Value ? 0 : Result["Reefer"]),
                            RMS = Convert.ToInt32(Result["RMS"] == DBNull.Value ? 0 : Result["RMS"]),
                            HeavyScrap = Convert.ToInt32(Result["HeavyScrap"] == DBNull.Value ? 0 : Result["HeavyScrap"]),
                            AppraisementPerct = Convert.ToInt32(Result["AppraisementPerct"] == DBNull.Value ? 0 : Result["AppraisementPerct"]),
                            IsInsured = Convert.ToInt32(Result["IsInsured"])
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
            List<CustomAppraisement> LstAppraisement = new List<CustomAppraisement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new CustomAppraisement
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
            List<CustomAppraisementDtl> LstAppraisement = new List<CustomAppraisementDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new CustomAppraisementDtl
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
        public void GetContnrDetForCustomAppraise(string CFSCode, string LineNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LineNo", MySqlDbType = MySqlDbType.VarChar, Value = LineNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCntrDetForImpCstmAppraise", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CustomAppraisementDtl ObjAppraisement = new CustomAppraisementDtl();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppraisement.ContainerNo = (Result["ContainerNo"] == null ? "" : Result["ContainerNo"]).ToString();
                    ObjAppraisement.CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString();
                    ObjAppraisement.Size = (Result["Size"] == null ? "" : Result["Size"]).ToString();
                    ObjAppraisement.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    // ObjAppraisement.LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString();
                    ObjAppraisement.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjAppraisement.Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString();
                    ObjAppraisement.CHANameId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]);
                    ObjAppraisement.CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString();
                    ObjAppraisement.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjAppraisement.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    ObjAppraisement.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                    ObjAppraisement.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjAppraisement.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjAppraisement.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjAppraisement.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjAppraisement.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                    ObjAppraisement.ContainerType = Convert.ToInt32(Result["ContainerType"] == DBNull.Value ? 0 : Result["ContainerType"]);
                    ObjAppraisement.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    ObjAppraisement.Reefer = Convert.ToInt32(Result["Reefer"] == DBNull.Value ? 0 : Result["Reefer"]);
                    // ObjAppraisement.RMS = Convert.ToInt32(Result["RMS"] == DBNull.Value ? 0 : Result["RMS"]);
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

        #endregion

        #region Custom Appraisement Work Order

        public void AddEditCstmAppraiseWorkOrdr(CstmAppraiseWorkOrder ObjWorkOrder, string AppraisementXML /* , string YardXML*/)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseWorkOrderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjWorkOrder.CstmAppraiseWorkOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Value = ObjWorkOrder.CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WorkOrderDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjWorkOrder.WorkOrderDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjWorkOrder.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjWorkOrder.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_YardId", MySqlDbType = MySqlDbType.Int32, Value = ObjWorkOrder.YardId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_YardWiseLocationIds", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = ObjWorkOrder.YardWiseLocationIds });
            LstParam.Add(new MySqlParameter { ParameterName = "in_YardWiseLctnNames", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = ObjWorkOrder.YardWiseLctnNames });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = ObjWorkOrder.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WorkOrderXML", MySqlDbType = MySqlDbType.Text, Value = AppraisementXML });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_YardXML", MySqlDbType = MySqlDbType.Text, Value = YardXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditImpCstmAppraiseWorkOrdr", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjWorkOrder.CstmAppraiseWorkOrderId == 0 ? "Custom Appraisement Work Order Details Saved Successfully" : "Custom Appraisement Work Order Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Update Custom Appraisement Work Order Details As It Already Exists In Another Page";
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
        public void GetAppraisementNoForWorkOrdr()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppraisementNoForWorkOrdr", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CstmAppraiseWorkOrder> LstAppraisement = new List<CstmAppraiseWorkOrder>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new CstmAppraiseWorkOrder
                    {
                        AppraisementNo = Result["AppraisementNo"].ToString(),
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
        public void GetCstmAppraiseDtlForWorkOrdr(int CustomAppraisementId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCstmAppraiseDtlForWorkOrdr", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CustomAppraisement ObjAppraisement = new CustomAppraisement();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppraisement.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjAppraisement.CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]);
                    ObjAppraisement.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    ObjAppraisement.AppraisementNo = Result["AppraisementNo"].ToString();
                    ObjAppraisement.AppraisementDate = Result["AppraisementDate"].ToString();
                    ObjAppraisement.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.LstAppraisement.Add(new CustomAppraisementDtl
                        {
                            CustomAppraisementDtlId = Convert.ToInt32(Result["CustomAppraisementDtlId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            Size = Result["Size"].ToString(),
                            Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString()
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
        public void GetCstmAppraiseWorkOrder(int CstmAppraiseWorkOrderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseWorkOrderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CstmAppraiseWorkOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCstmAppraiseWorkOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CstmAppraiseWorkOrder ObjAppraisement = new CstmAppraiseWorkOrder();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppraisement.CstmAppraiseWorkOrderId = Convert.ToInt32(Result["CstmAppraiseWorkOrderId"]);
                    ObjAppraisement.CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]);
                    ObjAppraisement.CstmAppraiseWorkOrderNo = Result["CstmAppraiseWorkOrderNo"].ToString();
                    ObjAppraisement.WorkOrderDate = Result["WorkOrderDate"].ToString();
                    ObjAppraisement.AppraisementNo = Result["AppraisementNo"].ToString();
                    ObjAppraisement.AppraisementDate = Result["AppraisementDate"].ToString();
                    ObjAppraisement.Remarks = Result["Remarks"].ToString();
                    ObjAppraisement.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    ObjAppraisement.Rotation = Result["Rotation"].ToString();
                    ObjAppraisement.ShippingLine = Result["ShippingLine"].ToString();
                    ObjAppraisement.YardId = Convert.ToInt32(Result["YardId"]);
                    ObjAppraisement.YardName = Convert.ToString(Result["YardName"] == null ? "" : Result["YardName"]);
                    ObjAppraisement.YardWiseLocationIds = Convert.ToString(Result["YardWiseLocationIds"] == null ? "" : Result["YardWiseLocationIds"]);
                    ObjAppraisement.YardWiseLctnNames = Convert.ToString(Result["YardWiseLctnNames"] == null ? "" : Result["YardWiseLctnNames"]);
                    ObjAppraisement.IsApproved = Convert.ToInt32(Result["IsApproved"]);
                    ObjAppraisement.Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]);
                    ObjAppraisement.GrossDuty = Convert.ToDecimal(Result["GrossDuty"] == DBNull.Value ? 0 : Result["GrossDuty"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.LstAppraisementDtl.Add(new CustomAppraisementDtl
                        {
                            CustomAppraisementDtlId = Convert.ToInt32(Result["CustomAppraisementDtlId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                            // LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString(),
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
        public void GetAllCstmAppraiseWorkOrder()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseWorkOrderId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = HttpContext.Current.Session["BranchId"] });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCstmAppraiseWorkOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<CstmAppraiseWorkOrder> LstAppraisement = new List<CstmAppraiseWorkOrder>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstAppraisement.Add(new CstmAppraiseWorkOrder
                    {
                        CstmAppraiseWorkOrderNo = Result["CstmAppraiseWorkOrderNo"].ToString(),
                        WorkOrderDate = Result["WorkOrderDate"].ToString(),
                        CstmAppraiseWorkOrderId = Convert.ToInt32(Result["CstmAppraiseWorkOrderId"])
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
        public void DelCstmAppraiseWorkOrder(int CstmAppraiseWorkOrderId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseWorkOrderId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = CstmAppraiseWorkOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelImpCstmAppraiseWorkOrder", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Custom Appraisement Work Order Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Custom Appraisement Work Order Details As It Exist In Another Page";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete Custom Appraisement Work Order Details As It Exist In Destuffing Application";
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
        public void GetCstmAppraiseWOForPreview(int CstmAppraiseWorkOrderId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseWorkOrderId", MySqlDbType = MySqlDbType.Int32, Value = CstmAppraiseWorkOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCstmAppraiseWOForPreview", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            CstmAppraiseWorkOrder ObjAppraisement = new CstmAppraiseWorkOrder();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjAppraisement.LstAppraisementDtl.Add(new CustomAppraisementDtl
                    {
                        CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                        Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                        GrossWeight = Convert.ToInt32(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                        Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                        ContainerNo = (Result["ContainerNo"] == null ? "" : Result["ContainerNo"]).ToString(),
                    });
                    ObjAppraisement.AppraisementNo = Result["CstmAppraiseWorkOrderNo"].ToString();
                    ObjAppraisement.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjAppraisement.YardWiseLctnNames = (Result["YardWiseLctnNames"] == null ? "" : Result["YardWiseLctnNames"]).ToString();
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

        #endregion

        #region Custom Appraisement Approval
        public void NewCustomeAppraisement(int Skip)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Skip", MySqlDbType = MySqlDbType.Int32, Value = Skip });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfNewAppfrCustApp", CommandType.StoredProcedure, dpram);
            IList<ListOfCustAppraismentAppr> lstApproval = new List<ListOfCustAppraismentAppr>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            bool State = false;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstApproval.Add(new ListOfCustAppraismentAppr
                    {
                        CstmAppraiseWorkOrderId = Convert.ToInt32(result["CstmAppraiseWorkOrderId"]),
                        CstmAppraiseWorkOrderNo = result["CstmAppraiseWorkOrderNo"].ToString(),
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
            IDataReader result = DA.ExecuteDataReader("ListOfAppHoldCustAppForRemoval", CommandType.StoredProcedure, dpram);
            IList<ListOfCustAppraismentAppr> lstApproval = new List<ListOfCustAppraismentAppr>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            bool State = false;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstApproval.Add(new ListOfCustAppraismentAppr
                    {
                        CstmAppraiseWorkOrderId = Convert.ToInt32(result["CstmAppraiseWorkOrderId"]),
                        CstmAppraiseWorkOrderNo = result["CstmAppraiseWorkOrderNo"].ToString(),
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
        public void UpdateCustomApproval(int CstmAppraiseWorkOrderId, int IsApproved, int Uid)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseWorkOrderId", MySqlDbType = MySqlDbType.Int32, Value = CstmAppraiseWorkOrderId });
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

        #region Destuffing Application
        public void AddEditDestuffing(Destuffing ObjDestuffing, string DestuffingXML, int BranchId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDestuffing.DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomAppraisementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDestuffing.CustomAppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.DestuffingDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.CHAId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Vessel });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_Voyage", MySqlDbType = MySqlDbType.VarChar, Value = ObjAppraisement.Voyage });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Rotation", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Rotation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fob", MySqlDbType = MySqlDbType.Decimal, Value = ObjDestuffing.Fob });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossDuty", MySqlDbType = MySqlDbType.Decimal, Value = ObjDestuffing.GrossDuty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryType", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.DeliveryType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsDO", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.IsDO });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingXML", MySqlDbType = MySqlDbType.Text, Value = DestuffingXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditImpDestuffingApp", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjDestuffing.DestuffingId == 0 ? "Destuffing Application Details Saved Successfully" : "Destuffing Application Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Edit Destuffing Application Details As It Already Exists In Another Page";
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
        public void DelDestuffing(int DestuffingId, int BranchId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelImpDestuffingApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Destuffing Application Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Destuffing Application Details As It Exists In Another Page";
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
        public void GetDestuffing(int DestuffingId, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            Destuffing ObjDestuffing = new Destuffing();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDestuffing.ContainerNo = "";
                    ObjDestuffing.DestuffingId = Convert.ToInt32(Result["DestuffingId"]);
                    ObjDestuffing.DestuffingNo = Result["DestuffingNo"].ToString();
                    ObjDestuffing.DestuffingDate = Result["DestuffingDate"].ToString();
                    ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    ObjDestuffing.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjDestuffing.CHAName = Result["CHA"].ToString();
                    ObjDestuffing.ShippingLine = Result["ShippingLine"].ToString();
                    //ObjAppraisement.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    //ObjAppraisement.Voyage = Result["Voyage"].ToString();
                    ObjDestuffing.Rotation = Result["Rotation"].ToString();
                    ObjDestuffing.Fob = Convert.ToDecimal(Result["Fob"]);
                    ObjDestuffing.GrossDuty = Convert.ToDecimal(Result["GrossDuty"]);
                    ObjDestuffing.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    ObjDestuffing.IsDO = Convert.ToInt32(Result["IsDO"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.LstDestuffing.Add(new DestuffingDtl
                        {
                            DestuffingDtlId = Convert.ToInt32(Result["DestuffingDtlId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                            Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                            LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString(),
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
                            IsInsured = Convert.ToInt32(Result["IsInsured"] == DBNull.Value ? 0 : Result["IsInsured"])
                        });
                    }
                }
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
        public void GetAllDestuffing(int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DestuffingAppList> LstDestuffing = new List<DestuffingAppList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new DestuffingAppList
                    {
                        DestuffingNo = Result["DestuffingNo"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        DestuffingId = Convert.ToInt32(Result["DestuffingId"]),
                        BOENo = Result["BOENo"].ToString(),
                        Importer = Result["Importer"].ToString(),
                        CHA = Result["CHA"].ToString()
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
        public void GetContnrNoForDestuffing(int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCntrNoForImpDestuffingApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DestuffingDtl> LstDestuffing = new List<DestuffingDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new DestuffingDtl
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString()
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
        public void GetContnrDetForDestuffing(string CFSCode, string LineNo, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LineNo", MySqlDbType = MySqlDbType.VarChar, Value = LineNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCntrDetForImpDestuffingApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DestuffingDtl ObjDestuffingDtl = new DestuffingDtl();
            Destuffing ObjDestuffing = new Destuffing();

            try
            {
                while (Result.Read())
                {
                    if (Convert.ToInt32(Result["CustomAppraisementId"]) == 0)
                    {
                        Status = 2;
                        //ObjDestuffingDtl.CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        ObjDestuffingDtl.ContainerNo = Result["ContainerNo"].ToString();
                        ObjDestuffingDtl.CFSCode = Result["CFSCode"].ToString();
                        ObjDestuffingDtl.Size = (Result["Size"] == null ? "" : Result["Size"]).ToString();
                        ObjDestuffingDtl.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                        // ObjDestuffingDtl.LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString();
                        ObjDestuffingDtl.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                        ObjDestuffingDtl.Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString();
                        ObjDestuffingDtl.CHANameId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]);
                        ObjDestuffingDtl.CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString();
                        ObjDestuffingDtl.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                        ObjDestuffingDtl.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                        ObjDestuffingDtl.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                        ObjDestuffingDtl.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                        ObjDestuffingDtl.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                        ObjDestuffingDtl.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                        ObjDestuffingDtl.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                        ObjDestuffingDtl.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    }
                    else
                    {
                        Status = 1;
                        ObjDestuffing.CustomAppraisementId = Convert.ToInt32(Result["CustomAppraisementId"]);
                        ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                        ObjDestuffing.CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]);
                        ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                        ObjDestuffing.Fob = Convert.ToDecimal(Result["Fob"] == DBNull.Value ? 0 : Result["Fob"]);
                        ObjDestuffing.GrossDuty = Convert.ToDecimal(Result["GrossDuty"] == DBNull.Value ? 0 : Result["GrossDuty"]);
                        ObjDestuffing.DeliveryType = Convert.ToInt32(Result["DeliveryType"] == DBNull.Value ? 0 : Result["DeliveryType"]);
                        ObjDestuffing.IsDO = Convert.ToInt32(Result["IsDO"] == DBNull.Value ? 0 : Result["IsDO"]);
                        ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                        ObjDestuffing.CHAName = (Result["CHA"] == null ? "" : Result["CHA"]).ToString();

                        if (Result.NextResult())
                        {
                            while (Result.Read())
                            {
                                ObjDestuffing.LstDestuffing.Add(new DestuffingDtl
                                {
                                    CustomAppraisementDtlId = Convert.ToInt32(Result["CustomAppraisementDtlId"]),
                                    ContainerNo = Result["ContainerNo"].ToString(),
                                    CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString(),
                                    Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                                    LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString(),
                                    LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                                    BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                                    BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                                    Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                                    Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString(),
                                    CHANameId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]),
                                    ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]),
                                    Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                                    CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                                    CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                                    NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                                    GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                                    CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                                    Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                                    WithoutDOSealNo = (Result["WithoutDOSealNo"] == null ? "" : Result["WithoutDOSealNo"]).ToString()
                                });
                            }
                        }
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDestuffing;
                }
                else if (Status == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDestuffingDtl;
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
        public void GetSearchDestuffing(int BranchId,string SearchText)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_SearchText", MySqlDbType = MySqlDbType.VarChar, Value = SearchText });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSearchImpDestuffingApp", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DestuffingAppList> LstDestuffing = new List<DestuffingAppList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new DestuffingAppList
                    {
                        DestuffingNo = Result["DestuffingNo"].ToString(),
                        DestuffingDate = Result["DestuffingDate"].ToString(),
                        DestuffingId = Convert.ToInt32(Result["DestuffingId"]),
                        BOENo = Result["BOENo"].ToString(),
                        Importer = Result["Importer"].ToString(),
                        CHA = Result["CHA"].ToString()
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

        #endregion

        #region Destuffing Workorder
        public void GetContainerDetails()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(ConfigurationManager.AppSettings["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContNoFrDeStuffWO", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<ContDetails> lstCont = new List<ContDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCont.Add(new ContDetails
                    {
                        DestuffingId = Convert.ToInt32(Result["DestuffingId"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCont;
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
        public void GetContainerWiseWODet(int DestuffingId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("DeStuffingWODet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DestuffingWO objDSWO = new DestuffingWO();
            IList<DestuffingWODtl> lstDSWODtl = new List<DestuffingWODtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objDSWO.ApplicationNo = Result["DestuffingNo"].ToString();
                    objDSWO.ApplicationDate = Result["DestuffingDate"].ToString();
                    objDSWO.ShippingLine = Result["ShippingLineName"].ToString();
                    objDSWO.Rotation = Result["Rotation"].ToString();
                    objDSWO.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstDSWODtl.Add(new DestuffingWODtl
                        {
                            DestuffingDtlId = Convert.ToInt32(Result["DestuffingDtlId"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            CFSCode = Result["CFSCode"].ToString(),
                            Size = Result["Size"].ToString(),
                            Vessel = Result["Vessel"].ToString(),
                            Voyage = Result["Voyage"].ToString(),
                            LineNo = Result["LineNo"].ToString(),
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterName = Result["ImporterName"].ToString(),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { Hdr = objDSWO, dtl = lstDSWODtl };
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
        public void GetDestuffingWODet(int DeStuffingWOId, string action = null)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeStuffingWOId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DeStuffingWOId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Action", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = action });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfDeStuffWO", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DestuffingWO objDSWO = new DestuffingWO();
            IList<DestuffingWODtl> lstDSWODtl = new List<DestuffingWODtl>();
            IList<GodownWiseLctn> lstLctn = new List<GodownWiseLctn>();
            try
            {
                while (Result.Read())
                {
                    objDSWO.ApplicationNo = Result["DestuffingNo"].ToString();
                    objDSWO.ApplicationDate = Result["DestuffingDate"].ToString();
                    objDSWO.ShippingLine = Result["ShippingLineName"].ToString();
                    objDSWO.Rotation = Result["Rotation"].ToString();
                    objDSWO.DeliveryType = Convert.ToInt32(Result["DeliveryType"]);
                    objDSWO.GodownId = Convert.ToInt32(Result["GodownId"]);
                    objDSWO.GodownName = Result["GodownName"].ToString();
                    objDSWO.GodwnWiseLctnIds = Result["GodownWiseLctnIds"].ToString();
                    objDSWO.GodwnWiseLctnNames = Result["GodownWiseLctnNames"].ToString();
                    objDSWO.WorkOrderNo = Result["WorkOrderNo"].ToString();
                    objDSWO.WorkOrderDate = Result["WorkOrderDate"].ToString();
                    objDSWO.Remarks = Result["Remarks"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        lstDSWODtl.Add(new DestuffingWODtl
                        {
                            DestuffingDtlId = Convert.ToInt32(Result["DestuffingDtlId"]),
                            ContainerNo = Convert.ToString(Result["ContainerNo"]),
                            CFSCode = Result["CFSCode"].ToString(),
                            Size = Result["Size"].ToString(),
                            Vessel = Result["Vessel"].ToString(),
                            Voyage = Result["Voyage"].ToString(),
                            LineNo = Result["LineNo"].ToString(),
                            BOENo = Result["BOENo"].ToString(),
                            BOEDate = Result["BOEDate"].ToString(),
                            CHAName = Result["CHAName"].ToString(),
                            ImporterName = Result["ImporterName"].ToString(),
                            CargoDescription = Result["CargoDescription"].ToString(),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstLctn.Add(new Areas.Import.Models.GodownWiseLctn
                        {
                            LocationId = Convert.ToInt32(Result["LocationId"]),
                            LocationName = Result["LocationName"].ToString()
                            //IsOccupied=Convert.ToInt32(Result["IsOccupied"])
                        });
                    }
                }
                if (lstDSWODtl.Count > 0)
                {
                    objDSWO.StringifyXML = Newtonsoft.Json.JsonConvert.SerializeObject(lstDSWODtl);
                }
                if (lstLctn.Count > 0)
                {
                    objDSWO.LctnXML = Newtonsoft.Json.JsonConvert.SerializeObject(lstLctn);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objDSWO;
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
        public void DeleteDestuffingWO(int DeStuffingWOId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeStuffingWOId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DeStuffingWOId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteDeStuffWO", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Destuffing WorkOrder Deleted Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete As It Is Used In Another Page";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = -1;
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
        public void ListOfDeStuffWO()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeStuffingWOId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Action", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = "" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfDeStuffWO", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<ListDestuffingWO> lstDSWO = new List<ListDestuffingWO>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstDSWO.Add(new ListDestuffingWO
                    {
                        DeStuffingWOId = Convert.ToInt32(Result["DeStuffingWOId"]),
                        WorkOrderNo = Result["WorkOrderNo"].ToString(),
                        ApplicationNo = Convert.ToString(Result["DestuffingNo"]),
                        ApplicationDate = Result["DestuffingDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstDSWO;
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
        public void AddEditDeStuffWO(DestuffingWO objWO, string XML)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeStuffingWOId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objWO.DeStuffingWOId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objWO.DeStuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WorkOrderDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objWO.WorkOrderDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = objWO.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownWiseLctnIds", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = objWO.GodwnWiseLctnIds });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownWiseLctnNames", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = objWO.GodwnWiseLctnNames });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objWO.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = XML });
            LstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Size = 11, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditDeStuffWO", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Destuffing WorkOrder Saved Successfully" : "Destuffing WorkOrder Updated Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Cannot Edit Destuffing Work Order Details As It Already Exist In Another Page";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = -1;
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
        public void GetDestuffingWOForPreview(int DeStuffingWOId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeStuffingWOId", MySqlDbType = MySqlDbType.Int32, Value = DeStuffingWOId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDestuffingWOForPreview", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DestuffingWODtl> LstDestuffing = new List<DestuffingWODtl>();
            DestuffingWO ObjDestuffingWO = new DestuffingWO();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new DestuffingWODtl
                    {
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Vessel = Convert.ToString(Result["Vessel"] == null ? "" : Result["Vessel"]),
                        NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                        GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                        ImporterName = Convert.ToString(Result["Importer"] == null ? "" : Result["Importer"]),
                        CHAName = Convert.ToString(Result["CHA"] == null ? "" : Result["CHA"])
                    });
                    ObjDestuffingWO.WorkOrderNo = Result["WorkOrderNo"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffingWO.GodwnWiseLctnNames = Convert.ToString(Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffingWO.ShippingLine = Convert.ToString(Result["ShippingLine"] == null ? "" : Result["ShippingLine"]);
                        ObjDestuffingWO.DestuffingNo = Convert.ToString(Result["DestuffingNo"] == null ? "" : Result["DestuffingNo"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffingWO.ContractorName = (Result["ContractorName"] == null ? "" : Result["ContractorName"]).ToString();
                    }
                }
                if (LstDestuffing.Count > 0)
                {
                    ObjDestuffingWO.StringifyXML = Newtonsoft.Json.JsonConvert.SerializeObject(LstDestuffing);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDestuffingWO;
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

        #region Destuffing Entry
        public void AddEditDestuffingEntry(DestuffingEntry ObjDestuffing, string DestuffingEntryXML /*, string GodownXML, string ClearLcoationXML */, int BranchId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDestuffing.DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeStuffingWODtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDestuffing.DeStuffingWODtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DODate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.DODate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.DestuffingEntryDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Vessel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Voyage", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Voyage });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Rotation", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Rotation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.SealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.LCLFCL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingXML", MySqlDbType = MySqlDbType.Text, Value = DestuffingEntryXML });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_GodownXML", MySqlDbType = MySqlDbType.Text, Value = GodownXML });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ClearLocation", MySqlDbType = MySqlDbType.Text, Value = ClearLcoationXML });
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
        public void AddEditDestuffingEntry_Kdl(DestuffingEntry ObjDestuffing, string DestuffingEntryXML /*, string GodownXML, string ClearLcoationXML */, int BranchId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDestuffing.DestuffingEntryId });
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_DeStuffingWODtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDestuffing.DeStuffingWODtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DODate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.DODate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjDestuffing.DestuffingEntryDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.ContainerNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Vessel });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Voyage", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Voyage });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Rotation", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Rotation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.SealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Value = ObjDestuffing.LCLFCL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjDestuffing.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingXML", MySqlDbType = MySqlDbType.Text, Value = DestuffingEntryXML });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_GodownXML", MySqlDbType = MySqlDbType.Text, Value = GodownXML });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ClearLocation", MySqlDbType = MySqlDbType.Text, Value = ClearLcoationXML });
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
            int Result = DataAccess.ExecuteNonQuery("DelImpDestuffingEntry", CommandType.StoredProcedure, DParam);
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
        public void GetDestuffingEntry(int DestuffingEntryId, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DestuffingEntry ObjDestuffing = new DestuffingEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDestuffing.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    ObjDestuffing.DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]);
                    ObjDestuffing.DODate = (Result["DODate"] == null ? "" : Result["DODate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjDestuffing.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                    ObjDestuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    ObjDestuffing.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.LstDestuffingEntry.Add(new DestuffingEntry
                        {
                            DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                            //DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            BOLNo = (Result["BOLNo"] == null ? "" : Result["BOLNo"]).ToString(),
                            BOLDate = (Result["BOLDate"] == null ? "" : Result["BOLDate"]).ToString(),
                            MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString(),
                            CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]),
                            ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]),
                            SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            DestuffingWeight = Convert.ToDecimal(Result["DestuffingWeight"] == DBNull.Value ? 0 : Result["DestuffingWeight"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            AppraisementStatus = Convert.ToInt32(Result["AppraisementStatus"] == DBNull.Value ? 0 : Result["AppraisementStatus"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            GodownId = Convert.ToInt32(Result["GodownId"] == DBNull.Value ? 0 : Result["GodownId"]),
                            LocationDetails = (Result["GodownWiseLocationIds"] == null ? "" : Result["GodownWiseLocationIds"]).ToString(),
                            GodownWiseLctnNames = (Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString()
                            //RemDestuffingWeight = Convert.ToDecimal(Result["RemDestuffingWeight"] == DBNull.Value ? 0 : Result["RemDestuffingWeight"]),
                            //RemDuty = Convert.ToDecimal(Result["RemDuty"] == DBNull.Value ? 0 : Result["RemDuty"]),
                            //RemNoOfPackages = Convert.ToInt32(Result["RemNoOfPackages"] == DBNull.Value ? 0 : Result["RemNoOfPackages"]),
                            //RemCIFValue = Convert.ToDecimal(Result["RemCIFValue"] == DBNull.Value ? 0 : Result["RemCIFValue"])
                        });
                    }
                }
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
        public void GetAllDestuffingEntry(int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DestuffingEntryList> LstDestuffing = new List<DestuffingEntryList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new DestuffingEntryList
                    {
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]),
                        CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                        BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
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
        public void GetContrNoForDestuffingEntry(int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContrNoForDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DestuffingEntry> LstDestuffing = new List<DestuffingEntry>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new DestuffingEntry
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        DestuffingDtlId = Convert.ToInt32(Result["DestuffingDtlId"])
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
        public void GetContrDetForDestuffingEntry(int DestuffingDtlId, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingDtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContrDetForDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DestuffingEntry ObjDestuffing = new DestuffingEntry();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDestuffing.DestuffingDtlId = Convert.ToInt32(Result["DestuffingDtlId"]);
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjDestuffing.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    ObjDestuffing.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjDestuffing.LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString();
                    ObjDestuffing.BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString();
                    ObjDestuffing.BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString();
                    ObjDestuffing.CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]);
                    ObjDestuffing.Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString();
                    ObjDestuffing.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjDestuffing.CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString();
                    ObjDestuffing.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                    ObjDestuffing.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]);
                    ObjDestuffing.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    //ObjDestuffing.DestuffingWeight = Convert.ToDecimal(Result["DestuffingWeight"] == DBNull.Value ? 0 : Result["DestuffingWeight"]);
                    ObjDestuffing.CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]);
                    ObjDestuffing.Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]);
                    ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.GodownId = Convert.ToInt32(Result["GodownId"] == DBNull.Value ? 0 : Result["GodownId"]);
                    ObjDestuffing.GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString();
                    ObjDestuffing.DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]);
                    ObjDestuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                    ObjDestuffing.MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString();
                    ObjDestuffing.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    ObjDestuffing.CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]);
                    ObjDestuffing.Commodity = Result["Commodity"].ToString();
                    ObjDestuffing.AppraisementStatus = Convert.ToInt32(Result["AppraisementStatus"] == DBNull.Value ? 0 : Result["AppraisementStatus"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDestuffing;
                }
                //else if (Status == 2)
                //{
                //    _DBResponse.Status = 2;
                //    _DBResponse.Message = "Success";
                //    _DBResponse.Data = ObjDestuffingDtl;
                //}
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
        public void GetContrDetForDestuffingEntry_Kdl(int DestuffingDtlId, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingDtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetContrDetForDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DestuffingEntry ObjDestuffing = new DestuffingEntry();

            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDestuffing.DestuffingDtlId = Convert.ToInt32(Result["DestuffingDtlId"]);
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjDestuffing.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    ObjDestuffing.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjDestuffing.LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString();
                    ObjDestuffing.BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString();
                    ObjDestuffing.BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString();
                    ObjDestuffing.CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]);
                    ObjDestuffing.Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString();
                    ObjDestuffing.ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]);
                    ObjDestuffing.CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString();
                    ObjDestuffing.CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString();
                    ObjDestuffing.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]);
                    ObjDestuffing.GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjDestuffing.DestuffingWeight = Convert.ToDecimal(Result["DestuffingWeight"] == DBNull.Value ? 0 : Result["DestuffingWeight"]);
                    ObjDestuffing.CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]);
                    ObjDestuffing.Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]);
                    ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.GodownId = Convert.ToInt32(Result["GodownId"] == DBNull.Value ? 0 : Result["GodownId"]);
                    ObjDestuffing.GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString();
                    ObjDestuffing.DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]);
                    ObjDestuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                    ObjDestuffing.MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString();
                    ObjDestuffing.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    ObjDestuffing.CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]);
                    ObjDestuffing.Commodity = Result["Commodity"].ToString();
                    ObjDestuffing.AppraisementStatus = Convert.ToInt32(Result["AppraisementStatus"] == DBNull.Value ? 0 : Result["AppraisementStatus"]);
                    ObjDestuffing.BOLNo = Result["BLNo"].ToString();

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjDestuffing;
                }
                //else if (Status == 2)
                //{
                //    _DBResponse.Status = 2;
                //    _DBResponse.Message = "Success";
                //    _DBResponse.Data = ObjDestuffingDtl;
                //}
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
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffEntryForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DestuffingEntry ObjDestuffing = new DestuffingEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // ObjDestuffing.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    // ObjDestuffing.DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]);
                    // ObjDestuffing.DODate = (Result["DODate"] == null ? "" : Result["DODate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    // ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjDestuffing.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                    // ObjDestuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    // ObjDestuffing.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.LstDestuffingEntry.Add(new DestuffingEntry
                        {
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            BOLNo = (Result["BOLNo"] == null ? "" : Result["BOLNo"]).ToString(),
                            BOLDate = (Result["BOLDate"] == null ? "" : Result["BOLDate"]).ToString(),
                            MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            Cargo = Convert.ToString(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            GodownWiseLctnNames = (Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]),
                            SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"])
                            //DestuffingWeight=Convert.ToDecimal(Result["DestuffingWeight"])
                        });
                    }
                }


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
        public void GetDestuffEntryForPrint_Kdl(int DestuffingEntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffEntryForPrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DestuffingEntry ObjDestuffing = new DestuffingEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    // ObjDestuffing.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    // ObjDestuffing.DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]);
                    // ObjDestuffing.DODate = (Result["DODate"] == null ? "" : Result["DODate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    // ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjDestuffing.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                    // ObjDestuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    // ObjDestuffing.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.LstDestuffingEntry.Add(new DestuffingEntry
                        {
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            BOLNo = (Result["BOLNo"] == null ? "" : Result["BOLNo"]).ToString(),
                            BOLDate = (Result["BOLDate"] == null ? "" : Result["BOLDate"]).ToString(),
                            MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            Cargo = Convert.ToString(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            GodownWiseLctnNames = (Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]),
                            SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]),
                            DestuffingWeight = Convert.ToDecimal(Result["DestuffingWeight"])
                        });
                    }
                }


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
        public void GetKdl_DestuffingEntry(int DestuffingEntryId, int BranchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DestuffingEntry ObjDestuffing = new DestuffingEntry();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDestuffing.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjDestuffing.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    // ObjDestuffing.DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]);
                    ObjDestuffing.DODate = (Result["DODate"] == null ? "" : Result["DODate"]).ToString();
                    ObjDestuffing.DestuffingEntryDate = (Result["DestuffingEntryDate"] == null ? "" : Result["DestuffingEntryDate"]).ToString();
                    ObjDestuffing.ContainerNo = Result["ContainerNo"].ToString();
                    ObjDestuffing.Size = Result["Size"].ToString();
                    ObjDestuffing.CFSCode = Result["CFSCode"].ToString();
                    ObjDestuffing.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    ObjDestuffing.Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString();
                    ObjDestuffing.Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString();
                    ObjDestuffing.Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString();
                    ObjDestuffing.SealNo = (Result["SealNo"] == null ? "" : Result["SealNo"]).ToString();
                    ObjDestuffing.CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString();
                    ObjDestuffing.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    ObjDestuffing.LCLFCL = (Result["LCLFCL"] == null ? "" : Result["LCLFCL"]).ToString();
                    ObjDestuffing.ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDestuffing.LstDestuffingEntry.Add(new DestuffingEntry
                        {
                            DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                            DeStuffingWODtlId = Convert.ToInt32(Result["DeStuffingWODtlId"]),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString(),
                            BOLNo = (Result["BOLNo"] == null ? "" : Result["BOLNo"]).ToString(),
                            BOLDate = (Result["BOLDate"] == null ? "" : Result["BOLDate"]).ToString(),
                            MarksNo = (Result["MarksNo"] == null ? "" : Result["MarksNo"]).ToString(),
                            CHAId = Convert.ToInt32(Result["CHAId"] == DBNull.Value ? 0 : Result["CHAId"]),
                            ImporterId = Convert.ToInt32(Result["ImporterId"] == DBNull.Value ? 0 : Result["ImporterId"]),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]),
                            SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            DestuffingWeight = Convert.ToDecimal(Result["DestuffingWeight"] == DBNull.Value ? 0 : Result["DestuffingWeight"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            AppraisementStatus = Convert.ToInt32(Result["AppraisementStatus"] == DBNull.Value ? 0 : Result["AppraisementStatus"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"]),
                            GodownId = Convert.ToInt32(Result["GodownId"] == DBNull.Value ? 0 : Result["GodownId"]),
                            LocationDetails = (Result["GodownWiseLocationIds"] == null ? "" : Result["GodownWiseLocationIds"]).ToString(),
                            GodownWiseLctnNames = (Result["GodownWiseLctnNames"] == null ? "" : Result["GodownWiseLctnNames"]).ToString(),
                            Importer = (Result["Importer"] == null ? "" : Result["Importer"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            GodownName = (Result["GodownName"] == null ? "" : Result["GodownName"]).ToString(),
                            CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                            RemDestuffingWeight = Convert.ToDecimal(Result["RemDestuffingWeight"] == DBNull.Value ? 0 : Result["RemDestuffingWeight"]),
                            RemDuty = Convert.ToDecimal(Result["RemDuty"] == DBNull.Value ? 0 : Result["RemDuty"]),
                            RemNoOfPackages = Convert.ToInt32(Result["RemNoOfPackages"] == DBNull.Value ? 0 : Result["RemNoOfPackages"]),
                            RemCIFValue = Convert.ToDecimal(Result["RemCIFValue"] == DBNull.Value ? 0 : Result["RemCIFValue"])
                        });
                    }
                }
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
        public void GetSearchDestuffingEntry(int BranchId, string ContainerName)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerName", MySqlDbType = MySqlDbType.VarChar, Value = ContainerName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetSearchImpDestuffingEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DestuffingEntryList> LstDestuffing = new List<DestuffingEntryList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDestuffing.Add(new DestuffingEntryList
                    {
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DestuffingEntryDate = Result["DestuffingEntryDate"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]),
                        CHA = (Result["CHA"] == null ? "" : Result["CHA"]).ToString(),
                        BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                        ShippingLine = (Result["ShippingLine"] == null ? "" : Result["ShippingLine"]).ToString(),
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
        #endregion

        #region Import Yard Payment Sheet
        public void GetAppraismentRequestForPaymentSheet(int AppraisementAppId = 0,string Type="I")
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
        public void GetPaymentParty()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentParty", CommandType.StoredProcedure);
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


        public void GetPaymentPartyForImportInvoice()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyForImportInvoice", CommandType.StoredProcedure);
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

        public void GetPaymentPartyForExportnvoice()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyForExportInvoice", CommandType.StoredProcedure);
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


        public bool HasColumn(IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        public void GetCFSCodeForEdit(int AppraisementAppId, string Type)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = AppraisementAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_type", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCFSCodeForEdit", CommandType.StoredProcedure, DParam);
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
                        ContainerNo = HasColumn(Result, "ContainerNo") ? Convert.ToString(Result["ContainerNo"]) : "",
                        Selected = false,
                        ArrivalDt = HasColumn(Result, "ArrivalDt") ? Convert.ToString(Result["ArrivalDt"]) : "",
                        IsHaz = HasColumn(Result, "IsHaz") ? Convert.ToString(Result["IsHaz"]) : "",
                        Size = HasColumn(Result, "Size") ? Convert.ToString(Result["Size"]) : "",
                        LCLFCL = HasColumn(Result, "LCLFCL") ? Convert.ToString(Result["LCLFCL"]) : "",
                        LineNo = HasColumn(Result, "LineNo") ? Convert.ToString(Result["LineNo"]) : "",
                        BOENo = HasColumn(Result, "BOENo") ? Convert.ToString(Result["BOENo"]) : "",
                        BOEDate = HasColumn(Result, "BOEDate") ? Convert.ToString(Result["BOEDate"]) : ""

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
        public void GetContainerForPaymentSheet(int AppraisementAppId,string Type="I")
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
                        ArrivalDt = Convert.ToString(Result["ArrivalDt"]),
                        IsHaz = Convert.ToString(Result["IsHaz"]),
                        Size = Convert.ToString(Result["Size"]),
                        OBLNo= Convert.ToString(Result["OBLNo"]),
                        SealCutDate = Convert.ToString(Result["SealCutDate"]),
                        NoOfPkg = Convert.ToInt32(Result["NoOfPackages"]),
                        GrWait = Convert.ToDecimal(Result["GrossWeight"]),
                    //    IsBond = Convert.ToBoolean(Result["IsBond"]),

                        
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

        // Not required
        public void GetContainerPaymentSheet(string InvoiceDate, int StuffingAppId, string ContainerXML)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = StuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetYardPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PaySheetChargeDetails objPaymentSheet = new PaySheetChargeDetails();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheet.lstPSContainer.Add(new PSContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Size = Convert.ToString(Result["Size"]),
                        IsReefer = Convert.ToBoolean(Result["Reefer"]),
                        Insured = Convert.ToString(Result["Insured"])
                    });
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        objPaymentSheet.lstEmptyGR.Add(new PSEmptyGroudRent()
                //        {
                //            ContainerType = Convert.ToString(Result["ContainerType"]),
                //            CommodityType = Convert.ToString(Result["CommodityType"]),
                //            IsReefer = Convert.ToBoolean(Result["Reefer"]),
                //            Size = Convert.ToString(Result["Size"]),
                //            DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"]),
                //            DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"]),
                //            RentAmount = Convert.ToDecimal(Result["RentAmount"]),
                //            ElectricityCharge = Convert.ToDecimal(Result["ElectricityCharge"]),
                //            GroundRentPeriod = Convert.ToInt32(Result["GRPeriod"]),
                //            CFSCode = Convert.ToString(Result["CFSCode"]),
                //            FOBValue = Convert.ToDecimal(Result["Fob"]),
                //            IsInsured = Convert.ToInt32(Result["Insured"])
                //        });
                //    }
                //}
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPaymentSheet.lstLoadedGR.Add(new PSLoadedGroudRent()
                        {
                            ContainerType = Convert.ToString(Result["ContainerType"]),
                            CommodityType = Convert.ToString(Result["CommodityType"]),
                            IsReefer = Convert.ToBoolean(Result["Reefer"]),
                            Size = Convert.ToString(Result["Size"]),
                            DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"]),
                            DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"]),
                            RentAmount = Convert.ToDecimal(Result["RentAmount"]),
                            ElectricityCharge = Convert.ToDecimal(Result["ElectricityCharge"]),
                            GroundRentPeriod = Convert.ToInt32(Result["GRPeriod"]),
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            FOBValue = Convert.ToDecimal(Result["Fob"]),
                            IsInsured = Convert.ToInt32(Result["Insured"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPaymentSheet.InsuranceRate = Convert.ToDecimal(Result["InsuranceCharge"]) / 100 / 1000;
                    }
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        objPaymentSheet.lstStorageRent.Add(new StorageRent()
                //        {
                //            CFSCode = Convert.ToString(Result["CFSCode"]),
                //            ActualCUM = Convert.ToDecimal(Result["CUM"]),
                //            ActualSQM = Convert.ToDecimal(Result["SQM"]),
                //            ActualWeight = Convert.ToDecimal(Result["GrossWeight"]),
                //            StuffCUM = Convert.ToDecimal(Result["DeStuffCUM"]),
                //            StuffSQM = Convert.ToDecimal(Result["DeStuffSQM"]),
                //            StuffWeight = Convert.ToDecimal(Result["DestuffingWeight"]),
                //            StorageDays = Convert.ToInt32(Result["StorageDays"]),
                //            StorageWeeks = Convert.ToInt32(Result["StorageWeeks"]),
                //            StorageMonths = Convert.ToInt32(Result["StorageMonths"]),
                //            StorageMonthWeeks = Convert.ToInt32(Result["StorageMonthWeeks"])
                //        });
                //    }
                //}
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPaymentSheet.RateSQMPerWeek = Convert.ToDecimal(Result["RateSqMPerWeek"]);
                        objPaymentSheet.RateSQMPerMonth = Convert.ToDecimal(Result["RateSqMeterPerMonth"]);
                        objPaymentSheet.RateCUMPerWeek = Convert.ToDecimal(Result["RateCubMeterPerDay"]);
                        objPaymentSheet.RateMTPerDay = Convert.ToDecimal(Result["RateMetricTonPerDay"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPaymentSheet.lstInsuranceCharges.Add(new InsuranceCharge()
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            StorageWeeks = Convert.ToInt32(Result["StorageWeeks"]),
                            IsInsured = Convert.ToInt16(Result["Insured"]),
                            FOB = Convert.ToDecimal(Result["Fob"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPaymentSheet.lstPSHTCharge.Add(new PSHTCharges()
                        {
                            ChargeId = Convert.ToInt32(Result["HTChargesID"]),
                            ChargeName = Convert.ToString(Result["Description"]),
                            Charge = Convert.ToDecimal(Result["RateCWC"])
                        });
                    }
                }
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
        public void AddEditContainerInvoice(PaymentSheetFinalModel ObjPSFinalModel, string ContainerXML, string ChargesXML, int BranchId, int Uid)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPSFinalModel.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.StuffingReqNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPSFinalModel.StuffingDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.GSTNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.Invoice });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = "IMP" });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditExpInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
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
        //
        #endregion

        #region Import Godown Payment Sheet
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

        // Not required
        public void GetCargoPaymentSheet(string InvoiceDate, int StuffingAppId, string ContainerXML)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = StuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PaySheetChargeDetails objPaymentSheet = new PaySheetChargeDetails();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheet.lstPSContainer.Add(new PSContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Size = Convert.ToString(Result["Size"]),
                        IsReefer = Convert.ToBoolean(Result["Reefer"]),
                        Insured = Convert.ToString(Result["Insured"])
                    });
                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        objPaymentSheet.lstEmptyGR.Add(new PSEmptyGroudRent()
                //        {
                //            ContainerType = Convert.ToString(Result["ContainerType"]),
                //            CommodityType = Convert.ToString(Result["CommodityType"]),
                //            IsReefer = Convert.ToBoolean(Result["Reefer"]),
                //            Size = Convert.ToString(Result["Size"]),
                //            DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"]),
                //            DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"]),
                //            RentAmount = Convert.ToDecimal(Result["RentAmount"]),
                //            ElectricityCharge = Convert.ToDecimal(Result["ElectricityCharge"]),
                //            GroundRentPeriod = Convert.ToInt32(Result["GRPeriod"]),
                //            CFSCode = Convert.ToString(Result["CFSCode"]),
                //            FOBValue = Convert.ToDecimal(Result["Fob"]),
                //            IsInsured = Convert.ToInt32(Result["Insured"])
                //        });
                //    }
                //}
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        objPaymentSheet.lstLoadedGR.Add(new PSLoadedGroudRent()
                //        {
                //            ContainerType = Convert.ToString(Result["ContainerType"]),
                //            CommodityType = Convert.ToString(Result["CommodityType"]),
                //            IsReefer = Convert.ToBoolean(Result["Reefer"]),
                //            Size = Convert.ToString(Result["Size"]),
                //            DaysRangeFrom = Convert.ToInt32(Result["DaysRangeFrom"]),
                //            DaysRangeTo = Convert.ToInt32(Result["DaysRangeTo"]),
                //            RentAmount = Convert.ToDecimal(Result["RentAmount"]),
                //            ElectricityCharge = Convert.ToDecimal(Result["ElectricityCharge"]),
                //            GroundRentPeriod = Convert.ToInt32(Result["GRPeriod"]),
                //            CFSCode = Convert.ToString(Result["CFSCode"]),
                //            FOBValue = Convert.ToDecimal(Result["Fob"]),
                //            IsInsured = Convert.ToInt32(Result["Insured"])
                //        });
                //    }
                //}
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPaymentSheet.InsuranceRate = Convert.ToDecimal(Result["InsuranceCharge"]) / 100 / 1000;
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPaymentSheet.lstStorageRent.Add(new Areas.Import.Models.StorageRent()
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            ActualCUM = Convert.ToDecimal(Result["CUM"]),
                            ActualSQM = Convert.ToDecimal(Result["SQM"]),
                            ActualWeight = Convert.ToDecimal(Result["GrossWeight"]),
                            StuffCUM = Convert.ToDecimal(Result["DeStuffCUM"]),
                            StuffSQM = Convert.ToDecimal(Result["DeStuffSQM"]),
                            StuffWeight = Convert.ToDecimal(Result["DestuffingWeight"]),
                            StorageDays = Convert.ToInt32(Result["StorageDays"]),
                            StorageWeeks = Convert.ToInt32(Result["StorageWeeks"]),
                            StorageMonths = Convert.ToInt32(Result["StorageMonths"]),
                            StorageMonthWeeks = Convert.ToInt32(Result["StorageMonthWeeks"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPaymentSheet.RateSQMPerWeek = Convert.ToDecimal(Result["RateSqMPerWeek"]);
                        objPaymentSheet.RateSQMPerMonth = Convert.ToDecimal(Result["RateSqMeterPerMonth"]);
                        objPaymentSheet.RateCUMPerWeek = Convert.ToDecimal(Result["RateCubMeterPerDay"]);
                        objPaymentSheet.RateMTPerDay = Convert.ToDecimal(Result["RateMetricTonPerDay"]);
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPaymentSheet.lstInsuranceCharges.Add(new InsuranceCharge()
                        {
                            CFSCode = Convert.ToString(Result["CFSCode"]),
                            StorageWeeks = Convert.ToInt32(Result["StorageWeeks"]),
                            IsInsured = Convert.ToInt16(Result["Insured"]),
                            FOB = Convert.ToDecimal(Result["Fob"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objPaymentSheet.lstPSHTCharge.Add(new PSHTCharges()
                        {
                            ChargeId = Convert.ToInt32(Result["HTChargesID"]),
                            ChargeName = Convert.ToString(Result["Description"]),
                            Charge = Convert.ToDecimal(Result["RateCWC"])
                        });
                    }
                }
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
        public void AddEditCargoInvoice(PaymentSheetFinalModel ObjPSFinalModel, string ContainerXML, string ChargesXML, int BranchId, int Uid)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPSFinalModel.InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.StuffingReqId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.StuffingReqNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingReqDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPSFinalModel.StuffingDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPSFinalModel.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.GSTNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CWCTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.CWCTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HTTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.HTTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AllTotal", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.AllTotal });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RoundUp", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.RoundUp });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPSFinalModel.Invoice });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPSFinalModel.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = "IMPCA" });

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.Text, Value = ChargesXML });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditExpInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
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
        //
        #endregion

        #region Import Delivery Payment Sheet

        #region For Kandla
        public void GetDeliveryApplicationForImpPaymentSheet(int DestuffingAppId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDeliveryApplicationForImpPaymentSheet", CommandType.StoredProcedure, DParam);
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
                        StuffingReqId = Convert.ToInt32(Result["DeliveryId"]),
                        StuffingReqNo = Convert.ToString(Result["DeliveryNo"]),
                        StuffingReqDate = Convert.ToString(Result["DeliveryAppDate"]),
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
        public void GetBOEForDeliveryPaymentSheet(int DestuffingAppId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingAppId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDeliveryApplicationForImpPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaymentSheetBOE> objPaymentSheetBOE = new List<PaymentSheetBOE>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetBOE.Add(new PaymentSheetBOE()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        LineNo = Convert.ToString(Result["LineNo"]),
                        BOENo = Convert.ToString(Result["BOENo"]),
                        BOEDate = Convert.ToString(Result["BOEDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheetBOE;
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

        public void GetBOEForPaymentSheet(string ContainerXML)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBOENoForImpDeliveryPaymentSheet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaymentSheetBOE> objPaymentSheetBOE = new List<PaymentSheetBOE>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetBOE.Add(new PaymentSheetBOE()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        LineNo = Convert.ToString(Result["LineNo"]),
                        BOENo = Convert.ToString(Result["BOENo"]),
                        BOEDate = Convert.ToString(Result["BOEDate"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objPaymentSheetBOE;
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

        #region Empty Container Payment Sheet
        public void GetApplicationForEmptyContainer(string ApplicationFor, int ApplicationId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ApplicationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ApplicationFor });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetApplicationForEmptyContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaySheetStuffingRequest> objPaySheetStuffing = new List<PaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    if (ApplicationFor == "YARD")
                    {
                        objPaySheetStuffing.Add(new PaySheetStuffingRequest()
                        {
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CHAName = Convert.ToString(Result["CHAName"]),
                            CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                            StuffingReqId = Convert.ToInt32(Result["CustomAppraisementId"]),
                            StuffingReqNo = Convert.ToString(Result["AppraisementNo"]),
                            StuffingReqDate = Convert.ToString(Result["AppraisementDate"]),
                            Address = Convert.ToString(Result["Address"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            State = Convert.ToString(Result["StateName"]),
                            StateCode = Convert.ToString(Result["StateCode"])
                        });
                    }
                    else
                    {
                        objPaySheetStuffing.Add(new PaySheetStuffingRequest()
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
        public void GetEmptyContForPaymentSheet(string ApplicationFor, int ApplicationId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ApplicationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ApplicationFor });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetApplicationForEmptyContainer", CommandType.StoredProcedure, DParam);
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
        #endregion

        #region Issue Slip
        public void AddEditIssueSlip(IssueSlip ObjIssueSlip)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjIssueSlip.IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjIssueSlip.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjIssueSlip.IssueSlipDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = ObjIssueSlip.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjIssueSlip.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditImpIssueSlip", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjIssueSlip.IssueSlipId == 0 ? "Issue Slip Details Saved Successfully" : "Issue Slip Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
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
        public void DelIssueSlip(int IssueSlipId)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DelImpIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Issue Slip Details Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Delete Issue Slip Details As It Exists In Another Page";
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
        public void GetIssueSlip(int IssueSlipId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IssueSlip ObjIssueSlip = new IssueSlip();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjIssueSlip.IssueSlipId = Convert.ToInt32(Result["IssueSlipId"]);
                    ObjIssueSlip.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    ObjIssueSlip.IssueSlipNo = Result["IssueSlipNo"].ToString();
                    ObjIssueSlip.IssueSlipDate = Result["IssueSlipDate"].ToString();
                    ObjIssueSlip.CargoDescription = Result["CargoDescription"].ToString();
                    ObjIssueSlip.InvoiceNo = Result["InvoiceNo"].ToString();
                    ObjIssueSlip.InvoiceDate = Result["InvoiceDate"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstContainer.Add(new IssueSlipContainer
                        {
                            CFSCode = Result["CFSCode"].ToString(),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            Size = (Result["Size"] == null ? "" : Result["Size"]).ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"])
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstCargo.Add(new IssueSlipCargo
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
        public void GetAllIssueSlip()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<IssueSlip> LstIssueSlip = new List<IssueSlip>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstIssueSlip.Add(new IssueSlip
                    {
                        IssueSlipNo = Result["IssueSlipNo"].ToString(),
                        IssueSlipDate = Result["IssueSlipDate"].ToString(),
                        IssueSlipId = Convert.ToInt32(Result["IssueSlipId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstIssueSlip;
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
        public void GetInvoiceNoForIssueSlip()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceNoForIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<IssueSlip> LstIssueSlip = new List<IssueSlip>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstIssueSlip.Add(new IssueSlip
                    {
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstIssueSlip;
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
        public void GetInvoiceDetForIssueSlip(int InvoiceId)
        {

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceDetForIssueSlip", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IssueSlip ObjIssueSlip = new IssueSlip();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjIssueSlip.InvoiceDate = Result["InvoiceDate"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstContainer.Add(new IssueSlipContainer
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            CFSCode = Result["CFSCode"].ToString(),
                            Size = Result["Size"].ToString(),
                            GrossWeight = Convert.ToDecimal(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]),
                            CIFValue = Convert.ToDecimal(Result["CIFValue"] == DBNull.Value ? 0 : Result["CIFValue"])
                        });
                    }

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjIssueSlip.LstCargo.Add(new IssueSlipCargo
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

        public void GetIssueSlipForPreview(int IssueSlipId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueSlipId", MySqlDbType = MySqlDbType.Int32, Value = IssueSlipId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DA.ExecuteDataReader("GetIssueSlipForPreview", CommandType.StoredProcedure, DParam);
            IssueSlip ObjIssueSlip = new IssueSlip();
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
                        ObjIssueSlip.LstIssueSlipRpt.Add(new IssueSlipReport
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

        #region Internal Movement
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
            List<InternalMovement> LstInternalMovement = new List<InternalMovement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new InternalMovement
                    {
                        MovementNo = Result["MovementNo"].ToString(),
                        BOENo = Result["BOENo"].ToString(),
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
            InternalMovement ObjInternalMovement = new InternalMovement();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjInternalMovement.MovementId = Convert.ToInt32(Result["MovementId"]);
                    ObjInternalMovement.MovementNo = Result["MovementNo"].ToString();
                    ObjInternalMovement.MovementDate = Result["MovementDate"].ToString();
                    ObjInternalMovement.CargoDescription = Result["CargoDescription"].ToString();
                    ObjInternalMovement.BOENo = Result["BOENo"].ToString();
                    ObjInternalMovement.BOEDate = Result["BOEDate"].ToString();
                    ObjInternalMovement.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjInternalMovement.GrossWeight = Convert.ToInt32(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjInternalMovement.OldLocationIds = Result["OldLocationIds"].ToString();
                    ObjInternalMovement.OldLctnNames = Result["OldLctnNames"].ToString();
                    ObjInternalMovement.NewLocationIds = Result["NewLocationIds"].ToString();
                    ObjInternalMovement.NewLctnNames = Result["NewLctnNames"].ToString();
                    ObjInternalMovement.FromGodownId = Convert.ToInt32(Result["FromGodownId"]);
                    ObjInternalMovement.ToGodownId = Convert.ToInt32(Result["ToGodownId"]);
                    ObjInternalMovement.NewGodownName = Result["NewGodownName"].ToString();
                    ObjInternalMovement.OldGodownName = Result["OldGodownName"].ToString();
                    ObjInternalMovement.DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]);
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
        public void AddEditImpInternalMovement(InternalMovement ObjInternalMovement)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.DestuffingEntryDtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.MovementDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.BOEDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.NoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.GrossWeight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.FromGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.ToGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLocationIds", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.OldLocationIds });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.OldLctnNames });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLocationIds", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.NewLocationIds });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.NewLctnNames });
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
        public void GetBOENoForInternalMovement()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBOENoForInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<InternalMovement> LstInternalMovement = new List<InternalMovement>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new InternalMovement
                    {
                        BOENo = Result["BOENo"].ToString(),
                        DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"])
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
        public void GetBOENoDetForMovement(int DestuffingEntryDtlId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DestuffingEntryDtlId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetBOENoForInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            InternalMovement ObjInternalMovement = new InternalMovement();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjInternalMovement.BOENo = Result["BOENo"].ToString();
                    ObjInternalMovement.BOEDate = Result["BOEDate"].ToString();
                    ObjInternalMovement.CargoDescription = Result["CargoDescription"].ToString();
                    ObjInternalMovement.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjInternalMovement.GrossWeight = Convert.ToInt32(Result["GrossWeight"] == DBNull.Value ? 0 : Result["GrossWeight"]);
                    ObjInternalMovement.OldLocationIds = Result["OldLocationIds"].ToString();
                    ObjInternalMovement.OldLctnNames = Result["OldLctnNames"].ToString();
                    ObjInternalMovement.FromGodownId = Convert.ToInt32(Result["FromGodownId"]);
                    ObjInternalMovement.OldGodownName = Result["OldGodownName"].ToString();
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
        #endregion

        #region Delivery Application
        public void GetAllDeliveryApplication()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDeliveryApplication", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DeliveryApplicationList> LstDeliveryApp = new List<DeliveryApplicationList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstDeliveryApp.Add(new DeliveryApplicationList
                    {
                        DestuffingEntryNo = Result["DestuffingEntryNo"].ToString(),
                        DeliveryNo = Result["DeliveryNo"].ToString(),
                        DeliveryAppDate = Result["DeliveryAppDate"].ToString(),
                        DeliveryId = Convert.ToInt32(Result["DeliveryId"])
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
        public void GetDeliveryApplication(int DeliveryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = DeliveryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDeliveryApplication", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DeliveryApplication ObjDeliveryApp = new DeliveryApplication();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjDeliveryApp.DeliveryId = Convert.ToInt32(Result["DeliveryId"]);
                    ObjDeliveryApp.DeliveryNo = Result["DeliveryNo"].ToString();
                    ObjDeliveryApp.DestuffingEntryNo = Result["DestuffingEntryNo"].ToString();
                    ObjDeliveryApp.DestuffingId = Convert.ToInt32(Result["DestuffingId"]);

                    ObjDeliveryApp.CHAId = Convert.ToInt32(Result["CHAId"]);
                    ObjDeliveryApp.CHA = Result["CHA"].ToString();

                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        ObjDeliveryApp.LstDeliveryAppDtl.Add(new DeliveryApplicationDtl
                        {
                            DeliveryDtlId = Convert.ToInt32(Result["DeliveryDtlId"]),
                            DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString(),
                            BEODate = (Result["BEODate"] == null ? "" : Result["BEODate"]).ToString(),
                            BOELineNo = (Result["BOELineNo"] == null ? "" : Result["BOELineNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            Commodity = (Result["Commodity"] == null ? "" : Result["Commodity"]).ToString(),
                            CommodityId = Convert.ToInt32(Result["CommodityId"] == DBNull.Value ? 0 : Result["CommodityId"]),
                            CUM = Convert.ToDecimal(Result["CUM"] == DBNull.Value ? 0 : Result["CUM"]),
                            SQM = Convert.ToDecimal(Result["SQM"] == DBNull.Value ? 0 : Result["SQM"]),
                            GrossWt = Convert.ToDecimal(Result["GrossWt"] == DBNull.Value ? 0 : Result["GrossWt"]),
                            CIF = Convert.ToDecimal(Result["CIF"] == DBNull.Value ? 0 : Result["CIF"]),
                            Duty = Convert.ToDecimal(Result["Duty"] == DBNull.Value ? 0 : Result["Duty"]),
                            NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]),
                            DelCUM = Convert.ToDecimal(Result["DelCUM"] == DBNull.Value ? 0 : Result["DelCUM"]),
                            DelSQM = Convert.ToDecimal(Result["DelSQM"] == DBNull.Value ? 0 : Result["DelSQM"]),
                            DelGrossWt = Convert.ToDecimal(Result["DelGrossWt"] == DBNull.Value ? 0 : Result["DelGrossWt"]),
                            DelCIF = Convert.ToDecimal(Result["DelCIF"] == DBNull.Value ? 0 : Result["DelCIF"]),
                            DelDuty = Convert.ToDecimal(Result["DelDuty"] == DBNull.Value ? 0 : Result["DelDuty"]),
                            DelNoOfPackages = Convert.ToInt32(Result["DelNoOfPackages"] == DBNull.Value ? 0 : Result["DelNoOfPackages"]),
                            ImporterId = Convert.ToInt32(Result["ImporterId"]),
                            Importer = Result["Importer"].ToString()
                        });
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
        public void AddEditDeliveryApplication(DeliveryApplication ObjDeliveryApp, string DeliveryXml)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliveryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryXML", MySqlDbType = MySqlDbType.Text, Value = DeliveryXml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.CHAId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.ImporterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditDeliveryApplication", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Delivery Application Details Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
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
                    _DBResponse.Message = "Cannot Edit Delivery Application Details As It Already Exist In Another Page";
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
            List<PPGBOELineNoList> LstBOELineNo = new List<PPGBOELineNoList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstBOELineNo.Add(new PPGBOELineNoList
                    {
                        BOELineNo = Result["BOELineNo"].ToString(),
                        DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]),
                        CHA = Result["CHA"].ToString(),
                        CHAId = Result["CHAId"].ToString()
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
            DeliveryApplicationDtl ObjDeliveryApp = new DeliveryApplicationDtl();
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
                    ObjDeliveryApp.CommodityId = Convert.ToInt32(Result["CommodityId"]);
                    ObjDeliveryApp.Commodity = Result["Commodity"].ToString();
                    ObjDeliveryApp.Importer = Result["Importer"].ToString();
                    ObjDeliveryApp.ImporterId = Convert.ToInt32(Result["ImporterId"]);
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
        public void GetDestuffEntryNo()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
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
                        DestuffingId = Convert.ToInt32(Result["DestuffingId"])
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

        #endregion



        #region Empty Container Paymentsheet Gate Out
        public void GetApplicationForEmptyContainerGateOut(int ApplicationId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ApplicationId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ApplicationFor });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetApplicationForEmptyContainerGateOut", CommandType.StoredProcedure, DParam);
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
                        StuffingReqId = Convert.ToInt32(Result["CustomAppraisementId"]),
                        StuffingReqNo = Convert.ToString(Result["AppraisementNo"]),
                        StuffingReqDate = Convert.ToString(Result["AppraisementDate"]),
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

        public void GetEmptyContForPaymentSheetGateOut(int ApplicationId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ApplicationId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ApplicationFor });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetApplicationForEmptyContainerGateOut", CommandType.StoredProcedure, DParam);
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

        #endregion

        #region removal
        public void ApprovalHoldCustomAppraisementForRemoval(int Skip)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_Skip", MySqlDbType = MySqlDbType.Int32, Value = Skip });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfAppHoldCustAppRemoval", CommandType.StoredProcedure, dpram);
            IList<ListOfCustAppraismentAppr> lstApproval = new List<ListOfCustAppraismentAppr>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            bool State = false;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstApproval.Add(new ListOfCustAppraismentAppr
                    {
                        CstmAppraiseWorkOrderId = Convert.ToInt32(result["CstmAppraiseWorkOrderId"]),
                        CstmAppraiseWorkOrderNo = result["CstmAppraiseWorkOrderNo"].ToString(),
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


        public void RemoveCustomApproval(int CstmAppraiseWorkOrderId, int IsApproved, int Uid)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CstmAppraiseWorkOrderId", MySqlDbType = MySqlDbType.Int32, Value = CstmAppraiseWorkOrderId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Approved", MySqlDbType = MySqlDbType.Int32, Value = IsApproved });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            // LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] dpram = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("RemoveCustomApp", CommandType.StoredProcedure, dpram);
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

        #region  Empty Container Paymentsheet Gate In

        public void GetAppForEmptyContGateIn(int ApplicationId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ApplicationId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ApplicationFor });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppForEmptyContGateIn", CommandType.StoredProcedure, DParam);
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
                        StuffingReqId = Convert.ToInt32(Result["CustomAppraisementId"]),
                        StuffingReqNo = Convert.ToString(Result["AppraisementNo"]),
                        StuffingReqDate = Convert.ToString(Result["AppraisementDate"]),
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

        public void GetEmptyContForPaySheetGateIn(int ApplicationId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ApplicationId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ApplicationFor });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppForEmptyContGateIn", CommandType.StoredProcedure, DParam);
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

        #endregion




        #region Import Tentative Yard Payment Sheet
        public void GetAppraismentRequestForTentativePaymentSheet(int AppraisementAppId = 0, string Type = "I")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = AppraisementAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppraismentRequestForTentativePaymentSheet", CommandType.StoredProcedure, DParam);
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



        public void GetContainerForTentativePaymentSheet(int AppraisementAppId, string Type = "I")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AppraisementAppId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = AppraisementAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = Type });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAppraismentRequestForTentativePaymentSheet", CommandType.StoredProcedure, DParam);
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
                        ArrivalDt = Convert.ToString(Result["ArrivalDt"]),
                        IsHaz = Convert.ToString(Result["IsHaz"]),
                        Size = Convert.ToString(Result["Size"])
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

        #endregion


        #region Empty Container Out Payment Sheet
        public void GetApplicationForEmptyOutContainer(string ApplicationFor, int ApplicationId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ApplicationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ApplicationFor });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetApplicationForEmptyContainer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaySheetStuffingRequest> objPaySheetStuffing = new List<PaySheetStuffingRequest>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    if (ApplicationFor == "YARD")
                    {
                        objPaySheetStuffing.Add(new PaySheetStuffingRequest()
                        {
                            CHAId = Convert.ToInt32(Result["CHAId"]),
                            CHAName = Convert.ToString(Result["CHAName"]),
                            CHAGSTNo = Convert.ToString(Result["GSTNo"]),
                            StuffingReqId = Convert.ToInt32(Result["CustomAppraisementId"]),
                            StuffingReqNo = Convert.ToString(Result["AppraisementNo"]),
                            StuffingReqDate = Convert.ToString(Result["AppraisementDate"]),
                            Address = Convert.ToString(Result["Address"]),
                            DeliveryType = Convert.ToInt32(Result["DeliveryType"]),
                            State = Convert.ToString(Result["StateName"]),
                            StateCode = Convert.ToString(Result["StateCode"])
                        });
                    }
                    else
                    {
                        objPaySheetStuffing.Add(new PaySheetStuffingRequest()
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
        public void GetEmptyContOutForDate(int ShippingLineId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_shippingId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ShippingLineId });
           // LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = ApplicationFor });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDateforEmptyContainerOut", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaymentSheetContainer> objPaymentSheetContainer = new List<PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objPaymentSheetContainer.Add(new PaymentSheetContainer()
                    {
                       
                        ArrivalDt = Convert.ToString(Result["MovementDate"]),
                       
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

        public void GetPaymentPartyforEmptyOutContainer()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyforEmptyOutContainer", CommandType.StoredProcedure);
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

        public void GetContainerForEmptyOut(int ShippingLIneId,String In_Date)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_shippingId", MySqlDbType = MySqlDbType.Int32, Value = ShippingLIneId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(In_Date) });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();


            IDataReader Result = DataAccess.ExecuteDataReader("GetContainerforEmptyContainerOut", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PaymentSheetContainer> obEmptyInvoice = new List<PaymentSheetContainer>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    obEmptyInvoice.Add(new PaymentSheetContainer()
                    {
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        Size = Convert.ToString(Result["Size"]),

                        CargoType = Convert.ToInt32(Result["cargotype"]),
                       
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = obEmptyInvoice;
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


        #region IRN Details
        public void GetIRNForYard(String InvoiceNo)
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
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
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
                        seller.TrdNm = Result["TrdNm"].ToString();
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
                        buyer.TrdNm = Result["TrdNm"].ToString();
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
                        ship.TrdNm = Result["TrdNm"].ToString();
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
                            PrdDesc = Result["PrdDesc"].ToString(),
                            IsServc = Result["IsServc"].ToString(),
                            HsnCd = Result["HsnCd"].ToString(),
                            Barcde = Result["Barcde"].ToString(),
                            Qty = Convert.ToDecimal(Result["Qty"].ToString()),
                            FreeQty = Convert.ToInt32(Result["FreeQty"].ToString()),
                            Unit = Result["Unit"].ToString(),
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
        public void GetIRNB2CForYard(String InvoiceNo)
        {
            KOL_IrnB2CDetails Obj = new KOL_IrnB2CDetails();

            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            HeaderParam hp = new HeaderParam();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetIrnB2CDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Obj.DocNo = Convert.ToString(Result["DocNo"]);
                    Obj.DocDt = Convert.ToString(Result["DocDt"]);
                    Obj.DocTyp = Convert.ToString(Result["DocTyp"]);
                    Obj.BuyerGstin = Convert.ToString(Result["BuyerGstin"]);
                    Obj.SellerGstin = Convert.ToString(Result["SellerGST"]);

                    Obj.MainHsnCode = Convert.ToString(Result["MainHsnCode"]);
                    Obj.TotInvVal = Convert.ToInt32(Result["TotInvVal"]);
                    Obj.ItemCnt = Convert.ToInt32(Result["ItemCnt"]);
                    Obj.iss = null;


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
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AckNo", MySqlDbType = MySqlDbType.String, Value = objOBL.AckNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_AckDt", MySqlDbType = MySqlDbType.String, Value = objOBL.AckDt });
            lstParam.Add(new MySqlParameter { ParameterName = "in_irn", MySqlDbType = MySqlDbType.String, Value = objOBL.irn }); // Convert.ToDateTime(objOBL.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SignedInvoice", MySqlDbType = MySqlDbType.String, Value = objOBL.SignedInvoice });
            lstParam.Add(new MySqlParameter { ParameterName = "in_SignedQRCode", MySqlDbType = MySqlDbType.VarChar, Value = objOBL.SignedQRCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_QRCodeImageBase64", MySqlDbType = MySqlDbType.LongText, Value = objOBL.QRCodeImageBase64 });// Value = Convert.ToDateTime(objOBL.IGM_Date).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_IrnStatus", MySqlDbType = MySqlDbType.String, Value = objOBL.IrnStatus });
            lstParam.Add(new MySqlParameter { ParameterName = "in_EwbNo", MySqlDbType = MySqlDbType.DateTime, Value = objOBL.EwbNo });
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
                    _DBResponse.Message = (Result == 1) ? "IRN Generate Successfully" : "IRN Generate Successfully";
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
        #endregion

    }
}