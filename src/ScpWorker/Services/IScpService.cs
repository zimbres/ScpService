namespace ScpWorker.Services;

public interface IScpService
{
    bool FileExists(string remoteFile);
    void UploadFile(string localFilePath, string remoteFilePath);
    IEnumerable<ISftpFile> ListAllFiles(string remoteDirectory = ".");
    void DownloadFile(string remoteFilePath, string localFilePath);
    void DeleteFile(string remoteFilePath);
    void Connect();
    void Disconnect();
}
