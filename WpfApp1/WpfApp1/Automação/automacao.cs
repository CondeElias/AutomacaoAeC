using AeCBot.Infraestrutura;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AeCBot.Front
{
    public class automacao
    {
        public void Iniciar()
        {
            var dbConnection = DbConnectionFactory.CreateConnection();
            var options = new ChromeOptions();
            var driver = new ChromeDriver(options);
            var siteBusca = new SiteBusca(driver);
            var buscaAec = new BuscaAec(dbConnection, siteBusca);

            buscaAec.Buscar("Automação");

            driver.Quit();
        }
    }
}
