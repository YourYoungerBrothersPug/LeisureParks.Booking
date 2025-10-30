using LeisureParks.Booking.Internal.Features.Booking.BookDates;
using LeisureParks.Booking.Internal.Features.Booking.Email;
using LeisureParks.Booking.Internal.Features.Booking.Payment;
using LeisureParks.Booking.Internal.Features.Booking.Validation;
using LeisureParks.Booking.Models.Features.Booking;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace LeisureParks.Booking.Internal.Features.Booking.Orchestration;

public static class BookingOrchestrationFunction
{
    [Function(nameof(BookingOrchestrationFunction))]
    public static async Task RunOrchestratorAsync([OrchestrationTrigger] TaskOrchestrationContext context)
    {
        ILogger logger = context.CreateReplaySafeLogger(nameof(BookingOrchestrationFunction));

        var startTime = context.CurrentUtcDateTime;

        try
        {
            var booking = context.GetInput<BookingMessageDto>() ?? throw new NullReferenceException("Booking should not be null");

			var retryOptions = TaskOptions.FromRetryPolicy(new RetryPolicy(3, TimeSpan.FromSeconds(2), 3));

            logger.LogInformation("Calling booking validation");
            var validationResult = await context.CallActivityAsync<BookingValidationResult>(nameof(BookingValidationFunction), booking, retryOptions);
            if (validationResult is BookingValidationResult.Invalid)
            {
                await context.CallActivityAsync(nameof(BookingEmailFunction), new BookingEmailRequest(booking.UserId, "Booking request was invalid."));
                return;
            }

            var bookDatesResult = await context.CallActivityAsync<BookDatesResult>(nameof(BookDatesFunction), booking, retryOptions);
            if (bookDatesResult is BookDatesResult.Duplicate)
            {
                await context.CallActivityAsync(nameof(BookingEmailFunction), new BookingEmailRequest(booking.UserId, "Another booking has already been reserved."));
                return;
            }

            await context.CallActivityAsync<BookingPaymentResult>(nameof(BookingPaymentFunction), new BookingPaymentRequest(booking.UserId, booking.Amount), retryOptions);

            await context.CallActivityAsync(nameof(BookingEmailFunction), new BookingEmailRequest(booking.UserId, $"You are booked in to {booking.LodgeCode}! £{booking.Amount} has been taken from your account."), retryOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during booking orchestration");
            throw;
        }
        finally
        {
            // Can't use stopwatch in Durable Functions
            logger.LogInformation("Orchestration took {Elapsed} to run", context.CurrentUtcDateTime - startTime);
        }
    }
}