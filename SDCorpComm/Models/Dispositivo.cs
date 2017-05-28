using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDCorpComm.Models
{
    public class Dispositivo
    {
        private static int quantidade=0;
        private int contadorMensagens = 0;

        public int id { get;private set;}
        public Usuario usuario { get; private set; }

        private Dictionary<int,Mensagem> filaMensagens = new Dictionary<int,Mensagem>();

        public Dispositivo(Usuario _usuario)
        {
            usuario = _usuario;
            id = quantidade++;
        }

        public void ProcessarMensagem(Mensagem mensagem)
        {
                filaMensagens.Add(contadorMensagens++,mensagem);
        }

        public void ProcessarMensagens(List<Mensagem> mensagens)
        {
            foreach(var msg in mensagens)
            {
                ProcessarMensagem(msg);
            }
        }

        public Dictionary<string,Mensagem> MensagensNaFila()
        {
            var mensagens = new Dictionary<string, Mensagem>();
            foreach(var key in filaMensagens.Keys)
            {
                mensagens.Add(key.ToString(), filaMensagens[key]);
            }

            return mensagens;
        }

        public void ProcessarAcks(IEnumerable<int> acks)
        {
            foreach(var id in acks)
            {
                if (filaMensagens.ContainsKey(id))
                {
                    filaMensagens.Remove(id);
                }
            }
        }
    }
}