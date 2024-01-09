using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using TaskFighter.Domain.TasksManagement;
using Xunit;

namespace TaskFighter.Tests;

public class BatchTaskTests
{
    [Fact]
    public async Task Batch_ParseLineIntoTaskList()
    {
        string batchContent = await File.ReadAllTextAsync("./Assets/batch.md"); 
        batchContent.Should().NotBeNullOrEmpty();
        var sut = new Batch(batchContent);
        sut.Tasks.Should().HaveCount(9);
    }
}