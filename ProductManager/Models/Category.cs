using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManager.Models
{
    public class Category
    {
        /*
            CREATE TABLE [Categoria] (
                [id] int IDENTITY (1,1) NOT NULL,
                [descricao] nvarchar(100) NOT NULL
            );
            GO

            ALTER TABLE [Categoria] ADD CONSTRAINT [PK_Categoria] PRIMARY KEY ([id]);
            GO
         */

        public int id { get; set; }
        public string descricao { get; set; }

        public Category() { }

        public Category(int _id, string _descricao)
        {
            id = _id;
            descricao = _descricao;
        }

    }
}