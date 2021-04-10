using UnityEngine;
using UnityEditor;

public class AddNameSpace : UnityEditor.AssetModificationProcessor {

  public static void OnWillCreateAsset(string path) {
    path = path.Replace(".meta", "");
    int index = path.LastIndexOf(".");
    if (index < 0)
      return;
    string file = path.Substring(index);
    if (file != ".cs" && file != ".js" && file != ".boo")
      return;
    index = Application.dataPath.LastIndexOf("Assets");
    path = Application.dataPath.Substring(0, index) + path;
    file = System.IO.File.ReadAllText(path);

    Debug.Log("path" + path);
    string lastPart = path.Substring(path.IndexOf("Assets"));
    Debug.Log("lastPart" + lastPart);
		string v = lastPart.Replace("Assets/", "");
    Debug.Log("replace" + v);
    lastPart = lastPart.Replace("Assets/", "");
    string _namespace = lastPart.Substring(0, lastPart.LastIndexOf('/'));
    _namespace = _namespace.Replace('/', '.');
    Debug.Log("_namespace" + _namespace);
    file = file.Replace("#NAMESPACE#", _namespace);
    System.IO.File.WriteAllText(path, file);
    AssetDatabase.Refresh();
  }
}