#nullable enable

using UnityEditor;

/**
 * Fix for Unity generating CS projects incorrectly which causes Visual Studio intellisense
 * to fail. See:
 * https://issuetracker.unity3d.com/issues/referenceoutputassembly-key-is-set-to-false-in-project-references
 * http://answers.unity.com/answers/1727077/view.html
 */
public sealed class UnityCsProjectGenerationFix : AssetPostprocessor
{
    private static string OnGeneratedCSProject(string path, string content)
    {
        return content.Replace(
            "<ReferenceOutputAssembly>false</ReferenceOutputAssembly>",
            "<ReferenceOutputAssembly>true</ReferenceOutputAssembly>"
        );
    }
}