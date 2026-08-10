function initializeCommonScript() {
    $(document).ready(function () {
        initializeHeaderSearchScripts();
        doGetCurrentAccount();
        doGetBasket();
        initializeHomePageScript();
        try {
            $(".show-register-modal").click(function () {
                $("#modal-login").modal("hide");
                $("#modal-register").modal("show");
            });
            $(".show-login-modal").click(function () {
                $("#modal-login").modal("show");
                $("#modal-register").modal("hide");
            });
            hljs.configure({ tabReplace: '  ' });
            hljs.initHighlightingOnLoad();
        }
        catch { }       
    });
}

function initCodeprocessUpload() {
    $(".dropify").dropify({
        messages: {
            'default': 'فایل را اینجا بیندازید',
            'replace': 'برای تغییر تصویر کلیک کنید',
            'remove': 'حذف',
            'error': 'خطا در هنگام ارسال تصویر'
        },
        error: {
            'fileSize': 'The file size is too big (1M max).'
        }
    });

    $(".dropify").change(function () {
        var fileUpload = $(this);
        var files = fileUpload.files;
        var target = $(fileUpload).attr("tatget");
        var data = new FormData();
        for (var i = 0; i < files.length; i++) {
            data.append(files[i].name, files[i]);
        }
        $.ajax({
            url: "/panel/Upload/UploadPhoto",
            type: "POST",
            data: data,
            contentType: "application/json; charset=utf-8",
            error: function (a) {
            },
            success: function (result) {
                $("[name="+target+"]").val(result.Id);
            }
        });
       
    });
}


function closeModal() {
    $(".modal").modal("hide");
}
function initializeProductPageScript()
{
    $(".color-box .btn-color").click(function () {
        var elm = $(this);
        $(".basket-btn").attr("color-id", elm.attr("color-id"));
        $(".color-box .btn-color").removeClass("selectColor");
        elm.addClass("selectColor");
    });
    $(".size-box .btn-size").click(function () {
        var elm = $(this);
        $(".basket-btn").attr("size-id", elm.attr("size-id"));
        $(".size-box .btn-size").removeClass("selectColor");
        elm.addClass("selectColor");
    });
}
function initializeSearchPageScript() {
    var searchForm = $("#searchForm");
    $('[check-name=check-brands]').change(function () {
        var ids = [];
        $('.brand-side input:checked').each(function () {
            ids.push($(this).val());
        });
        $("input[name='prbrandId']").val(ids.join("-"));
        $(searchForm).submit();

    });
    $('[check-name=check-colors]').change(function () {
        var ids = [];
        $('.color-side input:checked').each(function () {
            ids.push($(this).val());
        });
        $("input[name='prcolorId']").val(ids.join("-"));
        $(searchForm).submit();
    });

    $('.product-type-side .groupIds').click(function () {
        $('input[name=groupIds]').val($(this).attr("groupIds"));
        $(searchForm).submit();

    });

    $("[name='prorder']").change(function () {
        $(searchForm).submit();
    });
}
function initializeHeaderSearchScripts() {
    $("#txtHeaderSearch").keyup(function () {
        var searchValue = $(this).val().trim();
        if (searchValue.length > 0) {
            if (searchValue == "") {
                $(".header-search .quick-results.show-result").hide();
                $(".header-search.quick-no-results").hide();
            } else {
                $(".header-search.quick-results.show-result").show();
                $(".header-search.quick-no-results").show();

                viewModel.SearchResult.Name(searchValue);
                viewModel.SearchResult.PageSize(5);

                doSearchProduct();
            }
        } else {
            $(".header-search .quick-results.show-result").hide();
            $(".header-search.quick-no-results").hide();
        }
    });
}
function initializeCompareSearchScripts() {

    $("#txtCompareSearch").keyup(function () {
        var searchValue = $(this).val().trim();
        if (searchValue.length > 0) {
            if (searchValue == "") {
                $(".compare-search .quick-results.show-result").hide();
                $(".compare-search .quick-no-results").hide();
            } else {
                $(".compare-search.quick-results.show-result").show();
                $(".compare-search.quick-no-results").show();

                viewModel.CompareSearchResult.Name(searchValue);
                viewModel.CompareSearchResult.PageSize(5);

                doCompareSearchProduct();
            }
        } else {
            $(".compare-search .quick-results.show-result").hide();
            $(".compare-search .quick-no-results").hide();
        }
    });

    $(".btn-remove-pr").on("click", function () {
        $(this).parent().remove();
        $('.compare-form').submit();
    })
}
function addToCompareList(data, event) {
    $('<input>').attr({
        type: 'hidden',
        name: 'productId',
        value: data.Id,
    }).appendTo('.compare-form');
    $('.compare-form').submit();
}
function initializeBasketPageScript() {
    //Cart Partial Scripts
    $(".shoping_address").hide();
    $(".shoping_confirm").hide();

    $(".shoping_cart .next-page").click(function () {
        if (viewModel.Account.IsOnline() == false) {
            $("#modal-login").modal("show");
        }
        else {
            $(".shoping_cart").hide();
            $(".shoping_confirm").hide();
            $(".shoping_address").fadeIn("1000");
            $(".liAct").removeClass("liAct");
            $(".li-cart").addClass("liAct");
            $(".li-address").addClass("liAct");
        }
    });

    $(".shoping_address .pre-page").click(function () {
        $(".shoping_address").hide();
        $(".shoping_confirm").hide();
        $(".shoping_cart").fadeIn("1000");
        $(".liAct").removeClass("liAct");
        $(".li-cart").addClass("liAct");
    });
    $(".shoping_address .next-page").click(function () {
        if (viewModel.Basket.Address.AddressId() < 1) {
            createMessage("error", "لطفا آدرس را انتخاب کنید.")
        }
        else {
            $(".shoping_address").hide();
            $(".shoping_cart").hide();
            $(".shoping_confirm").fadeIn("1000");
            $(".liAct").removeClass("liAct");
            $(".li-cart").addClass("liAct");
            $(".li-address").addClass("liAct");
            $(".li-confirm").addClass("liAct");
        }
    });
    $(".shoping_confirm .pre-page").click(function () {
        $(".shoping_confirm").hide();
        $(".shoping_cart").hide();
        $(".shoping_address").fadeIn("1000");
        $(".liAct").removeClass("liAct");
        $(".li-cart").addClass("liAct");
        $(".li-address").addClass("liAct");
    });
    $(".shoping_confirm .next-page").click(function () {
        if (viewModel.Basket.Address.AddressId() < 1) {
            $(".shoping_confirm").hide();
            $(".shoping_cart").hide();
            $(".shoping_address").fadeIn("1000");
            createMessage("error", "Please Select a Address");
        }
        else {
            window.location.href = "/basket/NewOrder?addressId=" + viewModel.Basket.Address.AddressId();
        }
    });
    viewModel.Basket.Address.AddressId.subscribe(() => {
        
        if (viewModel.Account.Addresses.List().length > 0) {
            var match = ko.utils.arrayFirst(viewModel.Account.Addresses.List(), function (item) {
                return item.Id == viewModel.Basket.Address.AddressId();
            });
            if (match) {
                viewModel.Basket.Address.CurrentAddress(match);
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

function showModalAddressForEdit(data, target) {
    updateViewModelAccountAddressItem(data);
    $(".modal-add-address").modal("show");
};

function showModalAddressForDelete(data, target) {
    $(".btnRemoveAddressItem").attr("address-id", data.Id);
    $(".modal-address-delete").modal("show");
};
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




