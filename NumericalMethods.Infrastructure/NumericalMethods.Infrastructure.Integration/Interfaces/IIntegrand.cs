namespace NumericalMethods.Infrastructure.Integration.Interfaces;
/// <summary>
/// Подыинтегральная функция
/// </summary>
public interface IIntegrand
{
	public double Calculate(double argument);
}
