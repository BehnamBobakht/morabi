/* Adds Picture-based covers to course videos, private collections and sessions. */
SET XACT_ABORT ON;
BEGIN TRANSACTION;
IF COL_LENGTH('dbo.CourseVideoSession', 'PictureId') IS NULL ALTER TABLE dbo.CourseVideoSession ADD PictureId int NULL;
IF COL_LENGTH('dbo.VideoCollection', 'PictureId') IS NULL ALTER TABLE dbo.VideoCollection ADD PictureId int NULL;
IF COL_LENGTH('dbo.VideoCollectionSession', 'PictureId') IS NULL ALTER TABLE dbo.VideoCollectionSession ADD PictureId int NULL;
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_CourseVideoSession_Picture')
    ALTER TABLE dbo.CourseVideoSession WITH CHECK ADD CONSTRAINT FK_CourseVideoSession_Picture FOREIGN KEY(PictureId) REFERENCES dbo.Picture(ID);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_VideoCollection_Picture')
    ALTER TABLE dbo.VideoCollection WITH CHECK ADD CONSTRAINT FK_VideoCollection_Picture FOREIGN KEY(PictureId) REFERENCES dbo.Picture(ID);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_VideoCollectionSession_Picture')
    ALTER TABLE dbo.VideoCollectionSession WITH CHECK ADD CONSTRAINT FK_VideoCollectionSession_Picture FOREIGN KEY(PictureId) REFERENCES dbo.Picture(ID);
IF COL_LENGTH('dbo.CourseVideoSession', 'ImageUrl') IS NOT NULL ALTER TABLE dbo.CourseVideoSession DROP COLUMN ImageUrl;
IF COL_LENGTH('dbo.VideoCollectionSession', 'ImageUrl') IS NOT NULL ALTER TABLE dbo.VideoCollectionSession DROP COLUMN ImageUrl;
IF COL_LENGTH('dbo.VideoCollection', 'ImageUrl') IS NOT NULL ALTER TABLE dbo.VideoCollection DROP COLUMN ImageUrl;
COMMIT TRANSACTION;