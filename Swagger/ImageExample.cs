using Models;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger;
/// <summary>
/// Swagger Example
/// </summary>
public class ImageExample : IExamplesProvider<Image>
{
    public Image GetExamples()
    {
       return new Image{
        Prompt = "Image of a Darth Penguin , yielding a red light saber"
       };
    }
}