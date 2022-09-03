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
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    class Bird
    {
        public bool changemovecon = false;     // whether to change move or not
        public int move = 1;
        private int size = 40;
        private int speedy = 3;   // fall speed of the player or bird
        public bool jumping = false;
        private int jumpcount = 0;
        // public string url = $"E:\\customs\\csharpdev\\Flappy_BIrd\\WpfApp1\\img\\movement\\bird{2}.png";
        public string url;
        ImageBrush skin = new ImageBrush();
        Rectangle bird;
        
        public Bird(Canvas mycan)
        {
            // this.url = $"E:\\customs\\csharpdev\\Flappy_BIrd\\WpfApp1\\img\\movement\\bird{move}.png";
            // this.skin.ImageSource = new BitmapImage(new Uri(this.url));
            changemove();
            this.bird = new Rectangle(){Width=size,Height=size,Fill=this.skin};
            Canvas.SetLeft(this.bird,100);
            Canvas.SetTop(this.bird,200);
            mycan.Children.Add(this.bird);
        }

        public void changemove(){
            this.url = $"E:\\customs\\csharpdev\\Flappy_BIrd\\WpfApp1\\img\\movement\\bird{move}.png";
            BitmapImage img = new BitmapImage(new Uri(this.url));
            // img.EndInit();
            img.Rotation = Rotation.Rotate90;
            this.skin.ImageSource =  img;
            move+=1;
            move = (move>3)?1:move;
        }

        public void fall()
        {
            Canvas.SetTop(bird,Canvas.GetTop(bird)+speedy);
        }
        public void jump()
        {
            if (jumping){
                if (jumpcount<6){
                    Canvas.SetTop(bird,Canvas.GetTop(bird)-10);
                    jumpcount+=1;
                }
                else{
                    jumping=false;
                    jumpcount=0;
                }
            }
        }
        
    }


    public partial class MainWindow : Window
    {

        public int noofpipe = 10;
        public double lastxco;

        public int pipewidth = 60;
        Random rand  = new Random();

        // creating the player
        Bird player;

        Rectangle downpiperect;
        Rectangle uppiperect;


        public MainWindow()
        {
            // xco = Width;
            DispatcherTimer dispatcher = new DispatcherTimer();
            dispatcher.Tick += gameloop;
            dispatcher.Interval = TimeSpan.FromMilliseconds(10);
            dispatcher.Start();
            string baseurl1 = "E:\\customs\\csharpdev\\Flappy_BIrd\\WpfApp1\\img";
            InitializeComponent();

            // creating the background window
            addtoscrren($"{baseurl1}\\bg.png",Width,Height,0,-1,-1,0,"background");
            addtoscrren($"{baseurl1}\\ground.png",Width,100,0,-1,0,-1,"ground");
            player = new Bird(mycan);

            // creating the down and up pipe             :- thereby rotating the pipe
            ImageBrush downpipebrush = new ImageBrush();
            ImageBrush uppipebrush = new ImageBrush();
            BitmapImage downpipe = new BitmapImage(new Uri("E:\\customs\\csharpdev\\Flappy_BIrd\\WpfApp1\\img\\pipe.png"));
            downpipebrush.ImageSource = downpipe;
            downpiperect = new Rectangle(){Width=60,Height=100,Fill=downpipebrush,Tag="pipe"};
            RotateTransform rt = new RotateTransform(180);
            TransformedBitmap uppipe = new TransformedBitmap();
            uppipe.BeginInit();
            uppipe.Source =  downpipe;
            uppipe.Transform = rt;
            uppipebrush.ImageSource = uppipe;
            uppipe.EndInit();
            uppiperect = new Rectangle(){Width=60,Height=100,Fill=uppipebrush,Tag="pipe"};


            // sqawning the initial pipe
            Canvas.SetTop(uppiperect,0);
            Canvas.SetLeft(uppiperect,Width);
            mycan.Children.Add(uppiperect);

            Canvas.SetBottom(downpiperect,100);
            Canvas.SetLeft(downpiperect,Width);
            mycan.Children.Add(downpiperect);

        }

        public void addtoscrren(string url,double width,double height,int left,int right,int bottom,int top,string tag="no tag"){
            ImageBrush custom = new ImageBrush();
            custom.ImageSource = new BitmapImage(new Uri(url));
            Rectangle customrect = new Rectangle(){Width=width,Height=height,Fill=custom,Tag=tag};
            if (left>-1)Canvas.SetLeft(customrect,left);
            if (right>-1)Canvas.SetRight(customrect,right);
            if (top>-1)Canvas.SetTop(customrect,top);
            if (bottom>-1)Canvas.SetBottom(customrect,bottom);

            mycan.Children.Add(customrect);
        }

        public void spawnpipe()
        {
            ImageBrush downpipebrush = new ImageBrush();
            BitmapImage downpipe = new BitmapImage(new Uri("E:\\customs\\csharpdev\\Flappy_BIrd\\WpfApp1\\img\\pipe.png"));
            downpipebrush.ImageSource = downpipe;

            ImageBrush uppipebrush = new ImageBrush();
            RotateTransform rt = new RotateTransform(180);
            TransformedBitmap uppipe = new TransformedBitmap();
            uppipe.BeginInit();
            uppipe.Source =  downpipe;
            uppipe.Transform = rt;
            uppipebrush.ImageSource = uppipe;
            uppipe.EndInit();


            downpiperect = new Rectangle(){Width=60,Height=100,Fill=downpipebrush,Tag="pipe"};
            uppiperect = new Rectangle(){Width=60,Height=100,Fill=uppipebrush,Tag="pipe"};

            double x = 0;

            foreach (var item in mycan.Children.OfType<Rectangle>()){
                x = Canvas.GetLeft(item);
            }

            int rdis = rand.Next(100,200);
            Canvas.SetTop(uppiperect,0);
            Canvas.SetLeft(uppiperect,x+pipewidth+rdis);
            mycan.Children.Add(uppiperect);

            Canvas.SetBottom(downpiperect,100);
            Canvas.SetLeft(downpiperect,x+pipewidth+rdis);
            mycan.Children.Add(downpiperect);


        }

        private void gameloop(object sender,EventArgs e)
        {
            if (noofpipe!=0)
            {   
                spawnpipe();
                noofpipe-=1;
            }
            foreach(Rectangle item in mycan.Children.OfType<Rectangle>()){
                if (Convert.ToString(item.Tag) == "pipe"){
                    Canvas.SetLeft(item,Canvas.GetLeft(item)-3);
                    lastxco = Canvas.GetLeft(item);
                    if (lastxco+pipewidth<0){
                        // mycan.Children.Remove(item);
                        // noofpipe+=1;
                        if (mycan.Children.Contains(item)){
                            mycan.Children.RemoveAt(mycan.Children.IndexOf(item));
                            noofpipe+=1;
                        }
                    }
                }
            }
            player.fall();
            player.jump();
            if (player.changemovecon){
                player.changemove();
                player.changemovecon=false;
            }
            else{
                player.changemovecon=true;
            }
        }

        private void keyboard_keydown(object sender,KeyEventArgs e){
            if (e.Key == Key.Space){
                player.jumping=true;
            }
            if (e.Key==Key.T){
                player.changemove();
            }
        }
    }
}
