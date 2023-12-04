using System;
using System.Windows;
using BeatSpy.Helpers;
using System.Windows.Media;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Controls;
using BeatSpy.DataTypes.Constants;

namespace BeatSpy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TitleAnimationBuilder titleAnimBuilder;

        public MainWindow()
        {
            InitializeComponent();

            titleAnimBuilder = new(TimeSpan.FromSeconds(5), 0.5);
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
            if(sender is Button contextButton)
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
            if(sender is ContextMenu contextMenu)
            {
                contextMenu.DataContext = DataContext;
            }
        }

        /// <summary>
        /// This method is fired when the user clicks BeatSpy logo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBeatSpyClicked(object sender, RoutedEventArgs e)
        {
            BrowsUtil.OpenUrl(DefaultConstants.LINK_GITHUB_REPO);
        }

        /// <summary>
        /// This method is fired when the user's mouse enters the title element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTrackTitleMouseEnter(object sender, MouseEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(async () =>
            {
                //Allow for a 500ms delay
                await Task.Delay(500);

                //Check if the mouse is hovered over the track title
                if (TrackTitle.IsMouseOver)
                {
                    //Check if the tracks title width is passed the animation bounds
                    if (titleAnimBuilder.ShouldAnimate(TrackTitle.ActualWidth) && !titleAnimBuilder.IsAnimationPlaying)
                    {
                        //Set the animation to location
                        titleAnimBuilder.SetAnimationTo(-(TrackTitle.ActualWidth - 290));

                        TranslateTransform translateTransform = new();
                        TrackTitle.RenderTransform = translateTransform;

                        titleAnimBuilder.StartAnimation(translateTransform);
                    }
                }
            }));
        }
    }
}
