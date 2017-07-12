(function ($) {

	var manualStartFilter = '[data-start="manual"]';

	function initFoundation() {
		$(document).foundation({
			equalizer: {
				equalize_on_stack: false
			}
		});
	}

	function initTopBar() {
		var path = window.location.pathname.toLowerCase();
		$('.top-bar a').each(function () {
			var $this = $(this),
				href = $this.attr('href').toLowerCase();
			if (href == path) {
				$this.closest('li').addClass('active');
			}
		});
	}

	function initCarousels() {
		var carousels = $('.carousel').not(manualStartFilter);
		carousels.slick({
			adaptiveHeight: true,
			arrows: true,
			autoplay: true,
			autoplaySpeed: 5000,
			dots: true,
			fade: true
		});
		carousels.find('.slide.hide').removeClass('hide');
	}

	$(function () {
		initFoundation();
		initCarousels();
		initTopBar();
	});	

}(window.jQuery));