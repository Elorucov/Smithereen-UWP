using SmithereenUWP.API.Objects.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;

namespace SmithereenUWP.Core
{
    internal sealed class FediverseRouter
    {
        const string KEY = "router";

        internal delegate void NavigationRequestedDelegate(FediverseObjectType type, string id, object extra);
        internal event NavigationRequestedDelegate NavigationRequested;

        public void GoTo(FediverseObjectType type, string id, object extra = null)
        {
            NavigationRequested?.Invoke(type, id, extra);
        }

        public static FediverseRouter GetForCurrentView()
        {
            var props = CoreApplication.GetCurrentView().Properties;
            if (props.ContainsKey(KEY)) return props[KEY] as FediverseRouter;

            var router = new FediverseRouter();
            props[KEY] = router;
            return router;
        }
    }
}
