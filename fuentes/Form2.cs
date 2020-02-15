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
    public partial class Form2 : Form
    {
        Repositorio rep=null;
        public String pathmedio = "";
        public bool editar = false;

        public Form2(Repositorio r, String titulo)
        {
            InitializeComponent();
            rep = r;
            this.Text = titulo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            foreach (String path in openFileDialog1.FileNames)
            {
                if (!listBox1.Items.Contains(path))
                {
                    listBox1.Items.Add(path);
                }
            }
        }

        private bool validarNombre(String cadena)
        {
            if (cadena.Length == 0)
            {
                return false;
            }
            foreach (Char c in cadena)
            {
                if (!Char.IsLetterOrDigit(c))
                {
                    return false;                    
                }
            }
            return true;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            mostrarError("");
            if (validarNombre(textBox1.Text))
            {
                if (!rep.existeRecurso(textBox1.Text, rep.getArbol().TreeView.SelectedNode))
                {
                    List<ArchivoTorrent> listaT = new List<ArchivoTorrent>();
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        listaT.Add(new ArchivoTorrent((String)listBox1.Items[i]));
                    }

                    textBox2.Text = textBox2.Text.Replace("<", "[");
                    textBox2.Text = textBox2.Text.Replace(">", "]");                    
                    if (editar)
                    {                    
                        rep.eliminar();
                        editar = false;
                    }   
                    String p;
                    RecursoTorrent recTorrent;
                    bool escarpintermedia = false;
                    if (pathmedio != "")
                    {
                        p = pathmedio + "/";
                        recTorrent = new RecursoTorrent(textBox1.Text, listaT, textBox2.Text, p);
                        pathmedio = "";
                        escarpintermedia = true;
                    }
                    else
                    {
                        recTorrent = new RecursoTorrent(textBox1.Text, listaT, textBox2.Text);
                    }                                        
                    rep.insertarRecurso(recTorrent, escarpintermedia,pathmedio);
                    Close();
                }
                else mostrarError("Ya existe recurso con ese nombre");
            }
            else
            {
                mostrarError("Nombre de recurso no válido");
            }
                                    
        }

        private void mostrarError(String s)
        {
            label4.Text = s;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void setearPath(String p)
        {
            pathmedio = p;
        }

        public void editarRecurso(String nombre, String desc)
        {
            textBox1.Text = nombre;
            textBox2.Text = desc;
            editar = true;
        }
 
    }
}
