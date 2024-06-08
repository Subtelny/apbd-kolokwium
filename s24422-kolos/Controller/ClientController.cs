using Microsoft.AspNetCore.Mvc;
using s24422_kolos.Service;

namespace s24422_kolos.Controller;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClientWithSubscriptions(int id)
    {
        var clientData = await _clientService.GetClientWithSubscriptionsAsync(id);
        if (clientData == null)
        {
            return NotFound();
        }
        return Ok(clientData);
    }
}
