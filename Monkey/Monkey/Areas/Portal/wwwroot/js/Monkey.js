'use strict';

window.Monkey = {
    isDebug: true,

    constant: {
        momentDateTimeFormat: 'DD/MM/YYYY hh:mm:ss A',
        momentDateFormat: 'DD/MM/YYYY'
    },

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
                        swal({
                            title: $this.data("confirm-yes-title") || "Deleted!",
                            text: $this.data("confirm-yes-message") || "Delete Successful",
                            type: "success",
                            timer: 2000
                        });
                    } else {
                        swal({
                            title: $this.data("confirm-no-title") || "Canceled",
                            text: $this.data("confirm-no-message"),
                            type: "error",
                            timer: 2000
                        });
                    }
                });
        });
    },

    setupAjax: function () {
        $.ajaxSetup({
            headers: { 'X-XSRF-TOKEN': $("[name=ape]").val() },
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
                        // Check request already abort => return
                        if (xhr.status === 0) {
                            return;
                        }

                        var data = JSON.parse(xhr.responseText);
                        if (data.code) {
                            window.Monkey.notify("Error", data.message, "error");
                        } else {
                            window.Monkey.notify("Error", "System error, please try again or contact administrator!", "error");
                        }
                    } catch (e) {
                        window.Monkey.notify("Error", "System error, please try again or contact administrator!", "error");
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
                    method: "GET",
                    cache: false
                }
            },
                // Option
                {
                    direction: 'right'
                });
        });
    },

    initAutoRenderSlug: function () {
        $('[data-bind-type="slug"]').on("keyup blur", function () {
            var $this = $(this);

            var $slugElement = $($this.data("bind-to"));

            $slugElement.val($this.val().genSlug());
        });
    },

    notificationHub: {
        connection: null,
        connect: function () {
            window.Monkey.notificationHub.connection = new signalR.HubConnection("/hub/portal/notification");

            window.Monkey.notificationHub.connection.on("refreshNotification", function () {
                window.Monkey.notificationHub.refreshNotification();
                return;
            });

            window.Monkey.notificationHub.connection.on("setNotification", function (notification) {
                window.Monkey.notificationHub.setItems(notification);
                return;
            });

            window.Monkey.notificationHub.connection
                .start()
                .then(function () {
                    if (Monkey.isDebug === true) {
                        console.log("[Socket] connected to notification hub");
                    }
                    window.Monkey.notificationHub.refreshNotification();
                    return;
                }).catch(function () {
                    if (Monkey.isDebug === true) {
                        console.log('[Socket] connection error:' + err);
                    }
                    return;
                });
        },
        refreshNotification: function () {
            window.Monkey.notificationHub.connection.invoke("GetAllAsync");
            if (Monkey.isDebug === true) {
                console.log("refresh notification");
            }
        },
        setTotal: function (number) {
            var int = parseInt(number);

            if (int === NaN || int < 0) {
                return;
            }

            $(".notification-total").html(int);

            if (int <= 0) {
                $(".notification-total").parent().css("visibility", "hidden").css("display", "none");
            } else {
                $(".notification-total").parent().css("visibility", "").css("display", "");
            }
        },
        updateHeight: function () {
            var listHeight = 0;

            // Just show last 5 items
            for (var i = 0; i < 5; i++) {
                if ($(".notification-item").length <= i) {
                    break;
                }

                listHeight += $($(".notification-item")[i]).height();
            }

            $("#notification-list").parent().height(listHeight);

            var parentsScrollable = $("#notification-list").parents(".list-group");
            if (parentsScrollable && parentsScrollable.data('asScrollable')) {
                parentsScrollable.data('asScrollable').update();
            }
        },
        onOpenNotification: function () {
            setTimeout(function () {
                window.Monkey.notificationHub.updateHeight();

                window.Monkey.notificationHub.maskAllAsRead();
            }, 10);
        },
        getItemHtml: function (itemData) {
            if (!itemData) {
                return "";
            }

            var iconColorClass;

            if (itemData.type === "Success") {
                iconColorClass = "bg-green-600";
            }
            else if (itemData.type === "Warning") {
                iconColorClass = "bg-orange-600";
            } else if (itemData.type === "Error") {
                iconColorClass = "bg-red-600";
            }
            else if (itemData.type === "Critical") {
                iconColorClass = "bg-purple-600";
            } else {
                iconColorClass = "bg-blue-600";
            }

            var html =
                '<a class="list-group-item dropdown-item notification-item" href="' + itemData.url + '" role="menuitem">'
                + '<div class="media">'
                + '    <div class="pr-10">'
                + '        <i class="icon md-receipt ' + iconColorClass + ' white icon-circle verical-icon-center" aria-hidden="true"></i>'
                + '    </div>'
                + '    <div class="media-body">'
                + '       <h6 class="media-heading notification-title">' + itemData.message + '</h6>'
                + '      <time class="media-meta" datetime="${itemData.createdTime}">' + window.Monkey.formatDateTime(itemData.createdTime) + '</time>'
                + '    </div>'
                + '</div>'
                + '</a>';

            return html;
        },
        data: {},
        setItems: function (notification) {
            window.Monkey.notificationHub.data = notification;

            $("#notification-list").empty();

            for (var i = 0; i < notification.items.length; i++) {
                var html = window.Monkey.notificationHub.getItemHtml(notification.items[i]);

                $("#notification-list").append($(html));
            }

            window.Monkey.notificationHub.setTotal(notification.TotalUnRead);

            window.Monkey.notificationHub.updateHeight();
        },
        maskAllAsRead: function () {
            var endpoint = window.Endpoint.notification.maskAllAsReadEndpoint;
            $.ajax({
                url: endpoint,
                method: "PUT",
                contentType: "text/plain",
                success: function () {
                    window.Monkey.notificationHub.setTotal(0);
                }
            });

            console.log(endpoint);
        }
    },

    abbreviateNumber: function (number) {
        // what tier? (determines SI prefix)
        var tier = Math.log10(number) / 3 | 0;

        // if zero, we don't need a prefix
        if (tier == 0) return number;

        // get postfix and determine scale
        var postFix = ["", "k", "m", "g", "t", "p", "e"][tier];

        var scale = Math.pow(10, tier * 3);

        // scale the number
        var scaled = number / scale;

        // format number and add postfix as suffix
        return scaled.toFixed(1) + postFix;
    },

    decodeHtml: function (str) {
        return $("<div/>").html(str).text();
    },

    openLink: function (link, isNewTab) {
        if (isNewTab && isNewTab === true) {
            var win = window.open(link, "_blank");
            win.focus();
        } else {
            window.open(link, "_self");
        }
    },

    addListener_Keyboard_Enter: function (selectorsSource, elementDestination, action) {
        /// <summary>
        ///     Add listener when element source press enter make element destination fire a action
        /// </summary>
        /// <param name="selectorsSource" type="type">seperate "," multiple selector: ".class1,#element1"</param>
        /// <param name="elementDestination" type="type">destination selector</param>
        /// <param name="action" type="type">action: "click", "dbclick" and so on</param>

        var elements = selectorsSource.split(",");
        $.each(elements,
            function (index, element) {
                $(element).keydown(function (e) {
                    if (e.which === 13) {
                        $(elementDestination).trigger(action);
                    }
                });
            });
    },

    formatDateTime: function (dateTime) {
        return moment(dateTime).format(window.Monkey.constant.momentDateTimeFormat);
    },

    formatDate: function (dateTime) {
        return moment(dateTime).format(window.Monkey.constant.momentDateFormat);
    },

    rgbToHex: function (r, g, b) {
        return "#" + ((1 << 24) + (r << 16) + (g << 8) + b).toString(16).slice(1);
    },

    initAutoBindColorDominant: function () {
        $('[data-bind-type="color-dominant"]').change(function () {
            var $this = $(this);

            var colorSelector = $this.data("bind-to");

            window.Monkey.bindColorDominant(this, $(colorSelector));
        });
    },

    autoBindColorDominant: function (fileSelector, colorSelector) {
        $(fileSelector).change(
            function () {
                window.Monkey.bindColorDominant(this, $(colorSelector));
            });
    },

    bindColorDominant: function ($fileElement, $colorElement) {
        var fileReader = new FileReader();

        fileReader.onload = function (fileLoadedEvent) {
            var img = new Image();

            img.onload = function () {
                var colorThief = new ColorThief();

                var color = colorThief.getColor(img);

                var hexColor = window.Monkey.rgbToHex(color[0], color[1], color[2]);

                $colorElement.asColorPicker('val', hexColor);
            };

            var base64 = fileLoadedEvent.target.result;

            img.src = base64;
        }
        fileReader.readAsDataURL($fileElement.files[0]);
    },

    renderGoogleMap: function (divMapSelector, $address, $lat, $lng) {
        var map = new window.GMaps({
            div: divMapSelector,
            lat: $lat.val(),
            lng: $lng.val()
        });

        var marker;

        var infoWindow = new google.maps.InfoWindow({});

        window.GMaps.geocode({
            address: $address.val().trim(),

            callback: function (results, status) {
                if (status === 'OK') {
                    var data = results[0];

                    infoWindow.setContent(data.formatted_address);
                    $address.val(data.formatted_address);

                    var location = data.geometry.location;

                    $lat.val(location.lat());

                    $lng.val(location.lng());

                    map.setCenter(location.lat(), location.lng());

                    var markerOptions = {
                        lat: location.lat(),
                        lng: location.lng(),
                        draggable: true,
                        animation: google.maps.Animation.DROP
                    };

                    marker = map.addMarker(markerOptions);

                    google.maps.event.addListener(marker,
                        'dragend',
                        function (event) {
                            $lat.val(event.latLng.lat());

                            $lng.val(event.latLng.lng());

                            window.GMaps.geocode({
                                latLng: marker.getPosition(),

                                callback: function (results) {
                                    data = results[0];
                                    infoWindow.setContent(data.formatted_address);
                                    infoWindow.open(map, marker);
                                    $address.val(data.formatted_address);
                                }
                            });
                        });

                    infoWindow.open(map, marker);
                } else {
                    $address.val("");

                    $lat.val("");

                    $lng.val("");
                }
            }
        });
    },

    initDropify: function () {
        var $dropifys = $('[data-plugin="dropify"]');

        $.each($dropifys, function (i, ele) {
            var dropifyEvent = $(ele).dropify();

            var dropifyData = dropifyEvent.data('dropify');

            dropifyData.destroy();

            dropifyData.settings.messages = {
                'default': $(ele).data("message-default") || 'Drag and drop a file here or click',
                'replace': $(ele).data("message-error") || 'Drag and drop or click to replace',
                'remove': $(ele).data("message-remove") || 'Remove',
                'error': $(ele).data("message-replace") || 'Ooops, something wrong happended.'
            }

            dropifyData.settings.tpl = {
                wrap: '<div class="dropify-wrapper"></div>',
                loader: '<div class="dropify-loader"></div>',
                message: '<div class="dropify-message"><span class="file-icon" /> <p>' + dropifyData.settings.messages.default + '</p></div>',
                preview: '<div class="dropify-preview"><span class="dropify-render"></span><div class="dropify-infos"><div class="dropify-infos-inner"><p class="dropify-infos-message">' + dropifyData.settings.messages.replace + '</p></div></div></div>',
                filename: '<p class="dropify-filename"><span class="dropify-filename-inner"></span></p>',
                clearButton: '<button type="button" class="dropify-clear">' + dropifyData.settings.messages.remove + '</button>',
                errorLine: '<p class="dropify-error">' + dropifyData.settings.messages.error + '</p>',
                errorsContainer: '<div class="dropify-errors-container"><ul></ul></div>'
            }

            dropifyData.init();
        });
    },

    // Remove dropify image (preview image) and mark is remove previous for the selector
    clearDropify: function (dropifySelector, isRemovePreviousSelector, previewUrlSelector, callback) {
        debugger;

        var dropifyEvent = $(dropifySelector).dropify();

        var dropifyData = dropifyEvent.data('dropify');

        dropifyData.resetPreview();

        dropifyData.clearElement();

        if (previewUrlSelector && previewUrlSelector.length > 0) {
            $(previewUrlSelector).val("");
        }

        var colorSelector = $(dropifySelector).data("bind-to");

        if (colorSelector && colorSelector.length > 0) {
            $(colorSelector).val("");
        }

        if (isRemovePreviousSelector) {
            $(isRemovePreviousSelector).val(true);
        }

        if (callback && typeof callback === "function") {
            callback();
        }
    },

    initAutoDecimalFormat: function () {
        var $amounts = $('[data-plugin="decimal-format"]');

        $.each($amounts, function (i, ele) {
            $(ele).mask("#,##0.00", { reverse: true });
        });
    },

    initDateTimePicker: function () {
        var $dateTimePickers = $('[data-plugin="datetime-picker"]');

        $.each($dateTimePickers, function (i, ele) {
            $(ele).datetimepicker({
                format: window.Monkey.constant.momentDateTimeFormat
            });
        });
    },

    initTimePicker: function () {
        var $dateTimePickers = $('[data-plugin="time-picker"]');

        $.each($dateTimePickers, function (i, ele) {
            $(ele).datetimepicker({
                format: window.Monkey.constant.momentTimeFormat
            });
        });
    },

    dataTableAmountRender: function (data, type, row) {
        return window.Monkey.abbreviateNumber(data);
    },

    dataTableImageRender: function (data, type, row) {
        if (data && data.trim().length > 0) {
            return "<img class='menu-img' src=" + data + " />";
        }
        return "";
    },

    dataTableDrawCallBack: function (oSettings) {
        window.Monkey.initToolTip();
        window.Monkey.initConfirmDialog();
    },

    dataTableResponsiveCallBack: function (e, datatable, columns) {
        var count = columns.reduce(function (a, b) {
            return b === false ? a + 1 : a;
        }, 0);

        var $table = $("#" + e.currentTarget.id);

        var totalHeaderRow = $table.find("thead").find("tr");

        var lastHeaderRow = totalHeaderRow[totalHeaderRow.length - 1];

        if (count > 0) {
            $(lastHeaderRow).addClass("hidden");
        } else {
            $(lastHeaderRow).removeClass("hidden");
        }
    },

    resetFormValidation: function (formSelector) {
        formSelector = formSelector || $("form")[0];

        event.preventDefault();

        var $form = $(formSelector);

        $form.validate().resetForm();

        $form.valid();
    },

    initAutoResetFormValidation: function () {
        var $resetFormValidations = $('[data-reset-form-validation="true"]');

        $.each($resetFormValidations, function (i, ele) {
            $(ele).on('keyup blur', function () {
                var $form = $($(ele).closest("form")[0]);

                var formSelector = $form.attr('id') || $form.attr('class');

                window.Monkey.resetFormValidation(formSelector);
            });
        });
    },

    GenerateGuid: function () {
        return window.Monkey.s4() + window.Monkey.s4() + '-' + window.Monkey.s4() + '-' + window.Monkey.s4() + '-' + window.Monkey.s4() + '-' + window.Monkey.s4() + window.Monkey.s4() + window.Monkey.s4();
    },

    s4: function () {
        return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    },

    initialMenubar: function () {
        if (typeof (Storage) !== "undefined") {
            $("#toggleMenubar").on("click", function () {
                var isCollapsed = $("body").hasClass("site-menubar-fold");

                if (isCollapsed === true) {
                    localStorage.setItem("menu-bar-class", "site-menubar-unfold");
                } else {
                    localStorage.setItem("menu-bar-class", "site-menubar-fold");
                }
            });

            if (localStorage.getItem("menu-bar-class")) {
                $("body").removeClass("site-menubar-fold");

                $("body").removeClass("site-menubar-unfold");

                $("body").addClass(localStorage.getItem("menu-bar-class"));
            }
        } else {
            // Do Nothing
        }
    }
};

$(document).ready(function () {
    window.Monkey.setupAjax();
    window.Monkey.initToolTip();
    window.Monkey.initConfirmDialog();
    window.Monkey.initSlidePanel();
    window.Monkey.notificationHub.connect();
    window.Monkey.initAutoRenderSlug();
    window.Monkey.initAutoBindColorDominant();
    window.Monkey.initAutoDecimalFormat();
    window.Monkey.initDateTimePicker();
    window.Monkey.initTimePicker();
    window.Monkey.initDropify();
    window.Monkey.initAutoResetFormValidation();
    window.Monkey.initialMenubar();
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