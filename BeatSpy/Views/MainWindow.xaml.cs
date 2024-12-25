using System;
using System.Windows;
using BeatSpy.Services;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BeatSpy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TitleAnimationService titleAnimationService;
        private readonly TranslateTransform titleTransform;

        public MainWindow()
        {
            InitializeComponent();

            titleAnimationService = new TitleAnimationService();

            titleTransform = new TranslateTransform();
            TrackTitle.RenderTransform = titleTransform;
        }

        /// <summary>
        /// This method is fired when ever the user mouse clicks with the window border
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowBorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// This method is fired when ever the user mouse clicks on the context button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnContextMenuClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button contextButton)
            {
                contextButton.ContextMenu.IsOpen = true;
            }
        }

        /// <summary>
        /// This method is fired when ever the user loads the context menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnContextMenuLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                contextMenu.DataContext = DataContext;
            }
        }

        /// <summary>
        /// This method is fired when the user's mouse enters the title element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTrackTitleMouseEnter(object sender, MouseEventArgs e)
        {
            // If the title animation is already playing return
            if (titleAnimationService.IsPlaying) return;

            Dispatcher.BeginInvoke(new Action(async () =>
            {
                // Wait a second until procceeding
                await Task.Delay(1000);

                // Insure the user is still hoovering over the title and that the title is out of bounds
                if (TrackTitle.IsMouseOver && titleAnimationService.IsTitleOutOfView(TrackTitle.ActualWidth))
                {
                    // Start the title animation
                    titleAnimationService.PlayAnimation(titleTransform, TrackTitle.ActualWidth);
                }
            }));
        }

        /// <summary>
        /// This method is fired when the track title is updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTrackTitleUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            // Reset the track title if the animation is playing
            if (titleAnimationService.IsPlaying)
            {
                titleAnimationService.StopAnimation(titleTransform);
            }

            Dispatcher.BeginInvoke(new Action(async () =>
            {
                // Wait a second until procceeding
                await Task.Delay(1000);

                // Insure that the title is out of bounds
                if (titleAnimationService.IsTitleOutOfView(TrackTitle.ActualWidth))
                {
                    // Start the title animation
                    titleAnimationService.PlayAnimation(titleTransform, TrackTitle.ActualWidth);
                }
            }));
        }
    }
}
