using SDCorpComm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDCorpComm.Controllers
{
    public class Grupo
    {
        public string nome { get; set; }

        public List<Usuario> usuarios { get; set; } = new List<Usuario>();
    }
}