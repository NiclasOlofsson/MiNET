namespace MiNET.Config.Contracts
{
	public interface IDebugConfiguration
	{
		bool ProfilerEnabled { get; }
		string TracePacketsInclude { get; }
		string TracePacketsExclude { get; }
		int TracePacketsVerbosity { get; }
		int TracePacketsVerbosityFor(string typeName);
	}
}