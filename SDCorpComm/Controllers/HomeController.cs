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
        public static List<Grupo> grupos = new List<Grupo>();

        // GET: Home
        public ActionResult Hello()
        {
            return Content("Hello back");
        }


        private bool Autenticar(string usuario,string senha)
        {
            return (usuarios.Exists(c => c.nome == usuario && c.senha == senha));

        }

        private Usuario EncotnrarUsuario(string usuario)
        {
            return usuarios.FirstOrDefault(c => c.nome == usuario);
        }


        private bool AutenticarAdmin(string usuario,string senha)
        {
            if (usuario == "rsinohara" && senha == "asdf")
            {
                return true;
            }

            return false;
        }


        public ActionResult Login(string usuario,string senha)
        {
            if (AutenticarAdmin(usuario,senha))
            {
                return Content("Admin");
            }
            else if (Autenticar(usuario, senha))
            {
                var usuarioAtual = EncotnrarUsuario(usuario);
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



        public ActionResult Usuarios(string usuario, string senha)
        {
            if (!Autenticar(usuario, senha))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return Json(usuarios.Select(c => c.nome));
        }
        
        public ActionResult EnviarUnico(string mensagem, string remetente,string destinatario,string usuario, string senha)
        {
            if (!Autenticar(usuario, senha))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            var msg = new Mensagem(mensagem, remetente, destinatario);

            var usuarioDestino = EncotnrarUsuario(destinatario);
            var usuarioRemetente = EncotnrarUsuario(remetente);

            if(usuarioDestino==null || usuarioRemetente == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            }

            usuarioRemetente.ReceberMensagem(msg);
            usuarioDestino.ReceberMensagem(msg);

            return new HttpStatusCodeResult(HttpStatusCode.OK);


        }

        public ActionResult ReceberMensagens(string dispositivo,string usuario, string senha)
        {
            if (!Autenticar(usuario, senha))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var disp = EncotnrarUsuario(usuario).dispositivos.Where(c => c.id.ToString() == dispositivo).FirstOrDefault();

            if (disp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            }

            var mensagens = disp.MensagensNaFila();
            var result = Json(mensagens);
            return result;
        }

        public ActionResult Acks(string usuario, string senha, string dispositivo,string acks)
        {
            if (!Autenticar(usuario, senha))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var disp = EncotnrarUsuario(usuario).dispositivos.Where(c => c.id.ToString() == dispositivo).FirstOrDefault();

            if (disp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            }

            var ackLista = acks.Split(',').ToList();
            disp.ProcessarAcks(ackLista.Select(c=>int.Parse(c)));

            return new HttpStatusCodeResult(HttpStatusCode.OK);

        }

        public ActionResult IngressarGrupo(string usuario,string senha, string nomeGrupo)
        {
            if (!Autenticar(usuario, senha))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            Grupo grupo;
            if (grupos.Exists(c => c.nome == nomeGrupo))
            {
                grupo = grupos.FirstOrDefault(c => c.nome == nomeGrupo);
            } else
            {
                grupo = new Grupo { nome = nomeGrupo };
                grupos.Add(grupo);
            }

            var usuarioAtual = EncotnrarUsuario(usuario);

            if (!grupo.usuarios.Exists(c => c.nome == usuarioAtual.nome))
            {
                grupo.usuarios.Add(usuarioAtual);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);

        }

        public ActionResult SairGrupo(string usuario, string senha, string nomeGrupo)
        {
            if (!Autenticar(usuario, senha))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var grupo = grupos.FirstOrDefault(c => c.nome == nomeGrupo);

            if (grupo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);

            }

            var usuarioAtual = EncotnrarUsuario(usuario);

            if (!grupo.usuarios.Exists(c => c.nome == usuarioAtual.nome))
            {
                grupo.usuarios.Remove(usuarioAtual);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);

        }

        public ActionResult EnviarMensagemGrupo(string usuario,string senha,string destinatario,string mensagem)
        {
            if (!Autenticar(usuario, senha))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var grupo = grupos.FirstOrDefault(c => c.nome == destinatario);

            if (grupo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            }

            grupo.usuarios.ForEach(c => c.ReceberMensagem(new Mensagem(mensagem, usuario, destinatario, true)));

            return new HttpStatusCodeResult(HttpStatusCode.OK);

        }

        public ActionResult Grupos(string usuario,string senha)
        {
            if (!Autenticar(usuario, senha))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var resultado = new Dictionary<string, bool>();
            foreach(var grupo in grupos)
            {
                resultado.Add(grupo.nome, grupo.usuarios.Exists(c => c.nome == usuario));
            }

            return Json(resultado);
        }

    }
}
