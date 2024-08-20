using Plugins.webgl;

namespace Code.Utils
{
    public static class Platform
    {
        public static bool IsMobile()
        {
#if UNITY_WEBGL
            return WebGLPlugin.IsMobile();
#elif UNITY_ANDROID || UNITY_IOS
            return true;
#else
            return false;
#endif
        }
    }
}