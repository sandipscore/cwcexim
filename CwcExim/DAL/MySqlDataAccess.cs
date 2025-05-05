//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
using System.Data;
using MySql.Data.MySqlClient;
namespace CwcExim.DAL
{
    public class MySqlDataAccess: DataAccessLayerBaseClass
    {

        // Provide class constructors
        public MySqlDataAccess() { }
        public MySqlDataAccess(string connectionString) { this.ConnectionString = connectionString; }

        // DataAccessLayerBaseClass Members
        internal override IDbConnection GetDataProviderConnection()
        {
            return new MySqlConnection();
            //return new SqlConnection();
        }
        internal override IDbCommand GeDataProviderCommand()
        {
            return new MySqlCommand();
            //return new SqlCommand();
        }

        internal override IDbDataAdapter GetDataProviderDataAdapter()
        {
            return new MySqlDataAdapter();
           // return new SqlDataAdapter();
        }


    }
}