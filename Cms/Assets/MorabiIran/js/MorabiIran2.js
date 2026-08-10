

function initializeCommonScript() {
    ;
    $(document).ready(function () {
        initializeHeaderSearchScripts();
        //doGetCountryLocation();
        doGetCurrentAccount();
        //doGetStoreBasket();
        doGetProductLikes();
        doGetAccountAddresses();
        $(".show-register-modal").click(function () {
            $("#modal-login").modal("hide");
            $("#modal-register").modal("show");
        });
        $(".show-login-modal").click(function () {
            $("#modal-login").modal("show");
            $("#modal-register").modal("hide");
        });
    });
}
function deliveryEndProssess() {
    reLoadMerchant();
    endProssess();
}
function reLoadMerchant() {
    $(".merchant-form").submit();
}
function reLoadConfirmCart() {
    $(".confirm-cart").submit();
}
function beginProssess() {

    $(".content-div").css('opacity', '0.5');
    $(".btn-cart").addClass('pe-none');
}
function initializeOrderSearchScripts() {

    var searchForm = $("#searchForm");

    $("input[name='searchorder']").change(function () {
        $("input[name='aoStatus']").val($("input[name='searchorder']:checked").val());
        $(searchForm).submit();
    });
}
function beginProssess() {
    $(".content-div").css('opacity', '0.5');
}
function endProssess() {
    $(".content-div").css('opacity', '1');
}


function initializeHeaderSearchScripts() {

    $("#txtHeaderSearch").keyup(function () {
        var searchValue = $(this).val().trim();

        if (searchValue.length > 2) {

            viewModel.SearchResult.Name(searchValue);
            viewModel.SearchResult.isService(false);
            viewModel.SearchResult.PageSize(5);

            doSearchProduct();

        } else {
            viewModel.SearchResult.Name("");
            viewModel.SearchResult.Results.removeAll();
        }
    });
}
function startAddToBasket() {

    $(".btn-cart").addClass('pe-none');
}
function endAddToBasket() {
	
	if($("#error").val()!=1 && $("#error").val()!="1"){
    $("#go-to-cart").modal("show");
	}
    $(".btn-cart").removeClass('pe-none');

}
function endAddToBasket2() {
    location.replace("/cart/address");

}
function endAddToBasket4() {
    location.replace("/cart");

}
function initializeProductPageScript() {
    $(document).on("click", ".add-subtract .increase-count", function () {
        var stockCount = $(this).attr("stock-count");
        var stockCountInt = parseInt(stockCount);
        var currentCount = $(".add-subtract #add-count").html();

        if (currentCount != undefined) {
            var currentCountInt = parseInt(currentCount);
            currentCountInt = currentCountInt + 1;
            if (currentCountInt <= stockCountInt) {
                $(".add-subtract #add-count").html(currentCountInt);


                var addtocartBtn = $("#addtocart-btn");
                var newUrl = addtocartBtn.attr("sample-href") + currentCountInt;
                addtocartBtn.attr("href", newUrl);
            }
        }
    });
    $(document).on("click", ".add-subtract .decrease-count", function () {
        var currentCount = $(".add-subtract #add-count").html();
        if (currentCount != undefined) {
            var currentCountInt = parseInt(currentCount);
            currentCountInt = currentCountInt - 1;
            if (currentCountInt > 0) {
                $(".add-subtract #add-count").html(currentCountInt);
                var addtocartBtn = $("#addtocart-btn");
                var newUrl = addtocartBtn.attr("sample-href") + currentCountInt;
                addtocartBtn.attr("href", newUrl);
            }
        }
    });
    $(document).on("change", ".add-subtract #add-count", function () {
        var currentCount = $(".add-subtract #add-count").val();
        if (currentCount != undefined) {
            var currentCountInt = parseInt(currentCount);
            if (currentCountInt > 0) {
                $(".add-subtract #add-count").val(currentCountInt);
                var addtocartBtn = $("#addtocart-btn");
                var newUrl = addtocartBtn.attr("sample-href") + currentCountInt;
                addtocartBtn.attr("href", newUrl);
            }
        }
    });
    //    $(".master-form").submit()
}

function initializeSearchPageScript() {

    var $range = $(".js-range-slider"),
        $inputFrom = $(".js-input-from"),
        $inputTo = $(".js-input-to"),
        instance,
        min = 0,
        max = $(".range-slider").attr("max-price"),
        from = 0,
        to = 0;

    $range.ionRangeSlider({
        type: "double",
        min: min,
        max: max,
        from: $("[name=pricefrom]").val(),
        to: $("[name=priceto]").val(),
        prefix: 'تومان',
        onStart: updateInputs,
        onChange: updateInputs,
        step: 5000,
        prettify_enabled: true,
        prettify_separator: ".",
        values_separator: " - ",
        force_edges: true
    });

    instance = $range.data("ionRangeSlider");

    function updateInputs(data) {
        from = data.from;
        to = data.to;
        $("[name=pricefrom]").val(from);
        $("[name=priceto]").val(to);
        $inputFrom.prop("value", from);
        $inputTo.prop("value", to);
    }

    $inputFrom.on("input", function () {
        var val = $(this).prop("value");

        // validate
        if (val < min) {
            val = min;
        } else if (val > to) {
            val = to;
        }

        instance.update({
            from: val
        });
    });

    $inputTo.on("input", function () {
        var val = $(this).prop("value");

        // validate
        if (val < from) {
            val = from;
        } else if (val > max) {
            val = max;
        }

        instance.update({
            to: val
        });
    });

}

function initializeCompareSearchScripts() {

    $("#txtComapreSearch").keyup(function () {

        var searchValue = $(this).val().trim();
        if (searchValue.length > 0) {
            viewModel.CompareSearchResult.Name(searchValue);
            viewModel.CompareSearchResult.isService(false);
            viewModel.CompareSearchResult.PageSize(5);
            doCompareSearchProduct();
        }
        else {

            viewModel.CompareSearchResult.Name("");
            viewModel.CompareSearchResult.Results.removeAll();

        }
    });

    $(".compare-form .btn-remove-pr").on("click", function () {
        $(this).parent().remove();
        $('.compare-form').submit();
    })

}


function addToCompareList(data, event) {
    $('<input>').attr({
        type: 'hidden',
        name: 'productId',
        value: data.ID
    }).appendTo('.compare-form');
    $('.compare-form').submit();
}

function initializeBasketPageScript() {
    //Cart Partial Scripts
    //doGetCountryAddress();
    $(".shoping_address").removeClass("active");
    $(".shoping_confirm").removeClass("active");

    $(".shoping_cart .next-page").click(function () {
        if (viewModel.Account.IsOnline() == false) {
            $("#modal-login").modal("show");
        }
        else if (viewModel.Basket.StoreItems.Count() == 0) {
            GetNotifyMsg("error", lang.notify.BasketIsEmpty);
        }

        else {
            RemoveCookieOuOfSendStore();
            //$(".shoping_cart").removeClass("active");
            //$(".shoping_address").addClass("active");
            //$(".shoping_confirm").removeClass("active");


            //$(".div-confirm").removeClass("activeT");
            //$(".div-cart").addClass("activeT");
            //$(".div-address").addClass("activeT");

            //doGetAccountAddresses();
        }
        //$('html,body').animate({
        //    scrollTop: $(".basket-page").offset().top
        //}, 'slow');
    });

    $(".shoping_address .pre-page").click(function () {
        location.replace("/basket/");
        //$(".shoping_cart").addClass("active");
        //$(".shoping_confirm").removeClass("active");
        //$(".shoping_address").removeClass("active");

        //$(".div-cart").addClass("activeT");
        //$(".div-address").removeClass("activeT");
        //$(".div-confirm").removeClass("activeT");

        //$('html,body').animate({
        //    scrollTop: $(".basket-page").offset().top
        //}, 'slow');
    });
    $(".shoping_address .next-page").click(function () {
        if (viewModel.Basket.Address.AddressId() < 1) {
            GetNotifyMsg("error", lang.notify.PleaseSelectAddress);
        }
        else {
            location.replace("/basket/confirm");
            //$(".shoping_cart").removeClass("active");
            //$(".shoping_address").removeClass("active");
            //$(".shoping_confirm").addClass("active");

            //$(".div-confirm").addClass("activeT");
            //$(".div-cart").addClass("activeT");
            //$(".div-address").addClass("activeT");
        }
        //$('html,body').animate({
        //    scrollTop: $(".basket-page").offset().top
        //}, 'slow');
    });
    $(".shoping_confirm .pre-page").click(function () {
        location.replace("/basket/address");
        //$(".shoping_cart").removeClass("active");
        ////$(".shoping_address").addClass("active");
        //$(".shoping_confirm").removeClass("active");

        //$(".div-confirm").removeClass("activeT");
        //$(".div-cart").addClass("activeT");
        //$(".div-address").addClass("activeT");
        //$('html,body').animate({
        //    scrollTop: $(".basket-page").offset().top
        //}, 'slow');
    });
    $(".shoping_confirm .next-page").click(function () {
        if (viewModel.Basket.Address.AddressId() < 1) {
            //$(".shoping_cart").removeClass("active");
            //$(".shoping_confirm").removeClass("active");
            //$(".shoping_address").addClass("active");
            //$(".div-confirm").removeClass("activeT");
            //$(".div-cart").addClass("activeT");
            //$(".div-address").addClass("activeT");
            createMessage("error", "Please Select a Address");
        }
        else if ($('input[name=merchantId]:checked').val() == undefined) {
            GetNotifyMsg("error", lang.notify.PleaseSelectBank);
        }
        else {
            var merchantId = $('input[name=merchantId]:checked').val();
            var rebate = viewModel.Basket.Rebate.Value();
            var Description = $('input[name=time]').val();
            window.location.href = "/basket/NewOrder?addressId=" + viewModel.Basket.Address.AddressId() + "&rebateValue=" + rebate + "&merchantId=" + merchantId + "&store=true" + "&Description=" + Description;
        }

    });
    viewModel.Basket.Address.AddressId.subscribe(() => {

        if (viewModel.Account.Addresses.List().length > 0) {
            var match = ko.utils.arrayFirst(viewModel.Account.Addresses.List(), function (item) {
                return item.Id == viewModel.Basket.Address.AddressId();
            });
            if (match) {
                viewModel.Basket.Address.CurrentAddress(match);
                if (viewModel.Basket.Address.CurrentAddress().State.Id == 8) {
                    $(".hideForTehran").addClass("d-none");
                }
            }
            else {
                viewModel.Basket.Address.CurrentAddress("");
            }

        }

    });
    //---Cart Partial Scripts---

    //Address Partial Scripts
    doGetAccountAddresses();
    $(".shoping_address .add-new-address").click(function () {
        clearViewModelAccountAddressItem();
        $(".modal-add-address").modal("show");
    });
    //---Address Partial Scripts---

}

function initialArticleInfinity() {
    var infinityDiv = $(".infinity-div #infinity-content");
    var pageIndex = parseInt(infinityDiv.attr('page-index'));
    var pageSize = parseInt(infinityDiv.attr('page-size'));
    var poviewName = infinityDiv.attr('page-view');
    var pocategory = infinityDiv.attr('page-category');
    var pokeyword = infinityDiv.attr('page-keyword');
    var loading = infinityDiv.attr("loading");
    $(window).scroll(function () {
        pageIndex = parseInt(infinityDiv.attr('page-index'));
        pageSize = parseInt(infinityDiv.attr('page-size'));
        poviewName = infinityDiv.attr('page-view');
        pocategory = infinityDiv.attr('page-category');
        loading = infinityDiv.attr("loading");
        if (loading == "false") {
            if ((infinityDiv.offset().top + infinityDiv.height()) <= $(window).scrollTop() + $(window).height()) {
                getItems();
            }
        }
    });
    function getItems() {
        if (pageIndex != 0) {

            infinityDiv.attr("loading", "true");
            $.ajax({
                url: "/Post/Search",
                method: "GET",
                datatype: "Html",
                data: {
                    poviewName: poviewName,
                    pocategory: pocategory,
                    pokeyword: pokeyword,
                    poIndex: pageIndex,
                    poSize: pageSize
                },
                success: function (result) {
                    //console.log(result);
                    infinityDiv.append(result);
                    if (result.length < 5) {
                        infinityDiv.attr('page-index', "0");

                    }
                    else {
                        infinityDiv.attr('page-index', pageIndex + 1);
                        infinityDiv.attr("loading", "false");
                    }
                    $(".infinity-div .loader").hide();
                }

            });
        }
    }
    getItems();
}
function initialNewsInfinity() {
    var infinityDiv = $(".infinity-div2 #infinity-content2");
    var pageIndex = parseInt(infinityDiv.attr('page-index'));
    var pageSize = parseInt(infinityDiv.attr('page-size'));
    var poviewName = infinityDiv.attr('page-view');
    var pocategory = infinityDiv.attr('page-category');
    var pokeyword = infinityDiv.attr('page-keyword');
    var loading = infinityDiv.attr("loading");
    $(window).scroll(function () {
        pageIndex = parseInt(infinityDiv.attr('page-index'));
        pageSize = parseInt(infinityDiv.attr('page-size'));
        poviewName = infinityDiv.attr('page-view');
        pocategory = infinityDiv.attr('page-category');
        loading = infinityDiv.attr("loading");
        if (loading == "false") {
            if ((infinityDiv.offset().top + infinityDiv.height()) <= $(window).scrollTop() + $(window).height()) {
                getItems();
            }
        }
    });
    function getItems() {
        if (pageIndex != 0) {

            infinityDiv.attr("loading", "true");
            $.ajax({
                url: "/Post/Search",
                method: "GET",
                datatype: "Html",
                data: {
                    poviewName: poviewName,
                    pocategory: pocategory,
                    pokeyword: pokeyword,
                    poIndex: pageIndex,
                    poSize: pageSize
                },
                success: function (result) {
                    //console.log(result);
                    infinityDiv.append(result);
                    if (result.length < 5) {
                        infinityDiv.attr('page-index', "0");

                    }
                    else {
                        infinityDiv.attr('page-index', pageIndex + 1);
                        infinityDiv.attr("loading", "false");
                    }
                    $(".infinity-div2 .loader").hide();
                }

            });
        }
    }
    getItems();
}

function initializeBlogSearchScripts(label) {
    $("#txtBlogSearch").keyup(function () {
        var searchValue = $(this).val().trim();
        if (searchValue.length > 0) {
            viewModel.PostSearchResult.Name(searchValue);
            viewModel.PostSearchResult.Category(label);
            viewModel.PostSearchResult.PageSize(5);
            doSearchPostNew();
        }
        else {
            viewModel.PostSearchResult.Name("");
            viewModel.PostSearchResult.Results(null);
        }
    });
}

function getSimilarProduct(productId, categoryId, price, name, picture) {
    viewModel.FavoriteSimilarProduct.NameProduct(name);
    viewModel.FavoriteSimilarProduct.Price(price);
    viewModel.FavoriteSimilarProduct.PictureProduct(picture);
    $("#exampleModal").modal("show");
    var request = createRequest();
    request.type = apirequest.type.Get;
    request.url = request.url + codeprocess.api.product.name + "?notId=" + productId + "&categoryId=" + categoryId + "&pageSize=4" + "&exteraResult=true";
    request.success = function (result) {
        debugger
        if (result.Code == apirequest.status.Success) {
            debugger
            viewModel.FavoriteSimilarProduct.Items(result.Value.Products);
        }
    };
    $.ajax(request);
}

function showModalAddressForEdit(data, target) {
    updateViewModelAccountAddressItem(data);
    $("#addressCountry").val("118");
    GetStateByCountryIdAdress(viewModel.Account.Addresses.Item.CountryId());
    $(".modal-add-address").modal("show");
}
function showModalAddressForDelete(data, target) {
    $(".btnRemoveAddressItem").attr("address-id", data);
    $(".modal-address-delete").modal("show");
}
function ModalAddressForDelete(data, target) {
    $(".btnRemoveAddressItem").attr("address-id", data.Id);
    $(".modal-address-delete").modal("show");
}

function closeModal() {
    $(".modal").modal("hide");
}
function gotoProfile() {

    document.location = "/profile";
}
function gotoDashboard() {

    document.location = "/Account/Dashboard";
}




function showEditProfile() {
    $('#edit_profile_modal').modal('show');
    doGetCountryProfile();

    // GetStateByCountryIdShipping(viewModel.Account.CountryId());
    //$("#selectedState").val(viewModel.Account.StateId());
    //GetCityByStateId(viewModel.Account.StateId());
    //$("#selectedCity").val(viewModel.Account.CityId());
    //$("#addressCity").val(viewModel.Account.Addresses.Item.CityId());
    //viewModel.State.StateId = viewModel.Account.StateId();
    //viewModel.City.CityId = viewModel.Account.CityId();

}

function showImageCropper() {
    $('.modalImageCropper').modal('show');
}

function stateDropProfile() {
    var selectedstateID = $("#selectedStateProfile").val();
    viewModel.Account.StateId(selectedstateID);
    GetCityByStateIdProfile(selectedstateID);
}
function CityDropProfile() {
    var selectedCityID = $("#selectedCityProfile").val();
    viewModel.Account.CityId(selectedCityID);
}
function closeAddressModal() {
    $(".modal-add-address").modal("hide");
}
function showAddress() {
    viewModel.Account.Addresses.Item.NameFamily("");
    viewModel.Account.Addresses.Item.Mobile("");
    viewModel.Account.Addresses.Item.Phone("");
    viewModel.Account.Addresses.Item.PostalCode("");
    viewModel.Account.Addresses.Item.AddressValue("");
    doGetCountryAddress();
    GetStateByCountryIdAdress(118);
    $("#addressState").val(0);
    $("#addressCity").val(0);
    viewModel.Account.Addresses.Item.CountryId(118);
    viewModel.Account.Addresses.Item.StateId(0);
    viewModel.Account.Addresses.Item.CityId(0);
    GetCityByStateIdAdress(0);
    $('#addaddress_modal').modal('show');
}
function showAddressBasket() {
    viewModel.Account.Addresses.Item.NameFamily("");
    viewModel.Account.Addresses.Item.Mobile("");
    viewModel.Account.Addresses.Item.Phone("");
    viewModel.Account.Addresses.Item.PostalCode("");
    viewModel.Account.Addresses.Item.AddressValue("");
    doGetCountryAddress();
    GetStateByCountryIdAdress(118);
    $("#addressState").val(0);
    $("#addressCity").val(0);
    viewModel.Account.Addresses.Item.CountryId(118);
    viewModel.Account.Addresses.Item.StateId(0);
    viewModel.Account.Addresses.Item.CityId(0);
    GetCityByStateIdAdress(0);
    $('#basket_addaddress_modal').modal('show');
}
function showInvitation() {
    $("#txtInvitation").val("");
    $('#modalInvitation').modal('show');
}
function ShowAnswerComment(id) {
    GetComment(id);
    $('#mymodal').modal('show');
}
function ShowAnswerCommentStore(id) {
    GetStoreComment(id);
    $('#mymodalstore').modal('show');
}



function doGetCountryLocation(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.country.name;

    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.Country.CountryList(result.Value);
            viewModel.Country.CountryId(118);
            $("#locationCountry").val("118");
            GetStateByCountryIdLocation("118");
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}

function doUpdateLocation(id) {
    ajaxLoader(true);
    var request = createRequest();
    request.url = request.url + codeprocess.api.address.name + "?addressId=" + id;
    request.success = function (result) {
        viewModel.State.StateId(result.Value[0].State.Id);
        viewModel.City.CityId(result.Value[0].City.Id);
        viewModel.Country.CountryId(result.Value[0].Country.Id);
        doSetLocation(viewModel.Country.CountryId(), viewModel.City.CityId(), viewModel.State.StateId(), id);
        ajaxLoader(false);
    };
    $.ajax(request);
}

//function GetCurrentLocation() {
//    ajaxLoader(true);
//    var request = createRequest();
//    request.url = request.url + codeprocess.api.location.name;
//    request.success = function (result) {
//        console.log(result);
//        try {
//            viewModel.State.StateName(result.Value.StateName);
//            viewModel.City.CityName(result.Value.CityName);
//            viewModel.State.StateId(result.Value.StateId);
//            viewModel.City.CityId(result.Value.CityId);
//            viewModel.Country.CountryId(result.Value.CountryId);
//        }
//        catch (err) {
//            viewModel.State.StateName("تهران");
//            viewModel.City.CityName("تهران");
//            viewModel.State.StateId("8");
//            viewModel.City.CityId("87");
//            doSetLocation(viewModel.Country.CountryId(), viewModel.City.CityId(), viewModel.State.StateId(), 0);
//        }


//        viewModel.Basket.Address.AddressId(result.Value.AddressId);
//        if (viewModel.Account.Addresses.List().length > 0) {
//            if (viewModel.Basket.Address.AddressId() === 0) {
//                viewModel.Basket.Address.AddressId(viewModel.Account.Addresses.List()[0].Id);
//                doUpdateLocation(viewModel.Basket.Address.AddressId());

//            }

//            //doUpdateLocation(viewModel.Basket.Address.AddressId());
//        }
//        if (viewModel.City.CityId() === 0) {
//            viewModel.State.StateName("تهران");
//            viewModel.City.CityName("تهران");
//            viewModel.State.StateId("8");
//            viewModel.City.CityId("87");
//            doSetLocation(viewModel.Country.CountryId(), viewModel.City.CityId(), viewModel.State.StateId(), 0);
//        }

//        //doSetLocation(viewModel.Country.CountryId(), viewModel.City.CityId(), viewModel.State.StateId());
//        ajaxLoader(false);
//    };
//    $.ajax(request);
//}
function doGetStates(callback) {
    debugger;
    var request = createRequest();
    request.url = request.url + codeprocess.api.state.name;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            debugger
            viewModel.State.StateList(result.Value);
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}

function ProfileCountryDropShipping() {
    var countryId = $("#profileSelectedCountryShipping").val();
    viewModel.Account.CountryId(countryId);
    GetStateByCountryIdShipping(countryId);
}
function AddressCountryDropShipping() {
    var countryId = $("#selectedCountryShipping").val();
    viewModel.Account.Addresses.Item.CountryId(countryId);
    GetStateByCountryIdShipping(countryId);
}
function stateDropShipping() {
    var selectedstateID = $("#selectedStateShipping").val();
    GetCityByStateIdShipping(selectedstateID);
}
function stateDropAddress() {
    var selectedstateID = $("#addressState").val();
    viewModel.Account.Addresses.Item.StateId(selectedstateID);
    GetCityByStateIdAdress(selectedstateID);
}
function CountryDropShipping() {
    var countryId = $("#selectedCountryShipping").val();
    GetStateByCountryIdShipping(countryId);
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
function GetSelectCityIdAddress() {
    var selectedCityID = $("#addressCity").val();
    viewModel.Account.Addresses.Item.CityId(selectedCityID);

}
function CountryDropAddress() {
    var countryId = $("#addressCountry").val();
    viewModel.Account.Addresses.Item.CountryId(countryId);
    GetStateByCountryIdAdress(countryId);
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
            //$("#profileSelectedCountryShipping").val("118");
            //CountryDropShipping();
            //ProfileCountryDropShipping()
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function doGetCountryProfile(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.country.name;
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
            $("#profileSelectedCountryShipping").val("118");
            GetStateByCountryIdProfile(viewModel.Country.CountryId());
            //CountryDropShipping();
            //ProfileCountryDropShipping()
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function doGetCountryAddress(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.country.name;
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.Country.CountryList(result.Value);
            $("#addressCountry").val("118");
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

function GetStateByCountryIdShipping(countryId) {
    viewModel.State.StateList([]);
    var request = createRequest();
    request.url = request.url + codeprocess.api.state.name + "?countryId=" + countryId;

    request.success = function (result) {
        viewModel.State.StateList(result.Value);

    };
    $.ajax(request);
}
function GetStateByCountryIdLocation(countryId) {
    viewModel.State.StateList([]);
    var request = createRequest();
    request.url = request.url + codeprocess.api.state.name + "?countryId=" + countryId;
    request.success = function (result) {
        viewModel.State.StateList(result.Value);
        GetCurrentLocation();

    };
    $.ajax(request);
}
function GetStateByCountryIdAdress(countryId) {
    viewModel.State.StateList([]);
    var request = createRequest();
    request.url = request.url + codeprocess.api.state.name + "?countryId=" + countryId;

    request.success = function (result) {
        viewModel.State.StateList(result.Value);
        $("#addressState").val(viewModel.Account.Addresses.Item.StateId());
        GetCityByStateIdAdress(viewModel.Account.Addresses.Item.StateId());
    };
    $.ajax(request);
}
function GetStateByCountryIdProfile(countryId) {
    viewModel.State.StateList([]);
    var request = createRequest();
    request.url = request.url + codeprocess.api.state.name + "?countryId=" + countryId;
    ajaxLoader(true);
    request.success = function (result) {

        viewModel.State.StateList(result.Value);

        viewModel.State.StateId(viewModel.Account.StateId());
        $("#selectedStateProfile").val(viewModel.State.StateId());
        GetCityByStateIdProfile(viewModel.State.StateId());
    };
    $.ajax(request);
    ajaxLoader(false);
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
function GetCityByStateIdProfile(stateId) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.city.name + "?isShipping=true&id=" + stateId;
    ajaxLoader(true);

    request.success = function (result) {
        viewModel.City.CityList(result.Value);
        $("#selectedCityProfile").val(viewModel.Account.CityId());
    };
    $.ajax(request);
    ajaxLoader(false);
}
function GetCityByStateIdFilter(stateId) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.city.name + "?isShipping=true&id=" + stateId;
    ajaxLoader(true);
    request.success = function (result) {
        viewModel.CityFilter.CityList(result.Value);
        if ($("input[name='prcityId']").val() != "") {
            $("#cityFilter").val($("input[name='prcityId']").val());
        }
    };
    $.ajax(request);
    ajaxLoader(false);
}
function GetCityByStateIdAdress(stateId) {
    var request = createRequest();
    request.data = ko.toJSON(stateId);
    request.url = request.url + codeprocess.api.city.name + "?id=" + stateId;
    request.success = function (result) {
        viewModel.City.CityList(result.Value);
        $("#addressCity").val(viewModel.Account.Addresses.Item.CityId());
    };
    $.ajax(request);
}
function GetCityByStateIdLocation(stateId, stateName) {
    ajaxLoader(true);
    var request = createRequest();
    request.data = ko.toJSON(stateId);
    request.url = request.url + codeprocess.api.city.name + "?id=" + stateId;
    request.success = function (result) {
        viewModel.City.CityList(result.Value);
        viewModel.City.CityList.remove("0");
        viewModel.City.CityName("");
        viewModel.City.CityId("");
        viewModel.State.StateName(stateName);
        viewModel.State.StateId(stateId);
        ajaxLoader(false);
    };
    $.ajax(request);

}
function GetSelectCityIdlocation(cityId, cityName) {
    viewModel.City.CityId(cityId);
    viewModel.City.CityName(cityName);
    doSetLocation(viewModel.Country.CountryId(), viewModel.City.CityId(), viewModel.State.StateId(), viewModel.Basket.Address.AddressId());

    $("#exampleModalCenter").modal("hide");
}
function UpdateLocation() {
    var id = viewModel.Basket.Address.AddressId();
    if (id !== 0) {

        doUpdateLocation(id);

    }

}


/////////
function lodingOn() {
    $(".content-div").opacity("0.5");

}
function lodingOff() {
    $(".content-div").opacity("1");
}
function formSumbit() {
    $("form").submit();
}
function reLoadPage() {
    location.reload();
};

function testSearch() {
    var request = createRequest();
    request.url = "https://campingbazar.com/cpr/" + "search?q=دریل";
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    request.success = function (result) {
        console.log(result);
    };
    $.ajax(request);
}
function testSearch2() {
    var request = createRequest();
    request.url = "https://campingbazar.com/cpr/" + "productsearch?pname=دریل";
    request.datatype = "jsonp";

    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    request.success = function (result) {
        console.log(result);
    };
    $.ajax(request);
}
function testSearch3() {
    var request = createRequest();
    request.url = "http://localhost:300/cpr/" + "post?name=دریل";
    request.datatype = "jsonp";
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    request.success = function (result) {
        console.log(result);
    };
    $.ajax(request);
}

$("input[type='file']").change(function () {
    debugger;
    $("#modalLoading").modal("show");
    var fileUpload = $(this).get(0);
    var files = fileUpload.files;
    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }
    var request = createRequest();
    request.url = "/panel/Upload/UploadDocument";
    request.type = "POST";
    request.data = data,
        request.contentType = false;
    request.processData = false;
    request.success = function (entity) {
        // اینجا مسیر عکس را بر میگرداند
        var id = $(fileUpload).attr("id");
        $('.' + id).find("[form-field-id]").val(entity.Url);
        $("#DocumentId").val(entity.Id);
        $("#FileUpload").attr("href", entity.Url.replace("SYSTEM_TYPE_PANEL", "/panel"));
        $("#FileUpload").removeClass("d-none");
        $("#modalLoading").modal("hide");
    };
    $.ajax(request);
});
var chVal = 0;

function checkBMI() {


    var Height = $("#Height").val();
    var Weight = $("#Weight").val();
    var Age = $("#Age").val();
    var g = $('input[name=Gender]:checked').val();
    if (Height == "" || Height < 0) {
        createMessage("error", "قد بدرستی وارد نشده است");
    }
    else if (Weight < 0 || Weight == "") {
        createMessage("error", ",وزن بدرستی وارد نشده است");
    }
    else if (Age < 0 || Age == "") {
        createMessage("error", ",سن بدرستی وارد نشده است");
    }
    else if (g != 0 && g != 1) {
        createMessage("error", ",جنسیت بدرستی وارد نشده است");
    }
    else {
		Height=Height/100;
        var Height2 = Height * Height;
        var Bmi = Weight / Height2;
        var num1 = 1.2 * Bmi
        var num2 = 0.23 * Age
        var num3 = g * 10.8;
        var ch = num1 + num2 - num3 - 5.4;
        var Bmr = 0;
        if (g == 1) {
            Bmr = Weight * 1 * 24;
        }
        else {
            Bmr = Weight * 0.95 * 24;
        }


        var bmr1 = Bmr * 0.30;
        var bmr2 = Bmr * 0.50;
        var bmr3 = Bmr * 0.10;
        var bmr4 = Bmr * (1 - 2.5);
        Bmi = financial(Bmi);
        Bmr = financial(Bmr);
        bmr1 = financial(bmr1);
        bmr2 = financial(bmr2);
        bmr3 = financial(bmr3);
        bmr4 = financial(bmr4);
        ch = financial(ch);
        var t = Number.parseFloat(ch).toFixed(0);
        t = parseInt(t);
        Highcharts.chart('container', {
            chart: {
                type: 'gauge',
                plotBackgroundColor: null,
                plotBackgroundImage: null,
                plotBorderWidth: 0,
                plotShadow: false,
                height: '80%'
            },

            title: {
                text: 'در صد چربی شما'
            },

            pane: {
                startAngle: -90,
                endAngle: 89.9,
                background: null,
                center: ['50%', '75%'],
                size: '110%'
            },

            // the value axis
            yAxis: {
                min: 0,
                max: 100,
                tickPixelInterval: 72,
                tickPosition: 'inside',
                tickColor: Highcharts.defaultOptions.chart.backgroundColor || '#FFFFFF',
                tickLength: 20,
                tickWidth: 2,
                minorTickInterval: null,
                labels: {
                    distance: 20,
                    style: {
                        fontSize: '14px'
                    }
                },
                lineWidth: 0,
                plotBands: [{
                    from: 0,
                    to: 50,
                    color: '#55BF3B', // green
                    thickness: 20
                }, {
                    from: 50,
                    to: 75,
                    color: '#DDDF0D', // yellow
                    thickness: 20
                }, {
                    from: 75,
                    to: 100,
                    color: '#DF5353', // red
                    thickness: 20
                }]
            },
            
            series: [{
                name: 'Fat Percentage ',
                data: [t],
                tooltip: {
                    valueSuffix: ' %'
                },
                dataLabels: {
                    format: '{y}%',
                    borderWidth: 0,
                    color: (
                        Highcharts.defaultOptions.title &&
                        Highcharts.defaultOptions.title.style &&
                        Highcharts.defaultOptions.title.style.color
                    ) || '#333333',
                    style: {
                        fontSize: '16px'
                    }
                },
                dial: {
                    radius: '80%',
                    backgroundColor: 'gray',
                    baseWidth: 12,
                    baseLength: '0%',
                    rearLength: '0%'
                },
                pivot: {
                    backgroundColor: 'gray',
                    radius: 6
                }

            }]


            
        });
       
        $("#BMI").html(Bmi);
        $("#CH").html(ch);
        $(".imgf").addClass("d-none");
        if (Bmi < 18.5) {
            $(".img1").removeClass("d-none");
        }
        else if (Bmi < 24.5) {
            $(".img2").removeClass("d-none");
        }
        else if (Bmi < 29.9) {
            $(".img3").removeClass("d-none");
        }
        else {
            $(".img4").removeClass("d-none");
        }
        $("#formMBI").removeClass("d-block");
        $("#formMBI").addClass("d-none");

        $("#divRes1").removeClass("d-none");
        $("#divRes1").addClass("d-block");
        $("#divRes2").removeClass("d-none");
        $("#divRes2").addClass("d-block");
        $("#divRes3").removeClass("d-none");
        $("#divRes3").addClass("d-block");
        $("#Bmr").html(Bmr);
        $("#Bmr1").html(bmr1);
        $("#Bmr2").html(bmr2);
        $("#Bmr3").html(bmr3);
        $("#Bmr4").html(bmr4);
    }



}
function financial(x) {
    return Number.parseFloat(x).toFixed(2);
}
function refreshBMI() {
    $(".imgf").removeClass("d-none");
    $(".img1").addClass("d-none");
    $(".img2").addClass("d-none");
    $(".img3").addClass("d-none");
    $(".img4").addClass("d-none");

    $("#formMBI").removeClass("d-none");
    $("#formMBI").addClass("d-block");

    $("#divRes1").removeClass("d-block");
    $("#divRes1").addClass("d-none");
    $("#divRes2").removeClass("d-block");
    $("#divRes2").addClass("d-none");
    $("#divRes3").removeClass("d-block");
    $("#divRes3").addClass("d-none");
    $("#Bmr").html("");
    $("#Bmr1").html("");
    $("#Bmr2").html("");
    $("#Bmr3").html("");
    $("#Bmr4").html("");
    $("#BMI").html("");
    $("#CH").html("");
}
function FinishExame() {
    var model = [];
    var exameId = $("#exameId").val();
    var accountId = $("#accountId").val();

    var rowAnserItem = $("[question-value]");
    if (rowAnserItem.length > 0) {
        for (var i = 0; i < rowAnserItem.length; i++) {
            var Object = rowAnserItem[i];
            var ItemEntity = {

                QuestionId: parseInt($(Object).find("[question-name='QuestionId']").val()),
                AnswerId: parseInt($(Object).find("[question-name = 'Answer']:checked").val()),


            };
            model.push(ItemEntity);
        }
    }

    var request = createRequest();
    request.type = "POST";
    request.data = ko.toJSON(model),
        request.url = request.url + codeprocess.api.Exame.name + "?exameId=" + exameId + "&accountId=" + accountId;
    request.success = function (result) {
        if (result.Code == apirequest.status.Success) {
            createMessage(result.Message);
            window.location.replace("/exame/success?id=" + exameId);

        }
        else {
            createMessage("error", result.Message);
        }
    };
    $.ajax(request);
}
function AddExame(questionId,answerId) {
    var accountAnswerId = $("#accountAnswerId").val();

    var request = createRequest();
    request.type = "POST";
    request.url = request.url + codeprocess.api.ExameAccountAnswerDetails.name + "?accountAnswerId=" + accountAnswerId + "&answerId=" + answerId + "&questionId=" + questionId;
    request.success = function (result) {
    };
    $.ajax(request);
}
function showCourse(id) {
    if (id == 1) {
        $(".couseList").removeClass("d-none");
        $(".divetext").addClass("d-none");
        $("#desCription").val("");

        $("input[name='courseItem']:checkbox").prop('checked', false);
        var addtocartBtn = $("#addtocart-btn");
        var newUrl = addtocartBtn.attr("sample-href") + 1;
        addtocartBtn.attr("href", newUrl);
    }
    else {
        $(".couseList").addClass("d-none");
        $(".divetext").removeClass("d-none");
        $("#desCription").val("");
        $("input[name='courseItem']:checkbox").prop('checked', false);
        var addtocartBtn = $("#addtocart-btn");
        var newUrl = addtocartBtn.attr("sample-href") + 1;
        newUrl = newUrl + "&description=سایر";
        addtocartBtn.attr("href", newUrl);
    }
}
