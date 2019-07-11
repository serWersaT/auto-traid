using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;

namespace WindowsFormsApp1
{
    class InterfaceForm 
    {    
        public string TypeInvesting { get; set; }

        List<string> Trading_instrument = new List<string>(); /*{ get; set; }*/
        public void InterfaceLoad()     //сисок торговых категорий combobox1
        {
            Transport.Categories = new string[8];
            Transport.Categories[0] = "Валюты";
            Transport.Categories[1] = "Криптовалюты";
            Transport.Categories[2] = "Индексы";
            Transport.Categories[3] = "Нефть и газ";
            Transport.Categories[4] = "Металлы";
            Transport.Categories[5] = "Акции";
            Transport.Categories[6] = "Сельхозтовары";
            Transport.Categories[7] = "ETFs";
        }

        NameValueCollection Libertex;
        public void InterfaceCombobox2()
        { 
            switch (Transport.CategoriesFlag)     //список торговых инструментов в зависимости от категории combobox2
            {
                case 0:
                    Libertex = ConfigurationManager.GetSection("appSettings_Currency_Libertex") as NameValueCollection;
                    foreach (var x in Libertex.AllKeys)
                    {
                        Transport.CategoriesProduct = "appSettings_Currency_Libertex";
                        Trading_instrument.Add(x);
                    }
                    break;

                case 1:
                    Libertex = ConfigurationManager.GetSection("appSettings_Digital_currency_Libertex") as NameValueCollection;
                    foreach (var x in Libertex.AllKeys)
                    {
                        Transport.CategoriesProduct = "appSettings_Digital_currency_Libertex";
                        Trading_instrument.Add(x);
                    }
                    break;

                case 2:
                    Libertex = ConfigurationManager.GetSection("appSettings_Indexes_Libertex") as NameValueCollection;
                    foreach (var x in Libertex.AllKeys)
                    {
                        Transport.CategoriesProduct = "appSettings_Indexes_Libertex";
                        Trading_instrument.Add(x);
                    }
                    break;

                case 3:
                    Libertex = ConfigurationManager.GetSection("appSettings_Oil_and_gas_Libertex") as NameValueCollection;
                    foreach (var x in Libertex.AllKeys)
                    {
                        Transport.CategoriesProduct = "appSettings_Oil_and_gas_Libertex";
                        Trading_instrument.Add(x);
                    }
                    break;

                case 4:
                    Libertex = ConfigurationManager.GetSection("appSettings_Metals_Libertex") as NameValueCollection;
                    foreach (var x in Libertex.AllKeys)
                    {
                        Transport.CategoriesProduct = "appSettings_Metals_Libertex";
                        Trading_instrument.Add(x);
                    }
                    break;

                case 5:
                    Libertex = ConfigurationManager.GetSection("appSettings_Stock_Libertex") as NameValueCollection;
                    foreach (var x in Libertex.AllKeys)
                    {
                        Transport.CategoriesProduct = "appSettings_Stock_Libertex";
                        Trading_instrument.Add(x);
                    }
                    break;

                case 6:
                    Libertex = ConfigurationManager.GetSection("appSettings_Agricultural_products_Libertex") as NameValueCollection;
                    foreach (var x in Libertex.AllKeys)
                    {
                        Transport.CategoriesProduct = "appSettings_Agricultural_products_Libertex";
                        Trading_instrument.Add(x);
                    }
                    break;

                case 7:
                    Libertex = ConfigurationManager.GetSection("appSettings_ETFs_Libertex") as NameValueCollection;
                    foreach (var x in Libertex.AllKeys)
                    {
                        Transport.CategoriesProduct = "appSettings_ETFs_Libertex";
                        Trading_instrument.Add(x);
                    }
                    break;
            }

            if (Trading_instrument != null)
            {

                int i = 0;
                int k = Trading_instrument.Count;
                Transport.Trading = new string[k];
                foreach (var x in Trading_instrument)
                {
                    Transport.Trading[i] = x;
                    i++;
                }
                Trading_instrument.Clear();
            }
        }


        public void Leverage()  //заполняем combobox3 кредитное плечо
        {
            Transport.Lever = new string[50];
            for (int g = 0; g < 50; g++)
            {
                Transport.Lever[g] = (g+1).ToString();
            }
        }
    }
}
