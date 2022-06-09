using System;
using Common.Commands.Accounts;
using Common.Enums;
using PiggyBank.Domain.Validators.Accounts;
using FluentValidation.TestHelper;
using Xunit;

namespace Domain.Tests.Validators.Accounts
{
    public class AddAccountCommandValidatorTests
    {
        private readonly AddAccountCommandValidator _validator;

        public AddAccountCommandValidatorTests()
            => _validator = new AddAccountCommandValidator();

        [Fact]
        public void Validation_Default_NoErrors()
        {
            var command = new AddAccountCommand
            {
                Balance = 10,
                Currency = "USD",
                Title = "Hello",
                Type = AccountType.Card,
                CreatedBy = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                IsArchived = false
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
        
        [Theory]
        [InlineData(10)]
        [InlineData(-11)]
        [InlineData(13)]
        [InlineData(100)]
        public void Validation_AccountTypeIsWrong_AccountTypeError(int accountType)
        {
            var command = new AddAccountCommand
            {
                Balance = 10,
                Currency = "USD",
                Title = "Hello",
                Type = (AccountType)accountType,
                CreatedBy = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                IsArchived = false
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Type);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validation_CurrencyIsEmptyOrNull_CurrencyError(string currency)
        {
            var command = new AddAccountCommand
            {
                Balance = 10,
                Currency = currency,
                Title = "Hello",
                Type = AccountType.Card,
                CreatedBy = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                IsArchived = false
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Currency);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validation_TitleIsEmptyOrNull_TitleError(string title)
        {
            var command = new AddAccountCommand
            {
                Balance = 10,
                Currency = "USD",
                Title = title,
                Type = AccountType.Card,
                CreatedBy = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                IsArchived = false
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Title);
        }

        [Fact]
        public void Validation_CreatedByIsEmpty_CreatedByError()
        {
            var command = new AddAccountCommand
            {
                Balance = 10,
                Currency = "USD",
                Title = "NEW ACCOUNT",
                Type = AccountType.Card,
                CreatedBy = Guid.Empty,
                CreatedOn = DateTime.UtcNow,
                IsArchived = false
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.CreatedBy);
        }
        
        [Fact]
        public void Validation_CreatedOnIsYesterday_CreatedOnError()
        {
            var command = new AddAccountCommand
            {
                Balance = 10,
                Currency = "USD",
                Title = "NEW ACCOUNT",
                Type = AccountType.Card,
                CreatedBy = Guid.Empty,
                CreatedOn = DateTime.UtcNow.AddDays(-1),
                IsArchived = false
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.CreatedOn);
        }
    }
}