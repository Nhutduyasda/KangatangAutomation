using NUnit.Framework;

// Disable parallel execution to avoid race conditions with shared Singletons (ExtentReports, WebDriver log context)
[assembly: LevelOfParallelism(1)]
[assembly: Parallelizable(ParallelScope.None)]
