var express = require('express');
var app = express();
var http = require('http').Server(app);
var io = require('socket.io')(http);
var fs = require('fs');
var path = require('path');

app.use(express.static(path.join(__dirname, 'public')));

var players = {};

io.on('connection', function(client){

    console.log('client connected');

    client.emit('initial_state', {client_id: client.id, players: Object.keys(players)});

    players[client.id] = {x: 0, y: 0, rotation: 0, xspeed: 0, yspeed: 0, decay: 0}

    console.log(JSON.stringify(players));

    client.broadcast.emit('new_player', client.id);

    client.on('disconnect', function () {
        console.log('client disconnected');
        client.broadcast.emit('player_drop', client.id);
        delete players[client.id]
    });

    client.on('move', function(move){   

        var movingPlayer = players[client.id];

        var vx = Math.sin(movingPlayer.rotation * Math.PI / 180) * 0.3;
        var vy = Math.cos(movingPlayer.rotation * Math.PI / 180) * 0.3;

        if(move.left)
            movingPlayer.rotation -= 5;
        if(move.right)
            movingPlayer.rotation += 5;

        if(move.up) {
            movingPlayer.yspeed -= vy;
            movingPlayer.xspeed += vx;
        } else {
            movingPlayer.xspeed *= movingPlayer.decay;
            movingPlayer.yspeed *= movingPlayer.decay;
        }   

        movingPlayer.x += movingPlayer.xspeed;
        movingPlayer.y += movingPlayer.yspeed;

        io.emit('players_positions', players);
    });
});

http.listen(1337);
