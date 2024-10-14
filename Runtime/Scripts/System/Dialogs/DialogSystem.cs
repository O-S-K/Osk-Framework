using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    public class DialogSystem : MonoBehaviour
    {
        public List<Dialog> dialogs = new List<Dialog>();
        public Text npcNameText;
        public Text dialogText;
        public Button optionButtonPrefab;
        public Transform optionsContainer;

        private Dialog currentDialog;


        public void StartDialog(int dialogID)
        {
            currentDialog = dialogs.Find(d => d.dialogID == dialogID);
            DisplayDialog();
        }

        private void DisplayDialog()
        {
            if (currentDialog == null)
                return;

            npcNameText.text = currentDialog.npcName;
            dialogText.text = currentDialog.dialogText;

            // Xóa các nút lựa chọn cũ
            foreach (Transform child in optionsContainer)
            {
                Destroy(child.gameObject);
            }

            // Tạo các nút lựa chọn mới
            foreach (var option in currentDialog.options)
            {
                Button newButton = Instantiate(optionButtonPrefab, optionsContainer);
                newButton.GetComponentInChildren<Text>().text = option.optionText;
                int nextID = option.nextDialogID;
                newButton.onClick.AddListener(() => OnOptionSelected(nextID));
            }
        }

        private void OnOptionSelected(int nextDialogID)
        {
            currentDialog = dialogs.Find(d => d.dialogID == nextDialogID);
            DisplayDialog();
        }
    }
}