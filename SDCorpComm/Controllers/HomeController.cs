using SDCorpComm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SDCorpComm.Controllers
{
    public class HomeController : Controller
    {
        public static List<Usuario> usuarios = new List<Usuario> { new Usuario("beca", "asdf"),new Usuario("rere","asdf") };

        // GET: Home
        public ActionResult Hello()
        {
            return Content("Hello back");
        }


        private bool Autenticar(string usuario,string senha)
        {
            return (usuarios.Exists(c => c.nome == usuario && c.senha == senha));

        }

        private Usuario EncotnrarUsuario(string usuario,string senha)
        {
            return usuarios.FirstOrDefault(c => c.nome == usuario && c.senha == senha);
        }


        private bool AutenticarAdmin(string usuario,string senha)
        {
            if (usuario == "rsinohara" && senha == "asdf")
            {
                return true;
            }

            return false;
        }
        // GET: Home/Details/5
        public ActionResult Login(string usuario,string senha)
        {
            if (AutenticarAdmin(usuario,senha))
            {
                return Content("Admin");
            }
            else if (Autenticar(usuario, senha))
            {
                var usuarioAtual = EncotnrarUsuario(usuario, senha);
                var dispositivo = new Dispositivo(usuarioAtual);

                usuarioAtual.AdicionarDispositivo(dispositivo);

                return Json(new {
                    dispositivo = dispositivo.id,
                    usuarios = usuarios.Select(c => c.nome)
                });
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                
            }
        }

        public ActionResult Novo(string adminUsuario, string adminSenha,string novoNome, string novoSenha)
        {
            if (!AutenticarAdmin(adminUsuario, adminSenha))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var novoUsuario = new Usuario(novoNome, novoSenha);
            usuarios.Add(novoUsuario);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
