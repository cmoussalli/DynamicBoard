using DynamicBoard.DataServices;
using DynamicBoard.DataServices.Services;

namespace DynamicBoard.Application.DomainServices
{
    public class DynamicBoardDashboardServices
    {

        DynamicBoardDataServices db = new DynamicBoardDataServices(Storage.DBConnectionString);

        #region Dashboard
        public List<Dashboards> DashboardsAll(string userID = "")
        {
            List<Dashboards> dashboards = new();
            dashboards = db.DashboardsAll(userID);
            return dashboards;
        }
        public async Task DashboardAddEdit(long ID, string TitleEn, string TitleAr, string UserId, bool isActive, bool isDeleted)
        {
            await db.DashboardAddEditAsync(ID, TitleEn, TitleAr, UserId, isActive, isDeleted);
        }
        public async Task<bool> DeleteDashboardAsync(long dashboardId)
        {
            return await db.DeleteDashboardAsync(dashboardId);
        }
        public async Task<List<Dashboards>> DashboardsByIDAsync(long dashboardId)
        {
            return await db.DashboardsByIDAsync(dashboardId);
        }
        #endregion


       

    }
}
