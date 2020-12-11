using NFCE.API.Enums;

namespace NFCE.API.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = true)]
    public class ExtracaoAttribute : System.Attribute
    {
        public ExtracaoProcessamentoEnum Tipo;
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
        public bool FollowingSibling;
        #endregion
        #region Preceding
        public string PrecedingTag;
        public string PrecedingClass;
        public string PrecedingId;
        public string PrecedingText;
        public bool PrecedingSibling;
        #endregion
        #region Ancestor
        public string AncestorTag;
        public string AncestorClass;
        public string AncestorId;
        public string AncestorText;
        #endregion
        #region Index
        public int Index;
        public int RegexIndex;
        #endregion
        #region Funcitions
        public bool StartsWith;
        #endregion
    }
}