using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MolineMart.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedValueDeliveryAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EstimatedDays",
                table: "DeliveryAvailability",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "DeliveryAvailability",
                keyColumn: "Id",
                keyValue: 3,
                column: "EstimatedDays",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 11, 28, 10, 752, DateTimeKind.Utc).AddTicks(4528));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 11, 28, 10, 752, DateTimeKind.Utc).AddTicks(4532));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 11, 28, 10, 752, DateTimeKind.Utc).AddTicks(4534));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 11, 28, 10, 752, DateTimeKind.Utc).AddTicks(4535));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 11, 28, 10, 752, DateTimeKind.Utc).AddTicks(4535));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 6,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 11, 28, 10, 752, DateTimeKind.Utc).AddTicks(4536));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EstimatedDays",
                table: "DeliveryAvailability",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "DeliveryAvailability",
                keyColumn: "Id",
                keyValue: 3,
                column: "EstimatedDays",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 10, 41, 43, 42, DateTimeKind.Utc).AddTicks(2328));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 10, 41, 43, 42, DateTimeKind.Utc).AddTicks(2335));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 10, 41, 43, 42, DateTimeKind.Utc).AddTicks(2336));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 10, 41, 43, 42, DateTimeKind.Utc).AddTicks(2339));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 10, 41, 43, 42, DateTimeKind.Utc).AddTicks(2340));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 6,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 11, 10, 41, 43, 42, DateTimeKind.Utc).AddTicks(2341));
        }
    }
}
