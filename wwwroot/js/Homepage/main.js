// NavBar
$(function () {
  $(window).on('scroll', function () {
    if ($(window).scrollTop() > 40) {
      $('.navbar').addClass('active');
    } else {
      $('.navbar').removeClass('active');
    }
  });
});

// Counter
$(".counter").countUp();


// Back to Top
window.onscroll = function () { scrollFunction() };

function scrollFunction() {
  if (document.body.scrollTop > 650 || document.documentElement.scrollTop > 650) {
    document.getElementById("myBtn").style.display = "block";
  } else {
    document.getElementById("myBtn").style.display = "none";
  }
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
  document.body.scrollTop = 0;
  document.documentElement.scrollTop = 0;
}

// Owl Carousel
$(document).ready(function () {
  $(".owl-carousel").owlCarousel({
    loop: true,
    margin: 10,
    nav: true,
    dots: false,
    responsiveClass: true,
    autoplay: true,
    navText: [`<div class='nav-btn prev-slide'><i class="fa-solid fa-angle-left"></i></div>`, `<div class='nav-btn next-slide'><i class="fa-solid fa-angle-right"></i></div>`],
    autoplayTimeout: 4000,
    autoplayHoverPause: true,
    responsive: {
      0: {
        items: 1,
      },
      600: {
        items: 3,
      },
      1000: {
        items: 3,
      }
    }
  });
});