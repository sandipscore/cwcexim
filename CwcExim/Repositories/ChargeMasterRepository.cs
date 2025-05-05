using CwcExim.DAL;
using CwcExim.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CwcExim.Repositories
{
    public class ChargeMasterRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }
        public void GetAllCharges()
        {  
            int Status = 0;
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();

            _DBResponse = new DatabaseResponse();
            try
            {
                var ChargeName = DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = ChargeName;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }


        //public dynamic AllHnTCharges { get; set; }

        #region Auction Payment

        public void GetAuctionPaymentSheet(int AppraisementId, string AppraisementNo, string AppraisementDate, int PartyId, string PartyName,
        string PartyAddress, string PartyState, string PartyGST, string InvoiceType, string InvoiceDate)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_BidIdHdr", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetAuctionPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = AppraisementId;
                objPostPaymentSheet.RequestNo = AppraisementNo;
                objPostPaymentSheet.RequestDate = AppraisementDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyGST = PartyGST;

                if (objPrePaymentSheet.lstPreInvoiceCont.Count > 0)
                {
                    objPostPaymentSheet.OperationType = objPrePaymentSheet.lstPreInvoiceCont[0].OperationType;
                }

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));

                //objPostPaymentSheet.lstContWiseAmount= objPrePaymentSheet.lstPreContWiseAmount.Select(o=> o).ToList();

                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + ", ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.ApproveOn.Contains(item.ApproveOn))
                        objPostPaymentSheet.ApproveOn += item.ApproveOn + ", ";

                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }

                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");

                objPostPaymentSheet.CalculateCharges(9, ChargeName);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }


        #endregion

        #region Yard Payment Sheet
        public void GetYardPaymentSheet(string InvoiceDate, int AppraisementId, int DeliveryType,string SEZ, string AppraisementNo, string AppraisementDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName,
           decimal weight, string InvoiceType, string ContainerXML ,decimal WeighmentCharges, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
           // LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = Type });
     

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetYardPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = AppraisementId;
                objPostPaymentSheet.RequestNo = AppraisementNo;
                objPostPaymentSheet.RequestDate = AppraisementDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode;
                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));

                //objPostPaymentSheet.lstContWiseAmount= objPrePaymentSheet.lstPreContWiseAmount.Select(o=> o).ToList();

                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + " , ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.ApproveOn.Contains(item.ApproveOn))
                        objPostPaymentSheet.ApproveOn += item.ApproveOn + ", ";

                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM,
                            LCLFCL=item.LCLFCL
                        });
                    }

                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM,
                        LCLFCL = item.LCLFCL
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                //if (DeliveryType == 1)
                //    objPostPaymentSheet.CalculateCharges(1, ChargeName);
                //else
                //    objPostPaymentSheet.CalculateCharges(2, ChargeName);

                if (DeliveryType == 1)
                    objPostPaymentSheet.KDLCalculateCharges(1, ChargeName, 0, SEZ, WeighmentCharges);
                else
                    objPostPaymentSheet.KDLCalculateCharges(2, ChargeName, 0, SEZ, WeighmentCharges);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        #endregion

        #region Yard Payment Sheet For Kolkata
        public void GetYardPaymentSheet(string InvoiceDate, int AppraisementId, int DeliveryType, string SEZ,string AppraisementNo, string AppraisementDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, decimal Weight,
            string InvoiceType, string ContainerXML, decimal mechanical, decimal manual, int distance, int InvoiceId, string Type = "I")
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Type", MySqlDbType = MySqlDbType.VarChar, Size = 5, Value = Type });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = PartyId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetYardPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = AppraisementId;
                objPostPaymentSheet.RequestNo = AppraisementNo;
                objPostPaymentSheet.RequestDate = AppraisementDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode==""? objPrePaymentSheet.lstPreInvoiceHdr[0].StateCode : PartyStateCode;
                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));

                //objPostPaymentSheet.lstContWiseAmount= objPrePaymentSheet.lstPreContWiseAmount.Select(o=> o).ToList();

                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + " , ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.ApproveOn.Contains(item.ApproveOn))
                        objPostPaymentSheet.ApproveOn += item.ApproveOn + ", ";

                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM,
                            LCLFCL = item.LCLFCL
                        });
                    }

                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM,
                        LCLFCL = item.LCLFCL
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                if (DeliveryType == 1)
                    objPostPaymentSheet.CalculateChargesForSEZCal(1,ChargeName, SEZ,"Tariff");
                else
                    objPostPaymentSheet.CalculateChargesForSEZCal(2, ChargeName, SEZ, "Tariff");

                #region New Tariff

                //For Extra Charge 

                string ConvertInvoiceDate = DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

                List<MySqlParameter> LstParamAllNew = new List<MySqlParameter>();
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_ADestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AEffectDate", MySqlDbType = MySqlDbType.DateTime, Value = ConvertInvoiceDate });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_ADistance", MySqlDbType = MySqlDbType.Decimal, Value = distance });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AMechanicalWeight", MySqlDbType = MySqlDbType.Decimal, Size = 11, Value = mechanical });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AManualWeight", MySqlDbType = MySqlDbType.Decimal, Value = manual });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });

                IDataParameter[] ParamNew = { };
                ParamNew = LstParamAllNew.ToArray();
                var ExtraChargeName = (IList<Areas.Import.Models.Kol_AddExtraHTCharge>)DataAccess.ExecuteDynamicSet<Areas.Import.Models.Kol_AddExtraHTCharge>("YardPaymentSheetUpdate", ParamNew);

                var CompStateCode = ChargeName.CompanyDetails.FirstOrDefault().StateCode;
                //var PartyStateCode= objPostPaymentSheet.PartyStateCode;
                var GSTType = (objPostPaymentSheet.PartyStateCode == CompStateCode) || (objPostPaymentSheet.PartyStateCode == "");
                var cgst = objPostPaymentSheet.lstPostPaymentChrg.FirstOrDefault().CGSTPer;
                var sgst = objPostPaymentSheet.lstPostPaymentChrg.FirstOrDefault().SGSTPer;
                var igst = objPostPaymentSheet.lstPostPaymentChrg.FirstOrDefault().IGSTPer;
                if (SEZ == "SEZWP")
                {
                    GSTType = false;
                }

                if (SEZ == "SEZWOP")
                {
                    cgst = 0;
                    sgst = 0;
                    igst = 0;
                }
                //ExtraChargeName.to
                foreach (var i in ExtraChargeName)
                {
                    objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                    {
                        ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                        Clause = i.Clause,
                        ChargeName = i.ChargeName,
                        ChargeType = i.ChargeType,
                        SACCode = i.SACCode,
                        Quantity = 0,
                        Rate = 0,
                        Amount = i.Taxable,
                        Discount = 0,
                        Taxable = i.Taxable,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (i.Taxable * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees + (ActualEntryFees * (cgst / 100)) + (ActualEntryFees * (sgst / 100))) : (ActualEntryFees + (ActualEntryFees * (igst / 100)))) : ActualEntryFees
                        Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable +
                        (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (sgst / 100)) : 0) : 0, 2))) :
                        (i.Taxable + (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (i.Taxable * (igst / 100))) : 0, 2)))) : i.Taxable
                    });
                }



                objPostPaymentSheet.lstPostPaymentChrg.ToList().ForEach(item =>
                {
                    item.Amount = Math.Round(item.Amount, 2);
                    item.CGSTAmt = Math.Round(item.CGSTAmt, 2);
                    item.SGSTAmt = Math.Round(item.SGSTAmt, 2);
                    item.IGSTAmt = Math.Round(item.IGSTAmt, 2);
                    item.Total = Math.Round(item.Total, 2);
                });


                objPostPaymentSheet.TotalAmt = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
                objPostPaymentSheet.TotalDiscount = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Discount), 2);
                objPostPaymentSheet.TotalTaxable = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
                objPostPaymentSheet.TotalCGST = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.CGSTAmt), 2);
                objPostPaymentSheet.TotalSGST = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.SGSTAmt), 2);
                objPostPaymentSheet.TotalIGST = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.IGSTAmt), 2);
                objPostPaymentSheet.CWCTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total), 2);//tax calculated
                objPostPaymentSheet.HTTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total), 2);//tax calculated

                objPostPaymentSheet.CWCAmtTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount), 2);//without tax total
                objPostPaymentSheet.HTAmtTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Amount), 2);//without tax total
                objPostPaymentSheet.CWCTDS = Math.Round((objPostPaymentSheet.CWCAmtTotal * objPostPaymentSheet.CWCTDSPer) / 100);
                objPostPaymentSheet.HTTDS = Math.Round((objPostPaymentSheet.HTAmtTotal * objPostPaymentSheet.HTTDSPer) / 100);
                objPostPaymentSheet.TDS = Math.Round(objPostPaymentSheet.CWCTDS + objPostPaymentSheet.HTTDS);
                objPostPaymentSheet.TDSCol = objPostPaymentSheet.TDS;
                //this.AllTotal = (this.lstPostPaymentChrg.Sum(o => o.Total)) - this.TDS;
                objPostPaymentSheet.AllTotal = objPostPaymentSheet.CWCTotal + objPostPaymentSheet.HTTotal + objPostPaymentSheet.TDSCol - objPostPaymentSheet.TDS;
                objPostPaymentSheet.RoundUp = Math.Ceiling(objPostPaymentSheet.AllTotal) - objPostPaymentSheet.AllTotal;
                objPostPaymentSheet.InvoiceAmt = Math.Ceiling(objPostPaymentSheet.AllTotal);
                #endregion


                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        #endregion

        #region Export Payment Sheet
        public void GetExportPaymentSheet(string InvoiceDate, int AppraisementId, int DeliveryType,string SEZ, string AppraisementNo, string AppraisementDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, string InvoiceType, string ContainerXML, int InvoiceId,decimal Weightment)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetExportPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = AppraisementId;
                objPostPaymentSheet.RequestNo = AppraisementNo;
                objPostPaymentSheet.RequestDate = AppraisementDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode;
                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));

                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + ", ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateChargesKDLSEZ(3, ChargeName,SEZ,Weightment);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }


        public void GetExportPaymentSheet(string InvoiceDate, int AppraisementId, int DeliveryType, string SEZ, string AppraisementNo, string AppraisementDate, int PartyId, string PartyName,
          string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, string InvoiceType, string ContainerXML, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetExportPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = AppraisementId;
                objPostPaymentSheet.RequestNo = AppraisementNo;
                objPostPaymentSheet.RequestDate = AppraisementDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode;
                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));

                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + ", ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateChargesKDLSEZ(3, ChargeName, SEZ);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }

        public void GetExportPaymentSheet(string InvoiceDate, int AppraisementId, int DeliveryType,string SEZ, string AppraisementNo, string AppraisementDate, int PartyId, string PartyName,
         string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, decimal Weight,string InvoiceType, string ContainerXML, decimal Distance, decimal MechanicalWeight, decimal ManualWeight, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetExportPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = AppraisementId;
                objPostPaymentSheet.RequestNo = AppraisementNo;
                objPostPaymentSheet.RequestDate = AppraisementDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode == "" ? objPrePaymentSheet.lstPreInvoiceHdr[0].StateCode : PartyStateCode;

                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));

                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + ", ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM,
                            Clauseweight=item.Clauseweight,
                            StorageType=item.StorageType
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM,
                        Clauseweight = item.Clauseweight,
                        StorageType=item.StorageType
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateChargesForKolSEZ(3, ChargeName,SEZ);

                List<MySqlParameter> CLstParam = new List<MySqlParameter>();
                CLstParam.Add(new MySqlParameter { ParameterName = "in_EffectDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal, Value = Distance });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_MechanicalWeight", MySqlDbType = MySqlDbType.Decimal, Value = MechanicalWeight });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_ManualWeight", MySqlDbType = MySqlDbType.Decimal, Value = ManualWeight });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_StuffingId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });


                IDataParameter[] CParam = { };
                CParam = CLstParam.ToArray();
                var AddChargeName = (IList<Areas.Export.Models.KOL_CWCChargeModel>)DataAccess.ExecuteDynamicSet<Areas.Export.Models.KOL_CWCChargeModel>("GetExportPS", CParam);
                var CompStateCode = ChargeName.CompanyDetails.FirstOrDefault().StateCode;
                objPostPaymentSheet.CalculateChargesAdditionalForKolSEZ(3, AddChargeName, SEZ, objPostPaymentSheet, CompStateCode);
                

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }

        #endregion

        

        #region Godown Destuffing Payment Sheet
        public void GetDestuffingPaymentSheet(string InvoiceDate, int DestuffingAppId, int DeliveryType, string DestuffingAppNo, string DestuffingAppDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName,
            string InvoiceType, string ContainerXML, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
         
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetDestuffingPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = DestuffingAppId;
                objPostPaymentSheet.RequestNo = DestuffingAppNo;
                objPostPaymentSheet.RequestDate = DestuffingAppDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode;
                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));

                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + " , ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateCharges(4, ChargeName);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        public void GetBOL(int DestuffingId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingId });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetBOLDet", CommandType.StoredProcedure, dparam);
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
        #endregion

        #region Godown Destuffing Payment Sheet For Kol
        public void GetDestuffingPaymentSheet(string InvoiceDate, int DestuffingAppId, int DeliveryType,string SEZ, string DestuffingAppNo, string DestuffingAppDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, decimal Weight,
            string InvoiceType, string ContainerXML, decimal mechanical, decimal manual, int distance, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = PartyId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetDestuffingPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = DestuffingAppId;
                objPostPaymentSheet.RequestNo = DestuffingAppNo;
                objPostPaymentSheet.RequestDate = DestuffingAppDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode == "" ? objPrePaymentSheet.lstPreInvoiceHdr[0].StateCode : PartyStateCode;

                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));

                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + " , ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });
                //List<MySqlParameter> LstParamCAll = new List<MySqlParameter>();
                //LstParamCAll.Add(new MySqlParameter { ParameterName = "in_EffectDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
                //LstParamCAll.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal, Value = 10 });
                //LstParamCAll.Add(new MySqlParameter { ParameterName = "in_MechanicalWeight", MySqlDbType = MySqlDbType.Decimal, Size = 11, Value = 10 });
                //LstParamCAll.Add(new MySqlParameter { ParameterName = "in_ManualWeight", MySqlDbType = MySqlDbType.Decimal, Value = 10 });
                //LstParamCAll.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Text, Size = 11, Value = 1 });
                //IDataParameter[] DParamCAll = { };
                //DParamCAll = LstParamCAll.ToArray();
                string ConvertInvoiceDate = DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                //DateTime dt = DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy h:mm", System.Globalization.CultureInfo.InvariantCulture);

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateChargesForSEZCal(4, ChargeName,SEZ,"Tariff");

                //For Extra Charge 
             

              
                List<MySqlParameter> LstParamAllNew = new List<MySqlParameter>();
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_ADestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AEffectDate", MySqlDbType = MySqlDbType.DateTime, Value = ConvertInvoiceDate });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_ADistance", MySqlDbType = MySqlDbType.Decimal, Value = distance });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AMechanicalWeight", MySqlDbType = MySqlDbType.Decimal, Size = 11, Value = mechanical });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AManualWeight", MySqlDbType = MySqlDbType.Decimal, Value = manual });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });

                IDataParameter[] ParamNew = { };
                ParamNew = LstParamAllNew.ToArray();
                var ExtraChargeName = (IList<Areas.Import.Models.Kol_AddExtraHTCharge>)DataAccess.ExecuteDynamicSet<Areas.Import.Models.Kol_AddExtraHTCharge>("GetDistruffingPaymentSheet", ParamNew);

                var CompStateCode = ChargeName.CompanyDetails.FirstOrDefault().StateCode;
                //var PartyStateCode= objPostPaymentSheet.PartyStateCode;
                var GSTType = (objPostPaymentSheet.PartyStateCode == CompStateCode) || (objPostPaymentSheet.PartyStateCode == "");
                var cgst = objPostPaymentSheet.lstPostPaymentChrg.FirstOrDefault().CGSTPer;
                var sgst = objPostPaymentSheet.lstPostPaymentChrg.FirstOrDefault().SGSTPer;
                var igst = objPostPaymentSheet.lstPostPaymentChrg.FirstOrDefault().IGSTPer;
                if (SEZ == "SEZWP")
                {
                    GSTType = false;
                }

                if (SEZ == "SEZWOP")
                {
                    cgst = 0;
                    sgst = 0;
                    igst = 0;
                }
                //ExtraChargeName.to
                foreach ( var i in ExtraChargeName)
                {
                    objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                    {
                        ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                        Clause = i.Clause,
                        ChargeName = i.ChargeName,
                        ChargeType = i.ChargeType,
                        SACCode = i.SACCode,
                        Quantity = 0,
                        Rate = 0,
                        Amount = i.Taxable,
                        Discount = 0,
                        Taxable = i.Taxable,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (i.Taxable * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees + (ActualEntryFees * (cgst / 100)) + (ActualEntryFees * (sgst / 100))) : (ActualEntryFees + (ActualEntryFees * (igst / 100)))) : ActualEntryFees
                        Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable +
                        (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (sgst / 100)) : 0) : 0, 2))) :
                        (i.Taxable + (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (i.Taxable * (igst / 100))) : 0, 2)))) : i.Taxable
                    });
                }



                objPostPaymentSheet.lstPostPaymentChrg.ToList().ForEach(item =>
                {
                    item.Amount = Math.Round(item.Amount, 2);
                    item.CGSTAmt = Math.Round(item.CGSTAmt, 2);
                    item.SGSTAmt = Math.Round(item.SGSTAmt, 2);
                    item.IGSTAmt = Math.Round(item.IGSTAmt, 2);
                    item.Total = Math.Round(item.Total, 2);
                });


                objPostPaymentSheet.TotalAmt = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
                objPostPaymentSheet.TotalDiscount = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Discount), 2);
                objPostPaymentSheet.TotalTaxable = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
                objPostPaymentSheet.TotalCGST = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.CGSTAmt), 2);
                objPostPaymentSheet.TotalSGST = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.SGSTAmt), 2);
                objPostPaymentSheet.TotalIGST = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.IGSTAmt), 2);
                objPostPaymentSheet.CWCTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total), 2);//tax calculated
                objPostPaymentSheet.HTTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total), 2);//tax calculated

                objPostPaymentSheet.CWCAmtTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount), 2);//without tax total
                objPostPaymentSheet.HTAmtTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Amount), 2);//without tax total
                objPostPaymentSheet.CWCTDS = Math.Round((objPostPaymentSheet.CWCAmtTotal * objPostPaymentSheet.CWCTDSPer) / 100);
                objPostPaymentSheet.HTTDS = Math.Round((objPostPaymentSheet.HTAmtTotal * objPostPaymentSheet.HTTDSPer) / 100);
                objPostPaymentSheet.TDS = Math.Round(objPostPaymentSheet.CWCTDS + objPostPaymentSheet.HTTDS);
                objPostPaymentSheet.TDSCol = objPostPaymentSheet.TDS;
                //this.AllTotal = (this.lstPostPaymentChrg.Sum(o => o.Total)) - this.TDS;
                objPostPaymentSheet.AllTotal = objPostPaymentSheet.CWCTotal + objPostPaymentSheet.HTTotal + objPostPaymentSheet.TDSCol - objPostPaymentSheet.TDS;
                objPostPaymentSheet.RoundUp = Math.Ceiling(objPostPaymentSheet.AllTotal) - objPostPaymentSheet.AllTotal;
                objPostPaymentSheet.InvoiceAmt = Math.Ceiling(objPostPaymentSheet.AllTotal);


                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
    
        #endregion


        #region Godown Delivery Payment Sheet
        public void GetDeliveryPaymentSheet(string InvoiceDate, int DestuffingAppId, int DeliveryType, string DestuffingAppNo, string DestuffingAppDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName,
            string InvoiceType, string LineXML, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "LineXML", MySqlDbType = MySqlDbType.Text, Value = LineXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetDeliveryPaymentSheet1", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = DestuffingAppId;
                objPostPaymentSheet.RequestNo = DestuffingAppNo;
                objPostPaymentSheet.RequestDate = DestuffingAppDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode == "" ? objPrePaymentSheet.lstPreInvoiceHdr[0].StateCode : PartyStateCode;

                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));
                objPostPaymentSheet.lstPreInvoiceCargo = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreInvoiceCargo>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreInvoiceCargo));
                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + " , ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";

                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            LineNo = item.LineNo,
                            BOENo = item.BOENo,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        LineNo = item.LineNo,
                        BOENo = item.BOENo,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateCharges(5, ChargeName);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        #endregion

        #region Empty Container Payment Sheet
        public void GetEmptyContPaymentSheet(string InvoiceDate, int DestuffingAppId, int DeliveryType, string SEZ,string DestuffingAppNo, string DestuffingAppDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, string InvoiceType, string ContainerXML,
            string InvoiceFor,decimal WeighmentCharges=0, int InvoiceId=0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceFor });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetEmptyContPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = DestuffingAppId;
                objPostPaymentSheet.RequestNo = DestuffingAppNo;
                objPostPaymentSheet.RequestDate = DestuffingAppDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode;
                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));
                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + " , ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateChargesKDLSEZ(6, ChargeName,SEZ, WeighmentCharges);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

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

        #endregion
        #region Empty Container Payment Sheet For Kolkata
        public void GetEmptyContPaymentSheetForKol(string InvoiceDate, int DestuffingAppId, int DeliveryType,string SEZ, string DestuffingAppNo, string DestuffingAppDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, string InvoiceType, string ContainerXML,Decimal Weight,
            string InvoiceFor, int InvoiceId, decimal mechanical, decimal manual, int distance)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceFor });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = PartyId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetEmptyContPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = DestuffingAppId;
                objPostPaymentSheet.RequestNo = DestuffingAppNo;
                objPostPaymentSheet.RequestDate = DestuffingAppDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode == "" ? objPrePaymentSheet.lstPreInvoiceHdr[0].StateCode : PartyStateCode;

                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));
                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + " , ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateChargesForSEZCal(6, ChargeName,SEZ,"Tariff");
                #region New Tariff

                //For Extra Charge 

                string ConvertInvoiceDate = DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

                List<MySqlParameter> LstParamAllNew = new List<MySqlParameter>();
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_ADestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AEffectDate", MySqlDbType = MySqlDbType.DateTime, Value = ConvertInvoiceDate });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_ADistance", MySqlDbType = MySqlDbType.Decimal, Value = distance });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AMechanicalWeight", MySqlDbType = MySqlDbType.Decimal, Size = 11, Value = mechanical });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AManualWeight", MySqlDbType = MySqlDbType.Decimal, Value = manual });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });

                IDataParameter[] ParamNew = { };
                ParamNew = LstParamAllNew.ToArray();
                var ExtraChargeName = (IList<Areas.Import.Models.Kol_AddExtraHTCharge>)DataAccess.ExecuteDynamicSet<Areas.Import.Models.Kol_AddExtraHTCharge>("GetNewEmptyContainerPaymentSheet", ParamNew);

                var CompStateCode = ChargeName.CompanyDetails.FirstOrDefault().StateCode;
                //var PartyStateCode= objPostPaymentSheet.PartyStateCode;
                var GSTType = (objPostPaymentSheet.PartyStateCode == CompStateCode) || (objPostPaymentSheet.PartyStateCode == "");
                var cgst = objPostPaymentSheet.lstPostPaymentChrg.FirstOrDefault().CGSTPer;
                var sgst = objPostPaymentSheet.lstPostPaymentChrg.FirstOrDefault().SGSTPer;
                var igst = objPostPaymentSheet.lstPostPaymentChrg.FirstOrDefault().IGSTPer;
                if (SEZ == "SEZWP")
                {
                    GSTType = false;
                }

                if (SEZ == "SEZWOP")
                {
                    cgst = 0;
                    sgst = 0;
                    igst = 0;
                }
                //ExtraChargeName.to
                foreach (var i in ExtraChargeName)
                {
                    objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                    {
                        ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                        Clause = i.Clause,
                        ChargeName = i.ChargeName,
                        ChargeType = i.ChargeType,
                        SACCode = i.SACCode,
                        Quantity = 0,
                        Rate = 0,
                        Amount = i.Taxable,
                        Discount = 0,
                        Taxable = i.Taxable,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (i.Taxable * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees + (ActualEntryFees * (cgst / 100)) + (ActualEntryFees * (sgst / 100))) : (ActualEntryFees + (ActualEntryFees * (igst / 100)))) : ActualEntryFees
                        Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable +
                        (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (sgst / 100)) : 0) : 0, 2))) :
                        (i.Taxable + (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (i.Taxable * (igst / 100))) : 0, 2)))) : i.Taxable
                    });
                }



                objPostPaymentSheet.lstPostPaymentChrg.ToList().ForEach(item =>
                {
                    item.Amount = Math.Round(item.Amount, 2);
                    item.CGSTAmt = Math.Round(item.CGSTAmt, 2);
                    item.SGSTAmt = Math.Round(item.SGSTAmt, 2);
                    item.IGSTAmt = Math.Round(item.IGSTAmt, 2);
                    item.Total = Math.Round(item.Total, 2);
                });


                objPostPaymentSheet.TotalAmt = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
                objPostPaymentSheet.TotalDiscount = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Discount), 2);
                objPostPaymentSheet.TotalTaxable = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
                objPostPaymentSheet.TotalCGST = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.CGSTAmt), 2);
                objPostPaymentSheet.TotalSGST = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.SGSTAmt), 2);
                objPostPaymentSheet.TotalIGST = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.IGSTAmt), 2);
                objPostPaymentSheet.CWCTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total), 2);//tax calculated
                objPostPaymentSheet.HTTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total), 2);//tax calculated

                objPostPaymentSheet.CWCAmtTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount), 2);//without tax total
                objPostPaymentSheet.HTAmtTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Amount), 2);//without tax total
                objPostPaymentSheet.CWCTDS = Math.Round((objPostPaymentSheet.CWCAmtTotal * objPostPaymentSheet.CWCTDSPer) / 100);
                objPostPaymentSheet.HTTDS = Math.Round((objPostPaymentSheet.HTAmtTotal * objPostPaymentSheet.HTTDSPer) / 100);
                objPostPaymentSheet.TDS = Math.Round(objPostPaymentSheet.CWCTDS + objPostPaymentSheet.HTTDS);
                objPostPaymentSheet.TDSCol = objPostPaymentSheet.TDS;
                //this.AllTotal = (this.lstPostPaymentChrg.Sum(o => o.Total)) - this.TDS;
                objPostPaymentSheet.AllTotal = objPostPaymentSheet.CWCTotal + objPostPaymentSheet.HTTotal + objPostPaymentSheet.TDSCol - objPostPaymentSheet.TDS;
                objPostPaymentSheet.RoundUp = Math.Ceiling(objPostPaymentSheet.AllTotal) - objPostPaymentSheet.AllTotal;
                objPostPaymentSheet.InvoiceAmt = Math.Ceiling(objPostPaymentSheet.AllTotal);
                #endregion
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
      
        #endregion

        #region BTT Payment Sheet
        public void GetBTTPaymentSheet(string InvoiceDate, int AppraisementId, int DeliveryType,string SEZ, string AppraisementNo, string AppraisementDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, string InvoiceType, string ContainerXML, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetBTTPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = AppraisementId;
                objPostPaymentSheet.RequestNo = AppraisementNo;
                objPostPaymentSheet.RequestDate = AppraisementDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode == "" ? objPrePaymentSheet.lstPreInvoiceHdr[0].StateCode : PartyStateCode;

                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));

                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + ", ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateChargesKDLSEZ(7, ChargeName,SEZ);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }

        public void GetBTTPaymentSheet(string InvoiceDate, int AppraisementId, int DeliveryType, string SEZ, string AppraisementNo, string AppraisementDate, int PartyId, string PartyName,
        string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, string InvoiceType, string ContainerXML, int InvoiceId,decimal Weighment)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetBTTPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = AppraisementId;
                objPostPaymentSheet.RequestNo = AppraisementNo;
                objPostPaymentSheet.RequestDate = AppraisementDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode == "" ? objPrePaymentSheet.lstPreInvoiceHdr[0].StateCode : PartyStateCode;

                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));

                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + ", ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateChargesKDLSEZ(7, ChargeName, SEZ, Weighment);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        #endregion


        #region BTT Payment Sheet
        public void GetBTTPaymentSheet(string InvoiceDate, int AppraisementId, int DeliveryType,string SEZ, string AppraisementNo, string AppraisementDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, decimal Weight,string InvoiceType, string ContainerXML, decimal Distance, decimal MechanicalWeight, decimal ManualWeight, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_CartingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = PartyId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetBTTPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = AppraisementId;
                objPostPaymentSheet.RequestNo = AppraisementNo;
                objPostPaymentSheet.RequestDate = AppraisementDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode;
                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));

                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + ", ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM,
                            Clauseweight=item.Clauseweight
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM,
                        Clauseweight = item.Clauseweight
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateChargesForKolSEZ(7, ChargeName,SEZ);

                List<MySqlParameter> CLstParam = new List<MySqlParameter>();
                CLstParam.Add(new MySqlParameter { ParameterName = "in_EffectDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal, Value = Distance });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_MechanicalWeight", MySqlDbType = MySqlDbType.Decimal, Value = MechanicalWeight });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_ManualWeight", MySqlDbType = MySqlDbType.Decimal, Value = ManualWeight });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_CartingId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });


                IDataParameter[] CParam = { };
                CParam = CLstParam.ToArray();
                var AddChargeName = (IList<Areas.Export.Models.KOL_CWCChargeModel>)DataAccess.ExecuteDynamicSet<Areas.Export.Models.KOL_CWCChargeModel>("GetBTTPS", CParam);
                var CompStateCode = ChargeName.CompanyDetails.FirstOrDefault().StateCode;
                objPostPaymentSheet.CalculateChargesAdditionalForKolSEZ(7, AddChargeName, SEZ, objPostPaymentSheet, CompStateCode);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        #endregion

        #region Loaded Container Payment Sheet
        public void GetLoadedContPaymentSheet(string InvoiceDate, int AppraisementId, int DeliveryType,string SEZ, string AppraisementNo, string AppraisementDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, string InvoiceType, string ContainerXML, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetLoadedContPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = AppraisementId;
                objPostPaymentSheet.RequestNo = AppraisementNo;
                objPostPaymentSheet.RequestDate = AppraisementDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode == "" ? objPrePaymentSheet.lstPreInvoiceHdr[0].StateCode : PartyStateCode;

                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));

                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate+" "+item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate+" "+item.ArrivalTime + ", ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateChargesKDLSEZ(8, ChargeName,SEZ);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        public void GetLoadedContPaymentSheet(string InvoiceDate, int AppraisementId, int DeliveryType, string SEZ, string AppraisementNo, string AppraisementDate, int PartyId, string PartyName,
           string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, string InvoiceType, string ContainerXML, int InvoiceId,Decimal Weighment)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetLoadedContPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = AppraisementId;
                objPostPaymentSheet.RequestNo = AppraisementNo;
                objPostPaymentSheet.RequestDate = AppraisementDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode == "" ? objPrePaymentSheet.lstPreInvoiceHdr[0].StateCode : PartyStateCode;

                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));

                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + ", ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateChargesKDLSEZ(8, ChargeName, SEZ, Weighment);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        #endregion

        public void GetLoadedContPaymentSheetTab(string InvoiceDate, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate, int PartyId, string PartyName,
           string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, string InvoiceType, string ContainerXML, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = PartyId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetLoadedContTentPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = AppraisementId;
                objPostPaymentSheet.RequestNo = AppraisementNo;
                objPostPaymentSheet.RequestDate = AppraisementDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode == "" ? objPrePaymentSheet.lstPreInvoiceHdr[0].StateCode : PartyStateCode;

                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));

                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + ", ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateCharges(8, ChargeName);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }





        #region Loaded Container Payment Sheet For Kolkata
        public void GetLoadedContPaymentSheet(string InvoiceDate, int AppraisementId, int DeliveryType,string SEZ, string AppraisementNo, string AppraisementDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName,decimal Weight, string InvoiceType, string ContainerXML, decimal Distance, decimal MechanicalWeight, decimal ManualWeight,int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_StuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = PartyId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetLoadedContPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = AppraisementId;
                objPostPaymentSheet.RequestNo = AppraisementNo;
                objPostPaymentSheet.RequestDate = AppraisementDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode == "" ? objPrePaymentSheet.lstPreInvoiceHdr[0].StateCode : PartyStateCode;

                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));

                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + ", ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateChargesForSEZCal(8, ChargeName,SEZ);

                List<MySqlParameter> CLstParam = new List<MySqlParameter>();
                CLstParam.Add(new MySqlParameter { ParameterName = "in_EffectDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_MechanicalWeight", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_ManualWeight", MySqlDbType = MySqlDbType.Decimal, Value = 0 });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_OperationType", MySqlDbType = MySqlDbType.Int32, Value = 2 });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
                CLstParam.Add(new MySqlParameter { ParameterName = "in_LoadContReqId", MySqlDbType = MySqlDbType.Int32, Value = AppraisementId });


                IDataParameter[] CParam = { };
                CParam = CLstParam.ToArray();
                var AddChargeName = (IList<Areas.Export.Models.KOL_CWCChargeModel>)DataAccess.ExecuteDynamicSet<Areas.Export.Models.KOL_CWCChargeModel>("GetExportLoadContPS", CParam);
                var CompStateCode = ChargeName.CompanyDetails.FirstOrDefault().StateCode;
                objPostPaymentSheet.CalculateChargesAdditionalForKolSEZ(8, AddChargeName, SEZ, objPostPaymentSheet, CompStateCode);

                

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        #endregion

        #region Godown Delivery Payment Sheet _Kandla
        public void GetDeliveryPaymentSheet_Kandla(string InvoiceDate, int DestuffingAppId, int DeliveryType,string SEZ, string DestuffingAppNo, string DestuffingAppDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName,
            string InvoiceType, string LineXML, decimal WeighmentCharges=0, int InvoiceId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "LineXML", MySqlDbType = MySqlDbType.Text, Value = LineXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetDeliveryPaymentSheet", DParam);

                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = DestuffingAppId;
                objPostPaymentSheet.RequestNo = DestuffingAppNo;
                objPostPaymentSheet.RequestDate = DestuffingAppDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode;
                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                  objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));
                objPostPaymentSheet.lstPreInvoiceCargo = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreInvoiceCargo>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreInvoiceCargo));


                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + " , ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";

                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            LineNo = item.LineNo,
                            BOENo = item.BOENo,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        LineNo = item.LineNo,
                        BOENo = item.BOENo,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");

                //******************************************************************************************************
                //Get Godown Type From Godown Master By GodownId
                if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 1)
                {
                    List<MySqlParameter> LstParam2 = new List<MySqlParameter>();
                    LstParam2.Add(new MySqlParameter
                    {
                        ParameterName = "in_godownid",
                        MySqlDbType = MySqlDbType.Int32,
                        Value = objPostPaymentSheet.lstPreInvoiceCargo.Count > 0 ? objPostPaymentSheet.lstPreInvoiceCargo[0].GodownId : 0
                    });

                    IDataParameter[] DParam2 = { };
                    DParam2 = LstParam2.ToArray();

                    var GodowntypeId = Convert.ToInt32(DataAccess.ExecuteScalar("getgodowntypeid", CommandType.StoredProcedure, DParam2));
                    objPostPaymentSheet.KDLCalculateCharges(5, ChargeName, GodowntypeId,SEZ, WeighmentCharges);
                }
                else
                {
                    objPostPaymentSheet.CalculateChargesKDLSEZ(5, ChargeName,SEZ);
                }
                //*******************************************************************************************************
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        public void GetBOLForDeliverApp(int DeliveryId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_DeliveryId", MySqlDbType = MySqlDbType.Int32, Value = DeliveryId });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetBOLFrDeli", CommandType.StoredProcedure, dparam);
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
        #endregion

        #region Godown Delivery Payment Sheet Kolkata
        public void GetDeliveryPaymentSheet(string InvoiceDate, int DestuffingAppId, int DeliveryType, string SEZ,string DestuffingAppNo, string DestuffingAppDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName,decimal Weight,
            string InvoiceType, string LineXML, decimal mechanical, decimal manual, int distance, int InvoiceId = 0)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "LineXML", MySqlDbType = MySqlDbType.Text, Value = LineXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Party", MySqlDbType = MySqlDbType.Int32, Value = PartyId });

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetDeliveryPaymentSheet", DParam);

                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = DestuffingAppId;
                objPostPaymentSheet.RequestNo = DestuffingAppNo;
                objPostPaymentSheet.RequestDate = DestuffingAppDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode;
                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));
                objPostPaymentSheet.lstPreInvoiceCargo = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreInvoiceCargo>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreInvoiceCargo));


                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + " , ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";

                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            LineNo = item.LineNo,
                            BOENo = item.BOENo,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        LineNo = item.LineNo,
                        BOENo = item.BOENo,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM,
                        Clauseweight=item.Clauseweight
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");

                //******************************************************************************************************
                //Get Godown Type From Godown Master By GodownId
                if (Convert.ToInt32(HttpContext.Current.Session["BranchId"]) == 1)
                {
                    List<MySqlParameter> LstParam2 = new List<MySqlParameter>();
                    LstParam2.Add(new MySqlParameter
                    {
                        ParameterName = "in_godownid",
                        MySqlDbType = MySqlDbType.Int32,
                        Value = objPostPaymentSheet.lstPreInvoiceCargo.Count > 0 ? objPostPaymentSheet.lstPreInvoiceCargo[0].GodownId : 0
                    });

                    IDataParameter[] DParam2 = { };
                    DParam2 = LstParam2.ToArray();

                    var GodowntypeId = Convert.ToInt32(DataAccess.ExecuteScalar("getgodowntypeid", CommandType.StoredProcedure, DParam2));
                    objPostPaymentSheet.CalculateChargesForKolForSEZ(5, ChargeName, GodowntypeId,SEZ,"Tariff");
                }
                else
                {
                    objPostPaymentSheet.CalculateChargesForKolForSEZ(5, ChargeName,SEZ, "Tariff");
                }


                #region Update Charge
                //For Extra Charge 
                string ConvertInvoiceDate = DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");


                List<MySqlParameter> LstParamAllNew = new List<MySqlParameter>();
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_ADestuffingAppId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AEffectDate", MySqlDbType = MySqlDbType.DateTime, Value = ConvertInvoiceDate });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_ADistance", MySqlDbType = MySqlDbType.Decimal, Value = distance });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AMechanicalWeight", MySqlDbType = MySqlDbType.Decimal, Size = 11, Value = mechanical });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AManualWeight", MySqlDbType = MySqlDbType.Decimal, Value = manual });
                LstParamAllNew.Add(new MySqlParameter { ParameterName = "in_AContainerXML", MySqlDbType = MySqlDbType.Text, Size = 11, Value = LineXML });

                IDataParameter[] ParamNew = { };
                ParamNew = LstParamAllNew.ToArray();
                var ExtraChargeName = (IList<Areas.Import.Models.Kol_AddExtraHTCharge>)DataAccess.ExecuteDynamicSet<Areas.Import.Models.Kol_AddExtraHTCharge>("GetDeliveryPaymentSheetNewTraffic", ParamNew);

                var CompStateCode = ChargeName.CompanyDetails.FirstOrDefault().StateCode;
                //var PartyStateCode= objPostPaymentSheet.PartyStateCode;
                var GSTType = (objPostPaymentSheet.PartyStateCode == CompStateCode) || (objPostPaymentSheet.PartyStateCode == "");
                var cgst = objPostPaymentSheet.lstPostPaymentChrg.FirstOrDefault().CGSTPer;
                var sgst = objPostPaymentSheet.lstPostPaymentChrg.FirstOrDefault().SGSTPer;
                var igst = objPostPaymentSheet.lstPostPaymentChrg.FirstOrDefault().IGSTPer;
                if (SEZ == "SEZWP")
                {
                    GSTType = false;
                }

                if (SEZ == "SEZWOP")
                {
                    cgst = 0;
                    sgst = 0;
                    igst = 0;
                }
                //ExtraChargeName.to
                foreach (var i in ExtraChargeName)
                {
                    objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                    {
                        ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                        Clause = i.Clause,
                        ChargeName = i.ChargeName,
                        ChargeType = i.ChargeType,
                        SACCode = i.SACCode,
                        Quantity = 0,
                        Rate = 0,
                        Amount = i.Taxable,
                        Discount = 0,
                        Taxable = i.Taxable,
                        CGSTPer = cgst,
                        SGSTPer = sgst,
                        IGSTPer = igst,
                        CGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (cgst / 100)) : 0) : 0, 2),
                        SGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (sgst / 100)) : 0) : 0, 2),
                        IGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (i.Taxable * (igst / 100))) : 0, 2),
                        //Total = this.InvoiceType == "Tax" ? (GSTType ? (ActualEntryFees + (ActualEntryFees * (cgst / 100)) + (ActualEntryFees * (sgst / 100))) : (ActualEntryFees + (ActualEntryFees * (igst / 100)))) : ActualEntryFees
                        Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable +
                        (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (cgst / 100)) : 0) : 0, 2)) +
                        (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (i.Taxable * (sgst / 100)) : 0) : 0, 2))) :
                        (i.Taxable + (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (i.Taxable * (igst / 100))) : 0, 2)))) : i.Taxable
                    });
                }



                objPostPaymentSheet.lstPostPaymentChrg.ToList().ForEach(item =>
                {
                    item.Amount = Math.Round(item.Amount, 2);
                    item.CGSTAmt = Math.Round(item.CGSTAmt, 2);
                    item.SGSTAmt = Math.Round(item.SGSTAmt, 2);
                    item.IGSTAmt = Math.Round(item.IGSTAmt, 2);
                    item.Total = Math.Round(item.Total, 2);
                });


                objPostPaymentSheet.TotalAmt = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
                objPostPaymentSheet.TotalDiscount = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Discount), 2);
                objPostPaymentSheet.TotalTaxable = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
                objPostPaymentSheet.TotalCGST = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.CGSTAmt), 2);
                objPostPaymentSheet.TotalSGST = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.SGSTAmt), 2);
                objPostPaymentSheet.TotalIGST = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.IGSTAmt), 2);
                objPostPaymentSheet.CWCTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total), 2);//tax calculated
                objPostPaymentSheet.HTTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total), 2);//tax calculated

                objPostPaymentSheet.CWCAmtTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount), 2);//without tax total
                objPostPaymentSheet.HTAmtTotal = Math.Round(objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Amount), 2);//without tax total
                objPostPaymentSheet.CWCTDS = Math.Round((objPostPaymentSheet.CWCAmtTotal * objPostPaymentSheet.CWCTDSPer) / 100);
                objPostPaymentSheet.HTTDS = Math.Round((objPostPaymentSheet.HTAmtTotal * objPostPaymentSheet.HTTDSPer) / 100);
                objPostPaymentSheet.TDS = Math.Round(objPostPaymentSheet.CWCTDS + objPostPaymentSheet.HTTDS);
                objPostPaymentSheet.TDSCol = objPostPaymentSheet.TDS;
                //this.AllTotal = (this.lstPostPaymentChrg.Sum(o => o.Total)) - this.TDS;
                objPostPaymentSheet.AllTotal = objPostPaymentSheet.CWCTotal + objPostPaymentSheet.HTTotal + objPostPaymentSheet.TDSCol - objPostPaymentSheet.TDS;
                objPostPaymentSheet.RoundUp = Math.Ceiling(objPostPaymentSheet.AllTotal) - objPostPaymentSheet.AllTotal;
                objPostPaymentSheet.InvoiceAmt = Math.Ceiling(objPostPaymentSheet.AllTotal);
                #endregion
                //*******************************************************************************************************
                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
     
        #endregion
        public void AddEditInvoice(PostPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, int BranchId, int Uid,
            string Module, string CargoXML = "")
        {
            var cfsCodeWiseHtRate = HttpContext.Current.Session["lstCfsCodewiseRateHT"];

            string cfsCodeWiseHtRateXML= cfsCodeWiseHtRate==null?"":UtilityClasses.Utility.CreateXML(cfsCodeWiseHtRate);
            string GeneratedClientId = "0";
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
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
                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + GeneratedClientId)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                    try
                    {
                        var LstParam2 = new List<MySqlParameter>();
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_cfsCodeWiseHtRateXML", MySqlDbType = MySqlDbType.Text, Value = cfsCodeWiseHtRateXML });
                        LstParam2.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam2 = LstParam2.ToArray();
                        var DA2 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA2.ExecuteNonQuery("cfsCodeWiseHtRate", CommandType.StoredProcedure, DParam2);
                    }
                    catch { }
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

        public void AddEditInvoice(PostPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, int BranchId, int Uid,
         string Module, string ExportUnder, string CargoXML = "")
        {
            var cfsCodeWiseHtRate = HttpContext.Current.Session["lstCfsCodewiseRateHT"];

            string cfsCodeWiseHtRateXML = cfsCodeWiseHtRate == null ? "" : UtilityClasses.Utility.CreateXML(cfsCodeWiseHtRate);
            string GeneratedClientId = "0";
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
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
                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + GeneratedClientId)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                    try
                    {
                        var LstParam2 = new List<MySqlParameter>();
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_cfsCodeWiseHtRateXML", MySqlDbType = MySqlDbType.Text, Value = cfsCodeWiseHtRateXML });
                        LstParam2.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam2 = LstParam2.ToArray();
                        var DA2 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA2.ExecuteNonQuery("cfsCodeWiseHtRate", CommandType.StoredProcedure, DParam2);
                    }
                    catch { }
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

        public void AddEditInvoice(PostPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, int BranchId, int Uid,
           string Module,string ExportUnder, decimal Mechanical, decimal Manual, decimal distance, string CargoXML = "", decimal Incentive=0)
        {
            var cfsCodeWiseHtRate = HttpContext.Current.Session["lstCfsCodewiseRateHT"];

            string cfsCodeWiseHtRateXML = cfsCodeWiseHtRate == null ? "" : UtilityClasses.Utility.CreateXML(cfsCodeWiseHtRate);
            string GeneratedClientId = "0";
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
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
                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + GeneratedClientId)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                    try
                    {
                        var LstParam2 = new List<MySqlParameter>();
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_cfsCodeWiseHtRateXML", MySqlDbType = MySqlDbType.Text, Value = cfsCodeWiseHtRateXML });
                        LstParam2.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam2 = LstParam2.ToArray();
                        var DA2 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA2.ExecuteNonQuery("cfsCodeWiseHtRate", CommandType.StoredProcedure, DParam2);
                    }
                    catch { }
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


        public void AddEditDestuffingInvoice(PostPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, int BranchId, int Uid,
        string ExportUnder, string Module,decimal Mechanical,decimal Manual,decimal distance, decimal Incentive=0, string CargoXML = "")
        {
            var cfsCodeWiseHtRate = HttpContext.Current.Session["lstCfsCodewiseRateHT"];

            string cfsCodeWiseHtRateXML = cfsCodeWiseHtRate == null ? "" : UtilityClasses.Utility.CreateXML(cfsCodeWiseHtRate);
            string GeneratedClientId = "0";
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.Text, Value = ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Mechanical", MySqlDbType = MySqlDbType.Decimal, Value =Convert.ToDecimal(Mechanical) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Manual", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(Manual) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(distance) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Incentive", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(Incentive) });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditDestuffInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully";
                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + GeneratedClientId)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                    try
                    {
                        var LstParam2 = new List<MySqlParameter>();
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_cfsCodeWiseHtRateXML", MySqlDbType = MySqlDbType.Text, Value = cfsCodeWiseHtRateXML });
                        LstParam2.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam2 = LstParam2.ToArray();
                        var DA2 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA2.ExecuteNonQuery("cfsCodeWiseHtRate", CommandType.StoredProcedure, DParam2);
                    }
                    catch { }
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


        

        public void AddEditECInvoice(PostPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, int BranchId, int Uid,String Module,
      Decimal Weight,string InvoiceFor,string ExporterUnder, string CargoXML = "")
        {
            var cfsCodeWiseHtRate = HttpContext.Current.Session["lstCfsCodewiseRateHT"];

            string cfsCodeWiseHtRateXML = cfsCodeWiseHtRate == null ? "" : UtilityClasses.Utility.CreateXML(cfsCodeWiseHtRate);
            string GeneratedClientId = "0";
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_exportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExporterUnder });

            

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
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
                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + GeneratedClientId)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                    try
                    {
                        var LstParam2 = new List<MySqlParameter>();
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_cfsCodeWiseHtRateXML", MySqlDbType = MySqlDbType.Text, Value = cfsCodeWiseHtRateXML });
                        LstParam2.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam2 = LstParam2.ToArray();
                        var DA2 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA2.ExecuteNonQuery("cfsCodeWiseHtRate", CommandType.StoredProcedure, DParam2);
                    }
                    catch { }
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


        public void AddEditECInvoice(PostPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, int BranchId, int Uid, String Module,
     Decimal Weight, string InvoiceFor, string ExporterUnder, decimal Mechanical, decimal Manual, decimal distance,decimal Incentive=0, string CargoXML = "")
        {
            var cfsCodeWiseHtRate = HttpContext.Current.Session["lstCfsCodewiseRateHT"];

            string cfsCodeWiseHtRateXML = cfsCodeWiseHtRate == null ? "" : UtilityClasses.Utility.CreateXML(cfsCodeWiseHtRate);
            string GeneratedClientId = "0";
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_Mechanical", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(Mechanical) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Manual", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(Manual) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(distance) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Incentive", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(Incentive) });

            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_exportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExporterUnder });



            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditExpInvoice_KOL", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully";
                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + GeneratedClientId)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                    try
                    {
                        var LstParam2 = new List<MySqlParameter>();
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_cfsCodeWiseHtRateXML", MySqlDbType = MySqlDbType.Text, Value = cfsCodeWiseHtRateXML });
                        LstParam2.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam2 = LstParam2.ToArray();
                        var DA2 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA2.ExecuteNonQuery("cfsCodeWiseHtRate", CommandType.StoredProcedure, DParam2);
                    }
                    catch { }
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

        public void AddEditYardInvoice(PostPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, int BranchId, int Uid,
        string Module,string ExportUnder, decimal Mechanical, decimal Manual, decimal distance,decimal Incentive=0, string CargoXML = "")
        {
            var cfsCodeWiseHtRate = HttpContext.Current.Session["lstCfsCodewiseRateHT"];

            string cfsCodeWiseHtRateXML = cfsCodeWiseHtRate == null ? "" : UtilityClasses.Utility.CreateXML(cfsCodeWiseHtRate);
            string GeneratedClientId = "0";
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.Text, Value = ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_TCS", MySqlDbType = MySqlDbType.Decimal, Value = ObjPostPaymentSheet.TCS });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Mechanical", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(Mechanical) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Manual", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(Manual) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(distance) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Incentive", MySqlDbType = MySqlDbType.Decimal, Value = Convert.ToDecimal(Incentive) });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditYardInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully";
                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + GeneratedClientId)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                    try
                    {
                        var LstParam2 = new List<MySqlParameter>();
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_cfsCodeWiseHtRateXML", MySqlDbType = MySqlDbType.Text, Value = cfsCodeWiseHtRateXML });
                        LstParam2.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam2 = LstParam2.ToArray();
                        var DA2 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA2.ExecuteNonQuery("cfsCodeWiseHtRate", CommandType.StoredProcedure, DParam2);
                    }
                    catch { }
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

        public void AddEditInvoice(PostPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, int BranchId, int Uid,
      string Module, Decimal Weight,string ExportUnder,string CargoXML = "")
        {
            var cfsCodeWiseHtRate = HttpContext.Current.Session["lstCfsCodewiseRateHT"];

            string cfsCodeWiseHtRateXML = cfsCodeWiseHtRate == null ? "" : UtilityClasses.Utility.CreateXML(cfsCodeWiseHtRate);
            string GeneratedClientId = "0";
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_exportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
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
                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + GeneratedClientId)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                    try
                    {
                        var LstParam2 = new List<MySqlParameter>();
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_cfsCodeWiseHtRateXML", MySqlDbType = MySqlDbType.Text, Value = cfsCodeWiseHtRateXML });
                        LstParam2.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam2 = LstParam2.ToArray();
                        var DA2 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA2.ExecuteNonQuery("cfsCodeWiseHtRate", CommandType.StoredProcedure, DParam2);
                    }
                    catch { }
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

        public void AddEditInvoice(PostPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, int BranchId, int Uid,
   string Module, Decimal Weight, string ExportUnder, decimal Mechanical, decimal Manual, decimal distance, string CargoXML = "", decimal Incentive = 0)
        {
            var cfsCodeWiseHtRate = HttpContext.Current.Session["lstCfsCodewiseRateHT"];

            string cfsCodeWiseHtRateXML = cfsCodeWiseHtRate == null ? "" : UtilityClasses.Utility.CreateXML(cfsCodeWiseHtRate);
            string GeneratedClientId = "0";
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Weight", MySqlDbType = MySqlDbType.Decimal, Value = Weight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_exportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Mechanical", MySqlDbType = MySqlDbType.Decimal, Value = Mechanical });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Manual", MySqlDbType = MySqlDbType.Decimal, Value = Manual });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal, Value = distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Incentive", MySqlDbType = MySqlDbType.Decimal, Value = Incentive });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditExpInvoice_KOL", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully";
                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + GeneratedClientId)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                    try
                    {
                        var LstParam2 = new List<MySqlParameter>();
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_cfsCodeWiseHtRateXML", MySqlDbType = MySqlDbType.Text, Value = cfsCodeWiseHtRateXML });
                        LstParam2.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam2 = LstParam2.ToArray();
                        var DA2 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA2.ExecuteNonQuery("cfsCodeWiseHtRate", CommandType.StoredProcedure, DParam2);
                    }
                    catch { }
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

        public void AddEditLoadedInvoice(PostPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, int BranchId, int Uid,
    string Module,string ExportUnder, decimal MechanicalWeight, decimal ManualWeight, decimal Distance,  string CargoXML = "")
        {
            var cfsCodeWiseHtRate = HttpContext.Current.Session["lstCfsCodewiseRateHT"];

            string cfsCodeWiseHtRateXML = cfsCodeWiseHtRate == null ? "" : UtilityClasses.Utility.CreateXML(cfsCodeWiseHtRate);
            string GeneratedClientId = "0";
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.Text, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExpoterUnder", MySqlDbType = MySqlDbType.VarChar, Value =ExportUnder });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Distance", MySqlDbType = MySqlDbType.Decimal, Value = Distance });
            LstParam.Add(new MySqlParameter { ParameterName = "in_MechanicalWeight", MySqlDbType = MySqlDbType.Decimal, Value = MechanicalWeight });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ManualWeight", MySqlDbType = MySqlDbType.Decimal, Value = ManualWeight });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditLoadedInvoice", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully";
                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + GeneratedClientId)));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                    try
                    {
                        var LstParam2 = new List<MySqlParameter>();
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = GeneratedClientId });
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_cfsCodeWiseHtRateXML", MySqlDbType = MySqlDbType.Text, Value = cfsCodeWiseHtRateXML });
                        LstParam2.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam2 = LstParam2.ToArray();
                        var DA2 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA2.ExecuteNonQuery("cfsCodeWiseHtRate", CommandType.StoredProcedure, DParam2);
                    }
                    catch { }
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
        #region Empty Container Payment Sheet New
        public void GetEmptyContPaymentSheetNew(string InvoiceDate, int DestuffingAppId, int DeliveryType, string DestuffingAppNo, string DestuffingAppDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, string InvoiceType, string ContainerXML,
            string InvoiceFor, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceFor });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetEmptyContPaymentSheetNew", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = DestuffingAppId;
                objPostPaymentSheet.RequestNo = DestuffingAppNo;
                objPostPaymentSheet.RequestDate = DestuffingAppDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode;
                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));
                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + " , ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateCharges(10, ChargeName);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        #endregion



        #region Empty Container Payment Sheet Gate In 
        public void GetEmptyContPaySheetGateIn(string InvoiceDate, int DestuffingAppId, int DeliveryType, string DestuffingAppNo, string DestuffingAppDate, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, string InvoiceType, string ContainerXML,
            string InvoiceFor, int InvoiceId)
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ApplicationId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingAppId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.Text, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceFor });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EximTraderId", MySqlDbType = MySqlDbType.Int32, Value = PartyId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetEmptyContPaySheetGateIn", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = DestuffingAppId;
                objPostPaymentSheet.RequestNo = DestuffingAppNo;
                objPostPaymentSheet.RequestDate = DestuffingAppDate;
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode;
                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;
                objPostPaymentSheet.DeliveryType = DeliveryType;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));
                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + " , ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                objPostPaymentSheet.CalculateCharges(11, ChargeName);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        #endregion


        #region Empty Container Out Payment Sheet
        public void GetEmptyContOutPaymentSheet(string InvoiceDate, int DeliveryType,string SEZ,int SPartyId, int PartyId, string PartyName,
            string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName,  int SecondInvoiceFlag, int PartyIdSec, String PartyNameSec, String PartyGSTSec, String PartyAddressSec, String PartyStateSec, String PartyStateCodeSec, int PayeeIdSec, String PayeeNameSec,string InvoiceType, string ContainerXML,decimal WeighmentCharges,
             int InvoiceId)
        {
            int Status = 0;
            int FreePeriod = 0;         
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(InvoiceDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyId", MySqlDbType = MySqlDbType.Int32, Value = SPartyId });
            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.LongText, Value = ContainerXML });
         //   LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceFor", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = InvoiceFor });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceId", MySqlDbType = MySqlDbType.Int32, Value = InvoiceId });
            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            _DBResponse = new DatabaseResponse();
            try
            {
                var objPrePaymentSheet = (PrePaymentSheet)DataAccess.ExecuteDynamicSet<PrePaymentSheet>("GetEmptyContOutPaymentSheet", DParam);
                var objPostPaymentSheet = new PostPaymentSheet();
                objPostPaymentSheet.InvoiceType = InvoiceType;
                objPostPaymentSheet.InvoiceDate = InvoiceDate;
                objPostPaymentSheet.RequestId = objPrePaymentSheet.lstPreInvoiceHdr[0].RequestId;
                objPostPaymentSheet.RequestNo = objPrePaymentSheet.lstPreInvoiceHdr[0].RequestNo;
                objPostPaymentSheet.RequestDate = objPrePaymentSheet.lstPreInvoiceHdr[0].RequestDate; 
                objPostPaymentSheet.PartyId = PartyId;
                objPostPaymentSheet.PartyName = PartyName;
                objPostPaymentSheet.PartyAddress = PartyAddress;
                objPostPaymentSheet.PartyState = PartyState;
                objPostPaymentSheet.PartyStateCode = PartyStateCode;
                objPostPaymentSheet.PartyGST = PartyGST;
                objPostPaymentSheet.PayeeId = PayeeId;
                objPostPaymentSheet.PayeeName = PayeeName;

                objPostPaymentSheet.PartyIdSec = PartyIdSec;
                objPostPaymentSheet.PartyNameSec = PartyNameSec;
                objPostPaymentSheet.PartyAddressSec = PartyAddressSec;
                objPostPaymentSheet.PartyStateSec = PartyStateSec;
                objPostPaymentSheet.PartyStateCodeSec = PartyStateCodeSec;
                objPostPaymentSheet.PartyGSTSec = PartyGSTSec;
                objPostPaymentSheet.PayeeIdSec = PayeeIdSec;
                objPostPaymentSheet.PayeeNameSec = PayeeNameSec;
                objPostPaymentSheet.SecondInv = SecondInvoiceFlag;
                
                objPostPaymentSheet.DeliveryType = DeliveryType;
                FreePeriod = objPrePaymentSheet.lstPreInvoiceHdr[0].FreePeriod;

                objPostPaymentSheet.lstPreContWiseAmount = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PreContainerWiseAmount>>(Newtonsoft.Json.JsonConvert.SerializeObject(objPrePaymentSheet.lstPreContWiseAmount));
                objPrePaymentSheet.lstPreInvoiceCont.ToList().ForEach(item =>
                {
                    if (!objPostPaymentSheet.ShippingLineName.Contains(item.ShippingLineName))
                        objPostPaymentSheet.ShippingLineName += item.ShippingLineName + ", ";
                    if (!objPostPaymentSheet.CHAName.Contains(item.CHAName))
                        objPostPaymentSheet.CHAName += item.CHAName + ", ";
                    if (!objPostPaymentSheet.ImporterExporter.Contains(item.ImporterExporter))
                        objPostPaymentSheet.ImporterExporter += item.ImporterExporter + ", ";
                    if (!objPostPaymentSheet.BOENo.Contains(item.BOENo))
                        objPostPaymentSheet.BOENo += item.BOENo + ", ";
                    if (!objPostPaymentSheet.BOEDate.Contains(item.BOEDate))
                        objPostPaymentSheet.BOEDate += item.BOEDate + ", ";
                    if (!objPostPaymentSheet.CFSCode.Contains(item.CFSCode))
                        objPostPaymentSheet.CFSCode += item.CFSCode + ", ";
                    if (!objPostPaymentSheet.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                        objPostPaymentSheet.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + " , ";
                    if (!objPostPaymentSheet.DestuffingDate.Contains(item.DestuffingDate))
                        objPostPaymentSheet.DestuffingDate += item.DestuffingDate + ", ";
                    if (!objPostPaymentSheet.StuffingDate.Contains(item.StuffingDate))
                        objPostPaymentSheet.StuffingDate += item.StuffingDate + ", ";
                    if (!objPostPaymentSheet.CartingDate.Contains(item.CartingDate))
                        objPostPaymentSheet.CartingDate += item.CartingDate + ", ";
                    if (!objPostPaymentSheet.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                    {
                        objPostPaymentSheet.lstPostPaymentCont.Add(new PostPaymentContainer()
                        {
                            CargoType = item.CargoType,
                            CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                            CFSCode = item.CFSCode,
                            CIFValue = item.CIFValue,
                            ContainerNo = item.ContainerNo,
                            ArrivalDate = item.ArrivalDate,
                            ArrivalTime = item.ArrivalTime,
                            DeliveryType = item.DeliveryType,
                            DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                            Duty = item.Duty,
                            GrossWt = item.GrossWeight,
                            Insured = item.Insured,
                            NoOfPackages = item.NoOfPackages,
                            Reefer = item.Reefer,
                            RMS = item.RMS,
                            Size = item.Size,
                            SpaceOccupied = item.SpaceOccupied,
                            SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                            StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                            WtPerUnit = item.WtPerPack,
                            AppraisementPerct = item.AppraisementPerct,
                            HeavyScrap = item.HeavyScrap,
                            StuffCUM = item.StuffCUM
                        });
                    }
                    objPostPaymentSheet.lstStorPostPaymentCont.Add(new PostPaymentContainer()
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });

                    objPostPaymentSheet.TotalNoOfPackages = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.NoOfPackages);
                    objPostPaymentSheet.TotalGrossWt = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.GrossWeight);
                    objPostPaymentSheet.TotalWtPerUnit = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.WtPerPack);
                    objPostPaymentSheet.TotalSpaceOccupied = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.SpaceOccupied);
                    objPostPaymentSheet.TotalSpaceOccupiedUnit = objPrePaymentSheet.lstPreInvoiceCont.FirstOrDefault().SpaceOccupiedUnit;
                    objPostPaymentSheet.TotalValueOfCargo = objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.CIFValue)
                        + objPrePaymentSheet.lstPreInvoiceCont.Sum(o => o.Duty);
                });

                
                var ChargeName = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");                
                objPostPaymentSheet.KdlCalculateCharges(12, ChargeName, FreePeriod,SEZ, WeighmentCharges);

                _DBResponse.Status = 1;
                _DBResponse.Message = "Success";
                _DBResponse.Data = objPostPaymentSheet;
            }
            catch (Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "No Data";
                _DBResponse.Data = null;
            }
            finally
            {

            }
        }
        public void GetBOLForEmptyContOut( string RequestNo,int DestuffingId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_RequestNo", MySqlDbType = MySqlDbType.VarChar, Size = 10, Value = RequestNo });
            lstParam.Add(new MySqlParameter { ParameterName = "in_DestuffingId", MySqlDbType = MySqlDbType.Int32, Value = DestuffingId });
            IDataParameter[] dparam = lstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("GetBOLForEmpContOut", CommandType.StoredProcedure, dparam);
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
        

        public void AddEditContOutInvoice(PostPaymentSheet ObjPostPaymentSheet, string ContainerXML, string ChargesXML, string ContWiseChargeXML, int BranchId, int Uid,
          string Module,string ExportUnder, string CargoXML = "")
        {
            var cfsCodeWiseHtRate = HttpContext.Current.Session["lstCfsCodewiseRateHT"];

            string cfsCodeWiseHtRateXML = cfsCodeWiseHtRate == null ? "" : UtilityClasses.Utility.CreateXML(cfsCodeWiseHtRate);
            string GeneratedClientId = "0";
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
            LstParam.Add(new MySqlParameter { ParameterName = "in_SecondInv", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.SecondInv });

            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyIdSec", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PartyIdSec });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyNameSec", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyNameSec });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeIdSec", MySqlDbType = MySqlDbType.Int32, Value = ObjPostPaymentSheet.PayeeIdSec });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PayeeNameSec", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PayeeNameSec });
            LstParam.Add(new MySqlParameter { ParameterName = "in_GSTNoSec", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyGSTSec });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyAddressSec", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyAddressSec });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateSec", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateSec });
            LstParam.Add(new MySqlParameter { ParameterName = "in_PartyStateCodeSec", MySqlDbType = MySqlDbType.VarChar, Value = ObjPostPaymentSheet.PartyStateCodeSec });


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

            LstParam.Add(new MySqlParameter { ParameterName = "ContainerXML", MySqlDbType = MySqlDbType.LongText, Value = ContainerXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ChargesXML", MySqlDbType = MySqlDbType.LongText, Value = ChargesXML });
            LstParam.Add(new MySqlParameter { ParameterName = "ContWiseChargeXML", MySqlDbType = MySqlDbType.LongText, Value = ContWiseChargeXML });
            LstParam.Add(new MySqlParameter { ParameterName = "CargoXML", MySqlDbType = MySqlDbType.LongText, Value = CargoXML });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtml", MySqlDbType = MySqlDbType.LongText, Value = ObjPostPaymentSheet.InvoiceHtml });
            LstParam.Add(new MySqlParameter { ParameterName = "in_InvoiceHtmlSec", MySqlDbType = MySqlDbType.LongText, Value = ObjPostPaymentSheet.InvoiceHtmlSec });
            LstParam.Add(new MySqlParameter { ParameterName = "in_ExportUnder", MySqlDbType = MySqlDbType.VarChar, Value = ExportUnder });

            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.VarChar, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DA.ExecuteNonQuery("AddEditExpInvoiceContOut", CommandType.StoredProcedure, DParam, out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if (Result == 1)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Payment Invoice Saved Successfully";

                    var InvNo = GeneratedClientId.Split(',');
                    int leng = InvNo.Length;
                    try
                    {
                        var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtml));
                        var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + InvNo[0])));
                        var LstParam1 = new List<MySqlParameter>();
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = InvNo[0] });
                        LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                        LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam1 = LstParam1.ToArray();
                        var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);
                    }
                    catch { }
                    try
                    {
                        var LstParam2 = new List<MySqlParameter>();
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = InvNo[0] });
                        LstParam2.Add(new MySqlParameter { ParameterName = "in_cfsCodeWiseHtRateXML", MySqlDbType = MySqlDbType.Text, Value = cfsCodeWiseHtRateXML });
                        LstParam2.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                        IDataParameter[] DParam2 = LstParam2.ToArray();
                        var DA2 = DataAccessLayerFactory.GetDataAccessLayer();
                        DA2.ExecuteNonQuery("cfsCodeWiseHtRate", CommandType.StoredProcedure, DParam2);
                    }
                    catch { }

                    if (leng > 1)
                    {

                        try
                        {
                            var strHtmlOriginal = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(ObjPostPaymentSheet.InvoiceHtmlSec));
                            var strModified = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strHtmlOriginal.Replace("Invoice No. <span>", "Invoice No. <span>" + InvNo[2])));
                            var LstParam1 = new List<MySqlParameter>();
                            LstParam1.Add(new MySqlParameter { ParameterName = "in_InvNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = InvNo[2] });
                            LstParam1.Add(new MySqlParameter { ParameterName = "in_InvHtml", MySqlDbType = MySqlDbType.Text, Value = strModified });
                            LstParam1.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                            IDataParameter[] DParam1 = LstParam1.ToArray();
                            var DA1 = DataAccessLayerFactory.GetDataAccessLayer();
                            DA1.ExecuteNonQuery("UpdateInvoiceHtmlReport", CommandType.StoredProcedure, DParam1);
                        }
                        catch { }
                        try
                        {
                            var LstParam2 = new List<MySqlParameter>();
                            LstParam2.Add(new MySqlParameter { ParameterName = "in_InvoiceNo", MySqlDbType = MySqlDbType.VarChar, Size = 45, Value = InvNo[2] });
                            LstParam2.Add(new MySqlParameter { ParameterName = "in_cfsCodeWiseHtRateXML", MySqlDbType = MySqlDbType.Text, Value = cfsCodeWiseHtRateXML });
                            LstParam2.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = 0 });
                            IDataParameter[] DParam2 = LstParam2.ToArray();
                            var DA2 = DataAccessLayerFactory.GetDataAccessLayer();
                            DA2.ExecuteNonQuery("cfsCodeWiseHtRate", CommandType.StoredProcedure, DParam2);
                        }
                        catch { }
                    }
                }
                else if (Result == 2)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Payment Invoice Updated Successfully";
                }


                else if (Result == 4)
                {
                    _DBResponse.Data = GeneratedClientId;
                    _DBResponse.Status = 4;
                    _DBResponse.Message = GeneratedClientId;
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


    }
}