using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace BeatSpy.UserControls
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(SearchControl), new PropertyMetadata(null));


        public SearchControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This method is fired when ever the user presses a key in the search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSearchQueryPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter)
            {
                if (SearchCommand == null || !SearchCommand.CanExecute(Search.Text)) return;

                SearchCommand.Execute(Search.Text);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(Search), null);
                Keyboard.ClearFocus();
            }
        }
    }
}
