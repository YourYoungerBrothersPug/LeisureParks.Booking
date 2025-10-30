namespace LeisureParks.Booking.Internal.Features.Booking.Payment;

public record BookingPaymentRequest(Guid UserId, decimal Amount);