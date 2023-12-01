using Bookshelf_FL.ViewModels;
using Bookshelf_FL.ViewModels.UserViewModels;
using Bookshelf_SL.Repositories;
using Bookshelf_TL.Models;
using System.Diagnostics;
using System.Security.Principal;

namespace Bookshelf_FL.Extensions.Services
{
    public class CoverImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CoverImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> AddCover(IFormFile CoverImage, string folderName)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(CoverImage.FileName);

            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, $@"uploads\{folderName}");

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var filePath = Path.Combine(uploads, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await CoverImage.CopyToAsync(stream);
            }

            // Оновлення шляху до нової обкладинки
            string coversPath = $"~/uploads/{folderName}/" + uniqueFileName;

            return coversPath;
        }
        public void DeleteCover<T>(IRepository<T> repository, T baseEntity) where T : class, IEntity
        {
            if (baseEntity.CoverPath != null)
            {
                var baseDir = new DirectoryInfo(@".");
                string fullCoverPath = Path.Combine(baseDir.FullName + @"\wwwroot" + baseEntity.CoverPath.Replace('/', '\\').TrimStart('~'));

                if (File.Exists(fullCoverPath))
                {
                    File.Delete(fullCoverPath);

                    baseEntity.CoverPath = "";

                    repository.Update(baseEntity);
                }
                else
                {
                    throw new Exception("The file does not exist or the path is incorrect");
                }
            }
        }
        public ICoverImageViewModel FillCoverImageIntoViewModels(ICoverImageViewModel viewModel, IEntity baseEntity)
        {
            if (!string.IsNullOrEmpty(baseEntity.CoverPath))
            {
                var baseDir = new DirectoryInfo(@".");
                string fullCoverPath = Path.Combine(baseDir.FullName + @"\wwwroot" + baseEntity.CoverPath.Replace('/', '\\').TrimStart('~'));

                if (File.Exists(fullCoverPath))
                {
                    viewModel.CoverPath = baseEntity.CoverPath;
                }
            }
            else
            {
                viewModel.CoverPath = baseEntity.GetDefaultCover();
            }

            return viewModel;
        }
    }
}
