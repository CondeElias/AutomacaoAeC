using AeCBot.Infraestrutura;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AeCBot.Front
{
    public class BuscaAec
    {
        private readonly IDbConnection dbConnection;
        private readonly SiteBusca siteBusca;

        public BuscaAec(IDbConnection dbConnection, SiteBusca siteBusca)
        {
            this.dbConnection = dbConnection;
            this.siteBusca = siteBusca;
        }

        public void Buscar(string termo)
        {
            using (IDbConnection dbConnection = DbConnectionFactory.CreateConnection())
            {
                dbConnection.Open();
                siteBusca.Buscar(termo);
                var listaTitulos = siteBusca.ObterResultados();
                using (IDbCommand comandoVerificaTabela = dbConnection.CreateCommand())
                {
                    comandoVerificaTabela.CommandText = "SELECT name FROM sys.tables WHERE name='tabelaDadosPesquisa'";
                    var resultado = comandoVerificaTabela.ExecuteScalar();
                    if (resultado == null)
                    {
                        using (IDbCommand comandoCriaTabela = dbConnection.CreateCommand())
                        {
                            comandoCriaTabela.CommandText = "CREATE TABLE tabelaDadosPesquisa (id INTEGER PRIMARY KEY IDENTITY(1,1), titulo TEXT, area TEXT, autor TEXT, descricao TEXT, data TEXT)";
                            comandoCriaTabela.ExecuteNonQuery();
                        }
                    }
                }
                foreach (var pesquisa in listaTitulos)
                {
                    IDbCommand comando = dbConnection.CreateCommand();
                    comando.CommandText = "INSERT INTO tabelaDadosPesquisa (titulo, area, autor, descricao, data) VALUES (@titulo, @area, @autor, @descricao, @data)";
                    comando.Parameters.Add(new SqlParameter("@titulo", pesquisa.Titulo));
                    comando.Parameters.Add(new SqlParameter("@area", pesquisa.Area));
                    comando.Parameters.Add(new SqlParameter("@autor", pesquisa.Autor));
                    comando.Parameters.Add(new SqlParameter("@descricao", pesquisa.Descricao));
                    comando.Parameters.Add(new SqlParameter("@data", pesquisa.Data));

                    int linhasAfetadas = comando.ExecuteNonQuery();

                    if (linhasAfetadas == 1)
                    {
                        Console.WriteLine("Título adicionado com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("Erro ao adicionar título...");
                    }
                }
                dbConnection.Close();
            }
        }
    }
}
