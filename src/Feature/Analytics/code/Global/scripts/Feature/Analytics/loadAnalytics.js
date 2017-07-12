window.analytics || (window.analytics = []);
window.analytics.methods = ['identify', 'track', 'trackLink', 'trackForm', 'trackClick', 'trackSubmit', 'page', 'pageview', 'ab', 'alias', 'ready', 'group', 'on', 'once', 'off'];
window.analytics.factory = function (method) {
    return function () {
        var args = Array.prototype.slice.call(arguments);
        args.unshift(method);
        window.analytics.push(args);
        return window.analytics;
    };
};

for (var i = 0; i < window.analytics.methods.length; i++) {
    var method = window.analytics.methods[i];
    window.analytics[method] = window.analytics.factory(method);
}

// Load analytics async
analytics.load = function (callback) {
    if (document.getElementById('analytics-js')) return;

    // We make a copy if our dummy object
    window.a = window.analytics;
    var script = document.createElement('script');
    script.async = true;
    script.id = 'analytics-js';
    script.type = 'text/javascript';
    script.src =  '/Scripts/analytics.min.js';
    script.addEventListener('load', function (e) {
        if (typeof callback === 'function') {
            callback(e);
        }
    }, false);
    var first = document.getElementsByTagName('script')[0];
    first.parentNode.insertBefore(script, first);
};

analytics.load(function () {

    // On load init our integrations
    if (typeof gaCode !== 'undefined') { 

        analytics.initialize({
            'Google Analytics': {
                trackingId: gaCode
            }
        });
    }
    else
    {
        analytics.initialize();
    }

    // Now copy whatever we applied to our dummy object to the real analytics
    while (window.a.length > 0) {
        var item = window.a.shift();
        var method = item.shift();
        if (analytics[method]) analytics[method].apply(analytics, item);
    }

    
});

analytics.page();