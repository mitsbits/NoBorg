using Borg.Infra;
using Borg.MVC.BuildingBlocks.Contracts;
using Borg.MVC.Exceptions;
using System;
using System.ComponentModel;

namespace Borg
{
    public static class DeviceExtensions
    {
        public static string RelativePath(this IDevice device)
        {
            Preconditions.NotNull(device, nameof(device));
            return $"{device.Path}{device.QueryString}";
        }

        public static T RouteValue<T>(this IHaveAController device, string key)
        {
            Preconditions.NotEmpty(key, nameof(key));
            if (device.RouteValues.ContainsKey(key))
            {
                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    return (T)converter.ConvertFromInvariantString(device.RouteValues[key][0]);
                }
                catch (Exception e)
                {
                    throw new BorgApplicationException($"Can not convert {device.RouteValues[key]} to type {typeof(T).Name}");
                }
            }
            throw new BorgApplicationException($"Can not find route value {key}");
        }
    }
}