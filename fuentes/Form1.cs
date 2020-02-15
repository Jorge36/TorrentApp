using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace pro1
{
    public partial class Form1 : Form
    {
        private static String cliente="";
        Repositorio rep = null;
        
        
        public Form1()
        {
            InitializeComponent();
            Archivo arch = new Archivo(Application.StartupPath + "/Info.inf");
            FileInfo ArchInfo = new FileInfo(Application.StartupPath + "/Info.inf");
            if (ArchInfo.Exists)
            {
                StreamReader sw = new StreamReader(arch.getPath());

                Form1.cliente = sw.ReadLine();
                sw.Close();
            }
            
            radioButton1.Checked = true;
            cargarRepositorio(true);
        }

        public static void setCliente(String c)
        {
            cliente = c;            
        }
                      
        private void button1_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();           
            f.ShowDialog();
            treeView1.Focus();          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 f = new Form4(rep);
            f.ShowDialog();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2(rep, "Insertar Recurso");
            f.ShowDialog();
            treeView1.Focus();
        }
 
        

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                listBox1.Items.Clear();
                textBox1.Text = "";
                textBox2.Text = "";
                StringBuilder MyStringBuilder = new StringBuilder(treeView1.SelectedNode.FullPath);
                MyStringBuilder.Replace('\\', '/');
                FileInfo Arch = new FileInfo(treeView1.SelectedNode.FullPath);
                if (Arch.Exists)
                {
                    if (Repositorio.extensionValida(Arch))
                    {
                        try
                        {

                            if ((cliente != null) && (cliente.Length > 0))
                            {
                                FileInfo programacliente = new FileInfo(cliente);
                                if (programacliente.Exists)
                                {
                                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                                    p.StartInfo.FileName = "\"" + cliente + "\"";
                                    p.StartInfo.Arguments = "\"" + treeView1.SelectedNode.FullPath + "\"";
                                    p.Start();
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            textBox1.Text = "Recurso no válido";
                        }
                    }
                }
                else
                {
                    
                    if (rep.esRecurso(treeView1.SelectedNode.FullPath))
                    {
                        textBox1.Text = treeView1.SelectedNode.Text;
                        textBox2.Text = rep.getDescripcion(treeView1.SelectedNode);
                        List<ArchivoTorrent> lista = rep.getListaTorrents(treeView1.SelectedNode);
                        foreach (ArchivoTorrent a in lista)
                        {
                            listBox1.Items.Add(a.getPath());
                        }
                        
                    }
                }
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {   
                if ((cliente!=null) &&(cliente.Length > 0))
                {
                    FileInfo programacliente= new FileInfo(cliente);
                    if(programacliente.Exists)
                {
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = "\"" + cliente + "\"";
                    p.StartInfo.Arguments = "\"" + listBox1.SelectedItem.ToString() + "\"";
                    p.Start();
                }
                }
            }
        }

        private void cargarRepositorio(bool mostrarArchivos)
        {
            CambiarVisibilidad(!mostrarArchivos);
            treeView1.Nodes.Clear();
            rep = new Repositorio(mostrarArchivos);
            treeView1.Nodes.Add(rep.getArbol()); 
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                cargarRepositorio(true);                                               
            }
        }
        private void CambiarVisibilidad(bool b)
        {
            textBox1.Visible = b;
            textBox2.Visible = b;
            listBox1.Visible = b;
            label1.Visible = b;
            label2.Visible = b;
            label3.Visible = b;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            if ((radioButton2.Checked))
            {
                textBox2.Text = "";
                textBox1.Text = "";
                listBox1.Items.Clear();
                cargarRepositorio(false);           
            }
        }
      
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            Archivo arch = new Archivo(Application.StartupPath + "/Info.inf");
            StreamWriter sw = new StreamWriter(arch.getPath());
            textBox1.Text = Application.StartupPath;
            sw.WriteLine(Form1.cliente);            
            sw.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(treeView1.SelectedNode!=null)
            {
                rep.eliminar(treeView1.SelectedNode);
            }
            treeView1.Focus();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                TreeNode nodo = treeView1.SelectedNode;
                if (!(nodo is NodoArbolRecurso))
                {
                    Form2 f = new Form2(rep,"Insertar Recurso");                    
                    String path = nodo.FullPath;
                    String cadena = "";                    
                    for (int i = 24; i < path.Length; i++)
                    {
                        cadena = cadena + path[i];                        
                    }
                    f.setearPath(cadena);
                    rep.setearNodoRaizAux(nodo);
                    f.ShowDialog();
                }

            }
            treeView1.Focus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {                                                
                TreeNode nodo = treeView1.SelectedNode;
                DirectoryInfo dir = new DirectoryInfo(nodo.FullPath);                              
                if (rep.esRecurso(dir))
                {
                    Form2 f = new Form2(rep,"Editar Recurso");               
                    rep.nodoSeleccionado(nodo);
                    f.editarRecurso(((NodoArbolRecurso)nodo).getRecurso().getNombre(), ((NodoArbolRecurso)nodo).getRecurso().getDescripcion());
                    String path = ((NodoArbolRecurso)nodo).getRecurso().getPath();
                    rep.setearNodoRaizAux(nodo.Parent);
                    f.setearPath(SacarPathmedio(path));                    
                    f.ShowDialog();                             
                }                 
            }
            treeView1.Focus();
        }

        public String SacarPathmedio(String path)
        {
            String cadena = "";            
            String cadenaaux = "";
            for (int i = 24; i < path.Length; i++)
            {                
                if (Char.Equals(path[i], '\\'))
                {                    
                    cadena = cadena + cadenaaux + "/";                                        
                }
                else
                {
                    cadenaaux = cadenaaux + path[i];                                                           
                }                
            }
            if (cadena != "")
            {
                return (cadena.Remove(cadena.Length - 1));
            }
            else
            {
                return "";
            }
        }
      

                
    }
}
