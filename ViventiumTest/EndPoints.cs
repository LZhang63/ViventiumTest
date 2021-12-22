namespace ViventiumTest
{
    /// <summary>
    /// Using constants instead of literals for easy management
    /// </summary>
    static public class EndPoints
    {
        public static class CompanyController
        {
            public const string Root = "";
            public const string UploadCompanies = "DataStore";
            public const string GetAllCompanies = "Companies";
            public const string GetCompany = "Companies/{id}";
        }

    }
}
