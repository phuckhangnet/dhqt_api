using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.Role
{
    public class UpdateRoleResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public RoleDto? RESPONSES { get; set; }
    }
    public class UpdateRoleCommand : IRequest<UpdateRoleResponse>
    {
        public string? Code { get; set; }
        public string? Rule_List { get; set; }
    }
    public class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleValidator()
        {
        }
    }
    public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, UpdateRoleResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public UpdateRoleHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<UpdateRoleResponse> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Project.Models.Role? _Role_to_update = await _dbContext.Role.FirstOrDefaultAsync(x => x.CODE == command.Code, cancellationToken);
                    if (_Role_to_update != null)
                    {
                        _mapper.Map<UpdateRoleCommand, Project.Models.Role>(command, _Role_to_update);
                        _dbContext.Role.Update(_Role_to_update);
                        await _dbContext.SaveChangesAsync(cancellationToken);
                        dbContextTransaction.Commit();
                        return new UpdateRoleResponse
                        {
                            MESSAGE = "Cập nhật thành công!",
                            STATUSCODE = HttpStatusCode.OK,
                            RESPONSES = _mapper.Map<RoleDto>(_Role_to_update)
                        };
                    }
                    else
                    {
                        return new UpdateRoleResponse
                        {
                            MESSAGE = "Cập nhật thất bại!",
                            STATUSCODE = HttpStatusCode.BadRequest
                        };
                    }
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new UpdateRoleResponse
                    {
                        MESSAGE = "Cập nhật thất bại!",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
