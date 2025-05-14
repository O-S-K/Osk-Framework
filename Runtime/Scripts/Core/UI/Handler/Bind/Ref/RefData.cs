using System;
using UnityEngine;

namespace OSK
{
    [Serializable]
    public class RefData
    {
        public string name;
        public UnityEngine.Object bindObj;
        public object bindVal;
        public string typeName = "";
        public bool hasVal;

        public RefData(string name, EComponentType eComponent, object bindVal = null)
        {
            this.name = name;
            bindObj = null;
            this.bindVal = bindVal;
            TypeName = eComponent.ToString();
        }

        public RefData(string name, EComponentType eComponent, UnityEngine.Object bindObj)
        {
            this.name = name;
            this.bindObj = bindObj;
            bindVal = null;
            TypeName = eComponent.ToString();
        }


        public string TypeName
        {
            get => typeName;
            set
            {
                typeName = value;
                SetParams(value);
            }
        }

        public UnityEngine.Object GetBindObj() => this.bindObj;

        private void SetParams(string typeName)
        {
            // if ((EComponentType) Enum.Parse(typeof (EComponentType), typeName) >= EComponentType.GameObject)
            //     return;

            if (typeName == "GameObject")
            {
                bindObj = GetGameObject();
                return;
            }

            if (typeName == "Transform")
            {
                if (GetGameObject().transform is RectTransform)
                {
                    bindObj = GetGameObject().transform as RectTransform; 
                }
                else
                {
                    bindObj = GetGameObject().transform;
                }
                return;
            }

            if (typeName == "MonoBehaviour")
            {
                var m = GetGameObject().GetComponent<BindObjectMono>();
                if (m == null)
                {
                    Debug.LogError(
                        $"Component {typeName} not found on {GetGameObject().name}, Please add BindObject to {GetGameObject().name}");
                    return;
                }

                bindObj = m;
                return;
            }

            var o = GetGameObject().GetComponent(typeName);
            if (o == null)
            {
                Debug.LogError(
                    $"Component {typeName} not found on {GetGameObject().name}, Please add {typeName} to {GetGameObject().name}");
                return;
            }

            bindObj = o;
        }

        public GameObject GetGameObject()
        {
            GameObject gameObject = this.bindObj switch
            {
                Component bindObj2 => bindObj2.gameObject,
                GameObject bindObj1 => bindObj1,
                _ => null
            };
            return gameObject;
        }
    }
}