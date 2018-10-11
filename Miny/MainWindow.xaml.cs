using FileHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Resources;
using System.Windows.Shapes;

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
        private int reveled = 0;
        class cor
        {
            public int row { get; set; }
            public int col { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            GenerateBombsCordinates();
            GenereateGridButtons();
            RegenerateGrid();








        }
        Random rnd = new Random();

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
                    MessageBox.Show("Konec hry - výhra", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    WriteScore();
                }
                
            }
        }
        private void MouseRightClick(object sender, MouseEventArgs e)
        {
            PlayGroundButton button = (PlayGroundButton)sender;
            if (button.HasFalg == false)
            {
                Uri resourceUri = new Uri("Images/flag.png", UriKind.Relative);
                StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
                BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                var brush = new ImageBrush();
                brush.ImageSource = temp;
                button.Background = brush;
                button.HasFalg = true;
                

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
                Uri resourceUri = new Uri("Images/test.png", UriKind.Relative);
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
            
        }

        private void CheckButton(PlayGroundButton btn)
        {
            List<PlayGroundButton> list = new List<PlayGroundButton>();
            if (btn.IsEnabled)
            {
                reveled++;
            }
            int bombcnt = 0;
            if (!btn.IsBomb)
            {
                btn.IsEnabled = false;
                int btnstoreveled = BtnList.Count - Cordinates.Count;
                
                if (reveled == btnstoreveled)
                {
                    MessageBox.Show("Konec hry - výhra", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                        if (button.IsEnabled)
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
                        if (button.IsEnabled)
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
                        if (button.IsEnabled)
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
                        if (button.IsEnabled)
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
                btn.Content = bombcnt;
            }
            
            BtnsToCheckList.Remove(btn);
            SomeThingToCheck();

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
            
            var BombCount = 0;
            PlayGroundButton button = sender as PlayGroundButton;
            if(button.HasFalg == false)
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
                    MessageBox.Show("Objevil si bombu - konec hry", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                        Cordinates.Add(curcor);
                        BombNum--;
                        done = true;
                    }
                    else
                    {
                        Fine = true;
                        foreach (cor test in Cordinates)
                        {
                            if (test == curcor)
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
            Uri resourceUri = new Uri("Images/test.png", UriKind.Relative);
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
            GenerateBombsCordinates();
            GenereateGridButtons();
            RegenerateGrid();
        }
    }
}
