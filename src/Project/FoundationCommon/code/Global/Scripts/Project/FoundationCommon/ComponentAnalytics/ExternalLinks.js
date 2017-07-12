analytics.ready(function () {

    $.expr[':'].external = function (obj) {
        return !obj.href.match(/^mailto\:/)
                && !obj.href.match(/^javascript\:/)
                && (obj.hostname != location.hostname);
    };

    // Manage clicks on external links
    $('a:external').each(function (index, element) {
        var linkLabel = $(element).text().substring(0, 9) + "(" + $(element).attr('href') + ")";
        
        analytics.trackLink(element, 'Referred To', {
            label: linkLabel,
            category: 'External Links'
        });
    });
    
});