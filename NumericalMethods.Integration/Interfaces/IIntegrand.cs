namespace NumericalMethods.Integration.Interfaces;
/// <summary>
/// Подыинтегральная функция
/// </summary>
public interface IIntegrand
{
    public double Calculate(double argument);
}
