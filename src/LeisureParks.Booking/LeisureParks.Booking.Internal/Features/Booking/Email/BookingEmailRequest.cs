namespace LeisureParks.Booking.Internal.Features.Booking.Email;

public record BookingEmailRequest(Guid UserId, string Contents);