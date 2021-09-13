using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace PressKyLicense
{
    public class DBAccess : IDisposable
    {
        private SqlCommand cmd = new SqlCommand();
        private string strConnectionString = string.Empty;
        private SqlTransaction objTransaction;
        private SqlConnection cnn;

        public DBAccess()
        {
            try
            {
                
                ConnectionStringSettings objConnectionStringSettings = ConfigurationManager.ConnectionStrings["connString"];
                strConnectionString = objConnectionStringSettings.ConnectionString;
                cnn = new SqlConnection();
                cnn.ConnectionString = strConnectionString;
                cmd.Connection = cnn;
                cnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                this.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DBAccess(string connString)
        {
            strConnectionString = connString;
            cnn = new SqlConnection();
            cnn.ConnectionString = strConnectionString;
            cmd.Connection = cnn;
            cnn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            this.Open();
        }

        private void Open()
        {
            cmd.Connection = cnn;
        }

        private void Close()
        {
            cnn.Close();
            cnn = null;

            //SqlConnection.ClearAllPools();
        }

        public void Dispose()
        {
            cmd.Dispose();
            this.Close();
        }

        public SqlDataReader ExecuteReader()
        {
            SqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                this.ClearParametrs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reader;
        }

        public SqlDataReader ExecuteReader(string commandtext, bool bSPCommand = true)
        {
            SqlDataReader reader = null;
            try
            {
                if (bSPCommand == false)
                    cmd.CommandType = CommandType.Text;
                cmd.CommandText = commandtext;
                reader = this.ExecuteReader();
                this.ClearParametrs();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return reader;
        }

        public object ExecuteScalar()
        {
            object obj = null;
            try
            {
                obj = cmd.ExecuteScalar();
                this.ClearParametrs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }

        public object ExecuteScalar(string commandtext, bool bSPCommand = true)
        {
            object obj = null;
            try
            {
                if (bSPCommand == false)
                    cmd.CommandType = CommandType.Text;
                cmd.CommandText = commandtext;
                obj = this.ExecuteScalar();
                this.ClearParametrs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }

        public int ExecuteNonQuery()
        {
            int i = -1;
            try
            {
                i = cmd.ExecuteNonQuery();
                this.ClearParametrs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;
        }

        public int ExecuteNonQuery(string commandtext, bool bSPCommand = true)
        {
            int i = -1;
            try
            {
                if (bSPCommand == false)
                    cmd.CommandType = CommandType.Text;
                cmd.CommandText = commandtext;
                i = this.ExecuteNonQuery();
                this.ClearParametrs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;
        }

        public int ExecuteNonQuery(string commandtext, out object rowIdentifier)
        {
            int i = -1;
            rowIdentifier = null;
            try
            {
                cmd.CommandText = commandtext;
                if (cmd.CommandType == CommandType.StoredProcedure)
                {
                    SqlParameter parameter = new SqlParameter("RowIdentifier", SqlDbType.Int, 4000);
                    parameter.Direction = ParameterDirection.Output;
                    this.AddParameter(parameter);
                }

                i = cmd.ExecuteNonQuery();

                if (cmd.CommandType == CommandType.StoredProcedure)
                {
                    rowIdentifier = cmd.Parameters["RowIdentifier"].Value;
                }

                this.ClearParametrs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;
        }

        public DataTable ExecuteDataTable()
        {
            SqlDataAdapter da = null;
            DataTable dt = null;
            try
            {
                da = new SqlDataAdapter();
                da.SelectCommand = (SqlCommand)cmd;
                dt = new DataTable();
                da.Fill(dt);
                this.ClearParametrs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable ExecuteDataTable(string commandtext, bool bSPCommand = true)
        {
            DataTable dt = null;
            try
            {
                if (bSPCommand == false)
                    cmd.CommandType = CommandType.Text;
                cmd.CommandText = commandtext;
                dt = this.ExecuteDataTable();
                this.ClearParametrs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataSet ExecuteDataSet()
        {
            SqlDataAdapter da = null;
            DataSet ds = null;
            try
            {
                da = new SqlDataAdapter();
                da.SelectCommand = (SqlCommand)cmd;
                ds = new DataSet();
                da.Fill(ds);
                this.ClearParametrs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet ExecuteDataSet(string commandtext, bool bSPCommand = true)
        {
            DataSet ds = null;
            try
            {
                if (bSPCommand == false)
                    cmd.CommandType = CommandType.Text;
                cmd.CommandText = commandtext;
                ds = this.ExecuteDataSet();
                this.ClearParametrs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public bool BeginTransaction()
        {
            try
            {
                objTransaction = (SqlTransaction)cmd.Connection.BeginTransaction();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CommitTrans()
        {
            try
            {
                objTransaction.Commit();
                objTransaction = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void AddParameter(string paramname, object paramvalue)
        {
            SqlParameter param = new SqlParameter(paramname, paramvalue);
            cmd.Parameters.Add(param);
        }

        public void AddParameter(string paramname, SqlDbType dbType, ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter(paramname, dbType);
            param.Direction = direction;
            cmd.Parameters.Add(param);
        }

        public void AddParameter(IDataParameter param)
        {
            cmd.Parameters.Add(param);
        }

        public void ClearParametrs()
        {
            cmd.Parameters.Clear();
        }

        public void RemoveParameter(IDataParameter param)
        {
            cmd.Parameters.Remove(param);
        }

        public string ConnectionString
        {
            get
            {
                return strConnectionString;
            }
            set
            {
                strConnectionString = value;
            }
        }

        public string CommandText
        {
            get
            {
                return cmd.CommandText;
            }
            set
            {
                cmd.CommandText = value;
                cmd.Parameters.Clear();
            }
        }

        public IDataParameterCollection Parameters
        {
            get
            {
                return cmd.Parameters;
            }
        }

        public SqlConnection GetConnectionObject()
        {
            return cnn;
        }

        public DataTable ExecuteDataTable(System.Text.StringBuilder query, bool p)
        {
            throw new NotImplementedException();
        }

        public DataTable ExecuteDataTable(System.Text.StringBuilder query)
        {
            throw new NotImplementedException();
        }
    }
    
}
