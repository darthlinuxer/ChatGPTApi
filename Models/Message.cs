using System.ComponentModel.DataAnnotations;

namespace Models;

public class Message
{
    [Required]
    public string Role {get; set;}
    [Required]
    public string Content {get; set;}
    /// <summary>
    /// Name is optional! You don´t need to send it all
    /// </summary>
    /// <value></value>
    public string Name {get; set;} = "Your_Name";

}