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
        public List<Mensagem> mensagensNaoProcessadas = new List<Mensagem>();
        public List<string> mensagensProcessadas = new List<string>();
        public List<int> relogio = new List<int>();

        public string nome { get; private set; }
        public int id { get; private set; }

        public Usuario(string _nome)
        {
            nome = _nome;
            id = quantidade++;
        }

        private void ProcessarMensagem(Mensagem mensagem)
        {
            mensagensProcessadas.Add(mensagem.mensagem);
            relogio[mensagem.remetente]++;
            foreach(var dispositivo in dispositivos)
            {
                dispositivo.ProcessarMensagem(mensagem.mensagem);
                
            }

            ProcessarFila();
        }

        public void ProcessarFila()
        {

            foreach (var msg in mensagensNaoProcessadas.ToList())
            {
                if (msg.MensagemPodeSerProcessada(relogio))
                {
                    mensagensNaoProcessadas.Remove(msg);
                    ProcessarMensagem(msg);
                    break;
                }
            }
        }

        public void AdicionarDispositivo(Dispositivo novoDispositivo)
        {
            dispositivos.Add(novoDispositivo);

            novoDispositivo.ProcessarMensagens(mensagensProcessadas);

            relogio.Add(0);

        }

        public void ReceberMensagem(Mensagem mensagem)
        {
            if (mensagem.MensagemPodeSerProcessada(relogio))
            {
                ProcessarMensagem(mensagem);

            } else
            {
                mensagensNaoProcessadas.Add(mensagem);
            }
        }

    }
}