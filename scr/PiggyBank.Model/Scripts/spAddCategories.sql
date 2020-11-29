CREATE PROCEDURE [spAddCategories]
@UserId UNIQUEIDENTIFIER
AS
BEGIN
    DECLARE @Count INT = 0;
    DECLARE @Type INT = 1;
    DECLARE @Colores TABLE([Index] INT, Color nvarchar(10));
    DECLARE @Color NVARCHAR(10);

    INSERT @Colores([Index], Color) VALUES (0, '#ffb900');
    INSERT @Colores([Index], Color) VALUES (1, '#ff8c00');
    INSERT @Colores([Index], Color) VALUES (2, '#f7630c');
    INSERT @Colores([Index], Color) VALUES (3, '#ca5010');
    INSERT @Colores([Index], Color) VALUES (4, '#da3b01');
    INSERT @Colores([Index], Color) VALUES (5, '#ef6950');
    INSERT @Colores([Index], Color) VALUES (6, '#e74856');
    INSERT @Colores([Index], Color) VALUES (7, '#e81123');
    INSERT @Colores([Index], Color) VALUES (8, '#ea005e');
    INSERT @Colores([Index], Color) VALUES (9, '#c30052');

    WHILE @Count < 10
        BEGIN

            IF @Count % 2 = 0
                SET @Type = 1;
            ELSE
                SET @Type = 2;

            SELECT @Color = Color FROM @Colores WHERE [Index] = @Count;

            INSERT INTO [Pb].[Categories](CreatedOn, CreatedBy, IsDeleted, IsArchived, Title, HexColor, [Type])
            VALUES (GETDATE(), @UserId, 0, 0, CONCAT('Category', @Count), @Color, @Type);

            SET @Count = @Count + 1;
        END

END
GO