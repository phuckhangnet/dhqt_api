using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Project.UseCases.ListLanguage
{
    public class DeleteListLanguageResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
    }
    public class DeleteListLanguageCommand : IRequest<DeleteListLanguageResponse>
    {
        public string? Code { get; set; }
    }
    public class DeleteListLanguageValidator : AbstractValidator<DeleteListLanguageCommand>
    {
        public DeleteListLanguageValidator()
        {
        }
    }
    public class DeleteListLanguageHandler : IRequestHandler<DeleteListLanguageCommand, DeleteListLanguageResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public DeleteListLanguageHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<DeleteListLanguageResponse> Handle(DeleteListLanguageCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    _dbContext.ListLanguage.Remove(_dbContext.ListLanguage.Find(command.Code));
                    _dbContext.SaveChanges();
                    dbContextTransaction.Commit();
                    return new DeleteListLanguageResponse
                    {
                        MESSAGE = "DELETE_SUCCESSFUL",
                        STATUSCODE = HttpStatusCode.OK,
                    };

                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new DeleteListLanguageResponse
                    {
                        MESSAGE = "DELETE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }

        }
    }
}
