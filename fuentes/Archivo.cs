using System;
using System.IO;

public class Archivo
{

    public String nombre;
    public String path;
    

	public Archivo(String path)
	{
        FileInfo archivo = new FileInfo(path);
        this.nombre = archivo.Name;
        this.path = path;
        
	}

 

    public String getNombre()
    {
        return nombre;
    }

    public String getPath()
    {
        return path;
    }

    public void setPath(String path)
    {
        this.path = path;
    }

    public void setNombre(String nombre)
    {
        this.nombre=nombre;
    }

    public static bool Existe(String path)
    {
        FileInfo Archivo = new FileInfo(path);
        return (Archivo.Exists);
    }
   
}
