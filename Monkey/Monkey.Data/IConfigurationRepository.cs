using Monkey.Core.Constants;
using Monkey.Core.Entities;
using System.Collections.Generic;

namespace Monkey.Data
{
    public interface IConfigurationRepository : IEntityRepository<ConfigurationEntity>
    {
        T Get<T>(Configuration.Key key);

        Dictionary<Configuration.Key, string> GetByKeys(params Configuration.Key[] keys);

        Dictionary<Configuration.Key, string> GetByGroups(params Configuration.KeyGroup[] groups);

        void ReBuildCache();
    }
}