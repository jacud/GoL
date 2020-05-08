using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GoL.ViewModel.Behaviors
{
    /// <summary>
    /// Класс поведения обработчика ScrollViewer.
    /// </summary>
    public class GridScrollViewerBehavior : Behavior<ScrollViewer>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += ScrollLoaded;
        }

        /// <summary>
        /// Обрабочик события загрузки.
        /// </summary>
        /// <param name="sender">Вызывающий объект.</param>
        /// <param name="args">Аргументы.</param>
        private void ScrollLoaded(object sender, RoutedEventArgs args)
        {
            AssociatedObject.ScrollToVerticalOffset(AssociatedObject.ScrollableHeight / 2);
            AssociatedObject.ScrollToHorizontalOffset(AssociatedObject.ScrollableWidth / 2);
        }
    }
}