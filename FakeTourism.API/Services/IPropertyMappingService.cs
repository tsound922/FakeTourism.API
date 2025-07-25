﻿using System.Collections.Generic;

namespace FakeTourism.API.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool IsMappingKeyWordExist<TSource, TDestination>(string fields);
        bool IsPropertiesExist<T>(string fields);
    }
}