namespace GameLibrary
{
    public class OnInteractiveElementPressedEvent : IGameEvent
    {
        public ActionType Action { get; set; }

        public OnInteractiveElementPressedEvent(ActionType action)
        {
            Action = action;
        }
    }

    public enum ActionType
    {
        EndDay,
    }
}
