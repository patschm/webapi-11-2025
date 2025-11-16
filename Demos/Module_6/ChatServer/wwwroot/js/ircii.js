/* ---------- SignalR Connection ---------- */
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub")
    .withAutomaticReconnect()
    .build();

let myNick = null;
let currentChannel = "#ircii";
let users = [];
let channels = [];


connection.on("system", msg => addLine('msg-system', `*** ${msg}`));
connection.on("system-invalid-nick", msg => {
    addLine('msg-system', `*** ${msg}`);
    myNick = null;
});
connection.on("join", (nick, chan) => {
    if (chan !== currentChannel) return;
    addLine('msg-join', `*** ${nick} has joined ${chan}`);
    getChannelUsers();
});
connection.on("part", (nick, chan) => {
    if (chan !== currentChannel) return;
    addLine('msg-part', `*** ${nick} has left ${chan}`);
    getChannelUsers();
});
connection.on("message", (nick, text) => {
    const color = nick === myNick ? '#0ff' : '#fff';
    addLine('', `<span class="msg-nick" style="color:${color}">&lt;${nick}&gt;</span><span style="color:yellow">${text}</span>`);
});
connection.on("action", (nick, text) => {
    addLine('msg-action', `* ${nick} ${text}`);
});
connection.on("users", (chanusers) => {
    users = chanusers;
    refreshUserList();
});
connection.on("channels", (allchannels) => {
    channels = allchannels;
    refreshChannelList();
});

connection.start()
    .then(() => {
        setStatus(`Connected – /nick <yourname>`);
        getChannels();
    })
    .catch(err => setStatus(`Connection error: ${err}`));


/* ---------- Helper Functions ---------- */
const $ = s => document.querySelector(s);
const $$ = s => document.querySelectorAll(s);
const output = $('#chat-output');
const channelTabs = $('#channel-tabs');
const userList = $('#user-list');
const input = $('#message-input');
const status = $('#server-info');

function ts() {
    return new Date().toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' });
}
function addLine(cls, html) {
    const div = document.createElement('div');
    div.className = `msg-line ${cls}`;
    div.innerHTML = `<span class="msg-timestamp">[${ts()}]</span> ${html}`;
    output.appendChild(div);
    output.scrollTop = output.scrollHeight;
}
function setStatus(txt) { status.textContent = txt; }

/* ---------- Clock ---------- */
function updateClock() {
    $('#clock').textContent = new Date().toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit', hour12: true });
}
setInterval(updateClock, 1000);
updateClock();


/* ---------- Input Handling ---------- */
$('#input-form').addEventListener('submit', e => {
    e.preventDefault();
    const raw = input.value.trim();
    if (!raw) return;

    if (raw.startsWith('/nick ')) {
        myNick = raw.slice(6).trim();
        connection.invoke("SetNick", myNick).catch(console.error);
    }
    else if (raw.startsWith('/join ')) {
        const chan = raw.slice(6).trim();
        connection.invoke("JoinChannel", chan, myNick).catch(console.error);
        switchChannel(chan);
    }
    else if (raw.startsWith('/part')) {
        connection.invoke("PartChannel", currentChannel).catch(console.error);
    }
    else if (myNick) {
        connection.invoke("SendMessage", currentChannel, raw).catch(console.error);
    }
    else {
        addLine('msg-system', '*** Choose a nick first: /nick MyName');
    }

    input.value = '';
    input.style.height = 'auto';
});

/* ---------- Channel Switching ---------- */
function switchChannel(chan) {
    currentChannel = chan;
    $$('.channel-tab').forEach(t => t.classList.remove('active'));
    const tab = $(`.channel-tab[data-channel="${chan}"]`);
    if (tab) tab.classList.add('active');
    output.innerHTML = '';
    setStatus(`#${chan} | Nick: ${myNick || 'none'}`);
    getChannelUsers();
}

/* ---------- Dynamic Tabs & Users ---------- */
function addChannelTab(chan) {
    if ($(`.channel-tab[data-channel="${chan}"]`)) return;
    const div = document.createElement('div');
    div.className = 'channel-tab';
    div.dataset.channel = chan;
    div.innerHTML = `<i class="bi"></i> ${chan}`;
    div.onclick = () => switchChannel(chan);
    channelTabs.appendChild(div);
}

async function refreshUserList() {
    userList.innerHTML = '';
    users.forEach(val => {
        const div = document.createElement('div');
        div.className = 'user-item user-normal';
        div.innerHTML = `${val}`;
        userList.appendChild(div);
    });
}

async function refreshChannelList() {
    channelTabs.innerHTML = "";
    channels.forEach(val => {
        addChannelTab(val);
    });
}

/* ---------- Auto-resize textarea ---------- */
input.addEventListener('input', function () {
    this.style.height = 'auto';
    this.style.height = (this.scrollHeight) + 'px';
});

async function getChannelUsers()
{
    connection.invoke("GetChannelUsers", currentChannel);
}

async function getChannels() {
    connection.invoke("GetChannels");
}