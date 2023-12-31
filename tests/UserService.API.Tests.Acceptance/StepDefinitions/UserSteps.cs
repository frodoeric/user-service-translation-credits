using UserService.API.Contract;
using UserService.API.Contract.Users;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Azure;

namespace UserService.API.Test.Acceptance.StepDefinitions;

[Binding]
public class UserSteps : StepsBase
{
    public UserSteps(ScenarioContext scenarioContext, TestFixture fixture) : base(scenarioContext, fixture) { }

    public const string GivenUsersKey = "GivenUsers";
    public const string RequestBodyKey = "RequestBody";
    public const string HttpResponseKey = "HttpResponse";
    public const string IdKey = "Id";

    [Given(@"an existing user")]
    public async Task GivenAnExistingUser()
    {
        var givenUsers = new List<User>();
        if (_scenarioContext.ContainsKey(GivenUsersKey))
            givenUsers.AddRange((List<User>)_scenarioContext[GivenUsersKey]);

        var givenUser = new UserBuilder()
            .Build();

        await SaveEntity(givenUser);

        givenUsers.Add(givenUser);
        _scenarioContext[GivenUsersKey] = givenUsers;
        _scenarioContext[IdKey] = givenUser.Id;
    }

    [Given(@"an existing user with (.*) credits")]
    public async Task GivenAnExistingUserWithCredits(int credits)
    {
        var givenUsers = new List<User>();
        if (_scenarioContext.ContainsKey(GivenUsersKey))
            givenUsers.AddRange((List<User>)_scenarioContext[GivenUsersKey]);

        var givenUser = new UserBuilder()
            .Build();
        givenUser.AddCredits(credits);

        await SaveEntity(givenUser);

        givenUsers.Add(givenUser);
        _scenarioContext[GivenUsersKey] = givenUsers;
        _scenarioContext[IdKey] = givenUser.Id;
    }

    [Given(@"two existing users")]
    public async Task GivenTwoExistingUsers()
    {
        await GivenAnExistingUser();
        await GivenAnExistingUser();
    }

    [Given(@"a UserCreationRequest with the same email")]
    public void GivenAUserCreationRequestWithTheSameEmail()
    {
        var user = ((List<User>)_scenarioContext[GivenUsersKey]).First();
        var givenRequest = new UserCreationRequestBuilder()
            .WithEmail(user.Email)
            .Build();

        _scenarioContext[RequestBodyKey] = givenRequest;
    }

    [Given(@"a UserCreationRequest")]
    public void GivenAUserCreationRequest()
    {
        var givenRequest = new UserCreationRequestBuilder()
            .Build();

        _scenarioContext[RequestBodyKey] = givenRequest;
    }

    [Given(@"a UserUpdateRequest")]
    public void GivenAUserUpdateRequest()
    {
        var givenRequest = new UserUpdateRequestBuilder()
            .Build();

        _scenarioContext[RequestBodyKey] = givenRequest;
    }

    [Given(@"a UserUpdateRequest with invalid email")]
    public void GivenAUserUpdateRequestWithInvalidEmail()
    {
        var givenRequest = new UserUpdateRequestBuilder()
            .WithEmail("invalid")
            .Build();

        _scenarioContext[RequestBodyKey] = givenRequest;
    }

    [Given(@"a UserUpdateRequest with invalid name")]
    public void GivenAUserUpdateRequestWithInvalidName()
    {
        var givenRequest = new UserUpdateRequestBuilder()
            .WithName("DavidAlejandroAlexanderMichaelChristopherBenjamin" +
            "WilliamsonFrederickJonathanGregorySamuelHarringtonMontgomery")
            .Build();

        _scenarioContext[RequestBodyKey] = givenRequest;
    }

    [Given(@"a TranslationCreditsRequest with (.*) credits")]
    public void GivenATranslationCreditsRequestWithCredits(int credits)
    {
        var givenRequest = new TranslationCreditsRequestBuilder()
            .WithCredits(credits)
            .Build();

        _scenarioContext[RequestBodyKey] = givenRequest;
    }

    [When(@"I send a (.*) request to (.*)")]
    public async Task ISendARequestTo(string verb, string path)
    {
        if (path.Contains("{id}"))
            path = path.Replace("{id}", _scenarioContext[IdKey].ToString());

        var request = new HttpRequestMessage(new HttpMethod(verb), path);

        if (verb.ToLower() == "post" || verb.ToLower() == "put")
            request.Content = JsonContent.Create(_scenarioContext[RequestBodyKey]);

        var response = await _fixture.HttpClient.SendAsync(request);

        _scenarioContext[HttpResponseKey] = response;
    }

    [Then(@"I get a Bad Request response with an error message")]
    public void ThenIGetABadRequestReponseWithAnErrorMessage()
    {
        var httpResponse = (HttpResponseMessage)_scenarioContext[HttpResponseKey];

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var contentStream = httpResponse.Content.ReadAsStream();
        var response = JsonSerializer.Deserialize<ErrorResponse>(contentStream, _jsonOptions);
        response.Should().NotBeNull();
        response!.Code.Should().NotBeNullOrWhiteSpace();
        response!.Message.Should().NotBeNullOrWhiteSpace();
    }

    [Then(@"I get an OK response")]
    public void ThenIGetAnOkResponse()
    {
        var httpResponse = (HttpResponseMessage)_scenarioContext[HttpResponseKey];

        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Then(@"I get the users")]
    public void ThenIGetTheUsers()
    {
        var httpResponse = (HttpResponseMessage)_scenarioContext[HttpResponseKey];

        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var contentStream = httpResponse.Content.ReadAsStream();
        var response = JsonSerializer.Deserialize<IEnumerable<UserResponse>>(contentStream, _jsonOptions);
        response.Should().NotBeNull();

        var givenUsers = (List<User>)_scenarioContext[GivenUsersKey];
        foreach (var user in givenUsers)
        {
            var userResponse = response!.FirstOrDefault(r => r.Id == user.Id);
            userResponse.Should().NotBeNull();
            AssertEquivalent(userResponse!, user);
        }
    }

    [Then(@"I get the user")]
    public void ThenIGetTheUser()
    {
        var httpResponse = (HttpResponseMessage)_scenarioContext[HttpResponseKey];

        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var contentStream = httpResponse.Content.ReadAsStream();
        var response = JsonSerializer.Deserialize<UserResponse>(contentStream, _jsonOptions);
        response.Should().NotBeNull();

        var givenUser = ((List<User>)_scenarioContext[GivenUsersKey]).First();
        AssertEquivalent(response!, givenUser);

    }

    [Then(@"the user is persisted")]
    public async Task ThenTheUserIsPersisted()
    {
        var httpResponse = (HttpResponseMessage)_scenarioContext[HttpResponseKey];
        var content = await httpResponse.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(content);
        if (jsonDoc.RootElement.TryGetProperty("id", out var idElement) && idElement.TryGetInt64(out var id))
        {
            var user = await GetEntity<User>(id);
            user.Should().NotBeNull();
            var userCreationRequest = (UserCreationRequest)_scenarioContext[RequestBodyKey];
            userCreationRequest.Should().NotBeNull();
            AssertEquivalent(userCreationRequest!, user!);
        }
    }

    [Then(@"the user is updated")]
    public async Task ThenTheUserIsUpdated()
    {
        var httpResponse = (HttpResponseMessage)_scenarioContext[HttpResponseKey];
        var content = await httpResponse.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(content);
        if (jsonDoc.RootElement.TryGetProperty("id", out var idElement) && idElement.TryGetInt64(out var id))
        {
            var user = await GetEntity<User>(id);
            user.Should().NotBeNull();
            var userUpdateRequest = (UserUpdateRequest)_scenarioContext[RequestBodyKey];
            userUpdateRequest.Should().NotBeNull();
            AssertEquivalent(userUpdateRequest!, user!);
        }
    }

    [Then(@"the user's credits will be equal to (.*)")]
    public void ThenTheUserSCreditsAreIncreasedBy(int credits)
    {
        var httpResponse = (HttpResponseMessage)_scenarioContext[HttpResponseKey];

        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var contentStream = httpResponse.Content.ReadAsStream();
        var response = JsonSerializer.Deserialize<CreditsResponse>(contentStream, _jsonOptions);

        AssertEquivalent(response!, credits);
    }

    private static void AssertEquivalent(UserResponse response, User user)
    {
        response.Id.Should().Be(user.Id);
        response.Name.Should().Be(user.Name);
        response.Email.Should().Be(user.Email);
    }

    private static void AssertEquivalent(UserCreationRequest request, User user)
    {
        request.Name.Should().Be(user.Name);
        request.Email.Should().Be(user.Email);
    }

    private static void AssertEquivalent(UserUpdateRequest request, User user)
    {
        request.Name.Should().Be(user.Name);
        request.Email.Should().Be(user.Email);
    }

    private static void AssertEquivalent(CreditsResponse response, int credits)
    {
        response.Should().NotBeNull();
        response.Credits.Should().Be(credits);
    }

}
