namespace Kotoba.Core.Behaviors;

public sealed class OnStartupOnlyBehavior : ITranslationRefreshBehavior
{
    public string Name => nameof(OnStartupOnlyBehavior);
}