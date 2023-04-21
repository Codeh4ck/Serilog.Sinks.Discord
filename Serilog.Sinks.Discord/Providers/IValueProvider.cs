namespace Serilog.Sinks.Discord.Providers
{
    public interface IValueProvider<T, TParam>
    {
        T Provide(TParam param);
    }
}