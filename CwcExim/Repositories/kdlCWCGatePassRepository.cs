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
    public class kdlCWCGatePassRepository
    {
     


            private DatabaseResponse _DBResponse;
            public DatabaseResponse DBResponse
            {
                get
                {
                    return _DBResponse;

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
        public void ListOfGatePass( int GatePassId)
        {
            //,int UserId
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = GatePassId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
         
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


        public void InvoiceListForGatePass()
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceNoforGP", CommandType.StoredProcedure, DParam);
            List<kdlCWCGatePass> lstInvoice = new List<kdlCWCGatePass>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstInvoice.Add(new kdlCWCGatePass { InvoiceId = Convert.ToInt32(Result["InvoiceId"]), InvoiceNo = Result["InvoiceNo"].ToString(), Module = Result["Module"].ToString(), });
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
        //public void InvoiceListForGatePass(int uid)
        //    {
        //        //int uid
        //        //int uid = Convert.ToInt32(((Login)HttpContext.Current.Session["in_UserId"]).Uid);
        //        List<MySqlParameter> lstParam = new List<MySqlParameter>();
        //        //lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
        //        lstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Value = uid });
        //        IDataParameter[] DParam = lstParam.ToArray();
        //        DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
        //        IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceNoforGP", CommandType.StoredProcedure, DParam);
        //        List<kdlCWCGatePass> lstInvoice = new List<kdlCWCGatePass>();
        //        int Status = 0;
        //        _DBResponse = new DatabaseResponse();
        //        try
        //        {
        //            while (Result.Read())
        //            {
        //                Status = 1;
        //                lstInvoice.Add(new kdlCWCGatePass
        //                {
        //                    InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
        //                    InvoiceNo = Result["InvoiceNo"].ToString(),
        //                    Module = Result["Module"].ToString(),
        //                });
        //            }

        //            if (Status == 1)
        //            {
        //                _DBResponse.Status = 1;
        //                _DBResponse.Message = "Success";
        //                _DBResponse.Data = lstInvoice;
        //            }
        //            else
        //            {
        //                _DBResponse.Status = 0;
        //                _DBResponse.Message = "Error";
        //                _DBResponse.Data = null;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _DBResponse.Status = 0;
        //            _DBResponse.Message = "Error";
        //            _DBResponse.Data = null;
        //        }
        //        finally
        //        {
        //            Result.Close();
        //            Result.Dispose();
        //        }
        //    }
        public void DetailsForGP(int InvoiceId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDetForGP", CommandType.StoredProcedure, DParam);
            List<ContainerDet> lstContainerDet = new List<ContainerDet>();
            string CHAName = "", ImpExpName = "", ShippingLineName = "";
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    CHAName = Result["CHAName"].ToString();
                    ShippingLineName = Result["ShippingLineName"].ToString();
                    ImpExpName = Result["ImpExpName"].ToString();
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
                    _DBResponse.Data = new { lstContainerDet, ImpExpName, ShippingLineName, CHAName };
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

            //public void LoadMoreListOfGatePass(int Page, int UserId)
            //{
            //    //,int UserId
            //    List<MySqlParameter> lstParam = new List<MySqlParameter>();
            //    lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //    lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            //    lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            //    lstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = UserId });
            //    IDataParameter[] DParam = lstParam.ToArray();
            //    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //    IDataReader Result = DataAccess.ExecuteDataReader("LoadMoreListOfGP", CommandType.StoredProcedure, DParam);
            //    List<ListOfGP> lstGP = new List<ListOfGP>();
            //    int Status = 0;
            //    _DBResponse = new DatabaseResponse();
            //    try
            //    {
            //        while (Result.Read())
            //        {
            //            Status = 1;
            //            lstGP.Add(new ListOfGP
            //            {
            //                GatePassId = Convert.ToInt32(Result["GatePassId"]),
            //                GatePassNo = Convert.ToString(Result["GatePassNo"]),
            //                GatePassDate = Result["GatePassDate"].ToString(),
            //                InvoiceNo = Result["InvoiceNo"].ToString(),
            //                IsCancelled = Result["IsCancelled"].ToString()
            //            });
            //        }

            //        if (Status == 1)
            //        {
            //            _DBResponse.Status = 1;
            //            _DBResponse.Message = "Success";
            //            _DBResponse.Data = lstGP;
            //        }
            //        else
            //        {
            //            _DBResponse.Status = 0;
            //            _DBResponse.Message = "Error";
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
            //        Result.Close();
            //        Result.Dispose();
            //    }
            //}
        public void GetDetailsForGatePassPrint(int GatePassId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = GatePassId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGPDetfrPrint", CommandType.StoredProcedure, DParam);
            GPHdr objGP = new GPHdr();
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
                    objGP.CHAName = (Result["CHAName"] == null ? "" : Result["CHAName"]).ToString();
                    objGP.ImpExpName = (Result["ImpExpName"] == null ? "" : Result["ImpExpName"]).ToString();
                    objGP.ShippingLineName = (Result["ShippingLineName"] == null ? "" : Result["ShippingLineName"]).ToString();
                    objGP.BOENo = (Result["BOENo"] == null ? "" : Result["BOENo"]).ToString();
                    objGP.BOEDate = (Result["BOEDate"] == null ? "" : Result["BOEDate"]).ToString();
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        objGP.lstDet.Add(new GPDet
                        {
                            ContainerNo = Result["ContainerNo"].ToString(),
                            NoOfUnits = Convert.ToDecimal(Result["NoOfUnits"] == DBNull.Value ? 0 : Result["NoOfUnits"]),
                            VehicleNo = (Result["VehicleNo"] == null ? "" : Result["VehicleNo"]).ToString(),
                            Weight = Convert.ToDecimal(Result["Weight"] == DBNull.Value ? 0 : Result["Weight"]),
                            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                            Location = (Result["Location"] == null ? "" : Result["Location"]).ToString(),
                            Vessel = (Result["Vessel"] == null ? "" : Result["Vessel"]).ToString(),
                            Voyage = (Result["Voyage"] == null ? "" : Result["Voyage"]).ToString(),
                            LineNo = (Result["LineNo"] == null ? "" : Result["LineNo"]).ToString(),
                            Rotation = (Result["Rotation"] == null ? "" : Result["Rotation"]).ToString()
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
        public void GetDetForGatePass(int GatePassId)
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = GatePassId });
                lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
               // lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
               // lstParam.Add(new MySqlParameter { ParameterName = "in_UserId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = UserId });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                IDataReader Result = DataAccess.ExecuteDataReader("ListOfGP", CommandType.StoredProcedure, DParam);
                kdlCWCGatePass objGP = new kdlCWCGatePass();
                List<Kdl_CWCContainerDet> lstCntnDet = new List<Kdl_CWCContainerDet>();
            
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
                    }
                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                            lstCntnDet.Add(new Kdl_CWCContainerDet
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
                               // PortOfDispatch = Result["PortOfDispatch"].ToString(),
                            });
                        }
                    }
                    if (lstCntnDet.Count > 0)
                    {
                        objGP.StringifyData = JsonConvert.SerializeObject(lstCntnDet);
                    }

                    if (Result.NextResult())
                    {
                        List<KdlCWCGatepassVehicle> lstVch = new List<KdlCWCGatepassVehicle>();
                        while (Result.Read())
                        {
                            lstVch.Add(new KdlCWCGatepassVehicle
                            {
                                Id = Convert.ToInt32(Result["Id"]),
                                GatePassId = Convert.ToInt32(Result["GatePassId"]),
                                ContainerNo = Result["ContainerNo"].ToString(),
                                VehicleNo = Result["VehicleNo"].ToString(),
                                Package = Convert.ToDecimal(Result["Package"]),
                                Weight= Convert.ToDecimal(Result["Weight"]),
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
        public void AddEditGatePass(kdlCWCGatePass objGP, string XML, string VehicleXml)
            {
                List<MySqlParameter> lstParam = new List<MySqlParameter>();
                lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassId", MySqlDbType = MySqlDbType.Int32, Value = objGP.GatePassId });
                lstParam.Add(new MySqlParameter { ParameterName = "in_GatePassDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(objGP.GatePassDate) });
                lstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objGP.Remarks });
                lstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objGP.CHAName });
                lstParam.Add(new MySqlParameter { ParameterName = "in_ImpExpName", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objGP.ImpExpName });
                lstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineName", MySqlDbType = MySqlDbType.VarChar, Size = 500, Value = objGP.ShippingLineName });
                lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = objGP.InvoiceId });
                lstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(((Login)HttpContext.Current.Session["LoginUser"]).Uid) });
                lstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.Text, Value = XML });
                lstParam.Add(new MySqlParameter { ParameterName = "in_VehicleXML", MySqlDbType = MySqlDbType.Text, Value = VehicleXml });
                lstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
                lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                IDataParameter[] DParam = lstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("AddEditGP", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                try
                {
                    if (Result == 1 || Result == 2)
                    {

                        _DBResponse.Status = Result;
                        _DBResponse.Message = (Result == 1) ? "Gate Pass Saved Successfully" : "Gate Pass Updated Successfully";
                        _DBResponse.Data = null;
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

    }



    }
