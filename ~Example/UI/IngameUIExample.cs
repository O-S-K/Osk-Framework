using OSK;
using UnityEngine;

public class IngameUIExample  : View
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //World.UI.ShowPopup<WinPopupExample>();
            var winUI = Main.UI.Spawn<WinViewExample>("Popups/PopupWin");
            winUI.Open();
        }
    }

    public void BackHome()
    {
        var homeUI = Main.UI.Spawn<HomeUIExample>("Popups/PopupHome");
        homeUI.Open();
    }
}

