using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace PatientPortal.Web.AuthHelpers 
{
    public static class InputExtensions 
    {
        public static MvcHtmlString BasicCheckBoxFor<T>(this HtmlHelper<T> html,
                                            Expression<Func<T, bool>> expression,
                                            object htmlAttributes = null) 
        {
            var result = html.CheckBoxFor(expression, htmlAttributes).ToString();
            const string pattern = @"<input name=""[^""]+"" type=""hidden"" value=""false"" />";
            var single = Regex.Replace(result, pattern, "");
            return MvcHtmlString.Create(single);
        }
    }
}