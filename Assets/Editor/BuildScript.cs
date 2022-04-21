using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class BuildScript
{
    public static void WebGLBuild()
    {
        var paths = GetBuildScenePaths();
        var buildOptions = BuildOptions.Development;

        var buildReport = BuildPipeline.BuildPlayer(
            paths.ToArray(),
            $"./WebGL_Auto_Build/{PlayerSettings.productName}",
            BuildTarget.WebGL,
            buildOptions
        );

        var summary = buildReport.summary;

        if (summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log("Success");
        }
        else
        {
            Debug.LogError("Error");
        }
    }
    private static IEnumerable<string> GetBuildScenePaths()
    {
        var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        return scenes
            .Where((arg) => arg.enabled)
            .Select((arg) => arg.path);
    }
}