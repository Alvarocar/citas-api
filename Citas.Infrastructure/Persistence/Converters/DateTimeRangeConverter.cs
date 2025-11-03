using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Citas.Domain.ValueObj;
using NpgsqlTypes;

namespace Citas.Infrastructure.Persistence.Converters;
internal class DateTimeRangeConverter : ValueConverter<DateTimeRange, NpgsqlRange<DateTime>>
{
    public DateTimeRangeConverter()
    : base(
        range => new NpgsqlRange<DateTime>(range.Start, range.End),
        npgsqlRange => DateTimeRange.Create(npgsqlRange.LowerBound, npgsqlRange.UpperBound)
      )
    { }

}

