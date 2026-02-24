using Citas.Domain.ValueObj;

namespace Citas.Tests.Unit.Domain;

public class DateTimeRangeTests
{
  [Fact]
  public void Create_WithValidRange_ReturnsInstance()
  {
    var start = new DateTime(2025, 1, 1, 9, 0, 0);
    var end = new DateTime(2025, 1, 1, 10, 0, 0);

    var range = DateTimeRange.Create(start, end);

    Assert.Equal(start, range.Start);
    Assert.Equal(end, range.End);
  }

  [Fact]
  public void Create_WithEndBeforeStart_ThrowsArgumentException()
  {
    var start = new DateTime(2025, 1, 1, 10, 0, 0);
    var end = new DateTime(2025, 1, 1, 9, 0, 0);

    Assert.Throws<ArgumentException>(() => DateTimeRange.Create(start, end));
  }

  [Fact]
  public void Create_WithEndEqualToStart_ThrowsArgumentException()
  {
    var moment = new DateTime(2025, 1, 1, 9, 0, 0);

    Assert.Throws<ArgumentException>(() => DateTimeRange.Create(moment, moment));
  }

  [Fact]
  public void Overlaps_WithFullyOverlappingRange_ReturnsTrue()
  {
    var a = DateTimeRange.Create(new DateTime(2025, 1, 1, 9, 0, 0), new DateTime(2025, 1, 1, 11, 0, 0));
    var b = DateTimeRange.Create(new DateTime(2025, 1, 1, 10, 0, 0), new DateTime(2025, 1, 1, 12, 0, 0));

    Assert.True(a.Overlaps(b));
    Assert.True(b.Overlaps(a));
  }

  [Fact]
  public void Overlaps_WithContainedRange_ReturnsTrue()
  {
    var outer = DateTimeRange.Create(new DateTime(2025, 1, 1, 8, 0, 0), new DateTime(2025, 1, 1, 12, 0, 0));
    var inner = DateTimeRange.Create(new DateTime(2025, 1, 1, 9, 0, 0), new DateTime(2025, 1, 1, 11, 0, 0));

    Assert.True(outer.Overlaps(inner));
    Assert.True(inner.Overlaps(outer));
  }

  [Fact]
  public void Overlaps_WithNonOverlappingRange_ReturnsFalse()
  {
    var a = DateTimeRange.Create(new DateTime(2025, 1, 1, 8, 0, 0), new DateTime(2025, 1, 1, 9, 0, 0));
    var b = DateTimeRange.Create(new DateTime(2025, 1, 1, 10, 0, 0), new DateTime(2025, 1, 1, 11, 0, 0));

    Assert.False(a.Overlaps(b));
    Assert.False(b.Overlaps(a));
  }

  [Fact]
  public void Overlaps_WithAdjacentRange_ReturnsFalse()
  {
    var a = DateTimeRange.Create(new DateTime(2025, 1, 1, 8, 0, 0), new DateTime(2025, 1, 1, 9, 0, 0));
    var b = DateTimeRange.Create(new DateTime(2025, 1, 1, 9, 0, 0), new DateTime(2025, 1, 1, 10, 0, 0));

    Assert.False(a.Overlaps(b));
    Assert.False(b.Overlaps(a));
  }
}
