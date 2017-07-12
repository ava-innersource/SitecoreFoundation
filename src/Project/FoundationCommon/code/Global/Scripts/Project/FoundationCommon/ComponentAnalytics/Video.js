analytics.ready(function () {

    $('.video-js').each(function (index, element) {

        console.log('attaching to video events for analytics');

        var videoId = $(element).attr('id');
        var player = videojs(videoId);
        
        var videoPath = player.src().split('/');
        var videoFileName = videoPath[videoPath.length - 1].split('.');
        var videoName = videoFileName[0];

        var curWidth = player.width();
        var curHeight = player.height();

        var videoContainer = $(element).parents('.flex-video');
        if (videoContainer.length > 0){
            var newWidth = $(videoContainer[0]).width();
            var newHeight = (newWidth * curHeight) / curWidth;

            player.width(newWidth);
            player.height(newHeight);
        }

        player.on('play', function () {

            console.log('tracking play');

            analytics.track('Video Started', {
                label: videoName,
                category: 'Video'
            });
        });

        player.on('ended', function () {
            console.log('tracking finish');

            analytics.track('Video Finished', {
                label: videoName,
                category: 'Video'
            });
        });

        var percentsPlayedInterval = 25;
        var percentsAlreadyTracked = [];
        var __indexOf = [].indexOf || function (item) { for (var i = 0, l = this.length; i < l; i++) { if (i in this && this[i] === item) return i; } return -1; };

        player.on('timeupdate', function () {

            var currentTime, duration, percent, percentPlayed, _i;
            currentTime = Math.round(this.currentTime());
            duration = Math.round(this.duration());
            percentPlayed = Math.round(currentTime / duration * 100);
            for (percent = _i = 0; _i <= 99; percent = _i += percentsPlayedInterval) {
                if (percentPlayed >= percent && __indexOf.call(percentsAlreadyTracked, percent) < 0) {
                    if (percentPlayed !== 0) {
                        
                        console.log('tracking percent played ' + percent);

                        analytics.track('Video ' + percent + ' Percent Played', {
                            label: videoName,
                            category: 'Video'
                        });

                    }
                    if (percentPlayed > 0) {
                        percentsAlreadyTracked.push(percent);
                    }
                }
            }

        });
    });

    

});