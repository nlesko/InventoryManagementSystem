using InventoryManagementSystem.API.Common.Interfaces;

namespace InventoryManagementSystem.API.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}