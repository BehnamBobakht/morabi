using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Base
{
    public class BaseMessage
    {
        public static ViewMessage GetMessage()
        {
            return new ViewMessage() { Type = Enum_MessageType.NONE, Body = "" };
        }

        public static ViewMessage GetMessage(Enum_MessageType type, string message)
        {
            return new ViewMessage()
            {
                Type = type,
                Body = message
            };
        }

        public static ViewMessage GetMessage(Enum_Message message)
        {
            return GetMessage(Enum_MessageType.ERROR, message);
        }

        public static ViewMessage GetMessage(Enum_MessageType type)
        {
            return new ViewMessage() { Type = type, Body = "" };
        }

        public static ViewMessage GetMessage(Enum_MessageType type, Enum_Message message)
        {
            string msg = (GetMessageString(message, BaseLanguage.GetCurrentLanguage()));
            return new ViewMessage() { Type = type, Body = msg };
        }

        private static string GetMessageString(Enum_Message message, Enum_Lang lang)
        {
            string msg = "";
            switch (message)
            {
                case Enum_Message.REQUIRED_ACCOUNT_State_VALUE:
                    if (lang == Enum_Lang.EN)
                        msg = "State IS Requierd";
                    else if (lang == Enum_Lang.RU)
                        msg = "Представление успешно";
                    else
                        msg = "انتخاب استان الزامی می باشد";
                    break;
                case Enum_Message.REQUIRED_ACCOUNT_ADDRESS_ZipCode:
                    if (lang == Enum_Lang.EN)
                        msg = "ZipCode IS Requierd";
                    else if (lang == Enum_Lang.RU)
                        msg = "Представление успешно";
                    else
                        msg = "کدپستی الزامی می باشد";
                    break;
                case Enum_Message.REQUIRED_ACCOUNT_City_VALUE:
                    if (lang == Enum_Lang.EN)
                        msg = "city IS Requierd";
                    else if (lang == Enum_Lang.RU)
                        msg = "Представление успешно";
                    else
                        msg = "انتخاب شهر الزامی می باشد";
                    break;

                case Enum_Message.LastAddToBasket:
                    if (lang == Enum_Lang.EN)
                        msg = "This product has already been added to your cart";
                    else if (lang == Enum_Lang.RU)
                        msg = " ";
                    else
                        msg = "این محصول قبلا به سبد خرید شما افزوده شده است";
                    break;

                case Enum_Message.REQUIRED_ACCOUNT_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Nationalcode IS Requierd";
                    else if (lang == Enum_Lang.RU)
                        msg = "Представление успешно";
                    else
                        msg = "نام و نام خانوادگی الزامی است";
                    break;
                case Enum_Message.DUPLICATED_ACCOUNT_NationalCode:
                    if (lang == Enum_Lang.EN)
                        msg = "Nationalcode IS Requierd";
                    else if (lang == Enum_Lang.RU)
                        msg = "Представление успешно";
                    else
                        msg = "کد ملی وارد شده تکراری است";
                    break;
                    
                case Enum_Message.REQUIRED_ACCOUNT_NationalCode:
                    if (lang == Enum_Lang.EN)
                        msg = "Nationalcode IS Requierd";
                    else if (lang == Enum_Lang.RU)
                        msg = "Представление успешно";
                    else
                        msg = "کد ملی الزامی است";
                    break;      
                case Enum_Message.INVALID_ACCOUNT_NationalCode:
                    if (lang == Enum_Lang.EN)
                        msg = "Nationalcode IS Invalid";
                    else if (lang == Enum_Lang.RU)
                        msg = "Представление успешно";
                    else
                        msg = "کد ملی اشتباه است";
                    break;

                case Enum_Message.SUCCESSFULL_SUBMIT:
                    if (lang == Enum_Lang.EN)
                        msg = "Submission Successful";
                    else if (lang == Enum_Lang.RU)
                        msg = "Представление успешно";
                    else if (lang == Enum_Lang.ZH)
                        msg = " 成功註冊";
                    else
                        msg = "با موفقیت ثبت شد";
                    break;

                case Enum_Message.INVALID_ZipCode_FORMAT:
                    if (lang == Enum_Lang.EN)
                        msg = "ZipCode isnt in Correct format";
                    else if (lang == Enum_Lang.RU)
                        msg = "Представление успешно";
                    else
                        msg = "فرمت کد پستی صحیح نمی باشد";
                    break;


                case Enum_Message.INVALID_REAGENT_NationalCode:
                    if (lang == Enum_Lang.EN)
                        msg = "nationalcode isnt in Correct format";
                    else if (lang == Enum_Lang.RU)
                        msg = "Представление успешно";
                    else
                        msg = "فرمت کد ملی صحیح نمی باشد";
                    break;


                case Enum_Message.SUCCESSFULL_UPDATE:
                    if (lang == Enum_Lang.EN)
                        msg = "Update Complete";
                    else if (lang == Enum_Lang.RU)
                        msg = "Обновление завершено";
                    else
                        msg = "با موفقیت به روز رسانی شد";
                    break;
                case Enum_Message.SUCCESSFULL_API:
                    if (lang == Enum_Lang.EN)
                        msg = "Success";
                    else if (lang == Enum_Lang.AR)
                        msg = "فعلت بنجاح";
                    else if (lang == Enum_Lang.RU)
                        msg = "успех";
                    else if (lang == Enum_Lang.ZH)
                        msg = " 成功註冊";                 
                    else
                        msg = "با موفقیت انجام شد";
                    break;

                case Enum_Message.REQUIRED_ACCOUNT_CAPTCHA:
                    if (lang == Enum_Lang.EN)
                        msg = "captcha requier ";
                    else if (lang == Enum_Lang.RU)
                        msg = "успех";
                    else
                        msg = "کد امنیتی را  وارد کنید";
                    break;

                case Enum_Message.CHECK_ACCOUNT_CAPTCHA:
                    if (lang == Enum_Lang.EN)
                        msg = "captcha isnt valid ";
                    else if (lang == Enum_Lang.RU)
                        msg = "успех";
                    else
                        msg = "کد امنیتی صحیح نیست";
                    break;


                case Enum_Message.SUCCESSFULL_ADD_BASKET:
                    if (lang == Enum_Lang.EN)
                        msg = "Added To Basket";
                    else if (lang == Enum_Lang.RU)
                        msg = "Добавлено в корзину";
                    else
                        msg = "با موفقیت به سبد خرید اضافه شد";
                    break;
                case Enum_Message.SUCCESSFULL_DELETE_BASKET:
                    if (lang == Enum_Lang.EN)
                        msg = "Removed From Basket";
                    else if (lang == Enum_Lang.RU)
                        msg = "Удалено из корзины";
                    else
                        msg = "با موفقیت از سبد خرید حذف شد";
                    break;
                case Enum_Message.SUCCESSFULL_NEWSLETTER:
                    if (lang == Enum_Lang.EN)
                        msg = "Your Newsletter Registration Is Complete";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ваша рассылка Регистрация завершена";
                    else
                        msg = "شما با موفقیت در خبرنامه ثبت نام کردید";
                    break;
                case Enum_Message.SUCCESSFULL_NEWSLETTER_SENT:
                    if (lang == Enum_Lang.EN)
                        msg = "Sent To Newsletter Members Successfully";
                    else if (lang == Enum_Lang.RU)
                        msg = "Успешно отправлено участникам рассылки";
                    else
                        msg = "با موفقیت برای اعضای خبرنامه ارسال شد";
                    break;
                case Enum_Message.SUCCESSFULL_FORGET_PASSWORD:
                    if (lang == Enum_Lang.EN)
                        msg = "The password recovery link was sent to you via SMS / Email";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ссылка для восстановления пароля была отправлена ​​вам по SMS / электронной почте.";
                    else
                        msg = "لینک بازیابی رمز عبور برای شما پیامک/ایمیل شد";
                    break;
                case Enum_Message.SUCCESSFULL_FORGET_PASSWORD_SENDEMAIL:
                    if (lang == Enum_Lang.EN)
                        msg = "A user password has been sent to your email";
                    else if (lang == Enum_Lang.RU)
                        msg = "Пароль пользователя был отправлен на вашу электронную почту";
                    else
                        msg = "رمز عبور کاربری به ایمیل شما ارسال شده است";
                    break;
                case Enum_Message.FORGOTTEN_PASSWORD:
                    if (lang == Enum_Lang.EN)
                        msg = "Forget password";
                    else if (lang == Enum_Lang.RU)
                        msg = "забыть пароль";
                    else
                        msg = "فراموشی رمز عبور";
                    break;
                case Enum_Message.EDIT_PASSWORD:
                    if (lang == Enum_Lang.EN)
                        msg = "Your password has been edited";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ваш пароль изменен";
                    else
                        msg = "رمز عبور کاربری شما ویرایش شده است";
                    break;
                case Enum_Message.SUCCESSFULL_PRODUCT_NOTIFY:
                    if (lang == Enum_Lang.EN)
                        msg = "When This Product Will Be Available, You Will Be Notified By Email And SMS ";
                    else if (lang == Enum_Lang.RU)
                        msg = "Когда этот продукт будет доступен, вы будете уведомлены по электронной почте и SMS";
                    else
                        msg = "در صورت موجود شدن این محصول، ایمیل و پیامک اطلاع رسانی برای شما ارسال خواهد شد";
                    break;
                case Enum_Message.SUCCESSFULL_LOGIN:
                    if (lang == Enum_Lang.EN)
                        msg = "Your login was successful";
                    else if (lang == Enum_Lang.RU)
                        msg = "Your login was successful";
                    else
                        msg = "ورود شما با موفقیت انجام شد";
                    break;
                case Enum_Message.SUCCESSFULL_REGISTER:
                    if (lang == Enum_Lang.EN)
                        msg = "Your registration was successful";
                    else if (lang == Enum_Lang.RU)
                        msg = "Your registration was successful";
                    else if (lang == Enum_Lang.ZH)
                        msg = " 成功註冊";
                    else
                        msg = "ثبت نام شما با موفقیت انجام شد";
                    break;
                case Enum_Message.SUCCESSFULL_COMMENT:
                    if (lang == Enum_Lang.EN)
                        msg = "Your comment was successfully submitted";
                    else if (lang == Enum_Lang.RU)
                        msg = "Your comment was successfully submitted";
                    else if (lang == Enum_Lang.ZH)
                        msg = " 成功註冊";
                    else
                        msg = "نظر شما با موفقیت ثبت شد";
                    break;
                case Enum_Message.DUPLICATED_SITEUSER_EMAIL:
                    if (lang == Enum_Lang.EN)
                        msg = "Email Already Registered";
                    else if (lang == Enum_Lang.RU)
                        msg = "Электронная почта уже зарегистрирована";
                    else
                        msg = "این ایمیل قبلا در سیستم ثبت شده است";
                    break;
                case Enum_Message.DUPLICATED_ACCOUNT_EMAIL:
                    if (lang == Enum_Lang.EN)
                        msg = "Email Already User";
                    else if (lang == Enum_Lang.RU)
                        msg = "Электронная почта уже используется";
                    else
                        msg = "ایمیل وارد شده در سیستم تکراری می باشد";
                    break;
                case Enum_Message.DUPLICATED_ACCOUNT_MOBILE:
                    if (lang == Enum_Lang.EN)
                        msg = "Cell Number Already Used";
                    else if (lang == Enum_Lang.RU)
                        msg = "Учетная запись с таким телефоном уже существует";
                    else
                        msg = "این شماره موبایل قبلا در سیستم ثبت نام شده است";
                    break;
                case Enum_Message.INVALID_USERNAME_PASSWORD:
                    if (lang == Enum_Lang.EN)
                        msg = "Invalid Username or Password";
                    else if (lang == Enum_Lang.RU)
                        msg = "Неправильное имя пользователя или пароль";
                    else
                        msg = "نام کاربری یا رمز عبور صحیح وارد نشده است";
                    break;
                case Enum_Message.OutOfSend_Store:
                    if (lang == Enum_Lang.EN)
                        msg = "Invalid Username or Password";
                    else if (lang == Enum_Lang.RU)
                        msg = "Неправильное имя пользователя или пароль";
                    else
                        msg = "کاربر گرامی، آدرس انتخابی شما، خارج از محدوده‌ی ارسال فروشنده است.";
                    break;
                case Enum_Message.INVALID_RECOVER_EMAIL:
                    if (lang == Enum_Lang.EN)
                        msg = "Email Not Registered";
                    else if (lang == Enum_Lang.RU)
                        msg = "Электронная почта не зарегистрирована";
                    else
                        msg = "ایمیل وارد شده در سیستم ثبت نشده است";
                    break;
                case Enum_Message.INVALID_RECOVER_UNIQUE_ID:
                    if (lang == Enum_Lang.EN)
                        msg = "Error Validating Password";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ошибка проверки пароля";
                    else
                        msg = "خطا در اعتبارسنجی تغییر رمز عبور";
                    break;
                case Enum_Message.INVALID_SITEUSER_EMAIL:
                    if (lang == Enum_Lang.EN)
                        msg = "Email Format Is Not Correct";
                    else if (lang == Enum_Lang.AR)
                        msg = "تنسيق البريد الإلكتروني المدخل غير صحيح";
                    else if (lang == Enum_Lang.RU)
                        msg = "Неправильный формат адреса электронной почты";
                    else
                        msg = "فرمت ایمیل وارد شده صحیح نمی باشد";
                    break;
                case Enum_Message.INVALID_SHOP_CREATOR:
                    if (lang == Enum_Lang.EN)
                        msg = "Referring Code Is Not Correct";
                    else if (lang == Enum_Lang.RU)
                        msg = "Неправильный код реферера";
                    else
                        msg = "کد معرفی وارد شده صحیح نمی باشد";
                    break;
                case Enum_Message.INVALID_ACCOUNT_EMAIL:
                    if (lang == Enum_Lang.EN)
                        msg = "Email Format Is Not Correct";
                    else if (lang == Enum_Lang.AR)
                        msg = "تنسيق البريد الإلكتروني المدخل غير صحيح";
                    else if (lang == Enum_Lang.RU)
                        msg = "Неправильный формат адреса электронной почты";
                    else
                        msg = "فرمت ایمیل وارد شده صحیح نمی باشد";
                    break;
                case Enum_Message.INVALID_ACCOUNT_MOBILE:
                    if (lang == Enum_Lang.EN)
                        msg = "Cell Number Format Is Not Correct";
                    else if (lang == Enum_Lang.RU)
                        msg = "Неправильный формат номера мобильного телефона";
                    else
                        msg = "فرمت موبایل وارد شده صحیح نمی باشد";
                    break;
                case Enum_Message.INVALID_SHOP_USER:
                    if (lang == Enum_Lang.EN)
                        msg = "This Store Isn't Available";
                    else if (lang == Enum_Lang.RU)
                        msg = "Данный магазин временно недоступен";
                    else
                        msg = "عدم امکان دسترسی به این فروشگاه";
                    break;
                case Enum_Message.INVALID_SITEUSER_CREATORID:
                    if (lang == Enum_Lang.EN)
                        msg = "Referrer Code Is Not Correct";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "کد معرف صحیح وارد نشده است";
                    break;
                case Enum_Message.INVALID_SHOP_MAX_PRODUCT:
                    if (lang == Enum_Lang.EN)
                        msg = "The Max Number Of Products Is Registered For This Store";
                    else if (lang == Enum_Lang.RU)
                        msg = "Для данного магазина зарегистрировано максимальное количество товаров  ";
                    else
                        msg = "ماکسیمم تعداد محصول برای این فروشگاه ثبت شده است";
                    break;
                case Enum_Message.INVALID_API_KEY:
                    if (lang == Enum_Lang.EN)
                        msg = "Please login to your account first";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ошибка проверки запроса";
                    else
                        msg = "لطفا ابتدا وارد حساب کاربری خود شوید";
                    break;
                case Enum_Message.INVALID_DATA:
                    if (lang == Enum_Lang.EN)
                        msg = "Invalid Entry";
                    else if (lang == Enum_Lang.RU)
                        msg = "Неверный ввод";
                    else
                        msg = "داده های نامعتبر";
                    break;
                case Enum_Message.INVALID_PAYMENT_OBJECT:
                    if (lang == Enum_Lang.EN)
                        msg = "Error Making Payment Details";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ошибка при совершении платежа";
                    else
                        msg = "خطا در ساخت اطلاعات پرداخت";
                    break;
                case Enum_Message.INVALID_REBATE_VALUE:
                    if (lang == Enum_Lang.EN)
                        msg = "Discount Code Not Found";
                    else if (lang == Enum_Lang.RU)
                        msg = "Код скидки не найден";
                    else
                        msg = "کد تخفیف وارد شده صحیح نمی باشد";
                    break;
                case Enum_Message.INVALID_PRODUCT_ACTIVE:
                    if (lang == Enum_Lang.EN)
                        msg = "Selling Of This Product Is DE active";
                    else if (lang == Enum_Lang.RU)
                        msg = "Продажа данного товара неактивна";
                    else
                        msg = "وضعیت فروش این محصول غیرفعال می باشد";
                    break;
                case Enum_Message.INVALID_ACCOUNT_DATA:
                    if (lang == Enum_Lang.EN)
                        msg = "User Information Is Not Correct";
                    else if (lang == Enum_Lang.RU)
                        msg = "Информация о пользователе неверна";
                    else
                        msg = "اطلاعات کاربر صحیح وارد نشده است";
                    break;
                case Enum_Message.INVALID_PRODUCT_QUANTITY:
                    if (lang == Enum_Lang.EN)
                        msg = "Out Of Stock";
                    else if (lang == Enum_Lang.RU)
                        msg = "Нет на складе";
                    else
                        msg = "موجودی این کالا به اتمام رسیده است";
                    break;
                case Enum_Message.INVALID_PRODUCT_QUANTITY_SIZE:
                    if (lang == Enum_Lang.EN)
                        msg = "Out Of Stock On This Size";
                    else if (lang == Enum_Lang.RU)
                        msg = "Данного размера нет в наличии";
                    else
                        msg = "موجودی این کالا در این سایز به اتمام رسیده است";
                    break;
                case Enum_Message.INVALID_PRODUCT_QUANTITY_COLOR:
                    if (lang == Enum_Lang.EN)
                        msg = "Out Of Stock On This Color";
                    else if (lang == Enum_Lang.RU)
                        msg = "Данного цвета нет в наличии";
                    else
                        msg = "موجودی این کالا در این رنگ به اتمام رسیده است";
                    break;
                case Enum_Message.INVALID_PRODUCT_QUANTITY_COLOR_SIZE:
                    if (lang == Enum_Lang.EN)
                        msg = "Out Of Stock On This Color And Size";
                    else if (lang == Enum_Lang.RU)
                        msg = "Данного цвета и размера нет в наличии";
                    else
                        msg = "موجودی این کالا در این رنگ و سایز به اتمام رسیده است";
                    break;
                case Enum_Message.INVALID_MOBILE_FORMAT:
                    if (lang == Enum_Lang.EN)
                        msg = "Wrong Cell Number";
                    else if (lang == Enum_Lang.RU)
                        msg = "Неправильный формат номера мобильного телефона";
                    else
                        msg = "شماره موبایل وارد شده صحیح نمی باشد";
                    break;
                case Enum_Message.INVALID_EMAIL_FORMAT:
                    if (lang == Enum_Lang.EN)
                        msg = "Email Format Is Not Correct";
                    else if (lang == Enum_Lang.AR)
                        msg = "تنسيق البريد الإلكتروني المدخل غير صحيح";
                    else if (lang == Enum_Lang.RU)
                        msg = "Неправильный формат адреса электронной почты";
                    else
                        msg = "فرمت ایمیل وارد شده صحیح نمی باشد";
                    break;
                case Enum_Message.INVALID_RECAPTCHA:
                    if (lang == Enum_Lang.EN)
                        msg = "Please Click On I'm Not A Robot";
                    else if (lang == Enum_Lang.RU)
                        msg = "Пожалуйста, подтвердите, что Вы не робот";
                    else
                        msg = "لطفا بر روی من ربات نیستم کلیک کنید";
                    break;
                case Enum_Message.INVALID_REAGENT_CODE:
                    if (lang == Enum_Lang.EN)
                        msg = "Referred Code Not Found";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ссылочный код не найден";
                    else
                        msg = "کد معرف وارد شده صحیح نمی باشد";
                    break;
                case Enum_Message.INVALID_BARCODE_VALUE:
                    if (lang == Enum_Lang.EN)
                        msg = "There is no barcode with this information in system";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "اطلاعات این بارکد در سیستم یافت نشد";
                    break;
                case Enum_Message.INVALID_ORDER_TRACKING_CODE:
                    if (lang == Enum_Lang.EN)
                        msg = "There is no order with this information in system";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "سفارشی با این اطلاعات در سیستم ثبت نشده است";
                    break;

                case Enum_Message.INVALID_ORDER_REBATE_BEFORE:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "برای این سفارش قبلا کد تخفیف ثبت شده است";
                    break;
                case Enum_Message.INVALID_REBATE_DATETIME:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "تاریخ استفاده ازین کد تخفیف به پایان رسیده";
                    break;
                case Enum_Message.INVALID_REBATE_USECOUNT:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "تعداد استفاده ازین کد تخفیف به پایان رسیده";
                    break;
                case Enum_Message.INVALID_REBATE_MIN_INVOICE_VALUE:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "مبلغ این فاکتور برای استفاده ازین کد تخفیف به حد نصاب نرسیده است";
                    break;
                case Enum_Message.INVALID_REBATE_MAX_INVOICE_VALUE:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "مبلغ این فاکتور برای استفاده ازین کد تخفیف بیش از حد مجاز است";
                    break;
                case Enum_Message.INVALID_REBATE_MOBILE:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "این کد تخفیف برای شماره موبایل دیگری می باشد";
                    break;

                case Enum_Message.USER_ID_INACTIVE:
                    if (lang == Enum_Lang.EN)
                        msg = "Your Account Has Been Deactivated. Please Contact Support";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ваш аккаунт был деактивирован. Пожалуйста, свяжитесь со службой поддержки";
                    else
                        msg = "کاربری شما غیرفعال شده است. لطفا با مدیر سامانه تماس بگیرید";
                    break;
                case Enum_Message.REQUIRED_GALLERY_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Gallery Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод названия галереи обязателен";
                    else
                        msg = "وارد کردن نام گالری اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_CATEGORY_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Category Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести название категории";
                    else
                        msg = "وارد کردن نام دسته بندی اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_POST_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Post Title Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод названия поста обязателен";
                    else
                        msg = "وارد کردن عنوان پست اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_MENU_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Menu Title Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод названия меню обязателен";
                    else
                        msg = "وارد کردن عنوان منو اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_MENU_TYPE_ITEM:
                    if (lang == Enum_Lang.EN)
                        msg = "Menu Type Not Selected";
                    else if (lang == Enum_Lang.RU)
                        msg = "Тип меню не выбран";
                    else
                        msg = "نوع منو انتخاب نشده است";
                    break;
                case Enum_Message.REQUIRED_MENU_POST_ITEM:
                    if (lang == Enum_Lang.EN)
                        msg = "Related Post Not Selected";
                    else if (lang == Enum_Lang.RU)
                        msg = "Связанный пост не выбран";
                    else
                        msg = "پست مربوطه انتخاب نشده است";
                    break;
                case Enum_Message.REQUIRED_MENU_CATEGORY_ITEM:
                    if (lang == Enum_Lang.EN)
                        msg = "Related Category Not Selected";
                    else if (lang == Enum_Lang.RU)
                        msg = "Связанная категория не выбрана";
                    else
                        msg = "دسته بندی مربوطه انتخاب نشده است";
                    break;
                case Enum_Message.REQUIRED_CATEGORY_WEBSITE_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Web Site Name Not Entered";
                    else if (lang == Enum_Lang.RU)
                        msg = "Название сайта не введено";
                    else
                        msg = "نام وبسایت وارد نشده است";
                    break;
                case Enum_Message.REQUIRED_MENU_GALLERY_ITEM:
                    if (lang == Enum_Lang.EN)
                        msg = "Related Gallery Not Selected";
                    else if (lang == Enum_Lang.RU)
                        msg = "Связанная галерея не выбрана";
                    else
                        msg = "گالری مربوطه انتخاب نشده است";
                    break;
                case Enum_Message.REQUIRED_USERGROUP_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "User Group Name Not Entered";
                    else if (lang == Enum_Lang.RU)
                        msg = "Название группы пользователей не введено";
                    else
                        msg = "نام گروه کاربری وارد نشده است";
                    break;
                case Enum_Message.REQUIRED_SITEUSER_FULLNAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Full User Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести полное имя пользователя";
                    else
                        msg = "نام و نام خانوادگی کاربر اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_SITEUSER_EMAIL:
                    if (lang == Enum_Lang.EN)
                        msg = "User Email Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "E-mail пользователя обязателен";
                    else
                        msg = "ایمیل کاربر اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_SITEUSER_MOBILE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Cell Number Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести номер мобильного телефона пользователя ";
                    else
                        msg = "وارد کردن شماره موبایل اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_SITEUSER_PASSWORD:
                    if (lang == Enum_Lang.EN)
                        msg = "Password is mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Пароль обязателен";
                    else
                        msg = "رمز عبور کاربر اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_PASSWORD:
                    if (lang == Enum_Lang.EN)
                        msg = "Password is mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Пароль обязателен";
                    else
                        msg = "رمز عبور اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_PASSWORD_CONFIRM:
                    if (lang == Enum_Lang.EN)
                        msg = "Re Entering Password Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Повторный ввод пароля обязателен";
                    else
                        msg = "وارد کردن تکرار رمز عبور اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_PASSWORD_PREVIOUS:
                    if (lang == Enum_Lang.EN)
                        msg = "Previous Password Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "وارد کردن رمز عبور قبلی اجباری می باشد";
                    break;
                case Enum_Message.INVALID_PASSWORD_PREVIOUS:
                    if (lang == Enum_Lang.EN)
                        msg = "Previous Password Is Incorrect";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "رمز عبور قبلی صحیح وارد نشده است";
                    break;
                case Enum_Message.REQUIRED_SELECT_LANGUAGE:
                    if (lang == Enum_Lang.EN)
                        msg = "No language Selected";
                    else if (lang == Enum_Lang.RU)
                        msg = "Не выбран язык";
                    else
                        msg = "هیچ زبانی انتخاب نشده است";
                    break;
                case Enum_Message.REQUIRED_EVENT_DESCRIPTION:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Descriptions For The Event Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод описаний для события является обязательным";
                    else
                        msg = "وارد کردن توضیحات برای رویداد اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_WEBSITE_TITLE:
                    if (lang == Enum_Lang.EN)
                        msg = "Web Site Title Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Название сайта обязательно";
                    else
                        msg = "عنوان وبسایت اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_DOMAIN:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Domain Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести домен";
                    else
                        msg = "وارد کردن دامنه اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_SHOP_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Store Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести название магазина";
                    else
                        msg = "وارد کردن نام فروشگاه اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_SHOP_LABEL:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Store Tag Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести тег магазина";
                    else
                        msg = "وارد کردن برچسب فروشگاه اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_SHOP_DESCRIPTION:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Store Details Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести данные магазина";
                    else
                        msg = "وارد کردن توضیحات فروشگاه اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_PRODUCT_TYPE_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести имя или название";
                    else
                        msg = "وارد کردن نام اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_PRODUCT_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Product Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести название продукта";
                    else
                        msg = "وارد کردن نام محصول اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_PRODUCT_SHOP:
                    if (lang == Enum_Lang.EN)
                        msg = "Selecting A Store Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо выбрать магазин";
                    else
                        msg = "انتخاب فروشگاه اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_PRODUCT_BRAND_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Brand Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "وارد کردن نام برند اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_PRODUCT_STATUS:
                    if (lang == Enum_Lang.EN)
                        msg = "Selecting Product Status Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо выбрать статус продукта";
                    else
                        msg = "انتخاب وضعیت محصول اجباری می باشد";
                    break;
                //case Enum_Message.INVALID_PRODUCT_NAME_LENGTH:
                //    if (lang == Enum_Lang.EN)
                //        msg = "Selecting Product Status Is Mandatory";
                //    else if (lang == Enum_Lang.RU)
                //        msg = "Необходимо выбрать статус продукта";
                //    else
                //        msg = "نام محصول نباید بیش از 40 حرف باشد";
                //    break;
                case Enum_Message.REQUIRED_COMMENT_BODY:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Comment Text Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод текста комментария является обязательным";
                    else
                        msg = "وارد کردن متن نظر اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_BASKET_ITEM:
                    if (lang == Enum_Lang.EN)
                        msg = "No Item In The Basket";
                    else if (lang == Enum_Lang.RU)
                        msg = "Нет товара в корзине";
                    else
                        msg = "آیتم ای در سبد ثبت نشده است";
                    break;
                case Enum_Message.INVALID_PASSWORD_CONFIRM:
                    if (lang == Enum_Lang.EN)
                        msg = "The Passwords Do Not Match";
                    else if (lang == Enum_Lang.RU)
                        msg = "Пароли не совпадают";
                    else
                        msg = "رمز ها باهم مطابقت ندارند";
                    break;       
                case Enum_Message.INVALID_OLD_PASSWORD:
                    if (lang == Enum_Lang.EN)
                        msg = "Incorect old Passwords ";
                    else if (lang == Enum_Lang.RU)
                        msg = "Пароли не совпадают";
                    else
                        msg = "رمز قبلی صحیح نیست";
                    break;
                case Enum_Message.REQUIRED_EMAIL:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Cell Number Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести номер мобильного телефона пользователя";
                    else
                        msg = "وارد کردن آدرس ایمیل اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_CUSTOM_FIELD_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Field Title Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести название поля";
                    else
                        msg = "وارد کردن عنوان فیلد اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_PRODUCT_CUSTOM_ITEM_VALUE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Amount Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести сумму";
                    else
                        msg = "وارد کردن مقدار اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_PRODUCT_CATEGORY_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Category Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод названия категории обязателен";
                    else
                        msg = "وارد کردن نام دسته بندی اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_PRODUCT_SUB_CATEGORY_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Sub-Category Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести название подкатегории";
                    else
                        msg = "وارد کردن نام زیردسته اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_PRODUCTCOMMENT_BODY:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Comment Text Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести текст комментария";
                    else
                        msg = "وارد کردن متن نظر اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_TEMPLATE_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Template Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести название шаблона";
                    else
                        msg = "وارد کردن نام قالب اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_TEMPLATE_LABEL:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Template Tag Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести шаблон тега";
                    else
                        msg = "وارد کردن برچسب قالب اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_SLIDER_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Slide Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести название слайда";
                    else
                        msg = "وارد کردن عنوان اسلایدر اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_SLIDER_PICTURE:
                    if (lang == Enum_Lang.EN)
                        msg = "Uploading Slide File Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо загрузить файл слайда";
                    else
                        msg = "وارد کردن تصویر اسلایدر اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_ACCOUNT:
                    if (lang == Enum_Lang.EN)
                        msg = "IUser Details Not Entered";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести данные пользователя";
                    else
                        msg = "اطلاعات کاربر وارد نشده است";
                    break;
                case Enum_Message.REQUIRED_ACCOUNT_FULLNAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Full Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод полного имени обязателен";
                    else
                        msg = "وارد کردن نام و نام خانوادگی اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_ACCOUNT_FULLNAMEEN:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Full Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод полного имени обязателен";
                    else
                        msg = "وارد کردن نام و نام خانوادگی انگلیسی اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_ACCOUNT_EMAIL:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Email Is Mandatory";
                    else if (lang == Enum_Lang.AR)
                        msg = "مطلوب إدخال البريد الإلكتروني";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо указать адрес электронной почты";
                    else
                        msg = "وارد کردن ایمیل اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_USERNAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Username Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо указать адрес электронной почты";
                    else
                        msg = "وارد کردن نام کاربری الزامی می باشد";
                    break;
                case Enum_Message.DUPLICATED_ACCOUNT_USERNAME:
                    if (lang == Enum_Lang.EN)
                        msg = "This username is in use";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо указать адрес электронной почты";
                    else
                        msg = "این نام کاربری متعلق به کاربر دیگری می باشد";
                    break;
                case Enum_Message.Validate_ACCOUNT_PASSWORD:
                    if (lang == Enum_Lang.EN)
                        msg = "Password must contain at least 6 characters, one number and one letter";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "رمز عبور باید حداقل شامل 6 کاراکتر، یک عدد و یک حرف باشد";
                    break;
                case Enum_Message.REQUIRED_ACCOUNT_PASSWORD:
                    if (lang == Enum_Lang.EN)
                        msg = "Password Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести пароль";
                    else
                        msg = "رمز عبور اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_ACCOUNT_MOBILE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Cell Number Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести номер мобильного телефона пользователя ";
                    else
                        msg = "وارد کردن شماره موبایل اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_ACCOUNT_EMAIL_OR_MOBILE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Mobile Or Cell Number Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод номера мобильного или сотового телефона является обязательным";
                    else
                        msg = "وارد کردن ایمیل یا موبایل اجباری می باشد";
                    break;

                case Enum_Message.REQUIRED_ACCOUNT_EMAIL_AND_MOBILE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Mobile And Cell Number Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод номера мобильного или сотового телефона является обязательным";
                    else
                        msg = "وارد کردن ایمیل و موبایل اجباری می باشد";
                    break;

                case Enum_Message.REQUIRED_SHOPCONTACT_VALUE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Value Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод значения является обязательным";
                    else
                        msg = "وارد کردن مقدار اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_LANGUAGE_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Language Name Is mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Необходимо ввести название языка";
                    else
                        msg = "وارد کردن نام زبان اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_LANGUAGE_LABEL:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Language Tag Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод языкового тега обязателен";
                    else
                        msg = "وارد کردن برچسب زبان اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_LANGUAGE_CULTURE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Language Index Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод языкового индекса обязателен";
                    else
                        msg = "وارد کردن مشخصه زبان اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_DISCOUNTGROUP:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Discount Type Type Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод имени обязателен";
                    else
                        msg = "وارد کردن نوع تخفیف اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_DISCOUNT_TYPE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Discount Type Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод имени обязателен";
                    else
                        msg = "وارد کردن تخفیف اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_DISCOUNTGROUP_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод имени обязателен";
                    else
                        msg = "وارد کردن نام اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_DISCOUNT_SHOP:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Store Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Вход в магазин обязателен";
                    else
                        msg = "وارد کردن فروشگاه اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_DISCOUNT_PRODUCTTYPE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Product Sort Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод сортировки товара обязателен";
                    else
                        msg = "وارد کردن نوع محصول اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_DISCOUNT_CATEGORY:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Product Category Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод категории продукта является обязательным";
                    else
                        msg = "وارد کردن دسته بندی محصول اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_DISCOUNT_SUBCATEGORY:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Product Sub-Category Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод подкатегории продукта является обязательным";
                    else
                        msg = "وارد کردن زیردسته محصول اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_DISCOUNT_PRODUCT:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Product Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод продукта обязателен";
                    else
                        msg = "وارد کردن محصول اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_DISCOUNT_BRAND:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Product Brand Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Вводить товарный бренд обязательно";
                    else
                        msg = "وارد کردن برند محصول اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_DISCOUNT_VARIENTY:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Varienty Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Вводить товарный бренд обязательно";
                    else
                        msg = "وارد کردن تنوع اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_DISCOUNT_PRICE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Discount Value Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Вводить товарный бренд обязательно";
                    else
                        msg = "وارد کردن مبلغ اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_DISCOUNT_PERCENT:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Discount Value Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Вводить товарный бренд обязательно";
                    else
                        msg = "وارد کردن درصد تخفیف اجباری می باشد";
                    break;
                case Enum_Message.INVALID_DISCOUNT_PERCENT:
                    if (lang == Enum_Lang.EN)
                        msg = "The Discount Percent Do Not Match";
                    else if (lang == Enum_Lang.RU)
                        msg = "Вводить товарный бренд обязательно";
                    else
                        msg = "درصد تخفیف صحیح نیست";
                    break;
                case Enum_Message.REQUIRED_NEWSLETTER_VALUE:
                    if (lang == Enum_Lang.EN)
                        msg = "Newsletter Content Not Filled";
                    else if (lang == Enum_Lang.RU)
                        msg = "Содержание информационного бюллетеня не заполнено";
                    else
                        msg = "محتوای خبرنامه پر نشده است";
                    break;
                case Enum_Message.REQUIRED_SMS_NUMBER:
                    if (lang == Enum_Lang.EN)
                        msg = "Selecting Number Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Выбор номера обязателен";
                    else
                        msg = "انتخاب شماره اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_SMS_TYPE:
                    if (lang == Enum_Lang.EN)
                        msg = "Selecting Sims Type Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Выбор типа симов является обязательным";
                    else
                        msg = "انتخاب نوع ارسال پیام اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_SMS_BODY:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "وارد کردن متن پیامک اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_TELEGRAMBOT_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Robot Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод имени робота обязательно";
                    else
                        msg = "وارد کردن نام ربات اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_TELEGRAMBOT_TOKEN:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Robot Token Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод токена робота обязателен";
                    else
                        msg = "وارد کردن توکن ربات اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_TELEGRAMCOMMAND_COMMAND:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Command Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод имени команды является обязательным";
                    else
                        msg = "وارد کردن دستور اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_TELEGRAMCOMMAND_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Command Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод команды обязателен";
                    else
                        msg = "وارد کردن نام دستور اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_TELEGRAM_KEYBOARD_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Keyboard Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод имени клавиатуры обязателен";
                    else
                        msg = "وارد کردن نام کیبورد اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_TELEGRAMKEYBOARDITEM_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Item Title Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод названия элемента является обязательным";
                    else
                        msg = "وارد کردن عنوان آیتم اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_REBATE_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Discount Code Title Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод названия кода скидки обязательно";
                    else
                        msg = "وارد کردن عنوان کد تخفیف اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_REBATE_CODEVALUE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Discount Code Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод кода скидки обязателен";
                    else
                        msg = "وارد کردن کد تخفیف اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_REBATE_SHOP:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Store Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Вход в магазин обязателен";
                    else
                        msg = "وارد کردن فروشگاه اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_REBATE_PRODUCTTYPE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Product Type Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод типа продукта является обязательным";
                    else
                        msg = "وارد کردن نوع محصول اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_REBATE_PRODUCTCATEGORY:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Category Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод категории является обязательным";
                    else
                        msg = "وارد کردن دسته بندی اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_REBATE_PRODUCTSUBCATEGORY:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Sub-Category Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод подкатегории является обязательным";
                    else
                        msg = "وارد کردن زیردسته اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_REBATE_PRODUCTBRAND:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Brand Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод бренда является обязательным";
                    else
                        msg = "وارد کردن برند اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_REBATE_VALUE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Discount Code Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод кода скидки обязателен";
                    else
                        msg = "وارد کردن کد تخفیف اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_PACKAGE_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Package Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод имени пакета является обязательным";
                    else
                        msg = "وارد کردن نام بسته اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_USERGROUP_DASHBOARD:
                    if (lang == Enum_Lang.EN)
                        msg = "Selecting Dashboard Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Выбор панели инструментов является обязательным";
                    else
                        msg = "انتخاب داشبورد اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_ACCOUNT_ADDRESS_NAMEFAMILY:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Receiver's Full Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод полного имени получателя обязательно";
                    else
                        msg = "وارد کردن نام و نام خانوادگی تحویل گیرنده اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_ACCOUNT_ADDRESS_PHONE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Receiver's Phone Number Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод номера телефона получателя обязателен";
                    else
                        msg = "وارد کردن شماره تلفن تحویل گیرنده اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_ACCOUNT_ADDRESS_MOBILE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Receiver's Cell Number Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод номера ячейки получателя обязателен";
                    else
                        msg = "وارد کردن شماره موبایل تحویل گیرنده اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_ACCOUNT_ADDRESS_VALUE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Address Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод адреса обязателен";
                    else
                        msg = "وارد کردن آدرس اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_ACCOUNT_ADDRESS_STATEID:
                    if (lang == Enum_Lang.EN)
                        msg = "Selecting State Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод почтового индекса обязателен";
                    else
                        msg = "انتخاب اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_ACCOUNT_ADDRESS_POSTAL_CODE:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Post Code Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод почтового индекса обязателен";
                    else
                        msg = "وارد کردن کد پستی اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_BANNER_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Banner Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод имени баннера является обязательным";
                    else
                        msg = "وارد کردن نام بنر اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_MALL_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Mall Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод названия торгового центра является обязательным";
                    else
                        msg = "وارد کردن نام پاساژ اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_COLOR_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Color Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод названия цвета является обязательным";
                    else
                        msg = "وارد کردن نام رنگ اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_SIZE_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Size Title Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод размера Название обязательно";
                    else
                        msg = "وارد کردن عنوان سایز اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_COLOR_HEX:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Color Code Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод цветового кода обязателен";
                    else
                        msg = "وارد کردن کد رنگ اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_REAGENT_MOBILE_VALUES:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Cell Number Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод номера ячейки обязателен";
                    else
                        msg = "وارد کردن شماره موبایل اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_CONTACT_FORM:
                    if (lang == Enum_Lang.EN)
                        msg = "Contact Form Information Not Entered";
                    else if (lang == Enum_Lang.RU)
                        msg = "Контактная форма информациИ не введена";
                    else
                        msg = "اطلاعات فرم تماس وارد نشده است";
                    break;
                case Enum_Message.REQUIRED_CONTACT_FORM_FULLNAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Full Name Is Mandatory";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод полного имени обязателен";
                    else
                        msg = "وارد کردن نام و نام خانوادگی اجباری می باشد";
                    break;


                case Enum_Message.REQUIRED_FORM:
                    if (lang == Enum_Lang.EN)
                        msg = "Enter the essentials";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод полного имени обязателен";
                    else
                        msg = "اطلاعات اجباری فرم را وارد نمایید ";
                    break;


                case Enum_Message.REQUIRED_CONTACT_FORM_EMAIL:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Email Is Mandatory";
                    else if (lang == Enum_Lang.AR)
                        msg = "مطلوب إدخال البريد الإلكتروني";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод адреса электронной почты обязателен";
                    else
                        msg = "وارد کردن ایمیل اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_CONTACT_FORM_BODY:
                    if (lang == Enum_Lang.EN)
                        msg = "Entering Text Is Mandatory";
                    else if (lang == Enum_Lang.AR)
                        msg = "إدخال النص إلزامي";
                    else if (lang == Enum_Lang.RU)
                        msg = "Ввод текста обязателен";
                    else
                        msg = "وارد کردن متن اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_TRACKING_CODE:
                    if (lang == Enum_Lang.EN)
                        msg = "Tracking Code ins Required";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "وارد کردن کد پیگیری پرداخت اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_FILE:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "فایلی بارگذاری نشده است";
                    break;
                case Enum_Message.REQUIRED_RESELLER_GALLERY_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "وارد کردن عنوان اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_RESELLER_GALLERY_FILE:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "انتخاب فایل اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_RESELLER_GALLERY_IMAGE:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "انتخاب تصویر کاور اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_SHOPRESELLER_COLLECTION_PICTURE_ID:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "انتخاب عکس پیش فرض برای کالکشن اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_SHOPRESELLER_COLLECTION_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "وارد کردن نام کالکشن اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_WEBSITEFORM_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "وارد کردن عنوان فرم اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_WEBSITEFORM_LABEL:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "وارد کردن برچسب فرم اجباری می باشد";
                    break;
                case Enum_Message.REQUIRED_SHOPRESELLER_COLLECTION_PRODUCT:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "هیچ محصولی انتخاب نشده است";
                    break;
                case Enum_Message.REQUIRED_STORE_NAME:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "لطفا نام فروشگاه را وارد نمایید";
                    break;
                case Enum_Message.REQUIRED_STORE_ADDRESS_VALUE:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "لطفا آدرس فروشگاه را وارد نمایید";
                    break;
                case Enum_Message.REQUIRED_STORE_ADDRESS:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "لطفا آدرس خود را وارد  نمایید";
                    break;
                case Enum_Message.REQUIRED_STORE_DESCRIPTION:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "لطفا توضیحات را وارد نمایید";
                    break;
                case Enum_Message.REQUIRED_STORE_LOGIN:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "قبل از ثبت نام فروشگاه ورود یا ثبت نام کاربری را انجام دهید";
                    break;
                case Enum_Message.INVALID_STORE_MULTIPLE_STORE:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "شما قبلا با این حساب کاربری فروشگاه ثبت کرده اید";
                    break;
                case Enum_Message.SUCCESSFULL_REGISTER_STORE:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "فروشگاه با موفقیت ثبت شد لطفا منتظر تایید مدیر بمانید";
                    break;

                case Enum_Message.DUPLICATED_NEWSLETTER:
                    if (lang == Enum_Lang.EN)
                        msg = "You Already Enrolled In Newsletter";
                    else if (lang == Enum_Lang.RU)
                        msg = "Вы уже зарегистрированы в рассылке";
                    else
                        msg = "شما قبلا در خبرنامه ثبت نام کرده اید";
                    break;
                case Enum_Message.ERROR_REBATE_USED_BEFORE:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "این کد قبلا استفاده شده است";
                    break;
                case Enum_Message.ERROR_FILE_MAX_SIZE:
                    if (lang == Enum_Lang.EN)
                        msg = "";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "حجم فایل بیش از حد مجاز است";
                    break;
                case Enum_Message.UNIQUE_EMAIL:
                    if (lang == Enum_Lang.EN)
                        msg = "Email Already Used";
                    else if (lang == Enum_Lang.RU)
                        msg = "Email уже используется";
                    else
                        msg = "آدرس ایمیل وارد شده در سیستم تکراری می باشد";
                    break;
                case Enum_Message.UNIQUE_MOBILE:
                    if (lang == Enum_Lang.EN)
                        msg = "Cell Number Already Used";
                    else if (lang == Enum_Lang.RU)
                        msg = "Сотовый номер уже используется";
                    else
                        msg = "شماره موبایل وارد شده در سیستم تکراری می باشد";
                    break;

                case Enum_Message.REQUIRED_CLEARING_DETAIL:
                    if (lang == Enum_Lang.EN)
                        msg = "Insert The Detail";
                    else if (lang == Enum_Lang.RU)
                        msg = "Сотовый номер уже используется";
                    else
                        msg = "توضیحات را وارد نمایید";
                    break;
                case Enum_Message.REQUIRED_CLEARING_PRICE:
                    if (lang == Enum_Lang.EN)
                        msg = "Insert The Price";
                    else if (lang == Enum_Lang.RU)
                        msg = "Сотовый номер уже используется";
                    else
                        msg = "مبلغ را تعیین نمایید";
                    break;
                case Enum_Message.REQUIRED_CLEARING_CODE:
                    if (lang == Enum_Lang.EN)
                        msg = "Select Type";
                    else if (lang == Enum_Lang.RU)
                        msg = "Сотовый номер уже используется";
                    else
                        msg = "نوع تسویه را انتخاب نمایید";
                    break;
                case Enum_Message.REQUIRED_CLEARING_STOREID:
                    if (lang == Enum_Lang.EN)
                        msg = "Select The Store";
                    else if (lang == Enum_Lang.RU)
                        msg = "Сотовый номер уже используется";
                    else
                        msg = "لطفا فروشنده را انتخاب نمایید";
                    break;
                case Enum_Message.REQUIRED_CLEARING_FROMACCOUNT:
                    if (lang == Enum_Lang.EN)
                        msg = "Insert The From Account";
                    else if (lang == Enum_Lang.RU)
                        msg = "Сотовый номер уже используется";
                    else
                        msg = "حساب بانکی مبدا را وارد نمایید";
                    break;
                case Enum_Message.REQUIRED_CLEARING_TOACCOUNT:
                    if (lang == Enum_Lang.EN)
                        msg = "Insert The To Account";
                    else if (lang == Enum_Lang.RU)
                        msg = "Сотовый номер уже используется";
                    else
                        msg = "حساب بانکی مقصد را وارد نمایید";
                    break;
                case Enum_Message.INVALID_USERNAME:
                    if (lang == Enum_Lang.EN)
                        msg = "Username : Lowercase letters and numbers are acceptable";
                    else if (lang == Enum_Lang.RU)
                        msg = "Сотовый номер уже используется";
                    else
                        msg = "نام کاربری : حروف کوچک و اعداد انگلیسی قابل قبول است";
                    break;
                case Enum_Message.LIST_EMPTY:
                    if (lang == Enum_Lang.EN)
                        msg = "List IS Empty";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "لیست خالی است";
                    break;
                case Enum_Message.ERROR_BASKET_QUANTITY:
                    if (lang == Enum_Lang.EN)
                        msg = "Product inventory is not enough";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "موجودی محصول کافی نیست";
                    break;
                case Enum_Message.SUCCESSFULL_REBATE_HAS:
                    if (lang == Enum_Lang.EN)
                        msg = "Rebate is valid";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "کد تخفیف معتبر است";
                    break;
                case Enum_Message.INVALID_ACCOUNT_CardNumber:
                    if (lang == Enum_Lang.EN)
                        msg = "Card Number Invalid";
                    else if (lang == Enum_Lang.RU)
                        msg = "";
                    else
                        msg = "فرمت شماره کارت نا معتبر است";
                    break;
                case Enum_Message.SUCCESSFULL_CONTACTUS:
                    if (lang == Enum_Lang.EN)
                        msg = "Your message has been successfully registered";
                    else if (lang == Enum_Lang.AR)
                        msg = "تم تسجيل رسالتك بنجاح";
                    else if (lang == Enum_Lang.RU)
                        msg = "Успешно отправлено участникам рассылки";
                    else if (lang == Enum_Lang.ZH)
                        msg = " 成功註冊";
                    else
                        msg = "پیام شما با موفقیت ثبت شد";
                    break;          
                case Enum_Message.REQUIRED_CONTACTUS_VALUE:
                    if (lang == Enum_Lang.EN)
                        msg = "Complete the input information";
                    else if (lang == Enum_Lang.RU)
                        msg = "Успешно отправлено участникам рассылки";
                    else
                        msg = "لطفا اطلاعات را کامل وارد نمایید";
                    break;
                case Enum_Message.REQUIRED_CompetitiveFeature_NAME:
                    
                    msg = "ورود عنوان ویژگی های رقابتی اجباری است";
                    break;
                default:
                    break;
            }
            return msg;
        }
    }
}
