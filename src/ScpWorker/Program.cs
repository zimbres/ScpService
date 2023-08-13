IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var scpConfig = hostContext.Configuration.GetSection(nameof(ScpConfiguration)).Get<ScpConfiguration>();
        services.AddSingleton<Worker>();

        services.AddSingleton(scpConfig.Password != ""
            ? new SftpClient(scpConfig.Host, scpConfig.Port, scpConfig.UserName, scpConfig.Password)
            : new SftpClient(scpConfig.Host, scpConfig.Port, scpConfig.UserName, new PrivateKeyFile(scpConfig.KeyPath)));
         
        services.AddSingleton<IScpService, ScpService>();
        services.AddSingleton<IMailService, MailService>();
    }).Build();

await host.Services.GetRequiredService<Worker>().ExecuteAsync();
