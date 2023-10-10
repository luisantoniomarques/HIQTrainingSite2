using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.Mappers
{
    public class CompanyMapper
    {
        /// <summary>
        /// Translate companyId to Company enum
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public static HIQTraining.ActiveDirectory.Company GetCompany(int companyId, string companyName)
        {
            HIQTraining.ActiveDirectory.Company company = HIQTraining.ActiveDirectory.Company.Adentis;
            if (companyName != null && companyName.StartsWith("Aden"))
                company = HIQTraining.ActiveDirectory.Company.Adentis;
            else if (companyName != null && companyName.StartsWith("Aga"))
                company = HIQTraining.ActiveDirectory.Company.Agap2;
            else if (companyName != null && companyName.StartsWith("Bee"))
                company = HIQTraining.ActiveDirectory.Company.BeeEng;
            else if (companyName != null && companyName.StartsWith("Kcs"))
                company = HIQTraining.ActiveDirectory.Company.KcsIT;
            else if (companyName != null && companyName.StartsWith("HIQ"))
                company = HIQTraining.ActiveDirectory.Company.HIQ;
            else
                throw new Exception(string.Format("Unknown company Id {0}", companyId));
 
            return company;
        }
    }
}
