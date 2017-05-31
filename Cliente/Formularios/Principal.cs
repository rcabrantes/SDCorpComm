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
        //Destinatario para o qual as mensagens serao enviadas. Pode ser um grupo
        private string _destinatarioAtual="";

        //Flag se o destinatario é um grupo
        private bool EnviarParaGrupo = false;

        //Cliente HTTP para todos os requests
        private static readonly HttpClient client = new HttpClient();

        //Set e get para destinatario atual, simplesmtente para exibirmos no topo do chat
        private string DestinatarioAtual
        {
            get
            {
                return _destinatarioAtual;
            }
            set
            {
                _destinatarioAtual = value;
                lblDestinatario.Text = "Exibindo: " + (_destinatarioAtual == "" ? "nenhum." : _destinatarioAtual);
            }
        }

        //Mnsaagens armazenadas
        public List<Mensagem> mensagens = new List<Mensagem>();

        //Acks enviados, assim evitamos receber mensagens repetidas.
        public List<string> acksEnviados = new List<string>();

        //Informacoes sobre os grupos: <nome,ingressou no grupo>
        public Dictionary<string, bool> dadosGrupo = new Dictionary<string, bool>();


        public Principal()
        {
            InitializeComponent();

            CriarBotoesUsuarios();
        }


        //Cria botoes para os usuarios, baseado na lista de usuarios recebida pelo servidor
        public void CriarBotoesUsuarios()
        {
            foreach (var usuario in Global.usuarios)
            {
                //Verifica se já existe um botao para esse usuario
                if (splitContainer1.Panel1.Controls.OfType<Button>().Where(c => c.Text == usuario).Count() == 0)
                {
                    //Se nao existe, criar botao e associar clique
                    var botao = new Button { Text = usuario, Dock = DockStyle.Top };
                    botao.Click += CliqueBotaoUsuario;
                    splitContainer1.Panel1.Controls.Add(botao);
                }
            }
        }


        public void CliqueBotaoUsuario(object sender, System.EventArgs e)
        {
            //Verificar se o clique foi feito em um botao
            if (!(sender.GetType() == typeof(Button)))
            {
                return;
            }

            //Limpa cores dos demais botoes, só esse vai ficar colorido
            ResetarBotoes();

            //Converter o tipo para alterar as propriedades
            var botao = (Button)sender;

            botao.BackColor = Color.AliceBlue;

            
            DestinatarioAtual = botao.Text;
            EnviarParaGrupo = false;

            //Exibir historico da conversa com esse usuario
            ExibirHistorico();

        }


        //Exibe a conversa com o destinatario atual, usuario ou grupo
        private void ExibirHistorico(bool paraGrupo = false)
        {
            paraGrupo = paraGrupo || EnviarParaGrupo;
            txtHistorico.Clear();
            if (paraGrupo)
            {
                //Encontrar mensagens para o grupo e exibi-las
                foreach (var mensagem in mensagens.Where(c => c.destinatario == DestinatarioAtual))
                {
                    txtHistorico.Text += Environment.NewLine + mensagem.remetente + ":" + mensagem.mensagem;

                }
            }
            else
            {
                //Encontrar mensagens para o usuario ou mensagens privadas vindas do usuario, e exibi-las
                foreach (var mensagem in mensagens.Where(c =>
                 c.remetente == DestinatarioAtual || (c.remetente == Global.UsuarioNome && c.destinatario == DestinatarioAtual)))
                {
                    txtHistorico.Text += Environment.NewLine + mensagem.remetente + ":" + mensagem.mensagem;
                }
            }
        }

        //Limpa a cor dos botes. TODOS os botoes.
        private void ResetarBotoes()
        {
            foreach (var botao in splitContainer1.Panel1.Controls.OfType<Button>())
            {
                botao.BackColor = SystemColors.ButtonFace;
            }
        }

        //Ler usuarios no servidor
        private async void SincronizarUsuarios()
        {
            //Url para buscar os dados
            string url = Global.Domain + "usuarios";

            //valores para o post, nesse caso somente autenticacao
            var valores = new Dictionary<string, string>
            {
                {"usuario",Global.UsuarioNome },
                {"senha",Global.UsuarioSenha}
            };

            var conteudo = new FormUrlEncodedContent(valores);

            try
            {
                //Ler resposta
                var resposta = await client.PostAsync(url, conteudo);

                //Sucesso? interpretar resposta e salvar lista de usuarios
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
            }catch{ }
        }

        //Ler mensagens no servidor
        private async void LerMensagens()
        {
            string url = Global.Domain + "receberMensagens";

            //Enviar o numero de dispositivo
            var valores = new Dictionary<string, string>
            {
                {"dispositivo",Global.dispositivoID.ToString() }
            };

            //Enviar dados, após acrescentar credenciais
            var conteudo = new FormUrlEncodedContent(AdicionarCredenciais(valores));

            try
            {
                var resposta = await client.PostAsync(url, conteudo);

                //Se sucesso, salvar mensagens e enviar acks
                if (resposta.IsSuccessStatusCode)
                {
                    var mensagensJson = (await resposta.Content.ReadAsStringAsync());

                    //Somenta avaliar mensagens que nao foram recebidas.
                    var novasMensagens = ConverterMensagens(mensagensJson).Where(c => !acksEnviados.Contains(c.id)).ToList();

                    //Sem novas mensagens, terminar aqui
                    if (novasMensagens.Count() == 0)
                    {
                        return;
                    }

                    var acksEnviadosAgora = await EnviarAcks(mensagensJson);
                    //Enviar acks para novas mensagens
                    if (acksEnviadosAgora != null)
                    {
                        //Após sucesso do envio de acks, salvar mensagens na fila
                        //Verificar acks novamente, pois entre a hora que os acks foram enviados e agora, pode ter ocorrido envio de mais mensagens
                        mensagens.AddRange(novasMensagens.Where(c=>!acksEnviados.Contains(c.id)));
                        acksEnviados.AddRange(acksEnviadosAgora);

                        //Exibit historico com novas mensagens
                        ExibirHistorico();
                    }

                   
                }
            }
            catch { }

        }

        //Enviar acks, e retornar verdadeiro ou false se sucesso
        private async Task<List<string>> EnviarAcks(string mensagensJson)
        {
            var acks = new List<string>();
            try
            {
                //As chaves do JSON sao os ids das mensagens, extrair esses
                var dados = (JObject)JsonConvert.DeserializeObject(mensagensJson);
                foreach (var mensagem in dados)
                {
                    acks.Add(mensagem.Key);
                }


                //Enviar acks
                string url = Global.Domain + "acks";

                var valores = new Dictionary<string, string>{
                    {"dispositivo",Global.dispositivoID.ToString() },
                    {"acks",String.Join(",",acks) }
                 };

                var conteudo = new FormUrlEncodedContent(AdicionarCredenciais(valores));

                var resposta = await client.PostAsync(url, conteudo);

                if (resposta.IsSuccessStatusCode)
                {
                    
                    return (acks); ;
                }
                else
                {
                    return null;
                }


            }
            catch
            {
                return null;
            }
        }

        //Converte mensagens de JSON para o objeto mensagem
        private List<Mensagem> ConverterMensagens(string mensagensJson)
        {
            var mensagens = new List<Mensagem>();

            try
            {
                //Ler JSON e adicionar mensagens
                var dados = (JObject)JsonConvert.DeserializeObject(mensagensJson);
                foreach (var dadosJson in dados)
                {
                    var mensagemJson = dadosJson.Value;
                    mensagens.Add(new Mensagem(dadosJson.Key,
                        mensagemJson["mensagem"].ToString(),
                        mensagemJson["remetente"].ToString(),
                        mensagemJson["destinatario"].ToString(),
                        bool.Parse(mensagemJson["paraGrupo"].ToString())));
                }

            }
            catch { }
            return mensagens;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SincronizarUsuarios();

            LerMensagens();

            SincronizarGrupos();

        }

        public async void SincronizarGrupos()
        {
            string url = Global.Domain + "grupos";

            var valores = new Dictionary<string, string>
            {
                {"usuario",Global.UsuarioNome },
                {"senha",Global.UsuarioSenha}
            };

            var conteudo = new FormUrlEncodedContent(valores);

            try
            {
                var resposta = await client.PostAsync(url, conteudo);

            if (resposta.IsSuccessStatusCode)
            {

                var dados = (JObject)JsonConvert.DeserializeObject(await resposta.Content.ReadAsStringAsync());
                
                    //Armazena grupos existentes, para remover os botoes dos grupos que foram removidos
                    var gruposExistentes = new List<string>();

                    Global.usuarios = new List<string>();
                    foreach (var grupo in dados)
                    {
                        gruposExistentes.Add(grupo.Key.ToString());
                        if (!dadosGrupo.ContainsKey(grupo.Key.ToString()))
                        {
                            dadosGrupo.Add(grupo.Key.ToString(), bool.Parse(grupo.Value.ToString()));
                        }
                    }

                    //Se o grupo está na lista de grupos, mas nao no servidor, remover botao e remover da lista de grupos
                    foreach(var grupoRemovido in dadosGrupo.Where(c => !gruposExistentes.Contains(c.Key)).ToList())
                    {
                        splitContainer1.Panel1.Controls.Remove(splitContainer1.Panel1.Controls.OfType<Button>().Where(c => c.Text == grupoRemovido.Key).FirstOrDefault());
                        dadosGrupo.Remove(grupoRemovido.Key);
                    }

                    CriarBotoesGrupos();
                
            }
            }
            catch { }
        }

        //Cria botoes de grupos e atualisa as cores
        private void CriarBotoesGrupos()
        {
            foreach (var grupo in dadosGrupo)
            {
                if (splitContainer1.Panel1.Controls.OfType<Button>().Where(c => c.Text == grupo.Key).Count() == 0)
                {
                    var botao = new Button { Text = grupo.Key, Dock = DockStyle.Bottom };
                    botao.Click += CliqueBotaoGrupo;
                    splitContainer1.Panel1.Controls.Add(botao);
                }
                var cor = grupo.Value ? SystemColors.GradientActiveCaption : SystemColors.ButtonFace;
                splitContainer1.Panel1.Controls.OfType<Button>().FirstOrDefault(c => c.Text == grupo.Key).BackColor = cor;
            }
        }

        //Clique no botao de grupo: ingressa no grupo, escolhe grupo como destinatario atual, ou, se for o grupo atual, sai do grupo
        private void CliqueBotaoGrupo(object sender, EventArgs e)
        {
            if (sender.GetType() != typeof(Button))
            {
                return;
            }

            var botao = (Button)sender;

            var nomeGrupo = botao.Text;

            if (nomeGrupo == DestinatarioAtual)
            {
                //Clicou no grupo atual: sair do grupo
                SairDoGrupo(nomeGrupo);
                return;

            }
            else
            {
                if (dadosGrupo[nomeGrupo])
                {
                    //Usuario pertence ao grupo
                    DestinatarioAtual = nomeGrupo;
                    EnviarParaGrupo = true;

                }
                else
                {
                    IngressarGrupo(nomeGrupo);
                }
            }

            ExibirHistorico();
        }

        private async void SairDoGrupo(string nomeGrupo)
        {
            var url = Global.Domain + "sairGrupo";

            if (nomeGrupo == "")
            {
                return;
            }

            var valores = new Dictionary<string, string>{
                    {"nomeGrupo",nomeGrupo }
                 };

            var conteudo = new FormUrlEncodedContent(AdicionarCredenciais(valores));

            try
            {
                var resposta = await client.PostAsync(url, conteudo);

                if (resposta.IsSuccessStatusCode)
                {
                    //Remover destinatario atual, se for o grupo (geralmente é)
                    if (DestinatarioAtual == nomeGrupo)
                    {
                        DestinatarioAtual = "";
                        dadosGrupo[nomeGrupo] = false;
                        CriarBotoesGrupos();
                        ExibirHistorico();
                    }
                }
            }
            catch { }
        }

        private Dictionary<string, string> AdicionarCredenciais(Dictionary<string, string> dicionario)
        {
            dicionario.Add("usuario", Global.UsuarioNome);
            dicionario.Add("senha", Global.UsuarioSenha);
            return dicionario;
        }

        //Enviar mensagem
        private async void btnEnviar_Click(object sender, EventArgs e)
        {
            if (txtMensagem.Text != "" && DestinatarioAtual != "")
            {
                string url = Global.Domain;
                url += EnviarParaGrupo ? "enviarMensagemGrupo" : "EnviarUnico";

                var mensagem = new Dictionary<string, string>{
                    { "mensagem",txtMensagem.Text },
                    {"remetente", Global.UsuarioNome },
                    {"destinatario", DestinatarioAtual } };


                try
                {
                    var conteudo = new FormUrlEncodedContent(AdicionarCredenciais(mensagem));

                    var resposta = await client.PostAsync(url, conteudo);

                    if (!resposta.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Erro ao enviar mensagem.");
                    } else
                    {
                        txtMensagem.Text = "";
                    }
                }
                catch { }
            }
        }

        private async void btnCriarGrupo_Click(object sender, EventArgs e)
        {
            var nomeGrupo = Prompt.ShowDialog("Digite o nome do grupo", "Digite o nome do novo grupo.");

            if (!await IngressarGrupo(nomeGrupo))
            {
                MessageBox.Show("Erro ao criar grupo.");
            }
        }

        private async Task<bool> IngressarGrupo(string nomeGrupo)
        {
            var url = Global.Domain + "ingressarGrupo";

            if (nomeGrupo == "")
            {
                MessageBox.Show("O nome do grupo nao pode ser vazio.");
            }

            var valores = new Dictionary<string, string>{
                    {"nomeGrupo",nomeGrupo }
                 };

            var conteudo = new FormUrlEncodedContent(AdicionarCredenciais(valores));

            try
            {
                var resposta = await client.PostAsync(url, conteudo);

                //Se sucesso, marcar grupo como destinatario atual, atualizar botoes e historico
                if (resposta.IsSuccessStatusCode)
                {
                    dadosGrupo[nomeGrupo] = true;
                    DestinatarioAtual = nomeGrupo;
                    EnviarParaGrupo = true;
                    CriarBotoesGrupos();
                    ExibirHistorico();
                }

                return (resposta.IsSuccessStatusCode);
            }
            catch
            {
                return false;
            }
        }

        private void txtHistorico_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
