using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPTHelper
{
    public class GestureUtils
    {
        private static Dictionary<Control, Point> startPoint = new Dictionary<Control, Point>();
        private static Dictionary<Control, Action> downAction = new Dictionary<Control, Action>();
        private static Dictionary<Control, Action> upAction = new Dictionary<Control, Action>();

        private static List<Control> involved = new List<Control>();

        public static bool IsInvolved(Control control) => involved.Contains(control);
        public static bool Reject(Control control) => involved.Remove(control);

        public static bool IsGesturing = false;

        private static void SlideAndCallback(Control control, bool up, Action callback)
        {
            control.MouseDown += Control_MouseDown;
            control.MouseMove += Control_MouseMove;
            control.MouseUp += Control_MouseUp;
            if (up)
            {
                upAction[control] = callback;
            }
            else
            {
                downAction[control] = callback;
            }
        }

        private static void Control_MouseUp(object sender, MouseEventArgs e)
        {
            startPoint.Remove(sender as Control);
        }

        private static void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (!startPoint.ContainsKey(sender as Control))
            {
                return;
            }
            var control = sender as Control;
            var start = startPoint[control];
            var deltaX = e.X - start.X;
            var deltaY = e.Y - start.Y;
            if (Math.Abs(deltaX) < Math.Abs(deltaY))
            {
                var minDY = 10;
                if (deltaY > minDY)
                {
                    // Accept down gesture
                    downAction[control].Invoke();
                    startPoint.Remove(control);
                    if (!IsInvolved(control))
                    {
                        involved.Add(control);
                    }
                    IsGesturing = true;
                }
                else if (deltaY < -minDY)
                {
                    // Accept up gesture
                    upAction[control].Invoke();
                    startPoint.Remove(control);
                    if (!IsInvolved(control))
                    {
                        involved.Add(control);
                    }
                    IsGesturing = true;
                }
            }
        }

        private static void Control_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint[sender as Control] = e.Location;
        }

        public static void AddSlideDownGesture(Control control)
        {
            var form = control.FindForm() as HelperForm;
            SlideAndCallback(control, false, () => form.SlipDown(showHint: false));
        }

        public static void AddSlideUpGesture(Control control, Action callback)
        {
            SlideAndCallback(control, true, callback);
        }
    }
}
