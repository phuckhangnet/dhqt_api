using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.Menu
{
    public class UpdateMenuResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public MenuDto? RESPONSES { get; set; }
    }
    public class UpdateMenuCommand : IRequest<UpdateMenuResponse>
    {
        public int? ID { get; set; }
        public string? Description { get; set; }
        public string? Hastag { get; set; }
        public int Menu_Level { get; set; }
    }
    public class UpdateMenuValidator : AbstractValidator<UpdateMenuCommand>
    {
        public UpdateMenuValidator()
        {
            RuleFor(x => x.ID).NotNull().NotEmpty().WithMessage("ID không được trống");
            // RuleFor(x => x.Identify).NotNull().NotEmpty().WithMessage("CMND không được trống");
            // RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email không được trống");
            // RuleFor(x => x.Phone).NotNull().NotEmpty().WithMessage("SĐT không được trống");
            // RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password không được trống");
        }
    }
    public class UpdateMenuHandler : IRequestHandler<UpdateMenuCommand, UpdateMenuResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public UpdateMenuHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<UpdateMenuResponse> Handle(UpdateMenuCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Project.Models.Menu? _Menu_to_update = await _dbContext.Menu.FirstOrDefaultAsync(x => x.ID == command.ID, cancellationToken);
                    if (_Menu_to_update != null)
                    {
                        _mapper.Map<UpdateMenuCommand, Project.Models.Menu>(command, _Menu_to_update);
                        _dbContext.Menu.Update(_Menu_to_update);
                        await _dbContext.SaveChangesAsync(cancellationToken);
                        dbContextTransaction.Commit();
                        return new UpdateMenuResponse
                        {
                            MESSAGE = "Cập nhật thành công!",
                            STATUSCODE = HttpStatusCode.OK,
                            RESPONSES = _mapper.Map<MenuDto>(_Menu_to_update)
                        };
                    }
                    else
                    {
                        return new UpdateMenuResponse
                        {
                            MESSAGE = "Cập nhật thất bại!",
                            STATUSCODE = HttpStatusCode.BadRequest
                        };
                    }
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new UpdateMenuResponse
                    {
                        MESSAGE = "Cập nhật thất bại!",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
