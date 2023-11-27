using Microsoft.Extensions.DependencyInjection;

namespace Kotoba.Core.DependencyInjection;

public static class KotobaBuilderExtensions
{
    public static KotobaBuilder AddKotoba(this IServiceCollection serviceCollection) =>
        new(serviceCollection);
}