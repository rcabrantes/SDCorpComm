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
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
                var mensagem= await resposta.Content.ReadAsStringAsync();
                if (mensagem == "Admin")
                {
                    pnlAdmin.Visible = true;
                    pnlLogin.Visible = false;
                }
                else
                {
                    var dados = (JObject)JsonConvert.DeserializeObject(mensagem);
                    SalvarDados(dados);

                    var principal = new Principal();
                    principal.Visible = true;
                    principal.Show();
                    this.Hide();
                }
            }

        }

        public void SalvarDados(JObject dados)  
        {
            Global.dispositivoID = int.Parse(dados["dispositivo"].ToString());
            var usuariosJson = JArray.Parse(dados["usuarios"].ToString());
            Global.usuarios = new List<string>();
            foreach (var usuario in usuariosJson)
            {
                Global.usuarios.Add(usuario.ToString());
            }

        }

        private async void btnCriarUsuario_Click(object sender, EventArgs e)
        {
            string url = "http://localhost:48502/home/novo";

            if (txtAdminSenha.Text != txtAdminSenha2.Text)
            {
                MessageBox.Show("Senhas nao batem");
                return;
            }

            var valores = new Dictionary<string, string>
            {
                {"adminUsuario",txtNome.Text },
                {"adminSenha",txtSenha.Text },
                {"novoNome" ,txtAdminNome.Text},
                {"novoSenha",txtAdminSenha.Text }
            };

            var conteudo = new FormUrlEncodedContent(valores);


            var resposta = await client.PostAsync(url, conteudo);


            if (resposta.IsSuccessStatusCode)
            {
                pnlAdmin.Visible = false;
                pnlLogin.Visible = true;
            }
        }
    }
}
