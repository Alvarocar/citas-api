namespace Citas.Domain.Exceptions;

[Serializable]
public class AlreadyExistException : CitasException
{

  public AlreadyExistException()
      : base(Resources.Exceptions.AlreadyExists)
  {
  }

  public AlreadyExistException(string message)
      : base(message) { }

  public AlreadyExistException(string message, Exception innerException)
      : base(message, innerException) { }
}
