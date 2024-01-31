namespace TaskFighter.Domain.Common.Interfaces;

public interface ITableRenderable
{
    public string[] GetFieldValues(List<string> fields);
}