namespace Inventory_Elederos_IT13
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "Inventory_Elederos_IT13" };
        }
    }
}
