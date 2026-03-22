using SmithereenUWP.API.Objects.Main;
using System;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace SmithereenUWP.Extensions
{
    internal static class ImageExt
    {
        public static readonly DependencyProperty AvatarSourceProperty =
            DependencyProperty.RegisterAttached(
                "AvatarSource",
                typeof(IWithAvatar),
                typeof(ImageExt),
                new PropertyMetadata(null, AvatarChanged));

        public static void SetAvatarSource(Image obj, IWithAvatar value)
            => obj.SetValue(AvatarSourceProperty, value);

        public static IWithAvatar GetAvatarSource(Image obj)
            => (IWithAvatar)obj.GetValue(AvatarSourceProperty);

        public static readonly DependencyProperty PhotoSourceProperty =
            DependencyProperty.RegisterAttached(
            "PhotoSource",
                typeof(Photo),
                typeof(ImageExt),
                new PropertyMetadata(null, PhotoChanged));

        public static void SetPhotoSource(Image obj, Photo value)
            => obj.SetValue(PhotoSourceProperty, value);

        public static Photo GetPhotoSource(Image obj)
            => (Photo)obj.GetValue(PhotoSourceProperty);

        private static void AvatarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var image = d as Image;
            if (image == null) return;

            if (e.NewValue != null && e.NewValue is IWithAvatar avatar)
            {
                if (!avatar.HasPhoto)
                {
                    // TODO: SVG question icon
                    image.Source = null;
                    return;
                }

                double requestedSize = Math.Max(image.Width, image.Height);
                var displayInformation = DisplayInformation.GetForCurrentView();
                double rawSize = requestedSize * displayInformation.LogicalDpi;

                if (rawSize >= 200 && avatar.Photo200 != null)
                {
                    image.Source = new BitmapImage(new Uri(avatar.Photo200));
                    return;
                }
                else if (rawSize >= 100 && avatar.Photo100 != null)
                {
                    image.Source = new BitmapImage(new Uri(avatar.Photo100));
                    return;
                }
                else if (rawSize >= 50 && avatar.Photo50 != null)
                {
                    image.Source = new BitmapImage(new Uri(avatar.Photo200));
                    return;
                }
            }

            image.Source = null;
            return;
        }

        private static void PhotoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var image = d as Image;
            if (image == null) return;

            if (e.NewValue != null && e.NewValue is Photo photo)
            {
                var displayInformation = DisplayInformation.GetForCurrentView();

                double pw = image.Width * displayInformation.LogicalDpi;
                double ph = image.Height * displayInformation.LogicalDpi;

                if (photo.Sizes != null || photo.Sizes.Count != 0)
                {
                    PhotoSize ps = null;
                    foreach (PhotoSize s in photo.Sizes)
                    {
                        ps = s;
                        if (MathEx.IsLargeOrEqualThanMax(s.Width, s.Height, pw, ph)) break;
                    }

                    if (ps != null)
                    {
                        image.Source = new BitmapImage(new Uri(ps.Url));
                        return;
                    }
                }
            }

            image.Source = null;
            return;
        }
    }
}
