using System.Collections.Generic;
using System.Threading.Tasks;
using Ao3Domain.Models.Data;
using Ao3Domain.Models.Request;
using Ao3Domain.Models.Response;
using Ao3Reader.Extensions;
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

        public async Task<List<Work>> SearchWorks(string search)
        {
            var works = await _httpService
                .AddQuery("query", search)
                .Get("/works/search")
                .ExecuteAsync<WorkListResponse>();
            return works.Works;
        }

        public async Task<WorkIndexing> GetWork(int workId)
        {
            var work = await _httpService.Get($"/works/{workId}").ExecuteAsync<WorkResponse>();
            return work.getAs<WorkIndexing>();
        }

        public async Task<WorkChapter> GetWorkChapter(int workId, int chapterId)
        {
            var work = await _httpService.Get($"/works/{workId}/chapters/{chapterId}").ExecuteAsync<WorkChapter>();
            return work;
        }
    }
}