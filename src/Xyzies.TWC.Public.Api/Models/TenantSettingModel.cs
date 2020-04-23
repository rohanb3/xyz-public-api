using System.ComponentModel.DataAnnotations;

namespace Xyzies.TWC.Public.Api.Models
{
    public class TenantSettingModel
    {
        public bool FaceDetectionEnabled { get; set; }
        public int FaceDetectionCallTimeout { get; set; }
        public string FaceDetectionPopupSize { get; set; }

        [MaxLength(150)]
        public string FaceDetectionPopupTitle { get; set; }

        [MaxLength(150)]
        public string FaceDetectionPopupBody { get; set; }
        public string WebViewUrl { get; set; }
        public bool ShowWebViewButton { get; set; }
        public string WebViewButtonPosition { get; set; }
        public string WebViewButtonPrimaryColor { get; set; }
        public string WebViewButtonSecondaryColor { get; set; }
        public string PrimaryColor { get; set; }
        public string LinkColor { get; set; }
        public string Logo { get; set; }
        public string BackgroundVideo { get; set; }
        public string BackgroundImage { get; set; }
        public string InputBackgroundColor { get; set; }
        public string InputTextColor { get; set; }
        public string ActiveInputBackgroundColor { get; set; }
        public string ActiveInputTextColor { get; set; }
        public string PlaceholderTextColor { get; set; }
        public string PrimaryButtonColor { get; set; }
        public string PrimaryButtonTextColor { get; set; }
        public string TextColor { get; set; }
        public bool ShowXYZLogo { get; set; }
        public string CallImage { get; set; }
        public decimal CallImageScale { get; set; }
        public string CallHintTextColor { get; set; }
        public string DefaultButtonColor { get; set; }
        public string DefaultButtonTextColor { get; set; }
        public string IconColor { get; set; }
        public string BackgroundColorFrom { get; set; }
        public string BackgroundColorTo { get; set; }
    }
}
