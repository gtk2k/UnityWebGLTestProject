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

    private class BuildResult
    {
        public string buildResult;
    }

    public static void WebGLBuild()
    {
        var paths = GetBuildScenePaths();
        var buildOptions = BuildOptions.Development;


        var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        Debug.Log($"Documents Folder Path > {documentsPath}");
        var buildConfigPath = Path.Combine(documentsPath, "buildConfig.json");
        var buildResultPath = Path.Combine(documentsPath, "buildResult.json");
        Debug.Log($"buildConfigPath > {buildConfigPath}");
        var configJson = File.ReadAllText(buildConfigPath);
        Debug.Log($"configJson > {configJson}");
        var config = JsonUtility.FromJson<BuildConfig>(configJson);
        Debug.Log($"Output Directory > {config.outputDir}");
        Debug.Log($"LocationPathName > {config.outputDir}");
        var commitHash = config.outputDir.Substring(config.outputDir.LastIndexOf("\\") + 1);
        var buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = paths.ToArray();
        Debug.Log($"locationPathName > {config.outputDir}");
        buildPlayerOptions.locationPathName = config.outputDir;
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.options = BuildOptions.Development;

        var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);

        var summary = buildReport.summary;

        var buildResult = new BuildResult();
        if (summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log("Success");
            buildResult.buildResult = "success";
        }
        else
        {
            Debug.LogError("Error");
            buildResult.buildResult = "error";
        }
        File.WriteAllText(buildResultPath, JsonUtility.ToJson(buildResult, true));
    }

    private static IEnumerable<string> GetBuildScenePaths()
    {
        var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        return scenes
            .Where((arg) => arg.enabled)
            .Select((arg) => arg.path);
    }
}