namespace LeisureParks.Booking.External.Features.Booking;

public record BookingRequestDto(string LodgeCode, decimal Amount, DateTime StartDate, DateTime EndDate);