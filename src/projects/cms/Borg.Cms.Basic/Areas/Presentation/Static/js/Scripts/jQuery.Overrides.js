(function ($) {
    $.fn.enabled = function (e) {
        $(this).prop('disabled', !e);
        $(this).trigger('enabled:change', e);
    }
    var domMethods = ['html', 'append', 'prepend'];

    var onContentInjected = debounce(function ($element) {
        $element.trigger('contentInjected');
    }, 100);

    var onContentRemoved = debounce(function ($element, arg) {
        $element.trigger('contentRemoved', arg);
    }, 100);

    $.each(domMethods, function (index, item) {
        var original = $.fn[item];
        $.fn["__" + item] = original;
        $.fn[item] = function () {
            var result = original.apply(this, arguments);

            onContentInjected($(this));

            return result;
        };
    });

    $.each(['replaceWith', 'after', 'before'], function (index, item) {
        var original = $.fn[item];
        $.fn["__" + item] = original;
        $.fn[item] = function () {
            var parent = $(this).parent();
            var result = original.apply(this, arguments);
            onContentInjected(parent);

            return result;
        };
    });

    (function ($) {
        var remove = $.fn.remove;
        $.fn.remove = function () {
            var $this = $(this);
            var parent = $this.parent();

            var r = remove.apply(this, arguments);

            onContentRemoved(parent, this);

            return r;
        };
    })($);
})(jQuery);