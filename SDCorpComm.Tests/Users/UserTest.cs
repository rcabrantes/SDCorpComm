using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDCorpComm;
using SDCorpComm.Controllers;
using SDCorpComm.Models;

namespace SDCorpComm.Tests.Controllers
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void NovoUsuario_TemNomeFornecido()
        {
            var usuario = new Usuario("User 1");

            var resultado = usuario.nome;

            Assert.AreEqual("User 1", resultado);

        }

        [TestMethod]
        public void NovoUsuario_TemID_MaiorQueOAnterior()
        {
            var usuario = new Usuario("User 1");
            var usuario2 = new Usuario("User 2");

            var resultado = usuario.id;
            var resultado2 = usuario2.id;

            Assert.IsTrue(resultado >= 0);
            Assert.IsTrue(resultado2 == resultado + 1);

        }

        [TestMethod]
        public void Usuario_RecebeMensagem_ProcessaMensagem()
        {
            var usuario = new Usuario("User 1");

        }
    }
}
