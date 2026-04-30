using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace EditorTools.StageManagement
{
    public class StageEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset stageBrowserAsset;
        [SerializeField] private VisualTreeAsset stageInspectorAsset;
        
        
        private const string STAGE_CONFIGS_DIRECTORY = "Assets/Editor/Stage/SObjects";
        private ReactiveProperty<StageConfigAsset> _currentStageConfig;
        private VisualElement _stageContent;
        
        [MenuItem("Window/General/Stage")]
        public static void ShowExample()
        {
            var wnd = GetWindow<StageEditorWindow>();
            wnd.titleContent = new GUIContent("Stage");
        }

        public void CreateGUI()
        {
            _currentStageConfig = new ReactiveProperty<StageConfigAsset>();
            InitializeBrowser();
            
            EditorApplication.projectChanged -= OnProjectChanged;
            EditorApplication.projectChanged += OnProjectChanged;

            _currentStageConfig.Subscribe(InitializeStageInspector);
            return;

            void OnProjectChanged()
            {
                _currentStageConfig.Value = null;
                InitializeStagesList();
            }
        }

        private void InitializeBrowser()
        {
            var stageBrowser = stageBrowserAsset.CloneTree().Q<VisualElement>("STAGE_BROWSER");
            rootVisualElement.Add(stageBrowser);
            _stageContent = rootVisualElement.Q<VisualElement>("StageContent");
            InitializeStagesList();
            InitializeSceneToStageButton();
        }

        private void InitializeStagesList()
        {
            var stagesList = rootVisualElement.Q<ListView>("StagesList");
            var stages = GetAllStages();
            stagesList.itemsSource = stages;
            
            stagesList.selectionChanged -= OnSelectionChanged;
            stagesList.ClearSelection();
            stagesList.selectionChanged += OnSelectionChanged;
            return;

            void OnSelectionChanged(IEnumerable<object> selection)
            {
                var config = selection.FirstOrDefault() as StageConfigAsset;
                _currentStageConfig.Value = config;
            }
        }

        private void InitializeSceneToStageButton()
        {
            var sceneToStageButton = rootVisualElement.Q<Button>("SceneToStageButton");
            sceneToStageButton.clicked -= OnButtonClicked;
            sceneToStageButton.clicked += OnButtonClicked;
            
            return;

            void OnButtonClicked()
            {
                var currentSceneName = SceneManager.GetActiveScene().name;
                if (string.IsNullOrEmpty(currentSceneName))
                { Debug.LogError("Scene should be saved and set into project scenes"); return; }
                var stages = GetAllStages();
                var sameNameStage = stages.FirstOrDefault(stage => stage.name == currentSceneName);
                if (sameNameStage != null) { _currentStageConfig.Value = sameNameStage; return; }
                var newStage = CreateInstance<StageConfigAsset>();
                newStage.name = currentSceneName;
                AssetDatabase.CreateAsset(newStage, $"{STAGE_CONFIGS_DIRECTORY}/{newStage.name}.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                InitializeStagesList();
                var stagesList = rootVisualElement.Q<ListView>("StagesList");
                var newStageIndex = stagesList.itemsSource.IndexOf(newStage);
                stagesList.SetSelection(newStageIndex);
            }
        }
        
        private void InitializeStageInspector(StageConfigAsset config)
        {
            var currentInspector = _stageContent.Q<VisualElement>("STAGE_INSPECTOR");
            if (currentInspector != null) _stageContent.Remove(currentInspector);
            if (config == null) return;
            var inspector = stageInspectorAsset.CloneTree().Q<VisualElement>("STAGE_INSPECTOR");
            _stageContent.Add(inspector);

            var stageObjectField = inspector.Q<ObjectField>("StageObjectField");
            stageObjectField.value = config;
            
            var unityInspectorContainer = inspector.Q<VisualElement>("UnityInspectorContainer");
            var so = new SerializedObject(config);
            var unityInspector = new InspectorElement(so);
            unityInspectorContainer.Add(unityInspector);

            var refreshConfigButton = inspector.Q<Button>("RefreshConfigButton");
            var configToJsonButton = inspector.Q<Button>("ConfigToJsonButton");
            var deleteConfigButton = inspector.Q<Button>("DeleteConfigButton");
            
            refreshConfigButton.clicked -= OnRefreshButtonClicked;
            refreshConfigButton.clicked += OnRefreshButtonClicked;
            
            configToJsonButton.clicked -= OnToJsonButtonClicked;
            configToJsonButton.clicked += OnToJsonButtonClicked;
            
            deleteConfigButton.clicked -= OnDeleteButtonClicked;
            deleteConfigButton.clicked += OnDeleteButtonClicked;

            return;

            void OnRefreshButtonClicked()
            {
                SceneToStagePacker.Pack(config);
            }

            void OnToJsonButtonClicked()
            {
                
            }

            void OnDeleteButtonClicked()
            {
                
            }
        }

        private List<StageConfigAsset> GetAllStages()
        {
            var guids = AssetDatabase.FindAssets("", new []{STAGE_CONFIGS_DIRECTORY});
            var stages = guids.Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<StageConfigAsset>)
                .ToList();
            return stages;
        }
    }
}
