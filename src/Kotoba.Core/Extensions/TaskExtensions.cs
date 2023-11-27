using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;

namespace Kotoba.Core.Extensions;

internal static class TaskExtensions
{
    /// <summary>
    /// Enumerates a collection in parallel and calls an async method on each item. Useful for making 
    /// parallel async calls, e.g. independent web requests when the degree of parallelism needs to be
    /// limited.
    /// </summary>
    internal static async Task WhenAllPartitioned<T>(this IEnumerable<T> source, int partitionCount, Func<T, Task> func)
    {
        var partitionedTasks = Partitioner.Create(source)
            .GetPartitions(partitionCount)
            .Select(partition => Task.Run(async () =>
            {
                using (partition)
                    while (partition.MoveNext())
                        await func(partition.Current).ConfigureAwait(false);
            }));

        var allTasks = Task.WhenAll(partitionedTasks);

        await ExecuteWithExceptionHandling<T>(allTasks).ConfigureAwait(false);
    }

    private static async Task ExecuteWithExceptionHandling<T>(Task allTasks)
    {
        try
        {
            await allTasks.ConfigureAwait(false);
        }
        catch
        {
            if (allTasks.Exception != null) ExceptionDispatchInfo.Capture(allTasks.Exception).Throw();
            throw;
        }
    }
}