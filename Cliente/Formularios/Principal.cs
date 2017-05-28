using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente.Formularios
{
    public partial class Principal : Form
    {

        private static readonly HttpClient client = new HttpClient();
        private string DestinatarioAtual = "";
        private bool EnviarParaGrupo = false;

        public List<Mensagem> mensagens = new List<Mensagem>();
        public List<string> acksEnviados = new List<string>();


        public Principal()
        {
            InitializeComponent();

            CriarBotoesUsuarios();
        }

        public void CriarBotoesUsuarios()
        {
            foreach (var usuario in Global.usuarios)
            {
                if (splitContainer1.Panel1.Controls.OfType<Button>().Where(c => c.Text == usuario).Count() == 0)
                {
                    var botao = new Button { Text = usuario, Dock = DockStyle.Top };
                    botao.Click += CliqueBotaoUsuario;
                    splitContainer1.Panel1.Controls.Add(botao);
                }
            }
        }

        public void CliqueBotaoUsuario(object sender, System.EventArgs e)
        {
            if (!(sender.GetType() == typeof(Button)))
            {
                return;
            }

            ResetarBotoes();

            var botao = (Button)sender;

            botao.BackColor = Color.AliceBlue;

            DestinatarioAtual = botao.Text;
            EnviarParaGrupo = false;

            ExibirHistorico();

        }

        private void ExibirHistorico()
        {
            txtHistorico.Clear();
            foreach (var mensagem in mensagens.Where(c =>
             c.remetente == DestinatarioAtual || (c.remetente == Global.UsuarioNome && c.destinatario == DestinatarioAtual)))
            {
                txtHistorico.Text += Environment.NewLine + mensagem.remetente + ":" + mensagem.mensagem;
            }
        }

        private void ResetarBotoes()
        {
            foreach (var botao in splitContainer1.Panel1.Controls.OfType<Button>())
            {
                botao.BackColor = SystemColors.ButtonFace;
            }
        }

        private async void SincronizarUsuarios()
        {
            string url = "http://localhost:48502/home/usuarios";

            var valores = new Dictionary<string, string>
            {
                {"usuario",Global.UsuarioNome },
                {"senha",Global.UsuarioSenha}
            };

            var conteudo = new FormUrlEncodedContent(valores);

            var resposta = await client.PostAsync(url, conteudo);

            if (resposta.IsSuccessStatusCode)
            {
                var dados = (await resposta.Content.ReadAsStringAsync());
                var usuariosJson = JArray.Parse(dados);
                Global.usuarios = new List<string>();
                foreach (var usuario in usuariosJson)
                {
                    Global.usuarios.Add(usuario.ToString());
                }

                CriarBotoesUsuarios();
            }
        }
        private async void LerMensagens()
        {
            string url = "http://localhost:48502/home/receberMensagens";

            var valores = new Dictionary<string, string>
            {
                {"dispositivo",Global.dispositivoID.ToString() }
            };

            var conteudo = new FormUrlEncodedContent(AdicionarCredenciais(valores));

            var resposta = await client.PostAsync(url, conteudo);

            if (resposta.IsSuccessStatusCode)
            {
                var mensagensJson = (await resposta.Content.ReadAsStringAsync());

                var novasMensagens = ConverterMensagens(mensagensJson);

                if (novasMensagens.Count == 0)
                {
                    return;
                }

                if(await EnviarAcks(mensagensJson))
                {
                    mensagens.AddRange(novasMensagens);

                }
                ExibirHistorico();
            }

        }

        private async Task<bool> EnviarAcks(string mensagensJson)
        {
            var acks = new List<string>();
            try
            {
                var dados = (JObject)JsonConvert.DeserializeObject(mensagensJson);
                foreach (var mensagem in dados)
                {
                    acks.Add(mensagem.Key);
                }


                string url = "http://localhost:48502/home/acks";

                var valores = new Dictionary<string, string>
            {
                {"dispositivo",Global.dispositivoID.ToString() },
                    {"acks",String.Join(",",acks) }
            };

                var conteudo = new FormUrlEncodedContent(AdicionarCredenciais(valores));

                var resposta = await client.PostAsync(url, conteudo);

                if (resposta.IsSuccessStatusCode)
                {
                    acksEnviados.AddRange(acks);
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch
            {
                return false;
            }
        }

        private List<Mensagem> ConverterMensagens(string mensagensJson)
        {
            var mensagens = new List<Mensagem>();

            try
            {
                var dados = (JObject)JsonConvert.DeserializeObject(mensagensJson);
                foreach (var mensagemJson in dados.Values())
                {
                    mensagens.Add(new Mensagem(mensagemJson["mensagem"].ToString(),
                        mensagemJson["remetente"].ToString(),
                        mensagemJson["destinatario"].ToString()));
                }





            }
            catch { }
            return mensagens;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SincronizarUsuarios();

            LerMensagens();

        }

        private Dictionary<string, string> AdicionarCredenciais(Dictionary<string, string> dicionario)
        {
            dicionario.Add("usuario", Global.UsuarioNome);
            dicionario.Add("senha", Global.UsuarioSenha);
            return dicionario;
        }

        private async void btnEnviar_Click(object sender, EventArgs e)
        {
            if (txtMensagem.Text != "" && DestinatarioAtual != "")
            {
                string url = "http://localhost:48502/home/enviarUnico";

                var mensagem = new Dictionary<string, string>{
                    { "mensagem",txtMensagem.Text },
                    {"remetente", Global.UsuarioNome },
                    {"destinatario", DestinatarioAtual } };



                var conteudo = new FormUrlEncodedContent(AdicionarCredenciais(mensagem));

                var resposta = await client.PostAsync(url, conteudo);

                if (resposta.IsSuccessStatusCode)
                {
                    MessageBox.Show("Mensagem enviada");
                }
            }
        }
    }
}
