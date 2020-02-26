using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator _events;
        private SalesViewModel _salesVM;
        private ILoggedInUserModel _user;
        private IAPIHelper _apiHelper;
        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, ILoggedInUserModel user, IAPIHelper apiHelper)
        {
            _events = events;
            _salesVM = salesVM;
            _user = user;
            _apiHelper = apiHelper;

            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo("hu-HU");
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("hu-HU");

            System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo("hu-HU");
            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("hu-HU");

            _events.SubscribeOnPublishedThread(this);

            ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken()) ;
        }

        public void ExitApplication()
        {
            TryCloseAsync();
        }

        public async void UserManagement()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>(), new CancellationToken());
        }
        
        public async void LogOut()
        {
            _user.ResetUserModel();
            _apiHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken()) ;
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public bool IsLoggedIn
        {
            get
            {
                bool output = string.IsNullOrWhiteSpace(_user.Token) ? false : true;

                return output;
            }
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesVM, cancellationToken);
            NotifyOfPropertyChange(()=>IsLoggedIn);
        }
    }
}
