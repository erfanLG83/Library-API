using Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class FileManager : IFileManager
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileManager(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folder, CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(file.FileName);

        var fileName = $"{Guid.NewGuid()}{extension}";

        var fileRelativePath = Path.Combine(
                    folder,
                    fileName
                );


        var directoryFullPath = Path.Combine
            (
                _webHostEnvironment.WebRootPath,
                folder
            );

        var fileFullPath = Path.Combine(directoryFullPath, fileName);
        Directory.CreateDirectory(directoryFullPath);

        await using var fs = new FileStream(fileFullPath, FileMode.Create);
        await file.CopyToAsync(fs, cancellationToken);

        return fileRelativePath;
    }

    public void Delete(string filePath)
    {
        var path = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
        File.Delete(path);
    }

}
