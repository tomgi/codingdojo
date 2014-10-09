var http = require('http');
var fs = require('fs');

var loggedUsers = {};

var server = http.createServer(function (req, res) {
	fs.createReadStream('index.html').pipe(res);
});

var io = require('socket.io')(server);

io.on('connection', function(socket){

	console.log('client connected');

	socket.emit('client_id', socket.id);

	socket.on('disconnect', function () {
		console.log('client disconnected');
	});


	socket.on('position', function(position){  	
		loggedUsers[socket.id] = position;

    	io.emit('players_positions', loggedUsers);
	});
});

server.listen(1337);
