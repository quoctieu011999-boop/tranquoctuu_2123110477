using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tranquoctuu_2123110477.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Channel",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Channel",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "LoyaltyTransactions");

            migrationBuilder.DropColumn(
                name: "MinPointsToRedeem",
                table: "LoyaltyRules");

            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Rewards",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "PointsRequired",
                table: "Rewards",
                newName: "PointCost");

            migrationBuilder.RenameColumn(
                name: "PointsPerAmount",
                table: "LoyaltyRules",
                newName: "Points");

            migrationBuilder.RenameColumn(
                name: "TotalPoints",
                table: "LoyaltyAccounts",
                newName: "Points");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Rewards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Rewards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Rewards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Rewards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Rewards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Rewards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Rewards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Redemptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Redemptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Redemptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Redemptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Redemptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Redemptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Redemptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Payments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "OrderItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "OrderItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "OrderItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifications",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Notifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Notifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LoyaltyTransactions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "LoyaltyTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "LoyaltyTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "LoyaltyTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LoyaltyTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LoyaltyTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "LoyaltyTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "LoyaltyTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LoyaltyRules",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "ActionType",
                table: "LoyaltyRules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "LoyaltyRules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "LoyaltyRules",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "LoyaltyRules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LoyaltyRules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LoyaltyRules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "LoyaltyRules",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "LoyaltyRules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "LoyaltyAccounts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "LoyaltyAccounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "LoyaltyAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "LoyaltyAccounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "LoyaltyAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LoyaltyAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "LoyaltyAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "LoyaltyAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Customers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CustomerInteractions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CustomerInteractions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "CustomerInteractions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "CustomerInteractions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CustomerInteractions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CustomerInteractions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CustomerInteractions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CustomerChannels",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CustomerChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "CustomerChannels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "CustomerChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CustomerChannels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CustomerChannels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CustomerChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Redemptions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Redemptions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Redemptions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Redemptions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Redemptions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Redemptions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Redemptions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "LoyaltyTransactions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "LoyaltyTransactions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "LoyaltyTransactions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LoyaltyTransactions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LoyaltyTransactions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "LoyaltyTransactions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "LoyaltyTransactions");

            migrationBuilder.DropColumn(
                name: "ActionType",
                table: "LoyaltyRules");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "LoyaltyRules");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "LoyaltyRules");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "LoyaltyRules");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LoyaltyRules");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LoyaltyRules");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "LoyaltyRules");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "LoyaltyRules");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "LoyaltyAccounts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "LoyaltyAccounts");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "LoyaltyAccounts");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "LoyaltyAccounts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LoyaltyAccounts");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "LoyaltyAccounts");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "LoyaltyAccounts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CustomerInteractions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CustomerInteractions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CustomerInteractions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CustomerInteractions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CustomerInteractions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CustomerInteractions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CustomerChannels");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CustomerChannels");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CustomerChannels");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CustomerChannels");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CustomerChannels");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CustomerChannels");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Rewards",
                newName: "Stock");

            migrationBuilder.RenameColumn(
                name: "PointCost",
                table: "Rewards",
                newName: "PointsRequired");

            migrationBuilder.RenameColumn(
                name: "Points",
                table: "LoyaltyRules",
                newName: "PointsPerAmount");

            migrationBuilder.RenameColumn(
                name: "Points",
                table: "LoyaltyAccounts",
                newName: "TotalPoints");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Channel",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Channel",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LoyaltyTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReferenceId",
                table: "LoyaltyTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LoyaltyRules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinPointsToRedeem",
                table: "LoyaltyRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "LoyaltyAccounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CustomerInteractions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CustomerChannels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
