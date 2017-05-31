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

        //Verifica se as credenciais utilizadas sao validas
        private bool Autenticar(string usuario,string senha)
        {
            return (usuarios.Exists(c => c.nome == usuario && c.senha == senha));

        }

        //Acha o objeto do usuario
        private Usuario EncotnrarUsuario(string usuario)
        {
            return usuarios.FirstOrDefault(c => c.nome == usuario);
        }

        //Verifica credenciais do admin
        private bool AutenticarAdmin(string usuario,string senha)
        {
            if (usuario == "rsinohara" && senha == "asdf")
            {
                return true;
            }

            return false;
        }


        //Faz login, retornando o estado
        public ActionResult Login(string usuario,string senha)
        {

            //Credenciais do admin?
            if (AutenticarAdmin(usuario,senha))
            {
                return Content("Admin");
            }
            else if (Autenticar(usuario, senha))
            {
                //Credenciais válidas, associar um dispositivo e enviar lista de usuarios
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

        //Criar novo usuario
        public ActionResult Novo(string adminUsuario, string adminSenha,string novoNome, string novoSenha)
        {
            //Credenciais de admin?
            if (!AutenticarAdmin(adminUsuario, adminSenha))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            if (usuarios.Exists(c => c.nome == novoNome))
            {
                //Deu nao, já tem usuario com esse nome
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }

            if (grupos.Exists(c => c.nome == novoNome))
            {
                //Tambem nao pode, tem um grupo com esse nome.... mas mensagens iam ficar confusas
                //(Destinatario tem que ser unico)
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }

            var novoUsuario = new Usuario(novoNome, novoSenha);
            usuarios.Add(novoUsuario);

            //Deu certo véi, vai na fé
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        //Receber lista de usuarios. Envia a lista de usuarios, com todos os usuarios que estao na lista de usuarios.
        //Usuarios.
        public ActionResult Usuarios(string usuario, string senha)
        {
            if (!Autenticar(usuario, senha))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return Json(usuarios.Select(c => c.nome));
        }
        
        //Enviar mensagem para um unico destinatario (nao grupo)
        public ActionResult EnviarUnico(string mensagem, string remetente,string destinatario,string usuario, string senha)
        {
            if (!Autenticar(usuario, senha))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var msg = new Mensagem(mensagem, remetente, destinatario);

            var usuarioDestino = EncotnrarUsuario(destinatario);
            var usuarioRemetente = EncotnrarUsuario(remetente);

            //Xii, nao achou usuario.
            if(usuarioDestino==null || usuarioRemetente == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            }

            //Fechou, todo mundo recebe as mensagens
            usuarioRemetente.ReceberMensagem(msg);
            usuarioDestino.ReceberMensagem(msg);

            return new HttpStatusCodeResult(HttpStatusCode.OK);


        }


        //Dispositivo quer receber mensagens
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

            //Pegar mensagem do dispositivo e enviar pro cliente.
            var mensagens = disp.MensagensNaFila();
            var result = Json(mensagens);
            return result;
        }

        //Cliente envia ack para o servidor
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

            //Preparar lista de ack e enviar para o objeto dispositivo
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

            //Se o grupo existir mas foi fechado, cancelar ingresso ou criacao
            if (grupos.Exists(c => c.nome == nomeGrupo && c.fechado))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);

            }


            Grupo grupo;
            if (grupos.Exists(c => c.nome == nomeGrupo && !c.fechado))
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

            if (grupo.usuarios.Exists(c => c.nome == usuarioAtual.nome))
            {
                grupo.usuarios.Remove(usuarioAtual);
                if (grupo.usuarios.Count == 0)
                {
                    grupo.fechado = true;
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);

        }

        public ActionResult EnviarMensagemGrupo(string usuario,string senha,string destinatario,string mensagem)
        {
            if (!Autenticar(usuario, senha))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var grupo = grupos.FirstOrDefault(c => c.nome == destinatario && !c.fechado);

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
            foreach(var grupo in grupos.Where(c=>!c.fechado))
            {
                resultado.Add(grupo.nome, grupo.usuarios.Exists(c => c.nome == usuario));
            }

            return Json(resultado);
        }

    }
}
