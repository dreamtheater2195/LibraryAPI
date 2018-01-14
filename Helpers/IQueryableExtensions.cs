using System.Collections.Generic;
using System.Linq;
using LibraryAPI.Services;
using System.Linq.Dynamic.Core;
using System;

namespace LibraryAPI.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (mappingDictionary == null)
            {
                throw new ArgumentNullException("mappingDictionary");
            }
            if (string.IsNullOrEmpty(orderBy))
            {
                return source;
            }
            var orderByAfterSplit = orderBy.Split(',');

            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                var trimmedOrderbyClause = orderByClause.Trim();
                var orderDescending = trimmedOrderbyClause.EndsWith(" desc");

                var indexOfFirstSpace = trimmedOrderbyClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderbyClause : trimmedOrderbyClause.Remove(indexOfFirstSpace);

                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new Exception($"Key mapping for {propertyName} is missing");
                }

                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }
                    source = source.OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending"));
                }
            }
            return source;
        }
    }
}