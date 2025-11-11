using System;

namespace SmithereenUWP.DataModels
{
    public class MainMenuItem
    {
        public char Icon { get; private set; }
        public string Label { get; private set; }
        public Type PageType { get; private set; }

        public MainMenuItem(char icon, string label, Type pageType)
        {
            Icon = icon;
            Label = label;
            PageType = pageType;
        }
    }
}
