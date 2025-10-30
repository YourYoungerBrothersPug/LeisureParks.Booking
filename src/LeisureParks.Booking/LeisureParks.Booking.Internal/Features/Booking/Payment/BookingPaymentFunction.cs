using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;

namespace LeisureParks.Booking.Internal.Features.Booking.Payment;

public static class BookingPaymentFunction
{
    [Function(nameof(BookingPaymentFunction))]
    public static async Task<BookingPaymentResult> RunAsync([ActivityTrigger] BookingPaymentRequest request, FunctionContext executionContext)
    {
        var service = executionContext.InstanceServices.GetRequiredService<IBookingPaymentService>();
        return await service.TakePaymentAsync(request.UserId, request.Amount, executionContext.CancellationToken);
    }
}