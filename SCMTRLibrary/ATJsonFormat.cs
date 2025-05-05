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
  public  class ATJsonFormat
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string ATJsonCreation(DataSet ds)
        {
            try
            {
                int k = 0;
                int j = 1;
                ATModel obj = new ATModel();
                try
                {
                    #region Data Binding 
                    // Header Databinding

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
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
                    // declaration Databinding
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {

                            
                                obj.master.declaration.messageType = Convert.ToString(ds.Tables[1].Rows[0]["messageType"]);
                                obj.master.declaration.portOfReporting = Convert.ToString(ds.Tables[1].Rows[0]["portOfReporting"]);
                                obj.master.declaration.jobNo = Convert.ToInt32(ds.Tables[1].Rows[0]["jobNo"]);
                                obj.master.declaration.jobDate = Convert.ToString(ds.Tables[1].Rows[0]["jobDate"]);
                                obj.master.declaration.reportingEvent = Convert.ToString(ds.Tables[1].Rows[0]["reportingEvent"]);
                           
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
                     
                    }

                    

                 



                    //cargoItnry Databinding

                    if (ds.Tables[3].Rows.Count > 0)
                    {
                       
                        
                           


                                // cargoContainer DataBinding
                              
                              

                              obj.master.cargoContainer.equipmentID = Convert.ToString(ds.Tables[3].Rows[0]["equipmentID"]);                              
                              obj.master.cargoContainer.equipmentSequenceNo = 1;
                              obj.master.cargoContainer.equipmentSize = Convert.ToString(ds.Tables[3].Rows[0]["equipmentSize"]);
                              obj.master.cargoContainer.equipmentType = Convert.ToString(ds.Tables[3].Rows[0]["equipmentType"]);
                              obj.master.cargoContainer.messageType = Convert.ToString(ds.Tables[3].Rows[0]["messageType"]);
                               



                                
                          

                        
                    }

                   

                    // transportMeans
                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[4].Rows)
                        {

                            obj.master.transportMeans.totalEquipments = Convert.ToInt32(dr["totalEquipments"]);
                            obj.master.transportMeans.transportMeansNumber = Convert.ToString(dr["transportMeansNumber"]);
                            obj.master.transportMeans.transportMeansType = Convert.ToString(dr["transportMeansType"]);


                        }


                    }

                    //events
                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        obj.master.events.actualTimeOfArrival = Convert.ToString(ds.Tables[5].Rows[0]["expectedTimeOfArrival"]);



                    }
                    if(ds.Tables[6].Rows.Count>0)
                    {
                        obj.master.CIM.CIMNumber = Convert.ToInt32(ds.Tables[6].Rows[0]["CIMNo"]);
                        obj.master.CIM.CIMDate = Convert.ToString(ds.Tables[6].Rows[0]["CIMDate"]);

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
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
