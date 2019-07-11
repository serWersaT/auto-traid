using System;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using MySuperTextRegex;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;

namespace WindowsFormsApp1
{
    class ConfigBase
    {
        public bool Ping(string adress)    //проверка интернет доступа к сайту с новостями
        {
            bool traid_ping = false;         
            Ping ping = new Ping();   
            PingReply pingReply = ping.Send(adress);
            traid_ping = (pingReply.Status == IPStatus.Success);
            if (traid_ping)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }



        //метод для поиска гиперссылок в html коде
        public string[] HTML_Reference(string html) 
        {
            Match match;
            string[] html_ref;
            List<string> HTML_ref = new List<string>();
            string hrefPattern = "href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))";
            match = Regex.Match(html, hrefPattern,
                             RegexOptions.IgnoreCase | RegexOptions.Compiled);
            int i = 0;
            string stroka = "";    //строка в которую помещаются ссылки для анализа

            /*НАЧАЛО ПОИСКА ССЫЛОК НА ПЕРВОЙ СТРАНИЦЕ*/

            while (match.Success)   //цикл заполнения массива. Тут записываются все 
                                    //ссылки с одной странице
            {
                stroka = match.Groups[1].ToString();   //Начало анализа ссылки
                if (!HTML_ref.Contains(match.Groups[1].ToString()))
                    HTML_ref.Add(match.Groups[1].ToString());

                match = match.NextMatch();
                i++;

            }
            int k = 0;
            html_ref = new string[i];
            foreach (var x in HTML_ref)
            {
                if (x.Contains("article") & !x.Contains("#comments"))
                    html_ref[k] = x;
                k++;
            }
            return html_ref;
        }

    }
}
