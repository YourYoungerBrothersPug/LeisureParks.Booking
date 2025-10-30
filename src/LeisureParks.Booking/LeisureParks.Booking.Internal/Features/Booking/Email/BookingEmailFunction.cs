using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;

namespace LeisureParks.Booking.Internal.Features.Booking.Email;

public static class BookingEmailFunction
{
    [Function(nameof(BookingEmailFunction))]
    public static async Task RunAsync([ActivityTrigger] BookingEmailRequest request, FunctionContext executionContext)
    {
        var service = executionContext.InstanceServices.GetRequiredService<IBookingEmailService>();
        await service.SendEmailAsync(request.UserId, request.Contents, executionContext.CancellationToken);
    }
}