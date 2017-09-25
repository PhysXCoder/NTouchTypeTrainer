using Caliburn.Micro;
using NTouchTypeTrainer.Common.Files;
using NTouchTypeTrainer.Common.Gui;
using NTouchTypeTrainer.Common.Sound;
using NTouchTypeTrainer.Interfaces.Common.Files;
using NTouchTypeTrainer.Interfaces.Common.Gui;
using NTouchTypeTrainer.Interfaces.Common.Sound;
using NTouchTypeTrainer.Interfaces.View;
using NTouchTypeTrainer.ViewModels;
using NTouchTypeTrainer.Views;
using NTouchTypeTrainer.Views.Common;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace NTouchTypeTrainer.Bootstrapping
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly IContainer _diContainer;

        public Bootstrapper()
        {
            Initialize();
            _diContainer = new Container(RegisterMappings);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainWindowViewModel>();
        }

        protected override object GetInstance(Type service, string key) =>
            (key != null)
                ? _diContainer.GetInstance(service, key)
                : _diContainer.GetInstance(service);

        protected override IEnumerable<object> GetAllInstances(Type service) =>
            _diContainer.GetAllInstances(service).Cast<object>();

        protected override void BuildUp(object instance) =>
            _diContainer.BuildUp(instance);

        private static void RegisterMappings(ConfigurationExpression configExpression)
        {
            configExpression.For<IEventAggregator>().Use<EventAggregator>().Singleton();

            configExpression.For<IFileStreamProvider>().Use<FileStreamProvider>();
            configExpression.For<IFileReaderWriter<string>>().Use<StringFileReaderWriter>();

            configExpression.For<ISoundPlayer>().Use<SoundPlayer>();

            configExpression.For<IDialogProvider>().Use<DialogProvider>().Singleton();
            configExpression.For<IWindowManager>().Use<WindowManager>().Singleton();
            configExpression.For<IThemeProvider>().Use<DefaultThemeProvider>().Singleton();

            configExpression.For<ISizeGroup>().Use<SharedSizeGroup>();
        }
    }
}