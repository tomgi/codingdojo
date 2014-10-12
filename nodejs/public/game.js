$(document).ready(function() {

  var socket = io();

  socket.on('initial_state', function (initial_state) {
    var client_id = initial_state.client_id;

    console.log("initial state" + JSON.stringify(initial_state));

    Crafty.init();
    Crafty.canvas();

    Crafty.load(["http://craftyjs.com/demos/asteroids/images/sprite.png", "http://craftyjs.com/demos/asteroids/images/bg.png"], function() {
      Crafty.sprite(64, "http://craftyjs.com/demos/asteroids/images/sprite.png", {
        ship: [0,0],
        big: [1,0],
        medium: [2,0],
        small: [3,0]
      });

      Crafty.scene("main");
    });

    Crafty.scene("main", function() {
      Crafty.background("url('http://craftyjs.com/demos/asteroids/images/bg.png')");

      var players = {};

      initial_state.players.forEach(function (id) { 
          players[id] = Crafty.e("2D, Canvas, ship, Collision")
        .attr({x: Crafty.viewport.width / 2, y: Crafty.viewport.height / 2})
        .origin("center")
        });

      players[client_id] = Crafty.e("2D, Canvas, ship, Controls, Collision")
      .attr({move: {left: false, right: false, up: false, down: false}, x: Crafty.viewport.width / 2, y: Crafty.viewport.height / 2})
      .origin("center")
      .bind("keydown", function(e) {
        if(e.keyCode === Crafty.keys.RIGHT_ARROW) {
          this.move.right = true;
        } else if(e.keyCode === Crafty.keys.LEFT_ARROW) {
          this.move.left = true;
        } else if(e.keyCode === Crafty.keys.UP_ARROW) {
          this.move.up = true;
        } 
      }).bind("keyup", function(e) {
        if(e.keyCode === Crafty.keys.RIGHT_ARROW) {
          this.move.right = false;
        } else if(e.keyCode === Crafty.keys.LEFT_ARROW) {
          this.move.left = false;
        } else if(e.keyCode === Crafty.keys.UP_ARROW) {
          this.move.up = false;
        }
      }).bind("enterframe", function() {
        socket.emit('move', {left: this.move.left, right: this.move.right, up: this.move.up});
      });


      socket.on('new_player', function (new_player_id) {
        console.log('new player connected');
        players[new_player_id] = Crafty.e("2D, Canvas, ship, Collision")
        .attr({x: Crafty.viewport.width / 2, y: Crafty.viewport.height / 2})
        .origin("center")
      });

      socket.on('player_drop', function (drop_player_id) {
        console.log('player disconnected');
        players[drop_player_id].destroy();
        delete players[drop_player_id];
      });

      socket.on('players_positions', function (positions) {
        console.log("players_positions" + JSON.stringify(positions));
        Object.keys(positions).forEach(function (id) { 
          players[id].x = positions[id]['x'];
          players[id].y = positions[id]['y'];
          players[id].rotation = positions[id]['rotation'];
        });

      });
    });

});
});
