namespace OSK
{
    [System.Serializable]
    public class DialogOption
    {
        public string optionText;
        public int nextDialogID;
        
        public DialogOption(string optionText, int nextDialogID)
        {
            this.optionText = optionText;
            this.nextDialogID = nextDialogID;
        }
    }
}