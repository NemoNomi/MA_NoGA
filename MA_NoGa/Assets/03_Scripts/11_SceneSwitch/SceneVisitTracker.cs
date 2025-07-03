using System.Collections.Generic;

public static class SceneVisitTracker
{
    private static readonly HashSet<int> _visited = new();

    public static bool IsVisited(int buildIndex) => _visited.Contains(buildIndex);
    public static int VisitedCount => _visited.Count;
    public static void MarkVisited(int buildIndex) => _visited.Add(buildIndex);
}
