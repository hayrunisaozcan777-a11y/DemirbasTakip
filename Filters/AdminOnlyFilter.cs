using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemirbasTakip.Filters
{
    public class AdminOnlyFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var rol = context.HttpContext.Session.GetString("KullaniciRol");
            if (rol != "Admin")
            {
                context.Result = new RedirectToActionResult("ErisimEngellendi", "Login", null);
            }
        }
    }
}