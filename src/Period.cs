namespace Collaboration;

public record struct Period
{
    private Period(int day, int month, int year)
        => (Day, Month, Year) = (day, month, year);

    public int Day { get; }
    public int Month { get; }
    public int Year { get; }

    public static Period CreateFrom(DateTime date)
        => new(date.Day, date.Month, date.Year);

    public static Period CreateCurrent()
        => CreateFrom(DateTime.Now);

    public override readonly string ToString()
        => $"{Day}/{Month}/{Year}";
}
