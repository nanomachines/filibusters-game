namespace Filibusters
{
    public class JoinSessionInputHandler : SessionInputHandler
    {
        protected override void OnValidSanitizedInput(string input)
        {
            NetworkManager.JoinGameSession(input);
        }
    }
}
