using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MestreDigital.Data;

namespace MestreDigital.Filters
{ 
public class TokenAuthorizeAttribute : ActionFilterAttribute
{
        private readonly UserTokenDal _tokenDal;

        public TokenAuthorizeAttribute(UserTokenDal tokenDal)
        {
            _tokenDal = tokenDal;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
    {
            
            var token = context.HttpContext.Request.Cookies["UserToken"]; 


        if (string.IsNullOrEmpty(token) || !_tokenDal.ValidateToken(token))
        {

            context.Result = new RedirectToActionResult("Login", "Conta", null); 
        }

        base.OnActionExecuting(context);
    }
}

}