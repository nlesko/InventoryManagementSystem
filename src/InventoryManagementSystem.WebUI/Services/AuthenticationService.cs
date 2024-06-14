using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Blazored.LocalStorage;

using InventoryManagementSystem.WebUI.Entities.DTO;
using InventoryManagementSystem.WebUI.Util;

using Microsoft.AspNetCore.Components.Authorization;

namespace InventoryManagementSystem.WebUI.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(HttpClient client, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _client = client;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<RegistrationResponseDto> RegisterUser(UserForRegistrationDto userForRegistration)
    {
        var content = JsonSerializer.Serialize(userForRegistration);
        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

        var registrationResult = await _client.PostAsync("accounts/register", bodyContent);
        var registrationContent = await registrationResult.Content.ReadAsStringAsync();

        if (!registrationResult.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<RegistrationResponseDto>(registrationContent, _options);
            return result;
        }

        return new RegistrationResponseDto { IsSuccessfulRegistration = true };
    }

    public async Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication)
    {
        var content = JsonSerializer.Serialize(userForAuthentication);
        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
        var authResult = await _client.PostAsync("accounts/login", bodyContent);
        var authContent = await authResult.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<AuthResponseDto>(authContent, _options);
        if (!authResult.IsSuccessStatusCode)
            return result;
        await _localStorage.SetItemAsync("authToken", result.Token);
        ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(userForAuthentication.Email);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
        return new AuthResponseDto { IsAuthSuccessful = true };
    }
    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
        _client.DefaultRequestHeaders.Authorization = null;
    }
}