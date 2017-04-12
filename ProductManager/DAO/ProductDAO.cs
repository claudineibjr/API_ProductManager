using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;

namespace ProductManager.DAO
{
    static public class ProductDAO
    {
        #region Variáveis Globais
        //string global que contém o comando SQL que será executado
        static private string strSQL;

        //conexão com banco de dados SQLServer Compact
        private static SqlCeConnection Conn = new SqlCeConnection(Config.Config.ConnectionString);
        #endregion

        static public List<Models.Product> SelectProducts(int id = 0)
        {
            //Comando SQL que buscará todos os campos de todos os produtos na base de dados ou de um produto específico
            strSQL =    "SELECT * " +
                        "FROM   Produto " +
                        ( id == 0 ? "" : "WHERE  id = @id");

            //Caso a conexão esteja fechada, a abre
            if (Conn.State != ConnectionState.Open)
                Conn.Open();

            //Cria uma lista de produtos que serão retornados
            List<Models.Product> products = new List<Models.Product>();

            //Cria e instancia o command que irá executar a cláusula passada
            SqlCeCommand command = new SqlCeCommand(strSQL, Conn);

            //Se o ID for diferente de 0, deve-se adicionar os parâmetros
            if (id != 0)
            {
                //Cria uma lista de parâmetros
                List<SqlCeParameter> parametros = new List<SqlCeParameter>();

                //Adiciona na lista de parâmetros, todos os parâmetros necessários para a inserção do produto
                parametros.Add(new SqlCeParameter("@id", id));

                //Adiciona ao comando SQL todos os parâmetros criados
                for (int i = 0; i < parametros.Count; i++) command.Parameters.Add(parametros[i]);
            }

            //Cria, instancia o reader que conterá os registros retornados e executa o comando SQL
            SqlCeDataReader reader = command.ExecuteReader();

            //Enquanto houver registros a serem lidos, serão lidos
            while (reader.Read())
            {
                //Adiciona um novo produto na lista de produtos
                products.Add(new Models.Product
                                {
                                    id = int.Parse(reader["id"].ToString()),
                                    categoria = reader["categoria"].ToString(),
                                    descricao = reader["descricao"].ToString(),
                                    preco = float.Parse(reader["preco"].ToString())
                                }
                            );
            }

            //Caso a conexão esteja aberta, a fecha
            if (Conn.State == ConnectionState.Open)
                Conn.Close();

            return products;
        }
        static public bool InsertProduct(Models.Product produto)
        {

            if (produto != null)
            {
                //Comando SQL que irá inserir o produto no banco de dados
                strSQL =    "INSERT INTO Produto " +
                            "(descricao, categoria, preco) " +
                            "VALUES (@descricao, @categoria, @preco)";

                //Caso a conexão esteja fechada, a abre
                if (Conn.State != ConnectionState.Open)
                    Conn.Open();

                //Cria e instancia o command que irá executar a cláusula passada
                SqlCeCommand command = new SqlCeCommand(strSQL, Conn);

                //Cria uma lista de parâmetros
                List<SqlCeParameter> parametros = new List<SqlCeParameter>();

                //Adiciona na lista de parâmetros, todos os parâmetros necessários para a inserção do produto
                parametros.Add(new SqlCeParameter("@descricao", produto.descricao));
                parametros.Add(new SqlCeParameter("@categoria", produto.categoria));
                parametros.Add(new SqlCeParameter("@preco", produto.preco));

                //Adiciona ao comando SQL todos os parâmetros criados
                for (int i = 0; i < parametros.Count; i++) command.Parameters.Add(parametros[i]);

                try
                {

                    //Comando SQL que retorna o próximo ID de produto
                    strSQL =    "SELECT autoinc_next ID " +
                                "FROM   information_schema.columns " +
                                "WHERE  column_name = 'id' AND table_name = 'produto' ";

                    //Cria e instancia o command que irá executar a cláusula passada
                    SqlCeCommand command_aux = new SqlCeCommand(strSQL, Conn);

                    //Cria, instancia o reader que conterá os registros retornados e executa o comando SQL
                    SqlCeDataReader reader = command_aux.ExecuteReader();

                    //Enquanto houver registros a serem lidos, serão lidos
                    while (reader.Read())
                    {
                        //Define o ID do produto como o registro de próximo número de ID a ser criado para o produto
                        produto.id = int.Parse(reader["ID"].ToString());
                    }

                    //Executa o comando SQL
                    command.ExecuteNonQuery();

                    return true;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return false;
                }

                //Caso a conexão esteja aberta, a fecha
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
            else
            {
                return false;
            }
        }
        static public bool UpdateProduct(Models.Product produto)
        {
            if (produto != null)
            {
                try
                {
                    //Comando SQL que atualizará o produto na base de dados
                    strSQL = "UPDATE Produto " +
                                "SET    descricao = @descricao, " +
                                "       categoria = @categoria, " +
                                "       preco = @preco " +
                                "WHERE  id = @id";

                    //Caso a conexão esteja fechada, a abre
                    if (Conn.State != ConnectionState.Open)
                        Conn.Open();

                    //Cria e instancia o command que irá executar a cláusula passada
                    SqlCeCommand command = new SqlCeCommand(strSQL, Conn);

                    //Cria uma lista de parâmetros
                    List<SqlCeParameter> parametros = new List<SqlCeParameter>();

                    //Adiciona na lista de parâmetros, todos os parâmetros necessários para a atualização do produto
                    parametros.Add(new SqlCeParameter("@descricao", produto.descricao));
                    parametros.Add(new SqlCeParameter("@categoria", produto.categoria));
                    parametros.Add(new SqlCeParameter("@preco", produto.preco));
                    parametros.Add(new SqlCeParameter("@id", produto.id));

                    //Adiciona ao comando SQL todos os parâmetros criados
                    for (int i = 0; i < parametros.Count; i++) command.Parameters.Add(parametros[i]);

                    //Executa o comando SQL
                    command.ExecuteNonQuery();

                    return true;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return false;
                }

                //Caso a conexão esteja aberta, a fecha
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();

            }
            else
            {
                return false;
            }
        }
        static public bool DeleteProduct(int id)
        {
            //Comando SQL que deletará o produto da base de dados
            strSQL = "DELETE Produto " +
                        "WHERE  id = @id";

            try
            {

                //Caso a conexão esteja fechada, a abre
                if (Conn.State != ConnectionState.Open)
                    Conn.Open();

                //Cria e instancia o command que irá executar a cláusula passada
                SqlCeCommand command = new SqlCeCommand(strSQL, Conn);

                //Cria uma lista de parâmetros
                List<SqlCeParameter> parametros = new List<SqlCeParameter>();

                //Adiciona na lista de parâmetros, todos os parâmetros necessários para a atualização do produto
                parametros.Add(new SqlCeParameter("@id", id));

                //Adiciona ao comando SQL todos os parâmetros criados
                for (int i = 0; i < parametros.Count; i++) command.Parameters.Add(parametros[i]);

                //Executa o comando SQL
                command.ExecuteNonQuery();

                //Caso a conexão esteja aberta, a fecha
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();

                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}