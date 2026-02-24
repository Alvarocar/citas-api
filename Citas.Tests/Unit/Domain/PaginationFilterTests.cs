using Citas.Domain.Filters;

namespace Citas.Tests.Unit.Domain;

public class PaginationFilterTests
{
  [Fact]
  public void Page_WhenSetToValidValue_UsesValue()
  {
    var filter = new PaginationFilter { Page = 5 };
    Assert.Equal(5, filter.Page);
  }

  [Fact]
  public void Page_WhenSetToZero_DefaultsToOne()
  {
    var filter = new PaginationFilter { Page = 0 };
    Assert.Equal(1, filter.Page);
  }

  [Fact]
  public void Page_WhenSetToNegative_DefaultsToOne()
  {
    var filter = new PaginationFilter { Page = -10 };
    Assert.Equal(1, filter.Page);
  }

  [Fact]
  public void PageSize_WhenSetToValidValue_UsesValue()
  {
    var filter = new PaginationFilter { PageSize = 25 };
    Assert.Equal(25, filter.PageSize);
  }

  [Fact]
  public void PageSize_WhenSetBelowMinimum_ClampsToTen()
  {
    var filter = new PaginationFilter { PageSize = 5 };
    Assert.Equal(10, filter.PageSize);
  }

  [Fact]
  public void PageSize_WhenSetToZero_ClampsToTen()
  {
    var filter = new PaginationFilter { PageSize = 0 };
    Assert.Equal(10, filter.PageSize);
  }

  [Fact]
  public void PageSize_WhenSetAboveMaximum_ClampsToForty()
  {
    var filter = new PaginationFilter { PageSize = 100 };
    Assert.Equal(40, filter.PageSize);
  }

  [Fact]
  public void PageSize_WhenSetToExactBoundaries_UsesValue()
  {
    var filterMin = new PaginationFilter { PageSize = 10 };
    var filterMax = new PaginationFilter { PageSize = 40 };

    Assert.Equal(10, filterMin.PageSize);
    Assert.Equal(40, filterMax.PageSize);
  }
}
