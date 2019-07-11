using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Windows.Forms;
using MySuperTextRegex;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace WindowsFormsApp1
{
    class TraidConfig
    {
        ConfigBase cfb = new ConfigBase();

        string NewsResponse(string inst)   //переменная в которую заносится адресс страницы для получения htmk кода
        {           
            string isp = "";

            if (inst != null)//не по всем инструментам есть новости.
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(inst);
                request.UserAgent = "My applicartion name";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default, true, 8192))
                {
                    isp = reader.ReadToEnd();
                    byte[] www = Encoding.Default.GetBytes(isp);    //полученный html текст преобразуем в массиб байт
                    isp = Encoding.UTF8.GetString(www);     //перекодируем в формат UTF-8 иначе текст отображается кракозябрами
                                                            //return isp;     //возвращаем полученный текст
                }
                return isp;
            }
            else return isp = "По данноу торговому инструменту новостей не найдено";
        }

        private string news_html_regex(string news, List<string> lists) //метод очищает текс, составляет список гиперссылок и добавляет в список
        {
            news = Myregex.MyTextDelete(news, "<!DOCTYPE HTML>", "<!-- infoBox -->");
            news = Myregex.MyTextDelete(news, "doubleLineSeperator", "</html>");
            news = Myregex.MyTextDelete(news, "<span class=" + '"' + "noBold", "</span>");
            news = Myregex.MyTextDelete(news, "<script>", "</script>");
            news = Myregex.MyTextDelete(news, "<div ", "<div class=" + "mediumTitle1" + ">");

            //сохраняем гиперссылки всех новостей
            foreach (var x in cfb.HTML_Reference(news))
            {
                if (x != null)
                    lists.Add("https://ru.investing.com" + x);
            }

            news = Regex.Replace(news, "</article>", "*", RegexOptions.IgnoreCase);     //метка для разделения текста по новостям
                                                                                        //далее финальная очистка текста от html тегов
            news = Myregex.HtmlRegexSpeed(news);
            news = Regex.Replace(news, "Investing.com ", "", RegexOptions.IgnoreCase);
            news = Regex.Replace(news, "-&nbsp", "ДАТА: ", RegexOptions.IgnoreCase);
            news = Regex.Replace(news, "&nbsp;", "", RegexOptions.IgnoreCase);
            news = Regex.Replace(news, "&laquo;", "", RegexOptions.IgnoreCase);
            news = Regex.Replace(news, "&ndash;", "", RegexOptions.IgnoreCase);
            news = Regex.Replace(news, "&mdash;", "", RegexOptions.IgnoreCase);
            news = Regex.Replace(news, "<div class=", "", RegexOptions.IgnoreCase);
            news = Regex.Replace(news, ";", "", RegexOptions.IgnoreCase);
            news = Regex.Replace(news, "\t", "", RegexOptions.IgnoreCase);
            news = Regex.Replace(news, "  ", "", RegexOptions.IgnoreCase);
            return news;
        } 


        public void ReadyNews(string str) //метод очищает входной html код и выдает строку со списком новостей
        {
            string news = "";

            Transport.Reference_news = new List<string>();
            Transport.List_news = new List<string>();


            news = NewsResponse(str + "-news");

            //если страница не найдена, то проверить новости по другому адресу
            if (news.Contains("Ошибка 404: Страница не найдена"))
            {
                news = NewsResponse(str + "/news");
            }
            else
                //если новости найдены сразу, то выволнить очистку html кода для получения чистого текста
            if (!news.Contains("Ошибка 404: Страница не найдена"))
            {              
                news=news_html_regex(news, Transport.Reference_news);

                string list_text = "";                
                list_text = "";
                int k = 0;
                for (int i = 0; i <= news.Length - 1; i++)
                {
                    if (news[i] != '*')
                    {
                        list_text += news[i];
                    }

                    if (news[i] == '*')
                    {
                        Transport.List_news.Add(list_text);
                        list_text = "";
                        k++;
                    }
                }
            }
            else
            {
                news = "По данному торговому инструменту новости не найдены";
                Transport.List_news.Add("По данному торговому инструменту новости не найдены");
            }
          
        }


        public string NewsTraid(int number, List<string> lst) //в зависимости от выбраннй строки из списка новостей загружаем html код и очищаем его
        {
            string news = "";
            news = NewsResponse(lst[number]);
            if (!news.Contains("Ошибка 404: Страница не найдена"))      //если по неизвестным причинам страница работвать не будет, например на доработке 
            {
                news = Myregex.MyTextDelete(news, "<!DOCTYPE HTML>", "<div class=" + '"' + "WYSIWYG articlePage" + '"' + ">");
                news = Myregex.MyTextDelete(news, "<!-- /6938/FP_RU_site/FP_RU_TextNote -->", "</html>");
                news = Myregex.HtmlRegexSpeed(news);

                news = Regex.Replace(news, "Investing.com ", "", RegexOptions.IgnoreCase);
                news = Regex.Replace(news, "&mdash;", "", RegexOptions.IgnoreCase);
                news = Regex.Replace(news, "&laquo;", "", RegexOptions.IgnoreCase);
                news = Regex.Replace(news, "  ", "", RegexOptions.IgnoreCase);
                news = Regex.Replace(news, "&copy;", "", RegexOptions.IgnoreCase);
                news = Regex.Replace(news, "&raquo;", "", RegexOptions.IgnoreCase);
                news = Regex.Replace(news, "&ndash;", "", RegexOptions.IgnoreCase);
            }
                return news;
        }


        public void ReadyOpinion(string url)
        {
            string news = "";
            Transport.Reference_opinion = new List<string>();
            Transport.List_opinion = new List<string>();

            news = NewsResponse(url + "-opinion");
            //если страница не найдена, то проверить новости по другому адресу
            if (news.Contains("Ошибка 404: Страница не найдена"))
            {
                news = NewsResponse(url + "/opinion");
            }
            else
                //если новости найдены сразу, то выволнить очистку html кода для получения чистого текста
            if (!news.Contains("Ошибка 404: Страница не найдена"))
            {              
                news=news_html_regex(news, Transport.Reference_opinion);

                string list_text = "";                
                list_text = "";
                int k = 0;
                for (int i = 0; i <= news.Length - 1; i++)
                {
                    if (news[i] != '*')
                    {
                        list_text += news[i];
                    }

                    if (news[i] == '*')
                    {
                        Transport.List_opinion.Add(list_text);
                        list_text = "";
                        k++;
                    }

                }               
                                          
            }
            else
            {
                news = "По данному торговому инструменту нет аналитических данных";
                Transport.List_opinion.Add("По данному торговому инструменту нет аналитических данных");
            }
        }



        /*public string test(string url)
        {
            string text = "";
            //text = NewsResponse(url);
            //text = Regex.Replace(text,'"'.ToString(), " ", RegexOptions.IgnoreCase);           
            //text = Myregex.MyTextDelete(text, "<!DOCTYPE html>", "(function () {");
            ChromeOptions opt = new ChromeOptions();
            opt.AddArgument("headlesSet");
            var driver = new ChromeDriver();            
            //driver.Manage().Window.Minimize();
            driver.Navigate().GoToUrl(url);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            System.Threading.Thread.Sleep(10000);
            text = driver.PageSource;
            return text;
        }*/



        public void Traid_point(string instr, string time, string categories)   //процедура составляет список точек открытия, точек закрытия, точек максимальных и точек минимальных 
        {
            NameValueCollection Investing;
            Investing = ConfigurationManager.GetSection("appSettings_API_Libertex") as NameValueCollection;
            string url = Investing.Get("API");

            Investing = ConfigurationManager.GetSection(categories) as NameValueCollection;

            string url_ref = Investing.Get(instr);
            url_ref = Regex.Replace(url_ref, "https://app.libertex.org/products/", "");
            url_ref = Regex.Replace(url_ref, "agriculture/", "");
            url_ref = Regex.Replace(url_ref, "currency/", "");
            url_ref = Regex.Replace(url_ref, "crypto/", "");
            url_ref = Regex.Replace(url_ref, "indexes/", "");
            url_ref = Regex.Replace(url_ref, "energetics/", "");
            url_ref = Regex.Replace(url_ref, "metal/", "");
            url_ref = Regex.Replace(url_ref, "stock/", "");
            url_ref = Regex.Replace(url_ref, "etf/", "");
            url_ref = Regex.Replace(url_ref, "/", "");

            url = url + url_ref + "&interval=" + time;

            var webClient = new WebClient();
            string listing = webClient.DownloadString(url);
            //с помощью команд replace уменьшаем количество символов в тексте
            listing = Regex.Replace(listing, "Result", "");
            listing = Regex.Replace(listing, "Error", "");
            listing = Regex.Replace(listing, "CreatedDate", "");
            listing = Regex.Replace(listing, "RateHistory", "");
            listing = Regex.Replace(listing, "null", "");
            listing = Regex.Replace(listing, "ExpirationHistory", "");
            listing = Regex.Replace(listing, "dt", "d");
            listing = Regex.Replace(listing, '"'.ToString(), "");
            listing = Regex.Replace(listing, ",", "*");
            listing = Regex.Replace(listing, "{", "");
            listing = Regex.Replace(listing, "}", "");
            listing = Regex.Replace(listing, "T", " ");
            listing = Regex.Replace(listing, "[[]", "");
            listing = Regex.Replace(listing, "]", "");


            //создаем ряд списков для дальнейшего построения графиков

            Transport.date_time_traid = new List<string>();
            Transport.open_traid = new List<decimal>();
            Transport.high_traid = new List<decimal>();
            Transport.low_traid = new List<decimal>();
            Transport.close_traid = new List<decimal>();
            Transport.volatility_traid = new List<int>();

            //цикл перебирает текст, полученный гет запросом, для создания массивов со значениями цены торгового инструмента
            string date = "";
            string open = "";
            string high = "";
            string low = "";
            string close = "";
            string volatility = "";
            string flag = "";
            for (int i = 0; i < listing.Length; i++)
            {
                if (listing[i] == 'd') flag = "date";
                if (listing[i] == 'o') flag = "open";
                if (listing[i] == 'h') flag = "high";
                if (listing[i] == 'l') flag = "low";
                if (listing[i] == 'c') flag = "close";
                if (listing[i] == 'v') flag = "volatility";

                switch (flag)
                {
                    case "date":
                        {
                            if (listing[i] != '*')
                                date += listing[i];

                            if (listing[i] == '*')
                            {
                                date = Regex.Replace(date, "d:", "");
                                if (date != "") Transport.date_time_traid.Add(date);
                                date = "";
                            }
                            break;
                        }

                    case "open":
                        {
                            if (listing[i] == '.')
                            {
                                string str = ",";
                                open += str;
                            }
                            else
                                if (listing[i] != '*')
                                open += listing[i];


                            if (listing[i] == '*')
                            {
                                open = Regex.Replace(open, " ", "");
                                open = Regex.Replace(open, "o", "");
                                open = Regex.Replace(open, ":", "");
                                if (open != "") Transport.open_traid.Add(Convert.ToDecimal(open));
                                open = "";
                            }
                            break;
                        }

                    case "high":
                        {
                            if (listing[i] == '.')
                            {
                                string str = ",";
                                high += str;
                            }
                            else
                                if (listing[i] != '*')
                                high += listing[i];


                            if (listing[i] == '*')
                            {
                                high = Regex.Replace(high, " ", "");
                                high = Regex.Replace(high, "h", "");
                                high = Regex.Replace(high, ":", "");
                                if (high != "") Transport.high_traid.Add(Convert.ToDecimal(high));
                                high = "";
                            }
                            break;
                        }

                    case "low":
                        {
                            if (listing[i] == '.')
                            {
                                string str = ",";
                                low += str;
                            }
                            else
                                if (listing[i] != '*')
                                low += listing[i];


                            if (listing[i] == '*')
                            {
                                low = Regex.Replace(low, " ", "");
                                low = Regex.Replace(low, "l", "");
                                low = Regex.Replace(low, ":", "");
                                if (low != "") Transport.low_traid.Add(Convert.ToDecimal(low));
                                low = "";
                            }
                            break;
                        }

                    case "close":
                        {
                            if (listing[i] == '.')
                            {
                                string str = ",";
                                close += str;
                            }
                            else
                                if (listing[i] != '*')
                                close += listing[i];


                            if (listing[i] == '*')
                            {
                                close = Regex.Replace(close, " ", "");
                                close = Regex.Replace(close, "c", "");
                                close = Regex.Replace(close, ":", "");
                                if (close != "") Transport.close_traid.Add(Convert.ToDecimal(close));
                                close = "";
                            }
                            break;
                        }

                    case "volatility":
                        {
                            if (listing[i] != '*')
                                volatility += listing[i];

                            if (listing[i] == '*')
                            {
                                volatility = Regex.Replace(volatility, " ", "");
                                volatility = Regex.Replace(volatility, "v", "");
                                volatility = Regex.Replace(volatility, ":", "");
                                if (volatility != "") Transport.volatility_traid.Add(Convert.ToInt32(volatility));
                                volatility = "";
                            }
                            break;
                        }
                }


            }
        }

    }
}
