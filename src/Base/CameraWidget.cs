using System;
using System.Runtime.InteropServices;

namespace Gphoto2.Base
{
    public enum CameraWidgetType 
    {               /* Value (get/set):     */
        GP_WIDGET_WINDOW,
        GP_WIDGET_SECTION,
        GP_WIDGET_TEXT,     /* char *               */
        GP_WIDGET_RANGE,    /* float                */
        GP_WIDGET_TOGGLE,   /* int                  */
        GP_WIDGET_RADIO,    /* char *               */
        GP_WIDGET_MENU,     /* char *               */
        GP_WIDGET_BUTTON,   /* CameraWidgetCallback */
        GP_WIDGET_DATE      /* int                  */
    }
    
    public class CameraWidget : IDisposable
    {
        protected HandleRef handle;
        
        protected CameraWidget (IntPtr native)
        {
            this.handle = new HandleRef (this, native);
            gp_widget_ref (this.Handle);
        }
        
        public CameraWidget (CameraWidgetType type, string label)
        {
            ErrorCode result;
            unsafe 
            {
                IntPtr native;
                result = gp_widget_new(type, label,out native);
                this.handle = new HandleRef (this, native);
            }
            if (Error.IsError(result)) throw Error.ErrorException(result);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        ~CameraWidget()
        {
            Dispose(false);
        }
        
        protected virtual void Dispose (bool disposing)
        {
            ErrorCode result;
            unsafe
            {
                if (this.Handle.Handle != IntPtr.Zero)
                {
                    result = gp_widget_unref(this.Handle);
                    if (Error.IsError(result)) throw Error.ErrorException(result);
                }
            }
        }
        
        public HandleRef Handle
        {
            get {
                return handle;
            }
        }

        public void Append(CameraWidget child)
        {
            ErrorCode result;
    
            result = gp_widget_append(this.Handle, child.Handle);
            
            if (Error.IsError(result)) throw Error.ErrorException(result);
        }

        public void Prepend(CameraWidget child)
        {
            ErrorCode result;

            result = gp_widget_prepend(this.Handle, child.Handle);

            if (Error.IsError(result)) throw Error.ErrorException(result);
        }   
        
        public int ChildCount
        {
            get
            {
                ErrorCode result;
                
                result = gp_widget_count_children(this.Handle);
                    
                if (Error.IsError(result)) throw Error.ErrorException(result);
                return (int)result;
            }
        }
        
        public CameraWidget GetChild (int n)
        {
            ErrorCode result;
            IntPtr native;

            result = gp_widget_get_child(this.Handle, n, out native);
            
            CameraWidget child = new CameraWidget(native);
            
            if (Error.IsError(result)) throw Error.ErrorException(result);
            
            return child;
        }
        
        public CameraWidget GetChild (string label)
        {
            ErrorCode result;
            IntPtr native;
            
            result = gp_widget_get_child_by_label(this.Handle, label, out native);
            
            CameraWidget child = new CameraWidget (native);
            
            if (Error.IsError(result)) throw Error.ErrorException(result);
            return child;
        }
        
        public CameraWidget GetChildByID (int id)
        {
            ErrorCode result;           
            IntPtr native;
            
            result = gp_widget_get_child_by_id(this.Handle, id, out native);
            CameraWidget child = new CameraWidget (native);
            
            if (Error.IsError(result)) throw Error.ErrorException(result);
            return child;
        }

        public CameraWidget GetRoot ()
        {
            ErrorCode result;
            IntPtr native;

            result = gp_widget_get_root (this.Handle, out native);
            CameraWidget root = new CameraWidget(native);

            if (Error.IsError(result)) throw Error.ErrorException(result);
            return root;
        }
        
        public void SetInfo (string info)
        {
            ErrorCode result;

            result = gp_widget_set_info(this.Handle, info);
            if (Error.IsError(result)) throw Error.ErrorException(result);
        }
        
        public string GetInfo ()
        {
            ErrorCode result;
            string info;
            
            result = gp_widget_get_info (this.Handle, out info);

            if (Error.IsError(result)) throw Error.ErrorException(result);
            return info;
        }
        
        public int GetID ()
        {
            ErrorCode result;
            int id;

            result = gp_widget_get_id (this.Handle, out id);

            if (Error.IsError(result)) throw Error.ErrorException(result);
            return id;
        }
        
        public CameraWidgetType GetWidgetType ()
        {
            ErrorCode result;
            CameraWidgetType widget_type;

            result = gp_widget_get_type(this.Handle, out widget_type);

            if (Error.IsError(result)) throw Error.ErrorException(result);
            return widget_type;
        }
        
        public string GetLabel ()
        {
            ErrorCode result;
            string label;

            result = gp_widget_get_label(this.Handle, out label);

            if (Error.IsError(result)) throw Error.ErrorException(result);
            return label;
        }
        
        /* TODO: figure out what's going on here...
        public void SetValue (string value)
        {
            ErrorCode result;
            unsafe
            {
                result = _CameraWidget.gp_widget_set_value(obj, Marshal.StringToHGlobalAnsi(value));
            }
            if (Error.IsError(result)) throw Error.ErrorException(result);
        }
        
        public void SetValue (float value)
        {
            ErrorCode result;
            unsafe
            {
                IntPtr ptr = (void*)value;
                result = _CameraWidget.gp_widget_set_value(obj, ptr);
            }
            if (Error.IsError(result)) throw Error.ErrorException(result);
        }
        
        public void SetValue (int value)
        {
            ErrorCode result;
            unsafe
            {
                IntPtr ptr = value;
                result = _CameraWidget.gp_widget_set_value(obj, ptr);
            }
            if (Error.IsError(result)) throw Error.ErrorException(result);
        }
        
        /*public void SetValue (CameraWidgetCallback value)
        {
            ErrorCode result;
            unsafe
            {
                IntPtr ptr = &value;
                result = _CameraWidget.gp_widget_set_value(obj, ptr);
            }
            if (Error.IsError(result)) throw Error.ErrorException(result);
        }*/
        
        public void GetRange (out float min, out float max, out float increment)
        {
            ErrorCode result;

            result = gp_widget_get_range(this.Handle, out min, out max, out increment);

            if (Error.IsError(result)) throw Error.ErrorException(result);
        }

        public void AddChoice (string choice)
        {
            ErrorCode result;
            
            result = gp_widget_add_choice (this.Handle, choice);

            if (Error.IsError(result)) throw Error.ErrorException(result);
        }

        public int ChoicesCount ()
        {
            ErrorCode result;

            result = gp_widget_count_choices(this.Handle);
            
            if (Error.IsError(result)) throw Error.ErrorException(result);
            return (int)result;
        }

        public string GetChoice (int n)
        {
            ErrorCode result;
            string choice;

            result = gp_widget_get_choice(this.Handle, n, out choice);

            if (Error.IsError(result)) throw Error.ErrorException(result);
            return choice;
        }
        
        public bool Changed ()
        {
            ErrorCode result;

            result = gp_widget_changed(this.Handle);

            if (Error.IsError(result)) throw Error.ErrorException(result);

            if ((int)result == 1)
                return true;
            else
                return false;
        }

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_get_range (HandleRef range, out float min, out float max, out float increment);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_add_choice (HandleRef widget, string choice);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_count_choices (HandleRef widget);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_get_choice (HandleRef widget, int choice_number, out string choice);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_changed (HandleRef widget);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_ref (HandleRef widget);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_new (CameraWidgetType type, string lable, out IntPtr widget);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_unref (HandleRef widget);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_append (HandleRef widget, HandleRef child);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_prepend (HandleRef widget, HandleRef child);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_count_children (HandleRef widget);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_get_child (HandleRef widget, int child_number, out IntPtr child);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_get_child_by_label (HandleRef widget, string label, out IntPtr child);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_get_child_by_id (HandleRef widget, int id, out IntPtr child);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_get_root (HandleRef widget, out IntPtr root);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_set_info (HandleRef widget, string info);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_get_info (HandleRef widget, out string info);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_get_id (HandleRef widget, out int id);
        
        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_get_type (HandleRef widget, out CameraWidgetType type);

        [DllImport ("libgphoto2.so")]
        internal static extern ErrorCode gp_widget_get_label (HandleRef widget, out string label);
    }
}
