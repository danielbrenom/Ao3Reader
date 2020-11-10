using System.Linq;
using System.Threading.Tasks;
using Ao3Domain.Models.Data;
using Ao3Reader.Interfaces;

namespace Ao3Reader.Services
{
    public class ReadHistoryService : IReadHistoryService
    {
        private ILocalStorage _localStorage;

        public ReadHistoryService(ILocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public Task<bool> CheckHistory(int workId)
        {
            return Task.Run(() => _localStorage.GetStorage().ReadedChapters.Exists(works => works.WorkId == workId));
        }

        public Task InitializeHistory(WorkIndexing work)
        {
            return Task.Run(() =>
            {
                var workHistory = new WorkHistory{WorkId = work.WorkId};
                work.Chapters.ForEach(chapter =>
                {
                    workHistory.ChapterRead.Add(chapter.Id,false);
                });
                _localStorage.GetStorage().ReadedChapters.Add(workHistory);
                _localStorage.SaveFile();
            });
        }

        public Task MarkChapter(int workId, string chapter)
        {
            return Task.Run(() =>
            {
                var workIndex = _localStorage.GetStorage().ReadedChapters.FindIndex(workHistory => workHistory.WorkId == workId);
                _localStorage.GetStorage().ReadedChapters[workIndex].ChapterRead[chapter] = true;
                _localStorage.SaveFile();
            });
        }

        public Task<WorkHistory> GetWorkHistory(int workId)
        {
            return Task.Run(() => _localStorage.GetStorage().ReadedChapters.FirstOrDefault(workHistory => workHistory.WorkId == workId));
        }

        public Task<bool> CheckStatus(int workId, string chapter)
        {
            return Task.Run(() =>
            {
                var workIndex = _localStorage.GetStorage().ReadedChapters.FindIndex(workHistory => workHistory.WorkId == workId);
                return _localStorage.GetStorage().ReadedChapters[workIndex].ChapterRead[chapter];
            });
        }
    }
}