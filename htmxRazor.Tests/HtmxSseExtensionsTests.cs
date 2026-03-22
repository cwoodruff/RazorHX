using System.Runtime.CompilerServices;
using System.Text;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace htmxRazor.Tests;

public class HtmxSseExtensionsTests
{
    private static (HttpResponse response, MemoryStream body) CreateResponse()
    {
        var context = new DefaultHttpContext();
        var body = new MemoryStream();
        context.Response.Body = body;
        return (context.Response, body);
    }

    private static string GetBody(MemoryStream body)
    {
        body.Position = 0;
        return Encoding.UTF8.GetString(body.ToArray());
    }

    // ── PrepareSseResponse ──

    [Fact]
    public void PrepareSseResponse_Sets_ContentType()
    {
        var (response, _) = CreateResponse();

        response.PrepareSseResponse();

        Assert.Equal("text/event-stream", response.ContentType);
    }

    [Fact]
    public void PrepareSseResponse_Sets_CacheControl()
    {
        var (response, _) = CreateResponse();

        response.PrepareSseResponse();

        Assert.Equal("no-cache", response.Headers["Cache-Control"].ToString());
    }

    // ── WriteSseEventAsync ──

    [Fact]
    public async Task WriteSseEventAsync_Formats_Correctly()
    {
        var (response, body) = CreateResponse();

        await response.WriteSseEventAsync("hello", "message");

        var result = GetBody(body);
        Assert.Equal("event: message\ndata: hello\n\n", result);
    }

    [Fact]
    public async Task WriteSseEventAsync_With_Custom_Event()
    {
        var (response, body) = CreateResponse();

        await response.WriteSseEventAsync("data here", "status-update");

        var result = GetBody(body);
        Assert.Contains("event: status-update", result);
    }

    [Fact]
    public async Task WriteSseEventAsync_With_Id()
    {
        var (response, body) = CreateResponse();

        await response.WriteSseEventAsync("hello", "message", id: "42");

        var result = GetBody(body);
        Assert.Contains("id: 42", result);
        Assert.Contains("event: message", result);
        Assert.Contains("data: hello", result);
    }

    [Fact]
    public async Task WriteSseEventAsync_Escapes_Newlines()
    {
        var (response, body) = CreateResponse();

        await response.WriteSseEventAsync("line1\nline2\nline3");

        var result = GetBody(body);
        Assert.Contains("data: line1\n", result);
        Assert.Contains("data: line2\n", result);
        Assert.Contains("data: line3\n", result);
    }

    // ── WriteSseStreamAsync ──

    [Fact]
    public async Task WriteSseStreamAsync_Writes_All_Events()
    {
        var (response, body) = CreateResponse();
        var events = ToAsyncEnumerable(["event1", "event2", "event3"]);

        await response.WriteSseStreamAsync(events);

        var result = GetBody(body);
        Assert.Contains("data: event1", result);
        Assert.Contains("data: event2", result);
        Assert.Contains("data: event3", result);
    }

    [Fact]
    public async Task WriteSseStreamAsync_Respects_Cancellation()
    {
        var (response, body) = CreateResponse();
        using var cts = new CancellationTokenSource();

        // The stream produces 2 events then cancels; WriteSseStreamAsync
        // should complete (not hang) and only write the events before cancellation.
        await response.WriteSseStreamAsync(
            CancellingEvents(cts), cancellationToken: cts.Token);

        var result = GetBody(body);
        Assert.Contains("data: event-0", result);
        Assert.Contains("data: event-1", result);
        Assert.DoesNotContain("data: event-2", result);
    }

    private static async IAsyncEnumerable<string> CancellingEvents(
        CancellationTokenSource cts,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var i = 0;
        while (!ct.IsCancellationRequested)
        {
            yield return $"event-{i++}";
            await Task.CompletedTask;
            if (i >= 2)
                cts.Cancel();
        }
    }

    [Fact]
    public async Task WriteSseStreamAsync_Flushes_After_Each()
    {
        // We verify by checking that all events are in the body stream
        // (FlushAsync ensures data is written to the underlying stream)
        var (response, body) = CreateResponse();
        var events = ToAsyncEnumerable(["a", "b"]);

        await response.WriteSseStreamAsync(events);

        var result = GetBody(body);
        Assert.Contains("data: a\n\nevent: message\ndata: b", result);
    }

    // ── Helpers ──

    private static async IAsyncEnumerable<string> ToAsyncEnumerable(string[] items)
    {
        foreach (var item in items)
        {
            yield return item;
            await Task.CompletedTask;
        }
    }
}
