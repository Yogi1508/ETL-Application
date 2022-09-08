using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTD_Migration.ConfigFile
{
    public class SMTPconfig
    {
        #region Properties
        public string SMTPServer { get; set; }
        public string EmailFromAddress { get; set; }
        public string EmailToAddresses { get; set; }
        public string ErrorEmailList { get; set; }
        public string ErrorSubject { get; set; }
        public string SucessSubject { get; set; }

        #endregion
    }
}
