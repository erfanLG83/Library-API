using ByteSizeLib;
using Microsoft.AspNetCore.Http;

namespace Application.Common.CustomValidators;

public class ImageValidator : AbstractValidator<IFormFile>
{
    private static readonly IReadOnlyList<string> AllowedExtensions = new List<string>
    {
        ".svg",
        ".png",
        ".jpeg",
        ".jpg",
        ".webp"
    };

    private static readonly ByteSize MaxImageSize = ByteSize.FromMegaBytes(1);
    public ImageValidator()
    {
        RuleFor(x => x)
            .Custom((file, context) =>
            {
                var extension = Path.GetExtension(file.FileName);

                if (!AllowedExtensions.Contains(extension))
                {
                    context.AddFailure($"فرمت فایل نامناسب است . فرمت های مجاز : {string.Join(',', AllowedExtensions)}");
                }

                var fileSize = ByteSize.FromBytes(file.Length);
                if (fileSize > MaxImageSize)
                {
                    context.AddFailure($"حجم فایل بیشتر از حد مجاز است. حداکثر : {MaxImageSize.MegaBytes} مگابایت");
                }
            });
    }
}

public static class ImageValidatorExtension
{
    private readonly static ImageValidator ValidatorInstance = new();
    public static IRuleBuilderOptions<T, IFormFile> ValidateImage<T>(this IRuleBuilder<T, IFormFile> builder)
    {
        return builder.SetValidator(ValidatorInstance);
    }
}