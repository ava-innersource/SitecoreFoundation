var sitecore = sitecore || {};
sitecore.sharedsource = sitecore.sharedsource || {};
sitecore.sharedsource.codeeditor = sitecore.sharedsource.codeeditor || {};

(function($, sitecore) {
    $('head').append("<style>div[id$=RibbonPanel],textarea[id$=Code] { display: none; } #CodeEditor { width: 100%; height: 100%; } </style>");

    function isHtml(source) {
        // <foo> - looks like html
        // <!--\nalert('foo!');\n--> - doesn't look like html

        var trimmed = source.replace(/^[ \t\n\r]+/, '');
        var comment_mark = '<' + '!-' + '-';
        return (trimmed && (trimmed.substring(0, 1) === '<' && trimmed.substring(0, 4) !== comment_mark));
    }

    function qs(key) {
        var vars = null, hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            if (key) {
                if (hash[0] === key) {
                    return decodeURIComponent(hash[1]);
                }
            } else {
                vars = [];
                vars.push(hash[0]);
                vars[hash[0]] = decodeURIComponent(hash[1]);
            }
        }
        return vars;
    }

    $(function () {
        var code = $('textarea[id$=Code]');
        var ce = $("<div id='CodeEditor' />");
        code.after(ce);

        var codeeditor = ace.edit(ce[0]);
        codeeditor.setTheme("ace/theme/monokai");
        codeeditor.session.setMode("ace/mode/html");
        codeeditor.setShowPrintMargin(false);

        codeeditor.session.setValue(code.val().trim());
        codeeditor.session.on('change', function() {
            code.val(codeeditor.session.getValue());
        });

        ace.config.loadModule("ace/ext/emmet", function() {
            ace.require("ace/lib/net").loadScript("/sitecore/shell/Controls/Lib/ace/emmet-core/emmet.js", function() {
                codeeditor.setOption("enableEmmet", true);
            });
        });

        ace.config.loadModule("ace/ext/language_tools", function(module) {
            codeeditor.setOptions({
                enableSnippets: true,
                enableBasicAutocompletion: true
            });
        });

        var codeeeditorcommands = [{
            name: "format",
            bindKey: { win: "ctrl-shift-f", mac: "ctrl-command-enter", sender: 'codeeditor' },
            exec: function(env, args, request) {
                var source = codeeditor.session.getValue();
                if (source) {
                    var doUpdate = true;
                    var output = "";
                    switch (qs('mode')) {
                    case "html":
                        if (isHtml(source)) {
                            var opts = {};
                            opts.indent_size = '4';
                            opts.indent_char = opts.indent_size == 1 ? '\t' : ' ';
                            opts.max_preserve_newlines = '5';
                            opts.preserve_newlines = opts.max_preserve_newlines !== -1;
                            opts.keep_array_indentation = false;
                            opts.break_chained_methods = false;
                            opts.indent_scripts = 'normal';
                            opts.brace_style = 'collapse';
                            opts.space_before_conditional = true;
                            opts.unescape_strings = false;
                            opts.wrap_line_length = '0';
                            opts.space_after_anon_function = true;
                            output = html_beautify(source, opts);
                        }
                        break;
                        case "css":
                            output = css_beautify(source);
                        break;
                        case "javascript":
                            output = js_beautify(source);
                            break;
                        default:
                            doUpdate = false;
                            break;
                    }

                    if (doUpdate) {
                        codeeditor.session.setValue(output);
                        code.val(output);
                    }
                }
            },
            readOnly: true
        }, {
            name: "fontSizeIncrease",
            bindKey: {win: "Ctrl-Alt-Shift-=|Ctrl-Alt-Shift-+", mac: "Ctrl-Alt-Shift-=|Ctrl-Alt-Shift-+"},
            exec: function(editor) { 
                editor.setFontSize(Math.min(editor.getFontSize() + 1, 25)); 
            },
            readOnly: true
        }, {
            name: "fontSizeDecrease",
            bindKey: {win: "Ctrl-Alt-Shift--", mac: "Ctrl-Alt-Shift--"},
            exec: function(editor) { 
                editor.setFontSize(Math.max(editor.getFontSize() - 1, 12)); 
            },
            readOnly: true
        }];

        codeeditor.commands.addCommands(codeeeditorcommands);

        sitecore.sharedsource.codeeditor.changeMode = function(setting) {
            setting = setting || 'html';
            codeeditor.setOption("mode", "ace/mode/" + setting);
        };

        sitecore.sharedsource.codeeditor.changeMode(qs('mode'));
        
        sitecore.sharedsource.codeeditor.changeTheme = function (setting) {
            setting = setting || 'monokai';
            codeeditor.setOption("theme", "ace/theme/" + setting);
        };

        sitecore.sharedsource.codeeditor.changeTheme(qs('theme'));

        sitecore.sharedsource.codeeditor.changeFontSize = function (setting) {
            setting = parseInt(setting) || 12;
            codeeditor.setOption("fontSize", setting);
        };

        sitecore.sharedsource.codeeditor.changeFontSize(qs('fontSize'));

        sitecore.sharedsource.codeeditor.changeFontFamily = function (setting) {
            setting = setting || "Monaco";
            codeeditor.setOption("fontFamily", setting);
        };

        sitecore.sharedsource.codeeditor.changeFontFamily(qs('fontFamily'));
    });
}(jQuery, window.sitecore));