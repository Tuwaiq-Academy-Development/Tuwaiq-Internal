using DAL.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace DAL.Converters;

public class ArrayConverter : ValueConverter<string[], string>
{
    public ArrayConverter() : base(value => JsonConvert.SerializeObject(value),
        serializedValue => JsonConvert.DeserializeObject<string[]>(serializedValue) ??
                           new string[]{})
    {
    }
}

public class CheckTypesConverter : ValueConverter<CheckType[], string>
{
    public CheckTypesConverter() : base(value => JsonConvert.SerializeObject(value),
        serializedValue => JsonConvert.DeserializeObject<CheckType[]>(serializedValue) ??
                           new CheckType[]{})
    {
    }
}