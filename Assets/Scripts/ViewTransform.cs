using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        Matrix4x4 m = mvp * morth;

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
}