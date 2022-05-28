using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Adventure_Game.Classes
{
    public static class UserDataManager
    {
        public static UserData UserData;

        private readonly static string DataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\zonetecde\Text Adventure Zone\userdata.json";
        public static string ProjectsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\zonetecde\Text Adventure Zone\Projects\";

        public static void InitDataSaverAndLoader()
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\zonetecde\Text Adventure Zone\Projects");

            if (!File.Exists(DataPath))
            {
                UserData = new UserData()
                {
                    RecentlyOpenedProjects = new List<RecentlyOpenedProject>()
                };

                File.WriteAllText(DataPath, JsonConvert.SerializeObject(UserData));
            }
            else
                LoadData();
        }

        public static void LoadData()
        {
            UserData = JsonConvert.DeserializeObject<UserData>(File.ReadAllText(DataPath));

            // Check si tous les projets sont là
            foreach(string directory in Directory.GetDirectories(ProjectsPath))
            {
                string dName = new DirectoryInfo(directory).Name;
                if (!UserData.RecentlyOpenedProjects.Any(x => x.ProjectName.Equals(dName)))
                {
                    // Le projet existe pas alors qu'il est dans le dossier projet
                    ProjectProperties projectProperties = JsonConvert.DeserializeObject<ProjectProperties>(File.ReadAllText(ProjectsPath + dName + @"\projectprop.taz"));
                    UserData.RecentlyOpenedProjects.Add(new RecentlyOpenedProject()
                    {
                        ProjectDesc = projectProperties.Description,
                        ProjectName = projectProperties.Name,
                        ProjectPath = projectProperties.Name,
                    });
                }

                List<RecentlyOpenedProject> toDelete = new List<RecentlyOpenedProject>();
                foreach(var project in UserData.RecentlyOpenedProjects)
                {
                    string[] a = Directory.GetDirectories(ProjectsPath);

                    if (!a.Any(x => new DirectoryInfo(x).Name.Equals(project.ProjectName)))
                    {
                        // Un projet enregistré n'existe plus
                        toDelete.Add(project);
                    }
                }

                toDelete.ForEach(x => UserData.RecentlyOpenedProjects.Remove(x));
            }

            SaveData();
        }

        public static void SaveData()
        {
            File.WriteAllText(DataPath, JsonConvert.SerializeObject(UserData));

            if(GameWindow._GameWindow.OpenedProject != null)
            {
                File.WriteAllText(UserDataManager.ProjectsPath + GameWindow._GameWindow.ProjectProperties.Path + @"\blueprint.taz", JsonConvert.SerializeObject(GameWindow._GameWindow.FonctionElementsOpenedProject));
                File.WriteAllText(UserDataManager.ProjectsPath + GameWindow._GameWindow.ProjectProperties.Path + @"\connexion.taz", JsonConvert.SerializeObject(GameWindow._GameWindow.ConnexionElementsOpenedProject));
            }
        }
    }
}
