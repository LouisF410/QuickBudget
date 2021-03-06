// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuickBudget.Persistance;

#nullable disable

namespace QuickBudget.API.Migrations
{
    [DbContext(typeof(QuickBudgetDbContext))]
    [Migration("20220616051954_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BudgetPeriodTransactions", b =>
                {
                    b.Property<int>("BudgetPeriodsId")
                        .HasColumnType("int");

                    b.Property<int>("TransactionsId")
                        .HasColumnType("int");

                    b.HasKey("BudgetPeriodsId", "TransactionsId");

                    b.HasIndex("TransactionsId");

                    b.ToTable("BudgetPeriodTransactions");
                });

            modelBuilder.Entity("QuickBudget.Domain.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ApplicationUserId")
                        .HasColumnType("int");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("AccountId");

                    b.ToTable("Accounts", (string)null);
                });

            modelBuilder.Entity("QuickBudget.Domain.Entities.Budget", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(8, 2)
                        .HasColumnType("decimal(8,2)")
                        .HasDefaultValue(0m);

                    b.Property<int>("BudgetPeriodId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("BudgetId");

                    b.HasIndex("BudgetPeriodId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Budgets", (string)null);
                });

            modelBuilder.Entity("QuickBudget.Domain.Entities.BudgetPeriod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Period")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id")
                        .HasName("BudgetPeriodId");

                    b.ToTable("BudgetPeriods", (string)null);
                });

            modelBuilder.Entity("QuickBudget.Domain.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("CategoryId");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("QuickBudget.Domain.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasPrecision(8, 2)
                        .HasColumnType("decimal(8,2)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("TransactionDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id")
                        .HasName("TransactionId");

                    b.HasIndex("AccountId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Transactions", (string)null);
                });

            modelBuilder.Entity("BudgetPeriodTransactions", b =>
                {
                    b.HasOne("QuickBudget.Domain.Entities.BudgetPeriod", null)
                        .WithMany()
                        .HasForeignKey("BudgetPeriodsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuickBudget.Domain.Entities.Transaction", null)
                        .WithMany()
                        .HasForeignKey("TransactionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QuickBudget.Domain.Entities.Account", b =>
                {
                    b.OwnsOne("QuickBudget.Domain.ValueObjects.Bank", "Bank", b1 =>
                        {
                            b1.Property<int>("AccountId")
                                .HasColumnType("int");

                            b1.Property<int>("BankId")
                                .HasColumnType("int")
                                .HasColumnName("BankId");

                            b1.Property<string>("BankName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("BankName");

                            b1.HasKey("AccountId");

                            b1.ToTable("Accounts");

                            b1.WithOwner()
                                .HasForeignKey("AccountId");
                        });

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("QuickBudget.Domain.Entities.Budget", b =>
                {
                    b.HasOne("QuickBudget.Domain.Entities.BudgetPeriod", "BudgetPeriod")
                        .WithMany("Budgets")
                        .HasForeignKey("BudgetPeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuickBudget.Domain.Entities.Category", "Category")
                        .WithMany("Budgets")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BudgetPeriod");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("QuickBudget.Domain.Entities.Transaction", b =>
                {
                    b.HasOne("QuickBudget.Domain.Entities.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuickBudget.Domain.Entities.Category", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("QuickBudget.Domain.Entities.Account", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("QuickBudget.Domain.Entities.BudgetPeriod", b =>
                {
                    b.Navigation("Budgets");
                });

            modelBuilder.Entity("QuickBudget.Domain.Entities.Category", b =>
                {
                    b.Navigation("Budgets");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
