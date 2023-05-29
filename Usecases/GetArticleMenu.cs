using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.ArticleMenu
{
    public class GetArticleMenuResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public dynamic RESPONSES { get; set; }
    }
    public class GetArticleMenuCommand : IRequest<GetArticleMenuResponse>
    {
        public string? Type { get; set; }
        public IEnumerable<string> Data { get; set; } = Enumerable.Empty<string>();
    }
    public class GetArticleMenuValidator : AbstractValidator<GetArticleMenuCommand>
    {
        public GetArticleMenuValidator()
        {
            RuleFor(x => x.Type).NotNull().NotEmpty().WithMessage("Loại truy vấn không được trống");
            RuleFor(x => x.Data).NotNull().NotEmpty().WithMessage("Thông tin truy vấn không được trống");
        }
    }
    public class GetArticleMenuHandler : IRequestHandler<GetArticleMenuCommand, GetArticleMenuResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public GetArticleMenuHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<GetArticleMenuResponse> Handle(GetArticleMenuCommand command, CancellationToken cancellationToken)
        {

            try
            {
                IEnumerable<Project.Models.Article_Menu> list_ArticleMenu_response = Enumerable.Empty<Project.Models.Article_Menu>();

                switch (command.Type)
                {
                    case "GET_ALL":
                        list_ArticleMenu_response = await _dbContext.Article_Menu.Where(x => x.ARTICLEID == 38).ToListAsync(cancellationToken);
                        break;
                }

                return new GetArticleMenuResponse
                {
                    MESSAGE = "GET_SUCCESSFUL",
                    STATUSCODE = HttpStatusCode.OK,
                    RESPONSES = list_ArticleMenu_response
                };
            }
            catch
            {
                return new GetArticleMenuResponse
                {
                    MESSAGE = "GET_FAIL",
                    STATUSCODE = HttpStatusCode.InternalServerError
                };
            }

        }
    }
}
