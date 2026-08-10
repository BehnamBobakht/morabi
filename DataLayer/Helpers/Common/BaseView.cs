using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.ViewModels;
using DataLayer.Entities;

namespace DataLayer.Base
{
    public static class BaseView
    {
        public static List<ViewColor> ToView(this List<Color> list)
        {
            List<ViewColor> Output = new List<ViewColor>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewColor(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewSize> ToView(this List<Size> list)
        {
            List<ViewSize> Output = new List<ViewSize>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewSize(list[i], i, MaxZero));
            }
            return Output;
        }
        public static List<ViewProductCompany> ToView(this List<ProductCompany> list)
        {
            List<ViewProductCompany> Output = new List<ViewProductCompany>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewProductCompany(list[i]));
            }
            return Output;
        }
        public static List<ViewProductBrand> ToView(this List<ProductBrand> list)
        {
            List<ViewProductBrand> Output = new List<ViewProductBrand>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewProductBrand(list[i], i, MaxZero));
            }
            return Output;
        }
        public static List<ViewProductOption> ToView(this List<ProductOption> list)
        {
            List<ViewProductOption> Output = new List<ViewProductOption>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewProductOption(list[i]));
            }
            return Output;
        }
        public static List<ViewProduct> ToView(this List<Product> list)
        {
            List<ViewProduct> Output = new List<ViewProduct>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewProduct(list[i], i, MaxZero));
            }
            return Output;
        }
        public static List<ViewStoreProduct> ToView(this List<StoreProduct> list,bool complate=true)
        {
            List<ViewStoreProduct> Output = new List<ViewStoreProduct>();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewStoreProduct(list[i],complate:complate));
            }
            return Output;
        }
        public static List<ViewStoreComment> ToView(this List<StoreComment> list)
        {
            List<ViewStoreComment> Output = new List<ViewStoreComment>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewStoreComment(list[i], i, MaxZero));
            }
            return Output;
        }
        public static List<ViewStoreProductQuantity> ToView(this List<StoreProductQuantity> list)
        {
            List<ViewStoreProductQuantity> Output = new List<ViewStoreProductQuantity>();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewStoreProductQuantity(list[i]));
            }
            return Output;
        }
        public static List<ViewProductCustomField> ToView(this List<ProductCustomField> list)
        {
            List<ViewProductCustomField> Output = new List<ViewProductCustomField>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewProductCustomField(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewProductCustomItem> ToView(this List<ProductCustomItem> list)
        {
            List<ViewProductCustomItem> Output = new List<ViewProductCustomItem>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewProductCustomItem(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewProductSubCategory> ToView(this List<ProductSubCategory> list)
        {
            List<ViewProductSubCategory> Output = new List<ViewProductSubCategory>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewProductSubCategory(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewProductType> ToView(this List<ProductType> list)
        {
            List<ViewProductType> Output = new List<ViewProductType>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewProductType(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewProductCategory> ToView(this List<ProductCategory> list)
        {
            List<ViewProductCategory> Output = new List<ViewProductCategory>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewProductCategory(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewSiteUser> ToView(this List<SiteUser> list)
        {
            List<ViewSiteUser> Output = new List<ViewSiteUser>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewSiteUser(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewUserGroup> ToView(this List<UserGroup> list)
        {
            List<ViewUserGroup> Output = new List<ViewUserGroup>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewUserGroup(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewTemplate> ToView(this List<Template> list)
        {
            List<ViewTemplate> Output = new List<ViewTemplate>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewTemplate(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewLanguage> ToView(this List<Language> list)
        {
            List<ViewLanguage> Output = new List<ViewLanguage>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewLanguage(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewGallery> ToView(this List<Gallery> list)
        {
            List<ViewGallery> Output = new List<ViewGallery>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewGallery(list[i], i, MaxZero));
            }
            return Output;
        }
        public static List<ViewWebsiteContactForm> ToView(this List<WebsiteContactForm> list)
        {
            List<ViewWebsiteContactForm> Output = new List<ViewWebsiteContactForm>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewWebsiteContactForm(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewSlider> ToView(this List<Slider> list)
        {
            List<ViewSlider> Output = new List<ViewSlider>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewSlider(list[i], i, MaxZero));
            }
            return Output;
        }
        public static List<ViewSurvey> ToView(this List<Survey> list)
        {
            List<ViewSurvey> Output = new List<ViewSurvey>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewSurvey(list[i], i, MaxZero));
            }
            return Output;
        }
        public static List<ViewSurveyQuestion> ToView(this List<SurveyQuestion> list)
        {
            List<ViewSurveyQuestion> Output = new List<ViewSurveyQuestion>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewSurveyQuestion(list[i], i, MaxZero));
            }
            return Output;
        }
        public static List<ViewSurveyQuestionItem> ToView(this List<SurveyQuestionItem> list)
        {
            List<ViewSurveyQuestionItem> Output = new List<ViewSurveyQuestionItem>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewSurveyQuestionItem(list[i], i, MaxZero));
            }
            return Output;
        }
        public static List<ViewSurveyAnswer> ToView(this List<SurveyAnswer> list)
        {
            List<ViewSurveyAnswer> Output = new List<ViewSurveyAnswer>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewSurveyAnswer(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewShop> ToView(this List<Shop> list)
        {
            List<ViewShop> Output = new List<ViewShop>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewShop(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewShopContact> ToView(this List<ShopContact> list)
        {
            List<ViewShopContact> Output = new List<ViewShopContact>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewShopContact(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewShopUser> ToView(this List<ShopUser> list)
        {
            List<ViewShopUser> Output = new List<ViewShopUser>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewShopUser(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewShopChat> ToView(this List<ShopChat> list)
        {
            List<ViewShopChat> Output = new List<ViewShopChat>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewShopChat(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewWebsiteDomain> ToView(this List<WebsiteDomain> list)
        {
            List<ViewWebsiteDomain> Output = new List<ViewWebsiteDomain>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewWebsiteDomain(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewWebsite> ToView(this List<Website> list)
        {
            List<ViewWebsite> Output = new List<ViewWebsite>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewWebsite(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewAgreement> ToView(this List<Agreement> list)
        {
            List<ViewAgreement> Output = new List<ViewAgreement>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewAgreement(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewDiscountGroup> ToView(this List<DiscountGroup> list)
        {
            List<ViewDiscountGroup> Output = new List<ViewDiscountGroup>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewDiscountGroup(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewDiscount> ToView(this List<Discount> list)
        {
            List<ViewDiscount> Output = new List<ViewDiscount>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewDiscount(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewShopFollow> ToView(this List<ShopFollow> list)
        {
            List<ViewShopFollow> Output = new List<ViewShopFollow>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewShopFollow(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewMerchant> ToView(this List<Merchant> list)
        {
            List<ViewMerchant> Output = new List<ViewMerchant>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewMerchant(list[i], i, MaxZero));
            }
            return Output;
        }




        public static List<ViewCategory> ToView(this List<Category> list)
        {
            List<ViewCategory> Output = new List<ViewCategory>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewCategory(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewNotificationProject> ToView(this List<NotificationProject> list)
        {
            List<ViewNotificationProject> Output = new List<ViewNotificationProject>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewNotificationProject(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewPost> ToView(this List<Post> list)
        {
            List<ViewPost> Output = new List<ViewPost>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewPost(list[i], i, MaxZero));
            }
            return Output;
        }

        //public static List<ViewRebate> ToView(this List<Rebate> list)
        //{
        //    List<ViewRebate> Output = new List<ViewRebate>();
        //    string MaxZero = list.GetMaxZero();

        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        Output.Add(new ViewRebate(list[i], i, MaxZero));
        //    }
        //    return Output;
        //}
        public static List<ViewCompetitiveFeature> ToView(this List<CompetitiveFeature> list)
        {
            List<ViewCompetitiveFeature> Output = new List<ViewCompetitiveFeature>();
         
            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewCompetitiveFeature(list[i]));
            }
            return Output;
        }

        public static List<ViewAccount> ToView(this List<Account> list)
        {
            List<ViewAccount> Output = new List<ViewAccount>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewAccount(list[i], i, MaxZero));
            }
            return Output;
        }
        public static List<ViewPaymentExame> ToView(this List<PaymentExame> list)
        {
            List<ViewPaymentExame> Output = new List<ViewPaymentExame>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewPaymentExame(list[i], i, MaxZero));
            }
            return Output;
        }
        public static List<ViewPaymentCertificate> ToView(this List<PaymentCertificate> list)
        {
            List<ViewPaymentCertificate> Output = new List<ViewPaymentCertificate>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewPaymentCertificate(list[i], i, MaxZero));
            }
            return Output;
        }
        public static List<ViewAccountOrder> ToView(this List<AccountOrder> list)
        {
            List<ViewAccountOrder> Output = new List<ViewAccountOrder>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewAccountOrder(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewClearing> ToView(this List<Clearing> list)
        {
            List<ViewClearing> Output = new List<ViewClearing>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewClearing(list[i]));
            }
            return Output;
        }

        public static List<ViewProductComment> ToView(this List<ProductComment> list)
        {
            List<ViewProductComment> Output = new List<ViewProductComment>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewProductComment(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewSmsSetting> ToView(this List<SmsSetting> list)
        {
            List<ViewSmsSetting> Output = new List<ViewSmsSetting>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewSmsSetting(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewNewsletterGroup> ToView(this List<NewsletterGroup> list)
        {
            List<ViewNewsletterGroup> Output = new List<ViewNewsletterGroup>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewNewsletterGroup(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewTelegramBot> ToView(this List<TelegramBot> list)
        {
            List<ViewTelegramBot> Output = new List<ViewTelegramBot>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewTelegramBot(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewTelegramAccount> ToView(this List<TelegramAccount> list)
        {
            List<ViewTelegramAccount> Output = new List<ViewTelegramAccount>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewTelegramAccount(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewTelegramCommand> ToView(this List<TelegramCommand> list)
        {
            List<ViewTelegramCommand> Output = new List<ViewTelegramCommand>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewTelegramCommand(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewTelegramKeyBoard> ToView(this List<TelegramKeyBoard> list)
        {
            List<ViewTelegramKeyBoard> Output = new List<ViewTelegramKeyBoard>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewTelegramKeyBoard(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewTelegramKeyBoardItem> ToView(this List<TelegramKeyBoardItem> list)
        {
            List<ViewTelegramKeyBoardItem> Output = new List<ViewTelegramKeyBoardItem>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewTelegramKeyBoardItem(list[i], i, MaxZero));
            }
            return Output;
        }

        public static List<ViewMenu> ToView(this List<Menu> list)
        {
            List<ViewMenu> Output = new List<ViewMenu>();
            string MaxZero = list.GetMaxZero();

            for (int i = 0; i < list.Count; i++)
            {
                Output.Add(new ViewMenu(list[i], i, MaxZero));
            }
            return Output;
        }
    }
}
