using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace OSK
{
    public class View : MonoBehaviour
    {
        [Header("Settings")]
        [EnumToggleButtons]
        public EViewType viewType = EViewType.Popup;
        public int depth;
    
        [Space] 
        [ToggleLeft] public bool isAddToViewManager = true;
        [ToggleLeft] public bool isPreloadSpawn = true;
        [ToggleLeft] public bool isRemoveOnHide = false;
        
        [ReadOnly] 
        [ToggleLeft] public bool isInitOnScene;
        public bool IsShowing => _isShowing;
        [ShowInInspector, ReadOnly] 
        [ToggleLeft] private bool _isShowing;
        private UITransition _uiTransition;
        private RootUI _rootUI;

        [Space]
        [ToggleLeft] public bool isShowEvent = false;
        [ShowIf(nameof(isShowEvent))] public UnityEvent EventAfterInit;
        [ShowIf(nameof(isShowEvent))] public UnityEvent EventBeforeOpened;
        [ShowIf(nameof(isShowEvent))] public UnityEvent EventAfterOpened;
        [ShowIf(nameof(isShowEvent))] public UnityEvent EventBeforeClosed;
        [ShowIf(nameof(isShowEvent))] public UnityEvent EventAfterClosed;
        
        [Button]
        public void AddUITransition()
        {
            _uiTransition = gameObject.GetOrAdd<UITransition>();
        }
        

        public virtual void Initialize(RootUI rootUI)
        {
            if (isInitOnScene) return;

            isInitOnScene = true;
            _rootUI = rootUI; 

            _uiTransition = GetComponent<UITransition>();
            _uiTransition?.Initialize();

            if (_rootUI == null)
            {
                Logg.LogError("RootUI is still null after initialization.");
            }

            SetDepth(depth);
            EventAfterInit?.Invoke();
        }
  
        public void SetDepth(EViewType viewType, int order)
        {
            this.viewType = viewType;
            SetDepth(order);
        }
        
        private void SetDepth(int order)
        {
            /*var canvas = GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.sortingOrder = viewType switch
                {
                    EViewType.None => (0 + canvas.sortingOrder),
                    EViewType.Popup => (1000 + canvas.sortingOrder),
                    EViewType.Overlay => (10000 + canvas.sortingOrder),
                    EViewType.Screen => (-1000 + canvas.sortingOrder),
                    _ => canvas.sortingOrder
                };
            }
            else*/
            {
                var childPages = _rootUI.GetSortedChildPages(_rootUI.GetViewContainer);
                if (childPages.Count == 0)
                    return;

                var insertIndex = _rootUI.FindInsertIndex(childPages, order);
                if (insertIndex == childPages.Count) transform.SetAsLastSibling();
                else transform.SetSiblingIndex(insertIndex);

            }
        }


        public virtual void Open(object[] data = null)
        {
            if (!IsViewContainerInitialized() || IsAlreadyShowing()) return;

            _isShowing = true;
            EventBeforeOpened?.Invoke();
            gameObject.SetActive(true);

            if (_uiTransition != null) 
                _uiTransition.OpenTrans(() => EventAfterOpened?.Invoke());
            else EventAfterOpened?.Invoke();
        }
                  
        public virtual void Hide()
        {
            if (!_isShowing) return;

            _isShowing = false;
            EventBeforeClosed?.Invoke();

            if (_uiTransition != null) 
                _uiTransition.CloseTrans(FinalizeHide);
            else FinalizeHide();
        }

        public void CloseImmediately()
        {
            _isShowing = false;

            if (_uiTransition != null) _uiTransition.AnyClose(FinalizeImmediateClose);
            else FinalizeImmediateClose();
        }

        private bool IsViewContainerInitialized()
        {
            if (_rootUI == null)
            {
                Logg.LogError("View Manager is null. Ensure that the View has been initialized before calling Open.");
                return false;
            }

            return true;
        }

        private bool IsAlreadyShowing()
        {
            if (_isShowing)
            {
                Logg.LogWarning("View is already showing");
                return true;
            }

            return false;
        }

        private void FinalizeHide()
        {
            gameObject.SetActive(false);
            EventAfterClosed?.Invoke();

            if (isRemoveOnHide) _rootUI.Delete(this);
            else _rootUI.RemovePopup(this);
        }

        private void FinalizeImmediateClose()
        {
            gameObject.SetActive(false);
            _rootUI.RemovePopup(this);
        }

        public void Delete()
        {
            _rootUI.Delete(this);
        }
    }
}