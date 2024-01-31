using System.Diagnostics;
using TaskFighter.Domain;

namespace TaskFighter.Infrastructure.EditorTool;

public class TextEditor : ITextEditor
{
    public void Open(string filePath)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe", Arguments = $"/c nvim \"{filePath}\"", UseShellExecute = true
        };

        Process.Start(startInfo);
    }
}