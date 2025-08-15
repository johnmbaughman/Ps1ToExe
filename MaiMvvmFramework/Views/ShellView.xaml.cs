using System.ComponentModel;
using System.Windows;
using MaiMvvmFramework.ViewModels;

namespace MaiMvvmFramework.Views
{
    /// <summary>
    /// Represents the main shell view for the MaiMvvmFramework WPF application.
    /// Provides access to the associated <see cref="ShellViewModel"/> instance, which manages
    /// the main content view and its view model, and coordinates dialog management and inter-ViewModel communication.
    /// </summary>
    public partial class ShellView : IShellView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellView"/> class.
        /// Sets the <see cref="FrameworkElement.DataContext"/> to the provided <see cref="IShellViewModel"/> and
        /// registers the <see cref="FrameworkElement.Loaded"/> event to invoke <see cref="ShellViewModel.ViewLoaded"/>.
        /// </summary>
        /// <param name="shellViewModel">The shell view model to associate with this view.</param>
        public ShellView(IShellViewModel shellViewModel)
        {
            InitializeComponent();
            DataContext = shellViewModel;
            Loaded += (_, _) => ViewModel.ViewLoaded();
        }

        /// <summary>
        /// Gets the <see cref="ShellViewModel"/> associated with this shell view.
        /// The shell view model provides access to the main content view and its view model,
        /// and coordinates dialog management and inter-ViewModel communication.
        /// </summary>
        /// <value>
        /// An instance of <see cref="ShellViewModel"/> representing the shell's view model.
        /// </value>
        public ShellViewModel ViewModel => (ShellViewModel)DataContext;

        /// <summary>
        /// Handles the <see cref="Window.Closing"/> event for the shell view.
        /// Shuts down the current WPF application when the shell view is closing.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void ShellView_OnClosing(object? sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
