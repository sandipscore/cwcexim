using CwcExim.Areas.Pest.Models;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace CwcExim.Repositories
{
    public class DSR_PestRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

            }
        }
        public void ListOfChemicalNames()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("LstOfChemical", CommandType.StoredProcedure);
            List<SelectListItem> lstitem = new List<SelectListItem>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstitem.Add(new SelectListItem {Text=Result["ChemicalName"].ToString(),Value=Result["ChemicalId"].ToString() });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstitem;
                }
                else
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


        
        #region Chemical Stock In
        public void AddEditChemical(DSR_ChemicalStockIn ObjChem)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalStockId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjChem.ChemicalStockId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalId", MySqlDbType = MySqlDbType.Int32, Value = ObjChem.ChemicalId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalName", MySqlDbType = MySqlDbType.VarChar, Value = ObjChem.ChemicalName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjChem.Date).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExpDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjChem.ExpiryDate).ToString("yyyy-MM-dd") });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Quantity", MySqlDbType = MySqlDbType.Int32, Value = ObjChem.Quantity });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BatchNo", MySqlDbType = MySqlDbType.VarChar,Size=50, Value = ObjChem.BatchNo });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Branch_Id", MySqlDbType = MySqlDbType.Int32, Value = ObjChem.BranchId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjChem.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditChemicalStockIn", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjChem.ChemicalStockId == 0 ? "Chemical Details Saved Successfully" : "Chemical Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Chemical Name Already Exist";
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

        public void GetChemical(int ChemicalStockId)
        {
            int Status = 0;
            List<MySqlParameter> LstParm = new List<MySqlParameter>();
            LstParm.Add(new MySqlParameter { ParameterName = "in_ChemicalStockId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ChemicalStockId });
            IDataParameter[] DParam = { };
            DParam = LstParm.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetChemicalStockIn", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSR_ChemicalStockIn ObjChem = new DSR_ChemicalStockIn();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    ObjChem.ChemicalStockId = Convert.ToInt32(Result["ChemicalStockId"]);
                    ObjChem.ChemicalName = Result["ChemicalName"].ToString();
                    ObjChem.Date = Result["Date"].ToString();
                    ObjChem.Quantity = Convert.ToInt32(Result["Quantity"]);
                    ObjChem.ChemicalId = Convert.ToInt32(Result["ChemicalId"]);
                    ObjChem.ExpiryDate = Result["ExpiryDate"].ToString();
                    ObjChem.BatchNo = Result["BatchNo"].ToString();
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjChem;
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
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetAllChemical()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalStockId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetChemicalStockIn", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSR_ChemicalStockIn> LstYard = new List<DSR_ChemicalStockIn>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstYard.Add(new DSR_ChemicalStockIn
                    {
                        ChemicalStockId = Convert.ToInt32(Result["ChemicalStockId"]),
                        ChemicalName = Result["ChemicalName"].ToString(),
                        Date = Result["Date"].ToString(),
                        Quantity = Convert.ToInt32(Result["Quantity"]),
                        ExpiryDate =Result["ExpiryDate"].ToString(),
                        BatchNo = Result["BatchNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstYard;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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

        #region Chemical Consumption
        public void ListOfChemicalNamesforConsumption()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("LstOfChemicalforconsumption", CommandType.StoredProcedure);
            List<SelectListItem> lstitem = new List<SelectListItem>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstitem.Add(new SelectListItem { Text = Result["ChemicalName"].ToString(), Value = Result["ChemicalId"].ToString() });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstitem;
                }
                else
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



        public void AddEditChemicalConsumption(DSRChemicalConsumption ObjChem,string ChemicalXML)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalConsumptionId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjChem.ChemicalConsumptionId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjChem.Date).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_XML", MySqlDbType = MySqlDbType.LongText, Value = ChemicalXML });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Branch_Id", MySqlDbType = MySqlDbType.Int32, Value = ObjChem.BranchId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjChem.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditChemicalConsumption", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjChem.ChemicalConsumptionId == 0 ? "Chemical Consumption Details Saved Successfully" : "Chemical Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Chemical Name Already Exist";
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



        public void GetAllChemicalConsumption()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalConsumpId", MySqlDbType = MySqlDbType.Int32, Value = 0 });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetChemicalConsumption", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<DSRChemicalConsumption> LstYard = new List<DSRChemicalConsumption>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    LstYard.Add(new DSRChemicalConsumption
                    {
                        ChemicalConsumptionId = Convert.ToInt32(Result["ChemicalConsumptionId"]),
                        ChemicalName = Result["ChemicalName"].ToString(),
                        Date = Result["Date"].ToString(),
                        Quantity = Convert.ToInt32(Result["Quantity"]),
                        ExpiryDate = Result["ExpiryDate"].ToString(),
                        BatchNo = Result["BatchNo"].ToString(),
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstYard;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }

        public void ListOfChemicalNamesforConsumptionforID(int Id)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalConsumpId", MySqlDbType = MySqlDbType.Int32, Value = Id });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetChemicalConsumption", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRChemicalConsumption objChem = new DSRChemicalConsumption();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    objChem.ChemicalConsumptionId = Convert.ToInt32(Result["ChemicalConsumptionId"]);
                    objChem.ChemicalName = Result["ChemicalName"].ToString();
                    objChem.Date = Result["Date"].ToString();
                    objChem.Quantity = Convert.ToInt32(Result["Quantity"]);
                    objChem.ExpiryDate = Result["ExpiryDate"].ToString();
                    objChem.BatchNo = Result["BatchNo"].ToString();
                    objChem.Remarks = Result["Remarks"].ToString();


                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objChem;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }


        public void EditChemicalConsumption(DSRChemicalConsumption ObjChem)
        {
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalConsumpId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjChem.ChemicalConsumptionId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Date", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjChem.Date).ToString("yyyy-MM-dd") });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExpDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(ObjChem.ExpiryDate).ToString("yyyy-MM-dd") });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Quantity", MySqlDbType = MySqlDbType.Int32, Value = ObjChem.Quantity });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BatchNo", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjChem.BatchNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjChem.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Branch_Id", MySqlDbType = MySqlDbType.Int32, Value = ObjChem.BranchId });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjChem.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("EditChemicalConsumption", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = ObjChem.ChemicalConsumptionId == 0 ? "Chemical Consumption Details Saved Successfully" : "Chemical Consumption Details Updated Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Chemical Name Already Exist";
                    _DBResponse.Data = null;
                }

                else if (Result == 3)
                {
                    _DBResponse.Status = 3;
                    _DBResponse.Message = "Quantity should be less than Stock";
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
        #region Pest Invoice
        public void GetPaymentPartyForPage(string PartyCode, int Page = 0)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyCode", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = PartyCode });
            lstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = Page });
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetPaymentPartyForPage", CommandType.StoredProcedure, Dparam);
            IList<DSR_PartyForPage> lstParty = new List<DSR_PartyForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool State = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstParty.Add(new DSR_PartyForPage
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


        public void GetChemicalStock(int cid, string batch,string expiry)
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_chemicalid", MySqlDbType = MySqlDbType.Int32, Value = cid });
            lstParam.Add(new MySqlParameter { ParameterName = "in_batchNo", MySqlDbType = MySqlDbType.VarChar, Value = batch });
            lstParam.Add(new MySqlParameter { ParameterName = "in_expiry", MySqlDbType = MySqlDbType.Date, Value = expiry });

            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetQuantityforChemicalConsumption", CommandType.StoredProcedure, Dparam);
            IList<DSR_PartyForPage> lstParty = new List<DSR_PartyForPage>();
            _DBResponse = new DatabaseResponse();
            Decimal stock = 0;
            try
            {
                
               
                while (Result.Read())
                {
                    Status = 1;
                    stock = Convert.ToDecimal(Result["Stock"]);
                }
                
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = stock;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
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
            IList<DSR_PartyForPage> lstPayer = new List<DSR_PartyForPage>();
            _DBResponse = new DatabaseResponse();
            try
            {
                bool StatePayer = false;
                while (Result.Read())
                {
                    Status = 1;
                    lstPayer.Add(new DSR_PartyForPage
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




        public void GetPestControlCharges(decimal Amount, decimal HandlingAmount, string InvoiceType, int PartyId, string LineXML,string DeliveryDate, string ExportUnder, int Distance, int Cane)
        {

            int Status = 0;
            string dt = Convert.ToDateTime(DeliveryDate).ToString("yyyy-MM-dd");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HandlingAmount", MySqlDbType = MySqlDbType.Decimal, Value = HandlingAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "LineXML", MySqlDbType = MySqlDbType.Text, Value = LineXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.Text, Value = ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Int32, Value = Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Cane", MySqlDbType = MySqlDbType.Int32, Value = Cane });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(dt) });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("getPestControlCharges", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            DSRPestControl ObjPestControl = new DSRPestControl();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ObjPestControl.Amount = Convert.ToDecimal(Result["SUM"]);
                    ObjPestControl.HandlingAmount = Convert.ToDecimal(Result["HandlingAmount"]);
                    ObjPestControl.PestControlIGST = Convert.ToDecimal(Result["PestControlIGST"]);
                    ObjPestControl.PestControlCGST = Convert.ToDecimal(Result["PestControlCGST"]);
                    ObjPestControl.PestControlSGST = Convert.ToDecimal(Result["PestControlSGST"]);
                    ObjPestControl.HandlingIGST = Convert.ToDecimal(Result["HandlingIGST"]);
                    ObjPestControl.HandlingCGST = Convert.ToDecimal(Result["HandlingCGST"]);
                    ObjPestControl.HandlingSGST = Convert.ToDecimal(Result["HandlingSGST"]);
                    ObjPestControl.IGST = Convert.ToDecimal(Result["IGSTAmt"]);
                    ObjPestControl.CGST = Convert.ToDecimal(Result["CGSTAmt"]);
                    ObjPestControl.SGST = Convert.ToDecimal(Result["SGSTAmt"]);
                    ObjPestControl.Total = Convert.ToDecimal(Result["Total"]);
                    ObjPestControl.Round_up = Convert.ToDecimal(Result["Round_Up"]);
                    ObjPestControl.NetAmt = Convert.ToDecimal(Result["NetAmt"]);
                    ObjPestControl.Totaltaxable = Convert.ToDecimal(Result["Totaltaxable"]);
                    ObjPestControl.IGSTPer = Convert.ToDecimal(Result["IGSTPer"]);
                    ObjPestControl.CGSTPer = Convert.ToDecimal(Result["CGSTPer"]);
                    ObjPestControl.SGSTPer = Convert.ToDecimal(Result["SGSTPer"]);
                    ObjPestControl.SacCode = Convert.ToString(Result["SacCode"]);
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ObjPestControl;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }

            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
            finally
            {
                Result.Dispose();
                Result.Close();
            }
        }
       
        public void AddEditPestControl(DSRPestControl ObjPostPaymentSheet, string InsideCont, string OutsideCont, int BranchId, int Uid, string Module)
        {
            string GeneratedClientId = "0";
            string ReturnObj = "";
            string dt = Convert.ToDateTime(ObjPostPaymentSheet.DeliveryDate).ToString("yyyy-MM-dd hh:mm");
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_FumiType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.FumiType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IssueDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.IssueDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_FumigationDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjPostPaymentSheet.FumigationDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PlaceFumigation", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Place });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPkge", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Pkg });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvSplNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvSplNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Vessel", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.VesselNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_dosage", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Dosages });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortOfLoadingId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PortOfLoadingId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PortOfLoading", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PortOfLoadingName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Exposure", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Exposure });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PostOfDisId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PortOfDestId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PostOfDisc", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PortOfDestName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GasType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.GasTight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Pressor", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Pressor });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Chemical", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ChemicalFumigation });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Temp", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Temperature });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Country", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CountryId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PackType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PackageType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoCont", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Canes });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CargoDesc", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.CargoDesc });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.ExporterId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Exporter", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExporterName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Consignee", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Consignee });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AsmType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.AsmType });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.InvoiceNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(dt) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeId", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeName", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeName });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.GSTNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Container", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Container });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Amount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Amount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PestControlCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.PestControlCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PestControlSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.PestControlSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PestControlIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.PestControlIGST });

            LstParam.Add(new MySqlParameter { ParameterName = "in_HandlingAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HandlingAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HandlingCGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HandlingCGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HandlingSGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HandlingSGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_HandlingIGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.HandlingIGST });

            LstParam.Add(new MySqlParameter { ParameterName = "in_IGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.IGSTPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CGSTPer });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SGSTPer", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.SGSTPer });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.CGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.SGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IGST", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.IGST });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Round_up", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Round_up });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Total", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Total });

            LstParam.Add(new MySqlParameter { ParameterName = "in_NetAmt", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.NetAmt });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TotalTaxable", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.Totaltaxable });

            LstParam.Add(new MySqlParameter { ParameterName = "in_Naration", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.Naration });
         //   LstParam.Add(new MySqlParameter { ParameterName = "in_ChemicalXML", MySqlDbType = MySqlDbType.Text, Value = ChemicalData });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InsideContXML", MySqlDbType = MySqlDbType.Text, Value = InsideCont });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OutsideContXML", MySqlDbType = MySqlDbType.Text, Value = OutsideCont });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CreatedBy", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchID", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Module", MySqlDbType = MySqlDbType.VarChar, Value = Module });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_SacCode", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.SacCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NoOfPallets", MySqlDbType = MySqlDbType.Int32, Value = (ObjPostPaymentSheet.Pallets==null ? "0" : ObjPostPaymentSheet.Pallets) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            LstParam.Add(new MySqlParameter { ParameterName = "ReturnObj", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = ReturnObj });

            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditPestControlInv", CommandType.StoredProcedure, DParam, out GeneratedClientId, out ReturnObj);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Pest Control Operation Invoice Saved Successfully";
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Pest Control Operation Invoice Updated Successfully";
                }
                else if (Result == 4 || Result == 3)
                {
                    _DBResponse.Data = GeneratedClientId;
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

        public void ListOfChemicalNameforInvoice()
        {
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("LstOfChemicalNameforInv", CommandType.StoredProcedure);
            IList<DSRChemicalConsump> lstSac = new List<DSRChemicalConsump>();
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstSac.Add(new DSRChemicalConsump()
                    {
                        ChemicalId = Convert.ToInt32(Result["ChemicalId"]),
                        ChemicalName = Convert.ToString(Result["ChemicalGName"]),
                        Quantity = Convert.ToDecimal(Result["Quantity"]),
                        BatchNo = "A",
                        ExpiryDate = "2019-01-01",
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstSac;
                }
                else
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

        public void ListOfExporterforPESTInvoice()
        {
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ListOfExporterforPestInvoice", CommandType.StoredProcedure);
            IList<Areas.Export.Models.Exporter> lstExporter = new List<Areas.Export.Models.Exporter>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstExporter.Add(new Areas.Export.Models.Exporter
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

        public void GetContainerForPestInvoice()
        {
            DataSet Result = new DataSet();
            int Status = 0;
            try
            {
                //DateTime dt = DateTime.ParseExact(IGM_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
             //   LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = 6 });
             //   LstParam.Add(new MySqlParameter { ParameterName = "In_OBL", MySqlDbType = MySqlDbType.VarChar, Value = OBLNo });
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetInsideContforPestInvoice", CommandType.StoredProcedure);
                _DBResponse = new DatabaseResponse();

                DSRPestControl objOBLEntry = new DSRPestControl();
                //List<OblEntryDetails> OblEntryDetailsList = new List<OblEntryDetails>();
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    foreach (DataRow dr in Result.Tables[0].Rows)
                    {
                        insidecontainer objOBLCont = new insidecontainer();
                        objOBLCont.CfsCode = Convert.ToString(dr["CFSCode"]);
                        objOBLCont.ContainerNo = Convert.ToString(dr["ContainerNo"]);
                        objOBLCont.Size = Convert.ToString(dr["Size"]);

                        
                        objOBLEntry.InsideContDetails.Add(objOBLCont);
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
                    _DBResponse.Data = objOBLEntry.InsideContDetails;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data";
                    _DBResponse.Data = null;
                }
            }
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