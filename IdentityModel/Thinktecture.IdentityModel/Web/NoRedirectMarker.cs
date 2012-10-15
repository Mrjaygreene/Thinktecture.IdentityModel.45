﻿using System.Collections;
using System.Net.Http;
using System.Web;
using Thinktecture.IdentityModel.Constants;

namespace Thinktecture.IdentityModel.Web
{
    public static class NoRedirectMarker
    {
        public static void Set()
        {
            SetNoRedirectMarkerOnContext(value:true, overrideValue: true);
        }

        public static void Set(HttpRequestMessage request)
        {
            SetNoRedirectMarkerOnContextOrMessage(request, value: true, overrideValue: true);
        }

        public static void TrySet()
        {
            SetNoRedirectMarkerOnContext(value: true, overrideValue: false);
        }

        public static void TrySet(HttpRequestMessage request)
        {
            SetNoRedirectMarkerOnContextOrMessage(request, value: true, overrideValue: false);
        }

        public static void UnSet()
        {
            SetNoRedirectMarkerOnContext(value: false, overrideValue: true);
        }

        public static void UnSet(HttpRequestMessage request)
        {
            SetNoRedirectMarkerOnContextOrMessage(request, value: false, overrideValue: false);
        }

        public static bool? Get()
        {
            var context = HttpContext.Current;
            if (context == null)
            {
                return null;
            }

            var item = context.Items[Internal.NoRedirectLabel];
            if (item == null)
            {
                return null;
            }

            bool label;
            if (bool.TryParse(item.ToString(), out label))
            {
                return label;
            }

            return null;
        }

        private static bool SetNoRedirectMarkerOnContext(bool value, bool overrideValue)
        {
            if (HttpContext.Current != null)
            {
                SetNoRedirectMarkerOnItemsCollection(HttpContext.Current.Items, value, overrideValue);
                return true;
            }

            return false;
        }

        private static void SetNoRedirectMarkerOnContextOrMessage(HttpRequestMessage request, bool value, bool overrideValue)
        {
            if (!SetNoRedirectMarkerOnContext(value, overrideValue))
            {
                if (request.Properties.ContainsKey("MS_HttpContext") && request.Properties["MS_HttpContext"] != null)
                {
                    var context = request.Properties["MS_HttpContext"] as HttpContextWrapper;
                    SetNoRedirectMarkerOnItemsCollection(context.Items, value, overrideValue);
                }
            }
        }

        private static void SetNoRedirectMarkerOnItemsCollection(IDictionary items, bool value, bool overrideValue)
        {
            if (items == null)
            {
                return;
            }

            if (overrideValue)
            {
                items[Internal.NoRedirectLabel] = value;
                return;
            }
            else
            {
                var marker = items[Internal.NoRedirectLabel];

                if (marker == null)
                {
                    items[Internal.NoRedirectLabel] = value;
                }
            }
        }
    }
}
