﻿using System;
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
        private readonly ITitleAnimationService titleAnimationService;

        public MainWindow()
        {
            InitializeComponent();

            titleAnimationService = new TitleAnimationService(5, 0.5);
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
            Dispatcher.BeginInvoke(new Action(async () =>
            {
                //Allow for a 500ms delay
                await Task.Delay(500);

                //Check if the mouse is hovered over the track title
                if (TrackTitle.IsMouseOver)
                {
                    //Check if we should play the animation
                    if (titleAnimationService.ShouldAnimate(TrackTitle.ActualWidth, 315))
                    {
                        //Set the animation to location
                        titleAnimationService.SetAnimationPosition(-(TrackTitle.ActualWidth - 315));

                        TranslateTransform translateTransform = new();
                        TrackTitle.RenderTransform = translateTransform;

                        //Start the animation
                        titleAnimationService.StartAnimation(translateTransform);

                    }
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
            Dispatcher.BeginInvoke(new Action(() =>
            {
                //Check if we should play the animation
                if (titleAnimationService.ShouldAnimate(TrackTitle.ActualWidth, 315))
                {
                    //Set the animation to location
                    titleAnimationService.SetAnimationPosition(-(TrackTitle.ActualWidth - 315));

                    TranslateTransform translateTransform = new();
                    TrackTitle.RenderTransform = translateTransform;

                    //Start the animation
                    titleAnimationService.StartAnimation(translateTransform);
                }
            }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }
    }
}
