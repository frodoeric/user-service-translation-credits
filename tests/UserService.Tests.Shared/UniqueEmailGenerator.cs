namespace UserService.Tests.Shared
{
	public class UniqueEmailGenerator
	{
		private static long counter = 1;

		public static string Generate() => $"email{counter++}@domain.com";
	}
}