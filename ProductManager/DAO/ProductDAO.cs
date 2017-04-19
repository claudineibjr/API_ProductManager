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
            strSQL =    "SELECT produto.id AS p_id, produto.descricao AS p_descricao, CONVERT(DECIMAL(10, 2), produto.preco) as p_preco, " +
                        "       categoria.id AS c_id, categoria.descricao AS c_descricao " +
                        "FROM   Produto " +
                        "       LEFT JOIN Categoria ON produto.id_categoria = categoria.id " +
                        ( id == 0 ? "" : "WHERE  produto.id = @id");

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
                                    id = int.Parse(reader["p_id"].ToString()),
                                    descricao = reader["p_descricao"].ToString(),
                                    preco = float.Parse(reader["p_preco"].ToString()),
                                    categoria = new Models.Category()
                                    {
                                        id = int.Parse(reader["c_id"].ToString()),
                                        descricao = reader["c_descricao"].ToString()
                                    }
                                }
                            );
            }

            //Caso a conexão esteja aberta, a fecha
            if (Conn.State == ConnectionState.Open)
                Conn.Close();

            return products;
        }
        static public List<Object> InsertProduct(Models.Product produto)
        {
            //Criado objeto que conterá o retorno. Na posição 0, como true ou false e na posição 1 como string que pode conter o erro
            List<Object> retorno = new List<Object>();

            if (produto != null)
            {
                //Comando SQL que irá inserir o produto no banco de dados
                strSQL =    "INSERT INTO Produto " +
                            "(descricao, id_categoria, preco) " +
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
                parametros.Add(new SqlCeParameter("@categoria", produto.categoria.id));
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

                    retorno.Add(true);
                    retorno.Add("");
                    return retorno;

                }
                catch (Exception e)
                {
                    retorno.Add(false);
                    retorno.Add(e.ToString());
                    return retorno;
                }

                //Caso a conexão esteja aberta, a fecha
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
            else
            {
                retorno.Add(false);
                retorno.Add("O produto não pode ser nulo");
                return retorno;
            }
        }
        static public List<Object> UpdateProduct(Models.Product produto)
        {

            //Criado objeto que conterá o retorno. Na posição 0, como true ou false e na posição 1 como string que pode conter o erro
            List<Object> retorno = new List<Object>();

            if (produto != null)
            {
                try
                {
                    //Comando SQL que atualizará o produto na base de dados
                    strSQL =    "UPDATE Produto " +
                                "SET    descricao = @descricao, " +
                                "       id_categoria = @categoria, " +
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
                    parametros.Add(new SqlCeParameter("@categoria", produto.categoria.id));
                    parametros.Add(new SqlCeParameter("@preco", produto.preco));
                    parametros.Add(new SqlCeParameter("@id", produto.id));

                    //Adiciona ao comando SQL todos os parâmetros criados
                    for (int i = 0; i < parametros.Count; i++) command.Parameters.Add(parametros[i]);

                    //Executa o comando SQL
                    command.ExecuteNonQuery();

                    retorno.Add(true);
                    retorno.Add("");
                    return retorno;

                }
                catch (Exception e)
                {
                    retorno.Add(false);
                    retorno.Add(e.ToString());
                    return retorno;
                }

                //Caso a conexão esteja aberta, a fecha
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();

            }
            else
            {
                retorno.Add(false);
                retorno.Add("O produto não pode ser nulo");
                return retorno;
            }
        }
        static public List<Object> DeleteProduct(int id)
        {

            //Criado objeto que conterá o retorno. Na posição 0, como true ou false e na posição 1 como string que pode conter o erro
            List<Object> retorno = new List<Object>();

            //Comando SQL que deletará o produto da base de dados
            strSQL =    "DELETE Produto " +
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


                retorno.Add(true);
                retorno.Add("");
                return retorno;

            }
            catch (Exception e)
            {
                retorno.Add(false);
                retorno.Add(e.ToString());
                return retorno;
            }
        }
    }
}