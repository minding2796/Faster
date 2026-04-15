using System;
using UnityEngine;
using Utils;

namespace UI
{
    public class CustomCursor : SingleMono<CustomCursor>
    {
        public CursorType current;
        public Texture2D[] textures;
        public Vector2[] hotspots;

        [VisibleEnum(typeof(CursorType))]
        public void SetCursor(int type)
        {
            current = (CursorType) type;
            Cursor.SetCursor(textures[(int) current], hotspots[(int) current], CursorMode.Auto);
        }
    }

    [Serializable]
    public enum CursorType
    {
        Normal,
        Pointer,
        Input,
    }
}
