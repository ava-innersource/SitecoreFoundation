// Handles up to 8 up icon bar
$( document ).ready(function() {
	$(".icon-bar").each(function(index){
	  var numItems = $(this).find(".item").length;
	  var classes = [ "", "one-up", "two-up", "three-up", "four-up", "five-up", "six-up", "seven-up", "eight-up" ];
	  if (numItems < 9)
	  {
	    $(this).addClass(classes[numItems]);
	  }
	});
});

