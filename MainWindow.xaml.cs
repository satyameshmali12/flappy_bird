using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections;
using System.Media;
using System.Timers;
using System.Windows.Navigation;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    class Bird
    {
        public bool changemovecon = false;     // whether to change move or not
        private int move = 1;
        private int size = 40;

        private double speedy = 2.5;   // fall speed of the player or bird
        public bool jumping = false;
        private int jumpcount = 0;
        public string url;
        ImageBrush skin = new ImageBrush();
        public Rectangle bird;
        
        public Bird(Canvas mycan)
        {
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

        
        // creating the pipe class
        public class Pipe{
            public Rectangle recpipe;
            public Pipe(Rectangle gpipe,Canvas mycan){
                this.recpipe = gpipe;
                mycan.Children.Add(gpipe);
            }
        }

        
        // declaring all the global variable
        public int score = 0;

        public int noofpipe = 10;
        public double lastxco;

        public int pipewidth = 60;

        public int pipespeed = 3;

        public string audiobaseurl = "E:\\customs\\csharpdev\\Flappy_BIrd\\WpfApp1\\Audio\\";

        MediaPlayer mp = new MediaPlayer();
        
        Random rand  = new Random();

        // creating the player  i.e from the Bird class
        Bird player;


        // bothe up and down pipe rect
        Rectangle downpiperect;
        Rectangle uppiperect;


        bool scoreincremented = false;

        double lastpipex = 0;


        ArrayList pipelist = new ArrayList();

        public MainWindow()
        { 
            

            DispatcherTimer dispatcher = new DispatcherTimer();
            dispatcher.Tick += gameloop;
            dispatcher.Interval = TimeSpan.FromMilliseconds(5);
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

            int h1 = rand.Next(100,350);
            int h2 = 380-h1;

            downpiperect = new Rectangle(){Width=60,Height=h1,Fill=downpipebrush,Tag="pipe"};
            uppiperect = new Rectangle(){Width=60,Height=h2,Fill=uppipebrush,Tag="pipe"};

            // X_co-ordintes of the first set of pipes
            double x = Width+100;

            foreach (var item in mycan.Children.OfType<Rectangle>()){
                if (Convert.ToString(item.Tag)=="pipe"){
                    x = Canvas.GetLeft(item);
                }
            }



            int rdis = rand.Next(180,200);
            Canvas.SetTop(uppiperect,0);
            Canvas.SetLeft(uppiperect,x+pipewidth+rdis);
            pipelist.Add(new Pipe(uppiperect,mycan));

            Canvas.SetBottom(downpiperect,100);
            Canvas.SetLeft(downpiperect,x+pipewidth+rdis);
            pipelist.Add(new Pipe(downpiperect,mycan));


        }

        private void gameloop(object sender,EventArgs e)
        {
            try{
                // moving,jumping and changing the player move
                player.fall();
                player.jump();
                if (player.changemovecon){
                    player.changemove();
                    player.changemovecon=false;
                }
                else{
                    player.changemovecon=true;
                }

                if (Canvas.GetTop(player.bird)<0 || Canvas.GetTop(player.bird)+player.bird.Height>Height-140){
                    deathtune();
                    System.Environment.Exit(0);
                }


                if (noofpipe!=0)
                {   
                    spawnpipe();
                    noofpipe-=1;
                }


                // the player collision collision is checked over here
                // Note this collision can also be checked in the below loop but just for the purpose of readbility and to make the concern the seperate the login is written in an another logic and If you want can write both the logic in one loop
                foreach (Pipe item in pipelist)
                {
                    var pipex = Canvas.GetLeft(item.recpipe);
                    var pipey = Canvas.GetTop(item.recpipe);
                    var pbottom = (Height-(Canvas.GetBottom(item.recpipe)+item.recpipe.Height))-35;

                    var playerx = Canvas.GetLeft(player.bird);
                    var playery = Canvas.GetTop(player.bird);
                    
                    if (playerx+player.bird.Width>pipex && playerx<pipex+item.recpipe.Width){
                        if (playery>pipey && playery<pipey+item.recpipe.Height){
                            deathtune();
                            System.Environment.Exit(0);
                        }
                        else if (playery+player.bird.Height>pbottom){
                            deathtune();
                            System.Environment.Exit(0);
                        }
                        else{
                            if (Canvas.GetLeft(item.recpipe)-Canvas.GetLeft(player.bird)<3 && Canvas.GetLeft(item.recpipe)-Canvas.GetLeft(player.bird)>0){
                                mp.Pause();
                                mp.Open(new Uri(audiobaseurl+"point.wav"));
                                mp.Volume = 1;
                                mp.Play();
                                score+=1;
                                scoretext.Content = score;
                                break;
                            }
                        } 
                    }
                }


                foreach (Pipe item in pipelist)
                {
                    if (Convert.ToString(item.recpipe.Tag)=="pipe"){
                        Canvas.SetLeft(item.recpipe,Canvas.GetLeft(item.recpipe)-pipespeed);
                        lastxco = Canvas.GetLeft(item.recpipe);
                        if (lastxco+pipewidth<0){
                            if (mycan.Children.Contains(item.recpipe)){
                                mycan.Children.Remove(item.recpipe);
                                noofpipe+=1;
                            }
                        }
                    }
                }

            }
            catch (Exception exe){
                System.Console.WriteLine(exe);
            }
            }

        private void keyboard_keydown(object sender,KeyEventArgs e){
            if (e.Key == Key.Space){
                mp.Open(new Uri("E:\\customs\\csharpdev\\Flappy_BIrd\\WpfApp1\\audio\\wing.wav"));
                mp.Volume = 0.3;
                mp.Play();
                player.jumping=true;
            }
        }

        public void deathtune(){
            mp.Open(new Uri(audiobaseurl+"die.wav"));
            mp.Play();
        }
    }
}

// All done enjoy the gam

// Note
// As you lose the window or game is exited
// Have fun🌛

//Copyright©️ by AJTA's