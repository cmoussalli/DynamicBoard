using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBoard.DataServices
{
    public class Lnk_Dashboards_Users
    {
        public long ID { get; set; }

        public long DashboardId { get; set; }

        public string UserId { get; set; }

    }

}
