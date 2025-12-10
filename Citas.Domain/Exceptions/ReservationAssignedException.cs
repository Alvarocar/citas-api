namespace Citas.Domain.Exceptions;

public class ReservationAssignedException : CitasException
{
  public ReservationAssignedException()
      : base(Resources.Exceptions.ReservationAssignedException)
  {
  }

  public ReservationAssignedException(string message)
      : base(message) { }

  public ReservationAssignedException(string message, Exception innerException)
      : base(message, innerException) { }
}
