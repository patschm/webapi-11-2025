using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Hubs;

public class IRCIIHub : Hub
{
    private static readonly HashSet<string> _nicks = new();
    private static readonly Dictionary<string, HashSet<string>> _channels = new()
    {
        { "#ircii", new HashSet<string>() },
        { "#dutch", new HashSet<string>() }
    };

    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("system", "Welcome! Choose a nick with /nick <name>");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        var nick = Context.Items["nick"] as string;
        if (nick != null) await LeaveAllChannels(nick);
        await base.OnDisconnectedAsync(ex);
    }

    // nick MyName
    public async Task SetNick(string newNick)
    {
        var oldNick = Context.Items["nick"] as string;

        if (string.IsNullOrWhiteSpace(newNick) || newNick.Length > 20)
        {
            await Clients.Caller.SendAsync("system-invalid-nick", "Invalid nick.");
            return;
        }

        if (_nicks.Contains(newNick) && newNick != oldNick)
        {
            await Clients.Caller.SendAsync("system-invalid-nick", $"Nick '{newNick}' is already taken.");
            return;
        }

        if (oldNick != null) _nicks.Remove(oldNick);
        _nicks.Add(newNick);
        Context.Items["nick"] = newNick;

        await Clients.Caller.SendAsync("system", $"You are now known as **{newNick}**.");
        await JoinChannel("#ircii", newNick); // auto-join default channel
    }

    // /join #channel
    public async Task JoinChannel(string channel, string? nick = null)
    {
        nick ??= Context.Items["nick"] as string;
        if (nick == null) return;

        if (!_channels.ContainsKey(channel))
            _channels[channel] = new HashSet<string>();

        if (_channels[channel].Add(nick))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, channel);
            await Clients.Group(channel).SendAsync("join", nick, channel);
        }
    }

    // /part #channel
    public async Task PartChannel(string channel, string? nick = null)
    {
        nick ??= Context.Items["nick"] as string;
        if (nick == null) return;

        if (_channels.TryGetValue(channel, out var set) && set.Remove(nick))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, channel);
            await Clients.Group(channel).SendAsync("part", nick, channel);
        }
    }

    private async Task LeaveAllChannels(string nick)
    {
        foreach (var kv in _channels)
        {
            if (kv.Value.Remove(nick))
                await Clients.Group(kv.Key).SendAsync("part", nick, kv.Key);
        }
        _nicks.Remove(nick);
    }

    // Regular message or /me
    public async Task SendMessage(string channel, string text)
    {
        var nick = Context.Items["nick"] as string;
        if (nick == null) return;
        if (!_channels[channel].Contains(nick))
        {
            await Clients.Caller.SendAsync("system", $"You are not in channel **{channel}**. First /join {channel}");
            return;
        }
        if (text.StartsWith("/me "))
        {
            await Clients.Group(channel).SendAsync("action", nick, text["/me ".Length..]);
        }
        else
        {
            await Clients.Group(channel).SendAsync("message", nick, text);
        }
    }
    public async Task GetChannelUsers(string channel)
    {
        if (string.IsNullOrEmpty(channel)) return;

        await Clients.Group(channel).SendAsync("users", _channels[channel].ToArray());
        
    }
    public async Task GetChannels()
    {
        await Clients.Caller.SendAsync("channels", _channels.Select(k=>k.Key).ToArray());

    }
}