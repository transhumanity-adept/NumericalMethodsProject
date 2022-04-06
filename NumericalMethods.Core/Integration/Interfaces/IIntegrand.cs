namespace NumericalMethods.Core.Integration.Interfaces;
/// <summary>
/// Подыинтегральная функция
/// </summary>
public interface IIntegrand
{
    public double Calculate(double argument);
}
