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
using System.Windows.Shapes;

namespace BitmapPropertyEdit
{
    /// <summary>
    /// TabPageContent.xaml の相互作用ロジック
    /// </summary>
    public partial class TabPageContent : UserControl
    {
        private List<string> tags = new List<string>();

        /// <summary>
        /// チェックされたタグ名をすべて取得する
        /// </summary>
        public List<string> CheckedItems
        {
            get
            {
                List<string> list = new List<string>();
                foreach (var checkbox in TagArea.Children.Cast<CheckBox>().Where(x => x.IsChecked == true))
                {
                    list.Add(checkbox.Content as string);
                }
                return list;
            }
        }

        /// <summary>
        /// すべてのタグ名を取得する
        /// </summary>
        public List<string> Tags {
            get
            {
    
                List<string> list = new List<string>();
                foreach (var checkbox in TagArea.Children.Cast<CheckBox>())
                {
                    list.Add(checkbox.Content as string);
                }
                return list;
            }
        }


        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public TabPageContent()
        {
            InitializeComponent();
        }

        /// <summary>
        /// カスタムコンストラクタ
        /// </summary>
        /// <param name="tags"></param>
        public TabPageContent(List<string> tags) : this() {
            this.tags = tags;
            ShowTagAll();
        }

        /// <summary>
        /// タグをすべて表示する
        /// </summary>
        private void ShowTagAll() {
            TagArea.Children.Clear();
            foreach (var tag in tags.OrderBy(x => x)) {
                ShowTag(tag);
            }
        }

        /// <summary>
        /// タグのチェックボックスを表示する
        /// </summary>
        /// <param name="text"></param>
        private void ShowTag(string text) {
            CheckBox checkBox = new CheckBox() { Content = text, Tag = text };
            TagArea.Children.Add(checkBox);
        }

        /// <summary>
        /// 前方一致検索
        /// </summary>
        /// <param name="text"></param>
        private void Search(string text) {
            foreach (var checkbox in TagArea.Children.Cast<CheckBox>())
            {
                checkbox.Visibility = checkbox.Content.ToString().StartsWith(text) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        /// <summary>
        /// タグを追加する
        /// </summary>
        /// <param name="text"></param>
        private void AddTag(string text) {

            if (tags.Contains(text.Trim()))
                return;

            tags.Add(text);
            ShowTag(text);
          }

        /// <summary>
        /// すべてのタグからチェックを外す
        /// </summary>
        public void UncheckAllTags() {
            foreach (var checkbox in TagArea.Children.Cast<CheckBox>())
                checkbox.IsChecked = false;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string text = TagText.Text?.Trim();
            if (!string.IsNullOrEmpty(text))
                AddTag(text);

            TagText.Text = null;
            TagText.Focus();
        }

        private void UncheckButton_Click(object sender, RoutedEventArgs e)
        {
            UncheckAllTags();
        }

        private void TagText_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = TagText.Text;
            Search(text);
        }
    }
}
