﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using ResourceMetadata.Model;
using ResourceMetadata.Service;
using ResourceMetadata.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ResourceMetadata.API.Controllers
{
    public class AccountController : ApiController
    {

        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        public AccountController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;

            //Todo: This needs to be moved from here.
            this.userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };
        }

        [HttpPost]
        [OverrideAuthorization]
        public async Task<IHttpActionResult> Post(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = new ApplicationUser();
                    Mapper.Map(viewModel, user);

                    var identityResult = await userManager.CreateAsync(user, viewModel.Password);

                    if (identityResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user.Id, "Member");
                        return Ok();
                    }
                    else
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError(error,error);
                        }

                        return BadRequest(ModelState);
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        #region profile methods
        public IHttpActionResult GetProfile()
        {
            try
            {
                string userEmail = RequestContext.Principal.Identity.Name;
                var user = userManager.FindByName(userEmail);

                var model = new UserViewModel();

                Mapper.Map(user, model);

                return Ok(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }

        [HttpPost]
        [Route("api/Account/UpdateProfile")]
        public IHttpActionResult UpdateProfile(UserViewModel model)
        {

            string userEmail = RequestContext.Principal.Identity.Name;
            var user = userManager.FindByName(userEmail);
            Mapper.Map(model, user);

            userManager.Update(user);
            return Ok();
            
        }




        [HttpPost]
        [Route("api/Account/ChangeAvatar")]
        public IHttpActionResult ChangeAvatar(ChangeAvatarModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string userEmail = RequestContext.Principal.Identity.Name;
                    var user = userManager.FindByName(userEmail);
                    user.Avatar = model.Avatar;

                    userManager.Update(user);
                    return Ok();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            else
            {
                return BadRequest("Please reupload your avatar");
            }
        } 


        [HttpPost]
        [Route("api/Account/ChangeAvatar/{avatar}")]
        public IHttpActionResult ChangeAvatar(string avatar)
        {
            if (!String.IsNullOrEmpty(avatar))
            {
                try
                {
                    string userEmail = RequestContext.Principal.Identity.Name;
                    var user = userManager.FindByName(userEmail);
                    user.Avatar = avatar;

                    userManager.Update(user);
                    return Ok();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            else
            {
                return BadRequest("Please reupload your avatar");
            }

        } 
        #endregion

        #region Private methods
        #region SignInAsync
        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            try
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #endregion SignInAsync
        #endregion SignInAsync
    }
}