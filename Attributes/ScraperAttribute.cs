namespace NFCE.API.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.All)]
    public class ExtracaoAttribute : System.Attribute
    {
        public string HtmlTag;
        public string HtmlId;
        public string HtmlClass;
        public string HtmlText;
        public string Pattern;
        #region Following
        public string FollowingTag;
        public string FollowingClass;
        public string FollowingId;
        public string FollowingText;
        #endregion
        #region Preceding
        public string PrecedingTag;
        public string PrecedingClass;
        public string PrecedingId;
        public string PrecedingText;
        #endregion
        #region Index
        public int Index;
        public int RegexIndex;
        #endregion
    }
}