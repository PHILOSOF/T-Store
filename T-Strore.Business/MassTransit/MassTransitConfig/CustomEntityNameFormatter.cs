using MassTransit;
namespace T_Strore.Business.MassTransit.MassTransitConfig;

public class CustomEntityNameFormatter : IEntityNameFormatter
{
    public string FormatEntityName<T>()
    {
        return $"Contracts.{typeof(T).Name}";
    }
}
