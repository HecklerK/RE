using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms.MouseButtons;


namespace RE
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool drawing;
        GraphicsPath currentPath;
        System.Drawing.Point oldLocation;
        System.Drawing.Pen currentPen;

        public MainWindow()
        {
            InitializeComponent();
            drawing = false;
            currentPen = new System.Drawing.Pen(System.Drawing.Color.Black);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (picDrawingSurface.Source != null) {
                var result = System.Windows.MessageBox.Show("Сохранить текущее изображение перед созданием нового рисунка?", "Предупреждение", MessageBoxButton.YesNoCancel);
                switch (result) {
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Yes:
                        MenuItem_Click_1(sender, e);
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                }
            }
            BitmapImage pic = new BitmapImage();
            picDrawingSurface.Source = pic;
        }

        private Bitmap BitmapImage2Bitmap()
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create((BitmapSource)picDrawingSurface.Source));
                enc.Save(outStream);
                Bitmap bmp = new Bitmap(outStream);
                return new Bitmap(bmp);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.FileName = "Изображение";
            sd.Title = "Сохранить картинку как...";
            sd.OverwritePrompt = true;
            sd.CheckPathExists = true;
            sd.DefaultExt = ".jpg";
            sd.Filter = "Bitmap File(*.bmp)|*.bmp|" + "JPEG File(*.JPG)|*.jpg|" + "GIF File(*.GIF)|*.gif|" + "PNG File(*.PNG)|*.png";
            var result = sd.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    Bitmap bmp = BitmapImage2Bitmap();
                    string fn = sd.FileName;
                    string sfe = fn.Remove(0, fn.Length - 3);
                    switch (sfe)
                    {
                        case "bmp":
                            bmp.Save(fn, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        case "jpg":
                            bmp.Save(fn, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        case "gif":
                            bmp.Save(fn, System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                        case "png":
                            bmp.Save(fn, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                    }
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    break;
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var fd = new OpenFileDialog();
            fd.DefaultExt = ".jpg";
            fd.Filter = "Изображение (*.BMP, *.JPG, *.GIF, *.PNG)|*.bmp; *.jpg; *.gif; *.png";
            var result = fd.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    BitmapImage bm1 = new BitmapImage();
                    bm1.BeginInit();
                    bm1.UriSource = new Uri(fd.FileName, UriKind.Relative);
                    bm1.CacheOption = BitmapCacheOption.OnLoad;
                    bm1.EndInit();
                    picDrawingSurface.Source = bm1;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    break;
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void picDrawingSurface_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) {
                drawing = true;
                oldLocation = e.GetPosition(picDrawingSurface);
                currentPath = new GraphicsPath()};
        }

        private void picDrawingSurface_MouseUp(object sender, MouseButtonEventArgs e)
        {
            drawing = false;
            try { currentPath.Dispose(); }
            catch { };
        }

        private void picDrawingSurface_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (drawing) {
                Graphics g = Graphics.FromImage(picDrawingSurface.);
                currentPath.AddLine(oldLocation, e.GetPosition(picDrawingSurface));
                g.DrawPath(currentPen, currentPath);
                oldLocation = e.Location; g.Dispose();
                picDrawingSurface.Invalidate();
            }
        }
    }
}
