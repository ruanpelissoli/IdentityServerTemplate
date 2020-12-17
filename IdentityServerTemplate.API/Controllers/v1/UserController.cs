using System;
using System.Threading.Tasks;
using IdentityServerTemplate.Core.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerTemplate.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateNewUserCommand command) =>
            Ok(await _mediator.Send(command));

        [HttpGet]
        [Route("email-confirmation")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] Guid id) =>
            Ok(await _mediator.Send(new ConfirmEmailCommand(id, token)));

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command) =>
            Ok(await _mediator.Send(command));

        [HttpPut]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(
            [FromQuery] string token,
            [FromBody] ResetPasswordCommand command)
        {
            command.Token = token;
            return Ok(await _mediator.Send(command));
        }
    }
}