var express = require('express');
var cors = require('cors')
var bodyParser = require('body-parser');
var Pusher = require('pusher');

var pusher = new Pusher({
  appId: 'APP_ID',
  key: 'APP_KEY',
  secret: 'APP_SECRET',
  cluster: 'APP_CLUSTER',
  useTLS: true
});

var app = express();
app.use(express.static(__dirname + '/public'));

app.get('/game/character/run-left', function(req, res) {
  pusher.trigger('my-channel', 'run-left', {
    "message": ""
  });
  res.send();
});

app.get('/game/character/run-right', function(req, res) {
  pusher.trigger('my-channel', 'run-right', {
    "message": ""
  });
  res.send();
});

app.get('/game/character/attack', function(req, res) {
  pusher.trigger('my-channel', 'attack', {
    "message": ""
  });
  res.send();
});

app.get('/game/character/idle', function(req, res) {
  pusher.trigger('my-channel', 'idle', {
    "message": ""
  });
  res.send();
});

var port = process.env.PORT || 5000;
console.log('GameController - listening')
app.listen(port);
