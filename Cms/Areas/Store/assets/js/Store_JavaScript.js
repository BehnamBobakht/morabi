function stateDropShipping() {
    var selectedstateID = $("#selectedStateShipping").val();
    GetCityByStateIdShipping(selectedstateID);
}
function GetCityByStateIdShipping(stateId) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.city.name + "?isShipping=true&id=" + stateId;
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    request.success = function (result) {
        viewModel.City.CityList(result.Value);
    };
    $.ajax(request);
}
function AddShippingCity() {
    var isvalid = true;
    var stateId = $("#selectedStateShipping").val();
    var cityId = $("#selectedCityShipping").val();
    var countryId = $("#selectedCountryShipping").val();
    var sendTime = $("#sendTime").val();
    var sendPrice = $("#sendPrice").val();

    if (countryId == "-1") {
        alert("لطفا یک کشور را انتخاب نمایید");
        isvalid = false;

    }
    if (sendTime == "" || sendPrice == "") {
        alert("ورود هزینه و زمان ارسال اجباری می باشد");
        isvalid = false;

    }
    if (stateId == "-1") {
        alert("لطفا یک استان را انتخاب نمایید");
        isvalid = false;

    }

    if (isvalid == true) {
        ajaxLoader(true);

        var request = createRequest();
        request.type = apirequest.type.get;
        request.url = request.url + codeprocess.api.shippingCity.name + "?stateId=" + stateId + "&cityId=" + cityId + "&countryId=" + countryId + "&sendPrice=" + sendPrice + "&sendTime=" + sendTime;
        request.success = function (result) {
            if (result.Code === apirequest.status.Success) {

                $("#sendTime").val("");
                $("#sendPrice").val("");
                $("#selectedStateShipping").val("-1");
                $("#selectedCityShipping").val("-1");
                doGetListShippingCity();
                doGetCountryShipping();
                ajaxLoader(false);
            }
        };
        $.ajax(request);
    }

}
function GetStateByCountryIdShipping(countryId) {
    viewModel.State.StateList([]);
    var request = createRequest();
    request.url = request.url + codeprocess.api.state.name + "?countryId=" + countryId;

    request.success = function (result) {
        viewModel.State.StateList(result.Value);

    };
    $.ajax(request);
}
function doGetCountryShipping(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.country.name + "?isShipping=true";
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.Country.CountryList(result.Value);
            viewModel.Country.CountryId("118");
            $("#selectedCountryShipping").val("118");
            GetStateByCountryIdShipping("118");
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function doGetListShippingCity(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.shippingCity.name;
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.Shipping.List(result.Value);
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function RemoveShippingCity(shippingid) {
    var request = createRequest();
    request.type = apirequest.type.get;

    request.url = request.url + codeprocess.api.shippingCity.name + "?id=" + shippingid;
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            doGetListShippingCity();
        }
    };
    $.ajax(request);
}
function initializeStateCityScript() {
    $("#StateId").change(function () {
        $.ajax({
            type: "Get",
            url: "/store/statecity/GetCitiesDD",
            data: { StateId: $(this).val() }
        }).done(function (res) {
            $(".cities-div").html(res);
        });


    })
}
