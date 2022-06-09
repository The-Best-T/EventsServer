using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class AddEventConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "CreaterId", "Date", "Description", "Name", "Place", "Speaker" },
                values: new object[] { new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), "", new DateTime(2022, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Birthday of Elena", "Birthday", "Hot bar", "Clown Anton" });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "CreaterId", "Date", "Description", "Name", "Place", "Speaker" },
                values: new object[] { new Guid("80abbca8-664d-4b20-b5de-024705497d4a"), "", new DateTime(2022, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Minsk olympiad in programming", "Olympiad in programming", "School 32", "Genadiy Andreevich" });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "CreaterId", "Date", "Description", "Name", "Place", "Speaker" },
                values: new object[] { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "", new DateTime(2022, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wedding of Maxim and Anna", "Wedding", "North Church", "Holy Father Peter" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("80abbca8-664d-4b20-b5de-024705497d4a"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "EventId",
                keyValue: new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"));
        }
    }
}
