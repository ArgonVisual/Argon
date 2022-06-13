namespace Argon;

public interface IFileHandle : ISaveable
{
    public string Name { get; }

    public string Directory { get; }
}