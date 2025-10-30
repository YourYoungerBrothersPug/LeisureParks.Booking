namespace LeisureParks.Booking.Models.Features.Booking;

public record BookingMessageDto(string LodgeCode, decimal Amount, DateTime StartDate, DateTime EndDate, Guid UserId);