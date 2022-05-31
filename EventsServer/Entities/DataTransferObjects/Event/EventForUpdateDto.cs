namespace Entities.DataTransferObjects.Event
{
    public class EventForUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Speaker { get; set; }
        public string Place { get; set; }
        public DateTime Date { get; set; }
    }
}
