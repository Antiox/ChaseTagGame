namespace GameLibrary
{
    public class OnPointsAddedEvent : IGameEvent
    {
        public float Points { get; set; }


        public OnPointsAddedEvent(float points)
        {
            Points = points;
        }
    }
}
