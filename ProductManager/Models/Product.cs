using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManager.Models
{
    public class Product
    {
        /*
            CREATE TABLE [Produto] (
                [id] int IDENTITY (1,1) NOT NULL,
                [descricao] nvarchar(100) NOT NULL,
                [categoria] nvarchar(100) NOT NULL,
                [preco] float NOT NULL
            );
            GO

            ALTER TABLE [Produto] ADD CONSTRAINT [PK_Produto] PRIMARY KEY ([id]);
            GO
         */

        public int id { get; set; }
        public string descricao { get; set; }
        public string categoria { get; set; }
        public float preco { get; set; }
    }
}