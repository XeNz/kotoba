using Kotoba.Core.Integrations;
using Kotoba.Core.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Kotoba.Core.DependencyInjection;

public class KotobaBuilder
{
    private readonly Type _translationIntegrationInterfaceType = typeof(ITranslationIntegration);
    private readonly Type _translationStorageInterfaceType = typeof(ITranslationStorage);


    private readonly IServiceCollection _serviceCollection;

    /// <summary>
    /// Collection of <see cref="ServiceDescriptor"/> being made during the entire <see cref="KotobaBuilder"/> process.
    /// </summary>
    /// <remarks>
    /// Use this in extension methods to add or remove registrations.
    /// </remarks>
    // ReSharper disable once MemberCanBePrivate.Global
    public ICollection<ServiceDescriptor> ServiceDescriptors { get; }

    internal KotobaBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
        ServiceDescriptors = new List<ServiceDescriptor>();
    }

    public KotobaBuilder AddJsonFileIntegration()
    {
        var implType = typeof(JsonFileIntegration);
        var descriptor = new ServiceDescriptor(
            _translationIntegrationInterfaceType,
            implType,
            ServiceLifetime.Singleton
        );
        ServiceDescriptors.Add(descriptor);

        return this;
    }

    public KotobaBuilder AddTolgeeIntegration(Action<TolgeeConfiguration> configurationAction)
    {
        var tolgeeConfiguration = new TolgeeConfiguration(string.Empty);
#pragma warning disable CA1062
        configurationAction.Invoke(tolgeeConfiguration);
#pragma warning restore CA1062
        tolgeeConfiguration.Validate();

        var configurationDescriptor = new ServiceDescriptor(typeof(TolgeeConfiguration), tolgeeConfiguration);
        ServiceDescriptors.Add(configurationDescriptor);

        var implType = typeof(TolgeeIntegration);
        var descriptor = new ServiceDescriptor(
            _translationIntegrationInterfaceType,
            implType,
            ServiceLifetime.Singleton
        );
        ServiceDescriptors.Add(descriptor);

        return this;
    }

    public KotobaBuilder AddInMemoryCacheStorage()
    {
        var implType = typeof(InMemoryCacheStorage);
        var descriptor = new ServiceDescriptor(
            _translationStorageInterfaceType,
            implType,
            ServiceLifetime.Singleton
        );
        ServiceDescriptors.Add(descriptor);

        return this;
    }


    public KotobaBuilder AddDistributedCacheStorage()
    {
        var implType = typeof(DistributedCacheStorage);
        var descriptor = new ServiceDescriptor(
            _translationStorageInterfaceType,
            implType,
            ServiceLifetime.Singleton
        );
        ServiceDescriptors.Add(descriptor);

        return this;
    }

    public IServiceCollection Build()
    {
        foreach (var serviceDescriptor in ServiceDescriptors)
        {
            _serviceCollection.Add(serviceDescriptor);
        }

        return _serviceCollection;
    }

}