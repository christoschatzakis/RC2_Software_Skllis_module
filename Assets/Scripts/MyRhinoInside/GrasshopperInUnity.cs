using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEngine.UI;

//[ExecuteInEditMode]
public class GrasshopperInUnity : MonoBehaviour
{

    public GrasshopperInUnity() // constructor. it is called everytime an instance of this class is created. It is used to register the rhino callbacks
    {
        Rhino.Runtime.HostUtils.RegisterNamedCallback("Unity:FromGrasshopper", FromGrasshopper);
        Rhino.Runtime.HostUtils.RegisterNamedCallback("Unity:GetStr", GetStr);
        Rhino.Runtime.HostUtils.RegisterNamedCallback("Unity:GetNumber", GetNumber);
        Rhino.Runtime.HostUtils.RegisterNamedCallback("Unity:GetPoint", GetPoint);
        Rhino.Runtime.HostUtils.RegisterNamedCallback("Unity:GetList", GetList);
    }

    public Text text;    


    Rhino.Geometry.Mesh _mesh;
    Rhino.Geometry.GeometryBase[] _base;

    // This function will be called from a component in Grasshopper
    void FromGrasshopper(object sender, Rhino.Runtime.NamedParametersEventArgs args)
    {
        Rhino.Geometry.GeometryBase[] values;
        if (args.TryGetGeometry("mesh", out values))
            _mesh = values[0] as Rhino.Geometry.Mesh;
        if (_mesh != null)
            gameObject.GetComponent<MeshFilter>().mesh = _mesh.ToHost();
    }

    void GetStr(object sender, Rhino.Runtime.NamedParametersEventArgs args)
    {
        string values;
        if (args.TryGetString("word", out values))
            text.text = values;
    }

    void GetNumber(object sender, Rhino.Runtime.NamedParametersEventArgs args)
    {
        string name = null;
        string gname;
        double values;
        float num = float.NaN;

        if (args.TryGetDouble("num", out values))
            num = (float)values;

        if (args.TryGetString("name", out gname))
            name = gname;

        //Debug.Log(name +" "+num.ToString());

        if (num != float.NaN && name != null)
        {
            var textobj = GameObject.Find(name);
            if (textobj != null)
            {
                textobj.GetComponent<Text>().text = num.ToString();
            }
        }
    }

    void GetPoint(object sender, Rhino.Runtime.NamedParametersEventArgs args)
    {
        Vector3 point;
        Rhino.Geometry.Point3d values;
        
        string name;
        string gname;
        Debug.Log(args.TryGetPoint("point", out values));
        Debug.Log(args.TryGetString("name", out gname));
        Debug.Log(args.TryGetPoint("point", out values) && args.TryGetString("name", out gname));

        if (args.TryGetPoint("point", out values) && args.TryGetString("name", out gname))
        {
            
            point = values.ToHost();
            name = gname;

            GameObject randomObject = GameObject.Find(name);
            Debug.Log(randomObject);
            if (randomObject == null)
            {
                randomObject = new GameObject(name);
            }
            randomObject.GetComponent<Transform>().position = point;
        }
       
          

    }

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

    public void SendNumberToUnity(float sliderValue)
    {
        using (var args = new Rhino.Runtime.NamedParametersEventArgs())
        {
            args.Set("number", sliderValue);
            Rhino.Runtime.HostUtils.ExecuteNamedCallback("GH:GetNumber", args);
        }
    }

    //// Start is called before the first frame update
    //void Start()
    //{
    //    //gameObject.AddComponent<MeshFilter>();

    //    //gameObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"))
    //    //{
    //    //    color = new Color(0.5f, 0.0f, 0.0f, 1f)
    //    //};

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    var pt = Camera.main.gameObject.transform.position.ToRhino();
    //    using (var args = new Rhino.Runtime.NamedParametersEventArgs())
    //    {
    //        args.Set("point", new Rhino.Geometry.Point(pt));
    //        Rhino.Runtime.HostUtils.ExecuteNamedCallback("ToGrasshopper", args);
    //    }

    //}

}

