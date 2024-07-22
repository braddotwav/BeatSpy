using System.Windows.Input;

namespace BeatSpy.DataTypes.Interfaces;

public interface IApplicationCommands
{
    public ICommand RemoveFocusCommand { get; }
    public ICommand ExitApplicationCommand { get; }
    public ICommand MinimizeApplicationCommand { get; }
}