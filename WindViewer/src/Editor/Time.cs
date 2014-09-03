namespace WindViewer.Editor
{
    public class Time
    {
        /// <summary> The time it took to render the last frame. </summary>
        public static float DeltaTime { get; private set; }
        /// <summary> The time since the start of the application run (in seconds). </summary>
        public static float TimeSinceStart { get; private set; }


        public static void Internal_UpdateTime(float deltaTime)
        {
            DeltaTime = deltaTime;
            TimeSinceStart += deltaTime;
        }
    }
}