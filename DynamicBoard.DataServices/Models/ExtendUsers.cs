using DynamicBoard.DataServices.Models;

namespace DynamicBoard.DataServices
{
    public class ExtendUsers : Users
    {
        public RoleUser RoleUser { get; set; }
        public Roles Roles { get; set; }

    }
}
