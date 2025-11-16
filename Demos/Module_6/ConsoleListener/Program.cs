using Microsoft.AspNetCore.SignalR.Client;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

AnsiConsole.MarkupLine("[bold green]IRCII Console Client (SignalR)[/] – Spectre 0.54.0");
AnsiConsole.MarkupLine("[dim]Connecting…[/]");

// ---------------------------------------------------------------
// 1. SignalR connection
// ---------------------------------------------------------------
var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7147/hub")   // change if your server runs elsewhere
    .WithAutomaticReconnect()
    .Build();

// ---------------------------------------------------------------
// 2. State (thread-safe)
// ---------------------------------------------------------------
string? myNick = null;
string currentChannel = "#ircii";

var usersInChannel = new ConcurrentDictionary<string, ConcurrentBag<string>>(StringComparer.OrdinalIgnoreCase);
usersInChannel[currentChannel] = new ConcurrentBag<string>();

var messages = new ConcurrentQueue<string>();               // markup lines
var refreshQueue = new ConcurrentQueue<Action<LiveDisplayContext>>();

// ---------------------------------------------------------------
// 3. SignalR handlers – enqueue UI updates
// ---------------------------------------------------------------
void Enqueue(Action<LiveDisplayContext> a) => refreshQueue.Enqueue(a);

connection.On<string>("system", msg =>
{
    Enqueue(ctx =>
    {
        messages.Enqueue($"[grey]{TS()}[/] [dim]*** {Markup.Escape(msg)}[/]");
        Trim(50);
    });
});

connection.On<string, string>("join", (nick, chan) =>
{
    if (!chan.Equals(currentChannel, StringComparison.OrdinalIgnoreCase)) return;

    Enqueue(ctx =>
    {
        messages.Enqueue($"[green]{TS()}[/] [green]*** {nick} has joined {chan}[/]");
        usersInChannel[currentChannel].Add(nick);
        Trim(50);
    });
});

connection.On<string, string>("part", (nick, chan) =>
{
    if (!chan.Equals(currentChannel, StringComparison.OrdinalIgnoreCase)) return;

    Enqueue(ctx =>
    {
        messages.Enqueue($"[orange3]{TS()}[/] [orange3]*** {nick} has left {chan}[/]");
        // remove from bag
        var bag = usersInChannel[currentChannel];
        var newBag = new ConcurrentBag<string>(bag.Where(u => !u.Equals(nick, StringComparison.OrdinalIgnoreCase)));
        usersInChannel[currentChannel] = newBag;
        Trim(50);
    });
});

connection.On<string, string>("message", (nick, text) =>
{
    if (!currentChannel.Equals("#ircii", StringComparison.OrdinalIgnoreCase)) return;

    var col = nick.Equals(myNick, StringComparison.OrdinalIgnoreCase) ? "aqua" : "white";
    Enqueue(ctx =>
    {
        messages.Enqueue($"[grey]{TS()}[/] [bold {col}]<{nick}>[/] {Markup.Escape(text)}");
        Trim(50);
    });
});

connection.On<string, string>("action", (nick, text) =>
{
    Enqueue(ctx =>
    {
        messages.Enqueue($"[magenta]{TS()}[/] * [italic yellow]{nick}[/] {Markup.Escape(text)}");
        Trim(50);
    });
});

// Detect nick change from the server
connection.On<string>("system", msg =>
{
    var m = Regex.Match(msg, @"now known as \*\*(.+?)\*\*");
    if (m.Success)
    {
        myNick = m.Groups[1].Value;
        Enqueue(_ => { });
    }
});

// ---------------------------------------------------------------
// 4. Start connection
// ---------------------------------------------------------------
await connection.StartAsync();
AnsiConsole.MarkupLine("[bold green]Connected![/] Type /nick <name> to start.");

// ---------------------------------------------------------------
// 5. Background raw input (non-blocking)
// ---------------------------------------------------------------
_ = Task.Run(async () =>
{
    var line = string.Empty;
    while (true)
    {
        var key = Console.ReadKey(intercept: true);
        if (key.Key == ConsoleKey.Enter)
        {
            if (!string.IsNullOrWhiteSpace(line))
                _ = ProcessCommand(line.Trim());
            line = string.Empty;
            Console.WriteLine();               // new line after send
        }
        else if (key.Key == ConsoleKey.Backspace && line.Length > 0)
        {
            line = line[..^1];
            Console.Write("\b \b");
        }
        else if (!char.IsControl(key.KeyChar))
        {
            line += key.KeyChar;
            Console.Write(key.KeyChar);
        }
    }
});

// ---------------------------------------------------------------
// 6. Live UI – rebuild table on every loop
// ---------------------------------------------------------------
await AnsiConsole.Live(BuildTable())
    .StartAsync(async ctx =>
    {
        while (true)
        {
            // process queued updates
            while (refreshQueue.TryDequeue(out var a)) a(ctx);

            // rebuild the whole UI (fast for <100 lines)
            ctx.UpdateTarget(BuildTable());

            await Task.Delay(100);   // ~10 fps – responsive enough
        }
    });

// ---------------------------------------------------------------
// 7. Helper methods
// ---------------------------------------------------------------
static string TS() => DateTime.Now.ToString("HH:mm");

void Trim(int max)
{
    while (messages.Count > max)
        messages.TryDequeue(out _);
}

IRenderable BuildTable()
{
    var table = new Table()
        .RoundedBorder()
        .BorderColor(Color.Grey)
        .AddColumn(new TableColumn("Channel").Centered())
        .AddColumn(new TableColumn("Users"))
        .AddColumn(new TableColumn("Messages"));
    //.AddColumn(new TableColumn("Messages").Expand());

    // ---- Channel header ----
    var chanMarkup = new Markup($"[bold cyan]#{currentChannel}[/]");

    // ---- Users panel ----
    var userLines = usersInChannel.GetValueOrDefault(currentChannel, new())
        .OrderBy(u => u, StringComparer.OrdinalIgnoreCase)
        .Select(u => u.StartsWith("@") ? $"[red]{u}[/]" :
                     u.StartsWith("+") ? $"[green]{u}[/]" :
                     $"[white]{u}[/]");

    var usersBody = userLines.Any()
        ? string.Join("\n", userLines)
        : "[grey]No users[/]";

    var usersPanel = new Panel(new Markup(usersBody))
        .Header($"[grey]Users in {currentChannel}[/]")
        .Collapse();

    // ---- Messages panel ----
    var msgBody = messages.Any()
        ? string.Join("\n", messages)
        : "[grey]No messages yet.[/]";

    var msgPanel = new Panel(new Markup(msgBody))
        .Header("[grey]Messages[/]")
        .Expand();

    table.AddRow(chanMarkup, usersPanel, msgPanel);
    return table;
}

// ---------------------------------------------------------------
// 8. Command processing
// ---------------------------------------------------------------
async Task ProcessCommand(string cmd)
{
    if (cmd.StartsWith("/nick "))
    {
        var nick = cmd["/nick ".Length..].Trim();
        await connection.InvokeAsync("SetNick", nick);
        return;
    }

    if (cmd.StartsWith("/join "))
    {
        var chan = cmd["/join ".Length..].Trim();
        await connection.InvokeAsync("JoinChannel", chan, myNick);
        currentChannel = chan;
        if (!usersInChannel.ContainsKey(chan))
            usersInChannel[chan] = new ConcurrentBag<string>();
        Enqueue(_ => { });
        AnsiConsole.MarkupLine($"[yellow]Joined {chan}[/]");
        return;
    }

    if (cmd == "/part")
    {
        await connection.InvokeAsync("PartChannel", currentChannel);
        currentChannel = "#ircii";
        Enqueue(_ => { });
        return;
    }

    if (cmd.StartsWith("/me "))
    {
        if (myNick == null) { AnsiConsole.MarkupLine("[red]Set nick first![/]"); return; }
        await connection.InvokeAsync("SendMessage", currentChannel, cmd);
        return;
    }

    if (cmd == "/help")
    {
        AnsiConsole.Write(
            new Panel(
                """
                [bold]/nick <name>[/] – change nickname
                [bold]/join #chan[/] – join channel
                [bold]/part[/] – leave current channel
                [bold]/me <action>[/] – send action
                [bold]/help[/] – this help
                """)
                .Header("[bold]Commands[/]")
                .BorderColor(Color.Yellow));
        return;
    }

    if (cmd.StartsWith("/"))
    {
        AnsiConsole.MarkupLine("[red]Unknown command – /help[/]");
        return;
    }

    if (myNick == null)
    {
        AnsiConsole.MarkupLine("[red]Use /nick <name> first![/]");
        return;
    }

    await connection.InvokeAsync("SendMessage", cmd);
}

// ---------------------------------------------------------------
// 9. Graceful shutdown
// ---------------------------------------------------------------
Console.CancelKeyPress += async (_, e) =>
{
    e.Cancel = true;
    await connection.DisposeAsync();
    Environment.Exit(0);
};

await Task.Delay(Timeout.Infinite);