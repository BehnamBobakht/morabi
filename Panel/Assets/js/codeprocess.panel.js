var uploadPictureId = null;
var uploadWriterPictureId = null;
var uploadIconId = null;
var uploadDocumentId = null;
var editorList = [];
var AttachFile2 = null;
var AttachFile1 = null;
function DeletePic() {
    uploadPictureId = null;
    $('input[name=PictureId]').val(null);
    $('#img').attr("src", "");

    uploadWriterPictureId = null;
}
$(document).ready(function () {

    initCodeprocessValues();
});

String.prototype.GetInteger = function () {
    try {
        var output = parseInt(this);
        if (isNaN(output) === true)
            return 0;
        return output;
    }
    catch (ex) { return 0; }
};

jQuery.fn.extend({
    selected: function () {
        var value = $(this).find(":selected").attr("value");
        return value === undefined ? null : value;
    }
});

function initCodeprocessValues() {

    var components = $("[codeprocess]");
    for (var i = 0; i < components.length; i++) {
        var cp = components[i];
        var value = $(cp).attr("codeprocess");
        var Id = $(cp).attr("id");
        var targetId = $(cp).attr("targetid");
        if (value === "editor") {
            initCodeprocessEditor(Id);
        } else if (value === "date") {
            initCodeprocessDatetime(Id);
        } else if (value === "date-init") {
            initCodeprocessDatetime(Id);
            let today = new Date().toLocaleDateString('fa-IR');
            $(cp).val(today);
        } else if (value === "upload") {
            debugger
            initCodeprocessUpload(Id);
        } else if (value === "upload-n") {
            initializeCodeprocessUploadScript(Id);
        } else if (value === "upload-n-1") {
            initializeCodeprocessUploadScript(Id);
        } else if (value === "upload-n-2") {
            initializeCodeprocessUploadScript(Id);
        } else if (value === "document") {
            initializeCodeprocessUploadDocumentScript(Id, targetId);
        }
        else if (value === "documentAttachFile1") {
            initializeCodeprocessUploadDocumentAttachFile1Script(Id);
        }
        else if (value === "documentAttachFile2") {
            initializeCodeprocessUploadDocumentAttachFile2Script(Id);
        }
        else if (value === "show-part") {
            initCodeprocessShowPart(Id);
        }
    }

    function initCodeprocessShowPart(Id) {
        var selectedId = $("#" + Id).find(":selected").attr("value");
        $("[show-part='" + selectedId + "']").show();
        $("#" + Id).change(function () {
            selectedId = $(this).find(":selected").attr("value");
            $("[show-part]").hide();
            $("[show-part='" + selectedId + "']").show();
        });
    }

    function initializeCodeprocessUploadDocumentScript(Id,targetId) {
        $("#" + Id).change(function () {

            var fileUpload = $("#" + Id).get(0);
            var files = fileUpload.files;

            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            var request = createRequest();
            request.url = base_admin_url + "/Upload/UploadDocument";
            request.type = "POST";
            request.data = data,
                request.contentType = false;
            request.processData = false;
            request.beforeSend = function () {
                $("#modalLoading").modal("show");
            }
            request.error = function () {
                createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
            };
            request.success = function (entity) {
                uploadDocumentId = entity.Id;
                $("#" + targetId).val(entity.Id);
                $("#modalLoading").modal("hide");
            };
            $.ajax(request);
        });
    }
    function initializeCodeprocessUploadDocumentAttachFile1Script(Id) {
        $("#" + Id).dropify({
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
        $("#" + Id).change(function () {

            var fileUpload = $("#" + Id).get(0);
            var files = fileUpload.files;

            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            var request = createRequest();
            request.url = base_admin_url + "/Upload/UploadDocument";
            request.type = "POST";
            request.data = data,
                request.contentType = false;
            request.processData = false;
            request.beforeSend = function () {
                $("#modalLoading").modal("show");
            }
            request.error = function () {
                createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
            };
            request.success = function (entity) {
                AttachFile1 = entity.Id;
                $("#modalLoading").modal("hide");
            };
            $.ajax(request);
        });
    }
    function initializeCodeprocessUploadDocumentAttachFile2Script(Id) {
        $("#" + Id).dropify({
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
        $("#" + Id).change(function () {

            var fileUpload = $("#" + Id).get(0);
            var files = fileUpload.files;

            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            var request = createRequest();
            request.url = base_admin_url + "/Upload/UploadDocument";
            request.type = "POST";
            request.data = data,
                request.contentType = false;
            request.processData = false;
            request.beforeSend = function () {
                $("#modalLoading").modal("show");
            }
            request.error = function () {
                createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
            };
            request.success = function (entity) {
                AttachFile2 = entity.Id;
                $("#modalLoading").modal("hide");
            };
            $.ajax(request);
        });
    }

    function initializeCodeprocessUploadScript(Id) {
        $("#" + Id).dropify({
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
    }

    function initCodeprocessUpload(Id) {
        $("#" + Id).dropify({
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
        $(".dropify-clear").click(function () {
            var thisElement = $(this);
            var targetId = thisElement.parent().find(".dropify").attr("target-id");
            var targetUrl = thisElement.parent().find(".dropify").attr("target-url");
            $("#" + targetId).val("");
            $("#" + targetUrl).val("");
        });
        $("#" + Id).change(function () {
            debugger
            var fileUpload = $("#" + Id).get(0);
            var files = fileUpload.files;
            var uploadType = $(fileUpload).attr("uploadtype");
            var targetElement = $(fileUpload).attr("target-id");
            var targetUrlElement = $(fileUpload).attr("target-url");
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            var request = createRequest();
            request.url = base_admin_url + "/Upload/UploadPhoto";
            request.type = "POST";
            request.data = data,
                request.contentType = false;
            request.processData = false;
            request.beforeSend = function () {
                $("#modalLoading").modal("show");
            };
            request.error = function () {
                createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
            };
            request.success = function (entity) {
                debugger
                if (uploadType == 2) {
                    uploadIconId = entity.Id;
                }
                else if (uploadType == 3) {
                    uploadWriterPictureId = entity.Id;
                }
                else {
                    uploadPictureId = entity.Id;
                  
                }
                if (targetElement !== undefined) {
                    $("#" + targetElement).val(entity.Id);
                }
                if (targetUrlElement !== undefined) {
                    $("#" + targetUrlElement).val(entity.Url);
                }
                $("#modalLoading").modal("hide");
            };
            $.ajax(request);
        });
    }

    function initCodeprocessDatetime(Id) {
        var objCal = new AMIB.persianCalendar(Id);

    }
}

function initCodeprocessEditor(Id) {

    tinymce.init({
        selector: "input#" + Id,
        height: 250,
        theme: 'modern',
        plugins: [
            'advlist autolink lists link image charmap print preview hr anchor pagebreak',
            'searchreplace wordcount visualblocks visualchars code fullscreen',
            'insertdatetime media nonbreaking save table contextmenu directionality',
            'emoticons template paste textcolor colorpicker textpattern imagetools image'
        ],
        toolbar1: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link',
        toolbar2: 'print preview media | forecolor backcolor emoticons | image code',
        images_upload_url: 'upload.php',
        images_upload_handler: function (blobInfo, success, failure) {
            var data = new FormData();
            data.append('file', blobInfo.blob(), blobInfo.filename());
            var request = createRequest();
            request.url = base_admin_url + "/Upload/UploadPhoto";
            request.type = "POST";
            request.data = data,
                request.contentType = false;
            request.processData = false;
            request.error = function () {
                createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
            };
            request.success = function (entity) {
                let imageUrl = entity.Url.replace(window.location.origin, "");
                success(imageUrl);
            };
            $.ajax(request);
        }
    });
}

function clearDropDown(Id, isDefault) {
    $("#" + Id).find("option").remove();
    if (isDefault === true) {
        $("#" + Id).append("<option value='0'>انتخاب</option>");
    }
}

function bindDropDown(Id, entity, name, value, isDefault, selectedItem) {
    debugger
    $("#" + Id).find("option").remove();
    var result = entity;
    if (isDefault === true) {
        $("#" + Id).append("<option value='0'>انتخاب</option>");
    }
    for (var i = 0; i < result.length; i++) {
        var item = result[i];
        var itemValue = item[value];
        if (selectedItem != undefined && itemValue.toString() === selectedItem.toString()) {
            $("#" + Id).append("<option value='" + itemValue + "' selected='selected'>" + item[name] + "</option>");
        }
        else {
            $("#" + Id).append("<option value='" + itemValue + "'>" + item[name] + "</option>");
        }
    }
}

function getCustomFieldControl(item) {
    var itemValue = item.ProductFieldValue == null ? "" : item.ProductFieldValue;
    var itemId = item.ProductFieldItem == null ? 0 : item.ProductFieldItem;

    var html = "";
    html += "<div class='col-md-6'>";
    html += "<div class='form-group'>";
    html += "<label class='control-label col-md-4'>" + item.Name + "</label>";
    html += "<div class='col-md-8'>";

    if (item.Type.Label === "FIELD_TYPE_STRING") {
        html += "<input type='text' class='form-control' custom-id='" + item.Id + "' value='" + itemValue + "' custom-type='STRING' autocomplete='off' />";
    } else if (item.Type.Label === "FIELD_TYPE_INTEGER") {
        html += "<input type='text' class='form-control' custom-id='" + item.Id + "' value='" + itemValue + "' custom-type='INTEGER' autocomplete='off' />";
    } else if (item.Type.Label === "FIELD_TYPE_DATE") {
        html += "";
    } else if (item.Type.Label === "FIELD_TYPE_TEXT") {
        html += "<input type='text' class='form-control' custom-id='" + item.Id + "' value='" + itemValue + "' custom-type='STRING' autocomplete='off' />";
    } else if (item.Type.Label === "FIELD_TYPE_HTML") {
        html += "<input id='fieldId" + item.Id + "' type='text' class='form-control' custom-id='" + item.Id + "' value='" + itemValue + "' custom-type='HTML' autocomplete='off' />";
    } else if (item.Type.Label === "FIELD_TYPE_FILE") {
        html += "";
    } else if (item.Type.Label === "FIELD_TYPE_DROPDOWN") {
        html += "<select class='form-control' custom-type='DROPDOWN' custom-id='" + item.Id + "'>";
        html += "<option value='0'>انتخاب</option>";
        for (var i = 0; i < item.Items.length; i++) {
            var selectItem = item.Items[i];
            if (selectItem.Id == itemId) {
                html += "<option selected='selected' value='" + selectItem.Id + "'>" + selectItem.Value + "</option>";
            } else {
                html += "<option value='" + selectItem.Id + "'>" + selectItem.Value + "</option>";
            }
        }
        html += "<select>";
    } else if (item.Type.Label === "FIELD_TYPE_BOOLEAN") {
        var isSelected = itemValue == "True" ? " checked='checked' " : "";
        html += "<input type='checkbox' class='margin-checkbox' custom-id='" + item.Id + "' value='" + itemValue + "' " + isSelected + " custom-type='BOOLEAN' />";
    }
    html += "</div>";
    html += "</div>";
    html += "</div>";
    return html;
}

function getCheckedValue(obj) {
    return $(obj).is(':checked');
}

function getSelectedValue(obj) {
    var value = $(obj).find(":selected").attr("value");
    value = value === undefined ? parseInt($(obj).val()) : value;
    value = isNaN(value) ? 0 : value;
    return value;
}

function getCodeprocessBackUrl() {
    return $("[codeprocess-back-form]").attr("codeprocess-back-form");
}

function initializePermissionScript() {
    $("#btnSubmit").click(function () {
        var entity = {};
        var selectedItems = $("[aria-selected='true']");
        var selectedParents = $(".jstree-undetermined").closest("[role='treeitem']");

        entity.UserGroupId = $("[name='UserGroupId']").val();
        entity.PermissionList = [];

        for (var i = 0; i < selectedItems.length; i++) {
            var valueItem = parseInt($(selectedItems[i]).attr("value"));
            entity.PermissionList.push(valueItem);
        }

        for (var j = 0; j < selectedParents.length; j++) {
            var valueParent = parseInt($(selectedParents[j]).attr("value"));
            entity.PermissionList.push(valueParent);
        }

        var request = createRequest(entity);
        request.success = function (result) {
            if (result.Type === MESSAGE_ERROR) {
                createMessage(MESSAGE_TYPE_ERROR, result.Body);
            } else if (result.Type === MESSAGE_SUCCESS) {
                createMessage(MESSAGE_TYPE_SUCCESS, result.Body);
                var url = getCodeprocessBackUrl();
                if (url !== undefined) {
                    if (url.toUpperCase().startsWith("/PANEL") == false) {
                        url = base_admin_url + url;
                    }
                    document.location = url;
                }
            }
        }
        $.ajax(request);

    });
}

function initializeNewCategoryScript() {

    var category = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(category));
    });
    function fillEntity() {
        category.ID = $("#ID").val();
        category.WebsiteId = $("#WebsiteId").val();
        category.TypeId = $("#TypeId").val();
        category.Name = $("#Name").val();
        category.Label = $("#Label").val();
        category.PictureId = uploadPictureId == null ? $("#pictureUploadId").val() : uploadPictureId;
        category.WriterPictureId = uploadWriterPictureId == null ? $("#pictureWriterUploadId").val() : uploadWriterPictureId;
        category.URL = $("#URL").val();
        category.Body = tinymce.get('Body') != null ? tinymce.get('Body').getContent() : "";
        category.CategoryLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    Body: tinymce.get($(langObject).find("[lang-name='Body']").attr('id')).getContent(),
                    URL: $(langObject).find("[lang-name='URL']").val()
                };
                category.CategoryLanguage.push(langEntity);
            }
        }
    }
}

function initializeNewSliderScript() {
    var slider = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(slider));
    });
    function fillEntity() {
        slider.ID = $("#ID").val();
        slider.WebsiteId = $("#WebsiteId").val();
        slider.UserID = $("#UserID").val();
        slider.Name = $("#Name").val();
        slider.SecondName = $("#SecondName").val();
        slider.Link = $("#Link").val();
        slider.ShowNumber = $("#ShowNumber").val();
        slider.PictureId = uploadPictureId == null ? $("#pictureUploadId").val() : uploadPictureId;
        slider.WriterPictureId = uploadWriterPictureId == null ? $("#pictureWriterUploadId").val() : uploadWriterPictureId;
        slider.SliderLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    SecondName: $(langObject).find("[lang-name='SecondName']").val()
                };
                slider.SliderLanguage.push(langEntity);
            }
        }
    }
}

function initializeNewPostScript() {
    var post = {};
    $("#btnSubmit").click(function () {

        fillEntity();

        $.ajax(createRequest(post));
    });
    $(".divWriter .dropify-clear").click(function () {
        uploadWriterPictureId =null;
        $("#pictureWriterUploadId").val(null);
    });
    $(".divPicture .dropify-clear").click(function () {
        uploadPictureId = 0;
    });
    function fillEntity() {

        post.ID = $("#ID").val();
        post.StateId = $("#StateId").val();
        post.WebsiteId = $("#WebsiteId").val();
        post.CategoryId = $("#CategoryId").val();
        post.Name = $("#Name").val();
        post.Keywords = $("#Keywords").val();
        post.Summary = $("#Summary").val();
        post.Phone = $("#Phone").val();
        post.Email = $("#Email").val();
        post.Body = tinymce.get('Body') != null ? tinymce.get('Body').getContent() : "";
        post.UpdateDatetime = $("#UpdateDatetime").val();
        post.CreateDateTime = $("#CreateDateTime").val();
        post.ShowNumber = $("#ShowNumber").val();
        post.PostLabel = $("#PostLabel").val();
        //post.IsSlider = $("#IsSlider").val();
        //post.Active = $("#Active").val();
        if ($('#IsSlider').is(":checked")) {
            post.IsSlider = true;
        }
        else {
            post.IsSlider = false;
        }
        if ($('#Active').is(":checked")) {
            post.Active = true;
        }
        else {
            post.Active = false;
        }
        if ($('#NoIndex').is(":checked")) {
            post.NoIndex = true;
        }
        else {
            post.NoIndex = false;
        }
        if ($('#NoFollow').is(":checked")) {
            post.NoFollow = true;
        }
        else {
            post.NoFollow = false;
        }
        post.Canonical = $("#Canonical").val();

        post.VisitCount = $("#VisitCount").val();
        post.CreateDateTime = $("#CreateDateTime").val();
        post.Writer = $("#Writer").val();
        post.WriterPostion = $("#WriterPostion").val();
        post.H1 = $("#H1").val();
        post.URl = $("#URL").val();
        post.PictureId = $("#pictureUpload").val();
        post.WriterPictureId = $("#pictureWriterUploadId").val();
        post.FileId = $("#FileUpload").val();
        post.PictureId = uploadPictureId == null ? $("#pictureUploadId").val() : uploadPictureId;
        post.WriterPictureId = uploadWriterPictureId == null ? $("#pictureWriterUploadId").val() : uploadWriterPictureId;
        post.Aparat = $("#Aparat").val();
        post.FileId = uploadDocumentId == null ? $("#FileUploadId").val() : uploadDocumentId;
        post.PostLanguage = [];
        post.TagPost = [];
        var tagInputs = $(".tag-container input");
        for (var j = 0; j < tagInputs.length; j++) {
            var thisInput = tagInputs[j];
            if ($(thisInput).prop("checked") === true) {
                var tagId = $(thisInput).attr("tag-id");
                var tagEntity = {
                    PostId: post.ID,
                    TagId: parseInt(tagId)
                };
                post.TagPost.push(tagEntity);
            }
        }
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    Summary: $(langObject).find("[lang-name='Summary']").val(),
                    Body: tinymce.get($(langObject).find("[lang-name='Body']").attr('id')).getContent(),
                    Url: $(langObject).find("[lang-name='Url']").val(),
                    Keywords: $(langObject).find("[lang-name='Keywords']").val(),
                    H1: $(langObject).find("[lang-name='H1']").val()
                };
                post.PostLanguage.push(langEntity);
            }
        }
    }

}
function initializeAddDiscountScript() {
    $(document).on("change", "#ProductTypeId", function () {
        $("#ProductCategoryId").val('');
        $("#ProductSubCategoryId").val('');
        $("#ProductId").val('');
        $("#product-category-dropDown").addClass("disable-content");
        var request = createRequest();
        request.url = base_admin_url + "/store/ProductCategory/ProductCategoryDropDown";
        request.type = "Get";
        request.data = { typeId: $(this).val() };
        request.contentType = "html";
        request.success = function (result) {
            $("#product-category-dropDown").html(result);
            $("#product-category-dropDown").removeClass("disable-content");
            var resultCount = $('#ProductCategoryId option').length;
           
                getProduct();
            
            $(".selectpicker").attr("data-live-search", "true");
            $('.selectpicker').selectpicker('refresh');
        };
        $.ajax(request);
    });
    $(document).on("change", "#ProductCategoryId", function () {
        $("#product-sub-category-dropDown").addClass("disable-content");
        $("#ProductSubCategoryId").val('');
        $("#ProductId").val('');
        var request = createRequest();
        request.url = base_admin_url + "/store/ProductSubCategory/ProductSubCategoryDropDown";
        request.type = "Get";
        request.data = { categoryId: $(this).val() };
        request.contentType = "html";
        request.success = function (result) {
            $("#product-sub-category-dropDown").html(result);
            $("#product-sub-category-dropDown").removeClass("disable-content");
            var resultCount = $('#ProductSubCategoryId option').length;
            if (resultCount < 1) {
                getProduct();
            }
            $(".selectpicker").attr("data-live-search", "true");
            $('.selectpicker').selectpicker('refresh');
        };
        $.ajax(request);
    });
    $(document).on("change", "#ProductSubCategoryId", function () {
        getProduct();
        $(".selectpicker").attr("data-live-search", "true");
        $('.selectpicker').selectpicker('refresh');
    });
    function getProduct() {
        debugger
        $("#product-dropDown").addClass("disable-content");
        var storeId = $("#StoreId").val();
        var producttypeId = $("#ProductTypeId").val();
        var productCategoryId = $("#ProductCategoryId").val();
        var productSubCategoryId = $("#ProductSubCategoryId").val();
        var request = createRequest();
        request.url = base_admin_url + "/store/Product/ProductDropDown";
        request.type = "Get";
        request.data = {
            storeId: storeId,
            producttypeId: producttypeId,
            productCategoryId: productCategoryId,
            productSubCategoryId: productSubCategoryId
        };
        request.contentType = "html";
        request.success = function (result) {
            $("#product-dropDown").html(result);
            $("#product-dropDown").removeClass("disable-content");
            $(".selectpicker").attr("data-live-search", "true");
            $('.selectpicker').selectpicker('refresh');
        };
        $.ajax(request);
    };
    $(document).on("change", "#ProductId", function () {
        $("#product-quantity-dropDown").addClass("disable-content");
        var storeId = $("#StoreId").val();
        var productId = $(this).val();
        var request = createRequest();
        request.url = base_admin_url + "/store/Product/StoreProductQuantityDropDown";
        request.type = "Get";
        request.data = {
            storeId: storeId,
            productId: productId
        };
        request.contentType = "html";
        request.success = function (result) {
            $("#product-quantity-dropDown").html(result);
            $("#product-quantity-dropDown").removeClass("disable-content");
            $(".selectpicker").attr("data-live-search", "true");

            $('.selectpicker').selectpicker('refresh');
        };
        $.ajax(request);
    });
    $(document).on("change", "#GroupId", function () {
        window.location.href = "/panel/store/discount/create/" + $(this).val();
    });
    $(document).on("change", ".discount-form #TypeId", function () {
        showType();
    });
    $(document).on("change", ".discount-form #ValueTypeId", function () {
        
        var value = $(this).val();
        if (value === "52") {
            $(".discount-form #PriceValue").removeAttr('disabled');
            $(".discount-form #PercentValue").attr('disabled', 'disabled');
        }
        else {
            $(".discount-form #PercentValue").removeAttr('disabled');
            $(".discount-form #PriceValue").attr('disabled', 'disabled');
        }
    });

    function showType() {
        var typevalue = $(".discount-form #TypeId").val();
        var elmentDivs = $(".discount-form .show-type");
        elmentDivs.addClass("d-none");
        if (typevalue) {
            elmentDivs.each(function (i, obj) {
                var currentdiv = $(this);
                var showtypes = currentdiv.attr("show-type").split(",");
                var j;
                for (j = 0; j < showtypes.length; ++j) {
                    if (showtypes[j] === typevalue) {
                        currentdiv.removeClass("d-none");
                    }
                }

            
        });
    }
}
    showType();
}

function initializeAddProductScript() {
    resetListBoxIndexs("review-box");
    resetListBoxIndexs("other-pictures-box");
	$("#IsPackage").change(function() {
    if(this.checked) {
       $(".classPackge").removeClass("d-none");
    }
});
$(document).on("change", "#ProductId2", function () {
        $("#ProductsId").val($("#ProductId2").val())
    });
    $(document).on("change", "#ProductTypeId", function () {
        $("#product-category-dropDown").addClass("disable-content");
        $("#product-brand-dropDown").addClass("disable-content");
        $("#product-option-dropDown").addClass("disable-content");
        $("#product-option-item-container").html("");
        $("#color-container").addClass("disable-content");
        $("#size-container").addClass("disable-content");
        var request = createRequest();
        request.url = base_admin_url + "/store/ProductCategory/ProductCategoryDropDown";
        request.type = "Get";
        request.data = { typeId: $(this).val() };
        request.contentType = "html";
        request.success = function (result) {
            $("#product-category-dropDown").html(result);
            $("#product-category-dropDown").removeClass("disable-content");

        };
        $.ajax(request);
        request.url = base_admin_url + "/store/ProductBrand/ProductBrandDropDown";
        request.type = "Get";
        request.data = { typeId: $(this).val() };
        request.contentType = "html";

        request.success = function (result) {
            $("#product-brand-dropDown").html(result);
            $("#product-brand-dropDown").removeClass("disable-content");
        };
        $.ajax(request);

        request.url = base_admin_url + "/store/ProductOption/ProductOptionDropDown";
        request.type = "Get";
        request.data = { typeId: $(this).val() };
        request.contentType = "html";

        request.success = function (result) {
            $("#product-option-dropDown").html(result);
            $("#product-option-dropDown").removeClass("disable-content");
        };
        $.ajax(request);
        request.url = base_admin_url + "/store/Product/Colors";
        request.type = "Get";
        request.data = { producttypeId: $(this).val() };
        request.contentType = "html";
        request.success = function (result) {
            $("#color-container").html(result);
            $("#color-container").removeClass("disable-content");
        };
        $.ajax(request);
        request.url = base_admin_url + "/store/Product/Sizes";
        request.type = "Get";
        request.data = { producttypeId: $(this).val() };
        request.contentType = "html";
        request.success = function (result) {
            $("#size-container").html(result);
            $("#size-container").removeClass("disable-content");
        };
        $.ajax(request);
    });
    $(document).on("change", "#ProductCategoryId", function () {
        $("#product-sub-category-dropDown").addClass("disable-content");

        var request = createRequest();
        request.url = base_admin_url + "/store/ProductSubCategory/ProductSubCategoryDropDown";
        request.type = "Get";
        request.data = { categoryId: $(this).val() };
        request.contentType = "html";
        request.success = function (result) {
            $("#product-sub-category-dropDown").html(result);
            $("#product-sub-category-dropDown").removeClass("disable-content");

        };
        $.ajax(request);
    });
    $(document).on("change", "#ProductOptionId", function () {
        $("#product-option-item-container").addClass("disable-content");

        var request = createRequest();
        request.url = base_admin_url + "/store/ProductOptionItem/ProductOptionItems";
        request.type = "Get";
        request.data = { productOptionId: $(this).val() },
            request.contentType = "html";
        request.success = function (result) {
            $("#product-option-item-container").html(result);
            $("#product-option-item-container").removeClass("disable-content");
        };
        $.ajax(request);
    });
    $(document).on("change", "#StateId", function () {
        $("#product-city-dropDown").addClass("disable-content");
        var request = createRequest();
        request.url = base_admin_url + "/content/City/CityDropDown";
        request.type = "Get";
        request.contentType = "html";
        request.data = { stateId: $(this).val() },
        request.success = function (result) {
            $("#product-city-dropDown").html(result);
            $("#product-city-dropDown").removeClass("disable-content");
        };
        $.ajax(request);
    });
    $(document).on("click", ".delete-review-item", function () {
        var elm = $(this).closest(".review-item");
        elm.remove();
        resetListBoxIndexs("review-box");
    });
    $(document).on("click", ".add-review", function () {
        $(".review-box").addClass("disable-content");
        var request = createRequest();
        request.url = base_admin_url + "/store/Product/NewReview";
        request.type = "Get";
        request.contentType = "html";

        request.success = function (result) {
            $(".review-box").append(result);
            resetListBoxIndexs("review-box");
            $(".review-box").removeClass("disable-content");
        };
        $.ajax(request);
    });

    function resetListBoxIndexs(boxName) {
        var boxItems = $("." + boxName + " .box-item");
        boxItems.each(function (i) {
            var elements = $(this).find(".has-pattern");
            elements.each(function (j) {
                var child = $(this);
                var namePattern = child.attr("name-pattern");
                if (namePattern != undefined) {
                    child.attr("name", namePattern.replace("index", i));
                }
                namePattern = child.attr("target-src-name-pattern");
                if (namePattern != undefined) {
                    child.attr("target-src-name", namePattern.replace("index", i));
                }
                namePattern = child.attr("target-show-name-pattern");
                if (namePattern != undefined) {
                    child.attr("target-show-name", namePattern.replace("index", i));
                }
                namePattern = child.attr("target-name-pattern");
                if (namePattern != undefined) {
                    child.attr("target-name", namePattern.replace("index", i));
                }
            });
        });
    }




    $(".dropify-clear").click(function () {
        uploadPictureId = null;
        uploadWriterPictureId = null;
    });
    $(document).on("change", "#uplOtherPicture", function () {
        var fileUpload = $("#uplOtherPicture").get(0);
        var files = fileUpload.files;

        var data = new FormData();
        for (var i = 0; i < files.length; i++) {
            data.append(files[i].name, files[i]);
        }
        var request = createRequest();
        request.url = base_admin_url + "/Upload/UploadPhoto";
        request.type = "POST";
        request.data = data,
            request.contentType = false;
        request.processData = false;
        request.beforeSend = function () {
            $("#uplOtherPicture").val("");
            $("#modalLoading").modal("show");
        }
        request.error = function () {
            createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
        };
        request.success = function (entity) {
            addRelatedPicture(entity);
            $("#modalLoading").modal("hide");
        };
        $.ajax(request);
    });
    $(document).on("click", ".delete-other-picture", function () {
        $(this).closest(".picture-item").remove();
        resetListBoxIndexs("other-pictures-box");
    });



    function addRelatedPicture(picture) {
        var panelUrl = $("#PanelUrl").val();
        var url = picture.Url;
        url = url.replace("SYSTEM_TYPE_PANEL", panelUrl);
        var picHtml = "<div other-picture-Id='" + picture.Id + "' class='box-item picture-item col-pic-20'>";
        picHtml += "<label class='other-picture-item'>";
        picHtml += "<img src='" + url + "' />";
        picHtml += "<input class='has-pattern' name-pattern='ProductPictures[index].Id' type='hidden' value='" + picture.Id + "' />";
        picHtml += "<input class='has-pattern' name-pattern='ProductPictures[index].Url' type='hidden' value='" + url + "' />";
        picHtml += "<label class='delete-other-picture' picture-id='" + picture.Id + "'>";
        picHtml += "<i class='fa fa-close' aria-hidden='true'></i>";
        picHtml += "</label>";
        picHtml += "<label>";
        picHtml += "</div>";
        $(".related-picture-container > .row").prepend(picHtml);

        var allItems = $("[other-picture-Id]");
        if (allItems.length > 9) {
            $(".add-item").hide();
        }
        resetListBoxIndexs("other-pictures-box");
    }
}
function initializeNewProductScript() {
    var product = {};
    fillProductType();
    fillCustomFields();
    removeRelatedPicture();

    $(".dropify-clear").click(function () {

        uploadPictureId = null;
        uploadWriterPictureId = null;
    });

    $("#btnSubmit").click(function () {
        fillEntity();
        debugger;
        $.ajax(createRequest(product));
    });
    if ($('input[name=IsService]:checked').val() == "false") {

        $(".map-div").hide();
    }
    $("input[name=IsService]").change(function () {
        var elm = $(this);
        if (elm.val() == "true") {
            $(".map-div").show();
        }
        else {
            $(".map-div").hide();

        }
    });

    function getCustomValues(product) {
        product.ProductCustomValue = [];

        var customElements = $("[custom-id]");

        for (var i = 0; i < customElements.length; i++) {
            var element = customElements[i];
            var entity = {
                ID: 0,
                FieldId: $(element).attr("custom-id").GetInteger(),
                Value: getInputValue(element),
                ItemId: getSelectValue(element)
            };
            product.ProductCustomValue.push(entity);
        }
    }

    function getSelectValue(element) {
        var Value;
        var elementType = $(element).attr("custom-type");
        if (elementType === "DROPDOWN")
            Value = $(element).find(":selected").attr("value");
        else
            Value = null;
        return Value;
    }

    function getInputValue(element) {
        var Value;
        var elementType = $(element).attr("custom-type");
        if (elementType === "STRING")
            Value = $(element).val();
        else if (elementType === "BOOLEAN")
            Value = $(element).prop("checked") == true ? "True" : "False";
        else if (elementType === "INTEGER")
            Value = $(element).val();
        else if (elementType === "DROPDOWN")
            Value = "";
        else
            Value = "";
        return Value;
    }

    $("#ShopId").change(function () {
        fillProductType();
    });

    $("#ProductTypeId").change(function () {

        fillCategory();
        fillBrand();
        fillCustomFields();
        fillColors();
        fillSize();
        fillProductOption();

    });

    $("#ProductCategoryId").change(function () {
        fillSubCategory();
        fillCustomFields();
    });

    $("#ProductSubCategoryId").change(function () {
        fillCustomFields();
    });

    $("#BrandId").change(function () {
        fillCustomFields();
    });
    $(document).on("change", "#uplOtherPicture", function () {
        debugger
        var fileUpload = $("#uplOtherPicture").get(0);
        var files = fileUpload.files;

        var data = new FormData();
        for (var i = 0; i < files.length; i++) {
            data.append(files[i].name, files[i]);
        }
        var request = createRequest();
        request.url = base_admin_url + "/Upload/UploadPhoto";
        request.type = "POST";
        request.data = data,
            request.contentType = false;
        request.processData = false;
        request.beforeSend = function () {
            $("#uplOtherPicture").val("");
            $("#modalLoading").modal("show");
        }
        request.error = function () {
            createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
        };
        request.success = function (entity) {
            addRelatedPicture(entity);
            $("#modalLoading").modal("hide");
        };
        $.ajax(request);
    });

    function removeRelatedPicture() {
        $("[other-picture-Id]").click(function () {
            $(this).remove();
        });
    }

    function addRelatedPicture(picture) {
        var panelUrl = $("#PanelUrl").val();
        var url = picture.Url;
        url = url.replace("SYSTEM_TYPE_PANEL", panelUrl);
        var picHtml = "<div other-picture-Id='" + picture.Id + "' class='col-pic-20'>";
        picHtml += "<label class='other-picture-item'>";
        picHtml += "<img src='" + url + "' />";
        picHtml += "<label class='delete-other-picture' picture-id='" + picture.Id + "'>";
        picHtml += "<i class='fa fa-close' aria-hidden='true'></i>";
        picHtml += "</label>";
        picHtml += "<label>";
        picHtml += "</div>";
        $(".related-picture-container > .row").prepend(picHtml);

        var allItems = $("[other-picture-Id]");
        if (allItems.length > 9) {
            $(".add-item").hide();
        }

        removeRelatedPicture();
    }

    function fillEntity() {
        debugger;
        product.ID = $("#ID").val();
        product.ShopId = $("#ShopId").selected() == null ? parseInt($("#ShopId").val()) : $("#ShopId").selected();
        product.ProductTypeId = $("#ProductTypeId").selected() == null ? parseInt($("#ProductTypeId").val()) : $("#ProductTypeId").selected();
        product.ProductCategoryId = $("#ProductCategoryId").selected() == null ? parseInt($("#ProductCategoryId").val()) : $("#ProductCategoryId").selected();
        product.ProductSubCategoryId = $("#ProductSubCategoryId").selected() == null ? parseInt($("#ProductSubCategoryId").val()) : $("#ProductSubCategoryId").selected();
        product.UnitId = $("#UnitId").selected() == null ? parseInt($("#UnitId").val()) : $("#UnitId").selected();
        product.CompanyId = $("#CompanyId").selected() == null ? parseInt($("#CompanyId").val()) : $("#CompanyId").selected();
        product.BrandId = $("#BrandId").selected();
        product.StatusId = $("#StatusId").val();
        product.Name = $("#Name").val();
        product.Summary = $("#Summary").val();
        product.CodeValue = $("#CodeValue").val();
        product.ShowNumber = $("#ShowNumber").val() != undefined ? $("#ShowNumber").val().GetInteger() : 0;
        product.Description = tinymce.get('Description') != null ? tinymce.get('Description').getContent() : "";
        product.Price = $("#Price").val() != undefined ? $("#Price").val().GetInteger() : 0;
        product.ShowHomePage = $("#ShowHomePage").prop("checked");
        product.ProductCustomValue = [];
        product.PictureId = uploadPictureId == null ? $("#LastPictureId").val() : uploadPictureId;
        product.DocId = uploadDocumentId == null ? $("#LastDocId").val() : uploadDocumentId;
        product.Active = ($("#Active").val() === "True");
        product.SyncDatetime = $("#SyncDatetime").val();
        product.CreateDateTime = $("#CreateDateTime").val();
        product.SyncId = $("#SyncId").val();
        product.Quantity = $("#Quantity").val();
        product.VisitCount = $("#VisitCount").val();
        product.UpdateDatetime = $("#UpdateDatetime").val();
        product.SyncDatetime = $("#SyncDatetime").val();
        product.IsService = $('input[name=IsService]:checked').val();
        product.IsCopy = $('input[name=IsCopy]:checked').val();

        product.H1 = $('#H1').val();
        product.Title = $('#Title').val();
        product.Url = $('#Url').val();
        product.Img = $('#Img').val();
        product.Latitude = $("#Latitude").val();
        product.Longitude = $("#Longitude").val();
        product.ProductOptionId = $("#ProductOptionId").selected();
        product.Weight = $('#Weight').val();
        product.MaxOrderCount = $('#MaxOrderCount').val();
        product.MinOrder = $('#MinOrder').val();
        if ($('#NoIndex').is(":checked")) {
            product.NoIndex = true;
        }
        else {
            product.NoIndex = false;
        }
        if ($('#NoFollow').is(":checked")) {
            product.NoFollow = true;
        }
        else {
            product.NoFollow = false;
        }
        product.Canonical = $("#Canonical").val();
        product.MetaTagDescription = $("#MetaTagDescription").val();
        getCustomValues(product);
        product.ProductPicture = [];
        var relatedPictures = $("[other-picture-Id]");
        for (var i = 0; i < relatedPictures.length; i++) {
            var picId = $(relatedPictures[i]).attr("other-picture-Id");
            var picEntity = {
                PictureId: parseInt(picId),
            };
            product.ProductPicture.push(picEntity);
        }
        product.ProductColor = [];

        var colorInputs = $(".color.chk-container input");
        for (var i = 0; i < colorInputs.length; i++) {
            var thisInput = colorInputs[i];
            if ($(thisInput).prop("checked") === true) {
                var colorId = $(thisInput).attr("color-id");
                var colorEntity = {
                    ProductId: product.ID,
                    ColorId: parseInt(colorId),
                };
                product.ProductColor.push(colorEntity);
            }
        }

        product.Review = [];
        $(".rowReview").each(function () {
            var reviwEntity = {
                ProductId: product.ID,
                PictureId: $(this).find(".pictureReviewId").val(),
                Contect: $(this).find(".reviewContent").val()
            };
            product.Review.push(reviwEntity);
        });
        product.ProductSize = [];
        var sizeInputs = $(".size.chk-container input");
        for (var j = 0; j < sizeInputs.length; j++) {
            thisInput = sizeInputs[j];
            if ($(thisInput).prop("checked") === true) {
                var sizeId = $(thisInput).attr("size-id");
                var sizeEntity = {
                    ProductId: product.ID,
                    SizeId: parseInt(sizeId)
                };
                product.ProductSize.push(sizeEntity);
            }
        }
        product.ProductLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    Summary: $(langObject).find("[lang-name='Summary']").val(),
                    Description: tinymce.get($(langObject).find("[lang-name='Description']").attr('id')).getContent(),
                    /* Body: tinymce.get($(langObject).find("[lang-name='Body']").attr('id')).getContent(),*/
                    Url: $(langObject).find("[lang-name='Url']").val(),
                    Title: $(langObject).find("[lang-name='Title']").val(),
                    H1: $(langObject).find("[lang-name='H1']").val()
                };
                product.ProductLanguage.push(langEntity);
            }
        }

    }

    function fillProductType() {
        //var selected = getSelectedValue($("#ShopId")) == undefined ? $("#ShopId").val() : getSelectedValue($("#ShopId"));
        var request = createRequest();
        request.type = REQUEST_TYPE_GET;
        request.url = base_admin_url + "/store/product/FillProductType";
        //request.url = base_admin_url + "/store/product/FillProductType?ShopId=" + selected;
        request.success = function (entity) {
            clearDropDown("ProductTypeId", true);
            clearDropDown("ProductCategoryId", true);
            clearDropDown("SubCategoryId", true);
            clearDropDown("BrandId", true);
            clearDropDown("fillProductOptionId", true);
            var isTypeSelected = $("#isTypeSelected").val() === "False" ? true : false;
            bindDropDown("ProductTypeId", entity, "Name", "ID", isTypeSelected, $("#LastProductTypeId").val());

            fillCategory();
            fillBrand();
            fillCustomFields();
            fillColors();
            fillSize();
            fillProductOption();
        }
        $.ajax(request);
    }

    function fillCustomFields() {

        var productId = $("#ID").val();
        var selectedType = getSelectedValue($("#ProductTypeId"));
        var selectedCategory = getSelectedValue($("#ProductCategoryId"));
        var selectedSubCategory = getSelectedValue($("#ProductSubCategoryId"));
        var selectedBrandId = getSelectedValue($("#BrandId"));
        if (selectedType !== undefined) {
            selectedCategory = selectedCategory === undefined ? 0 : selectedCategory;
            selectedSubCategory = selectedSubCategory === undefined ? 0 : selectedSubCategory;
            selectedBrandId = selectedBrandId === undefined ? 0 : selectedBrandId;
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillProductCustomFields?TypeId=" + selectedType + "&CategoryId=" + selectedCategory + "&SubCategoryId=" + selectedSubCategory + "&BrandId=" + selectedBrandId + "&ProductId=" + productId;
            request.success = function (entity) {
                var result = entity;
                $("#div-custom-part").empty();
                if (result.length > 0) {
                    $("#div-custom-part").show();
                    var html = "";
                    for (var i = 0; i < result.length; i++) {
                        html += getCustomFieldControl(result[i]);
                    }
                    $("#div-custom-part").append(html);
                } else {
                    $("#div-custom-part").hide();
                }
                fillEditors();
            }
            $.ajax(request);

        } else {
            $("#div-custom-part").hide();
        }
    }

    function fillColors() {
        var productId = $("#ID").val();
        productId = productId === undefined ? 0 : productId;
        var selected = getSelectedValue($("#ProductTypeId"));
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillColors?TypeId=" + selected + "&ProductId=" + productId;
            request.success = function (entity) {
                $("#color-container").empty();
                var result = entity;
                if (result.length > 0) {
                    $("#div-color-part").show();
                    var html = "";
                    for (var i = 0; i < result.length; i++) {
                        var item = result[i];
                        var isSelected = item.IsSelected == true ? "checked='checked'" : "";
                        html += "<label class='chk-container color'>";
                        html += "<input color-id='" + item.Id + "' type='checkbox' " + isSelected + ">";
                        html += "<span class='checkmark' style='background-color:#" + item.Hex + "'></span>";
                        html += item.Name;
                        html += "</label>";
                    }
                    $("#color-container").html(html);
                } else {
                    $("#div-color-part").hide();
                }
            }
            $.ajax(request);
        } else {
            $("#div-color-part").hide();
        }
    }

    function fillSize() {
        var selected = getSelectedValue($("#ProductTypeId"));
        var productId = $("#ID").val();
        productId = productId === undefined ? 0 : productId;
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillSize?TypeId=" + selected + "&ProductId=" + productId;
            request.success = function (entity) {
                $("#size-container").empty();
                var result = entity;
                if (result.length > 0) {
                    $("#div-size-part").show();
                    var html = "";
                    for (var i = 0; i < result.length; i++) {
                        var item = result[i];
                        var isSelected = item.IsSelected == true ? "checked='checked'" : "";
                        html += "<label class='chk-container size'>";
                        html += "<input size-id='" + item.Id + "' type='checkbox' " + isSelected + ">";
                        html += "<span class='checkmark' style='background-color:#" + item.Hex + "'></span>";
                        html += item.Name;
                        html += "</label>";

                    }
                    $("#size-container").html(html);
                } else {
                    $("#div-size-part").hide();
                }
            }
            $.ajax(request);
        } else {
            $("#div-size-part").hide();
        }
    }

    function fillBrand() {
        var selected = getSelectedValue($("#ProductTypeId"));
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillProductBrand?TypeId=" + selected;
            request.success = function (entity) {
                clearDropDown("BrandId");
                bindDropDown("BrandId", entity, "Name", "ID", true, $("#LastBrandId").val());
                fillCustomFields();
            }
            $.ajax(request);
        }
    }
    function fillProductOption() {
        var selected = getSelectedValue($("#ProductTypeId"));
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillProductOption?TypeId=" + selected;
            request.success = function (entity) {
                clearDropDown("ProductOptionId");
                bindDropDown("ProductOptionId", entity, "Name", "Id", true, $("#LastProductOptionId").val());
                fillCustomFields();
            };
            $.ajax(request);
        }
    }
    function fillCategory() {
        var selected = getSelectedValue($("#ProductTypeId"));
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillProductCategory?TypeId=" + selected;
            request.success = function (entity) {
                clearDropDown("ProductCategoryId");
                clearDropDown("SubCategoryId");
                bindDropDown("ProductCategoryId", entity, "Name", "ID", true, $("#LastProductCategoryId").val());
                fillCustomFields();
                fillSubCategory();
            }
            $.ajax(request);
        }
    }

    function fillSubCategory() {

        var selected = getSelectedValue($("#ProductCategoryId"));
        if (selected !== undefined) {
            var request = createRequest();
            request.type = REQUEST_TYPE_GET;
            request.url = base_admin_url + "/store/product/FillProductSubCategory?CategoryId=" + selected;
            request.success = function (entity) {
                bindDropDown("ProductSubCategoryId", entity, "Name", "ID", true, $("#LastProductSubCateogryId").val());
                fillCustomFields();
            }
            $.ajax(request);
        }
    }

    function fillEditors() {
        var editors = $("[custom-type='HTML']");
        for (var i = 0; i < editors.length; i++) {
            var editorId = $(editors[i]).attr("id");
            var isFound = false;
            for (var j = 0; j < editorList.length; j++) {
                if (editorList[j] == editorId) {
                    isFound = true;
                }
            }
            if (isFound == false) {
                editorList.push(editorId);
                initCodeprocessEditor(editorId);
            }
        }
    }
}
function initializeProductTypeScript() {
    var entity = {};
    $("#btnSubmit").click(function () {

        fillEntity();
        $.ajax(createRequest(entity));
    });
    $(".attachFile1 .dropify-clear").click(function () {
        AttachFile1 = 0;
    });
    $(".attachFile2 .dropify-clear").click(function () {
        AttachFile2 = 0;
    });
    function fillEntity() {
        entity.CustomLabel = $("#CustomLabel").val();
        entity.ID = $("#ID").val();
        entity.Name = $("#Name").val();
        entity.SyncId = $("#SyncId").val();
        entity.SyncDatetime = $("#SyncDatetime").val();
        entity.Description = $("#Description").val(); /*tinymce.get('Description').getContent();*/
        entity.body = tinymce.get('body').getContent();
        entity.LandingText = tinymce.get('LandingText').getContent();
        entity.Url = $("#Url").val();
        entity.H1 = $("#H1").val();
        entity.Priority = $("#Priority").val();
        entity.PictureId = $("#LastPictureId").val();
        entity.IconId = $("#LastIconId").val();
        entity.title = $("#title").val();
        entity.IsService = $('#IsService').prop('checked');
        entity.Active = $('#Active').prop('checked');
        if ($('#NoIndex').is(":checked")) {
            entity.NoIndex = true;
        }
        else {
            entity.NoIndex = false;
        }
        if ($('#NoFollow').is(":checked")) {
            entity.NoFollow = true;
        }
        else {
            entity.NoFollow = false;
        }
        entity.Canonical = $("#Canonical").val();
        entity.MinBody = $("#MinBody").val();
        //entity.PictureId = uploadPictureId;
        entity.PictureId = uploadPictureId == null ? $("#LastPictureId").val() : uploadPictureId;
        entity.IconId = uploadIconId == null ? $("#LastIconId").val() : uploadIconId;
        entity.ProductTypeLanguage = [];
        var langValues = $("[lang-value]");

        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Description: $(langObject).find("[lang-name='Description']").val(),
                    MinBody: $(langObject).find("[lang-name='MinBody']").val(), /*tinymce.get($(langObject).find("[lang-name='Description']").attr('id')).getContent(),*/
                    H1: $(langObject).find("[lang-name='H1']").val(),
                    Title: $(langObject).find("[lang-name='Title']").val(),
                    Url: $(langObject).find("[lang-name='Url']").val(),
                    body: tinymce.get($(langObject).find("[lang-name='body']").attr('id')).getContent(),
                    landingtext: tinymce.get($(langObject).find("[lang-name='landing-text']").attr('id')).getContent()
                };
                entity.ProductTypeLanguage.push(langEntity);
            }
        }

    }
}
function initializeSmartOfferAnswerScript() {
    var entity = {};
    $("#btnSubmit").click(function () {

        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.Answer = $("#Answer").val();
        entity.Active = $("#Active").val();
        entity.QuestionId = $("#QuestionId").val();
        entity.ID = $("#ID").val();


    }
}

function initializeSmartOfferQuestionScript() {
    var entity = {};
    $("#btnSubmit").click(function () {

        fillEntity();
        $.ajax(createRequest(entity));
    });
   
    function fillEntity() {
        entity.Question = $("#Question").val();
        entity.Active = $("#Active").val();
        entity.TypeId = $("#TypeId").val();
        entity.ID = $("#ID").val();


    }
}
function initializeProductBrandScript() {
    var entity = {};
    $("#btnSubmit").click(function () {

        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ProductTypeId = $("#ProductTypeId").val();
        entity.ID = $("#ID").val();
        entity.Name = $("#Name").val();
        entity.Summary = $("#Summary").val();
        entity.Label = $("#Label").val();
        entity.ShowNumber = $("#ShowNumber").val(); /*tinymce.get('Description').getContent();*/
        entity.Description = tinymce.get('Description').getContent();
        entity.IsPublic = $("#IsPublic").prop('checked');
        entity.Active = $("#Active").prop('checked');
        entity.Title = $("#Title").val();
        entity.PictureId = uploadPictureId == null ? $("#LastPictureId").val() : uploadPictureId;
        //if ($('#NoFollow').is(":checked")) {
        //    entity.NoFollow = true;
        //}
        //else {
        //    entity.NoFollow = false;
        //}
        
        entity.ProductBrandLanguage = [];
        var langValues = $("[lang-value]");

        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Description: tinymce.get($(langObject).find("[lang-name='Description']").attr('id')).getContent(),
                    Summary: $(langObject).find("[lang-name='Summary']").val(),
                    Title: $(langObject).find("[lang-name='Title']").val()
                  
                };
                entity.ProductBrandLanguage.push(langEntity);
            }
        }

    }
}

function initializeColorScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.Name = $("#Name").val();
        entity.HexValue = $("#HexValue").val();
        entity.ProductTypeId = $("#ProductTypeId").val();
        entity.SyncId = $("#SyncId").val();
        entity.SyncDatetime = $("#SyncDatetime").val();
        entity.ColorLanguage = [];

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Description: ""
                };
                entity.ColorLanguage.push(langEntity);
            }
        }
    }
}

function initializeSizeScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.Name = $("#Name").val();
        entity.ProductTypeId = $("#ProductTypeId").val();
        entity.SyncId = $("#SyncId").val();
        entity.SyncDatetime = $("#SyncDatetime").val();
        entity.SizeLanguage = [];

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Description: ""
                };
                entity.SizeLanguage.push(langEntity);
            }
        }
    }
}

function initializeProductCategoryScript() {
    var entity = {};
    $("#btnSubmit").click(function () {

        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.CustomLabel = $("#CustomLabel").val();
        entity.ID = $("#ID").val();
        entity.TypeId = getSelectedValue("#TypeId");
        entity.Name = $("#Name").val();
        entity.Url = $("#Url").val();
        entity.Fee = $('#Fee').val();
        entity.TitlePage = $("#TitlePage").val();
        entity.Description = tinymce.get('Description').getContent();
        entity.LandingText = tinymce.get('LandingText').getContent();
        entity.DescriptionMeta = $("#DescriptionMeta").val(); /*tinymce.get('DescriptionMeta').getContent();*/
        entity.H1 = $("#H1").val();
        entity.Priority = $("#Priority").val();
        entity.SyncId = $("#SyncId").val();
        entity.SyncDatetime = $("#SyncDatetime").val();
        entity.ProductCategoryLanguage = [];
        entity.PictureId = uploadPictureId == null ? $("#LastPictureId").val() : uploadPictureId;
        entity.IconId = uploadIconId == null ? $("#LastIconId").val() : uploadIconId;
        entity.Active = $('#Active').prop('checked');
        if ($('#NoIndex').is(":checked")) {
            entity.NoIndex = true;
        }
        else {
            entity.NoIndex = false;
        }
        if ($('#NoFollow').is(":checked")) {
            entity.NoFollow = true;
        }
        else {
            entity.NoFollow = false;
        }
        entity.Canonical = $("#Canonical").val();
        entity.MinBody = $("#MinBody").val();
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Url: $(langObject).find("[lang-name='Url']").val(),
                    DescriptionMeta: $(langObject).find("[lang-name='DescriptionMeta']").val(),
                    MinBody: $(langObject).find("[lang-name='MinBody']").val(),

                    landingtext: tinymce.get($(langObject).find("[lang-name='landing-text']").attr('id')).getContent(),

                    TitlePage: $(langObject).find("[lang-name='TitlePage']").val(),
                    H1: $(langObject).find("[lang-name='H1']").val(),
                    Description: tinymce.get($(langObject).find("[lang-name='Description']").attr('id')).getContent()
                };
                entity.ProductCategoryLanguage.push(langEntity);
            }
        }
    }
}

function initializeProductCustomFieldScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.ProductTypeId = $("#ProductTypeId").val();
        entity.ProductCategoryId = $("#ProductCategoryId").val();
        entity.ProductSubCategoryId = $("#ProductSubCategoryId").val();
        entity.ProductBrandId = $("#ProductBrandId").val();
        entity.TypeId = getSelectedValue("#TypeId");
        entity.InSearchResult = getCheckedValue("#InSearchResult");
        entity.Hide = getCheckedValue("#Hide");
        entity.Name = $("#Name").val();
        entity.SyncId = $("#SyncId").val();
        entity.SyncName = $("#SyncName").val();
        entity.SyncDatetime = $("#SyncDatetime").val();
        entity.ShowNumber = $("#ShowNumber").val();
        entity.IsRequired = getCheckedValue("#IsRequired");
        entity.ProductCustomFieldLanguage = [];

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                };
                entity.ProductCustomFieldLanguage.push(langEntity);
            }
        }
    }
}
function initializeQuestionRegister() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
     
        entity.TypeId = getSelectedValue("#TypeId");

        entity.Name = $("#Name").val();
      
    }
}

function initializeProductCustomItemScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.FieldId = $("#FieldId").val();
        entity.SyncId = $("#SyncId").val();
        entity.SyncDatetime = $("#SyncDatetime").val();
        entity.Value = $("#Value").val();

        entity.ProductCustomItemLanguage = [];

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Value: $(langObject).find("[lang-name='Value']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val())
                };
                entity.ProductCustomItemLanguage.push(langEntity);
            }
        }
    }
}
function initializeQuestionRegisterItem() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.FieldId = $("#FieldId").val();
     
        entity.Value = $("#Value").val();

        
    }
}
function initializeQuestionRegisterProduct() {
    var entity = [];
    $("#btnSubmit").click(function () {
        fillEntity();
        console.log(entity);
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
       
        var tagInputs = $(".checkBoxQ");
        for (var i = 0; i < tagInputs.length; i++) {
            var thisInput = tagInputs[i];
            if ($(thisInput).prop("checked") === true) {
                var tagId = $(thisInput).attr("field-id");
                var tagEntity = {
                    ProductId: $("#ProductId").val(),
                    FieldId: parseInt(tagId)
                };
                entity.push(tagEntity);
            }
        }
        
    }
}
function initializeDiscountScript() {
    var request = createRequest();
    request.type = REQUEST_TYPE_GET;
    request.url = base_admin_url + "/store/product/SearchAjax";
    request.success = function (result) {
        var products = [];
        for (var i = 0; i < result.length; i++) {
            var entity = {
                value: result[i].Name,
                data: result[i].Id
            };
            products.push(entity);
        }
        $('#ProductBox').autocomplete({
            lookup: products,
            onSelect: function (suggestion) {
                $("#ProductId").val(suggestion.data);
            }
        });
    }
    $.ajax(request);
}

function initializeProductSubCategoryScript() {

    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {

        entity.CustomLabel = $("#CustomLabel").val();
        entity.ID = $("#ID").val();
        entity.CategoryId = getSelectedValue("#CategoryId");
        entity.Name = $("#Name").val();
        entity.H1 = $("#H1").val();
        entity.Description = $("#Description").val();
        entity.Body = tinymce.get('Body').getContent();
        entity.LandingText = tinymce.get('LandingText').getContent();
        entity.Title = $("#Title").val();
        entity.Url = $("#Url").val();
        entity.Priority = $("#Priority").val();
        entity.Active = $('#Active').prop('checked');

        if ($('#NoIndex').is(":checked")) {
            entity.NoIndex = true;
        }
        else {
            entity.NoIndex = false;
        }
        if ($('#NoFollow').is(":checked")) {
            entity.NoFollow = true;
        }
        else {
            entity.NoFollow = false;
        }
        entity.Canonical = $("#Canonical").val();
        entity.MinDescription = $("#MinDescription").val();
        entity.PictureId = uploadPictureId == null ? $("#LastPictureId").val() : uploadPictureId;
        entity.IconId = uploadIconId == null ? $("#LastIconId").val() : uploadIconId;

        entity.ProductSubCategoryLanguage = [];
        entity.TagSubCategory = [];
        var tagInputs = $(".tag-container input");
        for (var i = 0; i < tagInputs.length; i++) {
            var thisInput = tagInputs[i];
            if ($(thisInput).prop("checked") === true) {
                var tagId = $(thisInput).attr("tag-id");
                var tagEntity = {
                    SubCategory: entity.ID,
                    TagId: parseInt(tagId)
                };
                entity.TagSubCategory.push(tagEntity);
            }
        }
        entity.PictureId = uploadPictureId == null ? $("#LastPictureId").val() : uploadPictureId;
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    CategoryId: entity.CategoryId,
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Description: $(langObject).find("[lang-name='description']").val(),
                    MinDescription: $(langObject).find("[lang-name='MinDescription']").val(),
                    body: tinymce.get($(langObject).find("[lang-name='body']").attr('id')).getContent(),
                    landingtext: tinymce.get($(langObject).find("[lang-name='landing-text']").attr('id')).getContent(),

                    Url: $(langObject).find("[lang-name='Url']").val(),
                    H1: $(langObject).find("[lang-name='H1']").val(),
                    Title: $(langObject).find("[lang-name='Title']").val(),

                };
                entity.ProductSubCategoryLanguage.push(langEntity);
            }
        }

    }
}

function initializeShopProfileScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        var shopId = parseInt($("#ID").val())
        entity.ID = shopId;
        entity.PictureId = uploadPictureId === null ? parseInt($("#PictureId").val()) : uploadPictureId;
        entity.WriterPictureId = uploadWriterPictureId === null ? parseInt($("#WriterPictureId").val()) : uploadWriterPictureId;
        entity.Label = $("#Label").val();
        entity.UserCreatorId = parseInt($("#UserCreatorId").val());
        entity.WebsiteId = parseInt($("#WebsiteId").val());
        entity.Approved = $("#Approved").val();
        entity.Active = $("#Active").val();
        entity.Description = $("#Description").val();

        entity.Name = $("#Name").val();
        entity.TypeId = getSelectedValue("#TypeId");
        entity.CityId = getSelectedValue("#CityId");

        entity.ShopAddress = [];
        entity.ShopContact = [];
        entity.ShopProductType = [];
        entity.ShopPaymentType = [];

        var contactItems = $("[contact-id]");
        if (contactItems.length > 0) {
            for (var i = 0; i < contactItems.length; i++) {
                var value = $(contactItems[i]).val();
                if (value !== "") {
                    var typeId = parseInt($(contactItems[i]).attr("contact-id"));
                    var contactEntity = {
                        ShopId: shopId,
                        TypeId: typeId,
                        Value: value
                    };
                    entity.ShopContact.push(contactEntity);
                }
            }
        }

        var selectedTypes = $("[name='ProductType']:checked");
        for (var i = 0; i < selectedTypes.length; i++) {
            var value = $(selectedTypes[i]).val();
            var typeEntity = {
                ShopId: shopId,
                ProductTypeId: value
            };
            entity.ShopProductType.push(typeEntity);
        }

        var selectedPayment = $("[name='PaymentType']:checked");
        for (var i = 0; i < selectedPayment.length; i++) {
            var value = $(selectedPayment[i]).val();
            var paymentEntity = {
                ShopId: shopId,
                PaymentTypeId: value
            };
            entity.ShopPaymentType.push(paymentEntity);
        }

        var addEntity = {
            ID: parseInt($("#AddressId").val()),
            Address: $("#Address").val()
        };

        entity.ShopAddress.push(addEntity);
    }
}

function initializeReportByWeekdayScript() {
    var labels = [];
    var series = [];
    var visitCount = [];
    visitCount.push(0);
    var request = createRequest();
    request.type = REQUEST_TYPE_GET;
    request.url = base_admin_url + "/report/websiteview/reportbyweekday";
    request.success = function (entity) {
        for (var i = 0; i < entity.length; i++) {
            var temp = entity[i];
            if (i == 0)
                visitCount[0] = parseInt(temp.Value);
            labels.push(temp.Key);
            visitCount.push(parseInt(temp.Value));
        }
        series.push({
            name: 'بازدید روزانه',
            data: visitCount
        });

        new Chartist.Line('#simple-line-chart', {
            labels: labels,
            series: series
        },
            {
                plugins: [
                    Chartist.plugins.tooltip()
                ]
            }
        );
    }
    $.ajax(request);
}

function initializeOrderListScript() {
    $(document).ready(function () {
        $("[order-id]").click(function () {
            var isSelected = $(this).prop("checked");
            var orderId = $(this).attr("order-id");
            var selectedRow = $("tr[row-order-id='" + orderId + "']");
            $(selectedRow).attr("row-selected", isSelected);

            var selectedItems = $("[row-selected='true']");
            var selectedIds = "";
            for (var i = 0; i < selectedItems.length; i++) {
                var newRowId = $(selectedItems[i]).attr("row-order-id");
                selectedIds = selectedIds + "," + $(selectedItems[i]).attr("row-order-id");
            }
            var hrefValueStore = $(".goto-store").attr("href");
            var hrefValueProcess = $(".goto-process").attr("href");
            var splitHrefStore = hrefValueStore.split("orderId=");
            var splitHrefProcess = hrefValueProcess.split("orderId=");
            hrefValueStore = splitHrefStore[0] + "orderId=0" + selectedIds;
            hrefValueProcess = splitHrefProcess[0] + "orderId=0" + selectedIds;
            $(".goto-store").attr("href", hrefValueStore);
            $(".goto-process").attr("href", hrefValueProcess);
        });

        $("[select-all='true']").click(function () {
            var ischecked = $("[order-id]").prop("checked");
            $("[order-id]").prop("checked", ischecked);
            $("[order-id]").trigger("click");
        });

        $("#chkCheckAll").click(function () {
            var doAction = $(this).attr("do-action");
            if (doAction == "true") {
                $("[name='statusId']").prop("checked", true);
                $(this).attr("do-action", "false");
            } else {
                $("[name='statusId']").prop("checked", false);
                $(this).attr("do-action", "true");
            }
        });
    });
}


function initializeClearingListScript() {
    $(document).ready(function () {
        $("#chkCheckAll").click(function () {
            var doAction = $(this).attr("do-action");
            if (doAction == "true") {
                $("[name='codeId']").prop("checked", true);
                $(this).attr("do-action", "false");
            } else {
                $("[name='codeId']").prop("checked", false);
                $(this).attr("do-action", "true");
            }
        });
    });
}


function initializeProductQuantityScript() {
    $("#btnSubmit").click(function () {

        var productId = $("[name=ProductId]").val();
        var listQuantiy = [];
        var rowList = $("[quantity-id]");
        for (var i = 0; i < rowList.length; i++) {
            var item = rowList[i];
            var entity = {
                ID: parseInt($(item).attr("quantity-id")),
                Price: parseInt($(item).find("input.price").val()),
                Count: parseInt($(item).find("input.count").val())
            };
            listQuantiy.push(entity);
        }
        var data = { productId: productId, listQuantiy: listQuantiy };
        $.ajax(createRequest(data));
    });
}

function initializeMenuPageScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.ParentId = getSelectedValue("#ParentId");
        entity.TypeId = getSelectedValue("#TypeId");
        entity.PostId = getSelectedValue("#PostId");
        entity.CategoryId = getSelectedValue("#CategoryId");
        entity.GalleryId = getSelectedValue("#GalleryId");
        entity.Active = getCheckedValue("#Active");
        entity.Name = $("#Name").val();
        entity.Link = $("#Link").val();
        entity.ShowNumber = parseInt($("#ShowNumber").val());
        entity.MenuLanguage = [];

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    CategoryId: entity.CategoryId,
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Description: ""
                };
                entity.MenuLanguage.push(langEntity);
            }
        }
    }
}

function initializeDashboardPageScript() {

    $("#checkRebateOnline").click(function () {
        var codeValue = $("[name='codevalue']").val();
        var entity = {
            CodeValue: codeValue
        };
        var response = createRequest(entity);
        response.url = "/store/rebate/checkrebate";
        response.success = function (result) {
            if (result.Type === MESSAGE_ERROR) {
                createMessage(MESSAGE_TYPE_ERROR, result.Body);
            } else if (result.Type === MESSAGE_SUCCESS) {
                $(".btn-container").show();
                $("#btnPrintRebate").attr("href", "/store/rebate/print/" + codeValue);
            }
        };
        $.ajax(response);
    });

    $("#btnCancelRebate").click(function () {
        var codeValue = $("[name='codevalue']").val();
        var entity = {
            CodeValue: codeValue
        };
        var response = createRequest(entity);
        response.url = "/store/rebate/cancelrebate";
        response.success = function (result) {
            if (result.Type === MESSAGE_ERROR) {
                createMessage(MESSAGE_TYPE_ERROR, result.Body);
            } else if (result.Type === MESSAGE_SUCCESS) {
                $(".btn-container").hide();
                $("#btnPrintRebate").attr("href", "");
                $("[name='codevalue']").val("");
            }
        };
        $.ajax(response);
    });

}

function initializeBannerPageScript() {
    $(".banner_page .category_id").change(function () {
        var request = createRequest();
        request.type = REQUEST_TYPE_GET;
        request.url = base_admin_url + "/content/category/GetCategoryPicture?CategoryId=" + $(this).val();
        request.success = function (entity) {
            $(".banner_page img.category_img").attr("src", entity);
        }
        $.ajax(request);
    })

    var banner = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(banner));
    });
    function fillEntity() {
        banner.ID = $("#ID").val();
        banner.Name = $("#Name").val();
        banner.CategoryId = $("#CategoryId").val();
        banner.Summary = $("#Summary").val();
        banner.Url = $("#Url").val();
        banner.ShowNumber = $("#ShowNumber").val();
        banner.PictureId = uploadPictureId == null ? $("#pictureUploadId").val() : uploadPictureId;
        banner.BannerLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    Summary: $(langObject).find("[lang-name='Summary']").val()
                };
                banner.BannerLanguage.push(langEntity);
            }
        }
    }


}
function initializeGalleryPageScript() {
    $(".banner_page .category_id").change(function () {
        var request = createRequest();
        request.type = REQUEST_TYPE_GET;
        request.url = base_admin_url + "/content/category/GetCategoryPicture?CategoryId=" + $(this).val();
        request.success = function (entity) {
            $(".banner_page img.category_img").attr("src", entity);
        }
        $.ajax(request);
    })

    var gallery = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(gallery));
    });
    function fillEntity() {
        gallery.ID = $("#ID").val();
        gallery.Name = $("#Name").val();
        gallery.CategoryId = $("#CategoryId").val();
        gallery.WebsiteId = $("#WebsiteId").val();
        gallery.UpdateDateTime = $("#UpdateDateTime").val();
        gallery.CreateDateTime = $("#CreateDateTime").val();
        gallery.Name = $("#Name").val();
        gallery.Body = $("#Body").val();
        gallery.ShowNumber = $("#ShowNumber").val();
        gallery.Active = $("#Active").val();

        gallery.GalleryLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    Description: $(langObject).find("[lang-name='Description']").val()
                };
                gallery.GalleryLanguage.push(langEntity);
            }
        }
    }


}

function initializeClearingPageScript() {
    $(".clearing_page #StoreId").change(function () {
        var request = createRequest();
        request.type = REQUEST_TYPE_GET;
        request.url = base_admin_url + "/clearing/GetStoreClearing?storeId=" + $(this).val();
        request.success = function (result) {
            $(".clearing_page #Price").val(result.Price);
            $(".clearing_page #ToAccount").val(result.ToAccount);
        };
        $.ajax(request);
    });
}

function initializeStoreProductScript() {
    $(".tr-column .change-status").click(function () {

        var storeProductId = $(this).parents(".tr-column").children(".td-store-productid").attr("store-productid");
        var ProductName = $(this).parents(".tr-column").children(".td-product-name").attr("product-name");
        var statusId = $(this).parents(".tr-column").children(".td-statusid").attr("statusid");
        $("#modal-change-status").find(".store-productid").val(storeProductId);
        $("#modal-change-status").find(".product-name").html(ProductName);
        $("#modal-change-status").find(".statusId").val(statusId);
        $("#modal-change-status").modal("show");
    });

    $("#modal-change-status .btn-submit").click(function () {
        var storeProductId = $("#modal-change-status").find(".store-productid").val();
        var statusId = $("#modal-change-status").find(".statusId").val();
        var request = createRequest();
        request.type = REQUEST_TYPE_GET;
        request.url = base_admin_url + "/storeproduct/ChangeStatus?storeProductId=" + storeProductId + "&statusId=" + statusId;
        request.success = function (result) {
            location.reload();
        };
        $.ajax(request);
    });


}



$(document).on('change', ".rowReview input", function () {

    var $t = $(this);
    var fileUpload = $t.get(0);
    var files = fileUpload.files;
    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }
    var request = createRequest();
    request.url = base_admin_url + "/Upload/UploadPhoto";
    request.type = "POST";
    request.data = data,
        request.contentType = false;
    request.processData = false;
    request.beforeSend = function () {
        $t.val("");
        $("#modalLoading").modal("show");
    }
    request.error = function () {
        createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
    };
    request.success = function (entity) {
        entityobj = entity;
        var panelUrl = $("#PanelUrl").val();
        var url = entity.Url;
        url = url.replace("SYSTEM_TYPE_PANEL", panelUrl);
        $t.parent().parent(".rowReview").find(".reviewImg").attr("src", url);
        $t.parent().parent(".rowReview").find(".imgBox").removeClass("d-none");
        $t.parent().parent(".rowReview").find(".review-picture-item").addClass("d-none");
        $t.parent().parent(".rowReview").find(".delete-review-picture").removeClass("d-none");
        $t.parent().parent(".rowReview").find(".pictureReviewId").val(entity.Id);
        $("#modalLoading").modal("hide");
    };
    $.ajax(request);
});


function addReview() {
    var revHtml = "<div class='rowReview d-flex mt-3'><input type='hidden' class='pictureReviewId' value='' />";
    revHtml += "<textarea class='reviewContent w-100 m-2' placeholder='توضیحات' rows='3'></textarea>";
    revHtml += "<div class='imgBox d-none'><img class='reviewImg m-2' />";
    revHtml += "<label class='delete-review-picture d-none m-2' >حذف تصویر</label></div><div class='review-picture-item m-2'><input class='upoladFile ' type='file' /></div>";
    revHtml += "<label class='remove-review-item m-2'><i class='fa fa-close' aria-hidden='true'></i></label></div>";
    $(".reviewBox").prepend(revHtml);
}


$(document).on('click', ".delete-review-picture", function () {
    $(this).parent().parent(".rowReview").find(".review-picture-item").removeClass("d-none");
    $(this).parent(".imgBox").addClass("d-none");
    //$(this).parent(".imgBox").find(".delete-review-picture").addClass("d-none");
    $(this).parent(".imgBox").find(".reviewImg").attr("src", "");
    $(this).parent().parent(".rowReview").find(".pictureReviewId").val("");

});
$(document).on('click', ".remove-review-item", function () {
    $(this).parent('.rowReview').remove();

});
$("#CategoryId").change(function () {

    var request = createRequest();
    var selectedCategoryId = $(this).children("option:selected").val();
    request.type = REQUEST_TYPE_GET;
    request.url = base_admin_url + "/store/ProductSubCategory/PriorityList?categoryId=" + selectedCategoryId;
    request.success = function (result) {

        $("#divprority").html(result);
    }
    $.ajax(request);
});
$("#TypeId").change(function () {

    var request = createRequest();
    var selectedTypeId = $(this).children("option:selected").val();
    request.type = REQUEST_TYPE_GET;
    request.url = base_admin_url + "/store/ProductCategory/PriorityList?typeId=" + selectedTypeId;
    request.success = function (result) {
        $("#divprority").html(result);
    }
    $.ajax(request);
});

function UpdateTagSubcategory(subcategoryId) {

    var request = createRequest();
    //var subcategoryId = $("#subcategoryId").val();

    request.type = REQUEST_TYPE_GET;
    request.url = base_admin_url + "/store/ProductSubCategory/GetTags?id=" + subcategoryId;

    request.success = function (result) {

        $("#divAllTag").html(result);
    }
    $.ajax(request);
}
function SubmitTage(subcategoryId) {

    var request = createRequest();
    var tageName = $("#tagText").val();
    request.type = REQUEST_TYPE_POST;
    request.url = base_admin_url + "/store/ProductSubCategory/AddTage?tageName=" + tageName;
    request.success = function (result) {

        if (result.Type == "2") {
            UpdateTagSubcategory(subcategoryId);
            $("#tagText").val("");
        }
        else {
            alert(result.Body);
        }

    }
    $.ajax(request);
}
function UpdateTagPost(postId) {

    var request = createRequest();
    //var subcategoryId = $("#subcategoryId").val();

    request.type = REQUEST_TYPE_GET;
    request.url = base_admin_url + "/content/Post/GetTags?id=" + postId;

    request.success = function (result) {

        $("#divAllTag").html(result);
    }
    $.ajax(request);
}


function initializeNewGalleryScript() {
    //$(".banner_page .category_id").change(function () {
    //    var request = createRequest();
    //    request.type = REQUEST_TYPE_GET;
    //    request.url = base_admin_url + "/content/category/GetCategoryPicture?CategoryId=" + $(this).val();
    //    request.success = function (entity) {
    //        $(".banner_page img.category_img").attr("src", entity);
    //    }
    //    $.ajax(request);
    //})



    var Gallery = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(Gallery));
    });
    function fillEntity() {
        debugger;
        Gallery.ID = $("#ID").val();
        Gallery.CategoryId = $("#CategoryId").val();
        Gallery.WebsiteId = $("#WebsiteId").val();
        Gallery.ShowNumber = $("#ShowNumber").val();
        Gallery.Name = $("#Name").val();
        Gallery.CreateDateTime = $("#CreateDateTime").val();
        Gallery.GalleryLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                };
                Gallery.GalleryLanguage.push(langEntity);
            }
        }
    }
}

function initializeNewSurveyScript() {
    var survey = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(survey));
    });
    function fillEntity() {
        survey.ID = $("#ID").val();
        survey.Active = $("#Active").val();

        survey.DateTime = $("#DateTime").val();
        survey.Name = $("#Name").val();
        survey.Label = $("#Label").val();
        survey.SurveyLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),
                    Name: $(langObject).find("[lang-name='Name']").val(),
                };
                survey.SurveyLanguage.push(langEntity);
            }
        }
    }
}

function initializeSurveyQuestionScript() {
    var entity = {};
    $("#btnSubmit").click(function () {

        fillEntity();
        $.ajax(createRequest(entity));
    });
    $(".divPicture .dropify-clear").click(function () {
        uploadPictureId = 0;
    });
    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.SurveyId = getSelectedValue("#SurveyId");
        entity.QuestionType = getSelectedValue("#QuestionType");
        entity.Question = $("#Question").val();
        entity.ShowNumber = $("#ShowNumber").val();
        entity.ReletedQuestionId = $("#ReletedQuestionId").val();

        entity.Active = $("#Active").val();
        entity.PictureId = uploadPictureId == null ? $("#pictureUploadId").val() : uploadPictureId;
        entity.SurveyQuestionLanguage = [];
        //entity.Active = $('#Active').prop('checked');

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Question: $(langObject).find("[lang-name='Question']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),

                };
                entity.SurveyQuestionLanguage.push(langEntity);
            }
        }
    }
}

function initializeSurveyQuestionItemScript() {
    var entity = {};
    $("#btnSubmit").click(function () {

        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.QuestionId = $("#QuestionId").val();
        /*getSelectedValue("#QuestionId");*/
        entity.Name = $("#Name").val();
        entity.ShowNumber = $("#ShowNumber").val();
        entity.Active = $("#Active").val();

        entity.SurveyQuestionItemLanguage = [];
        //entity.Active = $('#Active').prop('checked');

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val()),

                };
                entity.SurveyQuestionItemLanguage.push(langEntity);
            }
        }
    }


}

function initializeStateCityScript() {
    $("#StateId").change(function () {
        $.ajax({
            type: "Get",
            url: "/panel/baseinfo/statecity/GetCitiesDD",
            data: { StateId: $(this).val(), withAll: "true" }
        }).done(function (res) {
            $(".cities-div").html(res);
        });


    })
}

$(document).on("change", ".simple-upload-pic", function () {
    var element = $(this);
    var idTarget = element.attr("target-id");
    var nameTarget = element.attr("target-name");
    var showTarget = element.attr("target-show-name");
    var srcNameTarget = element.attr("target-src-name");
    var fileUpload = element.get(0);
    var files = fileUpload.files;
    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }
    var request = createRequest();
    request.url = base_admin_url + "/Upload/UploadPhoto";
    request.type = "POST";
    request.data = data,
        request.contentType = false;
    request.processData = false;
    request.beforeSend = function () {
        element.val("");
        $("#modalLoading").modal("show");
    }
    request.error = function () {
        createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود تصویر");
    };
    request.success = function (entity) {
        if (idTarget !== undefined) {
            $("#" + idTarget).val(entity.Id);
        }
        if (nameTarget !== undefined) {
            $("[name='" + nameTarget + "']").val(entity.Id);
        }
        var src = entity.Url;
        src = src.replace("SYSTEM_TYPE_PANEL", base_admin_url);
        if (showTarget !== undefined) {

            $("[name='" + showTarget + "']").attr("src", src);
        }
        if (srcNameTarget !== undefined) {

            $("[name='" + srcNameTarget + "']").val(src);
        }
        $("#modalLoading").modal("hide");
    };
    $.ajax(request);
});

$(document).on("change", ".simple-upload-file", function () {
    var element = $(this);
    var idTarget = element.attr("target-id");
    var nameTarget = element.attr("target-name");
    var showTarget = element.attr("target-show-name");
    var hrefNameTarget = element.attr("target-href-name");
    var fileUpload = element.get(0);
    var files = fileUpload.files;
    var data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
    }
    var request = createRequest();
    request.url = base_admin_url + "/Upload/UploadDocument";
    request.type = "POST";
    request.data = data,
        request.contentType = false;
    request.processData = false;
    request.beforeSend = function () {
        element.val("");
        $("#modalLoading").modal("show");
    }
    request.error = function () {
        createMessage(MESSAGE_TYPE_ERROR, "خطا", "خطا در هنگام آپلود فایل");
    };
    request.success = function (entity) {
        if (idTarget !== undefined) {
            $("#" + idTarget).val(entity.Id);
        }
        if (nameTarget !== undefined) {
            $("[name='" + nameTarget + "']").val(entity.Id);
        }
        var src = entity.Url;
        src = src.replace("SYSTEM_TYPE_PANEL", base_admin_url);
        if (showTarget !== undefined) {

            $("[name='" + showTarget + "']").attr("src", src);
        }
        if (hrefNameTarget !== undefined) {

            $("[name='" + hrefNameTarget + "']").val(src);
        }
        $("#modalLoading").modal("hide");
    };
    $.ajax(request);
});


function initializeProductOptionScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.Name = $("#Name").val();
        entity.ProductTypeId = $("#ProductTypeId").val();
        entity.ShowNumber = $("#ShowNumber").val();
        entity.ProductOptionLanguage = [];

        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val())
                   
                };
                entity.ProductOptionLanguage.push(langEntity);
            }
        }
    }
}

function initializeProductOptionItemScript() {
    var entity = {};
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });

    function fillEntity() {
        entity.ID = $("#ID").val();
        entity.Value = $("#Value").val();
        entity.OptionId = $("#OptionId").val();
        entity.ProductOptionItemLanguage = [];
        var langValues = $("[lang-value]");
        if (langValues.length > 0) {
            for (var i = 0; i < langValues.length; i++) {
                var langObject = langValues[i];
                var langEntity = {
                    Name: $(langObject).find("[lang-name='Name']").val(),
                    LanguageId: parseInt($(langObject).find("[lang-name='LanguageId']").val())

                };
                entity.ProductOptionItemLanguage.push(langEntity);
            }
        }
    }
}
function initializeAnswerExamePageScript() {


    var entity = [];
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });
    function fillEntity() {

        var Values = $("[question-value]");
        if (Values.length > 0) {
            for (var i = 0; i < Values.length; i++) {
                var itemObject = Values[i];
                var itemEntity = {
                    QuestionId: parseInt($(itemObject).find("[question-name='QuestionId']").val()),
                    AnswerId: parseInt($(itemObject).find("[question-name = 'Answer']:checked").val()),
                };
                entity.push(itemEntity);
            }
        }
    }


}
function initializeExameReportByCoursePageScript() {


    var entity = [];
    $("#btnSubmit").click(function () {
        fillEntity();
        $.ajax(createRequest(entity));
    });
    function fillEntity() {

        var Values = $("[question-value]");
        if (Values.length > 0) {
            for (var i = 0; i < Values.length; i++) {
                var itemObject = Values[i];
                var itemEntity = {
                    ID: parseInt($(itemObject).find("[question-name='ID']").val()),
                    TotalRate:$(itemObject).find("[question-name = 'TotalRate']").val(),
                };
                entity.push(itemEntity);
            }
        }
    }


}