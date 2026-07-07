using System.Net.Mime;
using Electro.Corporation.Platform.Devices.Application.CommandServices;
using Electro.Corporation.Platform.Devices.Application.QueryServices;
using Electro.Corporation.Platform.Devices.Domain.Model;
using Electro.Corporation.Platform.Devices.Domain.Model.Queries;
using Electro.Corporation.Platform.Devices.Domain.Repositories;
using Electro.Corporation.Platform.Devices.Interfaces.Rest.Resources;
using Electro.Corporation.Platform.Devices.Interfaces.Rest.Transform;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Electro.Corporation.Platform.Devices.Interfaces.Rest;

[ApiController]
[Route("api/v1/properties")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Property management endpoints.")]
public class PropertiesController(
    IPropertyCommandService propertyCommandService,
    IPropertyQueryService propertyQueryService,
    ISpaceCommandService spaceCommandService,
    ISpaceRepository spaceRepository,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpGet]
    [SwaggerOperation(Summary = "List properties by user", OperationId = "GetPropertiesByUserId")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of properties", typeof(IEnumerable<PropertyResource>))]
    public async Task<IActionResult> GetPropertiesByUserId([FromQuery] int userId,
        CancellationToken cancellationToken)
    {
        var properties = await propertyQueryService.Handle(new GetPropertiesByUserIdQuery(userId), cancellationToken);
        var resources = new List<PropertyResource>();
        foreach (var property in properties)
        {
            var spaces = await spaceRepository.FindByPropertyIdAsync(property.Id, cancellationToken);
            var defaultSpaceId = spaces.FirstOrDefault()?.Id ?? 0;
            resources.Add(PropertyResourceFromEntityAssembler.ToResourceFromEntity(property, defaultSpaceId));
        }
        return Ok(resources);
    }

    [HttpGet("{propertyId:int}")]
    [SwaggerOperation(Summary = "Get property by id", OperationId = "GetPropertyById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Property found", typeof(PropertyResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Property not found")]
    public async Task<IActionResult> GetPropertyById(int propertyId, CancellationToken cancellationToken)
    {
        var property = await propertyQueryService.Handle(new GetPropertyByIdQuery(propertyId), cancellationToken);
        if (property == null)
        {
            return _problemDetailsFactory.CreateProblemDetails(this, StatusCodes.Status404NotFound,
                DevicesError.PropertyNotFound, _errorLocalizer[nameof(DevicesError.PropertyNotFound)]);
        }
        var spaces = await spaceRepository.FindByPropertyIdAsync(property.Id, cancellationToken);
        var defaultSpaceId = spaces.FirstOrDefault()?.Id ?? 0;
        return Ok(PropertyResourceFromEntityAssembler.ToResourceFromEntity(property, defaultSpaceId));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create property", OperationId = "CreateProperty")]
    [SwaggerResponse(StatusCodes.Status201Created, "Property created", typeof(PropertyResource))]
    public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreatePropertyCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await propertyCommandService.Handle(command, cancellationToken);
        if (!result.IsSuccess)
        {
            int statusCode = result.Error switch
            {
                DevicesError.OperationCancelled => StatusCodes.Status409Conflict,
                DevicesError.DatabaseError => StatusCodes.Status500InternalServerError,
                DevicesError.InternalServerError => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status400BadRequest
            };
            return _problemDetailsFactory.CreateProblemDetails(this, statusCode, result.Error, result.Message);
        }
        var createdProperty = result.Value!;
        var spaces = await spaceRepository.FindByPropertyIdAsync(createdProperty.Id, cancellationToken);
        var defaultSpaceId = spaces.FirstOrDefault()?.Id ?? 0;
        return CreatedAtAction(nameof(GetPropertyById), new { propertyId = createdProperty.Id },
            PropertyResourceFromEntityAssembler.ToResourceFromEntity(createdProperty, defaultSpaceId));
    }

    [HttpPost("{propertyId:int}/spaces")]
    [SwaggerOperation(Summary = "Create space in property", OperationId = "CreateSpace")]
    [SwaggerResponse(StatusCodes.Status201Created, "Space created", typeof(SpaceResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Property not found")]
    public async Task<IActionResult> CreateSpace(int propertyId, [FromBody] CreateSpaceResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateSpaceCommandFromResourceAssembler.ToCommandFromResource(propertyId, resource);
        var result = await spaceCommandService.Handle(command, cancellationToken);
        return DevicesActionResultAssembler.ToActionResultFromSpaceResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdSpace => CreatedAtAction(nameof(GetPropertyById), new { propertyId },
                SpaceResourceFromEntityAssembler.ToResourceFromEntity(createdSpace))
        );
    }
}
