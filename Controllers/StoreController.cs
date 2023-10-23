using Microsoft.AspNetCore.Mvc;
using MediatR;
using Project.Data;
using Newtonsoft.Json.Linq;
using Microsoft.Data.SqlClient;
using System.Data;
using ProjectBE.Models;
using InfluxDB3.Client;
using InfluxDB3.Client.Write;
using System.Diagnostics;

namespace ProjectBE.Controllers;
[Route("store")]
[ApiController]
public class StoreController : Controller
{
    private readonly ILogger<StoreController> _logger;
    private readonly IMediator _mediator;
    private readonly DataContext _dbContext;
    private readonly IConfiguration _configuration;
    public StoreController(ILogger<StoreController> logger, IMediator mediator, DataContext dbContext, IConfiguration configuration)
    {
        _logger = logger;
        _mediator = mediator;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    [HttpPost("StoredProcedure")]
    public async Task<dynamic> getListForUI([FromBody] dynamic request)
    {
        var parameters = JObject.Parse(request.ToString());

        try
        {
            var time = DateTime.UtcNow;
            var result = new List<Dictionary<string, dynamic>>();
            using (SqlConnection con = new SqlConnection(this._configuration.GetSection("ConnectionStrings")["IU_BT_DB"]))
            {
                using (SqlCommand cmd = new SqlCommand("getListForUI", con))
                {
                    var lstPara = new Dictionary<string, dynamic>();
                    foreach (JProperty para in (JToken)parameters)
                    {
                        var key = para.Name;
                        var val = parameters[key].Value;
                        cmd.Parameters.AddWithValue("@" + key, val);
                    }
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        var dt = new DataTable();
                        sda.Fill(dt);
                        con.Close();
                        result = TableToListDict(dt);
                    }
                }
            }

            try
            {
                if (_configuration.GetValue<bool>("Logging:Allow"))
                {
                    var hostUrl = InfluxDBModel.hostUrl;

                    using var client = new InfluxDBClient(hostUrl, token: InfluxDBModel.token);

                    const string database = "hcmiu";

                    var points = new PointData[]
                    {
                        PointData.Measurement("Store")
                        .SetField("Query", (DateTime.UtcNow - time).TotalSeconds),
                    

                    };

                    await client.WritePointsAsync(points: points, database: database);
                }

                


            }
            catch { }


            return Ok(new
            {
                message = "success",
                statuscode = 200,
                responses = result,
            });
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static List<Dictionary<string, dynamic>> TableToListDict(DataTable dt)
    {
        var columns = dt.Columns.Cast<DataColumn>();
        var temp = dt.AsEnumerable().Select(dataRow => columns.Select(column =>
                             new { Column = column.ColumnName, Value = dataRow[column] })
                         .ToDictionary(data => data.Column, data => data.Value)).ToList();
        return temp.ToList();
    }
}

// using Microsoft.AspNetCore.Mvc;
// using MediatR;
// using Project.Data;
// using Microsoft.EntityFrameworkCore;
// using Newtonsoft.Json.Linq;
// using Microsoft.Data.SqlClient;
// using System.Data;
// using Dapper;

// namespace ProjectBE.Controllers;
// [Route("store")]
// [ApiController]
// public class StoreController : Controller
// {
//     private readonly ILogger<StoreController> _logger;
//     private readonly IMediator _mediator;
//     private readonly DataContext _dbContext;
//     private readonly IConfiguration _configuration;
//     public StoreController(ILogger<StoreController> logger, IMediator mediator, DataContext dbContext, IConfiguration configuration)
//     {
//         _logger = logger;
//         _mediator = mediator;
//         _dbContext = dbContext;
//         _configuration = configuration;
//     }
//     public class StoreRequest
//     {
//         public int loai { get; set; }
//     }
//     [HttpPost("StoredProcedure")]
//     //[DecryptedAttribute] // Chỉ sử dụng attribute này cho những route cần Encrypt request!!
//     public async Task<dynamic> getListForUI(StoreRequest request)
//     {
//         var Connection = new SqlConnection(this._configuration.GetSection("ConnectionStrings")["IU_BT_DB"]);
//         using (IDbConnection conn = Connection)
//         {
//             string sQuery = "EXEC [phu70569_zeus].[getListForUI] @loai";
//             conn.Open();
//             var result = await conn.QueryAsync<dynamic>(sQuery,
//                 new
//                 {
//                     loai = request.loai,
//                 });
//             return result;
//         }
//     }
// }