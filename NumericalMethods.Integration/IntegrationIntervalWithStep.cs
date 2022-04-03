namespace NumericalMethods.Integration;
public struct IntegrationIntervalWithStep
{
    public double Start { get; init; }
    public double End { get; init; }
    public double Step { get; init; }
    public IntegrationIntervalWithStep(double start, double end, double step)
    {
        if (start < end)
        {
            throw new ArgumentException("The start value must be greater than the end value");
        }

        Start = start;
        End = end;
        Step = step;
    }
}
