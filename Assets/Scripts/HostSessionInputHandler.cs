namespace Filibusters
{
    public class HostSessionInputHandler : SessionInputHandler
    {
        protected override void OnValidSanitizedInput(string input)
        {
            NetworkManager.CreateAndJoinGameSession(input);
        }
    }
}
