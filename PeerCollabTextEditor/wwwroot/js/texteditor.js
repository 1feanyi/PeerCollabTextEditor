// Define editor's textarea
var editor = document.getElementById("editor");
editor.style.display = "none";
var group = document.getElementById("group");

// Create new connection using HubConnectionBuilder
// use full URL if used on sub of site or special cases
const connection = new signalR.HubConnectionBuilder().withUrl("/texthub").build();
connection.start().catch(err => console.error(err));

// Handle manages when the connection receives "ReceiveText" which is the call
// made on the server side when someone wants tot broadcasttext to all clients
connection.on("ReceiveText", (text) => {
    editor.value = text;
    editor.focus();
    editor.setSelectionRange(editor.value.length, editor.value.length);
});

function change() {
    connection.invoke("BroadcastText", editor.value).catch(err => console.error(err));
}

function join() {
    connection.invoke("JoinGroup", group.value).catch(err => console.error(err));
    editor.style.display = "initial";
}