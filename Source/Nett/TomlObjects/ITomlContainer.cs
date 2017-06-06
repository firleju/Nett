namespace Nett
{
    public interface ITomlContainer
    {
        ITomlContainer Parent { get; }

        ITomlRoot Root { get; }
    }
}
