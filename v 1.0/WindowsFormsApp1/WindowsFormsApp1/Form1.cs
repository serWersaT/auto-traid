using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Threading;
using MySuperTextRegex;
using System.Net.NetworkInformation;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        InterfaceForm InterfaceFRM = new InterfaceForm();
        TraidConfig TC = new TraidConfig();
        //Transport trpt = new Transport();
        ConfigBase cfb = new ConfigBase();
        static object locker = new object();

        public string Instrument_chart { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

        }


               
        private void button5_Click(object sender, EventArgs e)
        {

            Traid tr = new Traid();
            tr.POST_Traid(comboBox2.Text, Transport.CategoriesProduct);

        }

        private void Form1_Shown(object sender, EventArgs e)   
        {
            //выводим список категорий торговых инструментов
            Transport.CategoriesFlag = comboBox1.SelectedIndex;
            //Вывести список торговых инструметов в зависимости от категории
            InterfaceFRM.InterfaceLoad();
            //заполняем combobox3 кредитное плечо
            InterfaceFRM.Leverage();
            comboBox1.Items.AddRange(Transport.Categories);
            comboBox3.Items.AddRange(Transport.Lever);
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            Transport.Instrument = comboBox2.Text;  
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //в зависимости от выбранной категории, выводим список торговых инструментов в combobox2
        {
            //очищаем содержимое combobox2 для обновления списка
            comboBox2.Items.Clear();
            Transport.CategoriesFlag = comboBox1.SelectedIndex;           
            InterfaceFRM.InterfaceCombobox2();          
            comboBox2.Items.AddRange(Transport.Trading);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {           
            if (cfb.Ping("ru.investing.com") == true)   //проверяем доступ к ресурсу
            {
                //заносим значение выбранного торгового инструмента в переменную Instrument
                Transport.Instrument = comboBox2.Text;
                NameValueCollection Investing;
                Investing = ConfigurationManager.GetSection("appSettings_Investing") as NameValueCollection;
                string url = Investing.Get(Transport.Instrument);

                //уменьшить время ожидания завершения функций программы. Однако, все равно необходимо дождаться завершения данного потока
                Thread thrad = new Thread(() =>
                {                  
                    TC.ReadyNews(url);
                });
                thrad.Start();                

                Thread thrad1 = new Thread(() =>
                {
                    TC.ReadyOpinion(url);
                });
                thrad1.Start();
                thrad.Join();
                thrad1.Join();

                listBox1.Items.Clear();
                
                    if (Transport.List_news.Count != 0)
                    {
                        for (int i = 0; i < Transport.List_news.Count; i++)
                        {
                            listBox1.Items.Add(Transport.List_news[i]);
                        }
                    }
                    else listBox1.Items.Add("Новости по данному инструменту не найдены");

              
                listBox4.Items.Clear();

                if (Transport.List_opinion.Count != 0)
                {
                    for (int i = 0; i < Transport.List_opinion.Count; i++)
                    {
                        listBox4.Items.Add(Transport.List_opinion[i]);
                    }
                }
                else listBox1.Items.Add("Новости по данному инструменту не найдены");

            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)  //список последних новостей
        {
            //разворачиваем в richtextbox1, выбранную из списка, новоть
            int number = listBox1.SelectedIndex;
            richTextBox1.Text = TC.NewsTraid(number, Transport.Reference_news);
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)  //список последних аналитических статей
        {
            //разворачиваем в richtextbox1, выбранную из списка аналитика
            int number = listBox4.SelectedIndex;
            richTextBox1.Text = TC.NewsTraid(number, Transport.Reference_opinion);
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)  //открываем окно с графиком 
        {
            Transport.Instrument_chart = comboBox2.Text;
            //MessageBox.Show(Instrument_chart);
            chart ch = new chart();     
            ch.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Thread thr = new Thread(()=> {  //каждую секунду обновляем информацию о стоимости торгового инструмента
                TC.Traid_point(comboBox2.Text, "m1", Transport.CategoriesProduct);
                int i = Transport.close_traid.Count - 1;
                Transport.price_close_now = Transport.close_traid[i];
                label28.Text = "Цена: " + Transport.close_traid[i];
            });
        }
    }
}
