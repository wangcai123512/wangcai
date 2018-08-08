
$.extend($.fn.validatebox.defaults.rules, {
    spChar: {
        validator: function (value, param) {
            var reg = /\\/;
            return reg.exec(value) >= 0;
        },
        message: 'Please enter a valid Value.'
    }
});

$.extend($.fn.validatebox.defaults.rules, {
    pwd: {
        validator: function (value, param) {
            var reg = /^(?![0-9]+$)(?![A-Za-z]+$)[^\s]{6,}$/;
            return reg.test(value);
        },
        message: 'Password length of at least 6, and contain both letters and numbers.'
    }
});


