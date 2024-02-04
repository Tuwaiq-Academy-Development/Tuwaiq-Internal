namespace TuwaiqRecruitment.Helper;

public interface IGoogleReCaptcha
{
    bool ReCaptchaPassedV2(string gRecaptchaResponse);
}