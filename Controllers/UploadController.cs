using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Net;
using Project.Data;
using AutoMapper;
using Project.UseCases;

namespace ProjectBE.Controllers;
[Route("upload")]
public class UploadController : Controller
{
    private readonly ILogger<UploadController> _logger;
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;
    private readonly DataContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _accessor;

    public UploadController(ILogger<UploadController> logger, IMediator mediator, IConfiguration configuration, DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
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
                var iduser = httpRequest.Form["ID_User"][0];
                var countfile = httpRequest.Form.Files.Count;
                if (countfile == 0)
                {
                    var numberofbase64 = Convert.ToInt32(httpRequest.Form["Number_Of_Base64"][0]);
                    var iCount = 0;
                    while (numberofbase64 > 0)
                    {
                        var file_name = firstfile_name + "_" + DateTime.Now.ToString("ddMMyyyyhhmmssfff");
                        var myBase64 = httpRequest.Form["My_Base64_" + iCount.ToString()].ToString();
                        var myBase64Extension = httpRequest.Form["My_Base64_Extension_" + iCount.ToString()][0].ToString();
                        try
                        {
                            IFormFile file = Base64ToImage(myBase64, file_name + myBase64Extension);
                            var uploaded_result = UploadFileFtp(file, file_name);

                            GeneralRepository _generalRepo = new GeneralRepository(_dbContext);
                            Project.Models.Upload_Files_Mart _files_upload = new Project.Models.Upload_Files_Mart();
                            _files_upload.FILENAME = file_name;
                            _files_upload.FILESIZE = Convert.ToInt32(file.Length);
                            _files_upload.FILEEXTENSION = "image/" + myBase64Extension.Split(".")[1];
                            _files_upload.IDUSER = Int32.Parse(iduser);
                            Project.Models.Upload_Files_Mart _files_to_add_db = _mapper.Map<Project.Models.Upload_Files_Mart>(_files_upload);
                            _dbContext.Add(_files_to_add_db);
                            _dbContext.SaveChanges();
                            dbContextTransaction.Commit();

                            if (uploaded_result.IndexOf("err: ") == -1 && uploaded_result != "")
                                files_uploaded.Add(uploaded_result);
                        }
                        catch
                        {
                            files_uploaded.Add("");
                        }
                        iCount++;
                        numberofbase64--;
                    }
                }
                else
                {
                    var collectionfile = httpRequest.Form.Files;
                    foreach (IFormFile file in collectionfile)
                    {
                        var file_name = firstfile_name + "_" + DateTime.Now.ToString("ddMMyyyyhhmmssfff");
                        if (firstfile_name == "li")
                        {
                            var uploaded_result = UploadFileStorageFtp(file, file_name);
                            if (uploaded_result.IndexOf("err: ") == -1 && uploaded_result != "")
                                files_uploaded.Add(uploaded_result);
                            GeneralRepository _generalRepo = new GeneralRepository(_dbContext);
                            Project.Models.Upload_Files_Warehouse _files_upload = new Project.Models.Upload_Files_Warehouse();
                            _files_upload.REALNAME = file.FileName;
                            _files_upload.FILENAME = file_name;
                            _files_upload.FILESIZE = Convert.ToInt32(file.Length);
                            _files_upload.FILEEXTENSION = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
                            _files_upload.IDUSER = Int32.Parse(iduser);
                            Project.Models.Upload_Files_Warehouse _files_to_add_db = _mapper.Map<Project.Models.Upload_Files_Warehouse>(_files_upload);
                            _dbContext.Add(_files_to_add_db);
                            _dbContext.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                        else
                        {
                            var uploaded_result = UploadFileFtp(file, file_name);
                            if (uploaded_result.IndexOf("err: ") == -1 && uploaded_result != "")
                                files_uploaded.Add(uploaded_result);
                            GeneralRepository _generalRepo = new GeneralRepository(_dbContext);
                            Project.Models.Upload_Files_Mart _files_upload = new Project.Models.Upload_Files_Mart();
                            _files_upload.FILENAME = file_name;
                            _files_upload.FILESIZE = Convert.ToInt32(file.Length);
                            _files_upload.FILEEXTENSION = file.ContentType;
                            _files_upload.IDUSER = Int32.Parse(iduser);
                            Project.Models.Upload_Files_Mart _files_to_add_db = _mapper.Map<Project.Models.Upload_Files_Mart>(_files_upload);
                            _dbContext.Add(_files_to_add_db);
                            _dbContext.SaveChanges();
                            dbContextTransaction.Commit();
                        }


                    }
                }
                return files_uploaded;
            }
            return new List<string>();
        }
    }
    private string UploadFileFtp(IFormFile file, string filename)
    {
        var urlfile = this._configuration.GetSection("FtpImagesConnection")["url"];
        var ftpUsername = this._configuration.GetSection("FtpImagesConnection")["user"];
        var ftpPassword = this._configuration.GetSection("FtpImagesConnection")["pass"];
        //
        try
        {
            if (file != null)
            {
                filename += file.FileName.Substring(file.FileName.LastIndexOf("."));
                var uri = urlfile + filename;
                var request = (FtpWebRequest)WebRequest.Create(uri);
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
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
    private string UploadFileStorageFtp(IFormFile file, string filename)
    {
        var urlfile = this._configuration.GetSection("FtpStorageConnection")["url"];
        var ftpUsername = this._configuration.GetSection("FtpStorageConnection")["user"];
        var ftpPassword = this._configuration.GetSection("FtpStorageConnection")["pass"];
        //
        try
        {
            if (file != null)
            {
                filename += file.FileName.Substring(file.FileName.LastIndexOf("."));
                var uri = urlfile + filename;
                var request = (FtpWebRequest)WebRequest.Create(uri);
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
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
    private IFormFile Base64ToImage(string strbase64, string filename)
    {
        byte[] bytes = Convert.FromBase64String(strbase64);
        MemoryStream stream = new MemoryStream(bytes);
        IFormFile file = new FormFile(stream, 0, bytes.Length, filename, filename);

        return file;
    }

}
