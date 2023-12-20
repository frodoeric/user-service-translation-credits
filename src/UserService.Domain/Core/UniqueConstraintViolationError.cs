namespace UserService.Domain.Core;

public class UniqueConstraintViolationError : Error
{
	/// <summary>
	/// Property which must be unique
	/// </summary>
	public string Entity { get; set; } = null!;

	/// <summary>
	/// Property which must be unique
	/// </summary>
	public string Property { get; set; } = null!;

	public UniqueConstraintViolationError(string message, string entity, string property)
		: base(message)
	{
		Entity = entity;
		Property = property;
	}
}