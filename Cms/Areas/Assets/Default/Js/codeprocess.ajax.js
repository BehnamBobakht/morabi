var viewModel = new (function () {
    this.NewsLetter = new (function () {
        this.Phone = ko.observable();
        this.Email = ko.observable();
    })();
    this.ContactUs = new (function () {
        this.Name = ko.observable("");
        this.Title = ko.observable("");
        this.Body = ko.observable("");
        this.Phone = ko.observable("");
        this.Email = ko.observable("");
        this.PictureId = ko.observable(null);
        this.DocumentId = ko.observable(null);
        this.ContactUsField = ko.observableArray([]);
    })();
    this.Invait = new (function () {
        this.EmailOrPhone = ko.observable();
    })();
    this.Basket = new (function () {
        this.sendTypeMaxDay = ko.observable();
        this.Items = ko.observableArray([]);
        this.CurrentStoreId = ko.observable("");
        this.CurrentStore = ko.observable("");
        this.AddCount = ko.observable(1);
        this.SendType = new (function () {
            this.CurrnetPrice = ko.observable(0);
            this.Price = ko.observable(0);
            this.MinPriceForFree = ko.observable(0);
            this.MaxDays = ko.observable(0);
        })();
        this.StoreItems = new (function () {
            this.List = ko.observableArray([]);
            this.TotalPrice = ko.observable(0);
            this.ShippingPrice = ko.observable(0);
            this.TotalDiscount = ko.observable(0);
            this.Count = ko.observable(0);
        })();
        this.Address = new (function () {
            this.AddressId = ko.observable(0);
            this.CurrentAddress = ko.observable("");
        })();
        this.Count = ko.computed(function () {
            var basketCount = 0;
            for (var i = 0; i < this.Items().length; i++) {
                basketCount += +this.Items()[i].Count;
            }
            return basketCount;
        }, this);
        this.PriceInt = ko.computed(function () {
            var basketPrice = 0;
            for (var i = 0; i < this.Items().length; i++) {
                basketPrice += +this.Items()[i].Price;
            }
            return basketPrice;
        }, this);
        this.Price = ko.computed(function () {
            var basketPrice = 0;
            for (var i = 0; i < this.Items().length; i++) {
                basketPrice += this.Items()[i].Price;
            }
            return toCurrency(basketPrice);
        }, this);
        this.ProductPriceWithoutDiscount = ko.computed(function () {
            var basketPrice = 0;
            for (var i = 0; i < this.Items().length; i++) {
                basketPrice += this.Items()[i].ProductPrice * this.Items()[i].Count;
            }
            return basketPrice;
        }, this);
        this.Discount = ko.computed(function () {
            var basketDiscount = 0;
            for (var i = 0; i < this.Items().length; i++) {
                basketDiscount += +this.Items()[i].Discount;
            }
            return toCurrency(basketDiscount);
        }, this);
        this.DiscountInt = ko.computed(function () {
            var basketDiscount = 0;
            for (var i = 0; i < this.Items().length; i++) {
                basketDiscount += +this.Items()[i].Discount;
            }
            return basketDiscount;
        }, this);
        this.Rebate = new (function () {
            this.Value = ko.observable("");
            this.Price = ko.observable(0);
            this.Valid = ko.observable(false);
            this.Message = ko.observable("");
        })();
    })();
    this.ListOutOfSendStore = ko.observableArray([]);
    this.FavoriteSimilarProduct = new (function () {
        this.NameProduct = ko.observable("");
        this.PictureProduct = ko.observable("");
        this.Items = ko.observableArray([]);
        this.Price = ko.observable("");
    })();
    this.ContactForm = new (function () {
        this.WebsiteId = ko.observable();
        this.FullName = ko.observable();
        this.Email = ko.observable();
        this.Body = ko.observable();
        this.Subject = ko.observable();
        this.Mobile = ko.observable();
    })();
    this.Account = new (function () {
        this.FaBirthDay = ko.observable();
        this.FullName = ko.observable();
        this.Mobile = ko.observable();
        this.Email = ko.observable();
        this.Phone = ko.observable();
        this.NationalCode = ko.observable();
        this.Job = ko.observable();
        this.Company = ko.observable();
        this.Address = ko.observable();
        this.Password = ko.observable();
        this.ConfirmPassword = ko.observable();
        this.CompanyNo = ko.observable();
        this.IsMale = ko.observable(true);
        this.CardNumber = ko.observable();
        this.Sheba = ko.observable();
        this.StateId = ko.observable();
        this.CityId = ko.observable();
        this.CountryId = ko.observable();
        this.StoreId = ko.observable();
        this.PictureId = ko.observable();
        this.PictureUrl = ko.observable();
        this.NationalPictureId = ko.observable();
        this.NationalPictureUrl = ko.observable();
        this.FullNameEn = ko.observable();

        this.Message = ko.observable();
        this.Store = new (function () {
            this.Id = ko.observable();
            this.Name = ko.observable();
            this.Approve = ko.observable();
            this.Active = ko.observable();
        })();
        this.UserComment = ko.observable();
        this.StoreUserComment = ko.observable();
        this.ProductComment = function () {
            this.ID = ko.observable();
            this.AccountId = ko.observable();
            this.ProductId = ko.observable();
            this.Body = ko.observable();
            this.AnswerString = ko.observable();
            this.Approved = ko.observable();
            this.Rate = ko.observable();
            this.Datetime = ko.observable();
            this.AnswerDatetime = ko.observable();
            this.UserId = ko.observable();
            this.NameFamily = ko.observable();
            this.EmailAddress = ko.observable();
            this.Account = ko.observable();
            this.Product = ko.observable();
            this.SiteUser = ko.observable();
        };
        this.StoreComment = function () {
            this.ID = ko.observable();
            this.AccountId = ko.observable();
            this.StoreId = ko.observable();
            this.Body = ko.observable();
            this.AnswerString = ko.observable();
            this.Approved = ko.observable();
            this.Rate = ko.observable();
            this.Datetime = ko.observable();
            this.AnswerDatetime = ko.observable();
            this.UserId = ko.observable();
            this.NameFamily = ko.observable();
            this.EmailAddress = ko.observable();
            this.Account = ko.observable();
            this.Store = ko.observable();
            this.SiteUser = ko.observable();
        };
        this.remember = ko.observable(true);
        this.true = ko.computed(
            {
                read: function () {
                    return this.IsMale() == "true";
                },
                write: function (value) {
                    if (value) this.IsMale("true");
                },
            },
            this
        );
        this.Agent = ko.observable();
        this.AgentPhone = ko.observable();
        this.ReagentName = ko.observable();
        this.Captcha = ko.observable();
        this.Favorites = ko.observableArray([]);
        this.Addresses = new (function () {
            this.State = ko.observableArray([]);
            this.City = ko.observableArray([]);
            this.Item = new (function () {
                this.ID = ko.observable(0);
                this.NameFamily = ko.observable();
                this.NationalCode = ko.observable();
                this.Mobile = ko.observable();
                this.Phone = ko.observable();
                this.State = ko.observable(null);
                this.CityId = ko.observable();
                this.StateId = ko.observable();
                this.CountryId = ko.observable();
                this.LegalUser = ko.observable();
                this.StateName = ko.computed(function () {
                    var state = this.State();
                    if (state != null) {
                        var name = state.split("-");
                        return name[1];
                    } else {
                        return null;
                    }
                }, this);
                this.PostalCode = ko.observable();
                this.AddressValue = ko.observable();
                this.Description = ko.observable();
                this.Latitude = ko.observable("");
                this.Longitude = ko.observable("");
            })();
            this.List = ko.observableArray([]);
        })();
        this.IsOnline = ko.observable(false);
    })();
    this.Product = new (function () {
        this.Id = ko.observable();
        this.Name = ko.observable();
        this.CodeValue = ko.observable();
        this.Summary = ko.observable();
        this.Description = ko.observable();
        this.ProductBrand = ko.observable();
        this.Items = ko.observable();
        this.LikesCount = ko.observable();
        this.Status = new (function () {
            this.Name = ko.observable();
            this.Label = ko.observable();
        })();
        this.ProductType = new (function () {
            this.Name = ko.observable();
        })();
        this.ProductCategory = new (function () {
            this.Name = ko.observable();
        })();
        this.ProductSubCategory = new (function () {
            this.Name = ko.observable();
        })();
        this.ProductQuantity = ko.observableArray();
        this.LastPrice = ko.observable();
        this.Price = ko.observable();
        this.DiscountPrice = ko.observable();
        this.Picture = new (function () {
            this.Id = ko.observable();
            this.Url = ko.observable();
        })();
        this.Document = ko.observable();
        this.Pictures = ko.observableArray();
        this.Colors = ko.observableArray();
        this.Sizes = ko.observableArray();
        this.Active = ko.observable();
        this.GetLink = ko.computed(function () {
            return "/pr/" + this.Id() + "/" + this.Name();
        }, this);
    })();
    this.ShopReseller = new (function () {
        this.Id = ko.observable(0);
        this.Name = ko.observable();
        this.Website = ko.observable();
        this.Picture = new (function () {
            this.Id = ko.observable();
            this.Url = ko.observable();
        })();
        this.ShopResellerProducts = ko.observableArray([]);
        this.ShopResellerGalleries = ko.observableArray([]);
        this.ShopResellerCollections = ko.observableArray([]);
        this.ShopResellerStories = ko.observableArray([]);
        this.GetLink = ko.computed(function () {
            return "/rs/" + this.Id() + "/" + this.Name();
        }, this);
    })();
    this.SearchResult = new (function () {
        this.Pending = ko.observable(false);
        this.Name = ko.observable("");
        this.ProductType = ko.observable(null);
        this.ProductBrand = ko.observable(null);
        this.ProductCategory = ko.observable(null);
        this.ProductSubCategory = ko.observable(null);
        this.PageSize = ko.observable(10);
        this.PageIndex = ko.observable(1);
        this.Results = ko.observableArray();
        this.isService = ko.observable();
    })();
    this.StoreSearchResult = new (function () {
        this.Name = ko.observable("");
        this.PageSize = ko.observable(10);
        this.PageIndex = ko.observable(1);
        this.Results = ko.observableArray();
    })();
    this.PostSearchResult = new (function () {
        this.Pending = ko.observable(false);
        this.Name = ko.observable("");
        this.Category = ko.observable("");
        this.TypeId = ko.observable("");
        this.PageSize = ko.observable(10);
        this.PageIndex = ko.observable(1);
        this.Results = ko.observableArray();
    })();
    this.CompareSearchResult = new (function () {
        this.Name = ko.observable("");
        this.ProductType = ko.observable(null);
        this.ProductBrand = ko.observable(null);
        this.ProductCategory = ko.observable(null);
        this.ProductSubCategory = ko.observable(null);
        this.PageSize = ko.observable(10);
        this.PageIndex = ko.observable(1);
        this.Results = ko.observableArray();
        this.isService = ko.observable();
    })();
    this.Variety = new (function () {
        this.ProductId = ko.observable();
        this.Colors = ko.observableArray();
        this.Sizes = ko.observableArray();
        this.ColorId = ko.observable(null);
        this.SizeId = ko.observable(null);
        this.ColorName = ko.observable(null);
        this.ColorHex = ko.observable(null);
        this.SizeName = ko.observable(null);
        this.OptionName = ko.observable(null);
        this.OptionItemId = ko.observable(null);
        this.OptionItemValue = ko.observable(null);
        this.OptionItems = ko.observableArray();
        this.ProductQuantity = ko.observableArray();
    })();
    this.Store = new (function () {
        this.ProductList = ko.observableArray();
    })();
    this.State = new (function () {
        this.StateList = ko.observableArray();
        this.StateId = ko.observable();
        this.StateName = ko.observable("انتخاب نشده");
    })();
    this.Country = new (function () {
        this.CountryList = ko.observableArray();
        this.CountryId = ko.observable("118");
    })();
    this.City = new (function () {
        this.CityList = ko.observableArray();
        this.CityId = ko.observable(0);
        this.CityName = ko.observable("انتخاب نشده");
    })();
    this.CityFilter = new (function () {
        this.CityList = ko.observableArray();
    })();
    this.StateFilter = new (function () {
        this.StateList = ko.observableArray();
    })();
    this.Currency = new (function () {
        this.CurrencyList = ko.observableArray();
    })();
    this.ChangedCurrency = new (function (Id, Price) {
        this.ChangedCurrency = ko.observableArray();
        this.Id = ko.observable();
        this.Price = ko.observable();
    })();
    this.toCurrency = function (amount) {
        return toCurrency(ko.toJSON(amount));
    };
    this.toPersian = function (numberValue) {
        return toPersian(ko.toJSON(numberValue));
    };
    this.AccountLogVisit = new (function () {
        this.CheckLogVisit = ko.observable();
    })();
    this.FirstMessage = new (function () {
        this.Body = ko.observable();
        this.Picture = ko.observable();
    })();
    this.Shipping = new (function () {
        this.List = ko.observable();
    })();
})();
ko.applyBindings(viewModel);
var codeprocess = {
    function: "codeprocess-action",
    fileType: { document: "document", picture: "picture" },
    callback: { name: "codeprocess-callback", clear: { form: "clear-form" }, close: { modal: "close-modal" } },
    field: {
        attrName: "codeprocess-field",
        nameFamily: "namefamily",
        body: "body",
        productId: "product-id",
        rate: "rate",
        basketId: "basket-id",
        basketCount: "basket-count",
        orderId: "orderId",
        rebateValue: "rebate-value",
        rebateMobile: "rebate-mobile",
        newPassword: "new-password",
        confirmPassword: "confirm-password",
        actionType: { name: "action-type", add: "add", remove: "remove" },
        account: {
            fullname: "account-fullname",
            email: "account-email",
            password: "account-password",
            nationalCode: "account-nationalcode",
            mobile: "account-mobile",
            phone: "account-phone",
            address: "account-address",
            confirm: "account-confirm",
            reagent: "account-reagent",
            loginMobile: "account-login-mobile",
            loginPassword: "account-login-password",
            Captcha: "account-Captcha",
            bothEmailMobile: "both-EmailMobile",
            userName: "account-username",
            remember: "remember",
        },
        order: { tracking: { code: "tracking-code", result: "tracking-result" } },
        google: { recaptcha: "g-recaptcha-response" },
        reagent: { mobile: "reagent-mobile", email: "reagent-email", fullName: "reagent-fullname" },
        websiteform: { formId: "website-form-id" },
    },
    target: { button: "button", input: "input", modal: "modal", form: "form" },
    api: {
        Survey: { name: "Survey" },
        PostSearch: { name: "PostSearch" },
        Exame: { name: "Exame" },
        account: { name: "account", edit: "account-edit", get: "account-get", changepassword: "changepassword", changeusername: "changeusername" },
        profile: { UploadPictureProfile: "UploadPicture" },
        login: { name: "login" },
        register: { name: "register" },
        basket: { name: "accountbasket", store: "accountbasketstore", get: "basket-get", add: "basket-add", edit: "basket-edit", remove: "basket-remove", sendType: "sendType" },
        shopfollow: { name: "shopfollow", follow: "shop-follow", unfollow: "shop-unfollow" },
        address: { name: "accountaddress", add: "address-add", edit: "address-edit", remove: "address-remove" },
        country: { name: "Country" },
        state: { name: "state" },
        currency: { name: "currency" },
        city: { name: "City" },
        newsletter: { name: "newsletter", add: "newsletter-add" },
        contactUs: { name: "contactUs", add: "contactUs-add", uploadDocument: "upload-document", uploadPicture: "upload-picture" },
        invait: { name: "invait", add: "invait-add" },
        accountLogVisit: { name: "accountLogVisit", add: "accountLogVisit-add", check: "CheckUserVisit" },
        product: { name: "product" },
        ProductQuantity: { name: "ProductQuantity" },
        productCategory: { name: "ProductCategory" },
        productlike: { name: "productlike", add: "productlike-add", remove: "productlike-remove" },
        productnotify: { name: "productnotify" },
        productcomment: { name: "productcomment", add: "comment-add", get: "comment-get" },
        storecomment: { name: "storecomment", add: "storecomment-add", get: "storecomment-get" },
        rebate: { name: "rebate", remove: "rebate-remove", sms: "rebate-sms" },
        reagent: { name: "accountreagent" },
        contactform: { name: "contactform" },
        credit: { name: "credit" },
        payment: { name: "payment" },
        order: { name: "order", tracking: "tracking" },
        shopreseller: { name: "shopreseller" },
        shopresellercollection: { name: "shopresellercollection" },
        shopresellergallery: { name: "shopresellergallery" },
        shopresellerproduct: { name: "shopresellerproduct" },
        shopresellerstory: { name: "shopresellerstory" },
        websiteform: { name: "websiteform" },
        StoreProductQuantity: { name: "StoreProductQuantity" },
        store: { name: "Store" },
        SendType: { name: "SendType" },
        competitive: { name: "CompetitiveFeature" },
        Language: { name: "Language" },
        Post: { name: "Post" },
        FirstMessage: { name: "Category" },
        shippingCity: { name: "ShippingCity" },
        location: { name: "Location" },
        map: "codeprocess-map",
        mapApi: { marker: "map-marker" },
    },
};
var apirequest = { type: { Post: "POST", Get: "GET", Put: "PUT", Delete: "DELETE" }, status: { Exception: 0, Success: 1, InvalidKey: 2, Error: 3 } };
var paymentTypes = { ONLINE: 1, PLACE: 2, CART: 3, ACCOUNT: 4 };
function ajaxLoader(state) {
    if (state) {
        $(".loadBox").removeClass("d-none");
    } else {
        $(".loadBox").addClass("d-none");
    }
}
function createMessage(type, msg) {
    if (msg === "undefined" || msg === undefined) return;
    var title = "";
    var request = createRequest();
    request.url = request.url + codeprocess.api.Language.name + "?name=" + type;
    request.success = function (result) {
        title = result.Value;
        Command: toastr[type](msg, title);
        toastr.options = {
            closeButton: true,
            debug: false,
            newestOnTop: false,
            progressBar: false,
            positionClass: "toast-bottom-left",
            preventDuplicates: false,
            onclick: null,
            showDuration: "500",
            hideDuration: "1000",
            timeOut: "1000",
            extendedTimeOut: "1000",
            showEasing: "swing",
            hideEasing: "linear",
            showMethod: "fadeIn",
            hideMethod: "fadeOut",
        };
    };
    $.ajax(request);
}
$(document).ready(function () {
    $(document).on("click", "[" + codeprocess.function + "]", function () {
        var btn = $(this);
        var callFunction = $(btn).attr(codeprocess.callback.name);
        var request = createRequest(callFunction);
        var functionName = $(btn).attr(codeprocess.function);
        if (functionName === codeprocess.api.shopfollow.follow) {
            doShopFollow(request, btn, callFunction);
        } else if (functionName === codeprocess.api.shopfollow.unfollow) {
            doShopUnFollow(request, btn, callFunction);
        } else if (functionName === codeprocess.api.profile.UploadPictureProfile) {
            UploadPictureProfile(btn, callFunction);
        } else if (functionName === codeprocess.api.basket.get) {
            doGetBasket(request, btn, callFunction);
        } else if (functionName === codeprocess.api.basket.add) {
            doAddBasket(request, btn, callFunction);
        } else if (functionName === codeprocess.api.basket.remove) {
            doRemoveBasket(request, btn, callFunction);
        } else if (functionName === codeprocess.api.basket.edit) {
            doEditBasket(request, btn, callFunction);
        } else if (functionName === codeprocess.api.address.add) {
            doAddAddress(request, btn, callFunction);
        } else if (functionName === codeprocess.api.address.remove) {
            doRemoveAddress(request, btn, callFunction);
        } else if (functionName === codeprocess.api.newsletter.add) {
            doAddNewsletter(request, btn, callFunction);
        } else if (functionName === codeprocess.api.contactUs.add) {
            doAddContactUs(request, btn, callFunction);
        } else if (functionName === codeprocess.api.contactUs.uploadPicture) {
            doUploadPicture(request, btn, callFunction);
        } else if (functionName === codeprocess.api.contactUs.uploadDocument) {
            doUploadDocument(request, btn, callFunction);
        } else if (functionName === codeprocess.api.invait.add) {
            doAddInvait(request, btn, callFunction);
        } else if (functionName === codeprocess.api.productlike.add || functionName === codeprocess.api.productlike.remove) {
            doProductLike(request, btn, callFunction);
        } else if (functionName === codeprocess.api.productnotify.name) {
            doProductNotify(request, btn, callFunction);
        } else if (functionName === codeprocess.api.productcomment.add) {
            doProductComment(request, btn, callFunction);
        } else if (functionName === codeprocess.api.storecomment.add) {
            doStoreComment(request, btn, callFunction);
        } else if (functionName === codeprocess.api.login.name) {
            doAccountLogin(request, btn, callFunction);
        } else if (functionName === codeprocess.api.register.name) {
            doAccountRegister(request, btn, callFunction);
        } else if (functionName === codeprocess.api.account.get) {
            doGetCurrentAccount(request, btn, callFunction);
        } else if (functionName === codeprocess.api.rebate.name) {
            doCheckRebate(request, btn, callFunction);
        } else if (functionName === codeprocess.api.rebate.remove) {
            doRemoveRebate(request, btn, callFunction);
        } else if (functionName === codeprocess.api.rebate.sms) {
            doRebateSms(request, btn, callFunction);
        } else if (functionName === codeprocess.api.reagent.name) {
            doOrderReagent(request, btn, callFunction);
        } else if (functionName === codeprocess.api.contactform.name) {
            doContactForm(request, btn, callFunction);
        } else if (functionName === codeprocess.api.account.edit) {
            doAccountEdit(request, btn, callFunction);
        } else if (functionName === codeprocess.api.account.changepassword) {
            doAccountChangePassword(request, btn, callFunction);
        } else if (functionName === codeprocess.api.account.changeusername) {
            doAccountChangeUserName(request, btn, callFunction);
        } else if (functionName === codeprocess.api.credit.name || functionName === codeprocess.api.payment.name) {
            doAccountPayment(request, btn, callFunction);
        } else if (functionName === codeprocess.api.order.tracking) {
            doAccountOrderTracking(request, btn, callFunction);
        } else if (functionName === codeprocess.api.websiteform.name) {
            doWebsiteFormSubmit(request, btn, callFunction);
        } else return;
        $.ajax(request);
    });
    var maps = $("[" + codeprocess.map + "]");
    if (maps.length > 0) {
        for (var i = 0; i < maps.length; i++) {
            var mapEntity = maps[i];
            var mapFunction = $(mapEntity).attr(codeprocess.map);
            if (mapFunction === codeprocess.mapApi.marker) {
            }
        }
        google.maps.event.addDomListener(window, "load", initialize);
    }
});
function doWebsiteFormSubmit(request, btn, callback) {
    var formName = $(btn).attr(codeprocess.field.websiteform.formId);
    request.url = request.url + codeprocess.api.websiteform.name;
    request.type = apirequest.type.Post;
    request.data = JSON.stringify(getFormContent(formName));
    request.success = function (result) {
        if (doApiCallBack(result, callback) === true) {
            DeleteFormContent(formName);
        }
    };
}
function doAccountEdit(request, btn, callback) {
    request.url = request.url + codeprocess.api.account.name;
    request.type = apirequest.type.Put;
    request.data = ko.toJSON(viewModel.Account);
    request.success = function (result) {
        if (doApiCallBack(result, callback) === true) {
            updateViewModelAccount(result);
        }
    };
}
function doAccountChangePassword(request, btn, callFunction) {
    var old_password = getCodeprocessValue(codeprocess.field.account.password);
    var new_password = getCodeprocessValue(codeprocess.field.newPassword);
    var confirm_password = getCodeprocessValue(codeprocess.field.confirmPassword);
    request.url = request.url + codeprocess.api.account.name + "?oldPassword=" + old_password + "&newPassword=" + new_password + "&confirmPassword=" + confirm_password;
    request.type = apirequest.type.Put;
    request.success = function (result) {
        doApiCallBack(result, callFunction);
    };
}
function doAccountChangeUserName(request, btn, callFunction) {
    var username = getCodeprocessValue(codeprocess.field.account.userName);
    request.url = request.url + codeprocess.api.account.name + "?userName=" + username;
    request.type = apirequest.type.Put;
    request.success = function (result) {
        doApiCallBack(result, callFunction);
        if (result.Code === apirequest.status.Success) {
            $("#dvUserName").hide();
            $(".myusername").html(username.trim().replace(" ", ""));
        }
    };
}
function doHideUserNameDiv() { }
function doAccountOrderTracking(request, btn, callFunction) {
    var recaptchaValue = $("[name='" + codeprocess.field.google.recaptcha + "']").val();
    var trackingCode = $("[" + codeprocess.field.attrName + "='" + codeprocess.field.order.tracking.code + "']").val();
    request.type = apirequest.type.Post;
    request.url = request.url + codeprocess.api.order.tracking + "?code=" + trackingCode;
    request.success = function (result) {
        if (doApiCallBack(result, callFunction) === true) {
            var orderInfo = result.Value;
            var html = "شماره سفارش: " + orderInfo.Id + "، مشتری: " + orderInfo.Account.FullName + "، وضعیت سفارش: " + orderInfo.Status.Name;
            $("[" + codeprocess.field.attrName + "='" + codeprocess.field.order.tracking.result + "']").html(html);
        }
    };
}
function doOrderReagent(request, btn, callFunction) {
    var recaptchaValue = $("[name='" + codeprocess.field.google.recaptcha + "']").val();
    var fullName = getCodeprocessValue(codeprocess.field.reagent.fullName);
    var mobile = getCodeprocessValue(codeprocess.field.reagent.mobile);
    var email = getCodeprocessValue(codeprocess.field.reagent.email);
    request.type = apirequest.type.Post;
    request.url = request.url + codeprocess.api.reagent.name + "?fullName=" + fullName + "&mobile=" + mobile + "&email=" + email + "&recaptcha=" + recaptchaValue;
    request.success = function (result) {
        if (doApiCallBack(result, callFunction) === true) {
            $("[" + codeprocess.field.attrName + "='" + codeprocess.field.reagent.fullName + "']").val(null);
            $("[" + codeprocess.field.attrName + "='" + codeprocess.field.reagent.email + "']").val(null);
            $("[" + codeprocess.field.attrName + "='" + codeprocess.field.reagent.mobile + "']").val(null);
        }
    };
}
function doAccountPayment(request, btn, callFunction) {
    var entity = { PaymentSubject: { Label: "CREDIT" }, Description: "طرح فلان", Price: 1000 };
    request.data = JSON.stringify(entity);
    request.type = apirequest.type.Post;
    request.url = request.url + codeprocess.api.payment.name;
    request.success = function (result) {
        if (doApiCallBack(result, callFunction) === true) {
            document.location = result.Value.Url;
        } else {
            alert("ERROR");
        }
    };
}
function doContactForm(request, btn, callFunction) {
    request.data = ko.toJSON(viewModel.ContactForm);
    request.type = apirequest.type.Post;
    request.url = request.url + codeprocess.api.contactform.name;
    request.success = function (result) {
        if (doApiCallBack(result, callFunction) === true) {
            viewModel.ContactForm.FullName(null);
            viewModel.ContactForm.Subject(null);
            viewModel.ContactForm.Email(null);
            viewModel.ContactForm.Body(null);
            viewModel.ContactForm.Mobile(null);
        }
    };
}
function doRebateSms(request, btn, callFunction) {
    var rebateMobile = getCodeprocessValue(codeprocess.field.rebateMobile);
    request.type = apirequest.type.Put;
    request.url = request.url + codeprocess.api.rebate.name + "?rebateMobile=" + rebateMobile;
    request.success = function (result) {
        if (doApiCallBack(result, callFunction) === true) {
            $("[" + codeprocess.field.attrName + "='" + codeprocess.field.rebateMobile + "']").val(null);
        }
    };
}
function doCheckRebate(request, btn, callFunction) {
    var rebateValue = getCodeprocessValue(codeprocess.field.rebateValue);
    request.type = apirequest.type.Get;
    request.url = request.url + codeprocess.api.rebate.name + "?rebateValue=" + rebateValue;
    request.success = function (result) {
        viewModel.Basket.Rebate.Message(result.Message);
        if (result.Code === apirequest.status.Success) {
            viewModel.Basket.Rebate.Valid(true);
            viewModel.Basket.Rebate.Price(result.Value);
        } else {
            viewModel.Basket.Rebate.Valid(false);
            viewModel.Basket.Rebate.Price(0);
        }
    };
}
function doRemoveRebate(request, btn, callFunction) {
    var orderId = $(btn).attr(codeprocess.field.orderId);
    request.type = apirequest.type.Delete;
    request.url = request.url + codeprocess.api.rebate.name + "?orderId=" + orderId;
    request.success = function (result) {
        if (doApiCallBack(result, callFunction) === true) {
            document.location = document.location;
        }
    };
}
function doAccountRegister(request, btn, callFunction) {
    var confirm = getCodeprocessValue(codeprocess.field.account.confirm);
    if (confirm.length < 1) {
        confirm = viewModel.Account.ConfirmPassword();
    }
    var captcha = getCodeprocessValue(codeprocess.field.account.Captcha);
    var bothEmailMobile = getCodeprocessValue(codeprocess.field.account.bothEmailMobile);
    request.data = ko.toJSON(viewModel.Account);
    request.type = apirequest.type.Post;
    var session = '@Session["Captcha"]';
    request.url = request.url + codeprocess.api.register.name + "?remember=true&confirm=" + confirm + "&captcha=" + captcha + "&bothEmailMobile=" + bothEmailMobile;
    request.success = function (result) {
        if (doApiCallBack(result, callFunction) === true) {
            updateViewModelAccount(result);
        }
    };
}
function doAccountLogin(request, btn, callFunction) {
    var recaptchaValue = $("[name='" + codeprocess.field.google.recaptcha + "']").val();
    var remember = ko.toJSON(viewModel.Account.remember);
    request.data = getAccountEntity();
    request.type = apirequest.type.Post;
    request.url = request.url + codeprocess.api.login.name + "?remember=" + remember + "&&recaptcha=" + recaptchaValue;
    request.success = function (result) {
        if (doApiCallBack(result, callFunction) === true) {
            updateViewModelAccount(result);
        }
    };
}
function getFormContent(formName) {
    var entity = { Label: "", FormValues: [] };
    var formLabel = $("#" + formName)
        .find("[name='FormLabel']")
        .val();
    entity.Label = formLabel;
    var fields = $("#" + formName).find("[form-field-id]");
    for (var i = 0; i < fields.length; i++) {
        entity.FormValues.push({ ColumnId: $(fields[i]).attr("form-field-id"), Value: $(fields[i]).val() });
    }
    return entity;
}
function DeleteFormContent(formName) {
    var fields = $("#" + formName).find("[form-field-id]");
    for (var i = 0; i < fields.length; i++) {
        var type = $(fields[i]).attr("type");
        if (type == "checkbox") {
            $(fields[i]).prop("checked", false);
        } else {
            $(fields[i]).val("");
        }
    }
}
function getAccountEntity() {
    return ko.toJSON(viewModel.Account);
}
function getCodeprocessValue(fieldName) {
    var cprObject = $("[" + codeprocess.field.attrName + "='" + fieldName + "']");
    var value = $(cprObject).val();
    return value === undefined ? null : value;
}
function doSearchProduct() {
    debugger
    viewModel.SearchResult.Pending(true);
    var request = createRequest();
    request.url = request.url + codeprocess.api.product.name;
    request.url = request.url + "?pageSize=" + viewModel.SearchResult.PageSize();
    request.url = request.url + "&index=" + viewModel.SearchResult.PageIndex();
    request.url = request.url + "&name=" + viewModel.SearchResult.Name();
    request.url = request.url + "&typeId=" + viewModel.SearchResult.ProductType();
    request.url = request.url + "&categoryId=" + viewModel.SearchResult.ProductCategory();
    request.url = request.url + "&subCategoryId=" + viewModel.SearchResult.ProductSubCategory();
    request.url = request.url + "&brandId=" + viewModel.SearchResult.ProductBrand();
    request.url = request.url + "&isService=" + viewModel.SearchResult.isService();
    request.url = request.url + "&activeLocation=false";
    request.success = function (result) {
        var resultList = result.Value.Products;
        viewModel.SearchResult.Results(resultList);
    };
    request.complete = function () {
        viewModel.SearchResult.Pending(false);
    };
    $.ajax(request);
}
function doSearchStore() {
    var request = createRequest();
    request.url = request.url + codeprocess.api.store.name;
    request.url = request.url + "?pageSize=" + viewModel.StoreSearchResult.PageSize();
    request.url = request.url + "&index=" + viewModel.StoreSearchResult.PageIndex();
    request.url = request.url + "&name=" + viewModel.StoreSearchResult.Name();
    request.success = function (result) {
        var resultList = result.Value;
        viewModel.StoreSearchResult.Results(resultList);
    };
    $.ajax(request);
}
function doSearchPost() {
    viewModel.PostSearchResult.Pending(true);

    var request = createRequest();
    request.url = request.url + codeprocess.api.PostSearch.name;
    request.url = request.url + "?pageSize=" + viewModel.PostSearchResult.PageSize();
    request.url = request.url + "&index=" + viewModel.PostSearchResult.PageIndex();
    request.url = request.url + "&name=" + viewModel.PostSearchResult.Name();
    request.url = request.url + "&categoryLabel=" + viewModel.PostSearchResult.Category();
    request.url = request.url + "&typeId=" + viewModel.PostSearchResult.TypeId();
    request.success = function (result) {
        debugger
        var resultList = result.Value;
        viewModel.PostSearchResult.Results(resultList);
    };
    request.complete = function () {
        viewModel.PostSearchResult.Pending(false);
    };
    $.ajax(request);
}
function doCompareSearchProduct() {
    var request = createRequest();
    request.url = request.url + codeprocess.api.product.name;
    request.url = request.url + "?pageSize=" + viewModel.CompareSearchResult.PageSize();
    request.url = request.url + "&index=" + viewModel.CompareSearchResult.PageIndex();
    request.url = request.url + "&name=" + viewModel.CompareSearchResult.Name();
    request.url = request.url + "&typeId=" + viewModel.CompareSearchResult.ProductType();
    request.url = request.url + "&categoryId=" + viewModel.CompareSearchResult.ProductCategory();
    request.url = request.url + "&subCategoryId=" + viewModel.CompareSearchResult.ProductSubCategory();
    request.url = request.url + "&brandId=" + viewModel.CompareSearchResult.ProductBrand();
    request.url = request.url + "&isService=" + viewModel.CompareSearchResult.isService();
    request.success = function (result) {
        var resultList = result.Value.Products;
        viewModel.CompareSearchResult.Results(resultList);
    };
    $.ajax(request);
}
function doProductComment(request, btn, callFunction) {
    debugger;
    var productId = $(btn).attr("product-id");
    var rate = $("[name='" + codeprocess.field.rate + "']:checked").val();
    if (rate == undefined) {
        rate = null;
    }
    request.type = apirequest.type.Post;
    var entity = {
        Product: { Id: productId },
        NameFamily: $("[" + codeprocess.field.attrName + "='" + codeprocess.field.nameFamily + "']").val(),
        Body: $("[" + codeprocess.field.attrName + "='" + codeprocess.field.body + "']").val(),
        Rate: rate
    };
    request.data = JSON.stringify(entity);
    request.url = request.url + codeprocess.api.productcomment.name;
    request.success = function (result) {
        if (doApiCallBack(result, callFunction) === true) {
            $("[" + codeprocess.field.attrName + "='" + codeprocess.field.body + "']").val("");
        }
    };
}
function doStoreComment(request, btn, callFunction) {
    var storeId = $(btn).attr("store-id");
    var rate = $("[name='" + codeprocess.field.rate + "']:checked").val();
    if (rate == undefined) {
        rate = null;
    }
    request.type = apirequest.type.Post;
    var entity = {
        Store: { Id: storeId },
        NameFamily: $("[" + codeprocess.field.attrName + "='" + codeprocess.field.nameFamily + "']").val(),
        Body: $("[" + codeprocess.field.attrName + "='" + codeprocess.field.body + "']").val(),
        Rate: rate,
    };
    request.data = JSON.stringify(entity);
    request.url = request.url + codeprocess.api.storecomment.name;
    request.success = function (result) {
        if (doApiCallBack(result, callFunction) === true) {
            $("[" + codeprocess.field.attrName + "='" + codeprocess.field.body + "']").val("");
        }
    };
}
function doAddNewsletter(request, btn, callback) {
    request.data = ko.toJSON(viewModel.NewsLetter);
    request.type = apirequest.type.Post;
    request.url = request.url + codeprocess.api.newsletter.name;
    request.success = function (result) {
        if (doApiCallBack(result, callback) === true) {
            viewModel.NewsLetter.Phone(null);
            viewModel.NewsLetter.Email(null);
        }
    };
}
function doAddContactUs(request, btn, callback) {
    debugger
    $(".dv-require").hide();
    var required = false;
    $(".contactus-form [required]").each(function (index) {
        if ($(this).val().length === 0) {
            required = true;
        }
    });
    if (required) {
        GetNotifyMsg("error", lang.notify.PleaseRequired);
    } else {
        var fields = $(".contactus-form .contactus-field");
        fields.each(function (index) {
            var field = { Name: "", Value: "" };
            field.Name = $(this).attr("name");
            field.Value = $(this).val();
            viewModel.ContactUs.ContactUsField.push(field);
        });
        if ($(".contactus-form [name=documentId]").val() != undefined) {
            viewModel.ContactUs.DocumentId($(".contactus-form [name=documentId]").val());
        }
        if ($(".contactus-form [name=pictureId]").val() != undefined) {
            viewModel.ContactUs.PictureId($(".contactus-form [name=pictureId]").val());
        }
        var Div = $(".contactus-form");
        var ccode = Div.attr("ccode");
        if (ccode !== undefined && ccode !== "") {
            viewModel.ContactUs.Phone(ccode + viewModel.ContactUs.Phone());
        }
        request.data = ko.toJSON(viewModel.ContactUs);
        request.type = apirequest.type.Post;
        request.url = request.url + codeprocess.api.contactUs.name;
        request.success = function (result) {
            if (doApiCallBack(result, callback) === true) {
                debugger
                viewModel.ContactUs.Phone(null);
                viewModel.ContactUs.Email(null);
                viewModel.ContactUs.Title(null);
                viewModel.ContactUs.Body(null);
                viewModel.ContactUs.Name(null);
                viewModel.ContactUs.PictureId(null);
                viewModel.ContactUs.DocumentId(null);
                $(".contactus-form .contactus-field").val("");
                $(".contactus-form .contactus-checkbox").prop("checked", false);
                $(".upload-file").val("");
                $("#FileUpload").addClass("d-none");
                $("#FileUpload").attr("href", "");

                viewModel.ContactUs.ContactUsField([]);
                fields.each(function (index) {
                    $(this).val("");
                });
            }
        };
    }
}
function doAddInvait(request, btn, callback) {
    request.type = apirequest.type.Post;
    request.url = request.url + codeprocess.api.invait.name + "?emailOrPhone=" + viewModel.Invait.EmailOrPhone();
    request.success = function (result) {
        if (doApiCallBack(result, callback) === true) {
            viewModel.Invait.EmailOrPhone(null);
        }
    };
}
function doAddAccountLogVisit(sourceId, pageName) {
    var request = createRequest();
    request.type = apirequest.type.Get;
    request.url = request.url + codeprocess.api.accountLogVisit.name + "?sourceId=" + sourceId + "&pageName=" + pageName;
    $.ajax(request);
}
function doCheckAccountLogVisit(sId, pName) {
    var request = createRequest();
    request.type = apirequest.type.Get;
    request.url = request.url + codeprocess.api.accountLogVisit.name + "/" + codeprocess.api.accountLogVisit.check + "?sId=" + sId + "&pName=" + pName;
    request.success = function (result) {
        viewModel.AccountLogVisit.CheckLogVisit(result);
        if (result === false) {
            GetFirstMessage();
            doAddAccountLogVisit(sId, pName);
        }
    };
    $.ajax(request);
}
function GetFirstMessage() {
    var request = createRequest();
    request.type = apirequest.type.Get;
    request.url = request.url + codeprocess.api.FirstMessage.name + "?keyWord=FirstMessage";
    request.success = function (result) {
        if (result.Code == apirequest.status.Success) {
            viewModel.FirstMessage.Body(result.Value.Body);
            viewModel.FirstMessage.Picture(result.Value.Picture);
            $("#ModalPopUp").modal("show");
        }
    };
    $.ajax(request);
}
function doShopFollow(request, btn, callback) {
    var shopId = $(btn).attr("shop-id");
    request.type = apirequest.type.Post;
    request.url = request.url + codeprocess.api.shopfollow.name + "?shopId=" + shopId;
    request.success = function (result) {
        if (doApiCallBack(result, callback) === true) {
            $(btn).attr(codeprocess.function, codeprocess.api.shopfollow.unfollow);
        }
    };
}
function UploadPictureProfile(btn, callback) {
    var accountPictureId = $(btn).attr("account-PicturId");
    UploadPicture(accountPictureId);
}
function UploadPicture(accountPictureId) {
    var request = createRequest();
    request.type = "Post";
    request.url = request.url + codeprocess.api.profile.UploadPictureProfile + "?&pictureId=" + accountPictureId;
    request.success = function (result) { };
    $.ajax(request);
}
function DeletePictureProfile() {
    var request = createRequest();
    request.type = apirequest.type.Delete;
    request.url = request.url + codeprocess.api.profile.UploadPictureProfile;
    request.success = function (result) { };
    $.ajax(request);
}
function doShopUnFollow(request, btn, callback) {
    var shopId = $(btn).attr("shop-id");
    request.type = apirequest.type.Delete;
    request.url = request.url + codeprocess.api.shopfollow.name + "?shopId=" + shopId;
    request.success = function (result) {
        if (doApiCallBack(result, callback) === true) {
            $(btn).attr(codeprocess.function, codeprocess.api.shopfollow.follow);
        }
    };
}
function doAddBasket(request, btn, callback) {
    var productId = $(btn).attr("product-id");
    var productColorId = $(btn).attr("product-color-id");
    var productSizeId = $(btn).attr("product-size-id");
    var productOptionItemId = $(btn).attr("product-option-item-id");
    var resellerId = $(btn).attr("reseller-id");
    var storeId = $(btn).attr("store-id");
    var count = $(".addcount").val();
    console.log(count);
    request.type = apirequest.type.Post;
    request.url =
        request.url +
        codeprocess.api.basket.name +
        "?productId=" +
        productId +
        "&colorId=" +
        productColorId +
        "&sizeId=" +
        productSizeId +
        "&resellerId=" +
        resellerId +
        "&storeId=" +
        storeId +
        "&count=" +
        count +
        "&optionItemId=" +
        productOptionItemId;
    request.success = function (result) {
        if (doApiCallBack(result, callback) === true) {
            ShowModalFinishBasket();
            updateViewModelBasket(result);
        }
        doGetStoreBasket();
    };
}
function doEditBasketAjax(data, event) {
    var request = createRequest();
    var btn = $(event.target);
    var callback = $(btn).attr(codeprocess.callback.name);
    doEditBasket(request, btn, callback);
    $.ajax(request);
}
function doEditBasket(request, btn, callback) {
    var basketId = $(btn).attr(codeprocess.field.basketId);
    var actionType = $(btn).attr(codeprocess.field.actionType.name);
    var productId = $(btn).attr("product-id");
    var productColorId = $(btn).attr("product-color-id") == undefined ? null : $(btn).attr("product-color-id");
    var productSizeId = $(btn).attr("product-size-id") == undefined ? null : $(btn).attr("product-size-id");
    var ProductOptionItem = $(btn).attr("product-option-item-id") == undefined ? null : $(btn).attr("product-option-item-id");
    var storeId = $(btn).attr("store-id") == undefined ? null : $(btn).attr("store-id");
    if (basketId == undefined) {
        basketId = $(btn).parents().attr(codeprocess.field.basketId);
        actionType = $(btn).parents().attr(codeprocess.field.actionType.name);
        productId = $(btn).parents().attr("product-id");
        productColorId = $(btn).parents().attr("product-color-id") == undefined ? null : $(btn).parents().attr("product-color-id");
        productSizeId = $(btn).parents().attr("product-size-id") == undefined ? null : $(btn).parents().attr("product-size-id");
        storeId = $(btn).parents().attr("store-id") == undefined ? null : $(btn).parents().attr("store-id");
        if (basketId == undefined) {
            basketId = $(btn).parents().parents().attr(codeprocess.field.basketId);
            actionType = $(btn).parents().parents().attr(codeprocess.field.actionType.name);
            productId = $(btn).parents().parents().attr("product-id");
            productColorId = $(btn).parents().parents().attr("product-color-id") == undefined ? null : $(btn).parents().parents().attr("product-color-id");
            productSizeId = $(btn).parents().parents().attr("product-size-id") == undefined ? null : $(btn).parents().parents().attr("product-size-id");
            storeId = $(btn).parents().parents().attr("store-id") == undefined ? null : $(btn).parents().parents().attr("store-id");
            ProductOptionItem = $(btn).parents().parents().attr("product-option-item-id") == undefined ? null : $(btn).parents().parents().attr("product-option-item-id");
        }
    }
    request.type = apirequest.type.Put;
    request.url =
        request.url +
        codeprocess.api.basket.name +
        "?basketId=" +
        basketId +
        "&actionType=" +
        actionType +
        "&productId=" +
        productId +
        "&colorId=" +
        productColorId +
        "&sizeId=" +
        productSizeId +
        "&optionItemId=" +
        ProductOptionItem +
        "&storeId=" +
        storeId;
    request.success = function (result) {
        if (doApiCallBack(result, callback, "no-message") === true) {
            updateViewModelBasket(result);
        } else {
            doApiCallBack(result, callback, result.Message);
        }
        doGetStoreBasket();
    };
}
function doRemoveBasketAjax(data, event) {
    var btn = $(event.target);
    if ($(btn).attr("product-id") == undefined) {
        btn = $(event.target).parent();
    }
    var basketId = data.Id;
    var request = createRequest();
    var callFunction = $(btn).attr(codeprocess.callback.name);
    doRemoveBasket(request, btn, callFunction);
    $.ajax(request);
}
function doRemoveBasket(request, btn, callback) {
    var basketId = $(btn).attr(codeprocess.field.basketId);
    var productId = $(btn).attr("product-id");
    var productColorId = $(btn).attr("product-color-id") == undefined ? null : $(btn).attr("product-color-id");
    var productSizeId = $(btn).attr("product-size-id") == undefined ? null : $(btn).attr("product-size-id");
    var ProductOptionItem = $(btn).attr("product-option-item-id") == undefined ? null : $(btn).attr("product-option-item-id");
    var storeId = $(btn).attr("store-id") == undefined ? null : $(btn).attr("store-id");
    if (basketId == undefined) {
        basketId = $(btn).parents().attr(codeprocess.field.basketId);
        actionType = $(btn).parents().attr(codeprocess.field.actionType.name);
        productId = $(btn).parents().attr("product-id");
        productColorId = $(btn).parents().attr("product-color-id") == undefined ? null : $(btn).parents().attr("product-color-id");
        productSizeId = $(btn).parents().attr("product-size-id") == undefined ? null : $(btn).parents().attr("product-size-id");
        storeId = $(btn).parents().attr("store-id") == undefined ? null : $(btn).parents().attr("store-id");
        if (basketId == undefined) {
            basketId = $(btn).parents().parents().attr(codeprocess.field.basketId);
            actionType = $(btn).parents().parents().attr(codeprocess.field.actionType.name);
            productId = $(btn).parents().parents().attr("product-id");
            productColorId = $(btn).parents().parents().attr("product-color-id") == undefined ? null : $(btn).parents().parents().attr("product-color-id");
            productSizeId = $(btn).parents().parents().attr("product-size-id") == undefined ? null : $(btn).parents().parents().attr("product-size-id");
            storeId = $(btn).parents().parents().attr("store-id") == undefined ? null : $(btn).parents().parents().attr("store-id");
            ProductOptionItem = $(btn).parents().parents().attr("product-option-item-id") == undefined ? null : $(btn).parents().parents().attr("product-option-item-id");
        }
    }
    request.type = apirequest.type.Delete;
    request.url = request.url + codeprocess.api.basket.name + "?basketId=" + basketId + "&productId=" + productId + "&colorId=" + productColorId + "&sizeId=" + productSizeId + "&optionItemId=" + ProductOptionItem + "&storeId=" + storeId;
    request.success = function (result) {
        if (doApiCallBack(result, callback, "no-message") === true) {
            updateViewModelBasket(result);
        }
        doGetStoreBasket();
    };
}
function doRemoveBasketAll(request, btn, callback) {
    request.type = apirequest.type.Delete;
    request.url = request.url + codeprocess.api.basket.name;
    request.success = function (result) {
        if (doApiCallBack(result, callback, "no-message") === true) {
            updateViewModelBasket(result);
        }
    };
}
function clearViewModelAccountAddressItem() {
    var data = { Id: 0, NameFamily: "", Mobile: "", NationalCode: "", AddressValue: "", Phone: "", Latitude: "", Longitude: "", State: { Id: "8", Name: "تهران" }, City: { Id: "87", Name: "تهران" }, Country: { Id: "118", Name: "ایران" } };
    updateViewModelAccountAddressItem(data);
}
function updateViewModelAccountAddressItem(data) {
    viewModel.Account.Addresses.Item.ID(data.Id);
    viewModel.Account.Addresses.Item.NameFamily(data.NameFamily);
    viewModel.Account.Addresses.Item.NationalCode(data.NationalCode);
    viewModel.Account.Addresses.Item.AddressValue(data.AddressValue);
    viewModel.Account.Addresses.Item.Mobile(data.Mobile);
    viewModel.Account.Addresses.Item.Phone(data.Phone);
    viewModel.Account.Addresses.Item.PostalCode(data.PostalCode);
    viewModel.Account.Addresses.Item.State(data.State.Id + "-" + data.State.Name);
    viewModel.Account.Addresses.Item.CountryId(118);
    viewModel.Account.Addresses.Item.StateId(data.State.Id);
    viewModel.Account.Addresses.Item.CityId(data.City.Id);
    viewModel.Account.Addresses.Item.Latitude(data.Latitude);
    viewModel.Account.Addresses.Item.Longitude(data.Longitude);
    viewModel.Account.Addresses.Item.LegalUser(data.LegalUser);
}
function updateViewModelBasket(result, gotopaybasket) {
    if (result.Value == null) {
        viewModel.Basket.Items([]);
    } else if (result.Value.length == 0) {
        viewModel.Basket.Items([]);
    } else if (result.Value.List != null) {
        viewModel.Basket.Items(result.Value.List);
    }
    GetOuOfSendStore(gotopaybasket);
}
function updateViewModelStoreBasket(result) {
    viewModel.Basket.StoreItems.List(result.Value.List);
    viewModel.Basket.StoreItems.TotalDiscount(result.Value.TotalDiscount);
    viewModel.Basket.StoreItems.TotalPrice(result.Value.TotalPrice);
    viewModel.Basket.StoreItems.ShippingPrice(result.Value.ShippingPrice);
    viewModel.Basket.StoreItems.Count(result.Value.Count);
}
function updateViewModelProduct(result) {
    viewModel.Product.Id(result.Id);
    viewModel.Product.Name(result.Name);
    viewModel.Product.CodeValue(result.CodeValue);
    viewModel.Product.Summary(result.Summary);
    viewModel.Product.Description(result.Description);
    if (result.ProductType != null) {
        viewModel.Product.ProductType.Name(result.ProductType.Name);
    }
    if (result.ProductCategory != null) {
        viewModel.Product.ProductCategory.Name(result.ProductCategory.Name);
    }
    if (result.ProductSubCategory != null) {
        viewModel.Product.ProductSubCategory.Name(result.ProductSubCategory.Name);
    }
    if (result.Picture != null) {
        viewModel.Product.Picture.Id(result.Picture.Id);
        viewModel.Product.Picture.Url(result.Picture.Url);
    }
    viewModel.Product.ProductBrand(result.ProductBrand);
    viewModel.Product.Items(result.Items);
    viewModel.Product.LikesCount(result.LikesCount);
    viewModel.Product.Status.Name(result.Status.Name);
    viewModel.Product.Status.Label(result.Status.Label);
    viewModel.Product.LastPrice(result.LastPrice);
    viewModel.Product.Price(result.Price);
    viewModel.Product.DiscountPrice(result.DiscountPrice);
    viewModel.Product.Document(result.Document);
    viewModel.Product.Pictures(result.Pictures);
    viewModel.Product.Colors(result.Colors);
    viewModel.Product.Sizes(result.Sizes);
    viewModel.Product.Active(result.Active);
}
function updateViewModelAccount(result) {
    var entity = result.Value;
    viewModel.Account.Address(entity.Address);
    viewModel.Account.Company(entity.Company);
    viewModel.Account.CompanyNo(entity.CompanyNo);
    viewModel.Account.Email(entity.Email);
    viewModel.Account.FullName(entity.FullName);
    viewModel.Account.Job(entity.Job);
    viewModel.Account.Mobile(entity.Mobile);
    viewModel.Account.NationalCode(entity.NationalCode);
    viewModel.Account.Phone(entity.Phone);
    viewModel.Account.Agent(entity.Agent);
    viewModel.Account.AgentPhone(entity.AgentPhone);
    viewModel.Account.IsMale(entity.IsMale);
    viewModel.Account.IsOnline(true);
    viewModel.Account.StoreId(entity.StoreId);
    viewModel.Account.FaBirthDay(entity.FaBirthDay);
    viewModel.Account.CardNumber(entity.CardNumber);
    viewModel.Account.Sheba(entity.Sheba);
    viewModel.Account.PictureId(entity.PictureId);
    viewModel.Account.PictureUrl(entity.PictureUrl);
    viewModel.Account.NationalPictureId(entity.NationalPictureId);
    viewModel.Account.NationalPictureUrl(entity.NationalPictureUrl);
    viewModel.Account.FullNameEn(entity.FullNameEn);
    viewModel.Account.CityId(entity.CityId);
    viewModel.Account.StateId(entity.StateId);
    viewModel.Account.CountryId(entity.CountryId);
    if (entity.StoreId) {
        viewModel.Account.Store.Id(entity.Store.ID);
        viewModel.Account.Store.Name(entity.Store.Name);
        viewModel.Account.Store.Approve(entity.Store.Approve);
        viewModel.Account.Store.Active(entity.Store.Active);
    }
}
function updateViewModelShopReseller(result) {
    var reseller = result.Value;
    viewModel.ShopReseller.Name(reseller.Name);
    viewModel.ShopReseller.Website(reseller.Website);
    if (reseller.Picture != null) {
        viewModel.ShopReseller.Picture.Id(reseller.Picture.Id);
        viewModel.ShopReseller.Picture.Url(reseller.Picture.Url);
    }
}
function doAddAddress(request, btn, callback) {
    debugger
    if (viewModel.Account.Addresses.Item.ID() == 0) {
        request.type = apirequest.type.Post;
    } else {
        request.type = apirequest.type.Put;
    }
    request.url = request.url + codeprocess.api.address.name + "?returnAddresses=false";
    request.data = ko.toJSON(viewModel.Account.Addresses.Item);
    request.success = function (result) {
        if (doApiCallBack(result, callback) === true) {
            UpdateLocation();
            clearViewModelAccountAddressItem();
        }
    };
}
function doRemoveAddress(request, btn, callback) {
    var addressId = $(btn).attr("address-id");
    request.type = apirequest.type.Delete;
    request.url = request.url + codeprocess.api.address.name + "?addressId=" + addressId;
    request.success = function (result) {
        if (doApiCallBack(result, callback) === true) {
            viewModel.Account.Addresses.List(result.Value);
            if (viewModel.Account.Addresses.List().length > 0) {
                viewModel.Basket.Address.AddressId(viewModel.Account.Addresses.List()[0].Id);
            } else {
                viewModel.Basket.Address.AddressId(0);
            }
            doGetAccountAddresses();
            UpdateLocation();
        }
    };
}
function doAddBasketAjax(data, event) {
    var request = createRequest();
    var btn = $(event.target).parent();
    var productId = $(btn).attr("product-id");
    if (productId == undefined) {
        btn = $(btn).parent();
    }
    var callback = $(btn).attr(codeprocess.callback.name);
    doAddBasket(request, btn, callback);
    $.ajax(request);
}
function doProductLikeAjax(data, event) {
    var request = createRequest();
    var btn = $(event.target).parent();
    var productId = $(btn).attr("product-id");
    if (productId == undefined) {
        btn = $(btn).parent();
    }
    var callback = $(btn).attr(codeprocess.callback.name);
    doProductLike(request, btn, callback);
    $.ajax(request);
}
function doProductLike(request, btn, callback) {
    var productId = $(btn).attr("product-id");
    var functionName = $(btn).attr(codeprocess.function);
    if (functionName === codeprocess.api.productlike.add) {
        request.type = apirequest.type.Post;
        request.url = request.url + codeprocess.api.productlike.name + "?productId=" + productId;
        request.success = function (result) {
            if (doApiCallBack(result, callback) === true) {
                $(btn).attr(codeprocess.function, codeprocess.api.productlike.add);
                console.log(result.Value);
                viewModel.Account.Favorites(result.Value);
            }
        };
    } else {
        request.type = apirequest.type.Delete;
        request.url = request.url + codeprocess.api.productlike.name + "?productId=" + productId;
        request.success = function (result) {
            if (doApiCallBack(result, callback) === true) {
                $(btn).attr(codeprocess.function, codeprocess.api.productlike.remove);
                viewModel.Account.Favorites(result.Value);
            }
        };
    }
}
function doProductNotify(request, btn, callback) {
    var productId = $(btn).attr("product-id");
    request.type = apirequest.type.Post;
    request.url = request.url + codeprocess.api.productnotify.name + "?productId=" + productId;
}
function doApiCallBack(result, callback, msg) {
    if (result.Code === apirequest.status.Exception) {
        if (msg != "no-message") createMessage("error", "خطای سرور");
        return false;
    } else if (result.Code === apirequest.status.Success) {
        if (msg != "no-message") createMessage("success", result.Message);
        doCodeprocessCallback(callback);
        return true;
    } else if (result.Code === apirequest.status.InvalidKey) {
        var login = "<a href='/login?back=" + window.location.pathname + "'>باید ابتدا در سیستم وارد شوید</a>";
        if (msg != "no-message") createMessage("error", result.Message);
        return false;
    } else if (result.Code === apirequest.status.Error) {
        if (msg != "no-message") createMessage("error", result.Message);
        return false;
    }
}
function createRequest(callbackFunctions) {
    var response = {
        url: "/cpr/",
        type: "GET",
        dataType: "json",
        cache: false,
        contentType: "application/json; charset=utf-8",
        error: function (a) {
            createMessage("error", a.Message);
        },
        success: function (result) {
            doApiCallBack(result);
            doCodeprocessCallback(callbackFunctions);
        },
        beforeSend: function () {
            ajaxUpdateOn();
        },
        complete: function () {
            ajaxUpdateOff();
        },
    };
    return response;
}
function doCodeprocessCallback(callbackFunctions) {
    if (callbackFunctions !== undefined) {
        var funcArray = callbackFunctions.toString().split(",");
        for (var i = 0; i < funcArray.length; i++) {
            try {
                window[funcArray[i]]();
            } catch (e) {
                console.log(e);
            }
        }
    }
}
function doGetBasket(gotopaybasket, callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.basket.name;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            doCodeprocessCallback(callback);
            updateViewModelBasket(result, gotopaybasket);
        }
    };
    $.ajax(request);
}
function doGetStoreBasket(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.basket.name;
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            doCodeprocessCallback(callback);
            updateViewModelStoreBasket(result);
            GetOuOfSendStoreIndex();
        }
    };
    $.ajax(request);
}
function GetPriceBySizeColor(productId, colorId, sizeId) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.ProductQuantity.name + "?productId=" + productId + "&colorId=" + colorId + "&sizeId=" + sizeId;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            updateViewModelProduct(result.Value);
        }
    };
    $.ajax(request);
}
function GetComment(CommentId) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.productcomment.name + "?id=" + CommentId;
    request.success = function (result) {
        viewModel.Account.UserComment(result.Value);
    };
    $.ajax(request);
}
function GetStoreComment(CommentId) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.storecomment.name + "?id=" + CommentId;
    request.success = function (result) {
        viewModel.Account.StoreUserComment(result.Value);
    };
    $.ajax(request);
}
function doGetProduct(Id, callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.product.name + "?Id=" + Id;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            doCodeprocessCallback(callback);
            updateViewModelProduct(result.Value);
        }
    };
    $.ajax(request);
}
function doGetCurrentAccount(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.account.name;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            updateViewModelAccount(result);
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function doGetSendType(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.basket.sendType;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.Basket.sendTypeMaxDay(result.Value.MaxDays);
        }
    };
    $.ajax(request);
}
function doGetAccountAddresses(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.address.name;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.Account.Addresses.List(result.Value);
            if (viewModel.Basket.Address.AddressId < 0) {
                if (viewModel.Account.Addresses.List().length > 0) {
                    viewModel.Basket.Address.AddressId(viewModel.Account.Addresses.List()[0].Id);
                }
            }
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function UpdateLocation() {
    var id = viewModel.Basket.Address.AddressId();
    doUpdateLocation(id);
}
function doUpdateLocation(id, gotopaybasket) {
    ajaxLoader(true);
    var request = createRequest();
    request.url = request.url + codeprocess.api.address.name + "?addressId=" + id;
    request.success = function (result) {
        var addressId = 0;
        try {
            addressId = id;
            viewModel.State.StateId(result.Value[0].State.Id);
            viewModel.State.StateName(result.Value[0].State.Name);
            viewModel.City.CityId(result.Value[0].City.Id);
            viewModel.City.CityName(result.Value[0].City.Name);
            viewModel.Country.CountryId(result.Value[0].Country.Id);
        } catch (err) {
            viewModel.State.StateId("8");
            viewModel.City.CityId("87");
        }
        doSetLocation(viewModel.Country.CountryId(), viewModel.City.CityId(), viewModel.State.StateId(), addressId, gotopaybasket);
        ajaxLoader(false);
    };
    $.ajax(request);
}
function GetCurrentLocation() {
    ajaxLoader(true);
    var request = createRequest();
    request.url = request.url + codeprocess.api.location.name;
    request.success = function (result) {
        console.log(result);
        try {
            viewModel.State.StateName(result.Value.StateName);
            viewModel.City.CityName(result.Value.CityName);
            viewModel.State.StateId(result.Value.StateId);
            viewModel.City.CityId(result.Value.CityId);
            viewModel.Country.CountryId(result.Value.CountryId);
        } catch (err) {
            viewModel.State.StateName("تهران");
            viewModel.City.CityName("تهران");
            viewModel.State.StateId("8");
            viewModel.City.CityId("87");
            doSetLocation(viewModel.Country.CountryId(), viewModel.City.CityId(), viewModel.State.StateId(), 0);
        }
        viewModel.Basket.Address.AddressId(result.Value.AddressId);
        var x = window.location.href.includes("address");
        if (x && viewModel.Basket.Address.AddressId() > 0) {
            doGetBasket(true);
        }
        if (viewModel.Account.Addresses.List().length > 0) {
            if (viewModel.Basket.Address.AddressId() === 0) {
                viewModel.Basket.Address.AddressId(viewModel.Account.Addresses.List()[0].Id);
                doUpdateLocation(viewModel.Basket.Address.AddressId());
            }
        }
        if (viewModel.City.CityId() === 0) {
            viewModel.State.StateName("تهران");
            viewModel.City.CityName("تهران");
            viewModel.State.StateId("8");
            viewModel.City.CityId("87");
            doSetLocation(viewModel.Country.CountryId(), viewModel.City.CityId(), viewModel.State.StateId(), 0);
        }
        ajaxLoader(false);
    };
    $.ajax(request);
}
function doGetStates(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.state.name;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.State.StateList(result.Value);
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function doGetCurrency(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.currency.name;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.Currency.CurrencyList(result.Value);
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function doGetProductLikes(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.productlike.name;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.Account.Favorites(result.Value);
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function doGetShopReseller(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.shopreseller.name;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            updateViewModelShopReseller(result);
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function doGetShopResellerProducts(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.shopresellerproduct.name;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.ShopReseller.ShopResellerProducts(result.Value);
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function doGetShopResellerCollection(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.shopresellercollection.name;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.ShopReseller.ShopResellerCollections(result.Value);
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function doGetShopResellerGallery(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.shopresellergallery.name;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.ShopReseller.ShopResellerGalleries(result.Value);
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function doGetShopResellerStories(callback) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.shopresellerstory.name;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            viewModel.ShopReseller.ShopResellerStories(result.Value);
            doCodeprocessCallback(callback);
        }
    };
    $.ajax(request);
}
function getSendType(storeId, orderPrice) {
    var request = createRequest();
    request.url = request.url + codeprocess.api.SendType.name + "?storeId=" + storeId + "&orderPrice=" + orderPrice;
    request.success = function (result) {
        if (result.Code === apirequest.status.Success) {
            var res = result.Value;
            viewModel.Basket.SendType.CurrnetPrice(res.CurrentPrice);
            viewModel.Basket.SendType.Price(res.Price);
            viewModel.Basket.SendType.MinPriceForFree(res.MinPriceForFree);
            viewModel.Basket.SendType.MaxDays(res.MaxDays);
        }
    };
    $.ajax(request);
}
function reLoadPage() {
    location.reload();
}
function initialArticleInfinity() {
    var infinityDiv = $(".infinity-div #infinity-content");
    var pageIndex = parseInt(infinityDiv.attr("page-index"));
    var pageSize = parseInt(infinityDiv.attr("page-size"));
    var loading = infinityDiv.attr("loading");
    $(window).scroll(function () {
        pageIndex = parseInt(infinityDiv.attr("page-index"));
        pageSize = parseInt(infinityDiv.attr("page-size"));
        loading = infinityDiv.attr("loading");
        if (loading == "false") {
            if (infinityDiv.offset().top + infinityDiv.height() <= $(window).scrollTop() + $(window).height()) {
                getItems();
            }
        }
    });
    function getItems() {
        if (pageIndex != 0) {
            infinityDiv.attr("loading", "true");
            $.ajax({
                url: "/Post/Search?pocategory=news",
                method: "GET",
                datatype: "Html",
                data: { poviewName: "PostList", poIndex: pageIndex, poSize: pageSize },
                success: function (result) {
                    console.log(result);
                    infinityDiv.append(result);
                    if (result.length < 5) {
                        infinityDiv.attr("page-index", "0");
                    } else {
                        infinityDiv.attr("page-index", pageIndex + 1);
                        infinityDiv.attr("loading", "false");
                    }
                    $(".infinity-div .loader").hide();
                },
            });
        }
    }
    getItems();
}
function doSetLocation(countryId, cityId, stateId, addressId, gotopaybasket) {
    var request = createRequest();
    request.type = apirequest.type.Get;
    request.url = request.url + codeprocess.api.location.name + "?countryId=" + countryId + "&cityId=" + cityId + "&stateId=" + stateId + "&addressId=" + addressId;
    request.success = function (result) {
        if (result.Code == apirequest.status.Success) {
            $("#exampleModalCenter").modal("hide");
            location.replace(window.location.href);
        }
    };
    $.ajax(request);
}
function RemoveCookieOuOfSendStore() {
    var request = createRequest();
    request.type = apirequest.type.Delete;
    request.url = request.url + codeprocess.api.location.name;
    request.success = function (result) {
        if (result.Code == apirequest.status.Success) {
            location.replace("/basket/address");
        }
    };
    $.ajax(request);
}
function GetOuOfSendStore(gotopaybasket) {
    var request = createRequest();
    request.type = apirequest.type.Get;
    request.url = request.url + codeprocess.api.location.name + "?OutOfSendStore=true";
    request.success = function (result) {
        if (result.Code == apirequest.status.Success) {
            viewModel.ListOutOfSendStore(result.Value);
            if (gotopaybasket) {
                if (viewModel.ListOutOfSendStore().length > 0) {
                    gotoCartPage();
                }
            }
        }
    };
    $.ajax(request);
}
function GetOuOfSendStoreIndex() {
    var request = createRequest();
    request.type = apirequest.type.Get;
    request.url = request.url + codeprocess.api.location.name + "?OutOfSendStore=true";
    request.success = function (result) {
        if (result.Code == apirequest.status.Success) {
            viewModel.ListOutOfSendStore(result.Value);
        }
    };
    $.ajax(request);
}
function ShowNotifyOutOfSend() {
    var x = window.location.href.includes("Basket");
    if (viewModel.ListOutOfSendStore().length > 0) {
        if (x) {
            document.location = window.location.href;
        } else {
            if (viewModel.ListOutOfSendStore().length > 0) {
                $(".modal-OutOfSend").modal("show");
            }
        }
    }
}
$(".upload-file").change(function () {
    var element = $(this);
    var targetName = element.attr("target-name");

    var fileType = element.attr("file-type");
    var fileUpload = element.get(0);
    var files = fileUpload.files;
    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }
    var request = createRequest();
    if (fileType == codeprocess.fileType.document) {
        request.url = "/panel/Upload/UploadDocument";
    } else if (fileType == codeprocess.fileType.picture) {
        request.url = "/panel/Upload/UploadPicture";
    }
    request.type = "POST";
    (request.data = data), (request.contentType = false);
    request.processData = false;
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.error = function () { };
    request.success = function (entity) {
        $("[name=" + targetName + "]").val(entity.Id);
        $("#modalLoading").modal("hide");
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    $.ajax(request);
});
$(".upload-file1").change(function () {
    var element = $(this);
    var fileUpload = element.get(0);
    var files = fileUpload.files;
    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }
    var request = createRequest();

    request.url = "/panel/Upload/UploadPhoto";

    request.type = "POST";
    (request.data = data), (request.contentType = false);
    request.processData = false;
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.error = function () { };
    request.success = function (entity) {
        viewModel.Account.PictureId(entity.Id);
        $("#ProfilePicId").val(entity.Id);
        viewModel.Account.PictureUrl(entity.Url)
        $("#modalLoading").modal("hide");
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    $.ajax(request);
});
$(".upload-file2").change(function () {
    var element = $(this);
    var fileUpload = element.get(0);
    var files = fileUpload.files;
    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }
    var request = createRequest();

    request.url = "/panel/Upload/UploadPhoto";

    request.type = "POST";
    (request.data = data), (request.contentType = false);
    request.processData = false;
    request.beforeSend = function () {
        ajaxLoader(true);
    };
    request.error = function () { };
    request.success = function (entity) {
        viewModel.Account.NationalPictureId(entity.Id);
        $("#ProfilePicId2").val(entity.Id);

        viewModel.Account.NationalPictureUrl(entity.Url)
        $("#modalLoading").modal("hide");
    };
    request.complete = function () {
        ajaxLoader(false);
    };
    $.ajax(request);
});

function doSearchPostNew() {
    viewModel.PostSearchResult.Pending(true);

    var request = createRequest();
    request.url = request.url + codeprocess.api.PostSearch.name;
    request.url = request.url + "?pageSize=" + viewModel.PostSearchResult.PageSize();
    request.url = request.url + "&index=" + viewModel.PostSearchResult.PageIndex();
    request.url = request.url + "&name=" + viewModel.PostSearchResult.Name();
    request.url = request.url + "&categoryLabel=" + viewModel.PostSearchResult.Category();
    request.url = request.url + "&typeId=" + viewModel.PostSearchResult.TypeId();
    request.success = function (result) {
        var resultList = result.Value;
        viewModel.PostSearchResult.Results(resultList);
    };
    request.complete = function () {
        viewModel.PostSearchResult.Pending(false);
    };
    $.ajax(request);
}
function RemovePic1() {
    viewModel.Account.PictureId(null);
    viewModel.Account.PictureUrl('')
}
function RemovePic2() {
    viewModel.Account.NationalPictureId(null);
    viewModel.Account.NationalPictureUrl('')
}