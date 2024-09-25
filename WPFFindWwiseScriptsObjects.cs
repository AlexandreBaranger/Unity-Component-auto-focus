using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WPFFindWwiseScriptsObjects : EditorWindow
{
    // Liste des scripts � rechercher
    private List<System.Type> scriptTypesToFind = new List<System.Type>
    {
        typeof(WwiseAnimationCSV),
        typeof(WwiseAnimationEvent),
        typeof(WwiseRotationControlEvent),
        typeof(WwiseRotationControlEventSynthPreset),
        typeof(WwiseSpeedControlEvent),
        typeof(WwiseSpeedControlEventSynthPreset)
    };

    // Liste pour stocker les r�sultats
    private List<GameObject> foundObjects = new List<GameObject>();

    [MenuItem("WPF/Find Wwise Synth Scripts Objects")]
    public static void ShowWindow()
    {
        // Ouvrir la fen�tre de l'�diteur
        GetWindow<WPFFindWwiseScriptsObjects>("Find Wwise Synth Scripts Objects");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find Objects"))
        {
            FindAndLogObjectsWithScripts();
        }

        // Afficher les r�sultats
        if (foundObjects.Count > 0)
        {
            GUILayout.Label("Found Objects:", EditorStyles.boldLabel);
            foreach (GameObject obj in foundObjects)
            {
                if (GUILayout.Button(obj.name))
                {
                    // S�lectionner l'objet dans la hi�rarchie
                    Selection.activeGameObject = obj;
                    EditorGUIUtility.PingObject(obj);
                }
            }
        }
        else
        {
            GUILayout.Label("No objects found.");
        }
    }

    private void FindAndLogObjectsWithScripts()
    {
        // R�initialiser la liste des objets trouv�s
        foundObjects.Clear();

        // Obtenir tous les objets dans la sc�ne active, y compris ceux d�sactiv�s
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

        // Parcourir tous les objets pour v�rifier les scripts
        foreach (GameObject obj in allObjects)
        {
            CheckObjectForScripts(obj);
        }
    }

    private void CheckObjectForScripts(GameObject obj)
    {
        // V�rifier si l'objet contient un des scripts sp�cifi�s
        foreach (var scriptType in scriptTypesToFind)
        {
            var component = obj.GetComponent(scriptType);
            if (component != null)
            {
                // Ajouter l'objet � la liste des r�sultats
                if (!foundObjects.Contains(obj))
                {
                    foundObjects.Add(obj);
                }
            }
        }

        // Parcourir tous les enfants de l'objet
        foreach (Transform child in obj.transform)
        {
            CheckObjectForScripts(child.gameObject); // Appel r�cursif pour les enfants
        }
    }
}
