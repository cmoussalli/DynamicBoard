using CMouss.IdentityFramework;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace DynamicBoard.Application.Controllers
{
    public class BaseController : Controller
    {
        public const string SessionLanguage = ".Reporting.SessionLanguage";
        public const string SessionKeyUserSessionRole = ".Reporting.UserSessionRole";
        public const string SessionKeyUserId = ".Reporting.SessionKeyUserId";

        public PortalDataModel LoadDataModel(int NewLanguageID = 0)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionLanguage)))
            {
                HttpContext.Session.SetString(SessionLanguage, "2");
                NewLanguageID = 2;
            }
            else
            {
                if (NewLanguageID == 0)
                {
                    NewLanguageID = Convert.ToInt32(HttpContext.Session.GetString(SessionLanguage));
                }
                else
                {
                    HttpContext.Session.SetString(SessionLanguage, NewLanguageID.ToString());
                }
            }
            HttpContext.Session.SetString(SessionLanguage, NewLanguageID.ToString());
            Paging paging = new Paging();
            paging.PageSize = 1;

            if (NewLanguageID == 0)
            {
                NewLanguageID = 1;
            }
            PortalDataModel model = new PortalDataModel();
            if (NewLanguageID == 2)
            {
                //model.User.PreferedLanguageID = 2;
                // model.Language = StaticStorage.MultiLanguage.English;
                model.User.Id = HttpContext.Session.GetString(SessionKeyUserId);
            }
            else
            {
                //model.User.PreferedLanguageID = 1;
                // model.Language = StaticStorage.MultiLanguage.Arabic;
                model.User.Id = HttpContext.Session.GetString(SessionKeyUserId);
            }
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyUserSessionRole)))
            {
                model.UserRole = HttpContext.Session.GetString(SessionKeyUserSessionRole);
            }
            return model;
        }
    }
}
