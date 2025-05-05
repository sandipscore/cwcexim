using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSRPestCertificate
    {
        public string CertificateNo { get; set; }
        public string IssueDate { get; set; }

        public string FamugationDate { get; set; }

        public string PortOfDischarge { get; set; }
        public string GoodsDesc { get; set; }
        public string Quantity { get; set; }
        public string DistingMarks { get; set; }
        public string ContainerNo { get; set; }
        public string PortLoading { get; set; }
        public string NameOfVessel { get; set; }
        public string Destination { get; set; }
        public string Exporter { get; set; }

        public string Consignee { get; set; }
        public string NameOfFumigant { get; set; }
        public string DateOfFumi { get; set; }
        public string Place { get; set; }
        public string Dosage { get; set; }
        public string Duration { get; set; }
        public string Temp { get; set; }
       
        
        public string GasTight { get; set; }
        public string PackType { get; set; }


    }
}