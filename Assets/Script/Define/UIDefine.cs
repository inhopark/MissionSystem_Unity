public class UIDefine 
{
    public enum UIUnique
    {
        None,
        MainUI,
    }

    [System.Serializable]
    public class UIInfoTable
    {
        public UIUnique uiUnique;

        public string uiPath;
    }

}
