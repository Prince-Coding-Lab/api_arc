namespace ShnoFeeh.BusinessService.Common.Constant
{
    public static class ApiUris
    {
        //User
        public static string BaseURI = "BaseURI";
        public static string LoginUri { get; set; } = "LoginURI";        
        public static string ResetPassword { get; set; } = "ResetPassword";
        public static string ChangePassword { get; set; } = "ChangePassword";
        public static string ForgotPassword { get; set; } = "ForgotPassword";
        public static string AddUserUri { get; set; } = "AddUserURI";
        public static string GetAllUsers { get; set; } = "GetAllUsers";
        public static string DeleteUser { get; set; } = "DeleteUser";
        public static string UpdateUser { get; set; } = "UpdateUser";
        public static string AddUser { get; set; } = "AddUser";
        public static string ConfirmToken { get; set; } = "ConfirmToken";
        public static string ActivateEmail { get; set; } = "ActivateEmail";
        public static string UploadProfilePhoto { get; set; } = "UploadProfilePhoto";        

        //WebException
        public static string WebException { get; set; } = "WebException";

        //Demographics
        public static string GetAllCountries { get; set; } = "GetAllCountries";
        public static string GetCities { get; set; } = "GetCities";
        public static string UpdateCountry { get; set; } = "UpdateCountry";
        public static string UpdateCity { get; set; } = "UpdateCity";

        //Categories
        public static string GetCategories { get; set; } = "GetCategories";
        public static string AddCategory { get; set; } = "AddCategory";
        public static string DeleteCategory { get; set; } = "DeleteCategory";
        public static string UploadCategoryPhoto { get; set; } = "UploadCategoryPhoto";
        public static string GetAdvertisements { get; set; } = "GetAdvertisements";
        public static string UpdateAdvertisement { get; set; } = "UpdateAdvertisement";
        public static string DeleteAdvertisement { get; set; } = "DeleteAdvertisement";
        public static string DefaultImage { get; set; } = "DefaultImage";

        //Ads
        public static string GetAllAdsPrices { get; set; } = "GetAllAdsPrices";
        public static string UpdateAdPrices { get; set; } = "UpdateAdPrices";
        public static string AddCampaign { get; set; } = "AddCampaign";
        public static string AddAds { get; set; } = "AddAds";
        public static string GetCampaigns { get; set; } = "GetCampaigns";
        public static string DeleteCampaign { get; set; } = "DeleteCampaign";
        public static string DeleteAd { get; set; } = "DeleteAd";
        public static string GetAddById { get; set; } = "GetAdById";
        public static string UploadAdsPhoto { get; set; } = "UploadAdsPhoto";

        //Orders
        public static string CreateOrder { get; set; } = "CreateOrder";
        public static string GetOrders { get; set; } = "GetAllOrders";

        //Payments
        public static string CreatePayment { get; set; } = "CreatePayment";
        public static string InitiatePayment { get; set; } = "InitiatePayment";
        public static string ExecutePayment { get; set; } = "ExecutePayment";
        public static string GetPaymentStatus { get; set; } = "GetPaymentStatus";

    }
}
