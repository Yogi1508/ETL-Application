using DTD_Migration.ConfigFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTD_Migration
{
    public class JsonConfigData
    {
        #region Properties
        public string ConfigVersion { get; set; }
        //Log Details
        public string LogPath { get; set; }
        public int NoOfDaystoKeepLog { get; set; }
        public string LogFileName { get; set; }
        public string OldServiceLogSearchRegex { get; set; }
        public DbDetails DbConfig { get; set; }
        public SMTPconfig SMTPDetails { get; set; }

        #endregion
    }
}
