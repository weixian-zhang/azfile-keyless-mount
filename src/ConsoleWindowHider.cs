

using System.Runtime.InteropServices;

public static class ConsoleWindowHider
{
    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    const int SW_HIDE = 0;
    const int SW_SHOW = 5;

    public static void HideWindow()
    {
         var handle = GetConsoleWindow();
            // Hide
        ShowWindow(handle, SW_HIDE);

    }
}