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

        public async Task<List<ChartDataset>> GetChartDatasets(long chartID, string sqlScriptTemplate)
        {
            List<ExtendChart> extendDashboard = await db.ChartGetByIdAsync(chartID);
            var connesctionString = "Server=" + extendDashboard[0].DBConnections.Server + ";Database=" + extendDashboard[0].DBConnections.Database + ";Trusted_Connection=True;MultipleActiveResultSets=true;User Id=" + extendDashboard[0].DBConnections.User + ";Password=" + extendDashboard[0].DBConnections.Password + ";Integrated Security=False;";
            List<ChartDataset> datasetResult = await db.DatasetExecute(sqlScriptTemplate, connesctionString);
            return datasetResult;
        }
    }
}
