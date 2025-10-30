using LeisureParks.Booking.Internal.Features.Booking.Email;
using LeisureParks.Booking.Internal.Features.Booking.Orchestration;
using LeisureParks.Booking.Internal.Features.Booking.Validation;
using LeisureParks.Booking.Models.Features.Booking;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace LeisureParks.Booking.Internal.Test.Features.Booking.Orchestration;

public class BookingOrchestrationFunctionTests
{
    private readonly ILogger _logger = Substitute.For<ILogger>();
    private readonly TaskOrchestrationContext _context = Substitute.For<TaskOrchestrationContext>();

	public BookingOrchestrationFunctionTests()
	{
        _context.CreateReplaySafeLogger(nameof(BookingOrchestrationFunction))
            .Returns(_ => _logger);
	}

	[Fact]
    public async Task ValidationInvalid()
    {
        // ARRANGE
        var userId = Guid.NewGuid();
        var booking = new BookingMessageDto(string.Empty, 0, DateTime.MinValue, DateTime.MinValue, userId);

        _context.GetInput<BookingMessageDto>()
            .Returns(booking);

        _context.CallActivityAsync<BookingValidationResult>(nameof(BookingValidationFunction), booking, Arg.Any<TaskOptions>())
            .Returns(BookingValidationResult.Invalid);

        // ACT
        await BookingOrchestrationFunction.RunOrchestratorAsync(_context);

        // ASSERT
        // Check email was sent
        await _context.Received(1)
            .CallActivityAsync(nameof(BookingEmailFunction), new BookingEmailRequest(userId, "Booking request was invalid."));

        // Check no other activities were triggered
        await _context.Received(1)
            .CallActivityAsync(Arg.Any<TaskName>(), Arg.Any<object?>(), Arg.Any<TaskOptions?>());
    }
}