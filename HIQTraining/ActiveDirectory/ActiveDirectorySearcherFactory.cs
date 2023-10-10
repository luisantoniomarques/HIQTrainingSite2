using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ActiveDirectory
{
    public class ActiveDirectorySearcherFactory
    {
        public ActiveDirectorySearcherBase CreateActiveDirectorySearcher(Company company)
        {
            ActiveDirectorySearcherBase searcher = null;

            switch(company)
            {
                case Company.Adentis:
                    searcher = new AdentisActiveDirectorySearcher();
                    break;
                case Company.Agap2:
                    searcher = new Agap2ActiveDirectorySearcher();
                    break;
                case Company.BeeEng:
                    searcher = new BeeEngActiveDirectorySearcher();
                    break;
                case Company.KcsIT:
                    searcher = new KcsActiveDirectorySearcher();
                    break;
                case Company.HIQ:
                    searcher = new HIQActiveDirectorySearcher();
                    break;
            }

            return searcher;
        }


    }
}
