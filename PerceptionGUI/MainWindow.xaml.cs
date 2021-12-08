using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using Ookii.Dialogs.Wpf;
using System.Windows.Controls;
using PerceptionComponent;
using System.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Drawing;

namespace PerceptionGUI
{
    public partial class MainWindow : Window
    {
        WPFView view;
        HttpClient client;
        public MainWindow()
        {
            InitializeComponent();
            view = new WPFView();
            ListViewImagesInProcess.ItemsSource = view.Model.ImagesInProcess;
            client = new HttpClient();
            GetStartValuesAsync();
            ClassCounts.SelectionChanged += ClassCountSelectionChanged;
        }
        private async void GetStartValuesAsync()
        {
            string response;
            try
            {
                response = await client.GetStringAsync("https://localhost:44319/api/images");
                var res = JsonConvert.DeserializeObject<List<ImgDescription>>(response);
                RefreshDataButtonClick(this, new RoutedEventArgs());
                ImagesDBListBox.ItemsSource = res;
                for (int i = 0; i < res.Count; ++i)
                {
                    view.Model.Add(res[i], res[i].ImgName);
                }
                DataContext = view;
            } catch
            {
                MessageBox.Show("Сервис недоступен");
            }
}
        private void ChooseFolderButtonClick(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog dial = new VistaOpenFileDialog();
            bool? res = dial.ShowDialog();
            if (res.HasValue && res.Value)
            {
                FolderPathTextBox.Text = dial.FileName;
            }
        }
        private async void StartPerceptionButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(FolderPathTextBox.Text);
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StreamContent(memoryStream), "file", FolderPathTextBox.Text);
                await client.PostAsync("https://localhost:44319/api/images", content);
            } catch
            {
                MessageBox.Show("Сервис недоступен");
            }
}
        private async void RefreshDataButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await client.GetStringAsync("https://localhost:44319/api/images");
                var res = JsonConvert.DeserializeObject<List<ImgDescription>>(response);
                ImagesDBListBox.ItemsSource = res;
                WPFView newView = new WPFView();
                for (int i = 0; i < res.Count; ++i)
                {
                    newView.Model.Add(res[i], res[i].ImgName);
                    using (var bitmap = new Bitmap(System.Drawing.Image.FromFile(res[i].ImgName)))
                    {
                        foreach (var obj in res[i].Objs)
                        {
                            using (var g = Graphics.FromImage(bitmap))
                            {
                                g.DrawRectangle(Pens.Red, (int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height);
                                using (var brushes = new SolidBrush(Color.FromArgb(50, Color.Red)))
                                {
                                    g.FillRectangle(brushes, (int)obj.x, (int)obj.y, (int)obj.width, (int)obj.height);
                                }
                                g.DrawString(obj.Cls, new Font("Arial", 12), Brushes.Blue, new PointF((int)obj.x, (int)obj.y));
                            }
                        }
                        if (!view.Model.ImagesInProcess.Contains(res[i].ImgName + "Result" + ".jpg"))
                        {
                            bitmap.Save(res[i].ImgName + "Result" + ".jpg");
                        }
                        newView.Model.InsertImage(0, res[i].ImgName + "Result" + ".jpg");
                    }
                }
                view = newView;
                DataContext = view;
                ListViewImagesInProcess.ItemsSource = view.Model.ImagesInProcess;
            }
            catch
            {
                MessageBox.Show("Сервис недоступен");
            }
        }
        private void ClassCountSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (ClassCounts.SelectedItem == null)
            {
                ClassImages.ItemsSource = null;
                return;
            }
            string className = ((KeyValuePair<string, int>)ClassCounts.SelectedItem).Key;
            ClassImages.ItemsSource = view.ClassImages[className];
        }

        private async void ClearDB(object sender, RoutedEventArgs e)
        {
            try
            {
                await client.DeleteAsync("https://localhost:44319/api/images");
            } catch
            {
                MessageBox.Show("Сервис недоступен");
            }
        }
    }
}