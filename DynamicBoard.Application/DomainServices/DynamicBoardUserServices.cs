using DynamicBoard.DataServices.Models;
using DynamicBoard.DataServices;
using DynamicBoard.DataServices.Services;

namespace DynamicBoard.Application.DomainServices
{
    public class DynamicBoardUserServices
    {

        DynamicBoardDataServices db = new DynamicBoardDataServices(Storage.DBConnectionString);

        #region Users


        public async Task<List<Users>> GetAllUsers()
        {
            return await db.GetUsers();
        }

        public async Task<long> LinkChartWithUsers(Lnk_Charts_Users model)
        { 
            return await db.LinkChartWithUsers(model);
        }
        public async Task<bool> DeleteLnkChartUserAsync(long chartId, string userID)
        { 
            return await db.DeleteLnkChartUserAsync(chartId, userID);
        }
        #endregion
    }
}
