// See https://aka.ms/new-console-template for more information

using System.IO.Ports;


/*
 * TYPES:
 *  ID:
 *      - int 0-15
 *            11
 *         10   12 
 *        9      13
 *         8     14
 *            15
 *           
 *         4 5 6 7
 *         0 1 2 3
 *  EFFECT:
 *      - effect of given LED
 *      - 0: light
 *      - 1: blink
 *      - 4: rainbow
 *      - 128: key press?
 *
 *
 */

namespace COMSender
{
    class ComSender
    {
        static SerialPort port = new SerialPort();
        private static readonly HttpClient client = new HttpClient();
        
        public static void Main(string[] args)
        {
            string command = string.Join(" ", args);

            if (SerialPort.GetPortNames().Length == 0)
            {
                Console.Error.WriteLine("Cannot find any COM port, terminating...");
                Environment.Exit(1);
            }
            
            string portName = SerialPort.GetPortNames()[0];


            port.PortName = portName;
            port.BaudRate = 115200;
            port.Handshake = Handshake.None;

            if (command == "setup")
            {
                Setup();
                Environment.Exit(0);
            }

            if (command == "night")
            {
                NightMode();
                Environment.Exit(0);
            }
            
            Config.Initialize();
            bool meta = Config.IsMetaPressed();
            
            int id = int.Parse(command);

            switch (id)
            {
                case 0:
                    Actions.RunActionKey0(meta);
                    break;
                case 1:
                    Actions.RunActionKey1(meta);
                    break;
                case 2:
                    Actions.RunActionKey2(meta);
                    break;
                case 3:
                    Actions.RunActionKey3(meta);
                    break;
                case 4:
                    Actions.RunActionKey4(meta);
                    break;
                case 5:
                    Actions.RunActionKey5(meta);
                    break;
                case 6:
                    Actions.RunActionKey6(meta);
                    break;
                case 7:
                    Actions.RunActionKey7(meta);
                    break;

            }
        }

        static void NightMode()
        {
            AllOff();
            ChangeBar(7, Color.OFF, Effects.LIGHT);
            client.GetAsync($"http://{Config.lightIp}/night");
        }

        static void Setup()
        {
            AllWhite();
            ChangeLED(3, Color.BLUE, Effects.LIGHT);
            ChangeLED(6, Color.BLUE, Effects.LIGHT);
            ChangeLED(7, Color.RED, Effects.LIGHT);
            ChangeBar(7, Color.OFF, Effects.LIGHT);
            
            /*ChangeLED(2, Color.YELLOW, Effects.KEY_PRESS);
            ChangeLED(3, Color.WHITE, Effects.KEY_PRESS);
            ChangeLED(6, Color.WHITE, Effects.KEY_PRESS);
            ChangeLED(7, Color.WHITE, Effects.KEY_PRESS);*/
        }

        public static void SendCommand(string command)
        {
            Console.WriteLine($"Executing '{command}'");
            port.Open();
            port.WriteLine(command);
            port.Close();
        }

        public static void AllRed()
        {
            SendCommand("red");
        }
        
        public static void AllBlue()
        {
            SendCommand("blue");
        }
        
        public static void AllGreen()
        {
            SendCommand("green");
        }
        
        public static void AllWhite()
        {
            SendCommand("white");
        }
        
        public static void AllOff()
        {
            SendCommand("off");
        }

        public static void ChangeBar(int length, Color color, int effect)
        {
            SendCommand($"bar {length} {color.red} {color.green} {color.blue} {effect}");            
        }
        
        public static void ChangeLED(int ledId, Color color, int effect)
        {
            SendCommand($"LED {ledId} {color.red} {color.green} {color.blue} {effect}");            
        }

        public static void SetBrightness(int brightness)
        {
            SendCommand($"brightness {brightness}");
        }
    }
}