
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
    }
};

$(function() {
    owl.initToolTip();
    owl.initBootBox();
});