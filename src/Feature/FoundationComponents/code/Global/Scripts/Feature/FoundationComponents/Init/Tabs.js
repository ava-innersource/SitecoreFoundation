$( document ).ready(function() {
  $(".tabs-container").each(function(index){
    var tabContainer = $(this);
    var tabs = tabContainer.find(".tabs");
    
    var tabContentContainer = tabContainer.find(".tabs-content");
    var tabContent = tabContentContainer.children(".content[role='tabpanel']");
    
    tabContent.each(function(index){
    	var tabTitle = $(this).attr("data-title");
    	var tabID = $(this).attr("id");
    	tabs.append('<li class="tab-title" role="presentation"><a href="#' + tabID + '" role="tab" tabindex="0" aria-selected="false" aria-controls="' + tabID + '">' + tabTitle + '</a></li>');
    });
    
    //open the first item
    if (tabContainer.find(".tabs-content .content.active").length == 0)
    {
    	//None are active, lets make first tab active
    	tabContent.eq(0).addClass("active");
    }
    
  });
  
  //reflow tabs
  $(document).foundation('tab', 'reflow');
  
});