
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMC
{
    internal class Dados
    {
        public static MySqlConnection conexao;
        public static MySqlCommand comando;
        public static MySqlDataAdapter adaptador;
        public static DataTable dataTable;

        public static void conectar()
        {
            conexao = new MySqlConnection("server=localhost;uid=root;pwd=1234");

            conexao.Open();

            comando = new MySqlCommand("CREATE DATABASE IF NOT EXISTS bd_imc; use bd_imc", conexao);

            comando.ExecuteNonQuery();

            comando = new MySqlCommand("CREATE TABLE IF NOT EXISTS pessoas " +
                                       "(id integer auto_increment primary key, " +
                                       "nome varchar(150), " +
                                       "idade varchar(50), " +
                                       "altura varchar(50), " +
                                       "peso varchar(50), " +
                                       "imc varchar(50), " +
                                       "status varchar(50))", conexao);
            comando.ExecuteNonQuery();
            //Fecha a conexão com o banco de dados
            conexao.Close();
                                       
            
        }
    }
}
