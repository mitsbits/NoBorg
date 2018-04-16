$.fn.Loader = function (showLoader) {
    return $(this).each(function () {
        var $element = $(this);

        function show() {
            var offset = $element.offset();
            var width = $element.outerWidth();
            var height = $element.outerHeight();

            var loader = $('<div />')
                .addClass('spinLoader')
                .append('<div class="cnt"> < i class="blt" ></i > <i class="blt"></i> <i class="blt"></i> <i class="blt"></i></div >')
                .css({
                    'left': offset.left,
                    'top': offset.top,
                    'width': width,
                    'height': height
                });
            $element.data('loader', loader);
            loader.appendTo($('body'));
            loader.fadeIn();
        }

        function hide() {
            var loader = $element.data('loader');
            if (!loader) {
                return;
            }
            loader.remove();
            $element.data('loader', null);
        }

        if (showLoader) {
            show();
        }
        else {
            hide();
        }
    });
}