using ProductManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlServerCe;

namespace ProductManager.Controllers
{
    public class ProductController : ApiController
    {
        #region Variáveis globais
        //string global que contém o comando SQL que será executado
        private string strSQL;

        //conexão com banco de dados SQLServer Compact
        private static SqlCeConnection Conn = new SqlCeConnection(Config.config.ConnectionString);
        #endregion
        #region Get - Retornar todos os produtos ou um produto específico da base de dados
        public IHttpActionResult Get()//GET que retorna todos os produtos
        {
            
            //Comando SQL que buscará todos os campos de todos os produtos na base de dados
            strSQL =    "SELECT * " +
                        "FROM Produto";

            //Caso a conexão esteja fechada, a abre
            if (Conn.State != ConnectionState.Open)
                Conn.Open();

            //Cria uma lista de produtos que serão retornados
            List<Product> products = new List<Product>();

            //Cria e instancia o command que irá executar a cláusula passada
            SqlCeCommand command = new SqlCeCommand(strSQL, Conn);

            //Cria, instancia o reader que conterá os registros retornados e executa o comando SQL
            SqlCeDataReader reader = command.ExecuteReader();

            //Enquanto houver registros a serem lidos, serão lidos
            while (reader.Read())
            {
                //Adiciona um novo produto na lista de produtos
                products.Add(new Product    {   id = int.Parse(reader["id"].ToString()),
                                                categoria = reader["categoria"].ToString(),
                                                descricao = reader["descricao"].ToString(),
                                                preco = float.Parse(reader["preco"].ToString())
                                            }
                            );
            }

            //Caso a conexão esteja aberta, a fecha
            if (Conn.State == ConnectionState.Open)
                Conn.Close();

            // Retorna todos os produtos
            return Ok(products);
        }

        public IHttpActionResult Get(int id)//Get que retornará um produto específico
        {
            //Comando SQL que buscará todos os campos de um produto específico da base de dados
            strSQL =    "SELECT * " +
                        "FROM   Produto " +
                        "WHERE  id = @id";

            //Caso a conexão esteja fechada, a abre
            if (Conn.State != ConnectionState.Open)
                Conn.Open();

            //Cria e instancia um produto como nulo
            Product product = null;

            //Cria e instancia o command que irá executar a cláusula passada
            SqlCeCommand command = new SqlCeCommand(strSQL, Conn);

            //Cria um parametro e já o instancia passando o nome e o id daquele produto
            SqlCeParameter parametro = new SqlCeParameter("@id", id);
            
            //Adiciona ao comando SQL o parâmetro passado
            command.Parameters.Add(parametro);

            //Cria, instancia o reader que conterá os registros retornados e executa o comando SQL
            SqlCeDataReader reader = command.ExecuteReader();

            //Enquanto houver registros a serem lidos, serão lidos
            while (reader.Read())
            {
                product = new Product
                {
                    id =    int.Parse(reader["id"].ToString()),
                            categoria = reader["categoria"].ToString(),
                            descricao = reader["descricao"].ToString(),
                            preco = float.Parse(reader["preco"].ToString())
                };
            }

            //Caso a conexão esteja aberta, a fecha
            if (Conn.State == ConnectionState.Open)
                Conn.Close();

            // Retorna tudo
            return Ok(product);
        }
        #endregion
        #region Post - Inserir um produto na base de dados
        public HttpResponseMessage Post(Product produto)//POST que irá inserir um produto na base de dados conforme objeto passado como parâmetro
        {
            //Cria uma mensagem de resposta HTTP
            HttpResponseMessage response = null;

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
                parametros.Add(new SqlCeParameter ("@categoria", produto.categoria));
                parametros.Add(new SqlCeParameter ("@preco", produto.preco));

                //Adiciona ao comando SQL todos os parâmetros criados
                for (int i = 0; i < parametros.Count; i++) command.Parameters.Add(parametros[i]);   

                try
                {

                    //Comando SQL que retorna o próximo ID de produto
                    strSQL =    "SELECT autoinc_next ID " +
                                "FROM   information_schema.columns " +
                                "WHERE  column_name = 'id' ";

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

                    //Define a mensagem de resposta como OK
                    response = Request.CreateResponse(HttpStatusCode.OK);

                    //Define a mensagem personalizada de resposta
                    response.Content = new StringContent("Produto inserido com sucesso");   
                }
                catch (Exception e)
                {
                    //Caso tenha dado algum problema, define a mensagem de resposta com erro
                    response = Request.CreateResponse(HttpStatusCode.BadRequest);

                    //Caso tenha dado algum problema, define a mensagem personalizada de resposta
                    response.Content = new StringContent("Falha ao inserir o produto\n\n" + e.ToString());
                }

                //Caso a conexão esteja aberta, a fecha
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
            else
            {
                //Define a mensagem de resposta com erro
                response = Request.CreateResponse(HttpStatusCode.BadRequest);

                //Define a mensagem personalizada de resposta
                response.Content = new StringContent("Falha ao inserir o produto");
            }

            //Retorna a mensagem http
            return response;

        }
        #endregion
        #region Put - Atualizar um produto na base de dados
        public HttpResponseMessage Put(Product produto) //PUT responsável por inserir um produto na base de dados
        {
            HttpResponseMessage response;

            if (produto != null)
            {
                try
                {
                    //Comando SQL que atualizará o produto na base de dados
                    strSQL =    "UPDATE Produto " +
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

                    //Define a mensagem de resposta como OK
                    response = Request.CreateResponse(HttpStatusCode.OK);

                    //Define a mensagem personalizada de resposta
                    response.Content = new StringContent("Produto atualizado com sucesso");

                } catch (Exception e)
                {
                    //Caso tenha dado algum problema, define a mensagem de resposta com erro
                    response = Request.CreateResponse(HttpStatusCode.BadRequest);

                    //Caso tenha dado algum problema, define a mensagem personalizada de resposta
                    response.Content = new StringContent("Falha ao atualizar o produto\n\n" + e.ToString());
                }

                //Caso a conexão esteja aberta, a fecha
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                         
            }
            else
            {
                //Define a mensagem de resposta com erro
                response = Request.CreateResponse(HttpStatusCode.BadRequest);

                //Define a mensagem personalizada de resposta
                response.Content = new StringContent("Falha ao atualizar o produto");
            }

            //Retorna a mensagem http
            return response;

        }
        #endregion
        #region Delete - Deleta um produto da base de dados
        public HttpResponseMessage Delete(int id)
        {

            HttpResponseMessage response;

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

                //Define a mensagem de resposta como OK
                response = Request.CreateResponse(HttpStatusCode.OK);

                //Define a mensagem personalizada de resposta
                response.Content = new StringContent("Produto deletado com sucesso");

                //Caso a conexão esteja aberta, a fecha
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();

            }
            catch(Exception e)
            {
                //Caso tenha dado algum problema, define a mensagem de resposta com erro
                response = Request.CreateResponse(HttpStatusCode.BadRequest);

                //Caso tenha dado algum problema, define a mensagem personalizada de resposta
                response.Content = new StringContent("Falha ao deletar o produto\n\n" + e.ToString());
            }

            //Retorna a mensagem http
            return response;
        }
        #endregion
    }
}