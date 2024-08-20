var WebGLPlugin = {
   isMobile: function()
   {
      return Module.SystemInfo.mobile;
   }
};  
mergeInto(LibraryManager.library, WebGLPlugin);
