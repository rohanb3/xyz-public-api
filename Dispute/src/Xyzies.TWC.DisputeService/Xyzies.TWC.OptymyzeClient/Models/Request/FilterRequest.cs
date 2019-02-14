using System;
using System.Collections.Generic;
using System.Text;

namespace Xyzies.TWC.OptymyzeClient.Models.Request
{
    public class FilterRequest
    {
        public bool InvokeMethod { get; set; }
        public string NavigationInputHidden { get; set; }
        public bool EncodeResponse { get; set; }
        public string JavaxFacesViewStateForDefaultPage { get; set; }
        public string JavaxFacesViewStateForContentFrame { get; set; }
        public string ActionForDefaultPage { get; set; }
        public string ActionForContentFrame { get; set; }
        public string UrlDetails { get; set; }
        public string Ctoken { get; set; }
        public string Rtoken { get; set; }
        public bool AsyncAjax { get; set; }
        public string PageScopeIdForDefaultPage { get; set; }
        public string PageScopeIdForContentFrame { get; set; }
        public string ConversationScopeId { get; set; }
        public string PageRegionName { get; set; }
        public string InternalBackingBeanComponentId { get; set; }
        public bool JavaxFacesPartialAjax { get; set; }
        public string ComSpmsoftwarePageRegionViewInfo { get; set; }
        public string AjaxRequestData { get; set; }
        public string FilterDataColumnId { get; set; }
        public string CustomTableId { get; set; }
    }
}
