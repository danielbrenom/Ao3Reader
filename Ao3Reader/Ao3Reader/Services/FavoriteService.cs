using System.Threading.Tasks;
using Ao3Domain.Models.Data;
using Ao3Reader.Interfaces;

namespace Ao3Reader.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly ILocalStorage _localStorage;

        public FavoriteService(ILocalStorage localStorage)
        {
            _localStorage = localStorage;
        }
        
        public Task<bool> CheckFavorites(Work work)
        {
            return Task.Run(() => _localStorage.GetStorage().FavoriteWorks.Exists(favorite => favorite.WorkId == work.WorkId));
        }

        public Task AddToFavorites(Work work)
        {
            return Task.Run(() =>
            {
                _localStorage.GetStorage().FavoriteWorks.Add(work);
                _localStorage.SaveFile();
            });
        }

        public Task RemoveFromFavorites(Work work)
        {
            return Task.Run(() =>
            {
                var index = _localStorage.GetStorage().FavoriteWorks.FindIndex(favorites => favorites.WorkId == work.WorkId);
                _localStorage.GetStorage().FavoriteWorks.RemoveAt(index);
                _localStorage.SaveFile();
            });
        }
    }
}