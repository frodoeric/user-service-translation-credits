namespace UserService.Domain;

public abstract class ValueObject<T> : ValueObject where T : IComparable
{
	public T Value { get; internal set; }

	protected ValueObject(T value)
	{
		Value = value;
	}

	public static implicit operator T(ValueObject<T> o) => o.Value;

	protected override IEnumerable<IComparable> GetEqualityComponents()
	{
		yield return Value;
	}
}