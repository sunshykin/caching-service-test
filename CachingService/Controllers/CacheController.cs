using System.Collections.Generic;
using CachingService.Interface;
using CachingService.Model;
using Microsoft.AspNetCore.Mvc;

namespace CachingService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CacheController : ControllerBase
	{
		private readonly ICachingService cachingService;

		public CacheController(ICachingService cachingService)
		{
			this.cachingService = cachingService;
		}

		[HttpGet]
		public ActionResult<IEnumerable<string>> Get([FromQuery] int userId, [FromQuery] int entityId)
		{
			var result = cachingService.GetCache(new CacheKeyModel { UserId = userId, EntityId = entityId });

			if (result == null)
				return NotFound();

			return Ok(result);
		}

		[HttpHead]
		public IActionResult Head([FromQuery] int userId, [FromQuery] int entityId)
		{
			Response.ContentLength = cachingService.GetCacheLength(new CacheKeyModel { UserId = userId, EntityId = entityId });

			return Ok();
		}

		[HttpPut]
		public void Put([FromBody] object body, [FromQuery] int userId, [FromQuery] int entityId, [FromQuery] int lifeTime = 0)
		{
			var data = new CacheCreateModel { EntityId = entityId, UserId = userId, LifeTime = lifeTime };

			cachingService.PutCache(data, body);
		}

		[HttpDelete]
		public IActionResult Delete([FromQuery] int userId, [FromQuery] int entityId)
		{
			if (!cachingService.DeleteCache(new CacheKeyModel { UserId = userId, EntityId = entityId }))
				return NotFound();

			return Ok();
		}
	}
}
