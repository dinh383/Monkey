namespace Monkey.Business
{
    public interface IInitialBusiness : IBaseBusiness
    {
        void ReBuildConfigurationCache();
    }
}