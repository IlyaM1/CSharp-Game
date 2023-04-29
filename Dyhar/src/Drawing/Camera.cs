using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Dyhar.src.LevelsCreator;

namespace Dyhar.src.Drawing;

public class Camera
{
    public Matrix Transform;
    private Viewport viewport;

    public Vector2 Position{ get; private set; }

    public float Zoom { get; private set; }

    public Camera(Viewport viewport)
    {
        this.viewport = viewport;
        Zoom = 1f;
    }

    public void Update(Vector2 position, int mapWidth, int mapHeight)
    {
        // Calculate the smallest visible area that fits within the map boundaries
        float smallestWidth = Math.Min(viewport.Width / Zoom, mapWidth);
        float smallestHeight = Math.Min(viewport.Height / Zoom, mapHeight);

        // Calculate the bounds of the camera in the map
        float minX = smallestWidth / 2;
        float maxX = mapWidth - smallestWidth / 2;
        float minY = smallestHeight / 2;
        float maxY = mapHeight - smallestHeight / 2;

        // Set the camera position
        Position = new Vector2(
            MathHelper.Clamp(position.X, minX, maxX),
            MathHelper.Clamp(position.Y, minY, maxY)
        );

        // Calculate the transform matrix for the camera
        Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateScale(Zoom, Zoom, 1) *
                    Matrix.CreateTranslation(new Vector3(viewport.Width / 2f, viewport.Height / 2f, 0));
    }
}