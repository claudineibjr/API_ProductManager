using ProductManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;

namespace ProductManager.Controllers
{
    public class ProductController : ApiController
    {
        //static public DataSour
        private string strSQL;
        private static SqlConnection Conn = new SqlConnection(Config.config.ConnectionString);
        
        /*static List<Product> products = new List<Product>
        {
            new Product {id = 1, categoria = "Informática", descricao = "Notebook", preco = 1350.00M },
            new Product {id = 2, categoria = "Games", descricao = "Xbox One", preco = 1249.99M },
            new Product {id = 3, categoria = "Informática", descricao = "Mouse", preco = 24.99M },
            new Product {id = 4, categoria = "Games", descricao = "Playstation 4", preco = 1199.99M },
            new Product {id = 5, categoria = "Eletrodoméstico", descricao = "SMARTV 42''", preco = 2099.99M }
        };*/

        public IHttpActionResult Get()
        {
            strSQL = "SELECT * FROM Produto";

            if (Conn.State != ConnectionState.Open)
                Conn.Open();

            List<Product> products = new List<Product>();

            try
            {
                SqlCommand command = new SqlCommand(strSQL, Conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.HasRows)
                {
                    products.Add(new Product { id = int.Parse(reader["id"].ToString()), categoria = reader["categoria"].ToString(), descricao = reader["descricao"].ToString(), preco = float.Parse(reader["preco"].ToString()) });
                }

            } catch(Exception e)
            {

            }

                // Retorna tudo
            return Ok(products);
        }

        public IHttpActionResult Get(int id)
        {
            // Retorna um objeto de produto com o ID passado
            //return Ok(products.FirstOrDefault<Product>(x => x.id == id));
        }
        public HttpResponseMessage Post(Product produto)
        {
            if (produto != null)
            {
                products.Add(produto);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent("Produto inserido com sucesso");
                return response;
            }
            else
            {
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.BadRequest);
                responseError.Content = new StringContent("Falha ao inserir o produto");
                return responseError;
            }
        }

        public HttpResponseMessage Put(Product produto)
        {

            int index = products.FindIndex(p => p.id == produto.id);

            if (index > -1)
            {
                products[index] = produto;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent("Produto atualizado com sucesso");
                return response;
            }
            else
            {
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.BadRequest);
                responseError.Content = new StringContent("Falha ao atualizar o produto");
                return responseError;
            }
        }

        public HttpResponseMessage Delete(int id)
        {

            int index = products.FindIndex(p => p.id == id);

            if (index > -1)
            {
                products.RemoveAt(index);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent("Produto deletado com sucesso");
                return response;
            }
            else
            {
                HttpResponseMessage responseError = Request.CreateResponse(HttpStatusCode.BadRequest);
                responseError.Content = new StringContent("Falha ao deletar o produto");
                return responseError;
            }
        }

    }
}
