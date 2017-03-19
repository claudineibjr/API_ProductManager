using ProductManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductManager.Controllers
{
    public class ProductController : ApiController
    {
        static List<Product> products = new List<Product>
        {
            new Product {Id = 1, Category = "Groceries", Name = "Tomato Soup", Price = 1.75M },
            new Product {Id = 2, Category = "Toys", Name = "Yo-yo", Price = 10.60M },
            new Product {Id = 3, Category = "Hardware", Name = "Hammer", Price = 16.99M }
        };

        public IHttpActionResult Get()
        {
            // Retorna tudo
            return Ok(products);
        }

        public IHttpActionResult Get(int id)
        {
            // Retorna um objeto de produto com o ID passado
            return Ok(products.FirstOrDefault<Product>(x => x.Id == id));
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

            int index = products.FindIndex(p => p.Id == produto.Id);

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

            int index = products.FindIndex(p => p.Id == id);

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
