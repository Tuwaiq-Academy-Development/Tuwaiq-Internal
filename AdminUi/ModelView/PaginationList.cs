using Newtonsoft.Json;

namespace AdminUi.ModelView;

public class PaginationList
{
    public IEnumerable<dynamic> Data { get; set; } = null!;
    [JsonProperty("Last_page")]
    public int LastPage { get; set; }
    
}