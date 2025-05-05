using CwcExim.DAL;
using CwcExim.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace CwcExim.Repositories
{
    public class UserDashBoardRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

            }
        }
        public void GetDashboardMenuForUser(int RoleId, int PartyType)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();           
            lstParam.Add(new MySqlParameter { ParameterName = "in_RoleId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = RoleId });
            lstParam.Add(new MySqlParameter { ParameterName = "in_PartyType", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = PartyType });
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetDashboardMenuByRoleId", CommandType.StoredProcedure, DParam);
            EximDashboardMenu objEDM = new EximDashboardMenu();
            List<EximModule> lstModule = new List<EximModule>();
            List<EximMenu> lstMenu = new List<EximMenu>();
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstModule.Add(new EximModule
                    {
                        ModuleId = Convert.ToInt32(Result["ModuleId"]),
                        ModuleName = Convert.ToString(Result["ModuleName"])
                    });
                    objEDM.lstEModule = lstModule;
                }
                if (Result.NextResult())
                {
                    while (Result.Read())
                    {
                        lstMenu.Add(new EximMenu
                        {
                            ModuleId = Convert.ToInt32(Result["ModuleId"]),
                            MenuId = Convert.ToInt32(Result["MenuId"]),
                            MenuName = Convert.ToString(Result["MenuName"]),
                            ActionUrl = Convert.ToString(Result["ActionUrl"]),
                            ParentMenuId = Convert.ToInt32(Result["ParentMenuId"])
                        });
                        objEDM.lstEMenu = lstMenu;
                    }
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objEDM;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
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
                Result.Close();
                Result.Dispose();
            }
        }

        public void GetLastMonthVolume(int UId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_UId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = UId });           
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLastMonthVolume", CommandType.StoredProcedure, DParam);
            LastMonthVolume objLMV = new LastMonthVolume();
           
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    objLMV.LastMonth = Convert.ToString(Result["LastMonth"]);
                    objLMV.LastMonthTues = Convert.ToString(Result["LastMonthTues"]);
                    objLMV.LMonthVolume = Convert.ToString(Result["LMonthVolume"]);
                    objLMV.CurrentMonth = Convert.ToString(Result["CurrentMonth"]);
                    objLMV.CurrentMonthTues = Convert.ToString(Result["CurrentMonthTues"]);
                    objLMV.CMonthVolume = Convert.ToString(Result["CMonthVolume"]);
                }
               
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objLMV;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
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
                Result.Close();
                Result.Dispose();
            }
        }

        public void GetTopFiveContributor(int UId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_UId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = UId });
           
            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetTopFiveContributor", CommandType.StoredProcedure, DParam);
            
            List<TopFiveContributor> lstTC = new List<TopFiveContributor>();
           
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstTC.Add(new TopFiveContributor
                    {
                        Contributer = Convert.ToString(Result["Contributer"]),
                        ContAmount = Convert.ToString(Result["ContAmount"]),
                        MonthFor= Convert.ToString(Result["MonthFor"]),
                    });                    
                }
                
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstTC;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
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
                Result.Close();
                Result.Dispose();
            }
        }

        public void GetLastSixmonthCollection(int UId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_UId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = UId });

            IDataParameter[] DParam = { };
            DParam = lstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("GetLastSixmonthCollection", CommandType.StoredProcedure, DParam);

            List<LastSixdaysCollection> lstSC = new List<LastSixdaysCollection>();

            int Status = 0;
            _DBResponse = new DatabaseResponse();
            try
            {
                while (Result.Read())
                {
                    Status = 1;
                    lstSC.Add(new LastSixdaysCollection
                    {
                        ForMonth = Convert.ToString(Result["ForMonth"]),
                        CText = Convert.ToString(Result["CText"]),
                        CValue = Convert.ToString(Result["CValue"])
                    });
                }

                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstSC;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
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
                Result.Close();
                Result.Dispose();
            }
        }
    }
}