namespace ScpWorker.Services;

public class ScpService : IScpService
{
    private readonly SftpClient _sftpClient;
    private readonly RetryPolicy _retryPolicy = Policy.Handle<Exception>()
        .WaitAndRetry(3, retryCount => TimeSpan.FromSeconds(Math.Pow(3, retryCount)));

    public ScpService(SftpClient sftpClient)
    {
        _sftpClient = sftpClient;
    }

    public bool FileExists(string remoteFile)
    {
        if (_retryPolicy.Execute(() => _sftpClient.Exists(remoteFile)))
        {
            return true;
        }
        return false;
    }

    public void UploadFile(string localFilePath, string remoteFilePath)
    {
        try
        {
            using var localFile = File.OpenRead(localFilePath);
            _retryPolicy.Execute(() => _sftpClient.UploadFile(localFile, remoteFilePath));
        }
        catch (Exception)
        {
            throw;
        }
    }

    public IEnumerable<SftpFile> ListAllFiles(string remoteDirectory = ".")
    {
        try
        {
            return _retryPolicy.Execute(() => _sftpClient.ListDirectory(remoteDirectory));
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void DownloadFile(string remoteFilePath, string localFilePath)
    {
        try
        {
            using var localFile = File.Create(localFilePath);
            _retryPolicy.Execute(() => _sftpClient.DownloadFile(remoteFilePath, localFile));
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void DeleteFile(string remoteFilePath)
    {
        try
        {
            _retryPolicy.Execute(() => _sftpClient.DeleteFile(remoteFilePath));
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Connect()
    {
        _retryPolicy.Execute(() => _sftpClient.Connect());
    }

    public void Disconnect()
    {
        _sftpClient.Disconnect();
    }
}
