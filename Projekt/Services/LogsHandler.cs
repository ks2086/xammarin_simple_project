using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Projekt.Helpers;

namespace Projekt.Services
{
    public static class LogsHandler
    {

        public static async Task<bool> CreateLog(string logStatus, string module, string moduleUnitId)
        {
            if(await FirebaseLogsHelper.CreateLog(logStatus, module, moduleUnitId))
            {
                return true;
            }
            return false;
        }

        public static async Task<bool> CreateLogTableMigration()
        {
            await FirebaseLogsHelper.MakeLogsStatusTable();
            return true;
        }

    }
}
