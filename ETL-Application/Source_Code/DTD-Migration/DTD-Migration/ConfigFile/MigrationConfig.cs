using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTD_Migration
{
    public class MigrationConfig
    {
        #region Property

        public int Migration_ID { get; set; }
        public string Source_Server { get; set; }
        public string Source_DB_Name { get; set; }
        public string Source_Auth_Mode { get; set; }
        public string Source_DB_Username { get; set; }
        public string Source_DB_Password { get; set; }
        public string Source_Select_Query { get; set; }
        public string Destination_Server { get; set; }
        public string Destination_DB_Name { get; set; }
        public string Destination_Auth_Mode { get; set; }
        public string Destination_DB_Username { get; set; }
        public string Destination_DB_Password { get; set; }
        public string Destination_Table_Name { get; set; }



        #endregion
    }
}
