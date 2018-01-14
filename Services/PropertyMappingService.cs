using System;
using System.Collections.Generic;
using System.Linq;
using LibraryAPI.Entities;
using LibraryAPI.Models;

namespace LibraryAPI.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();
        private Dictionary<string, PropertyMappingValue> _authorPropertyMappings =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase) {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
                {"Genre", new PropertyMappingValue(new List<string>() {"Genre"})},
                {"Age", new PropertyMappingValue(new List<string>() {"DateOfBirth"}, true)},
                {"Name", new PropertyMappingValue(new List<string>() {"FirstName", "LastName"}, true)},
            };

        public PropertyMappingService()
        {
            propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(_authorPropertyMappings));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }
            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)}, {typeof(TDestination)}>");
        }

        public bool ValidMappingFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();
            if (string.IsNullOrEmpty(fields))
            {
                return true;
            }
            var fieldsAfterSplit = fields.Split(',');
            foreach (var field in fieldsAfterSplit)
            {
                var trimmedField = field.Trim();

                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; private set; }
        public bool Revert { get; private set; }
        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool revert = false)
        {
            DestinationProperties = destinationProperties;
            Revert = revert;
        }
    }

    public class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        public Dictionary<string, PropertyMappingValue> _mappingDictionary { get; private set; }
        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            _mappingDictionary = mappingDictionary;
        }
    }


}