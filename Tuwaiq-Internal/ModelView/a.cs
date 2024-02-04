using Newtonsoft.Json;

namespace TuwaiqRecruitment.ModelView;

public  class Answer
{
    [JsonProperty("ما تقييمك العام للمرشح")]
    public long ماتقييمكالعامللمرشح { get; set; }

    [JsonProperty("ما تقييمك لمستوى اللغة الإنجليزية للمرشح")]
    public long ماتقييمكلمستوىاللغةالإنجليزيةللمرشح { get; set; }

    [JsonProperty("ما تقييمك لمهارات التعلم الذاتي والمستمر للمرشح")]
    public long ماتقييمكلمهاراتالتعلمالذاتيوالمستمرللمرشح { get; set; }

    [JsonProperty("ما تقييمك لمهارات الاتصال والتواصل للمرشح")]
    public long ماتقييمكلمهاراتالاتصالوالتواصلللمرشح { get; set; }

    [JsonProperty("ما تقييمك للمهارات الناعمة للمرشح")]
    public long ماتقييمكللمهاراتالناعمةللمرشح { get; set; }

    [JsonProperty("ما تقييمك للمرشح بالجوانب التقنية ذات العلاقة بالوظيفة")]
    public long ماتقييمكللمرشحبالجوانبالتقنيةذاتالعلاقةبالوظيفة { get; set; }

    [JsonProperty("مهتم")]
    public string مهتم { get; set; }

    [JsonProperty("المجال المرشح له المتقدم")]
    public string المجالالمرشحلهالمتقدم { get; set; }
}