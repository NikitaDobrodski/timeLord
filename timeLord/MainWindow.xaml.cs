using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ClockApp
{
    public partial class MainWindow : Window
    {
        // Таймер для обновления времени каждую секунду
        private DispatcherTimer _timer;

        // Текущее время, которое используется для обновления часов
        private DateTime _currentTime;

        public MainWindow()
        {
            InitializeComponent();  // Инициализация графического интерфейса
            InitializeClock();      // Запуск и настройка часов при запуске приложения
        }

        // Метод инициализации часов и запуска таймера
        private void InitializeClock()
        {
            // Получаем текущее системное время и сохраняем его в переменной _currentTime
            _currentTime = DateTime.Now;

            // Создаем экземпляр таймера и задаем его интервал в 1 секунду
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);  // Вызов события каждую секунду

            // Подписываемся на событие Tick, которое будет вызываться каждый раз, когда пройдет одна секунда
            _timer.Tick += Timer_Tick;

            // Запускаем таймер, чтобы обновление времени началось
            _timer.Start();

            // Обновляем интерфейс, чтобы сразу отобразить текущее время при запуске
            UpdateClock();
        }

        // Метод, который вызывается каждую секунду таймером
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Увеличиваем текущее время на одну секунду
            _currentTime = _currentTime.AddSeconds(1);

            // Обновляем отображение времени на часах (цифровых и аналоговых)
            UpdateClock();
        }

        // Метод обновления отображения как цифровых, так и аналоговых часов
        private void UpdateClock()
        {
            // Обновляем текст цифровых часов, используя формат времени "часы:минуты:секунды"
            DigitalClock.Text = _currentTime.ToString("HH:mm:ss");

            // Рассчитываем угол для часовой стрелки: 
            // Часы * 30 градусов (12 часов = 360 градусов) + половина градуса за каждую минуту
            double hourAngle = (_currentTime.Hour % 12) * 30 + _currentTime.Minute * 0.5;

            // Рассчитываем угол для минутной стрелки: 
            // Минуты * 6 градусов (60 минут = 360 градусов)
            double minuteAngle = _currentTime.Minute * 6;

            // Рассчитываем угол для секундной стрелки: 
            // Секунды * 6 градусов (60 секунд = 360 градусов)
            double secondAngle = _currentTime.Second * 6;

            // Применяем анимацию для плавного движения стрелок на новый угол
            AnimateHand(RotateTransformHourHand, hourAngle);
            AnimateHand(RotateTransformMinuteHand, minuteAngle);
            AnimateHand(RotateTransformSecondHand, secondAngle);
        }

        // Метод для анимации движения стрелок
        // handTransform - трансформация, которая применяется к конкретной стрелке (часовой, минутной или секундной)
        // toValue - новый угол, на который необходимо переместить стрелку
        private void AnimateHand(RotateTransform handTransform, double toValue)
        {
            // Создаем анимацию, которая изменяет угол стрелки
            DoubleAnimation animation = new DoubleAnimation
            {
                // Задаем конечное значение угла
                To = toValue,

                // Устанавливаем продолжительность анимации (0.5 секунды для плавного движения)
                Duration = TimeSpan.FromSeconds(0.5),

                // Добавляем функцию плавного изменения (синусоидальная интерполяция) для более естественного движения
                EasingFunction = new SineEase()
            };

            // Применяем анимацию к свойству угла поворота стрелки
            handTransform.BeginAnimation(RotateTransform.AngleProperty, animation);
        }

        // Метод, который срабатывает при нажатии на кнопку установки времени пользователем
        private void SetTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем значение часов, минут и секунд, введенные пользователем
                int hours = int.Parse(HoursInput.Text);
                int minutes = int.Parse(MinutesInput.Text);
                int seconds = int.Parse(SecondsInput.Text);

                // Обновляем текущее время (_currentTime) на значения, введенные пользователем
                // Год, месяц и день остаются текущими (по системному времени)
                _currentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, seconds);

                // Обновляем отображение часов сразу после изменения времени
                UpdateClock();
            }
            catch (FormatException)
            {
                // Если пользователь ввел некорректные данные, показываем сообщение об ошибке
                MessageBox.Show("Пожалуйста, введите правильное значение времени.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}



