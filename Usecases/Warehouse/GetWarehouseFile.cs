using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.WarehouseFile
{
    public class GetWarehouseFileResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public dynamic RESPONSES { get; set; }
    }
    public class GetWarehouseFileCommand : IRequest<GetWarehouseFileResponse>
    {
        public string? Type { get; set; }
        public IEnumerable<string> Data { get; set; } = Enumerable.Empty<string>();
    }
    public class GetWarehouseFileValidator : AbstractValidator<GetWarehouseFileCommand>
    {
        public GetWarehouseFileValidator()
        {
            RuleFor(x => x.Type).NotNull().NotEmpty().WithMessage("Loại truy vấn không được trống");
            RuleFor(x => x.Data).NotNull().NotEmpty().WithMessage("Thông tin truy vấn không được trống");
        }
    }
    public class GetWarehouseFileHandler : IRequestHandler<GetWarehouseFileCommand, GetWarehouseFileResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public GetWarehouseFileHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<GetWarehouseFileResponse> Handle(GetWarehouseFileCommand command, CancellationToken cancellationToken)
        {

            try
            {
                dynamic list_WarehouseFile_response
                = Enumerable.Empty<Project.Models.Upload_Files_Warehouse>();
                var iduser = _accessor.HttpContext?.Items["ID"]?.ToString();
                var _iduser = Int32.Parse(iduser);
                switch (command.Type)
                {
                    case "GET_ALL":
                        list_WarehouseFile_response = from warehouse_file in _dbContext.Upload_Files_Warehouse.ToList()
                                                      orderby warehouse_file.ID descending
                                                      join urs in _dbContext.Users.ToList() on warehouse_file.IDUSER equals urs.ID
                                                      join ursdetail in _dbContext.User_Detail.ToList() on urs.ID equals ursdetail.USERID
                                                      select new { warehouse_file, createuser = ursdetail.NAME };

                        break;
                }
                return new GetWarehouseFileResponse
                {
                    MESSAGE = "GET_SUCCESSFUL",
                    STATUSCODE = HttpStatusCode.OK,
                    RESPONSES = list_WarehouseFile_response
                };
            }
            catch
            {
                return new GetWarehouseFileResponse
                {
                    MESSAGE = "GET_FAIL",
                    STATUSCODE = HttpStatusCode.InternalServerError
                };
            }

        }
    }
}
