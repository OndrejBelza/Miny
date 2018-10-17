using FileHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Miny
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<cor> Cordinates = new List<cor>();
        List<cor> FlaggedCordinates = new List<cor>();
        List<PlayGroundButton> BtnList = new List<PlayGroundButton>();
        List<PlayGroundButton> BtnsToCheckList = new List<PlayGroundButton>();
        ObservableCollection<Save> ScoreList = new ObservableCollection<Save>();
        DispatcherTimer timer = new DispatcherTimer();
        private bool firstlick = true;
        private cor BlockedCor = new cor();
        private int reveled = 0;
        private int bombtodiscover;
        private int time = 0;
        class cor
        {
            public int row { get; set; }
            public int col { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            reveled = 0;
            Cordinates.Clear();
            FlaggedCordinates.Clear();
            BtnsToCheckList.Clear();
            BtnList.Clear();
            GenereateGridButtons();
            RegenerateGrid();
            firstlick = true;
            bombtodiscover = int.Parse(BombNum_.Text);
            EditBmbCnt();
            Smile.Source = new BitmapImage(new Uri(@"/Images/smileface.jpg", UriKind.Relative));
            Smile.MouseDown += SmileClick;




            
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new System.EventHandler(timer_Tick);
            

            SetupTimer();



        }
        Random rnd = new Random();
        void timer_Tick(object sender, EventArgs e)
        {
            time++;
            SetupTimer();
        }

        private void SetupTimer()
        {
            var timeinstr = time.ToString();
            if (time < 10)
            {
                FirstTimeCnt.Source = new BitmapImage(new Uri(@"/Images/cislo0.jpg", UriKind.Relative));
                SecondTimeCnt.Source = new BitmapImage(new Uri(@"/Images/cislo0.jpg", UriKind.Relative));
                ThirdTimeCnt.Source = new BitmapImage(new Uri(@"/Images/cislo"+ timeinstr + ".jpg", UriKind.Relative));
            }
            else if (time < 100)
            {
                FirstTimeCnt.Source = new BitmapImage(new Uri(@"/Images/cislo0.jpg", UriKind.Relative));
                SecondTimeCnt.Source = new BitmapImage(new Uri(@"/Images/cislo"+ timeinstr[0] + ".jpg", UriKind.Relative));
                ThirdTimeCnt.Source = new BitmapImage(new Uri(@"/Images/cislo" + timeinstr[1] + ".jpg", UriKind.Relative));
            }
            else if(time < 1000)
            {
                FirstTimeCnt.Source = new BitmapImage(new Uri(@"/Images/cislo"+ timeinstr[0] + ".jpg", UriKind.Relative));
                SecondTimeCnt.Source = new BitmapImage(new Uri(@"/Images/cislo" + timeinstr[1] + ".jpg", UriKind.Relative));
                ThirdTimeCnt.Source = new BitmapImage(new Uri(@"/Images/cislo" + timeinstr[2] + ".jpg", UriKind.Relative));
            }
        }
        private void SmileClick(object sender, MouseEventArgs e)
        {
            timer.Stop();
            Smile.Source = new BitmapImage(new Uri(@"/Images/clickface.jpg", UriKind.Relative));
            reveled = 0;
            Cordinates.Clear();
            FlaggedCordinates.Clear();
            BtnsToCheckList.Clear();
            BtnList.Clear();
            GenerateBombsCordinates();
            GenereateGridButtons();
            RegenerateGrid();
            bombtodiscover = int.Parse(BombNum_.Text);
            EditBmbCnt();
            firstlick = true;
            time = 0;
            Smile.Source = new BitmapImage(new Uri(@"/Images/smileface.jpg", UriKind.Relative));
            SetupTimer();
        }
        private void EditBmbCnt()
        {
            if (bombtodiscover < 10)
            {
                FirstBombCnt.Source = new BitmapImage(new Uri(@"/Images/cislo0.jpg", UriKind.Relative));
                SecondBombCnt.Source = new BitmapImage(new Uri(@"/Images/cislo"+ bombtodiscover +".jpg", UriKind.Relative));
            }
            else
            {
                var test = bombtodiscover.ToString();
                FirstBombCnt.Source = new BitmapImage(new Uri(@"/Images/cislo"+ test[0] + ".jpg", UriKind.Relative));
                SecondBombCnt.Source = new BitmapImage(new Uri(@"/Images/cislo" + test[1] + ".jpg", UriKind.Relative));
            }
        }
        private void RegenerateGrid()
        {
            
            test.ColumnDefinitions.Clear();
            test.RowDefinitions.Clear();
            test.Children.Clear();
            int DH = int.Parse(Height_.Text);
            int DW = int.Parse(Width_.Text);
            test.Width = (DW*30);
            test.Height = (DH * 30);

            while (DH != 0)
            {
                RowDefinition gridRow1 = new RowDefinition();
                gridRow1.Height = new GridLength(1.0, GridUnitType.Star);
                test.RowDefinitions.Add(gridRow1);
                DH--;
            }
            while (DW != 0)
            {
                ColumnDefinition gridRow1 = new ColumnDefinition();
                gridRow1.Width = new GridLength(1.0, GridUnitType.Star);

                test.ColumnDefinitions.Add(gridRow1);
                DW--;
            }

            foreach (PlayGroundButton btn in BtnList)
            {
                PlayGroundButton newbtn = btn;
                Grid.SetColumn(newbtn, btn.Col_);
                Grid.SetRow(newbtn, btn.Row_);
                test.Children.Add(newbtn);
            }
           
           
        }

        private void SomeThingToCheck()
        {
            if (BtnsToCheckList.Count > 0)
            {
                CheckButton(BtnsToCheckList[0]);
            }
        }

        private void CheckFlagedList()
        {
            int Goal = Cordinates.Count;
            int Match = 0;
            foreach (cor flagedcor in FlaggedCordinates)
            {
                foreach (cor bombcor in Cordinates)
                {
                    if (bombcor.col == flagedcor.col && bombcor.row == flagedcor.row)
                    {
                        Match++;
                    }
                }
            }

            if (Match == Goal)
            {
                int btnstoreveled = BtnList.Count - Cordinates.Count;
                if (reveled == btnstoreveled)
                {
                    //MessageBox.Show("Konec hry - výhra", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    Smile.Source = new BitmapImage(new Uri(@"/Images/winface.jpg", UriKind.Relative));
                    timer.Stop();
                    WriteScore();
                }
                
            }
        }
        private void MouseRightClick(object sender, MouseEventArgs e)
        {
            PlayGroundButton button = (PlayGroundButton)sender;
            if (button.HasFalg == false)
            {
                Uri resourceUri = new Uri("Images/flag.jpg", UriKind.Relative);
                StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                var brush = new ImageBrush();
                brush.ImageSource = temp;
                button.Background = brush;
                button.HasFalg = true;
                bombtodiscover--;
                

                cor minecor = new cor();
                minecor.col = button.Col_;
                minecor.row = button.Row_;
                FlaggedCordinates.Add(minecor);
                if (FlaggedCordinates.Count == Cordinates.Count)
                {
                    CheckFlagedList();
                }

            }
            else
            {
                bombtodiscover++;
                Uri resourceUri = new Uri("Images/tile.jpg", UriKind.Relative);
                StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                var brush = new ImageBrush();
                brush.ImageSource = temp;
                button.Background = brush;
                button.HasFalg = false;

                cor loadcor = null;
               
                foreach (cor flagedcor in FlaggedCordinates)
                {
                    if (flagedcor.row == button.Row_ && flagedcor.col == button.Col_)
                    {
                        loadcor = flagedcor;
                    }
                }
                
                FlaggedCordinates.Remove(loadcor);
            }

            EditBmbCnt();


        }

        private void CheckButton(PlayGroundButton btn)
        {
            List<PlayGroundButton> list = new List<PlayGroundButton>();
            if (!btn.Checked)
            {
                reveled++;
            }
            int bombcnt = 0;
            if (!btn.IsBomb)
            {
                
                //btn.IsEnabled = false;
                
                btn.Click -= PlayGroundButtonClick;
                btn.MouseRightButtonDown -= MouseRightClick;
                btn.Checked = true;
                btn.Focusable = false;
                

                int btnstoreveled = BtnList.Count - Cordinates.Count;
                
                if (reveled == btnstoreveled)
                {
                    //MessageBox.Show("Konec hry - výhra", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    Smile.Source = new BitmapImage(new Uri(@"/Images/winface.jpg", UriKind.Relative));
                    timer.Stop();
                    WriteScore();
                }
            }
            

            foreach (PlayGroundButton button in BtnList)
            {
                //Napravo
                if (button.Row_ == btn.Row_ && btn.Col_ == button.Col_ -1)
                {
                    if (button.IsBomb)
                    {
                        bombcnt++;
                    }
                    else
                    {
                        if (!button.Checked)
                        {
                            list.Add(button);
                        }
                    }
                }
                //Nalevo
                if (button.Row_ == btn.Row_ && btn.Col_ == button.Col_ + 1)
                {
                    if (button.IsBomb)
                    {
                        bombcnt++;
                    }
                    else
                    {
                        if (!button.Checked)
                        {
                            list.Add(button);
                        }
                    }
                }
                //Nahoře
                if (button.Row_ + 1 == btn.Row_ && btn.Col_ == button.Col_)
                {
                    if (button.IsBomb)
                    {
                        bombcnt++;
                    }
                    else
                    {
                        if (!button.Checked)
                        {
                            list.Add(button);
                        }
                    }                  
                }
                //Dole
                if (button.Row_ - 1 == btn.Row_ && btn.Col_ == button.Col_)
                {
                    if (button.IsBomb)
                    {
                        bombcnt++;
                    }
                    else
                    {
                        if (!button.Checked)
                        {
                            list.Add(button);
                        }
                    }
                }
                //Dole rohy
                if (button.Row_ - 1 == btn.Row_ && (btn.Col_ == button.Col_ - 1 || btn.Col_ == button.Col_ + 1))
                {
                    if (button.IsBomb)
                    {
                        bombcnt++;
                    }
                }
                //Nahoře rohy
                if (button.Row_ + 1== btn.Row_ && (btn.Col_ == button.Col_ - 1 || btn.Col_ == button.Col_ + 1))
                {
                    if (button.IsBomb)
                    {
                        bombcnt++;
                    }
                }

               
            }
            if(bombcnt == 0)
            {
                foreach(PlayGroundButton btsn in list)
                {
                    BtnsToCheckList.Add(btsn);
                }
            }
            if(bombcnt > 0)
            {
                //btn.Content = bombcnt;
                Uri resourceUri = new Uri("Images/bmbcnt"+ bombcnt +".jpg", UriKind.Relative);
                StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                var brush = new ImageBrush();
                brush.ImageSource = temp;
                btn.Background = brush;


            }
            else
            {
                //btn.Content = bombcnt;
                Uri resourceUri = new Uri("Images/emptytile.jpg", UriKind.Relative);
                StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                var brush = new ImageBrush();
                brush.ImageSource = temp;
                btn.Background = brush;
            }
            
            BtnsToCheckList.Remove(btn);
            SomeThingToCheck();
            Smile.Source = new BitmapImage(new Uri(@"/Images/smileface.jpg", UriKind.Relative));

        }


        private void WriteScore()
        {
            Save save = new Save();
            save.PocetMin = int.Parse(BombNum_.Text);
            save.Objeveno = reveled;
            save.PocetPoli = BtnList.Count;
            //save.Ratio = save.PocetPoli / save.Objeveno;
            ScoreList.Add(save);


            var json = JsonConvert.SerializeObject(ScoreList);

            //string json = JsonConvert.SerializeObject(NewCar);
            string directory = System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString();
            directory = System.IO.Directory.GetParent(directory).ToString();

            System.IO.File.WriteAllText(directory + "/json.txt", json);
        }
        
        private void PlayGroundButtonClick(object sender, EventArgs e)
        {
            Smile.Source = new BitmapImage(new Uri(@"/Images/clickface.jpg", UriKind.Relative));
            var BombCount = 0;
            PlayGroundButton button = sender as PlayGroundButton;
            if (firstlick)
            {

                BlockedCor.row = button.Row_;
                BlockedCor.col = button.Col_;
                reveled = 0;
                Cordinates.Clear();
                FlaggedCordinates.Clear();
                BtnsToCheckList.Clear();
                BtnList.Clear();
                GenerateBombsCordinates();
                GenereateGridButtons();
                RegenerateGrid();
                bombtodiscover = int.Parse(BombNum_.Text);
                EditBmbCnt();
                firstlick = false;
                timer.Start();
                foreach (PlayGroundButton but in BtnList)
                {
                    if (but.Col_ == BlockedCor.col && but.Row_ == BlockedCor.row)
                    {
                        button = but;
                    }
                }
            }

            
            if (button.HasFalg == false)
            {
                int btn_r = button.Row_;
                int btn_c = button.Col_;
                bool RowMatch = false;
                bool ColMatch = false;
                bool IsBomb = false;
                foreach (cor test in Cordinates)
                {
                    RowMatch = false;
                    ColMatch = false;
                    if (btn_r == test.row)
                    {
                        RowMatch = true;
                    }
                    if (btn_c == test.col)
                    {
                        ColMatch = true;
                    }
                    if (RowMatch && ColMatch)
                    {
                        IsBomb = true;
                    }

                }
                if (IsBomb)
                {
                    //MessageBox.Show("Objevil si bombu - konec hry", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    Smile.Source = new BitmapImage(new Uri(@"/Images/lostface.jpg", UriKind.Relative));
                    timer.Stop();
                    Uri resourceUri;
                    StreamResourceInfo streamInfo;
                    BitmapFrame temp;
                    var brush = new ImageBrush();
                    
                   
                    foreach (PlayGroundButton butt in BtnList)
                    {
                        bool goodguess = false;
                        foreach (cor bmbcor in Cordinates)
                        {
                            
                            if (butt.Col_ == bmbcor.col && butt.Row_ == bmbcor.row)
                            {
                                if (butt.HasFalg)
                                {
                                    
                                }
                                else
                                {
                                    resourceUri = new Uri("Images/bmb.jpg", UriKind.Relative);
                                    streamInfo = Application.GetResourceStream(resourceUri);
                                    temp = BitmapFrame.Create(streamInfo.Stream);
                                    brush = new ImageBrush();
                                    brush.ImageSource = temp;
                                    butt.Background = brush;
                                }
                            }

                            if (butt.HasFalg == true && butt.Col_ == bmbcor.col && butt.Row_ == bmbcor.row)
                            {
                                goodguess = true;
                            }
                        }

                        if (butt.HasFalg && !goodguess)
                        {
                            resourceUri = new Uri("Images/badguess.jpg", UriKind.Relative);
                            streamInfo = Application.GetResourceStream(resourceUri);
                            temp = BitmapFrame.Create(streamInfo.Stream);
                            brush = new ImageBrush();
                            brush.ImageSource = temp;
                            butt.Background = brush;
                        }
                        

                    }
                    resourceUri = new Uri("Images/clickedbomb.jpg", UriKind.Relative);
                    streamInfo = Application.GetResourceStream(resourceUri);
                    temp = BitmapFrame.Create(streamInfo.Stream);
                    brush = new ImageBrush();
                    brush.ImageSource = temp;
                    button.Background = brush;
                    WriteScore();
                    /*GenerateBombsCordinates();
                    GenereateGridButtons();
                    RegenerateGrid();*/
                }
                else
                {

                    CheckButton(button);

                }
            }



            //Smile.Source = new BitmapImage(new Uri(@"/Images/smileface.jpg", UriKind.Relative));

        }
        private void GenerateBombsCordinates()
        {
            Cordinates.Clear();
            int DH = int.Parse(Height_.Text);
            int DW = int.Parse(Width_.Text);
            int BombNum = int.Parse(BombNum_.Text);
            bool Fine;


            while (BombNum != 0)
            {
                bool done = false;
                while (done == false)
                {

                    int Row = rnd.Next(0, DH);
                    int Col = rnd.Next(0, DW);
                    cor curcor = new cor();
                    curcor.col = Col;
                    curcor.row = Row;

                    if (Cordinates.Count() == 0)
                    {
                        if (curcor.col != BlockedCor.col && curcor.row != BlockedCor.row)
                        {
                            Cordinates.Add(curcor);
                            BombNum--;
                            done = true;
                        }
                        
                    }
                    else
                    {
                        Fine = true;
                        foreach (cor test in Cordinates)
                        {
                            if (test == curcor)
                            {
                                Fine = false;

                            }else if (curcor.col == BlockedCor.col && curcor.row == BlockedCor.row)
                            {
                                Fine = false;
                            }

                        }
                        if (Fine)
                        {
                            Cordinates.Add(curcor);
                            BombNum--;
                            done = true;
                        }
                    }
                }
            }
        }

        private void GenereateGridButtons()
        {
            BtnList.Clear();
            int DH = int.Parse(Height_.Text);
            int DW = int.Parse(Width_.Text);
            Uri resourceUri = new Uri("Images/tile.jpg", UriKind.Relative);
            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
            var brush = new ImageBrush();
            brush.ImageSource = temp;


            int count = 1;
            for (int i = 0; i < DH; i++)
            {
                for (int j = 0; j < DW; j++)
                {

                    PlayGroundButton MyControl1 = new PlayGroundButton();
                   
                    MyControl1.Name = "Btn" + count.ToString();
                    MyControl1.IsBomb = false;
                    MyControl1.Row_ = i;
                    MyControl1.Col_ = j;
                    MyControl1.Click += new RoutedEventHandler(PlayGroundButtonClick);
                    MyControl1.MouseRightButtonDown += new MouseButtonEventHandler(MouseRightClick);
                    MyControl1.HasFalg = false;
                    MyControl1.Background = brush;
                    MyControl1.Checked = false;


                    foreach (cor bombcor in Cordinates)
                    {
                        if(bombcor.row == i)
                        {
                            if(bombcor.col == j)
                            {
                                MyControl1.IsBomb = true;
                                
                            }
                        }
                    }
                    
                    BtnList.Add(MyControl1);
                    count++;
                }

            }
            
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            reveled = 0;
            Cordinates.Clear();
            FlaggedCordinates.Clear();
            BtnsToCheckList.Clear();
            BtnList.Clear();
            time = 0;

            //GenerateBombsCordinates();
            GenereateGridButtons();
            RegenerateGrid();
            firstlick = true;
            bombtodiscover = int.Parse(BombNum_.Text);
            EditBmbCnt();
        }
    }
}
