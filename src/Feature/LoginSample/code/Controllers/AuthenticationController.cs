using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Mvc.Configuration;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;
using SF.Foundation.Components;

namespace SF.Feature.LoginSample.Controllers
{
    public class AuthenticationController : SitecoreController
    {
        private const string ROOT_VIEW_PATH = @"/Views/Feature.LoginSample/";

        #region Login

        [HttpGet]
        public override ActionResult Index()
        {
            var model = new LoginViewModel();

            var configItem = this.GetItem();
            var redirectField = (Sitecore.Data.Fields.LinkField)configItem.Fields["RedirectWhenLoggedIn"];
            var redirectUrl = redirectField.GetFriendlyUrl();

            if (Sitecore.Context.IsLoggedIn && !Sitecore.Context.PageMode.IsExperienceEditorEditing && !string.IsNullOrEmpty(redirectUrl))
            {
                Sitecore.Web.WebUtil.Redirect(redirectUrl); 
            }

            return View(ROOT_VIEW_PATH + "Index.cshtml", model);
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            //check if this form was actually submitted
            if (Request["requestType"] != "loginSubmission")
            {
                //Default to Non Post Version
                return Index();
            }

            if (String.IsNullOrEmpty(model.UserName) || String.IsNullOrEmpty(model.Password))
            {
                var errorMessage = this.GetItem().Fields["MissingUserOrPassword"].Value;
                ViewBag.ErrorMessage = errorMessage;
            }            
            else
            {
                try
                {
                    Sitecore.Security.Domains.Domain domain = Sitecore.Context.Domain;
                    var userPrefix = this.GetItem().Fields["UserPrefix"].Value;
                    string domainUser = domain.Name + @"\" + userPrefix + model.UserName;

                    if (Sitecore.Security.Authentication.AuthenticationManager.Login(domainUser,
                    model.Password, model.Persistant))
                    {
                        var configItem = this.GetItem();
                        var redirectField = (Sitecore.Data.Fields.LinkField)configItem.Fields["RedirectAfterLogin"];
                        var redirectUrl = redirectField.GetFriendlyUrl();

                        //Track for xDB
                        Sitecore.Analytics.Tracker.Current.Session.Identify(domainUser);

                        
                        if (string.IsNullOrEmpty(redirectUrl))
                        {
                            //redirect to root page if not specified
                            redirectUrl = "/"; 
                        }
                        
                        Sitecore.Web.WebUtil.Redirect(redirectUrl);
                        
                    }
                    else
                    {
                        throw new System.Security.Authentication.AuthenticationException(
                        "Invalid username or password.");
                    }
                }
                catch (System.Security.Authentication.AuthenticationException)
                {
                    Sitecore.Diagnostics.Log.Audit("Failed Login attempt for username:[" + model.UserName + "]", this);
                    var errorMessage = this.GetItem().Fields["InvalidUserOrPassword"].Value;
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = "Invalid username or password.";
                    }
                    ViewBag.ErrorMessage = errorMessage;
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Exception encountered when attempting login", ex, this);
                    var errorMessage = this.GetItem().Fields["GeneralError"].Value;
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = "Error Occured when attempting login. Please try again.";
                    }
                    ViewBag.ErrorMessage = errorMessage;
                }
            }

            return View(ROOT_VIEW_PATH + "Index.cshtml", model);
        }

        #endregion

        #region Register

        [HttpGet]
        public ActionResult Register()
        {
            var model = new RegisterViewModel();

            var configItem = this.GetItem();
            var redirectField = (Sitecore.Data.Fields.LinkField)configItem.Fields["RedirectWhenLoggedIn"];
            var redirectUrl = redirectField.GetFriendlyUrl();

            if (Sitecore.Context.IsLoggedIn && !Sitecore.Context.PageMode.IsExperienceEditorEditing && !string.IsNullOrEmpty(redirectUrl))
            {
                Sitecore.Web.WebUtil.Redirect(redirectUrl);
            }

            AddQuestions(model);

            return View(ROOT_VIEW_PATH + "Register.cshtml", model);
        }

        private void AddQuestions(RegisterViewModel model)
        {

            model.Questions = new List<string>();

            var item = this.GetItem();
            var qPrompt = item.Fields["FirstQuestionPrompt"].Value;

            model.Questions.Add(qPrompt);

            //Get Rest of Questions.
            var fields = item.Fields["Questions"].Value.Split('\n');
            foreach (var field in fields)
            {
                model.Questions.Add(field);
            }

        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            //check if this form was actually submitted
            if (Request["requestType"] != "registerSubmission")
            {
                //Default to Non Post Version
                return Register();
            }


            if (!ModelState.IsValid || model.Password != model.ConfirmPassword)
            {
                ViewBag.ErrorMessage = "Invalid Data.";
            }
            else
            {
                var userPrefix = this.GetItem().Fields["UserPrefix"].Value;
                string domainUser = Sitecore.Context.Domain.GetFullName(userPrefix + model.UserName);

                try
                {
                    if (Sitecore.Security.Accounts.User.Exists(domainUser))
                    {
                        throw new System.Web.Security.MembershipCreateUserException(
                        domainUser + " exists.");
                    }
                    else if (System.Web.Security.Membership.Provider.RequiresUniqueEmail
                    && !String.IsNullOrEmpty(
                    System.Web.Security.Membership.Provider.GetUserNameByEmail(model.EmailAddress)))
                    {
                        throw new System.Web.Security.MembershipCreateUserException(
                            model.EmailAddress + " already registered.");
                    }
                    else
                    {
                        System.Web.Security.MembershipCreateStatus status;
                        

                        var autoApprove = ((Sitecore.Data.Fields.CheckboxField)this.GetItem().Fields["AutoApprove"]).Checked;

                       System.Web.Security.Membership.CreateUser(
                            domainUser,
                            model.Password,
                            model.EmailAddress,
                            model.QuestionText,
                            model.AnswerText,
                            autoApprove, out status);

                        if (!status.Equals(System.Web.Security.MembershipCreateStatus.Success))
                        {
                            throw new System.Web.Security.MembershipCreateUserException(status.ToString());
                        }

                        //Let's add some roles
                        var defaultRoles = this.GetItem().Fields["DefaultRoles"].Value.Split('\n');
                        var sitecoreUser = Sitecore.Security.Accounts.User.FromName(domainUser, true);
                        
                        var userProfile = sitecoreUser.Profile;
                        userProfile.Email = model.EmailAddress;
                        userProfile.SetCustomProperty("questionText", model.QuestionText);
                        userProfile.SetCustomProperty("answerText", model.AnswerText);

                        //Check for additional Posts
                        foreach(string key in Request.Form.Keys)
                        {
                            if (key.StartsWith("profile_"))
                            {
                                var profileKey = key.Substring(8).ToLower();
                                if (profileKey.Length > 1)
                                {
                                    userProfile.SetCustomProperty(profileKey, Request.Form[key]);
                                }
                            }
                        }

                        userProfile.Comment = string.Format("Q:{0}, A:{1}", model.QuestionText, model.AnswerText);

                        userProfile.Save();

                        foreach(var defaultRole in defaultRoles)
                        {
                            try
                            {
                                var role = Sitecore.Security.Accounts.Role.FromName(defaultRole);
                                Sitecore.Security.Accounts.UserRoles.FromUser(sitecoreUser).Add(role);
                            }
                            catch(Exception ex)
                            {
                                Sitecore.Diagnostics.Log.Error("Unable to add default role " + defaultRole + " for user " + domainUser, ex, this);
                            }
                        }

                        var autoLogin = ((Sitecore.Data.Fields.CheckboxField)this.GetItem().Fields["AutoLogin"]).Checked;

                        if (autoLogin)
                        {
                            if (!Sitecore.Security.Authentication.AuthenticationManager.Login(
                                    domainUser,
                                    model.Password,
                                    false))
                            {
                                throw new System.Web.Security.MembershipPasswordException(
                                "Unable to login after creating " + domainUser);
                            }
                        }
                        else
                        {
                            Sitecore.Security.Authentication.AuthenticationManager.Logout();
                        }

                        //regardless, track in xDB
                        Sitecore.Analytics.Tracker.Current.Session.Identify(domainUser);

                        //Sitecore.Analytics.Tracker.CurrentVisit.ContactVisitIndex
                        

                        //populate rest of contact
                        var contact = Sitecore.Analytics.Tracker.Current.Session.Contact;

                        // get the personal facet
                        var contactPersonalInfo = contact.GetFacet<Sitecore.Analytics.Model.Entities.IContactPersonalInfo>("Personal");

                        //For sample only
                        contactPersonalInfo.FirstName = model.UserName;

                        var contactEmail = contact.GetFacet<Sitecore.Analytics.Model.Entities.IContactEmailAddresses>("Emails");

                        // Create an email if not already present.
                        // This can be named anything, but must be the same as "Preferred" if you want
                        // this email to show in the Experience Profiles backend. 
                        if (!contactEmail.Entries.Contains("Home"))
                        {
                            contactEmail.Entries.Create("Home");
                        }

                        // set the email
                        var email = contactEmail.Entries["Home"];
                        email.SmtpAddress = model.EmailAddress;

                        contactEmail.Preferred = "Home";  

                        var configItem = this.GetItem();
                        var redirectField = (Sitecore.Data.Fields.LinkField)configItem.Fields["RedirectAfterRegistration"];
                        var redirectUrl = redirectField.GetFriendlyUrl();

                        if (string.IsNullOrEmpty(redirectUrl))
                        {
                            //redirect to root page if not specified
                            redirectUrl = "/";
                        }

                        Sitecore.Web.WebUtil.Redirect(redirectUrl);


                        
                    }
                }
                catch (System.Web.Security.MembershipPasswordException ex)
                {
                    Sitecore.Diagnostics.Log.Error("Exception encountered when attempting registration", ex, this);
                    var errorMessage = this.GetItem().Fields["GeneralError"].Value;
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = "Error Occured when attempting to login. Please try again.";
                    }
                    ViewBag.ErrorMessage = errorMessage;
                }
                catch (System.Web.Security.MembershipCreateUserException ex)
                {
                    var errorMessage = "Unable To Create Account.";
                    switch(ex.StatusCode){
                        case System.Web.Security.MembershipCreateStatus.DuplicateEmail:
                            errorMessage = this.GetItem().Fields["DuplicateEmailMessage"].Value;
                            break;
                        case System.Web.Security.MembershipCreateStatus.DuplicateUserName:
                            errorMessage = this.GetItem().Fields["DuplicateUserMessage"].Value;
                            break;
                    }
                    ViewBag.ErrorMessage = errorMessage;
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Exception encountered when attempting registration", ex, this);
                    var errorMessage = this.GetItem().Fields["GeneralError"].Value;
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = "Error Occured when attempting registration. Please try again.";
                    }
                    ViewBag.ErrorMessage = errorMessage;
                }
            }

            AddQuestions(model);
            return View(ROOT_VIEW_PATH + "Register.cshtml", model);

        }

        #endregion

        #region Logout

        [HttpGet]
        public ActionResult LogOut()
        {
            if (Sitecore.Context.PageMode.IsExperienceEditorEditing || Sitecore.Context.PageMode.IsPreview)
            {
                return Content(@"<div data-alert class=""alert-box warning round"">Log Out Component Disabled in Edit/Preview Mode</div>");
            }

            Sitecore.Security.Authentication.AuthenticationManager.Logout();

            var item = this.GetItem();
            var redirectUrl = (Sitecore.Data.Fields.LinkField) item.Fields["RedirectUrl"];
            string redirectPath = redirectUrl.GetFriendlyUrl();

            //hopefully force xDB write
            Session.Abandon();

            if (!string.IsNullOrEmpty(redirectPath))
            {
                Sitecore.Web.WebUtil.Redirect(redirectPath);
            }

            //Still need to return something...
            return Content("");
        }

        #endregion

        #region ForgotPassword

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            var model = new ForgottenPasswordViewModel();
            return View(ROOT_VIEW_PATH + "ForgotPassword.cshtml", model);
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgottenPasswordViewModel model)
        {
            if (!string.IsNullOrEmpty(model.UserName) && !model.AnswerProvided)
            {
                string domainUser = Sitecore.Context.Domain.GetFullName(model.UserName);
                if (!Sitecore.Security.Accounts.User.Exists(domainUser))
                {
                    ViewBag.ErrorMessage = "That user name doesn't exist.";
                    return View(model);
                }
                else
                {
                    var user = System.Web.Security.Membership.GetUser(domainUser);
                    model.QuestionText = user.PasswordQuestion;
                    model.AnswerProvided = true;

                    return View(ROOT_VIEW_PATH + "Challenge.cshtml", model);

                }
            }

            if (model.AnswerProvided)
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Please provide an answer";
                    return View(ROOT_VIEW_PATH + "Challenge.cshtml", model);
                }
                else
                {
                    string domainUser = Sitecore.Context.Domain.GetFullName(model.UserName);
                    var user = System.Web.Security.Membership.GetUser(domainUser);
                    try
                    {
                        model.NewPassword = user.ResetPassword(model.AnswerText);

                        //Log them on
                        Sitecore.Security.Authentication.AuthenticationManager.Login(
                            domainUser,
                            model.NewPassword,
                            false);

                        return View(ROOT_VIEW_PATH + "ResetPassword.cshtml", model);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "The answer supplied is incorrect";
                        Sitecore.Diagnostics.Log.Error("Error in Forgot Password", ex, this);
                        return View(ROOT_VIEW_PATH + "Challenge.cshtml", model);
                    }


                }
            }

            return View(model);
        }
        #endregion
    }
}