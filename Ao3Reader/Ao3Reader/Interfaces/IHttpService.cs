

using System.Threading.Tasks;

namespace Ao3Reader.Interfaces
{
    public interface IHttpService
    {
        public Task<T> ExecuteAsync<T>();
        public IHttpService AddHeader(string name, string value);
        public IHttpService AddBody(string name, string value);
        public IHttpService AddQuery(string name, string value);
        public IHttpService AddPath(string name, string value);
        public IHttpService Get(string url);
        public IHttpService Post(string url);
        public IHttpService AddParameter(string Type, string name, object value);
    }
}