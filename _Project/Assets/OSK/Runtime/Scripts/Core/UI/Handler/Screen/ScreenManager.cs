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
        [SerializeField, ReadOnly] public List<UIScreen> listScreens = new List<UIScreen>();
        [SerializeField, ReadOnly] public UIScreen currentScreen;
        private bool isAnimating;

        public UIScreen CurrentScreen
        {
            get { return currentScreen; }
        }

        public System.Action<UIScreen, UIScreen> OnSwitchingScreens;
        public System.Action<UIScreen> OnShowingScreen;

        public void Initialize()
        {
            var listUIScreenSo = Main.Save.SOData.Get<ListUIScreenSO>().UIScreens;
            if (listUIScreenSo == null)
            {
                OSK.Logg.LogError("[Screen] ListUIScreenSO.UIScreens is null");
                return;
            }

            PreloadScreens();
        }

        private void PreloadScreens()
        {
            listScreens.Clear();

            var listUIScreenSo = Main.Save.SOData.Get<ListUIScreenSO>().UIScreens;
            for (int i = 0; i < listUIScreenSo.Count; i++)
            {
                var screenUI = Instantiate(listUIScreenSo[i], transform);
                screenUI.Initialize();
                screenUI.Hide();
                screenUI.transform.localPosition = Vector3.zero;
                screenUI.transform.localScale = Vector3.one;
                listScreens.Add(screenUI);
            }
        }

        public void Show(UIScreen screen)
        {
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
        }


        public T Show<T>() where T : UIScreen
        {
            // Get the screen we want to show
            UIScreen screen = GetScreen<T>();
            if (screen == null)
            {
                OSK.Logg.LogError($"[Screen] Could not find screen in list screens " + typeof(T).Name);
                return null;
            }

            OSK.Logg.Log("[Screen] Showing screen " + screen.name);

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
                if (item is T screen)
                    return screen;
            OSK.Logg.LogError($"[Screen] Could not find screen in list screens " + typeof(T).Name);
            return null;
        }

        public UIScreen GetScreen(UIScreen screen)
        {
            foreach (var item in listScreens)
                if (item == screen)
                    return item;
            return null;
        }

        public void Hide(UIScreen screen)
        {
            screen.Hide();
        }

        public List<UIScreen> GetAllScreens()
        {
            return listScreens;
        }
    }
}