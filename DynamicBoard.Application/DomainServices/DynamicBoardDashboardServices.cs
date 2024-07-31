using DynamicBoard.DataServices;
using DynamicBoard.DataServices.Models;
using DynamicBoard.DataServices.Services;

namespace DynamicBoard.Application.DomainServices
{
    public class DynamicBoardDashboardServices
    {

        DynamicBoardDataServices db = new DynamicBoardDataServices(Storage.DBConnectionString);

        #region Dashboard
        public async Task<List<Dashboards>> DashboardsAllAsync(string userID = "")
        {
            List<Dashboards> dashboards = new();
            dashboards =await db.DashboardsAllAsync(userID);
            return dashboards;
        }

        public async Task<long> DashboardAddEditAsync(long ID, string TitleEn, string TitleAr, string UserId, bool isActive, bool isDeleted,bool hideChartButtons)
        {
           return await db.DashboardAddEditAsync(ID, TitleEn, TitleAr, UserId, isActive, isDeleted, hideChartButtons);
        }
        public async Task<bool> DeleteDashboardAsync(long dashboardId)
        {
            return await db.DeleteDashboardAsync(dashboardId);
        }
        public async Task<List<Dashboards>> DashboardsByIDAsync(long dashboardId)
        {
            return await db.DashboardsByIDAsync(dashboardId);
        }

        public async Task<Dashboards> GetDashboardByDashboardIDAsync(long dashboardId)
        {
            return await db.GetDashboardByDashboardIDAsync(dashboardId);
        }

        public async Task<List<Lnk_Dashboards_Charts_Size_Extended>> GetLnkDashboardsChartsByDashboardIDAsync(long dashboardId)
        {
            return await db.GetLnkDashboardsChartsByDashboardIDAsync(dashboardId);
        }

        public async Task<long> InsertLnk_Dashboard_Charts(Lnk_Dashboards_Charts lnk_Dashboards_Charts)
        {
            return await db.InsertLnk_Dashboard_Charts(lnk_Dashboards_Charts);
        }

         public async Task<List<Dashboard_Charts_Details>> GetDashboardChartsDetailByIDAsync(long dashboardID)
        {
            return await db.GetDashboardChartsDetailByIDAsync(dashboardID);
        }

        public async Task<bool> DeleteLnkDashboardChartsAsync(long dashboardID, long chartID)
        {
            return await db.DeleteLnkDashboardChartsAsync(dashboardID, chartID);
        }

        public async Task UpdateLnkDashboardChartsSortOrderAsync(List<Dashboard_Charts_Details> dashboard_Charts_Details)
        {
            for (int i = 0; i < dashboard_Charts_Details.Count; i++)
            {
                dashboard_Charts_Details[i].SortID = i+1;
                await db.UpdateLnkDashboardChartsSortOrderAsync(dashboard_Charts_Details[i]);
            }
        }

        public async Task<List<ChartParameter>> GetChartParametersByDashboardIDAsync(long dashboardId)
        {
            return await db.GetChartParametersByDashboardIDAsync(dashboardId);
        }


        #endregion

    }
}
