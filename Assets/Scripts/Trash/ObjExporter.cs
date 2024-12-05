using System.IO;
using UnityEngine;

public class ObjExporter : MonoBehaviour
{
    public static void ExportToObj(GameObject gameObject, string filePath)
    {
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.Write(MeshToString(gameObject));
        }
        Debug.Log($"Объект успешно экспортирован в {filePath}");
    }

    private static string MeshToString(GameObject gameObject)
    {
        MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
        StringWriter stringWriter = new StringWriter();

        int vertexOffset = 0;
        int normalOffset = 0;
        int uvOffset = 0;

        foreach (MeshFilter mf in meshFilters)
        {
            Mesh mesh = mf.sharedMesh;
            if (mesh == null) continue;

            // Позиции вершин
            foreach (Vector3 vertex in mesh.vertices)
            {
                Vector3 transformedVertex = mf.transform.TransformPoint(vertex);
                stringWriter.WriteLine($"v {transformedVertex.x} {transformedVertex.y} {transformedVertex.z}");
            }

            // Нормали
            foreach (Vector3 normal in mesh.normals)
            {
                Vector3 transformedNormal = mf.transform.TransformDirection(normal);
                stringWriter.WriteLine($"vn {transformedNormal.x} {transformedNormal.y} {transformedNormal.z}");
            }

            // UV координаты
            foreach (Vector2 uv in mesh.uv)
            {
                stringWriter.WriteLine($"vt {uv.x} {uv.y}");
            }

            // Фейсы (грани)
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                int v1 = mesh.triangles[i] + 1 + vertexOffset;
                int v2 = mesh.triangles[i + 1] + 1 + vertexOffset;
                int v3 = mesh.triangles[i + 2] + 1 + vertexOffset;
                stringWriter.WriteLine($"f {v1}/{v1}/{v1} {v2}/{v2}/{v2} {v3}/{v3}/{v3}");
            }

            vertexOffset += mesh.vertexCount;
            normalOffset += mesh.normals.Length;
            uvOffset += mesh.uv.Length;
        }

        return stringWriter.ToString();
    }
}