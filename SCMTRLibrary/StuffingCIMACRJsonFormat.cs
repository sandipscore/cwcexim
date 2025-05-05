using System;
using System.Collections.Generic;
using System.Linq;

using System.Collections;
using System.Configuration;
using System.Data;

using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using MySql.Data.MySqlClient;

using Newtonsoft.Json;
using SCMTRLibrary.Models;

namespace SCMTRLibrary
{
   

   public class StuffingCIMACRJsonFormat
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string StuffingCIMACRJson(DataSet ds,int ContainerStuffingDtlId=0)
        {
            int k = 0;
            int j = 1;
            ICES_SCMTRStuffingCIMASR obj = new ICES_SCMTRStuffingCIMASR();
            try
            {
                #region Data Binding 
                // Header Databinding

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(dr["ContainerStuffingDtlId"]) == ContainerStuffingDtlId)
                        {
                            obj.headerField.indicator = Convert.ToString(dr["indicator"]);
                            obj.headerField.messageID = Convert.ToString(dr["messageID"]);
                            obj.headerField.sequenceOrControlNumber = Convert.ToInt32(dr["sequenceOrControlNumber"]);
                            obj.headerField.date = Convert.ToString(dr["date"]);
                            obj.headerField.time = Convert.ToString(dr["Time"]);
                            obj.headerField.reportingEvent = Convert.ToString(dr["reportingEvent"]);
                            obj.headerField.senderID = Convert.ToString(dr["senderID"]);
                            obj.headerField.receiverID = Convert.ToString(dr["receiverID"]);
                            obj.headerField.versionNo = Convert.ToString(dr["versionNo"]);
                        }

                    }

                }
                // declaration Databinding
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {

                        if (Convert.ToInt32(dr["ContainerStuffingDtlId"]) == ContainerStuffingDtlId)
                        {
                            obj.master.declaration.messageType = Convert.ToString(dr["messageType"]);
                            obj.master.declaration.portOfReporting = Convert.ToString(dr["portOfReporting"]);
                            obj.master.declaration.jobNo = Convert.ToInt32(dr["jobNo"]);
                            obj.master.declaration.jobDate = Convert.ToString(dr["jobDate"]);
                            obj.master.declaration.reportingEvent = Convert.ToString(dr["reportingEvent"]);
                        }

                    }

                }

                //location Databinding
                if (ds.Tables[2].Rows.Count > 0)
                {
                    obj.master.location.reportingPartyType = Convert.ToString(ds.Tables[2].Rows[0]["reportingPartyType"]);
                    obj.master.location.reportingPartyCode = Convert.ToString(ds.Tables[2].Rows[0]["reportingPartyCode"]);
                    obj.master.location.reportingLocationCode = Convert.ToString(ds.Tables[2].Rows[0]["reportingLocationCode"]);
                    obj.master.location.reportingLocationName = Convert.ToString(ds.Tables[2].Rows[0]["reportingLocationName"]);
                    obj.master.location.authorisedPersonPAN = Convert.ToString(ds.Tables[2].Rows[0]["authorisedPersonPAN"]);
                }

                //cargoItnry Databinding
                List<cargoItnry> lstcargoItnry = new List<cargoItnry>();
                if (ds.Tables[5].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[5].Rows)
                    {
                        //if(Convert.ToInt32(dr["ContainerStuffingDtlId"])== ContainerStuffingDtlId)
                        //{
                            lstcargoItnry.Add(new cargoItnry
                            {
                                prtOfCallSeqNmbr = Convert.ToInt32(dr["prtOfCallSeqNmbr"]),
                                prtOfCallCdd = Convert.ToString(dr["prtOfCallCdd"]),
                                prtOfCallName = Convert.ToString(dr["prtOfCallName"]),
                                nxtPrtOfCallCdd = Convert.ToString(dr["nxtPrtOfCallCdd"]),
                                nxtPrtOfCallName = Convert.ToString(dr["nxtPrtOfCallName"]),
                                modeOfTrnsprt = Convert.ToString(dr["modeOfTrnsprt"])

                            });
                        //}
                        
                    }
                }
                if (ds.Tables[3].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[3].Rows)
                    {

                        // cargoContainer DataBinding
                        if (Convert.ToInt32(dr["ContainerStuffingDtlId"]) == ContainerStuffingDtlId)
                        {
                            List<CIMASRCargoContainer> lstCIMASRCargoContainer = new List<CIMASRCargoContainer>();
                            if (ds.Tables[4].Rows.Count > 0)
                            {
                             
                                foreach (DataRow Containerdr in ds.Tables[4].Rows)
                                {
                                    if (Convert.ToString(dr["documentNo"]) == Convert.ToString(Containerdr["ShippingBillNo"]))
                                    {
                                        if (Convert.ToInt32(Containerdr["ContainerStuffingDtlId"]) == ContainerStuffingDtlId)
                                        {
                                            int L = 1;
                                            lstCIMASRCargoContainer.Add(new CIMASRCargoContainer
                                            {

                                                equipmentSequenceNo = Convert.ToInt32(Containerdr["equipmentSequenceNo"]),
                                                messageType = Convert.ToString(Containerdr["messageType"]),
                                                equipmentID = Convert.ToString(Containerdr["equipmentID"]),
                                                equipmentType = Convert.ToString(Containerdr["equipmentType"]),
                                                equipmentSize = Convert.ToString(Containerdr["equipmentSize"]),
                                                additionalEquipmentHold = Convert.ToString(Containerdr["additionalEquipmentHold"]),
                                                equipmentLoadStatus = Convert.ToString(Containerdr["equipmentLoadStatus"]),
                                                finalDestinationLocation = Convert.ToString(Containerdr["finalDestinationLocation"]),
                                                eventDate = Convert.ToString(Containerdr["eventDate"]),
                                                equipmentSealType = Convert.ToString(Containerdr["equipmentSealType"]),
                                                equipmentSealNumber = Convert.ToString(Containerdr["equipmentSealNumber"]),
                                                otherEquipmentID = Convert.ToString(Containerdr["otherEquipmentID"]),
                                                equipmentStatus = Convert.ToString(Containerdr["equipmentStatus"]),
                                                equipmentQuantity = Convert.ToDecimal(Containerdr["equipmentQuantity"]),
                                                equipmentQUC = Convert.ToString(Containerdr["equipmentQUC"]),

                                            });
                                            L++;
                                        }

                                    }


                                }
                            }

                            CIMASRCargoDetails idata = new CIMASRCargoDetails();
                            idata.messageType = Convert.ToString(dr["messageType"]);
                            idata.cargoSequenceNo = Convert.ToInt32(dr["cargoSequenceNo"]);
                            idata.documentType = Convert.ToString(dr["documentType"]);
                            idata.documentSite = Convert.ToString(dr["documentSite"]);
                            idata.documentNo = Convert.ToInt32(dr["documentNo"]);
                            idata.documentDate = Convert.ToString(dr["documentDate"]);
                            idata.shipmentLoadStatus = Convert.ToString(dr["shipmentLoadStatus"]);
                            idata.packageType = Convert.ToString(dr["packageType"]);
                            idata.quantity = Convert.ToDecimal(dr["quantity"]);
                            idata.packetsFrom = Convert.ToInt32(dr["packetsFrom"]);
                            idata.packetsTo = Convert.ToInt32(dr["packetsTo"]);
                            idata.packUQC = Convert.ToString(dr["packUQC"]);
                            idata.MCINPCIN = Convert.ToString(dr["MCINPCIN"]);
                            idata.cargoContainer = lstCIMASRCargoContainer;
                            idata.cargoItnry = lstcargoItnry;



                            obj.master.cargoDetails.Add(idata);
                        }

                    }
                }









                #endregion

                string json = JsonConvert.SerializeObject(obj);


                return json;
            }
            catch(Exception ex)
            {
                log.Error("Error Message CIMASR:" + ex.Message);
                return "";
                
            }





            




        }
    }


    }

