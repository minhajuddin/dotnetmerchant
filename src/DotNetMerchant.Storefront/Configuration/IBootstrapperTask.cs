namespace DotNetMerchant.Storefront.Configuration
{
    public interface IBootstrapperTask
    {
        void Execute();
        bool HasExecuted { get; }
    }
}