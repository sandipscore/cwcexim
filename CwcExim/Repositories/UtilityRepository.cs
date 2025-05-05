using CwcExim.Areas.CashManagement.Models;
using CwcExim.Areas.Import.Models;
using CwcExim.Areas.Report.Models;
using CwcExim.DAL;
using CwcExim.Models;
using CwcExim.UtilityClasses;
using EinvoiceLibrary;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Repositories
{
    public class UtilityRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;
            }
        }

        public void GetServerDate()
        {
            int Status = 0;
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
          

            IDataParameter[] DParam = { };
            DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetServerDate", CommandType.StoredProcedure);
            _DBResponse = new DatabaseResponse();
            string ServerDate = "";
            try
            {
                while (Result.Read())
                {
                    Status = 1;

                    ServerDate = Convert.ToString(Result["ServerDate"]);
                   

                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = ServerDate;
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
    }
}