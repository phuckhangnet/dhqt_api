using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
namespace Project.UseCases.Users
{
    public class AddUserResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public UserDto? RESPONSES { get; set; }
        public dynamic? USERDETAIL { get; set; }
        public dynamic? ERROR { get; set; }
    }
    public class AddUserCommand : IRequest<AddUserResponse>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? PhoneSub { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Avatar { get; set; }
        public int? Colindex { get; set; }
        public string? Education { get; set; }
        public string? Office { get; set; }
        public string? Major { get; set; }
        public string? Aboutme { get; set; }
        public string? Research { get; set; }
        public string? Supervision { get; set; }
        public string? Publication { get; set; }
        public string? TeachingCourse { get; set; }
        public string? Moreinfo { get; set; }
        public IEnumerable<string>? Position { get; set; }
        public IEnumerable<string>? Title { get; set; }
        public IEnumerable<string>? Department { get; set; }
        //public string? Aboutme_Eng { get; set; }
        public string? Name_Eng { get; set; }
        public string? Education_Eng { get; set; }
        public string? Office_Eng { get; set; }
        public string? Major_Eng { get; set; }
        public string? Aboutme_Eng { get; set; }
        public string? Research_Eng { get; set; }
        public string? Supervision_Eng { get; set; }
        public string? Publication_Eng { get; set; }
        public string? TeachingCourse_Eng { get; set; }
        public string? Moreinfo_Eng { get; set; }
    }
    public class AddUserValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserValidator()
        {

        }
    }
    public class AddUserHandler : IRequestHandler<AddUserCommand, AddUserResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        //private readonly UserRepository _UserRepo;

        public AddUserHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            //_UserRepo = UserRepo;
        }
        public async Task<AddUserResponse> Handle(AddUserCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    GeneralRepository _generalRepo = new GeneralRepository(_dbContext);
                    bool _username_checked = _dbContext.Users.Any(u => u.USERNAME == command.Username);
                    bool _phone_checked = _dbContext.User_Detail.Any(u => u.PHONE == command.Phone);
                    bool _email_checked = _dbContext.User_Detail.Any(u => u.EMAIL == command.Email);
                    //Check trùng
                    List<string> _existed_prop = new List<string> { _username_checked ? "USER_ALREADY_EXISTS" : string.Empty ,
                                                                    _phone_checked ? "PHONE_NUMBER_USED_BY_ANOTHER_ACCOUNT": string.Empty,
                                                                    _email_checked ? "EMAIL_USED_BY_ANOTHER_ACCOUNT" : string.Empty
                                                                    };
                    _existed_prop.RemoveAll(s => s == string.Empty);
                    if (_existed_prop.Count() > 0)
                    {
                        return new AddUserResponse
                        {
                            MESSAGE = "USER_INFO_ALREADY_EXIST",
                            STATUSCODE = HttpStatusCode.BadRequest,
                            ERROR = _existed_prop
                        };
                    }
                    Project.Models.User _User_to_add = _mapper.Map<Project.Models.User>(command);
                    List<Project.Models.UserDetail> listDetail = new List<Project.Models.UserDetail>();
                    Project.Models.UserDetail _User_Detail_to_add = _mapper.Map<Project.Models.UserDetail>(command);
                    Project.Models.UserDetail _User_Detail_Eng_to_add = _mapper.Map<Project.Models.UserDetail>(command);
                    Project.Models.UserList _User_List_to_add = _mapper.Map<Project.Models.UserList>(command);
                    if (!string.IsNullOrEmpty(command.Password))
                    {
                        _User_to_add.PASSWORDHASH = _generalRepo.HashPassword(command.Password, out var salt);
                        _User_to_add.PASSWORDSALT = Convert.ToHexString(salt);
                    }
                    _User_to_add.STATUS = 1;
                    _User_to_add.CREATEDATE = DateTime.Now;
                    _dbContext.Add(_User_to_add);
                    _dbContext.SaveChanges();
                    _dbContext.Entry(_User_to_add).GetDatabaseValues();

                    //add user detail
                    _User_Detail_to_add.USERID = _User_to_add.ID;
                    _User_Detail_to_add.LANGUAGE = "vn";
                    _dbContext.Add(_User_Detail_to_add);
                    _dbContext.SaveChanges();
                    _dbContext.ChangeTracker.Clear();

                    _User_Detail_Eng_to_add.USERID = _User_to_add.ID;
                    _User_Detail_Eng_to_add.NAME = command.Name_Eng;
                    _User_Detail_Eng_to_add.EDUCATION = command.Education_Eng;
                    _User_Detail_Eng_to_add.OFFICE = command.Office_Eng;
                    _User_Detail_Eng_to_add.MAJOR = command.Major_Eng;
                    _User_Detail_Eng_to_add.ABOUTME = command.Aboutme_Eng;
                    _User_Detail_Eng_to_add.RESEARCH = command.Research_Eng;
                    _User_Detail_Eng_to_add.SUPERVISION = command.Supervision_Eng;
                    _User_Detail_Eng_to_add.PUBLICATION = command.Publication_Eng;
                    _User_Detail_Eng_to_add.TEACHINGCOURSE = command.TeachingCourse_Eng;
                    _User_Detail_Eng_to_add.MOREINFO = command.Moreinfo_Eng;
                    _User_Detail_Eng_to_add.LANGUAGE = "en";
                    _dbContext.Add(_User_Detail_Eng_to_add);
                    _dbContext.SaveChanges();

                    //add user position
                    _User_List_to_add.USERID = _User_to_add.ID;
                    foreach (string position in command.Position)
                    {
                        _User_List_to_add.TABLELIST = "POSITION";
                        _User_List_to_add.LISTCODE = position;
                        _dbContext.Add(_User_List_to_add);
                        _dbContext.SaveChanges();
                    }

                    //add user title
                    foreach (string title in command.Title)
                    {
                        _User_List_to_add.TABLELIST = "TITLE";
                        _User_List_to_add.LISTCODE = title;
                        _dbContext.Add(_User_List_to_add);
                        _dbContext.SaveChanges();
                    }

                    //add user department
                    foreach (string department in command.Department)
                    {
                        _User_List_to_add.TABLELIST = "DEPARTMENT";
                        _User_List_to_add.LISTCODE = department;
                        _dbContext.Add(_User_List_to_add);
                        _dbContext.SaveChanges();
                    }
                    dbContextTransaction.Commit();
                    return new AddUserResponse
                    {
                        MESSAGE = "ADD_SUCCESSFUL",
                        STATUSCODE = HttpStatusCode.OK,
                        RESPONSES = _mapper.Map<UserDto>(_User_to_add),
                        USERDETAIL = _User_Detail_to_add,
                    };
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new AddUserResponse
                    {
                        MESSAGE = "ADD_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
