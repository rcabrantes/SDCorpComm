using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDCorpComm.Models
{
    public class Mensagem
    {
        private static int quantidade = 0;

        public int id;

        public List<int> relogio = new List<int>();
        public string mensagem;
        public int remetente;

        public Mensagem(List<int> _relogio,string _mensagem, int _remetente)
        {
            relogio = _relogio;
            mensagem = _mensagem;
            id = quantidade++;
            remetente = _remetente;
        }

        public bool MensagemPodeSerProcessada(List<int> relogioAtual)
        {
            for(int i=0;i<relogio.Count;i++)
            {
                if((relogioAtual.Count-1)<i && relogio[i] > 0)
                {
                    return false;
                }
                if (relogioAtual[i] < relogio[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}