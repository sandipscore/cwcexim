using CwcExim.Areas.Import.Models;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CwcExim.Models;
using System.Globalization;
using EinvoiceLibrary;

namespace CwcExim.Repositories
{

    
    public class Kdl_ImportRepository
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
        public void GetAllImpEmtCntJO()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("LstOfImpEmtcntJbOrd", CommandType.StoredProcedure, dpram);
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
                        JobOrderDate = result["JobOrderDate"].ToString()
                        
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
        public void GetImpEmptcntJODetails(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("LstOfImpEmtcntJbOrd", CommandType.StoredProcedure, dpram);
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
                    objImpJO.JobOrderFor = Convert.ToInt32(result["JobOrderFor"]);
                    objImpJO.JobOrderNo = result["JobOrderNo"].ToString();

                    objImpJO.JobOrderDate = result["JobOrderDate"].ToString();
                    objImpJO.ShippingLineId = Convert.ToInt32(result["ShippingLineId"]);
                    objImpJO.CHAId = Convert.ToInt32(result["CHAId"] == DBNull.Value ? 0 : result["CHAId"]);
                    objImpJO.Remarks = result["Remarks"] == null ? "" : result["Remarks"].ToString();
                    objImpJO.ShippingLineName = result["ShippingLineName"].ToString();
                    objImpJO.CHAName = (result["CHAName"] == null ? "" : result["CHAName"]).ToString();
                    //objImpJO.BlNo = result["BLNo"].ToString();
                    
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        lstDtl.Add(new ImportJobOrderDtl
                        {

                            //JobOrderDtlId = result["JobOrderDtlId"].ToString(),
                            JobOrderId= Convert.ToInt32(ImpJobOrderId),       
                                              
                           JobOrderDtlId =Convert.ToInt32(result["JobOrderDtlId"]),
                            ContainerNo = result["ContainerNo"].ToString(),
                            ContainerSize = result["ContainerSize"].ToString(),
                            Reefer = (Convert.ToInt32(result["Reefer"]) == 1 ? "Reefer" : "Non Reefer").ToString(),
                          //  Reefer = Convert.ToInt32(result["Reefer"]),

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
        public void DelImpemptcntJO(int ImpJobOrderId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("DelImpemptcntJO", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Empty Job Order Deleted Successfully";
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
        public void AddEditImpContJO(ImportJobOrder objJO, string XML = null)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpJobOrderId", MySqlDbType = MySqlDbType.Int32, Value = objJO.ImpJobOrderId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PickUpLocation", MySqlDbType = MySqlDbType.VarChar, Size = 6, Value = objJO.PickUpLocation });
           // lstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Value = objJO.FormOneId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderFor", MySqlDbType = MySqlDbType.Int32, Size = 2, Value = objJO.JobOrderFor });
            lstParam.Add(new MySqlParameter { ParameterName = "in_JobOrderDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objJO.JobOrderDate).ToString("yyyy-MM-dd") });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objJO.ShippingLineId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Value = objJO.CHAId });
           // lstParam.Add(new MySqlParameter { ParameterName = "in_FromLocation", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = objJO.FromLocation });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_ToYardId", MySqlDbType = MySqlDbType.Int32, Value = objJO.ToYardId });
           // lstParam.Add(new MySqlParameter { ParameterName = "in_YardWiseLocationIds", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = objJO.YardWiseLocationIds });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_YardWiseLctnNames", MySqlDbType = MySqlDbType.VarChar, Size = 200, Value = objJO.YardWiseLctnNames });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = objJO.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
           
            //  lstParam.Add(new MySqlParameter { ParameterName = "in_LctnXML", MySqlDbType = MySqlDbType.Text, Value = LocationXML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = XML });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditImpContJo", CommandType.StoredProcedure, dpram);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Empty Job Order Saved Successfully" : "Empty Job Order Updated Successfully";
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



        public void SaveImportIgmFile(string FileName, int Uid, string VesselNo, string VoyageNo, int ShippingLineId, string ShippingLineName, string RotationNo, int BranchId, string CargoXML, string ContainerXML)
        {
            string Status = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FileName", MySqlDbType = MySqlDbType.VarChar, Value = FileName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VesselNo", MySqlDbType = MySqlDbType.VarChar, Value = VesselNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VoyageNo", MySqlDbType = MySqlDbType.VarChar, Value = VoyageNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.VarChar, Value = ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.VarChar, Value = ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RotationNo", MySqlDbType = MySqlDbType.VarChar, Value = RotationNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.VarChar, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.LongText, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.LongText, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("SaveImportIgmFile", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "File Imported Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == -1)
                {
                    DBResponse.Status = -1;
                    _DBResponse.Message = "File Already Uploaded Once.";
                    _DBResponse.Data = null;
                }
                else if (Result == -2)
                {
                    DBResponse.Status = -2;
                    _DBResponse.Message = "Voyage No. And Vessel No. Entered Does Not Match With The Details In File Uploaded.";
                    _DBResponse.Data = null;
                }
                else if (Result == -3)
                {
                    DBResponse.Status = 0;
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




        public void SaveImportIALFile(string FileName, int Uid, string VesselNo, string VoyageNo, int ShippingLineId, string ShippingLineName, string RotationNo, int BranchId, string Kdl_FormOneModelListXML)
        {
            string Status = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FileName", MySqlDbType = MySqlDbType.VarChar, Value = FileName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VesselNo", MySqlDbType = MySqlDbType.VarChar, Value = VesselNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VoyageNo", MySqlDbType = MySqlDbType.VarChar, Value = VoyageNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.VarChar, Value = ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.VarChar, Value = ShippingLineName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RotationNo", MySqlDbType = MySqlDbType.VarChar, Value = RotationNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.VarChar, Value = BranchId });
            //LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.LongText, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.LongText, Value = Kdl_FormOneModelListXML });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("SaveImportIALFile", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "File Imported Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == -1)
                {
                    DBResponse.Status = -1;
                    _DBResponse.Message = "File Already Uploaded Once.";
                    _DBResponse.Data = null;
                }
                else if (Result == -2)
                {
                    DBResponse.Status = -2;
                    _DBResponse.Message = "Voyage No. And Vessel No. Entered Does Not Match With The Details In File Uploaded.";
                    _DBResponse.Data = null;
                }
                else if (Result == -3)
                {
                    DBResponse.Status = 0;
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
        public void AddEditFormOne(Kdl_FormOneModel objFormOne, int BranchId, string FormOneDetailXML, int CreatedBy)
        {
            string Status = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.FormOneId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BLNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.BLNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VesselName", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.VesselName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VoyageNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.VoyageNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RotationNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.RotationNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortOfDischargeId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.PortOfDischargeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortCharge", MySqlDbType = MySqlDbType.Decimal, Value = objFormOne.PortCharge });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortChargeAmt", MySqlDbType = MySqlDbType.Decimal, Value = objFormOne.PortChargeAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.LCLFCL });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneDetailXML", MySqlDbType = MySqlDbType.Text, Value = FormOneDetailXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = CreatedBy });

            LstParam.Add(new MySqlParameter { ParameterName = "@RetValue", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "@GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Size = 4, Direction = ParameterDirection.Output, Value = 0 });

            DParam = LstParam.ToArray();
            int Result = DataAccess.ExecuteNonQuery("AddEditFormOne", CommandType.StoredProcedure, DParam, out Status);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Form-1 Saved Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Form-1 Updated Successfully.";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Edit Form-1 Details As It Already Exists In Another Page";
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
        public void DeleteFormOne(int FormOneId)
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
                    _DBResponse.Message = "Form-1 Data Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Form-1 Data Does Not Exists";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Cannot Delete Form-1 Details As It Already Exists In Another Page";
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
        public void ListOfShippingLine()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "ShippingLine" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTrader", CommandType.StoredProcedure, DParam);
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
                        ShippingLineId = Convert.ToInt32(result["EximTraderId"]),
                        ShippingLineName = result["NameAddress"].ToString()
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
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "CHA" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfEximTrader", CommandType.StoredProcedure, DParam);
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
                        CHAName = result["NameAddress"].ToString()
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
        public void ListOfPOD()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfPOD", CommandType.StoredProcedure);
            IList<PortOfDischarge> lstPOD = new List<PortOfDischarge>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstPOD.Add(new PortOfDischarge
                    {
                        PODId = Convert.ToInt32(result["PortId"]),
                        PODName = result["PortName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstPOD;
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
        public void ListOfCommodity()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetMstCommodity", CommandType.StoredProcedure, DParam);
            IList<CwcExim.Areas.Import.Models.Commodity> lstCommodity = new List<CwcExim.Areas.Import.Models.Commodity>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCommodity.Add(new CwcExim.Areas.Import.Models.Commodity
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
        public void GetFormOne()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFormOne", CommandType.StoredProcedure, DParam);
            IList<Kdl_FormOneModel> lstFormOne = new List<Kdl_FormOneModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new Kdl_FormOneModel
                    {
                        FormOneId = Convert.ToInt32(result["FormOneId"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        FormOneDate = Convert.ToString(result["FormOneDate"]),
                        ShippingLineName = Convert.ToString(result["EximTraderName"])
                        //,
                        //TrBondNo= Convert.ToString(result["TrBondNo"])
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
        #region Empty Cont Movement
        //public void GetImpPaymentPartyForPage(string PartyCode, int Page = 0)
        //{
        //    int Status = 0;
        //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //    List<MySqlParameter> lstParam = new List<MySqlParameter>();
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
        //    lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
        //    IDataParameter[] Dparam = lstParam.ToArray();
        //    IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPartyForPage", CommandType.StoredProcedure, Dparam);
        //    IList<WFLDImpPartyForpage> lstParty = new List<WFLDImpPartyForpage>();
        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        bool State = false;
        //        while (Result.Read())
        //        {
        //            Status = 1;
        //            lstParty.Add(new WFLDImpPartyForpage
        //            {
        //                PartyId = Convert.ToInt32(Result["CHAId"]),
        //                PartyName = Convert.ToString(Result["CHAName"]),
        //                Address = Convert.ToString(Result["Address"]).Replace("\r\n", ""),
        //                State = Convert.ToString(Result["StateName"]),
        //                StateCode = Convert.ToString(Result["StateCode"]),
        //                GSTNo = Convert.ToString(Result["GSTNo"]),
        //                PartyCode = Convert.ToString(Result["PartyCode"]),


        //                IsTransporter = Convert.ToBoolean(Result["IsTransporter"]),
        //                InsuredFrmDate = Convert.ToString(Result["InsuredFrmDate"]),
        //                InsuredToDate = Convert.ToString(Result["InsuredToDate"]),
        //                IsInsured = Convert.ToBoolean(Result["IsInsured"])
        //            });
        //        }
        //        if (Result.NextResult())
        //        {
        //            while (Result.Read())
        //            {
        //                State = Convert.ToBoolean(Result["State"]);
        //            }
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Status = 1;
        //            _DBResponse.Message = "Success";
        //            _DBResponse.Data = new { lstParty, State };
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
        public void GetImpPaymentPartyForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPartyForPage", CommandType.StoredProcedure, Dparam);
            IList<Kdl_ImporterParty> lstParty = new List<Kdl_ImporterParty>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new Kdl_ImporterParty
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]).Replace("\r\n", ""),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        PartyCode = Convert.ToString(Result["PartyCode"]),


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
        public void GetEmptyContainerListForMovement()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = DateTime.Now.ToString("yyyy-MM-dd") });
            //LstParam.Add(new MySqlParameter { ParameterName = "GateInId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("EmptyContList", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            IList<Kdl_EmptyMovement> objListForInvoice = new List<Kdl_EmptyMovement>();
            try
            {
                //ShippingLineId, ShippingLineName, GSTNo, CFSCode, ContainerNo, EmptyDate, Address, StateCode, StateName
                while (Result.Read())
                {
                    Status = 1;

                    objListForInvoice.Add(new Kdl_EmptyMovement()
                    {
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]),
                        ShippingLineName = Convert.ToString(Result["ShippingLineName"]),
                        Size = Convert.ToString(Result["Size"]),
                        CfsCode = Convert.ToString(Result["CFSCode"]),
                        ContainerNo = Convert.ToString(Result["ContainerNo"]),
                        EmptyDate = Convert.ToString(Result["EmptyDate"]),
                        //Address = Convert.ToString(Result["Address"]),
                        //StateName = Convert.ToString(Result["StateName"]),
                        //StateCode = Convert.ToString(Result["StateCode"]),
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
        public void AddEditEmptyContMovement(Kdl_EmptyMovement obj, int Uid)

        {
            string ReturnObj = "0";
            string GeneratedClientId = "0";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });

            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = obj.ShippingLineId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.VarChar, Value = obj.ShippingLineName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = obj.ContainerNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CfsCode", MySqlDbType = MySqlDbType.VarChar, Value = obj.CfsCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_MovementEntryDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(obj.MovementEntryDate).ToString("yyyy-MM-dd") });

            lstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Value = obj.EntryId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = obj.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Value = obj.Size });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = "0" });
            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] dpram = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("AddEditEmptyContMovement", CommandType.StoredProcedure, dpram,out GeneratedClientId,out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Empty Container Movement Saved Successfully" : "Empty Container Movement Updated Successfully";
                }
                else if (result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = "Can't Update As Empty Container Movement Invoice Generated";
                }
                else if (result == 4)
                {
                    _DBResponse.Data = ReturnObj;
                    _DBResponse.Status = result;
                    _DBResponse.Message = "Can't Update As Empty Container Movement Date Is Less than Gate Entry Gate";
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

    
        public void GetEmptyContMovementList(string ContainerNo)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = "FCL" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("EmptyContMovementList", CommandType.StoredProcedure, DParam);
            IList<Kdl_EmptyMovement> lstMovement = new List<Kdl_EmptyMovement>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstMovement.Add(new Kdl_EmptyMovement
                    {
                        EntryId = Convert.ToInt32(result["Id"]),
                        ShippingLineId = Convert.ToInt32(result["ShippingLineId"]),
                        MovementEntryDate = Convert.ToString(result["MovementEntryDate"]),
                        ShippingLineName = Convert.ToString(result["ShippingLineName"]),
                        ContainerNo = Convert.ToString(result["ContainerNo"]),
                        CfsCode = Convert.ToString(result["CfsCode"])
                        //,
                        //TrBondNo= Convert.ToString(result["TrBondNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstMovement;
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
        public void GetEmptyContMovementListSearch(String ContainerNo)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = "FCL" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("EmptyContMovementListSearch", CommandType.StoredProcedure, DParam);
            IList<Kdl_EmptyMovement> lstMovement = new List<Kdl_EmptyMovement>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstMovement.Add(new Kdl_EmptyMovement
                    {
                        EntryId = Convert.ToInt32(result["Id"]),
                        ShippingLineId = Convert.ToInt32(result["ShippingLineId"]),
                        MovementEntryDate = Convert.ToString(result["MovementEntryDate"]),
                        ShippingLineName = Convert.ToString(result["ShippingLineName"]),
                        ContainerNo = Convert.ToString(result["ContainerNo"]),
                        CfsCode = Convert.ToString(result["CfsCode"])
                        //,
                        //TrBondNo= Convert.ToString(result["TrBondNo"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstMovement;
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

        public void GetEmptyContMovement(string id)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = id });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = "FCL" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("EmptyContMovement", CommandType.StoredProcedure, DParam);
            Kdl_EmptyMovement objMovement = new Kdl_EmptyMovement();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;

                    objMovement.EntryId = Convert.ToInt32(result["Id"]);
                    objMovement.ShippingLineId = Convert.ToInt32(result["ShippingLineId"]);
                    objMovement.MovementEntryDate = Convert.ToString(result["MovementEntryDate"]);
                    objMovement.ShippingLineName = Convert.ToString(result["ShippingLineName"]);
                    objMovement.ContainerNo = Convert.ToString(result["ContainerNo"]);
                    objMovement.CfsCode = Convert.ToString(result["CfsCode"]);
                    objMovement.Size = Convert.ToString(result["Size"]);
                    objMovement.Remarks = Convert.ToString(result["Remarks"]);
                    //,
                    //TrBondNo= Convert.ToString(result["TrBondNo"])

                }
                if (Status == 1)
                {
                    _DBResponse.Data = objMovement;
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


        public void GetEmptyContMovementDelete(string id)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Id", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = id });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });

            //LstParam.Add(new MySqlParameter { ParameterName = "in_LCLFCL", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = "FCL" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int result = DA.ExecuteNonQuery("EmptyContMovementDelete", CommandType.StoredProcedure, DParam);
            Kdl_EmptyMovement objMovement = new Kdl_EmptyMovement();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                //while (result.Read())
                //{
                //    Status = 1;

                //    objMovement.EntryId = Convert.ToInt32(result["Id"]);
                //    objMovement.ShippingLineId = Convert.ToInt32(result["ShippingLineId"]);
                //    objMovement.MovementEntryDate = Convert.ToString(result["MovementEntryDate"]);
                //    objMovement.ShippingLineName = Convert.ToString(result["ShippingLineName"]);
                //    objMovement.ContainerNo = Convert.ToString(result["ContainerNo"]);
                //    objMovement.CfsCode = Convert.ToString(result["CfsCode"]);
                //    objMovement.Size = Convert.ToString(result["Size"]);
                //    objMovement.Remarks = Convert.ToString(result["Remarks"]);
                //    //,
                //    //TrBondNo= Convert.ToString(result["TrBondNo"])

                //}
                if (result == 1 || result == 2)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = result;
                    _DBResponse.Message = (result == 1) ? "Empty Container Movement Deleted Successfully" : "Can't Delete As Empty Container Movement Invoice Generated";
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
                //result.Dispose();
                //result.Close();
            }
        }




        #endregion

        public void GetFormOneByContainer(string ContainerName)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_containerNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = ContainerName });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFormBYContainer", CommandType.StoredProcedure, DParam);
            IList<Kdl_FormOneModel> lstFormOne = new List<Kdl_FormOneModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new Kdl_FormOneModel
                    {
                        FormOneId = Convert.ToInt32(result["FormOneId"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        FormOneDate = Convert.ToString(result["FormOneDate"]),
                        ShippingLineName = Convert.ToString(result["EximTraderName"])
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
        public void GetFormOneById(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFormOne", CommandType.StoredProcedure, DParam);
            Kdl_FormOneModel objFormOne = new Kdl_FormOneModel();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objFormOne.FormOneId = Convert.ToInt32(result["FormOneId"]);
                    objFormOne.FormOneNo = Convert.ToString(result["FormOneNo"]);
                    objFormOne.FormOneDate = Convert.ToString(result["FormOneDate"]);
                    objFormOne.BLNo = Convert.ToString(result["BLNo"]);
                    objFormOne.ShippingLineId = Convert.ToInt32(result["ShippingLineId"]);
                    objFormOne.ShippingLineName = Convert.ToString(result["EximTraderName"]);
                    objFormOne.VesselName = Convert.ToString(result["VesselName"]);
                    objFormOne.VoyageNo = Convert.ToString(result["VoyageNo"]);
                    objFormOne.RotationNo = Convert.ToString(result["RotationNo"]);
                    objFormOne.PortOfDischargeId = Convert.ToInt32(result["PortOfDischargeId"]);
                    objFormOne.PortName = Convert.ToString(result["PortName"]);
                    objFormOne.PortCharge = Convert.ToDecimal(result["PortCharge"]);
                    objFormOne.PortChargeAmt = Convert.ToDecimal(result["PortChargeAmount"]);
                    objFormOne.CargoType = Convert.ToInt32(result["CargoType"]);
                    objFormOne.LCLFCL = Convert.ToString(result["LCLFCL"]);
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objFormOne.lstFormOneDetail.Add(new Kdl_FormOneDetailModel()
                        {
                            FormOneDetailID = Convert.ToInt32(result["FormOneDetailId"]),
                            ContainerNo = Convert.ToString(result["ContainerNo"]),
                            ContainerSize = Convert.ToString(result["ContainerSize"]),
                            Reefer = Convert.ToInt32(result["Reefer"]),
                            FlatReck = Convert.ToInt32(result["FlatReck"]),
                            SealNo = Convert.ToString(result["SealNo"]),
                            LineNo = Convert.ToString(result["LineNo"]),
                            MarksNo = Convert.ToString(result["MarksNo"]),
                            CHAId = Convert.ToInt32(result["CHAId"]),
                            CHAName = Convert.ToString(result["CHAName"]),
                            ImporterId = Convert.ToInt32(result["ImporterId"]),
                            ImporterName = Convert.ToString(result["ImporterName"]),
                            CargoDesc = Convert.ToString(result["CargoDesc"]),
                            CommodityId = Convert.ToInt32(result["CommodityId"]),
                            CommodityName = Convert.ToString(result["CommodityName"]),
                            CargoType = Convert.ToInt32(result["CargoType"]),
                            DateOfLanding = Convert.ToString(result["DateOfLanding"]),
                            Remarks = Convert.ToString(result["Remarks"])
                        });
                    }
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
                    ObjDeliveryApp.DeliveryDate = Convert.ToString(Result["DeliveryDate"]);

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
                            DelNoOfPackages = Convert.ToDecimal(Result["DelNoOfPackages"] == DBNull.Value ? 0 : Result["DelNoOfPackages"]),
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
        public void FormOnePrint(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("FormOnePrint", CommandType.StoredProcedure, DParam);
            Kdl_FormOnePrintModel objFormOne = new Kdl_FormOnePrintModel();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objFormOne.ShippingLineNo = Convert.ToString(result["ShippingLine"]);
                    objFormOne.FormOneNo = Convert.ToString(result["CustomSlNo"]);
                    objFormOne.FormOneDate = Convert.ToString(result["FormOneDate"]);
                    objFormOne.CHAName = Convert.ToString(result["EximTraderName"]);
                    objFormOne.CHAAddress = Convert.ToString(result["Address"]);
                    objFormOne.CHAPhoneNo = Convert.ToString(result["PhoneNo"]);
                    objFormOne.ShippingLineNo = result["ShippingLine"].ToString();
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objFormOne.lstFormOnePrintDetail.Add(new Kdl_FormOnePrintDetailModel()
                        {
                            VesselName = Convert.ToString(result["VesselName"]),
                            VoyageNo = Convert.ToString(result["VoyageNo"]),
                            RotationNo = Convert.ToString(result["RotationNo"]),
                            ContainerNo = Convert.ToString(result["ContainerNo"]),
                            SealNo = Convert.ToString(result["SealNo"]),
                            ImpName = Convert.ToString(result["EximTraderName"]),
                            ImpAddress = Convert.ToString(result["Address"]),
                            ImpName2 = Convert.ToString(result["ImporterParty2"]),
                            ImpAddress2 = Convert.ToString(result["ImporterPartyAddress2"]),
                            Type = Convert.ToString(result["TypeLoadEmpty"]),
                            LineNo = Convert.ToString(result["LineNo"]),
                            CargoDesc = Convert.ToString(result["CargoDesc"]),
                            DateOfLanding = Convert.ToString(result["DateOfLanding"]),
                            HazType = Convert.ToInt32(result["HazType"]) == 2 ? "NON-HAZ" : "HAZ",
                            ReferType = Convert.ToInt32(result["ReferType"]) == 0 ? "" : "/ REEFER"
                        });
                    }
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
        public void AddEditDeliveryApplication(DeliveryApplication ObjDeliveryApp, string DeliveryXml)
        {
            DateTime dt = DateTime.ParseExact(ObjDeliveryApp.DeliveryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String DeliveryDate = dt.ToString("yyyy-MM-dd");

            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjDeliveryApp.DeliveryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.Date, Value = DeliveryDate });
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
                else if (Result == 5)
                {
                    _DBResponse.Status = 5;
                    _DBResponse.Message = "Delivery Application Date Should be Greater Than or Equal To Destuffing Date";
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

        #region IRN Responce
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
                            PrdDesc =null,// Result["PrdDesc"].ToString(),
                            IsServc = Result["IsServc"].ToString(),
                            HsnCd = Result["HsnCd"].ToString(),
                            Barcde = Result["Barcde"].ToString(),
                            Qty = Convert.ToDecimal(Result["Qty"].ToString()),
                            FreeQty = Convert.ToInt32(Result["FreeQty"].ToString()),
                            Unit =null,// Result["Unit"].ToString(),
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
            Kdl_IrnB2CDetails Obj = new Kdl_IrnB2CDetails();

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
                log.Info("Invoice No: "+InvoiceNo +" Erorr Message: "+ ex.Message);
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

        #region ImportBondConversion Godown
        public void GetAllOBLNo()
        {
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            IDataParameter[] Dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetOBLForBondConversion", CommandType.StoredProcedure, Dpram);
            _DBResponse = new DatabaseResponse();
            List<KDL_OBLNoForBondConversion> lstOBL = new List<KDL_OBLNoForBondConversion>();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstOBL.Add(new KDL_OBLNoForBondConversion
                    {
                        OBLNo = result["OBLNo"].ToString(),
                        DestuffingId = Convert.ToInt32(result["DestuffingEntryDtlId"]),
                        CHAId = Convert.ToInt32(result["CHAId"]),
                        ImporterId = Convert.ToInt32(result["ImporterId"])
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstOBL;
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


        public void GetGodwonById(int DestuffingId)
        {
            String ReturnObj = "";
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingId });
            //lstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Value = ReturnObj, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetGodownForBondTransfer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            KDL_ImportBondConversion objConv = new KDL_ImportBondConversion();
            try
            {


                if (Result.Read())
                {

                    Status = 1;

                    objConv.FromGodownId = Convert.ToInt32(Result["GodownId"]);
                    objConv.FromGodownName = Convert.ToString(Result["GodownName"]);
                }

                if (Status == 1)
                {
                    _DBResponse.Data = objConv;
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

        public void GetLocationForBondTransfer(int DestuffingId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLocationForBondTransfer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<LocationForBondTransfer> LstLocation = new List<LocationForBondTransfer>();
            try
            {
                while (Result.Read())

                {
                    Status = 1;
                    LstLocation.Add(new LocationForBondTransfer
                    {
                        FromLocationId = Convert.ToInt32(Result["LoactionId"]),
                        FromLocationName = Convert.ToString(Result["LocationName"]),
                        DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"])

                    });
                    if (Status == 1)
                    {
                        _DBResponse.Status = 1;
                        _DBResponse.Message = "Success";
                        _DBResponse.Data = LstLocation;
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

        public void GetOBLDetailsByDestuffingIdList(int DestuffingEntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingEntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLDetForBondTransfer", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<KDL_ImportBondConversion> LstobjImportBondConversion = new List<KDL_ImportBondConversion>();
            try
            {
                while (Result.Read())

                {
                    Status = 1;
                    LstobjImportBondConversion.Add(new KDL_ImportBondConversion
                    {
                        //FromLocationId = Convert.ToInt32(Result["LoactionId"]),
                        //FromLocationName = Convert.ToString(Result["LocationName"]),
                        //DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"])

                        FromGodownId = Convert.ToInt32(Result["GodownId"]),
                        FromGodownName = Convert.ToString(Result["GodownName"]),
                        FromLocationId = Convert.ToString(Result["FromLocationId"]),
                        FromLocationName = Convert.ToString(Result["LocationNames"]),
                        NoOfPkg = Convert.ToInt32(Result["NoOfPkg"]),
                        Weight = Convert.ToDecimal(Result["GrWeight"]),
                        SQM = Convert.ToDecimal(Result["SQM"]),
                        CUM = Convert.ToDecimal(Result["CUM"]),
                        CIF = Convert.ToDecimal(Result["CIF"]),
                        Duty = Convert.ToDecimal(Result["Duty"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstobjImportBondConversion;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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

        public void GetOBLDetailsByDestuffingId(int DestuffingEntryId)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingEntryId });

                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetOBLDetForBondTransfer", CommandType.StoredProcedure, DParam);

                _DBResponse = new DatabaseResponse();

                KDL_ImportBondConversion objImportBondConversion = new KDL_ImportBondConversion();

                if (Result != null && Result.Tables.Count > 0)
                {
                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        Status = 1;
                        objImportBondConversion.FromGodownId = Convert.ToInt32(Result.Tables[0].Rows[0]["GodownId"]);
                        objImportBondConversion.FromGodownName = Convert.ToString(Result.Tables[0].Rows[0]["GodownName"]);
                        objImportBondConversion.FromLocationId = Convert.ToString(Result.Tables[0].Rows[0]["FromLocationId"]);
                        objImportBondConversion.FromLocationName = Convert.ToString(Result.Tables[0].Rows[0]["LocationNames"]);
                        objImportBondConversion.NoOfPkg = Convert.ToInt32(Result.Tables[0].Rows[0]["NoOfPkg"]);
                        objImportBondConversion.Weight = Convert.ToDecimal(Result.Tables[0].Rows[0]["GrWeight"]);
                        objImportBondConversion.SQM = Convert.ToDecimal(Result.Tables[0].Rows[0]["SQM"]);
                        objImportBondConversion.CUM = Convert.ToDecimal(Result.Tables[0].Rows[0]["CUM"]);
                        objImportBondConversion.CIF = Convert.ToDecimal(Result.Tables[0].Rows[0]["CIF"]);
                        objImportBondConversion.Duty = Convert.ToDecimal(Result.Tables[0].Rows[0]["Duty"]);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objImportBondConversion;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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

        public void GetAllSacNo(string SAC = "", int ChaId = 0, int ImporterId = 0, string OBLNo = "")
        {
            String ReturnObj = "";
            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_SAC", MySqlDbType = MySqlDbType.VarChar, Value = SAC });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ChaId", MySqlDbType = MySqlDbType.Int32, Value = ChaId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ImporterId", MySqlDbType = MySqlDbType.Int32, Value = ImporterId });
            lstparam.Add(new MySqlParameter { ParameterName = "in_ObllNo", MySqlDbType = MySqlDbType.VarChar, Value = OBLNo });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = lstparam.ToArray();
            IDataReader Result = DA.ExecuteDataReader("GetSacForBondConversion", CommandType.StoredProcedure, Dparam);
            _DBResponse = new DatabaseResponse();
            int Status = 0;

            List<KDL_ImportBondConversion> LstSac = new List<KDL_ImportBondConversion>();
            try
            {
                while (Result.Read())

                {
                    Status = 1;
                    LstSac.Add(new KDL_ImportBondConversion
                    {
                        SACId = Convert.ToInt32(Result["SacId"]),
                        SACNo = Convert.ToString(Result["SACNo"]),
                        SACDate = Result["SACDate"].ToString()

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstSac;
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

        public void GetAllInternalBondMovement(string Area)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Area", MySqlDbType = MySqlDbType.VarChar, Value = Area });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<KDL_ImportBondConversion> LstInternalMovement = new List<KDL_ImportBondConversion>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstInternalMovement.Add(new KDL_ImportBondConversion
                    {
                        MovementNo = Result["MovementNo"].ToString(),
                        MovementDate = Result["MovementDate"].ToString(),
                        OBLNo = Result["BOENo"].ToString(),
                        MovementId = Convert.ToInt32(Result["MovementId"]),
                        OBLDate = Result["BOEDate"].ToString(),
                        GodownName = Result["GodownName"].ToString(),
                        DeliveryCount = Convert.ToInt32(Result["DeliveryCount"])
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

        public void AddEditImportBondConversion(KDL_ImportBondConversion ObjInternalMovement)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingEntryDtlId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.DestuffingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.MovementDate).ToString("yyyy/MM/dd") });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BOENo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.OBLNo });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BOEDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.OBLDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OBLNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.OBLNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OBLDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.OBLDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Value = "" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.NoOfPkg });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FromGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.FromGodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToGodownId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjInternalMovement.GodownId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLocationIds", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.FromLocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OldLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.FromLocationName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLocationIds", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.LocationId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NewLctnNames", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.Location });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ModuleNameMovement", MySqlDbType = MySqlDbType.VarChar, Value = "Import" });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacId", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.SACId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SACNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.SACNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SACDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.SACDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Value", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.CIF });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Duty", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.Duty });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SQM", MySqlDbType = MySqlDbType.Decimal, Value = ObjInternalMovement.SQM });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ((Login)(HttpContext.Current.Session["LoginUser"])).Uid });

            LstParam.Add(new MySqlParameter { ParameterName = "in_WrNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.WRNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_WrDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.WRDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BondNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.BondNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BondDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.BondDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomBondNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.CustomBondNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomBondDate", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToDateTime(ObjInternalMovement.CustomBondDate).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjInternalMovement.CustomSealNo });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditImportBondConversion", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjInternalMovement.MovementId == 0 ? "Internal Movement Details Saved Successfully" : "Internal Movement  Details Updated Successfully";
                    _DBResponse.Data = null;
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

        public void GetBondInternalMovement(int MovementId, string Area)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_MovementId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = MovementId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Area", MySqlDbType = MySqlDbType.VarChar, Value = Area });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpInternalMovement", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            KDL_ImportBondConversion ObjInternalMovement = new KDL_ImportBondConversion();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjInternalMovement.MovementId = Convert.ToInt32(Result["MovementId"]);
                    ObjInternalMovement.MovementNo = Result["MovementNo"].ToString();
                    ObjInternalMovement.MovementDate = Result["MovementDate"].ToString();
                    ObjInternalMovement.OBLNo = Result["OBLNo"].ToString();
                    ObjInternalMovement.OBLDate = Result["OBLDate"].ToString();
                    ObjInternalMovement.FromGodownId = Convert.ToInt32(Result["FromGodownId"] == DBNull.Value ? 0 : Result["FromGodownId"]);
                    ObjInternalMovement.FromGodownName = Result["FromGodownName"].ToString();
                    ObjInternalMovement.FromLocationId = Result["FromLocationId"].ToString();
                    ObjInternalMovement.FromLocationName = Result["FromLocationName"].ToString();
                    ObjInternalMovement.NoOfPkg = Convert.ToInt32(Result["NoOfPkg"].ToString());
                    ObjInternalMovement.Weight = Convert.ToDecimal(Result["Weight"]);
                    ObjInternalMovement.SQM = Convert.ToDecimal(Result["SQM"]);
                    ObjInternalMovement.CUM = Convert.ToDecimal(Result["CUM"]);
                    ObjInternalMovement.CIF = Convert.ToDecimal(Result["CIF"]);
                    ObjInternalMovement.Duty = Convert.ToDecimal(Result["Duty"]);
                    ObjInternalMovement.GodownId = Convert.ToInt32(Result["GodownId"]);
                    ObjInternalMovement.GodownName = Result["GodownName"].ToString();
                    ObjInternalMovement.SACId = Convert.ToInt32(Result["SACId"]);
                    ObjInternalMovement.SACNo = Result["SACNo"].ToString();
                    ObjInternalMovement.SACDate = Result["SACDate"].ToString();
                    ObjInternalMovement.DestuffingEntryDtlId = Convert.ToInt32(Result["DestuffingEntryDtlId"]);
                    ObjInternalMovement.DestuffingId = Convert.ToInt32(Result["DestuffingEntryDtlId"]);
                    ObjInternalMovement.DestuffingEntryId = Convert.ToInt32(Result["DestuffingEntryId"]);
                    ObjInternalMovement.LocationId = Result["LocationId"].ToString();
                    ObjInternalMovement.Location = Result["Location"].ToString();
                    ObjInternalMovement.WRNo = Result["WRNo"].ToString();
                    ObjInternalMovement.WRDate = Result["WRDate"].ToString();
                    ObjInternalMovement.BondNo = Result["BondNo"].ToString();
                    ObjInternalMovement.BondDate = Result["BondDate"].ToString();
                    ObjInternalMovement.CustomBondNo = Result["CustomBondNo"].ToString();
                    ObjInternalMovement.CustomBondDate = Result["CustomBondDate"].ToString();
                    ObjInternalMovement.CustomSealNo = Result["CustomSealNo"].ToString();
                    ObjInternalMovement.CHAId = Convert.ToInt32(Result["CHAId"].ToString());
                    ObjInternalMovement.ImporterId = Convert.ToInt32(Result["ImporterId"].ToString());

                    ObjInternalMovement.RemainingNoOfPkg = Convert.ToInt32(Result["RemainingNoOfPkg"].ToString());
                    ObjInternalMovement.RemainingGrWeight = Convert.ToDecimal(Result["RemainingGrWeight"]);
                    ObjInternalMovement.RemainingSQM = Convert.ToDecimal(Result["RemainingSQM"]);
                    ObjInternalMovement.RemainingCUM = Convert.ToDecimal(Result["RemainingCUM"]);
                    ObjInternalMovement.RemainingCIF = Convert.ToDecimal(Result["RemainingCIF"]);
                    ObjInternalMovement.RemainingDuty = Convert.ToDecimal(Result["RemainingDuty"]);
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
            List<Areas.Import.Models.Godown> LstGodown = new List<Areas.Import.Models.Godown>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstGodown.Add(new Areas.Import.Models.Godown
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
        public void GetAllGodownExpBond(int Uid, string SacNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacNo", MySqlDbType = MySqlDbType.VarChar, Value = SacNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllGodownExpBond", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Areas.Import.Models.Godown> LstGodown = new List<Areas.Import.Models.Godown>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstGodown.Add(new Areas.Import.Models.Godown
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
                        LocationName = result["LocationName"].ToString(),
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
        #region Show list Godowndeliverypaymentsheet
        public void ListOfInvoice(string Module, string InvoiceNo = null, string BOENo = null, string InvoiceDate = null, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BoENo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Page });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Kdl_ListOfImpInvoice> lstExpInvoice = new List<Kdl_ListOfImpInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExpInvoice.Add(new Kdl_ListOfImpInvoice()
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
        public void ListLoadMoreInvoice(string Module, string InvoiceNo = null, string BOENo = null, string InvoiceDate = null, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if (InvoiceDate == "") InvoiceDate = null;
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BoENo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = BOENo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofexpInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<Kdl_ListOfImpInvoice> lstExpInvoice = new List<Kdl_ListOfImpInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExpInvoice.Add(new Kdl_ListOfImpInvoice()
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
        #endregion
    }
}