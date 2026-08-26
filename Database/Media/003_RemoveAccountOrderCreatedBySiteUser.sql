/*
    Remove CreatedBySiteUserId from AccountOrder.
    Run after 002_ManualCourseEnrollment.sql. Safe to run more than once.
*/
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_AccountOrder_CreatedBySiteUser')
BEGIN
    ALTER TABLE dbo.AccountOrder DROP CONSTRAINT FK_AccountOrder_CreatedBySiteUser;
END;

IF COL_LENGTH('dbo.AccountOrder', 'CreatedBySiteUserId') IS NOT NULL
BEGIN
    ALTER TABLE dbo.AccountOrder DROP COLUMN CreatedBySiteUserId;
END;

COMMIT TRANSACTION;
