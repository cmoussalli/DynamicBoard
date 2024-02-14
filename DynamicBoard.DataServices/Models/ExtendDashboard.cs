using DynamicBoard.DataServices.Models;

namespace DynamicBoard.DataServices
{
    public class ExtendDashboard : Dashboards
    {
        public Lnk_Dashboards_Charts Lnk_Dashboards_Charts { get; set; }
        public Charts Charts { get; set; }
        public ChartTypes ChartTypes { get; set; }
        public DBConnections DBConnections { get; set; }
        public SizeStyles SizeStyles { get; set; }
        public Users User { get; set; }
        public Lnk_Dashboards_Users Lnk_Dashboards_Users { get; set; }


    }
}
