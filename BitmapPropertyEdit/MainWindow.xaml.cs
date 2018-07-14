using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;

namespace BitmapPropertyEdit
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private TagInfo tagInfo = TagInfo.Instance;

        private List<string> files = new List<string>();

        private int index = -1;

        private const string OUTPUT_DIRECTORY = "output";

        public IEnumerable<TabItem> AllPages {
            get
            {
                return GroupTab.Items.Cast<TabItem>();
            }
        }

        public IEnumerable<TabPageContent> AllContents
        {
            get
            {
                return AllPages.Select(x => x.Content).Cast<TabPageContent>();
            }
        }

        private string WorkingDirectory
        {
            get
            {

                if (!Directory.Exists("work"))
                    Directory.CreateDirectory("work");
                return "work";

            }

        }

        public bool ThisIsEnd { get { return index >= files.Count - 1; } }

        BitmapWarapper warapper;

        public MainWindow()
        {
            InitializeComponent();

            Prepare();
        }

        private bool hasTagProperty(string file) {

            BitmapWarapper warapper = new BitmapWarapper(file);
            try
            {
                warapper.Tag = "test";
            }
            catch {
                return false;
            }
            return true;

        }

        private void registerTag()
        {
            var tags = GetCheckedTags();
            warapper = new BitmapWarapper(files[index]);
            warapper.Tag=string.Join(";", tags);
            warapper.Save(OUTPUT_DIRECTORY);
            warapper.Dispose();
        }

        private void ViewImage(string file) {
            BitmapImage image = new BitmapImage( new Uri( file ) );
            ImageViewer.Source = image;
        }

        private void NextOrFinish() {

            Next.Content = files.Count > 0 && index == files.Count - 1 ? "finish" : "next";
            Next.IsEnabled = files.Count > 0;
        }

        private void Prepare()
        {
            Directory.Delete(WorkingDirectory,true);

            if (!Directory.Exists(OUTPUT_DIRECTORY))
                Directory.CreateDirectory(OUTPUT_DIRECTORY);

            foreach (var key in tagInfo.Groups.Keys) {

                TabItem tabItem = new TabItem() { Header = key };
                tabItem.Content = new TabPageContent(tagInfo.Groups[key] as List<Tag>)
                {
                    
                };
                GroupTab.Items.Add(tabItem);
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            registerTag();

            UncheckAll();

            if (ThisIsEnd)
            {
                Init();
            }
            else
            {
            index++;
                copy();
                NextOrFinish();
            }
        }

        private List<string> GetCheckedTags() {
            List<string> checkedTags = new List<string>();
            foreach (var tabitem in GroupTab.Items.Cast<TabItem>()) {
                var tab = tabitem.Content as TabPageContent;
                checkedTags.AddRange(tab.CheckedItems);
            }
            return checkedTags;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            foreach (var file in (e.Data.GetData(DataFormats.FileDrop) as string[]) ) {

                if (!hasTagProperty(file)) {
                    continue;
                }

                files.Add(file);
            }

            if (files.Count == 0)
                return;


            if (index == -1)
                index = 0;

            if (index == 0)
                copy();

            NextOrFinish();

        }

        private void Init() {
            ImageViewer.Source = null;
            warapper.Dispose();
            int x = 0;
            foreach (var file in files) {
                Console.WriteLine(x);
                File.Delete(file);
                x++;
            }

            files.Clear();
            index = -1;
            NextOrFinish();
        }

        private void copy() {
            string name = Path.GetFileName(files[index]);
            string copyto = Path.Combine(WorkingDirectory, name);

            if (File.Exists(copyto))
                File.Delete(copyto);
            File.Copy(files[index], copyto);
            ViewImage( Path.GetFullPath(copyto) );
        }

        private void UncheckAll() {
            foreach (var content in AllContents)
                content.UncheckAllTags();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            List<dynamic> list = new List<dynamic>();
            foreach (var item in GroupTab.Items.Cast<TabItem>()) {
                var content = item.Content as TabPageContent;

                List<dynamic> tags = new List<dynamic>();

                foreach (var t in content.Tags)
                {
                    var tag = new { name = t.Name, search_keys = t.SeartchKeys };
                    tags.Add(tag);
                }

                list.Add(new { name = item.Header, tags });
            }

            var obj = new { groups = list };
            Util.WriteJson("tag.json" , obj);
        }

        private void UncheckButton_Click(object sender, RoutedEventArgs e)
        {
            UncheckAll();
        }
    }
}
