using Monkey.Data;
using Puppy.DependencyInjection.Attributes;

namespace Monkey.Business.Logic
{
    [PerRequestDependency(ServiceType = typeof(IInitialBusiness))]
    public class InitialBusiness : IInitialBusiness
    {
        private readonly IConfigurationRepository _configurationRepository;

        public InitialBusiness(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public void ReBuildConfigurationCache()
        {
            _configurationRepository.ReBuildCache();
        }
    }
}