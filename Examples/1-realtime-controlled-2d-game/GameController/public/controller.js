var url = "game/character/"
var Action = {
  IDLE: 1,
  RUNLEFT: 2,
  RUNRIGHT: 3,
  ATTACK: 4,
}

var currAction = 1;

async function loop(){
  while (true){
    switch(currAction) {
      case 1:
        idle();
        break;
      case 2:
        runLeft();
        break;
      case 3:
        runRight();
        break;
      case 4:
        attack();
        break;
      default:
        idle();
        break;
    }
    start_time = new Date()
    target = new Date()
    target.setMilliseconds(target.getMilliseconds() + 500);
    currAction = 1
    await until(() => (new Date) > target)
  }
}

function httpGet(theUrl, action ) {
      var xmlHttp = new XMLHttpRequest();
      xmlHttp.open( "GET", theUrl+action, false);
      xmlHttp.send( null );
      return xmlHttp.responseText;
}

function attack(){
  httpGet(url, "attack");
}

function runLeft(){
  httpGet(url, "run-left");
}

function runRight(){
  httpGet(url, "run-right");
}

function idle(){
  httpGet(url, "idle");
}

function updateAction(num){
  currAction = num
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function until(fn) {
    while (!fn()) {
        await sleep(0)
    }
}
