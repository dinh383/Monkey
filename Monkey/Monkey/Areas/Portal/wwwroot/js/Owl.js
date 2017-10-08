var owl = {
    initToolTip: function () {
        $('[data-toggle="tooltip"]').tooltip();
    },

    initConfirmDialog: function () {
        $('[data-plugin="confirm"]').click(function () {
            var $this = $(this);

            swal({
                title: $this.data("confirm-title"),
                text: $this.data("confirm-message"),
                type: "warning",
                showCancelButton: true,
                confirmButtonClass: "btn-pink",
                confirmButtonText: 'Yes',
                cancelButtonText: "No",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        var action = $this.data("confirm-yes-callback");
                        eval(action);
                        swal($this.data("confirm-yes-title") || "Deleted!", $this.data("confirm-yes-message") || "Delete Successful", "success");
                    } else {
                        swal($this.data("confirm-no-title") || "Cancelled", $this.data("confirm-no-message"), "error");
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
            headers: { 'X-XSRF-TOKEN': $('[name=ape]').val() },
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
    },

    initSlidePanel: function () {
        $('[data-toggle="slidePanel"]').click(function() {
            var $this = $(this);
            $.slidePanel.show({
                    url: $this.data("url"),
                    settings: {
                        method: 'GET',
                        cache: false
                    }
                },
                // Option
                {
                    direction: 'right',
                });
        });
    }
};

$(function () {
    owl.setupAjax();
    owl.initRequiredField();
    owl.initToolTip();
    owl.initConfirmDialog();
    owl.initSlidePanel();
});