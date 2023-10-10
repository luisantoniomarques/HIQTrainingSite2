using HIQTrainingSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using HIQTrainingSite.ViewModel;
using System.Threading.Tasks;
using HIQTraining.Exceptions;
using HIQTraining.ActiveDirectory;
using HIQTraining.ModelDetail;

namespace HIQTrainingSite.Helper
{
    public class IdentityHelper
    {
        ApplicationDbContext context;
        CompanyUser companyUser;

        public IdentityHelper()
        {
            context = new ApplicationDbContext();
            companyUser = new CompanyUser();
        }


        /// <summary>
        /// Get a paged list of application users
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public IPagedList<UserViewModel> GetApplicationUsers(int page)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            IPagedList<UserViewModel> usersList = (from u in userManager.Users
                                                   orderby u.UserName
                                                   select (new UserViewModel
                                                   {
                                                       Id = u.Id,
                                                       Name = u.UserName,
                                                       Email = u.Email
                                                   }))
                                                     .ToPagedList(page, Common.HIQSiteConstants.General.PAGE_SIZE);
            return usersList;
        }

        /// <summary>
        /// Get Application User by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserViewModel GetUserById(string userId)
        {
            var db = new ApplicationDbContext();

            var user = (from u in db.Users
                        where u.Id.Equals(userId)
                        let query = (from ur in db.Set<IdentityUserRole>()
                                     where ur.UserId.Equals(u.Id)
                                     join r in db.Roles on ur.RoleId equals r.Id
                                     select new UserRoleViewModel() { Id = r.Id, Name = r.Name })
                        select new UserViewModel() {
                            Id = u.Id,
                            Name = u.UserName,
                            Email = u.Email,
                            DisplayColor = u.Color,
                            UserRoles = query.ToList<UserRoleViewModel>() }).FirstOrDefault();

            return user;
        }

        public UserViewModel GetUserDetailById(string userId)
        {
            var db = new ApplicationDbContext();

            var user = (from u in db.Users
                        where u.Id.Equals(userId)
                        select new UserViewModel()
                        {
                            Id = u.Id,
                            Name = u.UserName,
                            Email = u.Email
                        }).FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Gets all Application Roles
        /// </summary>
        /// <returns></returns>
        public List<ApplicationRoleViewModel> GetApplicationRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            List<ApplicationRoleViewModel> appRolesList =
                (roleManager.Roles
                .Select(s => new ApplicationRoleViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsSelected = false
                }))
                .ToList();
            return appRolesList;
        }



        public bool ChangePassword(string userId, string password, string confirmPassword)
        {
            return false;
        }
        public bool ChangeUserRoles(string userId, List<ApplicationRoleViewModel> userRoles)
        {
            IdentityResult result = null;
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            foreach (var role in userRoles)
            {
                if (role.IsSelected)
                    result = userManager.AddToRole(userId, role.Name);
                else
                    result = userManager.RemoveFromRole(userId, role.Name);
            }

            return result.Succeeded;

        }

        public IdentityResult RemoveApplicationUser(string userId)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var applicationUser = userManager.FindById(userId);
            if (applicationUser == null)
                throw new HIQTrainingException(HIQResources.errorMessageUnknownUser);

            return userManager.Delete(applicationUser);
        }

        public IdentityResult UpdateApplicationUser(UserViewModel model)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var applicationUser = userManager.FindById(model.Id);
            applicationUser.Color = model.DisplayColor;
            if (applicationUser == null)
                throw new HIQTrainingException(HIQResources.errorMessageUnknownUser);

            return userManager.Update(applicationUser);
        }

        public IdentityResult AddApplicationUser(int companyId, string userName, string email, List<ApplicationRoleViewModel> applicationRoles, string displayColor)
        {
            // validates if the user exists on the active directory
            List<ADUserDetail> adUser = companyUser.SearchUsersByName(companyId, userName);
            if (adUser.Count == 1 && adUser[0].Email.Equals(email))
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


                
                // Configure validation logic for passwords
                //userManager.PasswordValidator = new PasswordValidator
                //{
                //    RequiredLength = 6,
                //    RequireNonLetterOrDigit = true,
                //    RequireDigit = true,
                //    RequireLowercase = true,
                //    RequireUppercase = true,
                //};

                userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
                {
                    AllowOnlyAlphanumericUserNames = false
                };
                // Configure user lockout defaults
                userManager.UserLockoutEnabledByDefault = true;
                userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
                userManager.MaxFailedAccessAttemptsBeforeLockout = 5;
               
                ApplicationUser user = new ApplicationUser();
                user.Email = email;
                user.UserName = email;
                user.Color = "#" + displayColor.Trim().ToLower();

                // adding user...
                var userResult = userManager.Create(user);

                if (userResult.Succeeded)
                {
                    // adding password to the user...
                   // var resultPassword = userManager.AddPassword(user.Id, password);
                    //if (resultPassword.Succeeded)
                    //{
                        // adding user roles...
                        var rolesResult = this.AddUserRoles(user.Id, applicationRoles);
                        if (rolesResult.Succeeded)
                        {
                            return rolesResult;
                        }
                        else
                        {
                            // if error remove user...
                            this.RemoveApplicationUser(user.Id);
                            return rolesResult;
                        }
                    //}
                    //else
                    //{
                        // if error remove user...
                    //    this.RemoveApplicationUser(user.Id);
                    //    return resultPassword;
                    //}
                }
                else
                {
                    return userResult;
                }
            }
            else
            {
                throw new HIQTrainingException(HIQResources.errorMessageUnknownUser);
            }
        }

        
        private IdentityResult AddUserRoles(string userId, List<ApplicationRoleViewModel> applicationRoles)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };
            return userManager.AddToRoles(userId, applicationRoles.Where(x => x.IsSelected).Select(s => s.Name).ToArray());
        }
    }
}