using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;

using UnityEngine.UI;

//[ExecuteInEditMode]
public class GrasshopperInUnity : MonoBehaviour
{
    public float receivedArea;

    public GrasshopperInUnity() // constructor. it is called everytime an instance of this class is created. It is used to register the rhino callbacks
    {
        //Debug.Log("Object created");
       
        Rhino.Runtime.HostUtils.RegisterNamedCallback("Unity:GetList", GetList);
       

    }

  
 

 
    Rhino.Geometry.GeometryBase[] _base;
    void GetList(object sender, Rhino.Runtime.NamedParametersEventArgs args)
    {
        string name = null;
        string gname;
        Rhino.Geometry.GeometryBase[] values;

        //get the geometries
        if (args.TryGetGeometry("mesh", out values))
            _base = values;

        //get the name to assign to a game object
        if (args.TryGetString("name", out gname))
            name = gname;

        if (_base.Length > 0 && name != null)
        {
            Rhino.Geometry.Mesh mesh = new Rhino.Geometry.Mesh();

            for (int i = 0; i < _base.Length; i++)
            {
                mesh.Append((Rhino.Geometry.Mesh)_base[i]);
            }

            var obj = GameObject.Find(name);
            if (obj == null)
            {
                obj = new GameObject(name);
                obj.AddComponent<MeshFilter>();
                obj.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));

            }
            else
            {
                var filter = obj.GetComponent<MeshFilter>();
                if (filter == null)
                    obj.AddComponent<MeshFilter>();

                var render = obj.GetComponent<MeshRenderer>();
                if (render == null)
                    obj.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
            }
            obj.GetComponent<MeshFilter>().mesh = mesh.ToHost();
        }

    }
    //public static Stopwatch m_stopwatch = new Stopwatch();
    
              

    public void SendNumberToUnity(double num1, double num2)
    {
       
        using (var args = new Rhino.Runtime.NamedParametersEventArgs())
        {
            
            args.Set("number1", num1);
            args.Set("number2", num2);
            Rhino.Runtime.HostUtils.ExecuteNamedCallback("GH:GetSliders", args);
            
           
        }
       // Debug.Log("Function is ending");
    }
   
    

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

