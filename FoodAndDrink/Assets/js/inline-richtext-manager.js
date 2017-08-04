var InlineRichtextManager = function () {
    var quillOpts = {
        placeholder: 'Enter the value...',
        theme: 'bubble'
    }

    var editors = [];
    var umbApi = '/umbraco/backoffice/api';

    $('.inline-richtext-editor').each(function () {

        var $editor = $(this),
            $toolbar = $editor.find('.editor-toolbar'),
            $pen = $editor.find('.editor-pen'),
            $message = $editor.find('.editor-message'),
            $wrapper = $editor.find('.editor-wrapper'),
            $icons = $editor.find('i'),
            $slick = $editor.closest('.slides');

        var nodeId = $editor.data('node');
        var editorId = $editor.data('editorId');
        var customWrapperClass = $editor.data('customWrapperClass');
        var delta;

        $editor.find('.editor-pen').click(function () {
            if (editors[editorId]) {
                editors[editorId].enable();
            }
            else {
                var quill = new Quill($editor.find('.editor')[0], quillOpts);
                editors[editorId] = quill;
            }

            $toolbar.show();
            $pen.removeClass('on');
            $wrapper.addClass('on');
            if (customWrapperClass) {
                $wrapper.addClass(customWrapperClass);
            }

            if ($slick.length > 0) {
                $slick.slickPause();
                $slick.slick('slickPause'); //1.4
            }

            delta = editors[editorId].getContents();
        });

        $toolbar.find('.cancel').click(function () {
            editors[editorId].disable();
            $toolbar.removeClass('error success').hide();
            $pen.addClass('on');
            $wrapper.removeClass('on');
            $message.text('');
            if ($slick.length > 0) {
                $slick.slickPlay();
                $slick.slick('slickPlay'); //1.4
            }

            editors[editorId].setContents(delta);
        });

        $toolbar.find('.save').click(function () {
            var data = {
                NodeId: parseInt(nodeId, 10),
                PropertyAlias: $editor.data('prop'),
                Html: editors[editorId].root.innerHTML
            };

            $message.text('Saving...').show();
            $toolbar.removeClass('error success');

            $.post(umbApi + $editor.data('api'), data, function (result) {
                if (result.Success) {
                    $toolbar.addClass('success');
                    $message.text('The content has been saved.').show();
                    delta = editors[editorId].getContents();
                }
                else {
                    $toolbar.addClass('error');
                    $message.text(result.AllMessages).show();
                }
            }, 'json')
            .fail(function (response) {
                var res = JSON.parse(response.responseText);
                $toolbar.addClass('error');
                $message.text(response.statusText + ((res.Message) ? (': ' + res.Message) : '')).show();
            });
        });
    });
};