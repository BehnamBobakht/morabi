SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID('dbo.CourseDocument', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.CourseDocument
    (
        ID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_CourseDocument PRIMARY KEY,
        ProductId int NOT NULL,
        Title nvarchar(250) NOT NULL,
        Description nvarchar(max) NULL,
        PdfUrl nvarchar(2000) NOT NULL,
        SortOrder int NOT NULL CONSTRAINT DF_CourseDocument_SortOrder DEFAULT (0),
        Active bit NOT NULL CONSTRAINT DF_CourseDocument_Active DEFAULT (1),
        CreateDateTime datetime NOT NULL CONSTRAINT DF_CourseDocument_CreateDateTime DEFAULT (GETDATE()),
        UpdateDateTime datetime NULL,
        CONSTRAINT FK_CourseDocument_Product FOREIGN KEY (ProductId) REFERENCES dbo.Product(ID)
    );
    CREATE INDEX IX_CourseDocument_Product ON dbo.CourseDocument(ProductId, Active, SortOrder);
END;

IF OBJECT_ID('dbo.CourseDocumentVisit', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.CourseDocumentVisit
    (
        ID bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_CourseDocumentVisit PRIMARY KEY,
        CourseDocumentId int NOT NULL,
        AccountId bigint NOT NULL,
        FirstVisitDateTime datetime NOT NULL CONSTRAINT DF_CourseDocumentVisit_First DEFAULT (GETDATE()),
        LastVisitDateTime datetime NOT NULL CONSTRAINT DF_CourseDocumentVisit_Last DEFAULT (GETDATE()),
        CONSTRAINT FK_CourseDocumentVisit_Document FOREIGN KEY (CourseDocumentId) REFERENCES dbo.CourseDocument(ID),
        CONSTRAINT FK_CourseDocumentVisit_Account FOREIGN KEY (AccountId) REFERENCES dbo.Account(ID),
        CONSTRAINT UQ_CourseDocumentVisit UNIQUE (CourseDocumentId, AccountId)
    );
    CREATE INDEX IX_CourseDocumentVisit_Account ON dbo.CourseDocumentVisit(AccountId, LastVisitDateTime);
END;

COMMIT TRANSACTION;
