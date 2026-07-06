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
[SwaggerTag("Device management endpoints.")]
public class DevicesController(
    IDeviceCommandService deviceCommandService,
    IDeviceQueryService deviceQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
  private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
  private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

  [HttpGet]
  [SwaggerOperation(Summary = "Get all devices", OperationId = "GetAllDevices")]
  [SwaggerResponse(StatusCodes.Status200OK, "List of devices", typeof(IEnumerable<DeviceResource>))]
  public async Task<IActionResult> GetAllDevices(CancellationToken cancellationToken)
  {
    var devices = await deviceQueryService.Handle(new GetAllDevicesQuery(), cancellationToken);
    return Ok(devices.Select(DeviceResourceFromEntityAssembler.ToResourceFromEntity));
  }

  [HttpGet("{id:int}")]
  [SwaggerOperation(Summary = "Get device by id", OperationId = "GetDeviceById")]
  [SwaggerResponse(StatusCodes.Status200OK, "Device found", typeof(DeviceResource))]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Device not found")]
  public async Task<IActionResult> GetDeviceById(int id, CancellationToken cancellationToken)
  {
    var device = await deviceQueryService.Handle(new GetDeviceByIdQuery(id), cancellationToken);
    return DevicesActionResultAssembler.ToActionResultFromGetDeviceByIdResult(
      this,
      device,
      _errorLocalizer,
      _problemDetailsFactory,
      foundDevice => Ok(DeviceResourceFromEntityAssembler.ToResourceFromEntity(foundDevice))
    );
  }

  [HttpPost]
  [SwaggerOperation(Summary = "Create device", OperationId = "CreateDevice")]
  [SwaggerResponse(StatusCodes.Status201Created, "Device created", typeof(DeviceResource))]
  public async Task<IActionResult> CreateDevice([FromBody] CreateDeviceResource resource,
    CancellationToken cancellationToken)
  {
    var command = CreateDeviceCommandFromResourceAssembler.ToCommandFromResource(resource);
    var result = await deviceCommandService.Handle(command, cancellationToken);
    return DevicesActionResultAssembler.ToActionResultFromDeviceResult(
      this,
      result,
      _errorLocalizer,
      _problemDetailsFactory,
      createdDevice => CreatedAtAction(nameof(GetDeviceById), new { id = createdDevice.Id },
        DeviceResourceFromEntityAssembler.ToResourceFromEntity(createdDevice))
    );
  }

  [HttpPut("{id:int}")]
  [SwaggerOperation(Summary = "Update device", OperationId = "UpdateDevice")]
  [SwaggerResponse(StatusCodes.Status200OK, "Device updated", typeof(DeviceResource))]
  public async Task<IActionResult> UpdateDevice(int id, [FromBody] UpdateDeviceResource resource,
    CancellationToken cancellationToken)
  {
    var command = UpdateDeviceCommandFromResourceAssembler.ToCommandFromResource(id, resource);
    var result = await deviceCommandService.Handle(command, cancellationToken);
    return DevicesActionResultAssembler.ToActionResultFromDeviceResult(
      this,
      result,
      _errorLocalizer,
      _problemDetailsFactory,
      updatedDevice => Ok(DeviceResourceFromEntityAssembler.ToResourceFromEntity(updatedDevice))
    );
  }

  [HttpDelete("{id:int}")]
  [SwaggerOperation(Summary = "Delete device", OperationId = "DeleteDevice")]
  [SwaggerResponse(StatusCodes.Status204NoContent, "Device deleted")]
  public async Task<IActionResult> DeleteDevice(int id, CancellationToken cancellationToken)
  {
    var result = await deviceCommandService.Handle(new DeleteDeviceCommand(id), cancellationToken);
    return DevicesActionResultAssembler.ToActionResultFromDeleteResult(
      this,
      result,
      _errorLocalizer,
      _problemDetailsFactory,
      () => NoContent()
    );
  }
}
