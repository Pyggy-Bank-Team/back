﻿using Microsoft.EntityFrameworkCore;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Model
{
    public class PiggyContext : DbContext
    {
        public PiggyContext(DbContextOptions<PiggyContext> options)
            : base(options) { }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<BudgetOperation> BudgetOperations { get; set; }

        public DbSet<TransferOperation> TransferOperations { get; set; }

        public DbSet<BalanceHistory> BalanceHistories { get; set; }
        
        public DbSet<BotOperation> BotOperations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BudgetOperation>()
                .Property(b => b.AccountId)
                .HasColumnName(nameof(BudgetOperation.AccountId));

            modelBuilder.Entity<BudgetOperation>()
                .Property(b => b.CategoryId)
                .HasColumnName(nameof(BudgetOperation.CategoryId));

            modelBuilder.Entity<BudgetOperation>()
                .Property(b => b.Amount)
                .HasColumnName(nameof(BudgetOperation.Amount));

            modelBuilder.Entity<TransferOperation>()
                .Property(t => t.Amount)
                .HasColumnName(nameof(TransferOperation.Amount));

            modelBuilder.HasDefaultSchema("Pb");
        }
    }
}
