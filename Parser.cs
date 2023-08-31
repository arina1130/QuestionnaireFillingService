using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using QuestionnaireFillingService.Models;
using System.Net;
using System.Text;

namespace QuestionnaireFillingService
{
    public static class Parser
    {
        private static ChromeOptions options;
        private static IWebDriver driver;
        static Parser()
        {
            options = new ChromeOptions();
            options.AddArgument("headless");

            //string path = Directory.GetCurrentDirectory();
            //driver = new ChromeDriver(path, options);
            driver = new ChromeDriver(options);
        }
        /// <summary>
        /// Метод получения объекта LegalEntity для организации
        /// </summary>
        public static LegalEntity GetLegalEntityOrganization(string Tin)
        {
            try
            {
                // Получение html
                String urlAddress = "https://www.rusprofile.ru/search?query=" + Tin;
                driver.Url = urlAddress;

                // Ищем имя
                WebElement elem1 = (WebElement)driver.FindElement(By.XPath(@".//div[@class='company-header']"));
                string elem1_str = elem1.Text;
                string[] arr1 = elem1_str.Replace("\r", "").Split('\n');

                string name = arr1[0];

                // Ищем реквизиты
                WebElement elem2 = (WebElement)driver.FindElement(By.XPath(@".//div[@class='company-requisites']"));

                string elem2_str = elem2.Text;
                string[] arr2 = elem2_str.Replace("\r", "").Split('\n');

                string ogrn = arr2[1];
                string inn = arr2[4];
                string date = arr2[7];

                // Создание объекта LegalEntity
                // string fullName, string abbreviation, String registrationDate, uint tin, string pathTin, uint msrn, string pathMsrn, string pathUsrie, string pathRoomRental, bool noContract = false
                return new LegalEntity(name, "", DateTime.Parse(date), inn, "", ogrn, "", "", "");
            }
            catch (Exception e)
            {
                string s = e.Message;
            }

            return new LegalEntity();
        }

        /// <summary>
        /// Метод получения объекта LegalEntity для частного предпринимателя
        /// </summary>
        public static LegalEntity GetLegalEntityPersonal(string Tin)
        {
            try
            {
                // Получение html
                String urlAddress = "https://www.rusprofile.ru/search?query=" + Tin + "&type=ip";

                /*ChromeOptions options = new ChromeOptions();
                options.AddArgument("headless");

                IWebDriver driver = new ChromeDriver(options);*/
                driver.Url = urlAddress;

                // Ищем имя
                WebElement elem1 = (WebElement)driver.FindElement(By.XPath(@".//div[@class='company-header']"));
                string elem1_str = elem1.Text;
                string[] arr1 = elem1_str.Replace("\r", "").Split('\n');

                string name = arr1[0];

                // Ищем реквизиты
                WebElement elem2 = (WebElement)driver.FindElement(By.XPath(@".//div[@class='company-requisites']"));

                string elem2_str = elem2.Text;
                string[] arr2 = elem2_str.Replace("\r", "").Split('\n');

                string ogrn = arr2[5];
                string inn = arr2[8];
                string date = arr2[10];

                // Создание объекта LegalEntity
                // long tin, string pathTin, long msrnie, string pathMsrnie, DateTime registrationDate, string pathUsrie, string pathRoomRental, bool noContract = false
                return new LegalEntity(inn, "", ogrn, "", DateTime.Parse(date), "", "");
            }
            catch (Exception e)
            {
            }

            return new LegalEntity();
        }

        /// <summary>
        /// Метод получения объекта BankDetail
        /// </summary>
        public static BankDetail GetBankDetail(string Bic)
        {
            try
            {
                // Получение html
                String urlAddress = "https://bik10.ru/" + Bic;

                driver.Url = urlAddress;

                // Ищем имя и реквизиты
                WebElement elem1 = (WebElement)driver.FindElement(By.CssSelector("main[class='col-md-8']"));
                string elem1_str = elem1.Text;
                string[] arr1 = elem1_str.Replace("\r", "").Split('\n');

                string name = arr1[2].Replace("Наименование:", "").Trim();
                string bic = arr1[6].Replace("БИК:", "").Trim();
                string cor = arr1[7].Replace("Корреспондентский счет:", "").Trim();

                // Создание объекта BankDetail
                // string bic, string bankName, string correspondentAccount
                return new BankDetail(bic, name, cor);
            }
            catch (Exception e)
            {
            }

            return new BankDetail();
        }
    }
}
