using CachingService.Model;

namespace CachingService.Interface
{
	public interface ICachingService
	{
		void PutCache(CacheCreateModel model, object data);

		bool HasCache(CacheKeyModel key);

		bool DeleteCache(CacheKeyModel key);

		bool TryGetCache(CacheKeyModel key, out object data);

		object GetCache(CacheKeyModel key);

		long GetCacheLength(CacheKeyModel key);
	}
}