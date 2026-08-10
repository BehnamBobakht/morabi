/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.AccountAddress ADD
	NationalCode nvarchar(20) NULL
GO
ALTER TABLE dbo.AccountAddress SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
 

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ShopWebsiteSetting
	DROP CONSTRAINT FK_ShopWebsiteSetting_Website
GO
ALTER TABLE dbo.Website SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ShopWebsiteSetting ADD
	NationalCodeIsRequired bit NOT NULL CONSTRAINT DF_ShopWebsiteSetting_NationalCodeIsRequred DEFAULT 0,
	EmailIsRequired bit NOT NULL CONSTRAINT DF_ShopWebsiteSetting_EmailIsRequred DEFAULT 0,
	PhoneIsRequired bit NOT NULL CONSTRAINT DF_ShopWebsiteSetting_PhoneIsRequired DEFAULT 0,
	MobileIsRequired bit NOT NULL CONSTRAINT DF_ShopWebsiteSetting_MobileIsRequired DEFAULT 0,
	ZipCodeIsRequired bit NOT NULL CONSTRAINT DF_ShopWebsiteSetting_ZipCodeIsRequired DEFAULT 0
GO
ALTER TABLE dbo.ShopWebsiteSetting ADD CONSTRAINT
	FK_ShopWebsiteSetting_Website FOREIGN KEY
	(
	WebsiteId
	) REFERENCES dbo.Website
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ShopWebsiteSetting SET (LOCK_ESCALATION = TABLE)
GO
COMMIT



/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Account ADD
	SpacialAccount bit NOT NULL CONSTRAINT DF_Account_SpacialAccount DEFAULT 0
GO
ALTER TABLE dbo.Account SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductOptionItem SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.ProductProductOptionItem
	(
	ID int NOT NULL IDENTITY (1, 1),
	ProductId int NOT NULL,
	ProductOptionItemId int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ProductProductOptionItem ADD CONSTRAINT
	PK_ProductProductOptionItem PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ProductProductOptionItem ADD CONSTRAINT
	FK_ProductProductOptionItem_Product FOREIGN KEY
	(
	ProductId
	) REFERENCES dbo.Product
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ProductProductOptionItem ADD CONSTRAINT
	FK_ProductProductOptionItem_ProductOptionItem FOREIGN KEY
	(
	ProductOptionItemId
	) REFERENCES dbo.ProductOptionItem
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ProductProductOptionItem SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductCustomField ADD
	InSearchResult bit NOT NULL CONSTRAINT DF_ProductCustomField_InSearchResult DEFAULT 0
GO
ALTER TABLE dbo.ProductCustomField SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
-----------kohan 05-01-2022---------
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.[File] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	MusicFileId int NULL,
	PdfFileId int NULL,
	VideoFileId int NULL
GO
ALTER TABLE dbo.Product ADD CONSTRAINT
	FK_Product_File1 FOREIGN KEY
	(
	MusicFileId
	) REFERENCES dbo.[File]
	(
	FileId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Product ADD CONSTRAINT
	FK_Product_File2 FOREIGN KEY
	(
	PdfFileId
	) REFERENCES dbo.[File]
	(
	FileId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Product ADD CONSTRAINT
	FK_Product_File3 FOREIGN KEY
	(
	VideoFileId
	) REFERENCES dbo.[File]
	(
	FileId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.[File] ADD
	Private bit NULL
GO
ALTER TABLE dbo.[File] ADD CONSTRAINT
	DF_File_Private DEFAULT 0 FOR Private
GO
ALTER TABLE dbo.[File] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


-----------end kohan----------------

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductCustomField ADD
	Hide bit NOT NULL CONSTRAINT DF_ProductCustomField_hide DEFAULT 0
GO
ALTER TABLE dbo.ProductCustomField SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	MetaTagDescription nvarchar(MAX) NULL,
	Canonical nvarchar(MAX) NULL,
	NoFollow bit NOT NULL CONSTRAINT DF_Product_NoFollow DEFAULT 0,
	NoIndex bit NOT NULL CONSTRAINT DF_Product_NoIndex DEFAULT 0
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Post ADD
	Canonical nvarchar(MAX) NULL,
	NoFollow bit NOT NULL CONSTRAINT DF_Post_NoFollow DEFAULT 0,
	NoIndex bit NOT NULL CONSTRAINT DF_Post_NoIndex DEFAULT 0
GO
ALTER TABLE dbo.Post SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductType ADD
	Canonical nvarchar(MAX) NULL,
	MinBody nvarchar(MAX) NULL,
	NoFollow bit NOT NULL CONSTRAINT DF_ProductType_NoFollow DEFAULT 0,
	NoIndex bit NOT NULL CONSTRAINT DF_ProductType_NoIndex DEFAULT 0
GO
ALTER TABLE dbo.ProductType SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductSubCategory ADD
	Canonical nvarchar(MAX) NULL,
	MinDescription nvarchar(MAX) NULL,
	NoFollow bit NOT NULL CONSTRAINT DF_ProductSubCategory_NoFollow DEFAULT 0,
	NoIndex bit NOT NULL CONSTRAINT DF_ProductSubCategory_NoIndex DEFAULT 0
GO
ALTER TABLE dbo.ProductSubCategory SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductCategory ADD
	Canonical nvarchar(MAX) NULL,
	MinBody nvarchar(MAX) NULL,
	NoFollow bit NOT NULL CONSTRAINT DF_ProductCategory_NoFollow DEFAULT 0,
	NoIndex bit NOT NULL CONSTRAINT DF_ProductCategory_NoIndex DEFAULT 0
GO
ALTER TABLE dbo.ProductCategory SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Post ADD
	PostLabel nvarchar(300) NULL
GO
ALTER TABLE dbo.Post SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CartStore ADD
	PayementPrice float(53) NOT NULL CONSTRAINT DF_CartStore_PayementPrice DEFAULT 0
GO
ALTER TABLE dbo.CartStore SET (LOCK_ESCALATION = TABLE)
GO
COMMIT



/****** Object:  Table [dbo].[CacheSetting]    Script Date: 2/26/2022 4:16:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CacheSetting](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](300) NULL,
	[Label] [nvarchar](300) NULL,
	[Duration] [int] NOT NULL,
	[Parameters] [nvarchar](500) NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_CacheSetting] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permission]    Script Date: 2/26/2022 4:16:09 PM ******/

GO
SET IDENTITY_INSERT [dbo].[CacheSetting] ON 

INSERT [dbo].[CacheSetting] ([ID], [Name], [Label], [Duration], [Parameters], [Active]) VALUES (1, N'پیشفرض', N'Defualt', 1, NULL, 1)
INSERT [dbo].[CacheSetting] ([ID], [Name], [Label], [Duration], [Parameters], [Active]) VALUES (2, N'جستجوی محصول', N'ProductSearch', 1, NULL, 1)
INSERT [dbo].[CacheSetting] ([ID], [Name], [Label], [Duration], [Parameters], [Active]) VALUES (3, N'صفحه محصول', N'ProductIndex', 1, NULL, 1)
INSERT [dbo].[CacheSetting] ([ID], [Name], [Label], [Duration], [Parameters], [Active]) VALUES (4, N'توضیحات محصول', N'ProductIndexDetails', 1, NULL, 1)
INSERT [dbo].[CacheSetting] ([ID], [Name], [Label], [Duration], [Parameters], [Active]) VALUES (5, N'عکس های محصول', N'ProductIndexPictures', 1, NULL, 1)
INSERT [dbo].[CacheSetting] ([ID], [Name], [Label], [Duration], [Parameters], [Active]) VALUES (6, N'صفحه پست', N'PostIndex', 1, NULL, 1)
INSERT [dbo].[CacheSetting] ([ID], [Name], [Label], [Duration], [Parameters], [Active]) VALUES (7, N'جستجوی پست', N'PostSearch', 1, NULL, 1)
INSERT [dbo].[CacheSetting] ([ID], [Name], [Label], [Duration], [Parameters], [Active]) VALUES (8, N'اسلایدر', N'Slider', 1, NULL, 1)
INSERT [dbo].[CacheSetting] ([ID], [Name], [Label], [Duration], [Parameters], [Active]) VALUES (9, N'صفحات ثابت', N'StaticPage', 1, NULL, 1)
INSERT [dbo].[CacheSetting] ([ID], [Name], [Label], [Duration], [Parameters], [Active]) VALUES (10, N'بنر ها', N'BannerSearch', 1, NULL, 1)
INSERT [dbo].[CacheSetting] ([ID], [Name], [Label], [Duration], [Parameters], [Active]) VALUES (11, N'منو محصولات', N'ProductTypeMenu', 1, NULL, 1)
INSERT [dbo].[CacheSetting] ([ID], [Name], [Label], [Duration], [Parameters], [Active]) VALUES (12, N'عکس', N'Picture', 1, NULL, 1)
SET IDENTITY_INSERT [dbo].[CacheSetting] OFF
GO
INSERT [dbo].[Permission] ([ModuleId], [ParentId], [Name], [Label]) VALUES (4, 38, N'مدیریت کش', N'CacheSetting')
INSERT [dbo].[Permission] ([ModuleId], [ParentId], [Name], [Label]) VALUES (4, 38, N'ویرایش کش', N'CacheSetting_Edit')



/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Picture ADD
	Name nvarchar(150) NULL,
	Extension nvarchar(50) NULL,
	ContentType nvarchar(100) NULL,
	DirectUrl nvarchar(MAX) NULL
GO
ALTER TABLE dbo.Picture SET (LOCK_ESCALATION = TABLE)
GO
COMMIT



/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ShopWebsiteSetting
	DROP CONSTRAINT FK_ShopWebsiteSetting_Website
GO
ALTER TABLE dbo.Website SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ShopWebsiteSetting ADD
	CdnPictures bit NOT NULL CONSTRAINT DF_ShopWebsiteSetting_CdnPictures DEFAULT 0,
	CdnPicturesQuality int NOT NULL CONSTRAINT DF_ShopWebsiteSetting_CdnPicturesQuality DEFAULT 0
GO
ALTER TABLE dbo.ShopWebsiteSetting ADD CONSTRAINT
	FK_ShopWebsiteSetting_Website FOREIGN KEY
	(
	WebsiteId
	) REFERENCES dbo.Website
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ShopWebsiteSetting SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Payment ADD
	IsInPlace bit NOT NULL CONSTRAINT DF_Payment_IsInPlace_1 DEFAULT 0
GO
ALTER TABLE dbo.Payment SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DiscountGroup SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	DiscountGroupId int NULL
GO
ALTER TABLE dbo.Product ADD CONSTRAINT
	FK_Product_DiscountGroup FOREIGN KEY
	(
	DiscountGroupId
	) REFERENCES dbo.DiscountGroup
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Discount ADD
	Synced bit NOT NULL CONSTRAINT DF_Discount_Synced DEFAULT 0
GO
ALTER TABLE dbo.Discount SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DiscountGroup ADD
	Deleted bit NOT NULL CONSTRAINT DF_DiscountGroup_Deleted DEFAULT 0
GO
ALTER TABLE dbo.DiscountGroup SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Discount ADD
	Deleted bit NOT NULL CONSTRAINT DF_Discount_Deleted DEFAULT 0
GO
ALTER TABLE dbo.Discount SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ShopWebsiteSetting
	DROP CONSTRAINT FK_ShopWebsiteSetting_Website
GO
ALTER TABLE dbo.Website SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ShopWebsiteSetting ADD
	Earnest int NOT NULL CONSTRAINT DF_ShopWebsiteSetting_Earnest DEFAULT 0
GO
ALTER TABLE dbo.ShopWebsiteSetting ADD CONSTRAINT
	FK_ShopWebsiteSetting_Website FOREIGN KEY
	(
	WebsiteId
	) REFERENCES dbo.Website
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ShopWebsiteSetting SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	HasEarnest bit NOT NULL CONSTRAINT DF_Product_HasEarnest DEFAULT 0
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CartStoreItem ADD
	SingleEarnestRemainingPrice float(53) NOT NULL CONSTRAINT DF_CartStoreItem_SingleEarnestRemainingPrice DEFAULT 0
GO
ALTER TABLE dbo.CartStoreItem SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CartStore ADD
	EarnestRemainingPrice float(53) NOT NULL CONSTRAINT DF_CartStore_EarnestRemainingPrice DEFAULT 0
GO
ALTER TABLE dbo.CartStore SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Cart ADD
	EarnestRemainingPrice float(53) NOT NULL CONSTRAINT DF_Cart_EarnestRemainingPrice DEFAULT 0
GO
ALTER TABLE dbo.Cart SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CartStoreItem ADD
	TotalEarnestRemainingPrice float(53) NOT NULL CONSTRAINT DF_CartStoreItem_TotalEarnestRemainingPrice DEFAULT 0
GO
ALTER TABLE dbo.CartStoreItem SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.AccountOrderProduct ADD
	SingleEarnestRemainingPrice float(53) NOT NULL CONSTRAINT DF_AccountOrderProduct_SingleEarnestRemainingPrice DEFAULT ((0)),
	TotalEarnestRemainingPrice float(53) NOT NULL CONSTRAINT DF_AccountOrderProduct_TotalEarnestRemainingPrice DEFAULT 0
GO
ALTER TABLE dbo.AccountOrderProduct SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.AccountOrder ADD
	EarnestRemainingPrice float(53) NOT NULL CONSTRAINT DF_AccountOrder_EarnestRemainingPrice DEFAULT 0
GO
ALTER TABLE dbo.AccountOrder SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.SurveyAnswerDetail
	DROP CONSTRAINT FK_SurveyAnswerDetail_SurveyQuestionItem
GO
ALTER TABLE dbo.SurveyQuestionItem SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.SurveyAnswerDetail
	DROP COLUMN ItemId
GO
ALTER TABLE dbo.SurveyAnswerDetail SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.SurveyAnswerDetailItem
	(
	ID int NOT NULL IDENTITY (1, 1),
	AnswerDetailId int NOT NULL,
	ItemId int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.SurveyAnswerDetailItem ADD CONSTRAINT
	PK_SurveyAnswerDetailItem PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.SurveyAnswerDetailItem ADD CONSTRAINT
	FK_SurveyAnswerDetailItem_SurveyAnswerDetail FOREIGN KEY
	(
	AnswerDetailId
	) REFERENCES dbo.SurveyAnswerDetail
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.SurveyAnswerDetailItem ADD CONSTRAINT
	FK_SurveyAnswerDetailItem_SurveyQuestionItem FOREIGN KEY
	(
	ItemId
	) REFERENCES dbo.SurveyQuestionItem
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.SurveyAnswerDetailItem SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ContactUsField
	DROP CONSTRAINT FK_ContactUsField_ContactUs
GO
ALTER TABLE dbo.ContactUs SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ContactUsField
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(500) NULL,
	Label nvarchar(500) NULL,
	Value nvarchar(MAX) NULL,
	ContactUsId int NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ContactUsField SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ContactUsField ON
GO
IF EXISTS(SELECT * FROM dbo.ContactUsField)
	 EXEC('INSERT INTO dbo.Tmp_ContactUsField (ID, Name, Label, Value, ContactUsId)
		SELECT ID, Name, Label, Value, ContactUsId FROM dbo.ContactUsField WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ContactUsField OFF
GO
DROP TABLE dbo.ContactUsField
GO
EXECUTE sp_rename N'dbo.Tmp_ContactUsField', N'ContactUsField', 'OBJECT' 
GO
ALTER TABLE dbo.ContactUsField ADD CONSTRAINT
	PK_ContactUsField PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ContactUsField ADD CONSTRAINT
	FK_ContactUsField_ContactUs FOREIGN KEY
	(
	ContactUsId
	) REFERENCES dbo.ContactUs
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ShopWebsiteSetting ADD
	HasCentralizedDelivery bit NOT NULL CONSTRAINT DF_ShopWebsiteSetting_HasCentralizedDelivery DEFAULT 0
GO
ALTER TABLE dbo.ShopWebsiteSetting SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	InexpensiveItemBasePrice float(53) NOT NULL CONSTRAINT DF_Product_InexpensiveItemBasePrice DEFAULT 0
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.WebsiteDomain ADD
	PostUrl nvarchar(500) NULL
GO
ALTER TABLE dbo.WebsiteDomain SET (LOCK_ESCALATION = TABLE)
GO
COMMIT



/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductSubCategory SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductType SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.ProductExteraSubCategory
	(
	ID int NOT NULL IDENTITY (1, 1),
	ProductId int NOT NULL,
	ProductSubCategoryId int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ProductExteraSubCategory ADD CONSTRAINT
	PK_ProductExteraSubCategory PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ProductExteraSubCategory ADD CONSTRAINT
	FK_ProductExteraSubCategory_Product FOREIGN KEY
	(
	ProductId
	) REFERENCES dbo.Product
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ProductExteraSubCategory ADD CONSTRAINT
	FK_ProductExteraSubCategory_ProductSubCategory FOREIGN KEY
	(
	ProductSubCategoryId
	) REFERENCES dbo.ProductSubCategory
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ProductExteraSubCategory SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.ProductExteraType
	(
	ID int NOT NULL IDENTITY (1, 1),
	ProductId int NOT NULL,
	ProductTypeId int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ProductExteraType ADD CONSTRAINT
	PK_ProductExteraType PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ProductExteraType ADD CONSTRAINT
	FK_ProductExteraType_Product FOREIGN KEY
	(
	ProductId
	) REFERENCES dbo.Product
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ProductExteraType ADD CONSTRAINT
	FK_ProductExteraType_ProductType FOREIGN KEY
	(
	ProductTypeId
	) REFERENCES dbo.ProductType
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ProductExteraType SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductCategory SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.ProductExteraCategory
	(
	ID int NOT NULL IDENTITY (1, 1),
	ProductId int NOT NULL,
	ProductCategoryId int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ProductExteraCategory ADD CONSTRAINT
	PK_ProductExteraCategory PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ProductExteraCategory ADD CONSTRAINT
	FK_ProductExteraCategory_ProductCategory FOREIGN KEY
	(
	ProductCategoryId
	) REFERENCES dbo.ProductCategory
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ProductExteraCategory ADD CONSTRAINT
	FK_ProductExteraCategory_Product FOREIGN KEY
	(
	ProductId
	) REFERENCES dbo.Product
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ProductExteraCategory SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


--abak 2022-16-5 ----
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductBrandLanguage
	DROP CONSTRAINT FK_ProductBrandLanguage_ProductBrand
GO
ALTER TABLE dbo.ProductBrand SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductBrandLanguage
	DROP CONSTRAINT FK_ProductBrandLanguage_Language
GO
ALTER TABLE dbo.Language SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ProductBrandLanguage
	(
	ID int NOT NULL IDENTITY (1, 1),
	ProductBrandId int NOT NULL,
	LanguageId int NOT NULL,
	Name nvarchar(1000) NULL,
	Description nvarchar(MAX) NULL,
	Title nvarchar(1000) NULL,
	Summary nvarchar(MAX) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ProductBrandLanguage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ProductBrandLanguage ON
GO
IF EXISTS(SELECT * FROM dbo.ProductBrandLanguage)
	 EXEC('INSERT INTO dbo.Tmp_ProductBrandLanguage (ID, ProductBrandId, LanguageId, Name, Description, Title, Summary)
		SELECT ID, ProductBrandId, LanguageId, Name, Description, Title, Summary FROM dbo.ProductBrandLanguage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ProductBrandLanguage OFF
GO
DROP TABLE dbo.ProductBrandLanguage
GO
EXECUTE sp_rename N'dbo.Tmp_ProductBrandLanguage', N'ProductBrandLanguage', 'OBJECT' 
GO
ALTER TABLE dbo.ProductBrandLanguage ADD CONSTRAINT
	PK_ProductBrandLanguage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ProductBrandLanguage ADD CONSTRAINT
	FK_ProductBrandLanguage_Language FOREIGN KEY
	(
	LanguageId
	) REFERENCES dbo.Language
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ProductBrandLanguage ADD CONSTRAINT
	FK_ProductBrandLanguage_ProductBrand FOREIGN KEY
	(
	ProductBrandId
	) REFERENCES dbo.ProductBrand
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Account ADD
	VerifyCode nvarchar(50) NULL,
	VerifyTryCount int NOT NULL DEFAULT 0,
	LastVerifyTryDateTime datetime NULL,
	MobileVerified bit NOT NULL DEFAULT 0	
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ShopWebsiteSetting ADD
	HasLoginWithSms bit NOT NULL CONSTRAINT DF_ShopWebsiteSetting_HasLoginWithSms DEFAULT 0
GO
ALTER TABLE dbo.ShopWebsiteSetting SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.WebsiteDetail ADD
	IndexTitle nvarchar(1000) NULL,
	OtherTitle nvarchar(1000) NULL
GO
ALTER TABLE dbo.WebsiteDetail SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Picture SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductType ADD CONSTRAINT
	FK_ProductType_Picture1 FOREIGN KEY
	(
	IconId
	) REFERENCES dbo.Picture
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ProductType SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

ALTER TABLE [Producttype] ALTER COLUMN title nvarchar(MAX) NULL
---abak 2022-12-7
SET IDENTITY_INSERT [dbo].[Permission] ON 
INSERT [dbo].[Permission] (ID,[ModuleId], [ParentId], [Name], [Label]) VALUES (10216,3, 37, N'مدیریت فرم ها', N'WebsiteForm')
INSERT [dbo].[Permission] (ID,[ModuleId], [ParentId], [Name], [Label]) VALUES (10217,3, 10216, N'ثبت فرم جدید', N'WebsiteForm_New')
INSERT [dbo].[Permission] (ID,[ModuleId], [ParentId], [Name], [Label]) VALUES (10218,3 ,10216, N'ویرایش فرم جدید', N'WebsiteForm_Edit')
INSERT [dbo].[Permission] (ID,[ModuleId], [ParentId], [Name], [Label]) VALUES (10219,3 ,10216, N'حذف فرم جدید', N'WebsiteForm_Delete')
INSERT [dbo].[Permission] (ID,[ModuleId], [ParentId], [Name], [Label]) VALUES (10220,3 ,10216, N'نمایش اطلاعات فرم ها', N'WebsiteForm_Details')
SET IDENTITY_INSERT [dbo].[Permission] OFF


--zand
alter table delivery alter column BasePrice float null
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductTypeLanguage ADD
	LandingText nvarchar(MAX) NULL
GO
ALTER TABLE dbo.ProductTypeLanguage SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductCategoryLanguage ADD
	LandingText nvarchar(MAX) NULL
GO
ALTER TABLE dbo.ProductCategoryLanguage SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductSubCategoryLanguage ADD
	LandingText nvarchar(MAX) NULL
GO
ALTER TABLE dbo.ProductSubCategoryLanguage SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductSubCategory ADD
	LandingText nvarchar(MAX) NULL
GO
ALTER TABLE dbo.ProductSubCategory SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductCategory ADD
	LandingText nvarchar(MAX) NULL
GO
ALTER TABLE dbo.ProductCategory SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductType ADD
	LandingText nvarchar(MAX) NULL
GO
ALTER TABLE dbo.ProductType SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

--ShadiJewelry
--terico-omid
--TheWatchGarden
--mafreshoo
--behpardazan
--bisehna
--abak 2022-27-11--
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Gallery ADD
	Body nvarchar(MAX) NULL
GO
ALTER TABLE dbo.Gallery SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
---abak 20222/30/11---
insert into Code(Name,Label,Value,CodeGroupId)values(N'مپ','FIELD_TYPE_MAP',4,6) 
--abak 2022/4/12---
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.GalleryLanguage ADD
	Description nvarchar(MAX) NULL
GO
ALTER TABLE dbo.GalleryLanguage SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

--abak 2022-12-6--
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.GalleryPictures ADD
	NameEn nvarchar(1000) NULL
GO
ALTER TABLE dbo.GalleryPictures SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
--abak 2022-14-12--
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProductOptionLanguage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductOptionId] [int] NOT NULL,
	[LanguageId] [int] NOT NULL,
	[Name] [nvarchar](100) NULL,
 CONSTRAINT [PK_ProductOptionLanguage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProductOptionLanguage]  WITH CHECK ADD  CONSTRAINT [FK_ProductOptionLanguage_Language] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([ID])
GO

ALTER TABLE [dbo].[ProductOptionLanguage] CHECK CONSTRAINT [FK_ProductOptionLanguage_Language]
GO

ALTER TABLE [dbo].[ProductOptionLanguage]  WITH CHECK ADD  CONSTRAINT [FK_ProductOptionLanguage_ProductOption] FOREIGN KEY([ProductOptionId])
REFERENCES [dbo].[ProductOption] ([ID])
GO

ALTER TABLE [dbo].[ProductOptionLanguage] CHECK CONSTRAINT [FK_ProductOptionLanguage_ProductOption]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProductOptionItemLanguage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductOptionItemId] [int] NOT NULL,
	[LanguageId] [int] NOT NULL,
	[Name] [nvarchar](100) NULL,
 CONSTRAINT [PK_ProductOptionItemLanguage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProductOptionItemLanguage]  WITH CHECK ADD  CONSTRAINT [FK_ProductOptionItemLanguage_ProductOptionItem] FOREIGN KEY([ProductOptionItemId])
REFERENCES [dbo].[ProductOptionItem] ([ID])
GO

ALTER TABLE [dbo].[ProductOptionItemLanguage] CHECK CONSTRAINT [FK_ProductOptionItemLanguage_ProductOptionItem]
GO

ALTER TABLE [dbo].[ProductOptionItemLanguage]  WITH CHECK ADD  CONSTRAINT [FK_ProductOptionItemLanguage_ProductOptionItemLanguage] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([ID])
GO

ALTER TABLE [dbo].[ProductOptionItemLanguage] CHECK CONSTRAINT [FK_ProductOptionItemLanguage_ProductOptionItemLanguage]
GO



/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.WebsiteDocument SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	Doc2Id int NULL
GO
ALTER TABLE dbo.Product ADD CONSTRAINT
	FK_Product_WebsiteDocument1 FOREIGN KEY
	(
	Doc2Id
	) REFERENCES dbo.WebsiteDocument
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Post SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.PostQuestion
	(
	ID int NOT NULL IDENTITY (1, 1),
	QuestionText nvarchar(MAX) NULL,
	AnswerText nvarchar(MAX) NULL,
	PostId int NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.PostQuestion ADD CONSTRAINT
	PK_PostQuestion PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.PostQuestion ADD CONSTRAINT
	FK_PostQuestion_Post FOREIGN KEY
	(
	PostId
	) REFERENCES dbo.Post
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.PostQuestion SET (LOCK_ESCALATION = TABLE)
GO
COMMIT





/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ShopWebsiteSetting ADD
	LastContentUpdate datetime NULL
GO
ALTER TABLE dbo.ShopWebsiteSetting SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


update ShopWebsiteSetting set LastContentUpdate =GETDATE()
update ProductType set SyncDatetime =GETDATE()
update ProductCategory set UpdateDatetime =GETDATE()
update ProductSubCategory set UpdateDatetime =GETDATE()
update ProductBrand set SyncDatetime =GETDATE()
update product set UpdateDatetime =GETDATE()
update Post set UpdateDatetime =GETDATE()

--abak 2023-1-3
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductCategoryLanguage ADD
	MinBody nvarchar(MAX) NULL
GO
ALTER TABLE dbo.ProductCategoryLanguage SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductTypeLanguage ADD
	MinBody nvarchar(MAX) NULL
GO
ALTER TABLE dbo.ProductTypeLanguage SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductSubCategoryLanguage ADD
	MinDescription nvarchar(MAX) NULL
GO
ALTER TABLE dbo.ProductSubCategoryLanguage SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
--hamidreza z 3/6/2023

--zand
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ProductCustomValue SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Language SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.ProductCustomValueLanguage
	(
	ID int NOT NULL IDENTITY (1, 1),
	ProductCustomValueId int NOT NULL,
	Value nvarchar(MAX) NULL,
	LanguageId int NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.ProductCustomValueLanguage ADD CONSTRAINT
	PK_ProductCustomValueLanguage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ProductCustomValueLanguage ADD CONSTRAINT
	FK_ProductCustomValueLanguage_Language FOREIGN KEY
	(
	LanguageId
	) REFERENCES dbo.Language
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ProductCustomValueLanguage ADD CONSTRAINT
	FK_ProductCustomValueLanguage_ProductCustomValue FOREIGN KEY
	(
	ProductCustomValueId
	) REFERENCES dbo.ProductCustomValue
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ProductCustomValueLanguage SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
--abak-2023-24-6--
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	StateId int NULL,
	CityId int NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.City SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.State SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD CONSTRAINT
	FK_Product_State FOREIGN KEY
	(
	StateId
	) REFERENCES dbo.State
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Product ADD CONSTRAINT
	FK_Product_City FOREIGN KEY
	(
	CityId
	) REFERENCES dbo.City
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.State ADD
	Latitude nvarchar(50) NULL,
	Longitude nvarchar(50) NULL
GO
ALTER TABLE dbo.State SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.City ADD
	Latitude nvarchar(50) NULL,
	Longitude nvarchar(50) NULL
GO
ALTER TABLE dbo.City SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

SET IDENTITY_INSERT [dbo].[Permission] ON 
insert into Permission(ID,ModuleId,ParentId,Name,Label)values(11288,2,37,N'مدیریت شهر ها','City')
insert into Permission(ID,ModuleId,ParentId,Name,Label)values(11289,2,37,N'مدیریت استان ها','State')
SET IDENTITY_INSERT [dbo].[Permission] OFF

--abak 2023-28--
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Survey ADD
	Active bit NULL
GO
ALTER TABLE dbo.Survey SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.SurveyQuestion ADD
	ReletedQuestionId int NULL
GO
ALTER TABLE dbo.SurveyQuestion ADD CONSTRAINT
	FK_SurveyQuestion_SurveyQuestion1 FOREIGN KEY
	(
	ReletedQuestionId
	) REFERENCES dbo.SurveyQuestion
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.SurveyQuestion SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Picture SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.SurveyQuestion ADD
	PictureId int NULL
GO
ALTER TABLE dbo.SurveyQuestion ADD CONSTRAINT
	FK_SurveyQuestion_Picture FOREIGN KEY
	(
	PictureId
	) REFERENCES dbo.Picture
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.SurveyQuestion SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.WebsiteDocument SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.SurveyAnswerDetail ADD
	FileId int NULL
GO
ALTER TABLE dbo.SurveyAnswerDetail ADD CONSTRAINT
	FK_SurveyAnswerDetail_WebsiteDocument FOREIGN KEY
	(
	FileId
	) REFERENCES dbo.WebsiteDocument
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.SurveyAnswerDetail SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

--abak -2023-5-11--
/****** Object:  Table [dbo].[SmartOfferQuestion]    Script Date: 7/11/2023 1:51:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SmartOfferQuestion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Question] [nvarchar](max) NULL,
	[TypeId] [int] NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_SmartOfferQuestion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[SmartOfferQuestion]  WITH CHECK ADD  CONSTRAINT [FK_SmartOfferQuestion_Code] FOREIGN KEY([TypeId])
REFERENCES [dbo].[Code] ([ID])
GO

ALTER TABLE [dbo].[SmartOfferQuestion] CHECK CONSTRAINT [FK_SmartOfferQuestion_Code]
GO


/****** Object:  Table [dbo].[SmartOfferAnswer]    Script Date: 7/11/2023 1:52:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SmartOfferAnswer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Answer] [nvarchar](max) NULL,
	[Active] [bit] NULL,
	[QuestionId] [int] NULL,
 CONSTRAINT [PK_SmartOfferAnswer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[SmartOfferAnswer]  WITH CHECK ADD  CONSTRAINT [FK_SmartOfferAnswer_SmartOfferQuestion] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[SmartOfferQuestion] ([ID])
GO

ALTER TABLE [dbo].[SmartOfferAnswer] CHECK CONSTRAINT [FK_SmartOfferAnswer_SmartOfferQuestion]
GO
Set IDENTITY_INSERT [dbo].[Permission] ON 
insert into Permission(ID,ModuleId,ParentId,Name,Label)values(11588,5,75,N'مدیریت مشاوره محصول','SmartOfferQuestion')
SET IDENTITY_INSERT [dbo].[Permission] OFF

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	EuroPrice float(53) NULL,
	HasEuro bit NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
---
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	FromDatetime datetime NULL,
	ToDatetime datetime NULL,
	LinkTelegram nvarchar(1000) NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	InstallmentCount int NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	Image nvarchar(200) NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CoursePayment ADD CONSTRAINT
	FK_CoursePayment_Product FOREIGN KEY
	(
	CourseId
	) REFERENCES dbo.Product
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CoursePayment SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	IsPictureId bit NULL,
	IsNationalId bit NULL,
	IsParport bit NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CartStoreItem ADD
	Description nvarchar(1000) NULL
GO
ALTER TABLE dbo.CartStoreItem SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Exame ADD
	DateString nvarchar(500) NULL,
	ShowUser bit NULL,
	PassValue int,
	TiteString nvarchar(500) NULL
GO
ALTER TABLE dbo.Exame SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

update [dbo].[ExameQuestion] set ExameCourseId=1,ExameId=4  where ExameId=4
update [dbo].[ExameQuestion] set ExameCourseId=2,ExameId=4  where ExameId=5
update [dbo].[ExameQuestion] set ExameCourseId=3,ExameId=4  where ExameId=6
update [dbo].[ExameQuestion] set ExameCourseId=4,ExameId=4  where ExameId=7
update [dbo].[ExameQuestion] set ExameCourseId=5,ExameId=4  where ExameId=8
update [dbo].[ExameQuestion] set ExameCourseId=6,ExameId=4  where ExameId=9
update [dbo].[ExameQuestion] set ExameCourseId=6,ExameId=4  where ExameId=9
update [dbo].[ExameQuestion] set ExameCourseId=7,ExameId=4  where ExameId=10
update [dbo].[ExameQuestion] set ExameCourseId=7,ExameId=4  where ExameId=10
update [dbo].[ExameQuestion] set ExameCourseId=8,ExameId=4  where ExameId=11
update [dbo].[ExameQuestion] set ExameCourseId=9,ExameId=4  where ExameId=12
update [dbo].[ExameQuestion] set ExameCourseId=10,ExameId=4  where ExameId=13
update [dbo].[ExameQuestion] set ExameCourseId=11,ExameId=4  where ExameId=14
update [dbo].[ExameQuestion] set ExameCourseId=12,ExameId=4  where ExameId=15
update [dbo].[ExameQuestion] set ExameCourseId=12,ExameId=4  where ExameId=15
update [dbo].[ExameQuestion] set ExameCourseId=13,ExameId=4  where ExameId=16
update [dbo].[ExameQuestion] set ExameCourseId=14,ExameId=4  where ExameId=17

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	IsEduaction bit NULL,
	IsShenasnameh bit NULL,
	IsCoaching2 bit NULL,
	IsCoaching3 bit NULL,
	IsBirthday bit NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Exame ADD
	NumberString nvarchar(500) NULL
GO
ALTER TABLE dbo.Exame SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	NumberString nvarchar(500) NULL,
	NameFa nvarchar(500) NULL,
	NameEn nvarchar(500) NULL,
	DateFa nvarchar(50) NULL,
	DateEn nvarchar(50) NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Exame ADD
	Title nvarchar(500) NULL
GO
ALTER TABLE dbo.Exame SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	DatePrint nvarchar(50) NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
--new--
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ExameAccountAnswer ADD
	OrderBy uniqueidentifier NULL
GO
ALTER TABLE dbo.ExameAccountAnswer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ExameQuestion ADD
	OrderBy uniqueidentifier NULL
GO
ALTER TABLE dbo.ExameQuestion SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ExameQuestion ADD
	IDRandom int NULL
GO
ALTER TABLE dbo.ExameQuestion SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ExameAccountAnswer ADD
	IDRandom int NULL
GO
ALTER TABLE dbo.ExameAccountAnswer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	ProductsId nvarchar(150) NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	IsPackage bit NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	IsSelectMulti bit NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	IsHidden bit NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Account ADD
	LatName nvarchar(200) NULL,
	LatNameEn nvarchar(200) NULL
GO
ALTER TABLE dbo.Account SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Clubs ADD
	ShowNumbr int NULL
GO
ALTER TABLE dbo.Clubs SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Clubs ADD
	IsTop bit NULL
GO
ALTER TABLE dbo.Clubs SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Clubs ADD
	ShowName bit NOT NULL CONSTRAINT DF_Clubs_ShowName DEFAULT 0,
	ShowState bit NOT NULL CONSTRAINT DF_Clubs_ShowState DEFAULT 0,
	ShowCertificate bit NULL,
	ShowImage bit NULL
GO
ALTER TABLE dbo.Clubs SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Clubs ADD
	ShowMobile bit NOT NULL CONSTRAINT DF_Clubs_ShowImage21 DEFAULT ((0)),
	ShowInstagram bit NOT NULL CONSTRAINT DF_Clubs_ShowMobile1 DEFAULT ((0)),
	IsTopMap bit NOT NULL CONSTRAINT DF_Clubs_IsTopMap DEFAULT 0
GO
ALTER TABLE dbo.Clubs SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
--1403--
/****** Object:  Table [dbo].[PaymentCertificate]    Script Date: 3/28/2024 2:07:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PaymentCertificate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SiteUserId] [int] NULL,
	[AccountId] [int] NULL,
	[StatusId] [int] NOT NULL,
	[MerchantId] [int] NULL,
	[SubjectId] [int] NULL,
	[RefNumber] [nvarchar](50) NULL,
	[ExternalInfo1] [nvarchar](500) NULL,
	[ExternalInfo2] [nvarchar](500) NULL,
	[ExternalInfo3] [nvarchar](500) NULL,
	[ExternalInfo4] [nvarchar](500) NULL,
	[Amount] [float] NOT NULL,
	[IpAddress] [nvarchar](100) NULL,
	[Datetime] [datetime] NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[Sign] [nvarchar](500) NULL,
	[PaymentWebsiteId] [int] NULL,
	[CurrencyTypeId] [int] NULL,
	[ExternalInfo5] [nvarchar](500) NULL,
	[ExternalInfo6] [nvarchar](500) NULL,
	[ExternalInfo7] [nvarchar](500) NULL,
	[IsMobile] [bit] NULL,
	[IsInPlace] [bit] NULL,
	[Type] [int] NULL,
	[ProductId] [int] NULL,
 CONSTRAINT [PK_PaymentCertificate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PaymentCertificate] ADD  CONSTRAINT [DF_PaymentCertificate_IsMobile]  DEFAULT ((0)) FOR [IsMobile]
GO

ALTER TABLE [dbo].[PaymentCertificate] ADD  CONSTRAINT [DF_PaymentCertificate_IsInPlace]  DEFAULT ((0)) FOR [IsInPlace]
GO

ALTER TABLE [dbo].[PaymentCertificate]  WITH CHECK ADD  CONSTRAINT [FK_PaymentCertificate_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([ID])
GO

ALTER TABLE [dbo].[PaymentCertificate] CHECK CONSTRAINT [FK_PaymentCertificate_Account]
GO

ALTER TABLE [dbo].[PaymentCertificate]  WITH CHECK ADD  CONSTRAINT [FK_PaymentCertificate_Code] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Code] ([ID])
GO

ALTER TABLE [dbo].[PaymentCertificate] CHECK CONSTRAINT [FK_PaymentCertificate_Code]
GO

ALTER TABLE [dbo].[PaymentCertificate]  WITH CHECK ADD  CONSTRAINT [FK_PaymentCertificate_Merchant] FOREIGN KEY([MerchantId])
REFERENCES [dbo].[Merchant] ([ID])
GO

ALTER TABLE [dbo].[PaymentCertificate] CHECK CONSTRAINT [FK_PaymentCertificate_Merchant]
GO

ALTER TABLE [dbo].[PaymentCertificate]  WITH CHECK ADD  CONSTRAINT [FK_PaymentCertificate_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ID])
GO

ALTER TABLE [dbo].[PaymentCertificate] CHECK CONSTRAINT [FK_PaymentCertificate_Product]
GO



/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/

/****** Object:  Table [dbo].[PaymentCertificate]    Script Date: 3/22/2024 12:52:40 PM ******/
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	TeacherNameFa nvarchar(200) NULL,
	TeacherNameEn nvarchar(200) NULL,
	HasPrint bit NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	Hours nvarchar(200) NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT



/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.WebsiteDetail ADD
	CerPyzicalPrice float(53) NULL,
	CerOnlinePrice float(53) NULL
GO
ALTER TABLE dbo.WebsiteDetail SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	IsQuestion bit NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Account ADD
	QuestionAnswer nvarchar(500) NULL
GO
ALTER TABLE dbo.Account SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

---------------
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ExameAccountAnswer ADD
	QuestionAnswer nvarchar(50) NULL
GO
ALTER TABLE dbo.ExameAccountAnswer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ExameCourse ADD
	PassNumber float(53) NULL
GO
ALTER TABLE dbo.ExameCourse SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	CoursId int NULL
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/****** Object:  Table [dbo].[PaymentExame]    Script Date: 5/18/2024 3:12:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PaymentExame](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SiteUserId] [int] NULL,
	[AccountId] [bigint] NULL,
	[StatusId] [int] NOT NULL,
	[MerchantId] [int] NULL,
	[SubjectId] [int] NULL,
	[RefNumber] [nvarchar](50) NULL,
	[ExternalInfo1] [nvarchar](500) NULL,
	[ExternalInfo2] [nvarchar](500) NULL,
	[ExternalInfo3] [nvarchar](500) NULL,
	[ExternalInfo4] [nvarchar](500) NULL,
	[Amount] [float] NOT NULL,
	[IpAddress] [nvarchar](100) NULL,
	[Datetime] [datetime] NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[Sign] [nvarchar](500) NULL,
	[PaymentWebsiteId] [int] NULL,
	[CurrencyTypeId] [int] NULL,
	[ExternalInfo5] [nvarchar](500) NULL,
	[ExternalInfo6] [nvarchar](500) NULL,
	[ExternalInfo7] [nvarchar](500) NULL,
	[IsMobile] [bit] NULL,
	[IsInPlace] [bit] NULL,
	[Type] [int] NULL,
	[ProductId] [int] NULL,
	[AccountAnswerId] [int] NULL,
	[CourseId] [int] NULL,
 CONSTRAINT [PK_PaymentExame] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PaymentExame] ADD  CONSTRAINT [DF_PaymentExame_IsMobile]  DEFAULT ((0)) FOR [IsMobile]
GO

ALTER TABLE [dbo].[PaymentExame] ADD  CONSTRAINT [DF_PaymentExame_IsInPlace]  DEFAULT ((0)) FOR [IsInPlace]
GO

ALTER TABLE [dbo].[PaymentExame]  WITH CHECK ADD  CONSTRAINT [FK_PaymentExame_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([ID])
GO

ALTER TABLE [dbo].[PaymentExame] CHECK CONSTRAINT [FK_PaymentExame_Account]
GO

ALTER TABLE [dbo].[PaymentExame]  WITH CHECK ADD  CONSTRAINT [FK_PaymentExame_Code] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Code] ([ID])
GO

ALTER TABLE [dbo].[PaymentExame] CHECK CONSTRAINT [FK_PaymentExame_Code]
GO

ALTER TABLE [dbo].[PaymentExame]  WITH CHECK ADD  CONSTRAINT [FK_PaymentExame_Merchant] FOREIGN KEY([MerchantId])
REFERENCES [dbo].[Merchant] ([ID])
GO

ALTER TABLE [dbo].[PaymentExame] CHECK CONSTRAINT [FK_PaymentExame_Merchant]
GO

ALTER TABLE [dbo].[PaymentExame]  WITH CHECK ADD  CONSTRAINT [FK_PaymentExame_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ID])
GO

ALTER TABLE [dbo].[PaymentExame] CHECK CONSTRAINT [FK_PaymentExame_Product]
GO


/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ExameAccountAnswer ADD
	[Open] bit NULL
GO
ALTER TABLE dbo.ExameAccountAnswer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ExameCourse SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.PaymentExame ADD
	LastNumber nvarchar(50) NULL,
	NewNumber nvarchar(50) NULL
GO
ALTER TABLE dbo.PaymentExame ADD CONSTRAINT
	FK_PaymentExame_ExameCourse FOREIGN KEY
	(
	CourseId
	) REFERENCES dbo.ExameCourse
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.PaymentExame SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.PaymentExame ADD
	ExameId int NULL
GO
ALTER TABLE dbo.PaymentExame SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.PaymentExame ADD
	IsMulti bit NULL
GO
ALTER TABLE dbo.PaymentExame SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/****** Object:  Table [dbo].[PaymentExameCourseId]    Script Date: 6/9/2024 4:29:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PaymentExameCourseId](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PaymentExameId] [int] NULL,
	[CourseId] [int] NULL,
 CONSTRAINT [PK_PaymentExameCourseId] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.PaymentExameCourseId ADD
	Number float(53) NULL
GO
ALTER TABLE dbo.PaymentExameCourseId SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.PaymentExameCourseId ADD
	ProductId int NULL
GO
ALTER TABLE dbo.PaymentExameCourseId SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ExameCourse SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.PaymentExame SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.PaymentExameCourseId ADD CONSTRAINT
	FK_PaymentExameCourseId_PaymentExame FOREIGN KEY
	(
	PaymentExameId
	) REFERENCES dbo.PaymentExame
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.PaymentExameCourseId ADD CONSTRAINT
	FK_PaymentExameCourseId_ExameCourse FOREIGN KEY
	(
	CourseId
	) REFERENCES dbo.ExameCourse
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.PaymentExameCourseId SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ExameCourse ADD
	Deleted bit NULL
GO
ALTER TABLE dbo.ExameCourse ADD CONSTRAINT
	DF_ExameCourse_Deleted DEFAULT 0 FOR Deleted
GO
ALTER TABLE dbo.ExameCourse SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ExameAccountAnswer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.PaymentExame ADD CONSTRAINT
	FK_PaymentExame_ExameAccountAnswer FOREIGN KEY
	(
	AccountAnswerId
	) REFERENCES dbo.ExameAccountAnswer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.PaymentExame SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

----
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Product ADD
	ShowWord bit NOT NULL CONSTRAINT DF_Product_ShowWord DEFAULT 0
GO
ALTER TABLE dbo.Product SET (LOCK_ESCALATION = TABLE)
GO
COMMIT





/****** Object:  Table [dbo].[QuestionRegister]    Script Date: 9/3/2024 11:08:45 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[QuestionRegister](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TypeId] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[ShowNumber] [int] NULL,
 CONSTRAINT [PK_QuestionRegister] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[QuestionRegister]  WITH CHECK ADD  CONSTRAINT [FK_QuestionRegister_Code] FOREIGN KEY([TypeId])
REFERENCES [dbo].[Code] ([ID])
GO

ALTER TABLE [dbo].[QuestionRegister] CHECK CONSTRAINT [FK_QuestionRegister_Code]
GO



/****** Object:  Table [dbo].[QuestionRegisterItem]    Script Date: 9/3/2024 11:09:11 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[QuestionRegisterItem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FieldId] [int] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[SyncDatetime] [datetime] NULL,
 CONSTRAINT [PK_QuestionRegisterItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[QuestionRegisterItem]  WITH CHECK ADD  CONSTRAINT [FK_QuestionRegisterItem_QuestionRegister] FOREIGN KEY([FieldId])
REFERENCES [dbo].[QuestionRegister] ([ID])
GO

ALTER TABLE [dbo].[QuestionRegisterItem] CHECK CONSTRAINT [FK_QuestionRegisterItem_QuestionRegister]
GO


/****** Object:  Table [dbo].[QuestionRegisterProduct]    Script Date: 9/3/2024 11:09:26 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[QuestionRegisterProduct](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FieldId] [int] NOT NULL,
	[ProductId] [int] NULL,
 CONSTRAINT [PK_QuestionRegisterProduct] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[QuestionRegisterProduct]  WITH CHECK ADD  CONSTRAINT [FK_QuestionRegisterProduct_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ID])
GO

ALTER TABLE [dbo].[QuestionRegisterProduct] CHECK CONSTRAINT [FK_QuestionRegisterProduct_Product]
GO

ALTER TABLE [dbo].[QuestionRegisterProduct]  WITH CHECK ADD  CONSTRAINT [FK_QuestionRegisterProduct_QuestionRegister] FOREIGN KEY([FieldId])
REFERENCES [dbo].[QuestionRegister] ([ID])
GO

ALTER TABLE [dbo].[QuestionRegisterProduct] CHECK CONSTRAINT [FK_QuestionRegisterProduct_QuestionRegister]
GO


/****** Object:  Table [dbo].[QuestionRegisterValue]    Script Date: 9/3/2024 11:09:39 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[QuestionRegisterValue](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FieldId] [int] NOT NULL,
	[AccountId] [bigint] NOT NULL,
	[ItemId] [int] NULL,
	[FileId] [int] NULL,
	[Value] [nvarchar](max) NULL,
	[SyncDatetime] [datetime] NULL,
 CONSTRAINT [PK_QuestionRegisterValue] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[QuestionRegisterValue]  WITH CHECK ADD  CONSTRAINT [FK_QuestionRegisterValue_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([ID])
GO

ALTER TABLE [dbo].[QuestionRegisterValue] CHECK CONSTRAINT [FK_QuestionRegisterValue_Account]
GO

ALTER TABLE [dbo].[QuestionRegisterValue]  WITH CHECK ADD  CONSTRAINT [FK_QuestionRegisterValue_QuestionRegister] FOREIGN KEY([FieldId])
REFERENCES [dbo].[QuestionRegister] ([ID])
GO

ALTER TABLE [dbo].[QuestionRegisterValue] CHECK CONSTRAINT [FK_QuestionRegisterValue_QuestionRegister]
GO

ALTER TABLE [dbo].[QuestionRegisterValue]  WITH CHECK ADD  CONSTRAINT [FK_QuestionRegisterValue_QuestionRegisterItem] FOREIGN KEY([FieldId])
REFERENCES [dbo].[QuestionRegisterItem] ([ID])
GO

ALTER TABLE [dbo].[QuestionRegisterValue] CHECK CONSTRAINT [FK_QuestionRegisterValue_QuestionRegisterItem]
GO

ALTER TABLE [dbo].[QuestionRegisterValue]  WITH CHECK ADD  CONSTRAINT [FK_QuestionRegisterValue_QuestionRegisterValue] FOREIGN KEY([ID])
REFERENCES [dbo].[QuestionRegisterValue] ([ID])
GO

ALTER TABLE [dbo].[QuestionRegisterValue] CHECK CONSTRAINT [FK_QuestionRegisterValue_QuestionRegisterValue]
GO

ALTER TABLE [dbo].[QuestionRegisterValue]  WITH CHECK ADD  CONSTRAINT [FK_QuestionRegisterValue_WebsiteDocument] FOREIGN KEY([FileId])
REFERENCES [dbo].[WebsiteDocument] ([ID])
GO

ALTER TABLE [dbo].[QuestionRegisterValue] CHECK CONSTRAINT [FK_QuestionRegisterValue_WebsiteDocument]
GO


