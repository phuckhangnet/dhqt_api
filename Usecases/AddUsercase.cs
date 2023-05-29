using MediatR;
using AutoMapper;
using Project.Data;
using System.Net;
namespace Project.UseCases
{
    public class CreateResponse<T> 
    { 
        public string? MESSAGE {get;set;}
        public HttpStatusCode STATUSCODE {get;set;}
        public T? RESPONSES {get;set;} 
        public dynamic? ERROR {get;set;}
    }

    public class ICreateCommand<TCommand> : IRequest<bool> where TCommand : class, new() { }


    public class CreateCommandHandler<TEntity, TCommand, T> : IRequestHandler<TCommand,CreateResponse<T>>
        where TEntity : class, new()
        where TCommand :  IRequest<CreateResponse<T>> , new ()//ICreateCommand<TCommand, T>, new()
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public CreateCommandHandler(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CreateResponse<T>> Handle(TCommand request, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try {
                    var entity = mapper.Map<TCommand, TEntity>(request);

                    var entities = await context.Set<TEntity>().AddAsync(entity, cancellationToken);

                    var result = context.SaveChanges();

                    return  new CreateResponse<T>(){
                        MESSAGE = "Thành công",
                        STATUSCODE = HttpStatusCode.OK,
                        RESPONSES = mapper.Map<T>(entity)
                    };
                    //return true;
                }
                catch {
                    return  new CreateResponse<T>(){
                        MESSAGE = "Thành công",
                        STATUSCODE = HttpStatusCode.OK
                    };
                    //return false;
                }
            }
            
        }
    }
}