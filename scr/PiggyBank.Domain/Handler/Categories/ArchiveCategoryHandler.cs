﻿using Microsoft.EntityFrameworkCore;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Categories;

namespace PiggyBank.Domain.Handler.Categories
{
    public class ArchiveCategoryHandler : BaseHandler<ArchiveCategoryCommand>
    {
        public ArchiveCategoryHandler(PiggyContext context, ArchiveCategoryCommand command)
            : base(context, command) { }

        public override async Task Invoke(CancellationToken token)
        {
            var repository = GetRepository<Category>();
            var category = await repository.FirstOrDefaultAsync(a => a.Id == Command.Id && !a.IsDeleted, token);

            if (category == null || category.IsArchived)
                return;

            category.IsArchived = true;
            category.ModifiedOn = Command.ModifiedOn;
            category.ModifiedBy = Command.ModifiedBy;
            repository.Update(category);
        }
    }
}
