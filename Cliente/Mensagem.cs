using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente
{
    public class Mensagem
    {

        private static int quantidade = 0;

        public int id;

        public string mensagem;
        public string remetente;
        public string destinatario;
        public bool paraGrupo;

        public Mensagem(string _mensagem, string _remetente, string _destinatario, bool _paraGrupo = false)
        {
            mensagem = _mensagem;
            id = quantidade++;
            destinatario = _destinatario;
            paraGrupo = _paraGrupo;
            remetente = _remetente;
        }
    }
}
