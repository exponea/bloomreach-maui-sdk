using Exponea;

namespace ExampleApp;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();

        CustomerCookie.Text = "Customer cookie: \n" + Exponea.ExponeaSDK.GetCustomerCookie();
        SessionStartButton.IsVisible = !Exponea.ExponeaSDK.IsAutomaticSessionTracking();
        SessionEndButton.IsVisible = !Exponea.ExponeaSDK.IsAutomaticSessionTracking();
    }

    async void ShowConfiguration(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ConfigInfoPage());
    }

    void TrackCustomEvent(object sender, EventArgs e)
    {
        Exponea.ExponeaSDK.TrackEvent(new Event("custom_event") { ["thisIsAStringProperty"] = "thisIsAStringValue" });
    }

    void TrackPayment(object sender, EventArgs e)
    {
        Exponea.ExponeaSDK.TrackPaymentEvent(new Payment(12.34, "EUR", "Virtual", "handbag", "Awesome leather handbag"));
    }

    void SessionStart(object sender, EventArgs e)
    {
        Exponea.ExponeaSDK.TrackSessionStart();
    }

    void SessionEnd(object sender, EventArgs e)
    {
        Exponea.ExponeaSDK.TrackSessionEnd();
    }

    void Anonymize(object sender, EventArgs e)
    {
        Exponea.ExponeaSDK.Anonymize();
        CustomerCookie.Text = "Customer cookie: \n" + Exponea.ExponeaSDK.GetCustomerCookie();
    }

    async void Flush(object sender, EventArgs e)
    {
        if (sender == null) throw new ArgumentNullException(nameof(sender));
        try
        {
            await Exponea.ExponeaSDK.FlushData();
            await DisplayAlert("Flush finished", "Flush finished", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Flush failed", ex.Message, "OK");
        }
    }

    async void IdentifyCustomer(object sender, EventArgs e)
    {
        var registered = await DisplayPromptAsync("Identify customer", "Registered");
        var propertyName = await DisplayPromptAsync("Identify customer", "Property name");
        var propertyValue = await DisplayPromptAsync("Identify customer", "Property value");
        // Preparing the data.
        if (string.IsNullOrEmpty(registered) || string.IsNullOrEmpty(propertyName) ||
            string.IsNullOrEmpty(propertyValue))
        {
            await DisplayAlert("Error", "One or more fields were left empty, skipping identifying the customer.", "OK");
            return;
        }

        var customer = new Customer(registered)
            .WithProperty(propertyName, propertyValue);
        Exponea.ExponeaSDK.IdentifyCustomer(customer);
    }

    async void Switch_Project_ClickedAsync(object sender, EventArgs e)
    {
        var projectToken = await DisplayPromptAsync("Switch project", "Project token");
        var authorization = await DisplayPromptAsync("Switch project", "Authorization");
        var baseUrl = await DisplayPromptAsync("Switch project", "Base URL");

        if (string.IsNullOrEmpty(projectToken) || string.IsNullOrEmpty(authorization) || string.IsNullOrEmpty(baseUrl))
        {
            await DisplayAlert("Error", "One or more fields were left empty, skipping switching the project.", "OK");
            return;
        }

        Exponea.ExponeaSDK.Anonymize(new Project(projectToken, authorization, baseUrl));
        CustomerCookie.Text = "Customer cookie: \n" + Exponea.ExponeaSDK.GetCustomerCookie();
        Preferences.Set("projectToken", projectToken);
        Preferences.Set("authorization", authorization);
        Preferences.Set("baseURL", baseUrl);
    }
}