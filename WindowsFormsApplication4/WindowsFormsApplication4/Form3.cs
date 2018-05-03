using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        Stack<string> s = new Stack<string>();
        public Form3(Stack<string> sss)
        {
            InitializeComponent();
            s = sss;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            int deger2 = 0;
            foreach (var item in s)
            {

                var splitString = item.Split(',');

                deger2 = 0;
                foreach (Control kontrol in this.Controls)
                {
                    if (kontrol is TextBox)
                    {
                        if (Convert.ToInt32(splitString[0]) * 9 + Convert.ToInt32(splitString[1]) == deger2)
                        {
                            kontrol.Text = splitString[2];
                            var t = Task.Delay(100);
                            t.Wait();
                            break;
                        }
                        deger2++;
                    }
                }
            }
        }
    }
}
