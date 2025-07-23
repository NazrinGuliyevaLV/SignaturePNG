using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;

namespace Signature.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignaturesController : ControllerBase
    {

        [HttpPost]
        public IActionResult Post([FromBody] FullNameRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
                return BadRequest("FullName is required.");

            var fontPath = Path.Combine(Directory.GetCurrentDirectory(), "fonts", "ReenieBeanie-Regular.ttf");
            if (!System.IO.File.Exists(fontPath))
                return NotFound("Font file not found at: " + fontPath);

            var typeface = SKTypeface.FromFile(fontPath);
            var paint = new SKPaint
            {
                Typeface = typeface,
                TextSize = 48,
                IsAntialias = true,
                Color = SKColors.Black
            };

            var textWidth = paint.MeasureText(request.FullName);
            var imageWidth = (int)textWidth + 20;
            var imageHeight = 80;

            using var bitmap = new SKBitmap(imageWidth, imageHeight);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.Transparent);
            canvas.DrawText(request.FullName, 10, 60, paint);

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            string base64String = Convert.ToBase64String(data.ToArray());
            return Content(base64String, "text/plain");
        }


        public class FullNameRequest
        {
            public string? FullName { get; set; }
        }

    }
}
