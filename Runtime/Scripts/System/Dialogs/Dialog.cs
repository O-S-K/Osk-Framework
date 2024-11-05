using System.Collections.Generic;

namespace OSK
{
    [System.Serializable]
    public class Dialog
    {
        public int dialogID;
        public string npcName;
        public string dialogText;
        public List<DialogOption> options;
        
        public Dialog(int dialogID, string npcName, string dialogText, List<DialogOption> options)
        {
            this.dialogID = dialogID;
            this.npcName = npcName;
            this.dialogText = dialogText;
            this.options = options;
        }
    }
}