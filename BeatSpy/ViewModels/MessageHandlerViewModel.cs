using BeatSpy.Commands;
using BeatSpy.Services;
using System.Windows.Input;
using BeatSpy.ViewModels.Base;

namespace BeatSpy.ViewModels;

internal class MessageHandlerViewModel : ViewModelBase
{
    public string Message => messageDisplayService.Message;
    public bool IsMessageEmpty => messageDisplayService.IsMessageEmpty;

    public ICommand DismissErrorCommand { get; }

    private readonly IMessageDisplayService messageDisplayService;

    public MessageHandlerViewModel(IMessageDisplayService messageDisplayService)
    {
        this.messageDisplayService = messageDisplayService;
        messageDisplayService.OnMessageUpdated += OnDisplayMessageUpdated;
        DismissErrorCommand = new ClearMessageDisplayCommand(messageDisplayService);
    }

    private void OnDisplayMessageUpdated()
    {
        OnPropertyChanged(nameof(Message));
        OnPropertyChanged(nameof(IsMessageEmpty));
    }

    public override void Dispose()
    {
        messageDisplayService.OnMessageUpdated -= OnDisplayMessageUpdated;
    }
}