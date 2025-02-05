using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace OSK
{
    public class TabButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private int index;
        [SerializeField] private Sprite _selectSprite;
        [SerializeField] private Sprite _normalSprite;

        private TabGroup _tabGroup;
        private Image _bgImage;
        
        public int Index => index;
        
        public void Initialize(TabGroup tabGroup, int index)
        {
            this.index = index;
            this._bgImage = GetComponent<Image>();
            this._tabGroup = tabGroup;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Select();
        }
        
        public virtual void Select()
        {
            _tabGroup.SelectTab(this);
            _bgImage.sprite = _selectSprite;
        }
        
        public virtual void DeSelect()
        {
            _bgImage.sprite = _normalSprite;
        }
    }
}

