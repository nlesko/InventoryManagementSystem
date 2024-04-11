using Carter;

namespace InventoryManagementSystemApi.API.Common;

public abstract class BaseEndpointModule : CarterModule
{
    public BaseEndpointModule() : base("api") { }

    public BaseEndpointModule(string endpoint) : base($"api/{endpoint}") { }
}