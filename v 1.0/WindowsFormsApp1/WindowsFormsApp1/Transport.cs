using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    static class Transport //используется только для передачи данных с формы в другие классы
    {
        public static string Instrument { get; set; }  //инструмент используемый на данный момент, выбирается из combobox2
        public static string Instrument_chart { get; set; }    //передает название инструмента в окно chart

        //блок переменных из класса InterfaceForm
        public static string[] Trading { get; set; }   //массив для заполнения combobox2 список торговых инструментов
        public static string[] Lever { get; set; }     //массив для заполнения combobox3 кредитное плечо
        public static string[] Categories { get; set; }    //массив для заполнения combobox1 категории валютных инструментов
        public static string CategoriesProduct { get; set; } //строка в которую заносится название категории инструмента из списка app.config 
        public static List<string> Reference_news { get; set; }    //список гиперссылок новостей о торговом инструменте
        public static List<string> List_news { get; set; }     //список заголовков новостей для listbox1
        public static List<string> List_opinion { get; set; }     //список заголовков аналитики для listbox4
        public static List<string> Reference_opinion { get; set; }     //список гиперссылок заголовков аналитики для listbox4



        public static List<string> date_time_traid { get; set; }  //список временных точек. Получен из АПИ запроса
        public static List<decimal> open_traid { get; set; }  //список точек цены открытия таймфрейма
        public static List<decimal> high_traid { get; set; }  //список точек максимальной стоимости в данном таймфрейме
        public static List<decimal> low_traid  { get; set; }  //список точек минимальной стоимости в данном таймфрейме
        public static List<decimal> close_traid { get; set; } //список точек цены закрытия таймфрейма
        public static List<int> volatility_traid { get; set; }    //список величины волатильности



        public static int CategoriesFlag { get; set; } //список торговых инструментов в зависимости от категории combobox2
        public static decimal price_close_now { get; set; } //переменная, в которую вносится информация о нынешней стоимости торгового инструмента. Обновление каждую секунду timer1
        public static string DateNow { get { return DateTime.Now.ToString(); } set { } } //сегодняшняя дата
        public static string test { get; set; }
        
    }
}
