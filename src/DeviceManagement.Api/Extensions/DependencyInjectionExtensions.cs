using DeviceManagement.Domain.Repositories;
using DeviceManagement.Infrastructure.DataAccess;
using DeviceManagement.Infrastructure.DataAccess.Repositories;
using DeviceManagement.Infrastructure.Services;
using DeviceManagement.Application.Services.Auth;
using DeviceManagement.Application.UseCases.Customer.CreateCustomer;
using DeviceManagement.Application.UseCases.Customer.GetCustomer;
using DeviceManagement.Application.UseCases.Customer.UpdateCustomer;
using DeviceManagement.Application.UseCases.Customer.DeleteCustomer;
using DeviceManagement.Application.UseCases.Device.CreateDevice;
using DeviceManagement.Application.UseCases.Device.GetDevice;
using DeviceManagement.Application.UseCases.Event.CreateEvent;
using DeviceManagement.Application.UseCases.Event.GetEvent;
using DeviceManagement.Application.UseCases.Dashboard.GetDashboard;
using DeviceManagement.Application.UseCases.Auth.Login;

namespace DeviceManagement.Api.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<ICreateCustomerUseCase, CreateCustomerUseCase>();
        services.AddScoped<IGetCustomerUseCase, GetCustomerUseCase>();
        services.AddScoped<IUpdateCustomerUseCase, UpdateCustomerUseCase>();
        services.AddScoped<IDeleteCustomerUseCase, DeleteCustomerUseCase>();

        services.AddScoped<ICreateDeviceUseCase, CreateDeviceUseCase>();
        services.AddScoped<IGetDeviceUseCase, GetDeviceUseCase>();

        services.AddScoped<ICreateEventUseCase, CreateEventUseCase>();
        services.AddScoped<IGetEventUseCase, GetEventUseCase>();

        services.AddScoped<IGetDashboardUseCase, GetDashboardUseCase>();

        services.AddScoped<ILoginUseCase, LoginUseCase>();

        return services;
    }
}
