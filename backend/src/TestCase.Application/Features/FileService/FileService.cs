using Microsoft.AspNetCore.Http;

namespace TestCase.Application.Features.FileService;

public static class FileService
{
    public static string FileSaveToServer(IFormFile file, string filePath)
    {
        // Klasör yoksa oluştur
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        
        string fileName = string.Join(".", DateTime.Now.ToFileTime().ToString(), file.FileName);
        string path = filePath + fileName;
        using (var stream = File.Create(path))
        {
            file.CopyTo(stream);
        }
        return fileName;
    }
    
    public static byte[] FileConvertByteArrayToDatabase(IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            file.CopyTo(memoryStream);
            var fileBytes = memoryStream.ToArray();
            string fileString = Convert.ToBase64String(fileBytes);
            return fileBytes;
        }
    }
    
    public static void FileDeleteToServer(string path)
    {
        try
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
        catch (Exception)
        {
        }
    }
}