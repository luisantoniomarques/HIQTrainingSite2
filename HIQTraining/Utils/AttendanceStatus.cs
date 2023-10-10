using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.Utils
{
    public class AttendanceStatus : StatusBase
    {
        const string STATUS_PREFIX = "statusAttendance_";
        static readonly List<StatusDetail> states = LoadStates(STATUS_PREFIX);


        public static List<StatusDetail> GetStates()
        {
            return states;
        }

        public static string GetStatusDescription(int id)
        {
            return GetStatusDescription(string.Concat(STATUS_PREFIX, id));
        }
    }
}
