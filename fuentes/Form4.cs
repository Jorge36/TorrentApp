using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace pro1
{
    public partial class Form4 : Form
    {
        Repositorio rep;

        public Form4(Repositorio rep)
        {
            InitializeComponent();
            this.rep = rep;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(rep.buscar(rep.getArbol(),textBox1.Text).ToArray());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
