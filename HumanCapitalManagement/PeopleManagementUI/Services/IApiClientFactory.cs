namespace PeopleManagementUI.Services
{
    public interface IApiClientFactory
    {
        HttpClient CreateAuthenticationClient();
        HttpClient CreateEmployeeClient();
    }
}
