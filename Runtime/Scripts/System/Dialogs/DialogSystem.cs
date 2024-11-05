using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    public class DialogSystem : MonoBehaviour, IService
    {
        public List<Dialog> dialogs = new List<Dialog>();
        public Text npcNameText;
        public Text dialogText;
        public Button optionButtonPrefab;
        public Transform optionsContainer;

        private Dialog currentDialog;
 
        public void Run(int dialogID)
        {
            currentDialog = dialogs.Find(d => d.dialogID == dialogID);
            //Display();
            DrawCurrentDialog();
        }
        
        public void Add(Dialog dialog)
        {
            if(dialogs.Contains(dialog))
                return;
            dialogs.Add(dialog);
        }
        
        public void Add(List<Dialog> dialogs)
        {
            foreach (var dialog in dialogs)
            {
                Add(dialog);
            }
        }

        private void Display()
        {
            if (currentDialog == null)
                return;

            npcNameText.text = currentDialog.npcName;
            dialogText.text = currentDialog.dialogText;

            // Xóa các nút lựa chọn cũ
            foreach (Transform child in optionsContainer)
            {
                GameObject.Destroy(child.gameObject);
            }

            // Tạo các nút lựa chọn mới
            foreach (var option in currentDialog.options)
            {
                Button newButton = GameObject.Instantiate(optionButtonPrefab, optionsContainer);
                newButton.GetComponentInChildren<Text>().text = option.optionText;
                int nextID = option.nextDialogID;
                newButton.onClick.AddListener(() => OnOptionSelected(nextID));
            }
        }

        private void OnOptionSelected(int nextDialogID)
        {
            currentDialog = dialogs.Find(d => d.dialogID == nextDialogID);
            Display();
        }
        
        public void DrawCurrentDialog()
        {
            Logg.Log($"ID: {currentDialog.dialogID} | Name: {currentDialog.npcName} | Options: {currentDialog.options.Count}");
        }
        public void DrawDisplay()
        {
            Logg.Log($"Current Dialog: {currentDialog.dialogID}");
        }
    }
}