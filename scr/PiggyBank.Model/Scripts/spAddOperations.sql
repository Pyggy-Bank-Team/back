CREATE PROCEDURE [spAddOperations] @UserId UNIQUEIDENTIFIER
AS
BEGIN
    DECLARE @Count INT = 0;
    DECLARE @Type INT = 1;
    DECLARE @CategoryId INT = 0, @CategoryType INT = 0, @AccountId INT = 0, @To INT = 0;
    DECLARE CategoryCursor CURSOR
        FOR SELECT Id, [Type] FROM [Pb].[Categories] WHERE CreatedBy = @UserId;
    DECLARE AccountCursor CURSOR
        FOR SELECT Id FROM [Pb].[Accounts] WHERE CreatedBy = @UserId;

    DECLARE @CategoryCursorStatus INT = 0, @AccountCursorStatus INT = 0;

    OPEN CategoryCursor;
    OPEN AccountCursor;

    WHILE @Count < 100
        BEGIN

            FETCH NEXT FROM AccountCursor INTO @AccountId;

            SET @AccountCursorStatus = @@FETCH_STATUS;

            IF @AccountCursorStatus = -1
                BEGIN
                    --Re-open cursor
                    CLOSE AccountCursor;
                    OPEN AccountCursor;

                    FETCH NEXT FROM AccountCursor INTO @AccountId;
                END


            IF @Count % 2 = 0
                BEGIN
                    SET @Type = 1;

                    FETCH NEXT FROM CategoryCursor INTO @CategoryId, @CategoryType;

                    SET @CategoryCursorStatus = @@FETCH_STATUS;

                    IF @CategoryCursorStatus = -1
                        BEGIN
                            --Re-open cursor
                            CLOSE CategoryCursor;
                            OPEN CategoryCursor;

                            FETCH NEXT FROM CategoryCursor INTO @CategoryId, @CategoryType;
                        END

                    INSERT INTO [Pb].[Operations](CreatedOn, CreatedBy, Comment, [Type], IsDeleted, Snapshot, Discriminator, CategoryId, Amount,
                                                  AccountId)
                    VALUES (GETDATE() - @Count % 10, @UserId, CONCAT('The budget operation', @Count), @Type, 0,
                            CONCAT('{"CategoryType":', @CategoryType, '}'), 'BudgetOperation', @CategoryId, 100, @AccountId);
                END
            ELSE
                BEGIN
                    SET @Type = 2;

                    FETCH NEXT FROM AccountCursor INTO @To;

                    SET @AccountCursorStatus = @@FETCH_STATUS;

                    IF @AccountCursorStatus = -1
                        BEGIN
                            --Re-open cursor
                            CLOSE AccountCursor;
                            OPEN AccountCursor;

                            FETCH NEXT FROM AccountCursor INTO @To;
                        END

                    INSERT INTO [Pb].[Operations](CreatedOn, CreatedBy, Comment, [Type], IsDeleted, Discriminator, Amount, [From], [To])
                    VALUES (GETDATE() - @Count % 10, @UserId, CONCAT('The transfer operation', @Count), @Type, 0, 'TransferOperation', 100,
                            @AccountId, @To);
                END

            SET @Count = @Count + 1;
        END

    CLOSE AccountCursor;
    CLOSE CategoryCursor;

    DEALLOCATE AccountCursor;
    DEALLOCATE CategoryCursor;

END
GO