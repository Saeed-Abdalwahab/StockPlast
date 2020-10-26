using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PlasticStatic;

namespace StockPortal.Helpers
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] _allowedroles;

        public CustomAuthorizeAttribute(params string[] allowedroles)
        {
            _allowedroles = allowedroles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorize = false;

            var authorized = base.AuthorizeCore(httpContext);
            if (authorized)
            {

                var user = httpContext.User;
                if (user == null) return false;
                HttpCookie cookie = httpContext.Request.Cookies.Get("UserTypeId");

                if (cookie.Value == Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin))
                {
                    return true;
                }
              

                foreach (var role in _allowedroles)
                {
                    authorize = user.IsInRole(role);
                }
            }

            return authorize;

        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccessDenied" }));
            }
        }
    }
}