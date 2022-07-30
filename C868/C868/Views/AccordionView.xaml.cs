using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C868.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccordionView : ContentView
    {
        public static readonly BindableProperty IndicatorViewProperty = BindableProperty.Create(nameof(IndicatorView), typeof(View), typeof(AccordionView), default(View));
        public View IndicatorView
        {
            get => (View)GetValue(IndicatorViewProperty);
            set => SetValue(IndicatorViewProperty, value);
        }

        public static readonly BindableProperty AccordionContentViewProperty = BindableProperty.Create(nameof(AccordionContentView), typeof(View), typeof(AccordionView), default(View));
        public View AccordionContentView
        {
            get => (View)GetValue(AccordionContentViewProperty);
            set => SetValue(AccordionContentViewProperty, value);
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(AccordionView), default(string));
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty CenterProperty = BindableProperty.Create(nameof(Center), typeof(string), typeof(AccordionView), default(string));
        public string Center
        {
            get => (string)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        public static readonly BindableProperty CenterLeftProperty = BindableProperty.Create(nameof(CenterLeft), typeof(string), typeof(AccordionView), default(string));
        public string CenterLeft
        {
            get => (string)GetValue(CenterLeftProperty);
            set => SetValue(CenterLeftProperty, value);
        }

        public static readonly BindableProperty CenterRightProperty = BindableProperty.Create(nameof(CenterRight), typeof(string), typeof(AccordionView), default(string));
        public string CenterRight
        {
            get => (string)GetValue(CenterRightProperty);
            set => SetValue(CenterRightProperty, value);
        }

        public static readonly BindableProperty CenterTextProperty = BindableProperty.Create(nameof(CenterText), typeof(string), typeof(AccordionView), default(string));
        public string CenterText
        {
            get => (string)GetValue(CenterTextProperty);
            set => SetValue(CenterTextProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(string), typeof(AccordionView), default(string));
        public string FontSize
        {
            get => (string)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(nameof(IsOpen), typeof(bool), typeof(AccordionView), false, propertyChanged: IsOpenChanged);
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly BindableProperty HeaderBackgroundColorProperty = BindableProperty.Create(nameof(HeaderBackgroundColor), typeof(Color), typeof(AccordionView), Color.Transparent);
        public Color HeaderBackgroundColor
        {
            get { return (Color)GetValue(HeaderBackgroundColorProperty); }
            set { SetValue(HeaderBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(AccordionView), Color.Transparent);
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        private static void IsOpenChanged(BindableObject index, object oldVal, object newVal)
        {
            bool isOpen;

            if (index != null && newVal != null)
            {
                var control = (AccordionView)index;
                isOpen = (bool)newVal;

                if (control.IsOpen == false)
                {
                    control.Close();
                }
                else
                {
                    control.Open();
                }
            }
        }

        public uint aDuration { get; set; }

        public AccordionView()
        {
            InitializeComponent();
            Close();
            aDuration = 125;
            IsOpen = false;
        }

        async void Close()
        {
            await Task.WhenAll(
                contentView.TranslateTo(0, -10, aDuration),
                Indicator.RotateTo(-180, aDuration),
                contentView.FadeTo(0, 50)
                ); ; ;
            contentView.IsVisible = false;
        }

        async void Open()
        {
            contentView.IsVisible = true;
            await Task.WhenAll(
                contentView.TranslateTo(0, 10, aDuration),
                Indicator.RotateTo(0, aDuration),
                contentView.FadeTo(30, 50, Easing.SinIn)
            );
        }

        private void TitleTapped(object sender, EventArgs e)
        {
            IsOpen = !IsOpen;
        }
    }
}