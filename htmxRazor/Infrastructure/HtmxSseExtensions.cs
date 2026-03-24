using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace htmxRazor.Infrastructure;

/// <summary>
/// Extension methods for <see cref="HttpResponse"/> to produce
/// Server-Sent Events (SSE) compatible with the htmx SSE extension.
/// </summary>
public static class HtmxSseExtensions
{
    /// <summary>
    /// Configures the response headers for an SSE stream.
    /// Sets Content-Type to <c>text/event-stream</c>, disables caching,
    /// and requests keep-alive.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    public static void PrepareSseResponse(this HttpResponse response)
    {
        response.ContentType = "text/event-stream";
        response.Headers["Cache-Control"] = "no-cache";
        response.Headers["Connection"] = "keep-alive";
    }

    /// <summary>
    /// Writes a single SSE event to the response body.
    /// Format: <c>event: {eventName}\ndata: {data}\n\n</c>
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="data">The event data. Multi-line data is split into multiple <c>data:</c> lines.</param>
    /// <param name="eventName">The SSE event name. Default: "message".</param>
    /// <param name="id">Optional event ID for client reconnection tracking.</param>
    public static async Task WriteSseEventAsync(
        this HttpResponse response,
        string data,
        string eventName = "message",
        string? id = null)
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(id))
            sb.Append("id: ").Append(id).Append('\n');

        sb.Append("event: ").Append(eventName).Append('\n');

        // SSE spec: multi-line data requires one "data:" line per line
        var lines = data.Split('\n');
        foreach (var line in lines)
            sb.Append("data: ").Append(line).Append('\n');

        sb.Append('\n'); // blank line terminates the event

        await response.WriteAsync(sb.ToString());
        await response.Body.FlushAsync();
    }

    /// <summary>
    /// Streams an <see cref="IAsyncEnumerable{T}"/> of strings as SSE events,
    /// flushing after each event. Completes when the enumerable is exhausted
    /// or the <paramref name="cancellationToken"/> is triggered.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="events">An async sequence of event data strings.</param>
    /// <param name="eventName">The SSE event name for all events. Default: "message".</param>
    /// <param name="cancellationToken">Token to cancel the stream.</param>
    public static async Task WriteSseStreamAsync(
        this HttpResponse response,
        IAsyncEnumerable<string> events,
        string eventName = "message",
        CancellationToken cancellationToken = default)
    {
        response.PrepareSseResponse();

        await foreach (var data in events.WithCancellation(cancellationToken))
        {
            await response.WriteSseEventAsync(data, eventName);
        }
    }
}
