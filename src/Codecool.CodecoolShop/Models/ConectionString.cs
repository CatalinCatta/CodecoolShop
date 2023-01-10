using Newtonsoft.Json;

namespace Codecool.CodecoolShop.Models;

public class ConectionString
{
    
    [JsonProperty("DefaultConnection")]
    public string DefaultConnection { get; set; }
}