using LeisureParks.Booking.Models.Features.Booking;
using Microsoft.Extensions.Logging;

namespace LeisureParks.Booking.Internal.Features.Booking.Validation;

public interface IBookingValidationService
{
    public BookingValidationResult ValidateBooking(BookingMessageDto booking);
}

public class BookingValidationService(ILogger<BookingValidationService> logger) : IBookingValidationService
{
    private readonly ILogger<BookingValidationService> _logger = logger;

    public BookingValidationResult ValidateBooking(BookingMessageDto booking)
    {
        _logger.LogInformation("Validating booking for {LodgeCode}", booking.LodgeCode);

        if (string.IsNullOrWhiteSpace(booking.LodgeCode))
            return BookingValidationResult.Invalid;

        if (booking.Amount <= 0)
            return BookingValidationResult.Invalid;

        if (booking.StartDate < DateTime.Today || booking.EndDate < DateTime.Today || booking.EndDate < booking.StartDate)
            return BookingValidationResult.Invalid;

        return BookingValidationResult.Valid;
    }
}