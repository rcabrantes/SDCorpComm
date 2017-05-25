using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente.Formularios
{
    public partial class Inicio : Form
    {
        private static readonly HttpClient client = new HttpClient();


        public Inicio()
        {
            InitializeComponent();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            string url = "http://localhost:48502/home/hello";

            var request = WebRequest.Create(url);
            string result;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }


            }
            catch
            {
                //Exibir mensagem de erro e alteracao de IP

                return;
            }

            pnlLogin.Visible = true;


        }

        private async void btnConectar_Click(object sender, EventArgs e)
        {
            string url = "http://localhost:48502/home/login";

            var valores = new Dictionary<string, string>
            {
                {"usuario",txtNome.Text },
                {"senha",txtSenha.Text }
            };

            var conteudo = new FormUrlEncodedContent(valores);

            var resposta = await client.PostAsync(url, conteudo);

            if (resposta.IsSuccessStatusCode)
            {
                var principal = new Principal();
                principal.Visible = true;
                principal.Show();
                this.Close();
            }

        }
    }
}
