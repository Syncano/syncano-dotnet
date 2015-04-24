using System.Collections.Generic;
#if Unity3d
using Syncano4.Unity3d;

#endif
#if dotNET
using Syncano4.Net;
using System.Threading.Tasks;

#endif
namespace Syncano4.Shared
{
    public class PageableResult<T>
    {
        private readonly ISyncanoHttpClient _syncanoHttpClient;
        private string _linkToNext;
        private string _linkToPrevious;

        public PageableResult(ISyncanoHttpClient syncanoHttpClient, SyncanoResponse<T> response)

        {
            _syncanoHttpClient = syncanoHttpClient;
            this.Current = response.Objects;
            _linkToNext = response.Next;
            _linkToPrevious = response.Prev;
        }

#if dotNET
        public async Task<PageableResult<T>> GetNextAsync()
        {
            return await GetPage(_linkToNext);
        }

        public async Task<PageableResult<T>> GetPreviousAsync()
        {
            return await GetPage(_linkToPrevious);
        }

        private async Task<PageableResult<T>> GetPage(string link)
        {
            if (HasNext == false)
                return null;

            var response = await _syncanoHttpClient.ListAsync<T>(link, null);
            return new PageableResult<T>(_syncanoHttpClient, response);
        }
#endif

#if Unity3d

        public PageableResult<T> GetPrevious()
        {
            return GetPage(_linkToPrevious);
        }

        public PageableResult<T> GetNext()
        {
            return GetPage(_linkToNext);
        }

        private PageableResult<T> GetPage(string link)
        {
            if (HasNext == false)
                return null;

            var response = _syncanoHttpClient.List<T>(link, null);
            return new PageableResult<T>(_syncanoHttpClient, response);
        }

#endif
        public IList<T> Current { get; private set; }

        public bool HasPrevious
        {
            get { return _linkToPrevious != null; }
        }

        public bool HasNext
        {
            get { return _linkToNext != null; }
        }
    }
}