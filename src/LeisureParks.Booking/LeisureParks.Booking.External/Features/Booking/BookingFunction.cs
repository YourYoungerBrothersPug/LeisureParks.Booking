using LeisureParks.Booking.Models.Features.Booking;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;

namespace LeisureParks.Booking.External.Features.Booking;

public class BookingFunction(ILogger<BookingFunction> logger)
{
    private readonly ILogger<BookingFunction> _logger = logger;

    [Function(nameof(Booking))]
    public BookingResponse Booking([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, [FromBody] BookingRequestDto request)
    {
        Activity.Current?.AddTag("LodgeCode", request.LodgeCode);

        _logger.LogInformation("Received booking for {LodgeCode} at amount {Amount}", request.LodgeCode, request.Amount);

        // Read from auth claim
        var userId = Guid.NewGuid();

        var response = req.CreateResponse(HttpStatusCode.Accepted);

        return new BookingResponse()
        {
            QueueMessage = new BookingMessageDto(request.LodgeCode, request.Amount, request.StartDate, request.EndDate, userId),
            HttpResponse = response
        };
    }
}