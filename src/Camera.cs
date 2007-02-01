using System;
using System.Collections.Generic;

namespace Gphoto2
{
    public class Camera : Gphoto2.Base.Camera
    {
        protected List<File> files;
        
        private Base.PortInfoList port_info_list;
        private Base.CameraAbilitiesList abilities_list;

        public Camera (Base.PortInfoList port_info_list, Base.CameraAbilitiesList abilities_list) {

        }

        public void AddFile (File file) {

        }

        public void DeleteFile (File file) {

        }
    }
}

