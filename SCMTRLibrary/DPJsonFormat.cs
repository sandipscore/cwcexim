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
  
    public class DPJsonFormat
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static string DPJsonCreation(DataSet ds,int gatepassdtlid)
        {

            int k = 0;
            int j = 1;
            DPModel obj = new DPModel();
            try
            {
                #region Data Binding 
                // Header Databinding

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        if(Convert.ToInt32(dr["gatepassdtlid"])== gatepassdtlid)
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
                    foreach(DataRow dr in ds.Tables[1].Rows)
                    {
                        if(Convert.ToInt32(dr["gatepassdtlid"])==gatepassdtlid)
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
                    obj.master.location.nextDestinationOfUnlading=Convert.ToString(ds.Tables[2].Rows[0]["nextDestinationOfUnlading"]);
                    obj.master.location.bondNumber = Convert.ToString(ds.Tables[2].Rows[0]["bondNumber"]);
                }

                // transportMeans
                if (ds.Tables[3].Rows.Count > 0)
                {
                    foreach(DataRow dr in ds.Tables[3].Rows)
                    {
                        if(Convert.ToInt32(dr["GatePassDtlId"])==gatepassdtlid)
                        {
                            obj.master.transportMeans.totalEquipments = Convert.ToInt32(dr["totalEquipments"]);
                            obj.master.transportMeans.transportMeansNumber = Convert.ToString(dr["transportMeansNumber"]);
                            obj.master.transportMeans.transportMeansType = Convert.ToString(dr["transportMeansType"]);
                        }
                      
                    }
                   
                 
                }

                //events
                if (ds.Tables[4].Rows.Count > 0)
                {
                    obj.master.events.expectedTimeOfArrival = Convert.ToString(ds.Tables[4].Rows[0]["expectedTimeOfArrival"]);
                    obj.master.events.expectedTimeOfDeparture = Convert.ToString(ds.Tables[4].Rows[0]["expectedTimeOfDeparture"]);
                  

                }



                //cargoItnry Databinding
               
                if (ds.Tables[5].Rows.Count > 0)
                {
                    int L = 1;
                    foreach (DataRow dr in ds.Tables[5].Rows)
                    {
                        if (Convert.ToInt32(dr["GatePassDtlId"]) == gatepassdtlid)
                        {


                            // cargoContainer DataBinding
                            DPcargoContainer lstCIMASRCargoContainer = new DPcargoContainer();
                            List<DPcargoDocument> lstcargoItnry = new List<DPcargoDocument>();
                            if (ds.Tables[6].Rows.Count > 0)
                            {


                                if (ds.Tables[6].Rows.Count > 0)
                                {
                                    int a = 1;
                                    foreach (DataRow ddr in ds.Tables[6].Rows)
                                    {


                                        if (Convert.ToString(ddr["ContainerNo"]) == Convert.ToString(dr["equipmentID"]))
                                        {
                                            
                                            if (Convert.ToInt32(ddr["GatePassDtlId"]) == gatepassdtlid)
                                            {
                                                lstcargoItnry.Add(new DPcargoDocument
                                                {
                                                    cargoSequenceNo = Convert.ToInt32(ddr["cargoSequenceNo"]),
                                                    documentDate = Convert.ToString(ddr["documentDate"]),
                                                    documentNumber = Convert.ToInt32(ddr["documentNumber"]),
                                                    documentSite = Convert.ToString(ddr["documentSite"]),
                                                    documentType = Convert.ToString(ddr["documentType"]),
                                                    MCINPCIN = Convert.ToString(ddr["mcinPcin"]),
                                                    messageType = Convert.ToString(ddr["messageType"]),
                                                    packageType = Convert.ToString(ddr["packageType"]),
                                                    shipmentLoadStatus = Convert.ToString(ddr["shipmentLoadStatus"]),
                                                });
                                                a++;

                                            }

                                        }

                                    }
                                }




                            }



                            lstCIMASRCargoContainer.equipmentID = Convert.ToString(dr["equipmentID"]);
                            lstCIMASRCargoContainer.equipmentLoadStatus = Convert.ToString(dr["equipmentLoadStatus"]);
                            lstCIMASRCargoContainer.equipmentSealNumber = Convert.ToString(dr["equipmentSealNumber"]);
                            lstCIMASRCargoContainer.equipmentSealType = Convert.ToString(dr["equipmentSealType"]);
                            lstCIMASRCargoContainer.equipmentSequenceNo = Convert.ToInt32(L);
                            lstCIMASRCargoContainer.equipmentSize = Convert.ToString(dr["equipmentSize"]);
                            lstCIMASRCargoContainer.equipmentType = Convert.ToString(dr["equipmentType"]);
                            lstCIMASRCargoContainer.finalDestinationLocation = Convert.ToString(dr["finalDestinationLocation"]);
                            lstCIMASRCargoContainer.messageType = Convert.ToString(dr["messageType"]);
                            lstCIMASRCargoContainer.cargoDocument = lstcargoItnry;



                            L++;

                            obj.master.cargoContainer.Add(lstCIMASRCargoContainer);
                        }

                    }
                }









                #endregion

                string json = JsonConvert.SerializeObject(obj);


                return json;
            }
            catch (Exception ex)
            {
                log.Error("Error Message CIMASR:" + ex.Message);
                return "";

            }


        }
        }
}
