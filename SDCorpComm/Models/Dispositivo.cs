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

        private Dictionary<int,string> filaMensagens = new Dictionary<int,string>();

        public Dispositivo(Usuario _usuario)
        {
            usuario = _usuario;
            id = quantidade++;
        }

        public void ProcessarMensagem(string mensagem)
        {
            filaMensagens.Add(contadorMensagens++,mensagem);
        }

        public void ProcessarMensagens(List<string> mensagens)
        {
            foreach(var msg in mensagens)
            {
                ProcessarMensagem(msg);
            }
        }

        public Dictionary<int,string> MensagensNaFila()
        {
            var mensagens = new Dictionary<int, string>();
            foreach(var key in filaMensagens.Keys)
            {
                mensagens.Add(key, filaMensagens[key]);
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