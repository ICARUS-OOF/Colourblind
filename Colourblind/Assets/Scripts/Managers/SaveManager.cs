using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Colourblind.Managers
{
    public class SaveManager : MonoBehaviour
    {
        public static void SaveData(object saveData, string _fileName, string _fileType)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (!Directory.Exists(SaveLocation()))
            {
                Directory.CreateDirectory(SaveLocation());
            }

            string path = SaveLocation() + _fileName + "." + _fileType;

            FileStream stream = File.Create(path);

            formatter.Serialize(stream, saveData);

            stream.Close();
        }

        public static object Load(string _fileName, string _fileType)
        {
            string path = SaveLocation() + _fileName + "." + _fileType;

            if (!File.Exists(path))
            {
                return null;
            }

            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = File.Open(path, FileMode.Open);

            try
            {
                object save = formatter.Deserialize(stream);
                stream.Close();
                return save;
            }
            catch
            {
                Debug.LogErrorFormat("Failed to load file at {0}", path);
                stream.Close();
                return null;
            }
        }

        public static void Delete(string _fileName, string _fileType)
        {
            string path = SaveLocation() + _fileName + "." + _fileType;

            if (!File.Exists(path))
            {
                return;
            }

            File.Delete(path);
        }

        public static bool FileExists(string _fileName, string _fileType)
        {
            string path = SaveLocation() + _fileName + "." + _fileType;

            return File.Exists(path);
        }

        public static string SaveLocation()
        {
            return Application.persistentDataPath + "/saves/";
        }
    }
}