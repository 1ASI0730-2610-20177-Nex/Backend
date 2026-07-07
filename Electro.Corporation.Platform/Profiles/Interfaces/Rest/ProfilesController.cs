using System.Net.Mime;
using Electro.Corporation.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using Electro.Corporation.Platform.Profiles.Application.CommandServices;
using Electro.Corporation.Platform.Profiles.Application.QueryServices;
using Electro.Corporation.Platform.Profiles.Domain.Model.Commands;
using Electro.Corporation.Platform.Profiles.Domain.Model.Queries;
using Electro.Corporation.Platform.Profiles.Interfaces.Rest.Resources;
using Electro.Corporation.Platform.Profiles.Interfaces.Rest.Transform;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Electro.Corporation.Platform.Profiles.Interfaces.Rest;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Profile Endpoints.")]
public class ProfilesController(
    IProfileCommandService profileCommandService,
    IProfileQueryService profileQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet("{profileId:int}")]
    [SwaggerOperation("Get Profile by Id", OperationId = "GetProfileById")]
    [SwaggerResponse(200, "Profile found", typeof(ProfileResource))]
    [SwaggerResponse(404, "Profile not found")]
    public async Task<IActionResult> GetProfileById(int profileId, CancellationToken cancellationToken)
    {
        var profile = await profileQueryService.Handle(new GetProfileByIdQuery(profileId), cancellationToken);
        return ProfilesActionResultAssembler.ToActionResultFromGetProfileByIdResult(
            this, profile, errorLocalizer, problemDetailsFactory,
            found => Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(found)));
    }

    [HttpGet]
    [SwaggerOperation("Get Profile by UserId", OperationId = "GetProfileByUserId")]
    [SwaggerResponse(200, "Profile found", typeof(ProfileResource))]
    [SwaggerResponse(404, "Profile not found")]
    public async Task<IActionResult> GetProfileByUserId([FromQuery] int userId, CancellationToken cancellationToken)
    {
        var profile = await profileQueryService.Handle(new GetProfileByUserIdQuery(userId), cancellationToken);
        return ProfilesActionResultAssembler.ToActionResultFromGetProfileByIdResult(
            this, profile, errorLocalizer, problemDetailsFactory,
            found => Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(found)));
    }

    [HttpPost]
    [SwaggerOperation("Create Profile", OperationId = "CreateProfile")]
    [SwaggerResponse(201, "Profile created", typeof(ProfileResource))]
    [SwaggerResponse(400, "Invalid data")]
    [SwaggerResponse(409, "Email already registered")]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileResource resource,
        CancellationToken cancellationToken)
    {
        var result = await profileCommandService.Handle(
            CreateProfileCommandFromResourceAssembler.ToCommandFromResource(resource), cancellationToken);
        return ProfilesActionResultAssembler.ToActionResultFromProfileResult(
            this, result, errorLocalizer, problemDetailsFactory,
            created => CreatedAtAction(nameof(GetProfileById), new { profileId = created.Id },
                ProfileResourceFromEntityAssembler.ToResourceFromEntity(created)));
    }

    [HttpPut("{profileId:int}")]
    [SwaggerOperation("Update Profile", OperationId = "UpdateProfile")]
    [SwaggerResponse(200, "Profile updated", typeof(ProfileResource))]
    [SwaggerResponse(400, "Invalid data")]
    [SwaggerResponse(404, "Profile not found")]
    public async Task<IActionResult> UpdateProfile(int profileId, [FromBody] UpdateProfileResource resource,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProfileCommand(profileId, resource.FirstName, resource.LastName, resource.Email,
            resource.Street, resource.Number, resource.City, resource.PostalCode, resource.Country);
        var result = await profileCommandService.Handle(command, cancellationToken);
        return ProfilesActionResultAssembler.ToActionResultFromProfileResult(
            this, result, errorLocalizer, problemDetailsFactory,
            updated => Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }

    [HttpPut("{profileId:int}/preferences")]
    [SwaggerOperation("Update Profile Preferences", OperationId = "UpdateProfilePreferences")]
    [SwaggerResponse(200, "Preferences updated", typeof(ProfileResource))]
    [SwaggerResponse(400, "Invalid data")]
    [SwaggerResponse(404, "Profile not found")]
    public async Task<IActionResult> UpdateProfilePreferences(int profileId,
        [FromBody] UpdateProfilePreferencesResource resource, CancellationToken cancellationToken)
    {
        var command = new UpdateProfilePreferencesCommand(profileId, resource.Language, resource.Theme,
            resource.NotificationsEnabled);
        var result = await profileCommandService.Handle(command, cancellationToken);
        return ProfilesActionResultAssembler.ToActionResultFromProfileResult(
            this, result, errorLocalizer, problemDetailsFactory,
            updated => Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(updated)));
    }
}
