using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;

namespace ProductManager.DAO
{
    static public class CategoryDAO
    {
        #region Variáveis Globais
        //string global que contém o comando SQL que será executado
        static private string strSQL;

        //conexão com banco de dados SQLServer Compact
        private static SqlCeConnection Conn = new SqlCeConnection(Config.Config.ConnectionString);
        #endregion

        static public List<Models.Category> SelectCategories(int id = 0)
        {
            //Comando SQL que buscará todos os campos de todos as categorias na base de dados ou de uma categoria específica
            strSQL =    "SELECT categoria.id AS c_id, categoria.descricao AS c_descricao " +
                        "FROM   Categoria " +
                        ( id == 0 ? "" : "WHERE  categoria.id = @id");

            //Caso a conexão esteja fechada, a abre
            if (Conn.State != ConnectionState.Open)
                Conn.Open();

            //Cria uma lista de categorias que serão retornadas
            List<Models.Category> categories = new List<Models.Category>();

            //Cria e instancia o command que irá executar a cláusula passada
            SqlCeCommand command = new SqlCeCommand(strSQL, Conn);

            //Se o ID for diferente de 0, deve-se adicionar os parâmetros
            if (id != 0)
            {
                //Cria uma lista de parâmetros
                List<SqlCeParameter> parametros = new List<SqlCeParameter>();

                //Adiciona na lista de parâmetros, todos os parâmetros necessários para a inserção da categoria
                parametros.Add(new SqlCeParameter("@id", id));

                //Adiciona ao comando SQL todos os parâmetros criados
                for (int i = 0; i < parametros.Count; i++) command.Parameters.Add(parametros[i]);
            }

            //Cria, instancia o reader que conterá os registros retornados e executa o comando SQL
            SqlCeDataReader reader = command.ExecuteReader();

            //Enquanto houver registros a serem lidos, serão lidos
            while (reader.Read())
            {
                //Adiciona uma nova categoria na lista de categorias
                categories.Add(new Models.Category
                                {
                                    id = int.Parse(reader["c_id"].ToString()),
                                    descricao = reader["c_descricao"].ToString()
                                }
                            );
            }

            //Caso a conexão esteja aberta, a fecha
            if (Conn.State == ConnectionState.Open)
                Conn.Close();

            return categories;
        }
        static public bool InsertCategory(Models.Category categoria)
        {

            if (categoria != null)
            {
                //Comando SQL que irá inserir a categoria no banco de dados
                strSQL =    "INSERT INTO Categoria " +
                            "(descricao) " +
                            "VALUES (@descricao)";

                //Caso a conexão esteja fechada, a abre
                if (Conn.State != ConnectionState.Open)
                    Conn.Open();

                //Cria e instancia o command que irá executar a cláusula passada
                SqlCeCommand command = new SqlCeCommand(strSQL, Conn);

                //Cria uma lista de parâmetros
                List<SqlCeParameter> parametros = new List<SqlCeParameter>();

                //Adiciona na lista de parâmetros, todos os parâmetros necessários para a inserção da categoria
                parametros.Add(new SqlCeParameter("@descricao", categoria.descricao));

                //Adiciona ao comando SQL todos os parâmetros criados
                for (int i = 0; i < parametros.Count; i++) command.Parameters.Add(parametros[i]);

                try
                {

                    //Comando SQL que retorna o próximo ID de categoria
                    strSQL =    "SELECT autoinc_next ID " +
                                "FROM   information_schema.columns " +
                                "WHERE  column_name = 'id' AND table_name = 'categoria' ";

                    //Cria e instancia o command que irá executar a cláusula passada
                    SqlCeCommand command_aux = new SqlCeCommand(strSQL, Conn);

                    //Cria, instancia o reader que conterá os registros retornados e executa o comando SQL
                    SqlCeDataReader reader = command_aux.ExecuteReader();

                    //Enquanto houver registros a serem lidos, serão lidos
                    while (reader.Read())
                    {
                        //Define o ID da categoria como o registro de próximo número de ID a ser criado para a categoria
                        categoria.id = int.Parse(reader["ID"].ToString());
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
        static public bool UpdateCategory(Models.Category categoria)
        {
            if (categoria != null)
            {
                try
                {
                    //Comando SQL que atualizará a categoria na base de dados
                    strSQL =    "UPDATE Categoria " +
                                "SET    descricao = @descricao " +
                                "WHERE  id = @id";

                    //Caso a conexão esteja fechada, a abre
                    if (Conn.State != ConnectionState.Open)
                        Conn.Open();

                    //Cria e instancia o command que irá executar a cláusula passada
                    SqlCeCommand command = new SqlCeCommand(strSQL, Conn);

                    //Cria uma lista de parâmetros
                    List<SqlCeParameter> parametros = new List<SqlCeParameter>();

                    //Adiciona na lista de parâmetros, todos os parâmetros necessários para a atualização da categoria
                    parametros.Add(new SqlCeParameter("@descricao", categoria.descricao));
                    parametros.Add(new SqlCeParameter("@id", categoria.id));

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
        static public bool DeleteCategory(int id)
        {
            //Comando SQL que deletará a categoria da base de dados
            strSQL =    "DELETE Categoria " +
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

                //Adiciona na lista de parâmetros, todos os parâmetros necessários para a atualização da categoria
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