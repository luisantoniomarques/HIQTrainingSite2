using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ActiveDirectory
{
    class HIQActiveDirectorySearcher : ActiveDirectorySearcherBase
    {
        public override List<ADUserDetail> Search(string userName)
        {
            DirectorySearcher searcher = new ActiveDirectorySearcherBuilder("ADConnectionHiqPT", base.User, base.Password)
                .AddPropertiesToLoad("cn") //nome do consultor 
                .AddPropertiesToLoad("mail") //email
                .AddFilter(userName, FilterType.UserName)
                .Build();

           foreach (SearchResult item in searcher.FindAll())
            {
                base.AddUserDetail(
                        "HIQ Consulting",
                        item.Properties["cn"][0].ToString(),
                        item.Properties["mail"].Count == 0 ? "" : item.Properties["mail"][0].ToString()
                    );
            }


            return UserList;
        }

        public override bool GetUser(string userName, string email, string password, out string errorMessage)
        {
            errorMessage = string.Empty;
            DirectorySearcher searcher = new ActiveDirectorySearcherBuilder("ADConnectionHiqPT", userName, password)
                .AddPropertiesToLoad("cn") //nome do consultor 
                .AddPropertiesToLoad("mail") //email
                .AddFilter(email, FilterType.Email)
                .Build();

            try
            {
                if (searcher.FindOne().Properties["mail"].Count == 1)
                {
                    return true;
                }
                errorMessage = "login attempt failed";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

    }
}
