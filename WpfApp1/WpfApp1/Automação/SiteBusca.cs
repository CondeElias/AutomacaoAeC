using AeCBot.Dominio;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AeCBot.Front
{
    public class SiteBusca
    {
        private const string X_PATH_BUSCA = "//*[@id='header']/div[2]/div/div/div/div/ul[2]/li[2]/a";
        private const string X_PATH_CAMPO_BUSCA = "//*[@id='form']/input";
        private const string X_PATH_ACEITAR = "//*[@id='adopt-accept-all-button']";
        private readonly IWebDriver driver;

        public SiteBusca(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void Buscar(string termo)
        {
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://www.aec.com.br/");
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(X_PATH_ACEITAR)).Click();
            driver.FindElement(By.XPath(X_PATH_BUSCA)).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath(X_PATH_CAMPO_BUSCA)).SendKeys(termo);
            driver.FindElement(By.XPath(X_PATH_CAMPO_BUSCA)).SendKeys(Keys.Enter);
        }

        public IEnumerable<ResultadoBusca> ObterResultados()
        {
            var titulos = driver.FindElements(By.CssSelector("h3.tres-linhas"));
            var areas = driver.FindElements(By.CssSelector("span.hat"));
            var autores = ObterAutor();
            var descricoes = driver.FindElements(By.CssSelector("p.duas-linhas"));
            var datas = ObterData();
            

            List<ResultadoBusca> resultados = new List<ResultadoBusca>();

            for (int i = 0; i < titulos.Count; i++)
            {
                var resultado = new ResultadoBusca
                {
                    Titulo = titulos[i].Text,
                    Area = areas[i].Text,
                    Autor = autores[i].Autor,
                    Descricao = descricoes[i].Text,
                    Data = datas[i].Data,
                };
                resultados.Add(resultado);
            }

            return resultados;
        }
        public ResultadoDatas[] ObterData() 
        {
            IList<IWebElement> DataCompleta = driver.FindElements(By.CssSelector("small"));
            List<ResultadoDatas> datas = new List<ResultadoDatas>();
            for (int i = 0; i < DataCompleta.Count; i++)
            {
                string texto = DataCompleta[i].Text;
                string pattern = @"\d{2}/\d{2}/\d{4}";
                Match match = Regex.Match(texto, pattern);
                string data = match.Value;
                var resultado = new ResultadoDatas
                {
                    Data = data,
                };
                datas.Add(resultado);
            }
            return datas.ToArray();
        }
        public ResultadoAutor[] ObterAutor()
        {
            IList<IWebElement> AutorCompleto = driver.FindElements(By.CssSelector("small"));
            List<ResultadoAutor> autores = new List<ResultadoAutor>();
            for (int i = 0;i < AutorCompleto.Count;i++) 
            {
                string texto = AutorCompleto[i].Text;
                string pattern = @"^Publicado por (.+?) em";
                Match match = Regex.Match(texto, pattern);
                string autor = match.Groups[1].Value;
                var resultado = new ResultadoAutor
                {
                    Autor = autor,
                };
                autores.Add(resultado);
            }
            return autores.ToArray();
        }

    }
}
