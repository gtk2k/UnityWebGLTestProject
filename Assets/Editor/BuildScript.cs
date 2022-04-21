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
        public string outputDir { get; set; }
    }

    public static void WebGLBuild()
    {
        var paths = GetBuildScenePaths();
        var buildOptions = BuildOptions.Development;


        var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        Debug.Log($"Documents Folder Path > {documentsPath}");

        var configJson = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "buildConfig.json"));
        var config = JsonUtility.FromJson<BuildConfig>(configJson);
        Debug.Log($"config > {config}");
        Console.WriteLine($"Output Directory > {config.outputDir}");

        var buildScriptLogFilePath = Path.Combine(config.outputDir, "buildScript.log");
        File.AppendAllLines(buildScriptLogFilePath, new[] {
            $"Documents Folder Path > {documentsPath}",
            $"Output Directory > {config.outputDir}",
            $"Output Path > {config.outputDir}/{PlayerSettings.productName}"
        });

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