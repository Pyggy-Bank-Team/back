CREATE PROCEDURE [spAddAccounts]
@UserId UNIQUEIDENTIFIER
AS
BEGIN
DECLARE @Count INT = 0;
DECLARE @Type INT = 1;
DECLARE @Currency NVARCHAR(3);

SELECT @Currency = [CurrencyBase] FROM [Idt].[Users]  where Id = @UserId;

WHILE @Count < 10
    BEGIN
        
        IF @Count % 2 = 0
            SET @Type = 1;
        ELSE
            SET @Type = 2;
        
        INSERT INTO [Pb].[Accounts](CreatedOn, CreatedBy, IsDeleted, IsArchived, Title, [Type], Currency, Balance)
        VALUES (GETDATE(), @UserId, 0, 0, CONCAT('Account', @Count), @Type, @Currency, 1000);

        SET @Count = @Count + 1;
    END
    	
END
GO