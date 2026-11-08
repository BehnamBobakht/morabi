(function(){'use strict';
var section=document.querySelector('.books.section');
if(!section)return;
var container=section.querySelector(':scope > .container');
if(!container)return;
var frame=0;
function fit(){
  cancelAnimationFrame(frame);
  frame=requestAnimationFrame(function(){
    var compact=window.matchMedia('(max-width: 760px)').matches;
    var top=compact?22:34;
    var bottom=compact?16:28;
    var height=Math.ceil(container.getBoundingClientRect().height+top+bottom);
    section.style.setProperty('height',height+'px','important');
    section.style.setProperty('min-height','0','important');
    section.style.setProperty('max-height',height+'px','important');
  });
}
fit();
window.addEventListener('load',fit);
window.addEventListener('resize',fit,{passive:true});
section.querySelectorAll('img').forEach(function(img){if(!img.complete)img.addEventListener('load',fit,{once:true});});
if(window.ResizeObserver){var observer=new ResizeObserver(fit);observer.observe(container);}
}());
