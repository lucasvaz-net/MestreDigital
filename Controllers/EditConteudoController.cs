using MestreDigital.DAL;
using MestreDigital.Data;
using MestreDigital.Filters;
using MestreDigital.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MestreDigital.Controllers
{
    [ServiceFilter(typeof(TokenAuthorizeAttribute))]
    public class EditConteudoController : Controller
    {
        private readonly ConteudoDAL _conteudoDAL;
        private readonly CategoriaDAL _categoriaDAL;
        private readonly SubcategoriaDal _subcategoriaDAL;
        private readonly UserTokenDal _userTokenDal;
        public EditConteudoController(ConteudoDAL conteudoDAL, CategoriaDAL categoriaDAL, SubcategoriaDal subcategoriaDAL, UserTokenDal userTokenDal)
        {
            _conteudoDAL = conteudoDAL;
            _categoriaDAL = categoriaDAL;
            _subcategoriaDAL = subcategoriaDAL;
            _userTokenDal = userTokenDal;
        }


        public IActionResult Index()
        {
            var conteudos = _conteudoDAL.GetConteudo();
            return View(conteudos);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var subcategorias = _subcategoriaDAL.GetSubcategorias();
            ViewBag.Subcategorias = new SelectList(subcategorias, "SubcategoriaID", "Nome");
            var conteudo = _conteudoDAL.GetConteudoById(id);

            if (conteudo == null)
            {
                return NotFound();
            }

            return View(conteudo);
        }

        [HttpPost]
        public IActionResult Edit(Conteudo conteudo, string actionType = "U")
        {

            _conteudoDAL.UpsertConteudo(conteudo, actionType);
            return RedirectToAction("Index");


        }



        [HttpGet]
        public IActionResult Create()
        {
            var subcategorias = _subcategoriaDAL.GetSubcategorias();
            ViewBag.Subcategorias = new SelectList(subcategorias, "SubcategoriaID", "Nome");
            return View(new Conteudo());
        }

        [HttpPost]
        public IActionResult Create(Conteudo conteudo, string actionType = "I")
        {

            _conteudoDAL.UpsertConteudo(conteudo, actionType);
            return RedirectToAction("Index");

        }


    }
}