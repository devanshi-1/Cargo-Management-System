using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cargo_Management_Project.Migrations
{
    /// <inheritdoc />
    public partial class InitialLogisticsSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CargoEvent_Containers_ContainerId",
                table: "CargoEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CargoEvent",
                table: "CargoEvent");

            migrationBuilder.RenameTable(
                name: "CargoEvent",
                newName: "CargoEvents");

            migrationBuilder.RenameIndex(
                name: "IX_CargoEvent_ContainerId",
                table: "CargoEvents",
                newName: "IX_CargoEvents_ContainerId");

            migrationBuilder.AlterColumn<int>(
                name: "BookingStatus",
                table: "ShipmentBookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SealNumber",
                table: "Containers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<int>(
                name: "ContainerType",
                table: "Containers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ContainerStatus",
                table: "Containers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContainerNumber",
                table: "Containers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "EventType",
                table: "CargoEvents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CargoEvents",
                table: "CargoEvents",
                column: "EventId");

            migrationBuilder.CreateTable(
                name: "CustomsDeclarations",
                columns: table => new
                {
                    DeclarationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    HsCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeclaredValue = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    CalculatedDuty = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    ClearanceStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomsDeclarations", x => x.DeclarationId);
                    table.ForeignKey(
                        name: "FK_CustomsDeclarations_ShipmentBookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "ShipmentBookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FreightInvoices",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    FreightCharges = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    SurchargeAmount = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreightInvoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_FreightInvoices_ShipmentBookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "ShipmentBookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ShipmentBookings",
                columns: new[] { "BookingId", "BookingNumber", "BookingStatus", "ConsigneeName", "DestinationPort", "OriginPort", "ShipperName" },
                values: new object[] { 1, "BKG-2026-001", 3, "Euro Imports Ltd", "Rotterdam", "Shanghai", "Global Tech Exports" });

            migrationBuilder.InsertData(
                table: "Containers",
                columns: new[] { "ContainerId", "BookingId", "ContainerNumber", "ContainerStatus", "ContainerType", "SealNumber" },
                values: new object[] { 1, 1, "CONT-998877", 1, 0, "SEAL-XYZ123" });

            migrationBuilder.InsertData(
                table: "CustomsDeclarations",
                columns: new[] { "DeclarationId", "BookingId", "CalculatedDuty", "ClearanceStatus", "DeclaredValue", "HsCode" },
                values: new object[] { 1, 1, 2500.00m, 2, 50000.00m, "8471.30.00" });

            migrationBuilder.InsertData(
                table: "FreightInvoices",
                columns: new[] { "InvoiceId", "BookingId", "Currency", "FreightCharges", "InvoiceStatus", "SurchargeAmount", "TotalAmount" },
                values: new object[] { 1, 1, "USD", 3500.00m, 2, 500.00m, 4000.00m });

            migrationBuilder.InsertData(
                table: "CargoEvents",
                columns: new[] { "EventId", "ContainerId", "EventLocation", "EventTimestamp", "EventType", "Remarks" },
                values: new object[] { 1, 1, "Port of Shanghai", new DateTime(2026, 7, 10, 8, 30, 0, 0, DateTimeKind.Unspecified), 1, "Loaded securely on vessel." });

            migrationBuilder.CreateIndex(
                name: "IX_CustomsDeclarations_BookingId",
                table: "CustomsDeclarations",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_FreightInvoices_BookingId",
                table: "FreightInvoices",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_CargoEvents_Containers_ContainerId",
                table: "CargoEvents",
                column: "ContainerId",
                principalTable: "Containers",
                principalColumn: "ContainerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CargoEvents_Containers_ContainerId",
                table: "CargoEvents");

            migrationBuilder.DropTable(
                name: "CustomsDeclarations");

            migrationBuilder.DropTable(
                name: "FreightInvoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CargoEvents",
                table: "CargoEvents");

            migrationBuilder.DeleteData(
                table: "CargoEvents",
                keyColumn: "EventId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Containers",
                keyColumn: "ContainerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ShipmentBookings",
                keyColumn: "BookingId",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "CargoEvents",
                newName: "CargoEvent");

            migrationBuilder.RenameIndex(
                name: "IX_CargoEvents_ContainerId",
                table: "CargoEvent",
                newName: "IX_CargoEvent_ContainerId");

            migrationBuilder.AlterColumn<string>(
                name: "BookingStatus",
                table: "ShipmentBookings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SealNumber",
                table: "Containers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContainerType",
                table: "Containers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ContainerStatus",
                table: "Containers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ContainerNumber",
                table: "Containers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EventType",
                table: "CargoEvent",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CargoEvent",
                table: "CargoEvent",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_CargoEvent_Containers_ContainerId",
                table: "CargoEvent",
                column: "ContainerId",
                principalTable: "Containers",
                principalColumn: "ContainerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
