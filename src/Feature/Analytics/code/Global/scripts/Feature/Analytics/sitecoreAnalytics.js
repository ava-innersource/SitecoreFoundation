SF = window.SF || {};
SF.SitecoreData = null;

SF.Analytics = (function () {
    
    endSession = function () {
        var url = '/sitecore/api/SF/analytics/EndSession';

        $.ajax({
            url: url,
            success: function (data) {
                console.log('Success' + data);
            },
            failure: function (errMsg) {
                console.log('Failure' + errMsg);
            }
        });
    },
    getSitecoreEvents = function () {
       
        var url = '/sitecore/api/SF/analytics/GetTracker';

        var data = {
            pageId: SF.PageID
        };

        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (data) {

                console.log('Retreived Data from Sitecore');

                analytics.identify(data.contactId, {
                    name: data.name,
                    email: data.email,
                    userName: data.userName
                });

                $.each(data.events, function (index, event) {
                    
                    analytics.track(event.name, {
                        data: event.text,
                        text: event.text,
                        value: event.value,
                        origin: 'Sitecore'
                    });

                });

                SF.SitecoreData = data;
            },
            failure: function (errMsg) {
                console.log('Failure' + errMsg);
            }
        });
    },
    trackInteraction = function (id) {

        console.log('picked up track, posting to service');

        var url = '/sitecore/api/SF/analytics/RegisterInteraction';

        var data = {
            interactionId: id
        };

        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (data) {
                console.log('Success ' + data);
            },
            failure: function (errMsg) {
                console.log('Failure ' + errMsg);
            }
        });
    },
    trackEvent = function (eventName, eventLabel)
    {
        var url = '/sitecore/api/SF/analytics/RegisterEvent';

        var data = {
            name: eventName,
            data: eventLabel,
            pageId: SF.PageID
        };

        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (data) {
                console.log('Success' + data);
            },
            failure: function (errMsg) {
                console.log('Failure' + errMsg);
            }
        });
    },
    init = function () {

        if (!analytics){
            console.log('analytics not defined');
        }

        analytics.on('track', function (event, properties, options) {
            
            if (properties.origin != "Sitecore") {

                console.log('picked up track, posting to service');
                //todo, POST to SF end point

                var url = '/sitecore/api/SF/analytics/RegisterEvent';

                var data = {
                    name: event,
                    data: properties.label,
                    pageId: SF.PageID
                };

                $.ajax({
                    type: "POST",
                    url: url,
                    data: data,
                    success: function (data) {
                        console.log('Success' + data);
                    },
                    failure: function (errMsg) {
                        console.log('Failure' + errMsg);
                    }
                });

            }

        });

    };

    return{
        init: init,
        endSession: endSession,
        getSitecoreEvents: getSitecoreEvents,
        trackInteraction: trackInteraction,
        trackEvent: trackEvent
    }
})();

analytics.ready(function () {
    SF.Analytics.init();
    SF.Analytics.getSitecoreEvents();
});