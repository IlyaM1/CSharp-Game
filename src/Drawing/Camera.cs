using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Dyhar.src.Level;

namespace Dyhar.src.Drawing;

public class Camera
{
    public Matrix Transform;
    private Viewport viewport;

    public Vector2 Position { get; private set; }

    public float ZoomWidth { get; private set; }
    public float ZoomHeight { get; private set; }

    int mapWidth;
    int mapHeight;

    public Camera(Viewport viewport, int mapWidth, int mapHeight)
    {
        this.viewport = viewport;
        ZoomWidth = (float)Resolution.kWidth;
        ZoomHeight = (float)Resolution.kHeight;
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
    }

    public void Update(Vector2 position)
    {
        var smallestWidth = Math.Min(viewport.Width / ZoomWidth, mapWidth);
        var smallestHeight = Math.Min(viewport.Height / ZoomHeight, mapHeight);

        var minX = smallestWidth / 2;
        var maxX = mapWidth - smallestWidth / 2;
        var minY = smallestHeight / 2;
        var maxY = mapHeight - smallestHeight / 2;

        Position = new Vector2(
            MathHelper.Clamp(position.X, minX, maxX),
            MathHelper.Clamp(position.Y, minY, maxY)
        );

        Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateScale(ZoomWidth, ZoomHeight, 1) *
                    Matrix.CreateTranslation(new Vector3(viewport.Width / 2f, viewport.Height / 2f, 0));
    }

    public Vector2 MapPositionToScreenPosition(Vector2 mapPosition)
    {
        return Vector2.Transform(mapPosition, Transform);
    }
}