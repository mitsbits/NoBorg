(function ($) {

    function executePlugin($element, pluginName, pluginConfiguration) {
        var plugin = $.attributePlugins[pluginName];
        if (!plugin) {
            console.warn('plugin "' + pluginName + '" did not load properly');
            return;
        }
        plugin($element, pluginConfiguration);
    }
    function init($element, pluginName, pluginConfiguration) {
        var plugin = $.attributePlugins[pluginName];
        if (plugin) {
            executePlugin($element, pluginName, pluginConfiguration);
        }
        else {
            var pluginScript = 'Plugins/' + pluginName;
            $.require([pluginScript], function () {
                executePlugin($element, pluginName, pluginConfiguration);
            });
        }
    }
    function attach($container) {
        $container.find('*').addBack().each(function () {
            var $element = $(this);
            var data = $element.data();
            var attributeKeys = $.grep(Object.keys(data), function (key, index) {
                return key.indexOf('plugin') === 0 && key.indexOf('_initialized') == -1;
            });
            for (var i = 0; i < attributeKeys.length; i++) {
                var key = attributeKeys[i];
                var pluginName = key.substring('plugin'.length).toLowerCase();
                if (data[pluginName + '_initialized']) {
                    continue;
                }
                $element.data(pluginName + '_initialized', true);
                init($element, pluginName, data[key]);
            }
        });
    }
    $(function () {
        attach($('body'));
    });
    $('body').on('contentInjected', function (e) {
        attach($('body'));
    });
    $.defineAttributePlugin = function (pluginName, plugin) {
        if (!$.attributePlugins) {
            $.attributePlugins = {};
        }
        $.attributePlugins[pluginName] = plugin;
    };
    $.fn.parseAttributePlugins = function() {
        attach($(this));
    };

})(jQuery);