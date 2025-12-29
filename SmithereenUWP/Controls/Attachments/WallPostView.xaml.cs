using SmithereenUWP.API.Objects.Main;
using SmithereenUWP.Core;
using SmithereenUWP.Extensions;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SmithereenUWP.Controls.Attachments
{
    public sealed partial class WallPostView : UserControl
    {
        public static readonly DependencyProperty PostProperty =
            DependencyProperty.Register(nameof(Post), typeof(WallPost), typeof(WallPostView), new PropertyMetadata(default));

        public WallPost Post
        {
            get { return (WallPost)GetValue(PostProperty); }
            set { SetValue(PostProperty, value); }
        }

        public static readonly DependencyProperty IsRepostProperty =
            DependencyProperty.Register(nameof(IsRepost), typeof(bool), typeof(WallPostView), new PropertyMetadata(default));

        public bool IsRepost
        {
            get { return (bool)GetValue(IsRepostProperty); }
            set { SetValue(IsRepostProperty, value); }
        }

        public static readonly DependencyProperty RepostIconGlyphProperty =
            DependencyProperty.Register(nameof(RepostIconGlyph), typeof(string), typeof(WallPostView), new PropertyMetadata(default));

        public string RepostIconGlyph
        {
            get { return (string)GetValue(RepostIconGlyphProperty); }
            set { SetValue(RepostIconGlyphProperty, value); }
        }

        public static readonly DependencyProperty IsContentVisibleProperty =
            DependencyProperty.Register(nameof(IsContentVisible), typeof(bool), typeof(WallPostView), new PropertyMetadata(true));

        public bool IsContentVisible
        {
            get { return (bool)GetValue(IsContentVisibleProperty); }
            set { SetValue(IsContentVisibleProperty, value); }
        }

        long _pid = 0;
        long _rid = 0;
        long _gid = 0;
        long _cid = 0;

        public WallPostView()
        {
            this.InitializeComponent();

            _pid = RegisterPropertyChangedCallback(PostProperty, (a, b) => { RenderPost(); UpdateUI(); });
            _rid = RegisterPropertyChangedCallback(IsRepostProperty, (a, b) => { UpdateUI(); CheckAndRenderForeignLink(); UpdateCounters(); });
            _gid = RegisterPropertyChangedCallback(RepostIconGlyphProperty, (a, b) => UpdateUI());
            _cid = RegisterPropertyChangedCallback(RepostIconGlyphProperty, (a, b) => UpdateUI());
            Unloaded += WallPostView_Unloaded;
        }

        private void WallPostView_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= WallPostView_Unloaded;
            UnregisterPropertyChangedCallback(PostProperty, _pid);
            UnregisterPropertyChangedCallback(IsRepostProperty, _rid);
            UnregisterPropertyChangedCallback(RepostIconGlyphProperty, _gid);
        }

        private void RenderPost()
        {
            if (Reposts.Children.Count == 2) Reposts.Children.RemoveAt(0);
            RepostInRepost.Child = null;
            RepostInRepost.Visibility = Visibility.Collapsed;
            if (Post == null)
            {
                Visibility = Visibility.Collapsed;
                return;
            }

            // Author
            var author = CacheManager.GetUserNameAndAvatar(Post.FromId);
            PostAuthor.Text = string.Join(" ", author.Item1, author.Item2);
            if (author.Item3 != null) PostAuthorAvatar.Source = new BitmapImage(author.Item3);

            // Time and some other info
            string time = DateTimeOffset.FromUnixTimeSeconds(Post.Date).ToLocalTime().ToString("g");
            PostInfo.Text = time;

            // Post text
            RichTextBlockExt.SetHtml(PostText, Post.Text);
            PostText.Visibility = !string.IsNullOrEmpty(Post.Text) ? Visibility.Visible : Visibility.Collapsed;

            // Attachments
            // TODO

            // Reposts
            if (Post.RepostHistory != null && Post.RepostHistory.Count > 0)
            {
                var repost = Post.RepostHistory[0];
                var repostView = new WallPostView
                {
                    Post = repost,
                    IsRepost = true,
                    RepostIconGlyph = Post.IsMastodonStyleRepost ? "" : ""
                };
                Reposts.Children.Insert(0, repostView);

                if (Post.RepostHistory.Count > 1)
                {
                    var repostInRepostView = new WallPostView
                    {
                        Post = Post.RepostHistory[1],
                        IsRepost = true,
                        IsContentVisible = false,
                        RepostIconGlyph = Post.IsMastodonStyleRepost ? "" : "",
                        Margin = new Thickness(0)
                    };
                    RepostInRepost.Child = repostInRepostView;
                    RepostInRepost.Visibility = Visibility.Visible;
                }
                if (Reposts.Children.Count > 0) Reposts.Visibility = Visibility.Visible;
            }

            CheckAndRenderForeignLink();

            UpdateCounters();

            Visibility = Visibility.Visible;
        }

        private void UpdateUI()
        {
            PostContent.Visibility = IsContentVisible ? Visibility.Visible : Visibility.Collapsed;

            RepostIcon.Visibility = IsRepost ? Visibility.Visible : Visibility.Collapsed;
            RepostIcon.Glyph = RepostIconGlyph != null ? RepostIconGlyph : string.Empty;
        }

        private void CheckAndRenderForeignLink()
        {
            if (IsRepost || !Uri.IsWellFormedUriString(Post.Url, UriKind.Absolute))
            {
                ForeignLinkButton.Visibility = Visibility.Collapsed;
                return;
            }

            Uri postUri = new Uri(Post.Url);
            if (!postUri.Host.Equals(AppParameters.CurrentServer, StringComparison.OrdinalIgnoreCase))
            {
                ForeignLinkButton.Label = $"Open on {postUri.Host}";
                ForeignLinkButton.Visibility = Visibility.Visible;
            }
            else
            {
                ForeignLinkButton.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateCounters()
        {
            if (IsRepost)
            {
                PostFooter.Visibility = Visibility.Collapsed;
                return;
            }

            var interactPost = Post.IsMastodonStyleRepost ? Post.RepostHistory[0] : Post;

            CommentButton.Count = interactPost.Comments?.Count ?? 0;
            LikeButton.Count = interactPost.Likes?.Count ?? 0;
            ShareButton.Count = interactPost.Reposts?.Count ?? 0;

            LikeButton.IconGlyph = interactPost.Likes?.UserLikes == true ? "" : "";

            PostFooter.Visibility = Visibility.Visible;
        }
    }
}
