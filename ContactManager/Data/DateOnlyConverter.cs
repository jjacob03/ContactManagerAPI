using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ContactManager_API.Data
{
    public class DateOnlyConverter : ValueConverter<DateOnly?, DateTime?>
    {
        public DateOnlyConverter(ConverterMappingHints mappingHints = null)
        : base(
              v => v.HasValue ? v.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
              v => v.HasValue ? DateOnly.FromDateTime(v.Value) : (DateOnly?)null,
              mappingHints)
        {
        }
    }
}
