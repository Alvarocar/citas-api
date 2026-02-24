using Citas.Application.Dto;
using Citas.Application.Factories;
using Citas.Application.Services;
using Citas.Domain.Entities;
using Citas.Domain.Exceptions;
using Citas.Domain.Filters;
using Citas.Domain.Repositories;
using Citas.Tests.Helpers;
using Moq;

namespace Citas.Tests.Unit.Application.Services;

public class ServiceServiceTests
{
  private readonly Mock<IServiceRepository> _mockServiceRepo = new();
  private readonly Mock<ICompanyRepository> _mockCompanyRepo = new();
  private readonly Mock<IServiceFactory> _mockFactory = new();
  private readonly ServiceService _sut;

  private static readonly Company Company = TestEntities.MakeCompany(5, "Test Co", "co@test.com");
  private static readonly UserTokenDto UserToken = new() { Id = 1, CompanyId = 5, Role = Rol.Administrator, Email = "admin@test.com", FirstName = "Admin", LastName = "User" };

  private static Service MakeService(int id = 1) => new()
  {
    Id = id,
    Name = "Haircut",
    Description = "Basic haircut",
    SuggestedPrice = 15.0f,
    Company = Company
  };

  private static ServiceOverviewDto MakeOverview(int id = 1) => new()
  {
    Id = id,
    Name = "Haircut",
    Description = "Basic haircut",
    SuggestedPrice = 15.0f
  };

  public ServiceServiceTests()
  {
    _sut = new ServiceService(_mockServiceRepo.Object, _mockCompanyRepo.Object, _mockFactory.Object);
  }

  // --- Create ---

  [Fact]
  public async Task Create_WhenCompanyNotFound_ThrowsNotFoundException()
  {
    _mockCompanyRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Company?)null);

    await Assert.ThrowsAsync<NotFoundException>(() =>
        _sut.Create(new ServiceCreateDto { Name = "X", Description = "Y" }, UserToken, default));
  }

  [Fact]
  public async Task Create_WhenCompanyExists_AddsServiceAndReturnsOverview()
  {
    var dto = new ServiceCreateDto { Name = "Haircut", Description = "Basic haircut", SuggestedPrice = 15.0f };
    var service = MakeService();
    var overview = MakeOverview();

    _mockCompanyRepo.Setup(r => r.GetByIdAsync(UserToken.CompanyId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Company);
    _mockFactory.Setup(f => f.Create(dto, Company)).Returns(service);
    _mockFactory.Setup(f => f.CreateOverview(service)).Returns(overview);

    var result = await _sut.Create(dto, UserToken, default);

    _mockServiceRepo.Verify(r => r.Add(service), Times.Once);
    _mockServiceRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    Assert.Equal("Haircut", result.Name);
  }

  // --- GetAll ---

  [Fact]
  public async Task GetAll_ReturnsMappedListOfOverviews()
  {
    var services = new List<Service> { MakeService(1), MakeService(2) };
    var filter = new PaginationFilter();

    _mockServiceRepo.Setup(r => r.FindAllByCompany(UserToken.CompanyId, filter, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(services);
    _mockFactory.Setup(f => f.CreateOverview(It.IsAny<Service>()))
                .Returns<Service>(s => new ServiceOverviewDto { Id = s.Id, Name = s.Name, Description = s.Description, SuggestedPrice = s.SuggestedPrice });

    var result = await _sut.GetAll(UserToken, filter, default);

    Assert.Equal(2, result.Count);
  }

  // --- GetById ---

  [Fact]
  public async Task GetById_WhenServiceFound_ReturnsOverview()
  {
    var service = MakeService();
    var overview = MakeOverview();

    _mockServiceRepo.Setup(r => r.FindByIdAndCompany(1, UserToken.CompanyId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(service);
    _mockFactory.Setup(f => f.CreateOverview(service)).Returns(overview);

    var result = await _sut.GetById(1, UserToken, default);

    Assert.NotNull(result);
    Assert.Equal(1, result!.Id);
  }

  [Fact]
  public async Task GetById_WhenServiceNotFound_ReturnsNull()
  {
    _mockServiceRepo.Setup(r => r.FindByIdAndCompany(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Service?)null);

    var result = await _sut.GetById(99, UserToken, default);

    Assert.Null(result);
  }

  // --- Update ---

  [Fact]
  public async Task Update_WhenServiceNotFound_ThrowsNotFoundException()
  {
    _mockServiceRepo.Setup(r => r.FindByIdAndCompany(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Service?)null);

    await Assert.ThrowsAsync<NotFoundException>(() =>
        _sut.Update(99, new ServiceUpdateDto { Name = "X", Description = "Y" }, UserToken, default));
  }

  [Fact]
  public async Task Update_WhenFound_UpdatesFieldsAndSaves()
  {
    var service = MakeService();
    var dto = new ServiceUpdateDto { Name = "New Name", Description = "New Desc", SuggestedPrice = 30.0f, IsUnavailable = true };

    _mockServiceRepo.Setup(r => r.FindByIdAndCompany(1, UserToken.CompanyId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(service);
    _mockFactory.Setup(f => f.CreateOverview(service))
                .Returns(new ServiceOverviewDto { Id = 1, Name = dto.Name, Description = dto.Description, SuggestedPrice = dto.SuggestedPrice, IsUnavailable = true });

    var result = await _sut.Update(1, dto, UserToken, default);

    Assert.Equal("New Name", service.Name);
    Assert.Equal("New Desc", service.Description);
    Assert.Equal(30.0f, service.SuggestedPrice);
    Assert.True(service.IsUnavailable);
    _mockServiceRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
  }

  // --- Delete ---

  [Fact]
  public async Task Delete_WhenServiceNotFound_ThrowsNotFoundException()
  {
    _mockServiceRepo.Setup(r => r.FindByIdAndCompany(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Service?)null);

    await Assert.ThrowsAsync<NotFoundException>(() =>
        _sut.Delete(99, UserToken, default));
  }

  [Fact]
  public async Task Delete_WhenFound_DeletesAndSaves()
  {
    var service = MakeService();

    _mockServiceRepo.Setup(r => r.FindByIdAndCompany(1, UserToken.CompanyId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(service);

    await _sut.Delete(1, UserToken, default);

    _mockServiceRepo.Verify(r => r.Delete(service), Times.Once);
    _mockServiceRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
  }
}
