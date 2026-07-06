using System.Net.Mime;
using Electro.Corporation.Platform.Devices.Application.CommandServices;
using Electro.Corporation.Platform.Devices.Application.QueryServices;
using Electro.Corporation.Platform.Devices.Domain.Model.Commands;
using Electro.Corporation.Platform.Devices.Domain.Model.Queries;
using Electro.Corporation.Platform.Devices.Interfaces.Rest.Resources;
using Electro.Corporation.Platform.Devices.Interfaces.Rest.Transform;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Electro.Corporation.Platform.Devices.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Home management endpoints.")]
public class HomesController(
    IHomeCommandService homeCommandService,
    IHomeQueryService homeQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
  private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
  private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

  [HttpGet]
  [SwaggerOperation(Summary = "Get all homes", OperationId = "GetAllHomes")]
  [SwaggerResponse(StatusCodes.Status200OK, "List of homes", typeof(IEnumerable<HomeResource>))]
  public async Task<IActionResult> GetAllHomes(CancellationToken cancellationToken)
  {
    var homes = await homeQueryService.Handle(new GetAllHomesQuery(), cancellationToken);
    return Ok(homes.Select(HomeResourceFromEntityAssembler.ToResourceFromEntity));
  }

  [HttpGet("{id:int}")]
  [SwaggerOperation(Summary = "Get home by id", OperationId = "GetHomeById")]
  [SwaggerResponse(StatusCodes.Status200OK, "Home found", typeof(HomeResource))]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Home not found")]
  public async Task<IActionResult> GetHomeById(int id, CancellationToken cancellationToken)
  {
    var home = await homeQueryService.Handle(new GetHomeByIdQuery(id), cancellationToken);
    return DevicesActionResultAssembler.ToActionResultFromGetHomeByIdResult(
      this,
      home,
      _errorLocalizer,
      _problemDetailsFactory,
      foundHome => Ok(HomeResourceFromEntityAssembler.ToResourceFromEntity(foundHome))
    );
  }

  [HttpPost]
  [SwaggerOperation(Summary = "Create home", OperationId = "CreateHome")]
  [SwaggerResponse(StatusCodes.Status201Created, "Home created", typeof(HomeResource))]
  public async Task<IActionResult> CreateHome([FromBody] CreateHomeResource resource,
    CancellationToken cancellationToken)
  {
    var command = CreateHomeCommandFromResourceAssembler.ToCommandFromResource(resource);
    var result = await homeCommandService.Handle(command, cancellationToken);
    return DevicesActionResultAssembler.ToActionResultFromHomeResult(
      this,
      result,
      _errorLocalizer,
      _problemDetailsFactory,
      createdHome => CreatedAtAction(nameof(GetHomeById), new { id = createdHome.Id },
        HomeResourceFromEntityAssembler.ToResourceFromEntity(createdHome))
    );
  }

  [HttpPut("{id:int}")]
  [SwaggerOperation(Summary = "Update home", OperationId = "UpdateHome")]
  [SwaggerResponse(StatusCodes.Status200OK, "Home updated", typeof(HomeResource))]
  public async Task<IActionResult> UpdateHome(int id, [FromBody] UpdateHomeResource resource,
    CancellationToken cancellationToken)
  {
    var command = UpdateHomeCommandFromResourceAssembler.ToCommandFromResource(id, resource);
    var result = await homeCommandService.Handle(command, cancellationToken);
    return DevicesActionResultAssembler.ToActionResultFromHomeResult(
      this,
      result,
      _errorLocalizer,
      _problemDetailsFactory,
      updatedHome => Ok(HomeResourceFromEntityAssembler.ToResourceFromEntity(updatedHome))
    );
  }

  [HttpDelete("{id:int}")]
  [SwaggerOperation(Summary = "Delete home", OperationId = "DeleteHome")]
  [SwaggerResponse(StatusCodes.Status204NoContent, "Home deleted")]
  public async Task<IActionResult> DeleteHome(int id, CancellationToken cancellationToken)
  {
    var result = await homeCommandService.Handle(new DeleteHomeCommand(id), cancellationToken);
    return DevicesActionResultAssembler.ToActionResultFromDeleteResult(
      this,
      result,
      _errorLocalizer,
      _problemDetailsFactory,
      () => NoContent()
    );
  }
}
