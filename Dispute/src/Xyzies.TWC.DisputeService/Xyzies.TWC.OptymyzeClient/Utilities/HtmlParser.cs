using System;
using System.Collections.Generic;
using System.Linq;
using Xyzies.TWC.OptymyzeClient.Models.Request;
using HtmlAgilityPack;

namespace Xyzies.TWC.OptymyzeClient.Utilities
{
    /// <summary>
    /// Summary description for HtmlParser 
    /// </summary>
    public class HtmlParser
    {
        private readonly HtmlDocument _htmlDocument;
        private const string NOT_FOUND_INPUT = "Not found input: {0}";
        private const string NOT_FOUND_ATTRIBUTE = "Not found attribute: {0}";

        public HtmlParser()
        {
            _htmlDocument = new HtmlDocument();
        }

        public LoginFirstStepRequest GetLoginFirstRequest(string htmlText)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                throw new ArgumentNullException("htmlText");
            }

            _htmlDocument.LoadHtml(htmlText);

            return new LoginFirstStepRequest
            {
                Action = GetValueForOneNode("//form", "action"),
                From = GetValueForOneNode("//input[@name='FROM']", "value"),
                Token = GetValueForOneNode("//input[@name='LOGIN_METADATA_TOKEN']", "value")
            };
        }

        public LoginSecondStepRequest GetLoginSecondRequest(string htmlText)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                throw new ArgumentNullException("htmlText");
            }

            _htmlDocument.LoadHtml(htmlText);

            return new LoginSecondStepRequest
            {
                Action = GetValueForOneNode("//form", "action"),
                Token = GetValueForOneNode("//input[@name='protocolToken']", "value"),
                EnvId = GetValueForOneNode("//input[@name='envId']", "value")
            };
        }

        public FilterRequest GetFilterRequest(string htmlText)
        {
            string exceptionMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(htmlText))
            {
                throw new ArgumentNullException("htmlText");
            }
            var filterRequest = new FilterRequest();
            _htmlDocument.LoadHtml(htmlText);

            filterRequest.Rtoken = GetValueForOneNode("//input[@class='rtoken']", "value");
            filterRequest.Ctoken = GetValueForOneNode("//input[@class='stoken']", "value");
            filterRequest.ConversationScopeId = GetValueForOneNode("//input[@id='conversationScopeId']", "value");

            #region Get page scope id for default page

            string inputDefaultPagePageScopeName = "defaultPageRegion:mainForm";
            var inputDefaultPagePageScope = _htmlDocument.DocumentNode.SelectNodes("//input[@name='pageScopeId']").FirstOrDefault(x => x.ParentNode.Id == inputDefaultPagePageScopeName);
            if (inputDefaultPagePageScope == null)
            {
                exceptionMessage = string.Format(NOT_FOUND_INPUT, inputDefaultPagePageScopeName);

                throw new ApplicationException(exceptionMessage);
            }

            filterRequest.PageScopeIdForDefaultPage = GetValueFromInput(inputDefaultPagePageScope);

            #endregion

            #region Get action for default page 

            string attributeName = "Action";
            if (inputDefaultPagePageScope.ParentNode.Attributes[attributeName] == null)
            {
                exceptionMessage = string.Format(NOT_FOUND_ATTRIBUTE, attributeName);

                throw new ApplicationException(exceptionMessage);
            }

            filterRequest.ActionForDefaultPage = inputDefaultPagePageScope.ParentNode.Attributes[attributeName].Value;

            #endregion

            #region Get page scope id for content frame page

            string inputContentFramePageScopeName = "content_frame:prv";
            var inputContentFramePageScope = _htmlDocument.DocumentNode.SelectNodes("//input[@name='pageScopeId']").FirstOrDefault(x => x.ParentNode.Id == inputContentFramePageScopeName);

            if (inputContentFramePageScope == null)
            {
                exceptionMessage = string.Format(NOT_FOUND_INPUT, inputContentFramePageScopeName);

                throw new ApplicationException(exceptionMessage);
            }

            filterRequest.PageScopeIdForContentFrame = GetValueFromInput(inputContentFramePageScope);

            #endregion

            #region Get action for content frame page

            if (inputContentFramePageScope.ParentNode.Attributes[attributeName] == null)
            {
                exceptionMessage = string.Format(NOT_FOUND_ATTRIBUTE, attributeName);

                throw new ApplicationException(exceptionMessage);
            }
            filterRequest.ActionForContentFrame = inputContentFramePageScope.ParentNode.Attributes[attributeName].Value;

            #endregion

            #region Get view state for default page

            var inputDefaultPageViewState = _htmlDocument.DocumentNode.SelectNodes("//input[@name='javax.faces.ViewState']").FirstOrDefault(x => x.ParentNode.Id == inputDefaultPagePageScopeName);
            if (inputDefaultPageViewState == null)
            {
                exceptionMessage = string.Format(NOT_FOUND_INPUT, inputDefaultPagePageScopeName);

                throw new ApplicationException(exceptionMessage);
            }

            filterRequest.JavaxFacesViewStateForDefaultPage = GetValueFromInput(inputDefaultPageViewState);

            #endregion

            #region Get view state for content frame page

            var inputContentFrameViewState = _htmlDocument.DocumentNode.SelectNodes("//input[@name='javax.faces.ViewState']").FirstOrDefault(x => x.ParentNode.Id == inputContentFramePageScopeName);
            if (inputDefaultPageViewState == null)
            {
                exceptionMessage = string.Format(NOT_FOUND_INPUT, inputContentFramePageScopeName);

                throw new ApplicationException(exceptionMessage);
            }

            filterRequest.JavaxFacesViewStateForContentFrame = GetValueFromInput(inputContentFrameViewState);

            #endregion

            #region Get column id for filter

            string columnNodeName = "//a[@title='Process Date' and @class='tv-grid-column-label']";
            var columnNode = _htmlDocument.DocumentNode.SelectSingleNode(columnNodeName);
            if (columnNode == null)
            {
                exceptionMessage = string.Format("Not found node: ", columnNodeName);

                throw new ApplicationException(exceptionMessage);
            }

            string columnId = "columnid";
            if (columnNode.ParentNode.ParentNode.Attributes[columnId] == null)
            {
                exceptionMessage = string.Format(NOT_FOUND_ATTRIBUTE, columnId);

                throw new ApplicationException(exceptionMessage);
            }

            filterRequest.FilterDataColumnId = columnNode.ParentNode.ParentNode.Attributes[columnId].Value;

            #endregion

            #region Get custom table id

            string substringStartForGetCustomTableId = "ComponentSystem.createComponent('pr_customTable_";
            int indexCustomTable = htmlText.IndexOf(substringStartForGetCustomTableId) + substringStartForGetCustomTableId.Length;
            string customTableId = htmlText.Substring(indexCustomTable).Split('\'').FirstOrDefault();
            if (string.IsNullOrWhiteSpace(customTableId))
            {
                exceptionMessage = "Not found table id";

                throw new ApplicationException(exceptionMessage);
            }

            filterRequest.CustomTableId = string.Concat("pr_customTable_", customTableId);

            #endregion

            return filterRequest;
        }

        public FilterRequest GetSecondFilterRequest(string htmlText)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                throw new ArgumentNullException("htmlText");
            }
            var filterRequest = new FilterRequest();
            _htmlDocument.LoadHtml(htmlText);

            filterRequest.PageScopeIdForContentFrame = GetValueForOneNode("//input[@id='pageScopeId']", "value");
            filterRequest.ConversationScopeId = GetValueForOneNode("//input[@id='conversationScopeId']", "value");
            filterRequest.JavaxFacesViewStateForContentFrame = GetValueForOneNode("//input[@id='javax.faces.ViewState']", "value");
            filterRequest.ActionForContentFrame = GetValueForOneNode("//div[@id='pr_customTable_2006667154:prv']", "action");

            return filterRequest;
        }

        #region Private helpers

        private string GetValueForOneNode(string nameNode, string attributeName)
        {
            var node = _htmlDocument.DocumentNode.SelectSingleNode(nameNode);
            if (node != null)
            {
                var attribute = node.Attributes.FirstOrDefault(x => x.Name == attributeName);
                if (attribute != null)
                {
                    return attribute.Value;
                }
            }
            string exceptionMessage = string.Format("NodeName: {0}, AttributeName: {1}. It was not possible parse html page", nameNode, attributeName);

            throw new ApplicationException(exceptionMessage);
        }

        private string GetValueFromInput(HtmlNode input)
        {
            string valueName = "value";
            var attribute = input.Attributes.FirstOrDefault(x => x.Name == valueName);
            if (attribute == null)
            {
                string exceptionMessage = string.Format(NOT_FOUND_ATTRIBUTE, valueName);
                throw new ApplicationException(exceptionMessage);
            }

            return attribute.Value;
        }

        #endregion
    }
}
