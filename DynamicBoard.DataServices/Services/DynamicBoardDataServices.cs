using Dapper;
using DynamicBoard.DataServices.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBoard.DataServices.Services
{
    public class DynamicBoardDataServices
    {
        #region Fields
        string connStr;

        #endregion

        #region Constructor
        public DynamicBoardDataServices(string connectionString)
        {
            connStr = connectionString;
        }
        #endregion

        #region Dashboard
        public async Task<List<Dashboards>> DashboardsAllAsync(string userID = "")
        {
            List<Dashboards> dashboards = new List<Dashboards>();

            try
            {
                SqlConnection conn = new SqlConnection(connStr);
                var p = new
                {
                    UserId = userID
                };
                dashboards = (await conn.QueryAsync<Dashboards>("DashboardsGetALL @UserId", p)).ToList();
            }
            catch (Exception ex)
            {
                //throw;
            }
            return dashboards;
        }

        public async Task<List<Dashboards>> DashboardsByIDAsync(long dashboardId)
        {
            List<Dashboards> dashboards = new List<Dashboards>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                var p = new
                {
                    DashboardId = dashboardId
                };
                dashboards = (await conn.QueryAsync<Dashboards>("DashboardGetByID @DashboardId", p)).ToList();
            }

            return dashboards;
        }


        public async Task<Dashboards> GetDashboardByDashboardIDAsync(long dashboardId)
        {
            Dashboards dashboards = new Dashboards();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                var p = new
                {
                    DashboardId = dashboardId
                };
                dashboards = (await conn.QueryAsync<Dashboards>("GetDashboardByDashboardID @DashboardId", p)).FirstOrDefault();
            }

            return dashboards;
        }


        //Delete_Dashboards
        public async Task<bool> DeleteDashboardAsync(long dashboardId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@DashboardId", dashboardId, DbType.Int64);
                parameters.Add("@IsDeleted", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync("Delete_Dashboards", parameters, commandType: CommandType.StoredProcedure);

                var isDeleted = parameters.Get<bool>("@IsDeleted");

                return isDeleted;
            }
        }
        public async Task<long> DashboardAddEditAsync(long id, string titleEn, string titleAr, string userId, bool isActive, bool isDeleted, bool hideChartButtons, int chartHeight)
        {
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                using (var command = new SqlCommand("[dbo].[DashboardAddEdit]", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@TitleEn", titleEn);
                    command.Parameters.AddWithValue("@TitleAr", titleAr);
                    command.Parameters.AddWithValue("@IsActive", isActive);
                    command.Parameters.AddWithValue("@IsDeleted", isDeleted);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@HideChartButtons", hideChartButtons);
                    command.Parameters.AddWithValue("@ChartHeight", chartHeight);

                    var returnIdParameter = new SqlParameter
                    {
                        ParameterName = "@ReturnID",
                        SqlDbType = SqlDbType.BigInt,
                        Direction = ParameterDirection.Output

                    };
                    command.Parameters.Add(returnIdParameter);

                    // Execute the command
                    conn.Open();
                    await command.ExecuteNonQueryAsync();

                    // Retrieve the output value
                    long returnId = (long)returnIdParameter.Value;

                    // Close the connection and return the output value
                    conn.Close();
                    return returnId;
                }

            }
            catch (Exception EX)
            {

                return 0;
            }
        }




        public async Task<List<Lnk_Dashboards_Charts_Size_Extended>> GetLnkDashboardsChartsByDashboardIDAsync(long dashboardId)
        {
            List<Lnk_Dashboards_Charts_Size_Extended> lnk_Dashboards_Charts = new List<Lnk_Dashboards_Charts_Size_Extended>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    var p = new
                    {
                        DashboardId = dashboardId
                    };
                    lnk_Dashboards_Charts = (await conn.QueryAsync<Lnk_Dashboards_Charts_Size_Extended>("GetLnkDashboardsChartsByDashboardID @DashboardId", p)).ToList();

                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return lnk_Dashboards_Charts;
        }



        public async Task<List<Dashboard_Charts_Details>> GetDashboardChartsDetailByIDAsync(long dashboardID)
        {
            List<Dashboard_Charts_Details> dashboard_Charts_Details = new List<Dashboard_Charts_Details>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    var p = new
                    {
                        DashboardId = dashboardID
                    };
                    dashboard_Charts_Details = (await conn.QueryAsync<Dashboard_Charts_Details>("GetDashboardChartsDetailByID @DashboardId", p)).ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }
            return dashboard_Charts_Details;
        }




        public async Task<List<ExtendLnk_Charts_Users>> GetLnkDashboardsChartsWithSizesByDashboardIDAsync(long chartId = 0)
        {
            List<ExtendLnk_Charts_Users> Lnk_Charts_Users = new List<ExtendLnk_Charts_Users>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    var p = new
                    {
                        ChartID = chartId
                    };

                    Lnk_Charts_Users = (await conn.QueryAsync<ExtendLnk_Charts_Users, Charts, Users, ChartTypes, ExtendLnk_Charts_Users>(
                        "GetLnk_Charts_UsersByChartID @ChartID",
                        (ext, chart, user, charttypes) =>
                        {
                            ext.Charts = chart;
                            ext.User = user;
                            ext.ChartTypes = charttypes;
                            return ext;
                        },
                        p
                        )
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return Lnk_Charts_Users;
        }




        public async Task<List<ChartParameter>> GetChartParametersByDashboardIDAsync(long dashboardID)
        {
            List<ChartParameter> Lnk_Charts_Users = new List<ChartParameter>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    var p = new
                    {
                        DashboardID = dashboardID
                    };

                    Lnk_Charts_Users = (await conn.QueryAsync<ChartParameter>(
                        "GetChartParametersByDashboardID @DashboardID", p)).ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return Lnk_Charts_Users;
        }


        public async Task<long> InsertLnk_Dashboard_Charts(Lnk_Dashboards_Charts lnk_Dashboards_Charts)
        {
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                using (var command = new SqlCommand("[dbo].[InsertLnkDashboardChart]", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", lnk_Dashboards_Charts.ID);
                    command.Parameters.AddWithValue("@DashboardID", lnk_Dashboards_Charts.DashboardID);
                    command.Parameters.AddWithValue("@ChartID", lnk_Dashboards_Charts.ChartID);
                    command.Parameters.AddWithValue("@SizeID", lnk_Dashboards_Charts.SizeID);
                    command.Parameters.AddWithValue("@SortID", lnk_Dashboards_Charts.SortID);
                    command.Parameters.AddWithValue("@IsActive", lnk_Dashboards_Charts.IsActive);
                    command.Parameters.AddWithValue("@IsDeleted", lnk_Dashboards_Charts.IsDeleted);

                    var returnIdParameter = new SqlParameter
                    {
                        ParameterName = "@ReturnID",
                        SqlDbType = SqlDbType.BigInt,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(returnIdParameter);

                    // Execute the command
                    conn.Open();
                    await command.ExecuteNonQueryAsync();

                    // Retrieve the output value
                    long returnId = (long)returnIdParameter.Value;

                    // Close the connection and return the output value
                    conn.Close();
                    return returnId;
                }

            }
            catch (Exception EX)
            {

                return 0;
            }
        }

        public async Task<bool> DeleteLnkDashboardChartsAsync(long dashboardID, long chartID)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@DashboardId", dashboardID, DbType.Int64);
                parameters.Add("@ChartId", chartID, DbType.Int64);
                parameters.Add("@IsDeleted", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                await conn.ExecuteAsync("Delete_LnkDashboardCharts", parameters, commandType: CommandType.StoredProcedure);
                var isDeleted = parameters.Get<bool>("@IsDeleted");
                return isDeleted;
            }
        }


        public async Task<long> UpdateLnkDashboardChartsSortOrderAsync(Dashboard_Charts_Details dashboard_Charts_Details)
        {
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                using (var command = new SqlCommand("[dbo].[UpdateLnkDashboardChartsSortOrder]", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DashboardId", dashboard_Charts_Details.DashboardID);
                    command.Parameters.AddWithValue("@ChartId", dashboard_Charts_Details.ChartID);
                    command.Parameters.AddWithValue("@Order", dashboard_Charts_Details.SortID);
                    var returnIdParameter = new SqlParameter
                    {
                        ParameterName = "@ReturnID",
                        SqlDbType = SqlDbType.BigInt,
                        Direction = ParameterDirection.Output

                    };
                    command.Parameters.Add(returnIdParameter);

                    // Execute the command
                    conn.Open();
                    await command.ExecuteNonQueryAsync();

                    // Retrieve the output value
                    long returnId = (long)returnIdParameter.Value;

                    // Close the connection and return the output value
                    conn.Close();
                    return returnId;
                }

            }
            catch (Exception EX)
            {

                return 0;
            }
        }



        #endregion


        #region Chart
        public async Task<long> ChartAddEditAsync(long id, long chartTypeID, long dBConnectionID, string dataScript, string titleEn, string titleAr, long refershTime, bool isActive, bool isDeleted, long chartTheme = 1, string createdBy = " ", bool display = false)
        {
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                using (var command = new SqlCommand("[dbo].[ChartAddEdit]", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@ChartTypeID", chartTypeID);
                    command.Parameters.AddWithValue("@ChartTheme", chartTheme);
                    command.Parameters.AddWithValue("@DBConnectionID", dBConnectionID);
                    command.Parameters.AddWithValue("@DataScript", dataScript);
                    command.Parameters.AddWithValue("@TitleEn", titleEn);
                    command.Parameters.AddWithValue("@TitleAr", titleAr);
                    command.Parameters.AddWithValue("@RefershTime", refershTime);
                    command.Parameters.AddWithValue("@IsActive", isActive);
                    command.Parameters.AddWithValue("@IsDeleted", isDeleted);
                    command.Parameters.AddWithValue("@CreatedBy", createdBy);
                    command.Parameters.AddWithValue("@Display", display);
                    var returnIdParameter = new SqlParameter
                    {
                        ParameterName = "@ReturnID",
                        SqlDbType = SqlDbType.BigInt,
                        Direction = ParameterDirection.Output

                    };
                    command.Parameters.Add(returnIdParameter);

                    // Execute the command
                    conn.Open();
                    await command.ExecuteNonQueryAsync();

                    // Retrieve the output value
                    long returnId = (long)returnIdParameter.Value;

                    // Close the connection and return the output value
                    conn.Close();
                    return returnId;
                }

            }
            catch (Exception EX)
            {

                return 0;
            }


        }
        public async Task ChartParametersDeleteAsync(long chartid)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ChartId", chartid, DbType.Int64);
                parameters.Add("@IsDeleted", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                await conn.ExecuteAsync("Delete_ChartParameters", parameters, commandType: CommandType.StoredProcedure);
                var isDeleted = parameters.Get<bool>("@IsDeleted");
            }
        }

        public async Task<long> ChartParametersAddUpdateAsync(long chartid, string tag, string sqlplaceholder, bool isRequired, string defaultValue)
        {
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                using (var command = new SqlCommand("[dbo].[ChartParametersAddUpdate]", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ChartId", chartid);
                    command.Parameters.AddWithValue("@Tag", tag);
                    command.Parameters.AddWithValue("@SQLPlaceHolder", sqlplaceholder);
                    command.Parameters.AddWithValue("@IsRequired", isRequired);
                    command.Parameters.AddWithValue("@DefaultValue", defaultValue);
                    var returnIdParameter = new SqlParameter
                    {
                        ParameterName = "@ReturnID",
                        SqlDbType = SqlDbType.BigInt,
                        Direction = ParameterDirection.Output

                    };
                    command.Parameters.Add(returnIdParameter);

                    // Execute the command
                    conn.Open();
                    await command.ExecuteNonQueryAsync();

                    // Retrieve the output value
                    long returnId = (long)returnIdParameter.Value;

                    // Close the connection and return the output value
                    conn.Close();
                    return returnId;
                }

            }
            catch (Exception EX)
            {

                return 0;
            }


        }

        public async Task<bool> DeleteChartAsync(long chartId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ChartId", chartId, DbType.Int64);
                parameters.Add("@IsDeleted", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync("Delete_Chart", parameters, commandType: CommandType.StoredProcedure);

                var isDeleted = parameters.Get<bool>("@IsDeleted");

                return isDeleted;
            }
        }

        public async Task<List<ExtendChart>> ChartsGetAllAsync(string userID = "")
        {
            List<ExtendChart> charts = new List<ExtendChart>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    var p = new
                    {
                        UserId = userID
                    };

                    charts = (await conn.QueryAsync<ExtendChart, DBConnections, ChartTypes, ExtendChart>(
                        "ChartsGetALL @UserId",
                        (ext, dbc, ctype) =>
                        {
                            ext.DBConnections = dbc;
                            ext.ChartTypes = ctype;
                            return ext;
                        },
                        p
                        )
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return charts;
        }


        public async Task<List<ExtendChart>> ChartGetByIdAsync(long id)
        {
            List<ExtendChart> charts = new List<ExtendChart>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    var p = new
                    {
                        Id = id
                    };

                    charts = (await conn.QueryAsync<ExtendChart, DBConnections, ChartTypes, ExtendChart>(
                        "ChartGetById @Id",
                        (ext, dbc, ctype) =>
                        {
                            ext.DBConnections = dbc;
                            ext.ChartTypes = ctype;
                            return ext;
                        },
                        p
                        )
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return charts;
        }

        public async Task<List<ChartTypes>> GetChartTypes()
        {
            List<ChartTypes> chartTypes = new List<ChartTypes>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                await conn.OpenAsync();
                chartTypes = (List<ChartTypes>)await conn.QueryAsync<ChartTypes>("GetChartTypes");
            }
            return chartTypes;
        }

        public async Task<List<SizeStyles>> GetSizeStyles()
        {
            List<SizeStyles> sizeStyles = new List<SizeStyles>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                await conn.OpenAsync();
                sizeStyles = (List<SizeStyles>)await conn.QueryAsync<SizeStyles>("GetSizeStyles");
            }
            return sizeStyles;
        }




        public async Task<List<ChartColorTheme>> GetChartColorTheme()
        {
            List<ChartColorTheme> chartTypes = new List<ChartColorTheme>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                await conn.OpenAsync();
                chartTypes = (List<ChartColorTheme>)await conn.QueryAsync<ChartColorTheme>("GetChartTheems");
            }
            return chartTypes;
        }
        public async Task<long> LinkChartWithUsers(Lnk_Charts_Users model)
        {

            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                using (var command = new SqlCommand("[dbo].[Lnk_Charts_UsersAddEdit]", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", model.ID);
                    command.Parameters.AddWithValue("@ChartID", model.ChartId);
                    command.Parameters.AddWithValue("@UserId", model.UserId);

                    var returnIdParameter = new SqlParameter
                    {
                        ParameterName = "@ReturnID",
                        SqlDbType = SqlDbType.BigInt,
                        Direction = ParameterDirection.Output

                    };
                    command.Parameters.Add(returnIdParameter);
                    conn.Open();
                    await command.ExecuteNonQueryAsync();
                    long returnId = (long)returnIdParameter.Value;
                    conn.Close();
                    return returnId;
                }
            }
            catch (Exception EX)
            {
                return 0;
            }
        }

        public async Task<List<ExtendLnk_Charts_Users>> GetLinkChartsUsersByChartIDAsync(long chartId = 0)
        {
            List<ExtendLnk_Charts_Users> Lnk_Charts_Users = new List<ExtendLnk_Charts_Users>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    var p = new
                    {
                        ChartID = chartId
                    };

                    Lnk_Charts_Users = (await conn.QueryAsync<ExtendLnk_Charts_Users, Charts, Users, ChartTypes, ExtendLnk_Charts_Users>(
                        "GetLnk_Charts_UsersByChartID @ChartID",
                        (ext, chart, user, charttypes) =>
                        {
                            ext.Charts = chart;
                            ext.User = user;
                            ext.ChartTypes = charttypes;
                            return ext;
                        },
                        p
                        )
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return Lnk_Charts_Users;
        }
        public async Task<bool> DeleteLnkChartUserAsync(long chartId, string userID)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ChartId", chartId, DbType.Int64);
                parameters.Add("@UserId", userID, DbType.String);

                parameters.Add("@IsDeleted", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync("Delete_LnkChartUser", parameters, commandType: CommandType.StoredProcedure);

                var isDeleted = parameters.Get<bool>("@IsDeleted");

                return isDeleted;
            }
        }
        #endregion

        #region Users
        public async Task<List<Users>> GetUsers()
        {
            SqlConnection conn = new SqlConnection(connStr);
            var result = (await conn.QueryAsync<Users>("UsersGetAll")).ToList();
            return result;
        }
        #endregion


        #region Common Methods

        public async Task<List<ExtendDBConnections>> GetDBConnections(string userId = "")
        {
            List<ExtendDBConnections> connections = new List<ExtendDBConnections>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                var p = new
                {
                    UserId = userId,
                };
                connections = (await conn.QueryAsync<ExtendDBConnections, DBProvider, ExtendDBConnections>
                    ("GetDBConnections @UserId", (ext, dbp) =>
                    {
                        ext.DBProvider = dbp;
                        return ext;
                    }, p)).ToList();
            }
            return connections;
        }
        //public IEnumerable<ChartDataset> DatasetExecute(string script, string connectionString)
        //{
        //    using (var con = new SqlConnection(connectionString))
        //    {
        //        return con.Query<ChartDataset>(script);
        //    }
        //}
        //public IEnumerable<ChartDataset> DatasetExecute(string script, string connectionString)
        //{
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        var result = connection.Query<ChartDataset>(script, commandType: CommandType.Text);

        //        return result.ToList();
        //    }
        //}
        public async Task<List<ChartDataset>> DatasetExecute(string script, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                // Increase the command timeout to 120 seconds (2 minutes)
                connection.InfoMessage += (sender, e) =>
                {
                    if (e.Errors != null)
                    {
                        foreach (SqlError error in e.Errors)
                        {
                            // Check if this is a timeout error
                            if (error.Class == 11)
                            {
                                //throw new TimeoutException("Query execution timed out.");
                            }
                        }
                    }
                };

                connection.FireInfoMessageEventOnUserErrors = true;
                await connection.OpenAsync(); // Open connection asynchronously
                connection.CreateCommand().CommandTimeout = 120;

                var result = await connection.QueryAsync<ChartDataset>(script, commandType: CommandType.Text); // Execute query asynchronously

                return result.ToList();
            }
        }



        public async Task<List<ChartParameter>> GetChartParametersGetAll()
        {
            SqlConnection conn = new SqlConnection(connStr);
            var result = (await conn.QueryAsync<ChartParameter>("ChartParametersGetAll")).ToList();
            return result;
        }
        public async Task<List<ChartParameter>> GetChartParametersByChartID(long chartid)
        {
            SqlConnection conn = new SqlConnection(connStr);
            var result = (await conn.QueryAsync<ChartParameter>("ChartParametersGetByChartID @ChartID",
                new { ChartID = chartid })).ToList();
            return result;
        }

        public async Task<List<ChartThemeExtends>> GetChartTheme(long chartid)
        {
            List<ChartThemeExtends> chartThemeExtends = new List<ChartThemeExtends>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    var p = new
                    {
                        ChartID = chartid,
                    };
                    chartThemeExtends = (await conn.QueryAsync<ChartThemeExtends, ChartColorTheme, ChartColor, ChartThemeExtends>
                    ("GetChartTheemByChartID @ChartID", (ext, ct, cc) =>
                    {
                        ext.ChartColorTheme = ct;
                        ext.ChartColor = cc;
                        return ext;
                    }, p)).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return chartThemeExtends;
        }


        public async Task<ChartScriptTemplates> GetChartScriptTemplateByChartTypeIDAsync(long chartTypeID)
        {
            SqlConnection conn = new SqlConnection(connStr);
            var result = (await conn.QueryAsync<ChartScriptTemplates>("GetChartScriptTemplateByChartTypeID @ChartTypeID",
                new { ChartTypeID = chartTypeID })).FirstOrDefault();
            return result;
        }

        public async Task<List<Dictionary<string, object>>> GridDatasetExecute(string script, string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                var result = await conn.QueryAsync<dynamic>(script, commandType: CommandType.Text); // Execute query asynchronously


                // Convert dynamic objects into Dictionary<string, object>
                List<Dictionary<string, object>> resultList = new();

                foreach (var row in result)
                {
                    var dict = new Dictionary<string, object>();

                    foreach (var property in (IDictionary<string, object>)row)
                    {
                        dict[property.Key] = property.Value;
                    }

                    resultList.Add(dict);
                }

                return resultList; // Returns a list of dynamic rows as dictionaries
            }
            catch (Exception ex)
            {

                throw;
            }
        }      

        #endregion
    }
}
