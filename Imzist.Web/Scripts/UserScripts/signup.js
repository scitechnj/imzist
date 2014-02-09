$(function () {
    var $pass = $("#pass");
    var $conf = $("#conf");
    var $email = $("#email");
    var $submit = $("#submit");
    $email.on("keyup", function () {
        if (IsEmail($email)) {
            $email.addClass("inputSuccess3");
            $email.removeClass("inputError3");
        } else {
            $email.removeClass("inputSuccess3");
            $email.addClass("inputError3");
        }


    });
    $($conf).on("keyup", function () {
        if (PassMatch($pass, $conf)) {
            $conf.addClass("inputSuccess3");
            $conf.removeClass("inputError3");
        } else {
            $conf.removeClass("inputSuccess3");
            $conf.addClass("inputError3");
        }
    });
    $("input").on("keyup", function () {
        if (PassMatch($pass, $conf) && IsEmail($email)) {
            $.post("/account/UserNameExists", { username: $email.val() }, function (result) {
                if (result.exists == "false") {
                    $submit.removeAttr("disabled");
                } else {
                    //add error message...
                }
            });
        }
    });
});
function IsEmail(email) {
    var regex = /^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$/;
    return regex.test(email.val());
}
function PassMatch($pass, $conf) {
    if ($pass.val() != "" && $pass.val() == $conf.val()) {
        return true;
    }
    return false;
}