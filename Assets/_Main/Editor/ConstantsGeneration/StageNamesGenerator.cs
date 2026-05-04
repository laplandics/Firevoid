using UnityEditor;
using System.IO;
using System.Linq;

namespace EditorTools.ConstantsGeneration
{
    public static class StageNamesGenerator
    {
        private const string STAGE_SCENES_FOLDER = "Assets/_Main/Scenes/Stages";
        private const string STAGE_NAMES_PATH = "Assets/_Main/Scripts/Constant/StageNames.cs";

        [MenuItem("Tools/Generate StageNames")]
        public static void Generate()
        {
            var scenesGuids = AssetDatabase.FindAssets("", new[] {STAGE_SCENES_FOLDER});
            var scenes = new string[scenesGuids.Length];
            for (var guidIndex = 0; guidIndex < scenesGuids.Length; guidIndex++)
            {
                var sceneAssetPath = AssetDatabase.GUIDToAssetPath(scenesGuids[guidIndex]);
                var sceneAssetName = Path.GetFileNameWithoutExtension(sceneAssetPath);
                scenes[guidIndex] = sceneAssetName;
            }
            
            EnumGenerator.GenerateEnum(STAGE_NAMES_PATH, "StageNames", scenes.ToList());
        }
    }
}
