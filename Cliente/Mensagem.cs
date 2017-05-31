using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente
{
    public class Mensagem
    {


        public string id;

        public string mensagem;
        public string remetente;
        public string destinatario;
        public bool paraGrupo;

        public Mensagem(string _id, string _mensagem, string _remetente, string _destinatario, bool _paraGrupo = false)
        {
            mensagem = _mensagem;
            id = _id;
            destinatario = _destinatario;
            paraGrupo = _paraGrupo;
            remetente = _remetente;
        }
    }
}
