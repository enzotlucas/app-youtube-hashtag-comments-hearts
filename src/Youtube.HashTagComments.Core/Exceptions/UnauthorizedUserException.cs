namespace Youtube.HashTagComments.Core.Exceptions
{
    public class UnauthorizedUserException : BusinessException
    {
        public UnauthorizedUserException(string message = "The user is not authorized.") : base(message)
        {
        }
    }
}
