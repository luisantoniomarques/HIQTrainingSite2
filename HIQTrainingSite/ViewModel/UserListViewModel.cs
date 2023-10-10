using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HIQTrainingSite.ViewModel
{

    public class UserListViewModel : AlertViewModel
    {
        public UserListViewModel()
        {
            Alert = new AlertViewModel();
        }

        public AlertViewModel Alert { get; set; }

        public IPagedList<UserViewModel> UsersList { get; set; }
    }
}