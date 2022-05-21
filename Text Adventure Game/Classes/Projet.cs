using System;
using System.Collections.Generic;

namespace Text_Adventure_Game.Classes
{
    public class Projet
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public DateTime CreationDate { get; set; }

        public List<FonctionElement> FonctionElements { get; set; }
    }
}