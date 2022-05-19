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
                    Projets = new List<Projet>()
                };

                File.WriteAllText(DataPath, JsonConvert.SerializeObject(UserData));
            }
            else
                LoadData();
        }

        public static void LoadData()
        {
            UserData = JsonConvert.DeserializeObject<UserData>(File.ReadAllText(DataPath));
        }

        public static void SaveData()
        {
            File.WriteAllText(DataPath, JsonConvert.SerializeObject(UserData));
        }
    }
}
