using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using s24422_kolos.Database;
using s24422_kolos.Model;
using s24422_kolos.Service;

namespace s24422_kolos.Controller;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPayment(int idClient, int idSubscription, decimal paymentAmount)
    {
        var paymentId = await _paymentService.AddPaymentAsync(idClient, idSubscription, paymentAmount);
        if (paymentId == null)
        {
            return BadRequest("Payment could not be processed.");
        }

        return Ok(paymentId);
    }
}
