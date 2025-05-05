using Newtonsoft.Json;
using SCMTRLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMTRLibrary
{

    public class ARJsonFormat
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string  ARJsonCreation(DataSet ds, int Exitdtlid)
        {   
            try
            {
                int k = 0;
                int j = 1;
                ARModel obj = new ARModel();
                try
                {
                    #region Data Binding 
                    // Header Databinding

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (Convert.ToInt32(dr["Exitdtlid"]) == Exitdtlid)
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

                            if (Convert.ToInt32(dr["ExitIdDtls"]) == Exitdtlid)
                            {
                                obj.master.declaration.messageType = Convert.ToString(ds.Tables[1].Rows[0]["messageType"]);
                                obj.master.declaration.portOfReporting = Convert.ToString(ds.Tables[1].Rows[0]["portOfReporting"]);
                                obj.master.declaration.jobNo = Convert.ToInt32(ds.Tables[1].Rows[0]["jobNo"]);
                                obj.master.declaration.jobDate = Convert.ToString(ds.Tables[1].Rows[0]["jobDate"]);
                                obj.master.declaration.reportingEvent = Convert.ToString(ds.Tables[1].Rows[0]["reportingEvent"]);
                            }
                        }

                    }

                    //location Databinding
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        obj.master.location.reportingPartyType = Convert.ToString(ds.Tables[2].Rows[0]["reportingPartyType"]);
                        obj.master.location.reportingPartyCode = Convert.ToString(ds.Tables[2].Rows[0]["reportingPartyCode"]);
                        obj.master.location.nextDestinationOfUnlading = Convert.ToString(ds.Tables[2].Rows[0]["nextDestinationOfUnlading"]);
                        obj.master.location.reportingLocationCode = Convert.ToString(ds.Tables[2].Rows[0]["reportingLocationCode"]);                        
                        obj.master.location.authorisedPersonPAN = Convert.ToString(ds.Tables[2].Rows[0]["authorisedPersonPAN"]);
                        obj.master.location.bondNumber = Convert.ToString(ds.Tables[2].Rows[0]["bondNumber"]);
                    }

                    // transportMeans
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[3].Rows)
                        {
                            if (Convert.ToInt32(dr["ExitIdDtls"]) == Exitdtlid)
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



                    }



                    //cargoItnry Databinding

                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        int L = 1;
                        foreach (DataRow dr in ds.Tables[5].Rows)
                        {
                            if (Convert.ToInt32(dr["ExitIdDtls"]) == Exitdtlid)
                            {


                                // cargoContainer DataBinding
                                ARcargoContainer lstCIMARCargoContainer = new ARcargoContainer();
                                List<ARcargoDocument> lstcargoItnry = new List<ARcargoDocument>();
                                if (ds.Tables[6].Rows.Count > 0)
                                {


                                    if (ds.Tables[6].Rows.Count > 0)
                                    {
                                        int a = 1;
                                        foreach (DataRow ddr in ds.Tables[6].Rows)
                                        {


                                            if (Convert.ToString(ddr["ContainerNo"]) == Convert.ToString(dr["equipmentID"]))
                                            {

                                                if (Convert.ToInt32(ddr["ExitIdDtls"]) == Exitdtlid)
                                                {
                                                    lstcargoItnry.Add(new ARcargoDocument
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



                                lstCIMARCargoContainer.equipmentID = Convert.ToString(dr["equipmentID"]);
                                lstCIMARCargoContainer.equipmentLoadStatus = Convert.ToString(dr["equipmentLoadStatus"]);
                                lstCIMARCargoContainer.equipmentSealNumber = Convert.ToString(dr["equipmentSealNumber"]);
                                lstCIMARCargoContainer.equipmentSealType = Convert.ToString(dr["equipmentSealType"]);
                                lstCIMARCargoContainer.equipmentSequenceNo = Convert.ToInt32(L);
                                lstCIMARCargoContainer.equipmentSize = Convert.ToString(dr["equipmentSize"]);
                                lstCIMARCargoContainer.equipmentType = Convert.ToString(dr["equipmentType"]);
                                lstCIMARCargoContainer.finalDestinationLocation = Convert.ToString(dr["finalDestinationLocation"]);
                                lstCIMARCargoContainer.messageType = Convert.ToString(dr["messageType"]);
                                lstCIMARCargoContainer.cargoDocument = lstcargoItnry;



                                L++;

                                obj.master.cargoContainer.Add(lstCIMARCargoContainer);
                            }

                        }
                    }







                    #endregion

                    string json = JsonConvert.SerializeObject(obj);


                    return json;
                }
                catch (Exception ex)
                {
                    log.Error("Error Message CIMAR:" + ex.Message);
                    return "";

                }
            }
            catch(Exception ex)
            {
                return "";
            }


           

        }
    }
}