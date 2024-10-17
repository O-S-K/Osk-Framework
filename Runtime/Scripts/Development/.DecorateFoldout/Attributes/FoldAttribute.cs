using UnityEngine;

// example: [Foldout("Setup", true)]  // true will fold everything
// example: [Foldout("Setup")]  // false will fold only the specified group
#if UNITY_EDITOR
namespace OSK
{
    public class FoldoutAttribute : PropertyAttribute
    {
        public string name;
        public bool foldEverything;

        /// <summary>Adds the property to the specified foldout group.</summary>
        /// <param name="name">Name of the foldout group.</param>
        /// <param name="foldEverything">Toggle to put all properties to the specified group</param>
        public FoldoutAttribute(string name, bool foldEverything = false)
        {
            this.foldEverything = foldEverything;
            this.name = name;
        }
    }
}
#endif