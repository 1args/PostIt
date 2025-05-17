namespace PostIt.Contracts.Exceptions;

public class BadRequestException(string message) : Exception(message);