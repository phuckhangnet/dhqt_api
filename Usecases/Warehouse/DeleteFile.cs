using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Project.UseCases.File
{
    public class DeleteFileResponse
    {
        public string? MESSAGE { get; set; }
        public string? ERROR { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
    }
    public class DeleteFileCommand : IRequest<DeleteFileResponse>
    {
        public int? fileid { get; set; }
        public string? filename { get; set; }
    }
    public class DeleteFileValidator : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileValidator()
        {
        }
    }
    public class DeleteFileHandler : IRequestHandler<DeleteFileCommand, DeleteFileResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _configuration;

        public DeleteFileHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor, IConfiguration configuration)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
            _configuration = configuration;
        }
        public async Task<DeleteFileResponse> Handle(DeleteFileCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    var urlfile = this._configuration.GetSection("FtpStorageConnection")["url"] + command.filename;
                    var ftpUsername = this._configuration.GetSection("FtpStorageConnection")["user"];
                    var ftpPassword = this._configuration.GetSection("FtpStorageConnection")["pass"];

                    if (urlfile != null)
                    {
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(urlfile);
                        request.Method = WebRequestMethods.Ftp.DeleteFile;
                        request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                        {
                            if (response.ToString() == "System.Net.FtpWebResponse")
                            {
                                _dbContext.Upload_Files_Warehouse.Remove(_dbContext.Upload_Files_Warehouse.Find(command.fileid));
                                _dbContext.SaveChanges();
                                dbContextTransaction.Commit();
                                return new DeleteFileResponse
                                {
                                    MESSAGE = "DELETE_SUCCESSFUL",
                                    STATUSCODE = HttpStatusCode.OK
                                };
                            }
                            else return new DeleteFileResponse
                            {
                                MESSAGE = "DELETE_FAIL",
                                STATUSCODE = HttpStatusCode.InternalServerError
                            };
                        }
                    }
                    else
                    {
                        _dbContext.Upload_Files_Warehouse.Remove(_dbContext.Upload_Files_Warehouse.Find(command.fileid));
                        _dbContext.SaveChanges();
                        dbContextTransaction.Commit();
                        return new DeleteFileResponse
                        {
                            MESSAGE = "DELETE_SUCCESSFUL",
                            STATUSCODE = HttpStatusCode.OK
                        };
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() == "The remote server returned an error: (550) File unavailable (e.g., file not found, no access).")
                    {
                        _dbContext.Upload_Files_Warehouse.Remove(_dbContext.Upload_Files_Warehouse.Find(command.fileid));
                        _dbContext.SaveChanges();
                        dbContextTransaction.Commit();
                        return new DeleteFileResponse
                        {
                            MESSAGE = "DELETE_SUCCESSFUL",
                            STATUSCODE = HttpStatusCode.OK
                        };
                    }
                    else
                    {
                        dbContextTransaction.Rollback();
                        return new DeleteFileResponse
                        {
                            MESSAGE = "DELETE_FAIL",
                            ERROR = ex.ToString(),
                            STATUSCODE = HttpStatusCode.InternalServerError
                        };
                    }

                }
            }
        }
    }
}
