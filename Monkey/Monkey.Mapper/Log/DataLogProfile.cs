using AutoMapper;
using Monkey.Core.Entities.Log;
using Monkey.Core.Models.Log;
using Puppy.AutoMapper;

namespace Monkey.Mapper.Log
{
    public class DataLogProfile : Profile
    {
        public DataLogProfile()
        {
            CreateMap<DataLogEntity, DataLogModel>().IgnoreAllNonExisting();
        }
    }
}