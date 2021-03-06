﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPMS.Web.Infrastructure
{
    public class RouteTranslator : DynamicRouteValueTransformer
    {
        public async override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            var slug = ((string)values["slug"]);

            if (string.IsNullOrEmpty(slug))
            {
                values["controller"] = "Home";
                values["action"] = "Index";
                return values;
            }


            List<string> match = ((WhiteList[])Enum.GetValues(typeof(WhiteList))).Select(x => x.ToString())
                                                                                 .ToList();
            if (match.Any(x => x.ToLower() == slug.ToLower()))
            {
                var i = 1;
                values["controller"] = slug;
                values["action"] = "Index";
                return values;
            }

            values["controller"] = "Page";
            values["action"] = "Show";
            
            return values;
        }

       

        public enum WhiteList
        {
            home,
        }
    }
}
