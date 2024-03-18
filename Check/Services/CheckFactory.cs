using DAL.Enums;

namespace Check.Services;

public class CheckFactory(IServiceProvider serviceProvider)
{
    public ICheck GetCheck(CheckType checkType)
    {
        return checkType switch
        {
            CheckType.Gosi => serviceProvider.GetRequiredService<GosiCheck>(),
            _ => throw new ArgumentOutOfRangeException(nameof(checkType), checkType, null)
        };
    }
}