using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace калькулятор_на_проект
{
    public partial class journalOperation : UserControl
    {
        public journalOperation()
        {
            InitializeComponent();
        }

        private void journalOperation_MouseEnter(object sender, EventArgs e) => BackColor = Color.LightGray;

        private void journalOperation_MouseLeave(object sender, EventArgs e) => BackColor = Color.White;

        private void journalOperation_MouseDown(object sender, MouseEventArgs e) => BackColor = Color.Gray;

        private void journalOperation_MouseUp(object sender, MouseEventArgs e) => BackColor = Color.White;
    }
}
