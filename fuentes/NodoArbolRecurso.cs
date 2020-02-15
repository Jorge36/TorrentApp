using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pro1
{
    class NodoArbolRecurso: TreeNode
    {
        RecursoTorrent recTorrent;


        public NodoArbolRecurso(String nombreRec, RecursoTorrent recT)
            : base(nombreRec,1,1)
        {
            recTorrent = recT;
        }

        public RecursoTorrent getRecurso()
        {
            return recTorrent;
        }

        public void eliminarRecurso()
        {
            recTorrent.eliminar();
        }


    }
}
