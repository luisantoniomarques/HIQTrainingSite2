using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.Utils
{
    public abstract class StatusBase
    {
        protected static List<StatusDetail> LoadStates(string statusPrefix)
        {
            List<StatusDetail> attendanceStates = new List<StatusDetail>();
            int id = 0;
            string description = "";
            do
            {
                description = GetStatusDescription(string.Concat(statusPrefix, id));
                if (description != null)
                {
                    attendanceStates.Add(new StatusDetail() { Id = id, Description = description });
                }

                ++id;
            } while (description != null);

            return attendanceStates;
        }

        protected static string GetStatusDescription(string key)
        {
            ResourceManager rm = new ResourceManager(typeof(LocalResources.HIQResource));
            string value = rm.GetString(key);
            return value;
        }

    }

    
}
