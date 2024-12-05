using UnityEngine;
using System.IO;

public class ObjExporterExample : MonoBehaviour
{
    public GameObject objectToExport;

    void Start()
    {
        string filePath = Path.Combine(Application.dataPath, "ExportedModel.obj");
        ObjExporter.ExportToObj(objectToExport, filePath);
    }
}