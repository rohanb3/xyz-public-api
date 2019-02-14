using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using Xyzies.TWC.OptymyzeClient.Models;
using Xyzies.TWC.OptymyzeClient.Models.Request;
using Xyzies.TWC.OptymyzeClient.Utilities;

namespace Xyzies.TWC.OptymyzeClient.Client
{
    public class OptymyzeClient : BaseClient
    {
        #region Constants

        private const string ERROR_PAGE = "notificationPage";
        private const string OPYMYZED_PATH = "optymyzed9129496";
        private const string FES_PORTAL_PATH = "fes/portal";
        private const string PORTAL_VIEWS_PATH = "portal-views";
        private const string VIEWS_PATH = "views";
        private const string LAUNCH_PATH = "view/launch.jsf";
        private const string FACES_REQUEST = "partial/ajax";
        private const string PORTAL_VIEW_NAME = "Retailer Commission Earnings";
        private const string ACCESS_FORBIDDEN = "ACCESS FORBIDDEN";
        private const string REQUEST_NOT_SUCCESSFULLY = "The request with URI: {0} was not executed successfully";
        private const string HEADER_LOCATION = "Location";
        private const string HEADER_COOKIE = "Set-Cookie";

        #endregion

        private readonly OptymyzeOptions _options = null;

        private readonly HtmlParser _htmlParser = null;

        private readonly Encoding Encoding = Encoding.UTF8;

        public OptymyzeClient(OptymyzeOptions options)
        {
            _options = options;
            _htmlParser = new HtmlParser();
        }

        public BaseModelForNextRequest Login()
        {
            string exceptionMessage = string.Empty;

            string getPageLoginUri = string.Concat(_options.Host, string.Format("/{0}/?envId={1}", OPYMYZED_PATH, _options.EnvId));

            var getPageLoginResponse = base.Get(getPageLoginUri);

            if (getPageLoginResponse.StatusCode != HttpStatusCode.OK)
            {
                exceptionMessage = string.Format(REQUEST_NOT_SUCCESSFULLY, getPageLoginUri);

                throw new ApplicationException(exceptionMessage);
            }

            string loginHtml = GetTextFromResponse(getPageLoginResponse);

            #region First step login

            var loginFirtStepRequest = _htmlParser.GetLoginFirstRequest(loginHtml);

            // Parametrs for post request
            string loginFirtStepParameters = string.Format("FROM={0}&LOGIN_METADATA_TOKEN={1}&username={2}&password={3}",
                loginFirtStepRequest.From,
                HttpUtility.UrlEncode(loginFirtStepRequest.Token),
                _options.UserName,
                _options.Password);

            // Uri request
            string loginFirtStepUri = string.Concat(_options.Host, loginFirtStepRequest.Action);

            var loginFirtStepResponse = base.Post(loginFirtStepUri, loginFirtStepParameters);
            if (loginFirtStepResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(string.Format(REQUEST_NOT_SUCCESSFULLY, loginFirtStepUri));
            }

            if (loginFirtStepResponse.Headers[HEADER_LOCATION] != null && loginFirtStepResponse.Headers[HEADER_LOCATION].Contains(ERROR_PAGE))
            {
                throw new ApplicationException(ACCESS_FORBIDDEN);
            }

            string loginFirtStepHtml = GetTextFromResponse(loginFirtStepResponse);

            #endregion

            #region Second step login

            string headerCookieWithSession = loginFirtStepResponse.GetResponseHeader("Set-Cookie");
            var loginSecondStepRequest = _htmlParser.GetLoginSecondRequest(loginFirtStepHtml);

            var loginSecondStepParameters = string.Format("protocolToken={0}&envId={1}",
                HttpUtility.UrlEncode(loginSecondStepRequest.Token),
                loginSecondStepRequest.EnvId);

            var loginSecondStepResponse = base.Post(loginSecondStepRequest.Action, loginSecondStepParameters, loginFirtStepUri, headerCookieWithSession);

            string location = loginSecondStepResponse.GetResponseHeader(HEADER_LOCATION);

            if (loginSecondStepResponse.StatusCode != HttpStatusCode.Found)
            {
                throw new ApplicationException(string.Format(REQUEST_NOT_SUCCESSFULLY, loginSecondStepRequest.Action));
            }

            if (!string.IsNullOrEmpty(location) && location.Contains(ERROR_PAGE))
            {
                throw new ApplicationException(ACCESS_FORBIDDEN);
            }

            #endregion

            Uri indexUri = new Uri(location);
            string browserId = HttpUtility.ParseQueryString(indexUri.Query).Get("CustomFacesContext.BROWSER_ID");
            if (string.IsNullOrEmpty(browserId))
            {
                throw new ApplicationException("Invalid redirect. Missing browserId parameter.");
            }

            return new BaseModelForNextRequest()
            {
                Cookies = loginSecondStepResponse.GetResponseHeader(HEADER_COOKIE),
                Location = loginSecondStepResponse.GetResponseHeader(HEADER_LOCATION),
                Referer = loginSecondStepRequest.Action,
                XBrowserId = browserId
            };
        }

        public OptymyzeMenuModel GetMenuOptymyze(BaseModelForNextRequest loginResult)
        {
            var menuUri = string.Format("{0}/{1}/{2}/{3}", _options.Host, OPYMYZED_PATH, FES_PORTAL_PATH, loginResult.Location.Split('/').Last());
            var menuResponse = base.Get(menuUri, loginResult.Location, loginResult.Cookies, loginResult.XBrowserId);
            if (menuResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(string.Format(REQUEST_NOT_SUCCESSFULLY, menuUri));
            }

            if (menuResponse.Headers[HEADER_LOCATION] != null && menuResponse.Headers[HEADER_LOCATION].Contains(ERROR_PAGE))
            {
                throw new ApplicationException(ACCESS_FORBIDDEN);
            }

            var menuBody = GetTextFromResponse(menuResponse);
            JToken token = JObject.Parse(menuBody);
            IEnumerable<MenuModel> menuModels = JsonConvert.DeserializeObject<IEnumerable<MenuModel>>(token.SelectToken("$.folders").ToString());

            return new OptymyzeMenuModel()
            {
                Menu = menuModels,
                Cookies = loginResult.Cookies,
                Referer = loginResult.Location,
                Location = menuUri,
                XBrowserId = loginResult.XBrowserId
            };
        }

        public RetailerPageDataModel GoToRetailerCommissionPage(OptymyzeMenuModel menuModel)
        {
            var portalView = menuModel.Menu.SelectMany(x => x.PortalViews).FirstOrDefault(x => x.Name == PORTAL_VIEW_NAME);
            if (portalView == null)
            {
                throw new ApplicationException(string.Format("Not found page: {0}", PORTAL_VIEW_NAME));
            }

            #region Get view by id

            string viewUri = string.Format("{0}/{1}/{2}", menuModel.Location, PORTAL_VIEWS_PATH, portalView.Id);

            var viewResponse = base.Get(viewUri, menuModel.Referer, menuModel.Cookies, menuModel.XBrowserId);
            if (viewResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(string.Format(REQUEST_NOT_SUCCESSFULLY, viewUri));
            }

            if (viewResponse.Headers[HEADER_LOCATION] != null && viewResponse.Headers[HEADER_LOCATION].Contains(ERROR_PAGE))
            {
                throw new ApplicationException(ACCESS_FORBIDDEN);
            }

            string viewBody = this.GetTextFromResponse(viewResponse);
            JToken token = JObject.Parse(viewBody);
            PortalViewsModel portalViewDetail = JsonConvert.DeserializeObject<PortalViewsModel>(token.ToString());

            #endregion

            #region Get uri for retailer html page

            var viewDetailUri = string.Format("{0}/{1}/{2}", viewUri, VIEWS_PATH, portalViewDetail.ViewId);
            var viewDetailResponse = Get(viewDetailUri, menuModel.Referer, menuModel.Cookies, menuModel.XBrowserId);
            if (viewDetailResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(string.Format(REQUEST_NOT_SUCCESSFULLY, viewDetailUri));
            }

            if (viewDetailResponse.Headers[HEADER_LOCATION] != null && viewDetailResponse.Headers[HEADER_LOCATION].Contains(ERROR_PAGE))
            {
                throw new ApplicationException(ACCESS_FORBIDDEN);
            }

            string viewDetailBody = this.GetTextFromResponse(viewDetailResponse);
            token = JObject.Parse(viewDetailBody);
            PortalViewsModel viewDetail = JsonConvert.DeserializeObject<PortalViewsModel>(token.ToString());

            #endregion

            #region Get retailer commission HTML page

            string prefixUri = "../../";

            if (viewDetail.Data.Url.StartsWith(prefixUri))
            {
                viewDetail.Data.Url = viewDetail.Data.Url.Replace(prefixUri, string.Empty);
            }

            string viewDetailHtmlUri = string.Format("{0}/{1}/{2}", _options.Host, OPYMYZED_PATH, viewDetail.Data.Url);
            var viewDetailHtmlResponse = base.Get(viewDetailHtmlUri, menuModel.Referer, menuModel.Cookies, menuModel.XBrowserId);
            if (viewDetailHtmlResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(string.Format(REQUEST_NOT_SUCCESSFULLY, viewDetailUri));
            }

            if (viewDetailHtmlResponse.Headers[HEADER_LOCATION] != null && viewDetailHtmlResponse.Headers[HEADER_LOCATION].Contains(ERROR_PAGE))
            {
                throw new ApplicationException(ACCESS_FORBIDDEN);
            }

            string viewDetailHtml = this.GetTextFromResponse(viewDetailHtmlResponse);

            #endregion

            string filterCookie = viewDetailHtmlResponse.Headers["Set-Cookie"];

            return new RetailerPageDataModel()
            {
                Cookies = menuModel.Cookies,
                Referer = viewDetailHtmlUri,
                XBrowserId = menuModel.XBrowserId,
                HtmlPage = viewDetailHtml,
                ViewId = portalViewDetail.ViewId,
                FilterCookie = filterCookie,
                Location = menuModel.Referer
            };
        }

        public RetailerPageDataModel SetFilterRetailerPage(RetailerPageDataModel retailerPageData, DateTime from, DateTime to)
        {
            var filterRequest = _htmlParser.GetFilterRequest(retailerPageData.HtmlPage);
            FormRequestForRetailerPage(ref filterRequest, retailerPageData.XBrowserId);

            string filterUri = string.Concat(_options.Host, filterRequest.ActionForContentFrame);
            string filterHtmlParameters = GetParametersForGetFilterHtml(filterRequest, retailerPageData.XBrowserId, retailerPageData.ViewId);

            var filterHtmlResponse = base.Post(filterUri, filterHtmlParameters, retailerPageData.Referer, string.Concat(retailerPageData.Cookies, retailerPageData.FilterCookie));
            if (filterHtmlResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(string.Format(REQUEST_NOT_SUCCESSFULLY, filterUri));
            }

            if (filterHtmlResponse.Headers[HEADER_LOCATION] != null && filterHtmlResponse.Headers[HEADER_LOCATION].Contains(ERROR_PAGE))
            {
                throw new ApplicationException(ACCESS_FORBIDDEN);
            }

            string filterHtml = this.GetTextFromResponse(filterHtmlResponse);

            string newFilterCookie = filterHtmlResponse.Headers[HEADER_COOKIE];

            if (!string.IsNullOrWhiteSpace(newFilterCookie))
            {
                retailerPageData.FilterCookie = newFilterCookie;
            }

            var filterSecondRequest = _htmlParser.GetSecondFilterRequest(filterHtml);

            string customFilterDateParameters = GetParametersForFilterRequest(filterRequest, filterSecondRequest, from, to, retailerPageData.XBrowserId);

            var customFilterDateUri = string.Format("{0}/{1}", _options.Host, filterSecondRequest.ActionForContentFrame);
            var customFilterResponse = base.Post(customFilterDateUri, customFilterDateParameters, retailerPageData.Referer, string.Concat(retailerPageData.Cookies, retailerPageData.FilterCookie), FACES_REQUEST);

            if (customFilterResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(string.Format(REQUEST_NOT_SUCCESSFULLY, customFilterDateUri));
            }

            if (customFilterResponse.Headers[HEADER_LOCATION] != null && customFilterResponse.Headers[HEADER_LOCATION].Contains(ERROR_PAGE))
            {
                throw new ApplicationException(ACCESS_FORBIDDEN);
            }

            string customFilterHtml = this.GetTextFromResponse(customFilterResponse);

            FormRequestForRetailerPage(ref filterRequest, retailerPageData.XBrowserId);

            filterRequest.AjaxRequestData = @"{""backingBeanName"":""tableView2ComponentBean"",""methodName"":""getFilteredData"",""methodArgsValues"":[]}";

            // [DEPRECATED] For filter by one day
            //filterRequest.ComSpmsoftwarePageRegionViewInfo = string.Format("{{\"backingBeanName\":\"tableView2ComponentBean\",\"methodName\":\"getFilteredData\",\"methodArgsValues\":[\"\\\"{0}\\\"\",\"\\\"{1}\\\"\"]}}", filterRequest.FilterDataColumnId, lastDate.ToString("yyyy-MM-dd"));

            string filterParameters = ConvertParametersToString(filterRequest);

            var filterResponse = base.Post(filterUri, filterParameters, retailerPageData.Referer, string.Concat(retailerPageData.Cookies, retailerPageData.FilterCookie), FACES_REQUEST);
            if (filterResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(string.Format(REQUEST_NOT_SUCCESSFULLY, filterUri));
            }

            if (filterResponse.Headers[HEADER_LOCATION] != null && filterResponse.Headers[HEADER_LOCATION].Contains(ERROR_PAGE))
            {
                throw new ApplicationException(ACCESS_FORBIDDEN);
            }

            return retailerPageData;
        }

        public void GenerateRetailerCommissionReport(RetailerPageDataModel retailerPageData)
        {
            var request = _htmlParser.GetFilterRequest(retailerPageData.HtmlPage);
            FormRequestForRetailerPage(ref request, retailerPageData.XBrowserId);
            request.AjaxRequestData = @"{""backingBeanName"":""tableView2ComponentBean"",""methodName"":""verifyGenerateExportFileStatus"",""methodArgsValues"":[null]}";

            #region Verify generation file

            string verifyRequesParameters = ConvertParametersToString(request);
            string verifyRequestUri = string.Concat(_options.Host, request.ActionForContentFrame);
            var verifyResponse = base.Post(verifyRequestUri, verifyRequesParameters, retailerPageData.Referer, string.Concat(retailerPageData.Cookies, retailerPageData.FilterCookie), FACES_REQUEST);
            if (verifyResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(string.Format(REQUEST_NOT_SUCCESSFULLY, verifyRequestUri));
            }

            if (verifyResponse.Headers[HEADER_LOCATION] != null && verifyResponse.Headers[HEADER_LOCATION].Contains(ERROR_PAGE))
            {
                throw new ApplicationException(ACCESS_FORBIDDEN);
            }

            string verifyResponseBody = GetTextFromResponse(verifyResponse);

            #endregion

            #region Generation file

            request.AjaxRequestData = @"{""backingBeanName"":""tableView2ComponentBean"",""methodName"":""generateExportFile"",""methodArgsValues"":[null]}";
            string generateParameters = ConvertParametersToString(request);

            var generateResponse = base.Post(verifyRequestUri, generateParameters, retailerPageData.Referer, string.Concat(retailerPageData.Cookies, retailerPageData.FilterCookie), FACES_REQUEST);
            if (generateResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(string.Format(REQUEST_NOT_SUCCESSFULLY, verifyRequestUri));
            }

            if (generateResponse.Headers[HEADER_LOCATION] != null && generateResponse.Headers[HEADER_LOCATION].Contains(ERROR_PAGE))
            {
                throw new ApplicationException(ACCESS_FORBIDDEN);
            }

            string generateResponseBody = this.GetTextFromResponse(generateResponse);

            #endregion
        }

        public string GetFileUrl(RetailerPageDataModel retailerPageData)
        {
            bool isGenerationComplete = false;
            int maxRetryCount = 8,
                iteration = 0,
                delay = 8000;

            var request = _htmlParser.GetFilterRequest(retailerPageData.HtmlPage);
            FormRequestForRetailerPage(ref request, retailerPageData.XBrowserId);

            string requestUri = string.Concat(_options.Host, request.ActionForContentFrame);

            request.AjaxRequestData = @"{""backingBeanName"":""tableView2ComponentBean"",""methodName"":""getExportFileUrl"",""methodArgsValues"":[null]}";
            string getFileUrlParemeters = ConvertParametersToString(request);

            string responseContent = string.Empty;
            do
            {
                if (iteration != 0)
                {
                    Thread.Sleep(delay);
                }

                var getFileUrlResponse = base.Post(requestUri, getFileUrlParemeters, retailerPageData.Referer, string.Concat(retailerPageData.Cookies, retailerPageData.FilterCookie), FACES_REQUEST);

                if (getFileUrlResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException(string.Format(REQUEST_NOT_SUCCESSFULLY, requestUri));
                }
                if (getFileUrlResponse.Headers[HEADER_LOCATION] != null && getFileUrlResponse.Headers[HEADER_LOCATION].Contains(ERROR_PAGE))
                {
                    throw new ApplicationException(ACCESS_FORBIDDEN);
                }
                responseContent = this.GetTextFromResponse(getFileUrlResponse);

                isGenerationComplete = !responseContent.StartsWith("<notification>");

                ++iteration;
            } while (!isGenerationComplete && iteration < maxRetryCount);

            return isGenerationComplete ? responseContent : null;
        }

        public FileModel DownloadFile(RetailerPageDataModel retailerPageData, string fileUrl)
        {
            string requestUri = string.Empty;
            if (fileUrl.Contains(";"))
            {
                requestUri = string.Concat(_options.Host, fileUrl.Split(';').FirstOrDefault());
            }
            else
            {
                requestUri = string.Concat(_options.Host, fileUrl);
            }

            var fileResponse = base.Get(requestUri, retailerPageData.Referer, string.Concat(retailerPageData.Cookies, retailerPageData.FilterCookie));
            if (fileResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(string.Format(REQUEST_NOT_SUCCESSFULLY, requestUri));
            }

            if (fileResponse.Headers[HEADER_LOCATION] != null && fileResponse.Headers[HEADER_LOCATION].Contains(ERROR_PAGE))
            {
                throw new ApplicationException(ACCESS_FORBIDDEN);
            }

            string fileDesposition = fileResponse.Headers["Content-Disposition"];
            if (string.IsNullOrWhiteSpace(fileDesposition))
            {
                throw new ApplicationException("File has not been downloaded");
            }

            string fileName = fileDesposition
                .Split(';')
                .Single(x => x.Contains("filename"))
                .Replace("filename=", string.Empty)
                .Replace("\"", string.Empty);

            return new FileModel()
            {
                File = fileResponse.GetResponseStream(),
                Name = fileName.Trim(),
                Size = long.Parse(fileResponse.Headers["Content-Length"])
            };
        }

        #region Private helpers

        private string GetTextFromResponse(HttpWebResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            var stream = response.GetResponseStream();
            if (!stream.CanRead)
            {
                throw new InvalidOperationException("Can't read data from response stream");
            }

            return new StreamReader(stream, Encoding).ReadToEnd();
        }

        private void FormRequestForRetailerPage(ref FilterRequest request, string xbrowserId)
        {
            request.InvokeMethod = true;
            request.NavigationInputHidden = string.Format("{0};;;{1}- {2}-{1} ;{3}- ;{1};;false;false", xbrowserId, request.PageScopeIdForDefaultPage, request.PageScopeIdForContentFrame, request.ConversationScopeId);
            request.EncodeResponse = false;
            request.UrlDetails = @"{""group"":"""",""application"":"""",""tabLabel"":"""",""breadCrumb"":""""}";
            request.AsyncAjax = false;
            request.PageRegionName = "content_frame";
            request.JavaxFacesPartialAjax = true;
            request.InternalBackingBeanComponentId = "content_frame:prv:dataViewTableView:customTable";
            request.ComSpmsoftwarePageRegionViewInfo = string.Format(@"{{""defaultPageRegion"":[""{0}"",""{1}""],""content_frame"":[""{2}"",""{3}""]}}", request.ActionForDefaultPage, request.JavaxFacesViewStateForDefaultPage, request.ActionForContentFrame, request.JavaxFacesViewStateForContentFrame);
        }

        private string ConvertParametersToString(FilterRequest request)
        {
            return string.Format(@"invokeMethod={0}&navigation_input_hidden={1}&encodeResponse={2}&javax.faces.ViewState={3}&urlDetails={4}&ctoken={5}&asyncAjax={6}&pageScopeId={7}&conversationScopeId={8}&pageRegionName={9}&internalBackingBeanComponentId={10}&javax.faces.partial.ajax={11}&com.spmsoftware.page-region-view-info={12}&ajaxRequestData={13}",
                request.InvokeMethod.ToString().ToLower(), request.NavigationInputHidden,
                request.EncodeResponse.ToString().ToLower(), request.JavaxFacesViewStateForContentFrame,
                request.UrlDetails, request.Rtoken, request.AsyncAjax.ToString().ToLower(),
                request.PageScopeIdForContentFrame, request.ConversationScopeId,
                request.PageRegionName, request.InternalBackingBeanComponentId,
                request.JavaxFacesPartialAjax.ToString().ToLower(), request.ComSpmsoftwarePageRegionViewInfo,
                request.AjaxRequestData);
        }

        private string GetParametersForFilterRequest(FilterRequest firstRequest, FilterRequest secondRequest, DateTime startDate, DateTime endDate, string xBrowserId)
        {
            string navigation_input_hidden = string.Format("{0};content_frame;{1}:prv:done;{2}- {3}-{2} {4}-{3} ;{5}- {5}-{5} ;{2};;false;false",
                xBrowserId,
                firstRequest.CustomTableId,
                firstRequest.PageScopeIdForDefaultPage,
                firstRequest.PageScopeIdForContentFrame,
                secondRequest.PageScopeIdForContentFrame,
                secondRequest.ConversationScopeId);

            string comSpmsoftwarePageRegionViewInfo = string.Format(@"{{""defaultPageRegion"":[""{0}"",""{1}""],""content_frame"":[""{2}"",""{3}""], ""{6}"":[""{4}"",""{5}""]}}",
                firstRequest.ActionForDefaultPage,
                firstRequest.JavaxFacesViewStateForDefaultPage,
                firstRequest.ActionForContentFrame,
                firstRequest.JavaxFacesViewStateForContentFrame,
                secondRequest.ActionForContentFrame,
                secondRequest.JavaxFacesViewStateForContentFrame,
                firstRequest.CustomTableId);

            return string.Format("{9}:prv={9}:prv&{9}:prv:navigation_input_hidden={0}&{9}:prv:operator-input=9&{9}:prv:operator-label=is between&{9}:prv:j_idt10-hiddenLayout=&{9}:prv:radioGroup1-radioButtonGroup=constantOption&{9}:prv:dateLow-input={1}&{9}:prv:dateHigh-input={2}&{9}:prv:lowFilterable-input=-1&{9}:prv:lowFilterable-label=Select field...&{9}:prv:highFilterable-input=-1&{9}:prv:highFilterable-label=Select field...&{9}:prv:radioGroup1-optionsCount=2&{9}:prv:radioGroup1-hiddenRadioGroup=show&{9}:prv:radioGroup1-hiddenRadioOptions=&=&{9}:prv:j_idt9-hiddenLayout=&conversationScopeId={3}&pageScopeId={4}&javax.faces.ViewState={5}&form_with_validation_errors=false&pageRegionName={9}&isUndoNavigation=null&isValidationSkipped=false&navigation_by_ajax=true&ctoken={6}&PAGE_REGION_TO_COMPONENT=true&PAGE_REGION_HOLDER_ID=content_frame:prv:dataViewTableView:customTable&javax.faces.partial.execute=@all&javax.faces.source={9}:prv:done&com.spmsoftware.page-region-view-info={7}&HOLDER_PAGE_REGION_NAME=content_frame&PC_NAVIGATION_TYPE=DONE&urlDetails={8}",
                navigation_input_hidden,
                startDate.ToString("yyyy-MM-dd"),
                endDate.ToString("yyyy-MM-dd"),
                secondRequest.ConversationScopeId,
                secondRequest.PageScopeIdForContentFrame,
                secondRequest.JavaxFacesViewStateForContentFrame,
                firstRequest.Rtoken,
                comSpmsoftwarePageRegionViewInfo,
                firstRequest.UrlDetails,
                firstRequest.CustomTableId);
        }

        private string GetParametersForGetFilterHtml(FilterRequest request, string xBrowserId, string viewId)
        {
            string navigation_input_hidden = string.Format("{0};{4};content_frame:prv:dataViewTableView:customTable;{1}- {2}-{1} ;{3}- ;{2};{3};false;false",
                xBrowserId,
                request.PageScopeIdForDefaultPage,
                request.PageScopeIdForContentFrame,
                request.ConversationScopeId,
                request.CustomTableId);
            return string.Format("content_frame:prv=content_frame:prv&content_frame:prv:navigation_input_hidden={0}&content_frame:prv:workflow_genericToolbarComp_selectedInformation=SIMPLE_ITEM-{1}&content_frame:prv:j_idt6-hidden=&selectedFeatureItemIdcontent_frame:prv:dataViewTableView:customTable-customize=&content_frame:prv:dataViewTableView:j_id4_fromdropdown-input-input=(All)&=-1&content_frame:prv:dataViewTableView:j_id6_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id8_fromdropdown-input-input=Custom...&content_frame:prv:dataViewTableView:j_id10_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id12_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id14_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id16_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id18_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id20_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id22_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id24_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id26_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id28_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id30_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id32_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id34_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id36_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id38_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id40_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id42_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id44_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id46_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id48_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id50_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id52_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id54_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id56_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id58_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id60_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id62_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id64_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id66_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id68_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id70_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id72_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id74_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id76_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id78_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id80_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id82_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id84_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id86_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id88_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id90_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id92_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id94_fromdropdown-input-input=(All)&content_frame:prv:dataViewTableView:j_id96_fromdropdown-input-input=(All)&content_frame_prv_dataViewTableView_customTable-input-143463044_0-input=&content_frame_prv_dataViewTableView_customTable-input-58654896_0-input=&content_frame_prv_dataViewTableView_customTable-input-88259515_0-input=&content_frame_prv_dataViewTableView_customTable-input-2831323_0-input=&content_frame_prv_dataViewTableView_customTable-input-94134731_58381670_0-input=&content_frame_prv_dataViewTableView_customTable-input-58654834_0-input=&content_frame_prv_dataViewTableView_customTable-input-58383131_0-input=&content_frame_prv_dataViewTableView_customTable-input-259652821_98446943_0-input=&content_frame_prv_dataViewTableView_customTable-input-39949010_0-input=&content_frame_prv_dataViewTableView_customTable-input-98360109_0-input=&content_frame_prv_dataViewTableView_customTable-input-286693677_0-input=&content_frame_prv_dataViewTableView_customTable-input-98388259_0-input=&content_frame_prv_dataViewTableView_customTable-input-260599180_0-input=&content_frame_prv_dataViewTableView_customTable-input-286713002_0-input=&content_frame_prv_dataViewTableView_customTable-input-177213778_0-input=&content_frame_prv_dataViewTableView_customTable-input-260579141_64858975_0-input=&content_frame_prv_dataViewTableView_customTable-input-260579141_36187628_0-input=&content_frame_prv_dataViewTableView_customTable-input-389211_0-input=&content_frame_prv_dataViewTableView_customTable-input-174096253_0-input=&content_frame_prv_dataViewTableView_customTable-input-88263799_0-input=&content_frame_prv_dataViewTableView_customTable-input-88263746_0-input=&content_frame_prv_dataViewTableView_customTable-input-7484279_0-input=&content_frame_prv_dataViewTableView_customTable-input-5220943_0-input=&content_frame_prv_dataViewTableView_customTable-input-4023421_0-input=&content_frame_prv_dataViewTableView_customTable-input-3616055_0-input=&content_frame_prv_dataViewTableView_customTable-input-3616056_0-input=&content_frame_prv_dataViewTableView_customTable-input-8002370_0-input=&content_frame_prv_dataViewTableView_customTable-input-5220954_0-input=&content_frame_prv_dataViewTableView_customTable-input-4445859_0-input=&content_frame_prv_dataViewTableView_customTable-input-5220928_0-input=&content_frame_prv_dataViewTableView_customTable-input-38057107_0-input=&content_frame_prv_dataViewTableView_customTable-input-213789889_0-input=&content_frame_prv_dataViewTableView_customTable-input-215244122_0-input=&content_frame_prv_dataViewTableView_customTable-input-55862201_2831332_0-input=&content_frame_prv_dataViewTableView_customTable-input-55862201_184_0-input=&content_frame_prv_dataViewTableView_customTable-input-55862201_186_0-input=&content_frame_prv_dataViewTableView_customTable-input-39948730_0-input=&content_frame_prv_dataViewTableView_customTable-input-88263704_0-input=&content_frame_prv_dataViewTableView_customTable-input-141794613_0-input=&content_frame_prv_dataViewTableView_customTable-input-165326264_0-input=&content_frame_prv_dataViewTableView_customTable-input-123116149_0-input=&content_frame_prv_dataViewTableView_customTable-input-166826381_0-input=&content_frame_prv_dataViewTableView_customTable-input-123116151_0-input=&content_frame_prv_dataViewTableView_customTable-input-140211271_0-input=&content_frame_prv_dataViewTableView_customTable-input-97775715_0-input=&content_frame_prv_dataViewTableView_customTable-input-159689326_0-input=&content_frame:prv:dataViewTableView:customTable-tableview_rowsState={{\"columnId\":\"{2}\"}}&content_frame:prv:dataViewTableView:customTable-actionEvent=VIEW;\"CUSTOM_FILTER\"&exportConfirmationBox-hidden=false&content_frame:prv:j_idt23-hidden=false&conversationScopeId={3}&pageScopeId={4}&javax.faces.ViewState={5}&form_with_validation_errors=false&pageRegionName=content_frame&isUndoNavigation=null&isValidationSkipped=true&navigation_by_ajax=true&ctoken={6}&targetPageRegionName={8}&COMPONENT_TO_PAGE_REGION=true&SUBMITTED_CLIENT_IDS=[\"content_frame:prv:dataViewTableView:customTable\"]&urlDetails={7}",
                navigation_input_hidden,
                viewId,
                request.FilterDataColumnId,
                request.ConversationScopeId,
                request.PageScopeIdForContentFrame,
                request.JavaxFacesViewStateForContentFrame,
                request.Rtoken,
                request.UrlDetails,
                request.CustomTableId);
        }

        #endregion
    }
}
