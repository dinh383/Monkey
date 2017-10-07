
var owl = {
    initToolTip: function () {
        $('[data-toggle="tooltip"]').tooltip();
    },

    initBootBox: function () {
        $('[data-plugin="confirm"]').click(function () {
            var $this = $(this);
            bootbox.dialog({
                title: $this.data("confirm-title"),
                message: $this.data("confirm-message"),
                buttons: {
                    danger: {
                        label: "Confirm",
                        className: "btn-danger",
                        callback: function () {
                            var action = $this.data("confirm-action");
                            eval(action);
                        }
                    },
                    main: {
                        label: "Cancel",
                        className: "btn-primary",
                        callback: function () { }
                    }
                }
            });
        });
    },

    initRequiredField: function () {
        $('[data-val-required!=""]').each(function (i, e) {
            var label = $("label[for='" + $(this).attr("id") + "']");
            if (label.length === 0) {
                $(e).closest(".row").find("label").each(function (idx, el) {
                    if (el.children.length === 0) {
                        $(el).append('<span style="color:#f46c93"> *</span>');
                    }
                });
            } else if (label[0].children.length === 0) {
                $(label).append('<span style="color:#f46c93"> *</span>');
            }
        });
    },

    setupAjax: function () {
        $.ajaxSetup({
            type: "POST",
            cache: false,
            error: function (xhr, textStatus, errorThrown) {
                console.log("ajax error", data);
                if (xhr.status == 401 || xhr.status == 403) {
                    window.location.href = "/Portal/Auth";
                } else if (xhr.status == 404 || xhr.status == 403) {
                    window.location.href = "/Portal";
                } else {
                    var data = JSON.parse(xhr.responseText);
                    if (data.code) {
                        owl.notify("Error", data.message, "error");
                    } else {
                        owl.notify("Error", "System error, please try again later or contact administrator!", "error");
                    }
                }
            }
        });
    },

    notify: function (title, message, type, options) {
        options = options || {};
        title = title || undefined;
        message = message || '';
        type = type || 'info';
        options.progressBar = options.progressBar || true;
        options.timeOut = options.timeOut || 5000;
        options.preventDuplicates = options.preventDuplicates || false;
        switch (type) {
        case 'success':
            toastr.success(message, title, options);
            break;
        case 'warning':
            toastr.warning(message, title, options);
            break;
        case 'error':
            toastr.error(message, title, options);
            break;
        case 'info':
            toastr.info(message, title, options);
            break;
        default:
            toastr.info(message, title, options);
        }
    }
};



$(function () {
    owl.setupAjax();
    owl.initRequiredField();
    owl.initToolTip();
    owl.initBootBox();
});