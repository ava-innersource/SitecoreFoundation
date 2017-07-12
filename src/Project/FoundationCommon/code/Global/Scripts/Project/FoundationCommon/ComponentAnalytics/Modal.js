analytics.ready(function () {

    $("a[data-reveal-id]").click(function () {
        var event = "ModalOpened";
        var tabName = $(this).text();

        console.log(event + ": " + tabName);

        analytics.track(event, {
            label: tabName,
            category: 'Page Interaction'
        });

    });

});