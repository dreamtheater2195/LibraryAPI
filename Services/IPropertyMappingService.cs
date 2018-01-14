using System.Collections.Generic;

namespace LibraryAPI.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();

        bool ValidMappingFor<TSource, TDestination>(string fields);
    }
}