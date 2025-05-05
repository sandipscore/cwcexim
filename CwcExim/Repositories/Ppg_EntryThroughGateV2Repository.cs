using CwcExim.Areas.GateOperation.Models;
using CwcExim.Areas.Import.Models;
using CwcExim.DAL;

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace CwcExim.Repositories
{
    public class Ppg_EntryThroughGateV2Repository
    {

        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        
        public void AddEditEntryThroughGate(PpgEntryThroughGateV2 ObjEntryThroughGate)
        {
            //if(ObjEntryThroughGate.EntryDateTime != null && ObjEntryThroughGate.EntryDateTime != "")
            //{ }
            //if (ObjEntryThroughGate.ReferenceDate != null && ObjEntryThroughGate.ReferenceDate != "")
            //{ }
            //DateTime EntryDateTm = DateTime.ParseExact(ObjEntryThroughGate.EntryDateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime EntryDateTm =DateTime. ObjEntryThroughGate.EntryDateTime;
            DateTime? ReferenceDt = null;
            var Entrydt = (dynamic)null;
            var SysDateTime = (dynamic)null;
            if (ObjEntryThroughGate.ReferenceDate != null && ObjEntryThroughGate.ReferenceDate != "")
            {
                ReferenceDt = Convert.ToDateTime(ObjEntryThroughGate.ReferenceDate);
            }
            if (ObjEntryThroughGate.EntryDateTime != null && ObjEntryThroughGate.EntryDateTime != "")
            {
                Entrydt = DateTime.ParseExact(ObjEntryThroughGate.EntryDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            }

            if (ObjEntryThroughGate.SystemDateTime != null && ObjEntryThroughGate.SystemDateTime != "")
            {
                SysDateTime = DateTime.ParseExact(ObjEntryThroughGate.SystemDateTime, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            }
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo == null ? "" : ObjEntryThroughGate.ContainerNo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GateInNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.GateInNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryDateTime", MySqlDbType = MySqlDbType.DateTime, Value = Entrydt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ReferenceNo });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ReferenceDate", MySqlDbType = MySqlDbType.DateTime, Value = ReferenceDt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SystemDateTime", MySqlDbType = MySqlDbType.DateTime, Value = SysDateTime });

            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Size = 30, Value = ObjEntryThroughGate.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLine", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ShippingLine });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAId", MySqlDbType = MySqlDbType.Int32, Size = 30, Value = ObjEntryThroughGate.CHAId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CHAName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.CHAName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo1", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = (ObjEntryThroughGate.ContainerNo1 == null ? "" : ObjEntryThroughGate.ContainerNo1) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.VarChar, Size = 4, Value = ObjEntryThroughGate.Size });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Reefer", MySqlDbType = MySqlDbType.Bit, Value = ObjEntryThroughGate.Reefer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CustomSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.CustomSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineSealNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ShippingLineSealNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.VehicleNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChallanNo", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ChallanNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDescription", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.CargoDescription });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoType", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjEntryThroughGate.CargoType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPackages", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.NoOfPackages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GrossWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEntryThroughGate.GrossWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TareWeight", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjEntryThroughGate.TareWeight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DepositorName", MySqlDbType = MySqlDbType.VarChar, Size = 100, Value = ObjEntryThroughGate.DepositorName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Size = 1000, Value = ObjEntryThroughGate.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.ContainerType });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = ObjEntryThroughGate.OperationType });

            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportMode", MySqlDbType = MySqlDbType.Int32, Size = 1, Value = ObjEntryThroughGate.TransportMode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerLoadType", MySqlDbType = MySqlDbType.VarChar, Size = 3, Value = ObjEntryThroughGate.ContainerLoadType });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_TransportFrom", MySqlDbType = MySqlDbType.VarChar, Size = 1, Value = ObjEntryThroughGate.TransportFrom });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CBT", MySqlDbType = MySqlDbType.Int32, Value = ObjEntryThroughGate.IsCBT ? 1 : 0 });//in_OperationType
            LstParam.Add(new MySqlParameter { ParameterName = "in_TPNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ObjEntryThroughGate.TPNo });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Size = 100, Direction = ParameterDirection.Output });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditEntryThroughGateV2", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                   
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "Entry Through Gate Saved Successfully ICD No : "+ ReturnObj + "" : "Entry Through Gate Updated Successfully";
                }
                else if (Result == 3)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Duplicate Container No.";
                }
                else if (Result == 7)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Shipping line and container size should be same as OBL Entry";
                }
                else if (Result == -1)
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = ReturnObj;
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Entry Through Gate Could Not Be Saved";
                }
            }
            catch (Exception ex)
            {
                var file = (dynamic)null;
                string Error = ex.InnerException.Message.ToString();
                string CuurDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("/", "");
                string time = DateTime.Now.ToString("H:mm").Replace(":", "");

                string FolderPath2 = HttpContext.Current.Server.MapPath("~/GateError/EntryError/" + CuurDate);
                if (!System.IO.Directory.Exists(FolderPath2))
                {
                    System.IO.Directory.CreateDirectory(FolderPath2);

                }
                file = Path.Combine(FolderPath2, time + "_ErrorEntry.txt");


                using (var tw = new StreamWriter(file, true))
                {
                    tw.WriteLine(Error);
                    tw.Close();
                }

                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void GetTime()
        {
            int Status = 0;
            //List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            //IDataParameter[] DParam = { };
            //DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDateTime", CommandType.StoredProcedure, null);
            _DBResponse = new DatabaseResponse();
            PpgEntryThroughGateV2 objEntryThroughGate = new PpgEntryThroughGateV2();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objEntryThroughGate.EntryDateTime = Result["CurrentDate"].ToString();
                    objEntryThroughGate.Time = Result["CurrentTime"].ToString();
                    objEntryThroughGate.SystemDateTime = Result["CurrentDate"].ToString();

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void DeleteEntryThroughGate(int EntryId)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Size = 100, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteEnrtyThroughGateV2", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Record Of Entry Through Gate Deleted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    //_DBResponse.Message = "This Invoice could not be deleted. Because other invoice(s) already generated after this invoice.";
                    _DBResponse.Message = GeneratedClientId;
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Record Of Entry Through Gate Could Not Be Deleted";
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


        public void GetAutoPopulateData(string ContainerNumber)
        {
            ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNumber });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllContainerDetailsV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PpgEntryThroughGateV2 ObjEntryThroughGate = new PpgEntryThroughGateV2();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjEntryThroughGate.ReferenceNo = Result["ReferenceNo"].ToString();
                    ObjEntryThroughGate.ReferenceDate = Result["ReferenceDate"].ToString();
                    ObjEntryThroughGate.ShippingLine = Result["ShippingLineName"].ToString();
                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.ContainerNo = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.Size = Result["ContainerSize"].ToString();
                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"]);
                    ObjEntryThroughGate.EntryDateTime = Convert.ToDateTime(Result["EntryDateTime"]).ToString("dd/MM/yyyy");
                    ObjEntryThroughGate.EntryTime = Result["Entime"].ToString();
                    ObjEntryThroughGate.CargoDescription = Result["CargoDesc"].ToString();
                    ObjEntryThroughGate.Reefer = Convert.ToBoolean(Result["Reefer"].ToString());
                    if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 1)
                    {
                        ObjEntryThroughGate.CustomSealNo = Convert.ToString(Result["SealNo"]);
                    }
                    else if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 3)
                    {
                        ObjEntryThroughGate.CustomSealNo = Convert.ToString(Result["SealNo"]);
                        //ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GrossWt"]);
                        ObjEntryThroughGate.ContainerLoadType = Result["LoadType"].ToString();
                        ObjEntryThroughGate.ShippingLineSealNo = Result["SLineNo"].ToString();
                        ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"] == DBNull.Value ? 0 : Result["ShippingLineId"]);
                    }
                    //EntryThroughGate objEntryThroughGate = new EntryThroughGate();
                    // objEntryThroughGate = (EntryThroughGate)ObjGOR.DBResponse.Data;
                    //string strDate = ObjEntryThroughGate.ReferenceDate;
                    //string[] arrayDate = strDate.Split(' ');
                    //ObjEntryThroughGate.ReferenceDate = arrayDate[0];
                    ObjEntryThroughGate.EntryTime = ObjEntryThroughGate.EntryTime;

                    //ViewBag.strTime = objEntryThroughGate.EntryTime;



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void GetAutoPopulateDataByTrain(string ContainerNumber)
        {
            ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNumber });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllContainerDetailsByTrainV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PpgEntryThroughGateV2 ObjEntryThroughGate = new PpgEntryThroughGateV2();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjEntryThroughGate.ReferenceNo = Result["ReferenceNo"].ToString();
                    ObjEntryThroughGate.ReferenceDate = Result["ReferenceDate"].ToString();
                    ObjEntryThroughGate.ShippingLine = Result["ShippingLineName"].ToString();
                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.ContainerNo = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.Size = Result["ContainerSize"].ToString();
                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"]);
                    ObjEntryThroughGate.EntryDateTime = Convert.ToDateTime(Result["EntryDateTime"]).ToString("dd/MM/yyyy");
                    ObjEntryThroughGate.EntryTime = Result["Entime"].ToString();
                    ObjEntryThroughGate.CargoDescription = Result["CargoDesc"].ToString();
                    ObjEntryThroughGate.Reefer = Convert.ToBoolean(Convert.ToInt32(Result["Reefer"]));
                    //if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 1)
                    //{
                    //    ObjEntryThroughGate.CustomSealNo = Convert.ToString(Result["SealNo"]);
                    //}
                    //else if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 3)
                    //{
                    ObjEntryThroughGate.CustomSealNo = Convert.ToString(Result["SealNo"]);
                    ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GrossWt"]);
                    ObjEntryThroughGate.ContainerLoadType = Result["LoadType"].ToString();
                    ObjEntryThroughGate.ShippingLineSealNo = Result["SLineNo"].ToString();
                    ObjEntryThroughGate.TransportFrom = Result["TranFrom"].ToString();
                    ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    ObjEntryThroughGate.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"]);
                    ObjEntryThroughGate.Remarks = Convert.ToString(Result["Remarks"]);
                    //}
                    //EntryThroughGate objEntryThroughGate = new EntryThroughGate();
                    // objEntryThroughGate = (EntryThroughGate)ObjGOR.DBResponse.Data;
                    //string strDate = ObjEntryThroughGate.ReferenceDate;
                    //string[] arrayDate = strDate.Split(' ');
                    //ObjEntryThroughGate.ReferenceDate = arrayDate[0];
                    ObjEntryThroughGate.EntryTime = ObjEntryThroughGate.EntryTime;

                    //ViewBag.strTime = objEntryThroughGate.EntryTime;



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void ListOfShippingLinePartyCode(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLineForJO", CommandType.StoredProcedure, Dparam);
            IList<ShippingLineForPageV2> lstShippingLine = new List<ShippingLineForPageV2>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstShippingLine.Add(new ShippingLineForPageV2
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

        public void GetLoadedContData(int LoadContainerRefId)
        {
            //ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LoadAppId", MySqlDbType = MySqlDbType.Int32, Value = LoadContainerRefId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("DetailsForLoadedContainerV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<LoadedContainerListWithDataV2> lstLoadedContainerListWithData = new List<LoadedContainerListWithDataV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstLoadedContainerListWithData.Add(new LoadedContainerListWithDataV2
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        ShippingLineId = Result["ShippingLineId"].ToString(),
                        ShippingLineName = Result["shippingLine"].ToString(),
                        Size = Result["Size"].ToString(),
                        Reefer = Result["Reefer"].ToString(),
                        CargoType = Result["CargoType"].ToString(),
                        CargoDescription = Result["CargoDescription"].ToString(),
                        NoOfUnits = Result["NoOfUnits"].ToString(),
                        GrossWt = Result["GrossWt"].ToString()

                    });


                    //ViewBag.strTime = objEntryThroughGate.EntryTime;



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstLoadedContainerListWithData;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetShippingLineForGateEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PpgEntryThroughGateShippingV2> lstShippingList = new List<PpgEntryThroughGateShippingV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstShippingList.Add(new PpgEntryThroughGateShippingV2
                    {
                        ShippingLineId = Convert.ToInt32(Result["EximTraderId"]),
                        ShippingLine = Convert.ToString(Result["EximTraderName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstShippingList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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
        public void ListOfCHA()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "In_BranchId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetCHAForGateEntry", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<PpgEntryThroughGateCHAV2> lstCHAList = new List<PpgEntryThroughGateCHAV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstCHAList.Add(new PpgEntryThroughGateCHAV2
                    {
                        CHAId = Convert.ToInt32(Result["EximTraderId"]),
                        CHAName = Convert.ToString(Result["EximTraderName"]),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstCHAList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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
        public void GetAllEntryThroughGate()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "tmode", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            LstParam.Add(new MySqlParameter { ParameterName = "cbt", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Empty", MySqlDbType = MySqlDbType.VarChar, Value = null });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGateV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PpgEntryThroughGateV2> LstEntryThroughGate = new List<PpgEntryThroughGateV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new PpgEntryThroughGateV2
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DisPlayCfs = Result["DisPlayCfs"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void GetAllEntryThroughGateListPage(int Page,string OperationType, string ContainerType)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "tmode", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            LstParam.Add(new MySqlParameter { ParameterName = "cbt", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Empty", MySqlDbType = MySqlDbType.VarChar, Value = null });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = OperationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Value = ContainerType });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGateListPageV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PpgEntryThroughGateV2> LstEntryThroughGate = new List<PpgEntryThroughGateV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new PpgEntryThroughGateV2
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DisPlayCfs = Result["DisPlayCfs"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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
        public void GetAllEntryThroughGateTrain()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "tmode", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "cbt", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Empty", MySqlDbType = MySqlDbType.VarChar, Value = null });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGateV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PpgEntryThroughGateV2> LstEntryThroughGate = new List<PpgEntryThroughGateV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new PpgEntryThroughGateV2
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DisPlayCfs = Result["DisPlayCfs"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void GetAllEntryThroughTrainListPage(int Page, string OperationType, string ContainerType)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "tmode", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "cbt", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Empty", MySqlDbType = MySqlDbType.VarChar, Value = null });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = OperationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Value = ContainerType });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGateListPageV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PpgEntryThroughGateV2> LstEntryThroughGate = new List<PpgEntryThroughGateV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new PpgEntryThroughGateV2
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DisPlayCfs = Result["DisPlayCfs"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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


        public void GetAllEntryThroughGateSearchListPage(int Page, string OperationType, string ContainerType, string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "tmode", MySqlDbType = MySqlDbType.Int32, Value = 2 });
            LstParam.Add(new MySqlParameter { ParameterName = "cbt", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Empty", MySqlDbType = MySqlDbType.VarChar, Value = null });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = OperationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Value = ContainerType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            IDataReader Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGateListPageV2Search", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PpgEntryThroughGateV2> LstEntryThroughGate = new List<PpgEntryThroughGateV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new PpgEntryThroughGateV2
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DisPlayCfs = Result["DisPlayCfs"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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
        public void GetAllEntryThroughGateSearchTrainListPage(int Page, string OperationType, string ContainerType, string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "tmode", MySqlDbType = MySqlDbType.Int32, Value = 1 });
            LstParam.Add(new MySqlParameter { ParameterName = "cbt", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Empty", MySqlDbType = MySqlDbType.VarChar, Value = null });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = OperationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Value = ContainerType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result;
            
                Result = DataAccess.ExecuteDataReader("GetAllEntryThroughGateListPageV2Search", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<PpgEntryThroughGateV2> LstEntryThroughGate = new List<PpgEntryThroughGateV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstEntryThroughGate.Add(new PpgEntryThroughGateV2
                    {
                        CFSCode = Result["CFSCode"].ToString(),
                        DisPlayCfs = Result["DisPlayCfs"].ToString(),
                        GateInNo = Result["GateInNo"].ToString(),
                        EntryDateTime = Result["EntryDateTime"].ToString(),
                        ReferenceNo = Result["ReferenceNo"].ToString(),
                        EntryId = Convert.ToInt32(Result["EntryId"]),
                        OperationType = Result["OperationType"].ToString(),
                        ContainerType = Result["ContainerType"].ToString(),
                        VehicleNo = Result["VehicleNo"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"]),
                        InvoiceNo = Result["InvoiceNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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




        public void GetContainer()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllContainerV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<containerV2> Lstcontainer = new List<containerV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstcontainer.Add(new containerV2
                    {
                        ContainerName = Result["ContainerNo"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lstcontainer;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void GetContainerByTrain()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetAllContainerByTrainV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<containerV2> Lstcontainer = new List<containerV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Lstcontainer.Add(new containerV2
                    {
                        ContainerName = Result["ContainerNo"].ToString()

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Lstcontainer;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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
        public void GetReferenceNumber()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetReferenceNumberExportEmptyV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            ReferenceNumberCCINV2 ObjReferenceNumber = new ReferenceNumberCCINV2();

            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjReferenceNumber.ReferenceList.Add(new CCINListV2
                    {
                        CCINId = int.Parse(Convert.ToString(Result["Id"])),
                        CCINNo = Convert.ToString(Result["CCINNo"]),
                        CCINDate = Convert.ToString(Result["CCINDate"]),
                        ShippingLineId = int.Parse(Convert.ToString(Result["ShippingLineId"])),
                        ShippingLine = Result["ShippingLine"].ToString(),
                        NoOfUnits = Convert.ToString(Result["Package"]),
                        Weight = Convert.ToString(Result["Weight"]),
                        CargoType = Convert.ToInt32(Result["CargoType"]),
                        CHAId = int.Parse(Convert.ToString(Result["CHAId"])),
                        CHAName = Result["CHA"].ToString(),
                    });

                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        ObjReferenceNumber.listExport.Add(new GateEntryExport()
                //        {
                //            ReferenceNo = Convert.ToString(Result["ReferenceNo"]),
                //            ReferenceDate = Convert.ToString(Result["ReferenceDate"]),
                //            CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]),
                //            CargoDescription = Convert.ToString(Result["CargoDescription"])

                //        });
                //    }
                //}
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Status = 1;
                        ObjReferenceNumber.listShippingLine.Add(new ShippingLineListV2()
                        {
                            ShippingLine = Convert.ToString(Result["ShippingLine"]),
                            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"])


                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjReferenceNumber;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        #region Gateopration
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
            List<PPGPickupModelV2> LstPickUp = new List<PPGPickupModelV2>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstPickUp.Add(new PPGPickupModelV2
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
        #endregion
        
        public void GetLoadContainerRefNUmber()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GEtListOfLoadedContainerV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            //LoadedContainer ObjLoadedContainer = new LoadedContainer();
            List<LoadContainerReferenceNumberListV2> LoadContainerReferenceLst = new List<LoadContainerReferenceNumberListV2>();

            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LoadContainerReferenceLst.Add(new LoadContainerReferenceNumberListV2
                    {
                        LoadContReqId = int.Parse(Convert.ToString(Result["LoadContReqId"])),
                        LoadContReqNo = Convert.ToString(Result["LoadContReqNo"]),
                        LoadContReqDate = Convert.ToString(Result["LoadContReqDate"])
                    });

                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        ObjLoadedContainer.listShippingLine.Add(new ShippingLineList()
                //        {
                //            ShippingLine = Convert.ToString(Result["ShippingLine"]),
                //            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"])


                //        });
                //    }
                //}
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LoadContainerReferenceLst;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void GetShippingLineLoadCont()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //  LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ShippingLineList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            //LoadedContainer ObjLoadedContainer = new LoadedContainer();
            List<ShippingLineListV2> ShippingLineLst = new List<ShippingLineListV2>();

            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ShippingLineLst.Add(new ShippingLineListV2
                    {
                        ShippingLine = Convert.ToString(Result["ShippingLine"]),
                        ShippingLineId = Convert.ToInt32(Result["ShippingLineId"])
                    });

                }
                //if (Result.NextResult())
                //{
                //    while (Result.Read())
                //    {
                //        ObjLoadedContainer.listShippingLine.Add(new ShippingLineList()
                //        {
                //            ShippingLine = Convert.ToString(Result["ShippingLine"]),
                //            ShippingLineId = Convert.ToInt32(Result["ShippingLineId"])


                //        });
                //    }
                //}
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ShippingLineLst;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void GetEntryThroughGate(int EntryId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryId", MySqlDbType = MySqlDbType.Int32, Value = EntryId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ViewEditEntryThroughGateV2", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            PpgEntryThroughGateV2 ObjEntryThroughGate = new PpgEntryThroughGateV2();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjEntryThroughGate.EntryId = Convert.ToInt32(Result["EntryId"]);
                    ObjEntryThroughGate.ContainerNo = Result["ContainerNo"].ToString();
                    ObjEntryThroughGate.CFSCode = Result["CFSCode"].ToString();
                    ObjEntryThroughGate.GateInNo = Result["GateInNo"].ToString();
                    //ObjEntryThroughGate.EntryDateTime = Result["EntryDateTime"].ToString();
                    ObjEntryThroughGate.EntryDateTime = Convert.ToString(Result["EntryDateTime"] == null ? "" : Result["EntryDateTime"]);
                    ObjEntryThroughGate.SystemDateTime = Convert.ToString(Result["SystemDateTime"]);
                    ObjEntryThroughGate.ReferenceNo = Result["ReferenceNo"].ToString();
                    //ObjEntryThroughGate.ReferenceDate= Result["ReferenceDate"].ToString();
                    ObjEntryThroughGate.ReferenceDate = Convert.ToString(Result["ReferenceDate"] == null ? "" : Result["ReferenceDate"]);
                    ObjEntryThroughGate.ShippingLine = Result["ShippingLine"].ToString();
                    ObjEntryThroughGate.ShippingLineId = Convert.ToInt32(Result["ShippingLineId"]);
                    ObjEntryThroughGate.CHAName = Result["CHAName"].ToString();
                    ObjEntryThroughGate.ContainerNo1 = (Result["ContainerNo1"] == null ? "" : Result["ContainerNo1"]).ToString();


                    ObjEntryThroughGate.Size = Result["Size"].ToString();
                    //ObjEntryThroughGate.Reefer =Convert.ToBoolean(Result["Reefer"].ToString());

                    ObjEntryThroughGate.Reefer = Convert.ToBoolean(Result["Reefer"] == DBNull.Value ? 0 : Result["Reefer"]);

                    ObjEntryThroughGate.CustomSealNo = Result["CustomSealNo"].ToString();
                    ObjEntryThroughGate.ShippingLineSealNo = Result["ShippingLineSealNo"].ToString();
                    ObjEntryThroughGate.VehicleNo = Result["VehicleNo"].ToString();
                    ObjEntryThroughGate.ChallanNo = Result["ChallanNo"].ToString();

                    ObjEntryThroughGate.CargoType = Convert.ToInt32(Result["CargoType"] == DBNull.Value ? 0 : Result["CargoType"]);
                    ObjEntryThroughGate.NoOfPackages = Convert.ToInt32(Result["NoOfPackages"] == DBNull.Value ? 0 : Result["NoOfPackages"]);
                    ObjEntryThroughGate.ActualPackages = Convert.ToInt32(Result["ActualPackages"] == DBNull.Value ? 0 : Result["ActualPackages"]);

                    ObjEntryThroughGate.GrossWeight = Convert.ToDecimal(Result["GrossWeight"].ToString());
                    ObjEntryThroughGate.TareWeight = Convert.ToDecimal(Result["TareWeight"].ToString());
                    ObjEntryThroughGate.DepositorName = Result["DepositorName"].ToString();
                    ObjEntryThroughGate.Remarks = Result["Remarks"].ToString();
                    ObjEntryThroughGate.CargoDescription = Result["CargoDescription"].ToString();
                    ObjEntryThroughGate.ContainerType = Result["ContainerType"].ToString();
                    ObjEntryThroughGate.OperationType = Result["OperationType"].ToString();
                    ObjEntryThroughGate.TransportMode = Convert.ToInt32(Result["TransportMode"].ToString());
                    ObjEntryThroughGate.TransportFrom = Result["TransportFrom"].ToString();
                    ObjEntryThroughGate.ContainerLoadType = Result["ContainerLoadType"].ToString();
                    ObjEntryThroughGate.InvoiceId = Convert.ToInt32(Result["InvoiceId"]);
                    ObjEntryThroughGate.TPNo = Result["TPNo"].ToString();
                    ObjEntryThroughGate.IsCBT = Convert.ToInt32(Result["CBT"]) == 1 ? true : false;
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjEntryThroughGate;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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
        
        public void EntryMailStatus(string containerId, int lastInsertedId)
        {
            //string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_containerId", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = containerId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_lastInsertedId", MySqlDbType = MySqlDbType.Int32, Value = lastInsertedId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            // LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("EntryMailStatus", CommandType.StoredProcedure, DParam/*, out GeneratedClientId*/);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Email Of Entry Through Gate Sent Successfully";
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
       
        public void GetDetailsForGateEntryMail(string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = ContainerNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDetailsForGateEntryMail", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            EntryThroughGateMail objMail = new EntryThroughGateMail();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objMail.Email = Convert.ToString(Result["Email"]);
                    objMail.FileName = Convert.ToString(Result["FileName"]);
                    if (Result.NextResult())
                    {
                        while (Result.Read())
                        {
                            objMail.lstExcelData.Add(new ExcelData()
                            {
                                Line = Convert.ToString(Result["Line"]),
                                ContainerNumber = Convert.ToString(Result["ContainerNo"]),
                                Size = Convert.ToString(Result["Size"]),
                                MoveCode = Convert.ToString(Result["MoveCode"]),
                                EntryDateTime = Convert.ToString(Result["EntryDateTime"]),
                                CurrentLocation = Convert.ToString(Result["CurrentLocation"]),
                                ToLocation = Convert.ToString(Result["ToLocation"]),
                                BookingRefNo = Convert.ToString(Result["BookingReferenceNo"]),
                                Customer = Convert.ToString(Result["Customer"]),
                                Transporter = Convert.ToString(Result["Transporter"]),
                                TruckNumber = Convert.ToString(Result["TruckNumber"]),
                                Condition = Convert.ToString(Result["Conditn"]),
                                ReportedBy = Convert.ToString(Result["ReportedBy"]),
                                ReportDate = Convert.ToString(Result["ReportDate"]),
                                Remarks = Convert.ToString(Result["Remarks"]),
                                TransportMode = Convert.ToString(Result["TransportMode"]),
                                JobOrder = Convert.ToString(Result["JobOrderNo"])
                            });
                        }
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objMail;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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


        // Suman Das 07-09-2024
        #region ELWB Weight Capture

        public void GetELWBWeightCapContNo(string ContainerNo = null)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.VarChar, Value = ContainerNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetELWBWCContaiNoList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<WCContList> ContLst = new List<WCContList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ContLst.Add(new WCContList
                    {
                        ContainerNo = Result["ContainerNo"].ToString(),
                        CFSCode = Result["CFSCode"].ToString(),
                        CustomerName = Result["ShippingLine"].ToString(),
                        Material = Result["Material"].ToString(),
                        Remarks = Result["Remarks"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ContLst;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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

        public void GetELWBWeightCaptureList(string RefNo = null)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_RefNo", MySqlDbType = MySqlDbType.VarChar, Value = RefNo });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetELWBWeightCapture", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<WeightCaptureList> WClst = new List<WeightCaptureList>();
            List<ELWBWCVechicleNoList> Vlst = new List<ELWBWCVechicleNoList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    WClst.Add(new WeightCaptureList
                    {
                        ELEWId = Result["REFERANCENO"].ToString(),
                        RefNo = Result["REFERANCENO"].ToString(),
                        Date = Convert.ToString(Result["REFERANCEDATE"]),
                        ContNo = Result["CONTAINERNO"].ToString(),
                        CFSCode = Result["CFSCODE"].ToString(),
                        VeichelNoP = Result["VEHICLENOPRT1"].ToString(),
                        VeichelNoC = Result["VEHICLENOPRT2"].ToString(),
                        Weight = Convert.ToString(Result["WEIGHTFOR"]),
                        WeightInKg = Convert.ToString(Result["WEIGHT"]),
                        Time = Convert.ToString(Result["RefaranceTime"]),
                        Material = Convert.ToString(Result["MATERIAL"]),
                        CustomerName = Convert.ToString(Result["CUSTOMER_NAME"]),
                        Remarks = Convert.ToString(Result["REMARKS"])
                    });
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        Vlst.Add(new ELWBWCVechicleNoList
                        {
                            VecholNo = Result["VehicleNo"].ToString()
                        });
                    }
                    WClst[0].VechNoList = Vlst;
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = WClst;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
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

        public void AddEditELWBWeightCapture(WeightCapture Obj, int Uid)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_RefNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.ELEWId == null ? "0" : Obj.ELEWId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RefNoDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(Obj.RefNoDate).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RefaranceTime", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Time });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContNo", MySqlDbType = MySqlDbType.VarChar, Value = Obj.ContNo == null ? null : Obj.ContNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.VarChar, Value = Obj.CFSCode });

            LstParam.Add(new MySqlParameter { ParameterName = "in_VeichelNoP", MySqlDbType = MySqlDbType.VarChar, Value = Obj.VeichelNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VeichelNoC", MySqlDbType = MySqlDbType.VarChar, Value = (Obj.VeichelNoOther == null ? Obj.VeichelNoList : Obj.VeichelNoOther) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_WeightFor", MySqlDbType = MySqlDbType.VarChar, Value = Obj.WeightFor });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Obj.WeightInKg == null ? 0 : Convert.ToDecimal(Obj.WeightInKg) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_MATERIAL", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Material });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CUSTOMER_NAME", MySqlDbType = MySqlDbType.VarChar, Value = Obj.CustomerName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_REMARKS", MySqlDbType = MySqlDbType.VarChar, Value = Obj.Remarks });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });

            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditELWBWeightCapture", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1 || Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = (Result == 1) ? "ELWB Weight Capture Saved Successfully" : "ELWB Weight Capture Updated Successfully";
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

                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }

        public void GetELWBWCVechicleNoList(string VehicleNo = null)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_VehicleNo", MySqlDbType = MySqlDbType.VarChar, Value = VehicleNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetELWBWCVechicleNoList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<ELWBWCVechicleNoList> ContLst = new List<ELWBWCVechicleNoList>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ContLst.Add(new ELWBWCVechicleNoList
                    {
                        VecholNo = Result["VehicleNo"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ContLst;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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

        public void DeleteELWBWeightCapEntry(string ELWBWeightCapID)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_CaptureId", MySqlDbType = MySqlDbType.VarChar, Value = ELWBWeightCapID });
            lstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = lstParam.ToArray();
            int Result = DA.ExecuteNonQuery("DeleteELWBWeightCaptureDet", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = null;
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
        }

        public void ELWBWeightCapPrint(string RefNo = null)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_RefNo", MySqlDbType = MySqlDbType.VarChar, Value = RefNo });
            //DParam = LstParam.ToArray();
            //IDataReader Result = DataAccess.ExecuteDataSet("GetELWBWeightPrint", CommandType.StoredProcedure, DParam);
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetELWBWeightPrint", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[0].Rows.Count > 0)
                {
                    Status = 1;
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Result;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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
    }
}