using CTeleport.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CTeleport.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}