$(document).ready(function () {

  $(".composite.async").each(function (index) {
    var container = $(this); 
    var contentUrl = container.attr("data-src");

    console.log('fetching data from ' + contentUrl);

    $.get(contentUrl, function (data) {
      console.log('data obtained, loading into dom');
      container.html(data);
    });
    
  });
});