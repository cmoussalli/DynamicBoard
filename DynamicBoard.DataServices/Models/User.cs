using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBoard.DataServices.Models
{
    public class Users
    {
        public int PreferedLanguageID { get; set; }
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string PrivateKey { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public bool IsActive { get; set; }

        public string LastIPAddress { get; set; }
    }
}
