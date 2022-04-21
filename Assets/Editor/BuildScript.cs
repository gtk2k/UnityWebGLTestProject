using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

public static class BuildScript
{
    private class BuildConfig
    {
        public string outputDir;
    }

    public static void WebGLBuild()
    {
        var paths = GetBuildScenePaths();
        var buildOptions = BuildOptions.Development;


        var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        Debug.Log($"Documents Folder Path > {documentsPath}");
        var buildConfigPath = Path.Combine(documentsPath, "buildConfig.json");
        Debug.Log($"buildConfigPath > {buildConfigPath}");
        var configJson = File.ReadAllText(buildConfigPath);
        Debug.Log($"configJson > {configJson}");
        var config = JsonUtility.FromJson<BuildConfig>(configJson);
        Debug.Log($"Output Directory > {config.outputDir}");

        var buildReport = BuildPipeline.BuildPlayer(
            paths.ToArray(),
            $"{config.outputDir}/{PlayerSettings.productName}",
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