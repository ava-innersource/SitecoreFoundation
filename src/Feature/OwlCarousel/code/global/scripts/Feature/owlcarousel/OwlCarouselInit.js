$(document).ready(function () {
  console.log('initing carousels');
  $(".owl-carousel").each(function (index) {
    var dataOptionsStr = $(this).attr("data-options");
    console.log('initing carousel with params: ' + dataOptionsStr);
    var dataOptions = jQuery.parseJSON(dataOptionsStr);
    $(this).owlCarousel(dataOptions);
  });
});