namespace COMSender;

public class Color
{
    public int red;
    public int blue;
    public int green;

    public Color(int red, int green, int blue)
    {
        this.red = red;
        this.blue = blue;
        this.green = green;
    }

    public static Color RED = new Color(255, 0, 0);
    public static Color WHITE = new Color(255, 255, 255);
    public static Color BLUE = new Color(0, 0, 255);
    public static Color GREEN = new Color(0, 255, 0);
    public static Color YELLOW = new Color(255, 255, 0);
    public static Color OFF = new Color(0, 0, 0);
}

public class Effects
{
    public static int LIGHT = 0;
    public static int BLINK = 1;
    public static int RAINBOW = 4;
    public static int KEY_PRESS = 128;
}