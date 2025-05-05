using CwcExim.Areas.Auction.Models;
using CwcExim.Areas.CashManagement.Models;
using CwcExim.Areas.Export.Models;
using CwcExim.DAL;
using CwcExim.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CwcExim.Repositories
{
    public class WFLD_AuctionRepository
    {
        private DatabaseResponse _DBResponse;

        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }



        public void GetCargoDetailsList(string operationType, string auctionType, int Uid, int inPage)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "OperationType", MySqlDbType = MySqlDbType.String, Value = operationType });
            LstParam.Add(new MySqlParameter { ParameterName = "AuctionType", MySqlDbType = MySqlDbType.String, Value = auctionType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Page", MySqlDbType = MySqlDbType.Int32, Value = inPage });

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("AuctionGetDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            List<WFLD_AuctionNoticeDetails> LstAuctionNoticeDetails = new List<WFLD_AuctionNoticeDetails>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    int srNo = 0;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        srNo++;
                        WFLD_AuctionNoticeDetails objCargoDetails = new WFLD_AuctionNoticeDetails();
                        objCargoDetails.SrNo = srNo;
                        objCargoDetails.NoOfPackages = Convert.ToInt32(item["NoOfPackages"]);
                        objCargoDetails.GodownLocation = Convert.ToString(item["GodownLocation"]);
                        objCargoDetails.BLNo = Convert.ToString(item["BLNo"]);
                        objCargoDetails.BLDate = Convert.ToString(item["BOLDate"]);
                        objCargoDetails.ShipBillDate = Convert.ToString(item["ShippingBillDate"]);
                        objCargoDetails.PartyId = Convert.ToInt32(item["PartyId"]);
                        objCargoDetails.PartyName = Convert.ToString(item["PartyName"]);
                        objCargoDetails.CFSCode = Convert.ToString(item["CFSCode"]);
                        objCargoDetails.BOENo = Convert.ToString(item["BOENo"]);
                        objCargoDetails.CommodityId = Convert.ToInt32(item["CommodityId"]);
                        objCargoDetails.CommodityName = Convert.ToString(item["CommodityName"]);
                        objCargoDetails.Weight = Convert.ToString(item["Weight"]);
                        objCargoDetails.CUM = Convert.ToString(item["CUM"]);
                        objCargoDetails.SQM = Convert.ToString(item["SQM"]);
                        objCargoDetails.Duty = Convert.ToString(item["Duty"]);
                        objCargoDetails.Fob = Convert.ToString(item["Fob"]);
                        objCargoDetails.CIF = Convert.ToString(item["CIF"]);
                        objCargoDetails.IsInsured = Convert.ToString(item["IsInsured"]);
                        objCargoDetails.LineNo = Convert.ToString(item["LineNo"]);
                        objCargoDetails.ShippingBillNo = Convert.ToString(item["ShippingBillNo"]);
                        objCargoDetails.ContainerNo = Convert.ToString(item["ContainerNo"]);
                        objCargoDetails.AuctionEligibleDate = Convert.ToString(item["AuctionEligibleDate"]);
                        objCargoDetails.Size = Convert.ToString(item["Size"]);
                        objCargoDetails.EntryDate = Convert.ToString(item["EntryDate"]);
                        objCargoDetails.GodownID = Convert.ToInt32(item["GodownId"]);
                        objCargoDetails.ShippingLineNo = Convert.ToString(item["ShippingLineId"]);
                        objCargoDetails.CuttingDate = Convert.ToString(item["CuttingDate"]);
                        objCargoDetails.RefId = Convert.ToInt32(item["RefId"]);
                        objCargoDetails.TSANo = Convert.ToString(item["TSANo"]);
                        objCargoDetails.IGM = Convert.ToString(item["IGM_No"]);
                     

                        LstAuctionNoticeDetails.Add(objCargoDetails);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstAuctionNoticeDetails;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
                    _DBResponse.Data = null;
                }
            }
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

            //return LstAuctionNoticeDetails;
        }
        public void GetCargoDetailsListByOBLNoShipBill(string operationType, string auctionType, int Uid, string OblNo, string ShipBill)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "OperationType", MySqlDbType = MySqlDbType.String, Value = operationType });
            LstParam.Add(new MySqlParameter { ParameterName = "AuctionType", MySqlDbType = MySqlDbType.String, Value = auctionType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShipBill", MySqlDbType = MySqlDbType.String, Value = ShipBill });
            LstParam.Add(new MySqlParameter { ParameterName = "in_OBL", MySqlDbType = MySqlDbType.String, Value = OblNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("auctiongetdetailsByShipBillOBL", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            List<WFLD_AuctionNoticeDetails> LstAuctionNoticeDetails = new List<WFLD_AuctionNoticeDetails>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    int srNo = 0;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        srNo++;
                        WFLD_AuctionNoticeDetails objCargoDetails = new WFLD_AuctionNoticeDetails();
                        objCargoDetails.SrNo = srNo;
                        objCargoDetails.NoOfPackages = Convert.ToInt32(item["NoOfPackages"]);
                        objCargoDetails.GodownLocation = Convert.ToString(item["GodownLocation"]);
                        objCargoDetails.BLNo = Convert.ToString(item["BLNo"]);
                        objCargoDetails.BLDate = Convert.ToString(item["BOLDate"]);
                        objCargoDetails.ShipBillDate = Convert.ToString(item["ShippingBillDate"]);
                        objCargoDetails.PartyId = Convert.ToInt32(item["PartyId"]);
                        objCargoDetails.PartyName = Convert.ToString(item["PartyName"]);
                        objCargoDetails.CFSCode = Convert.ToString(item["CFSCode"]);
                        objCargoDetails.BOENo = Convert.ToString(item["BOENo"]);
                        objCargoDetails.CommodityId = Convert.ToInt32(item["CommodityId"]);
                        objCargoDetails.CommodityName = Convert.ToString(item["CommodityName"]);
                        objCargoDetails.Weight = Convert.ToString(item["Weight"]);
                        objCargoDetails.CUM = Convert.ToString(item["CUM"]);
                        objCargoDetails.SQM = Convert.ToString(item["SQM"]);
                        objCargoDetails.Duty = Convert.ToString(item["Duty"]);
                        objCargoDetails.Fob = Convert.ToString(item["Fob"]);
                        objCargoDetails.CIF = Convert.ToString(item["CIF"]);
                        objCargoDetails.IsInsured = Convert.ToString(item["IsInsured"]);
                        objCargoDetails.LineNo = Convert.ToString(item["LineNo"]);
                        objCargoDetails.ShippingBillNo = Convert.ToString(item["ShippingBillNo"]);
                        objCargoDetails.ContainerNo = Convert.ToString(item["ContainerNo"]);
                        objCargoDetails.AuctionEligibleDate = Convert.ToString(item["AuctionEligibleDate"]);
                        objCargoDetails.Size = Convert.ToString(item["Size"]);
                        objCargoDetails.EntryDate = Convert.ToString(item["EntryDate"]);
                        objCargoDetails.GodownID = Convert.ToInt32(item["GodownId"]);
                        objCargoDetails.ShippingLineNo = Convert.ToString(item["ShippingLineId"]);
                        objCargoDetails.CuttingDate = Convert.ToString(item["CuttingDate"]);
                        objCargoDetails.RefId = Convert.ToInt32(item["RefId"]);
                        objCargoDetails.TSANo = Convert.ToString(item["TSANo"]);
                        objCargoDetails.IGM = Convert.ToString(item["IGM_No"]);
                        LstAuctionNoticeDetails.Add(objCargoDetails);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstAuctionNoticeDetails;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
                    _DBResponse.Data = null;
                }
            }
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

            //return LstAuctionNoticeDetails;
        }


        public void GetAuctionNoticeOBLNoShipBill(string operationType, string auctionType, string AuctionNo, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "AuctionType_", MySqlDbType = MySqlDbType.String, Value = auctionType });
            LstParam.Add(new MySqlParameter { ParameterName = "OperationType_", MySqlDbType = MySqlDbType.String, Value = operationType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Auction", MySqlDbType = MySqlDbType.String, Value = AuctionNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("AuctionGetDataByOblShipbill", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<WFLD_AuctionNoticeItemDetails> LstAuctionNoticeDetails = new List<WFLD_AuctionNoticeItemDetails>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        WFLD_AuctionNoticeItemDetails objAuctionNoticeItemDetails = new WFLD_AuctionNoticeItemDetails();
                        objAuctionNoticeItemDetails.AuctionNoticeDtlId = Convert.ToInt32(item["AuctionNoticeId"]);
                        objAuctionNoticeItemDetails.NoticeNo = Convert.ToString(item["AuctionNoticeNo"]);
                        objAuctionNoticeItemDetails.AuctionNoticeDate = Convert.ToString(item["AuctionNoticeDate"]);
                        objAuctionNoticeItemDetails.PartyName = Convert.ToString(item["PartyName"]);
                        objAuctionNoticeItemDetails.CommodityName = Convert.ToString(item["CommodityName"]);
                        objAuctionNoticeItemDetails.BOENo = Convert.ToString(item["BLNo"]);
                        objAuctionNoticeItemDetails.ShippingBillNo = Convert.ToString(item["ShippingBillNo"]);
                        objAuctionNoticeItemDetails.BLDate = Convert.ToString(item["BLDate"]);
                        objAuctionNoticeItemDetails.ShipBillDate = Convert.ToString(item["ShipBillDate"]);
                        objAuctionNoticeItemDetails.Type = Convert.ToString(item["OperationType"]);
                        objAuctionNoticeItemDetails.SecondNotice = Convert.ToInt32(item["SecondNotice"]);
                        objAuctionNoticeItemDetails.ThirdNotice = Convert.ToInt32(item["ThirdNotice"]);
                        objAuctionNoticeItemDetails.NoOfNotice = Convert.ToInt32(item["NoOfNotice"]);


                        LstAuctionNoticeDetails.Add(objAuctionNoticeItemDetails);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstAuctionNoticeDetails;
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
            }
            //return LstAuctionNoticeDetails;
        }

        public void SaveAuctionIssueNotice(string operationType, string auctionType, int partyId, int branchId, int createdBy, string xmlAuctionNotice)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "OperationType_", MySqlDbType = MySqlDbType.String, Value = operationType });
            LstParam.Add(new MySqlParameter { ParameterName = "AuctionType_", MySqlDbType = MySqlDbType.String, Value = auctionType });
            LstParam.Add(new MySqlParameter { ParameterName = "PartyId_", MySqlDbType = MySqlDbType.Int32, Value = partyId });
            LstParam.Add(new MySqlParameter { ParameterName = "BranchId_", MySqlDbType = MySqlDbType.Int32, Value = branchId });
            LstParam.Add(new MySqlParameter { ParameterName = "CreatedBy_", MySqlDbType = MySqlDbType.Int32, Value = createdBy });
            LstParam.Add(new MySqlParameter { ParameterName = "xmlAuctionNoticeItemDetails", MySqlDbType = MySqlDbType.Text, Value = xmlAuctionNotice });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AuctionSaveData", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Data Saved Successfully";
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


        public void SaveAuctionReissueNotice(string operationType, string auctionType, int partyId, int branchId, int createdBy, string xmlAuctionNotice, int Auctionid, string AuctionDate)
        {


            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "OperationType_", MySqlDbType = MySqlDbType.String, Value = operationType });
            LstParam.Add(new MySqlParameter { ParameterName = "AuctionType_", MySqlDbType = MySqlDbType.String, Value = auctionType });
            LstParam.Add(new MySqlParameter { ParameterName = "PartyId_", MySqlDbType = MySqlDbType.Int32, Value = partyId });
            LstParam.Add(new MySqlParameter { ParameterName = "BranchId_", MySqlDbType = MySqlDbType.Int32, Value = branchId });
            LstParam.Add(new MySqlParameter { ParameterName = "CreatedBy_", MySqlDbType = MySqlDbType.Int32, Value = createdBy });
            LstParam.Add(new MySqlParameter { ParameterName = "xmlAuctionNoticeItemDetails", MySqlDbType = MySqlDbType.Text, Value = xmlAuctionNotice });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AuctionId", MySqlDbType = MySqlDbType.Int32, Value = Auctionid });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AuctionDate", MySqlDbType = MySqlDbType.Date, Value = AuctionDate });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("ReissueNoticeSaveData", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Data Saved Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Auction notice date should be different";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Auction notice date should be greater than previous date";
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



        public void GetAllAuctionNotice(string operationType, string auctionType, int Uid)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "OperationType_", MySqlDbType = MySqlDbType.String, Value = operationType });
            LstParam.Add(new MySqlParameter { ParameterName = "AuctionType_", MySqlDbType = MySqlDbType.String, Value = auctionType });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("AuctionGetData", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<WFLD_AuctionNoticeItemDetails> LstAuctionNoticeDetails = new List<WFLD_AuctionNoticeItemDetails>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        WFLD_AuctionNoticeItemDetails objAuctionNoticeItemDetails = new WFLD_AuctionNoticeItemDetails();
                        objAuctionNoticeItemDetails.AuctionNoticeDtlId = Convert.ToInt32(item["AuctionNoticeId"]);
                        objAuctionNoticeItemDetails.NoticeNo = Convert.ToString(item["AuctionNoticeNo"]);
                        objAuctionNoticeItemDetails.AuctionNoticeDate = Convert.ToString(item["AuctionNoticeDate"]);
                        objAuctionNoticeItemDetails.PartyName = Convert.ToString(item["PartyName"]);
                        objAuctionNoticeItemDetails.CommodityName = Convert.ToString(item["CommodityName"]);
                        objAuctionNoticeItemDetails.BOENo = Convert.ToString(item["BLNo"]);
                        objAuctionNoticeItemDetails.ShippingBillNo = Convert.ToString(item["ShippingBillNo"]);
                        objAuctionNoticeItemDetails.BLDate = Convert.ToString(item["BLDate"]);
                        objAuctionNoticeItemDetails.ShipBillDate = Convert.ToString(item["ShipBillDate"]);
                        objAuctionNoticeItemDetails.Type = Convert.ToString(item["OperationType"]);
                        objAuctionNoticeItemDetails.ContainerNo = Convert.ToString(item["ContainerNo"]);
                        objAuctionNoticeItemDetails.SecondNotice = Convert.ToInt32(item["SecondNotice"]);
                        objAuctionNoticeItemDetails.ThirdNotice = Convert.ToInt32(item["ThirdNotice"]);
                        objAuctionNoticeItemDetails.NoOfNotice= Convert.ToInt32(item["NoOfNotice"]);                        
                        LstAuctionNoticeDetails.Add(objAuctionNoticeItemDetails);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstAuctionNoticeDetails;
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
            }
        }

        public WFLD_AuctionNoticePrintViewModel GetAuctionNoticeDataToPrint(int AuctionNoticeDtlId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "AuctionNoticeDtlId_", MySqlDbType = MySqlDbType.Int32, Value = AuctionNoticeDtlId });

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("AuctionNoticePrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            WFLD_AuctionNoticePrintViewModel auctionNoticePrintViewModel = new WFLD_AuctionNoticePrintViewModel();
            auctionNoticePrintViewModel.ParticularsOfGoodsList = new List<WFLD_ParticularsOfGoods>();

            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        auctionNoticePrintViewModel.AuctionNoticeNo = Convert.ToString(Result.Tables[0].Rows[0]["AuctionNoticeNo"]);
                        auctionNoticePrintViewModel.DocNo = Convert.ToString(Result.Tables[0].Rows[0]["DocNo"]);
                        auctionNoticePrintViewModel.AuctionNoticeDocNo = Convert.ToString(Result.Tables[0].Rows[0]["AuctionNoticeDocNo"]);
                        auctionNoticePrintViewModel.CompanyAddress = Convert.ToString(Result.Tables[0].Rows[0]["CompanyAddress"]);
                        auctionNoticePrintViewModel.PartyName = Convert.ToString(Result.Tables[0].Rows[0]["PartyName"]);
                        auctionNoticePrintViewModel.PartyAddress = Convert.ToString(Result.Tables[0].Rows[0]["PartyAddress"]);
                        auctionNoticePrintViewModel.NoticeDate = Convert.ToString(Result.Tables[0].Rows[0]["NoticeDate"]);
                        auctionNoticePrintViewModel.AucNtcDaysAftrLanding = Convert.ToString(Result.Tables[0].Rows[0]["AucNtcDaysAftrLanding"]);
                        auctionNoticePrintViewModel.DueDaysAftrAucNtc = Convert.ToString(Result.Tables[0].Rows[0]["DueDaysAftrAucNtc"]);
                        auctionNoticePrintViewModel.GodownLocation = Convert.ToString(Result.Tables[0].Rows[0]["GodownLocation"]);
                        auctionNoticePrintViewModel.AuctionNoticeCC = Convert.ToString(Result.Tables[0].Rows[0]["AuctionNoticeCC"]);
                        auctionNoticePrintViewModel.CommodityName = Convert.ToString(Result.Tables[0].Rows[0]["CommodityName"]);
                        auctionNoticePrintViewModel.OperationType = Convert.ToString(Result.Tables[0].Rows[0]["OperationType"]);
                        auctionNoticePrintViewModel.AuctionType = Convert.ToString(Result.Tables[0].Rows[0]["AuctionType"]);
                        auctionNoticePrintViewModel.Shippingline = Convert.ToString(Result.Tables[0].Rows[0]["Shippingline"]);
                        auctionNoticePrintViewModel.ShippinglineAddress = Convert.ToString(Result.Tables[0].Rows[0]["shippingAddress"]);

                    }

                    foreach (DataRow item in Result.Tables[1].Rows)
                    {
                        Status = 1;
                        WFLD_ParticularsOfGoods objParticularsOfGoods = new WFLD_ParticularsOfGoods();
                        objParticularsOfGoods.ContainerNo = Convert.ToString(item["ContainerNo"]);
                        objParticularsOfGoods.ICDCode = Convert.ToString(item["ICDCode"]);
                        objParticularsOfGoods.EntryDate = Convert.ToString(item["EntryDate"]);
                        objParticularsOfGoods.BLNo = Convert.ToString(item["BLNo"]);
                        objParticularsOfGoods.NoOfPackages = Convert.ToString(item["NoOfPackages"]);
                        objParticularsOfGoods.Weight = Convert.ToString(item["Weight"]);
                        objParticularsOfGoods.ItemNo = Convert.ToString(item["ItemNo"]);
                        objParticularsOfGoods.ShipBillNo = Convert.ToString(item["ShippingBillNo"]);
                        objParticularsOfGoods.CartingDate = Convert.ToString(item["CartingDate"]);
                        objParticularsOfGoods.IGMNO = Convert.ToString(item["IGMNo"]);
                        objParticularsOfGoods.LineNo = Convert.ToString(item["LineNo"]);
                        objParticularsOfGoods.TSA = Convert.ToString(item["TSANo"]);


                        auctionNoticePrintViewModel.ParticularsOfGoodsList.Add(objParticularsOfGoods);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
                    _DBResponse.Data = null;
                }
            }
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

            return auctionNoticePrintViewModel;
        }

        public WFLD_AuctionNoticePrintViewModel GetAuctionSecondNoticeDataToPrint(int AuctionNoticeDtlId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "AuctionNoticeDtlId_", MySqlDbType = MySqlDbType.Int32, Value = AuctionNoticeDtlId });

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("AuctionSecondNoticePrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            WFLD_AuctionNoticePrintViewModel auctionNoticePrintViewModel = new WFLD_AuctionNoticePrintViewModel();
            auctionNoticePrintViewModel.ParticularsOfGoodsList = new List<WFLD_ParticularsOfGoods>();

            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        auctionNoticePrintViewModel.AuctionNoticeNo = Convert.ToString(Result.Tables[0].Rows[0]["AuctionNoticeNo"]);
                        auctionNoticePrintViewModel.DocNo = Convert.ToString(Result.Tables[0].Rows[0]["DocNo"]);
                        auctionNoticePrintViewModel.AuctionNoticeDocNo = Convert.ToString(Result.Tables[0].Rows[0]["AuctionNoticeDocNo"]);
                        auctionNoticePrintViewModel.CompanyAddress = Convert.ToString(Result.Tables[0].Rows[0]["CompanyAddress"]);
                        auctionNoticePrintViewModel.PartyName = Convert.ToString(Result.Tables[0].Rows[0]["PartyName"]);
                        auctionNoticePrintViewModel.PartyAddress = Convert.ToString(Result.Tables[0].Rows[0]["PartyAddress"]);
                        auctionNoticePrintViewModel.NoticeDate = Convert.ToString(Result.Tables[0].Rows[0]["NoticeDate"]);
                        auctionNoticePrintViewModel.AucNtcDaysAftrLanding = Convert.ToString(Result.Tables[0].Rows[0]["AucNtcDaysAftrLanding"]);
                        auctionNoticePrintViewModel.DueDaysAftrAucNtc = Convert.ToString(Result.Tables[0].Rows[0]["DueDaysAftrAucNtc"]);
                        auctionNoticePrintViewModel.GodownLocation = Convert.ToString(Result.Tables[0].Rows[0]["GodownLocation"]);
                        auctionNoticePrintViewModel.AuctionNoticeCC = Convert.ToString(Result.Tables[0].Rows[0]["AuctionNoticeCC"]);
                        auctionNoticePrintViewModel.CommodityName = Convert.ToString(Result.Tables[0].Rows[0]["CommodityName"]);
                        auctionNoticePrintViewModel.OperationType = Convert.ToString(Result.Tables[0].Rows[0]["OperationType"]);
                        auctionNoticePrintViewModel.AuctionType = Convert.ToString(Result.Tables[0].Rows[0]["AuctionType"]);
                        auctionNoticePrintViewModel.Shippingline = Convert.ToString(Result.Tables[0].Rows[0]["Shippingline"]);
                        auctionNoticePrintViewModel.ShippinglineAddress = Convert.ToString(Result.Tables[0].Rows[0]["shippingAddress"]);

                    }

                    foreach (DataRow item in Result.Tables[1].Rows)
                    {
                        Status = 1;
                        WFLD_ParticularsOfGoods objParticularsOfGoods = new WFLD_ParticularsOfGoods();
                        objParticularsOfGoods.ContainerNo = Convert.ToString(item["ContainerNo"]);
                        objParticularsOfGoods.ICDCode = Convert.ToString(item["ICDCode"]);
                        objParticularsOfGoods.EntryDate = Convert.ToString(item["EntryDate"]);
                        objParticularsOfGoods.BLNo = Convert.ToString(item["BLNo"]);
                        objParticularsOfGoods.NoOfPackages = Convert.ToString(item["NoOfPackages"]);
                        objParticularsOfGoods.Weight = Convert.ToString(item["Weight"]);
                        objParticularsOfGoods.ItemNo = Convert.ToString(item["ItemNo"]);
                        objParticularsOfGoods.ShipBillNo = Convert.ToString(item["ShippingBillNo"]);
                        objParticularsOfGoods.CartingDate = Convert.ToString(item["CartingDate"]);
                        objParticularsOfGoods.IGMNO = Convert.ToString(item["IGMNo"]);
                        objParticularsOfGoods.LineNo = Convert.ToString(item["LineNo"]);
                        objParticularsOfGoods.TSA = Convert.ToString(item["TSANo"]);
                        objParticularsOfGoods.Size = Convert.ToString(item["Size"]);


                        auctionNoticePrintViewModel.ParticularsOfGoodsList.Add(objParticularsOfGoods);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
                    _DBResponse.Data = null;
                }
            }
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

            return auctionNoticePrintViewModel;
        }

        public WFLD_AuctionNoticePrintViewModel GetAuctionThirdNoticeDataToPrint(int AuctionNoticeDtlId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "AuctionNoticeDtlId_", MySqlDbType = MySqlDbType.Int32, Value = AuctionNoticeDtlId });

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("AuctionThirdNoticePrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            WFLD_AuctionNoticePrintViewModel auctionNoticePrintViewModel = new WFLD_AuctionNoticePrintViewModel();
            auctionNoticePrintViewModel.ParticularsOfGoodsList = new List<WFLD_ParticularsOfGoods>();

            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    if (Result.Tables[0].Rows.Count > 0)
                    {
                        auctionNoticePrintViewModel.AuctionNoticeNo = Convert.ToString(Result.Tables[0].Rows[0]["AuctionNoticeNo"]);
                        auctionNoticePrintViewModel.DocNo = Convert.ToString(Result.Tables[0].Rows[0]["DocNo"]);
                        auctionNoticePrintViewModel.AuctionNoticeDocNo = Convert.ToString(Result.Tables[0].Rows[0]["AuctionNoticeDocNo"]);
                        auctionNoticePrintViewModel.CompanyAddress = Convert.ToString(Result.Tables[0].Rows[0]["CompanyAddress"]);
                        auctionNoticePrintViewModel.PartyName = Convert.ToString(Result.Tables[0].Rows[0]["PartyName"]);
                        auctionNoticePrintViewModel.PartyAddress = Convert.ToString(Result.Tables[0].Rows[0]["PartyAddress"]);
                        auctionNoticePrintViewModel.NoticeDate = Convert.ToString(Result.Tables[0].Rows[0]["NoticeDate"]);
                        auctionNoticePrintViewModel.AucNtcDaysAftrLanding = Convert.ToString(Result.Tables[0].Rows[0]["AucNtcDaysAftrLanding"]);
                        auctionNoticePrintViewModel.DueDaysAftrAucNtc = Convert.ToString(Result.Tables[0].Rows[0]["DueDaysAftrAucNtc"]);
                        auctionNoticePrintViewModel.GodownLocation = Convert.ToString(Result.Tables[0].Rows[0]["GodownLocation"]);
                        auctionNoticePrintViewModel.AuctionNoticeCC = Convert.ToString(Result.Tables[0].Rows[0]["AuctionNoticeCC"]);
                        auctionNoticePrintViewModel.CommodityName = Convert.ToString(Result.Tables[0].Rows[0]["CommodityName"]);
                        auctionNoticePrintViewModel.OperationType = Convert.ToString(Result.Tables[0].Rows[0]["OperationType"]);
                        auctionNoticePrintViewModel.AuctionType = Convert.ToString(Result.Tables[0].Rows[0]["AuctionType"]);
                        auctionNoticePrintViewModel.Shippingline = Convert.ToString(Result.Tables[0].Rows[0]["Shippingline"]);
                        auctionNoticePrintViewModel.ShippinglineAddress = Convert.ToString(Result.Tables[0].Rows[0]["shippingAddress"]);

                    }

                    foreach (DataRow item in Result.Tables[1].Rows)
                    {
                        Status = 1;
                        WFLD_ParticularsOfGoods objParticularsOfGoods = new WFLD_ParticularsOfGoods();
                        objParticularsOfGoods.ContainerNo = Convert.ToString(item["ContainerNo"]);
                        objParticularsOfGoods.ICDCode = Convert.ToString(item["ICDCode"]);
                        objParticularsOfGoods.EntryDate = Convert.ToString(item["EntryDate"]);
                        objParticularsOfGoods.BLNo = Convert.ToString(item["BLNo"]);
                        objParticularsOfGoods.NoOfPackages = Convert.ToString(item["NoOfPackages"]);
                        objParticularsOfGoods.Weight = Convert.ToString(item["Weight"]);
                        objParticularsOfGoods.ItemNo = Convert.ToString(item["ItemNo"]);
                        objParticularsOfGoods.ShipBillNo = Convert.ToString(item["ShippingBillNo"]);
                        objParticularsOfGoods.CartingDate = Convert.ToString(item["CartingDate"]);
                        objParticularsOfGoods.IGMNO = Convert.ToString(item["IGMNo"]);
                        objParticularsOfGoods.LineNo = Convert.ToString(item["LineNo"]);
                        objParticularsOfGoods.TSA = Convert.ToString(item["TSANo"]);
                        objParticularsOfGoods.Size = Convert.ToString(item["Size"]);

                        auctionNoticePrintViewModel.ParticularsOfGoodsList.Add(objParticularsOfGoods);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = null;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
                    _DBResponse.Data = null;
                }
            }
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

            return auctionNoticePrintViewModel;
        }

        public void GetContainerDetailsListByContainerNo(string operationType, string auctionType, string ContainerNo)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();

            LstParam.Add(new MySqlParameter { ParameterName = "in_ContainerNo", MySqlDbType = MySqlDbType.String, Value = ContainerNo });


            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("GetAuctionContainerDetailsByContainerNo", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            List<WFLD_AuctionNoticeDetails> LstAuctionNoticeDetails = new List<WFLD_AuctionNoticeDetails>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    int srNo = 0;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        srNo++;
                        WFLD_AuctionNoticeDetails objCargoDetails = new WFLD_AuctionNoticeDetails();
                        objCargoDetails.SrNo = srNo;
                        objCargoDetails.NoOfPackages = Convert.ToInt32(item["NoOfPackages"]);
                        objCargoDetails.GodownLocation = Convert.ToString(item["GodownLocation"]);
                        objCargoDetails.BLNo = Convert.ToString(item["BLNo"]);
                        objCargoDetails.BLDate = Convert.ToString(item["BOLDate"]);
                        objCargoDetails.ShipBillDate = Convert.ToString(item["ShippingBillDate"]);
                        objCargoDetails.PartyId = Convert.ToInt32(item["PartyId"]);
                        objCargoDetails.PartyName = Convert.ToString(item["PartyName"]);
                        objCargoDetails.CFSCode = Convert.ToString(item["CFSCode"]);
                        objCargoDetails.BOENo = Convert.ToString(item["BOENo"]);
                        objCargoDetails.CommodityId = Convert.ToInt32(item["CommodityId"]);
                        objCargoDetails.CommodityName = Convert.ToString(item["CommodityName"]);
                        objCargoDetails.Weight = Convert.ToString(item["Weight"]);
                        objCargoDetails.CUM = Convert.ToString(item["CUM"]);
                        objCargoDetails.SQM = Convert.ToString(item["SQM"]);
                        objCargoDetails.Duty = Convert.ToString(item["Duty"]);
                        objCargoDetails.Fob = Convert.ToString(item["Fob"]);
                        objCargoDetails.CIF = Convert.ToString(item["CIF"]);
                        objCargoDetails.IsInsured = Convert.ToString(item["IsInsured"]);
                        objCargoDetails.LineNo = Convert.ToString(item["ShippingLineId"]);
                        objCargoDetails.ShippingBillNo = Convert.ToString(item["ShippingBillNo"]);
                        objCargoDetails.ContainerNo = Convert.ToString(item["ContainerNo"]);
                        objCargoDetails.AuctionEligibleDate = Convert.ToString(item["AuctionEligibleDate"]);
                        objCargoDetails.Size = Convert.ToString(item["Size"]);
                        objCargoDetails.EntryDate = Convert.ToString(item["EntryDate"]);
                        objCargoDetails.GodownID = Convert.ToInt32(item["GodownId"]);
                        objCargoDetails.ShippingLineNo = Convert.ToString(item["ShippingLineId"]);
                        objCargoDetails.CuttingDate = Convert.ToString(item["CuttingDate"]);
                        objCargoDetails.RefId = Convert.ToInt32(item["RefId"]);
                        LstAuctionNoticeDetails.Add(objCargoDetails);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstAuctionNoticeDetails;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
                    _DBResponse.Data = null;
                }
            }
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

            //return LstAuctionNoticeDetails;
        }
        //Get All Auction Notice For Reissue
        public void GetAuctionDetailsForReissue(int AuctionNoticeId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_AuctionNoticesID", MySqlDbType = MySqlDbType.Int32, Value = AuctionNoticeId });
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("GetauctionNoticeforreissue", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            List<WFLD_AuctionNoticeDetails> LstAuctionNoticeDetails = new List<WFLD_AuctionNoticeDetails>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    int srNo = 0;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        srNo++;
                        WFLD_AuctionNoticeDetails objCargoDetails = new WFLD_AuctionNoticeDetails();
                        objCargoDetails.SrNo = srNo;
                        objCargoDetails.NoOfPackages = Convert.ToInt32(item["NoOfPackages"]);
                        objCargoDetails.GodownLocation = Convert.ToString(item["GodownLocation"]);
                        objCargoDetails.BLNo = Convert.ToString(item["BLNo"]);
                        objCargoDetails.BLDate = Convert.ToString(item["BOLDate"]);
                        objCargoDetails.ShipBillDate = Convert.ToString(item["ShippingBillDate"]);
                        objCargoDetails.PartyId = Convert.ToInt32(item["PartyId"]);
                        objCargoDetails.PartyName = Convert.ToString(item["PartyName"]);
                        objCargoDetails.CFSCode = Convert.ToString(item["CFSCode"]);
                        objCargoDetails.BOENo = Convert.ToString(item["BOENo"]);
                        objCargoDetails.CommodityId = Convert.ToInt32(item["CommodityId"]);
                        objCargoDetails.CommodityName = Convert.ToString(item["CommodityName"]);
                        objCargoDetails.Weight = Convert.ToString(item["Weight"]);
                        objCargoDetails.CUM = Convert.ToString(item["CUM"]);
                        objCargoDetails.SQM = Convert.ToString(item["SQM"]);
                        objCargoDetails.Duty = Convert.ToString(item["Duty"]);
                        objCargoDetails.Fob = Convert.ToString(item["Fob"]);
                        objCargoDetails.CIF = Convert.ToString(item["CIF"]);
                        objCargoDetails.IsInsured = Convert.ToString(item["IsInsured"]);
                        objCargoDetails.LineNo = Convert.ToString(item["ShippingLineId"]);
                        objCargoDetails.ShippingBillNo = Convert.ToString(item["ShippingBillNo"]);
                        objCargoDetails.ContainerNo = Convert.ToString(item["ContainerNo"]);
                        objCargoDetails.AuctionEligibleDate = Convert.ToString(item["AuctionEligibleDate"]);
                        objCargoDetails.Size = Convert.ToString(item["Size"]);
                        objCargoDetails.EntryDate = Convert.ToString(item["EntryDate"]);
                        objCargoDetails.GodownID = Convert.ToInt32(item["GodownId"]);
                        objCargoDetails.ShippingLineNo = Convert.ToString(item["ShippingLineId"]);
                        objCargoDetails.CuttingDate = Convert.ToString(item["CuttingDate"]);
                        objCargoDetails.RefId = Convert.ToInt32(item["RefId"]);
                        objCargoDetails.OperationType = Convert.ToString(item["OperationType"]);
                        objCargoDetails.AuctionType = Convert.ToString(item["AuctionType"]);

                        LstAuctionNoticeDetails.Add(objCargoDetails);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstAuctionNoticeDetails;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "No Data Found";
                    _DBResponse.Data = null;
                }
            }
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

            //return LstAuctionNoticeDetails;
        }


       
    }
}