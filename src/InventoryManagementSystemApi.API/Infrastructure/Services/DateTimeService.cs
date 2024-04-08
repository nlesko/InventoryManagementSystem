using InventoryManagementSystemApi.API.Common.Interfaces;

namespace InventoryManagementSystemApi.API.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}