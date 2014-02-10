$(function () {
    var $pass = $("#pass");
    var $conf = $("#conf");
    var $email = $("#email");
    var $submit = $("#submit");

    $email.on("keyup mouseleave", function () {

        ValidEmail($email, function(isValid) {
            if (isValid) {
                ValidationSuccess($email);
            } else {
                ValidationError($email);
            }
        });
    });
    
    $pass.on("keyup mouseleave", function () {
        if (ValidPassword($pass)) {
            ValidationSuccess($pass);
            $conf.removeAttr("disabled");
        } else {
            ValidationError($pass);
            $conf.attr("disabled", "disabled");
        }
    });
    $($conf).on("keyup mouseleave", function () {
        if (PassMatch($pass, $conf)) {
            ValidationSuccess($conf);
        } else {
            ValidationError($conf);
        }
    });
    $("input").on("mouseleave", function () {

        ValidEmail($email, function(isValid) {
            if (!isValid) {
                $submit.attr("disabled", "disabled");
            } else {
                if (PassMatch($pass, $conf)) {
                    $submit.removeAttr("disabled");
                } else {
                    $submit.attr("disabled", "disabled");
                }
            }
        });
    });
});
function ValidPassword($element) {
    var regex = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,10}$/;
    return regex.test($element.val());
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
            //return result.exists === "false";
            callback(!result.exists);
        });
    }
    

}
