using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class WFLDGatePass: GatePassCh
    {
        public string Module { get; set; }       
        public string VehicleXml { get; set; } 
        public string PortOfDispatch { get; set; }
         
    }
    public class GatePassCh
    {
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public string Remarks { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string InvoiceNo { get; set; }
        public int InvoiceId { get; set; }
        public string StringifyData { get; set; }
        public string CHAName { get; set; }
        public string ImpExpName { get; set; }
        public string ShippingLineName { get; set; }
        public string DeliveryDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string DepartureDate { get; set; }
       [Required(ErrorMessage = "Fill Out This Field")]
        public string ArrivalDate { get; set; }
    }
    public class WFLDInvoiceNoList: InvoiceNoListCh
    {
        public string Module { get; set; }
        public string ContainerNo { get; set; }
    }

    public class InvoiceNoListCh
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
    }
    public class WFLDGatepassVehicle
    {
        public int Id { get; set; } = 0;
        public int GatePassId { get; set; } = 0;

        public string ContainerNo { get; set; }
        public string VehicleNo { get; set; }
        public decimal Package { get; set; } = 0;
    }

    public class WFLD_ContainerDet
    {
        public int GatePassDtlId { get; set; }
        public string ContainerNo { get; set; }
        public bool IsReefer { get; set; }
        public string Size { get; set; }
       
        public string CargoDescription { get; set; }
        public int CargoType { get; set; }
        public int NoOfUnits { get; set; }
        public string VehicleNo { get; set; }
        public decimal Weight { get; set; }
        public string Location { get; set; }

        public string PortOfDispatch { get; set; }
    }

    public class WFLD_GPHdr
    {
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public string Remarks { get; set; }
        public string ExpiryDate { get; set; }
        public string CHAName { get; set; }
        public string ImpExpName { get; set; }
        public string ShippingLineName { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string EntryDate { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceType { get; set; } 
        public string Module { get; set; }
        public IList<WFLD_GPDet> lstDet { get; set; } = new List<WFLD_GPDet>();
        public IList<WFLD_Header> lstHed { get; set; } = new List<WFLD_Header>();
    }
    public class WFLD_GPDet
    {
        public string ContainerNo { get; set; }
        public decimal NoOfUnits { get; set; }
        public string VehicleNo { get; set; }
        public decimal Weight { get; set; }
        public int CargoType { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string LineNo { get; set; }
        public string Rotation { get; set; }
        public string Location { get; set; }
        public string PortOfDispatch { get; set; }
        public string ICDCode { get; set; }
        public string ShippingLineNo { get; set; }
        public string CargoDescription { get; set; }

        public string OBLNO { get; set; }
        public string EntryDate { get; set; }
        public string IGMNo { get; set; }
        public string InDate { get; set; }
        public string DestuffingDate { get; set; }
        public string CustomSealNo { get; set; }

        public string InvoiceNo { get; set; }

        public string IssueSlipNo { get; set; }
        public string IssueSlipDate { get; set; }
        public string CreatedTime { get; set; }
        public string NoOfShipBill { get; set; }
        public string Size { get; set; }
        public string ContainerType { get; set; }
        public string POD { get; set; }
        public string Forwarder { get; set; }
       

    }
    public class WFLD_Header
    {    
        public string InvoiceType { get; set; }
        public string CFSCode { get; set; }

    }
    public class WFLDListOfGP
    {
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; } 
        public string GatePassDate { get; set; }
        public string InvoiceNo { get; set; }
        public string IsCancelled { get; set; }
    }

}