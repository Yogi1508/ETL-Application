using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTD_Migration
{
    internal class DataMigrationOperation
    {
        protected static MigrationConfig migrationConfig;
        protected static DbOperation migrationDBOperation;
        protected static DataTable dt = new DataTable();
        public DataMigrationOperation(MigrationConfig mc)
        {
            migrationConfig = mc;
        }

        internal void StartMigration()
        {
            Program.appLogger.Info("Start Process for Migration ID - " + migrationConfig.Migration_ID);
            SelectSourceData();
            MigrateData();
            dt.Clear();
            dt.Dispose();
            Program.appLogger.Info("End Process for Migration ID - " + migrationConfig.Migration_ID);
        }

        private void SelectSourceData()
        {
            string sourceConnString = null;
            if (migrationConfig.Source_Auth_Mode.Equals("Windows Auth", StringComparison.CurrentCultureIgnoreCase))
            {
                sourceConnString = "Server=" + migrationConfig.Source_Server + ";Trusted_Connection=true;database=" + migrationConfig.Source_DB_Name;
            }
            else
            {
                sourceConnString = "Server=" + migrationConfig.Source_Server + ";user id="+ migrationConfig.Source_DB_Username+ ";password="+ migrationConfig.Source_DB_Password+ ";database=" + migrationConfig.Source_DB_Name;
            }

            migrationDBOperation = new DbOperation(Program.configData.DbConfig.NoOfDBConnAttempts, sourceConnString);
            migrationDBOperation.ExecuteSelectCommand(migrationConfig.Source_Select_Query, ref dt);
            migrationDBOperation.CloseConnection(Program.configData.DbConfig.NoOfDBConnAttempts);
            migrationDBOperation = null;
           
        }

        private void MigrateData()
        {
            string destinationConnString = null;
            if (migrationConfig.Destination_Auth_Mode.Equals("Windows Auth", StringComparison.CurrentCultureIgnoreCase))
            {
                destinationConnString = "Server=" + migrationConfig.Destination_Server + ";Trusted_Connection=true;database=" + migrationConfig.Destination_DB_Name;
            }
            else
            {
                destinationConnString = "Server=" + migrationConfig.Destination_Server + ";user id=" + migrationConfig.Destination_DB_Username + ";password=" + migrationConfig.Destination_DB_Password + ";database=" + migrationConfig.Destination_DB_Name;
            }

            migrationDBOperation = new DbOperation(Program.configData.DbConfig.NoOfDBConnAttempts, destinationConnString);
            migrationDBOperation.DbDump(dt, migrationConfig.Destination_Table_Name);
            migrationDBOperation.CloseConnection(Program.configData.DbConfig.NoOfDBConnAttempts);
            migrationDBOperation = null;

        }
    }
}
