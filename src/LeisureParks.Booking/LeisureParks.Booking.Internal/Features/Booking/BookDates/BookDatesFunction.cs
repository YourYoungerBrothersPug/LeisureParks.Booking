using LeisureParks.Booking.Models.Features.Booking;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;

namespace LeisureParks.Booking.Internal.Features.Booking.BookDates;

public static class BookDatesFunction
{
    [Function(nameof(BookDatesFunction))]
    public static async Task<BookDatesResult> RunAsync([ActivityTrigger] BookingMessageDto booking, FunctionContext executionContext)
    {
        var service = executionContext.InstanceServices.GetRequiredService<IBookDatesService>();
        return await service.BookDatesAsync(booking, executionContext.CancellationToken);
    }
}