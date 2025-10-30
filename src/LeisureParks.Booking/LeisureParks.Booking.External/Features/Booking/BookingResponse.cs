using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace LeisureParks.Booking.External.Features.Booking;

public class BookingResponse
{
    [ServiceBusOutput("bookings", Connection = "ServiceBusConnection")]
    public required object QueueMessage { get; set; }

    [HttpResult]
    public required HttpResponseData HttpResponse { get; set; }
}