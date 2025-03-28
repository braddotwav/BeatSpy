using System.Windows;
using BeatSpy.Helpers;
using System.Windows.Input;
using System.Windows.Controls;

namespace BeatSpy.UserControls
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        private readonly string[] placeholders = 
            ["Search for a song…", 
            "Type a song title to analyze…", 
            "What song are you curious about?",
            "Get insights on any track…",
            "Search by song, artist, or album…"];

        private Label? placeholderLabel;

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
            if (e.Key is not Key.Enter) return;
            if (SearchCommand == null || !SearchCommand.CanExecute(Search.Text)) return;
            
            SetRandomPlaceholderContent();
            SearchCommand.Execute(Search.Text);
            Search.ScrollToHome();
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(Search), null);
            Keyboard.ClearFocus();
        }

        private void SetRandomPlaceholderContent()
        {
            if (placeholderLabel is null) return;
            placeholderLabel.Content = placeholders[RandomHelper.Range(0, placeholders.Length)];
        }

        private void OnUserControlLoaded(object sender, RoutedEventArgs e)
        {
            placeholderLabel = (Label)Search.Template.FindName("SearchPlaceholder", Search);
            SetRandomPlaceholderContent();
        }
    }
}
