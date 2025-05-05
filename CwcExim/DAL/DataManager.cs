using System;
using System.Data;
using CwcExim.Models;
using CwcExim.UtilityClasses;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Web;
using System.Collections.Generic;
using System.IO;

namespace CwcExim.DAL
{
    public class DataManager
    {
        /// <summary>
        /// Defines the DataAccessLayer implemented data provider types.
        /// </summary>
        

        /// <summary>
        /// The DataAccessLayerBaseClass lists all the abstract methods that each data access layer provider (SQL Server, OleDb, etc.) must implement.
        /// </summary>
       

    }
    public enum DataProviderType
    {
        Access,
        Odbc,
        OleDb,
        Oracle,
        Sql,
        MySql
    }
    public abstract class DataAccessLayerBaseClass
    {

        #region private data members, methods & constructors

        // Private Members

        private string strConnectionString;
        private IDbConnection connection;
        private IDbCommand command;
        private IDbTransaction transaction;

        // Properties

        /// <summary>
        /// Gets or sets the string used to open a database.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                // make sure conection string is not empty
                if (strConnectionString == string.Empty
                    || strConnectionString.Length == 0)
                    throw new ArgumentException("Invalid database connection string.");

                return strConnectionString;
            }
            set
            {
                strConnectionString = value;
            }
        }


        // Since this is an abstract class, for better documentation and readability of source code, 
        // class is defined with an explicit protected constructor
        protected DataAccessLayerBaseClass() { }


        /// <summary>
        /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        /// to the provided command.
        /// </summary>
        private void PrepareCommand(CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            // provide the specific data provider connection object, if the connection object is null
            if (connection == null)
            {
                connection = GetDataProviderConnection();
                connection.ConnectionString = this.ConnectionString;
            }

            // if the provided connection is not open, then open it
            if (connection.State != ConnectionState.Open)
                connection.Open();

            // Provide the specific data provider command object, if the command object is null
            if (command == null)
                command = GeDataProviderCommand();

            // associate the connection with the command
            command.Connection = connection;
            // set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;
            // set the command type
            command.CommandType = commandType;
            command.CommandTimeout = 600;
            // if a transaction is provided, then assign it.
            //if (transaction != null)
            //    command.Transaction = transaction;

            // attach the command parameters if they are provided
            if (commandParameters != null)
            {
                foreach (IDataParameter param in commandParameters)
                    command.Parameters.Add(param);
            }
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Data provider specific implementation for accessing relational databases.
        /// </summary>
        internal abstract IDbConnection GetDataProviderConnection();
        /// <summary>
        /// Data provider specific implementation for executing SQL statement while connected to a data source.
        /// </summary>
        internal abstract IDbCommand GeDataProviderCommand();
        /// <summary>
        /// Data provider specific implementation for filling the DataSet.
        /// </summary>
        internal abstract IDbDataAdapter GetDataProviderDataAdapter();

        #endregion

        // Generic methods implementation

        #region Database Transaction

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        public void BeginTransaction()
        {
            if (transaction != null)
                return;

            try
            {
                // instantiate a connection object
                connection = GetDataProviderConnection();
                connection.ConnectionString = this.ConnectionString;
                // open connection
                connection.Open();
                // begin a database transaction with a read committed isolation level
                transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            catch
            {
                connection.Close();

                throw;
            }
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public void CommitTransaction()
        {
            if (transaction == null)
                return;

            try
            {
                // Commit transaction
                transaction.Commit();
            }
            catch
            {
                // rollback transaction
                RollbackTransaction();
                throw;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        public void RollbackTransaction()
        {
            if (transaction == null)
                return;

            try
            {
                transaction.Rollback();
            }
            catch { }
            finally
            {
                connection.Close();
                transaction = null;
            }
        }

        #endregion

        #region ExecuteDataReader

        /// <summary>
        /// Executes the CommandText against the Connection and builds an IDataReader.
        /// </summary>
        public IDataReader ExecuteDataReader(string commandText)
        {
            return this.ExecuteDataReader(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Executes the CommandText against the Connection and builds an IDataReader.
        /// </summary>
        public IDataReader ExecuteDataReader(string commandText, CommandType commandType)
        {
            return this.ExecuteDataReader(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a parameterized CommandText against the Connection and builds an IDataReader.
        /// </summary>
        public IDataReader ExecuteDataReader(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteDataReader(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Executes a stored procedure against the Connection and builds an IDataReader.
        /// </summary>
        public IDataReader ExecuteDataReader(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            IDataReader dr;
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);

               

                //if (transaction == null)
                //    // Generate the reader. CommandBehavior.CloseConnection causes the
                //    // the connection to be closed when the reader object is closed
                //    dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                //else
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);

                return dr;
            }
            catch( Exception ex)
            {

                var file = (dynamic)null;


                //var exp = ex.Message;
                //while (exp.InnerException != null) exp = exp.InnerException;

                //var msg = exp.GetBaseException().Message;
                //var stack = exp.StackTrace;

                string Error = ex.Message.ToString() + Environment.NewLine + ex.StackTrace;
                string CuurDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("/", "");
                string time = DateTime.Now.ToString("H:mm").Replace(":", "");

                string FolderPath2 = HttpContext.Current.Server.MapPath("~/GateError/ExitError/" + CuurDate);
                if (!System.IO.Directory.Exists(FolderPath2))
                {
                    System.IO.Directory.CreateDirectory(FolderPath2);

                }
                file = Path.Combine(FolderPath2, time + "_ErrorGate.txt");


                using (var tw = new StreamWriter(file, true))
                {
                    tw.WriteLine(Error);
                    tw.Close();
                }
                //if (transaction == null)
                //{
                connection.Close();
                command.Dispose();
                //}
                //else
                //    RollbackTransaction();

                //throw;
                    return null;
            }
        }
        public IDataReader ExecuteDataReader(string CommandText, CommandType CommandType, IDataParameter[] CommandParameters,out string ReturnObj)
        {
            IDataReader dr;
            try
            {
                PrepareCommand(CommandType, CommandText, CommandParameters);
                
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);

                ReturnObj = Convert.ToString(((MySqlCommand)command).Parameters["@ReturnObj"].Value);
                return dr;
            }
            catch (Exception ex)
            {
                //if (transaction == null)
                //{
                connection.Close();
                command.Dispose();
                //}
                //else
                //    RollbackTransaction();

                //throw;
                ReturnObj = "";
                return null;
            }
        }
        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        public DataSet ExecuteDataSet(string commandText)
        {
            return this.ExecuteDataSet(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            return this.ExecuteDataSet(commandText, commandType, null);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        public DataSet ExecuteDataSet(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteDataSet(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);
                //create the DataAdapter & DataSet
                IDbDataAdapter da = GetDataProviderDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();

                //fill the DataSet using default values for DataTable names, etc.
                da.Fill(ds);

                //return the dataset
                return ds;
            }
            catch(Exception ex)
            {
                //if (transaction == null)
                //    connection.Close();
                //else
                //    RollbackTransaction();

                //throw;
                return null;
            }
            finally {
                connection.Close();
                connection.Dispose();
            }
        }

        public object ExecuteDynamicSet<T>(string commandText, IDataParameter[] commandParameters = null)
        {
            try
            {
                var ds = this.ExecuteDataSet(commandText, CommandType.StoredProcedure, commandParameters);
                var i = 0;
                if (ds.Tables.Count > 1)
                {
                    foreach (var property in typeof(T).GetProperties())
                    {
                        try
                        {
                            if (ds.Tables.Count > i)
                            {
                                ds.Tables[i].TableName = property.Name;
                            }
                            i += 1;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                else
                {
                    var obj1 = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<T>>(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]));
                    return obj1;
                }
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(ds));
                
                return obj;
            }
            catch(Exception exp)
            {
                //if (transaction == null)
                //    connection.Close();
                //else
                //    RollbackTransaction();

                //throw;
                return null;
            }
            finally {
                connection.Close();
                connection.Dispose();
            }
        }

        #endregion

        #region ExecuteQuery

        /// <summary>
        /// Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        public int ExecuteQuery(string commandText)
        {
            return this.ExecuteQuery(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        public int ExecuteQuery(string commandText, CommandType commandType)
        {
            return this.ExecuteQuery(commandText, commandType, null);
        }

        internal int ExecuteScalar(string v, CommandType storedProcedure, IDataParameter[] dParam, out string status)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes an SQL parameterized statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        public int ExecuteQuery(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteQuery(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Executes a stored procedure against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        public int ExecuteQuery(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);

                // execute command
                int intAffectedRows = command.ExecuteNonQuery();
                // return no of affected records
                return intAffectedRows;
            }
            catch(Exception ex)
            {
                /*if (transaction != null)
                    RollbackTransaction();

                throw;*/
                

                return 0;
            }
            finally
            {
                //if (transaction == null)
                //{
                    connection.Close();
                    command.Dispose();
                //}
            }
        }
        public int ExecuteNonQuery(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            int Status=0;
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);

                // execute command
                int intAffectedRows = command.ExecuteNonQuery();

                

                Status = Convert.ToInt32(((MySqlCommand)command).Parameters["@RetValue"].Value);
                // return no of affected records
               // db.GetParameterValue(dbCommand, "@ProductID"),
                //if (intAffectedRows > 0)//success
                 //   Status = 1;


                return Status;
            }
            catch(Exception ex)
            {
                return Status;
            }
            finally
            {
                
                    connection.Close();
                    command.Dispose();
               
            }
        }
        public int ExecuteNonQuery(string commandText, CommandType commandType, IDataParameter[] commandParameters,out string OutParam)
        {
            int Status = 0;
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);

                // execute command
                int intAffectedRows = command.ExecuteNonQuery();



                Status = Convert.ToInt32(((MySqlCommand)command).Parameters["@RetValue"].Value);
                //Status= commandParameters[commandParameters.GetValue()]
                OutParam = Convert.ToString(((MySqlCommand)command).Parameters["@GeneratedClientId"].Value);
                //Convert.ToString(command.Parameters["@GeneratedClientId"].Value);


                return Status;
            }
            catch(Exception ex)
            {
                OutParam = "";
                var file = (dynamic)null;


                //var exp = ex.Message;
                //while (exp.InnerException != null) exp = exp.InnerException;

                //var msg = exp.GetBaseException().Message;
                //var stack = exp.StackTrace;
               
                string Error = ex.Message.ToString()+Environment.NewLine + ex.StackTrace;
                string CuurDate = DateTime.Now.ToString("MM/dd/yyyy").Replace("/", "");
                string time = DateTime.Now.ToString("H:mm").Replace(":", "");

                string FolderPath2 = HttpContext.Current.Server.MapPath("~/GateError/ExitError/" + CuurDate);
                if (!System.IO.Directory.Exists(FolderPath2))
                {
                    System.IO.Directory.CreateDirectory(FolderPath2);

                }
                file = Path.Combine(FolderPath2, time + "_ErrorGate.txt");


                using (var tw = new StreamWriter(file, true))
                {
                    tw.WriteLine(Error);
                    tw.Close();
                }

                return Status;
            }
            finally
            {

                connection.Close();
                command.Dispose();

            }
        }
        public int ExecuteNonQuery(string commandText, CommandType commandType, IDataParameter[] commandParameters, out string OutParam,out string ReturnObj)
        {
            int Status = 0;
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);
                int intAffectedRows = command.ExecuteNonQuery();
                Status = Convert.ToInt32(((MySqlCommand)command).Parameters["@RetValue"].Value);
                OutParam = Convert.ToString(((MySqlCommand)command).Parameters["@GeneratedClientId"].Value);
                ReturnObj = Convert.ToString(((MySqlCommand)command).Parameters["@ReturnObj"].Value);
                return Status;
            }
            catch (Exception ex)
            {
                OutParam = "";
                  ReturnObj = "";
                return Status;
            }
            finally
            {

                connection.Close();
                command.Dispose();

            }
        }
        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        public object ExecuteScalar(string commandText)
        {
            return this.ExecuteScalar(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            return this.ExecuteScalar(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a parameterized query, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        public object ExecuteScalar(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteScalar(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Executes a stored procedure, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        public object ExecuteScalar(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);

                // execute command
                object objValue = command.ExecuteScalar();
                // check on value
                if (objValue != DBNull.Value)
                    // return value
                    return objValue;
                else
                    // return null instead of dbnull value
                    return null;
            }
            catch
            {
                //if (transaction != null)
                //    RollbackTransaction();

                //throw;
                return null;
            }
            finally
            {
                //if (transaction == null)
                //{
                    connection.Close();
                    command.Dispose();
                //}
            }
        }

        #endregion

    }


    /// <summary>
    /// Loads different data access layer provider depending on the configuration settings file or the caller defined data provider type.
    /// </summary>
    public sealed class DataAccessLayerFactory
    {

        // Since this class provides only static methods, make the default constructor private to prevent 
        // instances from being created with "new DataAccessLayerFactory()"
        private DataAccessLayerFactory() { }

        /// <summary>
        /// Constructs a data access layer data provider based on application configuration settings.
        /// Application configuration file must contain two keys: 
        ///		1. "DataProviderType" key, with one of the DataProviderType enumerator.
        ///		2. "ConnectionString" key, holds the database connection string.
        /// </summary>
        public static DataAccessLayerBaseClass GetDataAccessLayer()
        {
            // Make sure application configuration file contains required configuration keys
            //if (System.Configuration.ConfigurationSettings.AppSettings["DataProviderType"] == null
            //    || System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"] == null)
            //    throw new ArgumentNullException("Please specify a 'DataProviderType' and 'ConnectionString' configuration keys in the application configuration file.");

            DataProviderType dataProvider;

            try
            {
                // try to parse the data provider type from configuration file
                dataProvider =
                    (DataProviderType)System.Enum.Parse(typeof(DataProviderType),
                    System.Configuration.ConfigurationSettings.AppSettings["DataProviderType"].ToString(),
                    true);
            }
            catch
            {
                throw new ArgumentException("Invalid data access layer provider type.");
            }

            // return data access layer provider
            //return GetDataAccessLayer(
            //    dataProvider,
            //    System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
            return GetDataAccessLayer(
               dataProvider,
              HttpContext.Current.Session["ConnectionString"].ToString());
        }

        //public static DataAccessLayerBaseClass GetDataAccessLayer(string Options)
        //{
        //    // Make sure application configuration file contains required configuration keys
        //    //if (System.Configuration.ConfigurationSettings.AppSettings["DataProviderType"] == null
        //    //    || System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"] == null)
        //    //    throw new ArgumentNullException("Please specify a 'DataProviderType' and 'ConnectionString' configuration keys in the application configuration file.");

        //    DataProviderType dataProvider;

        //    try
        //    {
        //        // try to parse the data provider type from configuration file
        //        dataProvider =
        //            (DataProviderType)System.Enum.Parse(typeof(DataProviderType),
        //            System.Configuration.ConfigurationSettings.AppSettings["DataProviderType"].ToString(),
        //            true);
        //    }
        //    catch
        //    {
        //        throw new ArgumentException("Invalid data access layer provider type.");
        //    }

        //    // return data access layer provider
        //    //return GetDataAccessLayer(
        //    //    dataProvider,
        //    //    System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
        //    return GetDataAccessLayer(
        //       dataProvider,
        //      HttpContext.Current.Session["DRConnectionString"].ToString());
        //}

        /// <summary>
        /// Constructs a data access layer based on caller specific data provider.
        /// Caller of this method must provide the database connection string through ConnectionString property.
        /// </summary>
        public static DataAccessLayerBaseClass GetDataAccessLayer(DataProviderType dataProviderType)
        {
            return GetDataAccessLayer(dataProviderType, null);
        }

        /// <summary>
        /// Constructs a data access layer data provider.
        /// </summary>
        public static DataAccessLayerBaseClass GetDataAccessLayer(DataProviderType dataProviderType, string connectionString)
        {
            // construct specific data access provider class
            switch (dataProviderType)
            {
                case DataProviderType.Access:
                case DataProviderType.OleDb:
                    return new OleDbDataAccessLayer(connectionString);

                case DataProviderType.Odbc:
                    return new OdbcDataAccessLayer(connectionString);

                //case DataProviderType.Oracle:
                //    return new OracleDataAccessLayer(connectionString);

                case DataProviderType.Sql:
                    return new SqlDataAccessLayer(connectionString);
                case DataProviderType.MySql:
                    return new MySqlDataAccess(connectionString);
                default:
                    throw new ArgumentException("Invalid data access layer provider type.");
            }
        }
    }

}
