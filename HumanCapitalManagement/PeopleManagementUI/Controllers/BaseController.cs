using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PeopleManagementUI.Services;
using System.IdentityModel.Tokens.Jwt;

namespace PeopleManagementUI.Controllers
{
    /// <summary>
    /// Base controller for shared functionality across controllers.
    /// </summary>
    public class BaseController : Controller
    {
        protected readonly IUserContextService _userContextService;

        public BaseController(IUserContextService userContextService)
        {
            _userContextService = userContextService;
        }

        /// <summary>
        /// Gets the current user's role from the JWT token.
        /// </summary>
        protected string GetUserRole() => _userContextService.GetUserRole();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["UserRole"] = GetUserRole();
            base.OnActionExecuting(context);
        }
    }
}
