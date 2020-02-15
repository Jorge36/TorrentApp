using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;

namespace pro1
{
    public class Repositorio
    {
        const String archXml = "index.xml";
        public TreeNode nodoraiz;
        public TreeNode nodoraizaux = null;
        public TreeNode nodoSelecc = null;

        public Repositorio(bool mostraArchivos) 
        {            
            DirectoryInfo dir = new DirectoryInfo("c:/Internet/MisTorrents");
            if (!dir.Exists)
            {
                Directory.CreateDirectory("c:/Internet/MisTorrents");
            }
            nodoraiz = new TreeNode("c:/Internet/MisTorrents");
            recorrerDirectorio(dir, nodoraiz, mostraArchivos);                                       
        }

        public bool existeRecurso(String clave, TreeNode nodoPadre)
        {
            foreach (TreeNode a in nodoPadre.Nodes)
            {
                    if (a.Text.Equals(clave))
                    {
                        return true;
                    }
            }
            return false;

        }

        public void eliminar()
        {
            eliminar(nodoSelecc);
        }

        public void nodoSeleccionado(TreeNode nodo)
        {
            nodoSelecc = nodo;
        }

        public TreeNode getArbol()
        {
            return (nodoraiz);
        }

        public bool esRecurso(DirectoryInfo f)
        {
            return Archivo.Existe(f.FullName + "/" + archXml);
        }

        public bool esRecurso(String s)
        {
            
            return Directory.Exists(s) && esRecurso(new DirectoryInfo(s));
        }

        public String getDescripcion(TreeNode a)
        {
            if (a is NodoArbolRecurso)
            {
                return ((NodoArbolRecurso)a).getRecurso().getDescripcion();
            }
            return null;
        }

        public void eliminar(TreeNode t)
        {
            if (t is NodoArbolRecurso)
            {
                ((NodoArbolRecurso)t).eliminarRecurso();
                nodoraiz.Nodes.Remove(t);
            }
            else
            {
                FileInfo archivo = new FileInfo(t.FullPath);
                if (RecursoTorrent.esExtensionValida(archivo))
                {
                    if (t.Parent is NodoArbolRecurso)
                    {
                        ((NodoArbolRecurso)t.Parent).getRecurso().eliminarArchivo(archivo.Name);
                        nodoraiz.Nodes.Remove(t);
                    }
                }
            }            
        }

        public List<ArchivoTorrent> getListaTorrents(TreeNode a)
        {
            if (a is NodoArbolRecurso)
            {
                return ((NodoArbolRecurso)a).getRecurso().devolverLista();
            }
            return null;
        }

        public void setearNodoRaizAux(TreeNode n)
        {
            nodoraizaux = n;
        }

        public void insertarRecurso(RecursoTorrent r, bool noespadrenodoraiz, String pathmedio)
        {
            TreeNode padre =new NodoArbolRecurso(r.getNombre(), r);
            if (noespadrenodoraiz)
            {                                                                
                nodoraizaux.Nodes.Add(padre);
                nodoraizaux = null;
            }
            else
            {
                nodoraiz.Nodes.Add(padre);
            }
            
            recorrerArchivos(padre);
            nodoraiz.TreeView.Sort();
        }

        

        private void recorrerArchivos(TreeNode padre)
        {
            ArchivoTorrent[] a = ((NodoArbolRecurso)padre).getRecurso().devolverLista().ToArray();
            foreach (ArchivoTorrent b in a)
            {
                padre.Nodes.Add(new TreeNode(b.getNombre(), 2, 2));
            }  
        }

        public List<String> buscar(TreeNode nodopadre, String clave)
        {
            List<String> resultado = new List<String>();
            

            foreach(TreeNode a in nodopadre.Nodes)
            {
                if (a is NodoArbolRecurso)
                {
                    if (a.Text.Contains(clave))
                    {
                        resultado.Add(a.FullPath);
                    }
                }
                List<String> c= buscar(a, clave);
 
                resultado=resultado.Concat<String>(c).ToList<String>();
  
            }
            return resultado;
        }

        public static bool extensionValida(FileInfo Arch)
        {
            return (Arch.Extension.Equals(".torrent"));
        }

        private void recorrerDirectorio(DirectoryInfo di, TreeNode padre, Boolean mostrararchivos)
        {
            
            DirectoryInfo[] ficheros = di.GetDirectories();
            
            foreach (DirectoryInfo f in ficheros)
            {
                TreeNode hijo;
                if (esRecurso(f))
                {
                    hijo = new NodoArbolRecurso(f.ToString(), new RecursoTorrent(f.Name, f.FullName));
                    if (mostrararchivos)
                    {
                        recorrerArchivos(hijo);
                    }
                }
                else
                {
                    hijo = new TreeNode(f.ToString());                    
                }
                padre.Nodes.Add(hijo);
                recorrerDirectorio(f, hijo,mostrararchivos);                
            }
           
        }
       
    }
}
