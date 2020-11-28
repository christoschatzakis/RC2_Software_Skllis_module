using System;
using System.Reflection;
using System.IO;

using UnityEngine;
using UnityEditor;

using Rhino;
using Rhino.Runtime.InProcess;

public class RhinoInside
{

    static System.IDisposable _rhinoCore;

    // This is a method to launch Rhino in a similar way as open it from the command line
    public static void Launch()
    {


        if (_rhinoCore == null)
        {
            string rhinoSystemDir = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\McNeel\Rhinoceros\7.0\Install",
              "Path",
              System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Rhino 7", "System")
              ); // this returns the location of rhino in your system 

            //Debug.Log(rhinoSystemDir);

            //Debug.Log(Environment.GetEnvironmentVariable("PATH"));

            //Debug.Log(Environment.GetEnvironmentVariable("PATH").Contains(rhinoSystemDir));

            if (!Environment.GetEnvironmentVariable("PATH").Contains(rhinoSystemDir))
            {
                var PATH = Environment.GetEnvironmentVariable("PATH");
                Environment.SetEnvironmentVariable("PATH", PATH + ";" + rhinoSystemDir);
            }
            // 
            //    opens rhino with specific arguments
            IntPtr hParent = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            _rhinoCore = new RhinoCore(new string[] { "/scheme=Unity", "/nosplash" }, WindowStyle.Normal, hParent);

        }
        GC.SuppressFinalize(_rhinoCore);


    }
}
