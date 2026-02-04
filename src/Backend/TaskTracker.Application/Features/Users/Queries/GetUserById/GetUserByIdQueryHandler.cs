using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDetailsDto>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetUserByIdQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Result<UserDetailsDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var user = await uow.UserRepository.GetAsync(request.Id);

        uow.Commit();

        return user is null
            ? Result<UserDetailsDto>.NotFound($"User with Id {request.Id} not found.")
            : Result<UserDetailsDto>.Success(user.ToUserDetailsDto());
    }
}
