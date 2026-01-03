using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace SmithereenUWP.Helpers
{
    internal static class AnimationHelper
    {
        const double DURATION_MS = 300;
        const string DURATION_RESOURCE_KEY = "hop_hey_lalaley";

        public static void ShowAnimated(Panel panel, double durationMs = 0)
        {
            if (!ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 4))
            {
                panel.Visibility = Visibility.Visible;
                return;
            }

            if (durationMs <= 0) durationMs = DURATION_MS;

            long id = 0;
            id = panel.RegisterPropertyChangedCallback(UIElement.VisibilityProperty, (a, b) =>
            {
                if (panel.ActualHeight == 0)
                {
                    panel.Resources.Add(DURATION_RESOURCE_KEY, durationMs);
                    panel.SizeChanged += Panel_SizeChanged;
                }
                else
                {
                    AnimateChildren(panel, durationMs);
                }
                panel.UnregisterPropertyChangedCallback(UIElement.VisibilityProperty, id);
            });
            panel.Visibility = Visibility.Visible;
        }

        public static void HideAnimated(Panel panel, double durationMs = 0)
        {
            if (!ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 4))
            {
                panel.Visibility = Visibility.Visible;
                return;
            }

            if (durationMs <= 0) durationMs = DURATION_MS;

            AnimateChildren(panel, durationMs, true);
            new System.Action(async () =>
            {
                await Task.Delay((int)DURATION_MS);
                panel.Visibility = Visibility.Collapsed;
            })();
        }

        private static void Panel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Panel panel = sender as Panel;
            double durationMs = (double)panel.Resources[DURATION_RESOURCE_KEY];
            panel.SizeChanged -= Panel_SizeChanged;
            AnimateChildren(panel, durationMs);
        }

        private static void AddChildrenRecursive(Panel panel, List<UIElement> list)
        {
            foreach (var child in panel.Children)
            {
                if (child.Visibility == Visibility.Collapsed) continue;
                if (child is Panel innerPanel)
                {
                    AddChildrenRecursive(innerPanel, list);
                }
                else
                {
                    list.Add(child);
                }
            }
        }

        private static void AnimateChildren(Panel panel, double durationMs = 200, bool reverse = false)
        {
            List<UIElement> children = new List<UIElement>();
            List<CompositionAnimationGroup> translationAnimations = new List<CompositionAnimationGroup>();

            var rootVisual = ElementCompositionPreview.GetElementVisual(panel);
            var compositor = rootVisual.Compositor;
            var height = panel.ActualHeight;
            var duration = TimeSpan.FromMilliseconds(durationMs);

            rootVisual.Clip = compositor.CreateInsetClip();
            rootVisual.Clip.Offset = new Vector2(0, 0);
            AddChildrenRecursive(panel, children);

            foreach (var child in children)
            {
                ElementCompositionPreview.SetIsTranslationEnabled(child, true);
                var visual = ElementCompositionPreview.GetElementVisual(child);
                var itemOffset = child.TransformToVisual(panel).TransformPoint(new Point(0, 0)).Y;
                var itemHeight = child.DesiredSize.Height;
                var animGroup = compositor.CreateAnimationGroup();

                if (!reverse)
                {
                    visual.Opacity = 0;
                }

                var slide = compositor.CreateScalarKeyFrameAnimation();
                slide.Target = "Translation.Y";
                slide.Duration = duration;
                slide.InsertKeyFrame(0, reverse ? 0 : (float)-height);
                slide.InsertKeyFrame(1, reverse ? (float)-height : 0);
                animGroup.Add(slide);

                translationAnimations.Add(animGroup);
            }

            new System.Action(async () =>
            {
                bool loop = true;
                int i = reverse ? 0 : children.Count - 1;
                while (loop)
                {
                    var child = children[i];
                    var visual = ElementCompositionPreview.GetElementVisual(child);
                    var animGroup = translationAnimations[i];

                    visual.Opacity = 1;
                    visual.StartAnimationGroup(animGroup);

                    await Task.Delay(1);

                    if (reverse)
                    {
                        i++;
                        loop = i < children.Count;
                    }
                    else
                    {
                        i--;
                        loop = i >= 0;
                    }
                }
            })();
        }
    }
}
