using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ActiveDirectory
{
    public abstract class ActiveDirectorySearcherBase
    {
        const int MAX_USERS = 10;

        public ActiveDirectorySearcherBase()
        {
            User = "consultor2";
            Password = "consultor2";
            UserList = new List<ADUserDetail>();
        }


        public List<ADUserDetail> UserList { get; set; }
        public string User { get; set; }
        public string Password { get; set; }


        public abstract List<ADUserDetail> Search(string userName);

        public abstract bool GetUser(string userName, string email, string password, out string errorMessage);

        public void AddUserDetail(string companyName, string userName, string email)
        {
            if (UserList == null)
                UserList = new List<ModelDetail.ADUserDetail>();

            if (UserList.Count() < MAX_USERS) { 

                UserList.Add(new ADUserDetail
                {
                    CompanyName = companyName,
                    Email = email,
                    Name = userName
                });

            }
        }

    }
}
