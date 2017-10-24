#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> dung.nguyen </Author>
//     <Project> Monkey → Business Logic </Project>
//     <File>
//         <Name> ImageBusiness.cs </Name>
//         <Created> 10/19/2017 02:27:43 PM </Created>
//         <Key> fa228e4a-bfa4-474f-9d63-f45f370788b9 </Key>
//     </File>
//     <Summary>
//         ImageBusiness.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System.Threading;
using System.Threading.Tasks;
using Monkey.Core.Entities;
using Monkey.Core.Exceptions;
using Monkey.Core.Models;
using System.Linq;
using Monkey.Data;
using Puppy.AutoMapper;
using Puppy.DataTable;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;
using Puppy.DependencyInjection.Attributes;

namespace Monkey.Business.Logic
{
    [PerRequestDependency(ServiceType = typeof(IImageBusiness))]
    public class ImageBusiness: IImageBusiness
	{
		private readonly IImageRepository _imageRepository;

        public ImageBusiness(IImageRepository imageRepository)
        {
        	_imageRepository = imageRepository;
        }

	    public Task<int> CreateAsync(AddImageModel model, CancellationToken cancellationToken = new CancellationToken())
	    {
	        var result = _imageRepository.SaveImage(model.File, model.Caption, model.ImageDominantHexColor);

	        cancellationToken.ThrowIfCancellationRequested();

	        return Task.FromResult(result.Id);
        }

	    public Task UpdateAsync(UpdateImageModel model, CancellationToken cancellationToken = new CancellationToken())
	    {
            //TODO: improvement update image without delete image
            _imageRepository.SaveImage(model.File, model.Caption, model.ImageDominantHexColor);
	        _imageRepository.Delete(new ImageEntity
	        {
	            Id = model.Id
	        });
            cancellationToken.ThrowIfCancellationRequested();

	        _imageRepository.SaveChanges();
            return Task.CompletedTask;
        }

	    public Task<ImageModel> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
	    {
	        var model = _imageRepository.Get(x => x.Id == id).QueryTo<ImageModel>().FirstOrDefault();
	        return Task.FromResult(model);
        }

	    public Task<DataTableResponseDataModel> GetDataTableAsync(DataTableParamModel model, CancellationToken cancellationToken = new CancellationToken())
	    {
	        var listData = _imageRepository.Get().QueryTo<ImageModel>();

	        var result = listData.GetDataTableResponse(model);

	        return Task.FromResult(result);
        }

	    public void CheckExist(params int[] ids)
	    {
	        ids = ids.Distinct().ToArray();
	        int totalInDb = _imageRepository.Get(x => ids.Contains(x.Id)).Count();
	        if (totalInDb != ids.Length)
	        {
	            throw new MonkeyException(ErrorCode.ClientNotFound);
	        }
        }

	    public Task RemoveAsync(int id, CancellationToken cancellationToken = new CancellationToken())
	    {
	        _imageRepository.Delete(new ImageEntity
	        {
	            Id = id
	        });

	        // Check cancellation token
	        cancellationToken.ThrowIfCancellationRequested();

	        _imageRepository.SaveChanges();

	        return Task.CompletedTask;
        }
	}
}