#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> dung.nguyen </Author>
//     <Project> Monkey → Service Facade </Project>
//     <File>
//         <Name> ImageService.cs </Name>
//         <Created> 10/19/2017 02:30:33 PM </Created>
//         <Key> f27712f9-3bba-4c4d-a616-9c475abf0649 </Key>
//     </File>
//     <Summary>
//         ImageService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System.Threading;
using Puppy.DependencyInjection.Attributes;
using Monkey.Business;
using System.Threading.Tasks;
using Monkey.Core.Models;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;

namespace Monkey.Service.Facade
{
    [PerRequestDependency(ServiceType = typeof(IImageService))]
    public class ImageService: IImageService
	{
        private readonly IImageBusiness _imageBusiness;
        public ImageService(IImageBusiness imageBusiness)
        {
            _imageBusiness = imageBusiness;
        }

	    public Task<int> CreateAsync(ImageAddModel model, CancellationToken cancellationToken = new CancellationToken())
	    {
	        return _imageBusiness.CreateAsync(model, cancellationToken);
        }

	    public Task UpdateAsync(ImageAddModel model, CancellationToken cancellationToken = new CancellationToken())
	    {
	        _imageBusiness.CheckExist(model.Id);
	        return _imageBusiness.UpdateAsync(model, cancellationToken);
        }

	    public Task<ImageModel> GetAsync(int id, CancellationToken cancellationToken = new CancellationToken())
	    {
	        _imageBusiness.CheckExist(id);
	        return _imageBusiness.GetAsync(id, cancellationToken);
        }

	    public Task<DataTableResponseDataModel> GetDataTableAsync(DataTableParamModel model, CancellationToken cancellationToken = new CancellationToken())
	    {
	        return _imageBusiness.GetDataTableAsync(model, cancellationToken);
	    }

	    public Task RemoveAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
	    {
	        _imageBusiness.CheckExist(id);
	        return _imageBusiness.RemoveAsync(id, cancellationToken);
        }
	}
}