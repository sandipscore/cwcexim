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
   public class DTJsonFormat
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static string DTJsonCreation(DataSet ds,int Exitdtlid)
        {

            int k = 0;
            int j = 1;
            DTModel obj = new DTModel();
            try
            {
                #region Data Binding 
                // Header Databinding

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow dr in ds.Tables[0].Rows)
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
                    foreach(DataRow dr in ds.Tables[1].Rows)
                    {

                        if (Convert.ToInt32(dr["Exitdtlid"]) == Exitdtlid)
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
                    obj.master.location.nextDestinationOfUnlading = Convert.ToString(ds.Tables[2].Rows[0]["nextDestinationOfUnlading"]);
                }

                // transportMeans
                if (ds.Tables[3].Rows.Count > 0)
                {
                    foreach(DataRow dr in ds.Tables[3].Rows)
                    {
                        if(Convert.ToInt32(dr["ExitIdDtls"])== Exitdtlid)
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
                    obj.master.events.actualTimeOfDeparture = Convert.ToString(ds.Tables[4].Rows[0]["actualTimeOfDeparture"]);
                 


                }



                //cargoItnry Databinding



                if (ds.Tables[5].Rows.Count > 0)
                {
                    foreach(DataRow dr in ds.Tables[5].Rows)
                    {
                        if(Convert.ToInt32(dr["ExitIdDtls"])== Exitdtlid)
                        {
                            obj.master.CIM.CIMDate = Convert.ToString(dr["CIMDate"]); 
                            obj.master.CIM.CIMNumber = Convert.ToInt32(dr["CIMNo"]);
                        }
                      
                    }
                  

                     

                }

                if (ds.Tables[6].Rows.Count > 0)
                {
                    DTcargoContainer idata = new DTcargoContainer();
                    int a = 1;
                    foreach (DataRow dr in ds.Tables[6].Rows)
                    { 
                        if(Convert.ToInt32(dr["ExitIdDtls"])== Exitdtlid)
                        {
                            idata.equipmentID = Convert.ToString(dr["equipmentID"]);
                            idata.equipmentSequenceNo = Convert.ToInt32(a);
                            idata.equipmentSize = Convert.ToString(dr["equipmentSize"]);
                            idata.equipmentType = Convert.ToString(dr["equipmentType"]);
                            idata.messageType = Convert.ToString(dr["messageType"]);
                            a++;
                        }
                     
                        



                    }
                    obj.master.cargoContainer.Add(idata);


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
