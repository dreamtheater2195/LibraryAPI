using System;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace LibraryAPI.Helpers
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class RequestHeaderMatchesMediaTypeAttribute : Attribute, IActionConstraint
    {
        private string _requestHeaderToMatch;
        private string[] _mediaTypes;
        public RequestHeaderMatchesMediaTypeAttribute(string requestHeaderToMatch,
            string[] mediaTypes)
        {
            _requestHeaderToMatch = requestHeaderToMatch;
            _mediaTypes = mediaTypes;
        }

        public int Order => 0;

        public bool Accept(ActionConstraintContext context)
        {
            var requestHeaders = context.RouteContext.HttpContext.Request.Headers;

            if (!requestHeaders.ContainsKey(_requestHeaderToMatch))
            {
                return false;
            }
            foreach (var mediaType in _mediaTypes)
            {
                var mediaTypeMatches = string.Equals(requestHeaders[_requestHeaderToMatch].ToString(),
                    mediaType, StringComparison.OrdinalIgnoreCase);

                if (mediaTypeMatches) return true;
            }
            return false;
        }
    }
}