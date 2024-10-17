namespace CustomExceptions.ObjectExceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
        public NotFoundException(string message, System.Exception inner) : base(message, inner)
        {
        }
        public NotFoundException()
        {
        }

    }
}
