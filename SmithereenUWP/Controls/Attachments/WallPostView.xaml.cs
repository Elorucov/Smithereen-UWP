using SmithereenUWP.API.Objects.Main;
using SmithereenUWP.Core;
using SmithereenUWP.Extensions;
using SmithereenUWP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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
            _cid = RegisterPropertyChangedCallback(IsContentVisibleProperty, (a, b) => UpdateUI());
            Unloaded += WallPostView_Unloaded;
        }

        private void WallPostView_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= WallPostView_Unloaded;
            UnregisterPropertyChangedCallback(PostProperty, _pid);
            UnregisterPropertyChangedCallback(IsRepostProperty, _rid);
            UnregisterPropertyChangedCallback(RepostIconGlyphProperty, _gid);
            UnregisterPropertyChangedCallback(IsContentVisibleProperty, _cid);
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

            // Spoiler
            if (!string.IsNullOrEmpty(Post.ContentWarning))
            {
                PostContent.Visibility = Visibility.Collapsed;
                SpoilerLabel.Text = Post.ContentWarning;
                SpoilerButton.Visibility = Visibility.Visible;
            }
            else
            {
                PostContent.Visibility = Visibility.Visible;
                SpoilerButton.Visibility = Visibility.Collapsed;
            }

            // Post text
            RichTextBlockExt.SetHtml(PostText, Post.Text);
            PostText.Visibility = !string.IsNullOrEmpty(Post.Text) ? Visibility.Visible : Visibility.Collapsed;

            // Attachments
            AddAttachmentsInUI();

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

        private void AddAttachmentsInUI()
        {
            PostAttachments.Children.Clear();
            if (Post?.Attachments == null || Post.Attachments.Count == 0) return;

            List<ISizedAttachment> previews = new List<ISizedAttachment>();
            Poll poll = null;
            List<string> unknown = new List<string>();

            foreach (var attachment in Post.Attachments)
            {
                switch (attachment.Type)
                {
                    case AttachmentType.Graffiti:
                        previews.Add(attachment.Graffiti);
                        break;
                    case AttachmentType.Photo:
                        previews.Add(attachment.Photo);
                        break;
                    case AttachmentType.Poll:
                        poll = attachment.Poll;
                        break;
                    case AttachmentType.Video:
                        previews.Add(attachment.Video);
                        break;
                    default:
                        unknown.Add(attachment.Type.ToString());
                        break;
                }
            }

            if (previews.Count > 0)
            {
                MediaGridPanel previewsGrid = new MediaGridPanel
                {
                    Margin = new Thickness(0, 12, 0, 0),
                    MaxHeight = 512,
                    Tag = Post.Id
                };

                var layout = PhotoLayout.MakeLayout(previews);
                previewsGrid.Layout = layout;
                var tiles = layout.tiles;

                for (int i = 0; i < previews.Count; i++)
                {
                    var preview = previews[i];

                    if (preview is Graffiti graffiti)
                    {
                        new Action(async () =>
                        {
                            var bitmap = new BitmapImage();
                            bitmap.UriSource = new Uri(graffiti.Url);

                            var graffitiView = new Image
                            {
                                Source = bitmap,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Stretch = Stretch.UniformToFill
                            };
                            MediaGridPanel.SetTile(graffitiView, tiles[i]);
                            previewsGrid.Children.Add(graffitiView);
                        })();
                    }
                    else if (preview is Photo photo)
                    {
                        new Action(async () =>
                        {
                            var bitmap = new BitmapImage();
                            var ps = photo.Sizes.Where(s => s.Type == "x").FirstOrDefault();
                            bitmap.UriSource = new Uri(ps.Url);

                            var tempPhotoView = new Image
                            {
                                Source = bitmap,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Stretch = Stretch.UniformToFill
                            };
                            MediaGridPanel.SetTile(tempPhotoView, tiles[i]);
                            previewsGrid.Children.Add(tempPhotoView);
                        })();
                    }
                    else if (preview is Video video)
                    {
                        var b = new Border
                        {
                            Background = new SolidColorBrush(Color.FromArgb(32, 128, 128, 128)),
                            Child = new TextBlock
                            {
                                Text = "Video",
                                FontStyle = Windows.UI.Text.FontStyle.Italic,
                                Margin = new Thickness(12, 12, 0, 0)
                            }
                        };
                        MediaGridPanel.SetTile(b, tiles[i]);
                        previewsGrid.Children.Add(b);
                    }
                }

                PostAttachments.Children.Add(previewsGrid);
            }

            if (poll != null)
            {
                PostAttachments.Children.Add(new TextBlock
                {
                    FontStyle = Windows.UI.Text.FontStyle.Italic,
                    Text = $"Poll is not supported yet.",
                    Margin = new Thickness(0, 12, 0, 0)
                });
            }

            foreach (var atchType in unknown)
            {
                PostAttachments.Children.Add(new TextBlock
                {
                    FontStyle = Windows.UI.Text.FontStyle.Italic,
                    Text = $"Unsupported attachment type: {atchType}",
                    Margin = new Thickness(0, 12, 0, 0)
                });
            }
        }

        private void UpdateUI()
        {
            PostContentRoot.Visibility = IsContentVisible ? Visibility.Visible : Visibility.Collapsed;

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

        private void ToggleSpoiler(object sender, RoutedEventArgs e)
        {
            if (PostContent.Visibility == Visibility.Collapsed)
            {
                SpoilerIcon.Glyph = "";
                AnimationHelper.ShowAnimated(PostContent);
            }
            else
            {
                SpoilerIcon.Glyph = "";
                AnimationHelper.HideAnimated(PostContent);
            }
        }
    }
}
