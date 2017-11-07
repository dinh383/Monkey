var monkey = {
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

    setupAjax: function () {
        $.ajaxSetup({
            headers: { 'X-XSRF-TOKEN': $('[name=ape]').val() },
            type: "POST",
            cache: false,
            error: function (xhr, textStatus, errorThrown) {
                console.log("[Request Error]", xhr);
                if (xhr.status === 401 || xhr.status === 403) {
                    window.location.href = "/Portal/Auth";
                } else if (xhr.status === 404) {
                    window.location.href = "/Portal";
                } else {
                    try {
                        var data = JSON.parse(xhr.responseText);
                        if (data.code) {
                            monkey.notify("Error", data.message, "error");
                        } else {
                            monkey.notify("Error", "System error, please try again or contact administrator!", "error");
                        }
                    } catch (e) {
                        monkey.notify("Error", "System error, please try again or contact administrator!", "error");
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
        options.positionClass = options.positionClass || "toast-bottom-right";

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
        $('[data-toggle="slidePanel"]').click(function () {
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
    },

    renderSlugAuto: function ($element, $slugElement) {
        $element.on("keyup",
            function () {
                $slugElement.val($element.val().genSlug());
            });
    },

    notificationHub: {
        connection: null,
        connect: function () {
            monkey.notificationHub.connection = new signalR.HubConnection("/portal/notification");

            monkey.notificationHub.connection
                .on("notification",
                    notification => {
                        // Handle notification on client side
                        console.log(notification.message);
                    });

            monkey.notificationHub.connection
                .start()
                .then(() => {
                    console.log("[Socket] connected to notification hub");
                })
                .catch(err => {
                    console.log(`[Socket] connection error: ${err}`);
                });
        },
        sendUsers: function (notification, subjects) {
            monkey.notificationHub.connection.invoke("notificationUsersAsync", notification, subjects);
        },
        sendPermissions: function (notification, permissions) {
            monkey.notificationHub.connection.invoke("notificationPermissionsAsync", notification, permissions);
        }
    },

    abbreviateNumber: function (number) {
        // what tier? (determines SI prefix)
        var tier = Math.log10(number) / 3 | 0;

        // if zero, we don't need a prefix
        if (tier == 0) return number;

        // get postfix and determine scale
        var postFix = ["", " K", " M", " G", " T", " P", " E"][tier];

        var scale = Math.pow(10, tier * 3);

        // scale the number
        var scaled = number / scale;

        // format number and add postfix as suffix
        return scaled.toFixed(1) + postFix;
    }
};

$(function () {
    monkey.setupAjax();
    monkey.initToolTip();
    monkey.initConfirmDialog();
    monkey.initSlidePanel();
    monkey.notificationHub.connect();
});

String.prototype.preventInjection = function preventInjection() {
    return this.replace(/</g, "&lt;").replace(/>/g, "&gt;");
};

String.prototype.genSlug = function changeToSlug() {
    var title, slug;
    title = this;

    slug = title.toLowerCase();

    slug = slug.replace(/á|à|ả|ạ|ã|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ/gi, "a");
    slug = slug.replace(/é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ/gi, "e");
    slug = slug.replace(/i|í|ì|ỉ|ĩ|ị/gi, "i");
    slug = slug.replace(/ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ/gi, "o");
    slug = slug.replace(/ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự/gi, "u");
    slug = slug.replace(/ý|ỳ|ỷ|ỹ|ỵ/gi, "y");
    slug = slug.replace(/đ/gi, "d");

    slug = slug.replace(/c#/gi, "c-sharp");

    slug = slug.replace(/\`|\~|\!|\@|\#|\||\$|\%|\^|\&|\*|\(|\)|\+|\=|\,|\.|\/|\?|\>|\<|\'|\"|\:|\;|_/gi, "");

    slug = slug.replace(/ /gi, "-");

    slug = slug.replace(/\-\-\-\-\-/gi, "-");
    slug = slug.replace(/\-\-\-\-/gi, "-");
    slug = slug.replace(/\-\-\-/gi, "-");
    slug = slug.replace(/\-\-/gi, "-");

    slug = "@" + slug + "@";
    slug = slug.replace(/\@\-|\-\@|\@/gi, "");

    return slug;
};

Array.prototype.remove = function () {
    var what, a = arguments, L = a.length, ax;
    while (L && this.length) {
        what = a[--L];
        while ((ax = this.indexOf(what)) !== -1) {
            this.splice(ax, 1);
        }
    }
    return this;
};

Array.prototype.contains = function (element) {
    return this.indexOf(element) > -1;
};