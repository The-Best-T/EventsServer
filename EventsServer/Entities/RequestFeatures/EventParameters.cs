namespace Entities.RequestFeatures
{
    public class EventParameters : RequestParameters
    {
        public DateTime MinDate { get; set; } = DateTime.MinValue;
        public DateTime MaxDate { get; set; } = DateTime.MaxValue;
        public bool ValidDateRange => MaxDate > MinDate;
    }

}
