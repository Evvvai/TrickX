using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Globalization;
using System.Threading;

namespace TrickX
{
    public partial class MainWindow : Window
    {
        public List<Newsfeed> newsfeed { get; set; }
        public List<NewsfeedTrick> newsfeed_trick { get; set; }

        public class Newsfeed
        {
            public string date { get; set; }
            public string text { get; set; }
        }
        public class NewsfeedTrick
        {
            public string date { get; set; }
            public Trick trick { get; set; }
        }

        #region AddTrickFormLoad

        public void AddTrickFormLoad()
        {
            MainBar.Children.Clear();

            TabControl tc = new TabControl();
            tc.SelectionChanged += TabControl_SelectionChanged;
            tc.Name = "tc";

            TabItem trickAddTab = new TabItem();
            trickAddTab.Name = "createnewtrick";
            trickAddTab.Header = "                              Create new trick                              ";
            trickAddTab.FontSize = 15;
            trickAddTab.FontStyle = FontStyles.Italic;
            trickAddTab.FontWeight = FontWeights.SemiBold;
            trickAddTab.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tc.Items.Add(trickAddTab);

            TabItem trickConsider = new TabItem();
            trickConsider.Name = "trickсonsideration";
            trickConsider.Header = "                              Trick сonsideration                              ";
            trickConsider.FontSize = 15;
            trickConsider.FontStyle = FontStyles.Italic;
            trickConsider.FontWeight = FontWeights.SemiBold;
            trickConsider.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tc.Items.Add(trickConsider);

            TabItem newsfeed = new TabItem();
            newsfeed.Name = "newsfeed";
            newsfeed.Header = "                                     Newsfeed                                     ";
            newsfeed.FontSize = 15;
            newsfeed.FontStyle = FontStyles.Italic;
            newsfeed.FontWeight = FontWeights.SemiBold;
            newsfeed.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tc.Items.Add(newsfeed);

            TabItem trickConsiderAdmin = new TabItem();
            trickConsiderAdmin.Name = "trickсonsiderationadmin";
            trickConsiderAdmin.Header = "                                     Trick сonsideration for Admins                                     ";
            trickConsiderAdmin.FontSize = 15;
            trickConsiderAdmin.FontStyle = FontStyles.Italic;
            trickConsiderAdmin.FontWeight = FontWeights.SemiBold;
            trickConsiderAdmin.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tc.Items.Add(trickConsiderAdmin);

            TabItem videoConsiderAdmin = new TabItem();
            videoConsiderAdmin.Name = "videoсonsiderationadmin";
            videoConsiderAdmin.Header = "                                     Video сonsideration for Admins                                     ";
            videoConsiderAdmin.FontSize = 15;
            videoConsiderAdmin.FontStyle = FontStyles.Italic;
            videoConsiderAdmin.FontWeight = FontWeights.SemiBold;
            videoConsiderAdmin.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tc.Items.Add(videoConsiderAdmin);

            if (UserSetting.iAdmin == false)
            {
                trickConsiderAdmin.Visibility = Visibility.Collapsed;
                videoConsiderAdmin.Visibility = Visibility.Collapsed;
            }


            MainBar.Children.Add(tc);

            StackPanel spTrick = new StackPanel();

            StackPanel spName = new StackPanel();
            TextBlock tblockName = new TextBlock();
            TextBox tboxName = new TextBox();
            trickAddTab.Content = spTrick;
            spTrick.Children.Add(spName);
            spName.Children.Add(tblockName);
            spName.Children.Add(tboxName);
            spName.Orientation = Orientation.Horizontal;

            tblockName.Text = "Name";
            tblockName.Margin = new Thickness(10, 0, 0, 0);
            tblockName.FontSize = 20;
            tblockName.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tblockName.Width = 100;

            tboxName.Name = "Name";
            tboxName.HorizontalAlignment = HorizontalAlignment.Left;
            tboxName.VerticalAlignment = VerticalAlignment.Bottom;
            tboxName.Margin = new Thickness(5, 0, 0, 0);
            tboxName.TextAlignment = TextAlignment.Left;
            tboxName.Width = 150;
            tboxName.Height = 20;
            tboxName.FontSize = 15;
            tboxName.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 40, 40, 40));
            tboxName.SelectionTextBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tboxName.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tboxName.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tboxName.LostFocus += TextBoxTrigger_LostFocus;


            StackPanel spPoint = new StackPanel();
            TextBlock tblockPoint = new TextBlock();
            TextBox tboxPoint = new TextBox();
            trickAddTab.Content = spTrick;
            spTrick.Children.Add(spPoint);
            spPoint.Children.Add(tblockPoint);
            spPoint.Children.Add(tboxPoint);
            spPoint.Orientation = Orientation.Horizontal;

            tblockPoint.Text = "Points";
            tblockPoint.Margin = new Thickness(10, 0, 0, 0);
            tblockPoint.FontSize = 20;
            tblockPoint.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tblockPoint.Width = 100;

            tboxPoint.Name = "Points";
            tboxPoint.HorizontalAlignment = HorizontalAlignment.Left;
            tboxPoint.VerticalAlignment = VerticalAlignment.Bottom;
            tboxPoint.Margin = new Thickness(5, 0, 0, 0);
            tboxPoint.TextAlignment = TextAlignment.Left;
            tboxPoint.Width = 150;
            tboxPoint.Height = 20;
            tboxPoint.FontSize = 15;
            tboxPoint.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 40, 40, 40));
            tboxPoint.SelectionTextBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tboxPoint.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tboxPoint.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tboxPoint.LostFocus += TextBoxTrigger_LostFocus;



            StackPanel spUrl = new StackPanel();
            TextBlock tblockUrl = new TextBlock();
            TextBox tboxUrl = new TextBox();
            trickAddTab.Content = spTrick;
            spTrick.Children.Add(spUrl);
            spUrl.Children.Add(tblockUrl);
            spUrl.Children.Add(tboxUrl);
            spUrl.Orientation = Orientation.Horizontal;

            tblockUrl.Text = "Video Link";
            tblockUrl.Margin = new Thickness(10, 0, 0, 0);
            tblockUrl.FontSize = 20;
            tblockUrl.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tblockUrl.Width = 100;

            tboxUrl.Name = "url";
            tboxUrl.HorizontalAlignment = HorizontalAlignment.Left;
            tboxUrl.VerticalAlignment = VerticalAlignment.Bottom;
            tboxUrl.Margin = new Thickness(5, 0, 0, 0);
            tboxUrl.TextAlignment = TextAlignment.Left;
            tboxUrl.Width = 150;
            tboxUrl.Height = 20;
            tboxUrl.FontSize = 15;
            tboxUrl.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 40, 40, 40));
            tboxUrl.SelectionTextBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tboxUrl.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tboxUrl.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tboxUrl.LostFocus += TextBoxTrigger_LostFocus;


            StackPanel spAuthor = new StackPanel();
            TextBlock tblockAuthor = new TextBlock();
            TextBox tboxAuthor = new TextBox();
            trickAddTab.Content = spTrick;
            spTrick.Children.Add(spAuthor);
            spAuthor.Children.Add(tblockAuthor);
            spAuthor.Children.Add(tboxAuthor);
            spAuthor.Orientation = Orientation.Horizontal;

            tblockAuthor.Text = "Author";
            tblockAuthor.Margin = new Thickness(10, 0, 0, 0);
            tblockAuthor.FontSize = 20;
            tblockAuthor.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tblockAuthor.Width = 100;

            tboxAuthor.Name = "Author";
            tboxAuthor.HorizontalAlignment = HorizontalAlignment.Left;
            tboxAuthor.VerticalAlignment = VerticalAlignment.Bottom;
            tboxAuthor.Margin = new Thickness(5, 0, 0, 0);
            tboxAuthor.TextAlignment = TextAlignment.Left;
            tboxAuthor.Width = 150;
            tboxAuthor.Height = 20;
            tboxAuthor.FontSize = 15;
            tboxAuthor.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 40, 40, 40));
            tboxAuthor.SelectionTextBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tboxAuthor.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tboxAuthor.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tboxAuthor.LostFocus += TextBoxTrigger_LostFocus;


            StackPanel spTrigger = new StackPanel();
            TextBlock tblockTrigger = new TextBlock();
            TextBox tboxTrigger = new TextBox();
            trickAddTab.Content = spTrick;
            spTrick.Children.Add(spTrigger);
            spTrigger.Children.Add(tblockTrigger);
            spTrigger.Children.Add(tboxTrigger);
            spTrigger.Orientation = Orientation.Horizontal;

            tblockTrigger.Text = "Triggers";
            tblockTrigger.Margin = new Thickness(10, 0, 0, 0);
            tblockTrigger.FontSize = 20;
            tblockTrigger.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tblockTrigger.Width = 100;

            tboxTrigger.Name = "Triggers";
            tboxTrigger.HorizontalAlignment = HorizontalAlignment.Left;
            tboxTrigger.VerticalAlignment = VerticalAlignment.Bottom;
            tboxTrigger.Margin = new Thickness(5, 0, 0, 0);
            tboxTrigger.TextAlignment = TextAlignment.Left;
            tboxTrigger.Width = 150;
            tboxTrigger.Height = 20;
            tboxTrigger.FontSize = 15;
            tboxTrigger.Text = "1";
            tboxTrigger.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 40, 40, 40));
            tboxTrigger.SelectionTextBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tboxTrigger.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tboxTrigger.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tboxTrigger.LostFocus += TextBoxTrigger_LostFocus;

            ScrollViewer sv = new ScrollViewer();
            StackPanel spTr = new StackPanel();
            spTr.Name = "sptr";
            sv.Margin = new Thickness(0, 15, 0, 0);
            sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            sv.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            sv.Style = Application.Current.FindResource("FavsScrollViewer") as Style;
            spTrick.Children.Add(sv);
            sv.Content = spTr;
            spTr.Orientation = Orientation.Horizontal;

            AddTriggerList(1);

            StackPanel spConfirm = new StackPanel();
            TextBlock tblockConfirm = new TextBlock();
            TextBlock tblockAlreadyYoure = new TextBlock();
            TextBlock tblockAlreadyAll = new TextBlock();
            trickAddTab.Content = spTrick;
            spTrick.Children.Add(spConfirm);
            spConfirm.Children.Add(tblockAlreadyYoure);
            spConfirm.Children.Add(tblockAlreadyAll);
            spConfirm.Children.Add(tblockConfirm);
            spConfirm.Orientation = Orientation.Vertical;
            spConfirm.Margin = new Thickness(0, 180, 0, 0);
            spConfirm.HorizontalAlignment = HorizontalAlignment.Left;

            tblockAlreadyYoure.Text = "Count your tricks on consideration - " + sql_CONN.Select($"SELECT count(id) FROM tricks_consider WHERE id_user = '{UserSetting.id}'")[0];
            tblockAlreadyYoure.Margin = new Thickness(10, 0, 0, 0);
            tblockAlreadyYoure.FontSize = 15;
            tblockAlreadyYoure.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 200, 200));
            tblockAlreadyYoure.Width = 1000;
            tblockAlreadyYoure.FontStyle = FontStyles.Italic;
            tblockAlreadyYoure.FontWeight = FontWeights.SemiBold;

            tblockAlreadyAll.Text = "Count all tricks on consideration - " + sql_CONN.Select($"SELECT count(id) FROM tricks_consider")[0];
            tblockAlreadyAll.Margin = new Thickness(10, 0, 0, 0);
            tblockAlreadyAll.FontSize = 15;
            tblockAlreadyAll.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 200, 200));
            tblockAlreadyAll.Width = 1000;
            tblockAlreadyAll.FontStyle = FontStyles.Italic;
            tblockAlreadyAll.FontWeight = FontWeights.SemiBold;

            tblockConfirm.Text = "Send request for a triсk";
            tblockConfirm.Margin = new Thickness(10, 0, 0, 0);
            tblockConfirm.FontSize = 25;
            tblockConfirm.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 255, 200));
            tblockConfirm.Width = 1000;
            tblockConfirm.FontStyle = FontStyles.Italic;
            tblockConfirm.FontWeight = FontWeights.SemiBold;
            tblockConfirm.MouseDown += SendRequestTrick_MouseDown;


            ScrollViewer svTrick = new ScrollViewer();
            trickConsiderAdmin.Content = svTrick;
            TrickConsiderationAdminsLoad();

            svTrick = new ScrollViewer();
            trickConsider.Content = svTrick;
            TrickConsiderationLoad();

            svTrick = new ScrollViewer();
            videoConsiderAdmin.Content = svTrick;
            VideoConsiderationAdminsLoad();

            svTrick = new ScrollViewer();
            newsfeed.Content = svTrick;
            UpadateNewsfeed();

        }

        public void AddTriggerList(int count = 0)
        {
            List<string> TriggersName = new List<string>();
            for (int i = 0; i < ski2_triggers.Count; i++) TriggersName.Add(ski2_triggers[i].name);
            StackPanel spTr = new StackPanel();

            foreach (var item in MainBar.Children)
            {
                if (item is TabControl tc)
                {
                    TabItem tabItem = (TabItem)tc.Items[0];
                    StackPanel spTrick = (StackPanel)tabItem.Content;
                    foreach (var item1 in spTrick.Children)
                    {
                        if (item1 is ScrollViewer sv)
                        {
                            spTr = (StackPanel)sv.Content;
                        }
                    }

                }
            }

            spTr.Children.Clear();

            for (int i = 0; i < count; i++)
            {
                StackPanel spTrPart = new StackPanel();
                ComboBox cb = new ComboBox();
                Image img = new Image();
                spTr.Children.Add(spTrPart);
                spTrPart.Children.Add(cb);
                spTrPart.Children.Add(img);

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("/Triggers/smthwrong.png", UriKind.Relative);
                bi.EndInit();

                spTrPart.Margin = new Thickness(5, 0, 0, 0);

                cb.Height = 25;
                cb.Width = 150;
                cb.ItemsSource = TriggersName;
                cb.IsEditable = true;
                cb.SelectionChanged += ComboBox_SelectionChanged;
                cb.Tag = i;

                img.Width = 150;
                img.Height = 150;
                img.Source = bi;

                Base64Converter imgConverter = new Base64Converter();
                Binding binding = new Binding();
                binding.Source = cb;
                binding.Path = new PropertyPath("Text");
                binding.Converter = imgConverter;
                binding.Mode = BindingMode.TwoWay;
                img.SetBinding(Image.SourceProperty, binding);
            }
        }

        public void TrickConsiderationAdminsLoad()
        {
            ScrollViewer svTrickCo = new ScrollViewer();

            foreach (var item in MainBar.Children)
            {
                if (item is TabControl tc)
                {
                    TabItem tabItem = (TabItem)tc.Items[3];
                    svTrickCo = (ScrollViewer)tabItem.Content;
                }
            }

            svTrickCo.Style = Application.Current.FindResource("MyCoolScrollViewerStyle") as Style;
            svTrickCo.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 200, 200, 200));
            StackPanel sp = new StackPanel();
            svTrickCo.Content = sp;
            sp.Children.Clear();

            for (int i = 0; i < ski2_tricks_consider.Count; i++)
            {
                if (ski2_tricks_consider[i].permission == "1") continue;

                Expander exp = new Expander();
                exp.Tag = ski2_tricks_consider[i].id;
                exp.Header = ski2_tricks_consider[i].points + " | " + ski2_tricks_consider[i].name;
                exp.Style = Application.Current.FindResource("MyCoolExpanderStyle") as Style;
                exp.MouseEnter += Expander_MouseEnter;
                exp.MouseLeave += Expander_MouseLeave;

                sp.Children.Add(exp);
                StackPanel sp1 = new StackPanel();
                exp.Content = sp1;

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                ScrollViewer svTrick = new ScrollViewer();
                svTrick.Style = Application.Current.FindResource("FavsScrollViewer") as Style;
                svTrick.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                svTrick.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                sp1.Children.Add(svTrick);
                StackPanel spTrick = new StackPanel();
                svTrick.Content = spTrick;

                StackPanel spTb = new StackPanel();
                spTb.Orientation = Orientation.Horizontal;
                spTrick.Children.Add(spTb);

                StackPanel spImg = new StackPanel();
                spImg.Orientation = Orientation.Horizontal;
                spTrick.Children.Add(spImg);

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                for (int j = 0; j < ski2_tricks_consider[i].id_triggers.Count; j++)
                {
                    TextBlock tb = new TextBlock();
                    tb.TextWrapping = TextWrapping.Wrap;
                    tb.Margin = new Thickness(5, 15, 0, 0);
                    tb.FontSize = 18f;
                    tb.Width = 150;
                    tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
                    tb.HorizontalAlignment = HorizontalAlignment.Left;
                    tb.TextAlignment = TextAlignment.Center;
                    tb.Text += (j + 1) + "\n" + ski2_tricks_consider[i].id_triggers[j].Item2;

                    spTb.Children.Add(tb);

                    Image img0 = new Image();
                    img0.Name = "trickImg";
                    img0.HorizontalAlignment = HorizontalAlignment.Left;
                    img0.VerticalAlignment = VerticalAlignment.Center;
                    img0.Margin = new Thickness(5, 4, 0, 5);
                    img0.Width = 150;
                    img0.Height = 150;
                    img0.Source = SwitchImage(ski2_tricks_consider[i].id_triggers[j].Item1);

                    spImg.Children.Add(img0);
                }

                TextBlock tbDate = new TextBlock();
                tbDate.Margin = new Thickness(25, 5, 0, 0);
                tbDate.FontSize = 18;
                tbDate.HorizontalAlignment = HorizontalAlignment.Left;
                tbDate.TextAlignment = TextAlignment.Center;
                tbDate.Text = "• Date - " + ski2_tricks_consider[i].date;
                spTrick.Children.Add(tbDate);

                TextBlock tbUser = new TextBlock();
                tbUser.Margin = new Thickness(25, 5, 0, 0);
                tbUser.FontSize = 18;
                tbUser.HorizontalAlignment = HorizontalAlignment.Left;
                tbUser.TextAlignment = TextAlignment.Center;
                tbUser.Text = "• User - " + ski2_tricks_consider[i].id_user + "|" + sql_CONN.Select($"SELECT login FROM surf_users WHERE id = '{ski2_tricks_consider[i].id_user}'")[0];
                spTrick.Children.Add(tbUser);

                TextBlock tbAuthor = new TextBlock();
                tbAuthor.Margin = new Thickness(25, 5, 0, 0);
                tbAuthor.FontSize = 18;
                tbAuthor.HorizontalAlignment = HorizontalAlignment.Left;
                tbAuthor.TextAlignment = TextAlignment.Center;
                tbAuthor.Text = "• Author - " + ski2_tricks_consider[i].video[0].Item2;
                spTrick.Children.Add(tbAuthor);

                TextBlock tbLink = new TextBlock();
                tbLink.Margin = new Thickness(25, 5, 0, 0);
                tbLink.FontSize = 18;
                tbLink.HorizontalAlignment = HorizontalAlignment.Left;
                tbLink.TextAlignment = TextAlignment.Center;
                tbLink.Text = "• Video trick complete";
                spTrick.Children.Add(tbLink);

                if (ski2_tricks_consider[i].video[0].Item1 != "")
                {
                    tbLink = new TextBlock();
                    tbLink.Margin = new Thickness(50, 5, 0, 0);
                    tbLink.FontSize = 18;
                    tbLink.HorizontalAlignment = HorizontalAlignment.Left;
                    tbLink.TextAlignment = TextAlignment.Center;
                    tbLink.Text = ski2_tricks_consider[i].video[0].Item1;
                    tbLink.Tag = ski2_tricks_consider[i].video[0].Item1;
                    tbLink.MouseDown += OpenLink;
                    spTrick.Children.Add(tbLink);
                }
                else
                {
                    tbLink = new TextBlock();
                    tbLink.Margin = new Thickness(50, 5, 0, 0);
                    tbLink.FontSize = 18;
                    tbLink.HorizontalAlignment = HorizontalAlignment.Left;
                    tbLink.TextAlignment = TextAlignment.Center;
                    tbLink.Text = "Nope";
                    spTrick.Children.Add(tbLink);
                }

                StackPanel condec = new StackPanel();
                condec.Orientation = Orientation.Horizontal;
                condec.Margin = new Thickness(0, 10, 0, 0);
                spTrick.Children.Add(condec);

                TextBlock textBlock = new TextBlock();
                textBlock.Text = "Confirm";
                textBlock.Tag = ski2_tricks_consider[i].id;
                textBlock.Margin = new Thickness(5, 0, 0, 0);
                textBlock.FontSize = 20;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 255, 200));
                textBlock.FontStyle = FontStyles.Normal;
                textBlock.FontWeight = FontWeights.SemiBold;
                textBlock.MouseDown += TrickConfirm_MouseDown;
                condec.Children.Add(textBlock);

                textBlock = new TextBlock();
                textBlock.Text = "Decline";
                textBlock.Tag = ski2_tricks_consider[i].id;
                textBlock.Margin = new Thickness(25, 0, 0, 0);
                textBlock.FontSize = 20;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 200, 200));
                textBlock.FontStyle = FontStyles.Normal;
                textBlock.FontWeight = FontWeights.SemiBold;
                textBlock.MouseDown += TrickDecline_MouseDown;
                condec.Children.Add(textBlock);

                textBlock = new TextBlock();
                textBlock.Text = "Send to ppl consider";
                textBlock.Tag = ski2_tricks_consider[i].id;
                textBlock.Margin = new Thickness(25, 0, 0, 0);
                textBlock.FontSize = 20;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 200, 200));
                textBlock.FontStyle = FontStyles.Normal;
                textBlock.FontWeight = FontWeights.SemiBold;
                textBlock.MouseDown += TrickSendPpl_MouseDown;
                condec.Children.Add(textBlock);
            }
        }

        public void TrickConsiderationLoad()
        {
            ScrollViewer svTrickCo = new ScrollViewer();

            foreach (var item in MainBar.Children)
            {
                if (item is TabControl tc)
                {
                    TabItem tabItem = (TabItem)tc.Items[1];
                    svTrickCo = (ScrollViewer)tabItem.Content;
                }
            }

            svTrickCo.Style = Application.Current.FindResource("MyCoolScrollViewerStyle") as Style;
            svTrickCo.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 200, 200, 200));

            svTrickCo.Style = Application.Current.FindResource("MyCoolScrollViewerStyle") as Style;
            StackPanel sp = new StackPanel();
            svTrickCo.Content = sp;
            sp.Children.Clear();

            for (int i = 0; i < ski2_tricks_consider.Count; i++)
            {
                if (ski2_tricks_consider[i].permission == "0") continue;

                Expander exp = new Expander();
                exp.Tag = ski2_tricks_consider[i].id;
                exp.Header = ski2_tricks_consider[i].points + " | " + ski2_tricks_consider[i].name;
                exp.Style = Application.Current.FindResource("MyCoolExpanderStyle") as Style;
                exp.MouseEnter += Expander_MouseEnter;
                exp.MouseLeave += Expander_MouseLeave;

                sp.Children.Add(exp);
                StackPanel sp1 = new StackPanel();
                exp.Content = sp1;

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                ScrollViewer svTrick = new ScrollViewer();
                svTrick.Style = Application.Current.FindResource("FavsScrollViewer") as Style;
                svTrick.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                svTrick.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                sp1.Children.Add(svTrick);
                StackPanel spTrick = new StackPanel();
                svTrick.Content = spTrick;

                StackPanel spTb = new StackPanel();
                spTb.Orientation = Orientation.Horizontal;
                spTrick.Children.Add(spTb);

                StackPanel spImg = new StackPanel();
                spImg.Orientation = Orientation.Horizontal;
                spTrick.Children.Add(spImg);

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                for (int j = 0; j < ski2_tricks_consider[i].id_triggers.Count; j++)
                {
                    TextBlock tb = new TextBlock();
                    tb.TextWrapping = TextWrapping.Wrap;
                    tb.Margin = new Thickness(5, 15, 0, 0);
                    tb.FontSize = 18f;
                    tb.Width = 150;
                    tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
                    tb.HorizontalAlignment = HorizontalAlignment.Left;
                    tb.TextAlignment = TextAlignment.Center;
                    tb.Text += (j + 1) + "\n" + ski2_tricks_consider[i].id_triggers[j].Item2;

                    spTb.Children.Add(tb);

                    Image img0 = new Image();
                    img0.Name = "trickImg";
                    img0.HorizontalAlignment = HorizontalAlignment.Left;
                    img0.VerticalAlignment = VerticalAlignment.Center;
                    img0.Margin = new Thickness(5, 4, 0, 5);
                    img0.Width = 150;
                    img0.Height = 150;
                    img0.Source = SwitchImage(ski2_tricks_consider[i].id_triggers[j].Item1);

                    spImg.Children.Add(img0);
                }

                TextBlock tbAuthor = new TextBlock();
                tbAuthor.Margin = new Thickness(25, 15, 0, 0);
                tbAuthor.FontSize = 18f;
                tbAuthor.HorizontalAlignment = HorizontalAlignment.Left;
                tbAuthor.TextAlignment = TextAlignment.Center;
                if(ski2_tricks_consider[i].video[0].Item2 == "") tbAuthor.Text = "• Author - Unknow";
                else tbAuthor.Text = "• Author - " + ski2_tricks_consider[i].video[0].Item2;

                spTrick.Children.Add(tbAuthor);

                if (ski2_tricks_consider[i].video[0].Item1 != "")
                {
                    TextBlock tbLink = new TextBlock();
                    tbLink.Margin = new Thickness(25, 15, 0, 0);
                    tbLink.FontSize = 18f;
                    tbLink.HorizontalAlignment = HorizontalAlignment.Left;
                    tbLink.TextAlignment = TextAlignment.Center;
                    tbLink.Text = "• Video trick complete";
                    tbLink.Tag = ski2_tricks_consider[i].video[0].Item1;
                    tbLink.MouseDown += OpenLink;
                    spTrick.Children.Add(tbLink);
                }

                StackPanel rating = new StackPanel();
                rating.Orientation = Orientation.Horizontal;
                rating.Margin = new Thickness(25, 25, 0, 0);
                spTrick.Children.Add(rating);
                TextBlock textBlock = new TextBlock();

                var rate = sql_CONN.Select($"SELECT count(id) FROM rate_trick WHERE id_user = '{UserSetting.id}' && id_trick = '{ski2_tricks_consider[i].id}'")[0];
                var rateYes = sql_CONN.Select($"SELECT count(id) FROM rate_trick WHERE rate = '1' && id_trick = '{ski2_tricks_consider[i].id}'")[0];
                var rateNo = sql_CONN.Select($"SELECT count(id) FROM rate_trick WHERE rate = '0' && id_trick = '{ski2_tricks_consider[i].id}'")[0];
                var ratePercent = 0;
                if (rateYes != "0") ratePercent = Convert.ToInt32(rateYes) * (100 / (Convert.ToInt32(rateYes) + Convert.ToInt32(rateNo)));

                if (rate == "0")
                {
                    textBlock = new TextBlock();
                    textBlock.Text = "+";
                    textBlock.Tag = ski2_tricks_consider[i].id;
                    textBlock.Margin = new Thickness(5, 0, 0, 0);
                    textBlock.FontSize = 30;
                    textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 255, 200));
                    textBlock.FontStyle = FontStyles.Normal;
                    textBlock.FontWeight = FontWeights.SemiBold;
                    textBlock.MouseDown += RateUp_MouseDown;
                    rating.Children.Add(textBlock);
                }


                textBlock = new TextBlock();
                textBlock.Text = ratePercent + "\n" + "Rating";
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.Margin = new Thickness(5, 0, 0, 0);
                textBlock.FontSize = 20;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
                textBlock.FontStyle = FontStyles.Normal;
                textBlock.FontWeight = FontWeights.SemiBold;
                rating.Children.Add(textBlock);

                if (rate == "0")
                {
                    textBlock = new TextBlock();
                    textBlock.Text = "-";
                    textBlock.Tag = ski2_tricks_consider[i].id;
                    textBlock.Margin = new Thickness(5, 0, 0, 0);
                    textBlock.FontSize = 30;
                    textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 200, 200));
                    textBlock.FontStyle = FontStyles.Normal;
                    textBlock.FontWeight = FontWeights.SemiBold;
                    textBlock.MouseDown += RateDown_MouseDown;
                    rating.Children.Add(textBlock);
                }


                if (UserSetting.iAdmin != true) return;
                StackPanel condec = new StackPanel();
                condec.Orientation = Orientation.Horizontal;
                condec.Margin = new Thickness(0, 10, 0, 0);
                spTrick.Children.Add(condec);

                textBlock = new TextBlock();
                textBlock.Text = "Confirm";
                textBlock.Margin = new Thickness(5, 0, 0, 0);
                textBlock.FontSize = 20;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 255, 200));
                textBlock.FontStyle = FontStyles.Normal;
                textBlock.FontWeight = FontWeights.SemiBold;
                textBlock.MouseDown += TrickConfirm_MouseDown;
                condec.Children.Add(textBlock);

                textBlock = new TextBlock();
                textBlock.Text = "Decline";
                textBlock.Margin = new Thickness(25, 0, 0, 0);
                textBlock.FontSize = 20;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 200, 200));
                textBlock.FontStyle = FontStyles.Normal;
                textBlock.FontWeight = FontWeights.SemiBold;
                textBlock.MouseDown += TrickDecline_MouseDown;
                condec.Children.Add(textBlock);
            }
        }

        public void NewsfeedLoad()
        {
            ScrollViewer svTrick = new ScrollViewer();

            foreach (var item in MainBar.Children)
            {
                if (item is TabControl tc)
                {
                    TabItem tabItem = (TabItem)tc.Items[2];
                    svTrick = (ScrollViewer)tabItem.Content;
                }
            }

            svTrick.Style = Application.Current.FindResource("MyCoolScrollViewerStyle") as Style;
            svTrick.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 200, 200, 200));

            StackPanel spNewAll = new StackPanel();
            svTrick.Content = spNewAll;
            spNewAll.Orientation = Orientation.Horizontal;

            StackPanel spNews = new StackPanel();
            spNewAll.Children.Add(spNews);
            spNews.Width = 500;

            StackPanel spNewsTrick = new StackPanel();
            spNewAll.Children.Add(spNewsTrick);

            for (int i = 0; i < newsfeed.Count; i++)
            {
                StackPanel news = new StackPanel();
                news.Orientation = Orientation.Horizontal;
                news.Margin = new Thickness(0, 15, 0, 0);
                spNews.Children.Add(news);

                TextBlock textBlock = new TextBlock();
                textBlock.Text = "• Date -";
                textBlock.Margin = new Thickness(5, 0, 0, 0);
                textBlock.FontSize = 15;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 200, 200));
                textBlock.FontStyle = FontStyles.Normal;
                textBlock.FontWeight = FontWeights.SemiBold;
                news.Children.Add(textBlock);

                textBlock = new TextBlock();
                textBlock.Text = newsfeed[i].date;
                textBlock.Margin = new Thickness(5, 0, 0, 0);
                textBlock.FontSize = 15;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 221, 221));
                textBlock.FontStyle = FontStyles.Normal;
                textBlock.FontWeight = FontWeights.SemiBold;
                news.Children.Add(textBlock);

                StackPanel newsText = new StackPanel();
                newsText.Orientation = Orientation.Horizontal;
                newsText.Margin = new Thickness(0, 1, 0, 0);
                spNews.Children.Add(newsText);

                textBlock = new TextBlock();
                textBlock.Text = newsfeed[i].text;
                textBlock.Margin = new Thickness(5, 0, 0, 0);
                textBlock.FontSize = 20;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
                textBlock.FontStyle = FontStyles.Normal;
                textBlock.FontWeight = FontWeights.SemiBold;
                newsText.Children.Add(textBlock);

            }


            TextBlock tblockAlreadyYoure = new TextBlock();
            tblockAlreadyYoure.Text = "Count your tricks on consideration - " + sql_CONN.Select($"SELECT count(id) FROM tricks_consider WHERE id_user = '{UserSetting.id}'")[0];
            tblockAlreadyYoure.Margin = new Thickness(10, 0, 0, 0);
            tblockAlreadyYoure.FontSize = 15;
            tblockAlreadyYoure.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 200, 200));
            tblockAlreadyYoure.Width = 1000;
            tblockAlreadyYoure.FontStyle = FontStyles.Italic;
            tblockAlreadyYoure.FontWeight = FontWeights.SemiBold;

            TextBlock tblockAlreadyAll = new TextBlock();
            tblockAlreadyAll.Text = "Count all tricks on consideration - " + sql_CONN.Select($"SELECT count(id) FROM tricks_consider")[0];
            tblockAlreadyAll.Margin = new Thickness(10, 0, 0, 0);
            tblockAlreadyAll.FontSize = 15;
            tblockAlreadyAll.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 200, 200));
            tblockAlreadyAll.Width = 1000;
            tblockAlreadyAll.FontStyle = FontStyles.Italic;
            tblockAlreadyAll.FontWeight = FontWeights.SemiBold;

            spNewsTrick.Children.Add(tblockAlreadyAll);
            spNewsTrick.Children.Add(tblockAlreadyYoure);

            tblockAlreadyYoure = new TextBlock();
            tblockAlreadyYoure.Text = "Count your videos on consideration - " + sql_CONN.Select($"SELECT count(id) FROM trick_video_consider WHERE id_user = '{UserSetting.id}'")[0];
            tblockAlreadyYoure.Margin = new Thickness(10, 10, 0, 0);
            tblockAlreadyYoure.FontSize = 15;
            tblockAlreadyYoure.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 200, 200));
            tblockAlreadyYoure.Width = 1000;
            tblockAlreadyYoure.FontStyle = FontStyles.Italic;
            tblockAlreadyYoure.FontWeight = FontWeights.SemiBold;

            tblockAlreadyAll = new TextBlock();
            tblockAlreadyAll.Text = "Count all videos on consideration - " + sql_CONN.Select($"SELECT count(id) FROM trick_video_consider")[0];
            tblockAlreadyAll.Margin = new Thickness(10, 0, 0, 0);
            tblockAlreadyAll.FontSize = 15;
            tblockAlreadyAll.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 200, 200));
            tblockAlreadyAll.Width = 1000;
            tblockAlreadyAll.FontStyle = FontStyles.Italic;
            tblockAlreadyAll.FontWeight = FontWeights.SemiBold;

            spNewsTrick.Children.Add(tblockAlreadyYoure);
            spNewsTrick.Children.Add(tblockAlreadyAll);

            TextBlock newtrick = new TextBlock();
            newtrick.Text = "New trick upload";
            newtrick.Margin = new Thickness(14, 15, 0, 0);
            newtrick.FontSize = 18;
            newtrick.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 235, 225));
            newtrick.Width = 1000;
            newtrick.FontStyle = FontStyles.Italic;
            newtrick.FontWeight = FontWeights.SemiBold;

            spNewsTrick.Children.Add(newtrick);

            for (int i = 0; i < newsfeed_trick.Count; i++)
            {
                StackPanel spTrick = new StackPanel();
                spTrick.Orientation = Orientation.Horizontal;
                spNewsTrick.Children.Add(spTrick);

                TextBlock trick = new TextBlock();
                trick.Margin = new Thickness(25, 4, 0, 0);
                trick.FontSize = 20;
                trick.HorizontalAlignment = HorizontalAlignment.Left;
                trick.TextAlignment = TextAlignment.Center;
                trick.VerticalAlignment = VerticalAlignment.Center;
                trick.Text = newsfeed_trick[i].trick.points + " | " + newsfeed_trick[i].trick.name;
                trick.Tag = newsfeed_trick[i].trick.id;
                trick.MouseDown += OpenTrick;
                spTrick.Children.Add(trick);

                TextBlock date = new TextBlock();
                date.Margin = new Thickness(20, 10, 0, 0);
                date.FontSize = 14;
                date.HorizontalAlignment = HorizontalAlignment.Left;
                date.TextAlignment = TextAlignment.Center;
                date.VerticalAlignment = VerticalAlignment.Center;
                date.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 221, 221));
                date.Text = newsfeed_trick[i].date;
                spTrick.Children.Add(date);
            }
        }

        public void VideoConsiderationAdminsLoad()
        {
            ScrollViewer svTrickCo = new ScrollViewer();

            foreach (var item in MainBar.Children)
            {
                if (item is TabControl tc)
                {
                    TabItem tabItem = (TabItem)tc.Items[4];
                    svTrickCo = (ScrollViewer)tabItem.Content;
                }
            }

            svTrickCo.Style = Application.Current.FindResource("MyCoolScrollViewerStyle") as Style;
            svTrickCo.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 200, 200, 200));

            StackPanel sp = new StackPanel();
            svTrickCo.Content = sp;
            sp.Children.Clear();

            for (int i = 0; i < ski2_videos_consider.Count; i++)
            {
                Expander exp = new Expander();
                exp.Tag = ski2_videos_consider[i].id;
                exp.Header = ski2_tricks.Find(item => item.id == ski2_videos_consider[i].id_trick).name;
                exp.Style = Application.Current.FindResource("MyCoolExpanderStyle") as Style;
                exp.MouseEnter += Expander_MouseEnter;
                exp.MouseLeave += Expander_MouseLeave;
                sp.Children.Add(exp);

                StackPanel video = new StackPanel();
                video.Orientation = Orientation.Vertical;
                exp.Content = video;

                StackPanel date = new StackPanel();
                date.Margin = new Thickness(0, 5, 0, 0);
                date.Orientation = Orientation.Horizontal;
                video.Children.Add(date);

                TextBlock textBlock = new TextBlock(); 
                textBlock.Text = "• Date - ";
                textBlock.Margin = new Thickness(10, 0, 0, 0);
                textBlock.FontSize = 18;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
                textBlock.FontStyle = FontStyles.Normal;
                textBlock.FontWeight = FontWeights.SemiBold;
                date.Children.Add(textBlock);

                textBlock = new TextBlock();
                textBlock.Text = ski2_videos_consider[i].date;
                textBlock.Margin = new Thickness(0, 0, 0, 0);
                textBlock.FontSize = 18;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 240));
                textBlock.FontStyle = FontStyles.Italic;
                textBlock.FontWeight = FontWeights.SemiBold;
                date.Children.Add(textBlock);

                StackPanel user = new StackPanel();
                user.Margin = new Thickness(0, 5, 0, 0);
                user.Orientation = Orientation.Horizontal;
                video.Children.Add(user);

                textBlock = new TextBlock();
                textBlock.Text = "• Id User - ";
                textBlock.Margin = new Thickness(10, 0, 0, 0);
                textBlock.FontSize = 18;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
                textBlock.FontStyle = FontStyles.Normal;
                textBlock.FontWeight = FontWeights.SemiBold;
                user.Children.Add(textBlock);

                textBlock = new TextBlock();
                textBlock.Text = ski2_videos_consider[i].id_user + "|" + sql_CONN.Select($"SELECT login FROM surf_users WHERE id = '{ski2_videos_consider[i].id_user}'")[0];
                textBlock.Margin = new Thickness(0, 0, 0, 0);
                textBlock.FontSize = 18;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 240));
                textBlock.FontStyle = FontStyles.Italic;
                textBlock.FontWeight = FontWeights.SemiBold;
                user.Children.Add(textBlock);

                StackPanel link = new StackPanel();
                link.Margin = new Thickness(0, 1, 0, 0);
                link.Orientation = Orientation.Horizontal;
                video.Children.Add(link);

                textBlock = new TextBlock();
                textBlock.Text = "• Link - ";
                textBlock.Margin = new Thickness(10, 0, 0, 0);
                textBlock.FontSize = 18;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
                textBlock.FontStyle = FontStyles.Normal;
                textBlock.FontWeight = FontWeights.SemiBold;
                link.Children.Add(textBlock);

                textBlock = new TextBlock();
                textBlock.Text = ski2_videos_consider[i].url;
                textBlock.Tag = ski2_videos_consider[i].url;
                textBlock.Margin = new Thickness(0, 0, 0, 0);
                textBlock.FontSize = 18;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 240));
                textBlock.FontStyle = FontStyles.Italic;
                textBlock.FontWeight = FontWeights.SemiBold;
                textBlock.MouseDown += OpenLink;
                link.Children.Add(textBlock);

                StackPanel author = new StackPanel();
                author.Margin = new Thickness(0, 1, 0, 0);
                author.Orientation = Orientation.Horizontal;
                video.Children.Add(author);

                textBlock = new TextBlock();
                textBlock.Text = "• Author - ";
                textBlock.Margin = new Thickness(10, 0, 0, 0);
                textBlock.FontSize = 18;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
                textBlock.FontStyle = FontStyles.Normal;
                textBlock.FontWeight = FontWeights.SemiBold;
                author.Children.Add(textBlock);

                textBlock = new TextBlock();
                textBlock.Text = ski2_videos_consider[i].author;
                textBlock.Margin = new Thickness(0, 0, 0, 0);
                textBlock.FontSize = 18;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 240));
                textBlock.FontStyle = FontStyles.Italic;
                textBlock.FontWeight = FontWeights.SemiBold;
                author.Children.Add(textBlock);

                StackPanel condec = new StackPanel();
                condec.Orientation = Orientation.Horizontal;
                condec.Margin = new Thickness(0, 5, 0, 0);
                video.Children.Add(condec);

                textBlock = new TextBlock();
                textBlock.Text = "Confirm";
                textBlock.Tag = ski2_videos_consider[i].id;
                textBlock.Margin = new Thickness(5, 0, 0, 0);
                textBlock.FontSize = 20;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 255, 200));
                textBlock.FontStyle = FontStyles.Normal;
                textBlock.FontWeight = FontWeights.SemiBold;
                textBlock.MouseDown += VideoConfirm_MouseDown;
                condec.Children.Add(textBlock);

                textBlock = new TextBlock();
                textBlock.Text = "Decline";
                textBlock.Tag = ski2_videos_consider[i].id;
                textBlock.Margin = new Thickness(25, 0, 0, 0);
                textBlock.FontSize = 20;
                textBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 200, 200));
                textBlock.FontStyle = FontStyles.Normal;
                textBlock.FontWeight = FontWeights.SemiBold;
                textBlock.MouseDown += VideoDecline_MouseDown;
                condec.Children.Add(textBlock);

            }
        }

        public void UpadateTrickConsider()
        {
            ski2_tricks_consider = new List<TrickConsider>();
            List<string> tricks_consider = sql_CONN.Select($"SELECT id,date,name,points,permission,url,author,id_user FROM tricks_consider WHERE map = '{mapId}'");
            for (int i = 0; i < tricks_consider.Count; i += 8)
            {
                TrickConsider trick = new TrickConsider()
                {
                    id = tricks_consider[i],
                    date = tricks_consider[i + 1],
                    name = tricks_consider[i + 2],
                    points = tricks_consider[i + 3],
                    permission = tricks_consider[i + 4],
                    video = new List<Tuple<string, string>>() { Tuple.Create(tricks_consider[i + 5], tricks_consider[i + 6]) },
                    id_triggers = new List<Tuple<string, string>>(),
                    id_user = tricks_consider[i + 7]
                };

                List<string> route_consider = sql_CONN.Select($"SELECT id_trigger FROM tricks_route_consider WHERE id_trick ='{tricks_consider[i]}'");


                for (int j = 0; j < route_consider.Count; j++)
                {
                    var name = ski2_triggers.Find(item => item.id == route_consider[j]).name;
                    trick.id_triggers.Add(Tuple.Create(route_consider[j], name));
                }

                ski2_tricks_consider.Add(trick);
            }
            TrickConsiderationLoad();
            if(UserSetting.iAdmin == true)
            {
                TrickConsiderationAdminsLoad();
            }
        }

        public void UpadateVideoConsider()
        {
            ski2_videos_consider = new List<VideoConsider>();
            List<string> videos_consider = sql_CONN.Select($"SELECT id,date,id_trick,url,author,id_user FROM trick_video_consider WHERE id_map = '{mapId}'");
            for (int i = 0; i < videos_consider.Count; i += 6)
            {
                VideoConsider video = new VideoConsider()
                {
                    id = videos_consider[i],
                    date = videos_consider[i + 1],
                    id_trick = videos_consider[i + 2],
                    url = videos_consider[i + 3],
                    author = videos_consider[i + 4],
                    id_map = "" + mapId,
                    id_user = videos_consider[i + 5]
                };

                ski2_videos_consider.Add(video);
            }
            VideoConsiderationAdminsLoad();
        }

        public void UpadateNewsfeed()
        {
            newsfeed = new List<Newsfeed>();
            List<string> newsfeedBuf = sql_CONN.Select($"SELECT date,text FROM newsfeed WHERE id_map = '{mapId}'");
            for (int i = 0; i < newsfeedBuf.Count; i += 2)
            {
                Newsfeed news = new Newsfeed()
                {
                    date = newsfeedBuf[i],
                    text = newsfeedBuf[i + 1]
                };

                newsfeed.Add(news);
            }
            NewsfeedLoad();
        }


        #endregion

        #region Event

        private void TextBoxTrigger_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            switch (tb.Name.ToLower())
            {
                case "name":
                    AddTrickSettings.name = tb.Text;
                    break;
                case "points":
                    AddTrickSettings.points = tb.Text;
                    break;
                case "author":
                    AddTrickSettings.author = tb.Text;
                    break;
                case "url":
                    AddTrickSettings.url = tb.Text;
                    break;
                case "triggers":
                    int count;
                    if (Int32.TryParse(tb.Text, out count))
                    {
                        if (count > 20) count = 20;
                        if (count < 1) count = 1;
                        tb.Text = count.ToString();
                        AddTrickSettings.triggers = new List<string>(count);
                        for (int i = 0; i < count; i++) AddTrickSettings.triggers.Add("");
                        AddTriggerList(count);
                    }
                    else
                    {
                        tb.Text = "1";
                        AddTrickSettings.triggers = new List<string>(0);
                        AddTriggerList(1);
                    }
                    break;
                default:
                    break;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            AddTrickSettings.triggers[Convert.ToInt32(cb.Tag)] = cb.SelectedItem.ToString();
        }

        private void SendRequestTrick_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (AddTrickSettings.name != null && AddTrickSettings.points != null)
            {
                List<string> triggers_consid = new List<string>();

                for (int i = 0; i < AddTrickSettings.triggers.Count; i++)
                {
                    if (AddTrickSettings.triggers[i] != "") triggers_consid.Add(ski2_triggers.Find(item => item.name == AddTrickSettings.triggers[i]).id);
                    else
                    {
                        triggers_consid = new List<string>();
                        break;
                    }
                }


                if (triggers_consid.Count > 0)
                {
                    sql_CONN.Insert($"INSERT INTO tricks_consider SET name = '{AddTrickSettings.name}', points = '{Convert.ToInt32(AddTrickSettings.points)}', map = '{mapId}', url = '{AddTrickSettings.url}', author = '{AddTrickSettings.author}', id_user = '{UserSetting.id}'");
                    var id = sql_CONN.Select($"SELECT id FROM tricks_consider ORDER BY id DESC LIMIT 1;")[0];
                    for (int i = 0; i < triggers_consid.Count; i++)
                    {
                        sql_CONN.Insert($"INSERT INTO tricks_route_consider SET id_trick = '{Convert.ToInt32(id)}', id_trigger = '{triggers_consid[i]}'");
                    }
                }
            }



            AddTrickSettings.name = null;
            AddTrickSettings.points = null;
            AddTrickSettings.author = null;
            AddTrickSettings.url = null;
            AddTrickFormLoad();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tabControl = (TabControl)sender;

            if (tabControl.SelectedIndex == 3 || tabControl.SelectedIndex == 1)
            {
                UpadateTrickConsider();
            }

            if (tabControl.SelectedIndex == 4)
            {
                UpadateNewsfeed();
            }

            if (tabControl.SelectedIndex == 4)
            {
                UpadateVideoConsider();
            }
        }

        private void VideoConfirm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            var str = sql_CONN.Select($"SELECT id_trick,url,author,id_map,id_user FROM trick_video_consider WHERE id = '{tb.Tag.ToString()}'");
            sql_CONN.Delete($"DELETE FROM trick_video_consider WHERE id = '{tb.Tag.ToString()}'");
            sql_CONN.Insert($"INSERT INTO trick_video SET id_trick = '{str[0]}', author = '{str[2]}', url = '{str[1]}', id_map = '{str[3]}'");

            sql_CONN.Update($"UPDATE trick_complete SET complete = '1' WHERE id_user = '{str[4]}' && id_trick = '{str[0]}'");

            UpadateVideoConsider();
        }

        private void VideoDecline_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            sql_CONN.Delete($"DELETE FROM trick_video_consider WHERE id = '{tb.Tag.ToString()}'");

            UpadateVideoConsider();
        }

        private void TrickConfirm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            var trick = sql_CONN.Select($"SELECT name,points,url,author,map FROM tricks_consider WHERE id = '{tb.Tag.ToString()}'");
            var route = sql_CONN.Select($"SELECT id_trigger FROM tricks_route_consider WHERE id_trick = '{tb.Tag.ToString()}'");
            var idLast = Convert.ToInt32(sql_CONN.Select($"SELECT id FROM tricks ORDER by id desc LIMIT 1")[0])+1;

            sql_CONN.Insert($"INSERT INTO tricks SET id = '{idLast}', name = '{trick[0]}', points = '{trick[1]}', map = '{trick[4]}'");
            if(trick[2] != "")
            {
                if (trick[3] == "") trick[3] = "Unknow";
                sql_CONN.Insert($"INSERT INTO trick_video SET id_trick = '{idLast}', author = '{trick[3]}', url = '{trick[2]}', id_map = '{trick[4]}'");
            }

            for (int i = 0; i < route.Count; i++)
            {
                    sql_CONN.Insert($"INSERT INTO tricks_route SET id_trick = '{idLast}', id_trigger = '{route[i]}', id_map = '{trick[4]}'");
            }

            sql_CONN.Delete($"DELETE FROM tricks_consider WHERE id = '{tb.Tag.ToString()}'");

            sql_CONN.Insert($"INSERT INTO newsfeed_trick SET id_trick = '{idLast}', id_map = '{trick[4]}'");

            UpadateTrickConsider();
        }

        private void TrickDecline_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            sql_CONN.Delete($"DELETE FROM tricks_consider WHERE id = '{tb.Tag.ToString()}'");

            UpadateTrickConsider();
        }

        private void TrickSendPpl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            sql_CONN.Update($"UPDATE tricks_consider SET permission = '1' WHERE id = '{tb.Tag.ToString()}'");

            UpadateTrickConsider();
        }

        private void RateUp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            sql_CONN.Insert($"INSERT INTO rate_trick SET id_trick = '{tb.Tag.ToString()}', id_user = '{UserSetting.id}', rate = '1'");

            UpadateTrickConsider();
        }

        private void RateDown_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            sql_CONN.Insert($"INSERT INTO rate_trick SET id_trick = '{tb.Tag.ToString()}', id_user = '{UserSetting.id}', rate = '0'");

            UpadateTrickConsider();
        }


        #endregion
    }

    class Base64Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Base64Encode((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is string)
                {
                    return Base64Decode((string)value);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return value;
        }

        public static BitmapImage Base64Encode(string text)
        {
            var id = MainWindow.ski2_triggers.Find(item => item.name == text)?.id;
            return MainWindow.SwitchImage(id);
        }

        public static BitmapImage Base64Decode(string text)
        {
            return MainWindow.SwitchImage(text);
        }
    }

}