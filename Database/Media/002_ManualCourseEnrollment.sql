/*
    Contract used by the admin application for manual course enrollment.
    The application performs these operations inside a transaction.
*/
CREATE OR ALTER PROCEDURE dbo.Media_EnrollAccountInCourse
    @AccountId bigint,
    @ProductId int
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Account WHERE ID = @AccountId)
        THROW 50001, N'کاربر پیدا نشد.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.Product WHERE ID = @ProductId)
        THROW 50002, N'دوره پیدا نشد.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.AccountOrderProduct aop
        INNER JOIN dbo.AccountOrder ao ON ao.ID = aop.OrderId
        LEFT JOIN dbo.Code c ON c.ID = ao.StatusId
        WHERE ao.AccountId = @AccountId
          AND aop.ProductId = @ProductId
          AND (c.Label = 'ORDER_STATUS_SUCCESS' OR ao.StatusId IN (2068, 2073))
    )
    BEGIN
        SELECT CAST(0 AS bit) AS Inserted, N'کاربر قبلاً عضو این دوره است.' AS Message;
        RETURN;
    END;

    DECLARE @StatusId int =
    (
        SELECT TOP (1) ID FROM dbo.Code WHERE Label = 'ORDER_STATUS_SUCCESS'
    );
    IF @StatusId IS NULL SET @StatusId = 2068;

    DECLARE @StoreProductQuantityId int =
    (
        SELECT TOP (1) spq.ID
        FROM dbo.StoreProductQuantity spq
        INNER JOIN dbo.StoreProduct sp ON sp.ID = spq.StoreProductId
        WHERE sp.ProductId = @ProductId
        ORDER BY spq.ID
    );

    IF @StoreProductQuantityId IS NULL
        THROW 50003, N'برای این محصول تنوع/موجودی قابل استفاده پیدا نشد.', 1;

    BEGIN TRANSACTION;

    INSERT dbo.AccountOrder
    (
        AccountId, StatusId, Price, Datetime, Description,
        DeliveryPrice, UniqueId, EarnestRemainingPrice,
        IsManualByAdmin
    )
    VALUES
    (
        @AccountId, @StatusId, 0, GETDATE(),
        N'عضویت دستی در دوره توسط مدیر', 0, CONVERT(nvarchar(50), NEWID()), 0,
        1
    );

    DECLARE @OrderId int = SCOPE_IDENTITY();

    INSERT dbo.AccountOrderProduct
    (
        OrderId, ProductId, Count, Price, Datetime,
        ProductPrice, ProductDiscount, StoreProductQuantityId,
        SingleEarnestRemainingPrice, TotalEarnestRemainingPrice
    )
    VALUES
    (
        @OrderId, @ProductId, 1, 0, GETDATE(),
        0, 0, @StoreProductQuantityId, 0, 0
    );

    COMMIT TRANSACTION;

    SELECT CAST(1 AS bit) AS Inserted, N'کاربر با موفقیت به دوره اضافه شد.' AS Message;
END;
