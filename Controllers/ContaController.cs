using Microsoft.AspNetCore.Mvc;
using MestreDigital.Data;


namespace MestreDigital.Controllers
{
    public class ContaController : Controller
    {
        private readonly UsuarioDal _usuarioDal;
        private readonly UserTokenDal _userTokenDal;

        public ContaController(UsuarioDal usuarioDal, UserTokenDal userTokenDal)
        {
            _usuarioDal = usuarioDal;
            _userTokenDal =  userTokenDal;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string senha)  
        {
            var hashedPassword = _usuarioDal.GetHashedPasswordByEmail(email);

            if (senha == hashedPassword)
            {
                int? userId = _usuarioDal.CheckUserCredentials(email, hashedPassword);
                if (userId.HasValue)
                {
                    string token = _userTokenDal.CreateToken(userId.Value); 

                    CookieOptions option = new CookieOptions
                    {
                        Expires = DateTime.Now.AddHours(1),
                        HttpOnly = true,
                        Secure = true
                    };

                    Response.Cookies.Append("UserToken", token, option);

                    return RedirectToAction("Index", "EditConteudo");
                }
            }

            ModelState.AddModelError("", "Tentativa de login inválida.");
            return View();
        }

        public IActionResult Logout()
        {
            if (Request.Cookies["UserToken"] != null)
            {
                string token = Request.Cookies["UserToken"].ToString();
                if (_userTokenDal.ValidateToken(token))
                {
                    int? userId = _usuarioDal.GetUserIdByToken(token);
                    if (userId.HasValue)
                    {
                        _userTokenDal.LogoutUser(userId.Value);
                        Response.Cookies.Delete("UserToken");
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
