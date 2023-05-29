using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FeetFinder.ViewElements
{
    /// <summary>
    /// Interaction logic for StarRatingFeet.xaml
    /// </summary>
    public partial class StarRatingFeet : UserControl
    {
        private static readonly SolidColorBrush FillColor = new(Color.FromRgb(204, 204, 0));

        public static readonly DependencyProperty RatingProperty = DependencyProperty.Register("Rating", typeof(float), typeof(StarRatingFeet), new PropertyMetadata(0.0f, RatingChanged));

        public float Rating
        {
            get => (float)GetValue(RatingProperty);
            set => SetValue(RatingProperty, value);
        }

        private static void RatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((float)e.NewValue < 1f)
            {
                ((StarRatingFeet)d).RatingString = "ugly";
            }

            if ((float)e.NewValue < 2f)
            {
                ((StarRatingFeet)d).RatingString = "bad";
            }

            if ((float)e.NewValue < 3f)
            {
                ((StarRatingFeet)d).RatingString = "ok";
            }

            if ((float)e.NewValue < 4f)
            {
                ((StarRatingFeet)d).RatingString = "nice";
            }

            if ((float)e.NewValue < 5f)
            {
                ((StarRatingFeet)d).RatingString = "beatiful";
            }

            if ((float)e.NewValue <= 0f)
            {
                ((StarRatingFeet)d).RatingString = "unknown";
            }

            if ((float)e.NewValue > 0f && (float)e.NewValue >= 0.5f)
            {
                ((StarRatingFeet)d).StarOne = FillColor;
            }

            if ((float)e.NewValue > 1f && (float)e.NewValue >= 1.5f)
            {
                ((StarRatingFeet)d).StarTwo = FillColor;
            }

            if ((float)e.NewValue > 2f && (float)e.NewValue >= 2.5f)
            {
                ((StarRatingFeet)d).StarThree = FillColor;
            }

            if ((float)e.NewValue > 3f && (float)e.NewValue >= 3.5f)
            {
                ((StarRatingFeet)d).StarFour = FillColor;
            }

            if ((float)e.NewValue > 4f && (float)e.NewValue >= 4.5f)
            {
                ((StarRatingFeet)d).StarFive = FillColor;
            }
        }

        public static readonly DependencyProperty StarOneProperty = DependencyProperty.Register("StarOne", typeof(Brush), typeof(StarRatingFeet), new PropertyMetadata(Brushes.Transparent));
        public Brush StarOne
        {
            get { return (Brush)base.GetValue(StarOneProperty); }
            set { base.SetValue(StarOneProperty, value); }
        }

        public static readonly DependencyProperty StarTwoProperty = DependencyProperty.Register("StarTwo", typeof(Brush), typeof(StarRatingFeet), new PropertyMetadata(Brushes.Transparent));
        public Brush StarTwo
        {
            get { return (Brush)base.GetValue(StarTwoProperty); }
            set { base.SetValue(StarTwoProperty, value); }
        }

        public static readonly DependencyProperty StarThreeProperty = DependencyProperty.Register("StarThree", typeof(Brush), typeof(StarRatingFeet), new PropertyMetadata(Brushes.Transparent));
        public Brush StarThree
        {
            get { return (Brush)base.GetValue(StarThreeProperty); }
            set { base.SetValue(StarThreeProperty, value); }
        }

        public static readonly DependencyProperty StarFourProperty = DependencyProperty.Register("StarFour", typeof(Brush), typeof(StarRatingFeet), new PropertyMetadata(Brushes.Transparent));
        public Brush StarFour
        {
            get { return (Brush)base.GetValue(StarFourProperty); }
            set { base.SetValue(StarFourProperty, value); }
        }

        public static readonly DependencyProperty StarFiveProperty = DependencyProperty.Register("StarFive", typeof(Brush), typeof(StarRatingFeet), new PropertyMetadata(Brushes.Transparent));
        public Brush StarFive
        {
            get { return (Brush)base.GetValue(StarFiveProperty); }
            set { base.SetValue(StarFiveProperty, value); }
        }

        public static readonly DependencyProperty RatingStringProperty = DependencyProperty.Register("RatingString", typeof(string), typeof(StarRatingFeet), new PropertyMetadata(null));
        public string RatingString
        {
            get { return (string)base.GetValue(RatingStringProperty); }
            set { base.SetValue(RatingStringProperty, value); }
        }

        public StarRatingFeet()
        {
            this.InitializeComponent();
        }
    }
}
