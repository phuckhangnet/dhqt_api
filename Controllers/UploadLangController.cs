using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Net;
using Project.Data;
using AutoMapper;
using Project.UseCases;

namespace ProjectBE.Controllers;
[Route("upload_lang")]
public class UploadLangController : Controller
{
    private readonly ILogger<UploadController> _logger;
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;
    private readonly DataContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _accessor;

    public UploadLangController(ILogger<UploadController> logger, IMediator mediator, IConfiguration configuration, DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
    {
        _logger = logger;
        _mediator = mediator;
        _configuration = configuration;
        _dbContext = dbContext;
        _mapper = mapper;
        _accessor = accessor;
    }
    [HttpPost("verify_upload")]
    public async Task<List<string>> UploadFile()
    {
        using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
        {
            var httpRequest = HttpContext.Request;
            if (httpRequest.Form.Files.Count > 0)
            {
                var files_uploaded = new List<string>();
                var firstfile_name = httpRequest.Form["File_Name"][0];

                var collectionfile = httpRequest.Form.Files;
                foreach (IFormFile file in collectionfile)
                {
                    //var file_name = firstfile_name + "_" + DateTime.Now.ToString("ddMMyyyyhhmmssfff") + "-";
                    var file_name = firstfile_name;
                    var uploaded_result = UploadFileFtp(file, file_name);
                    if (uploaded_result.IndexOf("err: ") == -1 && uploaded_result != "")
                        files_uploaded.Add(uploaded_result);

                }

                return files_uploaded;
            }
            return new List<string>();
        }
    }
    private string UploadFileFtp(IFormFile file, string filename)
    {
        var urlfile = this._configuration.GetSection("FtpStorageConnection")["url"];
        var ftpUsername = this._configuration.GetSection("FtpStorageConnection")["user"];
        var ftpPassword = this._configuration.GetSection("FtpStorageConnection")["pass"];
        //
        try
        {
            if (file != null)
            {
                var uri = urlfile + "Language/" + filename;
                var request = (FtpWebRequest)WebRequest.Create(uri);
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UseBinary = true;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var buffer = ms.ToArray();
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Close();
                    // act on the Base64 data
                }
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();

                return filename;
            }
            else
                return "";
        }
        catch (Exception ex) { return "err: " + ex.Message; }
    }
}
