using BeatSpy.Commands;
using BeatSpy.ViewModels.Base;
using System.Windows.Input;

namespace BeatSpy.ViewModels;

internal class MessageHandlerViewModel : ObservableObject
{
	private string? message;

	public string Message
	{
		get { return message ?? string.Empty; }
		set 
		{ 
			message = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(IsMessageEmpty));
		}
	}

    public ICommand? DismissError { get; }

    public bool IsMessageEmpty => string.IsNullOrEmpty(message);

    public MessageHandlerViewModel()
    {
		DismissError = new DismissErrorCommand(this);
    }
}