using LeisureParks.Booking.Models.Features.Booking;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;

namespace LeisureParks.Booking.Internal.Features.Booking.Validation;

public static class BookingValidationFunction
{
    [Function(nameof(BookingValidationFunction))]
    public static BookingValidationResult Run([ActivityTrigger] BookingMessageDto booking, FunctionContext executionContext)
    {
        var service = executionContext.InstanceServices.GetRequiredService<IBookingValidationService>();
        return service.ValidateBooking(booking);
    }
}