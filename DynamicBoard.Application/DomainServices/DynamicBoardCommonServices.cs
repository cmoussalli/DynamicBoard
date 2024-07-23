using DynamicBoard.DataServices;
using DynamicBoard.DataServices.Models;
using DynamicBoard.DataServices.Services;

namespace DynamicBoard.Application.DomainServices
{
    public class DynamicBoardCommonServices
    {
        DynamicBoardDataServices db = new DynamicBoardDataServices(Storage.DBConnectionString);
         
        public async Task<List<ExtendDBConnections>> GetDBConnections(string userId = "")
        { 
        return await db.GetDBConnections();
        }
        public async Task<List<ChartParameter>> GetChartParametersGetAll() 
        {
            return await db.GetChartParametersGetAll();
        }
        public async Task<List<ChartParameter>> GetChartParametersByChartID(long chartId)
        {
            return await db.GetChartParametersByChartID(chartId);
        }

        public async Task<List<SizeStyles>> SizeStylesGetAllAsync()
        {
            return await db.GetSizeStyles();
        }


    }
}
