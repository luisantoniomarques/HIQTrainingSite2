using HIQTraining.Business.Log;
using HIQTraining.DAL.Log;
using HIQTrainingSite.Helper;
using HIQTrainingSite.ModelsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            Log = new LogManager(new LogDao());
        }

        public LogManager Log { get; set; }


        protected override void ExecuteCore()
        {
            int culture = 0;
            if (this.Session == null || this.Session["CurrentCulture"] == null)
            {
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Culture"], out culture);
                this.Session["CurrentCulture"] = culture;
            }
            else
            {
                culture = (int)this.Session["CurrentCulture"];
            }
            // calling CultureHelper class properties for setting  
            CultureHelper.CurrentCulture = culture;

            base.ExecuteCore();
        }


        public ActionResult ChangeCurrentCulture(int id)
        {
            //  
            // Change the current culture for this user.  
            //  
            CultureHelper.CurrentCulture = id;
            //  
            // Cache the new current culture into the user HTTP session.   
            //  
            Session["CurrentCulture"] = id;
            HIQTrainingSite.ViewModel.BaseViewModel.Language = id;
            //  
            // Redirect to the same page from where the request was made!   
            //

            return Redirect(Request.UrlReferrer.ToString());
        }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }


        protected string GetLoggedUser()
        {
            return string.IsNullOrWhiteSpace(User.Identity.Name) ? "Anonymous" : User.Identity.Name;
        }

        protected string GetUserColor()
        {
            return User.Identity.GetColor();
        }
    }
}