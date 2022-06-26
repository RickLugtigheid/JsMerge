using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsMerge.Core
{
	public struct QueryResult
	{
		public readonly QueryResultType type;
		public readonly object? result;

		public QueryResult(QueryResultType type, object? result)
		{
			this.type = type;
			this.result = result;
		}
		
		/// <summary>
		/// Gets an empty query result
		/// </summary>
		public static QueryResult Empty => new(QueryResultType.Empty, null);
	}
}
