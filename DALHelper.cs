using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OracleClient;

namespace DAL
{
    public enum ProviderName
    {
        OracleClient
    }
    public class DALHelper
    {
        private static string _connStrName = "ConnString";
        private static string _connStrSettings;
        private static System.Data.Common.DbProviderFactory _providerFactory;

        private DALHelper()
        {
            //if (_connStrSettings == null)
            //{
            //    _connStrSettings = ConfigurationManager.ConnectionStrings[_connStrName].ConnectionString;

            //}

        }
        private static string ReturnConnection()
        {
            _connStrSettings = ConfigurationManager.ConnectionStrings[_connStrName].ToString();
            return _connStrSettings;

        }
        private static bool IsOracleConnection(IDbConnection connection)
        {
            return connection is System.Data.OracleClient.OracleConnection;
        }
        public static IDbConnection GetConnection()
        {
            IDbConnection connection = null;

            try
            {
                if (_connStrSettings == null)
                {
                    ReturnConnection();
                }
                if (_providerFactory == null)
                {
                    CreateProviderFactory();
                }
                connection = _providerFactory.CreateConnection();
                connection.ConnectionString = ConfigurationManager.ConnectionStrings[_connStrName].ToString();

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                return connection;
            }
            catch
            {
                connection = null;
                throw;
            }
        }


        private static void CreateProviderFactory()
        {
            _providerFactory = System.Data.Common.DbProviderFactories.GetFactory("System.Data.OracleClient");
        }
        public static IDbTransaction GetTransaction(string ConnectionString, ProviderName ProviderName)
        {
            IDbTransaction trans = null;
            IDbConnection cn = null;
            try
            {
                cn = GetConnection();
                if ((cn != null) && (cn.State == ConnectionState.Open))
                {
                    trans = cn.BeginTransaction();
                }
                return trans;
            }
            catch
            {
                CloseDB(cn);
                throw;
            }
        }

        public static IDbTransaction GetTransaction()
        {
            IDbTransaction trans = null;
            IDbConnection cn = null;
            try
            {
                cn = GetConnection();
                if ((cn != null) && (cn.State == ConnectionState.Open))
                {
                    trans = cn.BeginTransaction();
                }
                return trans;
            }
            catch
            {
                CloseDB(cn);
                throw;
            }
        }


        public static void CloseDB(IDbConnection cn)
        {
            try
            {
                if (cn != null)
                {
                    cn.Close();
                    cn.Dispose();
                    cn = null;
                }
            }
            catch
            {
                throw;
            }
        }
        public static void CloseDB(IDbTransaction tran, bool DBStatus)
        {
            try
            {
                if (tran != null)
                {
                    IDbConnection TranCon = tran.Connection;
                    if (DBStatus)
                    {
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                    }
                    DALHelper.CloseDB(TranCon);
                    tran.Dispose();
                    tran = null;
                }
            }
            catch
            {
                throw;
            }
        }

        public static IDbDataParameter CreateParameter(string ParameterName, DbType ObjDBtype, ParameterDirection ParamDirection, Object value)
        {
            return CreateParameter(ParameterName, ObjDBtype, 0, ParamDirection, value);
        }
        public static IDbDataParameter CreateParameter(string ParameterName, DbType ObjDBtype, ParameterDirection ParamDirection)
        {
            return CreateParameter(ParameterName, ObjDBtype, 0, ParamDirection);
        }
        public static IDbDataParameter CreateParameter(string ParameterName, DbType ObjDBtype, int size, ParameterDirection ParamDirection)
        {
            return CreateParameter(ParameterName, ObjDBtype, size, ParamDirection, null);
        }
        public static IDbDataParameter CreateParameter(string ParameterName, DbType ObjDBtype, int size, ParameterDirection ParamDirection, Object value)
        {
            if (_connStrSettings == null)
            {
                GetConnection();
            }
            IDbDataParameter objParam = GetConnection().CreateCommand().CreateParameter();
            objParam.ParameterName = ParameterName;
            objParam.DbType = ObjDBtype;
            objParam.Size = size;
            if (Convert.ToString(value) != "")
            {
                objParam.Value = value;
            }
            else
            {
                objParam.Value = DBNull.Value;
            }

            objParam.Direction = ParamDirection;
            return objParam;
        }

        private static OracleParameter[] GetOracleParameters(params IDbDataParameter[] commandParameters)
        {
            if (commandParameters == null) return null;
            OracleParameter[] oracleParam = new OracleParameter[commandParameters.Length];
            OracleParameter param = null;
            int intCount = 0;
            foreach (IDbDataParameter ObjDbParameter in commandParameters)
            {
                param = (OracleParameter)ObjDbParameter;
                oracleParam[intCount] = param;
                intCount += 1;
            }

            return oracleParam;
        }

        public static string ConnectionString
        {
            get
            {
                if (_connStrSettings == null)
                {
                    string connection = ReturnConnection();
                }
                return ReturnConnection();
            }
        }

        #region ExecuteNonQuery
        /// <summary>
        /// Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in 
        /// the connection string Default Read from the Application Configuration
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            int returnVal = 0;

            // returnVal = OracleHelper.ExecuteNonQuery(ConnectionString, commandType, commandText);
            returnVal = ExecuteNonQuery(ConnectionString, ProviderName.OracleClient, commandType, commandText);

            return returnVal;
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in 
        /// the connection string Default Read from the Application Configuration
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            int returnVal = 0;

            returnVal = ExecuteNonQuery(connectionString, ProviderName.OracleClient, commandType, commandText);

            return returnVal;
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in 
        /// the connection string
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString,ProviderName.SqlClient, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="objProvider">Name of the Provider Sql, Oracle</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string connectionString, ProviderName objProvider, CommandType commandType, string commandText)
        {
            int returnVal = 0;

            returnVal = OracleHelper.ExecuteNonQuery(connectionString, commandType, commandText);

            return returnVal;
        }

        /// <summary>
        /// Execute a Command (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(CommandType.StoredProcedure, "PublishOrders", new IDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            int returnVal = 0;

            returnVal = ExecuteNonQuery(ConnectionString, ProviderName.OracleClient, commandType, commandText, commandParameters);

            return returnVal;
        }

        /// <summary>
        /// Execute a Command (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(CommandType.StoredProcedure, "PublishOrders", new IDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            int returnVal = 0;
            returnVal = ExecuteNonQuery(connectionString, ProviderName.OracleClient, commandType, commandText, commandParameters);

            return returnVal;
        }

        /// <summary>
        /// Execute a Command (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString,ProviderName.SqlClient, CommandType.StoredProcedure, "PublishOrders", new IDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection</param>
        /// <param name="objProvider">Name of the Provider Sql, Oracle</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string connectionString, ProviderName objProvider, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            int returnVal = 0;

            returnVal = OracleHelper.ExecuteNonQuery(connectionString, commandType, commandText, GetOracleParameters(commandParameters));


            return returnVal;
        }

        /// <summary>
        /// Execute a Command (that returns no resultset) against the specified IDbConnection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new IDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid IDbConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        public static int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText)
        {
            int returnVal = 0;

            returnVal = OracleHelper.ExecuteNonQuery((OracleConnection)connection, commandType, commandText);

            return returnVal;
        }

        /// <summary>
        /// Execute a Command (that returns no resultset) against the specified IDbConnection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new IDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid IDbConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            int returnVal = 0;

            returnVal = OracleHelper.ExecuteNonQuery((OracleConnection)connection, commandType, commandText, GetOracleParameters(commandParameters));

            return returnVal;
        }

        /// <summary>
        /// Execute a Command (that returns no resultset) against the specified IDbTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid IDbTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            int returnVal = 0;

            returnVal = OracleHelper.ExecuteNonQuery((OracleTransaction)transaction, commandType, commandText);

            return returnVal;
        }

        /// <summary>
        /// Execute a Command (that returns no resultset) against the specified IDbTransaction
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new IDbDataParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">A valid IDbTransaction</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of IDbDataParameter used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            int returnVal = 0;

            returnVal = OracleHelper.ExecuteNonQuery((OracleTransaction)transaction, commandType, commandText, GetOracleParameters(commandParameters));

            return returnVal;
        }
        #endregion


        //cn, CommandType.StoredProcedure, "SELECTALL_ACC_GROUP", dataSet, new string[] { "ACC_GROUP" }, spParameterSet

        public static void FillDataset(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params IDbDataParameter[] commandParameters)
        {

            FillDataset(ConnectionString, ProviderName.OracleClient, commandType, commandText, dataSet, tableNames, GetOracleParameters(commandParameters));

        }
        public static void FillDataset(string connectionString, ProviderName objProvider, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params IDbDataParameter[] commandParameters)
        {

            OracleHelper.FillDataset(connectionString, commandType, commandText, dataSet, tableNames, GetOracleParameters(commandParameters));

        }

        public static void FillDataset(IDbConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params IDbDataParameter[] commandParameters)
        {

            OracleHelper.FillDataset((OracleConnection)connection, commandType, commandText, dataSet, tableNames, GetOracleParameters(commandParameters));

        }
    }


    public class DALHelperParameterCache
    {
        private static string _connStrName = "ConnString";
        private static string _connStrSettings = string.Empty;

        #region  Properties
        public DALHelperParameterCache()
        {
        }
        /// <summary>
        /// Is SQL is checked the Provider name is "System.Data.SqlClient" if so 
        /// this will returns true else return false.
        /// </summary>
        /// <returns></returns>
        /// public static bool IsSQL()

        /// <summary>
        /// Is Oracle is checked the Provider name is "System.Data.OracleClient" if so 
        /// this will returns true else return false.
        /// </summary>
        /// <returns></returns>
        /// public static bool IsOracle()


        #endregion

        #region Parameter Discovery Functions

        /// <summary>
        /// This Function Will Check the Connection is Sql Connection Or Not
        /// </summary>
        /// <param name="connection">IDbConnection</param>
        /// <returns>bool</returns>
        private static bool IsSqlConnection(IDbConnection connection)
        {
            return connection is System.Data.SqlClient.SqlConnection;
        }
        /// <summary>
        /// This Function Will Check the Connection is Oracle Connection Or Not
        /// </summary>
        /// <param name="connection">IDbConnection </param>
        /// <returns>bool</returns>
        private static bool IsOracleConnection(IDbConnection connection)
        {
            return connection is System.Data.OracleClient.OracleConnection;
        }


        /// <summary>
        /// Retrieves the set of OracleParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a OracleConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <returns>An array of OracleParameters</returns>
        //public static OracleParameter[] GetSpParameterSet(string connectionString, string spName)
        //{
        //    return GetSpParameterSet(connectionString, spName, false);
        //}

        /// <summary>
        /// Retrieves the set of OracleParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a Connection. </param>
        /// <param name="provider">The name of the Provider using</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>An array of OracleParameters</returns>
        public static IDbDataParameter[] GetSpParameterSet(string connectionString, ProviderName provider, string spName)
        {
            IDbDataParameter[] Parameters = null;

            Parameters = GetDbDataParameter(OracleHelperParameterCache.GetSpParameterSet(connectionString, spName, true));

            return Parameters;
        }

        /// <summary>
        /// Retrieves the set of OracleParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>An array of OracleParameters</returns>
        public static IDbDataParameter[] GetSpParameterSet(string spName)
        {
            IDbDataParameter[] Parameters = null;

            Parameters = GetDbDataParameter(OracleHelperParameterCache.GetSpParameterSet(_connStrSettings.ToString(), spName, true));

            return Parameters;
        }

        /// <summary>
        /// Retrieves the set of OracleParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a OracleConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>An array of OracleParameters</returns>
        public static IDbDataParameter[] GetSpParameterSet(IDbConnection connection, string spName)
        {
            IDbDataParameter[] Parameters = null;

            Parameters = GetDbDataParameter(OracleHelperParameterCache.GetSpParameterSet((OracleConnection)connection, spName, true));

            return Parameters;
        }

        /// <summary>
        /// Retrieves the set of OracleParameters appropriate for the stored procedure
        /// </summary>
        /// <param name="objTrans">The obj trans.</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <returns>An array of OracleParameters</returns>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        public static IDbDataParameter[] GetSpParameterSet(IDbTransaction objTrans, string spName)
        {
            return GetSpParameterSet(objTrans.Connection, spName);
        }
        private static IDbDataParameter[] GetDbDataParameter(params SqlParameter[] commandParameters)
        {
            IDbDataParameter[] IDbParam = new IDbDataParameter[commandParameters.Length];
            Int32 intCount = 0;
            foreach (SqlParameter sqlParam in commandParameters)
            {
                sqlParam.ParameterName = sqlParam.ParameterName.Substring(1);
                IDbParam[intCount] = (IDbDataParameter)((ICloneable)sqlParam);
                intCount = intCount + 1;
            }
            return IDbParam;

        }
        private static IDbDataParameter[] GetDbDataParameter(params OracleParameter[] commandParameters)
        {
            IDbDataParameter[] IDbParam = new IDbDataParameter[commandParameters.Length];
            Int32 intCount = 0;
            foreach (OracleParameter OracleParam in commandParameters)
            {
                IDbParam[intCount] = (IDbDataParameter)((ICloneable)OracleParam);
                intCount = intCount + 1;
            }
            return IDbParam;

        }

        #endregion Parameter Discovery Functions

    }
}
