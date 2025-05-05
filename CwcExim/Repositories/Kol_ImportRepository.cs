using CwcExim.Areas.Import.Models;
using CwcExim.DAL;
using EinvoiceLibrary;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CwcExim.Repositories
{
    public class Kol_ImportRepository
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
        public void AddEditFormOne(Kol_FormOneModel objFormOne, int BranchId, string FormOneDetailXML, int CreatedBy)
        {
            string Status = "0";
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.FormOneId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BLNo", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.BLNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLineId", MySqlDbType = MySqlDbType.Int32, Value = objFormOne.ShippingLineId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_VesselName", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.VesselName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ForeignLiner", MySqlDbType = MySqlDbType.VarChar, Value = objFormOne.FlinerName });

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

        public void ListOfForeignLiner()
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
          //  LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Value = "ShippingLine" });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfForeignLiner", CommandType.StoredProcedure);
            IList<ForeignLiner> lstForeignLiner = new List<ForeignLiner>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstForeignLiner.Add(new ForeignLiner
                    {
                       
                        ForeignLinerName = result["ForeignLinerName"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstForeignLiner;
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
            IList<Commodity> lstCommodity = new List<Commodity>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstCommodity.Add(new Commodity
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
        public void GetFormOne(int Page)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = Page });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFormOne", CommandType.StoredProcedure, DParam);
            IList<Kol_FormOneModel> lstFormOne = new List<Kol_FormOneModel>();
            int Status = 0;
             
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new Kol_FormOneModel
                    {
                        FormOneId = Convert.ToInt32(result["FormOneId"]),
                        FormOneNo = Convert.ToString(result["FormOneNo"]),
                        FormOneDate = Convert.ToString(result["FormOneDate"]),
                        ShippingLineName = Convert.ToString(result["EximTraderName"]),
                        TrBondNo= Convert.ToString(result["TrBondNo"])
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

        //public void GetFormOneGrid(int page)
        //{
        //    List<MySqlParameter> LstParam = new List<MySqlParameter>();
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
        //    LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
        //    IDataParameter[] DParam = { };
        //    DParam = LstParam.ToArray();
        //    DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
        //    IDataReader result = DA.ExecuteDataReader("GetFormOneGrid", CommandType.StoredProcedure, DParam);
        //    IList<Kol_FormOneModel> lstFormOne = new List<Kol_FormOneModel>();
        //    int Status = 0;

        //    _DBResponse = new DatabaseResponse();
        //    try
        //    {
        //        while (result.Read())
        //        {
        //            Status = 1;
        //            lstFormOne.Add(new Kol_FormOneModel
        //            {
        //                FormOneId = Convert.ToInt32(result["FormOneId"]),
        //                FormOneNo = Convert.ToString(result["FormOneNo"]),
        //                FormOneDate = Convert.ToString(result["FormOneDate"]),
        //                ShippingLineName = Convert.ToString(result["EximTraderName"]),
        //                TrBondNo = Convert.ToString(result["TrBondNo"])
        //            });
        //        }
        //        if (Status == 1)
        //        {
        //            _DBResponse.Data = lstFormOne;
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


        public void GetFormOneByContainer(string ContainerName)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_containerNo", MySqlDbType = MySqlDbType.VarChar, Size = 11, Value = ContainerName });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFormBYContainer", CommandType.StoredProcedure, DParam);
            IList<Kol_FormOneModel> lstFormOne = new List<Kol_FormOneModel>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstFormOne.Add(new Kol_FormOneModel
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFormOne", CommandType.StoredProcedure, DParam);
            Kol_FormOneModel objFormOne = new Kol_FormOneModel();
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
                    objFormOne.FlinerName = Convert.ToString(result["ForeignLinerName"]);
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
                        objFormOne.lstFormOneDetail.Add(new Kol_FormOneDetailModel()
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
        public void FormOnePrint(int FormOneId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_FormOneId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = FormOneId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("FormOnePrint", CommandType.StoredProcedure, DParam);
            Kol_FormOnePrintModel objFormOne = new Kol_FormOnePrintModel();
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
                    objFormOne.BLNo = result["BLNo"].ToString();
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objFormOne.lstFormOnePrintDetail.Add(new Kol_FormOnePrintDetailModel()
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
        #region getAppraisemnt For invoice
        public void GetInvoiceList()
        {
            //string FromDate, string ToDate
            //DateTime dtfrom = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            //DateTime dtTo = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //String PeriodTo = dtTo.ToString("yyyy/MM/dd");


            int Status = 0;

            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodFrom", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PeriodTo", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });



            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("InvoiceWithAppraisement", CommandType.StoredProcedure, DParam);
            IList<AllInvoiceListForappraisemnt> LstInvoice = new List<AllInvoiceListForappraisemnt>();
            // CollectionReportTotal objCollectionReportTotal = new CollectionReportTotal();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstInvoice.Add(new AllInvoiceListForappraisemnt
                    {


                        invoiceId=Convert.ToInt32(Result["InvoiceId"] == DBNull.Value?0: Result["InvoiceId"]),
                        InvoiceNumber = Result["InvoiceNo"].ToString(),
                        StuffingReqId= Result["StuffingReqId"].ToString(),
                        StuffingReqNo = Result["StuffingReqNo"].ToString(),
                        StuffingReqDate = Result["StuffingReqDate"].ToString()
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data =LstInvoice ;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        #region InvoiceType

        public void GetInvoiceType(string InvoiceNo)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetInvoiceType", CommandType.StoredProcedure, DParam);
            string InvoiceType = "";
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;

                    InvoiceType = Convert.ToString(result["InvoiceTypes"]);
                   
                }
                if (Status == 1)
                {
                    _DBResponse.Data = InvoiceType;
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

        #region IRN
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
                            PrdDesc = null,//Result["PrdDesc"].ToString(),
                            IsServc = Result["IsServc"].ToString(),
                            HsnCd = Result["HsnCd"].ToString(),
                            Barcde = Result["Barcde"].ToString(),
                            Qty = Convert.ToDecimal(Result["Qty"].ToString()),
                            FreeQty = Convert.ToInt32(Result["FreeQty"].ToString()),
                            Unit = null, // Result["Unit"].ToString(),
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
                    Obj.TotInvVal = Convert.ToDecimal(Result["TotInvVal"]);
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
            lstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = ((Models.Login)System.Web.HttpContext.Current.Session["LoginUser"]).Uid });

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

        #region Empty Container Payment Sheet

        public void AddEditEmptyContPaymentSheet(KolInvoiceYard ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, string OperationCfsCodeWiseAmtXML, string ChargesBreakupXML,
            int BranchId, int Uid,
           string Module, string SEZ, string CargoXML = "")
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


            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Value = SEZ });

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
        public void GetEmptyContPaymentSheet(string InvoiceDate, int DestuffingAppId, string InvoiceType, string ContainerXML, int InvoiceId, string InvoiceFor, int PartyId, string SEZ)
        {


            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 20, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceFor", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = InvoiceFor });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Size = 20, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SEZ", MySqlDbType = MySqlDbType.VarChar, Size = 20, Value = SEZ });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            KolInvoiceYard objInvoice = new KolInvoiceYard();
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
                    objInvoice.lstPrePaymentCont.Add(new KOLPreInvoiceContainer
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
                    objInvoice.lstPostPaymentCont.Add(new KOLPostPaymentContainer
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
                    objInvoice.lstPostPaymentChrg.Add(new KOLPostPaymentChrg
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
                    objInvoice.lstContWiseAmount.Add(new KOLContainerWiseAmount
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
                    objInvoice.lstOperationCFSCodeWiseAmount.Add(new KOLOperationCFSCodeWiseAmount
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
                    objInvoice.lstPostPaymentChrgBreakup.Add(new KOLPostPaymentChargebreakupdate
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
        public void GetBOLForEmptyCont(string InvoiceFor, int DestuffingId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceFor });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingId });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetBOLForEmpCont", CommandType.StoredProcedure, dparam);
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            var BOL = "";
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    BOL = result["BOLNo"].ToString() + ":" + result["BOLDate"].ToString();
                }
                if (Status == 1)
                {
                    _DBResponse.Data = BOL;
                    _DBResponse.Message = "Success";
                    _DBResponse.Status = 1;
                }
                else
                {
                    _DBResponse.Data = BOL;
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

        public void GetImpPaymentPartyForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPartyForPage", CommandType.StoredProcedure, Dparam);
            IList<WFLDImpPartyForpage> lstParty = new List<WFLDImpPartyForpage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new WFLDImpPartyForpage
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]).Replace("\r\n", ""),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        PartyCode = Convert.ToString(Result["PartyCode"]),


                        IsTransporter = Convert.ToBoolean(Result["IsTransporter"]),
                        InsuredFrmDate = Convert.ToString(Result["InsuredFrmDate"]),
                        InsuredToDate = Convert.ToString(Result["InsuredToDate"]),
                        IsInsured = Convert.ToBoolean(Result["IsInsured"])
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

        public void GetImpPaymentPayeeForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetImpPaymentPayerForPage", CommandType.StoredProcedure, Dparam);
            IList<WFLDImpPartyForpage> lstParty = new List<WFLDImpPartyForpage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new WFLDImpPartyForpage
                    {
                        PartyId = Convert.ToInt32(Result["CHAId"]),
                        PartyName = Convert.ToString(Result["CHAName"]),
                        Address = Convert.ToString(Result["Address"]).Replace("\r\n", ""),
                        State = Convert.ToString(Result["StateName"]),
                        StateCode = Convert.ToString(Result["StateCode"]),
                        GSTNo = Convert.ToString(Result["GSTNo"]),
                        PartyCode = Convert.ToString(Result["PartyCode"]),


                        IsTransporter = Convert.ToBoolean(Result["IsTransporter"]),
                        InsuredFrmDate = Convert.ToString(Result["InsuredFrmDate"]),
                        InsuredToDate = Convert.ToString(Result["InsuredToDate"]),
                        IsInsured = Convert.ToBoolean(Result["IsInsured"])
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
        #endregion

        public void ListOfExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null, int Page = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            if (InvoiceDate != null) InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("yyyy-MM-dd");
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "IN_InvoiceDate", MySqlDbType = MySqlDbType.Date, Value = InvoiceDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.VarChar, Size = 30, Value = Page });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("ListofexpInvoice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            IList<CwcExim.Areas.Export.Models.WFLDListOfExpInvoice> lstExpInvoice = new List<CwcExim.Areas.Export.Models.WFLDListOfExpInvoice>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstExpInvoice.Add(new CwcExim.Areas.Export.Models.WFLDListOfExpInvoice()
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


        #region --FOIS Data--
        public void GetInvoiceNoList()
        {
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetInvoiceNoList", CommandType.StoredProcedure, DParam);
            IList<AllInvoiceList> LstInvoice = new List<AllInvoiceList>();           
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    LstInvoice.Add(new AllInvoiceList
                    {
                        InvoiceId = Convert.ToInt32(Result["InvoiceId"].ToString()),
                        InvoiceNo = Result["InvoiceNo"].ToString()                        
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstInvoice;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
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

        public void GetFoisData(string InvoiceNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_invcid", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceNo });            
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetFoisData", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            RakeWagonHdr Objfd = new RakeWagonHdr();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    Objfd.RWHdrId = (Result["RWHdrId"] == null ? 0 : Convert.ToInt32(Result["RWHdrId"]));
                    Objfd.InvoiceNo = (Result["invcid"] == null ? "" : Result["invcid"]).ToString();
                    Objfd.RakeId = (Result["RakeId"] == null ? "" : Result["RakeId"]).ToString();
                    Objfd.StationFrom = (Result["sttnfrom"] == null ? "" : Result["sttnfrom"]).ToString();
                    Objfd.StationTO = (Result["sttnto"] == null ? "" : Result["sttnto"]).ToString();                   
                    Objfd.NoOfWagon = (Result["noofwgon"] == null ? "" : Result["noofwgon"]).ToString();
                    Objfd.RakeName = (Result["rakename"] == null ? "" : Result["rakename"]).ToString();
                    Objfd.FstOprPlcTime = (Result["oprgplcttime"] == null ? "" : Result["oprgplcttime"]).ToString();
                    Objfd.LstRelsTime = (Result["relstime"] == null ? "" : Result["relstime"]).ToString();
                    
                }
                if (Result.NextResult())
                {                    
                    while (Result.Read())
                    {
                        Status = 1;
                        Objfd.lstWgon.Add(new WgonDtl()                        
                        {
                            ID=Convert.ToInt32(Result["ID"]),
                            WgonId = Convert.ToString(Result["wgonid"]),
                            ContNo = Convert.ToString(Result["contnumb"]),
                            ContFlg = Convert.ToString(Result["contleflag"]),
                            ContSize = Convert.ToString(Result["contsize"]),
                            ContPosn = Convert.ToString(Result["contposn"]),
                            Commodity = Convert.ToString(Result["cmdtload"]),
                            TrafficType = Convert.ToString(Result["trfctype"]),
                            CommodityCode = Convert.ToString(Result["Cmdtstatcode"]),
                            ContLodWt = Convert.ToString(Result["contloadwght"]),
                            ContTareWt = Convert.ToString(Result["conttarewght"]),
                            SmtpNo = Convert.ToString(Result["smtpno"]),
                            SmtpDt = Convert.ToString(Result["smtpdate"]),
                            HsnCode = Convert.ToString(Result["hsncode"]),
                        });
                    }
                }
                    if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = Objfd;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void AddEditRakeCargo(RakeWagonHdr Objfd, string WagonXml, int CreatedBy) 
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();           

            LstParam.Add(new MySqlParameter { ParameterName = "in_WagonXml", MySqlDbType = MySqlDbType.Text, Value = WagonXml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = CreatedBy });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditRakeWagonData", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Saved Successfully";
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

        public void GetAllFOISEntry(int Page)
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
                Result = DataAccess.ExecuteDataSet("GetAllFOISEntry", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();

                List<RakeWagonHdr> FOISEntryList = new List<RakeWagonHdr>();

                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        RakeWagonHdr objFOISEntry = new RakeWagonHdr();
                        objFOISEntry.RWHdrId = Convert.ToInt32(dr["Id"]);
                        objFOISEntry.InvoiceNo = Convert.ToString(dr["invcid"]);
                        objFOISEntry.RakeId = Convert.ToString(dr["rakeid"]);
                        objFOISEntry.StationFrom = Convert.ToString(dr["sttnfrom"]);
                        objFOISEntry.StationTO = Convert.ToString(dr["sttnto"]);
                        objFOISEntry.IsSend = Convert.ToInt32(dr["IsSend"]);
                        FOISEntryList.Add(objFOISEntry);
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = FOISEntryList;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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

        public void GetFOISEntryById(int RWHdrId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_RWHdrId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = RWHdrId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetFOISEntryById", CommandType.StoredProcedure, DParam);
            RakeWagonHdr objFOIS = new RakeWagonHdr();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {                    
                    Status = 1;
                    objFOIS.RWHdrId = Convert.ToInt32(result["RWHdrId"]);
                    objFOIS.InvoiceNo = Convert.ToString(result["invcid"]);
                    objFOIS.RakeId = Convert.ToString(result["rakeid"]);
                    objFOIS.RakeName = Convert.ToString(result["rakename"]);
                    objFOIS.StationFrom = Convert.ToString(result["sttnfrom"]);
                    objFOIS.StationTO = Convert.ToString(result["sttnto"]);
                    objFOIS.NoOfWagon = Convert.ToString(result["noofwgon"]);
                    objFOIS.FstOprPlcTime = Convert.ToString(result["oprgplcttime"]);
                    objFOIS.LstRelsTime = Convert.ToString(result["relstime"]);
                    
                }
                if (result.NextResult())
                {
                    while (result.Read())
                    {
                        objFOIS.lstWgon.Add(new WgonDtl()
                        {
                             ID= Convert.ToInt32(result["ID"]),
                            WgonId = Convert.ToString(result["wgonid"]),
                            ContNo = Convert.ToString(result["contnumb"]),
                            ContFlg = Convert.ToString(result["contleflag"]),
                            ContSize = Convert.ToString(result["contsize"]),
                            ContPosn = Convert.ToString(result["contposn"]),
                            Commodity = Convert.ToString(result["cmdtload"]),
                            TrafficType = Convert.ToString(result["trfctype"]),
                            CommodityCode = Convert.ToString(result["Cmdtstatcode"]),
                            ContTareWt = Convert.ToString(result["conttarewght"]),
                            ContLodWt = Convert.ToString(result["contloadwght"]),
                            SmtpNo = Convert.ToString(result["smtpno"]),
                            SmtpDt = Convert.ToString(result["smtpdate"]),
                            HsnCode = Convert.ToString(result["hsncode"])
                            
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Data = objFOIS;
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

        public void AddExcelDataToTempTable(TempRakeWagonHdr ObjData, string pxml)
        {
            DataSet Result = new DataSet();
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_WagonXml", MySqlDbType = MySqlDbType.LongText, Value = pxml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjData.InvoiceId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjData.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("AddTempExcelData", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            RakeWagonHdr Objfd = new RakeWagonHdr();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Objfd.RWHdrId = Convert.ToInt32(dr["RWHdrId"]);
                        Objfd.InvoiceNo = Convert.ToString(dr["invcid"]);
                        Objfd.RakeId = Convert.ToString(dr["rakeid"]);
                        Objfd.RakeName = Convert.ToString(dr["rakename"]);
                        Objfd.StationFrom = Convert.ToString(dr["sttnfrom"]);
                        Objfd.StationTO = Convert.ToString(dr["sttnto"]);
                        Objfd.NoOfWagon = Convert.ToString(dr["noofwgon"]);
                        Objfd.FstOprPlcTime = Convert.ToString(dr["oprgplcttime"]);
                        Objfd.LstRelsTime = Convert.ToString(dr["relstime"]);
                    }

                    if (Result.Tables.Count > 1 && Result.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in Result.Tables[1].Rows)
                        {
                            Status = 1;
                            Objfd.lstWgon.Add(new WgonDtl()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                WgonId = Convert.ToString(dr["wgonid"]),
                                ContNo = Convert.ToString(dr["ContainerNo"]),
                                ContFlg = Convert.ToString(dr["ContainerFlag"]),
                                ContSize = Convert.ToString(dr["ContainerSize"]),
                                ContPosn = Convert.ToString(dr["ContainerPosition"]),
                                Commodity = Convert.ToString(dr["CommodityLoaded"]),
                                TrafficType = Convert.ToString(dr["TrafficType"]),
                                CommodityCode = Convert.ToString(dr["CommodityStatisticalCode"]),
                                ContLodWt = Convert.ToString(dr["ContainerTareWeight"]),
                                ContTareWt = Convert.ToString(dr["ContainerLoadedWeight"]),
                                SmtpNo = Convert.ToString(dr["SMTPNumber"]),
                                SmtpDt = Convert.ToString(dr["SMTPDate"]),
                                HsnCode = Convert.ToString(dr["HSNCode"]),
                                ParentId = Convert.ToInt32(dr["ParentId"]),
                            });
                        }
                    }

                    if (Status == 1)
                    {
                        _DBResponse.Status = 1;
                        _DBResponse.Message = "Success";
                        _DBResponse.Data = Objfd;
                    }
                    else
                    {
                        _DBResponse.Status = 0;
                        _DBResponse.Message = "No Data";
                        _DBResponse.Data = null;
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
            }
        }

        #endregion


        #region Send Rake Loading Details
        public void GetRakeLoadingDetails(int RWHdrId)
        {
            int Status = 0;
            DataSet Result = new DataSet();
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_RWHdrId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = RWHdrId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            Result = DataAccess.ExecuteDataSet("GetRakeLoadingDetails", CommandType.StoredProcedure, DParam);
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

        public void LoadingDetailsUpdateStatus(int RWHdrId, string json,string content)
        {
            
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_RWHdrId", MySqlDbType = MySqlDbType.Int32, Value = RWHdrId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Json", MySqlDbType = MySqlDbType.Text, Value = json });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Content", MySqlDbType = MySqlDbType.Text, Value = content });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });            
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddLodingResponseData", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Saved Successfully";
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
    }
}