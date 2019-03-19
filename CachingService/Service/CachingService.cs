using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using CachingService.Interface;
using CachingService.Model;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

namespace CachingService.Service
{
	public class CachingService : ICachingService
	{
		private readonly Mutex mutex;

		private readonly IMemoryCache cache;

		public CachingService(IMemoryCache cache)
		{
			this.cache = cache;
			this.mutex = new Mutex();
		}


		public void PutCache(CacheCreateModel model, object data)
		{
			mutex.WaitOne();

			// If value cached already - replacing it by removing and setting the new one
			if (cache.TryGetValue(model.ToString(), out object _))
			{
				cache.Remove(model.ToString());
			}

			if (model.LifeTime == 0)
				cache.Set(model.ToString(), data);
			else
				cache.Set(model.ToString(), data, TimeSpan.FromSeconds(model.LifeTime));

			mutex.ReleaseMutex();
		}

		public bool HasCache(CacheKeyModel key)
		{
			return TryGetCache(key, out object _);
		}

		public bool DeleteCache(CacheKeyModel key)
		{
			mutex.WaitOne();

			if (!cache.TryGetValue(key, out object _))
				return false;

			cache.Remove(key);
			mutex.ReleaseMutex();

			return true;
		}

		public bool TryGetCache(CacheKeyModel key, out object data)
		{
			mutex.WaitOne();
			var result = cache.TryGetValue(key.ToString(), out data);
			mutex.ReleaseMutex();

			return result;
		}

		public object GetCache(CacheKeyModel key)
		{
			if (TryGetCache(key, out object data))
				return data;

			return null;
		}

		public long GetCacheLength(CacheKeyModel key)
		{
			var data = GetCache(key);

			if (data is JObject obj)
				return obj.ToString().Length;

			return 0;
		}
	}
}