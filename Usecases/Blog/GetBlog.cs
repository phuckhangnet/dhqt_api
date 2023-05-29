using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.UseCases.Blog
{
    public class GetBlogResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public IEnumerable<BlogDto>? RESPONSES { get; set; }
    }
    public class GetBlogCommand : IRequest<GetBlogResponse>
    {
        public string? Type { get; set; }
        public IEnumerable<string> Data { get; set; } = Enumerable.Empty<string>();
        public int NoOfResult { get; set; }
    }
    public class GetBlogValidator : AbstractValidator<GetBlogCommand>
    {
        public GetBlogValidator()
        {
            RuleFor(x => x.Type).NotNull().NotEmpty().WithMessage("QUERY TYPE CANNOT BE EMPTY");
            RuleFor(x => x.Data).NotNull().NotEmpty().WithMessage("QUERY DATA CANNOT BE EMPTY");
        }
    }
    public class GetBlogHandler : IRequestHandler<GetBlogCommand, GetBlogResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public GetBlogHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<GetBlogResponse> Handle(GetBlogCommand command, CancellationToken cancellationToken)
        {

            try
            {
                var iduser = _accessor.HttpContext?.Items["ID"]?.ToString();
                var _iduser = Int32.Parse(iduser);
                IEnumerable<Project.Models.Blogs> list_Blog_response = Enumerable.Empty<Project.Models.Blogs>();
                IEnumerable<Project.Models.Blogs> result = Enumerable.Empty<Project.Models.Blogs>();
                if (command.Type == null)
                {
                    return new GetBlogResponse
                    {
                        MESSAGE = "MISSING_PARAMETER_TYPE",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
                else if (command.Type != "GET_ALL" && command.Type != "GET_BY_ID" && command.Type != "GET_BY_USER")
                {
                    return new GetBlogResponse
                    {
                        MESSAGE = "INVALID_PARAMETER_TYPE",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
                else if (command.Type == "GET_ALL")
                {
                    result = await _dbContext.Blogs.Where(x => x.IDUSER == _iduser).ToListAsync(cancellationToken);
                }
                else
                {
                    if (command.Data.Count() == 0)
                    {
                        return new GetBlogResponse
                        {
                            MESSAGE = "MISSING_PARAMETER_DATA",
                            STATUSCODE = HttpStatusCode.InternalServerError
                        };
                    }
                    else
                    {
                        switch (command.Type)
                        {
                            case "GET_BY_USER":

                                break;
                            case "GET_BY_ID":
                                result = await _dbContext.Blogs.Where(x => command.Data.Contains(x.ID.ToString())).ToListAsync(cancellationToken);
                                break;
                        }
                    }
                }

                return new GetBlogResponse
                {
                    MESSAGE = "GET_SUCCESSFUL",
                    STATUSCODE = HttpStatusCode.OK,
                    RESPONSES = _mapper.Map<IEnumerable<BlogDto>>(result)
                };
            }
            catch
            {
                return new GetBlogResponse
                {
                    MESSAGE = "GET_FAIL",
                    STATUSCODE = HttpStatusCode.InternalServerError
                };
            }

        }

        private bool checkArray(string[] a, string[] b)
        {
            a = a.Distinct().ToArray();
            b = b.Distinct().ToArray();
            var c = a.Concat(b).ToArray();
            var dup = c.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Count();
            if (dup == a.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
