using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Repositories;
using CwcExim.Models;
using CwcExim.Security;
using System.Web.Security;
using Newtonsoft.Json;
using System.Web.SessionState;
using System.Configuration;
using CwcExim.UtilityClasses;
using System.IO;
using CwcExim.Areas.GateOperation.Models;

namespace CwcExim.Controllers
{
    public class UserController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region Log In
        public ActionResult Login()
        {
            return PartialView("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Login(Login ObjLogin)
        {
            /// NOTE:-
            /// Status=1 means Login successful
            /// Status=-1 means 1st time failure attempt, Incorrect username or password 
            /// Status=-2 means 2nd time failure attempt, Incorrect password given
            /// Status=-3 means 3rd time failure attempt, Incorrect password given
            /// Status=-4 means Account is temporarily locked
            /// Status=-5 means Account is not active

            string DocPath = "";
            int Status = 0;
            string Msg = "";
            int RoleId = 0;
            int FirstLogin = 0;
            if (ModelState.IsValid)
            {
                SessionIDManager manager = new SessionIDManager();
                manager.RemoveSessionID(System.Web.HttpContext.Current);
                var SessionId = manager.CreateSessionID(System.Web.HttpContext.Current);
                Session["SessionId"] = SessionId;
                UserRepository ur = new UserRepository();
                ObjLogin.Password = ObjLogin.HdnLoginPassword;
                ur.Login(ObjLogin.LoginId, ObjLogin.Password);
                Login user = (Login)ur.DBResponse.Data;
                if (user != null && user.IsBlocked == true)
                {
                    Status = 5;
                    Msg = "Your account is not active.";
                }
                else if ((user != null && user.Locked == false && user.IsBlocked == false))
                {
                    HttpContext.Session["LoginUser"] = user;
                    string[] roles = new string[1];
                    roles[0] = user.Role.RoleName;
                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.LoginId = user.LoginId;
                    serializeModel.UserName = user.Name;
                    serializeModel.Email = user.Email;
                    serializeModel.roles = roles;//.RoleName;

                    string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                            user.Email,
                             DateTime.Now,
                             DateTime.Now.AddMinutes(20),
                             false,
                             userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);
                    //FormsAuthentication.SetAuthCookie(user.Name, false);
                    Status = 1;
                    Msg = "Login success";
                    RoleId = user.Role.RoleId;
                    FirstLogin = user.FirstLogin;
                 
                    DocPath = Server.MapPath("~/Docs") + "/" + Session.SessionID;
                    try
                    {
                        if (!Directory.Exists(DocPath))
                        {
                            Directory.CreateDirectory(DocPath);
                            log.InfoFormat("Session path created for session id :{0}", Session.SessionID);
                        }
                    }
                    catch (DirectoryNotFoundException DnfEx)
                    {
                        log.Error(DnfEx.Message);
                    }

                }
                else if ((user != null && user.Locked == true))
                {
                    Status = -4;
                    Msg = "Your account is temporarily locked now. Come back later.";
                }
                else
                {
                    ur.SaveUserFailureAttempt(ObjLogin.LoginId);
                    string FailMsg = ur.DBResponse.Message;
                    Status = ur.DBResponse.Status;
                    RoleId = 0;
                    if (ur.DBResponse.Status == -4)
                    {
                        Msg = "Your account is temporarily locked now. Come back later.";
                    }
                    else
                    {
                        if (ur.DBResponse.Status == -3)
                        {
                            AccountLock ObjAccountLock = new AccountLock();
                            ObjAccountLock.LoginId = ObjLogin.LoginId;
                            ObjAccountLock.SessionId = SessionId;
                            ObjAccountLock.IPAddress = Request.UserHostAddress;
                            ObjAccountLock.LockMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["LockTimePeriod"]);
                            ur.SaveAccountLock(ObjAccountLock);
                        }
                        //Status = 0;
                        Msg = "Incorrect user Id and/or password." + FailMsg;
                    }
                }

                /// Save Login Audit Trail

                LoginAuditTrail ObjTrail = new LoginAuditTrail();
                ObjTrail.LoginId = ObjLogin.LoginId;
                ObjTrail.SessionId = SessionId;
                ObjTrail.Status = Status;
                ObjTrail.IPAddress = Request.UserHostAddress;
                UserRepository UserRR = new UserRepository();
                UserRR.SaveLoginAuditTrail(ObjTrail);
            }

            var Data = new { Status = Status, Msg = Msg, RoleId = RoleId, FirstLogin= FirstLogin };
            return Json(Data);

        }


        public ActionResult LogOff()
        {
            LoginAuditTrail ObjTrail = new LoginAuditTrail();
            Login ObjLogin = (Login)Session["LoginUser"];
            ObjTrail.TrailId = 1;
            ObjTrail.LoginId = ObjLogin.LoginId;
            ObjTrail.SessionId = Session["SessionId"].ToString();
            UserRepository UserRR = new UserRepository();
            UserRR.SaveLoginAuditTrail(ObjTrail);
            string DownloadFolder = "";
            DownloadFolder = Server.MapPath("~/Docs") + "/" + Session.SessionID;
            FtpFileManager.ClearDownloadFolder(DownloadFolder);
            Session.Abandon();
            return Redirect(ConfigurationManager.AppSettings["MainDomainUrl"].ToString());
            //return RedirectToAction("MainLandingPage", "MainDashBoard");
        }

        public ActionResult ClearSession()
        {
            Session.Abandon();

            var Data = new { Status = 1, Msg = "Session clear" };
            return Json(Data, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Create External User With ID

        // Create User View
        public ActionResult SignUp(string LoginWithId)
        {
            SignupWithId Signup = new SignupWithId();
            ViewBag.LoginId = LoginWithId;
            Signup.LoginId = LoginWithId;
            if (LoginWithId != null && LoginWithId != "")
            {
                UserRepository ObjUserRR = new UserRepository();
                ObjUserRR.FillSignupWithId(LoginWithId); // Signup with id details fill up
                Signup = (SignupWithId)ObjUserRR.DBResponse.Data;
            }

            return PartialView("SignupWithId", Signup);
        }

        // Check User
        [HttpPost]
        public ActionResult CheckUser(string LoginWithId)
        {
            string ErrMsg = "";

            Login ObjLogin = new Login();
            UserRepository UserRR = new UserRepository();
            UserRR.CheckUser(LoginWithId);
            //Status = UserRR.DBResponse.Status;
            if (UserRR.DBResponse.Status == 0)
            {
                ErrMsg = "User Id Does Not Exist.";
                //ModelState.AddModelError("LoginId", Msg);
                //TryUpdateModel(Signup);
                //ViewBag.ErrLoginWithId = Msg;
                var Err = new { Status = 0, Msg = ErrMsg };
                return Json(Err);

            }
            else
            {
                ErrMsg = "Success";
                var Err = new { Status = 1, Msg = ErrMsg };
                return Json(Err);
            }


        }
        // Create user with id

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(SignupWithId ObjSignup)
        {
            string GeneratedMobileCode = Convert.ToString(Session["GeneratedMobileCode"]);
            string GeneratedEmailCode = Convert.ToString(Session["GeneratedEmailCode"]);
            /*if (GeneratedMobileCode != ObjSignup.GeneratedMobileCode)
            {
                return Content("Invalid Mobile Verification Code");
            }
            else if (GeneratedEmailCode != ObjSignup.GeneratedEmailCode)
            {
                return Content("Invalid Email Verification Code");
            }
            else*/
            if (ModelState.IsValid)
            {
                Session["GeneratedMobileCode"] = null;
                Session["GeneratedEmailCode"] = null;

                UserRepository ObjUserRR = new UserRepository();
                ObjSignup.Name = ObjSignup.Name.Trim();
                ObjSignup.Email = ObjSignup.Email.Trim();
                ObjSignup.MobileNo = ObjSignup.MobileNo.Trim();
                ObjSignup.PanNo = ObjSignup.PanNo.Trim();
                ObjUserRR.AddEditUserWithId(ObjSignup);
                ModelState.Clear();
                return Json(ObjUserRR.DBResponse);
            }
            else
            {
                ObjSignup.Password = ObjSignup.HdnPassword;
                ObjSignup.ConfirmPassword = ObjSignup.ConfirmPassword;
                var Err = new { Status = -1 };
                return Json(Err);
            }
        }

        // Verification Code Generation
        public ActionResult GenerateCodeForMobile(string Mobile)
        {
            Random r = new Random();
            int MobileCode = r.Next(1000, 10000); //for ints
                                                  //int range = 100;
                                                  //double rDouble = r.NextDouble() * range;          

            Session["GeneratedMobileCode"] = MobileCode;
            SmsDataModel ObjSmsDataModel = new SmsDataModel();
            ObjSmsDataModel.MsgRecepient = Mobile.Trim();
            ObjSmsDataModel.MsgText = "Greetings, your one time password for phone verification is " + Convert.ToString(MobileCode) + " Please note this verification is for mobile number verification purpose in CWC-CFS.COM Portal CWC";
            // ObjSmsDataModel.MsgText = "TourRequest with TourID: "+ Convert.ToString(MobileCode) + ", has been Rejected. CWC";// "Your Verification Code is " + Convert.ToString(MobileCode);
            UtilityClasses.CommunicationManager.SendSMS(ObjSmsDataModel);
            var State = new { Status = 1, Msg = "" };
            return Json(State);

            // return Content("Your code is " + MobileCode);
        }

        public ActionResult GenerateCodeForEmail(string EmailId)
        {
            Random r = new Random();
            int EmailCode = r.Next(1000, 10000); //for ints
                                                 //int range = 100;
                                                 //double rDouble = r.NextDouble() * range;          

            Session["GeneratedEmailCode"] = EmailCode;
            //string EmailMsg = "Verification Code has been sent to your email sucessfully";
            EmailDataModel ObjEmailDataModel = new EmailDataModel();
            ObjEmailDataModel.ReceiverEmail = EmailId.Trim();
            ObjEmailDataModel.Subject = "Email Verification Code";
            ObjEmailDataModel.MailBody = "Your Verification Code is " + Convert.ToString(EmailCode);
            UtilityClasses.CommunicationManager.SendMail(ObjEmailDataModel);
            var State = new { Status = 1, Msg = "" };
            return Json(State);
            // return Content("Email Verification Code Send Successfully " + EmailCode);
        }
        #endregion

        #region Create External User Without ID

        // Without Id User view
        public ActionResult UserCreation()
        {
            SignUpWithoutId ObjSignUpWithoutId = new SignUpWithoutId();
            ObjSignUpWithoutId.DistrictList = new List<District>();
            DistrictRepository ObjDistrictRR = new DistrictRepository();
            ObjDistrictRR.GetDistrictAll();
            if (ObjDistrictRR.DBResponse != null)
            {
                ObjSignUpWithoutId.DistrictList = (IEnumerable<District>)ObjDistrictRR.DBResponse.Data;
            }
            return PartialView("SignupWithoutId", ObjSignUpWithoutId);
        }

        // Create user without id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserCreation(SignUpWithoutId ObjSignUpWithoutId)
        {
            string GeneratedMobileCode = Convert.ToString(Session["GeneratedMobileCode"]);
            string GeneratedEmailCode = Convert.ToString(Session["GeneratedEmailCode"]);
            /*if (GeneratedMobileCode != ObjSignUpWithoutId.GeneratedMobileCode)
            {
                return Content("Invalid Mobile Verification Code");
            }
            else if (GeneratedEmailCode != ObjSignUpWithoutId.GeneratedEmailCode)
            {
                return Content("Invalid Email Verification Code");
            }
            else */
            if (ModelState.IsValid)
            {
                Session["GeneratedCode"] = null;

                UserRepository ObjUserRR = new UserRepository();
                ObjSignUpWithoutId.Name = ObjSignUpWithoutId.Name.Trim();
                ObjSignUpWithoutId.Email = ObjSignUpWithoutId.Email.Trim();
                ObjSignUpWithoutId.MobileNo = ObjSignUpWithoutId.MobileNo.Trim();
                ObjSignUpWithoutId.PanNo = ObjSignUpWithoutId.PanNo.Trim();
                ObjSignUpWithoutId.Address = ObjSignUpWithoutId.Address.Trim();
                ObjSignUpWithoutId.PinCode = ObjSignUpWithoutId.PinCode.Trim();
                ObjUserRR.AddEditUserWithoutId(ObjSignUpWithoutId);
                ModelState.Clear();
                return Json(ObjUserRR.DBResponse);
            }
            else
            {
                ObjSignUpWithoutId.Password = ObjSignUpWithoutId.HdnPassword;
                ObjSignUpWithoutId.ConfirmPassword = ObjSignUpWithoutId.ConfirmPassword;
                var Err = new { Status = -1 };
                return Json(Err);
            }
        }
        #endregion

        #region Create CwcExim Forced User
        // Create Forced User View
        public ActionResult ForcedUserCreation()
        {
            WbdedForcedUser ObjWbdedForcedUser = new WbdedForcedUser();
            ObjWbdedForcedUser.DistrictList = new List<District>();
            DistrictRepository ObjDistrictRR = new DistrictRepository();
            ObjDistrictRR.GetDistrictAll();
            if (ObjDistrictRR.DBResponse != null)
            {
                ObjWbdedForcedUser.DistrictList = (IEnumerable<District>)ObjDistrictRR.DBResponse.Data;
            }
            return PartialView("CreateWbdedForcedUser", ObjWbdedForcedUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForcedUserCreation(WbdedForcedUser ObjWbdedForcedUser)
        {
            UserRepository ObjUserRR = new UserRepository();
            if (ModelState.IsValid)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjWbdedForcedUser.CreatedBy = ObjLogin.Uid;
                ObjUserRR.AddEditWbdedForcedUser(ObjWbdedForcedUser);
                ModelState.Clear();

            }
            return Json(ObjUserRR.DBResponse);
        }

        //CwcExim Forced User List 
        public ActionResult WbdedForcedUserList()
        {
            UserRepository ObjUserRR = new UserRepository();
            ObjUserRR.GetWbdedForcedUserList();
            IEnumerable<WbdedForcedUser> lstWbdedForcedUser = new List<WbdedForcedUser>();
            if (ObjUserRR.DBResponse.Data != null)
            {
                lstWbdedForcedUser = (IEnumerable<WbdedForcedUser>)ObjUserRR.DBResponse.Data;
            }

            return View(lstWbdedForcedUser);
        }

        // Edit CwcExim Forced User
        public ActionResult EditWbdedForcedUser(string LoginId)
        {
            WbdedForcedUser ObjWbdedForcedUser = new WbdedForcedUser();
            if (LoginId != null && LoginId != "")
            {
                UserRepository ObjUserRR = new UserRepository();
                ObjUserRR.GetWbdedForcedUserForEdit(LoginId);

                if (ObjUserRR.DBResponse != null)
                {
                    ObjWbdedForcedUser = (WbdedForcedUser)ObjUserRR.DBResponse.Data;
                    DistrictRepository ObjDistrictRR = new DistrictRepository();
                    ObjDistrictRR.GetDistrictAll();
                    if (ObjDistrictRR.DBResponse != null)
                    {
                        ObjWbdedForcedUser.DistrictList = (IEnumerable<District>)ObjDistrictRR.DBResponse.Data;
                    }
                }
            }
            return View("EditWbdedForcedUser", ObjWbdedForcedUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWBDEDForcedUser(WbdedForcedUser ObjWbdedForcedUser)
        {
            if (ModelState.IsValid)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjWbdedForcedUser.UpdatedBy = ObjLogin.Uid;

                UserRepository ObjUserRR = new UserRepository();
                ObjUserRR.AddEditWbdedForcedUser(ObjWbdedForcedUser);
                return Json(ObjUserRR.DBResponse);
            }
            else
            {
                var Err = new { Status = -1 };
                return Json(Err);
            }
        }


        #endregion

        #region Create CWC Kandla User
        public ActionResult Create()
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            Models.User ObjUser = new Models.User();
            ObjUser.LoginId = "";
            ObjUser.Password = "";
            ObjUser.ConfirmPassword = "";
            ObjUser.RoleList = new List<RoleMaster>();
            RoleMasterRepository ObjRoleRR = new RoleMasterRepository();
            ObjRoleRR.GetAllRolesForAssignments(Convert.ToInt32(ObjLogin.Role.RoleId));
            if (ObjRoleRR.DBResponse != null)
            {
                ObjUser.RoleList = (IEnumerable<RoleMaster>)ObjRoleRR.DBResponse.Data;
            }
            return PartialView("CreateWBDEDUser", ObjUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.User ObjUser)
        {
            if (ModelState.IsValid)
            {
                UserRepository ObjUserRR = new UserRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjUser.CreatedBy = ObjLogin.Uid;
                ObjUser.Name = ObjUser.Name.Trim();
                ObjUser.Email = ObjUser.Email.Trim();
                ObjUser.MobileNo = ObjUser.MobileNo.Trim();
                ObjUser.IsBlocked = false;
                ObjUserRR.AddEditUser(ObjUser);
                ModelState.Clear();
                return Json(ObjUserRR.DBResponse);
            }
            else
            {
                ObjUser.Password = ObjUser.HdnPassword;
                ObjUser.ConfirmPassword = ObjUser.ConfirmPassword;
                var Err = new { Status = -1 };
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult ViewUser(int Uid)
        {
            EditWBDEDUser ObjUser = new EditWBDEDUser();
            if (Uid > 0)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                UserRepository ObjUserRR = new UserRepository();
                ObjUserRR.GetUser(Uid);
                if (ObjUserRR.DBResponse != null)
                {
                    ObjUser = (EditWBDEDUser)ObjUserRR.DBResponse.Data;
                    /*ObjUser.RoleList = new List<RoleMaster>();
                    ObjUser.DesignationList = new List<DesignationMaster>();
                    RoleMasterRepository ObjRoleRR = new RoleMasterRepository();
                    ObjRoleRR.GetAllRoles();
                    ObjRoleRR.GetAllRolesForAssignments(Convert.ToInt32(ObjLogin.Role.RoleId));
                    if (ObjRoleRR.DBResponse != null)
                    {
                        ObjUser.RoleList = (IEnumerable<RoleMaster>)ObjRoleRR.DBResponse.Data;
                    }
                    DesignationRepository ObjDesigRR = new DesignationRepository();
                    ObjDesigRR.GetAllDesignation();
                    if (ObjDesigRR.DBResponse != null)
                    {
                        ObjUser.DesignationList = (IEnumerable<DesignationMaster>)ObjDesigRR.DBResponse.Data;
                    }*/
                }
            }

            return PartialView("ViewUser", ObjUser);
        }

        [HttpPost]
        public JsonResult DeleteUserDetail(int Uid)
        {
            if (Uid > 0)
            {
                UserRepository ObjUR = new UserRepository();
                ObjUR.DeleteUser(Uid);
                return Json(ObjUR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        #endregion

        #region UserList View
        public ActionResult UserList()
        {
            UserRepository ObjUserRR = new UserRepository();
            ObjUserRR.GetAllUser(-1);
            IEnumerable<User> lstUser = new List<User>();
            if (ObjUserRR.DBResponse.Data != null)
            {
                lstUser = (IEnumerable<User>)ObjUserRR.DBResponse.Data;
            }

            return PartialView(lstUser);
        }

        #endregion


        #region ExternalUserList
        public ActionResult ExternalUserList()
        {
            UserRepository ObjUserRR = new UserRepository();
            Login ObjLogin = (Login)Session["LoginUser"];
            ObjUserRR.GetAllseconsaryUser(ObjLogin.Uid);
            IEnumerable<SecondaryUser> lstUser = new List<SecondaryUser>();
            if (ObjUserRR.DBResponse.Data != null)
            {
                lstUser = (IEnumerable<SecondaryUser>)ObjUserRR.DBResponse.Data;
            }

            return PartialView(lstUser);
        }

        public ActionResult createsecondaryuser()
        {
            SecondaryUser ObjUser = new SecondaryUser();
            
                Login ObjLogin = (Login)Session["LoginUser"];

            ObjUser.Name = Convert.ToString(ObjLogin.LoginId);
            ObjUser.CreatedBy = Convert.ToInt32(ObjLogin.Uid);
            ObjUser.Uid = 0;


            return PartialView(ObjUser);
        }



        public ActionResult EditSecondaryuser(int Uid)
        {
            SecondaryUser ObjUser = new SecondaryUser();

            if (Uid > 0)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
               
                UserRepository ObjUserRR = new UserRepository();
                ObjUserRR.GetSecondaryUserEDIT(Uid);
                if (ObjUserRR.DBResponse != null)
                {
                    ObjUser = (SecondaryUser)ObjUserRR.DBResponse.Data;
                    ObjUser.Name = Convert.ToString(ObjLogin.LoginId);
                    ObjUser.RoleList = new List<RoleMaster>();
                    //  ObjUser.DesignationList = new List<DesignationMaster>();
                    
                    /*DesignationRepository ObjDesigRR = new DesignationRepository();
                    ObjDesigRR.GetAllDesignation();
                    if (ObjDesigRR.DBResponse != null)
                    {
                        ObjUser.DesignationList = (IEnumerable<DesignationMaster>)ObjDesigRR.DBResponse.Data;
                    }*/
                }
            }

            return PartialView(ObjUser);
        }


        public ActionResult ViewSecondaryuser(int Uid)
        {
            SecondaryUser ObjUser = new SecondaryUser();

            if (Uid > 0)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                UserRepository ObjUserRR = new UserRepository();
                ObjUserRR.GetSecondaryUserEDIT(Uid);
              //  ObjUser.Name = Convert.ToString(ObjLogin.LoginId);
                if (ObjUserRR.DBResponse != null)
                {
                    ObjUser = (SecondaryUser)ObjUserRR.DBResponse.Data;
                    ObjUser.Name = Convert.ToString(ObjLogin.LoginId);
                    ObjUser.RoleList = new List<RoleMaster>();
                    //  ObjUser.DesignationList = new List<DesignationMaster>();

                    /*DesignationRepository ObjDesigRR = new DesignationRepository();
                    ObjDesigRR.GetAllDesignation();
                    if (ObjDesigRR.DBResponse != null)
                    {
                        ObjUser.DesignationList = (IEnumerable<DesignationMaster>)ObjDesigRR.DBResponse.Data;
                    }*/
                }
            }

            return PartialView(ObjUser);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditExtarnalUser(SecondaryUser ObjUser)
        {
            ModelState.Remove("LoginId");
            if (ModelState.IsValid)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
               // ObjUser.Uid = ObjLogin.Uid;
                ObjUser.CreatedBy = ObjLogin.Uid;
                ObjUser.Name = ObjLogin.Name.Trim();
                ObjUser.EximTraderId = ObjLogin.EximTraderId;
                UserRepository ObjUserRR = new UserRepository();
                ObjUserRR.AddEditExtarnalUser(ObjUser);
                return Json(ObjUserRR.DBResponse);
            }
            else
            {
                var Err = new { Status = -1 };
                return Json(Err);
            }
        }

        #endregion

        #region Edit CWC Kandla User 
        public ActionResult EditWBDEDUser(int Uid)
        {
            EditWBDEDUser ObjUser = new EditWBDEDUser();
            if (Uid > 0)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                UserRepository ObjUserRR = new UserRepository();
                ObjUserRR.GetUser(Uid);
                if (ObjUserRR.DBResponse != null)
                {
                    ObjUser = (EditWBDEDUser)ObjUserRR.DBResponse.Data;
                    ObjUser.RoleList = new List<RoleMaster>();
                    //  ObjUser.DesignationList = new List<DesignationMaster>();
                    RoleMasterRepository ObjRoleRR = new RoleMasterRepository();
                    ObjRoleRR.GetAllRoles();
                    ObjRoleRR.GetAllRolesForAssignments(Convert.ToInt32(ObjLogin.Role.RoleId));
                    if (ObjRoleRR.DBResponse != null)
                    {
                        ObjUser.RoleList = (IEnumerable<RoleMaster>)ObjRoleRR.DBResponse.Data;
                    }
                    /*DesignationRepository ObjDesigRR = new DesignationRepository();
                    ObjDesigRR.GetAllDesignation();
                    if (ObjDesigRR.DBResponse != null)
                    {
                        ObjUser.DesignationList = (IEnumerable<DesignationMaster>)ObjDesigRR.DBResponse.Data;
                    }*/
                }
            }

            return PartialView(ObjUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWBDEDUser(EditWBDEDUser ObjUser)
        {
            ModelState.Remove("LoginId");
            if (ModelState.IsValid)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjUser.UpdatedBy = ObjLogin.Uid;
                ObjUser.Name = ObjUser.Name.Trim();
                UserRepository ObjUserRR = new UserRepository();
                ObjUserRR.AddEditUser(ObjUser);
                return Json(ObjUserRR.DBResponse);
            }
            else
            {
                var Err = new { Status = -1 };
                return Json(Err);
            }
        }

        #endregion

        #region Forgot Password
        // Forgot Password View
        public ActionResult ForgotPassword()
        {
            ForgotPassword ObjForgotPassword = new ForgotPassword();
            return PartialView("ForgotPassword", ObjForgotPassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassword ObjForgotPassword)
        {

            UserRepository ObjUserRR = new UserRepository();
            // ModelState.Remove("LoginId");
            if (ModelState.IsValid)
            {
                var NewPassword = GeneratePassword();
                var EncryptNewPassword = CryptorEngine.getMd5Hash(NewPassword);
                var OptText = ObjForgotPassword.OptType.Trim() == "OptLoginid" ? ObjForgotPassword.LoginId : ObjForgotPassword.OptType.Trim() == "OptMobile" ? ObjForgotPassword.MobileNo : ObjForgotPassword.Email;
                ObjUserRR.ForogtPassword(OptText, ObjForgotPassword.OptType, EncryptNewPassword);
                //ObjForgotPassword = (ForgotPassword)ObjUserRR.DBResponse.Data;

                SmsDataModel ObjSmsDataModel = new SmsDataModel();
                EmailDataModel ObjEmailDataModel = new EmailDataModel();
                if (ObjUserRR.DBResponse.Data != null && ObjForgotPassword.OptType.Trim() == "OptLoginid")
                {
                    string MobileEmail = Convert.ToString(ObjUserRR.DBResponse.Data);
                    var Mobile = Convert.ToString(MobileEmail.Split('-')[0]);
                    var Email = Convert.ToString(MobileEmail.Split('-')[1]);
                    ObjEmailDataModel.ReceiverEmail = Convert.ToString(Email);
                    ObjEmailDataModel.Subject = "New Password";
                    ObjEmailDataModel.MailBody = "Your Password is " + Convert.ToString(NewPassword);
                    UtilityClasses.CommunicationManager.SendMail(ObjEmailDataModel);
                    ObjSmsDataModel.MsgRecepient = Convert.ToString(Mobile);
                    ObjSmsDataModel.MsgText = "Your Password is " + Convert.ToString(NewPassword);
                    UtilityClasses.CommunicationManager.SendSMS(ObjSmsDataModel);

                }
                else if (ObjUserRR.DBResponse.Data != null && ObjForgotPassword.OptType.Trim() == "OptMobile")
                {
                    string MobileLoginId = Convert.ToString(ObjUserRR.DBResponse.Data);
                    var Mobile = Convert.ToString(MobileLoginId.Split('-')[0]);
                    var LoginId = Convert.ToString(MobileLoginId.Split('-')[1]);
                    ObjSmsDataModel.MsgRecepient = ObjForgotPassword.MobileNo.Trim();
                    //ObjSmsDataModel.MsgText = "Password For User Id is " + Convert.ToString(LoginId) + " is " + Convert.ToString(NewPassword);
                    //Password has been reset successfully. New Password is BBBBBBBB  CWCMSG
                    string msgText = "Password has been reset successfully. New Password is "+ NewPassword+" CWCMSG";
                    ObjSmsDataModel.MsgText = msgText;
                    UtilityClasses.CommunicationManager.SendSMS(ObjSmsDataModel);

                }
                else if (ObjUserRR.DBResponse.Data != null && ObjForgotPassword.OptType.Trim() == "OptEmail")
                {
                    string EmailLoginId = Convert.ToString(ObjUserRR.DBResponse.Data);
                    var LoginId = Convert.ToString(EmailLoginId.Split('-')[0]);
                    var Email = Convert.ToString(EmailLoginId.Split('-')[1]);
                    ObjEmailDataModel.ReceiverEmail = ObjForgotPassword.Email.Trim();
                    ObjEmailDataModel.Subject = "New Password";
                    ObjEmailDataModel.MailBody = "Password For User Id is " + Convert.ToString(LoginId) + " is " + Convert.ToString(NewPassword);
                    UtilityClasses.CommunicationManager.SendMail(ObjEmailDataModel);
                }

                ModelState.Clear();
                return Json(ObjUserRR.DBResponse);
            }
            else
            {
                var Err = new { Status = -1 };
                return Json(Err);
            }

        }

        // Generate Passowrd
        private string GeneratePassword()
        {
            Random rand = new Random();
            string Caps = "ABCDE";
            string Short = "fghij";
            string SpChars = "@";
            string Digits = "7298";

            var Result = new string(Enumerable.Repeat(Caps, 3).Select(s => s[rand.Next(Caps.Length)]).ToArray());
            Result += new string(Enumerable.Repeat(Short, 2).Select(s => s[rand.Next(Short.Length)]).ToArray());
            Result += new string(Enumerable.Repeat(SpChars, 1).Select(s => s[rand.Next(SpChars.Length)]).ToArray());
            Result += new string(Enumerable.Repeat(Digits, 2).Select(s => s[rand.Next(Digits.Length)]).ToArray());

            return (Result);

        }
        #endregion

        #region Change Password
        public ActionResult ChangePassword()
        {
            ChangePassword ObjChangePassword = new ChangePassword();
            return PartialView("ChangePassword", ObjChangePassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword ObjChangePassword)
        {
            UserRepository ObjUserRR = new UserRepository();
            if (ModelState.IsValid)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjChangePassword.Uid = ObjLogin.Uid;
                ObjUserRR.ChangePassword(ObjChangePassword);
                ModelState.Clear();
                if (ObjUserRR.DBResponse.Status == 1)
                    Session.Abandon();
                /*return RedirectToAction("LogOff");*/
                return Json(ObjUserRR.DBResponse);
            }
            else
            {
                var Err = new { Status = -1 };
                return Json(Err);
            }

        }
        #endregion

        #region Edit Profile, Mobile And Email
        public ActionResult EditProfile()
        {
            Login ObjLogin = (Login)Session["LoginUser"];

            EditProfile ObjEditProfile = new EditProfile();
            if (ObjLogin.Uid > 0)
            {
                UserRepository ObjUserRR = new UserRepository();
                ObjUserRR.GetUserDetail(ObjLogin.Uid);

                if (ObjUserRR.DBResponse != null)
                {
                    ObjEditProfile = (EditProfile)ObjUserRR.DBResponse.Data;

                }
            }
            return PartialView("EditProfile", ObjEditProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(EditProfile ObjEditProfile)
        {

            ModelState.Remove("Email");
            if (ModelState.IsValid)
            {
                Login ObjLogin = (Login)Session["LoginUser"];

                ObjEditProfile.Name = ObjEditProfile.Name.Trim();
                ObjEditProfile.MobileNo = ObjEditProfile.MobileNo.Trim();
                ObjEditProfile.Email = ObjEditProfile.Email;
                ObjEditProfile.Address = ObjEditProfile.Address;
                ObjEditProfile.UpdatedBy = ObjLogin.Uid;
                UserRepository ObjUserRR = new UserRepository();
                ObjUserRR.UpdateUserProfile(ObjEditProfile);
                ModelState.Clear();
                return Json(ObjUserRR.DBResponse);
            }
            else
            {
                var Err = new { Status = -1 };
                return Json(Err);
            }
        }


        #region Get Mobile And Email Verification Code for EDIT Profile

        [HttpGet]
        public JsonResult GetMobileVerificationCode()
        {
            string Message = "";

            if (Session["GeneratedMobileCode"] != null)
            {
                string MobileVerificationCode = Convert.ToString(Session["GeneratedMobileCode"]);
                return Json(MobileVerificationCode, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Message = "Invalid Code";
                var Err = new { Status = 0, Msg = Message };
                return Json(Err);
            }

        }

        [HttpGet]
        public JsonResult GetEmailVerificationCode()
        {
            string Message = "";

            if (Session["GeneratedEmailCode"] != null)
            {
                string EmailVerificationCode = Convert.ToString(Session["GeneratedEmailCode"]);
                return Json(EmailVerificationCode, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Message = "Invalid Code";
                var Err = new { Status = 0, Msg = Message };
                return Json(Err);
            }

        }
        #endregion

        #endregion

        #region Unlock User
        public ActionResult UnlockUser()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnlockUser(ForgotPassword ObjChangePassword)
        {
            UserRepository ObjUserRR = new UserRepository();
            if (ModelState.IsValid)
            {
               
                ObjUserRR.UnlockUser(ObjChangePassword.Name);
              
                return Json(ObjUserRR.DBResponse);
            }
            else
            {
                var Err = new { Status = -1 };
                return Json(Err);
            }

        }
        #endregion

        #region  Unlock User Id Status
        public ActionResult UnlockUserIdStatus(string date = "")
        {
            UserRepository ObjETR = new UserRepository();
            UserStatus ObjCBT = new UserStatus();
            //  ObjETR.GetGatePassLst(date);
            List<MasterUserIdList> Lstuserid = new List<MasterUserIdList>();
            ObjETR.GetUserIdList();
            if (ObjETR.DBResponse.Data != null)
            {

                Lstuserid = (List<MasterUserIdList>)ObjETR.DBResponse.Data;
                ViewBag.Lstuserid = Lstuserid;
            }
            else
            {
                ViewBag.Lstuserid = null;
            }

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUserId(UserStatus objETG)
        {
            if (ModelState.IsValid)
            {
                UserRepository objETGR = new UserRepository();

                objETGR.UpdateUserId(objETG, ((Login)(Session["LoginUser"])).Uid);

                ModelState.Clear();
                return Json(objETGR.DBResponse);


            }
            else
            {
                string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
                var Err = new { Statua = -1, Messgae = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult GetAllUserIdstatus()
        {
            UserRepository ETGR = new UserRepository();
            ETGR.GetAllUserIdstatus();
            List<UserStatus> Lstuserid = new List<UserStatus>();

            if (ETGR.DBResponse.Data != null)
            {
                Lstuserid = (List<UserStatus>)ETGR.DBResponse.Data;
            }
            return PartialView("MasterUserId", Lstuserid);

        }

        #endregion

    }
}