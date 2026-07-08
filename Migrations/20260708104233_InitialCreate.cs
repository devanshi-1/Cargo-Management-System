using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cargo_Management_Project.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShipmentBookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ShipperName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConsigneeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OriginPort = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DestinationPort = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentBookings", x => x.BookingId);
                });

            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    ContainerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContainerNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ContainerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SealNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ContainerStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.ContainerId);
                    table.ForeignKey(
                        name: "FK_Containers_ShipmentBookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "ShipmentBookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Containers_BookingId",
                table: "Containers",
                column: "BookingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Containers");

            migrationBuilder.DropTable(
                name: "ShipmentBookings");
        }
    }
}
