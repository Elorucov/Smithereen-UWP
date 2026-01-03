using SmithereenUWP.DataModels;
using SmithereenUWP.Helpers;
using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmithereenUWP.Controls
{
    internal class MediaGridPanel : Panel
    {
        public const int MAX_WIDTH = 500;

        private double[] columnStarts, columnEnds, rowStarts, rowEnds = null;

        public static readonly DependencyProperty LayoutProperty =
            DependencyProperty.Register(nameof(Layout), typeof(TiledLayoutResult), typeof(MediaGridPanel), new PropertyMetadata(default, LayoutChanged));

        public TiledLayoutResult Layout
        {
            get { return (TiledLayoutResult)GetValue(LayoutProperty); }
            set { SetValue(LayoutProperty, value); }
        }

        public static readonly DependencyProperty TileProperty =
            DependencyProperty.RegisterAttached("Tile", typeof(Tile), typeof(MediaGridPanel), new PropertyMetadata(null));

        public static void SetTile(DependencyObject obj, Tile value)
            => obj.SetValue(TileProperty, value);

        public static Tile GetTile(DependencyObject obj)
            => (Tile)obj.GetValue(TileProperty);

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Layout == null || availableSize.Width < 100 || availableSize.Height < 100)
            {
                return base.MeasureOverride(availableSize);
            }

            double width = Math.Min(MAX_WIDTH, availableSize.Width);
            double height = Math.Round(width * ((double)Layout.height / (double)PhotoLayout.MAX_WIDTH));

            if (MaxHeight != double.PositiveInfinity && MaxHeight < height)
            {
                width = MaxHeight / height * width;
                height = MaxHeight;
            }

            if (Layout.width < PhotoLayout.MAX_WIDTH)
            {
                width = Math.Round(width * (Layout.width / (double)PhotoLayout.MAX_WIDTH));
            }

            if (rowStarts == null || rowStarts.Length < Layout.rowSizes.Length)
            {
                rowStarts = new double[Layout.rowSizes.Length];
                rowEnds = new double[Layout.rowSizes.Length];
            }
            if (columnStarts == null || columnStarts.Length < Layout.columnSizes.Length)
            {
                columnStarts = new double[Layout.columnSizes.Length];
                columnEnds = new double[Layout.columnSizes.Length];
            }
            double offset = 0;
            for (int i = 0; i < Layout.columnSizes.Length; i++)
            {
                columnStarts[i] = offset;
                var tileWidth = Layout.columnSizes[i] / (double)Layout.width * width;
                if (tileWidth < 1) Debug.WriteLine($"{nameof(MediaGridPanel)}: One of tiles has width less than 1px! Tile width: {tileWidth} Column size: {Layout.columnSizes[i]}; Layout width: {Layout.width}; Width: {width}");
                offset += Math.Round(tileWidth);
                columnEnds[i] = offset;
                offset += PhotoLayout.GAP;
            }
            columnEnds[Layout.columnSizes.Length - 1] = width;
            offset = 0;
            for (int i = 0; i < Layout.rowSizes.Length; i++)
            {
                rowStarts[i] = offset;
                var tileHeight = (double)Layout.rowSizes[i] / (double)Layout.height * height;
                if (tileHeight < 1) Debug.WriteLine($"{nameof(MediaGridPanel)}: One of tiles has height less than 1px! Tile height: {tileHeight}, Row size: {Layout.rowSizes[i]}; Layout height: {Layout.height}; Height: {height}");
                offset += Math.Round(tileHeight);
                rowEnds[i] = offset;
                offset += PhotoLayout.GAP;
            }
            rowEnds[Layout.rowSizes.Length - 1] = height;

            for (int i = 0; i < Children.Count; i++)
            {
                FrameworkElement element = Children[i] as FrameworkElement;
                Tile tile = GetTile(element);
                int colSpan = Math.Max(1, tile.colSpan) - 1;
                int rowSpan = Math.Max(1, tile.rowSpan) - 1;
                double w = columnEnds[tile.startCol + colSpan] - columnStarts[tile.startCol];
                double h = rowEnds[tile.startRow + rowSpan] - rowStarts[tile.startRow];
                element.Measure(new Size(w, h));
            }
            return new Size(width, height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Layout == null || finalSize.Width < 100 || finalSize.Height < 100 || rowStarts == null)
                return base.ArrangeOverride(finalSize);

            double maxWidth = (int)DesiredSize.Width;

            double calculatedHeight = 0;
            for (int i = 0; i < Children.Count; i++)
            {
                FrameworkElement element = Children[i] as FrameworkElement;
                Tile tile = GetTile(element);
                int colSpan = Math.Max(1, tile.colSpan) - 1;
                int rowSpan = Math.Max(1, tile.rowSpan) - 1;

                double x = columnStarts[tile.startCol];
                double y = rowStarts[tile.startRow];
                double w = columnEnds[tile.startCol + colSpan] - x;
                double h = rowEnds[tile.startRow + rowSpan] - y;

                calculatedHeight = Math.Max(calculatedHeight, h + y);
                element.Arrange(new Rect(x, y, w, h));
            }
            return new Size(maxWidth, calculatedHeight);
        }

        private static void LayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MediaGridPanel panel = d as MediaGridPanel;
            panel.UpdateLayout();
        }
    }
}
