using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MolineMart.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductEnhancements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DeliveryAvailability",
                columns: new[] { "Id", "EstimatedDays", "IsAvailable", "Pincode" },
                values: new object[,]
                {
                    { 1, 2, true, "110001" },
                    { 2, 3, true, "400001" },
                    { 3, null, false, "560001" }
                });

            migrationBuilder.InsertData(
                table: "ProductFeatures",
                columns: new[] { "Id", "FeatureDescription", "FeatureTitle", "ProductId" },
                values: new object[,]
                {
                    { 1, "A16 Bionic Chip", "Processor", 1 },
                    { 2, "6.1-inch Super Retina XDR Display", "Display", 1 }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "ImageUrl", "ProductId" },
                values: new object[] { 1, "/images/iphone14pro_front.jpg", 1 });

            migrationBuilder.InsertData(
                table: "ProductReviews",
                columns: new[] { "Id", "ProductId", "Rating", "ReviewDate", "ReviewText", "UserName" },
                values: new object[] { 1, 1, 5, new DateTime(2025, 11, 10, 16, 24, 42, 327, DateTimeKind.Utc).AddTicks(7950), "Great phone!", "Amit Sharma" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DeliveryAvailability",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DeliveryAvailability",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DeliveryAvailability",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductFeatures",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductFeatures",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductReviews",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
