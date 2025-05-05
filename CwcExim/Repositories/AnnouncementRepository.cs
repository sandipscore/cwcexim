using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.DAL;
using MySql.Data.MySqlClient;
using CwcExim.Models;
using System.Data;

namespace CwcExim.Repositories
{
    public class AnnouncementRepository
    {
        private DatabaseResponse _DBResponse;
        public DatabaseResponse DBResponse
        {
            get
            {
                return _DBResponse;

            }
       }


        public void AddEditAnnouncement(Announcement ObjAnnounce)
        {
            int RetValue = 0;
            string GeneratedClientId = "0";
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter {ParameterName = "in_AnnounceId", MySqlDbType = MySqlDbType.Int32, Size = 11, Value = ObjAnnounce.AnnounceId});
            LstParam.Add(new MySqlParameter { ParameterName = "in_Title", MySqlDbType = MySqlDbType.VarChar, Value = ObjAnnounce.Title});
            LstParam.Add(new MySqlParameter { ParameterName = "in_Description", MySqlDbType = MySqlDbType.LongText, Value = ObjAnnounce.Description});
            LstParam.Add(new MySqlParameter { ParameterName = "in_StartDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjAnnounce.StartDate) });
            LstParam.Add(new MySqlParameter { ParameterName = "in_EndDate", MySqlDbType = MySqlDbType.DateTime, Value = Convert.ToDateTime(ObjAnnounce.EndDate)});
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsActive", MySqlDbType = MySqlDbType.Bit, Value = ObjAnnounce.IsActive });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsPublished", MySqlDbType = MySqlDbType.Bit, Value = ObjAnnounce.IsPublished });
            LstParam.Add(new MySqlParameter { ParameterName = "in_Uid", MySqlDbType = MySqlDbType.Int32, Value = ObjAnnounce.Uid });
            LstParam.Add(new MySqlParameter { ParameterName = "RetValue", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = RetValue});
            LstParam.Add(new MySqlParameter { ParameterName = "GeneratedClientId", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output, Value = GeneratedClientId });
            IDataParameter[] DParam = { };
            DParam=LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            int Result = DataAccess.ExecuteNonQuery("AddEditAnnouncement", CommandType.StoredProcedure, DParam,out GeneratedClientId);
            _DBResponse = new DatabaseResponse();
            try
            {
                if(Result==1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Announcement Details Saved Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else if(Result==2)
                {
                    _DBResponse.Status = 2;
                    _DBResponse.Message = "Announcement Details Submitted Successfully";
                    _DBResponse.Data = GeneratedClientId;
                }
                else
                {
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Error";
                    _DBResponse.Data = null;
                }

            }
            catch(Exception ex)
            {
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
                _DBResponse.Data = null;
            }
        }
        public void SearchAnnouncement(string Title,bool IsActive,bool IsPortal)
        {
            List<MySqlParameter> LstParam = new List<MySqlParameter>();
            LstParam.Add(new MySqlParameter { ParameterName = "in_Title", MySqlDbType = MySqlDbType.VarChar,  Value = Title,Size=500 });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsActive", MySqlDbType = MySqlDbType.Bit, Value = IsActive });
            LstParam.Add(new MySqlParameter { ParameterName = "in_IsPortal", MySqlDbType = MySqlDbType.Bit, Value = IsPortal });
            IDataParameter[] DParam = LstParam.ToArray();
            DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader Result = DataAccess.ExecuteDataReader("SearchAnnounce", CommandType.StoredProcedure, DParam);
            _DBResponse = new DatabaseResponse();
            List<Announcement> lstAnn = new List<Announcement>();
            int Status = 0;
            try
            {
                while(Result.Read())
                {
                    Status = 1;
                    lstAnn.Add(new Announcement {
                        AnnounceId=Convert.ToInt32(Result["AnnounceId"]),
                        Title=Result["Title"].ToString(),
                        StartDate=Result["StartDate"].ToString(),
                        EndDate=Result["EndDate"].ToString(),
                        IsPublished=Convert.ToBoolean(Result["IsPublished"]==DBNull.Value?false:Result["IsPublished"]),
                        IsActive=Convert.ToBoolean(Result["IsActive"]==DBNull.Value?false:Result["IsActive"]),
                        PublishedDate=(Result["PublishedDate"]==DBNull.Value?"":Result["PublishedDate"]).ToString()
                    });
                }
                if (Status == 1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = lstAnn;
                }
                else 
                {
                    _DBResponse.Status =0 ;
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
                Result.Dispose();
                Result.Close();
            }
        }
        public void GetAnnouncementDet(int AnnounceId)
        {
            List<MySqlParameter> lstParam = new List<MySqlParameter>();
            lstParam.Add(new MySqlParameter { ParameterName = "in_AnnounceId", MySqlDbType = MySqlDbType.Int32, Value = AnnounceId });
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataParameter[] Dparam = lstParam.ToArray();
            IDataReader result = DA.ExecuteDataReader("GetAllAnnounce", CommandType.StoredProcedure,Dparam);
            _DBResponse = new DAL.DatabaseResponse();
            int Status = 0;
            Announcement objAnnc = new Announcement();
            try
            {
                while(result.Read())
                {
                    Status = 1;
                    objAnnc.AnnounceId = Convert.ToInt32(result["AnnounceId"]);
                    objAnnc.Title = result["Title"].ToString();
                    objAnnc.Description = result["Description"].ToString();
                    objAnnc.StartDate = result["StartDate"].ToString();
                    objAnnc.EndDate = result["EndDate"].ToString();
                    objAnnc.IsPublished = Convert.ToBoolean(result["IsPublished"] == DBNull.Value ? false : result["IsPublished"]);
                    objAnnc.IsActive = Convert.ToBoolean(result["IsActive"] == DBNull.Value ? false : result["IsActive"]);
                    objAnnc.PublishedDate = (result["PublishedDate"] == DBNull.Value ? "" : result["PublishedDate"]).ToString();
                }
                if(Status==1)
                {
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                    _DBResponse.Data = objAnnc;
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
                result.Dispose();
                result.Close();
            }
        }
        public void ActiveAnnouncement()
        {
            int Status = 0;
            _DBResponse = new DatabaseResponse();
            DataAccessLayerBaseClass DA = DataAccessLayerFactory.GetDataAccessLayer();
            IDataReader result = DA.ExecuteDataReader("ActiveAnnounce", CommandType.StoredProcedure, null);
            List<Announcement> objAnncm = new List<Announcement>();
            try
            {
                while (result.Read())
                {
                    Status = 1;
                    objAnncm.Add(new Announcement
                    {
                        Title = result["Title"].ToString(),
                        Description = result["Description"].ToString(),
                        StartDate = result["StartDate"].ToString(),
                        EndDate = result["EndDate"].ToString(),
                        IsPublished = Convert.ToBoolean(result["IsPublished"]),
                        IsActive = Convert.ToBoolean(result["IsActive"]),
                        PublishedDate = result["PublishedDate"].ToString()
                    });
                }
                if(Status==1)
                {
                    _DBResponse.Data = objAnncm;
                    _DBResponse.Status = 1;
                    _DBResponse.Message = "Success";
                }
                else
                {
                    _DBResponse.Data = null;
                    _DBResponse.Status = 0;
                    _DBResponse.Message = "Success";
                }
            }
            catch(Exception ex)
            {
                _DBResponse.Data = null;
                _DBResponse.Status = 0;
                _DBResponse.Message = "Error";
            }
            finally
            {
                   result.Dispose();
                    result.Close();
            }
        }

    }
}