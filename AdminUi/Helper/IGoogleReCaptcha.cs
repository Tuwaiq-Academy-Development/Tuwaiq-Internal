namespace AdminUi.Helper;

public interface IGoogleReCaptcha
{
    bool ReCaptchaPassedV2(string gRecaptchaResponse);
}