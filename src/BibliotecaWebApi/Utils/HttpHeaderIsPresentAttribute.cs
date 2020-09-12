using System;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace MyPrimerWebApi.Utils
{
    public class HttpHeaderIsPresentAttribute : Attribute, IActionConstraint
    {
        private string header;
        private string value;

        public HttpHeaderIsPresentAttribute(string header, string value)
        {
            this.value = value;
            this.header = header;
        }

        public int Order => 0;

        public bool Accept(ActionConstraintContext context)
        {
            var headers = context.RouteContext.HttpContext.Request.Headers;

            if (!headers.ContainsKey(this.header))
            {
                return false;
            }

            return string.Equals(headers[this.header], this.value, StringComparison.OrdinalIgnoreCase);
        }
    }
}