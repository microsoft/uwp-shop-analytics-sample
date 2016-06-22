namespace ShopAnalyticsPCL.Models
{
    public class TransformedData
    {
        public int Hour { get; set; }
        public int Day { get; set; }
        public int Year { get; set; }
        public int Minute { get; set; }

        public int Month { get; set; }
        public int Value { get; set; }
    }
}