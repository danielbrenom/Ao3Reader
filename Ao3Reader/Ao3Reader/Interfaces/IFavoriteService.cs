using System.Threading.Tasks;
using Ao3Domain.Models.Data;

namespace Ao3Reader.Interfaces
{
    public interface IFavoriteService
    {
        public Task<bool> CheckFavorites(Work work);
        public Task AddToFavorites(Work work);

        public Task RemoveFromFavorites(Work work);
    }
}