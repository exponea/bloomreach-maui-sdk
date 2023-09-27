using Microsoft.Maui.Controls;

namespace ExampleApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}

