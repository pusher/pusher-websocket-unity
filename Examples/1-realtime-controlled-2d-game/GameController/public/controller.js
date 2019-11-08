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
      case 3:
        runLeft();
        break;
      case 2:
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

document.addEventListener("keydown", ev => {

  switch(ev.key){
    case "ArrowLeft": 
      updateAction(3);
      break;
    case "ArrowRight":
      updateAction(2)
      break;
    case "b":
      updateAction(4)
      break;
  }
  console.log(ev.key)
})

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
  console.log("Update Action!")
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
