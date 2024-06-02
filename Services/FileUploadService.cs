using Amazon.S3;
using Amazon.S3.Model;

public class FileUploadService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public FileUploadService(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _bucketName = configuration["AWS:BucketName"]!;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            InputStream = fileStream,
            ContentType = contentType,
            AutoCloseStream = true
        };

        await _s3Client.PutObjectAsync(request);
        return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName
        };

        await _s3Client.DeleteObjectAsync(request);
    }
}
