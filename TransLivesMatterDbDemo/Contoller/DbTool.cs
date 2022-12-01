using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Data.Common;

namespace TransLivesMatterDbDemo.Contoller
{
    class DbTool
    {
        private readonly string _dbServer;
        private string _dbName;

        private string _dbUser;

        private string _dbPass;

        public DbTool(string dbName, string dbUser, string dbPass, string dbServer = @".\SQLExpress")
        {
            _dbServer = dbServer;
            _dbName = dbName;
            _dbUser = dbUser;
            _dbPass = dbPass;
        }
        public DbTool()
        {

            _dbName = "DevData";
            _dbUser = "rainbow";
            _dbPass = "everywhere";
        }

        public string TestConnection(out bool hasError, string dbName = "", string dbUser="", string dbPass = "")
        {
            hasError = false;
            if (dbName != string.Empty)
            {
                _dbName = dbName;
            }
            if (dbUser != string.Empty)
            {
                _dbUser = dbUser;
            }
            if (dbPass != string.Empty)
            {
                _dbPass = dbPass;
            }

            string log = "";
            string connectionString = @"Data Source=" + _dbServer + @";Initial Catalog=" + _dbName + @";User ID="+ _dbUser+@";Password="+ _dbPass + @"";
            using (SqlConnection connection = new(connectionString))
            {
                try
                {
                    connection.Open();
                    log += "Connection open!" + Environment.NewLine;
                }
                catch (SqlException ex)
                {
                    log += (ex.ToString());
                    hasError = true;
                }
                catch (Exception ex)
                {
                    log += (ex.ToString());
                    hasError = true;
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            return log;
        }

        public DataTable GetTableColumns(out bool hasError, ref string statusLog)
        {
            hasError = false;
            DataTable dataTable = new DataTable();

            string connectionString = @"Data Source=" + _dbServer + @";Initial Catalog=" + _dbName + @";User ID=" + _dbUser + @";Password=" + _dbPass + @"";
            using (SqlConnection connection = new(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = @"select TABLE_CATALOG, TABLE_NAME, COLUMN_NAME, DATA_TYPE
from INFORMATION_SCHEMA.COLUMNS
where TABLE_CATALOG = '"+_dbName+"' and DATA_TYPE = 'int'";
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable = new();
                            dataTable.Load(reader);
                            statusLog += "Columns: " + dataTable.Columns.Count + Environment.NewLine + "Rows: " + dataTable.Rows.Count + Environment.NewLine;
                        }
                        else
                        {
                            statusLog += "Columns: " + 0 + Environment.NewLine + "Rows: " + 0 + Environment.NewLine;
                        }
                    }

                }
                catch (Exception ex)
                {
                    hasError = true;
                    statusLog += ex.ToString();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

                return dataTable;

        }
    }

}
