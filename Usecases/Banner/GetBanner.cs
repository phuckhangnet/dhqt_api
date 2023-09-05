using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.Banner
{
    public class GetBannerResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public IEnumerable<Project.Models.Banner>? RESPONSES { get; set; }
    }
    public class GetBannerCommand : IRequest<GetBannerResponse>
    {
    }
    public class GetBannerValidator : AbstractValidator<GetBannerCommand>
    {
        public GetBannerValidator()
        {
        }
    }
    public class GetBannerHandler : IRequestHandler<GetBannerCommand, GetBannerResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public GetBannerHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<GetBannerResponse> Handle(GetBannerCommand command, CancellationToken cancellationToken)
        {

            try
            {
                IEnumerable<Project.Models.Banner> list_Banner_response = Enumerable.Empty<Project.Models.Banner>();


                list_Banner_response = await _dbContext.Banner.OrderBy(x => x.ID).ToListAsync(cancellationToken);


                return new GetBannerResponse
                {
                    MESSAGE = "GET_SUCCESSFUL",
                    STATUSCODE = HttpStatusCode.OK,
                    RESPONSES = _mapper.Map<IEnumerable<Project.Models.Banner>>(list_Banner_response)
                };
            }
            catch
            {
                return new GetBannerResponse
                {
                    MESSAGE = "GET_FAIL",
                    STATUSCODE = HttpStatusCode.InternalServerError
                };
            }

        }
    }
}
