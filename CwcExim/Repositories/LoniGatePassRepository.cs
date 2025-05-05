using CwcExim.Areas.GateOperation.Models;
using CwcExim.DAL;
using CwcExim.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CwcExim.Repositories
{
    public class LoniGatePassRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

            }
        }
        public void InvoiceListForGatePass(int uid, int Page, string InvoiceNo = "")
        {
            //int uid
            //int uid = Convert.ToInt32(((Login)HttpContext.Current.Session["in_UserId"]).Uid);
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            //lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = uid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceNoforGP", CommandType.StoredProcedure, DParam);
            List<PpgInvoiceNoList> lstInvoice = new List<PpgInvoiceNoList>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstInvoice.Add(new PpgInvoiceNoList
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        Module = Result["Module"].ToString(),
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
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void DetailsForGP(int InvoiceId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDetForGP", CommandType.StoredProcedure, DParam);
            List<ContainerDet> lstContainerDet = new List<ContainerDet>();
            string CHAName = "", ImpExpName = "", ShippingLineName = "", DeliveryDate = "";
            int Status = 0;
            string Module = "";
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    CHAName = Result["CHAName"].ToString();
                    ShippingLineName = Result["ShippingLineName"].ToString();
                    ImpExpName = Result["ImpExpName"].ToString();
                    Module = Result["Module"].ToString();
                    DeliveryDate = Result["DeliveryDate"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstContainerDet.Add(new ContainerDet
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            IsReefer = Convert.ToBoolean(Result["IsReefer"]),
                            Size = Result["Size"].ToString(),
                            Location = Result["Location"].ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"] == DBNull.Value ? 0 : Result["NoOfUnits"]),
                            VehicleNo = "",
                            Weight = Convert.ToDecimal(Result["Weight"])
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { lstContainerDet, ImpExpName, ShippingLineName, CHAName, Module, DeliveryDate };
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
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void ListOfGatePass(int Page, int UserId)
        {
            //,int UserId
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = UserId });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfGP", CommandType.StoredProcedure, DParam);
            List<ListOfGP> lstGP = new List<ListOfGP>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGP.Add(new ListOfGP
                    {
                        GatePassId = Convert.ToInt32(Result["GatePassId"]),
                        GatePassNo = Convert.ToString(Result["GatePassNo"]),
                        GatePassDate = Result["GatePassDate"].ToString(),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        IsCancelled = Result["IsCancelled"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstGP;
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
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void ListOfGatePassSearch(int UserId, string GatepassNo)
        {
            //,int UserId
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_GatepassNo", MySqlDbType = MySqlDbType.VarChar, Value = GatepassNo });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            // lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = UserId });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfGPSearch", CommandType.StoredProcedure, DParam);
            List<ListOfGP> lstGP = new List<ListOfGP>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGP.Add(new ListOfGP
                    {
                        GatePassId = Convert.ToInt32(Result["GatePassId"]),
                        GatePassNo = Convert.ToString(Result["GatePassNo"]),
                        GatePassDate = Result["GatePassDate"].ToString(),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        IsCancelled = Result["IsCancelled"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstGP;
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
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }

        public void LoadMoreListOfGatePass(int Page, int UserId)
        {
            //,int UserId
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = UserId });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("LoadMoreListOfGP", CommandType.StoredProcedure, DParam);
            List<ListOfGP> lstGP = new List<ListOfGP>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGP.Add(new ListOfGP
                    {
                        GatePassId = Convert.ToInt32(Result["GatePassId"]),
                        GatePassNo = Convert.ToString(Result["GatePassNo"]),
                        GatePassDate = Result["GatePassDate"].ToString(),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                        IsCancelled = Result["IsCancelled"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstGP;
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
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void GetDetForGatePass(int GatePassId, int UserId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = GatePassId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            lstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = UserId });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfGP", CommandType.StoredProcedure, DParam);
            PpgGatePass objGP = new PpgGatePass();
            List<PPG_ContainerDet> lstCntnDet = new List<PPG_ContainerDet>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objGP.GatePassId = Convert.ToInt32(Result["GatePassId"]);
                    objGP.GatePassNo = Convert.ToString(Result["GatePassNo"]);
                    objGP.GatePassDate = Result["GatePassDate"].ToString();
                    objGP.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    objGP.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    objGP.InvoiceNo = Result["InvoiceNo"].ToString();
                    objGP.CHAName = (Result["CHAName"] == null ? "" : Result["CHAName"]).ToString();
                    objGP.ImpExpName = (Result["ImpExpName"] == null ? "" : Result["ImpExpName"]).ToString();
                    objGP.ShippingLineName = (Result["ShippingLineName"] == null ? "" : Result["ShippingLineName"]).ToString();
                    objGP.Module = Result["Module"].ToString();
                    objGP.DeliveryDate = Result["DeliveryDate"].ToString();
                    objGP.DepartureDate = Result["DepartureDate"].ToString();
                    objGP.ArrivalDate = Result["ArrivalDate"].ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstCntnDet.Add(new PPG_ContainerDet
                        {
                            GatePassDtlId = Convert.ToInt32(Result["GatePassDtlId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            IsReefer = Convert.ToBoolean(Result["IsReefer"]),
                            Size = Result["Size"].ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            NoOfUnits = Convert.ToInt32(Result["NoOfUnits"] == DBNull.Value ? 0 : Result["NoOfUnits"]),
                            VehicleNo = (Result["VehicleNo"] == null ? "" : Result["VehicleNo"]).ToString(),
                            Weight = Convert.ToDecimal(Result["Weight"] == DBNull.Value ? 0 : Result["Weight"]),
                            Location = Result["Location"].ToString(),
                            PortOfDispatch = Result["PortOfDispatch"].ToString(),
                        });
                    }
                }
                if (lstCntnDet.Count > 0)
                {
                    objGP.StringifyData = JsonConvert.SerializeObject(lstCntnDet);
                }

                if (Result.NextResult())
                {
                    List<PpgGatepassVehicle> lstVch = new List<PpgGatepassVehicle>();
                    while (Result.Read())
                    {
                        lstVch.Add(new PpgGatepassVehicle
                        {
                            Id = Convert.ToInt32(Result["Id"]),
                            GatePassId = Convert.ToInt32(Result["GatePassId"]),
                            ContainerNo = Result["ContainerNo"].ToString(),
                            VehicleNo = Result["VehicleNo"].ToString(),
                            Package = Convert.ToDecimal(Result["Package"]),
                        });
                    }

                    if (lstVch.Count > 0)
                    {
                        objGP.VehicleXml = JsonConvert.SerializeObject(lstVch);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objGP;
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
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void DeleteGatePass(int GatePassId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = GatePassId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteGP", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Gate Pass Deleted Successfully";
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
        public void AddEditGatePass(PpgGatePass objGP, string XML, string VehicleXml, int isSingleVehicle)
        {
            string GeneratedClientId = "0";
            var DepartureDate = (dynamic)null;
            DepartureDate = Convert.ToDateTime(objGP.DepartureDate).ToString("yyyy-MM-dd HH:MM:ss");
            var ArrivalDate = (dynamic)null;
            ArrivalDate = Convert.ToDateTime(objGP.ArrivalDate).ToString("yyyy-MM-dd HH:MM:ss");
            var GatePassDate = (dynamic)null;
            GatePassDate = Convert.ToDateTime(objGP.GatePassDate).ToString("yyyy-MM-dd HH:mm:ss");
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = objGP.GatePassId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(GatePassDate) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objGP.Remarks });
            lstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objGP.CHAName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ImpExpName", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objGP.ImpExpName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objGP.ShippingLineName });
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = objGP.InvoiceId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = XML });
            lstParam.Add(new MySqlParameter { ParameterName = "in_VehicleXML", MySqlDbType = MySqlDbType.Text, Value = VehicleXml });
            lstParam.Add(new MySqlParameter { ParameterName = "in_isSingleVehicle", MySqlDbType = MySqlDbType.Int32, Value = isSingleVehicle });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DepartureDate", MySqlDbType = MySqlDbType.DateTime, Value = DepartureDate });
            lstParam.Add(new MySqlParameter { ParameterName = "in_ArrivalDate", MySqlDbType = MySqlDbType.DateTime, Value = ArrivalDate });

            lstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditGP", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {

                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Gate Pass Saved Successfully" : "Gate Pass Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Amount Not Paid Yet ";
                    _DBResponse.Data = null;
                }
                else if (Result == 4)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Can not update as gate exit allready done";
                    _DBResponse.Data = null;
                }
                else if (Result == 5)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Payment Sheet delivery date should be equal or greater than gate pass date";
                    _DBResponse.Data = null;
                }
                else if (Result == 7)
                {
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Please click the save button again";
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
        public void GetDetailsForGatePassPrint(int GatePassId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = GatePassId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGPDetfrPrint", CommandType.StoredProcedure, DParam);
            PPG_GPHdr objGP = new PPG_GPHdr();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objGP.GatePassNo = Convert.ToString(Result["GatePassNo"]);
                    objGP.GatePassDate = Result["GatePassDate"].ToString();
                    objGP.Remarks = (Result["Remarks"] == null ? "" : Result["Remarks"]).ToString();
                    objGP.ExpiryDate = (Result["ExpiryDT"] == null ? "" : Result["ExpiryDT"]).ToString();
                    objGP.CHAName = (Result["CHAName"] == null ? "" : Result["CHAName"]).ToString();
                    objGP.ImpExpName = (Result["ImpExpName"] == null ? "" : Result["ImpExpName"]).ToString();
                    objGP.ShippingLineName = (Result["ShippingLineName"] == null ? "" : Result["ShippingLineName"]).ToString();
                    objGP.BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString();
                    objGP.BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString();
                    objGP.InvoiceDate = (Result["InvoiceDate"] == null ? "" : Result["InvoiceDate"]).ToString();
                    objGP.EntryDate = (Result["EntryDate"] == null ? "" : Result["EntryDate"]).ToString();

                }

                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objGP.lstDet.Add(new PPG_GPDet
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            NoOfUnits = Convert.ToDecimal(Result["NoOfUnits"] == DBNull.Value ? 0 : Result["NoOfUnits"]),
                            VehicleNo = (Result["VehicleNo"] == null ? "" : Result["VehicleNo"]).ToString(),
                            Weight = Convert.ToDecimal(Result["Weight"] == DBNull.Value ? 0 : Result["Weight"]),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            Location = (Result["Location"] == null ? "" : Result["Location"]).ToString(),
                            PortOfDispatch = (Result["PortOfDispatch"] == null ? "" : Result["PortOfDispatch"]).ToString(),
                            Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString(),
                            ICDCode = (Result["ICDCODE"] == null ? "" : Result["ICDCODE"]).ToString(),
                            ShippingLineNo = (Result["ShippingLineNo"] == null ? "" : Result["ShippingLineNo"]).ToString(),
                            CargoDescription = (Result["CargoDescription"] == null ? "" : Result["CargoDescription"]).ToString(),
                            OBLNO = (Result["OBLNO"] == null ? "" : Result["OBLNO"]).ToString(),
                            IGMNo = (Result["IGMNo"] == null ? "" : Result["IGMNo"]).ToString(),
                            InDate = (Result["InDate"] == null ? "" : Result["InDate"]).ToString(),
                            CustomSealNo = (Result["CustomSealNo"] == null ? "" : Result["CustomSealNo"]).ToString(),
                            InvoiceNo = (Result["InvoiceNo"] == null ? "" : Result["InvoiceNo"]).ToString(),
                            IssueSlipNo = (Result["IssueSlipNo"] == null ? "" : Result["IssueSlipNo"]).ToString(),
                            IssueSlipDate = (Result["IssueSlipDate"] == null ? "" : Result["IssueSlipDate"]).ToString(),
                            CreatedTime = (Result["CreatedTime"] == null ? "" : Result["CreatedTime"]).ToString(),
                            DestuffingDate = (Result["DestuffingDate"] == null ? "" : Result["DestuffingDate"]).ToString()
                        });
                    }
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objGP.lstHed.Add(new PPG_Header
                        {
                            InvoiceType = (Result["InvoiceType"] == null ? "" : Result["InvoiceType"]).ToString(),
                            CFSCode = (Result["CFSCode"] == null ? "" : Result["CFSCode"]).ToString()

                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objGP;
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
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void GetServerDate()
        {
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDateTime", CommandType.StoredProcedure);
            string date = "", time = "";
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    date = Convert.ToDateTime(Result["CurrentDate"]).ToString("dd/MM/yyyy");
                    time = Result["CurrentTime"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = new { date = date, time = time };
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
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }
        public void CancelGatePass(int GatePassId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = GatePassId });
            lstParam.Add(new MySqlParameter { ParameterName = "Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("UpdateCancelGP", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Gate Pass Cancelled Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Cannot Cancelled As It Is Used In Another Page";
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

        #region Gate Pass Validity
        public void GetGatePassLstToRevalidate()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            // LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfGatepassToExtend", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Ppg_RevalidateGatePass> LstRevalidateGatePass = new List<Ppg_RevalidateGatePass>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstRevalidateGatePass.Add(new Ppg_RevalidateGatePass
                    {
                        GatePassId = Convert.ToInt32(Result["GatePassId"].ToString()),
                        GatePassNo = Result["GatePassNo"].ToString(),
                        ExpiryDT = Result["ExpiryDT"].ToString(),
                        DeliveryDate = Result["DeliveryDate"].ToString(),
                        ExtendDT = Result["ExtendDate"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstRevalidateGatePass;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void UpdateGatePassValidity(Ppg_RevalidateGatePass objRevalidateGatePass)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = objRevalidateGatePass.GatePassId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassNo", MySqlDbType = MySqlDbType.VarChar, Value = objRevalidateGatePass.GatePassNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExtendDT", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(objRevalidateGatePass.ExtendDT).ToString("yyyy/MM/dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("UpdateGatePassDate", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Gate Pass Expiry Date Updated Successfully";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Gate Pass  Expiry Date Can Not Be Updated ";
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

        public void ListOfGatePassExtend(int Page, string GatepassNo)
        {
            //,int UserId
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_GatepassNo", MySqlDbType = MySqlDbType.VarChar, Value = GatepassNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });

            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListOfGPExtend", CommandType.StoredProcedure, DParam);
            List<Ppg_RevalidateGatePass> lstGP = new List<Ppg_RevalidateGatePass>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstGP.Add(new Ppg_RevalidateGatePass
                    {
                        GatePassNo = Convert.ToString(Result["GatePassNo"]),
                        GatePassDate = Result["GatePassDate"].ToString(),
                        ExpiryDT = Result["OldExpiryDT"].ToString(),
                        ExtendDT = Result["ExtendDT"].ToString(),

                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstGP;
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
            finally
            {
                Result.Close();
                Result.Dispose();
            }
        }


        #endregion


        #region Get CIM-DP Details

        public void GetDPDetails(int GatePassId, string MsgType)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatepassId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = GatePassId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("GetScmtrDpDetails", CommandType.StoredProcedure, DParam);
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

        public void GetCIMDPDetailsUpdateStatus(int GatePassID)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_GatePassID", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = GatePassID });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_MsgType", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = MsgType });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_CrBy", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ((Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("GetCIMDPDetailsUpdateStatus", CommandType.StoredProcedure, DParam);
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
    }
}