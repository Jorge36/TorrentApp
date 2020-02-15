using System;
using System.IO;
namespace pro1
{
    public class ArchivoTorrent : Archivo
    {
        
        public ArchivoTorrent(String path)
            : base(path)
        {

        }

        public void eliminarArchivoTorrent()
        {
            FileInfo archivoTFisico = new FileInfo(path);
            archivoTFisico.Delete();
        }
    }
}