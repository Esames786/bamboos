using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Skyview
{
    public partial class circlarprogress : Form
    {
        int i = 0;
        public circlarprogress()
        {
            InitializeComponent(); circularProgressBar1.Value = 0; timer1.Start(); MessageBox.Show("Acknowledgement...Kindly Change the Date");
        } 

        private void circularprogress_Load(object sender, EventArgs e)
        {
   //  circularProgressBar1.Minimum = 0;   circularProgressBar1.Maximum = 100;
        }
         
        private void hit()
        {
        //for (int i = 0; i <= 100; i++)
        //{
        circularProgressBar1.Value = i+=10;  circularProgressBar1.Update(); label1.Text = circularProgressBar1.Value.ToString() + "%";
         //}
        if (circularProgressBar1.Value == 100)
        {
            timer1.Stop(); this.Hide(); Master_Form mf = new Master_Form(); mf.Show();
        }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            hit();
        }

        private void circlarprogress_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void circlarprogress_Load_1(object sender, EventArgs e)
        {

        }


    }
}
