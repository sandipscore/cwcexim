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
    public class Dnd_AuctionRepository
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

            List<Dnd_AuctionNoticeDetails> LstAuctionNoticeDetails = new List<Dnd_AuctionNoticeDetails>();
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
                        Dnd_AuctionNoticeDetails objCargoDetails = new Dnd_AuctionNoticeDetails();
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
                        objCargoDetails.ShippingLineNo = Convert.ToInt32(item["ShippingLineId"]);
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

            List<Dnd_AuctionNoticeDetails> LstAuctionNoticeDetails = new List<Dnd_AuctionNoticeDetails>();
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
                        Dnd_AuctionNoticeDetails objCargoDetails = new Dnd_AuctionNoticeDetails();
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
                        objCargoDetails.ShippingLineNo = Convert.ToInt32(item["ShippingLineId"]);
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
            List<AuctionNoticeItemDetails> LstAuctionNoticeDetails = new List<AuctionNoticeItemDetails>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        AuctionNoticeItemDetails objAuctionNoticeItemDetails = new AuctionNoticeItemDetails();
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
            List<Dnd_AuctionNoticeItemDetails> LstAuctionNoticeDetails = new List<Dnd_AuctionNoticeItemDetails>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Dnd_AuctionNoticeItemDetails objAuctionNoticeItemDetails = new Dnd_AuctionNoticeItemDetails();
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

        public Dnd_AuctionNoticePrintViewModel GetAuctionNoticeDataToPrint(int AuctionNoticeDtlId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "AuctionNoticeDtlId_", MySqlDbType = MySqlDbType.Int32, Value = AuctionNoticeDtlId });

            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("AuctionNoticePrint", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();

            Dnd_AuctionNoticePrintViewModel auctionNoticePrintViewModel = new Dnd_AuctionNoticePrintViewModel();
            auctionNoticePrintViewModel.ParticularsOfGoodsList = new List<Dnd_ParticularsOfGoods>();

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
                        Dnd_ParticularsOfGoods objParticularsOfGoods = new Dnd_ParticularsOfGoods();
                        objParticularsOfGoods.ContainerNo = Convert.ToString(item["ContainerNo"]);
                        objParticularsOfGoods.ICDCode = Convert.ToString(item["ICDCode"]);
                        objParticularsOfGoods.EntryDate = Convert.ToString(item["EntryDate"]);
                        objParticularsOfGoods.BLNo = Convert.ToString(item["BLNo"]);
                        objParticularsOfGoods.NoOfPackages = Convert.ToString(item["NoOfPackages"]);
                        objParticularsOfGoods.Weight = Convert.ToString(item["Weight"]);
                        objParticularsOfGoods.ItemNo = Convert.ToString(item["ItemNo"]);
                        objParticularsOfGoods.ShipBillNo = Convert.ToString(item["ShippingBillNo"]);
                        objParticularsOfGoods.CartingDate = Convert.ToString(item["CartingDate"]);


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

            List<Dnd_AuctionNoticeDetails> LstAuctionNoticeDetails = new List<Dnd_AuctionNoticeDetails>();
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
                        Dnd_AuctionNoticeDetails objCargoDetails = new Dnd_AuctionNoticeDetails();
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
                        objCargoDetails.ShippingLineNo = Convert.ToInt32(item["ShippingLineId"]);
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

            List<Dnd_AuctionNoticeDetails> LstAuctionNoticeDetails = new List<Dnd_AuctionNoticeDetails>();
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
                        Dnd_AuctionNoticeDetails objCargoDetails = new Dnd_AuctionNoticeDetails();
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
                        objCargoDetails.ShippingLineNo = Convert.ToInt32(item["ShippingLineId"]);
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


        public void GetAllMarkForNotice(int branchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "BranchId_", MySqlDbType = MySqlDbType.Int32, Value = branchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("AuctionGetDataForMark", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_MarkForNotice> LstMarkForNotice = new List<Dnd_MarkForNotice>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        var ifExistMarkForNotice = LstMarkForNotice.Where(x => x.AuctionNoticeId == Convert.ToInt32(item["AuctionNoticeId"])).FirstOrDefault();

                        if (ifExistMarkForNotice == null)
                        {
                            Dnd_MarkForNotice objMarkForNotice = new Dnd_MarkForNotice();
                            objMarkForNotice.MarkForNoticeDetailsList = new List<Dnd_MarkForNoticeDetails>();
                            objMarkForNotice.AuctionNoticeId = Convert.ToInt32(item["AuctionNoticeId"]);
                            objMarkForNotice.NoticeNo = Convert.ToString(item["AuctionNoticeNo"]);
                            objMarkForNotice.AuctionNoticeDate = Convert.ToString(item["AuctionNoticeDate"]);
                            objMarkForNotice.PartyName = Convert.ToString(item["PartyName"]);

                            Dnd_MarkForNoticeDetails objMarkForNoticeDetails = new Dnd_MarkForNoticeDetails();
                            objMarkForNoticeDetails.AuctionNoticeDtlId = Convert.ToInt32(item["AuctionNoticeDtlId"]);
                            objMarkForNoticeDetails.Weight = Convert.ToString(item["Weight"]);
                            objMarkForNoticeDetails.CommodityName = Convert.ToString(item["CommodityName"]);
                            objMarkForNotice.MarkForNoticeDetailsList.Add(objMarkForNoticeDetails);

                            LstMarkForNotice.Add(objMarkForNotice);
                        }
                        else
                        {
                            Dnd_MarkForNoticeDetails objMarkForNoticeDetails = new Dnd_MarkForNoticeDetails();
                            objMarkForNoticeDetails.AuctionNoticeDtlId = Convert.ToInt32(item["AuctionNoticeDtlId"]);
                            objMarkForNoticeDetails.Weight = Convert.ToString(item["Weight"]);
                            objMarkForNoticeDetails.CommodityName = Convert.ToString(item["CommodityName"]);
                            foreach (var itemLstMarkForNotice in LstMarkForNotice.Where(x => x.AuctionNoticeId == Convert.ToInt32(item["AuctionNoticeId"])).ToList())
                            {
                                itemLstMarkForNotice.MarkForNoticeDetailsList.Add(objMarkForNoticeDetails);
                            }

                        }

                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstMarkForNotice;
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
            }
        }


        #region update despatch

        public void GetAllDespatchForNotice(int branchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = branchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("GetAuctionListForDespatch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_MarkForNotice> LstMarkForNotice = new List<Dnd_MarkForNotice>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        LstMarkForNotice.Add(new Dnd_MarkForNotice
                        {
                            AuctionNoticeId = Convert.ToInt32(item["AuctionNoticeId"]),
                            NoticeNo = Convert.ToString(item["AuctionNoticeNo"]),
                        });
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstMarkForNotice;
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
            }
        }


        public void UpdateDespatch(Dnd_Despatch vm)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_DespatchNo", MySqlDbType = MySqlDbType.String, Value = vm.DespatchNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DespatchDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(vm.DespatchDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AuctionId", MySqlDbType = MySqlDbType.String, Value = vm.NoticeID });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedInvoiceId", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("UpdateAuctionNoticeDespatch", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Data Saved Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Despatch No Exist";
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


        public void GetAllDespatch(int branchId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = branchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("GetDespatchList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Despatch> LstDespatchNotice = new List<Despatch>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        LstDespatchNotice.Add(new Despatch
                        {
                            AuctionNo = Convert.ToString(item["AuctionNoticeNo"]),
                            DespatchNo = Convert.ToString(item["DespatchNo"]),
                            DespatchDate = Convert.ToString(item["DespatchDate"])
                        });

                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstDespatchNotice;
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
            }
        }


        #endregion

        #region NOC Details

        public void GetAllShipbillOBLFor(int Flag)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Flag", MySqlDbType = MySqlDbType.Int32, Value = Flag });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("GetShipbillOBLForNoc", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_NocDetails> LstNocDetails = new List<Dnd_NocDetails>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        LstNocDetails.Add(new Dnd_NocDetails
                        {
                            RefNo = Convert.ToString(item["RefNo"]),
                            RefDate = Convert.ToDateTime(item["RefDate"]).ToShortDateString()
                        });

                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstNocDetails;
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
            }
        }
        public void AddEditNOCDetails(Dnd_NocDetails vm, int BranchID, int CRby)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_NocID", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(vm.NocID) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NocNo", MySqlDbType = MySqlDbType.String, Value = Convert.ToString(vm.NocNo) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_NocDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(vm.NocDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RefNo", MySqlDbType = MySqlDbType.String, Value = vm.RefNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RefDate", MySqlDbType = MySqlDbType.Date, Value = Convert.ToDateTime(vm.RefDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Flag", MySqlDbType = MySqlDbType.Int32, Value = vm.Flag });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AuctionDestruction", MySqlDbType = MySqlDbType.String, Value = Convert.ToString(vm.AuctionDestruction) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks1", MySqlDbType = MySqlDbType.String, Value = Convert.ToString(vm.Remarks1) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Remarks2", MySqlDbType = MySqlDbType.String, Value = Convert.ToString(vm.Remarks2) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CRBy", MySqlDbType = MySqlDbType.Int32, Value = CRby });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Value = "", Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            string GeneratedClientId = "";
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditNocDetails", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Data Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "NOC No Exist";
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

        public void GetAllNocDetails(int NocId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_NocId", MySqlDbType = MySqlDbType.Int32, Value = NocId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataSet Result = DataAccess.ExecuteDataSet("GetNocDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_NocDetails> LstNocDetails = new List<Dnd_NocDetails>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        LstNocDetails.Add(new Dnd_NocDetails
                        {
                            NocID = Convert.ToInt32(item["NocID"]),
                            NocNo = Convert.ToString(item["NocNo"]),
                            NocDate = Convert.ToString(item["NocDate"]),
                            RefDate = Convert.ToString(item["RefDate"]),
                            Flag = Convert.ToInt32(item["IsOblSB"]),
                            RefNo = Convert.ToString(item["RefNo"]),
                            Remarks1 = Convert.ToString(item["Remarks1"]),
                            Remarks2 = Convert.ToString(item["Remarks2"]),
                            AuctionDestruction = Convert.ToString(item["AuctionDestruction"])
                        });

                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstNocDetails;
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
            }
        }

        public void DeleteNocDetails(int NocId)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_NocID", MySqlDbType = MySqlDbType.Int32, Value = NocId });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            string GeneratedClientId = "";
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteNocDetails", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "NOC Details Delete Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if (Result == 2)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "NOC No Exist";
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


        public void SaveMarkForNotice(string markIds, int markedBy, string LotNo)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "AuctionId_CSV", MySqlDbType = MySqlDbType.String, Value = markIds });
            LstParam.Add(new MySqlParameter { ParameterName = "MarkedBy_", MySqlDbType = MySqlDbType.Int32, Value = markedBy });
            LstParam.Add(new MySqlParameter { ParameterName = "in_LotNo", MySqlDbType = MySqlDbType.String, Value = LotNo });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Value = 0, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AuctionUpdateMark", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Data Saved Successfully";
                    _DBResponse.Data = null;
                }
                else if (Result == 3)
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Lot No Exist";
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


        public void GetAuctionMarkedNoticeList()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("AuctionGetMarkedData", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_MarkedNotice> LstMarkedNotice = new List<Dnd_MarkedNotice>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        var ifExistsMarkedNotice = LstMarkedNotice.Where(x => x.LOTNo == Convert.ToString(item["LOTNo"])).FirstOrDefault();

                        if (ifExistsMarkedNotice == null)
                        {
                            Dnd_MarkedNotice objMarkedNotice = new Dnd_MarkedNotice();
                            objMarkedNotice.MarkedNoticeDetailsList = new List<Dnd_MarkedNoticeDetails>();
                            objMarkedNotice.LOTNo = Convert.ToString(item["LOTNo"]);
                            objMarkedNotice.MarkedOn = Convert.ToString(item["MarkedOn"]);

                            Dnd_MarkedNoticeDetails objMarkedNoticeDetails = new Dnd_MarkedNoticeDetails();
                            objMarkedNoticeDetails.AuctionNoticeId = Convert.ToInt32(item["AuctionNoticeId"]);
                            objMarkedNoticeDetails.AuctionNoticeDate = Convert.ToString(item["AuctionNoticeDate"]);
                            objMarkedNoticeDetails.NoticeNo = Convert.ToString(item["AuctionNoticeNo"]);
                            objMarkedNoticeDetails.PartyName = Convert.ToString(item["PartyName"]);
                            objMarkedNoticeDetails.OBL = Convert.ToString(item["BLNo"]);
                            objMarkedNoticeDetails.Shipbill = Convert.ToString(item["ShippingBillNo"]);
                            objMarkedNotice.MarkedNoticeDetailsList.Add(objMarkedNoticeDetails);

                            LstMarkedNotice.Add(objMarkedNotice);
                        }
                        else
                        {
                            Dnd_MarkedNoticeDetails objMarkedNoticeDetails = new Dnd_MarkedNoticeDetails();
                            objMarkedNoticeDetails.AuctionNoticeId = Convert.ToInt32(item["AuctionNoticeId"]);
                            objMarkedNoticeDetails.AuctionNoticeDate = Convert.ToString(item["AuctionNoticeDate"]);
                            objMarkedNoticeDetails.NoticeNo = Convert.ToString(item["AuctionNoticeNo"]);
                            objMarkedNoticeDetails.PartyName = Convert.ToString(item["PartyName"]);
                            objMarkedNoticeDetails.OBL = Convert.ToString(item["BLNo"]);
                            objMarkedNoticeDetails.Shipbill = Convert.ToString(item["ShippingBillNo"]);
                            foreach (var itemLstMarkForNotice in LstMarkedNotice.Where(x => x.LOTNo == Convert.ToString(item["LOTNo"])).ToList())
                            {
                                itemLstMarkForNotice.MarkedNoticeDetailsList.Add(objMarkedNoticeDetails);
                            }

                        }

                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstMarkedNotice;
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
        public void GetBIDList(int branchId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "BranchId_", MySqlDbType = MySqlDbType.Int32, Value = branchId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("AuctionGetPendingBidNos", CommandType.StoredProcedure, DParam);
            var model = new Dnd_EMDReceived();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.BIDDetail.Add(new Dnd_BIDInfo { BIDId = Convert.ToInt32(Result["BidIdHdr"]), BIDNo = Convert.ToString(Result["BidNo"]) });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = model;
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
        public void GetBIDListForInvoice(int branchId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "BranchId_", MySqlDbType = MySqlDbType.Int32, Value = branchId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("AuctionGetBidNosForInv", CommandType.StoredProcedure, DParam);
            var model = new Dnd_EMDReceived();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    model.BIDDetail.Add(new Dnd_BIDInfo { BIDId = Convert.ToInt32(Result["BidIdHdr"]), BIDNo = Convert.ToString(Result["BidNo"]) });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = model;
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

        public void GetBIDDetails(int BIDId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "BidIdHdr_", MySqlDbType = MySqlDbType.Int32, Value = BIDId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("AuctionGetPendingBidDtls", CommandType.StoredProcedure, DParam);
            var model = new Dnd_EMDReceived();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    model.BIDId = Convert.ToInt32(Result["BidId"]);
                    model.BIDNo = Convert.ToString(Result["BidNo"]);
                    model.BIDAmount = Convert.ToString(Result["BidAmount"]);
                    model.EMDAmount = Convert.ToString(Result["EmdAmount"]);
                    model.PartyName = Convert.ToString(Result["Party"]);
                    model.AuctionDate = Convert.ToString(Result["AuctionDate"]);
                    model.PartyId = Convert.ToInt32(Result["PartyId"]);
                    model.PartyAddress = Convert.ToString(Result["Address"]);

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = model;
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

        public void GetBIDDetailsForInvoice(int BIDId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "BidIdHdr_", MySqlDbType = MySqlDbType.Int32, Value = BIDId });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("AuctionGetInvBidDtls", CommandType.StoredProcedure, DParam);
            var model = new AuctionInvoice();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    model.BIDId = Convert.ToInt32(Result["BidIdHdr"]);
                    model.BIDNo = Convert.ToString(Result["BidNo"]);
                    model.ReceiptDate = Convert.ToString(Result["BidDate"]);
                    model.PartyId = Convert.ToInt32(Result["PartyId"]);
                    model.PartyName = Convert.ToString(Result["Party"]);
                    model.PartyAddress = Convert.ToString(Result["Address"]);
                    model.GSTNo = Convert.ToString(Result["GSTNo"]);
                    model.State = Convert.ToString(Result["StateName"]);
                    model.StateCode = Convert.ToString(Result["StateCode"]);
                    model.InvoiceType = "Tax";
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = model;
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

        public void SaveEMDReceived(Dnd_EMDReceived ObjEMDReceived, string xml)
        {
            try
            {
                DateTime dt = DateTime.ParseExact(ObjEMDReceived.ReceiptDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                String ReceiptDate = dt.ToString("yyyy-MM-dd");

                DateTime dtt = DateTime.ParseExact(ObjEMDReceived.ValidUpTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                String ValidUpto = dtt.ToString("yyyy-MM-dd");

                string GeneratedClientId = "";

                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "BranchId_", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
                LstParam.Add(new MySqlParameter { ParameterName = "EmdRcvdDate_", MySqlDbType = MySqlDbType.DateTime, Value = ReceiptDate });
                LstParam.Add(new MySqlParameter { ParameterName = "BidIdHdr_", MySqlDbType = MySqlDbType.Int32, Value = ObjEMDReceived.BIDId });
                LstParam.Add(new MySqlParameter { ParameterName = "in_ValidUpto", MySqlDbType = MySqlDbType.Date, Value = ValidUpto });
                LstParam.Add(new MySqlParameter { ParameterName = "PartyId_", MySqlDbType = MySqlDbType.Int32, Value = ObjEMDReceived.PartyId });
                LstParam.Add(new MySqlParameter { ParameterName = "PartyName_", MySqlDbType = MySqlDbType.String, Value = ObjEMDReceived.PartyName });
                LstParam.Add(new MySqlParameter { ParameterName = "PartyAddress_", MySqlDbType = MySqlDbType.String, Value = ObjEMDReceived.PartyAddress });
                LstParam.Add(new MySqlParameter { ParameterName = "TotEmdRcvdAmt_", MySqlDbType = MySqlDbType.Decimal, Value = ObjEMDReceived.EMDAmount });
                LstParam.Add(new MySqlParameter { ParameterName = "in_AdvanceAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjEMDReceived.AdvancePaid });
                LstParam.Add(new MySqlParameter { ParameterName = "CreatedBy_", MySqlDbType = MySqlDbType.Int32, Value = ((Login)HttpContext.Current.Session["LoginUser"]).Uid });
                LstParam.Add(new MySqlParameter { ParameterName = "xmlCashReceipt", MySqlDbType = MySqlDbType.String, Value = xml });
                LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.String, Direction = ParameterDirection.Output, Value = GeneratedClientId });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                int Result = DataAccess.ExecuteNonQuery("AuctionInsertEmdReceipt", CommandType.StoredProcedure, DParam, out GeneratedClientId);
                _DBResponse = new DatabaseResponse();

                if (Result == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "EMD Received Successfully.";
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

        public void GetEMDReceivedList()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "BranchId_", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(HttpContext.Current.Session["BranchId"]) });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            DataSet Result = DataAccess.ExecuteDataSet("AuctionGetEmdRcvdList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_EMDReceived> LstEMDReceived = new List<Dnd_EMDReceived>();
            try
            {
                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;

                    foreach (DataRow item in Result.Tables[0].Rows)
                    {
                        Status = 1;
                        Dnd_EMDReceived objEMDReceived = new Dnd_EMDReceived();
                        objEMDReceived.EMDReceivedId = Convert.ToInt32(item["EmdRcvdId"]);
                        objEMDReceived.EMDReceivedNo = Convert.ToString(item["EmdRcvdNo"]);
                        objEMDReceived.BIDNo = Convert.ToString(item["BidNo"]);
                        objEMDReceived.PartyName = Convert.ToString(item["PartyName"]);
                        objEMDReceived.EMDAmount = Convert.ToString(item["TotEmdRcvdAmt"]);
                        objEMDReceived.OBL = Convert.ToString(item["OBL"]);
                        objEMDReceived.ShippBillNo = Convert.ToString(item["ShippBill"]);
                        objEMDReceived.ValidUpTo = Convert.ToString(item["ValidUpto"]);
                        objEMDReceived.AdvancePaid = Convert.ToDecimal(item["AdvanceAmount"]);
                        objEMDReceived.Container = Convert.ToString(item["Container"]);
                        LstEMDReceived.Add(objEMDReceived);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = LstEMDReceived;
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

        public void GetAuctionPrintData(int BIDId)
        {
            DataSet Result = null;
            try
            {
                int Status = 0;
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                LstParam.Add(new MySqlParameter { ParameterName = "in_BidIdHdr", MySqlDbType = MySqlDbType.Int32, Value = BIDId });
                IDataParameter[] DParam = { };
                DParam = LstParam.ToArray();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                Result = DataAccess.ExecuteDataSet("AuctionPrintInvoice", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                AuctionInvoicePrint objAuctionInvoicePrint = new AuctionInvoicePrint();

                if (Result != null && Result.Tables.Count > 0)
                {
                    Status = 1;
                    if (Result.Tables[0] != null)
                    {
                        objAuctionInvoicePrint.CompanyAddress = Convert.ToString(Result.Tables[0].Rows[0]["CompanyAddress"]);
                        objAuctionInvoicePrint.CwcGstNo = Convert.ToString(Result.Tables[0].Rows[0]["CompGSTNo"]);
                        objAuctionInvoicePrint.InvoiceNo = Convert.ToString(Result.Tables[0].Rows[0]["InvoiceNo"]);
                        objAuctionInvoicePrint.TaxInvoiceDate = Convert.ToString(Result.Tables[0].Rows[0]["InvoiceDate"]);
                        objAuctionInvoicePrint.PartyName = Convert.ToString(Result.Tables[0].Rows[0]["PartyName"]);
                        objAuctionInvoicePrint.PartyAddress = Convert.ToString(Result.Tables[0].Rows[0]["PartyAddress"]);
                        objAuctionInvoicePrint.PartyGstNo = Convert.ToString(Result.Tables[0].Rows[0]["PartyGSTNo"]);
                        objAuctionInvoicePrint.StateShortCode = Convert.ToString(Result.Tables[0].Rows[0]["StateShortCode"]);
                        objAuctionInvoicePrint.StateName = Convert.ToString(Result.Tables[0].Rows[0]["StateName"]);
                        objAuctionInvoicePrint.AuctionAsmNo = Convert.ToString(Result.Tables[0].Rows[0]["AuctionNoticeNo"]);
                        objAuctionInvoicePrint.AuctionDate = Convert.ToString(Result.Tables[0].Rows[0]["AuctionNoticeDate"]);
                        objAuctionInvoicePrint.LotNo = Convert.ToString(Result.Tables[0].Rows[0]["LotNo"]);
                        objAuctionInvoicePrint.ShedNo = Convert.ToString(Result.Tables[0].Rows[0]["ShedNo"]);
                        objAuctionInvoicePrint.SLine = Convert.ToString(Result.Tables[0].Rows[0]["SLine"]);
                    }

                    if (Result.Tables[1] != null)
                    {
                        objAuctionInvoicePrint.IgmNo = Convert.ToString(Result.Tables[1].Rows[0]["IGMNo"]);
                        objAuctionInvoicePrint.OblNo = Convert.ToString(Result.Tables[1].Rows[0]["BLNo"]);
                        objAuctionInvoicePrint.ItemNo = Convert.ToString(Result.Tables[1].Rows[0]["ItemNo"]);
                        objAuctionInvoicePrint.BOENo = Convert.ToString(Result.Tables[1].Rows[0]["BOENo"]);
                        objAuctionInvoicePrint.BOEDate = Convert.ToString(Result.Tables[1].Rows[0]["BOEDate"]);
                        objAuctionInvoicePrint.BidderName = Convert.ToString(Result.Tables[1].Rows[0]["PartyName"]);
                        objAuctionInvoicePrint.BidderAddress = Convert.ToString(Result.Tables[1].Rows[0]["PartyAddress"]);
                        objAuctionInvoicePrint.BidderStateName = Convert.ToString(Result.Tables[1].Rows[0]["StateName"]);
                        objAuctionInvoicePrint.Value = Convert.ToString(Result.Tables[1].Rows[0]["cValue"]);
                        objAuctionInvoicePrint.Duty = Convert.ToString(Result.Tables[1].Rows[0]["Duty"]);
                        objAuctionInvoicePrint.NatureOfCargo = Convert.ToString(Result.Tables[1].Rows[0]["CargoNature"]);
                        objAuctionInvoicePrint.NoOfPkg = Convert.ToString(Result.Tables[1].Rows[0]["NoOfPackages"]);
                        objAuctionInvoicePrint.TotalArea = Convert.ToString(Result.Tables[1].Rows[0]["TotalArea"]);
                        objAuctionInvoicePrint.TotalWt = Convert.ToString(Result.Tables[1].Rows[0]["Weight"]);
                        objAuctionInvoicePrint.TypeOfCargo = Convert.ToString(Result.Tables[1].Rows[0]["CargoType"]);
                    }

                    foreach (DataRow item in Result.Tables[2].Rows)
                    {
                        AuctionAccDetails objAuctionAccDetails = new AuctionAccDetails();
                        objAuctionAccDetails.BidAmount = Convert.ToString(item["BidAmount"]);
                        objAuctionAccDetails.GSTPercectage = Convert.ToString(item["GSTpct"]);
                        objAuctionAccDetails.CGST = Convert.ToString(item["CGSTAmt"]);
                        objAuctionAccDetails.SGST = Convert.ToString(item["SGSTAmt"]);
                        objAuctionAccDetails.IGST = Convert.ToString(item["IGSTAmt"]);
                        objAuctionAccDetails.TotalDues = Convert.ToString(item["TotalDues"]);
                        objAuctionAccDetails.EMDPaid = Convert.ToString(item["TotEmdRcvdAmt"]);
                        objAuctionAccDetails.EMDPaidReceiptrNo = Convert.ToString(item["EmdRcvdNo"]) + "<br/>" + Convert.ToString(item["EmdRcvdDate"]);
                        objAuctionAccDetails.AdvPaid = Convert.ToString(item["AdvTotEmdRcvdAmt"]);
                        objAuctionAccDetails.AdvPaidReceiptrNo = Convert.ToString(item["AdvEmdRcvdNo"]) + "<br/>" + Convert.ToString(item["AdvEmdRcvdDate"]);
                        objAuctionAccDetails.NetAmountPayable = Convert.ToString(item["TotDues_minus_TotRcvd"]);

                        objAuctionInvoicePrint.AuctionAccDetailsList.Add(objAuctionAccDetails);
                    }

                    foreach (DataRow item in Result.Tables[3].Rows)
                    {
                        ContainerDetails objContainerDetails = new ContainerDetails();
                        objContainerDetails.CFSCode = Convert.ToString(item["CFSCode"]);
                        objContainerDetails.ContainerNo = Convert.ToString(item["ContainerNo"]);
                        objContainerDetails.Size = Convert.ToString(item["Size"]);
                        objContainerDetails.DateOfArrival = Convert.ToString(item["EntryDateTime"]);
                        objContainerDetails.FreeDtFrom = Convert.ToString(item["FreeFrom"]);
                        objContainerDetails.FreeDtUpto = Convert.ToString(item["FreeUpto"]);
                        objContainerDetails.DateOfDelivery = objAuctionInvoicePrint.TaxInvoiceDate;
                        objContainerDetails.NoOfDays = Convert.ToString(item["NoOfDays"]);
                        objContainerDetails.NoOfWeek = Convert.ToString(item["NoWeek"]);

                        objAuctionInvoicePrint.ContainerDetailsList.Add(objContainerDetails);
                    }

                    foreach (DataRow item in Result.Tables[4].Rows)
                    {
                        ConatinerCharge objConatinerCharge = new ConatinerCharge();
                        objConatinerCharge.ChargeCode = Convert.ToString(item["ChargeCode"]);
                        objConatinerCharge.Description = Convert.ToString(item["ChargeName"]);
                        objConatinerCharge.SACode = Convert.ToString(item["SACCode"]);
                        objConatinerCharge.TaxableAmt = Convert.ToString(item["Taxable"]);
                        objConatinerCharge.IGSTPer = Convert.ToString(item["IGSTPer"]);
                        objConatinerCharge.IGSTAmt = Convert.ToString(item["IGSTAmt"]);
                        objConatinerCharge.CGSTPer = Convert.ToString(item["CGSTPer"]);
                        objConatinerCharge.CGSTAmt = Convert.ToString(item["CGSTAmt"]);
                        objConatinerCharge.SGSTPer = Convert.ToString(item["SGSTPer"]);
                        objConatinerCharge.SGSTAmt = Convert.ToString(item["SGSTAmt"]);
                        objConatinerCharge.Total = Convert.ToString(item["Total"]);

                        objAuctionInvoicePrint.ConatinerChargeList.Add(objConatinerCharge);
                    }
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objAuctionInvoicePrint;
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
            }
        }

        #region Bid
        public void GetBidPartyAndDetails()
        {
            //ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            //int BranchId = Convert.ToInt32(Session["BranchId"]);
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMarkedParty", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Areas.Auction.Models.Dnd_PartyDetails> lstPartyDetails = new List<Areas.Auction.Models.Dnd_PartyDetails>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstPartyDetails.Add(new Areas.Auction.Models.Dnd_PartyDetails
                    {
                        //Convert.ToInt32(Result["PartyId"] == DBNull.Value ? 0 : Result["PartyId"]),
                        // AuctionId = Convert.ToInt32(Result["AuctionNoticeId"] == DBNull.Value ? 0 : Result["AuctionNoticeId"]),
                        //AuctionNumber = Result["AuctionNoticeNo"].ToString(),
                        PartyId = Convert.ToInt32(Result["EximTraderId"]),
                        Party = Result["EximTraderName"].ToString(),
                        Address = Result["Address"].ToString(),
                        GstNo = Result["GSTNo"].ToString(),
                        Pan = Result["Pan"].ToString()


                    });


                    //ViewBag.strTime = objEntryThroughGate.EntryTime;



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstPartyDetails;
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

        public void GetListShippBillOBLByNOC(int Flag)//,int PartyID
        {
            //ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            //int BranchId = Convert.ToInt32(Session["BranchId"]);
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Flag", MySqlDbType = MySqlDbType.Int32, Value = Flag });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_PartyID", MySqlDbType = MySqlDbType.Int32, Value = PartyID });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLShippBillListByNOC", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<AucBidFinalizationDtl> lstAucBidFinalizationDtl = new List<AucBidFinalizationDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstAucBidFinalizationDtl.Add(new AucBidFinalizationDtl
                    {
                        RefNo = Convert.ToString(Result["RefNo"]),
                        RefDate = Convert.ToString(Result["RefDate"])

                    });


                    //ViewBag.strTime = objEntryThroughGate.EntryTime;



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstAucBidFinalizationDtl;
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

        public void GetListByNotice(int BranchId, string LotNo)//,int PartyID
        {
            //ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            //int BranchId = Convert.ToInt32(Session["BranchId"]);
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_LotNO", MySqlDbType = MySqlDbType.VarString, Value = LotNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_PartyID", MySqlDbType = MySqlDbType.Int32, Value = PartyID });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetMarkedNotice", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<AucBidFinalizationDtl> lstAucBidFinalizationDtl = new List<AucBidFinalizationDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstAucBidFinalizationDtl.Add(new AucBidFinalizationDtl
                    {

                        //BidFinalizationID = Convert.ToInt32(Result["AuctionNoticeId"] == DBNull.Value ? 0 : Result["AuctionNoticeId"]),
                        NoticeID = Convert.ToInt32(Result["AuctionNoticeId"] == DBNull.Value ? 0 : Result["AuctionNoticeId"]),
                        NoticeNumber = Result["AuctionNoticeNo"].ToString(),
                        Size = Result["Size"].ToString(),
                        ContainerNo = Result["ContainerNo"].ToString(),
                        Boe = Result["BOENo"].ToString(),
                        Commodity = Result["CommodityName"].ToString(),
                        //Weight = Result["Weight"].ToString(),
                        cum = Result["CUM"].ToString(),
                        sqm = Result["SQM"].ToString(),
                        BOLNo = Result["BLNo"].ToString(),
                        BolDate = Result["BLDate"].ToString(),
                        ShipBillDate = Result["ShipBillDate"].ToString(),
                        ShipBillNo = Result["ShippingBillNo"].ToString()


                    });


                    //ViewBag.strTime = objEntryThroughGate.EntryTime;



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstAucBidFinalizationDtl;
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
        public void GetListShippBillOBLForBid(int Flag)//,int PartyID
        {
            //ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            //int BranchId = Convert.ToInt32(Session["BranchId"]);
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Flag", MySqlDbType = MySqlDbType.Int32, Value = Flag });
            //LstParam.Add(new MySqlParameter { ParameterName = "in_BranchId", MySqlDbType = MySqlDbType.Int32, Value = BranchId });
            // LstParam.Add(new MySqlParameter { ParameterName = "in_PartyID", MySqlDbType = MySqlDbType.Int32, Value = PartyID });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetOBLShippBillListByNOC", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_AucBidFinalizationDtl> lstAucBidFinalizationDtl = new List<Dnd_AucBidFinalizationDtl>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstAucBidFinalizationDtl.Add(new Dnd_AucBidFinalizationDtl
                    {
                        RefNo = Convert.ToString(Result["RefNo"]),
                        RefDate = Convert.ToString(Result["RefDate"]),
                        Area = Convert.ToDecimal(Result["SQM"]),
                        CIF = Convert.ToDecimal(Result["CIF"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        Duty = Convert.ToDecimal(Result["Duty"]),
                        Fob = Convert.ToDecimal(Result["Fob"]),
                        EntryDate = Convert.ToString(Result["EntryDate"]),
                        Refid = Convert.ToInt32(Result["RefId"]),
                        CommodityID = Convert.ToInt32(Result["CommodityId"]),
                        Size = Convert.ToString(Result["Size"]),
                        Noofpkg = Convert.ToInt32(Result["NoOfPackages"]),
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        GodownID = Convert.ToInt32(Result["GodownId"]),
                        ShippingId = Convert.ToInt32(Result["LineNo"]),
                        ImporterID = Convert.ToInt32(Result["PartyID"]),
                    });


                    //ViewBag.strTime = objEntryThroughGate.EntryTime;



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstAucBidFinalizationDtl;
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

        public void SaveBid(Dnd_AucBidFinalizationHdr ObjAucBidFinalizationHdr, int Uid, int BranchId)
        {

            DateTime dt = DateTime.ParseExact(ObjAucBidFinalizationHdr.BidDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String BidDate = dt.ToString("yyyy-MM-dd");

            DateTime dtt = DateTime.ParseExact(ObjAucBidFinalizationHdr.AuctionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String AuctionDate = dtt.ToString("yyyy-MM-dd");

            DateTime dttt = DateTime.ParseExact(ObjAucBidFinalizationHdr.RefDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String RefDate = dtt.ToString("yyyy-MM-dd");
            DateTime dtttt = DateTime.ParseExact(ObjAucBidFinalizationHdr.EntryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            String EntryDate = dtttt.ToString("yyyy-MM-dd");




            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Bid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjAucBidFinalizationHdr.BidIdHdr) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BidDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(BidDate) });//ObjAucBidFinalizationHdr.BidDate
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjAucBidFinalizationHdr.PartyId) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.VarChar, Size = 50, Value = ObjAucBidFinalizationHdr.Party });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Address", MySqlDbType = MySqlDbType.VarChar, Size = 250, Value = ObjAucBidFinalizationHdr.Address });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GstNo", MySqlDbType = MySqlDbType.VarChar, Value = ObjAucBidFinalizationHdr.GstNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BidAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjAucBidFinalizationHdr.BidAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EmdAmount", MySqlDbType = MySqlDbType.Decimal, Value = ObjAucBidFinalizationHdr.EmdAmount });
            LstParam.Add(new MySqlParameter { ParameterName = "in_uid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(Uid) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_AuctionDate", MySqlDbType = MySqlDbType.Date, Value = AuctionDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RefNo", MySqlDbType = MySqlDbType.String, Value = ObjAucBidFinalizationHdr.RefNo });
            LstParam.Add(new MySqlParameter { ParameterName = "in_RefDate", MySqlDbType = MySqlDbType.Date, Value = RefDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsObLShipp", MySqlDbType = MySqlDbType.Int32, Value = ObjAucBidFinalizationHdr.RefFlag });

            LstParam.Add(new MySqlParameter { ParameterName = "in_CFSCode", MySqlDbType = MySqlDbType.String, Value = ObjAucBidFinalizationHdr.CFSCode });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CommodityID", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjAucBidFinalizationHdr.CommodityID) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Fob", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjAucBidFinalizationHdr.Fob) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Duty", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjAucBidFinalizationHdr.Duty) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CIF", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjAucBidFinalizationHdr.CIF) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Area", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(ObjAucBidFinalizationHdr.Area) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Refid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjAucBidFinalizationHdr.Refid) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Size", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjAucBidFinalizationHdr.Size) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Noofpkg", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjAucBidFinalizationHdr.Noofpkg) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EntryDate", MySqlDbType = MySqlDbType.Date, Value = EntryDate });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToDecimal(ObjAucBidFinalizationHdr.Weight) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GodownID", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjAucBidFinalizationHdr.GodownID) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExporterID", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjAucBidFinalizationHdr.ImporterID) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ShippingLileID", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(ObjAucBidFinalizationHdr.ShippingId) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output });




            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditBidFinalization", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Bid Details Saved Successfully";
                    //: "Exit Through Gate Updated Successfully
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Bid Details Updated Successfully";
                    //: "Exit Through Gate Updated Successfully
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

        public void GetBidLIst(int id)
        {
            //ContainerNumber = ContainerNumber.Trim();
            int Status = 0;
            //int BranchId = Convert.ToInt32(Session["BranchId"]);
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_BidID", MySqlDbType = MySqlDbType.Int32, Value = id });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("BidList", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_AucBidFinalizationHdr> lstAucBidFinalizationHdr = new List<Dnd_AucBidFinalizationHdr>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstAucBidFinalizationHdr.Add(new Dnd_AucBidFinalizationHdr
                    {
                        //Convert.ToInt32(Result["PartyId"] == DBNull.Value ? 0 : Result["PartyId"]),
                        BidIdHdr = Convert.ToInt32(Result["BidIdHdr"] == DBNull.Value ? 0 : Result["BidIdHdr"]),
                        BidNo = Result["BidNo"].ToString(),
                        BidDate = (Result["BidDate"] == DBNull.Value ? "" : Result["BidDate"]).ToString(),
                        //Convert.ToDateTime(Result["BidDate"] == DBNull.Value ? "N/A" : Result["BidDate"]).ToString(),//.ToString("MM/dd/yyyy")
                        Party = Result["Party"].ToString(),
                        BidAmount = Convert.ToDecimal(Result["BidAmount"] == DBNull.Value ? "0" : Result["BidAmount"].ToString()),
                        EmdAmount = Convert.ToDecimal(Result["EmdAmount"] == DBNull.Value ? "0" : Result["EmdAmount"].ToString()),
                        AuctionDate = Convert.ToString(Result["AuctionDate"]),
                        OBL = Convert.ToString(Result["OBLNo"]),
                        Shipbill = Convert.ToString(Result["ShippingBillNo"]),
                        Container = Convert.ToString(Result["Container"]),
                        RefNo = Convert.ToString(Result["RefNo"]),
                        RefFlag = Convert.ToInt32(Result["IsOblSB"]),
                        RefDate = Convert.ToString(Result["RefDate"]),
                        PartyId = Convert.ToInt32(Result["PartyId"]),
                        Address = Convert.ToString(Result["Address"]),
                        GstNo = Convert.ToString(Result["GstNo"]),
                        Area = Convert.ToDecimal(Result["Area"]),
                        CFSCode = Convert.ToString(Result["CFSCode"]),
                        CIF = Convert.ToDecimal(Result["CIF"]),
                        CommodityID = Convert.ToInt32(Result["CommodityID"]),
                        Duty = Convert.ToDecimal(Result["Duty"]),
                        Fob = Convert.ToDecimal(Result["Fob"]),
                        EntryDate = Convert.ToString(Result["EntryDate"]),
                        Noofpkg = Convert.ToInt32(Result["Noofpkg"]),
                        Size = Convert.ToString(Result["Size"]),
                        Refid = Convert.ToInt32(Result["Refid"]), //
                        Weight = Convert.ToDecimal(Result["Weight"]),
                        GodownID = Convert.ToInt32(Result["GodownID"]),
                        ShippingId = Convert.ToInt32(Result["ShippingLileID"]),
                        ImporterID = Convert.ToInt32(Result["ExporterID"])

                    });


                    //ViewBag.strTime = objEntryThroughGate.EntryTime;



                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstAucBidFinalizationHdr;
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
        public void DeleteBid(int id)
        {

            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Bid", MySqlDbType = MySqlDbType.Int32, Value = id });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });





            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteBidData", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = "";
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Bid Details Delete Successfully";
                    //: "Exit Through Gate Updated Successfully
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
        public void GetEMDReceived(string RecVidNo)
        {
            int Status = 0;
            //int BranchId = Convert.ToInt32(Session["BranchId"]);
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_ENDRcvNo", MySqlDbType = MySqlDbType.String, Value = RecVidNo });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetEMDReceived", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Dnd_AucBidFinalizationHdr> lstAucBidFinalizationHdr = new List<Dnd_AucBidFinalizationHdr>();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstAucBidFinalizationHdr.Add(new Dnd_AucBidFinalizationHdr
                    {
                        EmdReceiptNo = Result["EmdRcvdNo"].ToString(),
                        BidDate = (Result["EmdRcvdDate"] == DBNull.Value ? "" : Result["EmdRcvdDate"]).ToString(),
                        Party = Result["Party"].ToString(),
                        //BidAmount = Convert.ToDecimal(Result["BidAmount"] == DBNull.Value ? "0" : Result["BidAmount"].ToString()),
                        EmdAmount = Convert.ToDecimal(Result["AdvanceAmount"] == DBNull.Value ? "0" : Result["AdvanceAmount"].ToString()),
                        //Extraprice = Convert.ToDecimal(Result["TotalPaymentReceipt"] == DBNull.Value ? "0" : Result["TotalPaymentReceipt"].ToString()),
                        PaymentMode = Result["PayMode"].ToString(),
                        DraweeBank = Result["DraweeBank"].ToString(),
                        InstrumentNo = Result["InstrumentNo"].ToString(),
                        Amount = Convert.ToDecimal(Result["Amount"] == DBNull.Value ? "0" : Result["Amount"].ToString()),
                        Date = Result["RcvdDate"].ToString(),
                        BidNo = Result["BidNo"].ToString(),
                        RefDate = Result["BidDate"].ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstAucBidFinalizationHdr;
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
        public void GenericBulkInvoiceDetailsForPrint(Dnd_AuctionInvoiceViewModel ObjBulkInvoiceReport)
        {
            DateTime dtfrom = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodFrom = dtfrom.ToString("yyyy/MM/dd");
            DateTime dtTo = DateTime.ParseExact(ObjBulkInvoiceReport.PeriodTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String PeriodTo = dtTo.ToString("yyyy/MM/dd");
            ObjBulkInvoiceReport.InvoiceModule = "Auc";
            if (String.IsNullOrWhiteSpace(ObjBulkInvoiceReport.InvoiceModule))
            {
                ObjBulkInvoiceReport.InvoiceModule = "";
            }
            if (String.IsNullOrWhiteSpace(ObjBulkInvoiceReport.InvoiceNumber))
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }
            int Status = 0;
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataSet Result = new DataSet();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ObjBulkInvoiceReport.InvoiceNumber });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = ObjBulkInvoiceReport.PartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceType", MySqlDbType = MySqlDbType.VarChar, Size = 40, Value = ObjBulkInvoiceReport.InvoiceModule });

            LstParam.Add(new MySqlParameter { ParameterName = "in_FromDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodFrom });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ToDate", MySqlDbType = MySqlDbType.DateTime, Value = PeriodTo });
            try
            {
                DParam = LstParam.ToArray();
                Result = DataAccess.ExecuteDataSet("GetAuctioninvoicedetailsforprint", CommandType.StoredProcedure, DParam);
                _DBResponse = new DatabaseResponse();
                if (Result != null && Result.Tables[1].Rows.Count > 0)
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



        #region Destruction

        public void GetRefNoForDestruction(int Flag)
        {
            try
            {

                int Status = 0;
                IDataParameter[] DParam = { };
                List<MySqlParameter> LstParam = new List<MySqlParameter>();
                DataSet Result = new DataSet();
                DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                LstParam.Add(new MySqlParameter { ParameterName = "in_Flag", MySqlDbType = MySqlDbType.Int32, Size = 40, Value = Flag });
                try
                {
                    DParam = LstParam.ToArray();
                    Result = DataAccess.ExecuteDataSet("GetOblShippDestruction", CommandType.StoredProcedure, DParam);
                    _DBResponse = new DatabaseResponse();
                    List<Dnd_DestructionViewModel> lstDestruction = new List<Dnd_DestructionViewModel>();
                    if (Result != null && Result.Tables[0].Rows.Count > 0)
                    {
                        Status = 1;
                        foreach (DataRow dr in Result.Tables[0].Rows)
                        {
                            lstDestruction.Add(new Dnd_DestructionViewModel
                            {
                                RefNo = Convert.ToString(dr["RefNo"])
                            });
                        }


                    }
                    if (Status == 1)
                    {
                        _DBResponse.Status = 1;
                        _DBResponse.Message = "Success";
                        _DBResponse.Data = lstDestruction;
                    }
                    else
                    {
                        _DBResponse.Status = 0;
                        _DBResponse.Message = "No Data";
                        _DBResponse.Data = lstDestruction;
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
            catch (Exception ex)
            {

            }
        }


        public void GetGodownRightsWise(int Uid)
        {
            IDataParameter[] DParam = { };
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = Uid });
            DParam = LstParam.ToArray();
            IDataReader Result = DataAccess.ExecuteDataReader("GetGodownListAccdRights", CommandType.StoredProcedure, DParam);
            IList<GodownList> lst = new List<GodownList>();
            _DBResponse = new DatabaseResponse();
            int Status = 0;
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lst.Add(new GodownList
                    {
                        GodownName = Result["GodownName"].ToString(),
                        GodownId = Convert.ToInt32(Result["GodownId"])
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lst;
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



        public void GetLocationDetailsByGodownId(int GodownId)
        {

            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "in_godownid", MySqlDbType = MySqlDbType.Int32, Value = GodownId });
            IDataParameter[] Dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetLocationDetailsByGodownId", CommandType.StoredProcedure, Dpram);
            _DBResponse = new DatabaseResponse();
            List<Areas.Export.Models.GodownWiseLocation> lstApplication = new List<Areas.Export.Models.GodownWiseLocation>();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstApplication.Add(new Areas.Export.Models.GodownWiseLocation
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

        public void SaveDestruction(Dnd_DestructionViewModel vm, int uid)
        {

            DateTime dt = DateTime.ParseExact(vm.DestructionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            String DestructionDate = dt.ToString("yyyy-MM-dd");





            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "pDestructionid", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(vm.DestructionID) });
            LstParam.Add(new MySqlParameter { ParameterName = "pRefNo", MySqlDbType = MySqlDbType.VarChar, Value = Convert.ToString(vm.RefNo) });//ObjAucBidFinalizationHdr.BidDate
            LstParam.Add(new MySqlParameter { ParameterName = "pRefFlg", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(vm.RefFlag) });
            LstParam.Add(new MySqlParameter { ParameterName = "pGodownID", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(vm.GodownID) });
            LstParam.Add(new MySqlParameter { ParameterName = "pGatePassNo", MySqlDbType = MySqlDbType.VarChar, Size = 250, Value = vm.CustomGatePassNo });
            LstParam.Add(new MySqlParameter { ParameterName = "pAgencyName", MySqlDbType = MySqlDbType.VarChar, Value = vm.DestructionAgencyName });
            LstParam.Add(new MySqlParameter { ParameterName = "pDestructionDate", MySqlDbType = MySqlDbType.Date, Value = DestructionDate });
            LstParam.Add(new MySqlParameter { ParameterName = "pVehicleNo", MySqlDbType = MySqlDbType.VarChar, Value = vm.VehicleNo });
            LstParam.Add(new MySqlParameter { ParameterName = "pShedNo", MySqlDbType = MySqlDbType.VarChar, Value = vm.ShedNo });
            LstParam.Add(new MySqlParameter { ParameterName = "pRemarks", MySqlDbType = MySqlDbType.VarChar, Value = vm.Remarks });
            LstParam.Add(new MySqlParameter { ParameterName = "pCrby", MySqlDbType = MySqlDbType.Int32, Value = uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditDestruction", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Destruction Details Saved Successfully";
                    //: "Exit Through Gate Updated Successfully
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Destruction Details Updated Successfully";
                    //: "Exit Through Gate Updated Successfully
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




        public void GetDestructionDetails(int ID)
        {

            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "pId", MySqlDbType = MySqlDbType.Int32, Value = ID });
            IDataParameter[] Dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetDestructionDetails", CommandType.StoredProcedure, Dpram);
            _DBResponse = new DatabaseResponse();
            List<Dnd_DestructionViewModel> lstDestruction = new List<Dnd_DestructionViewModel>();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstDestruction.Add(new Dnd_DestructionViewModel
                    {
                        DestructionID = Convert.ToInt32(result["Id"]),
                        RefNo = Convert.ToString(result["RefNo"]),
                        DestructionAgencyName = Convert.ToString(result["AgencyName"]),
                        VehicleNo = Convert.ToString(result["VehicleNo"]),
                        ContainerNo = Convert.ToString(result["ContainerNo"]),
                        CustomGatePassNo = Convert.ToString(result["GatePassNo"]),
                        DestructionDate = Convert.ToString(result["DestructionDate"]),
                        GodownID = Convert.ToString(result["GodownID"]),
                        GodownName = Convert.ToString(result["GodownName"]),
                        OBL = Convert.ToString(result["OBL"]),
                        RefFlag = Convert.ToString(result["RefFlg"]),
                        Remarks = Convert.ToString(result["Remarks"]),



                        ShedNo = Convert.ToString(result["ShedNo"]),
                        Shipbill = Convert.ToString(result["SB"]),


                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstDestruction;
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

        public void DeleteDestruction(int DestructionID)
        {

            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "pDestructionID", MySqlDbType = MySqlDbType.Int32, Value = Convert.ToInt32(DestructionID) });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("DeleteDestruction", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = Result;
                    _DBResponse.Message = "Destruction Details Delete Successfully";
                    //: "Exit Through Gate Updated Successfully
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

        public void GetDestructionGatePassDetails(int ID)
        {

            List<MySqlParameter> lstparam = new List<MySqlParameter>();
            lstparam.Add(new MySqlParameter { ParameterName = "pDestructionId", MySqlDbType = MySqlDbType.Int32, Value = ID });
            IDataParameter[] Dpram = lstparam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetDestructionPrint", CommandType.StoredProcedure, Dpram);
            _DBResponse = new DatabaseResponse();
            List<DestructionGatepassViewModel> lstDestruction = new List<DestructionGatepassViewModel>();
            int Status = 0;
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    lstDestruction.Add(new DestructionGatepassViewModel
                    {

                        GatePassNo = Convert.ToString(result["GatePassNo"]),
                        GatePassDate = Convert.ToString(result["GatepassDate"]),
                        VehicleNo = Convert.ToString(result["VehicleNo"]),
                        ContainerNoAndSize = Convert.ToString(result["ContainerNoSize"]),
                        ImporterExporter = Convert.ToString(result["ImportExporterName"]),
                        DestructionAgencyName = Convert.ToString(result["AgencyName"]),
                        ShippingLine = Convert.ToString(result["ShippingName"]),
                        OBLNoShippbillNo = Convert.ToString(result["BLNoShippingBillNo"]),
                        DestructionDate = Convert.ToString(result["DestructionDate"]),
                        NoOfPkg = Convert.ToString(result["NoOfPackages"]),
                        Weight = Convert.ToString(result["Weight"]),
                        LocationName = Convert.ToString(result["ShedNo"]),
                        ICDCode = Convert.ToString(result["CFSCode"]),
                        CargoDesc = Convert.ToString(result["CommodityName"]),
                        GatepassTime = Convert.ToString(result["GatePassTimeOnly"]),
                        GatepassExpiryDateAndTime = Convert.ToString(result["GatePassExpireDate"]),
                        Remarks = Convert.ToString(result["Remarks"])

                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Data = lstDestruction;
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

    }
    #endregion

}

   