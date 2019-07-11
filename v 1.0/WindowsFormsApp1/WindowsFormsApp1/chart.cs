using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using System.Text.RegularExpressions;



namespace WindowsFormsApp1
{
    public partial class chart : Form
    {
        TraidConfig TC = new TraidConfig();
        public chart()
        {
            InitializeComponent();
            
        }

        private void chart_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;

            label4.Text = Transport.Instrument_chart;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            TC.Traid_point(Transport.Instrument_chart, comboBox1.Text, Transport.CategoriesProduct);    //получаем список значений цены торгового элемента
            //Draw(400, 272, 20, 20);

            decimal chart = (Transport.high_traid.Max() - Transport.low_traid.Min());
            int x = 10;
            for (int i = 0; i < Transport.low_traid.Count - 1; i++)
            {
                decimal local_high = Transport.high_traid.Max() - Transport.high_traid[i];
                decimal local_low = Transport.low_traid[i] - Transport.low_traid.Min();
                decimal local_open = Transport.open_traid.Max() - Transport.open_traid[i];
                decimal local_close = Transport.close_traid.Max() - Transport.close_traid[i];

                int y = 0;
                int h = 0;
                decimal chart_step = 0;
                while (chart_step <= local_open)
                {
                    chart_step = chart_step + chart / 1000;
                    y++;
                }

                chart_step = 0;

                while (chart_step <= local_close)
                {
                    chart_step = chart_step + chart / 1000;
                    h++;
                }

                Draw(x, y, 5, h);
                x = x + 10;
            }
        }



        public void Draw(int x, int y, int width, int height)
        {
            Pen myPen = new Pen(Color.Red, 1);
            Graphics myGraphics = pictureBox1.CreateGraphics();
            myGraphics.DrawRectangle(myPen, x, y, width, height);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            TC.Traid_point(Transport.Instrument_chart, comboBox1.Text, Transport.CategoriesProduct);    //получаем список значений цены торгового элемента
            //Draw(400, 272, 20, 20);

            decimal chart = (Transport.high_traid.Max() - Transport.low_traid.Min());
            int x = 10;
            for (int i = 0; i < Transport.low_traid.Count-1; i++)
            {
                decimal local_high = Transport.high_traid.Max() - Transport.high_traid[i];
                decimal local_low = Transport.low_traid[i] - Transport.low_traid.Min();
                decimal local_open = Transport.open_traid.Max() - Transport.open_traid[i];
                decimal local_close = Transport.close_traid.Max() - Transport.close_traid[i];

                int y = 0;
                decimal chart_step = 0;
                while (chart_step<=local_high)
                {
                    chart_step = chart_step + chart / 1000;
                    y++;
                }

                chart_step = 0;

                while (chart_step <= local_low)
                {
                    chart_step = chart_step + chart / 1000;
                    y++;
                }

                Draw(x, y, 5, 20);
                x = x + 10;
            }

            /*int x1 = Convert.ToInt32(textBox1.Text);
            int y1 = Convert.ToInt32(textBox2.Text);
            int h1 = Convert.ToInt32(textBox3.Text);
            int w1 = Convert.ToInt32(textBox4.Text);
            Draw(x1,y1,h1,w1);*/
            
        }
    }
}
