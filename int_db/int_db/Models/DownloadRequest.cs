namespace int_db.Models;

public class DownloadRequest
{
    public string BucketName { get; set; }
    public string ObjectName { get; set; }
    public string FilePath { get; set; }
}