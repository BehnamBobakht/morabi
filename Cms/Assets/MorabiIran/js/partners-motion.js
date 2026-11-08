(function(){'use strict';
var track=document.getElementById('partners-track');if(!track||window.matchMedia('(prefers-reduced-motion: reduce)').matches)return;
var timer,paused=false,direction=1,index=0;
function step(){if(paused)return;var cards=track.querySelectorAll('article');if(cards.length<2)return;index+=direction;if(index>=cards.length-1){index=cards.length-1;direction=-1}else if(index<=0){index=0;direction=1}var trackRect=track.getBoundingClientRect();var cardRect=cards[index].getBoundingClientRect();track.scrollBy({left:cardRect.left-trackRect.left,top:0,behavior:'smooth'});}
function start(){clearInterval(timer);timer=setInterval(step,3200)}
['mouseenter','touchstart','pointerdown'].forEach(function(e){track.addEventListener(e,function(){paused=true},{passive:true})});
['mouseleave','touchend','pointerup'].forEach(function(e){track.addEventListener(e,function(){paused=false;start()},{passive:true})});
start();
}());
