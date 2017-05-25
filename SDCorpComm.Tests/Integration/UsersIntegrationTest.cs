using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDCorpComm.Models;
using System.Collections.Generic;

namespace SDCorpComm.Tests.Integration
{
    [TestClass]
    public class UsersIntegrationTest
    { 
        [TestMethod]
        public void Usuario_RecebeMensagem_EnviaParaDispositivo()
        {
            var usuario = new Usuario("Usuario 1");
            var dispositivo = new Dispositivo(usuario);

            usuario.AdicionarDispositivo(dispositivo);

            usuario.ReceberMensagem(new Mensagem(new List<int> { 0, 0, 0 }, "Mensagem 1", 0));

            Assert.AreEqual(1, dispositivo.MensagensNaFila().Count);
        }
    }
}
