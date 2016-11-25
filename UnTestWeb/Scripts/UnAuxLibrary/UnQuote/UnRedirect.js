var UnRedirect = {
    back: function (is) {
        if (is) {
            location.href = document.referrer;
        }
        else {
            history.back();
        }
    },
    email: function (e) {
        var domain = e.substr(email.indexOf('@') + 1);
        var url = 'http://mail.' + domain + '/';
        location.href = url;
    }
}
