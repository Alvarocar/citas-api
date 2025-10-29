namespace Citas.Domain.ValueObj
{
    public sealed record DateTimeRange
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        private DateTimeRange(DateTime start, DateTime end)
        {
            if (end <= start)
                throw new ArgumentException("End date must be after start date.");

            Start = start;
            End = end;
        }

        public static DateTimeRange Create(DateTime start, DateTime end)
            => new(start, end);

        public bool Overlaps(DateTimeRange other)
            => Start < other.End && End > other.Start;
    }
}
