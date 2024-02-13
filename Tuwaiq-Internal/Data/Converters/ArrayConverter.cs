using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace TuwaiqInternal.Data.Converters;

public class ArrayConverter : ValueConverter<string[], string>
{
    public ArrayConverter() : base(value => JsonConvert.SerializeObject(value),
        serializedValue => JsonConvert.DeserializeObject<string[]>(serializedValue) ??
                           new string[]{})
    {
    }
}