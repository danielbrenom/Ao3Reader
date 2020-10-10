using System.Collections.Generic;
using System.Threading.Tasks;
using Ao3Domain.Models.Data;
using Ao3Domain.Models.Response;
using Ao3Reader.Interfaces;

namespace Ao3Reader.Services
{
    public class WorksService : IWorksService
    {
        protected IHttpService _httpService;
        public WorksService(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<List<Work>> LoadDiscoverWorks()
        {
            var works = await _httpService.Get("/works").ExecuteAsync<WorkListResponse>();
            return works.Works;
        }

        public Task<List<Work>> SearchWorks(string search)
        {
            throw new System.NotImplementedException();
        }

        public Task<Work> GetWork(int workId)
        {
            throw new System.NotImplementedException();
        }

        public Task<WorkChapter> GetWorkChapter(int workId, int chapterId)
        {
            throw new System.NotImplementedException();
        }
    }
}