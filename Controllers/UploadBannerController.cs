using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Net;
using Project.Data;
using AutoMapper;
using Project.UseCases;
using Project.Models;

namespace ProjectBE.Controllers;
[Route("upload_banner")]
public class UploadBannerController : Controller
{
    private readonly ILogger<UploadController> _logger;
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;
    private readonly DataContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _accessor;

    public UploadBannerController(ILogger<UploadController> logger, IMediator mediator, IConfiguration configuration, DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
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
            if (httpRequest.Form.Count > 0)
            {
                var files_uploaded = new List<string>();
                var firstfile_name = httpRequest.Form["File_Name"][0];
                var firstfolder_name = httpRequest.Form["Folder_Name"][0];
                var oldfile_name = httpRequest.Form["Old_File"][0];
                var oldfolder_ex = httpRequest.Form["Old_File_Ex"][0];
                var od = httpRequest.Form["Ordinal"][0];

                var collectionfile = httpRequest.Form.Files;
                foreach (IFormFile file in collectionfile)
                {
                    var file_name = firstfile_name + "-" + od;
                    var folder_name = firstfolder_name;

                    var uploaded_result = UploadFileFtp(file, file_name, folder_name, oldfile_name, oldfolder_ex);
                    GeneralRepository _generalRepo = new GeneralRepository(_dbContext);
                    var banner_to_delete = _dbContext.Banner.Where(x => x.TYPE == folder_name && x.FILENAME == oldfile_name).FirstOrDefault();
                    var iddelete = banner_to_delete.ID;
                    _dbContext.Banner.Remove(banner_to_delete);
                    _dbContext.SaveChanges();

                    Banner _files_upload = new Banner();
                    _files_upload.ID = iddelete;
                    _files_upload.TYPE = firstfolder_name;
                    _files_upload.REALNAME = file.FileName.Substring(0, file.FileName.LastIndexOf("."));
                    _files_upload.FILENAME = file_name;
                    _files_upload.FILEEXTENSION = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
                    Project.Models.Banner _files_to_add_db = _mapper.Map<Project.Models.Banner>(_files_upload);
                    _dbContext.Add(_files_to_add_db);
                    _dbContext.SaveChanges();
                    dbContextTransaction.Commit();
                    if (uploaded_result.IndexOf("err: ") == -1 && uploaded_result != "")
                        files_uploaded.Add(uploaded_result);

                }

                return files_uploaded;
            }
            return new List<string>();
        }
    }
    private string UploadFileFtp(IFormFile file, string filename, string foldername, string oldfile, string oldfile_ex)
    {
        var urlfile = this._configuration.GetSection("FtpImagesConnection")["url"];
        var ftpUsername = this._configuration.GetSection("FtpImagesConnection")["user"];
        var ftpPassword = this._configuration.GetSection("FtpImagesConnection")["pass"];
        //
        try
        {
            if (file != null)
            {

                var uridelete = urlfile + foldername + "/" + oldfile + "." + oldfile_ex;
                var requestdelete = (FtpWebRequest)WebRequest.Create(uridelete);
                requestdelete.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                requestdelete.Method = WebRequestMethods.Ftp.DeleteFile;
                requestdelete.UseBinary = true;
                FtpWebResponse responsedelete = (FtpWebResponse)requestdelete.GetResponse();
                responsedelete.Close();

                filename += file.FileName.Substring(file.FileName.LastIndexOf("."));
                var uriadd = urlfile + foldername + "/" + filename;
                var requestadd = (FtpWebRequest)WebRequest.Create(uriadd);
                requestadd.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                requestadd.Method = WebRequestMethods.Ftp.UploadFile;
                requestadd.UseBinary = true;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var buffer = ms.ToArray();
                    Stream requestStream = requestadd.GetRequestStream();
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Close();
                    // act on the Base64 data
                }
                FtpWebResponse responseadd = (FtpWebResponse)requestadd.GetResponse();
                responseadd.Close();

                return filename;
            }
            else
                return "";
        }
        catch (Exception ex) { return "err: " + ex.Message; }
    }
}
