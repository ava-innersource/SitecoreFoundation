analytics.ready(function () {

    $(".tab-title").click(function () {
        var event = "TabClicked";
        var tabName = $(this).text();

        console.log(event + ": " + tabName);

        analytics.track(event, {
            label: tabName,
            category: 'Page Interaction'
        });
        
    });

});