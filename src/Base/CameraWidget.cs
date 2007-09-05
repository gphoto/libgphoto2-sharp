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
            IntPtr native;
            Error.CheckError(gp_widget_new(type, label,out native));
            this.handle = new HandleRef (this, native);
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
        
		// FIXME: Is this correct? Will unrefing the object actually set the IntPtr to zero?
        protected virtual void Dispose (bool disposing)
        {
           if (this.Handle.Handle != IntPtr.Zero)
               Error.CheckError(gp_widget_unref(this.Handle));
        }
        
        public HandleRef Handle
        {
            get {
                return handle;
            }
        }

        public void Append(CameraWidget child)
        {
            Error.CheckError(gp_widget_append(this.Handle, child.Handle));
        }

        public void Prepend(CameraWidget child)
        {
            Error.CheckError(gp_widget_prepend(this.Handle, child.Handle));
        }   
        
        public int ChildCount
        {
            get
            {
                return (int) Error.CheckError(gp_widget_count_children(this.Handle));
            }
        }
        
        public CameraWidget GetChild (int n)
        {
            IntPtr native;

            Error.CheckError(gp_widget_get_child(this.Handle, n, out native));
            CameraWidget child = new CameraWidget(native);
            
            return child;
        }
        
        public CameraWidget GetChild (string label)
        {
            IntPtr native;
            
            Error.CheckError(gp_widget_get_child_by_label(this.Handle, label, out native));
            CameraWidget child = new CameraWidget (native);
            
            return child;
        }
        
        public CameraWidget GetChildByID (int id)
        {
            IntPtr native;
            
            Error.CheckError(gp_widget_get_child_by_id(this.Handle, id, out native));
            CameraWidget child = new CameraWidget (native);
            
            return child;
        }

        public CameraWidget GetRoot ()
        {
            IntPtr native;

            Error.CheckError(gp_widget_get_root (this.Handle, out native));
            CameraWidget root = new CameraWidget(native);

            return root;
        }
        
        public void SetInfo (string info)
        {
            Error.CheckError(gp_widget_set_info(this.Handle, info));
        }
        
        public string GetInfo ()
        {
            string info;
            
            Error.CheckError(gp_widget_get_info (this.Handle, out info));

            return info;
        }
        
        public int GetID ()
        {
			int id;
            
			Error.CheckError(gp_widget_get_id (this.Handle, out id));
			
			return id;
        }
        
        public CameraWidgetType GetWidgetType ()
        {
            CameraWidgetType widget_type;

            Error.CheckError(gp_widget_get_type(this.Handle, out widget_type));

            return widget_type;
        }
        
        public string GetLabel ()
        {
            string label;

            Error.CheckError(gp_widget_get_label(this.Handle, out label));

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
            Error.CheckError(gp_widget_get_range(this.Handle, out min, out max, out increment));
        }

        public void AddChoice (string choice)
        {
            Error.CheckError(gp_widget_add_choice (this.Handle, choice));
        }

        public int ChoicesCount ()
        {
            return (int) Error.CheckError(gp_widget_count_choices(this.Handle));
        }

        public string GetChoice (int n)
        {
            string choice;

            Error.CheckError(gp_widget_get_choice(this.Handle, n, out choice));

            return choice;
        }
        
		// FIXME: Is it 1 and 0 or 0 and non-zero?
        public bool Changed ()
        {
            return (int) Error.CheckError(gp_widget_changed(this.Handle)) == 1;
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
