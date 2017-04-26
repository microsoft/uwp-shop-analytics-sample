using ShopEvents.Models;
using System;
using System.Text;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RidoShop.Sensors
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Web API Base URI
        private readonly string baseuri = "http://ridoshopserver.azurewebsites.net";

        // Pin mappings for 2 LEDs and 2 IR Beam sensors
        private const int SensorEntrancePin = 5;
        private const int SensorExitPin = 17;
        private const int LightEntrancePin = 4;
        private const int LightExitPin = 6;

        // Used to determine how long to wait before refreshing the state of the IoT Device (after one sensor is triggered but not the other)
        // 500 means ~500 ms will pass before the state is refreshed
        private const int RefreshTimer = 500;

        // Pins for 2 LEDs and 2 IR Beam sensors
        private GpioPin entrySensor;
        private GpioPin exitSensor;
        private GpioPin entryLight;
        private GpioPin exitLight;

        // Determines if the event was an entrance or exit event
        private bool enter;
        private bool exit;


        // Used to reset the state of the device with no action
        private readonly DispatcherTimer timer;
        private int timesTicked = 0;



        // Used to submit data to asp.net endpoint 
        

        public MainPage()
        {

            this.InitializeComponent();
            // If 500 seconds passes between one sensor being triggered and the other, the state is reset
            // To prevent noise / false fires from mistakenly determining entry or exit
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(RefreshTimer) };
            timer.Tick += Timer_Tick;

            InitGpio();

        
        }

        /// <summary>
        /// Initialize the GPIO pins based on sensor/LED pin values 
        /// </summary>
        private void InitGpio()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if these is no GPIO controller
            if (gpio == null)
            {
                GpioStatus.Text = "There is no GPIO controller on this device.";

                // Enable buttons to simulate the enter and entrance events on a non-IoT device
                EnterButton.Visibility = ExitButton.Visibility = Visibility.Visible;

                return;
            }

            // Open pins for all of our devices
            entrySensor = gpio.OpenPin(SensorEntrancePin);
            exitSensor = gpio.OpenPin(SensorExitPin);
            entryLight = gpio.OpenPin(LightEntrancePin);
            exitLight = gpio.OpenPin(LightExitPin);

            // Set proper drive mode for input and output devices
            entrySensor.SetDriveMode(GpioPinDriveMode.Input);
            exitSensor.SetDriveMode(GpioPinDriveMode.Input);

            entryLight.SetDriveMode(GpioPinDriveMode.Output);
            exitLight.SetDriveMode(GpioPinDriveMode.Output);

            // Set a debounce timeout of 50 ms to filter out bounce noise from IR beam sensor 
            entrySensor.DebounceTimeout = TimeSpan.FromMilliseconds(50);
            exitSensor.DebounceTimeout = TimeSpan.FromMilliseconds(50);

            // Register for the ValueChanged events so when the IR beam sensor is triggered, we can handle it
            entrySensor.ValueChanged += EntrySensor_ValueChanged;
            exitSensor.ValueChanged += ExitSensor_ValueChanged;

            GpioStatus.Text = "GPIO pins initialized correctly.";
        }

        /// <summary>
        /// Executes when the "entry" sensor is triggered (i.e. the beam is broken)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void EntrySensor_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            // To visualize that IR sensors are working, we will light up the entryLight whenever the beam is broken
            // When the beam is broken, args.Edge is FallingEdge; when it is unbroken it is RisingEdge
            // As soon as the beam breaks, we check to see if an event has happened (i.e. the other beam has already broken).
            if (args.Edge == GpioPinEdge.RisingEdge)
            {
                entryLight.Write(GpioPinValue.High);
                return;
            }

            entryLight.Write(GpioPinValue.Low);

            if (exit)
            {
                CreateEvent(false);
                enter = false;
                exit = false;
                return;
            }

            enter = true;
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => timer.Start());

        }

        /// <summary>
        /// Executes when the "exit" sensor is triggered (i.e. the beam is broken)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ExitSensor_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            // To visualize that IR sensors are working, we will light up the entryLight whenever the beam is broken
            // When the beam is broken, args.Edge is FallingEdge; when it is unbroken it is RisingEdge
            // As soon as the beam breaks, we check to see if an event has happened (i.e. the other beam has already broken).
            if (args.Edge == GpioPinEdge.RisingEdge)
            {
                exitLight.Write(GpioPinValue.High);
                return;
            }

            exitLight.Write(GpioPinValue.Low);

            if (enter)
            {
                CreateEvent(true);
                enter = false;
                exit = false;
                return;
            }

            exit = true;
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => timer.Start());

        }

        /// <summary>
        /// Reset the state of the device if ~500ms passes before an event is triggered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, object e)
        {
            timesTicked++;
            if (timesTicked > 0)
            {
                enter = exit = false;
                timesTicked = 0;
                timer.Stop();
            }
        }

        /// <summary>
        /// Used to update the IoT UI (for debugging) and send a message to the web API
        /// </summary>
        private async void CreateEvent(bool eventType)
        {
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, 
                () => GpioStatus.Text = eventType.ToString());

            var newEvent = new TriggeredEvent
            {
                EventType = eventType,
                EventTime = DateTime.Now
            };

            HttpClient http = new HttpClient();// { BaseAddress = new Uri(baseuri) };
            HttpResponseMessage message = await http
              .PostAsync(new Uri(baseuri + "/api/event"),
                  new HttpStringContent(newEvent.ToJson(),
                          UnicodeEncoding.Utf8, "application/json"));
        }


        /// <summary>
        /// The following two methods are used to simulate enter and exit events if you do not have an IoT device handy. Simply run the app on desktop and you can click the buttons.
        /// </summary>
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            CreateEvent(true);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            CreateEvent(false);
        }
    }
}
