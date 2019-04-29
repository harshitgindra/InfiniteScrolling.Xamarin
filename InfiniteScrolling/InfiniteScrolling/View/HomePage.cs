using System;
using InfiniteScrolling.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Extended;

namespace InfiniteScrolling.View
{
    public class HomePage : ContentPage
    {
        private readonly HomePageViewModel _vm;

        public HomePage()
        {
            this.Title = "Infinite Scroll Demo";
            _vm = new HomePageViewModel();
            BindingContext = _vm;
            _SetupView();
        }

        private void _SetupView()
        {
            Editor searchField = new Editor()
            {
                Placeholder = "Search..",
                FontSize = 20,
                Margin = new Thickness(3, 0, 3, 5),
            };
            searchField.SetBinding(Editor.TextProperty, nameof(HomePageViewModel.Search));

            Label loadingLabel = new Label()
            {
                TextColor = Color.Black,
                Text = "Loading..",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            loadingLabel.SetBinding(IsVisibleProperty, nameof(HomePageViewModel.ShowLoadingText), BindingMode.OneWay);


            var imagesListView = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                ItemTemplate = new DataTemplate(typeof(DataCell)),
                SeparatorVisibility = SeparatorVisibility.Default,
                IsPullToRefreshEnabled = true,
                Margin = 10,
                Header = searchField,
                Footer = loadingLabel
            };

            imagesListView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, nameof(HomePageViewModel.Items), BindingMode.OneWay);

            var infiniteScrollBehavior = new InfiniteScrollBehavior { };
            infiniteScrollBehavior.SetBinding(InfiniteScrollBehavior.IsLoadingMoreProperty, nameof(HomePageViewModel.IsBusy));
            imagesListView.Behaviors.Add(infiniteScrollBehavior);

            this.Content = imagesListView;
        }
    }
}

