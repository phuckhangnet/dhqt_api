using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Project.UseCases.Users
{
    public class UpdateUserResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public UserDto? RESPONSES { get; set; }
    }
    public class UpdateUserCommand : IRequest<UpdateUserResponse>
    {
        public int ID { get; set; }
        public string? Role { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public int? Colindex { get; set; }
        public string? Education { get; set; }
        public string? Office { get; set; }
        public string? Major { get; set; }
        public string? Research { get; set; }
        public string? Supervision { get; set; }
        public string? Publication { get; set; }
        public string? Language { get; set; }
        public string? TeachingCourse { get; set; }
        public string? Aboutme { get; set; }
        public IEnumerable<string>? Position { get; set; }
        public IEnumerable<string>? Title { get; set; }
        public IEnumerable<string>? Department { get; set; }

    }
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.ID).NotNull().NotEmpty().WithMessage("ID CANNOT BE EMPTY");
            // RuleFor(x => x.Identify).NotNull().NotEmpty().WithMessage("CMND không được trống");
            // RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email không được trống");
            // RuleFor(x => x.Phone).NotNull().NotEmpty().WithMessage("SĐT không được trống");
            // RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password không được trống");
        }
    }
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public UpdateUserHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<UpdateUserResponse> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Project.Models.User? _User_to_update = await _dbContext.Users.FirstOrDefaultAsync(x => x.ID == command.ID, cancellationToken);
                    Project.Models.UserDetail? _User_Detail_to_update = await _dbContext.User_Detail.FirstOrDefaultAsync(x => x.USERID == command.ID, cancellationToken);
                    if (_User_to_update != null)
                    {
                        //update account
                        _mapper.Map<UpdateUserCommand, Project.Models.User>(command, _User_to_update);
                        _dbContext.Users.Update(_User_to_update);
                        _dbContext.SaveChanges();

                        //update detail
                        _mapper.Map<UpdateUserCommand, Project.Models.UserDetail>(command, _User_Detail_to_update);
                        _dbContext.User_Detail.Update(_User_Detail_to_update);
                        _dbContext.SaveChanges();

                        //delete position, title, detail
                        _dbContext.User_List.RemoveRange(_dbContext.User_List.Where(x => x.USERID == command.ID));
                        _dbContext.SaveChanges();

                        //update position
                        Project.Models.UserList _User_Position_to_add = new Project.Models.UserList();
                        if (command.Position != null)
                        {
                            foreach (var position in command.Position)
                            {
                                _User_Position_to_add.USERID = command.ID;
                                _User_Position_to_add.TABLELIST = "POSITION";
                                _User_Position_to_add.LISTCODE = position;
                                _dbContext.Add(_User_Position_to_add);
                                _dbContext.SaveChanges();
                            }
                        }

                        //update title
                        Project.Models.UserList _User_Title_to_add = new Project.Models.UserList();
                        if (command.Title != null)
                        {
                            foreach (var title in command.Title)
                            {
                                _User_Title_to_add.USERID = command.ID;
                                _User_Title_to_add.TABLELIST = "TITLE";
                                _User_Title_to_add.LISTCODE = title;
                                _dbContext.Add(_User_Title_to_add);
                                _dbContext.SaveChanges();
                            }
                        }

                        //update department
                        Project.Models.UserList _User_Department_to_add = new Project.Models.UserList();
                        foreach (var department in command.Department)
                        {
                            _User_Department_to_add.USERID = command.ID;
                            _User_Department_to_add.TABLELIST = "DEPARTMENT";
                            _User_Department_to_add.LISTCODE = department;
                            _dbContext.Add(_User_Department_to_add);
                            _dbContext.SaveChanges();
                        }

                        dbContextTransaction.Commit();
                        return new UpdateUserResponse
                        {
                            MESSAGE = "UPDATE_SUCCESSFUL",
                            STATUSCODE = HttpStatusCode.OK,
                            RESPONSES = _mapper.Map<UserDto>(_User_to_update)
                        };
                    }
                    else
                    {
                        return new UpdateUserResponse
                        {
                            MESSAGE = "UPDATE_FAIL",
                            STATUSCODE = HttpStatusCode.BadRequest
                        };
                    }
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new UpdateUserResponse
                    {
                        MESSAGE = "UPDATE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
