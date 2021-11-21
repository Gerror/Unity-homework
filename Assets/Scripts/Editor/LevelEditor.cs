using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Editor 
{ 
    public class LevelEditor : EditorWindow
    {
        private LevelConfig _levelConfig;
        
        private Vector2 scrollPos;
        private bool[,] fieldsArray = new bool[0,0];
        
        private int width = 1;
        private int height = 1;
        private string levelFileName = "level";

        private const string _levelPath = "Assets/Resources/Levels";
     
        [MenuItem("Window/LevelEditor")]
        public static void ShowWindow()
        {
            EditorWindow window = (EditorWindow) EditorWindow.GetWindow(typeof(LevelEditor), true, "Level editor");
            window.Show();
        }
     
        void OnGUI()
        {
            GUILayout.Label ("Level saving", EditorStyles.boldLabel);
            GUILayout.Label($"Save path: {_levelPath}/'Level filename'.asset");
            levelFileName = EditorGUILayout.TextField("Level filename", levelFileName);
            
            GUILayout.Label ("Level width/height", EditorStyles.boldLabel);
            width = EditorGUILayout.IntField ("Width", width);
            height = EditorGUILayout.IntField ("Height", height);
 
            if (width != fieldsArray.GetLength(0) || height != fieldsArray.GetLength(1)) {
                fieldsArray = new bool[width, height];
            }

            if (GUILayout.Button ("Save level")) {
                SaveLevel();
            }
            if (GUILayout.Button ("Load level"))
            {
                LoadLevel();
            }
            
            RenderField();
        }

        
        private void SaveLevel()
        {
            if (levelFileName == "")
            {
                Debug.LogWarning("Enter filename!");
                return;
            }
            
            LevelConfig currentLevelConfig = AssetDatabase.LoadAssetAtPath<LevelConfig>($"{_levelPath}/{levelFileName}.asset");

            if (currentLevelConfig)
                _levelConfig = currentLevelConfig;
            else
            {
                _levelConfig = ScriptableObject.CreateInstance<LevelConfig>();
                AssetDatabase.CreateAsset(_levelConfig, $"{_levelPath}/{levelFileName}.asset");
            }
            
            _levelConfig.Points = new List<Vector3>();
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++) {
                    if (fieldsArray[x, z])
                    {
                        _levelConfig.Points.Add(new Vector3(x, 1, height - z - 1));
                    }
                }
            }

            _levelConfig.width = width;
            _levelConfig.height = height;
            
            EditorUtility.SetDirty(_levelConfig);
            AssetDatabase.SaveAssets();
        }

        private void LoadLevel()
        {
            if (levelFileName == "")
            {
                Debug.LogWarning("Enter filename!");
                return;
            }

            _levelConfig = AssetDatabase.LoadAssetAtPath<LevelConfig>($"{_levelPath}/{levelFileName}.asset");

            if (_levelConfig == null)
            {
                Debug.LogWarning("File not found:" + $"{_levelPath}/{levelFileName}.asset");
                return;
            }
            
            width = _levelConfig.width;
            height = _levelConfig.height;
            
            fieldsArray = new bool[width, height];
            
            foreach (var point in _levelConfig.Points)
            {
                fieldsArray[(int) point.x, height - (int) point.z - 1] = true;
            }
        }
 
        private void RenderField()
        {
            GUILayout.BeginVertical();
            
            scrollPos = GUILayout.BeginScrollView(scrollPos,false,true);

            for (int y = 0; y < height; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < width; x++) {
                    fieldsArray[x, y] = EditorGUILayout.Toggle(fieldsArray[x, y]);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}