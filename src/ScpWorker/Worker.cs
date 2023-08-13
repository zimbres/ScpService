namespace ScpWorker;

public class Worker
{
    private readonly ILogger _logger;
    private readonly IScpService _scpService;
    private readonly IMailService _mailService;
    private readonly IConfiguration _configuration;


    public Worker(IConfiguration configuration, ILogger logger, IScpService scpService, IMailService mailService)
    {
        _logger = logger;
        _scpService = scpService;
        _mailService = mailService;
        _configuration = configuration;
    }

    public async Task ExecuteAsync()
    {
        var paths = _configuration.GetSection(nameof(PathConfiguration)).Get<PathConfiguration>();
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        _logger.LogWarning("Application version {version}", version);

        try
        {
            DirectoryInfo directoryInfo = new(paths.LocalFolder);
            var fileInfo = directoryInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly);

            if (fileInfo.Length == 0)
            {
                _logger.LogWarning("No files to process.");
                return;
            }

            _scpService.Connect();

            foreach (var file in fileInfo)
            {
                _scpService.UploadFile(file.FullName, $"{paths.RemoteFolder}/{file.Name}");

                if (_scpService.FileExists($"{paths.RemoteFolder}/{file.Name}"))
                {
                    _logger.LogInformation("File {file.Name} uploaded successfully.", file.Name);
                    File.Move(file.FullName, $"{paths.LocalProcessed}/{file.Name}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("{ex.Message}", ex.Message);
            _mailService.SendMail();
        }
        finally
        {
            await Task.CompletedTask;
        }
    }
}
