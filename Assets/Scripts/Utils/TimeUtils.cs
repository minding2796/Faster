using GameLogic;

namespace Utils
{
    public static class TimeUtils
    {
        public static float GetOffsetAffectedTime(float origin)
        {
            return origin * 1000f - GameStatics.GameStartTime - GameStatics.StartOffset - GameStatics.AudioOffset;
        }
        public static float GetCurrentTime(float origin)
        {
            return origin * 1000f - GameStatics.GameStartTime - GameStatics.StartOffset;
        }
    }
}