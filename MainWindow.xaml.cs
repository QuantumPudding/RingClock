using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RingClock
{
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        readonly IntPtr HWND_TOP = new IntPtr(0);
        readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const int WM_SETFOCUS = 0x0007;
        const uint SWP_NOMOVE = 0x2;
        const uint SWP_NOSIZE = 0x1;
        const uint SWP_SHOWWINDOW = 0x40;

        const double pi = Math.PI;
        const double twopi = pi * 2;
        const double halfpi = pi / 2;

        double secondDivisor;
        double minuteDivisor;
        double hourDivisor;
        double dayOfWeekDivisor;
        double dayOfMonthDivisor;

        int lastMinute;
        int lastHour;
        int lastDayOfWeek;
        int lastDayOfMonth;

        int mouseHoverCount = 0;
        int mouseHoverTimer = 300;

        Border popupBorder;
        TextBlock popupText;
        DispatcherTimer timer;

        IntPtr hWnd;

        SolidColorBrush highlightBrush = new SolidColorBrush(Colors.SkyBlue);
        SolidColorBrush regularBrush = new SolidColorBrush(Colors.White);

        public MainWindow()
        {
            InitializeComponent();

            Top = (SystemParameters.PrimaryScreenHeight / 2) - (Height / 2);
            Left = (SystemParameters.PrimaryScreenWidth / 2) - (Width / 2);

            secondDivisor = twopi / 60;
            minuteDivisor = twopi / 60;
            hourDivisor = twopi / 12;
            dayOfWeekDivisor = twopi / 7;
            dayOfMonthDivisor = twopi / DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);

            int w = (int)(Width / 2);
            int h = (int)(Height / 2);

            Canvas.SetTop(bgEllipse, h - (bgEllipse.Height / 2));
            Canvas.SetLeft(bgEllipse, w - (bgEllipse.Width / 2));
            Canvas.SetTop(secondEllipse, h - (secondEllipse.Height / 2));
            Canvas.SetLeft(secondEllipse, w - (secondEllipse.Width / 2));
            Canvas.SetTop(minuteEllipse, h - (minuteEllipse.Height / 2));
            Canvas.SetLeft(minuteEllipse, w - (minuteEllipse.Width / 2));
            Canvas.SetTop(hourEllipse, h - (hourEllipse.Height / 2));
            Canvas.SetLeft(hourEllipse, w - (hourEllipse.Width / 2));
            Canvas.SetTop(dowEllipse, h - (dowEllipse.Height / 2));
            Canvas.SetLeft(dowEllipse, w - (dowEllipse.Width / 2));
            Canvas.SetTop(domEllipse, h - (domEllipse.Height / 2));
            Canvas.SetLeft(domEllipse, w - (domEllipse.Width / 2));

            Canvas.SetTop(secondArc, h);
            Canvas.SetLeft(secondArc, w);
            Canvas.SetTop(minuteArc, h);
            Canvas.SetLeft(minuteArc, w);
            Canvas.SetTop(hourArc, h);
            Canvas.SetLeft(hourArc, w);
            Canvas.SetTop(dowArc, h);
            Canvas.SetLeft(dowArc, w);
            Canvas.SetTop(domArc, h);
            Canvas.SetLeft(domArc, w);

            secondArc.MouseEnter += arc_MouseEnter;
            secondArc.MouseEnter += arc_MouseLeave;
            minuteArc.MouseEnter += arc_MouseEnter;
            minuteArc.MouseEnter += arc_MouseLeave;
            hourArc.MouseEnter += arc_MouseEnter;
            hourArc.MouseEnter += arc_MouseLeave;
            dowArc.MouseEnter += arc_MouseEnter;
            dowArc.MouseEnter += arc_MouseLeave;
            domArc.MouseEnter += arc_MouseEnter;
            domArc.MouseEnter += arc_MouseLeave;

            Canvas.SetTop(timeText, h - (timeText.Height / 2) - 2);
            Canvas.SetLeft(timeText, w - (timeText.Width / 2));

            textEllipse.Width = timeText.Width * 1.4;
            textEllipse.Height = timeText.Width * 1.4;
            textEllipse.MouseDown += center_MouseDown;
            textEllipse.MouseEnter += ellipse_MouseEnter;
            textEllipse.MouseLeave += ellipse_MouseLeave;
            Canvas.SetTop(textEllipse, h - (textEllipse.Height / 2));
            Canvas.SetLeft(textEllipse, w - (textEllipse.Width / 2));

            Canvas.SetTop(btnExit, 0);
            Canvas.SetLeft(btnExit, Width - 24);
            btnExit.Visibility = Visibility.Hidden;

            timeText.MouseDown += center_MouseDown;
            timeText.MouseEnter += center_MouseEnter;
            timeText.MouseLeave += center_MouseLeave;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Tick;
            timer_Tick(null, null);
            timer.Start();

            popupText = new TextBlock();
            popupText.Background = new SolidColorBrush(Colors.Transparent);
            popupText.Foreground = new SolidColorBrush(Colors.White);
            popupText.Margin = new Thickness(4);
            popupText.FontFamily = new System.Windows.Media.FontFamily("Ubuntu");
            popupText.FontSize = 16;

            popupBorder = new Border();
            popupBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#55000000"));
            popupBorder.Child = popupText;

            canvas.Children.Add(popupBorder);
            popupBorder.Visibility = Visibility.Hidden;
        }

        void center_MouseLeave(object sender, MouseEventArgs e)
        {
            timeText.Foreground = regularBrush;
        }

        void center_MouseEnter(object sender, MouseEventArgs e)
        {
            timeText.Foreground = highlightBrush;
        }

        void center_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            secondArc.EndAngle = -halfpi + (secondDivisor * now.Second);
            minuteArc.EndAngle = -halfpi + (minuteDivisor * now.Minute);
            hourArc.EndAngle = -halfpi + (hourDivisor * (now.Hour / 2));
            dowArc.EndAngle = -halfpi + (dayOfWeekDivisor * (int)now.DayOfWeek);
            domArc.EndAngle = -halfpi + (dayOfMonthDivisor * now.Day);

            timeText.Text = now.ToString("HH:mm:ss dd/MM/yyyy");// now.ToShortDateString();//now.Hour.ToString() + ":" + now.Minute.ToString() + ":" + now.Second.ToString() + "\n" + now.Day.ToString() + "/" + now.Month.ToString() + "/" + now.Year.ToString();
        }

        private void ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            Ellipse el = ((Ellipse)sender);

            if (el == secondEllipse)
                secondArc.Stroke = highlightBrush;
            else if (el == minuteEllipse)
                minuteArc.Stroke = highlightBrush;
            else if (el == hourEllipse)
                hourArc.Stroke = highlightBrush;
            else if (el == dowEllipse)
                dowArc.Stroke = highlightBrush;
            else if (el == domEllipse)
                domArc.Stroke = highlightBrush;
            else if (el == textEllipse)
                timeText.Foreground = highlightBrush;

            if (el.Tag != null)
            {
                popupText.Text = el.Tag.ToString();

                Point p = e.GetPosition(canvas);
                Canvas.SetTop(popupBorder, p.Y);
                Canvas.SetLeft(popupBorder, p.X + 4);

                popupBorder.Visibility = Visibility.Visible;
            }
        }

        private void arc_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Arc)sender).Stroke = highlightBrush;
        }

        private void arc_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Arc)sender).Stroke = regularBrush;
        }

        private void ellipse_MouseLeave(object sender, MouseEventArgs e)
        {
            popupBorder.Visibility = Visibility.Hidden;

            secondArc.Stroke = regularBrush;
            minuteArc.Stroke = regularBrush;
            hourArc.Stroke = regularBrush;
            dowArc.Stroke = regularBrush;
            domArc.Stroke = regularBrush;
            timeText.Foreground = regularBrush;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_SETFOCUS)
            {
                SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
                handled = true;
            }

            return IntPtr.Zero;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            hWnd = new WindowInteropHelper(this).Handle;
            SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            btnExit.Visibility = Visibility.Visible;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            btnExit.Visibility = Visibility.Hidden;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
