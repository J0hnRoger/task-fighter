using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using TaskFighter.Domain.TasksManagement;
using Xunit;

namespace TaskFighter.Tests;

public class BatchTests
{
    [Fact]
    public async Task Batch_CreateFromMarkdown()
    {
        string batchContent = await File.ReadAllTextAsync("./Assets/batch.md"); 
        batchContent.Should().NotBeNullOrEmpty();
        var sut = new BatchFile(batchContent);
        sut.Tasks.Should().HaveCount(4);
    }
}