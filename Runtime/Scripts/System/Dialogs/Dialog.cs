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
    }
}