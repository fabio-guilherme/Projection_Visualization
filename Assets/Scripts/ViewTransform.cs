using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Visualization
{
    OrthographicProjection,
    CameraTransformation,
    PerspectiveProjection
}

public class ViewTransform : MonoBehaviour
{

    public World world;
    private float left_plane = 5f;
    private float right_plane = -5f;
    private float botton_plane = -5f;
    private float top_plane = 5f;
    private float near_plane = -1f;
    private float far_plane = -11f;

    private Texture2D frameBuffer;

    public Vector3 eye;
    public Vector3 gaze;
    public Vector3 up;

    public Visualization visualization;

    void Start()
    {
        world = GameObject.FindWithTag("World").GetComponent<World>();
        frameBuffer = new Texture2D(Screen.width, Screen.height);
    }

    void OnGUI()
    {

        Matrix4x4 mvp = new Matrix4x4();
        mvp.SetRow(0, new Vector4(Screen.width / 2f, 0f, 0f, (Screen.width - 1) / 2f));
        mvp.SetRow(1, new Vector4(0f, Screen.height / 2f, 0f, (Screen.height - 1) / 2f));
        mvp.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
        mvp.SetRow(3, new Vector4(0f, 0f, 0f, 1f));

        Matrix4x4 morth = new Matrix4x4();
        morth.SetRow(0, new Vector4(2f / (right_plane - left_plane), 0f, 0f,
                    -((right_plane + left_plane) / (right_plane - left_plane))));
        morth.SetRow(1, new Vector4(0f, 2f / (top_plane - botton_plane), 0f,
                    -((top_plane + botton_plane) / (top_plane - botton_plane))));
        morth.SetRow(2, new Vector4(0f, 0f, 2f / (near_plane - far_plane),
                    -((near_plane + far_plane) / (near_plane - far_plane))));
        morth.SetRow(3, new Vector4(0f, 0f, 0f, 1f));

        Vector3 w = -gaze.normalized;
        Vector3 u = Vector3.Cross(up, w).normalized;
        Vector3 v = Vector3.Cross(w, u);

        Matrix4x4 mcam = new Matrix4x4();
        mcam.SetRow(0, new Vector4(u.x, u.y, u.z,
                         -((u.x * eye.x) + (u.y * eye.y) + (u.z * eye.z))));
        mcam.SetRow(1, new Vector4(v.x, v.y, v.z,
                         -((v.x * eye.x) + (v.y * eye.y) + (v.z * eye.z))));
        mcam.SetRow(2, new Vector4(w.x, w.y, w.z,
                         -((w.x * eye.x) + (w.y * eye.y) + (w.z * eye.z))));
        mcam.SetRow(3, new Vector4(0, 0, 0, 1));

        if (visualization == Visualization.OrthographicProjection)
        {
            ResetViewVolume();
        }
        else
        {
            UpdateViewVolume(eye);
        }

        Matrix4x4 mper = new Matrix4x4();
        mper.SetRow(0, new Vector4(near_plane, 0f, 0f, 0f));
        mper.SetRow(1, new Vector4(0f, near_plane, 0f, 0f));
        mper.SetRow(2, new Vector4(0f, 0f, near_plane + far_plane,
                                                -(far_plane * near_plane)));
        mper.SetRow(3, new Vector4(0f, 0f, 1f, 0f));

        Matrix4x4 m = mvp;
        switch (visualization)
        {
            case Visualization.OrthographicProjection:
                {
                    m = mvp * morth;
                    break;
                }
            case Visualization.CameraTransformation:
                {
                    m = mvp * (morth * mcam);
                    break;
                }
            case Visualization.PerspectiveProjection:
                {
                    m = mvp * ((morth * mper) * mcam);
                    break;
                }
            default: break;
        }

        TextureDraw.ClearBuffer(frameBuffer, Color.black);

        for (int i = 0; i < world.lines.Length; i += 2)
        {
            Vector4 p = multiplyPoint(m,
                           new Vector4(world.vertices[world.lines[i]].x,
                                       world.vertices[world.lines[i]].y,
                                       world.vertices[world.lines[i]].z, 1));

            Vector4 q = multiplyPoint(m,
                       new Vector4(world.vertices[world.lines[i + 1]].x,
                                   world.vertices[world.lines[i + 1]].y,
                                   world.vertices[world.lines[i + 1]].z, 1));

            TextureDraw.DrawLine(frameBuffer, (int)p.x, (int)p.y, (int)q.x, (int)q.y, Color.white);
        }
    }

    Vector4 multiplyPoint(Matrix4x4 matrix, Vector4 point)
    {
        Vector4 result = new Vector4();
        for (int r = 0; r < 4; r++)
        {
            float s = 0;
            for (int z = 0; z < 4; z++)
                s += matrix[r, z] * point[z];
            result[r] = s;
        }
        return result;
    }

    void UpdateViewVolume(Vector3 e)
    {
        near_plane = e.z - 3;
        far_plane = e.z - 13;
        right_plane = e.x - 5;
        left_plane = e.x + 5;
        top_plane = e.y + 5;
        botton_plane = e.y - 5;
    }

    void ResetViewVolume()
    {
        left_plane = 5f;
        right_plane = -5f;
        botton_plane = -5f;
        top_plane = 5f;
        near_plane = -1f;
        far_plane = -11f;
    }
}