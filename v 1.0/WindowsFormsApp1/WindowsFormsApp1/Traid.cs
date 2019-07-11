using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Traid
    {
        public void POST_Traid(string instr,  string categories)
        {
            /*NameValueCollection Investing;
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
            MessageBox.Show(url_ref);*/


            string ProxyString = "";
            string URI = @"https://app.libertex.org/spa/investing/open-position"; ;
            string Parameters = "symbol=USDSEK&sumInv=50&mult=5&direction=reduction&number=12318195401&rate=9.275139999999999&spread=0.0038299999999999996&time=1561731033000" +
                "&isAutoIncreaseEnabled=0&csrfToken=3f1e3d4953a0dafce50d9df780002c55-99a786c105f3ce28216311484c8e45aa";

            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true); 
            req.ContentType = "application/json";
            req.Method = "POST";
            //byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Parameters);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream(); // создаем поток 
            os.Write(bytes, 0, bytes.Length); // отправляем в сокет 
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null)
            {
                MessageBox.Show("Что то ответ пустой");
                return;
            }
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            MessageBox.Show(sr.ReadToEnd().Trim());
        }
    }
}
