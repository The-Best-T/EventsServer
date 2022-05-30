using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasData(
                new Event
                {
                    Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                    Name = "Wedding",
                    Description = "Wedding of Maxim and Anna",
                    Speaker = "Holy Father Peter",
                    Place = "North Church",
                    Date = new DateTime(2022, 7, 20)
                },
                new Event
                {
                    Id = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                    Name = "Birthday",
                    Description = "Birthday of Elena",
                    Speaker = "Clown Anton",
                    Place = "Hot bar",
                    Date = new DateTime(2022, 8, 29)
                },
                new Event
                {
                    Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                    Name = "Olympiad in programming",
                    Description = "Minsk olympiad in programming",
                    Speaker = "Genadiy Andreevich",
                    Place = "School 32",
                    Date = new DateTime(2022, 9, 21)
                }

                );
        }
    }
}
