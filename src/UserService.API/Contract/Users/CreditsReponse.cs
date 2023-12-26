namespace UserService.API.Contract.Users;

public class CreditsResponse
{
    public int Credits { get; set; }

    public static CreditsResponse From(int credits) =>
        new CreditsResponse
		{
            Credits = credits
        };
}
