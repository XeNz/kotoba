namespace Kotoba.Core.Behaviors;

//TODO determine how we are going to schedule this
// separate hangfire and quartz addon projects?
public sealed class CronBehavior : ITranslationRefreshBehavior
{
    public string Name => nameof(OnStartupOnlyBehavior);
}