using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Drawing;//


using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Collections;

static class Convert
{
    #region ToRhino
    public static Point3d ToRhino(this Vector3 p) => new Point3d((double)p.x, (double)p.z, (double)p.y);

    static public IEnumerable<Point3d> ToRhino(this ICollection<Vector3> points)
    {
        var result = new List<Point3d>(points.Count);
        foreach (var p in points)
            result.Add(p.ToRhino());

        return result;
    }

    #endregion

    #region ToHost

    // no worries, just using expressions to write the method. https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-operator

    //static public Vector3 ToHost(this Point3d p) => new Vector3((float)p.X, (float)p.Z, (float)p.Y); 
    static public Vector3 ToHost(this Point3d p) 
    {
        return new Vector3((float)p.X, (float)p.Z, (float)p.Y);
    }
    
    static public Vector3 ToHost(this Point3f p) => new Vector3(p.X, p.Z, p.Y);
    static public Vector3 ToHost(this Vector3d p) => new Vector3((float)p.X, (float)p.Z, (float)p.Y);
    static public Vector3 ToHost(this Vector3f p) => new Vector3(p.X, p.Z, p.Y);

    static public Color ToHost(this System.Drawing.Color c) => new Color(c.R,c.G,c.B);



    static public List<Vector3> ToHost(this ICollection<Point3f> points)
    {
        var result = new List<Vector3>(points.Count);
        foreach (var p in points)
            result.Add(p.ToHost());

        return result;
    }

    static public List<Color> ToHost(this MeshVertexColorList colors)
    {
        var result = new List<Color>();
        foreach (var c in colors)
            result.Add(c.ToHost());
        return result;
    }

    static public List<Vector3> ToHost(this ICollection<Vector3f> vectors)
    {
        var result = new List<Vector3>(vectors.Count);
        foreach (var p in vectors)
            result.Add(p.ToHost());

        return result;
    }


    static public UnityEngine.Mesh ToHost(this Rhino.Geometry.Mesh _mesh)
    {
        var result = new UnityEngine.Mesh();
        using (var mesh = _mesh.DuplicateMesh())
        {
            mesh.Faces.ConvertQuadsToTriangles();

            result.SetVertices(mesh.Vertices.ToHost());
            result.SetNormals(mesh.Normals.ToHost());
            result.SetColors(mesh.VertexColors.ToHost());

            int i = 0;
            int[] indices = new int[mesh.Faces.Count * 3];
            foreach (var face in mesh.Faces)
            {
                indices[i++] = (face.C);
                indices[i++] = (face.B);
                indices[i++] = (face.A);
            }



            result.SetIndices(indices, MeshTopology.Triangles, 0);

        }
        
        return result;
    }
    #endregion
}
