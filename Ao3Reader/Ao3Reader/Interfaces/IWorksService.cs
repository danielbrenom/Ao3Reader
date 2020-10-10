using System.Collections.Generic;
using System.Threading.Tasks;
using Ao3Domain.Models.Data;

namespace Ao3Reader.Interfaces
{
    public interface IWorksService
    {
        public Task<List<Work>> LoadDiscoverWorks();
        public Task<List<Work>> SearchWorks(string search);
        public Task<Work> GetWork(int workId);
        public Task<WorkChapter> GetWorkChapter(int workId, int chapterId);
    }
}