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
   public  class StuffingSBJsonFormat
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string ContStuffingDetJson(DataSet ds,string ContainerNo="")
        {
            try
            {
                
                ICES_SCMTRStuffingMsgExchange obj = new ICES_SCMTRStuffingMsgExchange();

                #region Data Binding 
                // Binding Header Data
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        if(Convert.ToString(dr["ContainerNo"])== ContainerNo)
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
                // Binding declaration
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        if (Convert.ToString(dr["ContainerNo"]) == ContainerNo)
                        {

                            obj.master.declaration.messageType = Convert.ToString(dr["messageType"]);
                            obj.master.declaration.portOfReporting = Convert.ToString(dr["portOfReporting"]);
                            obj.master.declaration.jobNo = Convert.ToInt32(dr["jobNo"]);
                            obj.master.declaration.jobDate = Convert.ToString(dr["jobDate"]);
                            obj.master.declaration.reportingEvent = Convert.ToString(dr["reportingEvent"]);
                        }
                    }

                }
                // Binding location
                if (ds.Tables[2].Rows.Count > 0)
                {
                    obj.master.location.reportingPartyType = Convert.ToString(ds.Tables[2].Rows[0]["reportingPartyType"]);
                    obj.master.location.reportingPartyCode = Convert.ToString(ds.Tables[2].Rows[0]["reportingPartyCode"]);
                    obj.master.location.reportingLocationCode = Convert.ToString(ds.Tables[2].Rows[0]["reportingLocationCode"]);
                    obj.master.location.reportingLocationName = Convert.ToString(ds.Tables[2].Rows[0]["reportingLocationName"]);
                    obj.master.location.authorisedPersonPAN = Convert.ToString(ds.Tables[2].Rows[0]["authorisedPersonPAN"]);


                }

               
                //Binding Container Details

                if (ds.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow cargoContainerdr in ds.Tables[3].Rows)
                    {


                        // Binding cargoDetails
                        List<cargoDetails> lstCargoDetails = new List<cargoDetails>();
                        int i = 1;
                        foreach (DataRow cargoDetailsdr in ds.Tables[4].Rows)
                        {
                            if(Convert.ToString(cargoDetailsdr["ContainerNo"])== ContainerNo)
                            {
                                lstCargoDetails.Add(new cargoDetails
                                {
                                    messageType = Convert.ToString(cargoDetailsdr["messageType"]),
                                    cargoSequenceNo = Convert.ToInt32(cargoDetailsdr["cargoSequenceNo"]),
                                    documentType = Convert.ToString(cargoDetailsdr["documentType"]),
                                    documentSite = Convert.ToString(cargoDetailsdr["documentSite"]),
                                    documentNumber = Convert.ToString(cargoDetailsdr["documentNumber"]),
                                    documentDate = Convert.ToString(cargoDetailsdr["documentDate"]),
                                    shipmentLoadStatus = Convert.ToString(cargoDetailsdr["shipmentLoadStatus"]),
                                    packageType = Convert.ToString(cargoDetailsdr["packageType"]),
                                    quantity = Convert.ToInt64(cargoDetailsdr["quantity"]),
                                    packetsFrom = Convert.ToInt32(cargoDetailsdr["packetsFrom"]),
                                    packetsTo = Convert.ToInt32(cargoDetailsdr["packetsTo"]),
                                    packUQC = Convert.ToString(cargoDetailsdr["packUQC"]),
                                    mcinPcin = Convert.ToString(cargoDetailsdr["mcinPcin"]),
                                });
                                i++;
                            }
                           

                        }



                        if(Convert.ToString(cargoContainerdr["equipmentID"])==ContainerNo)
                        {
                            obj.master.cargoContainer.Add(new cargoContainer
                            {
                                messageType = Convert.ToString(cargoContainerdr["messageType"]),
                                equipmentSequenceNo = Convert.ToInt32(cargoContainerdr["equipmentSequenceNo"]),
                                equipmentID = Convert.ToString(cargoContainerdr["equipmentID"]),
                                equipmentType = Convert.ToString(cargoContainerdr["equipmentType"]),
                                equipmentSize = Convert.ToString(cargoContainerdr["equipmentSize"]),
                                equipmentLoadStatus = Convert.ToString(cargoContainerdr["equipmentLoadStatus"]),
                                finalDestinationLocation = Convert.ToString(cargoContainerdr["finalDestinationLocation"]),
                                eventDate = Convert.ToString(cargoContainerdr["eventDate"]),
                                equipmentSealType = Convert.ToString(cargoContainerdr["equipmentSealType"]),
                                equipmentSealNumber = Convert.ToString(cargoContainerdr["equipmentSealNumber"]),
                                equipmentStatus = Convert.ToString(cargoContainerdr["equipmentStatus"]),
                                equipmentQuantity = Convert.ToDecimal(cargoContainerdr["equipmentQuantity"]),
                                EquipmentQUC = Convert.ToString(cargoContainerdr["EquipmentQUC"]),
                                cargoDetails = lstCargoDetails,



                            });
                        }

                       

                    }
                }
                #endregion
                string json = JsonConvert.SerializeObject(obj);

               
                return json;
               // string FileName = strFolderName + "COCHE01_" + dttvalue + "_" + TimeVall + ".sb";
            }
            catch (Exception ex)
            {
                log.Error("CIM-SF Error Message:"+ex.Message);
                return "";
            }



         
        }

    }
}
