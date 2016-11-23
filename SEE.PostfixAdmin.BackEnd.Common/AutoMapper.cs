using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Common
{
    public static class AutoMapper
    {
        public static object Map(object source, object destination)
        {
            Type sType = source.GetType();
            Type dType = destination.GetType();
            var dProperties = dType.GetProperties()
                .Where(x => x.CanWrite)
                .Where(x => !x.PropertyType.GetInterfaces().Any(y => y == typeof(ICollection)))
                .ToList();
            var sProperties = sType.GetProperties()
                .Where(x => dProperties.Any(y => y.Name == x.Name))
                .ToList();
            foreach (var s in sProperties)
            {
                var d = dProperties.FirstOrDefault(x => x.Name == s.Name);
                if (d != null && d.PropertyType.FullName == s.PropertyType.FullName)
                {
                    if (s.PropertyType.GetTypeInfo().IsValueType || s.PropertyType == typeof(string))
                    {
                        d.SetValue(destination, s.GetValue(source));
                    }
                    else if (s.PropertyType.GetTypeInfo().IsClass)
                    {
                        var sVal = s.GetValue(source);
                        var dVal = d.GetValue(destination);
                        if (sVal != null)
                        {
                            if (dVal == null)
                            {
                                dVal = Activator.CreateInstance(d.PropertyType);
                            }
                            d.SetValue(destination, Map(sVal, dVal));
                        }
                    }
                }
            }
            return destination;
        }
    }

    public static class AutoMapper<TSource, TDestination>
    {
        public static void Map(TSource source, TDestination destination)
        {
            AutoMapper.Map(source, destination);
        }

        public static void Map(TSource source, TDestination destination, Action<TSource, TDestination> action)
        {
            AutoMapper.Map(source, destination);
            if (action != null)
            {
                action(source, destination);
            }
        }

    }
}
