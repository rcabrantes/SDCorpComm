using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDCorpComm.Models;
using System.Linq;
using System.Collections.Generic;

namespace SDCorpComm.Tests.Unit
{
    [TestClass]
    public class TestesDispositivos
    {

        private Usuario user;
        int quantidade = 0;

        Mensagem mensagem;

        [TestInitialize]
        public void Init()
        {
            user= new Usuario("Usuario qualquer");
            mensagem = new Mensagem(new List<int> { 0, 0, 0 }, "Mensagem", 0);
        }

        [TestMethod]
        public void NovoDispositivo_CriadoCorretamente()
        {
            var disp = new Dispositivo(user);

            Assert.AreEqual(quantidade++, disp.id);
            Assert.AreEqual(user.nome, disp.usuario.nome);
            Assert.AreEqual(0, disp.MensagensNaFila().Count);

        }

        [TestMethod]
        public void Dispositivo_ColocaMensagemNaFila()
        {
            var disp = new Dispositivo(user);

            disp.ProcessarMensagem(mensagem);

            var fila = disp.MensagensNaFila();

            Assert.AreEqual(1, fila.Count);
            Assert.AreEqual("Mensagem", fila.First().Value.mensagem);
        }

        [TestMethod]
        public void Dispositivo_RecebeAck_RemoveDaFila()
        {
            var disp = new Dispositivo(user);

            disp.ProcessarMensagem(mensagem);

            var fila = disp.MensagensNaFila();

            disp.ProcessarAcks(fila.Keys);

            fila = disp.MensagensNaFila();

            Assert.AreEqual(0, fila.Count);
        }

        [TestMethod]
        public void Dispositivo_RecebeAck_NaoRemoveOutrasMensagens()
        {
            var disp = new Dispositivo(user);
            var mensagem2 = new Mensagem(new List<int> { 0, 0, 0 }, "Mensagem 2", 0);

            disp.ProcessarMensagem(mensagem);
            disp.ProcessarMensagem(mensagem2);

            var fila = disp.MensagensNaFila();

            disp.ProcessarAcks(new List<int> { fila.First().Key });

            fila = disp.MensagensNaFila();

            Assert.AreEqual(1, fila.Count);
            Assert.AreEqual("Mensagem 2", fila.First().Value.mensagem);

        }
    }
}
