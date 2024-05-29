using Carter;

namespace InventoryManagementSystem.API.Common;

public abstract class BaseEndpointModule : CarterModule
{
    public BaseEndpointModule() : base("api") { }

    public BaseEndpointModule(string endpoint) : base($"api/{endpoint}") { }
}