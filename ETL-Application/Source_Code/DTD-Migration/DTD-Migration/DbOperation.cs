using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTD_Migration
{
    internal class DbOperation
    {
        public SqlConnection objConnection = null;
        public int NoOfDBConnAttempts = 1;
        public string ConnString = string.Empty;

        public DbOperation(int NoOfConnAttempts,string ConnectionString)
        {
            NoOfDBConnAttempts = NoOfConnAttempts;
            ConnString = ConnectionString;
        }

        /// <summary> Open database connection
        /// <param name="NoOfConnAttempts">[NoOfConnAttempts] Number of times to prform attemp to open DB connection</param>
        public bool OpenConnection(int NoOfConnAttempts)
        {
            int iAttempt = 0;
            bool connStatus = false;
            while (iAttempt < NoOfConnAttempts)
            {
                try
                {
                    objConnection.Open();
                    Program.appLogger.Info("DB Connection Open!! ");
                    connStatus = true;
                    return connStatus;
                }
                catch (SqlException ex)
                {

                    Program.appLogger.Error("Error Connecting Database!! Attempt # " + (iAttempt + 1).ToString());
                    Program.appLogger.Error("[Message]: " + ex.Message);
                    Program.appLogger.Error("[Trace]: " + ex.StackTrace);

                    if (NoOfConnAttempts == iAttempt)
                    {
                        return connStatus;
                    }
                    iAttempt++;
                }
            }
            return connStatus;
        }

        public bool CloseConnection(int NoOfConnAttempts)
        {
            int iAttempt = 0;
            bool connStatus = true;
            while (iAttempt < NoOfConnAttempts)
            {
                try
                {
                    if (objConnection.State != ConnectionState.Closed)
                        objConnection.Close();
                    Program.appLogger.Info("DB Connection Closed!! ");
                    connStatus = false;
                    return connStatus;
                }
                catch (SqlException ex)
                {

                    Program.appLogger.Error("Error Closing Database Connection!! Attempt # " + (iAttempt + 1).ToString());
                    Program.appLogger.Error("[Message]: " + ex.Message);
                    Program.appLogger.Error("[Trace]: " + ex.StackTrace);

                    if (NoOfConnAttempts == iAttempt)
                    {
                        return connStatus;
                    }
                    iAttempt++;
                }
            }
            return connStatus;
        }

        public bool ExecuteSelectCommand(string SqlCMD, ref DataTable dtResult)
        {
            bool flag = false;
            Program.appLogger.Info("Initiating Query[" + SqlCMD.Trim() + "]");

            try
            {
                objConnection = new SqlConnection(ConnString);
                SqlDataAdapter da = new SqlDataAdapter(SqlCMD, objConnection);
                da.SelectCommand.CommandTimeout = 21600;

                if (OpenConnection(NoOfDBConnAttempts))
                {
                    try
                    {
                        da.Fill(dtResult);
                        flag = true;
                        Program.appLogger.Info("[" + dtResult.Rows.Count.ToString() + "] record Collected.");
                    }
                    catch (Exception ex)
                    {
                        Program.appLogger.Error("Error collecting data from Query[" + SqlCMD.Trim() + "].");
                        Program.appLogger.Error("[Message]: " + ex.Message);
                        Program.appLogger.Error("[Trace]: " + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.appLogger.Error("Error in Initiating query[" + SqlCMD.Trim() + "]");
                Program.appLogger.Error("[Message]: " + ex.Message);
                Program.appLogger.Error("[Trace]: " + ex.StackTrace);
            }

            return flag;
        }

        public bool DbDump(DataTable dt, string destination_Table_Name)
        {
            bool flag = false;
            Program.appLogger.Info("Initiating Bulk Insert.");

            try
            {
                objConnection = new SqlConnection(ConnString);

                SqlBulkCopy objbulk = new SqlBulkCopy(objConnection);
                objbulk.DestinationTableName = destination_Table_Name;

                foreach (DataColumn dc in dt.Columns)
                {
                    objbulk.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                }
                Program.appLogger.Info("Bulk Insert Column Mapping complete.");
                if (OpenConnection(NoOfDBConnAttempts))
                {
                    try
                    {
                        objbulk.WriteToServer(dt);
                        flag = true;
                        Program.appLogger.Info("[" + dt.Rows.Count.ToString() + "] record Inserted successfully");
                    }
                    catch (Exception ex)
                    {
                        Program.appLogger.Error("Error inserting data into [" + destination_Table_Name.Trim() + "].");
                        Program.appLogger.Error("[Message]: " + ex.Message);
                        Program.appLogger.Error("[Trace]: " + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.appLogger.Error("Error setting up bulk insert.");
                Program.appLogger.Error("[Message]: " + ex.Message);
                Program.appLogger.Error("[Trace]: " + ex.StackTrace);
            }

            return flag;
        }
    }
}
