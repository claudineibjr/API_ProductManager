﻿using ProductManager.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlServerCe;

namespace ProductManager.Controllers
{
    public class ProductController : ApiController
    {
        #region Variáveis globais
        #endregion
        #region Get - Retornar todos os produtos ou um produto específico da base de dados
        public IHttpActionResult Get()//GET que retorna todos os produtos
        {
            // Retorna todos os produtos
            return Ok(DAO.ProductDAO.SelectProducts());
        }

        public IHttpActionResult Get(int id)//Get que retornará um produto específico
        {
            // Retorna o produto selecionado
            return Ok(DAO.ProductDAO.SelectProducts(id));
        }
        #endregion
        #region Post - Inserir um produto na base de dados
        public HttpResponseMessage Post(Product produto)//POST que irá inserir um produto na base de dados conforme objeto passado como parâmetro
        {
            //Cria uma mensagem de resposta HTTP
            HttpResponseMessage response = null;

            //Criado objeto que conterá o retorno. Na posição 0, como true ou false e na posição 1 como string que pode conter o erro
            List<Object> insercao = DAO.ProductDAO.InsertProduct(produto);

            if (Convert.ToBoolean(insercao[0]))
            {
                //Caso tenha dado algum problema, define a mensagem de resposta com erro
                response = Request.CreateResponse(HttpStatusCode.OK);

                //Caso tenha dado algum problema, define a mensagem personalizada de resposta
                response.Content = new StringContent("Produto inserido com sucesso");
            }
            else
            {
                //Caso tenha dado algum problema, define a mensagem de resposta com erro
                response = Request.CreateResponse(HttpStatusCode.BadRequest);

                //Caso tenha dado algum problema, define a mensagem personalizada de resposta
                response.Content = new StringContent("Falha ao inserir o produto\n\n" + insercao[1].ToString());
            }

            //Retorna a mensagem http
            return response;

        }
        #endregion
        #region Put - Atualizar um produto na base de dados
        public HttpResponseMessage Put(Product produto) //PUT responsável por atualizar um produto na base de dados
        {
            //Cria uma mensagem de resposta HTTP
            HttpResponseMessage response = null;

            //Criado objeto que conterá o retorno. Na posição 0, como true ou false e na posição 1 como string que pode conter o erro
            List<Object> atualizacao = DAO.ProductDAO.UpdateProduct(produto);

            if (Convert.ToBoolean(atualizacao[0]))
            {
                //Caso tenha dado algum problema, define a mensagem de resposta com erro
                response = Request.CreateResponse(HttpStatusCode.OK);

                //Caso tenha dado algum problema, define a mensagem personalizada de resposta
                response.Content = new StringContent("Produto atualizado com sucesso");
            }
            else
            {
                //Caso tenha dado algum problema, define a mensagem de resposta com erro
                response = Request.CreateResponse(HttpStatusCode.BadRequest);

                //Caso tenha dado algum problema, define a mensagem personalizada de resposta
                response.Content = new StringContent("Falha ao atualizar o produto\n\n" + atualizacao[1].ToString());
            }

            //Retorna a mensagem http
            return response;

        }
        #endregion
        #region Delete - Deleta um produto da base de dados
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response;

            //Criado objeto que conterá o retorno. Na posição 0, como true ou false e na posição 1 como string que pode conter o erro
            List<Object> delecao = DAO.ProductDAO.DeleteProduct(id);

            if (Convert.ToBoolean(delecao[0]))
            {
                //Caso tenha dado algum problema, define a mensagem de resposta com erro
                response = Request.CreateResponse(HttpStatusCode.OK);

                //Caso tenha dado algum problema, define a mensagem personalizada de resposta
                response.Content = new StringContent("Produto deletado com sucesso");
            }
            else
            {
                //Caso tenha dado algum problema, define a mensagem de resposta com erro
                response = Request.CreateResponse(HttpStatusCode.BadRequest);

                //Caso tenha dado algum problema, define a mensagem personalizada de resposta
                response.Content = new StringContent("Falha ao deletar o produto\n\n" + delecao[1].ToString());
            }

            //Retorna a mensagem http
            return response;
        }
        #endregion
    }
}