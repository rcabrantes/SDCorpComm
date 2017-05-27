using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente.Formularios
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();

            CriarBotoesUsuarios();
        }

        public void CriarBotoesUsuarios()
        {
            foreach(var usuario in Global.usuarios)
            {
                if (usuario != Global.UsuarioNome && this.Controls.OfType<Button>().Where(c=>c.Text==usuario).Count()==0)
                {
                    splitContainer1.Panel1.Controls.Add(new Button { Text = usuario, Dock =  DockStyle.Top });
                }
            }
        }

        private async void SincronizarUsuarios()
        {

        }

        private async void LerMensagens()
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SincronizarUsuarios();

            LerMensagens();

        }
    }
}
