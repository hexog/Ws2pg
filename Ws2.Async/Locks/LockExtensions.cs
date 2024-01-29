﻿using JetBrains.Annotations;
using Ws2.EqualityComparison;

namespace Ws2.Async.Locks;

public static class LockExtensions
{
    [MustUseReturnValue]
    public static ValueTask<ILockHolder> AcquireAsync(
        this ILock @lock,
        byte[] bytes,
        CancellationToken cancellationToken
    )
    {
        return @lock.AcquireAsync(bytes, EqualityComparers.ByteArrayEqualityComparer, cancellationToken);
    }

    [MustUseReturnValue]
    public static ValueTask<ILockHolder> AcquireAsync(
        this ILock @lock,
        byte[] bytes,
        TimeSpan timeout
    )
    {
        return @lock.AcquireAsync(bytes, EqualityComparers.ByteArrayEqualityComparer, timeout);
    }

    [MustUseReturnValue]
    public static ValueTask<ILockHolder> AcquireAsync(
        this ILock @lock,
        string key,
        CancellationToken cancellationToken
    )
    {
        return @lock.AcquireAsync(key, StringComparer.Ordinal, cancellationToken);
    }

    [MustUseReturnValue]
    public static ValueTask<ILockHolder> AcquireAsync(
        this ILock @lock,
        string key,
        TimeSpan timeout
    )
    {
        return @lock.AcquireAsync(key, StringComparer.Ordinal, timeout);
    }

    [MustUseReturnValue]
    public static ValueTask<ILockHolder> AcquireAsync<TKey>(
        this ILock @lock,
        TKey key,
        CancellationToken cancellationToken
    ) where TKey : notnull
    {
        return @lock.AcquireAsync(key, EqualityComparer<TKey>.Default, cancellationToken);
    }

    [MustUseReturnValue]
    public static ValueTask<ILockHolder> AcquireAsync<TKey>(
        this ILock @lock,
        TKey key,
        TimeSpan timeout
    ) where TKey : notnull
    {
        return @lock.AcquireAsync(key, EqualityComparer<TKey>.Default, timeout);
    }
}