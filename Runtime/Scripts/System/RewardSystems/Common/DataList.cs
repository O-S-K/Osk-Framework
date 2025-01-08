using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    public class DataListOptions : UIBehaviourOptions
    {
        public UIBehaviour prefab;
        public List<UIBehaviourOptions> list;
    }

    public class DataList : UIBehaviour
    {
        DataListOptions _data;
        Dictionary<int, UIBehaviour> items;

        public void SetData(DataListOptions data)
        {
            this._data = data;

            if (data == null)
                return;

            for (int index = 0; index < data.list.Count; index++)
            {
                this.AddItem(data.list[index], data.prefab.gameObject);
            }

        }

        public void AddItem(UIBehaviourOptions options, GameObject prefab)
        {
            if (items == null)
                items = new Dictionary<int, UIBehaviour>();

            int itemId = options.index;

            UIBehaviour inst = null;
            if (!items.ContainsKey(itemId))
            {
                inst = CreateInstance(prefab, this.gameObject, true).GetComponent<UIBehaviour>();
                items.Add(itemId, inst);
            }
            else
            {
                inst = items[itemId];
            }



            inst.SetData(options);
            inst.gameObject.SetActive(true);

            if (this.GetComponent<VerticalLayoutGroup>() != null || this.GetComponent<GridLayoutGroup>() != null)
                this.UpdateHeight();

        }

        public void Clear()
        {
            if (items == null)
                return;

            foreach (KeyValuePair<int, UIBehaviour> entry in items)
            {
                entry.Value.gameObject.SetActive(false);
            }
        }

        public void Remove(int itemId)
        {
            if (items.ContainsKey(itemId))
                items[itemId].gameObject.SetActive(false);
        }

        public UIBehaviour GetItem(int itemId)
        {
            if (items.ContainsKey(itemId))
                return items[itemId];

            return null;
        }

        public override void Refresh()
        {
            foreach (KeyValuePair<int, UIBehaviour> entry in this.items)
            {
                entry.Value.Refresh();
            }
        }

        private void UpdateHeight()
        {
             _UpdateHeight().Run();
        }

        private IEnumerator _UpdateHeight()
        {
            yield return new WaitForSeconds(0.1f);
            RectTransform rt = this.GetComponent<RectTransform>();
            Vector2 sizeDelta = rt.sizeDelta;
            sizeDelta.y = LayoutUtility.GetPreferredHeight(rt);
            rt.sizeDelta = sizeDelta;
        }

        public GameObject CreateInstance(GameObject original, GameObject parent, bool isActive)
        {
            GameObject instance = UnityEngine.Object.Instantiate(original, parent.transform, false);
            instance.SetActive(isActive);
            return instance;
        }
    }
}
