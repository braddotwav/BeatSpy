using System.Windows.Input;

namespace BeatSpy.DataTypes.Interfaces;

public interface IApplicationCommands
{
    public ICommand RemoveFocus { get; }
    public ICommand ExitApplication { get; }
    public ICommand MinimizeApplication { get; }
}