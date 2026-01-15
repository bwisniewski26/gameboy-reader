namespace GameBoyReader.GUI
{
    public partial class App : Application
    {
        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new MainPage());
            window.MinimumHeight = 720;
            window.MinimumWidth = 1280;
            window.Page = new MainPage();
            return window;
        }

        public App()
        {
            InitializeComponent();
        }
    }
}
