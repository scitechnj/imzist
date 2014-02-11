﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Imzist.Data;
using Imzist.Logic;

namespace Imzist.Web.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            try
            {
                if (Membership.ValidateUser(username, password))
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                    return Redirect(returnUrl ?? "/");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string username, string password)
        {
            try
            {
                Membership.CreateUser(username, password);
                if (!Roles.RoleExists("User"))
                {
                    Roles.CreateRole("User");
                }
                Roles.AddUserToRole(username, "User");
                Emailer.SendEmail(username,"Welcome to Imzist","");
                
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult UserNameExists(string username)
        {
            return Json(new {exists = Membership.GetUser(username) != null});
        }
    }
}
