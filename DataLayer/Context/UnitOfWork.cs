using DataLayer.Entities;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Entities
{
    public class UnitOfWork : IDisposable
    {
        DatabaseEntities db = new DatabaseEntities();
        private Entity_ProductPiceHistory _productPiceHistory;

        private Entity_Account _account;
        private Entity_Review _review;
        private Entity_ShippingCity _shippingCity;

        private Entity_File _file;

        private Entity_AccountCredit _accountcredit;
        private Entity_AccountAddress _accountaddress;
        private Entity_AccountLog _accountlog;
        private Entity_AccountLogVisit _accountlogvisit;
        private Entity_AccountPasswordForget _accountpasswordforget;
        private Entity_AccountReagent _accountreagent; private Entity_AccountBasket _accountbasket;
        private Entity_AccountOrder _order;
        private Entity_AccountOrderProduct _orderproduct;
        private Entity_AccountOrderComment _ordercomment;



        private Entity_Category _category;
        private Entity_Comment _comment;
        private Entity_Gallery _gallery;
        private Entity_GalleryLanguage _gallerylanguage;

        private Entity_GalleryPicture _gallerypicture;
        private Entity_Notification _notification;
        private Entity_Permission _permission;
        private Entity_Picture _picture;
        private Entity_Post _post;
        private Entity_Banner _banner;
        private Entity_BannerLanguage _bannerlanguage;
        private Entity_PostLike _postlike;
        private Entity_Setting _setting;
        private Entity_Menu _menu;
        private Entity_MenuLanguage _menulanguage;
        private Entity_CodeGroup _codegroups;
        private Entity_Code _codes;
        private Entity_RecoverPassword _recoverpassword;
        private Entity_Survey _survey;
        private Entity_SurveyLanguage _surveylanguage;
        private Entity_SurveyQuestion _surveyquestion;
        private Entity_SurveyQuestionLanguage _surveyquestionlanguage;
        private Entity_SurveyQuestionItem _surveyquestionitem;
        private Entity_SurveyQuestionItemLanguage _surveyquestionitemlanguage;
        private Entity_SurveyAnswer _surveyanswer;
        private Entity_SurveyAnswerDetailItem _surveyAnswerDetailItem;


        private Entity_Email _email;
        private Entity_EmailHost _emailhost;
        private Entity_EmailInbox _emailinbox;
        private Entity_EmailAddress _emailaddress;
        private Entity_EmailAttachment _emailattachement;

        private Entity_SyncLog _synclog;
        private Entity_Language _language;
        private Entity_Module _module;
        private Entity_SetupLevel _setuplevel;

        private Entity_SmsProvider _smsprovider;
        private Entity_SmsSetting _smssetting;
        private Entity_SmsNumber _smsnumber;
        private Entity_SmsType _smstype;
        private Entity_Sms _sms;

        private Entity_Event _event;
        private Entity_Website _website;
        private Entity_AlexaRank _alexarank;

        private Entity_Mall _mall;

        private Entity_Product _product;
        private Entity_OnlineOrder _OnlineOrder;


        private Entity_ProductVisit _ProductVisit;
        private Entity_ProductType _producttype;
        private Entity_ProductLanguage _productlanguage;
        private Entity_PostLanguage _PostLanguage;
        private Entity_ProductTypeLanguage _producttypelanguage;
        private Entity_ProductCategoryLanguage _productcategorylanguage;
        private Entity_ProductSubCategoryLanguage _productsubcategorylanguage;
        private Entity_CategoryLanguage _CategoryLanguage;
        private Entity_SliderLanguage _SliderLanguage;
        private Entity_ProductCategory _productcategory;
        private Entity_ProductSubCategory _productsubcategory;
        private Entity_ProductComment _productcomment;
        private Entity_StoreComment _storecomment;
        private Entity_ProductQuantity _productquantity;
        private Entity_ProductCustomField _productcustomfield;
        private Entity_ProductCustomFieldLanguage _productcustomfieldlanguage;
        private Entity_ProductCustomItem _productcustomitem;
        private Entity_ProductCustomItemLanguage _productcustomitemlanguage;
        private Entity_ProductCustomValue _productcustomvalue;
        private Entity_ProductColor _productcolor;
        private Entity_ProductSize _productsize;
        private Entity_ProductBrand _productbrand;
        private Entity_ProductCategoryCompany _productCategoryCompany;
        private Entity_ProductCompany _productcompany;
        private Entity_ProductBrandUser _productbranduser;
        private Entity_ProductNotify _productnotify;
        private Entity_ProductBarcode _productbarcode;
        private Entity_ProductTypeBrand _producttypebrand;
        private Entity_ProductOption _productoption;
        private Entity_ProductOptionItem _productoptionitem;

        private Entity_Color _color;
        private Entity_ColorLanguage _colorlanguage;
        private Entity_Size _size;
        private Entity_SizeLanguage _sizelanguage;
        private Entity_Template _template;
        private Entity_ProductPicture _productpicture;
        private Entity_Slider _slider;
        private Entity_Country _country;
        private Entity_State _state;
        private Entity_City _city;
        private Entity_ProductLike _productlike;
        private Entity_ShopFollow _shopfollow;
        private Entity_Package _package;
        private Entity_Rebate _rebate;

        private Entity_NewsLetter _newsletter;
        private Entity_NewsletterGroup _newslettergroup;
        private Entity_NewsletterItem _newsletteritem;

        private Entity_UserRole _userrole;
        private Entity_SiteUser _siteuser;
        private Entity_UserGroup _usergroup;
        private Entity_UserGroupPermission _usergrouppermission;
        private Entity_SiteUserUserGroup _siteuserusergroup;

        private Entity_Agreement _agreement;
        private Entity_AgreementPlatform _agreementplatform;
        private Entity_Discount _discount;
        private Entity_DiscountGroup _discountgroup;
        private Entity_PaymentType _paymenttype;

        private Entity_Unit _unit;
        private Entity_Shop _shop;
        private Entity_ShopUser _shopuser;
        private Entity_ShopChat _shopchat;
        private Entity_ShopContact _shopcontact;
        private Entity_ShopAddress _shopaddress;
        private Entity_ShopPicture _shoppicture;
        private Entity_ShopDocument _shopdocument;
        private Entity_ShopReportBlock _shopreportblock;
        private Entity_ShopPaymentType _shoppaymenttype;
        private Entity_ShopProductType _shopproducttype;
        private Entity_ShopWebsiteSetting _shopwebsitesetting;

        private Entity_Store _store;
        private Entity_StoreProduct _storeproduct;
        private Entity_StoreProductQuantity _storeproductquantity;


        private Entity_ShopReseller _shopreseller;
        private Entity_ShopResellerProduct _shopresellerproduct;
        private Entity_ShopResellerGallery _shopresellergallery;
        private Entity_ShopResellerCollection _shopresellercollection;
        private Entity_ShopResellerCollectionProduct _shopresellercollectionproduct;
        private Entity_ShopResellerStory _shopresellerstory;

        private Entity_SendType _sendtype;
        private Entity_Merchant _merchant;

        private Entity_Currency _Currency;

        private Entity_Bank _bank;
        private Entity_Payment _payment;
        private Entity_PaymentProductOrder _paymentproductorder;
        private Entity_PaymentSubject _paymentsubject;

        private Entity_NotificationProject _notificationproject;
        private Entity_NotificationApp _notificationapp;

        private Entity_WebsiteForm _websiteform;
        private Entity_WebsiteFormField _websiteformfield;
        private Entity_WebsiteView _websiteview;
        private Entity_WebsiteDomain _websitedomain;
        private Entity_WebsiteDocument _websitedocument;
        private Entity_WebsiteLanguage _websitelanguage;
        private Entity_WebsiteContactForm _websitecontactform;
        private Entity_WebsiteFormAccount _websiteformaccount;
        private Entity_WebsiteFormValue _websiteformvalue;

        private Entity_TelegramBot _telegrambot;
        private Entity_TelegramAccount _telegramaccount;
        private Entity_TelegramCommand _telegramcommand;
        private Entity_TelegramKeyBoard _telegramkeyboard;
        private Entity_TelegramKeyBoardItem _telegramkeyboarditem;

        private Entity_Dashboard _dashboard;
        private Entity_UserGroupDashboard _usergroupdashboard;

        private Entity_Shipping _shipping;
        private Entity_Clearing _clearing;

        private Entity_PaymentWebsite _paymentwebsite;
        private Entity_StaticPage _staticpage;
        private Entity_WebsiteDetail _websitedetail;
        private Entity_Tag _tag;
        private Entity_TagPost _tagPost;
        private Entity_TagSubCategory _tagSubCategory;
        private Entity_ContactUs _contactUs;
        private Entity_ContactUsField _contactUsField;
        private Entity_PriceRange _priceRange;
        private Entity_CompetitiveFeature _competitiveFeature;

        private Entity_StoreCompetitiveFeature _storeCompetitiveFeature;
        private Entity_Cart _cart;
        private Entity_CartStore _cartstore;
        private Entity_CartStoreItem _cartstoreitem;
        private Entity_Delivery _delivery;
        private Entity_DeliveryType _deliverytype;
        private Entity_ProductProductOptionItem _productProductOptionItem;
        private Entity_CacheSetting _cachesetting;
        private Entity_ProductExteraType _productExteraType;
        private Entity_ProductExteraCategory _productExteraCategory;
        private Entity_ProductExteraSubCategory _productExteraSubCategory;
        private Entity_ProductBrandLanguage _productBrandLanguage;
        private Entity_ProductOptionLanguage _productOptionLanguage;
        private Entity_PostQuestion _PostQuestion;
        private Entity_SurveyAnswerDetail _SurveyAnswerDetail;
        private Entity_SmartOfferQuestion _SmartOfferQuestion;
        private Entity_SmartOfferAnswer _SmartOfferAnswer;
        private Entity_Exame _Exame;
        private Entity_ExameAccountAnswer _ExameAccountAnswer;
        private Entity_ExameAccountAnswerDetails _ExameAccountAnswerDetails;
        private Entity_ExameAnswerItem _ExameAnswerItem;
        private Entity_ExameCourse _ExameCourse;
        private Entity_ExameQuestion _ExameQuestion;
        private Entity_ExameReportByCourse _ExameReportByCourse;
        private Entity_ExameCourseCategory _ExameCourseCategory;
        private Entity_CoursePayment _CoursePayment;
        private Entity_QuizPayment _QuizPayment;
        private Entity_BookPayment _BookPayment;
        private Entity_Clubs _Clubs;
        private Entity_Students _Students;
        private Entity_PaymentCertificate _PaymentCertificate;
        private Entity_PaymentExame _PaymentExame;
        private Entity_PaymentExameCourseId _PaymentExameCourseId;
        private Entity_QuestionRegister _QuestionRegister;
        private Entity_QuestionRegisterItem _QuestionRegisterItem;
        private Entity_QuestionRegisterProduct _QuestionRegisterProduct;
        private Entity_QuestionRegisterValue _QuestionRegisterValue;
        public Entity_QuestionRegisterValue QuestionRegisterValue { get { if (_QuestionRegisterValue == null) { _QuestionRegisterValue = new Entity_QuestionRegisterValue(db); } return _QuestionRegisterValue; } }

        public Entity_QuestionRegisterProduct QuestionRegisterProduct { get { if (_QuestionRegisterProduct == null) { _QuestionRegisterProduct = new Entity_QuestionRegisterProduct(db); } return _QuestionRegisterProduct; } }

        public Entity_QuestionRegisterItem QuestionRegisterItem { get { if (_QuestionRegisterItem == null) { _QuestionRegisterItem = new Entity_QuestionRegisterItem(db); } return _QuestionRegisterItem; } }

        public Entity_QuestionRegister QuestionRegister { get { if (_QuestionRegister == null) { _QuestionRegister = new Entity_QuestionRegister(db); } return _QuestionRegister; } }

        public Entity_PaymentExameCourseId PaymentExameCourseId { get { if (_PaymentExameCourseId == null) { _PaymentExameCourseId = new Entity_PaymentExameCourseId(db); } return _PaymentExameCourseId; } }

        public Entity_PaymentExame PaymentExame { get { if (_PaymentExame == null) { _PaymentExame = new Entity_PaymentExame(db); } return _PaymentExame; } }

        public Entity_PaymentCertificate PaymentCertificate { get { if (_PaymentCertificate == null) { _PaymentCertificate = new Entity_PaymentCertificate(db); } return _PaymentCertificate; } }

        public Entity_Students Students { get { if (_Students == null) { _Students = new Entity_Students(db); } return _Students; } }

        public Entity_Clubs Clubs { get { if (_Clubs == null) { _Clubs = new Entity_Clubs(db); } return _Clubs; } }

        public Entity_BookPayment BookPayment { get { if (_BookPayment == null) { _BookPayment = new Entity_BookPayment(db); } return _BookPayment; } }

        public Entity_QuizPayment QuizPayment { get { if (_QuizPayment == null) { _QuizPayment = new Entity_QuizPayment(db); } return _QuizPayment; } }

        public Entity_CoursePayment CoursePayment { get { if (_CoursePayment == null) { _CoursePayment = new Entity_CoursePayment(db); } return _CoursePayment; } }

        public Entity_ExameCourseCategory ExameCourseCategory { get { if (_ExameCourseCategory == null) { _ExameCourseCategory = new Entity_ExameCourseCategory(db); } return _ExameCourseCategory; } }

        public Entity_Exame Exame { get { if (_Exame == null) { _Exame = new Entity_Exame(db); } return _Exame; } }
        public Entity_ExameAccountAnswer ExameAccountAnswer { get { if (_ExameAccountAnswer == null) { _ExameAccountAnswer = new Entity_ExameAccountAnswer(db); } return _ExameAccountAnswer; } }
        public Entity_ExameAccountAnswerDetails ExameAccountAnswerDetails { get { if (_ExameAccountAnswerDetails == null) { _ExameAccountAnswerDetails = new Entity_ExameAccountAnswerDetails(db); } return _ExameAccountAnswerDetails; } }
        public Entity_ExameAnswerItem ExameAnswerItem { get { if (_ExameAnswerItem == null) { _ExameAnswerItem = new Entity_ExameAnswerItem(db); } return _ExameAnswerItem; } }
        public Entity_ExameCourse ExameCourse { get { if (_ExameCourse == null) { _ExameCourse = new Entity_ExameCourse(db); } return _ExameCourse; } }
        public Entity_ExameQuestion ExameQuestion { get { if (_ExameQuestion == null) { _ExameQuestion = new Entity_ExameQuestion(db); } return _ExameQuestion; } }
        public Entity_ExameReportByCourse ExameReportByCourse { get { if (_ExameReportByCourse == null) { _ExameReportByCourse = new Entity_ExameReportByCourse(db); } return _ExameReportByCourse; } }

        public Entity_SmartOfferAnswer SmartOfferAnswer { get { if (_SmartOfferAnswer == null) { _SmartOfferAnswer = new Entity_SmartOfferAnswer(db); } return _SmartOfferAnswer; } }

        public Entity_SmartOfferQuestion SmartOfferQuestion { get { if (_SmartOfferQuestion == null) { _SmartOfferQuestion = new Entity_SmartOfferQuestion(db); } return _SmartOfferQuestion; } }

        public Entity_SurveyAnswerDetail SurveyAnswerDetail { get { if (_SurveyAnswerDetail == null) { _SurveyAnswerDetail = new Entity_SurveyAnswerDetail(db); } return _SurveyAnswerDetail; } }

        public Entity_ProductOptionLanguage ProductOptionLanguage { get { if (_productOptionLanguage == null) { _productOptionLanguage = new Entity_ProductOptionLanguage(db); } return _productOptionLanguage; } }
        private Entity_ProductOptionItemLanguage _productOptionItemLanguage;
        public Entity_ProductOptionItemLanguage ProductOptionItemLanguage { get { if (_productOptionItemLanguage == null) { _productOptionItemLanguage = new Entity_ProductOptionItemLanguage(db); } return _productOptionItemLanguage; } }

        public Entity_ProductBrandLanguage ProductBrandLanguage { get { if (_productBrandLanguage == null) { _productBrandLanguage = new Entity_ProductBrandLanguage(db); } return _productBrandLanguage; } }

        public Entity_File File { get { if (_file == null) { _file = new Entity_File(db); } return _file; } }

        public Entity_Account Account { get { if (_account == null) { _account = new Entity_Account(db); } return _account; } }
        public Entity_Review Review { get { if (_review == null) { _review = new Entity_Review(db); } return _review; } }
        public Entity_ShippingCity ShippingCity { get { if (_shippingCity == null) { _shippingCity = new Entity_ShippingCity(db); } return _shippingCity; } }

        public Entity_ProductPiceHistory ProductPiceHistory { get { if (_productPiceHistory == null) { _productPiceHistory = new Entity_ProductPiceHistory(db); } return _productPiceHistory; } }

        public Entity_AccountAddress AccountAddress { get { if (_accountaddress == null) { _accountaddress = new Entity_AccountAddress(db); } return _accountaddress; } }
        public Entity_AccountCredit AccountCredit { get { if (_accountcredit == null) { _accountcredit = new Entity_AccountCredit(db); } return _accountcredit; } }
        public Entity_AccountLog AccountLog { get { if (_accountlog == null) { _accountlog = new Entity_AccountLog(db); } return _accountlog; } }
        public Entity_AccountLogVisit AccountLogVisit { get { if (_accountlogvisit == null) { _accountlogvisit = new Entity_AccountLogVisit(db); } return _accountlogvisit; } }
        public Entity_AccountPasswordForget AccountPasswordForget { get { if (_accountpasswordforget == null) { _accountpasswordforget = new Entity_AccountPasswordForget(db); } return _accountpasswordforget; } }
        public Entity_AccountReagent AccountReagent { get { if (_accountreagent == null) { _accountreagent = new Entity_AccountReagent(db); } return _accountreagent; } }
        public Entity_AccountOrderComment AccountOrderComment { get { if (_ordercomment == null) { _ordercomment = new Entity_AccountOrderComment(db); } return _ordercomment; } }
        public Entity_Category Category { get { if (_category == null) { _category = new Entity_Category(db); } return _category; } }
        public Entity_Comment Comment { get { if (_comment == null) { _comment = new Entity_Comment(db); } return _comment; } }
        public Entity_Gallery Gallery { get { if (_gallery == null) { _gallery = new Entity_Gallery(db); } return _gallery; } }
        public Entity_GalleryLanguage GalleryLanguage { get { if (_gallerylanguage == null) { _gallerylanguage = new Entity_GalleryLanguage(db); } return _gallerylanguage; } }

        public Entity_GalleryPicture GalleryPicture { get { if (_gallerypicture == null) { _gallerypicture = new Entity_GalleryPicture(db); } return _gallerypicture; } }
        public Entity_Notification Notification { get { if (_notification == null) { _notification = new Entity_Notification(db); } return _notification; } }
        public Entity_Permission Permission { get { if (_permission == null) { _permission = new Entity_Permission(db); } return _permission; } }
        public Entity_Picture Picture { get { if (_picture == null) { _picture = new Entity_Picture(db); } return _picture; } }
        public Entity_Post Post { get { if (_post == null) { _post = new Entity_Post(db); } return _post; } }
        public Entity_Banner Banner { get { if (_banner == null) { _banner = new Entity_Banner(db); } return _banner; } }
        public Entity_BannerLanguage BannerLanguage { get { if (_bannerlanguage == null) { _bannerlanguage = new Entity_BannerLanguage(db); } return _bannerlanguage; } }
        public Entity_Rebate Rebate { get { if (_rebate == null) { _rebate = new Entity_Rebate(db); } return _rebate; } }
        public Entity_PostLike PostLike { get { if (_postlike == null) { _postlike = new Entity_PostLike(db); } return _postlike; } }
        public Entity_Setting Setting { get { if (_setting == null) { _setting = new Entity_Setting(db); } return _setting; } }
        public Entity_SiteUser SiteUser { get { if (_siteuser == null) { _siteuser = new Entity_SiteUser(db); } return _siteuser; } }
        public Entity_UserGroup UserGroup { get { if (_usergroup == null) { _usergroup = new Entity_UserGroup(db); } return _usergroup; } }
        public Entity_UserGroupPermission UserGroupPermission { get { if (_usergrouppermission == null) { _usergrouppermission = new Entity_UserGroupPermission(db); } return _usergrouppermission; } }
        public Entity_SiteUserUserGroup SiteUserUserGroup { get { if (_siteuserusergroup == null) { _siteuserusergroup = new Entity_SiteUserUserGroup(db); } return _siteuserusergroup; } }
        public Entity_Menu Menu { get { if (_menu == null) { _menu = new Entity_Menu(db); } return _menu; } }
        public Entity_MenuLanguage MenuLanguage { get { if (_menulanguage == null) { _menulanguage = new Entity_MenuLanguage(db); } return _menulanguage; } }
        public Entity_CodeGroup CodeGroup { get { if (_codegroups == null) { _codegroups = new Entity_CodeGroup(db); } return _codegroups; } }
        public Entity_Code Code { get { if (_codes == null) { _codes = new Entity_Code(db); } return _codes; } }
        public Entity_RecoverPassword RecoverPassword { get { if (_recoverpassword == null) { _recoverpassword = new Entity_RecoverPassword(db); } return _recoverpassword; } }
        public Entity_EmailAddress EmailAddress { get { if (_emailaddress == null) { _emailaddress = new Entity_EmailAddress(db); } return _emailaddress; } }
        public Entity_EmailInbox EmailInbox { get { if (_emailinbox == null) { _emailinbox = new Entity_EmailInbox(db); } return _emailinbox; } }
        public Entity_EmailAttachment EmailAttachment { get { if (_emailattachement == null) { _emailattachement = new Entity_EmailAttachment(db); } return _emailattachement; } }
        public Entity_EmailHost EmailHost { get { if (_emailhost == null) { _emailhost = new Entity_EmailHost(db); } return _emailhost; } }
        public Entity_Email Email { get { if (_email == null) { _email = new Entity_Email(db); } return _email; } }
        public Entity_SyncLog SyncLog { get { if (_synclog == null) { _synclog = new Entity_SyncLog(db); } return _synclog; } }
        public Entity_WebsiteDomain WebsiteDomain { get { if (_websitedomain == null) { _websitedomain = new Entity_WebsiteDomain(db); } return _websitedomain; } }
        public Entity_WebsiteLanguage WebsiteLanguage { get { if (_websitelanguage == null) { _websitelanguage = new Entity_WebsiteLanguage(db); } return _websitelanguage; } }
        public Entity_Language Language { get { if (_language == null) { _language = new Entity_Language(db); } return _language; } }
        public Entity_Module Module { get { if (_module == null) { _module = new Entity_Module(db); } return _module; } }
        public Entity_SetupLevel SetupLevel { get { if (_setuplevel == null) { _setuplevel = new Entity_SetupLevel(db); } return _setuplevel; } }

        public Entity_Sms Sms { get { if (_sms == null) { _sms = new Entity_Sms(db); } return _sms; } }
        public Entity_SmsNumber SmsNumber { get { if (_smsnumber == null) { _smsnumber = new Entity_SmsNumber(db); } return _smsnumber; } }
        public Entity_SmsProvider SmsProvider { get { if (_smsprovider == null) { _smsprovider = new Entity_SmsProvider(db); } return _smsprovider; } }
        public Entity_SmsSetting SmsSetting { get { if (_smssetting == null) { _smssetting = new Entity_SmsSetting(db); } return _smssetting; } }
        public Entity_SmsType SmsType { get { if (_smstype == null) { _smstype = new Entity_SmsType(db); } return _smstype; } }
        public Entity_AlexaRank AlexaRank { get { if (_alexarank == null) { _alexarank = new Entity_AlexaRank(db); } return _alexarank; } }

        public Entity_Event Event { get { if (_event == null) { _event = new Entity_Event(db); } return _event; } }
        public Entity_Website Website { get { if (_website == null) { _website = new Entity_Website(db); } return _website; } }
        public Entity_Mall Mall { get { if (_mall == null) { _mall = new Entity_Mall(db); } return _mall; } }
        public Entity_Unit Unit { get { if (_unit == null) { _unit = new Entity_Unit(db); } return _unit; } }
        public Entity_Shop Shop { get { if (_shop == null) { _shop = new Entity_Shop(db); } return _shop; } }
        public Entity_ShopUser ShopUser { get { if (_shopuser == null) { _shopuser = new Entity_ShopUser(db); } return _shopuser; } }
        public Entity_ShopAddress ShopAddress { get { if (_shopaddress == null) { _shopaddress = new Entity_ShopAddress(db); } return _shopaddress; } }
        public Entity_Product Product { get { if (_product == null) { _product = new Entity_Product(db); } return _product; } }

        public Entity_ProductVisit ProductVisit { get { if (_ProductVisit == null) { _ProductVisit = new Entity_ProductVisit(db); } return _ProductVisit; } }

        public Entity_ProductComment ProductComment { get { if (_productcomment == null) { _productcomment = new Entity_ProductComment(db); } return _productcomment; } }
        public Entity_StoreComment StoreComment { get { if (_storecomment == null) { _storecomment = new Entity_StoreComment(db); } return _storecomment; } }
        public Entity_ProductQuantity ProductQuantity { get { if (_productquantity == null) { _productquantity = new Entity_ProductQuantity(db); } return _productquantity; } }
        public Entity_ProductType ProductType { get { if (_producttype == null) { _producttype = new Entity_ProductType(db); } return _producttype; } }
        public Entity_OnlineOrder OnlineOrder { get { if (_OnlineOrder == null) { _OnlineOrder = new Entity_OnlineOrder(db); } return _OnlineOrder; } }
        public Entity_ProductLanguage ProductLanguage { get { if (_productlanguage == null) { _productlanguage = new Entity_ProductLanguage(db); } return _productlanguage; } }
        public Entity_PostLanguage PostLanguage { get { if (_PostLanguage == null) { _PostLanguage = new Entity_PostLanguage(db); } return _PostLanguage; } }

        public Entity_ProductTypeLanguage ProductTypeLanguage { get { if (_producttypelanguage == null) { _producttypelanguage = new Entity_ProductTypeLanguage(db); } return _producttypelanguage; } }
        public Entity_ProductCategoryLanguage ProductCategoryLanguage { get { if (_productcategorylanguage == null) { _productcategorylanguage = new Entity_ProductCategoryLanguage(db); } return _productcategorylanguage; } }


        public Entity_CategoryLanguage CategoryLanguage { get { if (_CategoryLanguage == null) { _CategoryLanguage = new Entity_CategoryLanguage(db); } return _CategoryLanguage; } }

        public Entity_SliderLanguage SliderLanguage { get { if (_SliderLanguage == null) { _SliderLanguage = new Entity_SliderLanguage(db); } return _SliderLanguage; } }


        public Entity_ProductSubCategoryLanguage ProductSubCategoryLanguage { get { if (_productsubcategorylanguage == null) { _productsubcategorylanguage = new Entity_ProductSubCategoryLanguage(db); } return _productsubcategorylanguage; } }
        public Entity_ProductCategory ProductCategory { get { if (_productcategory == null) { _productcategory = new Entity_ProductCategory(db); } return _productcategory; } }
        public Entity_ProductSubCategory ProductSubCategory { get { if (_productsubcategory == null) { _productsubcategory = new Entity_ProductSubCategory(db); } return _productsubcategory; } }
        public Entity_ProductTypeBrand ProductTypeBrand { get { if (_producttypebrand == null) { _producttypebrand = new Entity_ProductTypeBrand(db); } return _producttypebrand; } }
        public Entity_ShopProductType ShopProductType { get { if (_shopproducttype == null) { _shopproducttype = new Entity_ShopProductType(db); } return _shopproducttype; } }
        public Entity_SendType SendType { get { if (_sendtype == null) { _sendtype = new Entity_SendType(db); } return _sendtype; } }
        public Entity_ProductCustomField ProductCustomField { get { if (_productcustomfield == null) { _productcustomfield = new Entity_ProductCustomField(db); } return _productcustomfield; } }
        public Entity_ProductCustomFieldLanguage ProductCustomFieldLanguage { get { if (_productcustomfieldlanguage == null) { _productcustomfieldlanguage = new Entity_ProductCustomFieldLanguage(db); } return _productcustomfieldlanguage; } }
        public Entity_ProductCustomItem ProductCustomItem { get { if (_productcustomitem == null) { _productcustomitem = new Entity_ProductCustomItem(db); } return _productcustomitem; } }
        public Entity_ProductCustomItemLanguage ProductCustomItemLanguage { get { if (_productcustomitemlanguage == null) { _productcustomitemlanguage = new Entity_ProductCustomItemLanguage(db); } return _productcustomitemlanguage; } }
        public Entity_ProductCustomValue ProductCustomValue { get { if (_productcustomvalue == null) { _productcustomvalue = new Entity_ProductCustomValue(db); } return _productcustomvalue; } }
        public Entity_ProductBrand ProductBrand { get { if (_productbrand == null) { _productbrand = new Entity_ProductBrand(db); } return _productbrand; } }
        public Entity_ProductCategoryCompany ProductCategoryCompany { get { if (_productCategoryCompany == null) { _productCategoryCompany = new Entity_ProductCategoryCompany(db); } return _productCategoryCompany; } }
        public Entity_ProductCompany ProductCompany { get { if (_productcompany == null) { _productcompany = new Entity_ProductCompany(db); } return _productcompany; } }
        public Entity_Package Package { get { if (_package == null) { _package = new Entity_Package(db); } return _package; } }
        public Entity_ProductBrandUser ProductBrandUser { get { if (_productbranduser == null) { _productbranduser = new Entity_ProductBrandUser(db); } return _productbranduser; } }
        public Entity_ProductColor ProductColor { get { if (_productcolor == null) { _productcolor = new Entity_ProductColor(db); } return _productcolor; } }
        public Entity_ProductSize ProductSize { get { if (_productsize == null) { _productsize = new Entity_ProductSize(db); } return _productsize; } }
        public Entity_ProductNotify ProductNotify { get { if (_productnotify == null) { _productnotify = new Entity_ProductNotify(db); } return _productnotify; } }
        public Entity_ProductBarcode ProductBarcode { get { if (_productbarcode == null) { _productbarcode = new Entity_ProductBarcode(db); } return _productbarcode; } }

        public Entity_ProductOption ProductOption { get { if (_productoption == null) { _productoption = new Entity_ProductOption(db); } return _productoption; } }
        public Entity_ProductOptionItem ProductOptionItem { get { if (_productoptionitem == null) { _productoptionitem = new Entity_ProductOptionItem(db); } return _productoptionitem; } }

        public Entity_Color Color { get { if (_color == null) { _color = new Entity_Color(db); } return _color; } }
        public Entity_ColorLanguage ColorLanguage { get { if (_colorlanguage == null) { _colorlanguage = new Entity_ColorLanguage(db); } return _colorlanguage; } }
        public Entity_Size Size { get { if (_size == null) { _size = new Entity_Size(db); } return _size; } }
        public Entity_SizeLanguage SizeLanguage { get { if (_sizelanguage == null) { _sizelanguage = new Entity_SizeLanguage(db); } return _sizelanguage; } }
        public Entity_Template Template { get { if (_template == null) { _template = new Entity_Template(db); } return _template; } }
        public Entity_ProductPicture ProductPicture { get { if (_productpicture == null) { _productpicture = new Entity_ProductPicture(db); } return _productpicture; } }
        public Entity_Slider Slider { get { if (_slider == null) { _slider = new Entity_Slider(db); } return _slider; } }
        public Entity_Country Country { get { if (_country == null) { _country = new Entity_Country(db); } return _country; } }
        public Entity_State State { get { if (_state == null) { _state = new Entity_State(db); } return _state; } }
        public Entity_City City { get { if (_city == null) { _city = new Entity_City(db); } return _city; } }
        public Entity_ProductLike ProductLike { get { if (_productlike == null) { _productlike = new Entity_ProductLike(db); } return _productlike; } }
        public Entity_ShopFollow ShopFollow { get { if (_shopfollow == null) { _shopfollow = new Entity_ShopFollow(db); } return _shopfollow; } }
        public Entity_ShopPicture ShopPicture { get { if (_shoppicture == null) { _shoppicture = new Entity_ShopPicture(db); } return _shoppicture; } }
        public Entity_ShopDocument ShopDocument { get { if (_shopdocument == null) { _shopdocument = new Entity_ShopDocument(db); } return _shopdocument; } }
        public Entity_NewsLetter NewsLetter { get { if (_newsletter == null) { _newsletter = new Entity_NewsLetter(db); } return _newsletter; } }
        public Entity_NewsletterGroup NewsletterGroup { get { if (_newslettergroup == null) { _newslettergroup = new Entity_NewsletterGroup(db); } return _newslettergroup; } }
        public Entity_NewsletterItem NewsLetterItem { get { if (_newsletteritem == null) { _newsletteritem = new Entity_NewsletterItem(db); } return _newsletteritem; } }
        public Entity_UserRole UserRole { get { if (_userrole == null) { _userrole = new Entity_UserRole(db); } return _userrole; } }
        public Entity_ShopContact ShopContact { get { if (_shopcontact == null) { _shopcontact = new Entity_ShopContact(db); } return _shopcontact; } }
        public Entity_ShopChat ShopChat { get { if (_shopchat == null) { _shopchat = new Entity_ShopChat(db); } return _shopchat; } }
        public Entity_ShopReportBlock ShopReportBlock { get { if (_shopreportblock == null) { _shopreportblock = new Entity_ShopReportBlock(db); } return _shopreportblock; } }
        public Entity_Agreement Agreement { get { if (_agreement == null) { _agreement = new Entity_Agreement(db); } return _agreement; } }
        public Entity_AgreementPlatform AgreementPlatform { get { if (_agreementplatform == null) { _agreementplatform = new Entity_AgreementPlatform(db); } return _agreementplatform; } }
        public Entity_DiscountGroup DiscountGroup { get { if (_discountgroup == null) { _discountgroup = new Entity_DiscountGroup(db); } return _discountgroup; } }
        public Entity_Discount Discount { get { if (_discount == null) { _discount = new Entity_Discount(db); } return _discount; } }
        public Entity_PaymentType PaymentType { get { if (_paymenttype == null) { _paymenttype = new Entity_PaymentType(db); } return _paymenttype; } }
        public Entity_ShopPaymentType ShopPaymentType { get { if (_shoppaymenttype == null) { _shoppaymenttype = new Entity_ShopPaymentType(db); } return _shoppaymenttype; } }
        public Entity_ShopWebsiteSetting ShopWebsiteSetting { get { if (_shopwebsitesetting == null) { _shopwebsitesetting = new Entity_ShopWebsiteSetting(db); } return _shopwebsitesetting; } }
        public Entity_ShopReseller ShopReseller { get { if (_shopreseller == null) { _shopreseller = new Entity_ShopReseller(db); } return _shopreseller; } }
        public Entity_ShopResellerProduct ShopResellerProduct { get { if (_shopresellerproduct == null) { _shopresellerproduct = new Entity_ShopResellerProduct(db); } return _shopresellerproduct; } }
        public Entity_ShopResellerGallery ShopResellerGallery { get { if (_shopresellergallery == null) { _shopresellergallery = new Entity_ShopResellerGallery(db); } return _shopresellergallery; } }
        public Entity_ShopResellerStory ShopResellerStory { get { if (_shopresellerstory == null) { _shopresellerstory = new Entity_ShopResellerStory(db); } return _shopresellerstory; } }
        public Entity_ShopResellerCollection ShopResellerCollection { get { if (_shopresellercollection == null) { _shopresellercollection = new Entity_ShopResellerCollection(db); } return _shopresellercollection; } }
        public Entity_ShopResellerCollectionProduct ShopResellerCollectionProduct { get { if (_shopresellercollectionproduct == null) { _shopresellercollectionproduct = new Entity_ShopResellerCollectionProduct(db); } return _shopresellercollectionproduct; } }

        public Entity_Store Store { get { if (_store == null) { _store = new Entity_Store(db); } return _store; } }
        public Entity_StoreProduct StoreProduct { get { if (_storeproduct == null) { _storeproduct = new Entity_StoreProduct(db); } return _storeproduct; } }
        public Entity_StoreProductQuantity StoreProductQuantity { get { if (_storeproductquantity == null) { _storeproductquantity = new Entity_StoreProductQuantity(db); } return _storeproductquantity; } }


        public Entity_AccountBasket AccountBasket { get { if (_accountbasket == null) { _accountbasket = new Entity_AccountBasket(db); } return _accountbasket; } }
        public Entity_AccountOrder AccountOrder { get { if (_order == null) { _order = new Entity_AccountOrder(db); } return _order; } }
        public Entity_AccountOrderProduct AccountOrderProduct { get { if (_orderproduct == null) { _orderproduct = new Entity_AccountOrderProduct(db); } return _orderproduct; } }
        public Entity_Merchant Merchant { get { if (_merchant == null) { _merchant = new Entity_Merchant(db); } return _merchant; } }
        public Entity_Currency Currency { get { if (_Currency == null) { _Currency = new Entity_Currency(db); } return _Currency; } }
        public Entity_Bank Bank { get { if (_bank == null) { _bank = new Entity_Bank(db); } return _bank; } }
        public Entity_Payment Payment { get { if (_payment == null) { _payment = new Entity_Payment(db); } return _payment; } }
        public Entity_PaymentProductOrder PaymentProductOrder { get { if (_paymentproductorder == null) { _paymentproductorder = new Entity_PaymentProductOrder(db); } return _paymentproductorder; } }
        public Entity_PaymentSubject PaymentSubject { get { if (_paymentsubject == null) { _paymentsubject = new Entity_PaymentSubject(db); } return _paymentsubject; } }
        public Entity_NotificationProject NotificationProject { get { if (_notificationproject == null) { _notificationproject = new Entity_NotificationProject(db); } return _notificationproject; } }
        public Entity_NotificationApp NotificationApp { get { if (_notificationapp == null) { _notificationapp = new Entity_NotificationApp(db); } return _notificationapp; } }
        public Entity_WebsiteView WebsiteView { get { if (_websiteview == null) { _websiteview = new Entity_WebsiteView(db); } return _websiteview; } }
        public Entity_WebsiteDocument WebsiteDocument { get { if (_websitedocument == null) { _websitedocument = new Entity_WebsiteDocument(db); } return _websitedocument; } }
        public Entity_WebsiteContactForm WebsiteContactForm { get { if (_websitecontactform == null) { _websitecontactform = new Entity_WebsiteContactForm(db); } return _websitecontactform; } }
        public Entity_WebsiteFormAccount WebsiteFormAccount { get { if (_websiteformaccount == null) { _websiteformaccount = new Entity_WebsiteFormAccount(db); } return _websiteformaccount; } }
        public Entity_WebsiteFormValue WebsiteFormValue { get { if (_websiteformvalue == null) { _websiteformvalue = new Entity_WebsiteFormValue(db); } return _websiteformvalue; } }
        public Entity_WebsiteForm WebsiteForm { get { if (_websiteform == null) { _websiteform = new Entity_WebsiteForm(db); } return _websiteform; } }
        public Entity_WebsiteFormField WebsiteFormField { get { if (_websiteformfield == null) { _websiteformfield = new Entity_WebsiteFormField(db); } return _websiteformfield; } }

        public Entity_TelegramBot TelegramBot { get { if (_telegrambot == null) { _telegrambot = new Entity_TelegramBot(db); } return _telegrambot; } }
        public Entity_TelegramAccount TelegramAccount { get { if (_telegramaccount == null) { _telegramaccount = new Entity_TelegramAccount(db); } return _telegramaccount; } }
        public Entity_TelegramCommand TelegramCommand { get { if (_telegramcommand == null) { _telegramcommand = new Entity_TelegramCommand(db); } return _telegramcommand; } }
        public Entity_TelegramKeyBoard TelegramKeyBoard { get { if (_telegramkeyboard == null) { _telegramkeyboard = new Entity_TelegramKeyBoard(db); } return _telegramkeyboard; } }
        public Entity_TelegramKeyBoardItem TelegramKeyBoardItem { get { if (_telegramkeyboarditem == null) { _telegramkeyboarditem = new Entity_TelegramKeyBoardItem(db); } return _telegramkeyboarditem; } }

        public Entity_Dashboard Dashboard { get { if (_dashboard == null) { _dashboard = new Entity_Dashboard(db); } return _dashboard; } }
        public Entity_UserGroupDashboard UserGroupDashboard { get { if (_usergroupdashboard == null) { _usergroupdashboard = new Entity_UserGroupDashboard(db); } return _usergroupdashboard; } }

        public Entity_Shipping Shipping { get { if (_shipping == null) { _shipping = new Entity_Shipping(db); } return _shipping; } }

        public Entity_Clearing Clearing { get { if (_clearing == null) { _clearing = new Entity_Clearing(db); } return _clearing; } }

        public Entity_PaymentWebsite PaymentWebsite { get { if (_paymentwebsite == null) { _paymentwebsite = new Entity_PaymentWebsite(db); } return _paymentwebsite; } }

        public Entity_StaticPage StaticPage { get { if (_staticpage == null) { _staticpage = new Entity_StaticPage(db); } return _staticpage; } }
        public Entity_WebsiteDetail WebsiteDetail { get { if (_websitedetail == null) { _websitedetail = new Entity_WebsiteDetail(db); } return _websitedetail; } }
        public Entity_Tag Tag { get { if (_tag == null) { _tag = new Entity_Tag(db); } return _tag; } }
        public Entity_TagPost TagPost { get { if (_tagPost == null) { _tagPost = new Entity_TagPost(db); } return _tagPost; } }
        public Entity_TagSubCategory TagSubCategory { get { if (_tagSubCategory== null) { _tagSubCategory = new Entity_TagSubCategory(db); } return _tagSubCategory; } }
        public Entity_ContactUs ContactUs { get { if (_contactUs == null) { _contactUs = new Entity_ContactUs(db); } return _contactUs; } }
        public Entity_ContactUsField ContactUsField { get { if (_contactUsField == null) { _contactUsField = new Entity_ContactUsField(db); } return _contactUsField; } }
        public Entity_PriceRange PriceRange { get { if (_priceRange == null) { _priceRange = new Entity_PriceRange(db); } return _priceRange; } }
        public Entity_CompetitiveFeature CompetitiveFeature { get { if (_competitiveFeature == null) { _competitiveFeature = new Entity_CompetitiveFeature(db); } return _competitiveFeature; } }
        public Entity_StoreCompetitiveFeature StoreCompetitiveFeature { get { if (_storeCompetitiveFeature == null) { _storeCompetitiveFeature = new Entity_StoreCompetitiveFeature(db); } return _storeCompetitiveFeature; } }

        public Entity_Survey Survey { get { if (_survey == null) { _survey = new Entity_Survey(db); } return _survey; } }
        public Entity_SurveyLanguage SurveyLanguage { get { if (_surveylanguage == null) { _surveylanguage = new Entity_SurveyLanguage(db); } return _surveylanguage; } }
        public Entity_SurveyQuestion SurveyQuestion { get { if (_surveyquestion == null) { _surveyquestion = new Entity_SurveyQuestion(db); } return _surveyquestion; } }
        public Entity_SurveyQuestionLanguage SurveyQuestionLanguage { get { if (_surveyquestionlanguage == null) { _surveyquestionlanguage = new Entity_SurveyQuestionLanguage(db); } return _surveyquestionlanguage; } }
        public Entity_SurveyQuestionItem SurveyQuestionItem { get { if (_surveyquestionitem == null) { _surveyquestionitem = new Entity_SurveyQuestionItem(db); } return _surveyquestionitem; } }
        public Entity_SurveyQuestionItemLanguage SurveyQuestionItemLanguage { get { if (_surveyquestionitemlanguage == null) { _surveyquestionitemlanguage = new Entity_SurveyQuestionItemLanguage(db); } return _surveyquestionitemlanguage; } }
        public Entity_SurveyAnswer SurveyAnswer { get { if (_surveyanswer == null) { _surveyanswer = new Entity_SurveyAnswer(db); } return _surveyanswer; } }
        public Entity_SurveyAnswerDetailItem SurveyAnswerDetailItem { get { if (_surveyAnswerDetailItem == null) { _surveyAnswerDetailItem = new Entity_SurveyAnswerDetailItem(db); } return _surveyAnswerDetailItem; } }
        public Entity_Cart Cart { get { if (_cart == null) { _cart = new Entity_Cart(db); } return _cart; } }
        public Entity_CartStore CartStore { get { if (_cartstore == null) { _cartstore = new Entity_CartStore(db); } return _cartstore; } }
        public Entity_CartStoreItem CartStoreItem { get { if (_cartstoreitem == null) { _cartstoreitem = new Entity_CartStoreItem(db); } return _cartstoreitem; } }
        public Entity_Delivery Delivery { get { if (_delivery == null) { _delivery = new Entity_Delivery(db); } return _delivery; } }
        public Entity_DeliveryType DeliveryType { get { if (_deliverytype == null) { _deliverytype = new Entity_DeliveryType(db); } return _deliverytype; } }
        public Entity_ProductProductOptionItem ProductProductOptionItem { get { if (_productProductOptionItem == null) { _productProductOptionItem = new Entity_ProductProductOptionItem(db); } return _productProductOptionItem; } }
        public Entity_CacheSetting CacheSetting { get { if (_cachesetting == null) { _cachesetting = new Entity_CacheSetting(db); } return _cachesetting; } }
        public Entity_ProductExteraType productExteraType{ get { if (_productExteraType == null) { _productExteraType = new Entity_ProductExteraType(db); } return _productExteraType; } }
        public Entity_ProductExteraCategory ProductExteraCategory { get { if (_productExteraCategory == null) { _productExteraCategory = new Entity_ProductExteraCategory(db); } return _productExteraCategory; } }
        public Entity_ProductExteraSubCategory ProductExteraSubCategory { get { if (_productExteraSubCategory == null) { _productExteraSubCategory = new Entity_ProductExteraSubCategory(db); } return _productExteraSubCategory; } }
        public Entity_PostQuestion PostQuestion { get { if (_PostQuestion == null) { _PostQuestion = new Entity_PostQuestion(db); } return _PostQuestion; } }

        public bool Save()
        {
            try
            {
                int result = db.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException)
            {

                //HttpContext.Current.Response.Redirect("~/error/index/502");
                return false;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (DbUpdateException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                throw;
                //  HttpContext.Current.Response.Redirect("~/error/index/501");
                //return false;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/error/index/" + ex.ErrorCode);
                return false;
                throw ex;
            }
        }
        public void Dispose() { db.Dispose(); }

    }
}
