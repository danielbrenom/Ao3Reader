using System.Threading.Tasks;
using Ao3Domain.Models.Data;

namespace Ao3Reader.Interfaces
{
    public interface ILocalStorage
    {
        public StorageData GetStorage();
        public Task SaveFile();
        public Task LoadFile();
        public Task DeleteFile();
    }
}