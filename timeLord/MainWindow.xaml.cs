using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ClockApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private DateTime _currentTime;

        public MainWindow()
        {
            InitializeComponent();
            InitializeClock();
        }

        // Инициализация часов и запуск таймера
        private void InitializeClock()
        {
            _currentTime = DateTime.Now;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            UpdateClock();
        }

        // Обновление времени каждую секунду
        private void Timer_Tick(object sender, EventArgs e)
        {
            _currentTime = _currentTime.AddSeconds(1);
            UpdateClock();
        }

        // Обновление как цифровых, так и аналоговых часов
        private void UpdateClock()
        {
            DigitalClock.Text = _currentTime.ToString("HH:mm:ss");

            double hourAngle = (_currentTime.Hour % 12) * 30 + _currentTime.Minute * 0.5;
            double minuteAngle = _currentTime.Minute * 6;
            double secondAngle = _currentTime.Second * 6;

            AnimateHand(RotateTransformHourHand, hourAngle);
            AnimateHand(RotateTransformMinuteHand, minuteAngle);
            AnimateHand(RotateTransformSecondHand, secondAngle);
        }

        // Анимация для плавного движения стрелок
        private void AnimateHand(RotateTransform handTransform, double toValue)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                To = toValue,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new SineEase()
            };
            handTransform.BeginAnimation(RotateTransform.AngleProperty, animation);
        }

        // Установка времени пользователем через текстовые поля
        private void SetTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int hours = int.Parse(HoursInput.Text);
                int minutes = int.Parse(MinutesInput.Text);
                int seconds = int.Parse(SecondsInput.Text);

                // Обновляем текущее время на то, которое ввел пользователь
                _currentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, seconds);

                // Сразу обновляем отображение на часах
                UpdateClock();
            }
            catch (FormatException)
            {
                MessageBox.Show("Пожалуйста, введите правильное значение времени.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}


