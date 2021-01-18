using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OpenGH : MonoBehaviour
{
    static bool _firstRun = true;

    [MenuItem("Grasshopper/Show...")]
    public static void ShowGrasshopperWindow()
    {

        string script = "!_-Grasshopper _W _T ENTER"; // rhinoscript command to open grasshopper
        if (_firstRun)
        {
            _firstRun = false;
            RhinoInside.Launch();

            var ghWatcher = GameObject.Find("Grasshopper Geometry");
            if (ghWatcher == null)
            {
                ghWatcher = new GameObject("Grasshopper Geometry");
                ghWatcher.AddComponent<GrasshopperInUnity>();
            }


        }
        Rhino.RhinoApp.RunScript(script, false); // runs the script to open gh
    }
}
