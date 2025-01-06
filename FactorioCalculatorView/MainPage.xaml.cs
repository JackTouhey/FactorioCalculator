namespace FactorioCalculatorView;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnInputClicked(object sender, EventArgs e)
	{
		AddInput();
	}

	private void AddInput(){
		Label TestLabel = new Label();
		TestLabel.Text = "Working";
		InputItems.Add(TestLabel);
	}
}

