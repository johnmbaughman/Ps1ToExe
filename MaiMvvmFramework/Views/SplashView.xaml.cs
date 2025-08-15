using MaiMvvmFramework.ViewModels;

namespace MaiMvvmFramework.Views;

/// <summary>
/// Represents the splash window view displayed during application startup.
/// </summary>
/// <remarks>
/// This view is associated with an <see cref="ISplashViewModel"/> instance that provides
/// the data and logic for the splash screen. It is intended for internal use within the framework.
/// </remarks>
internal sealed partial class SplashView : ISplashView
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SplashView"/> class with the specified view model.
    /// </summary>
    /// <param name="viewModel">
    /// The <see cref="ISplashViewModel"/> instance that provides data and logic for the splash window.
    /// </param>
    public SplashView(ISplashViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    /// <summary>
    /// Gets the <see cref="ISplashViewModel"/> associated with this splash window.
    /// </summary>
    /// <value>
    /// The view model instance used as the <c>DataContext</c> for this view.
    /// </value>
    public ISplashViewModel ViewModel => (ISplashViewModel)DataContext;
}
