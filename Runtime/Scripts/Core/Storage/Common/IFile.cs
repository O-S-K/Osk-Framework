using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public interface IFile
    {
        public void Save<T>(string fileName, T data, bool isEncrypt = false);
        public T Load<T>(string fileName, bool isDecrypt = false);
        public void Delete(string fileName);
        
        public T Query<T>(string fileName, bool condition);
    }
}
