﻿using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ActiveDirectory
{
    public class AdentisActiveDirectorySearcher : ActiveDirectorySearcherBase
    {
        
        public override List<ADUserDetail> Search(string userName)
        {
            DirectorySearcher searcher = new ActiveDirectorySearcherBuilder("ADConnectionAdentisPT", base.User, base.Password)
                .AddPropertiesToLoad("Name") //nome do consultor 
                .AddPropertiesToLoad("mail") //email
                .AddFilter(userName, FilterType.UserName)
                .Build();

            foreach (SearchResult item in searcher.FindAll())
            {
                base.AddUserDetail(
                        "Adentis",
                        item.Properties["Name"][0].ToString(),
                        item.Properties["mail"].Count == 0 ? "" : item.Properties["mail"][0].ToString()
                    );
            }


            return UserList;
        }

        public override bool GetUser(string userName, string email, string password, out string errorMessage)
        {
            errorMessage = string.Empty;
            DirectorySearcher searcher = new ActiveDirectorySearcherBuilder("ADConnectionAdentisPT", userName, password)
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
