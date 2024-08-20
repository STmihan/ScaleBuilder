using System.Runtime.InteropServices;

namespace Plugins.webgl
{
    public static class WebGLPlugin
    {
        [DllImport("__Internal")]
        private static extern bool isMobile();

        public static bool IsMobile()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
         return isMobile();
#endif
            return DebugWebGL.Instance.IsMobile;
        }
    }
}