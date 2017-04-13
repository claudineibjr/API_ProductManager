using ProductManager.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductManager.Controllers
{
    public class CategoryController : ApiController
    {
        #region Variáveis globais
        #endregion
        #region Get - Retornar todas as categorias ou uma categoria específica da base de dados
        public IHttpActionResult Get()//GET que retorna todos as categorias
        {
            // Retorna todas as categorias
            return Ok(DAO.CategoryDAO.SelectCategories());
        }

        public IHttpActionResult Get(int id)//Get que retornará uma categoria específica
        {
            // Retorna a categoria selecionada
            return Ok(DAO.CategoryDAO.SelectCategories(id));
        }
        #endregion
        #region Post - Inserir uma categoria na base de dados
        public HttpResponseMessage Post(Category categoria)//POST que irá inserir uma categoria na base de dados conforme objeto passado como parâmetro
        {
            //Cria uma mensagem de resposta HTTP
            HttpResponseMessage response = null;

            if (DAO.CategoryDAO.InsertCategory(categoria))
            {
                //Caso tenha dado algum problema, define a mensagem de resposta com erro
                response = Request.CreateResponse(HttpStatusCode.OK);

                //Caso tenha dado algum problema, define a mensagem personalizada de resposta
                response.Content = new StringContent("Categoria inserida com sucesso");
            }
            else
            {
                //Caso tenha dado algum problema, define a mensagem de resposta com erro
                response = Request.CreateResponse(HttpStatusCode.BadRequest);

                //Caso tenha dado algum problema, define a mensagem personalizada de resposta
                response.Content = new StringContent("Falha ao inserir a categoria");
            }

            //Retorna a mensagem http
            return response;

        }
        #endregion
        #region Put - Atualizar uma categoria na base de dados
        public HttpResponseMessage Put(Category categoria) //PUT responsável por atualizar uma categoria na base de dados
        {
            //Cria uma mensagem de resposta HTTP
            HttpResponseMessage response = null;

            if (DAO.CategoryDAO.UpdateCategory(categoria))
            {
                //Caso tenha dado algum problema, define a mensagem de resposta com erro
                response = Request.CreateResponse(HttpStatusCode.OK);

                //Caso tenha dado algum problema, define a mensagem personalizada de resposta
                response.Content = new StringContent("Categoria atualizada com sucesso");
            }
            else
            {
                //Caso tenha dado algum problema, define a mensagem de resposta com erro
                response = Request.CreateResponse(HttpStatusCode.BadRequest);

                //Caso tenha dado algum problema, define a mensagem personalizada de resposta
                response.Content = new StringContent("Falha ao atualizar a categoria");
            }

            //Retorna a mensagem http
            return response;

        }
        #endregion
        #region Delete - Deleta uma categoria da base de dados
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response;

            if (DAO.CategoryDAO.DeleteCategory(id))
            {
                //Caso tenha dado algum problema, define a mensagem de resposta com erro
                response = Request.CreateResponse(HttpStatusCode.OK);

                //Caso tenha dado algum problema, define a mensagem personalizada de resposta
                response.Content = new StringContent("Categoria deletada com sucesso");
            }
            else
            {
                //Caso tenha dado algum problema, define a mensagem de resposta com erro
                response = Request.CreateResponse(HttpStatusCode.BadRequest);

                //Caso tenha dado algum problema, define a mensagem personalizada de resposta
                response.Content = new StringContent("Falha ao deletar a categoria");
            }

            //Retorna a mensagem http
            return response;
        }
        #endregion
    }
}