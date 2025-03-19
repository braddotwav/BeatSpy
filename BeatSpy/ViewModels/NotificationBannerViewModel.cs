using BeatSpy.Models;
using BeatSpy.Services;
using BeatSpy.Commands;
using System.Windows.Input;

namespace BeatSpy.ViewModels;

class NotificationBannerViewModel : ViewModelBase
{
	private readonly INotificationSerivce notificationSerivce;

	private Notification currentNotification = Notification.Empty;

	public Notification CurrentNotification
	{
		get { return currentNotification; }
		set 
		{
			if (currentNotification != value)
			{
				currentNotification = value;
				OnPropertyChanged(nameof(CurrentNotification));
				OnPropertyChanged(nameof(IsNotificationEmpty));
			}
		}
	}

    public bool IsNotificationEmpty => currentNotification == Notification.Empty;

    #region Commands
    public ICommand DismissNotificationCommand { get; }
    #endregion

    public NotificationBannerViewModel(INotificationSerivce notificationSerivce)
    {
		this.notificationSerivce = notificationSerivce;
        notificationSerivce.OnNotificationRecived += OnNotificationRecived;

        DismissNotificationCommand = new DelegateCommand(ExecuteDismissMessageCommand, (_) => true);
    }

    private void OnNotificationRecived(Notification newNotification)
    {
		CurrentNotification = newNotification;
    }

	private void ExecuteDismissMessageCommand(object parameter)
	{
		if (currentNotification is ProgressNotification progress)
		{
			progress.Cancel();
		}

		notificationSerivce.ClearNotification();
	}

    public override void Dispose()
    {
        notificationSerivce.OnNotificationRecived += OnNotificationRecived;
    }
}