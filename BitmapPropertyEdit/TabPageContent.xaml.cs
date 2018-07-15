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
        private List<Tag> tags = new List<Tag>();

        /// <summary>
        /// チェックされたタグ名をすべて取得する
        /// </summary>
        public List<string> CheckedItems
        {
            get
            {
                List<string> list = new List<string>();
                foreach (var checkbox in TagArea.Items.Cast<CheckBox>().Where(x => x.IsChecked == true))
                {
                    list.Add(checkbox.Content as string);
                }
                return list;
            }
        }

        /// <summary>
        /// すべてのタグ名を取得する
        /// </summary>
        public List<Tag> Tags {
            get
            {
    
                var list = new List<Tag>();
                foreach (var checkbox in TagArea.Items.Cast<CheckBox>())
                {
                    list.Add(checkbox.Tag as Tag);
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
        public TabPageContent(List<Tag> tags) : this() {
            this.tags = tags;
            ShowTagAll();
        }

        /// <summary>
        /// タグをすべて表示する
        /// </summary>
        private void ShowTagAll() {
            TagArea.Items.Clear();
            foreach (var tag in tags.OrderBy(x => x.Name)) {
                ShowTag(tag);
            }
        }

        /// <summary>
        /// タグのチェックボックスを表示する
        /// </summary>
        /// <param name="text"></param>
        private void ShowTag(Tag tag) {
            CheckBox checkBox = new CheckBox() { Content = tag.Name, Tag = tag };
            TagArea.Items.Add(checkBox);
        }

        /// <summary>
        /// 前方一致検索
        /// </summary>
        /// <param name="text"></param>
        private void Search(string text) {
            foreach (var checkbox in TagArea.Items.Cast<CheckBox>())
            {
                checkbox.Visibility =
                    checkbox.Content.ToString().StartsWith(text) || (checkbox.Tag as Tag).hasSearchKeysThathStartsWithKey(text)
                    ? Visibility.Visible : Visibility.Hidden;
            }
        }

        /// <summary>
        /// タグを追加する
        /// </summary>
        /// <param name="text"></param>
        private void AddTag(string text) {

            if (tags.Select(x => x.Name).Contains(text.Trim()))
                return;

            var tag = new Tag() { Name = text };
            tags.Add(tag);
            ShowTag(tag);
          }

        /// <summary>
        /// すべてのタグからチェックを外す
        /// </summary>
        public void UncheckAllTags() {
            foreach (var checkbox in TagArea.Items.Cast<CheckBox>())
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
