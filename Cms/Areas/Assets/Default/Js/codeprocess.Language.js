var lang = {
    notify: {
        PleaseSelectAddress: "PleaseSelectAddress",
        BasketIsEmpty: "BasketIsEmpty",
        PleaseSelectBank: "PleaseSelectBank",
        PleaseRequired:"PleaseRequired"
    }
};

function GetNotifyMsg(type,name) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.Language.name + "?name=" + name;
    request.success = function (result) {
        createMessage(type, result.Value);
    };
    $.ajax(request);

}
