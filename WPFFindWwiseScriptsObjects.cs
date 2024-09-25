using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WPFFindWwiseScriptsObjects : EditorWindow
{
    // Liste des scripts à rechercher
    private List<System.Type> scriptTypesToFind = new List<System.Type>
    {
        typeof(WwiseAnimationCSV),
        typeof(WwiseAnimationEvent),
        typeof(WwiseRotationControlEvent),
        typeof(WwiseRotationControlEventSynthPreset),
        typeof(WwiseSpeedControlEvent),
        typeof(WwiseSpeedControlEventSynthPreset)
    };

    // Liste pour stocker les résultats
    private List<GameObject> foundObjects = new List<GameObject>();

    [MenuItem("WPF/Find Wwise Synth Scripts Objects")]
    public static void ShowWindow()
    {
        // Ouvrir la fenêtre de l'éditeur
        GetWindow<WPFFindWwiseScriptsObjects>("Find Wwise Synth Scripts Objects");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find Objects"))
        {
            FindAndLogObjectsWithScripts();
        }

        // Afficher les résultats
        if (foundObjects.Count > 0)
        {
            GUILayout.Label("Found Objects:", EditorStyles.boldLabel);
            foreach (GameObject obj in foundObjects)
            {
                if (GUILayout.Button(obj.name))
                {
                    // Sélectionner l'objet dans la hiérarchie
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
        // Réinitialiser la liste des objets trouvés
        foundObjects.Clear();

        // Obtenir tous les objets dans la scène active, y compris ceux désactivés
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

        // Parcourir tous les objets pour vérifier les scripts
        foreach (GameObject obj in allObjects)
        {
            CheckObjectForScripts(obj);
        }
    }

    private void CheckObjectForScripts(GameObject obj)
    {
        // Vérifier si l'objet contient un des scripts spécifiés
        foreach (var scriptType in scriptTypesToFind)
        {
            var component = obj.GetComponent(scriptType);
            if (component != null)
            {
                // Ajouter l'objet à la liste des résultats
                if (!foundObjects.Contains(obj))
                {
                    foundObjects.Add(obj);
                }
            }
        }

        // Parcourir tous les enfants de l'objet
        foreach (Transform child in obj.transform)
        {
            CheckObjectForScripts(child.gameObject); // Appel récursif pour les enfants
        }
    }
}
