namespace NumericalMethods.Integration;
public struct IntegrationInterval
{
    public double Start { get; init; }
    public double End { get; init; }
    public IntegrationInterval(double start, double end)
    {
        if (start > end)
        {
            throw new ArgumentException("The start value must be greater than the end value");
        }

        Start = start;
        End = end;
    }
}
