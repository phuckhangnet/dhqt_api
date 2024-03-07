using AutoMapper;
using Project.UseCases.Users;
using Project.UseCases.Article;
using Project.UseCases.Blog;
using Project.UseCases.Menu;
using Project.UseCases.Hastag;
using Project.UseCases.Role;
using Project.UseCases.Rule;
using Project.UseCases.ListItem;
using Project.UseCases.ListLanguage;
using Project.Models.Dto;

namespace Project.Models
{
    // ----- User ----
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
    public class AddUserMappingProfile : Profile
    {
        public AddUserMappingProfile()
        {
            CreateMap<AddUserCommand, User>();
            CreateMap<AddUserCommand, UserDetail>();
            CreateMap<AddUserCommand, UserList>();
        }
    }
    public class UserLoginMappingProfile : Profile
    {
        public UserLoginMappingProfile()
        {
            CreateMap<User, UserLoginDto>();
        }
    }
    public class UpdateUserMappingProfile : Profile
    {
        public UpdateUserMappingProfile()
        {
            CreateMap<UpdateUserCommand, User>()
            .ForMember(des => des.ROLE, act => { act.Condition(src => src.Role != null); act.MapFrom(src => src.Role); })
            .ForMember(des => des.AVATAR, act => { act.Condition(src => src.Avatar != null); act.MapFrom(src => src.Avatar); })
            .ForMember(des => des.COLINDEX, act => { act.Condition(src => src.Colindex != null); act.MapFrom(src => src.Colindex); })
            .ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<UpdateUserCommand, UserDetail>()
            .ForMember(des => des.NAME, act => { act.Condition(src => src.Name != null); act.MapFrom(src => src.Name); })
            .ForMember(des => des.EMAIL, act => { act.Condition(src => src.Email != null); act.MapFrom(src => src.Email); })
            .ForMember(des => des.PHONE, act => { act.Condition(src => src.Phone != null); act.MapFrom(src => src.Phone); })
            .ForMember(des => des.MOBILE, act => { act.Condition(src => src.Mobile != null); act.MapFrom(src => src.Mobile); })
            .ForMember(des => des.PHONESUB, act => { act.Condition(src => src.PhoneSub != null); act.MapFrom(src => src.PhoneSub); })
            .ForMember(des => des.WEBSITE, act => { act.Condition(src => src.Website != null); act.MapFrom(src => src.Website); })
            .ForMember(des => des.EDUCATION, act => { act.Condition(src => src.Education != null); act.MapFrom(src => src.Education); })
            .ForMember(des => des.OFFICE, act => { act.Condition(src => src.Office != null); act.MapFrom(src => src.Office); })
            .ForMember(des => des.MAJOR, act => { act.Condition(src => src.Major != null); act.MapFrom(src => src.Major); })
            .ForMember(des => des.RESEARCH, act => { act.Condition(src => src.Research != null); act.MapFrom(src => src.Research); })
            .ForMember(des => des.SUPERVISION, act => { act.Condition(src => src.Supervision != null); act.MapFrom(src => src.Supervision); })
            .ForMember(des => des.PUBLICATION, act => { act.Condition(src => src.Publication != null); act.MapFrom(src => src.Publication); })
            .ForMember(des => des.TEACHINGCOURSE, act => { act.Condition(src => src.TeachingCourse != null); act.MapFrom(src => src.TeachingCourse); })
            .ForMember(des => des.ABOUTME, act => { act.Condition(src => src.Aboutme != null); act.MapFrom(src => src.Aboutme); })
            .ForMember(des => des.MOREINFO, act => { act.Condition(src => src.Moreinfo != null); act.MapFrom(src => src.Moreinfo); })
            .ForAllOtherMembers(opts => opts.Ignore());
        }
    }
    public class UpdateUserStatusMappingProfile : Profile
    {
        public UpdateUserStatusMappingProfile()
        {
            CreateMap<UpdateUserStatusCommand, User>();
        }
    }

    //ARTICLE
    public class ArticleMappingProfile : Profile
    {
        public ArticleMappingProfile()
        {
            CreateMap<Articles, ArticleDto>();
        }
    }
    public class AddArticleMappingProfile : Profile
    {
        public AddArticleMappingProfile()
        {
            CreateMap<AddArticleCommand, Articles>();
        }
    }
    public class AddDraftArticleMappingProfile : Profile
    {
        public AddDraftArticleMappingProfile()
        {
            CreateMap<AddDraftArticleCommand, Articles>();
        }
    }
    public class UpdateArticleMappingProfile : Profile
    {
        public UpdateArticleMappingProfile()
        {
            CreateMap<UpdateArticleCommand, Articles>()
            .ForMember(des => des.AVATAR, act => { act.Condition(src => src.Avatar != null); act.MapFrom(src => src.Avatar); })
            .ForMember(des => des.TITLE, act => { act.Condition(src => src.Title != null); act.MapFrom(src => src.Title); })
            .ForMember(des => des.SUMMARY, act => { act.Condition(src => src.Summary != null); act.MapFrom(src => src.Summary); })
            .ForMember(des => des.HASTAG, act => { act.Condition(src => src.Hastag != null); act.MapFrom(src => src.Hastag); })
            .ForMember(des => des.LANGUAGE, act => { act.Condition(src => src.Language != null); act.MapFrom(src => src.Language); })
            .ForMember(des => des.ARTICLECONTENT, act => { act.Condition(src => src.Article_Content != null); act.MapFrom(src => src.Article_Content); })
            .ForMember(des => des.SLUG, act => { act.Condition(src => src.Slug != null); act.MapFrom(src => src.Slug); })
            .ForMember(des => des.PAGE, act => { act.Condition(src => src.Page != null); act.MapFrom(src => src.Page); })
            .ForAllOtherMembers(opts => opts.Ignore());
        }
    }
    public class UpdateNoMenuArticleMappingProfile : Profile
    {
        public UpdateNoMenuArticleMappingProfile()
        {
            CreateMap<UpdateNoMenuArticleCommand, Articles>()
            .ForMember(des => des.AVATAR, act => { act.Condition(src => src.Avatar != null); act.MapFrom(src => src.Avatar); })
            .ForMember(des => des.TITLE, act => { act.Condition(src => src.Title != null); act.MapFrom(src => src.Title); })
            .ForMember(des => des.SUMMARY, act => { act.Condition(src => src.Summary != null); act.MapFrom(src => src.Summary); })
            .ForMember(des => des.HASTAG, act => { act.Condition(src => src.Hastag != null); act.MapFrom(src => src.Hastag); })
            .ForMember(des => des.LANGUAGE, act => { act.Condition(src => src.Language != null); act.MapFrom(src => src.Language); })
            .ForMember(des => des.ARTICLECONTENT, act => { act.Condition(src => src.Article_Content != null); act.MapFrom(src => src.Article_Content); })
            .ForAllOtherMembers(opts => opts.Ignore());
        }
    }


    //BLOG
    public class BlogMappingProfile : Profile
    {
        public BlogMappingProfile()
        {
            CreateMap<Blogs, BlogDto>();
        }
    }
    public class AddBlogMappingProfile : Profile
    {
        public AddBlogMappingProfile()
        {
            CreateMap<AddBlogCommand, Blogs>();
        }
    }
    public class AddDraftBlogMappingProfile : Profile
    {
        public AddDraftBlogMappingProfile()
        {
            CreateMap<AddDraftBlogCommand, Blogs>();
        }
    }
    public class UpdateBlogMappingProfile : Profile
    {
        public UpdateBlogMappingProfile()
        {
            CreateMap<UpdateBlogCommand, Blogs>()
            .ForMember(des => des.TITLE, act => { act.Condition(src => src.Title != null); act.MapFrom(src => src.Title); })
            .ForMember(des => des.ARTICLECONTENT, act => { act.Condition(src => src.Article_Content != null); act.MapFrom(src => src.Article_Content); })
            .ForMember(des => des.HASTAG, act => { act.Condition(src => src.Hastag != null); act.MapFrom(src => src.Hastag); })
            .ForMember(des => des.LANGUAGE, act => { act.Condition(src => src.Language != null); act.MapFrom(src => src.Language); })
            .ForAllOtherMembers(opts => opts.Ignore());
        }
    }

    //MENU
    public class MenuMappingProfile : Profile
    {
        public MenuMappingProfile()
        {
            CreateMap<Menu, MenuDto>();
        }
    }
    public class AddMenuMappingProfile : Profile
    {
        public AddMenuMappingProfile()
        {
            CreateMap<AddMenuCommand, Menu>();
            CreateMap<AddMenuCommand, Role_Menu>();
        }
    }
    public class UpdateMenuMappingProfile : Profile
    {
        public UpdateMenuMappingProfile()
        {
            CreateMap<UpdateMenuCommand, Menu>()
            .ForMember(des => des.NAME, act => { act.Condition(src => src.Name != null); act.MapFrom(src => src.Name); })
            .ForMember(des => des.DESCRIPTION, act => { act.Condition(src => src.Description != null); act.MapFrom(src => src.Description); })
            .ForMember(des => des.MENULEVEL, act => { act.Condition(src => src.MenuLevel != null); act.MapFrom(src => src.MenuLevel); })
            .ForMember(des => des.PARENT, act => { act.Condition(src => src.Parent != null); act.MapFrom(src => src.Parent); })
            .ForMember(des => des.POSITION, act => { act.Condition(src => src.Parent != null); act.MapFrom(src => src.Position); })
            .ForMember(des => des.VISIBLE, act => { act.Condition(src => src.Visible != null); act.MapFrom(src => src.Visible); })
            .ForMember(des => des.ISACTIVE, act => { act.Condition(src => src.IsActive != null); act.MapFrom(src => src.IsActive); })
            .ForMember(des => des.ISPAGE, act => { act.Condition(src => src.IsPage != null); act.MapFrom(src => src.IsPage); })
            .ForAllOtherMembers(opts => opts.Ignore());
        }
    }

    //HASTAG
    public class HastagMappingProfile : Profile
    {
        public HastagMappingProfile()
        {
            CreateMap<Hastag, HastagDto>();
        }
    }
    public class AddHastagMappingProfile : Profile
    {
        public AddHastagMappingProfile()
        {
            CreateMap<AddHastagCommand, Hastag>();
        }
    }
    public class UpdateHastagMappingProfile : Profile
    {
        public UpdateHastagMappingProfile()
        {
            CreateMap<UpdateHastagCommand, Hastag>()
            .ForMember(des => des.CODE, act => { act.Condition(src => src.Code != null); act.MapFrom(src => src.Code); })
            .ForAllOtherMembers(opts => opts.Ignore());
        }
    }
    //ROLE
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role, RoleDto>();
        }
    }
    public class AddRoleMappingProfile : Profile
    {
        public AddRoleMappingProfile()
        {
            CreateMap<AddRoleCommand, Role>();
        }
    }
    public class UpdateRoleMappingProfile : Profile
    {
        public UpdateRoleMappingProfile()
        {
            CreateMap<UpdateRoleCommand, Role>()
            .ForMember(des => des.CODE, act => { act.Condition(src => src.Code != null); act.MapFrom(src => src.Code); })
            .ForMember(des => des.DESCRIPTION, act => { act.Condition(src => src.Rule_List != null); act.MapFrom(src => src.Rule_List); })
            .ForAllOtherMembers(opts => opts.Ignore());
        }
    }
    //RULE
    public class RuleMappingProfile : Profile
    {
        public RuleMappingProfile()
        {
            CreateMap<Rule, RuleDto>();
        }
    }
    public class AddRuleMappingProfile : Profile
    {
        public AddRuleMappingProfile()
        {
            CreateMap<AddRuleCommand, Rule>();
        }
    }
    public class UpdateRuleMappingProfile : Profile
    {
        public UpdateRuleMappingProfile()
        {
            CreateMap<UpdateRuleCommand, Rule>()
            .ForMember(des => des.CODE, act => { act.Condition(src => src.Code != null); act.MapFrom(src => src.Code); })
            .ForMember(des => des.DESCRIPTION, act => { act.Condition(src => src.Description != null); act.MapFrom(src => src.Description); })
            .ForAllOtherMembers(opts => opts.Ignore());
        }
    }
    public class AddListItemMappingProfile : Profile
    {
        public AddListItemMappingProfile()
        {
            CreateMap<AddListItemCommand, ListPosition>();
            CreateMap<AddListItemCommand, ListTitle>();
            CreateMap<AddListItemCommand, ListDepartment>();
        }
    }
    public class UpdateListItemMappingProfile : Profile
    {
        public UpdateListItemMappingProfile()
        {
            CreateMap<UpdateListItemCommand, ListPosition>()
            .ForMember(des => des.DESCRIPTION, act => { act.Condition(src => src.Description != null); act.MapFrom(src => src.Description); })
            .ForMember(des => des.LEVEL, act => { act.Condition(src => src.Level != null); act.MapFrom(src => src.Level); })
            .ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<UpdateListItemCommand, ListTitle>()
            .ForMember(des => des.DESCRIPTION, act => { act.Condition(src => src.Description != null); act.MapFrom(src => src.Description); })
            .ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<UpdateListItemCommand, ListDepartment>()
            .ForMember(des => des.DESCRIPTION, act => { act.Condition(src => src.Description != null); act.MapFrom(src => src.Description); })
            .ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<UpdateListLanguageCommand, ListLanguage>()
            .ForMember(des => des.TEXT, act => { act.Condition(src => src.Text != null); act.MapFrom(src => src.Text); })
            .ForAllOtherMembers(opts => opts.Ignore());
        }
    }

}