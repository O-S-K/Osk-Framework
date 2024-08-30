using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OSK
{
    public class ScreenManager : MonoBehaviour
    {
        public List<UIScreen> Screens = null;
        public UIScreen currentScreen;

        private bool isAnimating;

        public UIScreen CurrentScreen
        {
            get { return currentScreen; }
        }

        public System.Action<UIScreen, UIScreen> OnSwitchingScreens;
        public System.Action<UIScreen> OnShowingScreen;

        public void Initialize()
        {
            if (Screens == null)
            {
                Debug.LogError($"[ScreenController] Screens is null");
                return;
            }


            foreach (var item in Screens)
            {
                item.Initialize();
                item.Hide();
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
            if (Screens != null)
                foreach (var item in Screens)
                    item.RefreshUI();
        }


        public T GetScreen<T>() where T : UIScreen
        {
            foreach (var item in Screens)
                if (item is T)
                    return item as T;
            Debug.LogError($"[ScreenTransitionController] No Screen exists in List Screens");
            return null;
        }
    }
}