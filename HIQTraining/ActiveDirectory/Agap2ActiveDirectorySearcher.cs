using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;

namespace HIQTraining.ActiveDirectory
{
    internal class Agap2ActiveDirectorySearcher : ActiveDirectorySearcherBase
    {
        public override List<ADUserDetail> Search(string userName)
        {
            DirectorySearcher searcher = new ActiveDirectorySearcherBuilder("ADConnectionAgap2PT", base.User, base.Password)
                .AddPropertiesToLoad("Name") //nome do consultor 
                .AddPropertiesToLoad("mail") //email
                .AddFilter(userName, FilterType.UserName)
                .Build();

            foreach (SearchResult item in searcher.FindAll())
            {
                base.AddUserDetail(
                        "Agap2",
                        item.Properties["Name"][0].ToString(),
                        item.Properties["mail"].Count == 0 ? "" : item.Properties["mail"][0].ToString()
                    );
            }


            return UserList;
        }

        public override bool GetUser(string userName, string email, string password, out string errorMessage)
        {
            errorMessage = string.Empty;
            DirectorySearcher searcher = new ActiveDirectorySearcherBuilder("ADConnectionAgap2PT", userName, password)
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