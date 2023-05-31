using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Dyhar.src.Levels;
using System.Drawing;

namespace Dyhar.src.Drawing;

public class Camera
{
    public Matrix Transform { get; set; }
    public Vector2 Position { get; set; }

    public float ZoomWidth { get; private set; }
    public float ZoomHeight { get; private set; }

    public Camera(Viewport viewport, int mapWidth, int mapHeight)
    {
        _viewport = viewport;
        ZoomWidth = (float)Resolution.kWidth;
        ZoomHeight = (float)Resolution.kHeight;
        _mapWidth = mapWidth;
        _mapHeight = mapHeight;
    }

    public void Update(Vector2 position)
    {
        var smallestWidth = Math.Min(_viewport.Width / ZoomWidth, _mapWidth);
        var smallestHeight = Math.Min(_viewport.Height / ZoomHeight, _mapHeight);

        var minX = smallestWidth / 2;
        var maxX = _mapWidth - smallestWidth / 2;
        var minY = smallestHeight / 2;
        var maxY = _mapHeight - smallestHeight / 2;

        Position = new Vector2(
            MathHelper.Clamp(Position.X + (position.X - Position.X)*0.1f, minX, maxX),
            MathHelper.Clamp(Position.Y + (position.Y - Position.Y)*0.1f, minY, maxY)
        );

        Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateScale(ZoomWidth, ZoomHeight, 1) *
                    Matrix.CreateTranslation(new Vector3(_viewport.Width / 2f, _viewport.Height / 2f, 0));
    }

    public Vector2 ConvertMapPositionToScreenPosition(Vector2 mapPosition)
    {
        return Vector2.Transform(mapPosition, Transform);
    }

    public Vector2 ConvertScreenPositionToMapPosition(Vector2 screenPosition)
    {
        return Vector2.Transform(screenPosition, Matrix.Invert(Transform));
    }

    public void SetNewMapSize(Size size)
    {
        _mapWidth = size.Width;
        _mapHeight = size.Height;
    }

    private int _mapWidth;
    private int _mapHeight;
    private Viewport _viewport;
}