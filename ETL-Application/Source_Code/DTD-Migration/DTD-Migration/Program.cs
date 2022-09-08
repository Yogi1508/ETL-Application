using DTD_Migration.Common;
using Newtonsoft.Json;
using NLog;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DTD_Migration
{
    internal class Program
    {
        public static ILogger appLogger;
        public static NLogConfiguration NLogConfiguration = new NLogConfiguration();
        public static List<MigrationConfig> migrationConfigs = new List<MigrationConfig>();
        public static DbOperation dbOperation;
        public static JsonConfigData configData;
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Environment.Exit(-1);
                //Console.WriteLine("Please Provide Json Config File Path.");
            }

            string jsonConfigFile = args[0];

            if (File.Exists(jsonConfigFile))
            {
                try
                {
                    string configStream = File.ReadAllText(jsonConfigFile);
                    configData = JsonConvert.DeserializeObject<JsonConfigData>(configStream);


                    if (configData == null)
                    {
                        Console.WriteLine("Unable to load JSON config data.");
                        Thread.Sleep(4000);
                        Environment.Exit(-1);
                    }
                    else
                    {

                        try
                        {
                            NLogConfiguration.GetLoggerInstance("ApplicationLogger", configData);
                            appLogger = NLog.LogManager.GetLogger("ApplicationLogger");
                            appLogger.Info("Application Start");

                            var SQLcmd = PrepSelectQuery(args);

                            dbOperation = new DbOperation(configData.DbConfig.NoOfDBConnAttempts, configData.DbConfig.ConnectionString);

                            DataTable dt = new DataTable();
                            appLogger.Info("Collecting Data from [" + configData.DbConfig.MigrationConfigTable + "]");
                            dbOperation.ExecuteSelectCommand(SQLcmd, ref dt);
                            dbOperation.CloseConnection(configData.DbConfig.NoOfDBConnAttempts);
                            FillMigrationConfig(dt);
                            dt.Clear();
                            dt.Dispose();
                            foreach (MigrationConfig record in migrationConfigs)
                            {
                                DataMigrationOperation dataMigrationOperation = new DataMigrationOperation(record);
                                dataMigrationOperation.StartMigration();
                            }
                            appLogger.Info("Application End.");
                        }
                        catch (Exception ex)
                        {
                            Program.appLogger.Error("Error inserting data into .");
                            Program.appLogger.Error("[Message]: " + ex.Message);
                            Program.appLogger.Error("[Trace]: " + ex.StackTrace);
                        }
                        finally
                        {
                            NLogConfiguration = null;
                            appLogger = null;
                            dbOperation = null;
                            configData = null;
                        }


                        Console.ReadKey();

                    }

                }
                catch
                {
                    Environment.Exit(-1);
                }
            }
        }

        public static void FillMigrationConfig(DataTable dt)
        {
            Program.appLogger.Info("Filling Migration Config using DB result.");

            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MigrationConfig migrationConfig = new MigrationConfig();

                    migrationConfig.Migration_ID = Convert.ToInt32(dr["Migration_ID"]);
                    migrationConfig.Source_Server = Convert.ToString(dr["Source_Server"]);
                    migrationConfig.Source_DB_Name = Convert.ToString(dr["Source_DB_Name"]);
                    migrationConfig.Source_Auth_Mode = Convert.ToString(dr["Source_Auth_Mode"]);
                    migrationConfig.Source_DB_Username = Convert.ToString(dr["Source_DB_Username"]);
                    migrationConfig.Source_DB_Password = Convert.ToString(dr["Source_DB_Password"]);
                    migrationConfig.Source_Select_Query = Convert.ToString(dr["Source_Select_Query"]);
                    migrationConfig.Destination_Server = Convert.ToString(dr["Destination_Server"]);
                    migrationConfig.Destination_DB_Name = Convert.ToString(dr["Destination_DB_Name"]);
                    migrationConfig.Destination_Auth_Mode = Convert.ToString(dr["Destination_Auth_Mode"]);
                    migrationConfig.Destination_DB_Username = Convert.ToString(dr["Destination_DB_Username"]);
                    migrationConfig.Destination_DB_Password = Convert.ToString(dr["Destination_DB_Password"]);
                    migrationConfig.Destination_Table_Name = Convert.ToString(dr["Destination_Table_Name"]);

                    migrationConfigs.Add(migrationConfig);
                }
            }
            catch (Exception ex)
            {
                Program.appLogger.Error("Error at Filling Migration Config using DB result.");
                Program.appLogger.Error("[Message]: " + ex.Message);
                Program.appLogger.Error("[Trace]: " + ex.StackTrace);
            }

            Program.appLogger.Info(migrationConfigs.Count().ToString()+" Migration Config record loaded.");
        }

        public static string PrepSelectQuery(string[] args)
        {
            var SQLcmd = "select Migration_ID, Source_Server, Source_DB_Name, Source_Auth_Mode, Source_DB_Username, Source_DB_Password, Source_Select_Query, Destination_Server, Destination_DB_Name, Destination_Auth_Mode, Destination_DB_Username, Destination_DB_Password, Destination_Table_Name, IsActive from " + configData.DbConfig.MigrationConfigTable;

            try
            {
                if (args.Length > 1)
                {
                    var migrationIDs = args[1].Split(',');

                    string mID = migrationIDs[0];
                    for (int i = 1; i < migrationIDs.Count(); i++)
                    {
                        mID = mID + "," + migrationIDs[i];
                    }
                    SQLcmd = SQLcmd + " where Migration_ID in (" + mID + ")";
                    
                }
                else
                {

                    SQLcmd = SQLcmd + " where IsActive=1";
                }
            }
            catch (Exception ex)
            {
                Program.appLogger.Error("Error While Preparing Select Query");
                Program.appLogger.Error("[Message]: " + ex.Message);
                Program.appLogger.Error("[Trace]: " + ex.StackTrace);
            }

            return SQLcmd;
        }
    }
}
