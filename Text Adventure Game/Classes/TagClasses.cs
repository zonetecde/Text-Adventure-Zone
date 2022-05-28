using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Adventure_Game.Classes
{
    internal class FonctionTag
    {
        internal int FonctionId {get;set;}
        internal int IdOnBlueprint {get;set;}

        internal int IdConnecteurEntrée {get;set;}
        internal int IdConnecteurSortie {get;set;}
    }

    internal class ConnecteurTag
    {
        internal int ConnecteurId { get; set; }

        internal int IdFonctionConnecteurEntrée { get; set; }
        internal int IdFonctionConnecteurSortie { get; set; }
    }
}
