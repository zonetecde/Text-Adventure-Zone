using System.Collections.Generic;

namespace Text_Adventure_Game.Classes
{
    public class UserData
    {
        public List<RecentlyOpenedProject> RecentlyOpenedProjects { get; set; }
    }

    public class RecentlyOpenedProject
    {
        public string ProjectPath { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDesc { get; set; }
    }
}