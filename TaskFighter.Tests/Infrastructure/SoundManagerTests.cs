using Xunit;

namespace TaskFighter.Tests;

public class SoundManagerTests
{
    [Fact]
    public void SoundManager_PlaySound()
    {
        SoundManager manager = new SoundManager("./Assets/");
        manager.Play("clock-sounds");
    }
}