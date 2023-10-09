using BeatSpy.Commands;
using System.Windows.Input;
using BeatSpy.ViewModels.Base;
using BeatSpy.DataTypes.Interfaces;

namespace BeatSpy.ViewModels;

internal class MessageHandlerViewModel : ObservableObject, IMessageNotify
{
	private string message = string.Empty;

	public string Message
	{
		get { return message; }
		private set 
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

	public void SetMessage(string message)
	{
		Message = message;
	}

	public void ClearMessage()
	{
		Message = string.Empty;
	}
}