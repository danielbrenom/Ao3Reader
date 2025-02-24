﻿using System;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Ao3Reader.Extensions
{
    public static class TypeExtensions
    {
        public static T getAs<T>(this object obj, bool useMapper = true,
            T defaultObject = default, bool allowExceptions = false)
        {
            try
            {
                if (obj is null)
                    return defaultObject;
                if (obj.GetType() == typeof(T))
                    return (T) obj;
                if (!useMapper) return (T) obj;
                var mapper = Startup.ServiceProvider.GetService<IMapper>();
                return mapper.Map<T>(obj);
            }
            catch (Exception)
            {
                if (allowExceptions)
                    throw;
                if (typeof(T) == obj?.GetType() || typeof(T) == obj?.GetType().GetTypeInfo().BaseType)
                    return (T) obj;
                return defaultObject;
            }
        }
    }
}