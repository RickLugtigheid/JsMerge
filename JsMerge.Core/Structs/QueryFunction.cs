namespace JsMerge.Core
{
	public struct QueryFunction
	{
		public readonly string name;
		public readonly string[] parameters;

		public QueryFunction(string name, string[] parameters)
		{
			this.name = name;
			this.parameters = parameters;
		}
	}
}