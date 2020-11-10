using System.Threading.Tasks;
using Ao3Domain.Models.Data;

namespace Ao3Reader.Interfaces
{
    public interface IReadHistoryService
    {
        public Task<bool> CheckHistory(int workId);
        public Task InitializeHistory(WorkIndexing work);
        public Task MarkChapter(int workId, string chapter);
        public Task<WorkHistory> GetWorkHistory(int workId);
        public Task<bool> CheckStatus(int workId, string chapter);
    }
}