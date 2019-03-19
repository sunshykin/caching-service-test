namespace CachingService.Model
{
	public class CacheKeyModel
	{
		public int UserId { get; set; }
		public int EntityId { get; set; }

		public override string ToString()
		{
			return $"{UserId}__{EntityId}";
		}
	}
}