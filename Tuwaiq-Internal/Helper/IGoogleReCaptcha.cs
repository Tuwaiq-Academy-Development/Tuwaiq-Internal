namespace TuwaiqInternal.Helper;

public interface IGoogleReCaptcha
{
    bool ReCaptchaPassedV2(string gRecaptchaResponse);
}