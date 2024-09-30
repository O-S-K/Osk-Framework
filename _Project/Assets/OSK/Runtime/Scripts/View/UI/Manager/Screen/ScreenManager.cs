using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine.Serialization;

namespace OSK
{
    public class ScreenManager : MonoBehaviour
    {
        public ListUIScreenSO ListUIScreenSO;
        [ShowInInspector, ReadOnly] public List<UIScreen> listScreens = new List<UIScreen>();
        [ShowInInspector, ReadOnly] public UIScreen currentScreen;

        private bool isAnimating;

        public UIScreen CurrentScreen
        {
            get { return currentScreen; }
        }

        public System.Action<UIScreen, UIScreen> OnSwitchingScreens;
        public System.Action<UIScreen> OnShowingScreen;

        public void Initialize()
        {
            if(ListUIScreenSO == null)
            {
                Debug.LogError("[ScreenController] ListUIScreenSO is null");
                return;
            }
            
            if(ListUIScreenSO.UIScreens == null)
            {
                Debug.LogError("[ScreenController] ListUIScreenSO.UIScreens is null");
                return;
            }
            
            listScreens.Clear();
            for (int i = 0; i < ListUIScreenSO.UIScreens.Count; i++)
            {
                var s = Instantiate(ListUIScreenSO.UIScreens[i], transform);
                s.Initialize();
                s.Hide();
                s.transform.localPosition = Vector3.zero;
                s.transform.localScale = Vector3.one;
                listScreens.Add(s);
            }
        }
 
        public T Show<T>() where T : UIScreen
        {
            // Get the screen we want to show
            UIScreen screen = GetScreen<T>();
            if (screen == null)
            {
                Debug.LogError($"[ScreenController] Could not find screen in list screens ");
                return null;
            }

            Debug.Log("[ScreenController] Showing screen " + screen.name);

            // Check if there is a current screen showing
            if (currentScreen != null)
            {
                // Hide the current screen
                currentScreen.Hide();
                OnSwitchingScreens?.Invoke(currentScreen, screen);
            }

            // Show the new screen
            screen.gameObject.SetActive(true);
            screen.Show();

            // Set the new screen as the current screen
            currentScreen = screen;
            OnShowingScreen?.Invoke(screen);
            return currentScreen as T;
        }

        public void RefreshUI()
        {
            if (listScreens != null)
                foreach (var item in listScreens)
                    item.RefreshUI();
        }
        
        public T GetScreen<T>() where T : UIScreen
        {
            foreach (var item in listScreens)
                if (item is T)
                    return item as T;
            Debug.LogError($"[ScreenTransitionController] No Screen exists in List Screens");
            return null;
        }
        
        #if UNITY_EDITOR
        public UIScreen screenSpawn;

        [Button]
        private void SpawnScreen()
        {
                var s = Instantiate(screenSpawn, transform);
                s.Initialize();
                s.Hide();
                s.transform.localPosition = Vector3.zero;
                s.transform.localScale = Vector3.one;
        }
        #endif 
    }
}