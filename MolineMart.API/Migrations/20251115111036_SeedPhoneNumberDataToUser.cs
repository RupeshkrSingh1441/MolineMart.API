using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MolineMart.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedPhoneNumberDataToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 15, 11, 10, 36, 311, DateTimeKind.Utc).AddTicks(3351));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 15, 11, 10, 36, 311, DateTimeKind.Utc).AddTicks(3364));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 15, 11, 10, 36, 311, DateTimeKind.Utc).AddTicks(3366));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 15, 11, 10, 36, 311, DateTimeKind.Utc).AddTicks(3367));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 15, 11, 10, 36, 311, DateTimeKind.Utc).AddTicks(3368));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 6,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 15, 11, 10, 36, 311, DateTimeKind.Utc).AddTicks(3369));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 15, 11, 7, 52, 765, DateTimeKind.Utc).AddTicks(553));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 15, 11, 7, 52, 765, DateTimeKind.Utc).AddTicks(561));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 15, 11, 7, 52, 765, DateTimeKind.Utc).AddTicks(562));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 15, 11, 7, 52, 765, DateTimeKind.Utc).AddTicks(563));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 15, 11, 7, 52, 765, DateTimeKind.Utc).AddTicks(564));

            migrationBuilder.UpdateData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 6,
                column: "ReviewDate",
                value: new DateTime(2025, 11, 15, 11, 7, 52, 765, DateTimeKind.Utc).AddTicks(565));
        }
    }
}
