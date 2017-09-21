jQuery.validator.setDefaults({
    highlight: function (element, errorClass, validClass) {
        $(element).parent("div").addClass("has-danger").removeClass("has-success");
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).parent("div").removeClass("has-danger").addClass("has-success");
    }
});