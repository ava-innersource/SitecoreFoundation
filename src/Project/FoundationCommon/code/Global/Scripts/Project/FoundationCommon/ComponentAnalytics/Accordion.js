analytics.ready(function () {

    $(".accordion-navigation a[role='tab']").click(function () {
        var event = "AccordionClicked";
        var tabName = $(this).find("a").text();

        console.log(event + ": " + tabName);

        analytics.track(event, {
            label: tabName,
            category: 'Page Interaction'
        });

    });

});