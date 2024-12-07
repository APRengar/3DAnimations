using UnityEditor;
using UnityEngine;

public class BuildAllAssetBundles
{
    [MenuItem("Assets/Build AssetBundles with Platform Selection")]
    static void BuildBundles()
    {
        // Платформы, поддерживаемые в сборке
        string[] platforms = { "Android", "iOS", "WebGL" };
        
        // Показываем окно для выбора платформы
        int selectedPlatform = EditorUtility.DisplayDialogComplex(
            "Выбор платформы",
            "На какую платформу запаковать AssetBundle?",
            "Android", // Кнопка 0
            "iOS",     // Кнопка 1
            "WebGL"    // Кнопка 2
        );

        BuildTarget targetPlatform;

        // Определяем выбранную платформу
        switch (selectedPlatform)
        {
            case 0: // Android
                targetPlatform = BuildTarget.Android;
                break;
            case 1: // iOS
                targetPlatform = BuildTarget.iOS;
                break;
            case 2: // WebGL
                targetPlatform = BuildTarget.WebGL;
                break;
            default:
                Debug.LogWarning("Сборка отменена пользователем.");
                return; // Отмена
        }

        // Папка, куда сохраняем AssetBundles
        string assetBundleDirectory = "Assets/AssetBundles/" + platforms[selectedPlatform];
        if (!System.IO.Directory.Exists(assetBundleDirectory))
        {
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        }

        // Сборка AssetBundles для выбранной платформы
        BuildPipeline.BuildAssetBundles(
            assetBundleDirectory,
            BuildAssetBundleOptions.None,
            targetPlatform
        );

        Debug.Log($"AssetBundles собраны для платформы {platforms[selectedPlatform]} и сохранены в {assetBundleDirectory}");
    }
}