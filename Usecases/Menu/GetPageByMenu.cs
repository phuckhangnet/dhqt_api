using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Project.UseCases.Menu
{
    public class GetPageByMenuResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public dynamic? RESPONSES { get; set; }
    }
    public class GetPageByMenuCommand : IRequest<GetPageByMenuResponse>
    {
        public string? ID { get; set; }
    }
    public class GetPageByMenuValidator : AbstractValidator<GetPageByMenuCommand>
    {
        public GetPageByMenuValidator()
        {
            RuleFor(x => x.ID).NotNull().NotEmpty().WithMessage("Loại truy vấn không được trống");
        }
    }
    public class GetPageByMenuHandler : IRequestHandler<GetPageByMenuCommand, GetPageByMenuResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public GetPageByMenuHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<GetPageByMenuResponse> Handle(GetPageByMenuCommand command, CancellationToken cancellationToken)
        {

            try
            {
                string page;
                var menu = await _dbContext.Menu.Where(x => x.ID == Int32.Parse(command.ID)).FirstOrDefaultAsync(cancellationToken);
                if (menu.PARENT == "0")
                {
                    page = "";
                }
                else if (menu.DESCRIPTION.Substring(menu.DESCRIPTION.IndexOf("relg=") + 6, menu.DESCRIPTION.Substring(menu.DESCRIPTION.IndexOf("relg=") + 6).IndexOf("\"")) == "biotechnology")
                {
                    page = "biotechnology";
                }
                else if (menu.DESCRIPTION.Substring(menu.DESCRIPTION.IndexOf("relg=") + 6, menu.DESCRIPTION.Substring(menu.DESCRIPTION.IndexOf("relg=") + 6).IndexOf("\"")) == "food-technology")
                {
                    page = "food-technology";
                }
                else if (menu.DESCRIPTION.Substring(menu.DESCRIPTION.IndexOf("relg=") + 6, menu.DESCRIPTION.Substring(menu.DESCRIPTION.IndexOf("relg=") + 6).IndexOf("\"")) == "applied-chemistry")
                {
                    page = "applied-chemistry";
                }
                else if (menu.DESCRIPTION.Substring(menu.DESCRIPTION.IndexOf("relg=") + 6, menu.DESCRIPTION.Substring(menu.DESCRIPTION.IndexOf("relg=") + 6).IndexOf("\"")) == "aquascience")
                {
                    page = "aquascience";
                }
                else if (menu.DESCRIPTION.Substring(menu.DESCRIPTION.IndexOf("relg=") + 6, menu.DESCRIPTION.Substring(menu.DESCRIPTION.IndexOf("relg=") + 6).IndexOf("\"")) == "alumni")
                {
                    page = "alumni";
                }
                else if (menu.DESCRIPTION.Substring(menu.DESCRIPTION.IndexOf("relg=") + 6, menu.DESCRIPTION.Substring(menu.DESCRIPTION.IndexOf("relg=") + 6).IndexOf("\"")) == "admission")
                {
                    page = "admission";
                }
                else
                {
                    var result = await _dbContext.Menu.Where(x => x.ID == Int32.Parse(menu.PARENT)).FirstOrDefaultAsync(cancellationToken);
                    while (result.PARENT != "0" && !result.DESCRIPTION.Contains("page=\"home\"") && !result.DESCRIPTION.Contains("webpage=\"true\""))
                    {
                        result = await _dbContext.Menu.Where(x => x.ID == Int32.Parse(result.PARENT)).FirstOrDefaultAsync(cancellationToken);
                    }
                    if (result.DESCRIPTION.Contains("page=\"home\"") || result.PARENT == "0")
                    {
                        page = "";
                    }
                    else
                    {
                        page = result.DESCRIPTION.Substring(result.DESCRIPTION.IndexOf("relg=") + 6, result.DESCRIPTION.Substring(result.DESCRIPTION.IndexOf("relg=") + 6).IndexOf("\""));
                    }
                }

                return new GetPageByMenuResponse
                {
                    MESSAGE = "GET_SUCCESSFUL",
                    STATUSCODE = HttpStatusCode.OK,
                    RESPONSES = page,
                };
            }
            catch
            {
                return new GetPageByMenuResponse
                {
                    MESSAGE = "GET_FAIL",
                    STATUSCODE = HttpStatusCode.InternalServerError
                };
            }

        }
    }
}