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

        // creating the player
        Bird player;

        BitmapImage downpipe;
        TransformedBitmap uppipe;

        public MainWindow()
        {
            DispatcherTimer dispatcher = new DispatcherTimer();
            dispatcher.Tick += gameloop;
            dispatcher.Interval = TimeSpan.FromMilliseconds(10);
            dispatcher.Start();
            string baseurl1 = "E:\\customs\\csharpdev\\Flappy_BIrd\\WpfApp1\\img";
            InitializeComponent();
            // creating the background of the game window
            ImageBrush ground = new ImageBrush();
            ImageBrush back = new ImageBrush();
            ground.ImageSource = new BitmapImage(new Uri($"{baseurl1}\\ground.png"));
            back.ImageSource = new BitmapImage(new Uri($"{baseurl1}\\bg.png"));
            Rectangle groundrect = new Rectangle(){Width=Width,Height=100,Fill=ground};
            Canvas.SetLeft(groundrect,0);
            Canvas.SetBottom(groundrect,0);
            Rectangle backrect = new Rectangle(){Width=Width,Height=Height,Fill=back};
            mycan.Children.Add(backrect);
            mycan.Children.Add(groundrect);  
            player = new Bird(mycan);


            ImageBrush downpipebrush = new ImageBrush();
            ImageBrush uppipebrush = new ImageBrush();
            downpipe = new BitmapImage(new Uri("E:\\customs\\csharpdev\\Flappy_BIrd\\WpfApp1\\img\\pipe.png"));
            RotateTransform rt = new RotateTransform(180);
            uppipe = new TransformedBitmap();
            uppipe.Source =  downpipe;
            uppipe.Transform = rt;
            

        }

        public void addtoscrren(string url,int width,int height,int left,int right,string tag="no tag"){
            ImageBrush custom = new ImageBrush();
            custom.ImageSource = new BitmapImage(new Uri(url));
            Rectangle customrect = new Rectangle(){Width=width,Height=height,Fill=custom,Tag=tag};
            Canvas.SetLeft(customrect,left);
            Canvas.SetBottom(customrect,right);
            mycan.Children.Add(customrect);
        }

        private void gameloop(object sender,EventArgs e)
        {
            if (noofpipe>10)
            {
                

            }
            foreach(var item in mycan.Children.OfType<Rectangle>()){
                
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
