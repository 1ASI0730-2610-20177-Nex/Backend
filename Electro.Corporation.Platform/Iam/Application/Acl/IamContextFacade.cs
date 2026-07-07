using Electro.Corporation.Platform.Iam.Application.CommandServices;
using Electro.Corporation.Platform.Iam.Application.QueryServices;
using Electro.Corporation.Platform.Iam.Domain.Model.Commands;
using Electro.Corporation.Platform.Iam.Domain.Model.Queries;
using Electro.Corporation.Platform.Iam.Interfaces.Acl;

namespace Electro.Corporation.Platform.Iam.Application.Acl;

public class IamContextFacade(IUserCommandService userCommandService, IUserQueryService userQueryService)
    : IIamContextFacade
{
    public async Task<int> CreateUser(string username, string password, CancellationToken cancellationToken)
    {
        var signUpCommand = new SignUpCommand(username, password);
        var signUpResult = await userCommandService.Handle(signUpCommand, cancellationToken);
        if (signUpResult.IsFailure) return 0;
        var getUserByUsernameQuery = new GetUserByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery, cancellationToken);
        return result?.Id ?? 0;
    }

    public async Task<int> FetchUserIdByUsername(string username, CancellationToken cancellationToken)
    {
        var getUserByUsernameQuery = new GetUserByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery, cancellationToken);
        return result?.Id ?? 0;
    }

    public async Task<string> FetchUsernameByUserId(int userId, CancellationToken cancellationToken)
    {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var result = await userQueryService.Handle(getUserByIdQuery, cancellationToken);
        return result?.Username ?? string.Empty;
    }
}
