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

namespace TrickX
{
    public partial class MainWindow : Window
    {
        public static int minPoints;
        public static int maxPoints;
        public static int mapId;
        public static string trickName;
        public static string triggerName;
        public static int completeStatus;
        public static int videoStatus;

        public static string SuggestVideoLink;
        public static string SuggestVideoAuthor;


        MySql_CONN sql_CONN = new MySql_CONN();

        public List<Tuple<string, string>> maps { get; set; }
        public List<Trick> ski2_tricks { get; set; }
        public List<TrickConsider> ski2_tricks_consider { get; set; }
        public List<VideoConsider> ski2_videos_consider { get; set; }
        public static List<Trigger> ski2_triggers { get; set; }

        public class Trick
        {
            public string id { get; set; }
            public string name { get; set; }
            public string points { get; set; }
            public string complete { get; set; }
            public List<Tuple<string, string>> video { get; set; }
            public List<Tuple<string,string>> id_triggers{ get; set; }
        }

        public class TrickConsider: Trick
        {
            public string date { get; set; }
            public string permission { get; set; }
            public Tuple<int, int> stats { get; set; }
            public string id_map { get; set; }
            public string id_user { get; set; }
        }

        public class VideoConsider
        {
            public string id { get; set; }
            public string date { get; set; }
            public string id_trick { get; set; }
            public string url { get; set; }
            public string author{ get; set; }
            public string id_map { get; set; }
            public string id_user { get; set; }
        }

        public class Trigger
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        class PeopleComparer : IComparer<Trick>
        {
            public int Compare(Trick x, Trick y)
            {
                if (Convert.ToInt32(x.points) < Convert.ToInt32(y.points))
                    return 1;
                else
                    return 0;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            ToolBar.Children.Clear();
            MainBar.Children.Clear();   
            Init();
        }

        #region Init

        public async void Init()
        {
            await Task.Run(() =>
            {
                sql_CONN.OpenConnection();
                maps = new List<Tuple<string, string>>();
                var str = sql_CONN.Select($"SELECT id,name FROM maps");
                for (int i = 0; i < str.Count/2; i++) maps.Add(Tuple.Create(str[i * 2], str[(i * 2) + 1]));
            });

            Auh();
        }

        #endregion

        #region MainWindowLoad

        public void MainWindowLoad()
        {
            MainBar.Children.Clear();
            ToolBar.Children.Clear();


            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            StackPanel spMap = new StackPanel();
            spMap.Name = "spMap";
            StackPanel spNews = new StackPanel();
            sp.Children.Add(spMap);
            sp.Children.Add(spNews);

            MainBar.Children.Add(sp);

            TextBlock tb = new TextBlock();
            tb.Text = "Trick surf map list";
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(15, 15, 0, 15);
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 25F;
            tb.FontWeight = FontWeights.SemiBold;
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));

            spMap.Children.Add(tb);

            tb = new TextBlock();
            tb.Text = "• Trick Surf book for easy searching and viewings tricks";
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(15, 25, 0, 0);
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 20;
            tb.Width = 700;
            tb.FontWeight = FontWeights.SemiBold;
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));

            spNews.Children.Add(tb);

            tb = new TextBlock();
            tb.Text = "• Tricks can be searched using any of the filters, to use multiple triggers at once use the following construction - triggername1,triggername2";
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(15, 25, 0, 0);
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 20;
            tb.Width = 700;
            tb.FontWeight = FontWeights.SemiBold;
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));

            spNews.Children.Add(tb);

            tb = new TextBlock();
            tb.Text = "• You can also suggest your own videos for the tricks and the tricks themselves. Your suggestion will be submitted to the administration for consideration and then either added or rejected";
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(15, 25, 0, 0);
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 20;
            tb.Width = 700;
            tb.FontWeight = FontWeights.SemiBold;
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));

            spNews.Children.Add(tb);

            tb = new TextBlock();
            tb.Text = "• You can mark the completion of a tricks yourself, but tricks higher than 800 pts will only be automatically marked if there is video confirmation";
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(15, 25, 0, 0);
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 20;
            tb.Width = 700;
            tb.FontWeight = FontWeights.SemiBold;
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));

            spNews.Children.Add(tb);

            tb = new TextBlock();
            tb.Text = "• Write any feedback here - https://steamcommunity.com/id/evvvai/";
            tb.Tag = "https://steamcommunity.com/id/evvvai/";
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(15, 25, 0, 0);
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 20;
            tb.Width = 700;
            tb.MouseDown += OpenLink;
            tb.FontWeight = FontWeights.SemiBold;
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));

            spNews.Children.Add(tb);


            tb = new TextBlock();
            tb.Text = "• Trick Surf server Connect";
            tb.Tag = "steam://connect/5.101.160.15:27015";
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(15, 270, 0, 0);
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 20;
            tb.Width = 700;
            tb.MouseDown += OpenLink;
            tb.FontWeight = FontWeights.SemiBold;
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 221, 221));

            spNews.Children.Add(tb);

            for (int i = 0; i < maps.Count; i++)
            {
                tb = new TextBlock();
                if(i==0)
                {
                    tb.Text = "• surf_" + maps[i].Item2 + " Loading...";
                }
                else
                {
                    tb.Text = "• surf_" + maps[i].Item2 + " Not implemented...";
                }
                tb.Tag = 0;
                tb.HorizontalAlignment = HorizontalAlignment.Left;
                tb.Name = "_" + (i + 1);
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Margin = new Thickness(30, 5, 0, 0);
                tb.FontFamily = new FontFamily("Century Gothic");
                tb.FontStyle = FontStyles.Italic;
                tb.FontSize = 25F;
                tb.FontWeight = FontWeights.SemiBold;
                tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 221, 221));
                tb.MouseDown += TrickList_MouseDown;

                spMap.Children.Add(tb);
            }

            sp = new StackPanel();
            sp.HorizontalAlignment = HorizontalAlignment.Right;
            sp.Orientation = Orientation.Horizontal;
            sp.Margin = new Thickness(5, 0, 0, 0);
            ToolBar.Children.Add(sp);

            BitmapImage bi1 = new BitmapImage();
            bi1.BeginInit();
            bi1.UriSource = new Uri("/Res/-.png", UriKind.Relative);
            bi1.EndInit();

            Image img = new Image();
            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.Margin = new Thickness(0, 0, 5, 0);
            img.Width = 25;
            img.Height = 25;
            img.MouseDown += MinButton_MouseDown;
            img.Source = bi1;

            sp.Children.Add(img);

            BitmapImage bi2 = new BitmapImage();
            bi2.BeginInit();
            bi2.UriSource = new Uri("/Res/x.png", UriKind.Relative);
            bi2.EndInit();

            img = new Image();
            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.Margin = new Thickness(0, 0, 5, 0);
            img.Width = 25;
            img.Height = 25;
            img.MouseDown += ExitButton_MouseDown;
            img.Source = bi2;

            sp.Children.Add(img);

        }

        #endregion

        #region TrickWindowLoad

        public void TrickWindowLoad()
        {
            TrickPlace();

            ToolBar.Children.Clear();
            MainBar.Children.Clear();

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            TextBlock tBlock = new TextBlock();
            tBlock.FontFamily = new FontFamily("Century Gothic");
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            tBlock.HorizontalAlignment = HorizontalAlignment.Center;
            tBlock.TextAlignment = TextAlignment.Center;
            tBlock.FontStyle = FontStyles.Normal;
            tBlock.FontWeight = FontWeights.SemiBold;
            tBlock.HorizontalAlignment = HorizontalAlignment.Left;
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            tBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tBlock.Width = 150;
            tBlock.FontSize = 20F;
            tBlock.TextWrapping = TextWrapping.Wrap;
            tBlock.Margin = new Thickness(0, 3, 0, 0);
            tBlock.Text = "Point | Name";
            tBlock.MouseDown += TextBlockNamePoint_MouseDown;
            ToolBar.Children.Add(tBlock);

            DockPanel dp = new DockPanel();
            dp.Margin = new Thickness(200, 0, 800, 0);
            ToolBar.Children.Add(dp);

            tBlock = new TextBlock();
            tBlock.FontFamily = new FontFamily("Century Gothic");
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            tBlock.HorizontalAlignment = HorizontalAlignment.Center;
            tBlock.TextAlignment = TextAlignment.Center;
            tBlock.FontStyle = FontStyles.Normal;
            tBlock.FontWeight = FontWeights.SemiBold;
            tBlock.HorizontalAlignment = HorizontalAlignment.Left;
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            tBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tBlock.Width = 100;
            tBlock.FontSize = 15F;
            tBlock.TextWrapping = TextWrapping.Wrap;
            tBlock.Margin = new Thickness(18, 5, 0, 0);
            tBlock.Text = "TrickName";
            DockPanel.SetDock(tBlock, Dock.Top);

            TextBox tBox = new TextBox();
            tBox.HorizontalAlignment = HorizontalAlignment.Left;
            tBox.VerticalAlignment = VerticalAlignment.Center;
            tBox.Margin = new Thickness(0, 5, 0, 0);
            tBox.TextAlignment = TextAlignment.Left;
            tBox.Width = 150;
            tBox.Height = 24;
            tBox.FontSize = 15F;
            tBox.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 40, 40, 40));
            tBox.SelectionTextBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tBox.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tBox.KeyDown += TextBoxTrickName_KeyDown;
            tBox.LostFocus += TextBoxTrickNameLostFocus;
            DockPanel.SetDock(tBox, Dock.Bottom);

            dp.Children.Add(tBlock);
            dp.Children.Add(tBox);

            dp = new DockPanel();
            dp.Margin = new Thickness(400, 0, 650, 0);
            ToolBar.Children.Add(dp);

            tBlock = new TextBlock();
            tBlock.FontFamily = new FontFamily("Century Gothic");
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            tBlock.HorizontalAlignment = HorizontalAlignment.Center;
            tBlock.TextAlignment = TextAlignment.Center;
            tBlock.FontStyle = FontStyles.Normal;
            tBlock.FontWeight = FontWeights.SemiBold;
            tBlock.HorizontalAlignment = HorizontalAlignment.Left;
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            tBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tBlock.Width = 100;
            tBlock.FontSize = 15F;
            tBlock.TextWrapping = TextWrapping.Wrap;
            tBlock.Margin = new Thickness(18, 5, 0, 0);
            tBlock.Text = "TriggerName";
            DockPanel.SetDock(tBlock, Dock.Top);

            tBox = new TextBox();
            tBox.HorizontalAlignment = HorizontalAlignment.Left;
            tBox.VerticalAlignment = VerticalAlignment.Center;
            tBox.Margin = new Thickness(0, 5, 0, 0);
            tBox.TextAlignment = TextAlignment.Left;
            tBox.Width = 150;
            tBox.Height = 24;
            tBox.FontSize = 15F;
            tBox.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 40, 40, 40));
            tBox.SelectionTextBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tBox.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tBox.KeyDown += TextBoxTriggerName_KeyDown;
            tBox.LostFocus += TextBoxTriggerNameLostFocus;
            DockPanel.SetDock(tBox, Dock.Bottom);

            dp.Children.Add(tBlock);
            dp.Children.Add(tBox);

            StackPanel sp =new StackPanel();
            sp.HorizontalAlignment = HorizontalAlignment.Right;
            sp.Orientation = Orientation.Horizontal;
            sp.Margin = new Thickness(5, 0, 0, 0);
            ToolBar.Children.Add(sp);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            dp = new DockPanel();
            dp.Margin = new Thickness(600, 0, 450, 0);
            ToolBar.Children.Add(dp);

            tBlock = new TextBlock();
            tBlock.FontFamily = new FontFamily("Century Gothic");
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            tBlock.HorizontalAlignment = HorizontalAlignment.Center;
            tBlock.TextAlignment = TextAlignment.Center;
            tBlock.FontStyle = FontStyles.Normal;
            tBlock.FontWeight = FontWeights.SemiBold;
            tBlock.HorizontalAlignment = HorizontalAlignment.Left;
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            tBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tBlock.Width = 100;
            tBlock.FontSize = 15F;
            tBlock.TextWrapping = TextWrapping.Wrap;
            tBlock.Margin = new Thickness(4, 5, 0, 0);
            tBlock.Text = "Min  -  Max";
            DockPanel.SetDock(tBlock, Dock.Top);
            dp.Children.Add(tBlock);

            tBox = new TextBox();
            tBox.Text = minPoints.ToString();
            tBox.HorizontalAlignment = HorizontalAlignment.Left;
            tBox.VerticalAlignment = VerticalAlignment.Center;
            tBox.Margin = new Thickness(0, 5, 0, 0);
            tBox.TextAlignment = TextAlignment.Center;
            tBox.Width = 50;
            tBox.Height = 24;
            tBox.MaxLength = 4;
            tBox.FontSize = 15F;
            tBox.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 40, 40, 40));
            tBox.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 255, 40, 40));
            tBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tBox.KeyDown += TextBoxMinPoints_KeyDown;
            tBox.LostFocus += TextBoxMinLostFocus;
            DockPanel.SetDock(tBox, Dock.Left);

            dp.Children.Add(tBox);

            tBox = new TextBox();
            tBox.Text = maxPoints.ToString();
            tBox.HorizontalAlignment = HorizontalAlignment.Left;
            tBox.VerticalAlignment = VerticalAlignment.Center;
            tBox.Margin = new Thickness(0, 5, 0, 0);
            tBox.TextAlignment = TextAlignment.Center;
            tBox.Width = 50;
            tBox.Height = 24;
            tBox.MaxLength = 4;
            tBox.FontSize = 15F;
            tBox.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 40, 40, 40));
            tBox.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 255, 40, 40));
            tBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
            tBox.KeyDown += TextBoxMaxPoints_KeyDown;
            tBox.LostFocus += TextBoxMaxLostFocus;
            DockPanel.SetDock(tBox, Dock.Right);

            dp.Children.Add(tBox);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            BitmapImage sEmpty = new BitmapImage();
            sEmpty.BeginInit();
            sEmpty.UriSource = new Uri("/Res/sEmpty.png", UriKind.Relative);
            sEmpty.EndInit();

            dp = new DockPanel();
            dp.Margin = new Thickness(700, 0, 350, 0);
            ToolBar.Children.Add(dp);

            tBlock = new TextBlock();
            tBlock.FontFamily = new FontFamily("Century Gothic");
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            tBlock.HorizontalAlignment = HorizontalAlignment.Center;
            tBlock.TextAlignment = TextAlignment.Center;
            tBlock.FontStyle = FontStyles.Italic;
            tBlock.FontWeight = FontWeights.SemiBold;
            tBlock.HorizontalAlignment = HorizontalAlignment.Left;
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            tBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tBlock.Width = 150;
            tBlock.FontSize = 15F;
            tBlock.TextWrapping = TextWrapping.Wrap;
            tBlock.Margin = new Thickness(0, 5, 0, 0);
            tBlock.Text = "Complete";
            DockPanel.SetDock(tBlock, Dock.Top);
            dp.Children.Add(tBlock);

            Image img = new Image();
            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.Margin = new Thickness(0, 0, 0, 0);
            img.Width = 20;
            img.Height = 20;
            img.MouseDown += CompleteStatus_MouseDown;
            img.Source = sEmpty;
            img.Tag = 0;
            DockPanel.SetDock(img, Dock.Bottom);

            dp.Children.Add(img);


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            dp = new DockPanel();
            dp.Margin = new Thickness(750, 0, 200, 0);
            ToolBar.Children.Add(dp);

            tBlock = new TextBlock();
            tBlock.FontFamily = new FontFamily("Century Gothic");
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            tBlock.HorizontalAlignment = HorizontalAlignment.Center;
            tBlock.TextAlignment = TextAlignment.Center;
            tBlock.FontStyle = FontStyles.Italic;
            tBlock.FontWeight = FontWeights.SemiBold;
            tBlock.HorizontalAlignment = HorizontalAlignment.Center;
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            tBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tBlock.Width = 150;
            tBlock.FontSize = 15F;
            tBlock.TextWrapping = TextWrapping.Wrap;
            tBlock.Margin = new Thickness(0, 5, 0, 0);
            tBlock.Text = "Video";
            DockPanel.SetDock(tBlock, Dock.Top);
            dp.Children.Add(tBlock);

            img = new Image();
            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.Margin = new Thickness(0, 0, 0, 0);
            img.Width = 20;
            img.Height = 20;
            img.MouseDown += VideoStatus_MouseDown;
            img.Source = sEmpty;
            img.Tag = 0;
            DockPanel.SetDock(img, Dock.Bottom);

            dp.Children.Add(img);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            
            dp = new DockPanel();
            dp.Margin = new Thickness(850, 0, 50, 0);
            ToolBar.Children.Add(dp);

            tBlock = new TextBlock();
            tBlock.FontFamily = new FontFamily("Century Gothic");
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            tBlock.HorizontalAlignment = HorizontalAlignment.Center;
            tBlock.TextAlignment = TextAlignment.Center;
            tBlock.FontStyle = FontStyles.Italic;
            tBlock.FontWeight = FontWeights.SemiBold;
            tBlock.HorizontalAlignment = HorizontalAlignment.Center;
            tBlock.VerticalAlignment = VerticalAlignment.Center;
            tBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tBlock.Width = 150;
            tBlock.FontSize = 15;
            tBlock.TextWrapping = TextWrapping.Wrap;
            tBlock.Margin = new Thickness(0, 5, 0, 0);
            tBlock.Text = "Trick Editor";
            tBlock.Tag = 0;
            tBlock.MouseDown += TrickAdd_MouseDown;

            DockPanel.SetDock(tBlock, Dock.Top);
            dp.Children.Add(tBlock);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            BitmapImage bi1 = new BitmapImage();
            bi1.BeginInit();
            bi1.UriSource = new Uri("/Res/-.png", UriKind.Relative);
            bi1.EndInit();

            img = new Image();
            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.Margin = new Thickness(0, 0, 10, 0);
            img.Width = 25;
            img.Height = 25;
            img.MouseDown += MinButton_MouseDown;
            img.Source = bi1;

            sp.Children.Add(img);

            BitmapImage bi2 = new BitmapImage();
            bi2.BeginInit();
            bi2.UriSource = new Uri("/Res/x.png", UriKind.Relative);
            bi2.EndInit();

            img = new Image();
            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.Margin = new Thickness(0, 0, 10, 0);
            img.Width = 25;
            img.Height = 25;
            img.MouseDown += ExitButton_MouseDown;
            img.Source = bi2;

            sp.Children.Add(img);

            TrickPlace();
        }

        #endregion

        #region TrickListLoad
        
        public async void TrickListLoad()
        {
            for (int mapi = 1; mapi <= 1; mapi++)
            {
                await Task.Run(() =>
                {
                    ski2_tricks = new List<Trick>();
                    ski2_triggers = new List<Trigger>();
                    Dictionary<string, List<string>> routes = new Dictionary<string, List<string>>();
                    Dictionary<string, List<Tuple<string, string>>> videos = new Dictionary<string, List<Tuple<string, string>>>();
                    List<string> tricks = sql_CONN.Select($"SELECT id,name,points FROM tricks WHERE map = '{mapi}'");

                    List<string> triggersAll = sql_CONN.Select($"SELECT id,name FROM triggers WHERE map = '{mapi}'"); for (int i = 0; i < triggersAll.Count / 2; i++) ski2_triggers.Add(new Trigger { id = triggersAll[i * 2], name = triggersAll[(i * 2) + 1] });
                    List<string> videosAll = sql_CONN.Select($"SELECT id_trick, author, url FROM trick_video WHERE id_map = '{mapi}'");
                    List<string> routesAll = sql_CONN.Select($"SELECT id_trick, id_trigger FROM tricks_route WHERE id_map = '{mapi}'");

                    string buf = string.Empty;
                    for (int i = 0; i < routesAll.Count; i += 2)
                    {
                        if (buf != routesAll[i])
                        {
                            routes.Add(routesAll[i], new List<string>());
                            buf = routesAll[i];
                        }
                    }

                    for (int i = 0; i < routesAll.Count; i += 2)
                    {
                        routes[routesAll[i]].Add(routesAll[i + 1]);
                    }

                    for (int i = 0; i < videosAll.Count; i += 3)
                    {
                        if (buf != videosAll[i])
                        {
                            if(!videos.ContainsKey(videosAll[i]))
                            {
                                videos.Add(videosAll[i], new List<Tuple<string, string>>());
                                buf = videosAll[i];
                            }
                        }
                    }

                    for (int i = 0; i < videosAll.Count; i += 3)
                    {
                        videos[videosAll[i]].Add(Tuple.Create(videosAll[i + 1], videosAll[i + 2]));
                    }

                    for (int i = 0; i < tricks.Count; i += 3)
                    {
                        Trick trick = new Trick()
                        {
                            id = tricks[i],
                            name = tricks[i + 1],
                            points = tricks[i + 2],
                            id_triggers = new List<Tuple<string, string>>(),
                            video = new List<Tuple<string, string>>()
                        };
                        if (routes.ContainsKey(tricks[i]))
                        {
                            for (int j = 0; j < routes[tricks[i]].Count; j++)
                            {
                                var name = ski2_triggers.Find(item => item.id == routes[tricks[i]][j]).name;
                                trick.id_triggers.Add(Tuple.Create(routes[tricks[i]][j], name));
                            }
                        }
                        if (videos.ContainsKey(tricks[i]))
                        {
                            for (int j = 0; j < videos[tricks[i]].Count; j++)
                            {
                                trick.video.Add(Tuple.Create(videos[tricks[i]][j].Item1, videos[tricks[i]][j].Item2));
                            }
                        }

                        ski2_tricks.Add(trick);
                    }

                    var faa = Process.GetCurrentProcess().Threads;

                    ski2_tricks.Sort(delegate (Trick p1, Trick p2){return Convert.ToInt32(p1.points).CompareTo(Convert.ToInt32(p2.points));});

                    var f = sql_CONN.Select($"SELECT id_trick, complete FROM trick_complete WHERE id_user = '{UserSetting.id}' && id_map = '{mapi}'");

                    for (int i = 0; i < ski2_tricks.Count; i++)
                    {
                        bool flag = false;
                        for (int j = 0; j < f.Count / 2; j++)
                        {
                            if (ski2_tricks[i].id == f[j * 2])
                            {
                                ski2_tricks[i].complete = Convert.ToString(f[(j * 2) + 1]);
                                f.RemoveRange(j, 2);
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            sql_CONN.Insert($"INSERT INTO trick_complete SET complete = '0', id_user = '{UserSetting.id}', id_map = '{mapi}', id_trick ='{ski2_tricks[i].id}'");
                            ski2_tricks[i].complete = "0";
                        }
                    }


                    ski2_tricks_consider = new List<TrickConsider>();
                    List<string> tricks_consider = sql_CONN.Select($"SELECT id,date,name,points,permission,url,author,id_user FROM tricks_consider WHERE map = '{mapi}'");
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
                            id_map = "" + mapi,
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

                    ski2_videos_consider = new List<VideoConsider>();
                    List<string> videos_consider = sql_CONN.Select($"SELECT id,date,id_trick,url,author,id_user FROM trick_video_consider WHERE id_map = '{mapi}'");
                    for (int i = 0; i < videos_consider.Count; i += 6)
                    {
                        VideoConsider video = new VideoConsider()
                        {
                            id = videos_consider[i],
                            date = videos_consider[i + 1],
                            id_trick = videos_consider[i + 2],
                            url = videos_consider[i + 3],
                            author = videos_consider[i + 4],
                            id_map = "" + mapi,
                            id_user = videos_consider[i + 5]
                        };

                        ski2_videos_consider.Add(video);
                    }

                    newsfeed_trick = new List<NewsfeedTrick>();
                    List<string> newsfeed_trick_buf = sql_CONN.Select($"SELECT date,id_trick FROM newsfeed_trick WHERE id_map = '{mapi}'");
                    for (int i = 0; i < newsfeed_trick_buf.Count; i += 2)
                    {
                        NewsfeedTrick newTrick = new NewsfeedTrick()
                        {
                            date = newsfeed_trick_buf[i],
                            trick = ski2_tricks.Find(item => item.id == newsfeed_trick_buf[i + 1])
                    };
                        //newTrick.trick = ski2_tricks.Find(item => item.id == newsfeed_trick_buf[i + 1]);

                        newsfeed_trick.Add(newTrick);
                    }

                    minPoints = 0;
                    maxPoints = Convert.ToInt32(ski2_tricks[ski2_tricks.Count - 1].points);
                    trickName = "";
                    triggerName = "";
                    completeStatus = 0;
                });
            }
            

            foreach (var item5 in MainBar.Children)
            {
                if (item5 is StackPanel spMain)
                {
                    foreach (var item in spMain.Children)
                    {
                        if (item is StackPanel sp)
                        {
                            if (sp.Name == "spMap")
                            {
                                foreach (var item1 in sp.Children)
                                {
                                    if (item1 is TextBlock tb)
                                    {
                                        if (tb.Name?.ToString() == "_"+1)
                                        {
                                            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 255, 221));
                                            tb.Text = tb.Text.Substring(0,tb.Text.LastIndexOf(" "));
                                            tb.Tag = 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void TrickPlace()
        {
            List<Trick> sortTricks = new List<Trick>();

            if (trickName != string.Empty)
            {
                for (int i = 0; i < ski2_tricks.Count; i++)
                {
                    string[] arrTrick = ski2_tricks[i].name.ToLower().Split(' ');
                    string[] arrStr = trickName.ToLower().Split(' ');

                    int count = 0;
                    for (int i1 = 0; i1 < arrTrick.Length; i1++)
                    {
                        for (int j1 = 0; j1 < arrStr.Length; j1++)
                        {
                            if (arrTrick[i1].StartsWith(arrStr[j1]))
                            {
                                ++count;
                            }
                        }
                    }
                    if(count >= arrStr.Length && triggerName != string.Empty)
                    {
                        string[] arrTrigger = triggerName.ToLower().Split(' ');
                        List<string> arrTriggerOg = new List<string>();
                        List<Tuple<string, string>> id_triggers = new List<Tuple<string, string>>();


                        for (int i4 = 0; i4 < ski2_tricks[i].id_triggers.Count; i4++)
                        {
                            bool flag = true;
                            for (int j4 = i4 + 1; j4 < ski2_tricks[i].id_triggers.Count; j4++)
                            {
                                if (ski2_tricks[i].id_triggers[i4].Item1 == ski2_tricks[i].id_triggers[j4].Item1) flag = false;
                            }
                            if (flag) id_triggers.Add(ski2_tricks[i].id_triggers[i4]);
                        }


                        for (int i3 = 0; i3 < id_triggers.Count; i3++)
                        {
                            string[] arrTriggerBuf = id_triggers[i3].Item2.ToLower().Split(' ');
                            for (int j3 = 0; j3 < arrTriggerBuf.Length; j3++)
                            {
                                arrTriggerOg.Add(arrTriggerBuf[j3]);
                            }
                        }

                        for (int i5 = 0; i5 < arrTriggerOg.Count; i5++)
                        {
                            bool flag = true;
                            for (int j5 = i5 + 1; j5 < arrTriggerOg.Count; j5++)
                            {
                                if (arrTriggerOg[i5] == arrTriggerOg[j5]) flag = false;
                            }
                            if (!flag)
                            {
                                arrTriggerOg.RemoveAt(i5);
                                --i5;
                            }
                        }

                        count = 0;
                        for (int i2 = 0; i2 < arrTriggerOg.Count; i2++)
                        {
                            for (int j2 = 0; j2 < arrTrigger.Length; j2++)
                            {
                                if (arrTriggerOg[i2].StartsWith(arrTrigger[j2]))
                                {
                                    ++count;
                                }
                            }
                        }
                        if (count >= arrTrigger.Length) { sortTricks.Add(ski2_tricks[i]); }
                    }
                    else if(count >= arrStr.Length)
                    {
                        sortTricks.Add(ski2_tricks[i]);
                    }
                }
            }
            else if(triggerName != string.Empty)
            {
                string[] trig = triggerName.ToLower().Split(',');
                string[] arrTrigger = triggerName.ToLower().Split(' ');
                List<string> arrTriggerOg = new List<string>();
                List<Tuple<string, string>> id_triggers = new List<Tuple<string, string>>();


                if (trig.Length != 0)
                {
                    for (int i = 0; i < ski2_tricks.Count; i++)
                    {
                        for (int i5 = 0; i5 < ski2_tricks[i].id_triggers.Count; i5++)
                        {
                            id_triggers.Add(Tuple.Create(ski2_tricks[i].id_triggers[i5].Item1, ski2_tricks[i].id_triggers[i5].Item2));
                        }
                        int count = 0;
                        for (int j2 = 0; j2 < trig.Length; j2++)
                        {
                            for (int i2 = 0; i2 < id_triggers.Count; i2++)
                            {
                                if (id_triggers[i2].Item2.ToLower().StartsWith(trig[j2]))
                                {
                                    ++count;
                                    id_triggers.RemoveAt(i2);
                                    break;
                                }
                            }
                        }
                           
                        if (count >= trig.Length) {
                            sortTricks.Add(ski2_tricks[i]); 
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ski2_tricks.Count; i++)
                    {
                        for (int i4 = 0; i4 < ski2_tricks[i].id_triggers.Count; i4++)
                        {
                            bool flag = true;
                            for (int j4 = i4 + 1; j4 < ski2_tricks[i].id_triggers.Count; j4++)
                            {
                                if (ski2_tricks[i].id_triggers[i4].Item1 == ski2_tricks[i].id_triggers[j4].Item1) flag = false;
                            }
                            if (flag) id_triggers.Add(ski2_tricks[i].id_triggers[i4]);
                        }


                        for (int i3 = 0; i3 < id_triggers.Count; i3++)
                        {
                            string[] arrTriggerBuf = id_triggers[i3].Item2.ToLower().Split(' ');
                            for (int j3 = 0; j3 < arrTriggerBuf.Length; j3++)
                            {
                                arrTriggerOg.Add(arrTriggerBuf[j3]);
                            }
                        }

                        for (int i5 = 0; i5 < arrTriggerOg.Count; i5++)
                        {
                            bool flag = true;
                            for (int j5 = i5 + 1; j5 < arrTriggerOg.Count; j5++)
                            {
                                if (arrTriggerOg[i5] == arrTriggerOg[j5]) flag = false;
                            }
                            if (!flag)
                            {
                                arrTriggerOg.RemoveAt(i5);
                                --i5;
                            }
                        }

                        int count = 0;
                        for (int i2 = 0; i2 < arrTriggerOg.Count; i2++)
                        {
                            for (int j2 = 0; j2 < arrTrigger.Length; j2++)
                            {
                                if (arrTriggerOg[i2].StartsWith(arrTrigger[j2]))
                                {
                                    ++count;
                                }
                            }
                        }
                        if (count >= arrTrigger.Length) { sortTricks.Add(ski2_tricks[i]); }
                    }
                }
            }
            else
            {
                sortTricks = ski2_tricks;
            }

            MainBar.Children.Clear();
            ScrollViewer sv = new ScrollViewer();
            sv.Style = Application.Current.FindResource("MyCoolScrollViewerStyle") as Style;
            MainBar.Children.Add(sv);
            StackPanel sp = new StackPanel();
            sv.Content = sp;

            for (int i = 0; i < sortTricks.Count; i++)
            {
                if (!(Convert.ToInt32(sortTricks[i].points) >= minPoints && Convert.ToInt32(sortTricks[i].points) <= maxPoints)) continue;

                Expander exp = new Expander();
                exp.Margin = new Thickness(4, 4, 0, 4);
                exp.Tag = sortTricks[i].id;
                exp.Header = sortTricks[i].points + " | " + sortTricks[i].name;
                exp.Style = Application.Current.FindResource("MyCoolExpanderStyle") as Style;
                exp.MouseEnter += Expander_MouseEnter;
                exp.MouseLeave += Expander_MouseLeave;
                exp.Expanded += Expander_Expanded;

                if (completeStatus == 1 && sortTricks[i].complete == "0") continue;
                if (completeStatus == 2 && sortTricks[i].complete == "1") continue;


                if (videoStatus == 1 && sortTricks[i].video.Count == 0) continue; 
                if (videoStatus == 2 && sortTricks[i].video.Count != 0) continue;

                sp.Children.Add(exp);
                StackPanel sp1 = new StackPanel();
                exp.Content = sp1;


                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                StackPanel spTrick = new StackPanel();

                if (sortTricks[i].id_triggers.Count > 7)
                {
                    ScrollViewer svTrick = new ScrollViewer(); svTrick.Style = Application.Current.FindResource("FavsScrollViewer") as Style;
                    svTrick.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    svTrick.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    sp1.Children.Add(svTrick);
                    svTrick.Content = spTrick;
                }
                else
                {
                    sp1.Children.Add(spTrick);
                }
      
                StackPanel spTb = new StackPanel();
                spTb.Orientation = Orientation.Horizontal;
                spTrick.Children.Add(spTb);

                StackPanel spImg = new StackPanel();
                spImg.Orientation = Orientation.Horizontal;
                spTrick.Children.Add(spImg);

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                for (int j = 0; j < sortTricks[i].id_triggers.Count; j++)
                {
                    TextBlock tb = new TextBlock();
                    tb.TextWrapping = TextWrapping.Wrap;
                    tb.Margin = new Thickness(5, 15, 0, 0);
                    tb.FontSize = 18;
                    tb.Width = 150;
                    tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
                    tb.HorizontalAlignment = HorizontalAlignment.Left;
                    tb.TextAlignment = TextAlignment.Center;
                    tb.Text += (j + 1) + "\n" + sortTricks[i].id_triggers[j].Item2;

                    spTb.Children.Add(tb);

                    Image img0 = new Image();
                    img0.Name = "trickImg";
                    img0.HorizontalAlignment = HorizontalAlignment.Left;
                    img0.VerticalAlignment = VerticalAlignment.Center;
                    img0.Margin = new Thickness(5, 4, 0, 5);
                    img0.Width = 150;
                    img0.Height = 150;
                    img0.Source = SwitchImage(sortTricks[i].id_triggers[j].Item1);

                    spImg.Children.Add(img0);
                }

                Expander expMi = new Expander();
                expMi.Header = "Info";
                expMi.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 200, 225));
                expMi.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(50, 15, 15, 15));
                expMi.FontSize = 15;
                expMi.Style = Application.Current.FindResource("MyCoolExpanderStyle") as Style;
                sp1.Children.Add(expMi);

                StackPanel spMi = new StackPanel();
                expMi.Content = spMi;
                spMi.Orientation = Orientation.Vertical;

                Expander expVid = new Expander();
                expVid.Header = "• Video trick complete";
                expVid.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 200, 200, 225));
                expVid.FontSize = 18;
                expVid.Margin = new Thickness(25, 15, 0, 0);
                expVid.Style = Application.Current.FindResource("MyCoolExpanderStyle") as Style;
                spMi.Children.Add(expVid);

                StackPanel spVid = new StackPanel();
                expVid.Content = spVid;

                //TextBlock tbLink = new TextBlock();
                //tbLink.Margin = new Thickness(25, 15, 0, 0);
                //tbLink.FontSize = 18;
                //tbLink.HorizontalAlignment = HorizontalAlignment.Left;
                //tbLink.TextAlignment = TextAlignment.Center;
                //tbLink.Text = "• Video trick complete";
                //spMi.Children.Add(tbLink);

                if (sortTricks[i].video.Count != 0)
                {
                    for (int i1 = 0; i1 < sortTricks[i].video.Count; i1++)
                    {
                        TextBlock tbLink0 = new TextBlock();
                        tbLink0.Margin = new Thickness(50, 15, 0, 0);
                        tbLink0.FontSize = 18;
                        tbLink0.HorizontalAlignment = HorizontalAlignment.Left;
                        tbLink0.TextAlignment = TextAlignment.Center;
                        tbLink0.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 100, 100, 240));
                        tbLink0.Text = sortTricks[i].video[i1].Item1;
                        tbLink0.Tag = sortTricks[i].video[i1].Item2;
                        tbLink0.MouseDown += OpenLink;
                        spVid.Children.Add(tbLink0);
                    }
                }
                else
                {
                    TextBlock tbLink1 = new TextBlock();
                    tbLink1.Margin = new Thickness(50, 15, 0, 0);
                    tbLink1.FontSize = 18;
                    tbLink1.HorizontalAlignment = HorizontalAlignment.Left;
                    tbLink1.TextAlignment = TextAlignment.Center;
                    tbLink1.Text = "<empty>";
                    tbLink1.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 200, 150, 150));
                    spVid.Children.Add(tbLink1);
                }

                Expander expVi = new Expander();
                expVi.Header = "Suggest a video";
                expVi.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
                expVi.FontSize = 14;
                expVi.Margin = new Thickness(0, 10, 0, 0);
                expVi.Style = Application.Current.FindResource("MyCoolExpanderStyle") as Style;
                spVid.Children.Add(expVi);

                StackPanel spVi = new StackPanel();
                spVi.Orientation = Orientation.Vertical;
                expVi.Content = spVi;

                StackPanel spUrl = new StackPanel();
                spUrl.Orientation = Orientation.Horizontal;
                spVi.Children.Add(spUrl);

                TextBlock tbUrl = new TextBlock();
                tbUrl.Margin = new Thickness(20, 0, 0, 0);
                tbUrl.FontSize = 18;
                tbUrl.HorizontalAlignment = HorizontalAlignment.Left;
                tbUrl.TextAlignment = TextAlignment.Center;
                tbUrl.Text = "Link      ";

                TextBox tBox = new TextBox();
                tBox.HorizontalAlignment = HorizontalAlignment.Left;
                tBox.VerticalAlignment = VerticalAlignment.Center;
                tBox.Margin = new Thickness(1, 0, 0, 0);
                tBox.TextAlignment = TextAlignment.Left;
                tBox.Width = 400;
                tBox.Height = 18;
                tBox.FontSize = 15;
                tBox.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 40, 40, 40));
                tBox.SelectionTextBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
                tBox.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
                tBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
                tBox.TextChanged += TextBoxUrl_TextChanged;

                spUrl.Children.Add(tbUrl);
                spUrl.Children.Add(tBox);


                StackPanel spAuthor = new StackPanel();
                spAuthor.Orientation = Orientation.Horizontal;
                spVi.Children.Add(spAuthor);

                TextBlock tbAuthor = new TextBlock();
                tbAuthor.Margin = new Thickness(20, 0, 0, 0);
                tbAuthor.FontSize = 18;
                tbAuthor.HorizontalAlignment = HorizontalAlignment.Left;
                tbAuthor.TextAlignment = TextAlignment.Center;
                tbAuthor.Text = "Author ";

                tBox = new TextBox();
                tBox.HorizontalAlignment = HorizontalAlignment.Left;
                tBox.VerticalAlignment = VerticalAlignment.Center;
                tBox.Margin = new Thickness(1, 0, 0, 0);
                tBox.TextAlignment = TextAlignment.Left;
                tBox.Width = 400;
                tBox.Height = 18;
                tBox.FontSize = 15;
                tBox.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 40, 40, 40));
                tBox.SelectionTextBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
                tBox.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
                tBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 255, 40, 40));
                tBox.TextChanged += TextBoxAuthor_TextChanged;

                spAuthor.Children.Add(tbAuthor);
                spAuthor.Children.Add(tBox);


                TextBlock tbSend = new TextBlock();
                tbSend.Margin = new Thickness(25, 10, 0, 0);
                tbSend.FontSize = 15;
                tbSend.HorizontalAlignment = HorizontalAlignment.Left;
                tbSend.TextAlignment = TextAlignment.Center;
                tbSend.Tag = sortTricks[i].id;
                tbSend.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 255, 221));
                tbSend.Text = "Send";
                tbSend.MouseDown += TextBoxSend_MouseDown;
                spVi.Children.Add(tbSend);

                StackPanel spComplete = new StackPanel();
                spComplete.Orientation = Orientation.Horizontal;
                spMi.Children.Add(spComplete);

                TextBlock tbLink = new TextBlock();
                tbLink.Margin = new Thickness(25, 15, 0, 0);
                tbLink.FontSize = 18;
                tbLink.HorizontalAlignment = HorizontalAlignment.Left;
                tbLink.TextAlignment = TextAlignment.Center;
                tbLink.Text = "• Complete Status  - ";

                BitmapImage bi1 = new BitmapImage();
                bi1.BeginInit();
                if (sortTricks[i].complete == "1") bi1.UriSource = new Uri("/Res/da.png", UriKind.Relative);
                else bi1.UriSource = new Uri("/Res/x.png", UriKind.Relative);
                bi1.EndInit();

                Image img = new Image();
                img.HorizontalAlignment = HorizontalAlignment.Left;
                img.VerticalAlignment = VerticalAlignment.Bottom;
                img.Margin = new Thickness(5, 0, 5, 0);
                img.Width = 25;
                img.Height = 25;
                img.MouseDown += Complete_MouseDown;
                img.Source = bi1;
                img.Name = "id_" + sortTricks[i].id;
                img.Tag = sortTricks[i].complete;

                spComplete.Children.Add(tbLink);
                spComplete.Children.Add(img);
            }
        }

        #endregion

        #region Event

        private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void MinButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void EndButton_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void VideoStatus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            if (Convert.ToInt32(img.Tag) == 0)
            {
                img.Tag = 1;

                BitmapImage bi2 = new BitmapImage();
                bi2.BeginInit();
                bi2.UriSource = new Uri("/Res/sYe.png", UriKind.Relative);
                bi2.EndInit();

                img.Source = bi2;

                videoStatus = 1;
            }
            else if (Convert.ToInt32(img.Tag) == 1)
            {
                img.Tag = 2;

                BitmapImage bi2 = new BitmapImage();
                bi2.BeginInit();
                bi2.UriSource = new Uri("/Res/sNo.png", UriKind.Relative);
                bi2.EndInit();

                img.Source = bi2;

                videoStatus = 2;
            }
            else
            {
                img.Tag = 0;

                BitmapImage bi2 = new BitmapImage();
                bi2.BeginInit();
                bi2.UriSource = new Uri("/Res/sEmpty.png", UriKind.Relative);
                bi2.EndInit();

                img.Source = bi2;

                videoStatus = 0;
            }
            TrickPlace();
        }

        private void Complete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;

            int i = 0;
            for (; i < ski2_tricks.Count; i++)
            {
                if (ski2_tricks[i].id == img.Name.Substring(3))
                {
                    break;
                }
            }

            if (Convert.ToInt32(ski2_tricks[i].points) >= 800) return;
            
            if (Convert.ToInt32(img.Tag) == 0)
            {
                img.Tag = 1;

                ski2_tricks[i].complete = "1";

                BitmapImage bi2 = new BitmapImage();
                bi2.BeginInit();
                bi2.UriSource = new Uri("/Res/da.png", UriKind.Relative);
                bi2.EndInit();

                img.Source = bi2;

                sql_CONN.Update($"UPDATE trick_complete SET complete = 1 WHERE id_user = '{UserSetting.id}', id_trick = '{img.Name.Substring(3)}', id_map = '{mapId}'");
            }
            else
            {
                img.Tag = 0;

                ski2_tricks[i].complete = "0";

                BitmapImage bi2 = new BitmapImage();
                bi2.BeginInit();
                bi2.UriSource = new Uri("/Res/x.png", UriKind.Relative);
                bi2.EndInit();

                img.Source = bi2;
                string str = img.Name.Substring(3);
                sql_CONN.Update($"UPDATE trick_complete SET complete = 0 WHERE id_user = '{UserSetting.id}', id_trick = '{img.Name.Substring(3)}', id_map = '{mapId}'");
            }
            TrickPlace();
        }

        private void CompleteStatus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            if (Convert.ToInt32(img.Tag) == 0)
            {
                img.Tag = 1;

                BitmapImage bi2 = new BitmapImage();
                bi2.BeginInit();
                bi2.UriSource = new Uri("/Res/sYe.png", UriKind.Relative);
                bi2.EndInit();

                img.Source = bi2;

                completeStatus = 1;
            }
            else if (Convert.ToInt32(img.Tag) == 1)
            {
                img.Tag = 2;

                BitmapImage bi2 = new BitmapImage();
                bi2.BeginInit();
                bi2.UriSource = new Uri("/Res/sNo.png", UriKind.Relative);
                bi2.EndInit();

                img.Source = bi2;

                completeStatus = 2;
            }
            else
            {
                img.Tag = 0;

                BitmapImage bi2 = new BitmapImage();
                bi2.BeginInit();
                bi2.UriSource = new Uri("/Res/sEmpty.png", UriKind.Relative);
                bi2.EndInit();

                img.Source = bi2;

                completeStatus = 0;
            }
            TrickPlace();
        }

        private void TextBlockNamePoint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            trickName = string.Empty;
            triggerName = string.Empty;
            TrickWindowLoad();
            TrickPlace();
        }

        private void TrickList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            if (textBlock.Tag.ToString() == "1")
            {
                mapId = Convert.ToInt32(textBlock.Name.ToString().Substring(1));
                TrickWindowLoad();
            }
        }

        private void OpenLink(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock tb)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(tb.Tag.ToString()) { UseShellExecute = true });

                }
                catch (Exception)
                {

                }
            }
        }

        private void OpenTrick(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            trickName = ski2_tricks.Find(item => item.id == textBlock.Tag.ToString()).name;
            triggerName = string.Empty;
            TrickWindowLoad();
            TrickPlace();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void TextBoxTrickName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = (TextBox)sender;
                trickName = textBox.Text;
                TrickPlace();
            }
        }

        private void TextBoxTrickNameLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            trickName = textBox.Text;
            TrickPlace();
        }

        private void TextBoxTriggerNameLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            triggerName = textBox.Text;
            TrickPlace();
        }

        private void TextBoxTriggerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = (TextBox)sender;
                triggerName = textBox.Text;
                TrickPlace();
            }
        }

        private void TextBoxMinPoints_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = (TextBox)sender;
                if(textBox.Text != string.Empty)
                {
                    minPoints = Convert.ToInt32(textBox.Text);
                    TrickPlace();
                }
            }
        }

        private void TextBoxMaxPoints_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = (TextBox)sender;
                if (textBox.Text != string.Empty)
                {
                    maxPoints = Convert.ToInt32(textBox.Text);
                    TrickPlace();
                }
            }
        }

        private void TextBoxMinLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text != string.Empty)
            {
                try
                {
                    minPoints = Convert.ToInt32(textBox.Text);
                    TrickPlace();
                }
                catch (Exception)
                {

                }
          
            }
        }

        private void TextBoxMaxLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text != string.Empty)
            {
                maxPoints = Convert.ToInt32(textBox.Text);
                TrickPlace();
            }
        }

        private void TrickAdd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            trickName = string.Empty;
            triggerName = string.Empty;
            AddTrickFormLoad();

            //if ((int)textBlock.Tag == 0)
            //{
            //    textBlock.Tag = 1;
            //    AddTrickFormLoad();
            //}
            //else
            //{
            //    textBlock.Tag = 0;
            //    TrickWindowLoad();
            //}
        }

        private void Expander_MouseEnter(object sender, MouseEventArgs e)
        {
            Expander exp = (Expander)sender;
            exp.Margin = new Thickness(11, 4, 0, 4);
        }

        private void Expander_MouseLeave(object sender, MouseEventArgs e)
        {
            Expander exp = (Expander)sender;
            exp.Margin = new Thickness(4, 4, 0, 4);
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            SuggestVideoLink = string.Empty;
            SuggestVideoAuthor = string.Empty;

            foreach (var item in MainBar.Children)
            {
                if (item is ScrollViewer sv)
                {
                    StackPanel sp = (StackPanel)sv.Content;
                    foreach (var item1 in sp.Children)
                    {
                        if (item1 is Expander exp)
                        {
                            if (exp != sender)
                            {
                                exp.IsExpanded = false;
                            }
                        }
                    }
                }
            }
        }

        private void TextBoxUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            SuggestVideoLink = tb.Text;
        }

        private void TextBoxAuthor_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            SuggestVideoAuthor = tb.Text;
        }

        private void TextBoxSend_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;

            if(SuggestVideoAuthor !=string.Empty && SuggestVideoLink != string.Empty)
            {
                sql_CONN.Insert($"INSERT INTO trick_video_consider SET id_trick = '{textBlock.Tag.ToString()}', url = '{SuggestVideoLink}', author = '{SuggestVideoAuthor}', id_user='{UserSetting.id}', id_map='{mapId}'");
                TrickPlace();
            }
        }

        #endregion

        #region SwitchImage

        public static BitmapImage SwitchImage(string id)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            
            switch (id)
            {
                case "2004": image.UriSource = new Uri("/Triggers/2004.png", UriKind.Relative); break;
                case "2005": image.UriSource = new Uri("/Triggers/2005.png", UriKind.Relative); break;
                case "2006": image.UriSource = new Uri("/Triggers/2006.png", UriKind.Relative); break;
                case "2007": image.UriSource = new Uri("/Triggers/2007.png", UriKind.Relative); break;
                case "2008": image.UriSource = new Uri("/Triggers/2008.png", UriKind.Relative); break;
                case "2009": image.UriSource = new Uri("/Triggers/2009.png", UriKind.Relative); break;
                case "2016": image.UriSource = new Uri("/Triggers/2016.png", UriKind.Relative); break;
                case "2033": image.UriSource = new Uri("/Triggers/2033.png", UriKind.Relative); break;
                case "2036": image.UriSource = new Uri("/Triggers/2036.png", UriKind.Relative); break;
                case "2037": image.UriSource = new Uri("/Triggers/2037.png", UriKind.Relative); break;
                case "2038": image.UriSource = new Uri("/Triggers/2038.png", UriKind.Relative); break;
                case "2039": image.UriSource = new Uri("/Triggers/2039.png", UriKind.Relative); break;
                case "2041": image.UriSource = new Uri("/Triggers/2041.png", UriKind.Relative); break;
                case "2042": image.UriSource = new Uri("/Triggers/2042.png", UriKind.Relative); break;
                case "2043": image.UriSource = new Uri("/Triggers/2043.png", UriKind.Relative); break;
                case "2044": image.UriSource = new Uri("/Triggers/2044.png", UriKind.Relative); break;
                case "2045": image.UriSource = new Uri("/Triggers/2045.png", UriKind.Relative); break;
                case "2047": image.UriSource = new Uri("/Triggers/2047.png", UriKind.Relative); break;
                case "2052": image.UriSource = new Uri("/Triggers/2052.png", UriKind.Relative); break;
                case "2053": image.UriSource = new Uri("/Triggers/2053.png", UriKind.Relative); break;
                case "2055": image.UriSource = new Uri("/Triggers/2055.png", UriKind.Relative); break;
                case "2056": image.UriSource = new Uri("/Triggers/2056.png", UriKind.Relative); break;
                case "2057": image.UriSource = new Uri("/Triggers/2057.png", UriKind.Relative); break;
                case "2059": image.UriSource = new Uri("/Triggers/2059.png", UriKind.Relative); break;
                case "2060": image.UriSource = new Uri("/Triggers/2060.png", UriKind.Relative); break;
                case "2061": image.UriSource = new Uri("/Triggers/2061.png", UriKind.Relative); break;
                case "2062": image.UriSource = new Uri("/Triggers/2062.png", UriKind.Relative); break;
                case "2063": image.UriSource = new Uri("/Triggers/2063.png", UriKind.Relative); break;
                case "2064": image.UriSource = new Uri("/Triggers/2064.png", UriKind.Relative); break;
                case "2065": image.UriSource = new Uri("/Triggers/2065.png", UriKind.Relative); break;
                case "2066": image.UriSource = new Uri("/Triggers/2066.png", UriKind.Relative); break;
                case "2067": image.UriSource = new Uri("/Triggers/2067.png", UriKind.Relative); break;
                case "2071": image.UriSource = new Uri("/Triggers/2071.png", UriKind.Relative); break;
                case "2072": image.UriSource = new Uri("/Triggers/2072.png", UriKind.Relative); break;
                case "2073": image.UriSource = new Uri("/Triggers/2073.png", UriKind.Relative); break;
                case "2074": image.UriSource = new Uri("/Triggers/2074.png", UriKind.Relative); break;
                case "2075": image.UriSource = new Uri("/Triggers/2075.png", UriKind.Relative); break;
                case "2076": image.UriSource = new Uri("/Triggers/2076.png", UriKind.Relative); break;
                case "2077": image.UriSource = new Uri("/Triggers/2077.png", UriKind.Relative); break;
                case "2078": image.UriSource = new Uri("/Triggers/2078.png", UriKind.Relative); break;
                case "2079": image.UriSource = new Uri("/Triggers/2079.png", UriKind.Relative); break;
                case "2080": image.UriSource = new Uri("/Triggers/2080.png", UriKind.Relative); break;
                case "2082": image.UriSource = new Uri("/Triggers/2082.png", UriKind.Relative); break;
                case "2087": image.UriSource = new Uri("/Triggers/2087.png", UriKind.Relative); break;
                case "2088": image.UriSource = new Uri("/Triggers/2088.png", UriKind.Relative); break;
                case "2089": image.UriSource = new Uri("/Triggers/2089.png", UriKind.Relative); break;
                case "2091": image.UriSource = new Uri("/Triggers/2091.png", UriKind.Relative); break;
                case "2093": image.UriSource = new Uri("/Triggers/2093.png", UriKind.Relative); break;
                case "2094": image.UriSource = new Uri("/Triggers/2094.png", UriKind.Relative); break;
                case "2095": image.UriSource = new Uri("/Triggers/2095.png", UriKind.Relative); break;
                case "2097": image.UriSource = new Uri("/Triggers/2097.png", UriKind.Relative); break;
                case "2098": image.UriSource = new Uri("/Triggers/2098.png", UriKind.Relative); break;
                case "2099": image.UriSource = new Uri("/Triggers/2099.png", UriKind.Relative); break;
                case "2100": image.UriSource = new Uri("/Triggers/2100.png", UriKind.Relative); break;
                case "2101": image.UriSource = new Uri("/Triggers/2101.png", UriKind.Relative); break;
                case "2102": image.UriSource = new Uri("/Triggers/2102.png", UriKind.Relative); break;
                case "2103": image.UriSource = new Uri("/Triggers/2103.png", UriKind.Relative); break;
                case "2104": image.UriSource = new Uri("/Triggers/2104.png", UriKind.Relative); break;
                case "2105": image.UriSource = new Uri("/Triggers/2105.png", UriKind.Relative); break;
                case "2106": image.UriSource = new Uri("/Triggers/2106.png", UriKind.Relative); break;
                case "2107": image.UriSource = new Uri("/Triggers/2107.png", UriKind.Relative); break;
                case "2109": image.UriSource = new Uri("/Triggers/2109.png", UriKind.Relative); break;
                case "2111": image.UriSource = new Uri("/Triggers/2111.png", UriKind.Relative); break;
                case "2112": image.UriSource = new Uri("/Triggers/2112.png", UriKind.Relative); break;
                case "2113": image.UriSource = new Uri("/Triggers/2113.png", UriKind.Relative); break;
                case "2114": image.UriSource = new Uri("/Triggers/2114.png", UriKind.Relative); break;
                case "2115": image.UriSource = new Uri("/Triggers/2115.png", UriKind.Relative); break;
                case "2116": image.UriSource = new Uri("/Triggers/2116.png", UriKind.Relative); break;
                case "2117": image.UriSource = new Uri("/Triggers/2117.png", UriKind.Relative); break;
                case "2118": image.UriSource = new Uri("/Triggers/2118.png", UriKind.Relative); break;
                case "2120": image.UriSource = new Uri("/Triggers/2120.png", UriKind.Relative); break;
                case "2122": image.UriSource = new Uri("/Triggers/2122.png", UriKind.Relative); break;
                case "2124": image.UriSource = new Uri("/Triggers/2124.png", UriKind.Relative); break;
                case "2126": image.UriSource = new Uri("/Triggers/2126.png", UriKind.Relative); break;
                case "2127": image.UriSource = new Uri("/Triggers/2127.png", UriKind.Relative); break;
                case "2128": image.UriSource = new Uri("/Triggers/2128.png", UriKind.Relative); break;
                case "2129": image.UriSource = new Uri("/Triggers/2129.png", UriKind.Relative); break;
                case "2130": image.UriSource = new Uri("/Triggers/2130.png", UriKind.Relative); break;
                case "2131": image.UriSource = new Uri("/Triggers/2131.png", UriKind.Relative); break;
                case "2132": image.UriSource = new Uri("/Triggers/2132.png", UriKind.Relative); break;
                case "2133": image.UriSource = new Uri("/Triggers/2133.png", UriKind.Relative); break;
                case "2140": image.UriSource = new Uri("/Triggers/2140.png", UriKind.Relative); break;
                case "2141": image.UriSource = new Uri("/Triggers/2141.png", UriKind.Relative); break;
                case "2142": image.UriSource = new Uri("/Triggers/2142.png", UriKind.Relative); break;
                case "2143": image.UriSource = new Uri("/Triggers/2143.png", UriKind.Relative); break;
                case "2144": image.UriSource = new Uri("/Triggers/2144.png", UriKind.Relative); break;
                case "2145": image.UriSource = new Uri("/Triggers/2145.png", UriKind.Relative); break;
                case "2146": image.UriSource = new Uri("/Triggers/2146.png", UriKind.Relative); break;
                case "2147": image.UriSource = new Uri("/Triggers/2147.png", UriKind.Relative); break;
                case "2150": image.UriSource = new Uri("/Triggers/2150.png", UriKind.Relative); break;
                case "2151": image.UriSource = new Uri("/Triggers/2151.png", UriKind.Relative); break;
                case "2155": image.UriSource = new Uri("/Triggers/2155.png", UriKind.Relative); break;
                case "2156": image.UriSource = new Uri("/Triggers/2156.png", UriKind.Relative); break;
                case "2157": image.UriSource = new Uri("/Triggers/2157.png", UriKind.Relative); break;
                case "2158": image.UriSource = new Uri("/Triggers/2158.png", UriKind.Relative); break;
                case "2159": image.UriSource = new Uri("/Triggers/2159.png", UriKind.Relative); break;
                case "2160": image.UriSource = new Uri("/Triggers/2160.png", UriKind.Relative); break;
                case "2161": image.UriSource = new Uri("/Triggers/2161.png", UriKind.Relative); break;
                case "2162": image.UriSource = new Uri("/Triggers/2162.png", UriKind.Relative); break;
                case "2163": image.UriSource = new Uri("/Triggers/2163.png", UriKind.Relative); break;
                case "2164": image.UriSource = new Uri("/Triggers/2164.png", UriKind.Relative); break;
                case "2165": image.UriSource = new Uri("/Triggers/2165.png", UriKind.Relative); break;
                case "2166": image.UriSource = new Uri("/Triggers/2166.png", UriKind.Relative); break;
                case "2167": image.UriSource = new Uri("/Triggers/2167.png", UriKind.Relative); break;
                case "2170": image.UriSource = new Uri("/Triggers/2170.png", UriKind.Relative); break;
                case "2172": image.UriSource = new Uri("/Triggers/2172.png", UriKind.Relative); break;
                case "2173": image.UriSource = new Uri("/Triggers/2173.png", UriKind.Relative); break;
                case "2174": image.UriSource = new Uri("/Triggers/2174.png", UriKind.Relative); break;
                case "2175": image.UriSource = new Uri("/Triggers/2175.png", UriKind.Relative); break;
                case "2176": image.UriSource = new Uri("/Triggers/2176.png", UriKind.Relative); break;
                case "2178": image.UriSource = new Uri("/Triggers/2178.png", UriKind.Relative); break;
                case "2179": image.UriSource = new Uri("/Triggers/2179.png", UriKind.Relative); break;
                case "2180": image.UriSource = new Uri("/Triggers/2180.png", UriKind.Relative); break;
                case "2181": image.UriSource = new Uri("/Triggers/2181.png", UriKind.Relative); break;
                case "2182": image.UriSource = new Uri("/Triggers/2182.png", UriKind.Relative); break;
                case "2185": image.UriSource = new Uri("/Triggers/2185.png", UriKind.Relative); break;
                case "2203": image.UriSource = new Uri("/Triggers/2203.png", UriKind.Relative); break;
                case "2221": image.UriSource = new Uri("/Triggers/2221.png", UriKind.Relative); break;
                case "2226": image.UriSource = new Uri("/Triggers/2226.png", UriKind.Relative); break;
                case "2230": image.UriSource = new Uri("/Triggers/2230.png", UriKind.Relative); break;
                case "2232": image.UriSource = new Uri("/Triggers/2232.png", UriKind.Relative); break;
                case "2233": image.UriSource = new Uri("/Triggers/2233.png", UriKind.Relative); break;
                case "2235": image.UriSource = new Uri("/Triggers/2235.png", UriKind.Relative); break;
                case "2240": image.UriSource = new Uri("/Triggers/2240.png", UriKind.Relative); break;
                case "2243": image.UriSource = new Uri("/Triggers/2243.png", UriKind.Relative); break;
                case "2244": image.UriSource = new Uri("/Triggers/2244.png", UriKind.Relative); break;
                case "2246": image.UriSource = new Uri("/Triggers/2246.png", UriKind.Relative); break;
                case "2248": image.UriSource = new Uri("/Triggers/2248.png", UriKind.Relative); break;
                case "2249": image.UriSource = new Uri("/Triggers/2249.png", UriKind.Relative); break;
                case "2250": image.UriSource = new Uri("/Triggers/2250.png", UriKind.Relative); break;
                case "2251": image.UriSource = new Uri("/Triggers/2251.png", UriKind.Relative); break;
                case "2252": image.UriSource = new Uri("/Triggers/2252.png", UriKind.Relative); break;
                case "2253": image.UriSource = new Uri("/Triggers/2253.png", UriKind.Relative); break;
                case "2254": image.UriSource = new Uri("/Triggers/2254.png", UriKind.Relative); break;
                case "2255": image.UriSource = new Uri("/Triggers/2255.png", UriKind.Relative); break;
                case "2256": image.UriSource = new Uri("/Triggers/2256.png", UriKind.Relative); break;
                case "2257": image.UriSource = new Uri("/Triggers/2257.png", UriKind.Relative); break;
                case "2258": image.UriSource = new Uri("/Triggers/2258.png", UriKind.Relative); break;
                case "2260": image.UriSource = new Uri("/Triggers/2260.png", UriKind.Relative); break;
                case "2261": image.UriSource = new Uri("/Triggers/2261.png", UriKind.Relative); break;
                case "2263": image.UriSource = new Uri("/Triggers/2263.png", UriKind.Relative); break;
                case "2264": image.UriSource = new Uri("/Triggers/2264.png", UriKind.Relative); break;
                case "2265": image.UriSource = new Uri("/Triggers/2265.png", UriKind.Relative); break;
                case "2267": image.UriSource = new Uri("/Triggers/2267.png", UriKind.Relative); break;
                case "2269": image.UriSource = new Uri("/Triggers/2269.png", UriKind.Relative); break;
                case "2270": image.UriSource = new Uri("/Triggers/2270.png", UriKind.Relative); break;
                case "2271": image.UriSource = new Uri("/Triggers/2271.png", UriKind.Relative); break;
                case "2275": image.UriSource = new Uri("/Triggers/2275.png", UriKind.Relative); break;
                case "2307": image.UriSource = new Uri("/Triggers/2307.png", UriKind.Relative); break;
                case "2309": image.UriSource = new Uri("/Triggers/2309.png", UriKind.Relative); break;
                case "2316": image.UriSource = new Uri("/Triggers/2316.png", UriKind.Relative); break;
                case "2326": image.UriSource = new Uri("/Triggers/2326.png", UriKind.Relative); break;
                case "2339": image.UriSource = new Uri("/Triggers/2339.png", UriKind.Relative); break;
                case "2348": image.UriSource = new Uri("/Triggers/2348.png", UriKind.Relative); break;
                case "2353": image.UriSource = new Uri("/Triggers/2353.png", UriKind.Relative); break;
                case "2354": image.UriSource = new Uri("/Triggers/2354.png", UriKind.Relative); break;
                case "2355": image.UriSource = new Uri("/Triggers/2355.png", UriKind.Relative); break;
                case "2363": image.UriSource = new Uri("/Triggers/2363.png", UriKind.Relative); break;
                case "2210": image.UriSource = new Uri("/Triggers/2210.png", UriKind.Relative); break;
                case "2040": image.UriSource = new Uri("/Triggers/2243.png", UriKind.Relative); break;
                case "2081": image.UriSource = new Uri("/Triggers/2081.png", UriKind.Relative); break;

                default: image.UriSource = new Uri("/Triggers/smthwrong.png", UriKind.Relative); break;
            }

            image.EndInit();
            return image;
        }


        #endregion

        #region Authorization

        private void Auh()
        {
            MainBar.Children.Clear();

            string json = File.ReadAllText("userconfig.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            if (jsonObj["login"] != string.Empty)
            {
                if (sql_CONN.Select($"SELECT count(login) FROM surf_users WHERE login ='{jsonObj["login"]}'")[0] != "0")
                {
                    if (sql_CONN.Select($"SELECT password FROM surf_users WHERE login ='{jsonObj["login"]}'")[0] == Convert.ToString(jsonObj["password"]))
                    {
                        var lstr = sql_CONN.Select($"SELECT id, login FROM surf_users WHERE login ='{jsonObj["login"]}'");
                        var admin = sql_CONN.Select($"SELECT COUNT(id) FROM admins WHERE id_user = {lstr[0]}");

                        UserSetting.log = true;

                        UserSetting.id = lstr[0];
                        UserSetting.name = lstr[1];

                        if (admin[0] != "0") UserSetting.iAdmin = true;

                        MainWindowLoad();
                        TrickListLoad();
                        return;
                    }
                }
            }

            StackPanel sp = new StackPanel();
            sp.Margin = new Thickness(500, 0, 0, 0);
            MainBar.Children.Add(sp);

            TextBlock tb = new TextBlock();
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.Name = $"log_in";
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 30F;
            tb.FontWeight = FontWeights.SemiBold;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Width = 450;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(0, 225, 0, 5);
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tb.Text = "Log in";
            tb.MouseDown += SigninUserMouseDown;

            sp.Children.Add(tb);

            tb = new TextBlock();
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.Name = $"sign_up";
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 30F;
            tb.FontWeight = FontWeights.SemiBold;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Width = 450;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(0, 10, 0, 5);
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tb.Text = "Sign Up";
            tb.MouseDown += SignupUserMouseDown;

            sp.Children.Add(tb);
        }

        private void UserMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainBar.Children.Clear();

            if (UserSetting.log)
            {
                InfoUser();
            }
            else
            {
                Auh();
            }
        }

        private void InfoUser()
        {
            StackPanel sp = new StackPanel();
            sp.Margin = new Thickness(450, 0, 0, 0);
            MainBar.Children.Add(sp);

            TextBlock tb = new TextBlock();
            tb.Name = $"login_tb";
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 25F;
            tb.FontWeight = FontWeights.SemiBold;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Width = 450;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(0, 5, 0, 5);
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tb.Text = $"Login: {UserSetting.name}";
            sp.Children.Add(tb);

            tb = new TextBlock();
            tb.Name = $"editset_tb";
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 15F;
            tb.FontWeight = FontWeights.SemiBold;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Width = 450;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(0, 15, 0, 5);
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 150, 150, 150));
            tb.Text = "Edit settings";
            sp.Children.Add(tb);

            tb = new TextBlock();
            tb.Name = $"changeacc_tb";
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 15F;
            tb.FontWeight = FontWeights.SemiBold;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Width = 450;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(0, 15, 0, 5);
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 150, 150, 150));
            tb.Text = "Change account";
            tb.MouseDown += ChangeAccount;
            sp.Children.Add(tb);


        }

        private void ChangeAccount(object sender, MouseButtonEventArgs e)
        {
            string json = File.ReadAllText("userconfig.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj["login"] = string.Empty;
            jsonObj["password"] = string.Empty;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("userconfig.json", output);

            UserSetting.log = false;
            UserSetting.name = string.Empty;
            UserSetting.id = string.Empty;
            UserSetting.iAdmin = false;

            Auh();
        }

        #endregion

        #region Sign In

        private void SigninUserMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainBar.Children.Clear();
            SigninUser();
        }

        private void SigninUser()
        {
            StackPanel sp = new StackPanel();
            sp.Margin = new Thickness(450, 0, 0, 0);
            MainBar.Children.Add(sp);

            TextBlock tb = new TextBlock();
            tb.Name = $"login_tb";
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 25F;
            tb.FontWeight = FontWeights.SemiBold;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Width = 450;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(0, 125, 0, 5);
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tb.Text = "Login";
            sp.Children.Add(tb);

            TextBox tbox = new TextBox();
            tbox.Name = $"login_tbox";
            tbox.HorizontalAlignment = HorizontalAlignment.Left;
            tbox.VerticalAlignment = VerticalAlignment.Center;
            tbox.Margin = new Thickness(0, 5, 0, 0);
            tbox.Width = 250;
            tbox.Height = 25;
            tbox.MaxLength = 10;
            tbox.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
            tbox.FontSize = 15;
            sp.Children.Add(tbox);

            tb = new TextBlock();
            tb.Name = $"password_tb";
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 25F;
            tb.FontWeight = FontWeights.SemiBold;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Width = 450;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(0, 15, 0, 5);
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tb.Text = "Password";
            sp.Children.Add(tb);

            PasswordBox pb = new PasswordBox();
            pb.Name = $"password_tbox";
            pb.HorizontalAlignment = HorizontalAlignment.Left;
            pb.VerticalAlignment = VerticalAlignment.Center;
            pb.Margin = new Thickness(0, 5, 0, 0);
            pb.Width = 250;
            pb.Height = 25;
            pb.MaxLength = 10;
            pb.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
            pb.FontSize = 15;
            sp.Children.Add(pb);


            tb = new TextBlock();
            tb.Name = $"sign_in_tb";
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 25F;
            tb.FontWeight = FontWeights.SemiBold;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Width = 450;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(0, 25, 0, 5);
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tb.Text = "Sign in";
            tb.MouseDown += SignInCheck;
            sp.Children.Add(tb);

            tb = new TextBlock();
            tb.Name = $"another_tb";
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 15F;
            tb.FontWeight = FontWeights.SemiBold;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Width = 450;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(0, 25, 0, 5);
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tb.Text = "I dont have account";
            tb.MouseDown += SignupUserMouseDown;
            sp.Children.Add(tb);
        }

        private void SignInCheck(object sender, MouseButtonEventArgs e)
        {
            bool a = true;
            string strLogin = string.Empty;

            foreach (object i in MainBar.Children)
            {
                if (i is StackPanel sp)
                {
                    foreach (object g in sp.Children)
                    {
                        if (g is TextBox tb)
                        {
                            if (tb.Name == "login_tbox" && tb.Text != string.Empty)
                            {
                                var f = sql_CONN.Select($"SELECT count(login) FROM surf_users WHERE login = '{tb.Text}'")[0];
                                if (tb.Text == string.Empty || tb.Text.Length < 5 || tb.Text.Length > 15 || sql_CONN.Select($"SELECT count(login) FROM surf_users WHERE login = '{tb.Text}'")[0] == "0")
                                {
                                    tb.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                                    a = false;
                                }
                                else
                                {
                                    strLogin = tb.Text;
                                    tb.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
                                }
                            }
                        }
                        else if (g is PasswordBox pb)
                        {
                            if (pb.Name == "password_tbox")
                            {
                                if (pb.Password == string.Empty || pb.Password.Length < 5 || pb.Password.Length > 15) pb.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                                else if (!a)
                                {
                                    pb.Password = string.Empty;
                                    pb.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
                                }
                                else
                                {
                                    if (sql_CONN.Select($"SELECT password FROM surf_users WHERE login = '{strLogin}'")[0] == pb.Password)
                                    {
                                        var lstr = sql_CONN.Select($"SELECT id, login FROM surf_users WHERE login ='{strLogin}'");

                                        UserSetting.log = true;

                                        UserSetting.id = lstr[0];
                                        UserSetting.name = lstr[1];

                                        string json = File.ReadAllText("userconfig.json");
                                        dynamic jsonObj = JsonConvert.DeserializeObject(json);
                                        jsonObj["login"] = strLogin;
                                        jsonObj["password"] = pb.Password;
                                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                                        File.WriteAllText("userconfig.json", output);

                                        Auh();
                                        return;
                                    }
                                    else
                                    {
                                        pb.Password = "";
                                        pb.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Sign Up

        private void SignupUserMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainBar.Children.Clear();
            SignUpUser();
        }

        private void SignUpUser()
        {
            StackPanel sp = new StackPanel();
            sp.Margin = new Thickness(450, 0, 0, 0);
            MainBar.Children.Add(sp);

            TextBlock tb = new TextBlock();
            tb.Name = $"login_tb";
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 25F;
            tb.FontWeight = FontWeights.SemiBold;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Width = 450;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(0, 125, 0, 5);
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tb.Text = "Login";
            sp.Children.Add(tb);

            TextBox tbox = new TextBox();
            tbox.Name = $"login_tbox";
            tbox.HorizontalAlignment = HorizontalAlignment.Left;
            tbox.VerticalAlignment = VerticalAlignment.Center;
            tbox.Margin = new Thickness(0, 5, 0, 0);
            tbox.Width = 250;
            tbox.Height = 25;
            tbox.MaxLength = 10;
            tbox.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
            tbox.FontSize = 15;
            sp.Children.Add(tbox);

            tb = new TextBlock();
            tb.Name = $"password_tb";
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 25F;
            tb.FontWeight = FontWeights.SemiBold;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Width = 450;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(0, 15, 0, 5);
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tb.Text = "Password";
            sp.Children.Add(tb);

            PasswordBox pb = new PasswordBox();
            pb.Name = $"password_tbox";
            pb.HorizontalAlignment = HorizontalAlignment.Left;
            pb.VerticalAlignment = VerticalAlignment.Center;
            pb.Margin = new Thickness(0, 5, 0, 0);
            pb.Width = 250;
            pb.Height = 25;
            pb.MaxLength = 10;
            pb.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
            pb.FontSize = 15;
            sp.Children.Add(pb);

            tb = new TextBlock();
            tb.Name = $"sign_up_tb";
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 25F;
            tb.FontWeight = FontWeights.SemiBold;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Width = 450;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(0, 25, 0, 5);
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tb.Text = "Sign up";
            tb.MouseDown += SignUpCheck;
            sp.Children.Add(tb);

            tb = new TextBlock();
            tb.Name = $"another_tb";
            tb.FontFamily = new FontFamily("Century Gothic");
            tb.FontStyle = FontStyles.Italic;
            tb.FontSize = 15F;
            tb.FontWeight = FontWeights.SemiBold;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Width = 450;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Margin = new Thickness(0, 25, 0, 5);
            tb.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 221, 221, 221));
            tb.Text = "I have an account";
            tb.MouseDown += SigninUserMouseDown;
            sp.Children.Add(tb);
        }

        private void SignUpCheck(object sender, MouseButtonEventArgs e)
        {
            string strLogin = string.Empty;
            string strPassword = string.Empty;

            foreach (object i in MainBar.Children)
            {
                if (i is StackPanel sp)
                {
                    foreach (var item in sp.Children)
                    {
                        if (item is TextBox tb)
                        {
                            if (tb.Name == "login_tbox" && tb.Text != string.Empty)
                            {
                                if (tb.Text == string.Empty || tb.Text.Length < 5 || tb.Text.Length > 15) tb.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                                else if (sql_CONN.Select($"SELECT login FROM surf_users WHERE login = '{tb.Text}'").Count != 0) tb.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                                else
                                {
                                    strLogin = tb.Text;
                                    tb.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
                                }
                            }
                        }
                        else if (item is PasswordBox pb)
                        {
                            if (pb.Name == "password_tbox")
                            {
                                if (pb.Password == string.Empty || pb.Password.Length < 5 || pb.Password.Length > 15) pb.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0));
                                else
                                {
                                    strPassword = pb.Password;
                                    pb.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 100, 100, 100));
                                }
                            }
                        }

                        if (strLogin != string.Empty && strPassword != string.Empty)
                        {
                            sql_CONN.Insert($"INSERT INTO surf_users SET login = '{strLogin}', password = '{strPassword}'");

                            Auh();
                            return;
                        }
                    }
                }
            }
        }


        #endregion

    }
}
