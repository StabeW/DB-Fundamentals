﻿using System.Reflection;
using System.Collections;

namespace AutoMapper
{
    public class Mapper
    {
        public TDest CreateMappedObject<TDest>(object source)
        {
            if (source == null)
            {
                throw new ArgumentException(ExceptionUtils.NullSource);
            }
            var dest = Activator.CreateInstance(typeof(TDest));

            return (TDest)MapObject(source, dest);
        }

        private object MapObject(object source, object dest)
        {
            foreach (var destProp in dest.GetType()
               .GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Where(p => p.CanWrite))
            {
                var sourceProp = source.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.Name == destProp.Name)
                    .FirstOrDefault();

                if (sourceProp != null)
                {
                    var sourceValue = sourceProp.GetMethod.Invoke(source, []) 
                        ?? throw new ArgumentException(ExceptionUtils.NullableSourceValueGetMethod);
                    if (ReflectionUtils.IsPrimitive(sourceValue.GetType()))
                    {
                        destProp.SetValue(dest, sourceProp.GetValue(source, null), null);
                        continue;
                    }

                    if (ReflectionUtils.IsGenericCollection(sourceValue.GetType()))
                    {

                        if (ReflectionUtils.IsPrimitive(sourceValue.GetType().GetGenericArguments()[0]))
                        {
                            var destinationCollection = sourceValue;
                            destProp.SetMethod.Invoke(dest, [destinationCollection]);
                        }
                        else
                        {
                            var destColl = destProp.GetMethod.Invoke(dest, []);
                            var destType = destColl.GetType().GetGenericArguments()[0];
                            foreach (var destP in (IEnumerable)sourceValue)
                            {
                                ((IList)destColl).Add(this.CreateMappedObject(destP, destType));
                            }
                        }
                    }
                    else if (ReflectionUtils.IsNonGenericCollection(sourceValue.GetType()))
                    {

                        var destColl = (IList)Activator.CreateInstance(destProp.PropertyType,
                            [((object[])sourceValue).Length]);
                        for (int i = 0; i < ((object[])sourceValue).Length; i++)
                        {
                            destColl[i] = CreateMappedObject(((object[])sourceValue)[i], destProp.PropertyType.GetElementType());
                        }
                        destProp.SetValue(dest, destColl);
                    }
                    else
                    {
                        destProp.SetValue(dest, this.CreateMappedObject(sourceProp.GetValue(source), destProp.PropertyType));
                    }
                }


            }
            return dest;
        }


        private object CreateMappedObject(object source, Type ret)
        {
            if (source == null || ret == null)
            {
                throw new ArgumentException(ExceptionUtils.NullSourceValueOrReturnType);
            }

            var dest = Activator.CreateInstance(ret);

            return MapObject(source, dest);
        }

    }
}
