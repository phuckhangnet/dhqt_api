using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.UI
{
    public class GetUIResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public IEnumerable<Project.Models.UI>? RESPONSES { get; set; }
    }
    public class GetUICommand : IRequest<GetUIResponse>
    {
    }
    public class GetUIValidator : AbstractValidator<GetUICommand>
    {
        public GetUIValidator()
        {
        }
    }
    public class GetUIHandler : IRequestHandler<GetUICommand, GetUIResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public GetUIHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<GetUIResponse> Handle(GetUICommand command, CancellationToken cancellationToken)
        {

            try
            {
                IEnumerable<Project.Models.UI> list_UI_response = Enumerable.Empty<Project.Models.UI>();


                list_UI_response = await _dbContext.UI.OrderBy(x => x.ID).ToListAsync(cancellationToken);


                return new GetUIResponse
                {
                    MESSAGE = "GET_SUCCESSFUL",
                    STATUSCODE = HttpStatusCode.OK,
                    RESPONSES = _mapper.Map<IEnumerable<Project.Models.UI>>(list_UI_response)
                };
            }
            catch
            {
                return new GetUIResponse
                {
                    MESSAGE = "GET_FAIL",
                    STATUSCODE = HttpStatusCode.InternalServerError
                };
            }

        }
    }
}
