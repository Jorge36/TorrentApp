using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace pro1

{


    class ArchivoXml : Archivo
    {
        String descripcion;

        public ArchivoXml(String des, String path)
            : base(path)
        { 
            descripcion=des;

        }

        public void escribirXml()
        {
            String xml;
            xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            xml += "<recurso><descripcion>" + descripcion + "</descripcion>";            
            xml += "</recurso>";
            Archivo arch = new Archivo(path);
            StreamWriter sw = new StreamWriter(arch.getPath());
            sw.WriteLine(xml);
            sw.Close();       
        }

    }
}
