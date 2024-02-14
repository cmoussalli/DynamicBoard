using DynamicBoard.DataServices;
using DynamicBoard.DataServices.Models;
using DynamicBoard.DataServices.Services;

namespace DynamicBoard.Application.DomainServices
{
    public class DynamicBoardChartServices
    {
        DynamicBoardDataServices db = new DynamicBoardDataServices(Storage.DBConnectionString);
        #region Chart
        public async Task<long> ChartAddEdit(long id, long chartTypeID, long dBConnectionID, string dataScript, string titleEn, string titleAr, int refershTime, bool isActive, bool isDeleted, string createdBy)
        {
            return await db.ChartAddEditAsync(id, chartTypeID, dBConnectionID, dataScript, titleEn, titleAr, refershTime, isActive, isDeleted, createdBy);
        }
        public async Task<long> ChartParametersAddUpdateAsync(long chartid, string tag, string sqlplaceholder, bool isRequired, string defaultValue)
        {
            return await db.ChartParametersAddUpdateAsync(chartid,tag, sqlplaceholder, isRequired, defaultValue);   
        }
        public async Task<bool> DeleteChartAsync(long chartId)
        {
            return await db.DeleteChartAsync(chartId);
        }

        public async Task<List<ExtendChart>> ChartsGetAllAsync(string userID = "")
        {
            return await db.ChartsGetAllAsync(userID);
        }

        public async Task<List<ExtendChart>> ChartsGetByIdAsync(long id)
        {
            return await db.ChartGetByIdAsync(id);
        }
        public async Task<List<ChartTypes>> GetChartTypes()
        {
            return await db.GetChartTypes();
        }

        public async Task<List<ExtendLnk_Charts_Users>> GetLinkChartsUsersByChartIDAsync(long chartId = 0)
        {
            return await db.GetLinkChartsUsersByChartIDAsync(chartId);
        }
        #endregion
    }
}
