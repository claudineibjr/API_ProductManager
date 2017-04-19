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
                [id_categoria] int NULL,
                [preco] float NOT NULL
            );
            GO

            ALTER TABLE [Produto] ADD CONSTRAINT [PK_Produto] PRIMARY KEY ([id]);
            GO
            ALTER TABLE [Produto] ADD CONSTRAINT [FK_Produto] FOREIGN KEY ([id_categoria]) REFERENCES Categoria ([id]);
            GO
         */

        public int id { get; set; }
        public string descricao { get; set; }
        public Category categoria { get; set; }
        public float preco { get; set; }

        public Product() { }

        public Product(int _id, string _descricao, float _preco, Category _categoria = null)
        {
            id = _id;
            descricao = _descricao;
            preco = _preco;
            categoria = _categoria;
        }

        override
        public string ToString() {
            return "ID: " + this.id + "\n" +
                    "Descrição: " + this.descricao + "\n" +
                    "Preço: " + this.preco + "\n" +
                    "Categoria: " + this.categoria.id + " - " + this.categoria.descricao;
        }

    }
}