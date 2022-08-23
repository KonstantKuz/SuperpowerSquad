namespace Survivors
{
    public class VibrationManager
    {
        public VibrationManager()
        {
            Vibration.Init();
        }

        public void VibrateLow()
        {
            Vibration.Vibrate(5);
        }

        public void VibrateHigh()
        {
            Vibration.Vibrate(50);
        }
    }
}