#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Interface </Project>
//     <File>
//         <Name> ISearchableRepository.cs </Name>
//         <Created> 09 May 17 11:28:57 AM </Created>
//         <Key> 808d32f2-8e13-44a3-bcd8-53edd6c3d066 </Key>
//     </File>
//     <Summary>
//         ISearchableRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Monkey.Core;
using Monkey.Data.Entities;
using System.Collections.Generic;

namespace Monkey.Data.Interfaces
{
    public interface ISearchableRepository<TElastic, TKey> where TElastic : Puppy.EF.Entity, IBaseElastic<TKey> where TKey : struct
    {
        /// <summary>
        ///     Initial Elastic Map if not exist 
        /// </summary>
        void InitElasticMap();

        /// <summary>
        ///     Check is Elastic Map already exist or not 
        /// </summary>
        /// <returns></returns>
        bool IsExistElasticMap();

        /// <summary>
        ///     Force Re-Initial Elastic Map even Exist 
        /// </summary>
        void ReInitElasticMap();

        /// <summary>
        ///     AddUpdate all document from database to Elastic 
        /// </summary>
        void ReInitAllElastic(int pageSize = 2000);

        /// <summary>
        ///     AddUpdate list of elastic 
        /// </summary>
        /// <param name="listElastic"></param>
        void AddUpdateElastic(IEnumerable<TElastic> listElastic);

        /// <summary>
        ///     Delete list of elastic by id 
        /// </summary>
        /// <param name="listElasticId"></param>
        void DeleteElastic(IEnumerable<int> listElasticId);

        /// <summary>
        ///     Delete elastic map 
        /// </summary>
        void DeleteElasticMap();

        /// <summary>
        ///     Get value with paging 
        /// </summary>
        /// <param name="total"></param>
        /// <param name="skip"> </param>
        /// <param name="take"> </param>
        /// <returns></returns>
        IEnumerable<TElastic> GetElastic(out int total, int skip, int take = Constants.ElasticSearch.MaxTakeRecord);

        /// <summary>
        ///     Get value by field name in list value 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="listValue"></param>
        /// <returns></returns>
        IEnumerable<TElastic> GetElastic(string fieldName, List<object> listValue);

        bool IsExistElastic(object id);

        TElastic GetElastic(object id);

        void SaveElastics<TEntity>(List<TEntity> listEntityAddUpdate, List<TEntity> listEntityDelete) where TEntity : Puppy.EF.Entity;
    }
}