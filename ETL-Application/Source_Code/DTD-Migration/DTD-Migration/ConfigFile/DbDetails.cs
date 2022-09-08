using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTD_Migration.ConfigFile
{
    public class DbDetails
    {
        #region Properties
        public string ConnectionString { get; set; }
        public int NoOfDBConnAttempts { get; set; }
        public string MigrationConfigTable { get; set; }
        #endregion
    }
}
