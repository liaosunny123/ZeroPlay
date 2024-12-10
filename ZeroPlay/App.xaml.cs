using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using ZeroPlay.Interface;
using ZeroPlay.Service;
using ZeroPlay.ShareModel;
using ZeroPlay.View;
using ZeroPlay.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZeroPlay
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public IHost Host { get; private set; }

        public static T? GetRequiredService<T>() where T : class => (Application.Current as App)?.Host.Services.GetService<T>();

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {

            this.InitializeComponent();

            // Configure Host for Dependency Injection
            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                   .UseContentRoot(AppContext.BaseDirectory)
                   .ConfigureServices((ctx, services) =>
                   {
                       // Page
                       services.AddSingleton<HomePage, HomePage>();
                       services.AddSingleton<SettingsPage, SettingsPage>();
                       services.AddSingleton<ProfilePage, ProfilePage>();
                       services.AddSingleton<CommentTestPage, CommentTestPage>();
                       services.AddSingleton<ChatPage, ChatPage>();


                       // ViewModel
                       services.AddSingleton<HomeViewModel, HomeViewModel>();
                       services.AddSingleton<ProfileViewModel, ProfileViewModel>();
                       services.AddSingleton<CommentTestViewModel, CommentTestViewModel>();
                       services.AddSingleton<ChatViewModel, ChatViewModel>();


                       // ShareModel
                       services.AddSingleton<UserDataShareModel, UserDataShareModel>();

                       // Service
                       services.AddSingleton<IZeroPlayService, ZeroPlayClient>();
                       services.AddSingleton<ICommentService, CommentClient>();
                       services.AddSingleton<IChatService, ChatClient>();
                   })
                   .Build();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window? m_window;
    }
}
