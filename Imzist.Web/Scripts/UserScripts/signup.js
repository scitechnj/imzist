$(function () {
    var $pass = $("#pass");
    var $conf = $("#conf");
    var $email = $("#email");
    var $submit = $("#submit");

    $email.on("blur", function () {

        ValidEmail($email, function(isValid) {
            if (isValid) {
                ValidationSuccess($email);
            } else {
                ValidationError($email);
            }
        });
    });
    $email.on("keyup", function () {

        ValidEmail($email, function (isValid) {
            if (isValid) {
                ValidationSuccess($email);
            } 
        });
    });
    $pass.on("blur", function () {
        if (ValidPassword($pass)) {
            ValidationSuccess($pass);
            $conf.removeAttr("disabled");
        } else {
            ValidationError($pass);
            $conf.attr("disabled", "disabled");
        }
    });
    $pass.on("keyup", function () {
        if (ValidPassword($pass)) {
            ValidationSuccess($pass);
        } 
    });
    $($conf).on("blur", function () {
        if (PassMatch($pass, $conf)) {
            ValidationSuccess($conf);
        } else {
            ValidationError($conf);
        }
    });
    $($conf).on("keyup", function () {
        if (PassMatch($pass, $conf)) {
            ValidationSuccess($conf);
        } 
    });
    $submit.on("click", function () {
        ValidEmail($email, function(isValid) {
            if (isValid && PassMatch($pass,$conf)) {
                $('form').submit();
            } 
        });
    });
});
function ValidPassword($element) {
    return $element.val() != '';
}
function PassMatch($pass, $conf) {
    if ($pass.val() !== "" && $pass.val() == $conf.val()) {
        return true;
    }
    return false;
}
function ValidationError($element) {
    $element.addClass("valid-error");
    $element.removeClass("valid-success");
}
function ValidationSuccess($element) {
    $element.addClass("valid-success");
    $element.removeClass("valid-error");
}
function ValidEmail($email, callback) {
    var regex = /^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$/;
    if (regex.test($email.val())) {
        $.post("/account/UserNameExists", { username: $email.val() }, function (result) {
            callback(!result.exists);
        });
    }
    callback(false);
}
