using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using NLog.Targets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTD_Migration.ConfigFile;


namespace DTD_Migration.Common
{
    internal class NLogConfiguration
    {
        public void GetLoggerInstance(string loggerName, JsonConfigData configData)
        {
            //ILogger appLogger = null;
            try
            {
                string[] filePart = configData.LogFileName.ToString().Split('\'');

                foreach (FileTarget target in LogManager.Configuration.AllTargets)
                {
                    var nLogFileName = Path.GetFileName(target.FileName.ToString().Trim()).Replace("'", "") + filePart[0];

                    string log_filename = String.Format(
                                          nLogFileName,
                                          DateTime.Now.ToString(filePart[1]),
                                          filePart[2]);

                    target.FileName = Path.Combine(configData.LogPath, log_filename);
                }


                //FileTarget target = LogManager.Configuration.FindTargetByName("ApplicationLogger") as FileTarget;

                //appLogger = NLog.LogManager.GetLogger(loggerName);

                //target.Dispose();
            }
            catch
            { }
            //return appLogger;
        }
    }
}
