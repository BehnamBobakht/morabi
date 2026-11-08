"use strict";
const $=(selector,scope=document)=>scope.querySelector(selector);
const $$=(selector,scope=document)=>[...scope.querySelectorAll(selector)];

const header=$(".site-header");
addEventListener("scroll",()=>header.classList.toggle("scrolled",scrollY>24),{passive:true});

const menu=$(".mobile-menu"),openMenu=$(".menu-toggle"),closeMenu=$(".menu-close");
function setMenu(open){menu.classList.toggle("open",open);menu.setAttribute("aria-hidden",String(!open));openMenu.setAttribute("aria-expanded",String(open));document.body.classList.toggle("menu-open",open)}
openMenu.addEventListener("click",()=>setMenu(true));
closeMenu.addEventListener("click",()=>setMenu(false));
$$(".mobile-menu a").forEach(a=>a.addEventListener("click",()=>setMenu(false)));
addEventListener("keydown",e=>{if(e.key==="Escape")setMenu(false)});

const revealObserver=new IntersectionObserver(entries=>entries.forEach(entry=>{if(entry.isIntersecting){entry.target.classList.add("visible");revealObserver.unobserve(entry.target)}}),{threshold:.14});
$$(".reveal").forEach(el=>revealObserver.observe(el));

$$(".slider-btn").forEach(button=>button.addEventListener("click",()=>{
  const track=document.getElementById(button.dataset.slider);
  const amount=Math.max(280,track.clientWidth*.78);
  track.scrollBy({left:button.classList.contains("next")?-amount:amount,behavior:"smooth"});
}));

$$(".card-track,.book-track,.video-track").forEach(track=>{
  let down=false,startX=0,startScroll=0;
  track.addEventListener("pointerdown",e=>{if(e.pointerType==="mouse"){down=true;startX=e.clientX;startScroll=track.scrollLeft;track.setPointerCapture(e.pointerId)}});
  track.addEventListener("pointermove",e=>{if(down)track.scrollLeft=startScroll+(e.clientX-startX)});
  track.addEventListener("pointerup",()=>down=false);
  track.addEventListener("pointercancel",()=>down=false);
});

const finePointer=matchMedia("(hover:hover) and (pointer:fine)").matches;
if(finePointer&&!matchMedia("(prefers-reduced-motion:reduce)").matches){
  const dot=$(".cursor-dot"),ring=$(".cursor-ring");let x=-100,y=-100,rx=-100,ry=-100,ringSize=34;
  const animate=()=>{rx+=(x-rx)*.17;ry+=(y-ry)*.17;dot.style.transform=`translate3d(${x-3.5}px,${y-3.5}px,0)`;ring.style.transform=`translate3d(${rx-ringSize/2}px,${ry-ringSize/2}px,0)`;requestAnimationFrame(animate)};animate();
  addEventListener("pointermove",e=>{x=e.clientX;y=e.clientY},{passive:true});
  document.addEventListener("pointerover",e=>{const action=e.target.closest("a,button,input,.tilt");ring.classList.toggle("action",!!action);ringSize=action?54:34});
  $$(".tilt").forEach(card=>{
    card.addEventListener("pointermove",e=>{const r=card.getBoundingClientRect(),px=e.clientX-r.left,py=e.clientY-r.top;card.style.setProperty("--mx",px+"px");card.style.setProperty("--my",py+"px");card.style.setProperty("--ry",((px/r.width-.5)*5).toFixed(2)+"deg");card.style.setProperty("--rx",((.5-py/r.height)*5).toFixed(2)+"deg")});
    card.addEventListener("pointerleave",()=>{card.style.setProperty("--rx","0deg");card.style.setProperty("--ry","0deg")});
  });
  const hero=$(".hero-image");
  addEventListener("pointermove",e=>{if(scrollY<innerHeight){const dx=(e.clientX/innerWidth-.5)*8,dy=(e.clientY/innerHeight-.5)*5;hero.style.transform=`scale(1.015) translate3d(${dx}px,${dy}px,0)`}},{passive:true});
}

$$(".event-shot button,.testimonial>button").forEach(button=>button.addEventListener("click",()=>{
  const playing=button.dataset.playing==="true";button.dataset.playing=String(!playing);button.textContent=playing?"▶":"Ⅱ";button.setAttribute("aria-label",playing?"پخش ویدئو":"توقف ویدئو");
}));

$(".newsletter form").addEventListener("submit",e=>{e.preventDefault();const button=$("button",e.currentTarget),previous=button.textContent;button.textContent="ثبت شد ✓";button.disabled=true;setTimeout(()=>{button.textContent=previous;button.disabled=false;e.currentTarget.reset()},2200)});

const sections=$$("main section[id]"),navLinks=$$(".desktop-nav a");
const activeObserver=new IntersectionObserver(entries=>entries.forEach(entry=>{if(entry.isIntersecting){navLinks.forEach(a=>a.classList.toggle("active",a.getAttribute("href")==="#"+entry.target.id))}}),{rootMargin:"-42% 0px -50%"});
sections.forEach(section=>activeObserver.observe(section));