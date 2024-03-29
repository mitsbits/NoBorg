﻿using Borg.Bookstore.Features.Users.Commands;
using Borg.Bookstore.Features.Users.Policies;
using Borg.Bookstore.Features.Users.Requests;
using Borg.Bookstore.Features.Users.ViewModels;
using Borg.Infra.Collections;
using Borg.Infra.DAL;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.Services.Breadcrumbs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Bookstore.Areas.Backoffice.Controllers
{
    [Route("[area]/Users")]//[Authorize(policy: BackofficePolicies.UserManagement)]
    public class UsersController : BackofficeController
    {
        public UsersController(ILoggerFactory loggerFactory, IMediator dispatcher) : base(loggerFactory, dispatcher)
        {
        }

        [HttpGet]
        [Route("{searchterm?}")]
        public async Task<IActionResult> Users([FromServices] RoleManager<IdentityRole> manager, string searchterm = "")
        {
            var searchMessage = string.IsNullOrWhiteSpace(searchterm) ? string.Empty : $"Search for: {searchterm}";
            SetPageTitle("Users", searchMessage);
            var roles = await manager.Roles.Select(x => x.Name).ToArrayAsync();
            ViewBag.Roles = roles;
            var result = await Dispatcher.Send(new UsersSearchRequest(searchterm ?? string.Empty));

            var device = PageDevice<Device>();
            device.Breadcrumbs.Add(new BreadcrumbLink("Back office", Url.Action("Home", "Home", new { area = "Backoffice" })));
            device.Breadcrumbs.Add(new BreadcrumbLink("Users", Url.Action("Users", "Users", new { area = "Backoffice" })));
            PageDevice(device);

            if (result.Succeded)
            {
                return View(result.Payload);
            }
            Logger.Debug("{controller} - {error}", GetType(), string.Join(", ", result.Errors));
            return View(new PagedResult<UserRowViewModel>(new UserRowViewModel[0], 1, 0, 0));
        }

        [HttpGet]
        [Route("email/{email}")]
        [Authorize(policy: BackofficePolicies.UserManagement)]
        public async Task<IActionResult> Item([FromServices] RoleManager<IdentityRole> manager, string email)
        {
            var result = await Dispatcher.Send(new UserRequest(email));
            if (result.Succeded)
            {
                var roles = await manager.Roles.Select(x => x.Name).ToArrayAsync();
                ViewBag.Roles = roles;

                var model = result.Payload;
                SetPageTitle(model.DisplayName, model.UserName.ToLower());

                var device = PageDevice<Device>();
                device.Breadcrumbs.Add(new BreadcrumbLink("Back office", Url.Action("Home", "Home", new { area = "Backoffice" })));
                device.Breadcrumbs.Add(new BreadcrumbLink("Users", Url.Action("Users", "Users")));
                device.Breadcrumbs.Add(new BreadcrumbLink(model.DisplayName, Url.Action("Item", "Users", new { email })));
                PageDevice(device);

                return View(result.Payload);
            }
            return RedirectToAction("Users");
        }

        [Route("ToggleLockOut")]
        [HttpPost]
        public async Task<IActionResult> ToggleLockOut(UserRowViewModel model, string redirecturl)
        {
            var result = await Dispatcher.Send(new ToggleLockOutCommand(model.UserName));
            if (!result.Succeded)
            {
                Logger.Debug($"{nameof(UsersController)} - {nameof(ToggleLockOut)} failed for {model.UserName} because {string.Join(", ", result.Errors)}");
            }
            return Redirect(redirecturl);
        }

        [Route("ToggleRole")]
        [HttpPost]
        public async Task<IActionResult> ToggleRole([FromServices] RoleManager<IdentityRole> rmanager, string userId, string role, string action)
        {
            var commandResult = await Dispatcher.Send(new ToggleRolesCommand(userId, role));
            if (!commandResult.Succeded) return BadRequest(commandResult.Errors);
            var userRoles = (commandResult as CommandResult<string[]>).Payload;

            ViewBag.Roles = await rmanager.Roles.OrderBy(x => x.Name).Select(x => x.Name).ToArrayAsync();

            var message = (userRoles.Any()) ? string.Join(", ", userRoles) : "---";
            var result = new { Message = message, Role = role, UserId = userId, Action = action };
            Logger.Debug("{@message}", result);
            return Ok(result);
        }

        [Route("ResetPassword")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                var result = await Dispatcher.Send(new ResetPasswordCommand(model.Email, model.Password));
                if (!result.Succeded)
                {
                    Logger.Debug($"{nameof(UsersController)} - {nameof(ResetPassword)} failed for {model.Email} because {string.Join(", ", result.Errors)}");
                }
            }

            return Redirect(redirecturl);
        }

        [Route("Profile")]
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileCommand model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                var result = await Dispatcher.Send(model);
                if (!result.Succeded)
                {
                    Logger.Debug($"{nameof(UsersController)} - {nameof(Profile)} failed for {model.Email} because {string.Join(", ", result.Errors)}");
                }
            }

            return Redirect(redirecturl);
        }

        [Route("UserClaim")]
        [HttpPost]
        public async Task<IActionResult> UserClaim(UserClaimCommand model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                var result = await Dispatcher.Send(model);
                if (!result.Succeded)
                {
                    AddErrors(result);
                    Logger.Debug($"{nameof(UsersController)} - {nameof(UserClaim)} failed for {model.Email} because {string.Join(", ", result.Errors)}");
                }
            }

            return Redirect(redirecturl);
        }

        [Route("UserAvatar")]
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UserAvatar(UserAvatarCommand model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                var result = await Dispatcher.Send(model);
                if (!result.Succeded)
                {
                    AddErrors(result);
                    Logger.Debug($"{nameof(UsersController)} - {nameof(UserAvatar)} failed for {model.Email} because {string.Join(", ", result.Errors)}");
                }
            }

            return Redirect(redirecturl);
        }

        [Route("DeleteUser")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(DeleteUserCommand model, string redirecturl)
        {
            if (ModelState.IsValid)
            {
                var result = await Dispatcher.Send(model);
                if (!result.Succeded)
                {
                    Logger.Debug($"{nameof(UsersController)} - {nameof(DeleteUser)} failed for {model.Email} because {string.Join(", ", result.Errors)}");
                }
            }

            return Redirect(redirecturl);
        }
    }
}