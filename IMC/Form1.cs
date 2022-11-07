using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IMC
{
    
    public partial class Form1 : Form
    {
        public static MySqlConnection conexao;
        public string status;
        private string dataSource = "server=localhost;uid=root;pwd=1234;database=bd_imc";
        private int selecionarID = -1;
        public Form1()
        {
            InitializeComponent();
            

            lstPessoas.View = View.Details;
            lstPessoas.LabelEdit = true;
            lstPessoas.AllowColumnReorder = true;
            lstPessoas.FullRowSelect = true;
            lstPessoas.GridLines = true;


            
            lstPessoas.Columns.Add("ID", 60, HorizontalAlignment.Left);
            lstPessoas.Columns.Add("Nome", 180, HorizontalAlignment.Left);
            lstPessoas.Columns.Add("Idade", 60, HorizontalAlignment.Left);
            lstPessoas.Columns.Add("Altura", 60, HorizontalAlignment.Left);
            lstPessoas.Columns.Add("Peso", 60, HorizontalAlignment.Left);
            lstPessoas.Columns.Add("IMC", 60, HorizontalAlignment.Left);
            lstPessoas.Columns.Add("Status", 60, HorizontalAlignment.Left);

            carregarBanco();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Dados.conectar();

        }
        

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if(!(textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBoxIdade.Text == "" || textBoxIMC.Text == "")) { 
                try
                {

                    conexao = new MySqlConnection(dataSource);


                    string sql = "INSERT INTO pessoas (nome, idade, altura, peso, imc, status) " +
                                 "VALUES" +
                                 "('" +textBox1.Text+ "', '" +textBoxIdade.Text+ "', '" +textBox2.Text + "', '" +textBox3.Text + "', '" +textBoxIMC.Text + "', '" +status+ "')";

                    MySqlCommand comando = new MySqlCommand(sql, conexao);

                    conexao.Open();

                    comando.ExecuteNonQuery();

                    txtApagar(); 

                    MessageBox.Show("Salvo!" + MessageBoxButtons.OK);

                }

                catch(MySqlExeption ex)
                {
                    MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message, "Erro", MessageBoxButtons.OK,MessageBoxIcon.Error); 
                }
           
                finally
                {
                conexao.Close(); 
                carregarBanco();
                }
            }
            else
            {
                MessageBox.Show("Alguns Campos estão em branco!", "Campos em Branco!",MessageBoxButtons.OK ,MessageBoxIcon.Error); 
            }

        }

        private void sair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        

        public void button1_Click(object sender, EventArgs e)
        {
            double altura, peso, imc;

            if (!(textBox2.Text == "" || textBox3.Text == "")) { 
            imc = (Convert.ToDouble(textBox3.Text) / (Convert.ToDouble(textBox2.Text) * Convert.ToDouble(textBox2.Text)));
            
            if (imc <= 18.5)
            {
                pictureBox1.Image = Properties.Resources._1_fw;
                textBoxStatus.Text = "abaixo do peso";
            }
            if (imc >= 18.6 && imc <= 24.9)
            {
                pictureBox1.Image = Properties.Resources._2_fw;
                textBoxStatus.Text = "normal";
            }


            if (imc >= 25 && imc <= 29.9)
            {
                pictureBox1.Image = Properties.Resources._3_fw;
                textBoxStatus.Text = "sobrepeso";
            }


            if (imc >= 30 && imc <= 34.9)
            {
                pictureBox1.Image = Properties.Resources._4_fw;
                textBoxStatus.Text = "obesidade 1";
            }


            if (imc >= 35 && imc <= 39.9)
            {
                pictureBox1.Image = Properties.Resources._5_fw;
                textBoxStatus.Text = "obesidade 2";
            }


            if (imc >= 40)
            {
                pictureBox1.Image = Properties.Resources._6_fw;
                textBoxStatus.Text = "obesidade 3";
            }

            textBoxIMC.Text = imc.ToString("F");

            btnEnviar.Enabled = true;
            }
            else
            {
                MessageBox.Show("Insira a Altura e o Peso para Calcular!", "Campos em Branco!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            txtApagar();
        }

        private void carregarBanco()
        {
            conexao = new MySqlConnection(dataSource);
            string sql = "SELECT * FROM pessoas";

            MySqlCommand comando = new MySqlCommand(sql, conexao);

            conexao.Open();

            MySqlDataReader reader = comando.ExecuteReader();

            lstPessoas.Items.Clear();

            while (reader.Read())
            {
                string[] row = {
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.GetString(6)
                        };
                var linha_listview = new ListViewItem(row);

                lstPessoas.Items.Add(linha_listview);
            }
            conexao.Close();
        }
        private void txtApagar()
        {
            textBox1.ResetText();
            textBox2.ResetText();
            textBox3.ResetText();
            textBoxIdade.ResetText();
            textBoxIMC.ResetText();
            pictureBox1.Image = Properties.Resources.question;
        }

        private void btnExluir_Click(object sender, EventArgs e)
        {
            if (!(textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBoxIdade.Text == ""))
            {
                try
                {
                    DialogResult confirm = MessageBox.Show("Deseja excluir o registro?", "Excluir Registro",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Warning);



                    if (confirm == DialogResult.Yes)
                    {
                        conexao = new MySqlConnection(dataSource);



                        conexao.Open();



                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = conexao;



                        cmd.CommandText = "DELETE FROM pessoas WHERE id=@id";



                        cmd.Parameters.AddWithValue("@id", selecionarID);



                        cmd.Prepare();



                        cmd.ExecuteNonQuery();



                        MessageBox.Show("Registro excluído", "Concluído",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information);



                        carregarBanco();
                        txtApagar();
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message, "Erro",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocorreu um erro: " + ex.Message, "Erro",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                finally
                {
                    conexao.Close();
                }
            }
            else
            {
                MessageBox.Show("Alguns Campos estão em branco!", "Campos em Branco!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lstPessoas_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            
                ListView.SelectedListViewItemCollection itemSelect = lstPessoas.SelectedItems;



                foreach (ListViewItem item in itemSelect)
                {
                    selecionarID = Convert.ToInt32(item.SubItems[0].Text);



                    textBox1.Text = item.SubItems[1].Text;
                    textBoxIdade.Text = item.SubItems[2].Text;
                    textBox2.Text = item.SubItems[3].Text;
                    textBox3.Text = item.SubItems[4].Text;
                    textBoxIMC.Text = item.SubItems[5].Text;
                    textBoxStatus.Text = item.SubItems[6].Text;


                    btnAtualizar.Enabled = true;
                    btnExcluir.Enabled = true;
                }
            
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            if(!(textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBoxIdade.Text == "")) {
                conexao = new MySqlConnection(dataSource);



                conexao.Open();



                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;




                cmd.CommandText = "UPDATE pessoas SET " +
                                  "nome=@nome, idade=@idade, peso=@peso, altura=@altura, " +
                                  "imc=@imc, status=@status " +
                                  "WHERE id=@id";



                cmd.Parameters.AddWithValue("@nome", textBox1.Text);
                cmd.Parameters.AddWithValue("@idade", textBoxIdade.Text);
                cmd.Parameters.AddWithValue("@peso", textBox2.Text);
                cmd.Parameters.AddWithValue("@altura", textBox3.Text);
                cmd.Parameters.AddWithValue("@imc", textBoxIMC.Text);
                cmd.Parameters.AddWithValue("@status", textBoxStatus.Text);
                cmd.Parameters.AddWithValue("@id", selecionarID);



                cmd.Prepare();



                cmd.ExecuteNonQuery();



                carregarBanco();



                MessageBox.Show("Atualização realizada!");
            }
            else
            {
                MessageBox.Show("Alguns Campos estão em branco!", "Campos em Branco!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            calcular();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            calcular();
        }
        private void calcular()
        {
            double altura, peso, imc;
            if (textBox2.Text != "" && textBox3.Text != "") { 
            
                imc = (Convert.ToDouble(textBox3.Text) / (Convert.ToDouble(textBox2.Text) * Convert.ToDouble(textBox2.Text)));

            
                if (imc <= 18.5)
            
                {
               
                    pictureBox1.Image = Properties.Resources._1_fw;
                
                    textBoxStatus.Text = "abaixo do peso";
            
                }
                if (imc >= 18.6 && imc <= 24.9)
                {
                    pictureBox1.Image = Properties.Resources._2_fw;
                    textBoxStatus.Text = "normal";
                }


                if (imc >= 25 && imc <= 29.9)
                {
                    pictureBox1.Image = Properties.Resources._3_fw;
                    textBoxStatus.Text = "sobrepeso";
                }


                if (imc >= 30 && imc <= 34.9)
                {
                    pictureBox1.Image = Properties.Resources._4_fw;
                    textBoxStatus.Text = "obesidade 1";
                }


                if (imc >= 35 && imc <= 39.9)
                {
                    pictureBox1.Image = Properties.Resources._5_fw;
                    textBoxStatus.Text = "obesidade 2";
                }


                if (imc >= 40)
                {
                    pictureBox1.Image = Properties.Resources._6_fw;
                    textBoxStatus.Text = "obesidade 3";
                }

                textBoxIMC.Text = imc.ToString("F");
            }
                
            
        }
    }
}
