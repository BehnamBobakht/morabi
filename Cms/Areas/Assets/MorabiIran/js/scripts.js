$(".menu-btn , .menu-back ").click(() => {
  $(".menu-btn").toggleClass("collapsrBar");
  $(".menu-back").toggleClass("d-none"), $(".menu").toggleClass("menu-off");
  $("body").toggleClass("overlay");
  $(".category-btn").next(".category-content").slideUp();
  $(".category-btn").find("svg").removeClass("rote180");
}),
// $(".menu-back , .close_menu").click(() => {
//   $(".menu").addClass("menu-off"), $(".menu-back").addClass("d-none");
//   $(".menu-btn").toggleClass("collapsrBar");
//   $("body").toggleClass("overlay");
// })



// $(document).click(function(e) {
//   if ( $(e.target).closest('.sort').length === 0 ) {
//       $(".sort").find(".multi-collapse").removeClass("show")
//   }
// }); 

$(".category-btn.first-category").click(function () {
  if ($(this).next(".category-content").is(':visible')){
    $(".category-btn").next(".category-content").slideUp();
    $(".category-btn").find("svg").removeClass("rote180");
  }
  else{
    $(".category-btn").next(".category-content").slideUp();
    $(".category-btn").find("svg").removeClass("rote180");
      $(this).find("svg").addClass("rote180");
      $(this).next(".category-content").slideDown();
  }

});
$(".first-content .category-btn").click(function () {
if ($(this).next(".category-content").is(':visible')){
  $(this).next(".category-content").slideUp();
  $(this).find("svg").removeClass("rote180");
}
else{
  $(".first-content .category-btn").next(".category-content").slideUp();
  $(".first-content .category-btn").find("svg").removeClass("rote180");
    $(this).find("svg").addClass("rote180");
    $(this).next(".category-content").slideDown();
}
});


if ($(window).width() < 992){
  $(".userProfile").addClass("dropup")
}
 
   
$('.main_slider .owl-carousel').owlCarousel({
  loop:true,
  margin:0,
 dots:true,
 nav:false,
  rtl:true,
  autoplay:true,
  responsive:{
      0:{
          items:1
      },
      600:{
          items:1  
      },
      1000:{
          items:1
      }
  }
}); 
$('.home_blog .blog_slider.owl-carousel').owlCarousel({
  loop:true,
  margin:0,
 dots:true,
  rtl:true,
  autoplay:true,
  nav: false,
  stagePadding:55,
  margin:7,
  responsive:{
      0:{
          items:1
      },
      600:{
          items:3
      },
      1000:{
          items:4,
          stagePadding:0,
          margin:15
      }
  }
});       
 
$('.blog_slider.owl-carousel').owlCarousel({
  loop:true,
  margin:0,
 dots:true,
  rtl:true,
  autoplay:true,
  nav: false,
  stagePadding:55,
  margin:7,
  responsive:{
      0:{
          items:1
      },
      600:{
          items:3
      },
      1000:{
          items:3,
          stagePadding:0
      }
  }
});


$('.product_slider').owlCarousel({
  loop:true,
  margin:0,
 dots:true,
  rtl:true,
  autoplay:true,
  nav: false,
  stagePadding:0,
  margin:7,
  responsive:{
      0:{
          items:2
      },
      600:{
          items:4
      },
      1000:{
          items:5,
          margin:20
      }
  }
});




var swiper = new Swiper(".blog_swiper", {
  spaceBetween: 5,
  loop:true,
  pagination: {
    el: ".training_pagination",
    clickable: true,
  },
  breakpoints: {
    0: {
      slidesPerView: 1,
      centeredSlides:true
    },
    600: {
      slidesPerView: 2,
      spaceBetween:20
    },
    1000: {
      slidesPerView: 4,
      spaceBetween:20
    }
  }
});

// var swiper = new Swiper(".mostSale_slider", {
//   slidesPerView: 2,
//   spaceBetween: 10,
//   loop:true,
//   pagination: {
//     el: ".sale-pagination",
//     clickable: true,
//   },
//   breakpoints: {
//     0: {
//       slidesPerView: 2
//     },
//     600: {
//       slidesPerView: 3
//     },
//     1000: {
//       slidesPerView: 6
//     }
//   }
// });
// var swiper = new Swiper(".consumption_slider", {
//   spaceBetween: 10,
//   loop:true,
//   pagination: {
//     el: ".consumption-pagination",
//     clickable: true,
//   },
//   breakpoints: {
//     0: {
//       slidesPerView: 2
//     },
//     600: {
//       slidesPerView: 3
//     },
//     1000: {
//       slidesPerView: 6
//     }
//   }
// });
// var swiper = new Swiper(".suggested_slider", {
//   spaceBetween: 10,
//   loop:true,
//   pagination: {
//     el: ".suggested-pagination",
//     clickable: true,
//   },
//   breakpoints: {
//     0: {
//       slidesPerView: 2
//     },
//     600: {
//       slidesPerView: 3
//     },
//     1000: {
//       slidesPerView: 6
//     }
//   }
// });
// var swiper = new Swiper(".topBrands_slider", {
//   spaceBetween: 10,
//   loop:true,
//   pagination: {
//     el: ".topBrands-pagination",
//     clickable: true,
//   },
//   breakpoints: {
//     0: {
//       slidesPerView: 2
//     },
//     600: {
//       slidesPerView: 3
//     },
//     1000: {
//       slidesPerView: 6
//     }
//   }
// });
// var swiper = new Swiper(".newest_slider", {
//   spaceBetween: 10,
//   loop:true,
//   pagination: {
//     el: ".newest-pagination",
//     clickable: true,
//   },
//   breakpoints: {
//     0: {
//       slidesPerView: 2
//     },
//     600: {
//       slidesPerView: 3
//     },
//     1000: {
//       slidesPerView: 6
//     }
//   }
// });
// var swiper = new Swiper(".home_blog_slider", {
//   spaceBetween: 10,
//   loop:false,
//   pagination: {
//     el: ".homeBlog-pagination",
//     clickable: true,
//   },
//   breakpoints: {
//     0: {
//       slidesPerView: 1
//     },
//     600: {
//       slidesPerView: 2
//     },
//     1000: {
//       slidesPerView: 3
//     }
//   }
// });

var swiper = new Swiper(".similar_slider", {
  spaceBetween: 10,
  loop:true,
  pagination: {
    el: ".similar-pagination",
    clickable: true,
  },
  breakpoints: {
    0: {
      slidesPerView: 2
    },
    600: {
      slidesPerView: 4
    },
    1000: {
      slidesPerView: 6
    }
  }
});

// $(".scroll_btn").click(function(){
//   $([document.documentElement, document.body]).animate({
//     scrollTop: 0
// }, 500);
// })

$('.relatedArticles_slider').owlCarousel({
  loop: true,
  margin: 0,
  dots: false,
  rtl: true,
  autoplay: false,
  nav: false,
  responsive: {
      0: {
          items: 1
      },
      600: {
          items: 1
      },
      1000: {
          items: 4
      }
  }
});
/*==================================range slider======================================*/
$(function () {
  $('.slider').on('input change', function () {
      $(this).next($('.slider_label')).html(this.value);


  });
  $('.slider_label').each(function () {
      var value = $(this).prev().attr('value');
      $(this).html(value);
  });


})
$(function () {

  var $range = $(".js-range-slider"),
      $inputFrom = $(".js-input-from"),
      $inputTo = $(".js-input-to"),
      instance,
      min = 0,
      max = 10000000,
      from = 0,
      to = 0;

  $range.ionRangeSlider({
      type: "double",
      min: min,
      max: max,
      from: 0,
      to: 5000000,
      prefix: 'تومان ',
      onStart: updateInputs,
      onChange: updateInputs,
      step: 50000,
      prettify_enabled: true,
      prettify_separator: ".",
      values_separator: " - ",
      force_edges: true


  });

  instance = $range.data("ionRangeSlider");

  function updateInputs(data) {
      from = data.from;
      to = data.to;

      $inputFrom.prop("value", from);
      $inputTo.prop("value", to);
  }

  $inputFrom.on("input", function () {
      var val = $(this).prop("value");

      // validate
      if (val < min) {
          val = min;
      } else if (val > to) {
          val = to;
      }

      instance.update({
          from: val
      });
  });

  $inputTo.on("input", function () {
      var val = $(this).prop("value");

      // validate
      if (val < from) {
          val = from;
      } else if (val > max) {
          val = max;
      }

      instance.update({
          to: val
      });
  });
});


// /*=================================total products=========================================*/





$(".products_filter_slide  .filter_item_icon").click(function () {
  if ($(this).closest(".filter_dropdown").find(".panel_slide").find("div:first").is(':visible')) {
      $(this).closest(".filter_dropdown").find(".panel_slide:first").find(".expand").removeClass("expand");
      $(this).closest(".filter_dropdown").removeClass("expand");
  }
  else {
      $(".product_filter_item ").removeClass("expand");
      $(this).closest(".filter_dropdown").removeClass("expand");
      $(this).closest(".panel_slide").find(".expand").removeClass("expand");
      $(this).closest(".filter_dropdown").addClass("expand")
  }

});

$(".subCat_filter_slide .filter_item_icon").click(function () {
  if ($(this).closest(".filter_dropdown").find(".panel_slide").find("div:first").is(':visible')) {
      $(this).closest(".filter_dropdown").find(".panel_slide:first").find(".expand").removeClass("expand");
      $(this).closest(".filter_dropdown").removeClass("expand");
  }
  else {
      $(this).closest(".filter_dropdown").removeClass("expand");
      $(this).closest(".panel_slide").find(".expand").removeClass("expand");
      $(this).closest(".filter_dropdown").addClass("expand")
  }

});

$(".showFilter").click(function () {
  $("body").addClass("overlay");
  $(".Filter_Product").addClass("slideFilter")

});

$(".closeFilter").click(function () {
  $("body").removeClass("overlay");
  $(".Filter_Product").removeClass("slideFilter")

});


$(document).click(function (e) {
  if ($(e.target).closest('.filter_box').length === 0 && $(e.target).closest('.showFilter').length === 0) {
      $("body").removeClass("overlay");
      $(".Filter_Product").removeClass("slideFilter")
  }
});


// jQuery(".quantity").each(function () {
//     var spinner = jQuery(this),
//         input = spinner.find('input[type="number"]'),
//         btnUp = spinner.find(".quantity-up"),
//         btnDown = spinner.find(".quantity-down"),
//         min = input.attr("min"),
//         max = input.attr("max");
//     btnUp.click(function () {
//         var oldValue = parseFloat(input.val());
//         if (oldValue >= max) {
//             var newVal = oldValue;
//         } else {
//             var newVal = oldValue + 1;
//         }
//         spinner.find("input").val(newVal);
//         spinner.find("input").trigger("change");
//     });
//     btnDown.click(function () {
//         var oldValue = parseFloat(input.val());
//         if (oldValue <= min) {
//             var newVal = oldValue;
//         } else {
//             var newVal = oldValue - 1;
//         }
//         spinner.find("input").val(newVal);
//         spinner.find("input").trigger("change");
//     });
// });



// Fancybox.bind('[data-fancybox="gallery"]', {
//   animated: false,
//   showClass: false,
//   hideClass: false,

//   click: false,

//   dragToClose: false,

//   Image: {
//     zoom: false,
//   },

//   Toolbar: {
//     display: [{ id: "counter", position: "center" }, "close"],
//   },
// });

// /*============================================profile==========================================*/
$(".itemFooter a").click(function () {
  var parent = $(this).closest(".orderParent");

  parent.find(".orderItemContent ").slideToggle('slow');

});
if ($(window).width() < 992) {
$(".panelOptions").slideUp();  
}

$(".toggle_sideList").click(function(){
  $(".panelOptions").slideToggle();
});

// /*=====================================mega menu==============================*/
if ($(window).width() > 990) {
  $(".dropdown").hover(function () {
      $(this).find(".dropDownContent").toggleClass("show animate__animated  animate__fadeIn ") 
  });
}
if ($(window).width() < 991) {
  $(".category_link").click(function () {
      $("body").addClass("body_filter");
      $(this).next(".dropDownContent").addClass("showsidebar");
  });
  $(".closeSide").click(function () {
      $("body").removeClass("body_filter");
      $(".dropDownContent").removeClass("showsidebar")
  });
}
$(".dropItem").hover(function (e) {
  $(".dropItem").removeClass("active");
  $(this).addClass("active");
  var itemName = $(this).find("a").attr('dtarget');
  var targetName = $(".dropContentBox[name=" + itemName + "]").attr("name");
  $('.dropContentBox').removeClass("d-block");
  $(".dropContentBox[name=" + itemName + "]").addClass("d-block");

});

$(".dropItem").click(function (e) {
  var itemName = $(this).find("a").attr('dtarget');
  var targetName = $(".dropContentBox[name=" + itemName + "]").attr("name");
  $('.dropContentBox').removeClass("d-block");
  $(".dropContentBox[name=" + itemName + "]").addClass("d-block");

});

$(".closeSide").click(function () {
  $("body").removeClass("body_filter");
  $(".dropDownContent").removeClass("showsidebar")
});


// $(".userProfile_icon").click(function () {
//   $(this).closest(".userProfile").find(".profile_items").toggle();
// });

// $(".login_btn").click(function(){
//   $(".register_col").addClass("hide_register");
//   $(".login_col").removeClass("hide_login")
// });
// $(".register_btn").click(function(){
//   $(".login_col").addClass("hide_login");
//   $(".register_col").removeClass("hide_register");
// });



   
$(".seeMoreButton").click(function () {
  $(".limitedText").slideUp();
  $(".totlalText").slideDown();
});

$(".hideMoreTextBtn").click(function () {
  $(".totlalText").slideUp();
  $(".limitedText").slideDown();
});


$(window).scroll(function() {    
  var scroll = $(window).scrollTop();

  if (scroll >= 200) {
      $("header").addClass("sticky");
  }
  else{
    $("header").removeClass("sticky");

  }
}); 


Fancybox.bind('[data-fancybox="gallery"]', {
  animated: false,
  showClass: false,
  hideClass: false,

  click: false,

  dragToClose: false,

  Image: {
    zoom: false,
  },

  Toolbar: {
    display: [{ id: "counter", position: "center" }, "close"],
  },
});


$(".blogItem").find(".blogItemText").text(function (index, currentText) {
  return (currentText.substr(0, 200) + '...');
});


$(".toggle_search").click(function(){
  $("header").find(".serachBox").toggleClass("show");
  document.getElementById("searchInput").value = "";
  $(".search_result").addClass("d-none");
  $(".search_loading").addClass("d-none")
})
/* =================================Ghorbani Scripts============================== */
$(".show-comments").click(function(){
  $([document.documentElement,document.body]).animate({
    scrollTop:780,
  },1000);
  $(".nav-link").removeClass(" active");
  $("#product-comments-tab").addClass(" active");
  $(".tab-pane").removeClass(" show active");
  $("#product-comments").addClass(" show active");
  return false;
});
$(".show-available").click(function(){
  $(".show-available-parent").toggleClass(" show");
});
var swiper = new Swiper(".main-swiper", {
  slidesPerView: 1,
  centeredSlides: false,
  slidesPerGroupSkip: 1,
  grabCursor: true,
  spaceBetween: 0,
  loop: true,
  keyboard: {
    enabled: true,
  },
  autoplay: {
    delay: 10000,
    disableOnInteraction: true,
  },
  pagination: {
    el: ".swiper-pagination",
    clickable: true,
  },
  navigation: {
    nextEl: ".swiper-button-next",
    prevEl: ".swiper-button-prev",
  },
});
var swiper = new Swiper(".slider1", {
  slidesPerView: 1,
  centeredSlides: false,
  slidesPerGroupSkip: 1,
  grabCursor: true,
  spaceBetween: 10,
  loop: true,
  keyboard: {
    enabled: true,
  },
  breakpoints: {
    576: {
      slidesPerView: 2,
    },
    768: {
      slidesPerView: 3,
    },
    992: {
      slidesPerView: 4,
    },
  }
});
var swiper = new Swiper(".slider2", {
  slidesPerView: 1,
  centeredSlides: false,
  slidesPerGroupSkip: 1,
  grabCursor: true,
  spaceBetween: 10,
  loop: true,
  keyboard: {
    enabled: true,
  },
  pagination: {
    el: ".swiper-pagination",
    clickable: true,
  },
  navigation: {
    nextEl: ".swiper-button-next",
    prevEl: ".swiper-button-prev",
  },
  breakpoints: {
    576: {
      slidesPerView: 2,
    },
    992: {
      slidesPerView: 3,
    },
  }
});
var swiper = new Swiper(".slider3", {
  slidesPerView: 1,
  centeredSlides: false,
  slidesPerGroupSkip: 1,
  grabCursor: true,
  spaceBetween: 30,
  loop: true,
  keyboard: {
    enabled: true,
  },
  breakpoints: {
    576: {
      slidesPerView: 2,
    },
    992: {
      slidesPerView: 3,
    },
    1300: {
      slidesPerView: 4,
    },
  }
});
var swiper = new Swiper(".slider4", {
  effect: "cards",
  grabCursor: true,
  spaceBetween: 30,
  keyboard: {
    enabled: true,
  },
  navigation: {
    nextEl: ".swiper-button-next",
    prevEl: ".swiper-button-prev",
  },
});
$(document).ready(function(){
  $(".slider5").owlCarousel({
      loop:false,
      autoplay:true,
      autoplayTimeout:10000,
      autoplayHoverPause:true,
      margin:10,
      nav:false,
      dots:false,
      rtl:true,
      responsive:{
          0:{ 
              items:1						
          },
          576:{
              items:2
          },
          768:{
            items:3
          },
          992:{
              items:4
          }
      }
  })
});


$(".go-top-btn").click(function(){
  $([document.documentElement,document.body]).animate({
    scrollTop:0,
  },1000);
  return false;
});
$(document).ready(function(){
  $('.slider20').owlCarousel({
      loop:true,
      autoplay:true,
      autoplayTimeout:10000,
      autoplayHoverPause:true,
      margin:25,
      nav:false,
      dots:false,
      rtl:true,
      responsive:{
          0:{ 
              items:1						
          },
          576:{
              items:2
          },
          768:{
              items:2
          },
          992:{
              items:4
          }
      }
  })
});
$(".next").click(function () {
  var currentowl = $(this).parents("section").find(".owl-carousel");
  currentowl.trigger('next.owl.carousel');
})
$(".prev").click(function () {
  var currentowl = $(this).parents("section").find(".owl-carousel");
  currentowl.trigger('prev.owl.carousel');
});
$(".filtering_item.box .filtering_title").click(function () {
  $(this).find("svg").toggleClass("rote180");
  $(this).next(".open-close-box").slideToggle();
});
$(".select-cat").click(function () {
  $(this).find(".select-cat-content").slideToggle();
});
$(document).ready(function(){
  $('.slider30').owlCarousel({
      loop:true,
      autoplay:true,
      autoplayTimeout:10000,
      autoplayHoverPause:true,
      margin:10,
      nav:false,
      dots:false,
      rtl:true,
      responsive:{
          0:{ 
              items:1						
          },
          576:{
              items:2
          },
          992:{
              items:3
          }
      }
  })
});