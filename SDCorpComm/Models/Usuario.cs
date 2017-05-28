using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDCorpComm.Models
{
    public class Usuario
    {
        static int quantidade = 0;

        public List<Dispositivo> dispositivos { get; private set; } = new List<Dispositivo>();
        public List<Mensagem> mensagensProcessadas = new List<Mensagem>();

        public string senha { get; private set; }

        public string nome { get; private set; }
        public int id { get; private set; }

        public Usuario(string _nome, string _senha)
        {
            nome = _nome;
            senha = _senha;
            id = quantidade++;
        }


        private void ProcessarMensagem(Mensagem mensagem)
        {
            mensagensProcessadas.Add(mensagem);
            foreach (var dispositivo in dispositivos)
            {
                dispositivo.ProcessarMensagem(mensagem);

            }

        }

        public void AdicionarDispositivo(Dispositivo novoDispositivo)
        {
            dispositivos.Add(novoDispositivo);

            novoDispositivo.ProcessarMensagens(mensagensProcessadas);
        }


        public void ReceberMensagem(Mensagem mensagem)
        {
                ProcessarMensagem(mensagem);
        }

    }
}