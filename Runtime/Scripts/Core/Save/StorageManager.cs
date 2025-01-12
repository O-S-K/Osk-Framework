using UnityEngine;

namespace OSK
{
    public class StorageManager : GameFrameworkComponent
    {
        private JsonSystem _json = new JsonSystem();
        private FileSystem _file = new FileSystem();
        private XMLSystem _xml = new XMLSystem();

        public override void OnInit()
        {
        }

        /// <summary>
        ///  Save data to file, T is the file system type, U is the data type
        /// </summary> 
        public void Save<T, U>(string fileName, U data)
        {
            IFile fileSystem = GetFileSystem<T>();
            if (fileSystem != null)
            {
                fileSystem.Save(fileName, data, Main.Instance.configInit.isEncrypt);
            }
        }

        /// <summary>
        ///  Load data from file, T is the file system type, U is the data type
        /// </summary>
        public U Load<T, U>(string fileName)
        {
            IFile fileSystem = GetFileSystem<T>();
            if (fileSystem != null)
            {
                return fileSystem.Load<U>(fileName, Main.Instance.configInit.isEncrypt);
            }

            return default(U);
        }

        /// <summary>
        ///  Query data from file, T is the file system type, U is the data type
        /// </summary>
        public U Query<T, U>(string fileName, bool condition)
        {
            IFile fileSystem = GetFileSystem<T>();
            if (fileSystem != null)
            {
                return fileSystem.Query<U>(fileName, condition);
            }

            return default(U);
        }

        /// <summary>
        ///  Delete file, T is the file system type
        /// </summary>
        public void Delete(string fileName)
        {
            _json.Delete(fileName);
            _file.Delete(fileName);
            _xml.Delete(fileName);
        }

        private IFile GetFileSystem<T>()
        {
            if (typeof(T) == typeof(JsonSystem))
                return _json;
            if (typeof(T) == typeof(FileSystem))
                return _file;
            if (typeof(T) == typeof(XMLSystem))
                return _xml;
            return null;
        }
    }
}