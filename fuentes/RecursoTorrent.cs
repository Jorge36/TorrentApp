using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;

namespace pro1
{
    public class RecursoTorrent
    {
        List <ArchivoTorrent> listaTorrents;
        ArchivoXml Arch;
        String path = "c:/Internet/MisTorrents/";
        String nombre;
        String descripcion;
       

        public RecursoTorrent(String nombre, String fullpath)
        {
            listaTorrents = new List<ArchivoTorrent>();            
            Arch = new ArchivoXml(descripcion=LeerXml(fullpath),fullpath + "/index.xml");
            cargarLista(fullpath);
            path = fullpath;
            this.nombre = nombre;
        }

        public RecursoTorrent(String nombre, List<ArchivoTorrent> l, String descripcion, String pathmedio)
        {            
            path = path + pathmedio + nombre;       
            CrearRec(nombre,l,descripcion);
        }

        

        private void CrearRec(String nombre, List<ArchivoTorrent> l, String descripcion)
        {
            Directory.CreateDirectory(path);
            foreach (ArchivoTorrent arch in l)
            {
                FileInfo file = new FileInfo(arch.getPath());
                if (!(File.Exists(path +"/" + file.Name)))
                {
                    File.Copy(arch.getPath(), path + "/" + file.Name);
                }
            }
            listaTorrents = l;
            this.nombre = nombre;
            this.descripcion = descripcion;
            Arch = new ArchivoXml(descripcion, path + "/index.xml");
            Arch.escribirXml();
        }

        public RecursoTorrent(String nombre, List<ArchivoTorrent> l, String descripcion)
        {
            path = path + nombre;         
            CrearRec(nombre, l, descripcion);
        }

        private void cargarLista(String fullpath)        
        {
            DirectoryInfo di = new DirectoryInfo(fullpath);
            FileInfo[] ficheros1 = di.GetFiles("*.torrent");
            foreach (FileInfo f in ficheros1)
            {
                listaTorrents.Add(new ArchivoTorrent(f.FullName));                
            }
        }


        private static String LeerXml(String fullpath)
        {
            XmlDocument xDoc = new XmlDocument();
            StringBuilder MyStringBuilder = new StringBuilder(fullpath);
            MyStringBuilder.Replace('\\', '/');
            xDoc.Load(MyStringBuilder.ToString()+ "/index.xml");
            XmlNodeList recurso = xDoc.GetElementsByTagName("recurso");
            XmlNodeList descripcion =
            ((XmlElement)recurso[0]).GetElementsByTagName("descripcion");
            String desc = descripcion[0].InnerText;
            return desc;
        }

        public List<ArchivoTorrent> devolverLista()
        {
            return listaTorrents;
        }


        public void agregarTorrent(ArchivoTorrent archivo)
        {
            listaTorrents.Add(archivo);

        }

        public void eliminarArchivo(String nombreArchivo)
        {
            foreach (ArchivoTorrent archtorrent in listaTorrents)
            {
                if (archtorrent.getNombre().Equals(nombreArchivo))
                {
                    archtorrent.eliminarArchivoTorrent();
                    listaTorrents.Remove(archtorrent);
                    break;
                }                
            }

        }

        public void eliminar()
        {
            listaTorrents=null;
            DirectoryInfo f = new DirectoryInfo(path);
            f.Delete(true);
        }

        public static bool esExtensionValida(FileInfo archivo)
        {
            return (archivo.Extension.Equals(".torrent"));
        }

        public String getNombre()
        {
            return nombre;
        }

        public String getPath()
        {
            return path;
        }
     
        public String getDescripcion()
        {
            return descripcion;
        }

        public void setearPath(String p)
        {
            path = p;
        }
    }
}
