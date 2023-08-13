using Moq;
using ScpWorker.Services;
using Xunit;

namespace TestScpWorker.Services;

public class ScpServiceTests
{
    private readonly Mock<IScpService> _sut = new();

    public ScpServiceTests()
    {
    }

    [Fact]
    public void FileExist_ShouldReturnTrue_WhenFileExist()
    {
        var file1 = "folder/file1.csv";

        var file2 = "folder/file1.csv";

        _sut.Setup(s => s.FileExists(file1)).Returns(true);

        var fileExist = _sut.Object.FileExists(file2);

        Assert.True(fileExist);
    }

    [Fact]
    public void FileExist_ShouldReturnFalse_WhenFileNotExist()
    {
        var file1 = "folder/file1.csv";

        var file2 = "folder/file2.csv";

        _sut.Setup(s => s.FileExists(file1)).Returns(true);

        var fileExist = _sut.Object.FileExists(file2);

        Assert.False(fileExist);
    }
}
