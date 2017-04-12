using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManager.Config
{
    static public class Config
    {
        static public string URLDatabase = "D:\\GoogleDrive\\Ciência da computação\\7º Semestre\\Tópicos Especiais 1\\Exercícios\\ProductManager\\database\\empresa.sdf";
        static public string ConnectionString = "Data Source=" + URLDatabase + ";Persist Security Info=False;";
    }
}