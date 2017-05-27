using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDCorpComm;
using SDCorpComm.Models;
using System.Collections.Generic;

namespace SDCorpComm.Tests.Controllers
{
    [TestClass]
    public class UserTest
    {

        private Mensagem mensagem;
        private Mensagem mensagem2;


        [TestInitialize]    
        public void Init()
        {
            mensagem = new Mensagem(new List<int> { 0, 0, 0 }, "Mensagem", 0);
            mensagem2 = new Mensagem(new List<int> { 1, 0, 0 }, "Mensagem 2", 0);


        }

        [TestMethod]
        public void NovoUsuario_TemNomeFornecido()
        {
            var usuario = new Usuario("User 1","senha");

            var resultado = usuario.nome;

            Assert.AreEqual("User 1", resultado);

        }

        [TestMethod]
        public void NovoUsuario_TemID_MaiorQueOAnterior()
        {
            var usuario = new Usuario("User 1","senha");
            var usuario2 = new Usuario("User 2", "senha");

            var resultado = usuario.id;
            var resultado2 = usuario2.id;

            Assert.IsTrue(resultado >= 0);
            Assert.IsTrue(resultado2 == resultado + 1);

        }

        [TestMethod]
        public void Usuario_RecebeMensagemNaOrdem_ProcessaMensagem()
        {
            var usuario = new Usuario("User 1", "senha");

            usuario.ReceberMensagem(mensagem);

            Assert.AreEqual(1, usuario.mensagensProcessadas.Count);


        }

        [TestMethod]
        public void Usuario_RecebeMensagemForaDeOrdem_NaoProcessa()
        {
            var usuario = new Usuario("User 1", "senha");

            usuario.ReceberMensagem(mensagem2);

            Assert.AreEqual(0, usuario.mensagensProcessadas.Count);


        }

        [TestMethod]
        public void Usuario_RecebeMensagemForaDeOrdem_ProcessaNaOrdem()
        {
            var usuario = new Usuario("User 1", "senha");

            usuario.ReceberMensagem(mensagem2);

            Assert.AreEqual(0, usuario.mensagensProcessadas.Count);

            usuario.ReceberMensagem(mensagem);

            Assert.AreEqual(2, usuario.mensagensProcessadas.Count);

            Assert.AreEqual("Mensagem", usuario.mensagensProcessadas[0].mensagem);
            Assert.AreEqual("Mensagem 2", usuario.mensagensProcessadas[1].mensagem);


        }


    }
}
