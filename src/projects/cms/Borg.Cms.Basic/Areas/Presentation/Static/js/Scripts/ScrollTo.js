$.fn.scrollTo = function () {
    $('html, body').animate({
        scrollTop: $(this).offset().top
    }, 500);
};

$.fn.scrollToWithOffset = function (margin) {
    $('html, body').animate({
        scrollTop: $(this).offset().top + margin
    }, 500);
};