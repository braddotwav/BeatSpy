using BeatSpy.Commands;
using BeatSpy.Services;
using System.Windows.Input;
using BeatSpy.ViewModels.Base;

namespace BeatSpy.ViewModels;

internal class MessageHandlerViewModel : ViewModelBase
{
    public string Message => messageDisplayService.Message;
    public bool IsMessageEmpty => messageDisplayService.IsMessageEmpty;

    public ICommand DismissError { get; }

    private readonly IMessageDisplayService messageDisplayService;

    public MessageHandlerViewModel(IMessageDisplayService messageDisplayService)
    {
        this.messageDisplayService = messageDisplayService;
        messageDisplayService.OnMessageUpdated += OnDisplayMessageUpdated;
        DismissError = new DismissErrorCommand(messageDisplayService);
    }

    /// <summary>
    /// Updates property notifications when the display message has been updated
    /// </summary>
    private void OnDisplayMessageUpdated()
    {
        OnPropertyChanged(nameof(Message));
        OnPropertyChanged(nameof(IsMessageEmpty));
    }

    /// <summary>
    /// Disposes of resources by unsubscribing to any events
    /// </summary>
    public override void Dispose()
    {
        messageDisplayService.OnMessageUpdated -= OnDisplayMessageUpdated;
        base.Dispose();
    }
}