using System;
using System.Collections.Generic;

namespace Text_Adventure_Game.Classes
{
    public class ProjectProperties
    {
        public int TotalIdIncrementation { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class FonctionElement
    {
        public int Id { get; set; }
        public int IdOnBlueprint { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
    }

    public class Connexion
    {
        public int Id { get; set; }

        public int IdDepart { get; set; }
        public int IdArrivee { get; set; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }
    }
}