using BoDi;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)] // Disable parallel feature execution
namespace UserService.API.Test.Acceptance;

[Binding]
internal class Initializer
{
	private readonly IObjectContainer _objectContainer;
	private readonly TestFixture _testFixture;

	public Initializer(IObjectContainer objectContainer)
	{
		_objectContainer = objectContainer;
		_testFixture = new TestFixture();
	}

	[BeforeScenario]
	public void RegisterContext()
	{
		_objectContainer.RegisterInstanceAs(_testFixture);
	}
}
