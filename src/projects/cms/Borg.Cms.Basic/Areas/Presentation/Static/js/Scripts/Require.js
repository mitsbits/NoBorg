(function ($) {
    $.getScriptCached = function (url, callback) {
        var d = document, t = 'script',
            o = d.createElement(t),
            s = d.getElementsByTagName(t)[0];
        o.src = url;
        if (callback) {
            o.addEventListener('load', function (e) {
                callback(null, e);
            }, false);
        }
        s.parentNode.insertBefore(o, s);
    };

    $.getStyleSheet = function (path, fn, scope) {
        var head = document.getElementsByTagName('head')[0], // reference to document.head for appending/ removing link nodes
            link = document.createElement('link');           // create the link node
        link.setAttribute('href', path);
        link.setAttribute('rel', 'stylesheet');
        link.setAttribute('type', 'text/css');

        var sheet, cssRules;
        // get the correct properties to check for depending on the browser
        if ('sheet' in link) {
            sheet = 'sheet'; cssRules = 'cssRules';
        }
        else {
            sheet = 'styleSheet'; cssRules = 'rules';
        }

        var interval_id = setInterval(function () {                     // start checking whether the style sheet has successfully loaded
            try {
                if (link[sheet] && link[sheet][cssRules].length) { // SUCCESS! our style sheet has loaded
                    clearInterval(interval_id);                      // clear the counters
                    clearTimeout(timeout_id);
                    fn.call(scope || window, true, link);           // fire the callback with success == true
                }
            } catch (e) { } finally { }
        }, 10),                                                   // how often to check if the stylesheet is loaded
            timeout_id = setTimeout(function () {       // start counting down till fail
                clearInterval(interval_id);             // clear the counters
                clearTimeout(timeout_id);
                head.removeChild(link);                // since the style sheet didn't load, remove the link node from the DOM
                fn.call(scope || window, false, link); // fire the callback with success == false
            }, 15000);                                 // how long to wait before failing

        head.appendChild(link);  // insert the link node into the DOM and start loading the style sheet

        return link; // return the link node;
    }

    $.require = function (scripts, callback) {
        function next() {
            if (!scripts.length) {
                try {
                    callback();
                } catch (err) {
                    console.error('Error while executing callback', err, callback);
                }
                return;
            }

            if (!window.loadedScripts) {
                window.loadedScripts = [];
            }
            if (!window.loadingScriptCallbacks) {
                window.loadingScriptCallbacks = {};
            }

            var script = scripts.pop();
            var css = script.indexOf('.css') != -1;
            if (!(script.indexOf('http') == 0 || script.indexOf('//') == 0 || css)) {
                script = '/Scripts/' + script + '.js?v=7'
            }

            if (window.loadingScriptCallbacks[script]) {
                window.loadingScriptCallbacks[script].push(function () {
                    next();
                });
                return;
            }

            if (!css) {
                if ($('script[src="' + script + '"]').length || window.loadedScripts.indexOf(script) != -1) {
                    next();
                    return;
                }
            }
            else {
                if ($('link[href="' + script + '"]').length || window.loadedScripts.indexOf(script) != -1) {
                    next();
                    return;
                }
            }

            window.loadingScriptCallbacks[script] = [function () {
                next();
            }];

            function done() {
                window.loadedScripts.push(script);
                for (var i = 0; i < window.loadingScriptCallbacks[script].length; i++) {
                    try {
                        window.loadingScriptCallbacks[script][i]();
                    }
                    catch (err) {
                        console.error('Error on script callback. Script: [' + script + '], Error: ' + err, window.loadingScriptCallbacks[script][i]);
                    }
                }
                delete window.loadingScriptCallbacks[script];
            }

            if (css) {
                $.getStyleSheet(script, function () {
                    done();
                });
            }
            else {
                $.getScriptCached(script, function () {
                    done();
                });
            }
        }
        next();
    };
    window.require = function (s, fn) { (window[s] = window[s] || []).push(fn); };
    window.define = function (s) {
        if (typeof (window[s]) != 'undefined') {
            for (var i = 0; i < window[s].length; i++) {
                window[s][i]();
            }
        }
        window[s] = {
            push: function (fn) {
                fn();
            }
        };
    };

    $.getOnce = function (options) {
        if (!window.getOnceCallbacks) {
            window.getOnceCallbacks = {};
        }
        if (options.data) {
            var s = $.param(options.data, options.traditional);
            if (options.url.indexOf('?') == -1) {
                options.url += '?' + s;
            }
            else {
                options.url += '&' + s;
            }
            delete options.data;
        }
        if (window.getOnceCallbacks[options.url]) {
            window.getOnceCallbacks[options.url].push({
                success: options.success,
                error: options.error
            });
            return;
        }

        window.getOnceCallbacks[options.url] = [{
            success: options.success,
            error: options.error
        }];
        function done(m, a) {
            var callbacks = window.getOnceCallbacks[options.url];
            for (var i = 0; i < callbacks.length; i++) {
                callbacks[i][m].apply(this, a);
            }
            delete window.getOnceCallbacks[options.url];
        }

        options.success = function () {
            done.apply(this, ['success', arguments]);
        };
        options.error = function () {
            done.apply(this, ['error', arguments]);
        };
        $.ajax(options);
    }
})(jQuery);