using UnityEngine;
using System.Collections;

public class LoadSceneFromBundle : MonoBehaviour
{
    [SerializeField] private string assetBundleName = "example_bundle";
    IEnumerator Start()
    {
        // Путь к AssetBundle
        string path = "file:///" + Application.dataPath + "/AssetBundles/"+ assetBundleName ;

        // Загрузка AssetBundle
        var www = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(path);
        yield return www.SendWebRequest();

        if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            AssetBundle bundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(www);

            // Получение имени сцены и загрузка
            string[] scenePaths = bundle.GetAllScenePaths();
            UnityEngine.SceneManagement.SceneManager.LoadScene(System.IO.Path.GetFileNameWithoutExtension(scenePaths[0]));
        }
        else
        {
            Debug.LogError("Failed to load AssetBundle: " + www.error);
        }
    }

    
}