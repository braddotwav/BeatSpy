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

	/// <summary>
	/// Set the message of the message handler
	/// </summary>
	/// <param name="message">Message to be set</param>
	public void SetMessage(string message)
	{
		Message = message;
	}

	/// <summary>
	/// Clear the message handler
	/// </summary>
	public void ClearMessage()
	{
		Message = string.Empty;
	}
}