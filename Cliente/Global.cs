using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente
{
    public static class Global
    {
        public static string Domain { get; set; }
        public static string UsuarioSenha { get; set; }
        public static string UsuarioNome { get; set; }
        public static int dispositivoID { get; set; }

        public static List<string> usuarios { get; set; }
    }
}
