using Core.Persistence;
using Game.Infrastructure.Controllers;
using Game.Model.Services.Authentication;
using Game.Model.Services.Connection;
using VContainer;
using VContainer.Unity;

namespace Game.Infrastructure.Scopes
{
    public class ControllerScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<ConnectionController>();
            builder.Register<PopupsController>(Lifetime.Singleton);
            builder.Register<AuthenticationServices>(Lifetime.Singleton);
            builder.Register<ClientFactory>(Lifetime.Singleton);
            builder.Register<SessionTokenRefresher>(Lifetime.Singleton);
            builder.Register<DeviceAuthentication>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<EmailAuthentication>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<GoogleAuthentication>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<FileStorageService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<JsonSerializer>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }
    }
}