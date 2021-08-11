using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace W
{
    public partial class MainPage : ContentPage
    {

        public string myProperty { get; set; }

        public MainPage()
        {
            InitializeComponent();
            myProperty = "Hello!";


        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).Text = "Click me again!";
            myProperty = "Button";
        }

        void OnSwiped(object sender, SwipedEventArgs e)
        {
            myProperty = $"You swiped: {e.Direction.ToString()}";

            /*
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    // Handle the swipe
                    break;
                case SwipeDirection.Right:
                    // Handle the swipe
                    break;
                case SwipeDirection.Up:
                    // Handle the swipe
                    break;
                case SwipeDirection.Down:
                    // Handle the swipe
                    break;
           
            }
             */
        }
    }
}
