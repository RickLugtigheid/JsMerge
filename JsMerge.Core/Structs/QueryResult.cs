namespace JsMerge.Core
{
	public struct QueryResult
	{
		public readonly QueryResultType type;
		public readonly object? value;
		public readonly string origin;

		public QueryResult(QueryResultType type, object? value, string origin = "")
		{
			this.type = type;
			this.value = value;
			this .origin = origin;
		}
		
		/// <summary>
		/// Gets an empty query result
		/// </summary>
		public static QueryResult Empty => new(QueryResultType.Empty, null);
	}
}
