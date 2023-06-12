using System.ComponentModel.DataAnnotations;

namespace Models;

public class Image
{
    [Required]
    public string Prompt { get; set; }
    
    [Range(1,10)]
    public int N { get; set; } = 1;
    public string Size { get; set; } = "1024x1024";
    /// <summary>
    /// Response format
    /// </summary>
    /// <value>url or b64_json</value>
    public string Response_format { get; set; } = "url";
    public string User { get; set; } = "ChatGPTApi";

}