using Bookshelf_SL.Repositories.IntermediateModelsRepositories;
using Bookshelf_SL.Repositories;
using Bookshelf_TL.Models.IntermediateModels;
using Bookshelf_TL.Models;
using Bookshelf_FL.Models.UserViewModels;
using Bookshelf_FL.Models;
using Bookshelf_FL.Models.BookViewModels;

namespace Bookshelf_FL.Extensions.Services
{
    public class FactoryOfUserService
    {
        private readonly CoverImageService _coverImageService;
        public FactoryOfUserService(CoverImageService coverImageService)
        {
            _coverImageService = coverImageService;
        }

        public UserEditViewModel UserEditViewModel(User user)
        {
            var userEditViewModel = new UserEditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Description = user.Description,
                BirthDate = user.BirthDate,
            };

            _coverImageService.FillCoverImageIntoViewModels(userEditViewModel, user);

            return userEditViewModel;
        }
        public IEnumerable<UserListViewModel> UserListViewModels(IEnumerable<User> users)
        {
            var userListViewModels = new List<UserListViewModel>();

            foreach (var user in users)
            {
                var userListViewModel = new UserListViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                };

                _coverImageService.FillCoverImageIntoViewModels(userListViewModel, user);

                userListViewModels.Add(userListViewModel);
            }

            return userListViewModels;
        }
        public UserViewModel UserViewModel(User user, IEnumerable<BookListViewModel> bookListOfUserViewModel)
        {
            var userViewModel = new UserViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Description = user.Description,

                ReadBooks = new List<BookListViewModel>(),
                CurrentlyReadingBooks = new List<BookListViewModel>(),
                WantToReadBooks = new List<BookListViewModel>(),
            };

            _coverImageService.FillCoverImageIntoViewModels(userViewModel, user);

            if (user.BirthDate != null)
            {
                userViewModel.BirthDate = user.BirthDate.Value;
            }

            var readBooks = bookListOfUserViewModel.Where(b => b.StatusOfUser == "Прочитано").ToList();
            userViewModel.ReadBooks = readBooks;

            var currentlyReadingBooks = bookListOfUserViewModel.Where(b => b.StatusOfUser == "Читаю").ToList();
            userViewModel.CurrentlyReadingBooks = currentlyReadingBooks;

            var wantToReadBooks = bookListOfUserViewModel.Where(b => b.StatusOfUser == "Хочу прочитати").ToList();
            userViewModel.WantToReadBooks = wantToReadBooks;

            return userViewModel;
        }

    }
}
