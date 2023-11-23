﻿using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ws2.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceAttributeServiceCollectionExtensions
{
	public static IServiceCollection AddServicesByAttributes(this IServiceCollection serviceCollection,
		params Assembly[] assembliesToAdd)
	{
		var types = assembliesToAdd
			.SelectMany(x => x.DefinedTypes);

		foreach (var type in types)
		{
			var serviceAttributes =
				type.GetCustomAttributes(typeof(ServiceAttribute)).Cast<ServiceAttribute>().ToArray();

			if (serviceAttributes.Length == 0)
			{
				continue;
			}

			var firstServiceAttribute = serviceAttributes[0];
			var lifetime = firstServiceAttribute.Lifetime;
			Debug.Assert(Enum.IsDefined(lifetime));

			if (serviceAttributes.Any(x => x.Lifetime != lifetime))
			{
				continue;
			}

			if (lifetime == ServiceLifetime.Singleton)
			{
				AddSingletonService(serviceCollection, type, serviceAttributes);
			}
			else
			{
				foreach (var serviceAttribute in serviceAttributes)
				{
					var service = serviceAttribute.Service ?? type;
					serviceCollection.TryAdd(new ServiceDescriptor(service, type, lifetime));
				}
			}
		}

		return serviceCollection;
	}

	private static void AddSingletonService(IServiceCollection serviceCollection, TypeInfo type,
		ServiceAttribute[] serviceAttributes)
	{
		serviceCollection.TryAdd(new ServiceDescriptor(type, type, ServiceLifetime.Singleton));
		foreach (var serviceAttribute in serviceAttributes)
		{
			if (serviceAttribute.Service is not null)
			{
				serviceCollection.TryAddEnumerable(new ServiceDescriptor(serviceAttribute.Service,
					p => p.GetRequiredService(type), ServiceLifetime.Singleton));
			}
		}
	}
}