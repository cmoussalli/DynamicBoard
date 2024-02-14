using CMouss.IdentityFramework;

namespace DynamicBoard.Application.Controllers
{
    public class PortalDataModel
    {
        public User User { get; set; } = new User();

        public string UserRole { get; set; }
        public Language Language { get; set; } = new Language();
    }
}
